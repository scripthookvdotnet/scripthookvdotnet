// 
// Copyright (C) 2015 crosire
//
// This software is  provided 'as-is', without any express  or implied  warranty. In no event will the
// authors be held liable for any damages arising from the use of this software.
// Permission  is granted  to anyone  to use  this software  for  any  purpose,  including  commercial
// applications, and to alter it and redistribute it freely, subject to the following restrictions:
//
//   1. The origin of this software must not be misrepresented; you must not claim that you  wrote the
//      original  software. If you use this  software  in a product, an  acknowledgment in the product
//      documentation would be appreciated but is not required.
//   2. Altered source versions must  be plainly  marked as such, and  must not be  misrepresented  as
//      being the original software.
//   3. This notice may not be removed or altered from any source distribution.
// 

using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SHVDN
{
	//Used to describe console args (Needed for creating a Help-func)
	internal class ConsoleArg
	{
		public ConsoleArg(string type, string name)
		{
			Type = type;
			Name = type;
		}

		internal string Type { get; }
		internal string Name { get; }
	}

	public class ConsoleCommand : Attribute
	{
		public ConsoleCommand(string help)
		{
			Help = help;
			ConsoleArgs = new List<ConsoleArg>();
		}
		public ConsoleCommand() : this("No help text available")
		{
		}

		internal string Help { get; set; }
		internal string Name { get; set; }
		internal string Namespace { get; set; }
		internal List<ConsoleArg> ConsoleArgs { get; set; }

		internal string BuildFormattedHelp()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("~h~" + Name + "~w~(");

			foreach (var arg in ConsoleArgs)
			{
				builder.Append(arg.Type + " " + arg.Name + ",");
			}

			if (ConsoleArgs.Count > 0)
				builder.Length--; //Remove last , if we have >0 Args

			builder.Append(")");
			return builder.ToString();
		}
	};

	public class Console
	{
		internal static Console Instance { get; set; } = new Console();

		int cursorPos = 0;
		int commandPos = -1;
		int currentPage = 1;
		bool isOpen = false;
		string input = string.Empty;

		List<string> lineHistory = new List<String>();
		List<string> commandHistory = new List<String>();

		DateTime _lastClosed;
		Regex _getEachWordRegex = new Regex(@"[^\W_]+", RegexOptions.Compiled);

		Task<Assembly> _compilerTask;
		ConcurrentQueue<string[]> _outputQueue = new ConcurrentQueue<string[]>();
		Dictionary<string, List<Tuple<ConsoleCommand, MethodInfo>>> _commands = new Dictionary<String, List<Tuple<ConsoleCommand, MethodInfo>>>();

		Keys PageUpKey = Keys.F3;
		Keys PageDownKey = Keys.PageDown;
		Keys ToggleKey = Keys.PageUp;

		static readonly Color InputColor = Color.White;
		static readonly Color InputColorBusy = Color.DarkGray;
		static readonly Color OutputColor = Color.White;
		static readonly Color PrefixColor = Color.FromArgb(255, 52, 152, 219);
		static readonly Color BackgroundColor = Color.FromArgb(200, Color.Black);
		static readonly Color AltBackgroundColor = Color.FromArgb(200, 52, 73, 94);

		//TODO Temporary UI Stuff
		const int WIDTH = 1280;
		const int HEIGHT = 720;

		internal Console()
		{
            AddLines("[~b~INFO~w~] ", new string[1] { "--- Community Script Hook V .NET " + Assembly.GetExecutingAssembly().GetName().Version + " ---" });
			AddLines("[~b~INFO~w~] ", new string[1] { "--- Type \"Help()\" to print an overview of available commands ---" });

			RegisterCommands(typeof(DefaultConsoleCommands), true);
		}

		internal bool IsOpen
		{
			get { return isOpen; }
			set {
				isOpen = value;
				SetControlsEnabled(false);
				if (!value)
					_lastClosed = DateTime.UtcNow.AddMilliseconds(200); // Hack so the input gets blocked long enough
			}
		}

		[DllImport("user32.dll")]
		static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState, [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer, int bufferSize, uint flags);

		internal void RegisterCommand(ConsoleCommand command, System.Reflection.MethodInfo methodInfo, bool defaultCommand = false)
		{
			File.WriteAllText("console.log", (command == null) + ":" + (methodInfo == null));

			command.Name = defaultCommand ? methodInfo.Name : methodInfo.DeclaringType.FullName + "." + methodInfo.Name; //TODO FIX
			command.Namespace = defaultCommand ? "Default Commands" : methodInfo.DeclaringType.FullName;

			foreach (var args in methodInfo.GetParameters())
				command.ConsoleArgs.Add(new ConsoleArg(args.ParameterType.Name, args.Name));

			if (!_commands.ContainsKey(command.Namespace))
				_commands[command.Namespace] = new List<Tuple<ConsoleCommand, MethodInfo>>();
			_commands[command.Namespace].Add(new Tuple<ConsoleCommand, MethodInfo>(command, methodInfo));
		}
		internal void RegisterCommands(Type type, bool defaultCommands = false)
		{
			foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
				foreach (var attribute in method.GetCustomAttributes<ConsoleCommand>(true))
					RegisterCommand(attribute, method, defaultCommands);
		}
		internal void UnregisterCommands(Type type)
		{
			foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
			{
				foreach (var attribute in method.GetCustomAttributes<ConsoleCommand>(true))
				{
					var command = attribute;
					command.Namespace = method.DeclaringType.FullName;

					if (_commands.ContainsKey(command.Namespace))
					{
						List<Tuple<ConsoleCommand, MethodInfo>> Namespace = _commands[command.Namespace];

						for (int i = 0; i < Namespace.Count; i++)
						{
							if (Namespace[i].Item1 == command || Namespace[i].Item2 == method)
							{
								Namespace.RemoveAt(i--);
							}
						}

						if (Namespace.Count == 0)
						{
							_commands.Remove(command.Namespace);
						}
					}
				}
			}
		}

		public static void PrintInfo(string msg, params object[] args)
		{
			if (args.Length > 0)
				msg = String.Format(msg, args);
			Instance.AddLines("[~b~INFO~w~] ", msg.Split('\n'));
		}
		public static void PrintError(string msg, params object[] args)
		{
			if (args.Length > 0)
				msg = String.Format(msg, args);
			Instance.AddLines("[~r~ERROR~w~] ", msg.Split('\n'));
		}
		public static void PrintWarning(string msg, params object[] args)
		{
			if (args.Length > 0)
				msg = String.Format(msg, args);
			Instance.AddLines("[~o~WARN~w~] ", msg.Split('\n'));
		}
		public static void PrintDebug(string msg, params object[] args)
		{
			if (args.Length > 0)
				msg = String.Format(msg, args);
			Instance.AddLines("[~c~DEBUG~w~] ", msg.Split('\n'));
		}
		internal void PrintHelpString()
		{
			StringBuilder help = new StringBuilder();
			help.AppendLine("--- Help ---");
			foreach (var namespaceS in _commands.Keys)
			{
				help.AppendLine($"[{namespaceS}]");
				foreach (var command in _commands[namespaceS])
				{
					var consoleCommand = command.Item1;

					help.AppendLine("    " + consoleCommand.BuildFormattedHelp());
				}
			}
			PrintInfo(help.ToString());
		}

        /// <summary>
        /// Add text lines to the console. This call is thread-safe.
        /// </summary>
        /// <param name="prefix">The prefix for each line.</param>
        /// <param name="messages">The lines to add to the console.</param>
        void AddLines(string prefix, string[] messages)
		{
			AddLines(prefix, messages, "~w~");
		}
		/// <summary>
		/// Add colored text lines to the console. This call is thread-safe.
		/// </summary>
		/// <param name="prefix">The prefix for each line.</param>
		/// <param name="messages">The lines to add to the console.</param>
		/// <param name="color">The color of those lines.</param>
		void AddLines(string prefix, string[] messages, string color)
		{
			for (int i = 0; i < messages.Length; i++) // Add proper styling
				messages[i] = $"~c~[{DateTime.Now.ToString("HH:mm:ss")}] ~w~{prefix} {color}{messages[i]}";

			_outputQueue.Enqueue(messages);
		}
		/// <summary>
		/// Add text to the console input line.
		/// </summary>
		/// <param name="text">The text to add.</param>
		void AddToInput(string text)
		{
			if (string.IsNullOrEmpty(text))
				return;

			input = input.Insert(cursorPos, text);
			cursorPos++;
		}
		/// <summary>
		/// Paste clipboard content into the console input line.
		/// </summary>
		void AddClipboardContent()
		{
			string text = Clipboard.GetText();
			text = text.Replace("\n", string.Empty); // TODO Keep this?

			AddToInput(text);
		}

		/// <summary>
		/// Clear the console input line.
		/// </summary>
		void ClearInput()
		{
			input = string.Empty;
			cursorPos = 0;
		}
		/// <summary>
		/// Clear the console history lines.
		/// </summary>
		internal void Clear()
		{
			lineHistory.Clear();
		}

		/// <summary>
		/// Main execution logic of the console.
		/// </summary>
		internal void DoTick()
		{
			DateTime now = DateTime.UtcNow;

			// Execute compiled input line script
			if (_compilerTask != null && _compilerTask.IsCompleted)
			{
				if (_compilerTask.Result != null)
				{
					var result = _compilerTask.Result.GetType("ConsoleInput").GetMethod("Execute").Invoke(null, null);
					if (result != null)
						PrintInfo($"[Return Value: {result}]");
				}

				ClearInput();

				// Reset compiler task
				_compilerTask = null;
			}

			// Add lines from concurrent queue to history
			if (_outputQueue.TryDequeue(out string[] lines))
				foreach (string line in lines)
					lineHistory.Add(line);

			// Hack so the input gets blocked long enough
			if (_lastClosed > now)
			{
				if (IsInputDisabled())
					SetControlsEnabled(false);
				return;
			}

			// Nothing more to do here when the console is not open
			if (!IsOpen)
				return;

			if (IsInputDisabled())
				SetControlsEnabled(false);

			const int inputH = 20;
			const int consoleW = WIDTH;
			const int consoleH = HEIGHT / 3;

			// Draw background
			DrawRect(0, 0, consoleW, consoleH, BackgroundColor);
			// Draw input field
			DrawRect(0, consoleH, consoleW, inputH, AltBackgroundColor);
			DrawRect(0, consoleH + inputH, 80, inputH, AltBackgroundColor);
			// Draw input prefix
			DrawText(0, consoleH, "$>", PrefixColor);
			// Draw input text
			DrawText(25, consoleH, input, _compilerTask == null ? InputColor : InputColorBusy);
			// Draw page information
			DrawText(5, consoleH + inputH, "Page " + currentPage + "/" + (lineHistory.Count == 0 ? 0 : ((lineHistory.Count + 16 - 1) / 16)), InputColor); //TODO Nicer way for page-max

			// Draw blinking cursor
			if (now.Millisecond < 500)
			{
				float length = GetTextLength(input.Substring(0, cursorPos));
				DrawText(25 + (length * consoleW) - 4, consoleH, "~w~~h~|~w~", InputColor);
			}

			// Draw console history text
			for (int i = (currentPage - 1) * 16; i <= System.Math.Min(lineHistory.Count, currentPage * 16 - 1); ++i)
			{
				DrawText(2, (float)((15 - (i % 16)) * 14), lineHistory[i], OutputColor);
			}
		}
		/// <summary>
		/// Keyboard handling logic of the console.
		/// </summary>
		/// <param name="e"></param>
		internal void DoKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == ToggleKey)
			{
				IsOpen = !IsOpen;
				return;
			}

			// Nothing more to do here when the console is not open
			if (!IsOpen)
				return;

			if (e.KeyCode == PageUpKey)
			{
				PageUp();
				return;
			}
			if (e.KeyCode == PageDownKey)
			{
				PageDown();
				return;
			}

			switch (e.KeyCode)
			{
				case Keys.Back:
					RemoveCharLeft();
					break;
				case Keys.Delete:
					RemoveCharRight();
					break;
				case Keys.Left:
					if (e.Control)
						BackwardWord();
					else
						MoveCursorLeft();
					break;
				case Keys.Right:
					if (e.Control)
						ForwardWord();
					else
						MoveCursorRight();
					break;
				case Keys.Up:
					GoUpCommandList();
					break;
				case Keys.Down:
					GoDownCommandList();
					break;
				case Keys.Enter:
					ExecuteInput();
					break;
				case Keys.Escape:
					IsOpen = false;
					break;
				case Keys.B:
					if (e.Control)
						MoveCursorLeft();
					else if (e.Alt)
						BackwardWord();
					else
						goto default;
					break;
				case Keys.F:
					if (e.Control)
						MoveCursorRight();
					else if (e.Alt)
						ForwardWord();
					else
						goto default;
					break;
				case Keys.A:
					if (e.Control)
						MoveCursorToBegOfLine();
					else
						goto default;
					break;
				case Keys.E:
					if (e.Control)
						MoveCursorToEndOfLine();
					else
						goto default;
					break;
				case Keys.P:
					if (e.Control)
						GoUpCommandList();
					else
						goto default;
					break;
				case Keys.N:
					if (e.Control)
						GoDownCommandList();
					else
						goto default;
					break;
				case Keys.L:
					if (e.Control)
						Clear();
					else
						goto default;
					break;
				case Keys.V:
					if (e.Control)
						AddClipboardContent();
					else
						goto default;
					break;
				default:
					// Translate key event to character for text input
					var buf = new StringBuilder(256);
					var keyboardState = new byte[256];
					keyboardState[(int)Keys.Menu] = e.Alt ? (byte)0xff : (byte)0;
					keyboardState[(int)Keys.ShiftKey] = e.Shift ? (byte)0xff : (byte)0;
					keyboardState[(int)Keys.ControlKey] = e.Control ? (byte)0xff : (byte)0;
					ToUnicode((uint)e.KeyCode, 0, keyboardState, buf, 256, 0);

					AddToInput(buf.ToString());
					break;
			}
		}

		void PageUp()
		{
			if (currentPage + 1 <= ((lineHistory.Count + 16 - 1) / 16))
				currentPage++;
		}
		void PageDown()
		{
			if (currentPage - 1 >= 1)
				currentPage--;
		}
		void GoUpCommandList()
		{
			if (commandHistory.Count == 0 || commandPos >= commandHistory.Count - 1)
				return;

			commandPos++;
			input = commandHistory[commandHistory.Count - commandPos - 1];
		}
		void GoDownCommandList()
		{
			if (commandHistory.Count == 0 || commandPos <= 0)
				return;

			commandPos--;
			input = commandHistory[commandHistory.Count - commandPos - 1];
		}

		void ForwardWord()
		{
			Match match = _getEachWordRegex.Match(input, cursorPos);
			cursorPos = match.Success ? match.Index + match.Length : input.Length;
		}
		void BackwardWord()
		{
			cursorPos = _getEachWordRegex.Matches(input).Cast<Match>().Where(x => x.Index < cursorPos).Select(x => x.Index).LastOrDefault();
		}
		void RemoveCharLeft()
		{
			if (input.Length > 0 && cursorPos > 0)
			{
				input = input.Remove(cursorPos - 1, 1);
				cursorPos--;
			}
		}
		void RemoveCharRight()
		{
			if (input.Length > 0 && cursorPos < input.Length)
			{
				input = input.Remove(cursorPos, 1);
			}
		}

		void MoveCursorLeft()
		{
			if (cursorPos > 0)
				cursorPos--;
		}
		void MoveCursorRight()
		{
			if (cursorPos < input.Length)
				cursorPos++;
		}
		void MoveCursorToBegOfLine()
		{
			cursorPos = 0;
		}
		void MoveCursorToEndOfLine()
		{
			cursorPos = input.Length;
		}

		void ExecuteInput()
		{
			if (string.IsNullOrEmpty(input))
				return;

			commandPos = -1;
			if (commandHistory.LastOrDefault() != input)
				commandHistory.Add(input);

			if (_compilerTask != null)
			{
				PrintError("Can't compile input - Compiler is busy");
				return;
			}

			_compilerTask = Task.Factory.StartNew(new Func<Assembly>(() => {
				var compiler = new Microsoft.CSharp.CSharpCodeProvider();
				var compilerOptions = new System.CodeDom.Compiler.CompilerParameters();
				compilerOptions.GenerateInMemory = true;
				compilerOptions.IncludeDebugInformation = true;
				compilerOptions.ReferencedAssemblies.Add("System.dll");
				compilerOptions.ReferencedAssemblies.Add(typeof(Script).Assembly.Location); // TODO scriptAPI assembly

				foreach (ScriptDomain domain in ScriptDomain.Instances)
					foreach (Script script in domain.RunningScripts)
						if (!string.IsNullOrEmpty(script.Filename))
							compilerOptions.ReferencedAssemblies.Add(script.Filename);

				const string template = "using System; using GTA; using GTA.Native; using Console = SHVDN.Console; public class ConsoleInput : GTA.DefaultConsoleCommands {{ public static Object Execute(){{ {0}; return null; }} }}";

				System.CodeDom.Compiler.CompilerResults compilerResult = compiler.CompileAssemblyFromSource(compilerOptions, string.Format(template, input));

				if (!compilerResult.Errors.HasErrors)
				{
					return compilerResult.CompiledAssembly;
				}
				else
				{
					PrintError($"Couldn't compile input-string: {input}");

					StringBuilder errors = new StringBuilder();

					for (int i = 0; i < compilerResult.Errors.Count; ++i)
					{
						errors.Append("   at line ");
						errors.Append(compilerResult.Errors[i].Line);
						errors.Append(": ");
						errors.Append(compilerResult.Errors[i].ErrorText);

						if (i < compilerResult.Errors.Count - 1)
							errors.AppendLine();
					}

					PrintError(errors.ToString());
					return null;
				}
			}));
		}

        static int GetUtf8CodePointSize(string str, int index)
        {
            uint chr = str[index];

            if (chr < 0x80)
            {
                return 1;
            }
            if (chr < 0x800)
            {
                return 2;
            }
            if (chr < 0x10000)
            {
                return 3;
            }
            #region Surrogate check
            const int HighSurrogateStart = 0xD800;
            const int LowSurrogateStart = 0xD800;

            var temp1 = (int)chr - HighSurrogateStart;
            if (temp1 < 0 || temp1 > 0x7ff)
            {
                return 0;
            }
            // Found a high surrogate
            if (index < str.Length - 1)
            {
                var temp2 = str[index + 1] - LowSurrogateStart;
                if (temp2 >= 0 && temp2 <= 0x3ff)
                {
                    // Found a low surrogate
                    return 4;
                }

                return 0;
            }
            else
            {
                return 0;
            }
            #endregion
        }
        static unsafe void PushLongString(string str, int maxLengthUtf8 = 99)
        {
            if (maxLengthUtf8 <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLengthUtf8));
            }

            int size = System.Text.Encoding.UTF8.GetByteCount(str);

            if (size <= maxLengthUtf8)
            {
                NativeFunc.Invoke(0x6C188BE134E074AAul /*ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME*/,
                    (ulong)ScriptDomain.Instances[0].PinString(str).ToInt64());
                return;
            }

            int currentUtf8StrLength = 0;
            int startPos = 0;
            int currentPos;

            for (currentPos = 0; currentPos < str.Length; currentPos++)
            {
                int codePointSize = GetUtf8CodePointSize(str, currentPos);

                if (currentUtf8StrLength + codePointSize > maxLengthUtf8)
                {
                    NativeFunc.Invoke(0x6C188BE134E074AAul /*ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME*/,
                        (ulong)ScriptDomain.Instances[0].PinString(str.Substring(startPos, currentPos - startPos)).ToInt64());

                    currentUtf8StrLength = 0;
                    startPos = currentPos;
                }
                else
                {
                    currentUtf8StrLength += codePointSize;
                }

                //if the code point size is 4, additional increment is needed
                if (codePointSize == 4)
                {
                    currentPos++;
                }
            }

            NativeFunc.Invoke(0x6C188BE134E074AAul /*ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME*/,
                (ulong)ScriptDomain.Instances[0].PinString(str.Substring(startPos, str.Length - startPos)).ToInt64());
        }

        void DrawRect(float x, float y, int width, int height, Color color)
		{
			float w = (float)(width) / WIDTH;
			float h = (float)(height) / HEIGHT;
			float xNew = (x / WIDTH) + w * 0.5f;
			float yNew = (y / HEIGHT) + h * 0.5f;

            unsafe
            {
                NativeFunc.Invoke(0x3A618A217E5154F0ul /*DRAW_RECT*/, *(ulong*)&xNew, *(ulong*)&yNew, *(ulong*)&w, *(ulong*)&h, (ulong)color.R, (ulong)color.G, (ulong)color.B, (ulong)color.A);
            }
		}
		void DrawText(float x, float y, string text, Color color)
		{
			float xNew = (x / WIDTH);
			float yNew = (y / HEIGHT);

            unsafe
            {
                NativeFunc.Invoke(0x66E0276CC5F6B9DAul /*SET_TEXT_FONT*/, 0ul); // Chalet London :>
                NativeFunc.Invoke(0x07C837F9A01C34C9ul /*SET_TEXT_SCALE*/, 0x3eb33333ul /*0.35f*/, 0x3eb33333ul /*0.35f*/);
                NativeFunc.Invoke(0xBE6B23FFA53FB442ul /*SET_TEXT_COLOUR*/, (ulong)color.R, (ulong)color.G, (ulong)color.B, (ulong)color.A);
                NativeFunc.Invoke(0x25FBB336DF1804CBul /*BEGIN_TEXT_COMMAND_DISPLAY_TEXT*/, (ulong)NativeMemory.CellEmailBcon.ToInt64());
                PushLongString(text);
                NativeFunc.Invoke(0xCD015E5BB0D96A57ul /*END_TEXT_COMMAND_DISPLAY_TEXT*/, *(ulong*)&xNew, *(ulong*)&yNew);
            }
		}

        bool IsInputDisabled()
        {
            unsafe
            {
                return *NativeFunc.Invoke(0xA571D46727E2B718ul /*_IS_INPUT_DISABLED*/, 2ul) != 0;
            }
        }
		void SetControlsEnabled(bool enabled)
		{
            unsafe
            {
                NativeFunc.Invoke(0x5F4B6931816E599Bul /*DISABLE_ALL_CONTROL_ACTIONS*/, 0ul);

                for (int i = 1; i <= 6; i++)
                    NativeFunc.Invoke(0x351220255D64C155ul /*ENABLE_CONTROL_ACTION*/, 0ul, (ulong)i, enabled ? 1ul : 0ul);
            }
		}

		float GetTextLength(string text) //TODO Maybe implement somewhere else?
		{
            unsafe
            {
                NativeFunc.Invoke(0x54CE8AC98E120CABul /*_BEGIN_TEXT_COMMAND_WIDTH*/, (ulong)NativeMemory.CellEmailBcon.ToInt64());
                PushLongString(text);
                NativeFunc.Invoke(0x66E0276CC5F6B9DAul /*SET_TEXT_FONT*/, 0ul);
                NativeFunc.Invoke(0x07C837F9A01C34C9ul /*SET_TEXT_SCALE*/, 0x3eb33333ul /*0.35f*/, 0x3eb33333ul /*0.35f*/);

                return *(float*)NativeFunc.Invoke(0x85F061DA64ED2F67ul /*_END_TEXT_COMMAND_GET_WIDTH*/, 1ul);
            }
		}
	}

	public class DefaultConsoleCommands
	{
		[ConsoleCommand("Prints the default help")]
		public static void Help()
		{
			Console.Instance.PrintHelpString();
		}

		[ConsoleCommand("Prints the help for the specific command")]
		public static void Help(string command) //TODO Add all commands
		{

		}

		[ConsoleCommand("Loads scripts from a file")]
		public static void Load(string filename)
		{
			var domain = ScriptDomain.Instances.Last();

			string basedirectory = domain.AppDomain.BaseDirectory;

			if (!File.Exists(Path.Combine(basedirectory, filename)))
			{
				string[] files = Directory.GetFiles(basedirectory, filename, SearchOption.AllDirectories);

				if (files.Length != 1)
				{
					Console.PrintError("The file " + filename + " was not found in " + basedirectory);
					return;
				}

				Console.PrintWarning("The file " + filename + " was not found in " + basedirectory + ", loading from " + Path.GetDirectoryName(files[0].Substring(basedirectory.Length + 1)) + " instead");

				filename = files[0].Substring(basedirectory.Length + 1);
			}
			else
			{
				filename = Path.Combine(basedirectory, filename);
			}

			filename = Path.GetFullPath(filename);

			string extension = Path.GetExtension(filename).ToLower();

			if (extension != ".cs" && extension != ".vb" && extension != ".dll")
			{
				Console.PrintError("The file '" + filename + "' was not recognized as a script file");
				return;
			}

			foreach (var script in domain.RunningScripts)
			{
				if (filename.Equals(script.Filename, StringComparison.OrdinalIgnoreCase) && script.IsRunning)
				{
					Console.PrintError("The script is already running");
					return;
				}
			}

			domain.StartScripts(filename);
		}

		[ConsoleCommand("Reloads scripts from a file")]
		public static void Reload(string filename)
		{
			Abort(filename);
			Load(filename);
		}

		[ConsoleCommand("Reloads all scripts found in the script folder")]
		public static void ReloadAllScripts()
		{
			var domain = ScriptDomain.Instances.Last();

			domain.Abort();
			domain.StartAllScripts();
		}

		[ConsoleCommand("List all loaded scripts")]
		public static void List()
		{
			var scripts = ScriptDomain.CurrentDomain.RunningScripts;
			if (scripts.Length == 0)
			{
				Console.PrintInfo("There are no scripts loaded");
				return;
			}

			Console.PrintInfo("---");
			foreach (var script in scripts)
				Console.PrintInfo("   " + Path.GetFileName(script.Filename) + ": " + script.Name + (script.IsRunning ? " ~g~[running]" : " ~r~[aborted]"));
			Console.PrintInfo("---");
		}

		[ConsoleCommand("Aborts all scripts from the specified file")]
		public static void Abort(string filename)
		{
			var domain = ScriptDomain.Instances.Last();

			string basedirectory = domain.AppDomain.BaseDirectory;

			filename = Path.Combine(basedirectory, filename);

			if (!File.Exists(filename))
			{
				Console.PrintError("The file '" + filename + "' was not found");
				return;
			}

			domain.AbortScripts(filename);
		}

		[ConsoleCommand("Aborts all scripts found in the script folder")]
		public static void AbortAllScripts()
		{
			var domain = ScriptDomain.Instances.Last();

			domain.Abort();
		}

		[ConsoleCommand("Clears the console output")]
		public static void Clear()
		{
			Console.Instance.Clear();
		}
	}
}
