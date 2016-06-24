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
		DateTime now = DateTime::Now;
		String ^logpath = IO::Path::ChangeExtension(Assembly::GetExecutingAssembly()->Location, ".log");

		logpath = logpath->Insert(logpath->IndexOf(".log"), "-" + now.ToString("yyyy-MM-dd"));

		try
		{
			auto fs = gcnew IO::FileStream(logpath, IO::FileMode::Append, IO::FileAccess::Write, IO::FileShare::Read);
			auto sw = gcnew IO::StreamWriter(fs);

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
	String^ FixFilePathCasing(String ^filePath, String ^BaseDir = nullptr)
	{
		if (!String::IsNullOrEmpty(BaseDir))
		{
			filePath = System::IO::Path::Combine(BaseDir, filePath);
		}
		String^  sTmp = "";
		for each(String^  sPth in filePath->Split('\\'))
		{
			if (String::IsNullOrEmpty(sTmp))
			{
				sTmp = sPth + "\\";
				continue;
			}
			sTmp = System::IO::Directory::GetFileSystemEntries(sTmp, sPth)[0];
		}
		if (!String::IsNullOrEmpty(BaseDir))
		{
			if (sTmp->StartsWith(BaseDir))
			{
				sTmp = sTmp->Substring(BaseDir->Length + (BaseDir->EndsWith("\\") ? 0 : 1));
			}
		}
		return sTmp;
	}
	Assembly ^HandleResolve(Object ^sender, ResolveEventArgs ^args)
	{
		auto assembly = Script::typeid->Assembly;
		auto assemblyName = gcnew AssemblyName(args->Name);

		if (assemblyName->Name->StartsWith("ScriptHookVDotNet", StringComparison::CurrentCultureIgnoreCase))
		{
			if (assemblyName->Version->Major != assembly->GetName()->Version->Major)
			{
				Log("[WARNING]", "A script references v", assemblyName->Version->ToString(3), " which may not be compatible with the current v" + assembly->GetName()->Version->ToString(3), ".");
			}

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

	ScriptDomain::ScriptDomain() : _appdomain(System::AppDomain::CurrentDomain), _executingThreadId(Thread::CurrentThread->ManagedThreadId)
	{
		sCurrentDomain = this;

		_appdomain->AssemblyResolve += gcnew ResolveEventHandler(&HandleResolve);
		_appdomain->UnhandledException += gcnew UnhandledExceptionEventHandler(&HandleUnhandledException);

		Log("[INFO]", "Created new script domain with v", ScriptDomain::typeid->Assembly->GetName()->Version->ToString(3), ".");

		_console = gcnew ConsoleScript();
	}
	ScriptDomain::~ScriptDomain()
	{
		CleanupStrings();
	}

	void ScriptDomain::ConsoleLoadScript(String ^filename)
	{
		if (!System::IO::File::Exists(System::IO::Path::Combine(_appdomain->BaseDirectory, filename)))
		{
			array<String ^> ^AllFiles = System::IO::Directory::GetFiles(_appdomain->BaseDirectory, filename, System::IO::SearchOption::AllDirectories);
			if (AllFiles->Length == 0)
			{
				Console->Error("The specified file '" + filename + "' was not found in '" + _appdomain->BaseDirectory + "'");
				return;
			}
			else if (AllFiles->Length > 1)
			{
				Console->Error("The specified file '" + filename + "' was found in multiple subdirectories of '" + _appdomain->BaseDirectory + "'. Please specify which file you want to load");
				int remLength = _appdomain->BaseDirectory->Length + 1;
				for each (auto file in AllFiles)
				{
					Console->Info(file->Substring(remLength));
				}
				return;
			}
			else
			{
				Console->Warn("The specified file was not found in '" + _appdomain->BaseDirectory + "', loading from the '" + System::IO::Path::GetDirectoryName(AllFiles[0]->Substring(_appdomain->BaseDirectory->Length + 1)) + "' sub directory");
				filename = AllFiles[0]->Substring(_appdomain->BaseDirectory->Length + 1);
			}
		}
		else
		{
			filename = FixFilePathCasing(filename, _appdomain->BaseDirectory);
		}
		String ^ext = System::IO::Path::GetExtension(filename)->ToLower();
		if (ext != ".cs" && ext != ".vb" && ext != ".dll")
		{
			Console->Error("The specified file '" + filename + "' was not recognised as a script, please include the file extension.");
			return;
		}
		String ^filepath = System::IO::Path::Combine(_appdomain->BaseDirectory, filename);
		for each(auto val in _scriptTypes)
		{
			if (filepath->Equals(val->Item1, StringComparison::InvariantCultureIgnoreCase))
			{
				Console->Warn("The script is currently loaded, unloading");
				ConsoleUnloadScript(filename);
				break;
			}
		}
		ConsoleStartScript(filename);
	}
	void ScriptDomain::ConsoleUnloadScript(String ^filename)
	{
		String ^filepath = System::IO::Path::Combine(_appdomain->BaseDirectory, filename);
		//cant use the same file finding method incase the script was deleted, could maybe implement a searching method that would use the filenames in ScriptTypes
		bool foundany = false;
		for (int i = 0; i<_scriptTypes->Count;i++)
		{
			auto val = _scriptTypes[i];
			if (filepath->Equals(val->Item1, StringComparison::InvariantCultureIgnoreCase))
			{
				Console->UnregisterCommands(val->Item2);
				bool found = false;
				for each(auto script in _runningScripts)
				{
					if (script->GetType() == val->Item2)
					{
						if (!Object::ReferenceEquals(script->_thread, nullptr))
						{
							script->_running = false;

							script->_thread->Abort();
							script->_thread = nullptr;

							Console->Info("Aborted script '" + script->Name + "'.");
						}

						
						_runningScripts->Remove(script);
						_scriptTypes->RemoveAt(i--);
						found = true;
						break;
					}
				}
				if (!found)
				{
					Console->Warn("Couldn't find a running instance of '" + val->Item2->Name + "'.");
				}
				foundany = true;
			}
		}
		if (!foundany)
		{
			if (System::IO::File::Exists(filepath))
			{
				Console->Error("There were no loaded scripts found in the file '" + filename + "'.");
			}
			else
			{
				Console->Error("The specified file '" + filename + "'does not exist, did you include directoy information and file extension?");
			}
		}
	}
	void ScriptDomain::ConsoleReloadScript(String ^filename)
	{
		if (!System::IO::File::Exists(System::IO::Path::Combine(_appdomain->BaseDirectory, filename)))
		{
			array<String ^> ^AllFiles = System::IO::Directory::GetFiles(_appdomain->BaseDirectory, filename, System::IO::SearchOption::AllDirectories);
			if (AllFiles->Length == 0)
			{
				Console->Error("The specified file '" + filename + "' was not found in '" + _appdomain->BaseDirectory + "'");
				return;
			}
			else if (AllFiles->Length > 1)
			{
				Console->Error("The specified file '" + filename + "' was found in multiple subdirectories of '" + _appdomain->BaseDirectory + "'. Please specify which file you want to load");
				int remLength = _appdomain->BaseDirectory->Length + 1;
				for each (auto file in AllFiles)
				{
					Console->Info(file->Substring(remLength));
				}
				return;
			}
			else
			{
				Console->Warn("The specified file was not found in '" + _appdomain->BaseDirectory + "', loading from the '" + System::IO::Path::GetDirectoryName(AllFiles[0]->Substring(_appdomain->BaseDirectory->Length + 1)) + "' sub directory");
				filename = AllFiles[0]->Substring(_appdomain->BaseDirectory->Length + 1);
			}
		}
		else
		{
			filename = FixFilePathCasing(filename, _appdomain->BaseDirectory);
		}
		String ^ext = System::IO::Path::GetExtension(filename)->ToLower();
		if (ext != ".cs" && ext != ".vb" && ext != ".dll")
		{
			Console->Error("The specified file '" + filename + "' was not recognised as a script, please include the file extension.");
			return;
		}
		String ^filepath = System::IO::Path::Combine(_appdomain->BaseDirectory, filename);
		bool found = false;
		for each(auto val in _scriptTypes)
		{
			if (filepath->Equals(val->Item1, StringComparison::InvariantCultureIgnoreCase))
			{
				ConsoleUnloadScript(filename);
				found = true;
				break;
			}
		}
		if (!found)
		{
			Console->Warn("The script was not currently running.");
		}
		ConsoleStartScript(filename);
	}
	void ScriptDomain::ConsoleStartScript(String ^filename)
	{
		String ^filepath = System::IO::Path::Combine(_appdomain->BaseDirectory, filename);
		if (!System::IO::File::Exists(filepath))
		{
			Console->Error("The specified script file '" + filename + "' doesnt exist");
			return;
		}
		String ^extension = IO::Path::GetExtension(filepath);
		Assembly^ assembly = nullptr;
		if (extension->Equals(".dll", StringComparison::InvariantCultureIgnoreCase))
		{
			try
			{
				assembly = Assembly::LoadFrom(filepath);
			}
			catch (Exception ^ex)
			{
				Console->Error("Failed to load assembly '" + IO::Path::GetFileName(filepath) + "':" + Environment::NewLine + ex->ToString());

				return;
			}
		}
		else
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
				return;
			}
			CodeDom::Compiler::CompilerResults ^compilerResult = compiler->CompileAssemblyFromFile(compilerOptions, filepath);

			if (!compilerResult->Errors->HasErrors)
			{
				Console->Info("Successfully compiled '" + IO::Path::GetFileName(filepath) + "'.");

				assembly = compilerResult->CompiledAssembly;
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

				Console->Error("Failed to compile '" + IO::Path::GetFileName(filepath) + "' with " + compilerResult->Errors->Count.ToString() + " error(s):" + Environment::NewLine + errors->ToString());

				return;
			}
		}

		unsigned int count = 0;
		List<Tuple<String ^, Type ^>^>^ ScriptTypes = gcnew List<Tuple<String ^, Type ^>^>();
		try
		{
			for each (auto type in assembly->GetTypes())
			{
				if (!type->IsSubclassOf(Script::typeid))
				{
					continue;
				}

				count++;
				_scriptTypes->Add(gcnew Tuple<String ^, Type ^>(filepath, type));
				ScriptTypes->Add(gcnew Tuple<String ^, Type ^>(filepath, type));
			}
		}
		catch (ReflectionTypeLoadException ^ex)
		{
			Console->Error("Failed to load assembly '" + IO::Path::GetFileName(filepath) + "':" + Environment::NewLine, ex->ToString());
			return;
		}
		Console->Info("Found " + count.ToString() + " script(s) in '" + IO::Path::GetFileName(filepath) + "'.");
		Console->Info("Starting " + count.ToString() + " script(s) ...");

	//	if (!SortScripts(ScriptTypes))
	//	{
	//		return;
	//	}

		for each (auto scriptType in ScriptTypes)
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

			Console->Info("Started script '" + script->Name + "'.");

			_runningScripts->Add(script);
		}
		return;
		
	}

	ScriptDomain ^ScriptDomain::Load(String ^path)
	{
		if (!IO::Path::IsPathRooted(path))
		{
			path = IO::Path::Combine(IO::Path::GetDirectoryName(Assembly::GetExecutingAssembly()->Location), path);
		}

		path = IO::Path::GetFullPath(path);

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

		Log("[INFO]", "Loading scripts from '", path, "' ...");

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
			Log("[INFO]", "Successfully compiled '", IO::Path::GetFileName(filename), "'.");

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
			Log("[ERROR]", "Failed to load assembly '", IO::Path::GetFileName(filename), "':", Environment::NewLine, ex->ToString());

			return false;
		}

		Log("[INFO]", "Found ", count.ToString(), " script(s) in '", IO::Path::GetFileName(filename), "'.");

		return count != 0;
	}
	void ScriptDomain::Unload(ScriptDomain ^%domain)
	{
		Log("[INFO]", "Unloading script domain ...");

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

		Log("[INFO]", "Instantiating script '", scriptType->FullName, "' in script domain '", Name, "' ...");

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
		if (_runningScripts->Count != 0)
		{
			return;
		}

		// Start console
		_console->_running = true;
		_console->_thread = gcnew Thread(gcnew ThreadStart(_console, &Script::MainLoop));

		_console->_thread->Start();

		_runningScripts->Add(_console);

		// Start script threads
		Log("[INFO]", "Starting ", _scriptTypes->Count.ToString(), " script(s) ...");

		if (_scriptTypes->Count == 0 || !SortScripts(_scriptTypes))
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

			Log("[INFO]", "Started script '", script->Name, "'.");

			_runningScripts->Add(script);

			_console->RegisterCommands(scriptType->Item2);
		}
	}
	void ScriptDomain::Abort()
	{
		_runningScripts->Remove(_console);

		Log("[INFO]", "Stopping ", _runningScripts->Count.ToString(), " script(s) ...");

		for each (Script ^script in _runningScripts)
		{
			script->Abort();

			delete script;
		}

		_console->Abort();

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

		Log("[INFO]", "Aborted script '", script->Name, "'.");
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

			if (_console->IsOpen())
			{
				_console->_keyboardEvents->Enqueue(eventinfo);
			}
			else
			{
				for each (Script ^script in _runningScripts)
				{
					script->_keyboardEvents->Enqueue(eventinfo);
				}
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
