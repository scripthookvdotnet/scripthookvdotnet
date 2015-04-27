#include "ScriptDomain.hpp"

#include <windows.h>

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	namespace
	{
		void Log(... array<String ^> ^message)
		{
			String ^logpath = IO::Path::ChangeExtension(Reflection::Assembly::GetExecutingAssembly()->Location, ".log");
			IO::FileStream ^fs = gcnew IO::FileStream(logpath, IO::FileMode::Append, IO::FileAccess::Write, IO::FileShare::Read);
			IO::StreamWriter ^sw = gcnew IO::StreamWriter(fs);

			sw->Write(String::Concat("[", DateTime::Now.ToString("HH:mm:ss"), "] "));

			for each (String ^string in message)
			{
				sw->Write(string);
			}

			sw->WriteLine();

			sw->Close();
			fs->Close();
		}

		void UnhandledExceptionHandler(Object ^sender, UnhandledExceptionEventArgs ^args)
		{
			if (args->IsTerminating)
			{
				Log("Caught fatal unhandled exception:", Environment::NewLine, args->ExceptionObject->ToString());
			}
			else
			{
				ScriptDomain::CurrentDomain->HandleException(static_cast<Exception ^>(args->ExceptionObject));
			}
		}
		Reflection::Assembly ^ResolveHandler(Object ^sender, ResolveEventArgs ^args)
		{
			if (args->Name->ToLower()->Contains("scripthookvdotnet"))
			{
				return Reflection::Assembly::GetAssembly(GTA::Script::typeid);
			}
			else
			{
				return nullptr;
			}
		}
	}

	ScriptDomain::ScriptDomain() : mKeyboardState(gcnew array<bool>(256)), mPinnedStrings(gcnew List<IntPtr>()), mRunningScripts(gcnew List<Script ^>()), mScriptTypes(gcnew Dictionary<String ^, Type ^>())
	{
		Log("Initialized script domain.");

		AppDomain::CurrentDomain->AssemblyResolve += gcnew ResolveEventHandler(&ResolveHandler);
		AppDomain::CurrentDomain->UnhandledException += gcnew UnhandledExceptionEventHandler(&UnhandledExceptionHandler);
	}

	bool ScriptDomain::Reload()
	{
		if (!Object::ReferenceEquals(sAppDomain, nullptr))
		{
			Unload();
		}

		Reflection::Assembly ^assembly = Reflection::Assembly::GetAssembly(Script::typeid);

		sAppDomain = AppDomain::CreateDomain("ScriptDomain");
		sAppDomain->InitializeLifetimeService();

		try
		{
			sScriptDomain = static_cast<ScriptDomain ^>(sAppDomain->CreateInstanceFromAndUnwrap(assembly->Location, ScriptDomain::typeid->FullName));
		}
		catch (Exception ^ex)
		{
			Log("Failed to create script domain:", Environment::NewLine, ex->ToString());

			return false;
		}

		String ^path = IO::Path::Combine(IO::Path::GetDirectoryName(assembly->Location), "scripts");
		List<String ^> ^filenameScripts = gcnew List<String ^>(), ^filenameAssemblies = gcnew List<String ^>();

		Log("Loading scripts from '", path, "' ...");

		try
		{
			filenameScripts->AddRange(IO::Directory::GetFiles(path, "*.vb"));
			filenameScripts->AddRange(IO::Directory::GetFiles(path, "*.cs"));
			filenameAssemblies->AddRange(IO::Directory::GetFiles(path, "*.dll"));
		}
		catch (Exception ^ex)
		{
			Log("Failed to reload scripts:", Environment::NewLine, ex->ToString());

			return false;
		}

		for each (String ^filename in filenameScripts)
		{
			sScriptDomain->LoadScript(filename);
		}
		for each (String ^filename in filenameAssemblies)
		{
			sScriptDomain->LoadAssembly(filename);
		}

		return true;
	}
	void ScriptDomain::Unload()
	{
		if (Object::ReferenceEquals(sAppDomain, nullptr))
		{
			return;
		}

		Log("Unloading scripts ...");

		sScriptDomain->AbortScripts();

		try
		{
			AppDomain::Unload(sAppDomain);
		}
		catch (Exception ^ex)
		{
			Log("Failed to unload script domain:", Environment::NewLine, ex->ToString());
		}

		sAppDomain = nullptr;
		sScriptDomain = nullptr;

		GC::Collect();
	}

	bool ScriptDomain::LoadScript(String ^filename)
	{
		String ^extension = IO::Path::GetExtension(filename);
		CodeDom::Compiler::CodeDomProvider ^compiler = nullptr;

		if (extension->Equals(".cs", StringComparison::InvariantCultureIgnoreCase))
		{
			compiler = gcnew Microsoft::CSharp::CSharpCodeProvider();
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
		compilerOptions->CompilerOptions = "/optimize";
		compilerOptions->GenerateInMemory = true;
		compilerOptions->ReferencedAssemblies->Add("System.dll");
		compilerOptions->ReferencedAssemblies->Add("System.Drawing.dll");
		compilerOptions->ReferencedAssemblies->Add("System.Windows.Forms.dll");
		compilerOptions->ReferencedAssemblies->Add(Reflection::Assembly::GetAssembly(GTA::Script::typeid)->Location);

		CodeDom::Compiler::CompilerResults ^compilerResult = compiler->CompileAssemblyFromFile(compilerOptions, filename);

		if (!compilerResult->Errors->HasErrors)
		{
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

			Log("Failed to compile '", IO::Path::GetFileName(filename), "' with ", compilerResult->Errors->Count.ToString(), " error(s):", Environment::NewLine, errors->ToString());

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
			Log("Failed to load assembly '", IO::Path::GetFileName(filename), "':", Environment::NewLine, ex->ToString());

			return false;
		}

		return LoadAssembly(filename, assembly);
	}
	bool ScriptDomain::LoadAssembly(String ^filename, Reflection::Assembly ^assembly)
	{
		unsigned int count = 0;

		for each (Type ^type in assembly->GetTypes())
		{
			if (!type->IsSubclassOf(Script::typeid))
			{
				continue;
			}

			count++;
			this->mScriptTypes->Add(filename, type);
		}

		Log("Found ", count.ToString(), " script(s) in '", IO::Path::GetFileName(filename), "'.");

		return count != 0;
	}
	Script ^ScriptDomain::InstantiateScript(Type ^scripttype)
	{
		if (!scripttype->IsSubclassOf(Script::typeid))
		{
			return nullptr;
		}

		Log("Instantiating script '", scripttype->FullName, "' ...");

		try
		{
			return static_cast<Script ^>(Activator::CreateInstance(scripttype));
		}
		catch (MissingMethodException ^)
		{
			Log("Failed to instantiate script '", scripttype->FullName, "' because no public default constructor was found.");
		}
		catch (Reflection::TargetInvocationException ^ex)
		{
			Log("Failed to instantiate script '", scripttype->FullName, "' because constructor threw an exception:", Environment::NewLine, ex->InnerException->ToString());
		}
		catch (Exception ^ex)
		{
			Log("Failed to instantiate script '", scripttype->FullName, "':", Environment::NewLine, ex->ToString());
		}

		return nullptr;
	}

	void ScriptDomain::StartScripts()
	{
		if (this->mRunningScripts->Count != 0 || this->mScriptTypes->Count == 0)
		{
			return;
		}

		Log("Starting ", this->mScriptTypes->Count.ToString(), " script(s) ...");

		for each (KeyValuePair<String ^, Type ^> ^scripttype in this->mScriptTypes)
		{
			Script ^script = InstantiateScript(scripttype->Value);

			if (Object::ReferenceEquals(script, nullptr))
			{
				continue;
			}

			script->mRunning = true;
			script->mFilename = scripttype->Key;
			script->mScriptDomain = this;

			Log("Started script '", script->Name, "'.");

			this->mRunningScripts->Add(script);
		}
	}
	void ScriptDomain::AbortScript(Script ^script)
	{
		script->mRunning = false;

		Log("Aborted script '", script->Name, "'.");
	}
	void ScriptDomain::AbortScripts()
	{
		Log("Stopping ", this->mRunningScripts->Count.ToString(), " script(s) ...");

		for each (Script ^script in this->mRunningScripts)
		{
			AbortScript(script);
		}

		this->mScriptTypes->Clear();
		this->mRunningScripts->Clear();
	}

	void ScriptDomain::Run()
	{
		CurrentDomain = this;

		// Update keyboard input
		for (int key = 1; key < 255; ++key)
		{
			const bool status = (GetAsyncKeyState(key) & 0x8000) != 0;

			if (status == this->mKeyboardState[key])
			{
				continue;
			}

			const bool ctrl = IsKeyPressed(Windows::Forms::Keys::ControlKey) || IsKeyPressed(Windows::Forms::Keys::LControlKey) || IsKeyPressed(Windows::Forms::Keys::RControlKey);
			const bool shift = IsKeyPressed(Windows::Forms::Keys::ShiftKey) || IsKeyPressed(Windows::Forms::Keys::LShiftKey) || IsKeyPressed(Windows::Forms::Keys::RShiftKey);
			const bool alt = IsKeyPressed(Windows::Forms::Keys::Menu) || IsKeyPressed(Windows::Forms::Keys::LMenu) || IsKeyPressed(Windows::Forms::Keys::RMenu);

			Windows::Forms::KeyEventArgs ^args = gcnew Windows::Forms::KeyEventArgs(static_cast<Windows::Forms::Keys>(key) | (ctrl ? Windows::Forms::Keys::Control : Windows::Forms::Keys::None) | (shift ? Windows::Forms::Keys::Shift : Windows::Forms::Keys::None) | (alt ? Windows::Forms::Keys::Alt : Windows::Forms::Keys::None));

			for each (Script ^script in this->mRunningScripts)
			{
				try
				{
					if (!status)
					{
						script->RaiseKeyUp(this, args);
					}
					else
					{
						script->RaiseKeyDown(this, args);
					}
				}
				catch (Exception ^ex)
				{
					HandleException(ex);
				}
			}

			this->mKeyboardState[key] = status;
		}

		// Update scripts
		for each (Script ^script in this->mRunningScripts)
		{
			if (!script->mRunning)
			{
				continue;
			}
			else if (script->mInterval > 0)
			{
				if (script->mNextTick > DateTime::Now)
				{
					continue;
				}
				else
				{
					script->mNextTick = DateTime::Now + TimeSpan(0, 0, 0, 0, script->mInterval);
				}
			}

			try
			{
				script->RaiseTick(this);
			}
			catch (Exception ^ex)
			{
				HandleException(ex);

				AbortScript(script);
			}
		}

		for each (IntPtr handle in this->mPinnedStrings)
		{
			Runtime::InteropServices::Marshal::FreeHGlobal(handle);
		}

		this->mPinnedStrings->Clear();
	}
	void ScriptDomain::HandleException(Exception ^exception)
	{
		Log("Caught unhandled exception:", Environment::NewLine, exception->ToString());
	}
	IntPtr ScriptDomain::PinString(String ^string)
	{
		IntPtr handle = Runtime::InteropServices::Marshal::StringToHGlobalAnsi(string);

		this->mPinnedStrings->Add(handle);

		return handle;
	}
	bool ScriptDomain::IsKeyPressed(Windows::Forms::Keys key)
	{
		return this->mKeyboardState[static_cast<int>(key)];
	}
}