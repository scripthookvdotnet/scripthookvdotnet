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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SHVDN
{
	public class Console : MarshalByRefObject
	{
		int cursorPos = 0;
		int commandPos = -1;
		int currentPage = 1;
		string input = string.Empty;
		List<string> lineHistory = new List<string>();
		List<string> commandHistory = new List<string>();
		ConcurrentQueue<string[]> outputQueue = new ConcurrentQueue<string[]>();
		Dictionary<string, List<ConsoleCommand>> commands = new Dictionary<string, List<ConsoleCommand>>();
		DateTime lastClosed;
		Task<MethodInfo> compilerTask;
		const int BASE_WIDTH = 1280;
		const int BASE_HEIGHT = 720;
		const int CONSOLE_WIDTH = BASE_WIDTH;
		const int CONSOLE_HEIGHT = BASE_HEIGHT / 3;
		const int INPUT_HEIGHT = 20;
		const int LINES_PER_PAGE = 16;

		static readonly Color InputColor = Color.White;
		static readonly Color InputColorBusy = Color.DarkGray;
		static readonly Color OutputColor = Color.White;
		static readonly Color PrefixColor = Color.FromArgb(255, 52, 152, 219);
		static readonly Color BackgroundColor = Color.FromArgb(200, Color.Black);
		static readonly Color AltBackgroundColor = Color.FromArgb(200, 52, 73, 94);

		[DllImport("user32.dll")]
		static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState, [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer, int bufferSize, uint flags);

		/// <summary>
		/// Gets or sets whether the console is open.
		/// </summary>
		public bool IsOpen { get; set; }

		/// <summary>
		/// Gets or sets the key used to open or close the console.
		/// </summary>
		public Keys ToggleKey { get; set; } = Keys.F3;

		/// <summary>
		/// Register the specified method as a console command.
		/// </summary>
		/// <param name="command">The command attribute of the method.</param>
		/// <param name="methodInfo">The method information.</param>
		public void RegisterCommand(ConsoleCommand command, MethodInfo methodInfo)
		{
			command.MethodInfo = methodInfo;

			if (!commands.ContainsKey(command.Namespace))
				commands[command.Namespace] = new List<ConsoleCommand>();
			commands[command.Namespace].Add(command);
		}
		/// <summary>
		/// Register all methods with a <see cref="ConsoleCommand"/> attribute in the specified type as console commands.
		/// </summary>
		/// <param name="type">The type to search for console command methods.</param>
		public void RegisterCommands(Type type)
		{
			foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
				foreach (var attribute in method.GetCustomAttributes<ConsoleCommand>(true))
					RegisterCommand(attribute, method);
		}
		/// <summary>
		/// Unregister all methods with a <see cref="ConsoleCommand"/> attribute that were previously registered.
		/// </summary>
		/// <param name="type">The type to search for console command methods.</param>
		public void UnregisterCommands(Type type)
		{
			foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
			{
				foreach (var attribute in method.GetCustomAttributes<ConsoleCommand>(true))
				{
					string space = method.DeclaringType.FullName;

					if (commands.ContainsKey(space))
					{
						commands[space].RemoveAll(x => x.MethodInfo == method);
						if (commands[space].Count == 0)
							commands.Remove(space);
					}
				}
			}
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

			outputQueue.Enqueue(messages);
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
		/// Clears the console output.
		/// </summary>
		public void Clear()
		{
			lineHistory.Clear();
		}

		/// <summary>
		/// Writes an info message to the console.
		/// </summary>
		/// <param name="msg">The composite format string.</param>
		/// <param name="args">The formatting arguments.</param>
		public void PrintInfo(string msg, params object[] args)
		{
			if (args.Length > 0)
				msg = String.Format(msg, args);
			AddLines("[~b~INFO~w~] ", msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
		}
		/// <summary>
		/// Writes an error message to the console.
		/// </summary>
		/// <param name="msg">The composite format string.</param>
		/// <param name="args">The formatting arguments.</param>
		public void PrintError(string msg, params object[] args)
		{
			if (args.Length > 0)
				msg = String.Format(msg, args);
			AddLines("[~r~ERROR~w~] ", msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
		}
		/// <summary>
		/// Writes a warning message to the console.
		/// </summary>
		/// <param name="msg">The composite format string.</param>
		/// <param name="args">The formatting arguments.</param>
		public void PrintWarning(string msg, params object[] args)
		{
			if (args.Length > 0)
				msg = String.Format(msg, args);
			AddLines("[~o~WARNING~w~] ", msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
		}

		/// <summary>
		/// Writes the help text for all commands to the console.
		/// </summary>
		internal void PrintHelpText()
		{
			StringBuilder help = new StringBuilder();
			foreach (var space in commands.Keys)
			{
				help.AppendLine($"[{space}]");
				foreach (var command in commands[space])
				{
					help.Append("    ~h~" + command.Name + "(");
					foreach (var arg in command.MethodInfo.GetParameters())
						help.Append(arg.ParameterType.Name + " " + arg.Name + ",");
					if (command.MethodInfo.GetParameters().Length > 0)
						help.Length--; // Remove trailing comma
					if (command.Help.Length > 0)
						help.AppendLine(")~h~: " + command.Help);
					else
						help.AppendLine(")~h~");
				}
			}

			PrintInfo(help.ToString());
		}
		/// <summary>
		/// Writes the help text for the specified command to the console.
		/// </summary>
		/// <param name="commandName">The command name to check.</param>
		internal void PrintHelpText(string commandName)
		{
			foreach (var space in commands.Keys)
			{
				foreach (var command in commands[space])
				{
					if (command.Name == commandName)
					{
						PrintInfo(command.Name + ": " + command.Help);
						return;
					}
				}
			}
		}

		/// <summary>
		/// Main execution logic of the console.
		/// </summary>
		internal void DoTick()
		{
			DateTime now = DateTime.UtcNow;

			// Execute compiled input line script
			if (compilerTask != null && compilerTask.IsCompleted)
			{
				if (compilerTask.Result != null)
				{
					try
					{
						var result = compilerTask.Result.Invoke(null, null);
						if (result != null)
							PrintInfo($"[Return Value]: {result}");
					}
					catch (TargetInvocationException ex)
					{
						PrintError($"[Exception]: {ex.InnerException.ToString()}");
					}
				}

				ClearInput();

				// Reset compiler task
				compilerTask = null;
			}

			// Add lines from concurrent queue to history
			if (outputQueue.TryDequeue(out string[] lines))
				foreach (string line in lines)
					lineHistory.Add(line);

			// Hack so the input gets blocked long enough
			if (lastClosed > now)
			{
				if (IsInputDisabled())
					SetControlsEnabled(false);
				return;
			}

			if (!IsOpen)
				return; // Nothing more to do here when the console is not open

			if (IsInputDisabled())
				SetControlsEnabled(false);

			// Draw background
			DrawRect(0, 0, CONSOLE_WIDTH, CONSOLE_HEIGHT, BackgroundColor);
			// Draw input field
			DrawRect(0, CONSOLE_HEIGHT, CONSOLE_WIDTH, INPUT_HEIGHT, AltBackgroundColor);
			DrawRect(0, CONSOLE_HEIGHT + INPUT_HEIGHT, 80, INPUT_HEIGHT, AltBackgroundColor);
			// Draw input prefix
			DrawText(0, CONSOLE_HEIGHT, "$>", PrefixColor);
			// Draw input text
			DrawText(25, CONSOLE_HEIGHT, input, compilerTask == null ? InputColor : InputColorBusy);
			// Draw page information
			DrawText(5, CONSOLE_HEIGHT + INPUT_HEIGHT, "Page " + currentPage + "/" + System.Math.Max(1, ((lineHistory.Count + (LINES_PER_PAGE - 1)) / LINES_PER_PAGE)), InputColor);

			// Draw blinking cursor
			if (now.Millisecond < 500)
			{
				float length = GetTextLength(input.Substring(0, cursorPos));
				DrawText(25 + (length * CONSOLE_WIDTH) - 4, CONSOLE_HEIGHT, "~w~~h~|~w~", InputColor);
			}

			// Draw console history text
			int historyOffset = lineHistory.Count - (LINES_PER_PAGE * currentPage);
			int historyLength = historyOffset + LINES_PER_PAGE;
			for (int i = System.Math.Max(0, historyOffset); i < historyLength; ++i)
			{
				DrawText(2, (float)((i - historyOffset) * 14), lineHistory[i], OutputColor);
			}
		}
		/// <summary>
		/// Keyboard handling logic of the console.
		/// </summary>
		/// <param name="keys">The key that was originated this event and its modifiers.</param>
		/// <param name="status"><c>true</c> on a key down, <c>false</c> on a key up event.</param>
		internal void DoKeyEvent(Keys keys, bool status)
		{
			if (!status)
				return; // Only interested in key down events

			var e = new KeyEventArgs(keys);

			if (e.KeyCode == ToggleKey)
			{
				// Toggle open state
				IsOpen = !IsOpen;
				SetControlsEnabled(false);
				if (!IsOpen)
					lastClosed = DateTime.UtcNow.AddMilliseconds(200); // Hack so the input gets blocked long enough
				return; // The toggle key does not need any additional handling
			}

			if (!IsOpen)
			{
				// Do not need to handle keyboard events when the console is not open
				return;
			}

			if (e.KeyCode == Keys.PageUp)
			{
				PageUp();
				return;
			}
			if (e.KeyCode == Keys.PageDown)
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
				case Keys.Home:
					MoveCursorToBegOfLine();
					break;
				case Keys.End:
					MoveCursorToEndOfLine();
					break;
				case Keys.Up:
					GoUpCommandList();
					break;
				case Keys.Down:
					GoDownCommandList();
					break;
				case Keys.Enter:
					CompileExpression();
					break;
				case Keys.Escape:
					IsOpen = false;
					SetControlsEnabled(false);
					lastClosed = DateTime.UtcNow.AddMilliseconds(200); // Hack so the input gets blocked long enough
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
					var buf = new StringBuilder(256);
					var keyboardState = new byte[256];
					keyboardState[(int)Keys.Menu] = e.Alt ? (byte)0xff : (byte)0;
					keyboardState[(int)Keys.ShiftKey] = e.Shift ? (byte)0xff : (byte)0;
					keyboardState[(int)Keys.ControlKey] = e.Control ? (byte)0xff : (byte)0;

					// Translate key event to character for text input
					ToUnicode((uint)e.KeyCode, 0, keyboardState, buf, 256, 0);
					AddToInput(buf.ToString());
					break;
			}
		}

		void PageUp()
		{
			if (currentPage < ((lineHistory.Count + LINES_PER_PAGE - 1) / LINES_PER_PAGE))
				currentPage++;
		}
		void PageDown()
		{
			if (currentPage > 1)
				currentPage--;
		}
		void GoUpCommandList()
		{
			if (commandHistory.Count == 0 || commandPos >= commandHistory.Count - 1)
				return;

			commandPos++;
			input = commandHistory[commandHistory.Count - commandPos - 1];
			// Reset cursor position to end of input text
			cursorPos = input.Length;
		}
		void GoDownCommandList()
		{
			if (commandHistory.Count == 0 || commandPos <= 0)
				return;

			commandPos--;
			input = commandHistory[commandHistory.Count - commandPos - 1];
			cursorPos = input.Length;
		}

		void ForwardWord()
		{
			var regex = new Regex(@"[^\W_]+");
			Match match = regex.Match(input, cursorPos);
			cursorPos = match.Success ? match.Index + match.Length : input.Length;
		}
		void BackwardWord()
		{
			var regex = new Regex(@"[^\W_]+");
			MatchCollection matches = regex.Matches(input);
			cursorPos = matches.Cast<Match>().Where(x => x.Index < cursorPos).Select(x => x.Index).LastOrDefault();
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

		void CompileExpression()
		{
			if (string.IsNullOrEmpty(input) || compilerTask != null)
				return;

			commandPos = -1;
			if (commandHistory.LastOrDefault() != input)
				commandHistory.Add(input);

			compilerTask = Task.Factory.StartNew(() => {
				var compiler = new Microsoft.CSharp.CSharpCodeProvider();
				var compilerOptions = new System.CodeDom.Compiler.CompilerParameters();
				compilerOptions.GenerateInMemory = true;
				compilerOptions.IncludeDebugInformation = true;
				compilerOptions.ReferencedAssemblies.Add("System.dll");
				compilerOptions.ReferencedAssemblies.Add("System.Core.dll");
				compilerOptions.ReferencedAssemblies.Add("System.Drawing.dll");
				compilerOptions.ReferencedAssemblies.Add("System.Windows.Forms.dll");
				// Reference the newest scripting API
				compilerOptions.ReferencedAssemblies.Add("ScriptHookVDotNet3.dll");
				compilerOptions.ReferencedAssemblies.Add(typeof(ScriptDomain).Assembly.Location);

				foreach (var assembly in ScriptDomain.CurrentDomain.LoadedAssemblies)
					compilerOptions.ReferencedAssemblies.Add(assembly.Location);

				const string template = "using System; using System.Linq; using GTA; using GTA.Native; public class ConsoleInput : ScriptHookVDotNet {{ public static object Execute() {{ {0}; return null; }} }}";

				System.CodeDom.Compiler.CompilerResults compilerResult = compiler.CompileAssemblyFromSource(compilerOptions, string.Format(template, input));

				if (!compilerResult.Errors.HasErrors)
				{
					return compilerResult.CompiledAssembly.GetType("ConsoleInput").GetMethod("Execute");
				}
				else
				{
					PrintError($"Couldn't compile input expression: {input}");

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
			});
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}

		static unsafe void DrawRect(float x, float y, int width, int height, Color color)
		{
			float w = (float)(width) / BASE_WIDTH;
			float h = (float)(height) / BASE_HEIGHT;
			float xNew = (x / BASE_WIDTH) + w * 0.5f;
			float yNew = (y / BASE_HEIGHT) + h * 0.5f;

			NativeFunc.Invoke(0x3A618A217E5154F0ul /*DRAW_RECT*/, *(ulong*)&xNew, *(ulong*)&yNew, *(ulong*)&w, *(ulong*)&h, (ulong)color.R, (ulong)color.G, (ulong)color.B, (ulong)color.A);
		}
		static unsafe void DrawText(float x, float y, string text, Color color)
		{
			float xNew = (x / BASE_WIDTH);
			float yNew = (y / BASE_HEIGHT);

			NativeFunc.Invoke(0x66E0276CC5F6B9DAul /*SET_TEXT_FONT*/, 0ul); // Chalet London :>
			NativeFunc.Invoke(0x07C837F9A01C34C9ul /*SET_TEXT_SCALE*/, 0x3eb33333ul /*0.35f*/, 0x3eb33333ul /*0.35f*/);
			NativeFunc.Invoke(0xBE6B23FFA53FB442ul /*SET_TEXT_COLOUR*/, (ulong)color.R, (ulong)color.G, (ulong)color.B, (ulong)color.A);
			NativeFunc.Invoke(0x25FBB336DF1804CBul /*BEGIN_TEXT_COMMAND_DISPLAY_TEXT*/, (ulong)NativeMemory.CellEmailBcon.ToInt64());
			PushLongString(text);
			NativeFunc.Invoke(0xCD015E5BB0D96A57ul /*END_TEXT_COMMAND_DISPLAY_TEXT*/, *(ulong*)&xNew, *(ulong*)&yNew);
		}

		static unsafe bool IsInputDisabled()
		{
			return *NativeFunc.Invoke(0xA571D46727E2B718ul /*_IS_INPUT_DISABLED*/, 2ul) != 0;
		}
		static unsafe void SetControlsEnabled(bool enabled)
		{
			NativeFunc.Invoke(0x5F4B6931816E599Bul /*DISABLE_ALL_CONTROL_ACTIONS*/, 0ul);

			for (int i = 1; i <= 6; i++)
				NativeFunc.Invoke(0x351220255D64C155ul /*ENABLE_CONTROL_ACTION*/, 0ul, (ulong)i, enabled ? 1ul : 0ul);
		}

		static int GetUtf8CodePointSize(string str, int index)
		{
			var chr = str[index];
			if (chr < 0x80)
				return 1;
			if (chr < 0x800)
				return 2;
			if (chr < 0x10000)
				return 3;

			#region Surrogate check
			const int LowSurrogateStart = 0xD800;
			const int HighSurrogateStart = 0xD800;

			var temp1 = (int)chr - HighSurrogateStart;
			if (temp1 >= 0 && temp1 <= 0x7ff)
			{
				// Found a high surrogate
				if (index < str.Length - 1)
				{
					var temp2 = str[index + 1] - LowSurrogateStart;
					if (temp2 >= 0 && temp2 <= 0x3ff)
					{
						// Found a low surrogate
						return 4;
					}
				}
			}
			#endregion

			return 0;
		}
		static unsafe void PushLongString(string str)
		{
			int size = System.Text.Encoding.UTF8.GetByteCount(str);
			if (size <= 99)
			{
				NativeFunc.Invoke(0x6C188BE134E074AAul /*ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME*/,
					(ulong)ScriptDomain.CurrentDomain.PinString(str).ToInt64());
				return;
			}

			int startPos = 0;
			int currentUtf8StrLength = 0;

			for (int currentPos = 0; currentPos < str.Length; currentPos++)
			{
				int codePointSize = GetUtf8CodePointSize(str, currentPos);

				if (currentUtf8StrLength + codePointSize > 99)
				{
					NativeFunc.Invoke(0x6C188BE134E074AAul /*ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME*/,
						(ulong)ScriptDomain.CurrentDomain.PinString(str.Substring(startPos, currentPos - startPos)).ToInt64());

					currentUtf8StrLength = 0;
					startPos = currentPos;
				}
				else
				{
					currentUtf8StrLength += codePointSize;
				}

				if (codePointSize == 4)
					currentPos++; // If the code point size is 4, additional increment is needed
			}

			NativeFunc.Invoke(0x6C188BE134E074AAul /*ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME*/,
				(ulong)ScriptDomain.CurrentDomain.PinString(str.Substring(startPos, str.Length - startPos)).ToInt64());
		}
		static unsafe float GetTextLength(string text)
		{
			NativeFunc.Invoke(0x54CE8AC98E120CABul /*_BEGIN_TEXT_COMMAND_WIDTH*/, (ulong)NativeMemory.CellEmailBcon.ToInt64());
			PushLongString(text);
			NativeFunc.Invoke(0x66E0276CC5F6B9DAul /*SET_TEXT_FONT*/, 0ul);
			NativeFunc.Invoke(0x07C837F9A01C34C9ul /*SET_TEXT_SCALE*/, 0x3eb33333ul /*0.35f*/, 0x3eb33333ul /*0.35f*/);

			return *(float*)NativeFunc.Invoke(0x85F061DA64ED2F67ul /*_END_TEXT_COMMAND_GET_WIDTH*/, 1ul);
		}
	}

	public class ConsoleCommand : Attribute
	{
		public ConsoleCommand() : this(string.Empty)
		{
		}
		public ConsoleCommand(string help)
		{
			Help = help;
		}

		public string Help { get; }

		internal string Name => MethodInfo.Name;
		internal string Namespace => MethodInfo.DeclaringType.FullName;
		internal MethodInfo MethodInfo { get; set; }
	}
}
