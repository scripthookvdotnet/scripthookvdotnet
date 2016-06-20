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
	using namespace System::Threading::Tasks;
	using namespace System::Reflection;
	using namespace GTA::Native;

	ConsoleCommand::ConsoleCommand() : _help("No help text available"), _consoleArgs(gcnew List<ConsoleArg^>())
	{
		
	}

	ConsoleCommand::ConsoleCommand(System::String ^help) : _help(help), _consoleArgs(gcnew List<ConsoleArg^>())
	{

	}

	System::String^ ConsoleCommand::BuildFormattedHelp()
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
		Interval = 0;
		Tick += gcnew System::EventHandler(this, &GTA::ConsoleScript::OnTick);
		KeyDown += gcnew System::Windows::Forms::KeyEventHandler(this, &GTA::ConsoleScript::OnKeyDown);
		KeyUp += gcnew System::Windows::Forms::KeyEventHandler(this, &GTA::ConsoleScript::OnKeyUp);

		_outputQueue = gcnew ConcurrentQueue<array<String^>^>;
		_commands = gcnew Dictionary<String^, List<Tuple<ConsoleCommand^, MethodInfo^>^>^>();

		_input = "";
		_isOpen = false;
		_page = 0;
		_cursorPos = 0;
		_lines = gcnew LinkedList<String^>();

		Info("--- Console ready to go ---");

		RegisterCommands(DefaultConsoleCommands::typeid, true);
	}

	bool ConsoleScript::IsOpen()
	{
		return _isOpen;
	}

	void ConsoleScript::Info(System::String ^ msg)
	{
		AddLines("[~b~INFO~w~] ", msg->Split('\n'));
	}

	void ConsoleScript::Error(System::String ^ msg)
	{
		AddLines("[~r~ERROR~w~]", msg->Split('\n'));
	}

	void ConsoleScript::Warn(System::String ^ msg)
	{
		AddLines("[~o~WARN~w~] ", msg->Split('\n'));
	}

	void ConsoleScript::RegisterCommands(System::Type ^ type)
	{
		RegisterCommands(type, false);
	}

	void ConsoleScript::RegisterCommands(System::Type ^ type, bool defaultCommands)
	{
		for each(auto method in type->GetMethods(BindingFlags::Static | BindingFlags::Public))
		{
			for each(auto attribute in method->GetCustomAttributes(ConsoleCommand::typeid, true))
			{
				RegisterCommand(static_cast<ConsoleCommand^>(attribute), method, defaultCommands);
			}
		}
	}

	void ConsoleScript::RegisterCommand(ConsoleCommand ^ command, System::Reflection::MethodInfo ^ methodInfo)
	{
		RegisterCommand(command, methodInfo, false);
	}

	void ConsoleScript::RegisterCommand(ConsoleCommand ^ command, System::Reflection::MethodInfo ^ methodInfo, bool defaultCommand)
	{
		File::WriteAllText("console.log", (command == nullptr) + ":" + (methodInfo == nullptr));
		command->Name = defaultCommand ? methodInfo->Name : methodInfo->DeclaringType->FullName + "." + methodInfo->Name; //TODO FIX
		command->Namespace = defaultCommand ? "Default Commands" : methodInfo->DeclaringType->FullName;
		for each(auto args in methodInfo->GetParameters())
		{
			command->ConsoleArgs->Add(gcnew ConsoleArg(args->ParameterType->Name, args->Name));
		}
		if (!_commands->ContainsKey(command->Namespace))
			_commands[command->Namespace] = gcnew List<Tuple<ConsoleCommand^, MethodInfo^>^>();

		_commands[command->Namespace]->Add(gcnew Tuple<ConsoleCommand^, MethodInfo^>(command, methodInfo));
		Info("Registered Command: " + command->Name);
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
			SetControlsEnabled(false);
			return;
		}

		if (!_isOpen)
			return;

		SetControlsEnabled(false);

		DrawRect(0, 0, WIDTH, HEIGHT / 3, BackgroundColor);
		DrawRect(0, HEIGHT / 3, WIDTH, InputHeight, AltBackgroundColor);

		DrawText(0, HEIGHT / 3, "$>", DefaultScale, DefaultFont, PrefixColor);
		DrawText(25, HEIGHT / 3, _input, DefaultScale, DefaultFont, _compilerTask == nullptr ? InputColor : InputColorBusy);

		if (now.Millisecond < 500)
		{
			float length = GetTextLength(_input->Substring(0, _input->Length - _cursorPos), DefaultScale, DefaultFont);
			DrawText(25 + (length * WIDTH) - 4, HEIGHT / 3, "~w~~h~|~w~", DefaultScale, DefaultFont, InputColor);
		}

		//We can't get the n-th, so let's do it with a counter
		int i = 0;
		for each (String ^line in _lines)
		{
			if (i > 15)
				break;
			DrawText(2, (float)((15 - i) * 14), line, DefaultScale, DefaultFont, OutputColor);
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
		case Keys::Enter:
			ExecuteInput();
			break;
		default:
			AddToInput(GetCharsFromKeys(e->KeyCode, e->Shift, e->Alt));
			break;
		}
	}

	void ConsoleScript::OnKeyUp(Object ^sender, KeyEventArgs ^e)
	{
		if (!_isOpen)
			return;
	}

	void ConsoleScript::SetControlsEnabled(bool enabled)
	{
		for (int i = 0; i < 338; i++)
		{
			Function::Call(Hash::DISABLE_CONTROL_ACTION, 0, i, enabled);
		}
	}
	void ConsoleScript::AddLines(System::String ^prefix, array<System::String^> ^msgs)
	{
		for (int i = 0; i < msgs->Length; i++)
		{
			msgs[i] = String::Format("~c~[{0}] ~w~{1} {2}", DateTime::Now.ToString("HH:mm:ss"), prefix, msgs[i]); //Add proper styling
		}
		_outputQueue->Enqueue(msgs);
	}
	System::String ^ ConsoleScript::GetCharsFromKeys(System::Windows::Forms::Keys keys, bool shift, bool alt)
	{
		System::String ^output = nullptr;
		wchar_t buf[256] = {};
		BYTE keyboardState[256] = {};

		if (shift)
			keyboardState[(int)Keys::ShiftKey] = 0xff;

		if (alt)
		{
			keyboardState[(int)Keys::ControlKey] = 0xff;
			keyboardState[(int)Keys::Menu] = 0xff;
		}

		if (ToUnicode((UInt32)keys, 0, keyboardState, buf, 256, 0) == 1)
			output = gcnew System::String(buf);

		return output;
	}

	void ConsoleScript::AddToInput(System::String ^ input)
	{
		if (input != nullptr)
			_input = _input->Insert(_input->Length - _cursorPos, input);
	}

	void ConsoleScript::RemoveCharRight()
	{
		if (_input->Length > 0)
			_input = _input->Remove(_input->Length - _cursorPos, 1);
		if (_cursorPos > 0)
			_cursorPos--;
	}

	void ConsoleScript::RemoveCharLeft()
	{
		if (_input->Length > 0)
			_input = _input->Remove(_input->Length - _cursorPos - 1, 1);
	}

	void ConsoleScript::ClearInput()
	{
		_input = "";
		_cursorPos = 0;
	}

	Assembly^ ConsoleScript::CompileInput()
	{
		String ^inputStr = String::Format(CompileTemplate, _input);

		_compiler = gcnew Microsoft::CSharp::CSharpCodeProvider();
		_compilerOptions = gcnew CodeDom::Compiler::CompilerParameters();
		_compilerOptions->GenerateInMemory = true;
		_compilerOptions->IncludeDebugInformation = true;
		_compilerOptions->ReferencedAssemblies->Add("System.dll");
		_compilerOptions->ReferencedAssemblies->Add(Script::typeid->Assembly->Location);

		CodeDom::Compiler::CompilerResults ^compilerResult = _compiler->CompileAssemblyFromSource(_compilerOptions, inputStr);

		if (!compilerResult->Errors->HasErrors)
		{
			return compilerResult->CompiledAssembly;
		}
		else
		{
			Error("Couldn't compile input-string");

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
		if (_input == "")
			return;
		if (_compilerTask != nullptr)
		{
			Error("Can't compile input - Compiler is busy");
			return;
		}
		_compilerTask = Task::Factory->StartNew(gcnew Func<Assembly^>(this, &ConsoleScript::CompileInput));
	}
	void ConsoleScript::MoveCursorRight()
	{
		if (_cursorPos > 0)
			_cursorPos--;
	}
	void ConsoleScript::MoveCursorLeft()
	{
		if (_cursorPos < _input->Length)
			_cursorPos++;
	}

	void ConsoleScript::DrawRect(float x, float y, int width, int height, Color color)
	{
		const float w = static_cast<float>(width) / WIDTH;
		const float h = static_cast<float>(height) / HEIGHT;
		float xNew = ((static_cast<float>(x)) / WIDTH) + w * 0.5f;
		float yNew = ((static_cast<float>(y)) / HEIGHT) + h * 0.5f;

		Native::Function::Call(Native::Hash::DRAW_RECT, xNew, yNew, w, h, color.R, color.G, color.B, color.A);
	}
	void ConsoleScript::DrawText(float x, float y, System::String ^text, float scale, int font, Color color)
	{
		float xNew = (static_cast<float>(x) / WIDTH);
		float yNew = (static_cast<float>(y) / HEIGHT);


		Native::Function::Call(Native::Hash::SET_TEXT_FONT, font);
		Native::Function::Call(Native::Hash::SET_TEXT_SCALE, scale, scale);
		Native::Function::Call(Native::Hash::SET_TEXT_COLOUR, color.R, color.G, color.B, color.A);
		Native::Function::Call(Native::Hash::_SET_TEXT_ENTRY, "CELL_EMAIL_BCON");

		const int maxStringLength = 99;

		for (int i = 0; i < text->Length; i += maxStringLength)
		{
			Native::Function::Call(Native::Hash::ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text->Substring(i, System::Math::Min(maxStringLength, text->Length - i)));
		}

		Native::Function::Call(Native::Hash::_DRAW_TEXT, xNew, yNew);
	}
	float ConsoleScript::GetTextLength(String ^text, float scale, int font)
	{
		Function::Call(Hash::_SET_TEXT_ENTRY_FOR_WIDTH, "CELL_EMAIL_BCON");

		const int maxStringLength = 99;

		for (int i = 0; i < text->Length; i += maxStringLength)
		{
			Function::Call(Hash::ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text->Substring(i, System::Math::Min(maxStringLength, text->Length - i)));
		}

		Function::Call(Hash::SET_TEXT_FONT, font);
		Function::Call(Hash::SET_TEXT_SCALE, scale, scale);

		return Function::Call<float>(Hash::_GET_TEXT_SCREEN_WIDTH, 1);
	}

	ConsoleArg::ConsoleArg(System::String ^ type, System::String ^ name) : _type(type), _name(name)
	{
		
	}

	void DefaultConsoleCommands::Help()
	{
		Console::Script->PrintHelpString();
	}
	void DefaultConsoleCommands::Help(String ^ command) //TODO Add all commands
	{
		
	}
	void DefaultConsoleCommands::Load(String ^ script)
	{
		
	}
	void DefaultConsoleCommands::Unload(String ^ script)
	{
		
	}
	void DefaultConsoleCommands::Reload(String ^ script)
	{
		
	}
	void DefaultConsoleCommands::List()
	{
		
	}
	void DefaultConsoleCommands::Clear()
	{
		Console::Script->Clear();
	}

	void Console::Info(... array<String^> ^messages)
	{
		for each (String^ message in messages)
		{
			Console::Script->Info(message);
		}
	}
	void Console::Error(... array<String^> ^messages)
	{
		for each (String^ message in messages)
		{
			Console::Script->Error(message);
		}
	}
	void Console::Warn(... array<String^> ^messages)
	{
		for each (String^ message in messages)
		{
			Console::Script->Warn(message);
		}
	}
}