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
using namespace System::Threading;
using namespace System::Reflection;
using namespace System::Collections::Generic;
namespace WinForms = System::Windows::Forms;

namespace
{
	inline void SignalAndWait(SemaphoreSlim ^toSignal, SemaphoreSlim ^toWaitOn)
	{
		toSignal->Release();
		toWaitOn->Wait();
	}
	inline bool SignalAndWait(SemaphoreSlim ^toSignal, SemaphoreSlim ^toWaitOn, int timeout)
	{
		toSignal->Release();
		return toWaitOn->Wait(timeout);
	}
}

namespace GTA
{
	void Log(String ^logLevel, ... array<String ^> ^message)
	{
		auto datetime = DateTime::Now;
		String ^logPath = IO::Path::ChangeExtension(Assembly::GetExecutingAssembly()->Location, ".log");

		try
		{
			auto fs = gcnew IO::FileStream(logPath, IO::FileMode::Append, IO::FileAccess::Write, IO::FileShare::Read);
			auto sw = gcnew IO::StreamWriter(fs);

			try
			{
				sw->Write(String::Concat("[", datetime.ToString("HH:mm:ss"), "] ", logLevel, " "));

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
		catch (...) { }
	}
	Assembly ^HandleResolve(Object ^sender, ResolveEventArgs ^args)
	{
		auto assembly = Script::typeid->Assembly;
		auto assemblyName = gcnew AssemblyName(args->Name);

		if (assemblyName->Name->StartsWith("ScriptHookVDotNet", StringComparison::CurrentCultureIgnoreCase))
		{
			if (assemblyName->Version->Major > assembly->GetName()->Version->Major)
			{
				Log("[WARNING]", "A script references v", assemblyName->Version->ToString(3), " which may not be compatible with the current v" + assembly->GetName()->Version->ToString(3), " and was therefore ignored.");
			}
			else
			{
				return assembly;
			}
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

	ScriptDomain::ScriptDomain() : _appdomain(System::AppDomain::CurrentDomain), _executingThreadId(Thread::CurrentThread->ManagedThreadId)
	{
		sCurrentDomain = this;

		_appdomain->AssemblyResolve += gcnew ResolveEventHandler(&HandleResolve);
		_appdomain->UnhandledException += gcnew UnhandledExceptionEventHandler(&HandleUnhandledException);

		Log("[DEBUG]", "Created script domain '", _appdomain->FriendlyName, "' with v", ScriptDomain::typeid->Assembly->GetName()->Version->ToString(3), ".");
	}
	ScriptDomain::~ScriptDomain()
	{
		CleanupStrings();

		Log("[DEBUG]", "Deleted script domain '", _appdomain->FriendlyName, "'.");
	}

	ScriptDomain ^ScriptDomain::Load(String ^path)
	{
		if (!IO::Path::IsPathRooted(path))
		{
			path = IO::Path::Combine(IO::Path::GetDirectoryName(Assembly::GetExecutingAssembly()->Location), path);
		}

		path = IO::Path::GetFullPath(path);

		// Clear log
		String ^logPath = IO::Path::ChangeExtension(Assembly::GetExecutingAssembly()->Location, ".log");

		try
		{
			IO::File::WriteAllText(logPath, String::Empty);
		}
		catch (...) { }

		// Create AppDomain
		auto setup = gcnew AppDomainSetup();
		setup->ApplicationBase = path;
		setup->ShadowCopyFiles = "true";
		setup->ShadowCopyDirectories = path;

		auto appdomain = System::AppDomain::CreateDomain("ScriptDomain_" + (path->GetHashCode() * Environment::TickCount).ToString("X"), nullptr, setup, gcnew Security::PermissionSet(Security::Permissions::PermissionState::Unrestricted));
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
			auto filenameScripts = gcnew List<String ^>();
			auto filenameAssemblies = gcnew List<String ^>();

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
				if (IsManagedAssembly(filename))
				{
					scriptdomain->LoadAssembly(filename);
				}
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
		compilerOptions->ReferencedAssemblies->Add("System.dll");
		compilerOptions->ReferencedAssemblies->Add("System.Core.dll");
		compilerOptions->ReferencedAssemblies->Add("System.Drawing.dll");
		compilerOptions->ReferencedAssemblies->Add("System.Windows.Forms.dll");
		compilerOptions->ReferencedAssemblies->Add("System.XML.dll");
		compilerOptions->ReferencedAssemblies->Add("System.XML.Linq.dll");
		compilerOptions->ReferencedAssemblies->Add(GTA::Script::typeid->Assembly->Location);

		String ^extension = IO::Path::GetExtension(filename);
		CodeDom::Compiler::CodeDomProvider ^compiler = nullptr;

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
			auto errors = gcnew Text::StringBuilder();

			for each (CodeDom::Compiler::CompilerError ^error in compilerResult->Errors)
			{
				errors->Append("   at line ");
				errors->Append(error->Line);
				errors->Append(": ");
				errors->Append(error->ErrorText);
				errors->AppendLine();
			}

			Log("[ERROR]", "Failed to compile '", IO::Path::GetFileName(filename), "' with ", compilerResult->Errors->Count.ToString(), " error(s):", Environment::NewLine, errors->ToString());

			return false;
		}
	}
	bool ScriptDomain::LoadAssembly(String ^filename)
	{
		if (IO::Path::GetFileNameWithoutExtension(filename)->StartsWith("ScriptHookVDotNet", StringComparison::CurrentCultureIgnoreCase))
		{
			Log("[WARNING]", "Skipped assembly '", IO::Path::GetFileName(filename), "'. Please remove it from the directory.");

			return false;
		}

		Log("[INFO]", "Loading assembly '", IO::Path::GetFileName(filename), "' ...");

		Assembly ^assembly = nullptr;

		try
		{
			assembly = Assembly::LoadFrom(filename);
		}
		catch (Exception ^ex)
		{
			Log("[ERROR]", "Failed to load assembly '", IO::Path::GetFileName(filename), "':", Environment::NewLine, ex->ToString());

			return false;
		}

		return LoadAssembly(filename, assembly);
	}
	bool ScriptDomain::LoadAssembly(String ^filename, Assembly ^assembly)
	{
		unsigned int count = 0;

		try
		{
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
		catch (ReflectionTypeLoadException ^ex)
		{
			auto fileNotFoundException = safe_cast<IO::FileNotFoundException ^>(ex->LoaderExceptions[0]);

			if (ReferenceEquals(fileNotFoundException, nullptr) || fileNotFoundException->Message->IndexOf("ScriptHookVDotNet", StringComparison::OrdinalIgnoreCase) < 0)
			{
				Log("[ERROR]", "Failed to load assembly '", IO::Path::GetFileName(filename), "':", Environment::NewLine, ex->LoaderExceptions[0]->ToString());
			}

			return false;
		}

		Log("[DEBUG]", "Found ", count.ToString(), " script(s) in '", IO::Path::GetFileName(filename), "'.");

		return count != 0;
	}
	bool ScriptDomain::IsManagedAssembly(String ^filename)
	{
		System::IO::FileStream^ fileStream = gcnew System::IO::FileStream(filename, System::IO::FileMode::Open, System::IO::FileAccess::Read);
		System::IO::BinaryReader^ binaryReader = gcnew System::IO::BinaryReader(fileStream);

		try
		{
			if (fileStream->Length < 64)
			{
				return false;
			}

			//PE Header starts @ 0x3C (60). Its a 4 byte header.
			fileStream->Position = 0x3C;
			unsigned int peHeaderPointer = binaryReader->ReadUInt32();
			if (peHeaderPointer == 0)
			{
				peHeaderPointer = 0x80;
			}

			// Ensure there is at least enough room for the following structures:
			//     24 byte PE Signature & Header
			//     28 byte Standard Fields         (24 bytes for PE32+)
			//     68 byte NT Fields               (88 bytes for PE32+)
			// >= 128 byte Data Dictionary Table
			if (peHeaderPointer > fileStream->Length - 256)
			{
				return false;
			}

			// Check the PE signature.  Should equal 'PE\0\0'.
			fileStream->Position = peHeaderPointer;
			unsigned int peHeaderSignature = binaryReader->ReadUInt32();
			if (peHeaderSignature != 0x00004550)
			{
				return false;
			}

			// skip over the PEHeader fields
			fileStream->Position += 20;

			const unsigned short PE32 = 0x10b;
			const unsigned short PE32Plus = 0x20b;

			// Read PE magic number from Standard Fields to determine format.
			auto peFormat = binaryReader->ReadUInt16();
			if (peFormat != PE32 && peFormat != PE32Plus)
			{
				return false;
			}

			// Read the 15th Data Dictionary RVA field which contains the CLI header RVA.
			// When this is non-zero then the file contains CLI data otherwise not.
			unsigned short dataDictionaryStart = static_cast<unsigned short>(peHeaderPointer + (peFormat == PE32 ? 232 : 248));
			fileStream->Position = dataDictionaryStart;

			unsigned int cliHeaderRva = binaryReader->ReadUInt32();
			if (cliHeaderRva == 0)
			{
				return false;
			}

			return true;
		}
		finally
		{
			delete binaryReader;
			delete fileStream;
		}
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
	Script ^ScriptDomain::InstantiateScript(Type ^scriptType)
	{
		if (!scriptType->IsSubclassOf(Script::typeid))
		{
			return nullptr;
		}

		Log("[DEBUG]", "Instantiating script '", scriptType->FullName, "' in script domain '", Name, "' ...");

		try
		{
			return static_cast<Script ^>(Activator::CreateInstance(scriptType));
		}
		catch (MissingMethodException ^)
		{
			Log("[ERROR]", "Failed to instantiate script '", scriptType->FullName, "' because no public default constructor was found.");
		}
		catch (TargetInvocationException ^ex)
		{
			Log("[ERROR]", "Failed to instantiate script '", scriptType->FullName, "' because constructor threw an exception:", Environment::NewLine, ex->InnerException->ToString());
		}
		catch (Exception ^ex)
		{
			Log("[ERROR]", "Failed to instantiate script '", scriptType->FullName, "':", Environment::NewLine, ex->ToString());
		}

		return nullptr;
	}

	bool SortScripts(List<Tuple<String ^, Type ^> ^> ^%scriptTypes)
	{
		auto graph = gcnew Dictionary<Tuple<String ^, Type ^> ^, List<Type ^> ^>();

		for each (auto scriptType in scriptTypes)
		{
			auto dependencies = gcnew List<Type ^>();

			for each (RequireScript ^attribute in static_cast<MemberInfo ^>(scriptType->Item2)->GetCustomAttributes(RequireScript::typeid, true))
			{
				dependencies->Add(attribute->_dependency);
			}

			graph->Add(scriptType, dependencies);
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

		scriptTypes = result;

		return true;
	}
	void ScriptDomain::Start()
	{
		if (_runningScripts->Count != 0 || _scriptTypes->Count == 0)
		{
			return;
		}

		Log("[DEBUG]", "Starting ", _scriptTypes->Count.ToString(), " script(s) ...");

		if (!SortScripts(_scriptTypes))
		{
			return;
		}

		for each (auto scriptType in _scriptTypes)
		{
			Script ^script = InstantiateScript(scriptType->Item2);

			if (Object::ReferenceEquals(script, nullptr))
			{
				continue;
			}

			script->_running = true;
			script->_filename = scriptType->Item1;
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
	void ScriptDomain::DoKeyboardMessage(WinForms::Keys key, bool status, bool statusCtrl, bool statusShift, bool statusAlt)
	{
		const int keycode = static_cast<int>(key);

		if (keycode < 0 || keycode >= _keyboardState->Length)
		{
			return;
		}

		_keyboardState[keycode] = status;

		if (_recordKeyboardEvents)
		{
			if (statusCtrl)
			{
				key = key | WinForms::Keys::Control;
			}
			if (statusShift)
			{
				key = key | WinForms::Keys::Shift;
			}
			if (statusAlt)
			{
				key = key | WinForms::Keys::Alt;
			}

			auto args = gcnew WinForms::KeyEventArgs(key);
			auto eventinfo = gcnew Tuple<bool, WinForms::KeyEventArgs ^>(status, args);

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
		for each (auto scriptType in _scriptTypes)
		{
			if (scriptType->Item2 == type)
			{
				return scriptType->Item1;
			}
		}

		return String::Empty;
	}
	Object ^ScriptDomain::InitializeLifetimeService()
	{
		return nullptr;
	}
}
