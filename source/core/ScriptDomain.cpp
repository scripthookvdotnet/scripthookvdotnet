/**
 * Copyright (C) 2015 crosire
 *
 * This software is  provided 'as-is', without any express  or implied  warranty. In no event will the
 * authors be held liable for any damages arising from the use of this software.
 * Permission  is granted  to anyone  to use  this software  for  any  purpose,  including  commercial
 * applications, and to alter it and redistribute it freely, subject to the following restrictions:
 *
 *   1. The origin of this software must not be misrepresented; you must not claim that you  wrote the
 *      original  software. If you use this  software  in a product, an  acknowledgment in the product
 *      documentation would be appreciated but is not required.
 *   2. Altered source versions must  be plainly  marked as such, and  must not be  misrepresented  as
 *      being the original software.
 *   3. This notice may not be removed or altered from any source distribution.
 */

#include "ScriptDomain.hpp"

using namespace System;
using namespace System::IO;
using namespace System::Threading;
using namespace System::Reflection;
using namespace System::Windows::Forms;
using namespace System::Collections::Generic;

namespace
{
	inline void SignalAndWait(AutoResetEvent ^toSignal, AutoResetEvent ^toWaitOn)
	{
		toSignal->Set();
		toWaitOn->WaitOne();
	}
	inline bool SignalAndWait(AutoResetEvent ^toSignal, AutoResetEvent ^toWaitOn, int timeout)
	{
		toSignal->Set();
		return toWaitOn->WaitOne(timeout);
	}
}

namespace GTA
{
	void Log(String ^logLevel, ... array<String ^> ^message)
	{
		auto now = DateTime::Now;
		auto logPath = String::Format("{0}-{1}.log", Assembly::GetExecutingAssembly()->Location, now.ToString("yyyy-MM-dd"));

		try
		{
			auto fs = gcnew FileStream(logPath, FileMode::Append, FileAccess::Write, FileShare::Read);
			auto sw = gcnew StreamWriter(fs);

			try
			{
				sw->Write(String::Concat("[", now.ToString("HH:mm:ss"), "] ", logLevel, " "));

				for each (String ^string in message)
				{
					sw->Write(string);
				}

				sw->WriteLine();
			}
			finally
			{
				sw->Close();
				fs->Close();
			}
		}
		catch (...)
		{
			return;
		}
	}
	void Logf(String ^logLevel, String ^format, ... array<Object ^> ^args)
	{
		Log(logLevel, String::Format(format, args));
	}
	Assembly ^HandleResolve(Object ^sender, ResolveEventArgs ^args)
	{
		auto assembly = Script::typeid->Assembly;
		auto assemblyName = gcnew AssemblyName(args->Name);

		if (assemblyName->Name->StartsWith("ScriptHookVDotNet", StringComparison::CurrentCultureIgnoreCase) &&
			assemblyName->Version->Major == assembly->GetName()->Version->Major)
		{
			return assembly;
		}

		return nullptr;
	}
	void HandleUnhandledException(Object ^sender, UnhandledExceptionEventArgs ^args)
	{
		if (!args->IsTerminating)
		{
			Log("[ERROR]", "Caught unhandled exception:", Environment::NewLine, args->ExceptionObject->ToString());
		}
		else
		{
			Log("[ERROR]", "Caught fatal unhandled exception:", Environment::NewLine, args->ExceptionObject->ToString());
		}
	}
	ScriptDomain::ScriptDomain() :
		_appDomain(System::AppDomain::CurrentDomain),
		_executingThreadId(Thread::CurrentThread->ManagedThreadId)
	{
		sCurrentDomain = this;

		_appDomain->AssemblyResolve += gcnew ResolveEventHandler(&HandleResolve);
		_appDomain->UnhandledException += gcnew UnhandledExceptionEventHandler(&HandleUnhandledException);

		Logf("[DEBUG]", "Created script domain '{0}' with v{1}.", _appDomain->FriendlyName,
			ScriptDomain::typeid->Assembly->GetName()->Version->ToString(3));
	}
	ScriptDomain::~ScriptDomain()
	{
		CleanupStrings();

		Logf("[DEBUG]", "Deleted script domain '{0}'.", _appDomain->FriendlyName);
	}

	ScriptDomain ^ScriptDomain::Load(String ^path)
	{
		path = Path::GetFullPath(path);

		auto setup = gcnew AppDomainSetup();
		setup->ApplicationBase = path;
		setup->ShadowCopyFiles = "true";
		setup->ShadowCopyDirectories = path;

		auto permissions = gcnew Security::PermissionSet(Security::Permissions::PermissionState::Unrestricted);

		auto appDomainName = String::Format("ScriptDomain_{0:X}", (path->GetHashCode() * Environment::TickCount));
		auto appDomain = System::AppDomain::CreateDomain(appDomainName, nullptr, setup, permissions);

		appDomain->InitializeLifetimeService();

		ScriptDomain ^scriptdomain = nullptr;

		try
		{
			scriptdomain = static_cast<ScriptDomain ^>(appDomain->CreateInstanceFromAndUnwrap(ScriptDomain::typeid->Assembly->Location, ScriptDomain::typeid->FullName));
		}
		catch (Exception ^ex)
		{
			Logf("[ERROR]", "Failed to create script domain '{0}':{1}{2}", appDomainName, Environment::NewLine, ex);

			System::AppDomain::Unload(appDomain);

			return nullptr;
		}

		Logf("[DEBUG]", "Loading scripts from '{0}' into script domain '{1}' ...", path, appDomainName);

		if (Directory::Exists(path))
		{
			auto filenameScripts = gcnew List<String ^>();
			auto filenameAssemblies = gcnew List<String ^>();

			try
			{
				filenameScripts->AddRange(Directory::GetFiles(path, "*.vb", SearchOption::AllDirectories));
				filenameScripts->AddRange(Directory::GetFiles(path, "*.cs", SearchOption::AllDirectories));
				filenameAssemblies->AddRange(Directory::GetFiles(path, "*.dll", SearchOption::AllDirectories));
			}
			catch (Exception ^ex)
			{
				Log("[ERROR]", "Failed to reload scripts:", Environment::NewLine, ex->ToString());

				System::AppDomain::Unload(appDomain);

				return nullptr;
			}

			for each (String ^filename in filenameScripts)
			{
				scriptdomain->LoadScript(filename);
			}
			for each (String ^filename in filenameAssemblies)
			{
				scriptdomain->LoadAssembly(filename);
			}
		}
		else
		{
			Log("[ERROR]", "Failed to reload scripts because the directory is missing.");
		}

		return scriptdomain;
	}
	bool ScriptDomain::LoadScript(String ^filename)
	{
		auto compilerOptions = gcnew CodeDom::Compiler::CompilerParameters();
		compilerOptions->CompilerOptions = "/optimize";
		compilerOptions->GenerateInMemory = true;
		compilerOptions->IncludeDebugInformation = true;

		for each (auto assembly in _referenceAssemblies)
		{
			compilerOptions->ReferencedAssemblies->Add(assembly);
		}

		compilerOptions->ReferencedAssemblies->Add(Script::typeid->Assembly->Location);

		CodeDom::Compiler::CodeDomProvider ^compiler = nullptr;

		auto extension = Path::GetExtension(filename);

		if (extension->Equals(".cs", StringComparison::InvariantCultureIgnoreCase))
		{
			compiler = gcnew Microsoft::CSharp::CSharpCodeProvider();
			compilerOptions->CompilerOptions += " /unsafe";
		}
		else if (extension->Equals(".vb", StringComparison::InvariantCultureIgnoreCase))
		{
			compiler = gcnew Microsoft::VisualBasic::VBCodeProvider();
		}
		else
		{
			Logf("[DEBUG]", "Could not load script file '{0}': Unknown file extension '{1}'.", filename, extension);

			return false;
		}

		auto compilerResult = compiler->CompileAssemblyFromFile(compilerOptions, filename);

		if (!compilerResult->Errors->HasErrors)
		{
			Logf("[DEBUG]", "Successfully compiled '{0}'.", Path::GetFileName(filename));

			return LoadAssembly(filename, compilerResult->CompiledAssembly);
		}
		else
		{
			auto errors = gcnew Text::StringBuilder();

			errors->AppendFormat("Failed to compile '{0}' with {1} error(s):", Path::GetFileName(filename), compilerResult->Errors->Count);
			errors->AppendLine();

			for each (CodeDom::Compiler::CompilerError ^err in compilerResult->Errors)
			{
				errors->AppendFormat("   at line {0}: {1}", err->Line, err->ErrorText);
				errors->AppendLine();
			}

			Log("[ERROR]", errors->ToString());

			return false;
		}
	}
    bool ScriptDomain::LoadAssembly(String ^filename)
    {
        return LoadAssembly(filename, nullptr);
    }
	bool ScriptDomain::LoadAssembly(String ^filename, Assembly ^assembly)
	{
		if (Path::GetFileNameWithoutExtension(filename)->StartsWith("ScriptHookVDotNet", StringComparison::CurrentCultureIgnoreCase))
		{
			Logf("[ERROR]", "Skipped assembly '{0}'. Please remove it from the 'scripts' directory.", Path::GetFileName(filename));

			return false;
		}

		auto count = 0u;

		try
		{
			if (Object::ReferenceEquals(assembly, nullptr))
			{
				assembly = Assembly::LoadFrom(filename);
			}

			for each (auto type in assembly->GetTypes())
			{
				if (!type->IsSubclassOf(Script::typeid))
				{
					continue;
				}

				count++;
				_scriptTypes->Add(gcnew Tuple<String ^, Type ^>(filename, type));
			}
		}
		catch (Exception ^ex)
		{
			Logf("[ERROR]", "Failed to load assembly '{0}':{1}{2}", Path::GetFileName(filename), Environment::NewLine, ex);

			return false;
		}

		Logf("[DEBUG]", "Found {0} script(s) in '{1}'.", count.ToString(), Path::GetFileName(filename));

		return count != 0;
	}
	void ScriptDomain::Unload(ScriptDomain ^%domain)
	{
		Logf("[DEBUG]", "Unloading script domain '{0}' ...", domain->Name);

		domain->Abort();

		System::AppDomain ^appdomain = domain->AppDomain;

		delete domain;

		try
		{
			System::AppDomain::Unload(appdomain);
		}
		catch (Exception ^ex)
		{
			Log("[ERROR]", "Failed to unload deleted script domain:", Environment::NewLine, ex->ToString());
		}

		domain = nullptr;

		GC::Collect();
	}
	Script ^ScriptDomain::InstantiateScript(Type ^scripttype)
	{
		if (!scripttype->IsSubclassOf(Script::typeid))
		{
			return nullptr;
		}

		Logf("[DEBUG]", "Instantiating script '{0}' in script domain '{1}' ....", scripttype->FullName, Name);

		try
		{
			return static_cast<Script ^>(Activator::CreateInstance(scripttype));
		}
		catch (MissingMethodException ^)
		{
			Logf("[ERROR]", "Failed to instantiate script '{0}' because no public default constructor was found.", scripttype->FullName);
		}
		catch (TargetInvocationException ^ex)
		{
			Logf("[ERROR]", "Failed to instantiate script '{0}' because constructor threw an exception:{1}{2}", scripttype->FullName, Environment::NewLine, ex->InnerException);
		}
		catch (Exception ^ex)
		{
			Logf("[ERROR]", "Failed to instantiate script '{0}':{1}{2}", scripttype->FullName, Environment::NewLine, ex);
		}

		return nullptr;
	}

	bool SortScripts(List<Tuple<String ^, Type ^> ^> ^%scripttypes)
	{
		auto graph = gcnew Dictionary<Tuple<String ^, Type ^> ^, List<Type ^> ^>();

		for each (auto scripttype in scripttypes)
		{
			auto dependencies = gcnew List<Type ^>();

			for each (RequireScript ^attribute in static_cast<MemberInfo ^>(scripttype->Item2)->GetCustomAttributes(RequireScript::typeid, true))
			{
				dependencies->Add(attribute->_dependency);
			}

			graph->Add(scripttype, dependencies);
		}

		auto result = gcnew List<Tuple<String ^, Type ^> ^>(graph->Count);

		while (graph->Count > 0)
		{
			Tuple<String ^, Type ^> ^scriptype = nullptr;

			for each (auto item in graph)
			{
				if (item.Value->Count == 0)
				{
					scriptype = item.Key;
					break;
				}
			}

			if (scriptype == nullptr)
			{
				Log("[ERROR]", "Detected a circular script dependency. Aborting ...");
				return false;
			}

			result->Add(scriptype);
			graph->Remove(scriptype);

			for each (auto item in graph)
			{
				item.Value->Remove(scriptype->Item2);
			}
		}

		scripttypes = result;

		return true;
	}
	void ScriptDomain::Start()
	{
		if (_runningScripts->Count != 0 || _scriptTypes->Count == 0)
		{
			return;
		}

		auto assemblyPath = Assembly::GetExecutingAssembly()->Location;
		auto assemblyFilename = Path::GetFileNameWithoutExtension(assemblyPath);

		for each (auto path in Directory::GetFiles(Path::GetDirectoryName(assemblyPath), "*.log"))
		{
			if (!path->StartsWith(assemblyFilename))
			{
				continue;
			}

			try
			{
				auto logAge = DateTime::Now - DateTime::Parse(Path::GetFileNameWithoutExtension(path)->Substring(path->IndexOf('-') + 1));

				// Delete logs older than 5 days
				if (logAge.Days >= 5)
				{
					File::Delete(path);
				}
			}
			catch (...)
			{
				continue;
			}
		}

		Logf("[DEBUG]", "Starting {0} script(s) ...", _scriptTypes->Count);

		if (!SortScripts(_scriptTypes))
		{
			return;
		}

		for each (auto scripttype in _scriptTypes)
		{
			auto script = InstantiateScript(scripttype->Item2);

			if (Object::ReferenceEquals(script, nullptr))
			{
				continue;
			}

			script->_running = true;
			script->_filename = scripttype->Item1;
			script->_scriptdomain = this;
			script->_thread = gcnew Thread(gcnew ThreadStart(script, &Script::MainLoop));

			script->_thread->Start();

			Logf("[DEBUG]", "Started script '{0}'.", script->Name);

			_runningScripts->Add(script);
		}
	}
	void ScriptDomain::Abort()
	{
		Logf("[DEBUG]", "Stopping {0} script(s) ...", _runningScripts->Count);

		for each (Script ^script in _runningScripts)
		{
			script->Abort();

			delete script;
		}

		_scriptTypes->Clear();
		_runningScripts->Clear();

		GC::Collect();
	}
	void ScriptDomain::AbortScript(Script ^script)
	{
		if (Object::ReferenceEquals(script->_thread, nullptr))
		{
			return;
		}

		script->_running = false;

		script->_thread->Abort();
		script->_thread = nullptr;

		Logf("[DEBUG]", "Aborted script '{0}'.", script->Name);
	}
	void ScriptDomain::DoTick()
	{
		// Execute scripts
		for each (Script ^script in _runningScripts)
		{
			if (!script->_running)
			{
				continue;
			}

			_executingScript = script;

			while ((script->_running = SignalAndWait(script->_continueEvent, script->_waitEvent, 5000)) && _taskQueue->Count > 0)
			{
				_taskQueue->Dequeue()->Run();
			}

			_executingScript = nullptr;

			if (!script->_running)
			{
				Logf("[ERROR]", "Script '{0}' is not responding! Aborting ...", script->Name);

				AbortScript(script);
				continue;
			}
		}

		// Clean up pinned strings
		CleanupStrings();
	}
	void ScriptDomain::DoKeyboardMessage(Keys key, bool status, bool statusCtrl, bool statusShift, bool statusAlt)
	{
		const int keycode = static_cast<int>(key);

		if (keycode < 0 || keycode >= 256)
		{
            return;
		}

		_keyboardState[keycode] = status;

		if (_recordKeyboardEvents)
		{
			auto keyData = key;
			
			if (statusCtrl)
				keyData = (keyData |Keys::Control);
			if (statusShift)
				keyData = (keyData | Keys::Shift);
			if (statusAlt)
				keyData = (keyData | Keys::Alt);

			auto args = gcnew KeyEventArgs(key);
			auto eventinfo = gcnew Tuple<bool, KeyEventArgs ^>(status, args);

			for each (Script ^script in _runningScripts)
			{
				script->_keyboardEvents->Enqueue(eventinfo);
			}
		}
	}

	void ScriptDomain::PauseKeyboardEvents(bool pause)
	{
		_recordKeyboardEvents = !pause;
	}
	void ScriptDomain::ExecuteTask(IScriptTask ^task)
	{
		if (Thread::CurrentThread->ManagedThreadId == _executingThreadId)
		{
			task->Run();
		}
		else
		{
			_taskQueue->Enqueue(task);

			SignalAndWait(ExecutingScript->_waitEvent, ExecutingScript->_continueEvent);
		}
	}
	IntPtr ScriptDomain::PinString(String ^string)
	{
		auto bytes = Text::Encoding::UTF8->GetBytes(string);
		auto size = bytes->Length;

		IntPtr handle(new unsigned char[size + 1]());

		Runtime::InteropServices::Marshal::Copy(bytes, 0, handle, size);

		_pinnedStrings->Add(handle);

		return handle;
	}
	void ScriptDomain::CleanupStrings()
	{
		for each (IntPtr handle in _pinnedStrings)
		{
			delete[] handle.ToPointer();
		}

		_pinnedStrings->Clear();
	}
	String ^ScriptDomain::LookupScriptFilename(Type ^type)
	{
		for each (auto scripttype in _scriptTypes)
		{
			if (scripttype->Item2 == type)
			{
				return scripttype->Item1;
			}
		}

		return String::Empty;
	}
	Object ^ScriptDomain::InitializeLifetimeService()
	{
		return nullptr;
	}
}
