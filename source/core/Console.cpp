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

#pragma once

#include "Console.hpp"
#include "ScriptDomain.hpp"
#include "Native.hpp"
#include "NativeHashes.hpp"
#include "Settings.hpp"
#include <Windows.h>

namespace GTA
{
	using namespace System;
	using namespace System::Text;
	using namespace System::Windows::Forms;
	using namespace System::IO;
	using namespace System::Collections::Generic;
	using namespace System::Collections::Concurrent;
	using namespace System::Drawing;
	using namespace System::Net;
	using namespace System::Text::RegularExpressions;
	using namespace System::Threading;
	using namespace System::Threading::Tasks;
	using namespace System::Reflection;

	void SetControlsEnabled(bool enabled)
	{
		for (int i = 0; i < 338; i++)
		{
			if (i >= 1 && i <= 6)
			{
				continue;
			}

			Native::Function::Call(Native::Hash::DISABLE_CONTROL_ACTION, 0, i, enabled);
		}
	}
	String ^GetCharsFromKeys(System::Windows::Forms::Keys keys, bool shift, bool alt)
	{
		wchar_t buf[256] = { };
		BYTE keyboardState[256] = { };

		if (shift)
		{
			keyboardState[(int)Keys::ShiftKey] = 0xff;
		}

		if (alt)
		{
			keyboardState[(int)Keys::ControlKey] = 0xff;
			keyboardState[(int)Keys::Menu] = 0xff;
		}

		if (ToUnicode((UInt32)keys, 0, keyboardState, buf, 256, 0) == 1)
		{
			return gcnew String(buf);
		}

		return nullptr;
	}

	ConsoleCommand::ConsoleCommand() : _help("No help text available"), _consoleArgs(gcnew List<ConsoleArg^>())
	{
		
	}
	ConsoleCommand::ConsoleCommand(String ^help) : _help(help), _consoleArgs(gcnew List<ConsoleArg^>())
	{

	}

	String ^ConsoleCommand::BuildFormattedHelp()
	{
		StringBuilder^ builder = gcnew StringBuilder();
		builder->Append("~h~" + Name + "~w~(");
		
		for each(auto arg in _consoleArgs)
		{
			builder->Append(arg->Type + " " + arg->Name + ",");
		}
		if(_consoleArgs->Count > 0)
			builder->Length--; //Remove last , if we have >0 Args
		builder->Append(")");
		return builder->ToString();
	}

	ConsoleScript::ConsoleScript()
	{
		Tick += gcnew EventHandler(this, &ConsoleScript::OnTick);
		KeyDown += gcnew KeyEventHandler(this, &ConsoleScript::OnKeyDown);

		_outputQueue = gcnew ConcurrentQueue<array<String^>^>;
		_commands = gcnew Dictionary<String^, List<Tuple<ConsoleCommand^, MethodInfo^>^>^>();

		_input = "";
		_isOpen = false;
		_page = 1;
		_cursorPos = 0;
		_commandPos = -1;
		_lines = gcnew LinkedList<String^>();
		_commandHistory = gcnew List<String ^>();

		Version ^version = Assembly::GetExecutingAssembly()->GetName()->Version;
		Info("--- Community Script Hook V .NET {0} ---", version);
		Info("--- Type \"Help()\" to print an overview of available commands ---");

		//Start a update check Task
		Task::Factory->StartNew(gcnew Action(this, &ConsoleScript::DoUpdateCheck));

		RegisterCommands(DefaultConsoleCommands::typeid, true);

		String ^assemblyPath = Assembly::GetExecutingAssembly()->Location;
		String ^assemblyFilename = Path::GetFileNameWithoutExtension(assemblyPath);
		ScriptSettings ^settings = ScriptSettings::Load(Path::ChangeExtension(assemblyPath, ".ini"));

		ToggleKey = settings->GetValue<Keys>("Console", "ToggleKey", Keys::F3);
		PageDownKey = settings->GetValue<Keys>("Console", "PageDown", Keys::PageDown);
		PageUpKey = settings->GetValue<Keys>("Console", "PageUp", Keys::PageUp);
	}

	void ConsoleScript::RegisterCommand(ConsoleCommand ^command, MethodInfo ^methodInfo)
	{
		RegisterCommand(command, methodInfo, false);
	}
	void ConsoleScript::RegisterCommand(ConsoleCommand ^command, MethodInfo ^methodInfo, bool defaultCommand)
	{
		File::WriteAllText("console.log", (command == nullptr) + ":" + (methodInfo == nullptr));

		command->Name = defaultCommand ? methodInfo->Name : methodInfo->DeclaringType->FullName + "." + methodInfo->Name; //TODO FIX
		command->Namespace = defaultCommand ? "Default Commands" : methodInfo->DeclaringType->FullName;

		for each(auto args in methodInfo->GetParameters())
		{
			command->ConsoleArgs->Add(gcnew ConsoleArg(args->ParameterType->Name, args->Name));
		}

		if (!_commands->ContainsKey(command->Namespace))
		{
			_commands[command->Namespace] = gcnew List<Tuple<ConsoleCommand ^, MethodInfo ^> ^>();
		}

		_commands[command->Namespace]->Add(gcnew Tuple<ConsoleCommand ^, MethodInfo ^>(command, methodInfo));
	}
	void ConsoleScript::RegisterCommands(Type ^type)
	{
		RegisterCommands(type, false);
	}
	void ConsoleScript::RegisterCommands(Type ^type, bool defaultCommands)
	{
		for each (auto method in type->GetMethods(BindingFlags::Static | BindingFlags::Public))
		{
			for each (auto attribute in method->GetCustomAttributes(ConsoleCommand::typeid, true))
			{
				RegisterCommand(static_cast<ConsoleCommand ^>(attribute), method, defaultCommands);
			}
		}
	}
	void ConsoleScript::UnregisterCommands(Type ^ type)
	{
		for each (auto method in type->GetMethods(BindingFlags::Static | BindingFlags::Public))
		{
			for each (auto attribute in method->GetCustomAttributes(ConsoleCommand::typeid, true))
			{
				auto command = static_cast<ConsoleCommand ^>(attribute);
				command->Namespace = method->DeclaringType->FullName;

				if (_commands->ContainsKey(command->Namespace))
				{
					List<Tuple<ConsoleCommand ^, MethodInfo ^> ^> ^Namespace = _commands[command->Namespace];

					for (int i = 0; i < Namespace->Count; i++)
					{
						if (Namespace[i]->Item1 == command || Namespace[i]->Item2 == method)
						{
							Namespace->RemoveAt(i--);
						}
					}

					if (Namespace->Count == 0)
					{
						_commands->Remove(command->Namespace);
					}
				}
			}
		}
	}

	void ConsoleScript::Info(String ^msg, ...array<Object ^> ^args)
	{
		if (args->Length > 0)
		{
			msg = String::Format(msg, args);
		}

		AddLines("[~b~INFO~w~] ", msg->Split('\n'));
	}
	void ConsoleScript::Error(String ^msg, ...array<Object ^> ^args)
	{
		if (args->Length > 0)
		{
			msg = String::Format(msg, args);
		}

		AddLines("[~r~ERROR~w~]", msg->Split('\n'), "~r~");
	}
	void ConsoleScript::Warn(String ^msg, ...array<Object ^> ^args)
	{
		if (args->Length > 0)
		{
			msg = String::Format(msg, args);
		}

		AddLines("[~o~WARN~w~] ", msg->Split('\n'));
	}
	void ConsoleScript::Debug(String ^msg, ...array<Object ^> ^args)
	{
		if (args->Length > 0)
		{
			msg = String::Format(msg, args);
		}

		AddLines("[~c~DEBUG~w~] ", msg->Split('\n'), "~c~");
	}
	void ConsoleScript::PrintHelpString()
	{
		StringBuilder ^help = gcnew StringBuilder();
		help->AppendLine("--- Help ---");
		for each(auto namespaceS in _commands->Keys)
		{
			help->AppendLine(String::Format("[{0}]", namespaceS));
			for each(auto command in _commands[namespaceS])
			{
				auto consoleCommand = command->Item1;

				help->AppendLine("    " + consoleCommand->BuildFormattedHelp());
			}
		}
		Info(help->ToString());
	}

	void ConsoleScript::AddLines(String ^prefix, array<String^> ^msgs)
	{
		AddLines(prefix, msgs, "~w~");
	}
	void ConsoleScript::AddLines(String ^prefix, array<String^> ^msgs, String ^textColor)
	{
		for (int i = 0; i < msgs->Length; i++)
		{
			msgs[i] = String::Format("~c~[{0}] ~w~{1} {2}{3}", DateTime::Now.ToString("HH:mm:ss"), prefix, textColor, msgs[i]); //Add proper styling
		}

		_outputQueue->Enqueue(msgs);
	}
	void ConsoleScript::Clear()
	{
		_lines->Clear();
	}

	void ConsoleScript::OnTick(Object ^sender, EventArgs ^e)
	{
		DateTime now = DateTime::UtcNow;

		array<String^> ^outputLines;
		if (_outputQueue->TryDequeue(outputLines))
		{
			for each (String ^outputLine in outputLines)
			{
				_lines->AddFirst(outputLine);
			}
		}

		if (_compilerTask != nullptr)
		{
			if (_compilerTask->IsCompleted)
			{
				Assembly ^compileResult = _compilerTask->Result;
				if (compileResult != nullptr)
				{
					Type ^type = compileResult->GetType("ConsoleInput");
					Object ^result = type->GetMethod("Execute")->Invoke(nullptr, nullptr);
					if (result != nullptr)
						Info(String::Format("[Return Value: {0}]", result));
				}
				ClearInput();
				_compilerTask = nullptr;
			}
		}


		//Hack so the input gets blocked long enogh
		if (_lastClosed > now)
		{
			if (Native::Function::Call<bool>(Native::Hash::_IS_INPUT_DISABLED, 2))
				SetControlsEnabled(false);
			return;
		}

		if (!_isOpen)
			return;
		if (Native::Function::Call<bool>(Native::Hash::_IS_INPUT_DISABLED, 2))
			SetControlsEnabled(false);

		DrawRect(0, 0, WIDTH, HEIGHT / 3, BackgroundColor);
		DrawRect(0, HEIGHT / 3, WIDTH, InputHeight, AltBackgroundColor);
		DrawRect(0, HEIGHT / 3 + InputHeight, 80, InputHeight, AltBackgroundColor);

		DrawText(0, HEIGHT / 3, "$>", DefaultScale, DefaultFont, PrefixColor);
		DrawText(25, HEIGHT / 3, _input, DefaultScale, DefaultFont, _compilerTask == nullptr ? InputColor : InputColorBusy);
		DrawText(5, HEIGHT / 3 + InputHeight, "Page " + _page + "/" +  (_lines->Count == 0 ? 0 : ((_lines->Count + 16 - 1) / 16)), DefaultScale, DefaultFont, InputColor); //TODO Nicer way for page-max

		if (now.Millisecond < 500)
		{
			float length = GetTextLength(_input->Substring(0, _input->Length - _cursorPos), DefaultScale, DefaultFont);
			DrawText(25 + (length * WIDTH) - 4, HEIGHT / 3, "~w~~h~|~w~", DefaultScale, DefaultFont, InputColor);
		}

		//We can't get the n-th, so let's do it with a counter
		//page = 1 --> start: 0 end: 15
		//page = 2 --> start: 16 end: 31
		int start = (_page - 1) * 16; //0
		int end = System::Math::Min(_lines->Count, _page * 16 - 1); //12

		int i = 0;
		for each (String ^line in _lines)
		{
			if (i >= start && i <= end)
			{
				DrawText(2, (float)((15 - (i % 16)) * 14), line, DefaultScale, DefaultFont, OutputColor);
			}
			i++;
		}
	}
	void ConsoleScript::OnKeyDown(Object ^sender, KeyEventArgs ^e)
	{
		if (e->KeyCode == ToggleKey)
		{
			_isOpen = !_isOpen;
			SetControlsEnabled(false);
			if (!_isOpen)
				_lastClosed = DateTime::UtcNow.AddMilliseconds(200); //Hack so the input gets blocked long enogh
			return;
		}

		if (!_isOpen)
			return;

		if (e->KeyCode == PageUpKey)
		{
			PageUp();
			return;
		}
		else if (e->KeyCode == PageDownKey)
		{
			PageDown();
			return;
		}

		switch (e->KeyCode)
		{
		case Keys::Back:
			RemoveCharLeft();
			break;
		case Keys::Delete:
			RemoveCharRight();
			break;
		case Keys::Left:
			MoveCursorLeft();
			break;
		case Keys::Right:
			MoveCursorRight();
			break;
		case Keys::Up:
			GoUpCommandList();
			break;
		case Keys::Down:
			GoDownCommandList();
			break;
		case Keys::V:
			if (e->Control)
				PasteClipboard();
			break;
		case Keys::Enter:
			ExecuteInput();
			break;
		case Keys::Escape:
			_isOpen = false;
			SetControlsEnabled(false);
			_lastClosed = DateTime::UtcNow.AddMilliseconds(200); //Hack so the input gets blocked long enogh
			break;
		default:
			AddToInput(GetCharsFromKeys(e->KeyCode, e->Shift, e->Alt));
			break;
		}
	}

	void ConsoleScript::AddToInput(String ^input)
	{
		if (String::IsNullOrEmpty(input))
		{
			return;
		}

		_input = _input->Insert(_input->Length - _cursorPos, input);
	}
	void ConsoleScript::AddClipboardContent()
	{
		String ^text = Clipboard::GetText();
		text = text->Replace("\n", ""); //TODO Keep this?

		AddToInput(text);
	}
	void ConsoleScript::ClearInput()
	{
		_input = "";
		_cursorPos = 0;
	}
	Assembly ^ConsoleScript::CompileInput()
	{
		String ^inputStr = String::Format(CompileTemplate, _input);

		_compiler = gcnew Microsoft::CSharp::CSharpCodeProvider();
		_compilerOptions = gcnew CodeDom::Compiler::CompilerParameters();
		_compilerOptions->GenerateInMemory = true;
		_compilerOptions->IncludeDebugInformation = true;
		_compilerOptions->ReferencedAssemblies->Add("System.dll");
		_compilerOptions->ReferencedAssemblies->Add(Script::typeid->Assembly->Location);

		for each (Script ^script in ScriptDomain::CurrentDomain->RunningScripts)
		{
			if (!String::IsNullOrEmpty(script->_filename))
			{
				_compilerOptions->ReferencedAssemblies->Add(script->_filename);
			}
		}

		CodeDom::Compiler::CompilerResults ^compilerResult = _compiler->CompileAssemblyFromSource(_compilerOptions, inputStr);

		if (!compilerResult->Errors->HasErrors)
		{
			return compilerResult->CompiledAssembly;
		}
		else
		{
			Error(String::Format("Couldn't compile input-string: {0}", _input));

			StringBuilder ^errors = gcnew StringBuilder();

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

			Error(errors->ToString());
			return nullptr;
		}
	}
	void ConsoleScript::ExecuteInput()
	{
		if (String::IsNullOrEmpty(_input))
		{
			return;
		}

		_commandPos = -1;

		if (_commandHistory->Count == 0 || _commandHistory[_commandHistory->Count -1] != _input)
		{
			_commandHistory->Add(_input);
		}

		if (_compilerTask != nullptr)
		{
			Error("Can't compile input - Compiler is busy");
			return;
		}

		_compilerTask = Task::Factory->StartNew(gcnew Func<Assembly^>(this, &ConsoleScript::CompileInput));
	}

	void ConsoleScript::PasteClipboard()
	{
		Thread ^thread = gcnew Thread(gcnew ThreadStart(this, &ConsoleScript::AddClipboardContent));
		thread->SetApartmentState(ApartmentState::STA);
		thread->Start();
	}

	void ConsoleScript::MoveCursorLeft()
	{
		if (_cursorPos < _input->Length)
			_cursorPos++;
	}
	void ConsoleScript::MoveCursorRight()
	{
		if (_cursorPos > 0)
			_cursorPos--;
	}
	void ConsoleScript::RemoveCharLeft()
	{
		if (_input->Length > 0)
		{
			_input = _input->Remove(_input->Length - _cursorPos - 1, 1);
		}
	}
	void ConsoleScript::RemoveCharRight()
	{
		if (_input->Length > 0 && _cursorPos < _input->Length)
		{
			_input = _input->Remove(_input->Length - _cursorPos, 1);
		}

		if (_cursorPos > 0)
		{
			_cursorPos--;
		}
	}
	void ConsoleScript::PageUp()
	{
		if (_page + 1 <= ((_lines->Count + 16 - 1) / 16))
			_page++;
	}
	void ConsoleScript::PageDown()
	{
		if (_page - 1 >= 1)
			_page--;
	}
	void ConsoleScript::GoUpCommandList()
	{
		if (_commandHistory->Count == 0)
			return;
		if (_commandPos >= _commandHistory->Count - 1)
			return;
		_commandPos++;
		_input = _commandHistory[_commandHistory->Count - _commandPos - 1];
	}
	void ConsoleScript::GoDownCommandList()
	{
		if (_commandHistory->Count == 0)
			return;
		if (_commandPos <= 0)
			return;
		_commandPos--;
		_input = _commandHistory[_commandHistory->Count - _commandPos - 1];
	}

	void ConsoleScript::DrawRect(float x, float y, int width, int height, Color color)
	{
		const float w = static_cast<float>(width) / WIDTH;
		const float h = static_cast<float>(height) / HEIGHT;
		float xNew = ((static_cast<float>(x)) / WIDTH) + w * 0.5f;
		float yNew = ((static_cast<float>(y)) / HEIGHT) + h * 0.5f;

		Native::Function::Call(Native::Hash::DRAW_RECT, xNew, yNew, w, h, color.R, color.G, color.B, color.A);
	}
	void ConsoleScript::DrawText(float x, float y, String ^text, float scale, int font, Color color)
	{
		float xNew = (static_cast<float>(x) / WIDTH);
		float yNew = (static_cast<float>(y) / HEIGHT);

		Native::Function::Call(Native::Hash::SET_TEXT_FONT, font);
		Native::Function::Call(Native::Hash::SET_TEXT_SCALE, scale, scale);
		Native::Function::Call(Native::Hash::SET_TEXT_COLOUR, color.R, color.G, color.B, color.A);
		Native::Function::Call(Native::Hash::BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");

		const int maxStringLength = 99;

		for (int i = 0; i < text->Length; i += maxStringLength)
		{
			Native::Function::Call(Native::Hash::ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text->Substring(i, System::Math::Min(maxStringLength, text->Length - i)));
		}

		Native::Function::Call(Native::Hash::END_TEXT_COMMAND_DISPLAY_TEXT, xNew, yNew);
	}
	float ConsoleScript::GetTextLength(String ^text, float scale, int font)
	{
		Native::Function::Call(Native::Hash::_BEGIN_TEXT_COMMAND_WIDTH, "CELL_EMAIL_BCON");

		const int maxStringLength = 99;

		for (int i = 0; i < text->Length; i += maxStringLength)
		{
			Native::Function::Call(Native::Hash::ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text->Substring(i, System::Math::Min(maxStringLength, text->Length - i)));
		}

		Native::Function::Call(Native::Hash::SET_TEXT_FONT, font);
		Native::Function::Call(Native::Hash::SET_TEXT_SCALE, scale, scale);

		return Native::Function::Call<float>(Native::Hash::_END_TEXT_COMMAND_GET_WIDTH, 1);
	}

	void ConsoleScript::DoUpdateCheck()
	{
		Version ^curVersion = Assembly::GetExecutingAssembly()->GetName()->Version;
		WebClient ^webclient = gcnew WebClient();
		webclient->Headers->Add("user-agent", UpdateCheckUserAgent);

		try
		{
			String ^response = webclient->DownloadString(UpdateCheckUrl);
			if (!String::IsNullOrEmpty(response))
			{
				Regex ^regex = gcnew Regex(UpdateCheckPattern);
				Match ^match = regex->Match(response);

				if (match->Success)
				{
					Version ^fetchedVersion = gcnew Version(match->Groups[1]->Value);
					if (fetchedVersion > curVersion)
						Info("There is a new SHV.NET Version available: ~y~v{0}", fetchedVersion);
					else
						Info("You're running on the latest SHV.NET Version");
					return;
				}
			}
		}
		catch (Exception ^e)
		{
			Warn("SHV.NET Update-Check failed: {0}", e->Message);
			return;
		}
		Warn("SHV.NET Update-Check failed");

		delete webclient;
	}

	ConsoleArg::ConsoleArg(String ^ type, String ^ name) : _type(type), _name(name)
	{
		
	}

	void DefaultConsoleCommands::Help()
	{
		ScriptDomain::CurrentDomain->Console->PrintHelpString();
	}
	void DefaultConsoleCommands::Help(String ^command) //TODO Add all commands
	{
		
	}
	void DefaultConsoleCommands::Load(String ^filename)
	{
		String ^basedirectory = ScriptDomain::CurrentDomain->AppDomain->BaseDirectory;

		if (!IO::File::Exists(IO::Path::Combine(basedirectory, filename)))
		{
			array<String ^> ^files = IO::Directory::GetFiles(basedirectory, filename, IO::SearchOption::AllDirectories);

			if (files->Length != 1)
			{
				Console::Error("The file '" + filename + "' was not found in '" + basedirectory + "'");
				return;
			}

			Console::Warn("The file '" + filename + "' was not found in '" + basedirectory + "', loading from '" + IO::Path::GetDirectoryName(files[0]->Substring(basedirectory->Length + 1)) + "' instead");

			filename = files[0]->Substring(basedirectory->Length + 1);
		}
		else
		{
			filename = IO::Path::Combine(basedirectory, filename);
		}

		filename = IO::Path::GetFullPath(filename);

		String ^extension = IO::Path::GetExtension(filename)->ToLower();

		if (extension != ".cs" && extension != ".vb" && extension != ".dll")
		{
			Console::Error("The file '" + filename + "' was not recognized as a script file");
			return;
		}

		for each (auto script in ScriptDomain::CurrentDomain->RunningScripts)
		{
			if (filename->Equals(script->Filename, StringComparison::OrdinalIgnoreCase) && script->_running)
			{
				Console::Error("The script is already running");
				return;
			}
		}

		ScriptDomain::CurrentDomain->StartScript(filename);
	}
	void DefaultConsoleCommands::Reload(String ^filename)
	{
		Abort(filename);
		Load(filename);
	}
	void DefaultConsoleCommands::ReloadAllScripts()
	{
		AbortAllScripts();
		ScriptDomain::CurrentDomain->StartAllScripts();
	}
	void DefaultConsoleCommands::List()
	{
		auto scripts = gcnew Collections::Generic::List<Script ^>(ScriptDomain::CurrentDomain->RunningScripts);
		scripts->Remove(ScriptDomain::CurrentDomain->Console);

		if (scripts->Count == 0)
		{
			Console::Info("There are no scripts loaded");
			return;
		}

		String ^basedirectory = ScriptDomain::CurrentDomain->AppDomain->BaseDirectory;

		Console::Info("---");
		for each (auto script in scripts)
		{
			String ^filename = script->Filename;

			if (filename->StartsWith(basedirectory, StringComparison::OrdinalIgnoreCase))
			{
				filename = filename->Substring(basedirectory->Length + 1);
			}

			Console::Info("   " + filename + ": " + script->Name + (script->_running ? " ~g~[running]" : " ~r~[aborted]"));
		}
		Console::Info("---");
	}
	void DefaultConsoleCommands::Abort(String ^filename)
	{
		String ^basedirectory = ScriptDomain::CurrentDomain->AppDomain->BaseDirectory;

		filename = IO::Path::Combine(basedirectory, filename);

		if (!IO::File::Exists(filename))
		{
			Console::Error("The file '" + filename + "' was not found");
			return;
		}

		ScriptDomain::CurrentDomain->AbortScript(filename);
	}
	void DefaultConsoleCommands::AbortAllScripts()
	{
		ScriptDomain::CurrentDomain->AbortAllScriptsExceptConsole();
	}
	void DefaultConsoleCommands::Clear()
	{
		ScriptDomain::CurrentDomain->Console->Clear();
	}

	void Console::Info(String ^msg, ...array<Object ^> ^args)
	{
		ScriptDomain::CurrentDomain->Console->Info(msg, args);
	}
	void Console::Error(String ^msg, ...array<Object ^> ^args)
	{
		ScriptDomain::CurrentDomain->Console->Error(msg, args);
	}
	void Console::Warn(String ^msg, ...array<Object ^> ^args)
	{
		ScriptDomain::CurrentDomain->Console->Warn(msg, args);
	}
	void Console::Debug(String ^msg, ...array<Object ^> ^args)
	{
		ScriptDomain::CurrentDomain->Console->Debug(msg, args);
	}
}
