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

namespace GTA
{
	using namespace System;
	using namespace System::Threading;
	using namespace System::Reflection;
	using namespace System::Windows::Forms;
	using namespace System::Collections::Generic;

	void Log(String ^logLevel, ... array<String ^> ^message)
	{
		DateTime now = DateTime::Now;
		String ^logpath = IO::Path::ChangeExtension(Reflection::Assembly::GetExecutingAssembly()->Location, ".log");

		logpath = logpath->Insert(logpath->IndexOf(".log"), "-" + now.ToString("yyyy-MM-dd"));

		try
		{
			IO::FileStream ^fs = gcnew IO::FileStream(logpath, IO::FileMode::Append, IO::FileAccess::Write, IO::FileShare::Read);
			IO::StreamWriter ^sw = gcnew IO::StreamWriter(fs);

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
	Reflection::Assembly ^HandleResolve(Object ^sender, ResolveEventArgs ^args)
	{
		if (args->Name->ToLower()->Contains("scripthookvdotnet"))
		{
			return Reflection::Assembly::GetAssembly(GTA::Script::typeid);
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

	ScriptDomain::ScriptDomain() : _appdomain(System::AppDomain::CurrentDomain), _executingThreadId(Thread::CurrentThread->ManagedThreadId), _runningScripts(gcnew List<Script ^>()), _taskQueue(gcnew Queue<IScriptTask ^>()), _pinnedStrings(gcnew List<IntPtr>()), _scriptTypes(gcnew List<Tuple<String ^, Type ^> ^>()), _recordKeyboardEvents(true), _keyboardState(gcnew array<bool>(256))
	{
		sCurrentDomain = this;

		_appdomain->AssemblyResolve += gcnew ResolveEventHandler(&HandleResolve);
		_appdomain->UnhandledException += gcnew UnhandledExceptionEventHandler(&HandleUnhandledException);

		Log("[DEBUG]", "Created script domain '", _appdomain->FriendlyName, "'.");
	}
	ScriptDomain::~ScriptDomain()
	{
		CleanupStrings();

		Log("[DEBUG]", "Deleted script domain '", _appdomain->FriendlyName, "'.");
	}

	ScriptDomain ^ScriptDomain::Load(String ^path)
	{
		path = IO::Path::GetFullPath(path);

		AppDomainSetup ^setup = gcnew AppDomainSetup();
		setup->ApplicationBase = path;
		setup->ShadowCopyFiles = "true";
		setup->ShadowCopyDirectories = path;
		Security::PermissionSet ^permissions = gcnew Security::PermissionSet(Security::Permissions::PermissionState::Unrestricted);

		System::AppDomain ^appdomain = System::AppDomain::CreateDomain("ScriptDomain_" + (path->GetHashCode() * Environment::TickCount).ToString("X"), nullptr, setup, permissions);
		appdomain->InitializeLifetimeService();

		ScriptDomain ^scriptdomain = nullptr;

		try
		{
			scriptdomain = static_cast<ScriptDomain ^>(appdomain->CreateInstanceFromAndUnwrap(ScriptDomain::typeid->Assembly->Location, ScriptDomain::typeid->FullName));
		}
		catch (Exception ^ex)
		{
			Log("[ERROR]", "Failed to create script domain '", appdomain->FriendlyName, "':", Environment::NewLine, ex->ToString());

			System::AppDomain::Unload(appdomain);

			return nullptr;
		}

		Log("[DEBUG]", "Loading scripts from '", path, "' into script domain '", appdomain->FriendlyName, "' ...");

		if (IO::Directory::Exists(path))
		{
			List<String ^> ^filenameScripts = gcnew List<String ^>();
			List<String ^> ^filenameAssemblies = gcnew List<String ^>();

			try
			{
				filenameScripts->AddRange(IO::Directory::GetFiles(path, "*.vb", IO::SearchOption::AllDirectories));
				filenameScripts->AddRange(IO::Directory::GetFiles(path, "*.cs", IO::SearchOption::AllDirectories));
				filenameAssemblies->AddRange(IO::Directory::GetFiles(path, "*.dll", IO::SearchOption::AllDirectories));
			}
			catch (Exception ^ex)
			{
				Log("[ERROR]", "Failed to reload scripts:", Environment::NewLine, ex->ToString());

				System::AppDomain::Unload(appdomain);

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
			Log("[ERROR]", "Failed to reload scripts because directory is missing.");
		}

		return scriptdomain;
	}
	bool ScriptDomain::LoadScript(String ^filename)
	{
		CodeDom::Compiler::CodeDomProvider ^compiler = nullptr;
		CodeDom::Compiler::CompilerParameters ^compilerOptions = gcnew CodeDom::Compiler::CompilerParameters();
		compilerOptions->CompilerOptions = "/optimize";
		compilerOptions->GenerateInMemory = true;
		compilerOptions->IncludeDebugInformation = true;
		compilerOptions->ReferencedAssemblies->Add("System.dll");
		compilerOptions->ReferencedAssemblies->Add("System.Core.dll");
		compilerOptions->ReferencedAssemblies->Add("System.Drawing.dll");
		compilerOptions->ReferencedAssemblies->Add("System.Windows.Forms.dll");
		compilerOptions->ReferencedAssemblies->Add("System.XML.dll");
		compilerOptions->ReferencedAssemblies->Add("System.XML.Linq.dll");
		compilerOptions->ReferencedAssemblies->Add(GTA::Script::typeid->Assembly->Location);

		String ^extension = IO::Path::GetExtension(filename);

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
			return false;
		}

		CodeDom::Compiler::CompilerResults ^compilerResult = compiler->CompileAssemblyFromFile(compilerOptions, filename);

		if (!compilerResult->Errors->HasErrors)
		{
			Log("[DEBUG]", "Successfully compiled '", IO::Path::GetFileName(filename), "'.");

			return LoadAssembly(filename, compilerResult->CompiledAssembly);
		}
		else
		{
			Text::StringBuilder ^errors = gcnew Text::StringBuilder();

			for (int i = 0; i < compilerResult->Errors->Count; ++i)
			{
				errors->Append("   at line ");
				errors->Append(compilerResult->Errors->default[i]->Line);
				errors->Append(": ");
				errors->Append(compilerResult->Errors->default[i]->ErrorText);

				if (i < compilerResult->Errors->Count - 1)
				{
					errors->AppendLine();
				}
			}

			Log("[ERROR]", "Failed to compile '", IO::Path::GetFileName(filename), "' with ", compilerResult->Errors->Count.ToString(), " error(s):", Environment::NewLine, errors->ToString());

			return false;
		}
	}
	bool ScriptDomain::LoadAssembly(String ^filename)
	{
		Reflection::Assembly ^assembly = nullptr;

		try
		{
			assembly = Reflection::Assembly::LoadFrom(filename);
		}
		catch (Exception ^ex)
		{
			Log("[ERROR]", "Failed to load assembly '", IO::Path::GetFileName(filename), "':", Environment::NewLine, ex->ToString());

			return false;
		}

		return LoadAssembly(filename, assembly);
	}
	bool ScriptDomain::LoadAssembly(String ^filename, Reflection::Assembly ^assembly)
	{
		unsigned int count = 0;

		try
		{
			for each (Type ^type in assembly->GetTypes())
			{
				if (!type->IsSubclassOf(Script::typeid))
				{
					continue;
				}

				count++;
				_scriptTypes->Add(gcnew Tuple<String ^, Type ^>(filename, type));
			}
		}
		catch (Reflection::ReflectionTypeLoadException ^ex)
		{
			Log("[ERROR]", "Failed to list assembly types:", Environment::NewLine, ex->ToString());

			return false;
		}

		Log("[DEBUG]", "Found ", count.ToString(), " script(s) in '", IO::Path::GetFileName(filename), "'.");

		return count != 0;
	}
	void ScriptDomain::Unload(ScriptDomain ^%domain)
	{
		Log("[DEBUG]", "Unloading script domain '", domain->Name, "' ...");

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

		Log("[DEBUG]", "Instantiating script '", scripttype->FullName, "' in script domain '", Name, "' ...");

		try
		{
			return static_cast<Script ^>(Activator::CreateInstance(scripttype));
		}
		catch (MissingMethodException ^)
		{
			Log("[ERROR]", "Failed to instantiate script '", scripttype->FullName, "' because no public default constructor was found.");
		}
		catch (Reflection::TargetInvocationException ^ex)
		{
			Log("[ERROR]", "Failed to instantiate script '", scripttype->FullName, "' because constructor threw an exception:", Environment::NewLine, ex->InnerException->ToString());
		}
		catch (Exception ^ex)
		{
			Log("[ERROR]", "Failed to instantiate script '", scripttype->FullName, "':", Environment::NewLine, ex->ToString());
		}

		return nullptr;
	}

	bool SortScripts(List<Tuple<String ^, Type ^> ^> ^%scripttypes)
	{
		Dictionary<Tuple<String ^, Type ^> ^, List<Type ^> ^> ^graph = gcnew Dictionary<Tuple<String ^, Type ^> ^, List<Type ^> ^>();

		for each (auto scripttype in scripttypes)
		{
			List<Type ^> ^dependencies = gcnew List<Type ^>();

			for each (RequireScript ^attribute in static_cast<MemberInfo ^>(scripttype->Item2)->GetCustomAttributes(RequireScript::typeid, true))
			{
				dependencies->Add(attribute->_dependency);
			}

			graph->Add(scripttype, dependencies);
		}

		List<Tuple<String ^, Type ^> ^> ^result = gcnew List<Tuple<String ^, Type ^> ^>(graph->Count);

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

		String ^assemblyPath = Reflection::Assembly::GetExecutingAssembly()->Location;
		String ^assemblyFilename = IO::Path::GetFileNameWithoutExtension(assemblyPath);

		for each (System::String ^path in IO::Directory::GetFiles(IO::Path::GetDirectoryName(assemblyPath), "*.log"))
		{
			if (!path->StartsWith(assemblyFilename))
			{
				continue;
			}

			try
			{
				TimeSpan logAge = DateTime::Now - DateTime::Parse(IO::Path::GetFileNameWithoutExtension(path)->Substring(path->IndexOf('-') + 1));

				// Delete logs older than 5 days
				if (logAge.Days >= 5)
				{
					IO::File::Delete(path);
				}
			}
			catch (...)
			{
				continue;
			}
		}

		Log("[DEBUG]", "Starting ", _scriptTypes->Count.ToString(), " script(s) ...");

		if (!SortScripts(_scriptTypes))
		{
			return;
		}

		for each (Tuple<String ^, Type ^> ^scripttype in _scriptTypes)
		{
			Script ^script = InstantiateScript(scripttype->Item2);

			if (Object::ReferenceEquals(script, nullptr))
			{
				continue;
			}

			script->_running = true;
			script->_filename = scripttype->Item1;
			script->_scriptdomain = this;
			script->_thread = gcnew Thread(gcnew ThreadStart(script, &Script::MainLoop));

			script->_thread->Start();

			Log("[DEBUG]", "Started script '", script->Name, "'.");

			_runningScripts->Add(script);
		}
	}
	void ScriptDomain::Abort()
	{
		Log("[DEBUG]", "Stopping ", _runningScripts->Count.ToString(), " script(s) ...");

		for each (Script ^script in _runningScripts)
		{
			AbortScript(script);

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

		Log("[DEBUG]", "Aborted script '", script->Name, "'.");
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
				Log("[ERROR]", "Script '", script->Name, "' is not responding! Aborting ...");

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
			KeyEventArgs ^args = gcnew KeyEventArgs(key | (statusCtrl ? Keys::Control : Keys::None) | (statusShift ? Keys::Shift : Keys::None) | (statusAlt ? Keys::Alt : Keys::None));
			Tuple<bool, KeyEventArgs ^> ^eventinfo = gcnew Tuple<bool, KeyEventArgs ^>(status, args);

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
		const int size = Text::Encoding::UTF8->GetByteCount(string);
		IntPtr handle(new unsigned char[size + 1]());

		Runtime::InteropServices::Marshal::Copy(Text::Encoding::UTF8->GetBytes(string), 0, handle, size);

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
		for each (Tuple<String ^, Type ^> ^scripttype in _scriptTypes)
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
