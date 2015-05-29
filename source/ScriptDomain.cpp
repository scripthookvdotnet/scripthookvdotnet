/**
 * Copyright (C) 2015 Crosire
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

#include "Log.hpp"
#include "ScriptDomain.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Threading;
	using namespace System::Windows::Forms;
	using namespace System::Collections::Generic;

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
			Log::Error("Caught unhandled exception:", Environment::NewLine, args->ExceptionObject->ToString());
		}
		else
		{
			Log::Error("Caught fatal unhandled exception:", Environment::NewLine, args->ExceptionObject->ToString());
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

	ScriptDomain::ScriptDomain() : mAppDomain(System::AppDomain::CurrentDomain), mExecutingThreadId(Thread::CurrentThread->ManagedThreadId), mRunningScripts(gcnew List<Script ^>()), mTaskQueue(gcnew Queue<IScriptTask ^>()), mPinnedStrings(gcnew List<IntPtr>()), mScriptTypes(gcnew List<Tuple<String ^, Type ^> ^>()), mKeyboardState(gcnew array<bool>(255))
	{
		sCurrentDomain = this;

		this->mAppDomain->AssemblyResolve += gcnew ResolveEventHandler(&HandleResolve);
		this->mAppDomain->UnhandledException += gcnew UnhandledExceptionEventHandler(&HandleUnhandledException);

		Log::Debug("Created script domain '", this->mAppDomain->FriendlyName, "'.");
	}
	ScriptDomain::~ScriptDomain()
	{
		CleanupStrings();

		Log::Debug("Deleted script domain '", this->mAppDomain->FriendlyName, "'.");
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
			Log::Error("Failed to create script domain '", appdomain->FriendlyName, "':", Environment::NewLine, ex->ToString());

			System::AppDomain::Unload(appdomain);

			return nullptr;
		}

		Log::Debug("Loading scripts from '", path, "' into script domain '", appdomain->FriendlyName, "' ...");

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
				Log::Error("Failed to reload scripts:", Environment::NewLine, ex->ToString());

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
			Log::Error("Failed to reload scripts because directory is missing.");
		}

		return scriptdomain;
	}
	bool ScriptDomain::LoadScript(String ^filename)
	{
		String ^extension = IO::Path::GetExtension(filename);
		CodeDom::Compiler::CodeDomProvider ^compiler = nullptr;
		bool csharp = false;
		if (extension->Equals(".cs", StringComparison::InvariantCultureIgnoreCase))
		{
			compiler = gcnew Microsoft::CSharp::CSharpCodeProvider();
			csharp = true;
		}
		else if (extension->Equals(".vb", StringComparison::InvariantCultureIgnoreCase))
		{
			compiler = gcnew Microsoft::VisualBasic::VBCodeProvider();
		}
		else
		{
			return false;
		}

		CodeDom::Compiler::CompilerParameters ^compilerOptions = gcnew CodeDom::Compiler::CompilerParameters();
		compilerOptions->CompilerOptions = "/optimize" + csharp ? " /unsafe" : "";
		compilerOptions->GenerateInMemory = true;
		compilerOptions->IncludeDebugInformation = true;
		compilerOptions->ReferencedAssemblies->Add("System.dll");
		compilerOptions->ReferencedAssemblies->Add("System.Drawing.dll");
		compilerOptions->ReferencedAssemblies->Add("System.Windows.Forms.dll");
		compilerOptions->ReferencedAssemblies->Add(GTA::Script::typeid->Assembly->Location);

		CodeDom::Compiler::CompilerResults ^compilerResult = compiler->CompileAssemblyFromFile(compilerOptions, filename);

		if (!compilerResult->Errors->HasErrors)
		{
			Log::Debug("Successfully compiled '", IO::Path::GetFileName(filename), "'.");

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

			Log::Error("Failed to compile '", IO::Path::GetFileName(filename), "' with ", compilerResult->Errors->Count.ToString(), " error(s):", Environment::NewLine, errors->ToString());

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
			Log::Error("Failed to load assembly '", IO::Path::GetFileName(filename), "':", Environment::NewLine, ex->ToString());

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
				this->mScriptTypes->Add(gcnew Tuple<String ^, Type ^>(filename, type));
			}
		}
		catch (Reflection::ReflectionTypeLoadException ^ex)
		{
			Log::Error("Failed to list assembly types:", Environment::NewLine, ex->ToString());

			return false;
		}

		Log::Debug("Found ", count.ToString(), " script(s) in '", IO::Path::GetFileName(filename), "'.");

		return count != 0;
	}
	void ScriptDomain::Unload(ScriptDomain ^%domain)
	{
		Log::Debug("Unloading script domain '", domain->Name, "' ...");

		domain->Abort();

		System::AppDomain ^appdomain = domain->AppDomain;

		delete domain;

		try
		{
			System::AppDomain::Unload(appdomain);
		}
		catch (Exception ^ex)
		{
			Log::Error("Failed to unload deleted script domain:", Environment::NewLine, ex->ToString());
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

		Log::Debug("Instantiating script '", scripttype->FullName, "' in script domain '", Name, "' ...");

		try
		{
			return static_cast<Script ^>(Activator::CreateInstance(scripttype));
		}
		catch (MissingMethodException ^)
		{
			Log::Error("Failed to instantiate script '", scripttype->FullName, "' because no public default constructor was found.");
		}
		catch (Reflection::TargetInvocationException ^ex)
		{
			Log::Error("Failed to instantiate script '", scripttype->FullName, "' because constructor threw an exception:", Environment::NewLine, ex->InnerException->ToString());
		}
		catch (Exception ^ex)
		{
			Log::Error("Failed to instantiate script '", scripttype->FullName, "':", Environment::NewLine, ex->ToString());
		}

		return nullptr;
	}

	void ScriptDomain::Start()
	{
		if (this->mRunningScripts->Count != 0 || this->mScriptTypes->Count == 0)
		{
			return;
		}

		Log::Debug("Starting ", this->mScriptTypes->Count.ToString(), " script(s) ...");

		for each (Tuple<String ^, Type ^> ^scripttype in this->mScriptTypes)
		{
			Script ^script = InstantiateScript(scripttype->Item2);

			if (Object::ReferenceEquals(script, nullptr))
			{
				continue;
			}

			script->mRunning = true;
			script->mFilename = scripttype->Item1;
			script->mScriptDomain = this;
			script->mThread = gcnew Thread(gcnew ThreadStart(script, &Script::MainLoop));

			script->mThread->Start();

			Log::Debug("Started script '", script->Name, "'.");

			this->mRunningScripts->Add(script);
		}
	}
	void ScriptDomain::Abort()
	{
		Log::Debug("Stopping ", this->mRunningScripts->Count.ToString(), " script(s) ...");

		for each (Script ^script in this->mRunningScripts)
		{
			AbortScript(script);

			delete script;
		}

		this->mScriptTypes->Clear();
		this->mRunningScripts->Clear();

		GC::Collect();
	}
	void ScriptDomain::AbortScript(Script ^script)
	{
		if (Object::ReferenceEquals(script->mThread, nullptr))
		{
			return;
		}

		script->mRunning = false;

		script->mThread->Abort();
		script->mThread = nullptr;

		Log::Debug("Aborted script '", script->Name, "'.");
	}
	void ScriptDomain::DoTick()
	{
		// Execute scripts
		for each (Script ^script in this->mRunningScripts)
		{
			if (!script->mRunning)
			{
				continue;
			}

			this->mExecutingScript = script;

			while ((script->mRunning = SignalAndWait(script->mContinueEvent, script->mWaitEvent, 5000)) && this->mTaskQueue->Count > 0)
			{
				this->mTaskQueue->Dequeue()->Run();
			}

			this->mExecutingScript = nullptr;

			if (!script->mRunning)
			{
				Log::Error("Script '", script->Name, "' is not responding! Aborting ...");

				AbortScript(script);
				continue;
			}
		}

		// Clean up pinned strings
		CleanupStrings();
	}
	void ScriptDomain::DoKeyboardMessage(Keys key, bool status, bool statusCtrl, bool statusShift, bool statusAlt)
	{
		this->mKeyboardState[static_cast<int>(key)] = status;

		KeyEventArgs ^args = gcnew KeyEventArgs(key | (statusCtrl ? Keys::Control : Keys::None) | (statusShift ? Keys::Shift : Keys::None) | (statusAlt ? Keys::Alt : Keys::None));
		Tuple<bool, KeyEventArgs ^> ^eventinfo = gcnew Tuple<bool, KeyEventArgs ^>(status, args);

		for each (Script ^script in this->mRunningScripts)
		{
			script->mKeyboardEvents->Enqueue(eventinfo);
		}
	}

	void ScriptDomain::ExecuteTask(IScriptTask ^task)
	{
		if (Thread::CurrentThread->ManagedThreadId == this->mExecutingThreadId)
		{
			task->Run();
		}
		else
		{
			this->mTaskQueue->Enqueue(task);

			SignalAndWait(ExecutingScript->mWaitEvent, ExecutingScript->mContinueEvent);
		}
	}
	IntPtr ScriptDomain::PinString(String ^string)
	{
		const IntPtr handle = Runtime::InteropServices::Marshal::StringToHGlobalAnsi(string);

		this->mPinnedStrings->Add(handle);

		return handle;
	}
	void ScriptDomain::CleanupStrings()
	{
		for each (IntPtr handle in this->mPinnedStrings)
		{
			Runtime::InteropServices::Marshal::FreeHGlobal(handle);
		}

		this->mPinnedStrings->Clear();
	}
	Object ^ScriptDomain::InitializeLifetimeService()
	{
		return nullptr;
	}
}