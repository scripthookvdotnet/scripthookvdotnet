//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SHVDN
{
	public sealed class Console : MarshalByRefObject
	{
		private int _cursorPos = 0;
		private int _commandPos = -1;
		private int _currentPage = 1;
		private bool _isOpen = false;
		private string _input = string.Empty;
		private List<string> _lineHistory = new();
		private List<string> _commandHistory; // This must be set via CommandHistory property
		private ConcurrentQueue<string[]> _outputQueue = new();
		private Dictionary<string, List<ConsoleCommand>> _commands = new();
		private int _lastClosedTickCount;
		private bool _shouldBlockControls;
		private Task<MethodInfo> _compilerTask;
		private const int BaseWidth = 1280;
		private const int BaseHeight = 720;
		private const int ConsoleWidth = BaseWidth;
		private const int ConsoleHeight = BaseHeight / 3;
		private const int InputHeight = 20;
		private const int LinesPerPage = 16;

		private static readonly Color s_inputColor = Color.White;
		private static readonly Color s_inputColorBusy = Color.DarkGray;
		private static readonly Color s_outputColor = Color.White;
		private static readonly Color s_prefixColor = Color.FromArgb(255, 52, 152, 219);
		private static readonly Color s_backgroundColor = Color.FromArgb(200, Color.Black);
		private static readonly Color s_altBackgroundColor = Color.FromArgb(200, 52, 73, 94);

		[DllImport("user32.dll")]
		private static extern int ToUnicode(
			uint virtualKeyCode, uint scanCode, byte[] keyboardState,
			[Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer, int bufferSize, uint flags);

		/// <summary>
		/// Gets or sets whether the console is open.
		/// </summary>
		public bool IsOpen
		{
			get => _isOpen;
			set
			{
				_isOpen = value;
				DisableControlsThisFrame();
				if (_isOpen)
				{
					return;
				}

				_lastClosedTickCount = Environment.TickCount + 200; // Hack so the input gets blocked long enough
				_shouldBlockControls = true;
			}
		}

		/// <summary>
		/// Gets or sets the command history. This is used to avoid losing the command history on SHVDN reloading.
		/// </summary>
		public List<string> CommandHistory
		{
			get => _commandHistory;
			set => _commandHistory = value;
		}

		/// <summary>
		/// Register the specified method as a console command.
		/// </summary>
		/// <param name="command">The command attribute of the method.</param>
		/// <param name="methodInfo">The method information.</param>
		public void RegisterCommand(ConsoleCommand command, MethodInfo methodInfo)
		{
			command.MethodInfo = methodInfo;

			if (!_commands.ContainsKey(command.Namespace))
			{
				_commands[command.Namespace] = new List<ConsoleCommand>();
			}

			_commands[command.Namespace].Add(command);
		}
		/// <summary>
		/// Register all methods with a <see cref="ConsoleCommand"/> attribute in the specified type as console commands.
		/// </summary>
		/// <param name="type">The type to search for console command methods.</param>
		public void RegisterCommands(Type type)
		{
			foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
			{
				try
				{
					foreach (ConsoleCommand attribute in method.GetCustomAttributes<ConsoleCommand>(true))
					{
						RegisterCommand(attribute, method);
					}
				}
				catch (Exception ex)
				{
					Log.Message(Log.Level.Error, "Failed to search for console commands in ", type.FullName, ".", method.Name, ": ", ex.ToString());
				}
			}
		}
		/// <summary>
		/// Unregister all methods with a <see cref="ConsoleCommand"/> attribute that were previously registered.
		/// </summary>
		/// <param name="type">The type to search for console command methods.</param>
		public void UnregisterCommands(Type type)
		{
			foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
			{
				string space = method.DeclaringType.FullName;

				if (!_commands.TryGetValue(space, out List<ConsoleCommand> command))
				{
					continue;
				}

				command.RemoveAll(x => x.MethodInfo == method);

				if (command.Count == 0)
				{
					_commands.Remove(space);
				}
			}
		}

		/// <summary>
		/// Add text lines to the console. This call is thread-safe.
		/// </summary>
		/// <param name="prefix">The prefix for each line.</param>
		/// <param name="messages">The lines to add to the console.</param>
		private void AddLines(string prefix, string[] messages)
		{
			AddLines(prefix, messages, "~w~");
		}
		/// <summary>
		/// Add colored text lines to the console. This call is thread-safe.
		/// </summary>
		/// <param name="prefix">The prefix for each line.</param>
		/// <param name="messages">The lines to add to the console.</param>
		/// <param name="color">The color of those lines.</param>
		private void AddLines(string prefix, string[] messages, string color)
		{
			for (int i = 0; i < messages.Length; i++) // Add proper styling
			{
				messages[i] = $"~c~[{DateTime.Now.ToString("HH:mm:ss")}] ~w~{prefix} {color}{messages[i]}";
			}

			_outputQueue.Enqueue(messages);
		}
		/// <summary>
		/// Add text to the console input line.
		/// </summary>
		/// <param name="text">The text to add.</param>
		private void AddToInput(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}

			_input = _input.Insert(_cursorPos, text);
			_cursorPos += text.Length;
		}
		/// <summary>
		/// Paste clipboard content into the console input line.
		/// </summary>
		private void AddClipboardContent()
		{
			string text = Clipboard.GetText();
			text = text.Replace("\n", string.Empty); // TODO Keep this?

			AddToInput(text);
		}

		/// <summary>
		/// Clear the console input line.
		/// </summary>
		private void ClearInput()
		{
			_input = string.Empty;
			_cursorPos = 0;
		}
		/// <summary>
		/// Clears the console output.
		/// </summary>
		public void Clear()
		{
			_lineHistory.Clear();
			_currentPage = 1;
		}

		/// <summary>
		/// Writes an info message to the console.
		/// </summary>
		/// <param name="msg">The composite format string.</param>
		public void PrintInfo(string msg)
		{
			AddLines("[~b~INFO~w~] ", msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
		}
		/// <summary>
		/// Writes an info message to the console.
		/// </summary>
		/// <param name="msg">The composite format string.</param>
		/// <param name="args">The formatting arguments.</param>
		public void PrintInfo(string msg, params object[] args)
		{
			if (args.Length > 0)
			{
				msg = String.Format(msg, args);
			}

			AddLines("[~b~INFO~w~] ", msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
		}
		/// <summary>
		/// Writes an error message to the console.
		/// </summary>
		/// <param name="msg">The composite format string.</param>
		public void PrintError(string msg)
		{
			AddLines("[~r~ERROR~w~] ", msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
		}
		/// <summary>
		/// Writes an error message to the console.
		/// </summary>
		/// <param name="msg">The composite format string.</param>
		/// <param name="args">The formatting arguments.</param>
		public void PrintError(string msg, params object[] args)
		{
			if (args.Length > 0)
			{
				msg = String.Format(msg, args);
			}

			AddLines("[~r~ERROR~w~] ", msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
		}
		/// <summary>
		/// Writes a warning message to the console.
		/// </summary>
		/// <param name="msg">The composite format string.</param>
		public void PrintWarning(string msg)
		{
			AddLines("[~o~WARNING~w~] ", msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
		}
		/// <summary>
		/// Writes a warning message to the console.
		/// </summary>
		/// <param name="msg">The composite format string.</param>
		/// <param name="args">The formatting arguments.</param>
		public void PrintWarning(string msg, params object[] args)
		{
			if (args.Length > 0)
			{
				msg = String.Format(msg, args);
			}

			AddLines("[~o~WARNING~w~] ", msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
		}

		/// <summary>
		/// Writes the help text for all commands to the console.
		/// </summary>
		internal void PrintHelpText()
		{
			var help = new StringBuilder();
			foreach (string space in _commands.Keys)
			{
				help.AppendLine($"[{space}]");
				foreach (ConsoleCommand command in _commands[space])
				{
					help.Append("    ~h~" + command.Name + "(");
					foreach (ParameterInfo arg in command.MethodInfo.GetParameters())
					{
						help.Append(arg.ParameterType.Name + " " + arg.Name + ",");
					}

					if (command.MethodInfo.GetParameters().Length > 0)
					{
						help.Length--; // Remove trailing comma
					}

					if (command.Help.Length > 0)
					{
						help.AppendLine(")~h~: " + command.Help);
					}
					else
					{
						help.AppendLine(")~h~");
					}
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
			foreach (string space in _commands.Keys)
			{
				foreach (ConsoleCommand command in _commands[space])
				{
					if (command.Name != commandName)
					{
						continue;
					}

					PrintInfo(command.Name + ": " + command.Help);
					return;
				}
			}
		}

		/// <summary>
		/// Main execution logic of the console.
		/// </summary>
		internal void DoTick()
		{
			int nowTickCount = Environment.TickCount;

			// Execute compiled input line script
			if (_compilerTask != null && _compilerTask.IsCompleted)
			{
				if (_compilerTask.Result != null)
				{
					try
					{
						object result = _compilerTask.Result.Invoke(null, null);
						if (result != null)
						{
							PrintInfo($"[Return Value]: {result}");
						}
					}
					catch (TargetInvocationException ex)
					{
						PrintError($"[Exception]: {ex.InnerException.ToString()}");
					}
				}

				ClearInput();

				// Reset compiler task
				_compilerTask = null;
			}

			// Add lines from concurrent queue to history
			if (_outputQueue.TryDequeue(out string[] lines))
			{
				foreach (string line in lines)
				{
					_lineHistory.Add(line);
				}
			}

			if (!IsOpen)
			{
				// Hack so the input gets blocked long enough
				if ((_lastClosedTickCount - nowTickCount) > 0)
				{
					if (_shouldBlockControls)
					{
						DisableControlsThisFrame();
					}
				}
				// The console is not open for more than about 24.9 days, calculating the elapsed time with 2 int tick count vars doesn't do the job
				else if (_shouldBlockControls)
				{
					_shouldBlockControls = false;
				}
				return; // Nothing more to do here when the console is not open
			}

			// Disable controls while the console is open
			DisableControlsThisFrame();

			// Draw background
			DrawRect(0, 0, ConsoleWidth, ConsoleHeight, s_backgroundColor);
			// Draw input field
			DrawRect(0, ConsoleHeight, ConsoleWidth, InputHeight, s_altBackgroundColor);
			DrawRect(0, ConsoleHeight + InputHeight, 80, InputHeight, s_altBackgroundColor);
			// Draw input prefix
			DrawText(0, ConsoleHeight, "$>", s_prefixColor);
			// Draw input text
			DrawText(25, ConsoleHeight, _input, _compilerTask == null ? s_inputColor : s_inputColorBusy);
			// Draw page information
			DrawText(5, ConsoleHeight + InputHeight, "Page " + _currentPage + "/" + System.Math.Max(1, ((_lineHistory.Count + (LinesPerPage - 1)) / LinesPerPage)), s_inputColor);

			// Draw blinking cursor
			if (nowTickCount % 1000 < 500)
			{
				float lengthBetweenInputStartAndCursor = GetTextLength(_input.Substring(0, _cursorPos)) - GetMarginLength();
				DrawRect(26 + (lengthBetweenInputStartAndCursor * ConsoleWidth), ConsoleHeight + 2, 2, InputHeight - 4, Color.White);
			}

			// Draw console history text
			int historyOffset = _lineHistory.Count - (LinesPerPage * _currentPage);
			int historyLength = historyOffset + LinesPerPage;
			for (int i = System.Math.Max(0, historyOffset); i < historyLength; ++i)
			{
				DrawText(2, (float)((i - historyOffset) * 14), _lineHistory[i], s_outputColor);
			}
		}
		/// <summary>
		/// Keyboard handling logic of the console.
		/// </summary>
		/// <param name="keys">The key that was originated this event and its modifiers.</param>
		/// <param name="status"><see langword="true" /> on a key down, <see langword="false" /> on a key up event.</param>
		internal void DoKeyEvent(Keys keys, bool status)
		{
			if (!status || !IsOpen)
			{
				return; // Only interested in key down events and do not need to handle events when the console is not open
			}

			var e = new KeyEventArgs(keys);

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
					if (e.Alt)
					{
						BackwardKillWord();
					}
					else
					{
						BackwardDeleteChar();
					}

					break;
				case Keys.Delete:
					ForwardDeleteChar();
					break;
				case Keys.Left:
					if (e.Control)
					{
						BackwardWord();
					}
					else
					{
						MoveCursorLeft();
					}

					break;
				case Keys.Right:
					if (e.Control)
					{
						ForwardWord();
					}
					else
					{
						MoveCursorRight();
					}

					break;
				case Keys.Insert:
					if (e.Shift)
					{
						AddClipboardContent();
					}

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
					break;
				case Keys.B:
					if (e.Control)
					{
						MoveCursorLeft();
					}
					else if (e.Alt)
					{
						BackwardWord();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.D:
					if (e.Alt)
					{
						KillWord();
					}
					else if (e.Control)
					{
						ForwardDeleteChar();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.F:
					if (e.Control)
					{
						MoveCursorRight();
					}
					else if (e.Alt)
					{
						ForwardWord();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.H:
					if (e.Control)
					{
						BackwardDeleteChar();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.A:
					if (e.Control)
					{
						MoveCursorToBegOfLine();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.E:
					if (e.Control)
					{
						MoveCursorToEndOfLine();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.P:
					if (e.Control)
					{
						GoUpCommandList();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.K:
					if (e.Control)
					{
						BackwardKillLine();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.M:
					if (e.Control)
					{
						CompileExpression();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.N:
					if (e.Control)
					{
						GoDownCommandList();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.L:
					if (e.Control)
					{
						Clear();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.T:
					if (e.Alt)
					{
						TransposeTwoWords();
					}
					else if (e.Control)
					{
						TransposeTwoChars();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.U:
					if (e.Control)
					{
						KillLine();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.V:
					if (e.Control)
					{
						AddClipboardContent();
					}
					else
					{
						goto default;
					}

					break;
				case Keys.W:
					if (e.Control)
					{
						UnixWordRubout();
					}
					else
					{
						goto default;
					}

					break;
				default:
					var buf = new StringBuilder(256);
					byte[] keyboardState = new byte[256];
					keyboardState[(int)Keys.Menu] = e.Alt ? (byte)0xff : (byte)0;
					keyboardState[(int)Keys.ShiftKey] = e.Shift ? (byte)0xff : (byte)0;
					keyboardState[(int)Keys.ControlKey] = e.Control ? (byte)0xff : (byte)0;

					// Translate key event to character for text input
					ToUnicode((uint)e.KeyCode, 0, keyboardState, buf, 256, 0);
					AddToInput(buf.ToString());
					break;
			}
		}

		private void PageUp()
		{
			if (_currentPage < ((_lineHistory.Count + LinesPerPage - 1) / LinesPerPage))
			{
				_currentPage++;
			}
		}

		private void PageDown()
		{
			if (_currentPage > 1)
			{
				_currentPage--;
			}
		}

		private void GoUpCommandList()
		{
			if (_commandHistory.Count == 0 || _commandPos >= _commandHistory.Count - 1)
			{
				return;
			}

			_commandPos++;
			_input = _commandHistory[_commandHistory.Count - _commandPos - 1];
			// Reset cursor position to end of input text
			_cursorPos = _input.Length;
		}

		private void GoDownCommandList()
		{
			if (_commandHistory.Count == 0 || _commandPos <= 0)
			{
				return;
			}

			_commandPos--;
			_input = _commandHistory[_commandHistory.Count - _commandPos - 1];
			_cursorPos = _input.Length;
		}

		/// <summary>
		/// Moves to the end of the next word, just like emacs and GNU readline (does not move to the beginning of the next word like zsh does for forward-word).
		/// Words are composed of letters and digits.
		/// </summary>
		private void ForwardWord()
		{
			if (_cursorPos >= _input.Length)
			{
				return;
			}

			// Note: Char.IsLetterOrDigit returns true for most characters where iswalnum returns true in Windows (exactly same result in the ASCII range), but does not apply for all of them
			// bash (GNU readline) and zsh use iswalnum (zsh uses iswalnum only if tested char is a non-ASCII one) to detect if characters can be used as words for your information
			if (!char.IsLetterOrDigit(_input[_cursorPos]))
			{
				_cursorPos++;
				for (; _cursorPos < _input.Length; _cursorPos++)
				{
					if (char.IsLetterOrDigit(_input[_cursorPos]))
					{
						break;
					}
				}
			}

			for (; _cursorPos < _input.Length; _cursorPos++)
			{
				if (!char.IsLetterOrDigit(_input[_cursorPos]))
				{
					break;
				}
			}
		}
		/// <summary>
		/// Moves back to the start of the current or previous word.
		/// Words are composed of letters and digits.
		/// </summary>
		private void BackwardWord()
		{
			if (_cursorPos == 0)
			{
				return;
			}

			char prevChar = _input[_cursorPos - 1];
			if (!char.IsLetterOrDigit(prevChar))
			{
				_cursorPos--;
				for (; _cursorPos > 0; _cursorPos--)
				{
					prevChar = _input[_cursorPos - 1];
					if (char.IsLetterOrDigit(prevChar))
					{
						break;
					}
				}
			}

			for (; _cursorPos > 0; _cursorPos--)
			{
				prevChar = _input[_cursorPos - 1];
				if (!char.IsLetterOrDigit(prevChar))
				{
					break;
				}
			}
		}
		/// <summary>
		/// Deletes the character behind the cursor.
		/// </summary>
		private void BackwardDeleteChar()
		{
			if (_input.Length <= 0 || _cursorPos <= 0)
			{
				return;
			}

			_input = _input.Remove(_cursorPos - 1, 1);
			_cursorPos--;
		}
		/// <summary>
		/// Deletes the character at point.
		/// </summary>
		private void ForwardDeleteChar()
		{
			if (_input.Length <= 0 || _cursorPos >= _input.Length)
			{
				return;
			}

			_input = _input.Remove(_cursorPos, 1);
		}

		/// <summary>
		/// Kills the text from the cursor to the end of the line.
		/// </summary>
		private void KillLine()
		{
			if (_input.Length <= 0 || _cursorPos <= 0)
			{
				return;
			}

			KillText(ref _input, 0, _cursorPos);
			_cursorPos = 0;
		}
		/// <summary>
		/// Kills backward from the cursor to the beginning of the current line.
		/// </summary>
		private void BackwardKillLine()
		{
			if (_input.Length <= 0 || _cursorPos >= _input.Length)
			{
				return;
			}

			KillText(ref _input, _cursorPos, _input.Length - _cursorPos);
		}
		/// <summary>
		/// Kills from point to the end of the current word, or if between words, to the end of the next word.
		/// Word boundaries are the same as <see cref="ForwardWord"/>.
		/// </summary>
		private void KillWord()
		{
			int origCursorPos = _cursorPos;
			ForwardWord();

			if (_cursorPos == origCursorPos)
			{
				return;
			}

			KillText(ref _input, origCursorPos, _cursorPos - origCursorPos);
			_cursorPos = origCursorPos;
		}
		/// <summary>
		/// Kill the word behind the cursor.
		/// Word boundaries are the same as <see cref="BackwardWord"/>.
		/// </summary>
		private void BackwardKillWord()
		{
			int origCursorPos = _cursorPos;
			BackwardWord();

			if (_cursorPos == origCursorPos)
			{
				return;
			}

			KillText(ref _input, _cursorPos, origCursorPos - _cursorPos);
		}
		/// <summary>
		/// Kills the word behind the cursor, using white space as a word boundary.
		/// </summary>
		private void UnixWordRubout()
		{
			if (_cursorPos == 0)
			{
				return;
			}

			int origCursorPos = _cursorPos;

			while (_cursorPos > 0 && IsRegularWhiteSpaceOrTab(_input[_cursorPos - 1]))
			{
				_cursorPos--;
			}


			while (_cursorPos > 0 && !IsRegularWhiteSpaceOrTab(_input[_cursorPos - 1]))
			{
				_cursorPos--;
			}


			KillText(ref _input, _cursorPos, origCursorPos - _cursorPos);

			// yields exactly the same result as a internal "whitespace" function in bash
			static bool IsRegularWhiteSpaceOrTab(char ch) => ch == ' ' || ch == '\t';
		}

		/// <summary>
		/// Drags the character before the cursor forward over the character at the cursor, moving the cursor forward as well.
		/// If the insertion point is at the end of the line, then this transposes the last two characters of the line.
		/// </summary>
		private void TransposeTwoChars()
		{
			int inputLength = _input.Length;
			if (inputLength < 2)
			{
				return;
			}

			if (_cursorPos == 0)
			{
				SwapTwoCharacters(_input, 0);
				_cursorPos = 2;
			}
			else if (_cursorPos < inputLength)
			{
				SwapTwoCharacters(_input, _cursorPos - 1);
				_cursorPos += 1;
			}
			else
			{
				SwapTwoCharacters(_input, _cursorPos - 2);
			}

			void SwapTwoCharacters(string str, int index)
			{
				unsafe
				{
					fixed (char* stringPtr = str)
					{
						char tmp = stringPtr[index];
						stringPtr[index] = stringPtr[index + 1];
						stringPtr[index + 1] = tmp;
					}
				}
			}
		}
		/// <summary>
		/// Drags the word before point past the word after point, moving point past that word as well.
		/// If the insertion point is at the end of the line, this transposes the last two words on the line.
		/// </summary>
		private void TransposeTwoWords()
		{
			if (_input.Length < 3)
			{
				return;
			}

			int origCursorPos = _cursorPos;

			ForwardWord();
			int word2End = _cursorPos;
			BackwardWord();
			int word2Beg = _cursorPos;
			BackwardWord();
			int word1Beg = _cursorPos;
			ForwardWord();
			int word1End = _cursorPos;

			if ((word1Beg == word2Beg) || (word2Beg < word1End))
			{
				_cursorPos = origCursorPos;
				return;
			}

			string word1 = _input.Substring(word1Beg, word1End - word1Beg);
			string word2 = _input.Substring(word2Beg, word2End - word2Beg);

			var stringBuilder = new StringBuilder(_input.Length + Math.Max((word1.Length - word2.Length), 0)); // Prevent reallocation of internal array
			stringBuilder.Append(_input);

			stringBuilder.Remove(word2Beg, word2.Length);
			stringBuilder.Insert(word2Beg, word1);

			stringBuilder.Remove(word1Beg, word1.Length);
			stringBuilder.Insert(word1Beg, word2);

			_input = stringBuilder.ToString();
			_cursorPos = word2End;
		}

		private void KillText(ref string str, int startIndex, int length)
		{
			Clipboard.SetText(str.Substring(startIndex, length));
			str = str.Remove(startIndex, length);
		}

		private void MoveCursorLeft()
		{
			if (_cursorPos > 0)
			{
				_cursorPos--;
			}
		}

		private void MoveCursorRight()
		{
			if (_cursorPos < _input.Length)
			{
				_cursorPos++;
			}
		}

		private void MoveCursorToBegOfLine()
		{
			_cursorPos = 0;
		}

		private void MoveCursorToEndOfLine()
		{
			_cursorPos = _input.Length;
		}

		private void CompileExpression()
		{
			if (string.IsNullOrEmpty(_input) || _compilerTask != null)
			{
				return;
			}

			_commandPos = -1;
			if (_commandHistory.LastOrDefault() != _input)
			{
				_commandHistory.Add(_input);
			}

			_compilerTask = Task.Factory.StartNew(() =>
			{
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

				// With this parameter, you can use natives that require accessible addresses without having to use
				// members of the Marshall class (e.g. SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED)
				compilerOptions.CompilerOptions += " /unsafe";

				foreach (Script script in ScriptDomain.CurrentDomain.RunningScripts.Where(x => x.IsRunning))
				{
					if (System.IO.File.Exists(script.Filename) && System.IO.Path.GetExtension(script.Filename) == ".dll")
					{
						compilerOptions.ReferencedAssemblies.Add(script.Filename);
					}
				}

				const string template =
					"using System; using System.Linq; using System.Drawing; using System.Windows.Forms; using GTA; using GTA.Math; using GTA.Native; " +
					// Define some shortcut variables to simplify commands
					"public sealed class ConsoleInput : ScriptHookVDotNet {{ public static object Execute() {{ var P = Game.LocalPlayerPed; var V = P.CurrentVehicle; {0}; return null; }} }}";

				CompilerResults compilerResult = compiler.CompileAssemblyFromSource(compilerOptions, string.Format(template, _input));

				if (!compilerResult.Errors.HasErrors)
				{
					return compilerResult.CompiledAssembly.GetType("ConsoleInput").GetMethod("Execute");
				}

				PrintError($"Couldn't compile input expression: {_input}");

				var errors = new StringBuilder();

				for (int i = 0; i < compilerResult.Errors.Count; ++i)
				{
					errors.Append("   at line ");
					errors.Append(compilerResult.Errors[i].Line);
					errors.Append(": ");
					errors.Append(compilerResult.Errors[i].ErrorText);

					if (i < compilerResult.Errors.Count - 1)
					{
						errors.AppendLine();
					}
				}

				PrintError(errors.ToString());
				return null;
			});
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}

		private static unsafe void DrawRect(float x, float y, int width, int height, Color color)
		{
			float w = (float)(width) / BaseWidth;
			float h = (float)(height) / BaseHeight;

			NativeFunc.Invoke(0x3A618A217E5154F0ul /* DRAW_RECT */,
				(x / BaseWidth) + w * 0.5f,
				(y / BaseHeight) + h * 0.5f,
				w, h,
				color.R, color.G, color.B, color.A);
		}

		private static unsafe void DrawText(float x, float y, string text, Color color)
		{
			NativeFunc.Invoke(0x66E0276CC5F6B9DA /* SET_TEXT_FONT */, 0); // Chalet London :>
			NativeFunc.Invoke(0x07C837F9A01C34C9 /* SET_TEXT_SCALE */, 0.35f, 0.35f);
			NativeFunc.Invoke(0xBE6B23FFA53FB442 /* SET_TEXT_COLOUR */, color.R, color.G, color.B, color.A);
			NativeFunc.Invoke(0x25FBB336DF1804CB /* BEGIN_TEXT_COMMAND_DISPLAY_TEXT */, NativeMemory.CellEmailBcon);
			NativeFunc.PushLongString(text, 99);
			NativeFunc.Invoke(0xCD015E5BB0D96A57 /* END_TEXT_COMMAND_DISPLAY_TEXT */, (x / BaseWidth), (y / BaseHeight));
		}

		private static unsafe void DisableControlsThisFrame()
		{
			NativeFunc.Invoke(0x5F4B6931816E599B /* DISABLE_ALL_CONTROL_ACTIONS */, 0);

			// LookLeftRight .. LookRightOnly
			for (ulong i = 1; i <= 6; i++)
			{
				NativeFunc.Invoke(0x351220255D64C155 /* ENABLE_CONTROL_ACTION */, 0, i, 0);
			}
		}

		private static unsafe float GetTextLength(string text)
		{
			NativeFunc.Invoke(0x66E0276CC5F6B9DA /* SET_TEXT_FONT */, 0);
			NativeFunc.Invoke(0x07C837F9A01C34C9 /* SET_TEXT_SCALE */, 0.35f, 0.35f);
			NativeFunc.Invoke(0x54CE8AC98E120CAB /* BEGIN_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT */, NativeMemory.CellEmailBcon);
			NativeFunc.PushLongString(text, 98); // 99 byte string chunks don't process properly in END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT
			return *(float*)NativeFunc.Invoke(0x85F061DA64ED2F67 /* END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT */, true);
		}

		private static float GetMarginLength()
		{
			float len1 = GetTextLength("A");
			float len2 = GetTextLength("AA");
			return len1 - (len2 - len1); // [Margin][A] - [A] = [Margin]
		}
	}

	public sealed class ConsoleCommand : Attribute
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
