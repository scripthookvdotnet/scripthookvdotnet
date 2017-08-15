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

#include "Script.hpp"

namespace GTA 
{
	//Used to describe console args (Needed for creating a Help-func)
	private ref class ConsoleArg
	{
	public:
		ConsoleArg(System::String ^type, System::String ^name);

	internal:
		property System::String^ Type
		{
			System::String ^get()
			{
				return _type;
			}
		}
		property System::String^ Name
		{
			System::String ^get()
			{
				return _name;
			}
		}

	private:
		System::String ^_type;
		System::String ^_name;
	};

	public ref class ConsoleCommand : System::Attribute
	{
	public:
		ConsoleCommand();
		ConsoleCommand(System::String ^help);
		System::String^ BuildFormattedHelp();

	internal:
		property System::String^ Help
		{
			System::String ^get()
			{
				return _help;
			}
		}

		property System::String^ Name
		{
			System::String ^get()
			{
				return _name;
			}
			void set(System::String ^name)
			{
				_name = name;
			}
		}

		property System::String^ Namespace
		{
			System::String ^get()
			{
				return _namespace;
			}
			void set(System::String ^namespaceS)
			{
				_namespace = namespaceS;
			}
		}

		property System::Collections::Generic::List<ConsoleArg^>^ ConsoleArgs
		{
			System::Collections::Generic::List<ConsoleArg^> ^get()
			{
				return _consoleArgs;
			}
			void set(System::Collections::Generic::List<ConsoleArg^> ^consoleArgs)
			{
				_consoleArgs = consoleArgs;
			}
		}
	private:
		System::String ^_help;
		System::String ^_name;
		System::String ^_namespace;
		System::Collections::Generic::List<ConsoleArg^> ^_consoleArgs;
	};

	private ref class ConsoleScript : Script
	{
	internal:
		ConsoleScript();

		property bool IsOpen
		{
			inline bool get() { return _isOpen; }
		}

		void RegisterCommand(ConsoleCommand ^command, System::Reflection::MethodInfo ^methodInfo);
		void RegisterCommand(ConsoleCommand ^command, System::Reflection::MethodInfo ^methodInfo, bool defaultCommand);
		void RegisterCommands(System::Type ^type);
		void RegisterCommands(System::Type ^type, bool defaultCommands);
		void UnregisterCommands(System::Type ^type);

		void Info(System::String ^msg, ...array<System::Object ^> ^args);
		void Error(System::String ^msg, ...array<System::Object ^> ^args);
		void Warn(System::String ^msg, ...array<System::Object ^> ^args);
		void Debug(System::String ^msg, ...array<System::Object ^> ^args);
		void PrintHelpString();

		void Clear();

		void OnTick(Object ^sender, System::EventArgs ^e);
		void OnKeyDown(System::Object ^sender, System::Windows::Forms::KeyEventArgs ^e);

	private:
		void AddLines(System::String ^prefix, array<System::String^> ^msgs);
		void AddLines(System::String ^prefix, array<System::String^> ^msgs, System::String ^textColor);

		void AddToInput(System::String ^input);
		void AddClipboardContent();
		void ClearInput();
		System::Reflection::Assembly ^CompileInput();
		void ExecuteInput();

		void PasteClipboard();

		void MoveCursorLeft();
		void MoveCursorRight();
		void RemoveCharLeft();
		void RemoveCharRight();
		void PageUp();
		void PageDown();
		void GoUpCommandList();
		void GoDownCommandList();

		void DrawRect(float x, float y, int width, int height, System::Drawing::Color color);
		void DrawText(float x, float y, System::String ^text, float scale, int font, System::Drawing::Color color);

		float GetTextLength(System::String ^text, float scale, int font); //TODO Maybe implement somewhere else?

		void DoUpdateCheck();

		bool _isOpen;
		int _page;
		int _cursorPos;
		int _commandPos;
		System::String ^_input;
		System::DateTime _lastClosed;
		System::Collections::Generic::LinkedList<System::String^> ^_lines;
		System::Collections::Generic::List<System::String^> ^_commandHistory;

		System::Threading::Tasks::Task<System::Reflection::Assembly^> ^_compilerTask;
		System::Collections::Concurrent::ConcurrentQueue<array<System::String^>^> ^_outputQueue;
		System::Collections::Generic::Dictionary<System::String^, System::Collections::Generic::List<System::Tuple<ConsoleCommand^, System::Reflection::MethodInfo^>^>^> ^_commands;

		System::CodeDom::Compiler::CodeDomProvider ^_compiler;
		System::CodeDom::Compiler::CompilerParameters ^_compilerOptions;

		System::Windows::Forms::Keys PageUpKey;
		System::Windows::Forms::Keys PageDownKey;
		System::Windows::Forms::Keys ToggleKey;

		static int DefaultFont = 0; //Chalet London :>

		static const System::Drawing::Color InputColor = System::Drawing::Color::White;
		static const System::Drawing::Color InputColorBusy = System::Drawing::Color::DarkGray;
		static const System::Drawing::Color OutputColor = System::Drawing::Color::White;
		static const System::Drawing::Color PrefixColor = System::Drawing::Color::FromArgb(255, 52, 152, 219);
		static const System::Drawing::Color BackgroundColor = System::Drawing::Color::FromArgb(200, System::Drawing::Color::Black);
		static const System::Drawing::Color AltBackgroundColor = System::Drawing::Color::FromArgb(200, 52, 73, 94);

		static const int VersionWidth = 50;
		static const int InputHeight = 20;
		static const float DefaultScale = 0.35f;
		
		static System::String ^UpdateCheckUserAgent = "scripthookvdotnet"; //Oh my dear github-api, why you do this...
		static System::String ^UpdateCheckUrl = "https://api.github.com/repos/crosire/scripthookvdotnet/releases/latest";
		static System::String ^UpdateCheckPattern = "\"tag_name\":\"v(.*?)\"";

		static System::String ^CompileTemplate = "using System; using GTA; using GTA.Native; using Console = GTA.Console;" +
			" public class ConsoleInput : GTA.DefaultConsoleCommands {{ public static Object Execute(){{ {0}; return null; }} }}";

		//TODO Temporary UI Stuff
		static const int WIDTH = 1280;
		static const int HEIGHT = 720;
	};

	public ref class DefaultConsoleCommands
	{
	public:
		[ConsoleCommand("Prints the default Help-Output")]
		static void Help();

		[ConsoleCommand("Prints the Help-Output for the specific command")]
		static void Help(System::String ^command);

		[ConsoleCommand("Loads scripts from a file")]
		static void Load(System::String ^filename);

		[ConsoleCommand("Reloads scripts from a file")]
		static void Reload(System::String ^filename);

		[ConsoleCommand("Reloads all scripts found in the script folder")]
		static void ReloadAllScripts();

		[ConsoleCommand("List all loaded scripts")]
		static void List();

		[ConsoleCommand("Aborts all scripts from the specified file")]
		static void Abort(System::String ^filename);

		[ConsoleCommand("Aborts all scripts found in the script folder")]
		static void AbortAllScripts();

		[ConsoleCommand("Clears the console output")]
		static void Clear();
	};

	public ref struct Console
	{
		//TODO Maybe change the structure, we basically just create delegate methods?
		static void Info(System::String ^msg, ... array<System::Object^> ^args);
		static void Error(System::String ^msg, ... array<System::Object^> ^args);
		static void Warn(System::String ^msg, ... array<System::Object^> ^args);
		static void Debug(System::String ^msg, ... array<System::Object^> ^args);
	};
}
