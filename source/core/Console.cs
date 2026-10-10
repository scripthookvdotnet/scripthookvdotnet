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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SHVDN
{
    public sealed class Console : MarshalByRefObject
    {
        private static class ColorPalette
        {
            public static readonly Color Input = Color.White;
            public static readonly Color InputBusy = Color.DarkGray;
            public static readonly Color Output = Color.White;
            public static readonly Color Prefix = Color.FromArgb(255, 52, 152, 219);
            public static readonly Color Background = Color.FromArgb(200, Color.Black);
            public static readonly Color AltBackground = Color.FromArgb(200, 52, 73, 94);

            public static readonly Color Candidate = Color.White;
            public static readonly Color CandidateSelected = Color.Yellow;

            public static readonly Color Cursor = Color.White;
        }

        // Constants
        private const int BaseWidth = 1280;
        private const int BaseHeight = 720;
        private const int ConsoleWidth = BaseWidth;
        private const int ConsoleHeight = BaseHeight / 3;
        private const int InputHeight = 20;
        private const int LinesPerPage = 16;

        // Cursor
        private int _cursorPos = 0;
        private int _lastCursorPos = 0;
        private int _lastCursorBlinkTick = 0;
        private bool _cursorVisible = false;

        // Input
        private string _input = string.Empty;
        private string _lastInput = string.Empty;
        private string _lastRenderedInput = string.Empty;
        private string _lastRenderedCursorInput = string.Empty;

        private KeyManager _keyManager = new();
        private ConcurrentQueue<Keys> _keyboardEventQueue = new();

        // State
        private int _lastClosedTickCount;
        private bool _shouldBlockControls;
        private volatile bool _isOpen = false;

        private Task<MethodInfo> _compilerTask;
        private Queue<string[]> _outputQueue = new();

        // Commands
        private int _selectedCandidateIndex = -1;
        private bool _hideCandidates = false;

        private Dictionary<string, List<ConsoleCommand>> _commands = new();
        private List<string> _commandCandidates = new();

        // History
        private int _currentPage = 1;
        private int _commandPos = -1;

        private List<string> _lineHistory = new();
        private List<string> _commandHistory; // This must be set via CommandHistory property

        [DllImport("user32.dll")]
        private static extern int ToUnicode(
            uint virtualKeyCode, uint scanCode, byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer, int bufferSize, uint flags);

        public Console()
        {
            // Register Key Actions
            _keyManager.Register(Keys.PageUp, PageUp);
            _keyManager.Register(Keys.PageDown, PageDown);
            _keyManager.Register(Keys.Back, BackwardDeleteChar);
            _keyManager.Register(Keys.Back | Keys.Alt, BackwardKillWord);
            _keyManager.Register(Keys.Delete, ForwardDeleteChar);
            _keyManager.Register(Keys.Left, MoveCursorLeft);
            _keyManager.Register(Keys.Left | Keys.Control, BackwardWord);
            _keyManager.Register(Keys.Right, MoveCursorRight);
            _keyManager.Register(Keys.Right | Keys.Control, ForwardWord);
            _keyManager.Register(Keys.Insert | Keys.Shift, AddClipboardContent);
            _keyManager.Register(Keys.Home, MoveCursorToBegOfLine);
            _keyManager.Register(Keys.End, MoveCursorToEndOfLine);
            _keyManager.Register(Keys.Up, () =>
            {
                if (_hideCandidates || _commandCandidates.Count == 0)
                {
                    GoUpCommandList();
                    return;
                }

                NextCandidate();
            });

            _keyManager.Register(Keys.Down, () =>
            {
                if (_hideCandidates || _commandCandidates.Count == 0)
                {
                    GoDownCommandList();
                    return;
                }

                PreviousCandidate();
            });

            _keyManager.Register(Keys.Enter, CompileExpression);

            _keyManager.Register(Keys.Escape, () =>
            {
                if (_hideCandidates || _commandCandidates.Count == 0)
                {
                    Close();
                    return;
                }

                _hideCandidates = true;
            });

            _keyManager.Register(Keys.Tab, CompleteCandidate);

            _keyManager.Register(Keys.A | Keys.Control, MoveCursorToBegOfLine);
            _keyManager.Register(Keys.B | Keys.Control, MoveCursorLeft);
            _keyManager.Register(Keys.B | Keys.Alt, MoveCursorRight);
            _keyManager.Register(Keys.D | Keys.Alt, KillWord);
            _keyManager.Register(Keys.D | Keys.Control, ForwardDeleteChar);
            _keyManager.Register(Keys.E | Keys.Control, MoveCursorToEndOfLine);
            _keyManager.Register(Keys.F | Keys.Control, MoveCursorRight);
            _keyManager.Register(Keys.F | Keys.Alt, ForwardWord);
            _keyManager.Register(Keys.H | Keys.Control, BackwardDeleteChar);
            _keyManager.Register(Keys.P | Keys.Control, GoUpCommandList);
            _keyManager.Register(Keys.N | Keys.Control, GoDownCommandList);
            _keyManager.Register(Keys.K | Keys.Control, BackwardKillLine);
            _keyManager.Register(Keys.U | Keys.Control, KillLine);
            _keyManager.Register(Keys.M | Keys.Control, CompileExpression);
            _keyManager.Register(Keys.L | Keys.Control, Clear);
            _keyManager.Register(Keys.V | Keys.Control, AddClipboardContent);
            _keyManager.Register(Keys.W | Keys.Control, UnixWordRubout);
            _keyManager.Register(Keys.T | Keys.Alt, TransposeTwoWords);
            _keyManager.Register(Keys.T | Keys.Control, TransposeTwoChars);
        }

        /// <summary>
        /// Gets or sets whether the console is open.
        /// </summary>
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                DisableControlsThisFrame();

                _isOpen = value;
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
        /// Add colored text lines to the console. This call is thread-safe.
        /// </summary>
        /// <param name="prefix">The prefix for each line.</param>
        /// <param name="messages">The lines to add to the console.</param>
        /// <param name="color">The color of those lines.</param>
        private void AddLines(string prefix, string[] messages, string color = "~w~")
        {
            // Add proper styling
            for (int i = 0; i < messages.Length; i++)
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
            string text = GetClipboardContent();
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
        /// Writes a message to the console.
        /// </summary>
        public void Print(Log.Level level, string msg, params object[] args)
        {
            const string DebugMessageHeaderStr = "[~b~DEBUG~w~]";
            const string InfoMessageHeaderStr = "[~b~INFO~w~]";
            const string ErrorMessageHeaderStr = "[~r~ERROR~w~]";
            const string WarningMessageHeaderStr = "[~o~WARNING~w~]";

            string header;

            switch (level)
            {
                case Log.Level.Debug:
                    header = DebugMessageHeaderStr;
                    break;

                case Log.Level.Info:
                    header = InfoMessageHeaderStr;
                    break;

                case Log.Level.Warning:
                    header = WarningMessageHeaderStr;
                    break;

                case Log.Level.Error:
                    header = ErrorMessageHeaderStr;
                    break;

                default:
                    return;
            }

            if (args != null && args.Length > 0)
            {
                msg = String.Format(msg, args);
            }

            AddLines(header + " ", msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Writes the help text for all commands to the console.
        /// </summary>
        public void PrintHelpText()
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

            Print(Log.Level.Info, help.ToString());
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

                    Print(Log.Level.Info, command.Name + ": " + command.Help);
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
                            Print(Log.Level.Info, $"[Return Value]: {result}");
                        }
                    }
                    catch (TargetInvocationException ex)
                    {
                        Print(Log.Level.Error, $"[Exception]: {ex.InnerException.ToString()}");
                    }
                }

                ClearInput();

                // Reset compiler task
                _compilerTask = null;
            }

            if (_keyboardEventQueue.TryDequeue(out Keys keys))
            {
                if (!_keyManager.TryHandle(keys))
                {
                    var e = new KeyEventArgs(keys);

                    var buf = new StringBuilder(256);
                    byte[] keyboardState = new byte[256];
                    keyboardState[(int)Keys.Menu] = e.Alt ? (byte)0xff : (byte)0;
                    keyboardState[(int)Keys.ShiftKey] = e.Shift ? (byte)0xff : (byte)0;
                    keyboardState[(int)Keys.ControlKey] = e.Control ? (byte)0xff : (byte)0;

                    // Translate key event to character for text input
                    ToUnicode((uint)e.KeyCode, 0, keyboardState, buf, 256, 0);
                    AddToInput(buf.ToString());

                    // We only want to show candidates again if the actual input has changed.
                    _hideCandidates = false;
                }
            }

            // Add lines from concurrent queue to history
            if (_outputQueue.Count > 0)
            {
                string[] lines = _outputQueue.Dequeue();

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

            if (_input != _lastInput)
            {
                _lastInput = _input;
                _lastRenderedInput = TextFormatting.EscapeTokens(_input);

                _lastCursorPos = -1;

                UpdateCommandCandidates();
            }

            if (_cursorPos != _lastCursorPos)
            {
                _lastCursorPos = _cursorPos;
                _lastRenderedCursorInput = TextFormatting.EscapeTokens(_input.Substring(0, _cursorPos));

                ResetCursorBlinking();
            }

            // Draw background
            DrawRect(0, 0, ConsoleWidth, ConsoleHeight, ColorPalette.Background);

            // Draw input field
            DrawRect(0, ConsoleHeight, ConsoleWidth, InputHeight, ColorPalette.AltBackground);
            DrawRect(0, ConsoleHeight + InputHeight, 80, InputHeight, ColorPalette.AltBackground);

            // Draw input prefix
            DrawText(0, ConsoleHeight, "$>", ColorPalette.Prefix);

            // Draw input text
            DrawText(25, ConsoleHeight, _lastRenderedInput, _compilerTask == null ? ColorPalette.Input : ColorPalette.InputBusy);

            // Draw page information
            DrawText(5, ConsoleHeight + InputHeight, "Page " + _currentPage + "/" + System.Math.Max(1, (_lineHistory.Count + (LinesPerPage - 1)) / LinesPerPage), ColorPalette.Input);

            if (_lastCursorBlinkTick + 500 < nowTickCount)
            {
                _cursorVisible = !_cursorVisible;
                _lastCursorBlinkTick = nowTickCount;
            }

            // Draw blinking cursor
            if (_cursorVisible)
            {
                float lengthBetweenInputStartAndCursor = TextFormatting.GetTextLength(_lastRenderedCursorInput) - TextFormatting.GetMarginLength();

                DrawRect(26 + (lengthBetweenInputStartAndCursor * ConsoleWidth), ConsoleHeight + 2, 2, InputHeight - 4, ColorPalette.Cursor);
            }

            // Draw console history text
            int historyOffset = _lineHistory.Count - (LinesPerPage * _currentPage);
            int historyLength = historyOffset + LinesPerPage;
            for (int i = System.Math.Max(0, historyOffset); i < historyLength; ++i)
            {
                DrawText(2, (float)((i - historyOffset) * 14), _lineHistory[i], ColorPalette.Output);
            }

            // Draw command candidates
            if (!_hideCandidates && _commandCandidates.Count > 0)
            {
                for (int i = 0; i < _commandCandidates.Count && i < 5; i++)
                {
                    var color = (i == _selectedCandidateIndex) ? ColorPalette.CandidateSelected : ColorPalette.Candidate;
                    DrawText(25, ConsoleHeight + InputHeight + 16 + i * 16, _commandCandidates[i], color);
                }
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

            _keyboardEventQueue.Enqueue(keys);
        }

        private void NextCandidate()
        {
            _selectedCandidateIndex = (_selectedCandidateIndex - 1 + _commandCandidates.Count) % (_commandCandidates.Count == 0 ? 1 : _commandCandidates.Count);
        }

        private void PreviousCandidate()
        {
            _selectedCandidateIndex = (_selectedCandidateIndex + 1) % (_commandCandidates.Count == 0 ? 1 : _commandCandidates.Count);
        }

        private void CompleteCandidate()
        {
            if (_commandCandidates.Count > 0)
            {
                if (_selectedCandidateIndex >= 0 && _selectedCandidateIndex < _commandCandidates.Count)
                {
                    string candidate = _commandCandidates[_selectedCandidateIndex];
                    int parenIndex = candidate.IndexOf('(');
                    int closeParenIndex = candidate.IndexOf(')');
                    bool hasParams = closeParenIndex - parenIndex > 1;

                    if (parenIndex >= 0)
                    {
                        if (hasParams)
                        {
                            string cmdName = candidate.Substring(0, parenIndex);
                            _input = $"{cmdName}(\"\")";
                            _cursorPos = cmdName.Length + 2;
                        }
                        else
                        {
                            _input = candidate.Substring(0, closeParenIndex + 1);
                            _cursorPos = _input.Length;
                        }
                    }
                    else
                    {
                        _input = candidate;
                        _cursorPos = _input.Length;
                    }
                }
                else if (_commandCandidates.Count > 1)
                {
                    var names = _commandCandidates
                        .Select(c => {
                            int idx = c.IndexOf('(');
                            return idx > 0 ? c.Substring(0, idx) : c;
                        })
                        .ToList();

                    string prefix = TextFormatting.LongestCommonPrefix(names, _input);

                    if (prefix.Length > _input.Length)
                    {
                        _input = prefix;
                        _cursorPos = _input.Length;
                    }
                }
            }
        }

        private void Close()
        {
            IsOpen = false;
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

            // Note: Char.IsLetterOrDigit returns true for most characters where `iswalnum` returns true in Windows
            // (exactly same result in the ASCII range), but does not apply for all of them.
            // bash (GNU readline) and zsh use `iswalnum` (zsh uses iswalnum only if tested char is a non-ASCII one)
            // to detect if characters can be used as words for your information.
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
            if (_input.Length < 2)
            {
                return;
            }

            if (_cursorPos == 0)
            {
                _input = SwapTwoCharacters(_input, 0);
                _cursorPos = 2;
            }
            else if (_cursorPos < _input.Length)
            {
                _input = SwapTwoCharacters(_input, _cursorPos - 1);
                _cursorPos += 1;
            }
            else
            {
                _input = SwapTwoCharacters(_input, _cursorPos - 2);
            }

            string SwapTwoCharacters(string str, int index)
            {
                StringBuilder sb = new StringBuilder(str);
                char tmp = sb[index];
                sb[index] = sb[index + 1];
                sb[index + 1] = tmp;

                return sb.ToString();
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

            // Prevent reallocation of internal array
            var stringBuilder = new StringBuilder(_input.Length + Math.Max(word1.Length - word2.Length, 0));
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
            SetClipboardContent(str.Substring(startIndex, length));
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
            // We need to capture the input for the task action below, so it won't read a stale reference of
            // the console input
            string capturedInput = _input;

            if (string.IsNullOrEmpty(capturedInput) || _compilerTask != null)
            {
                return;
            }

            _commandPos = -1;
            if (_commandHistory.LastOrDefault() != capturedInput)
            {
                _commandHistory.Add(capturedInput);
            }

            Task<MethodInfo> newCompilerTask = Task.Factory.StartNew(() =>
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

                const string Template =
                    "using System; using System.Linq; using System.Drawing; using System.Windows.Forms; using GTA; using GTA.Math; using GTA.Native; " +
                    // Define some shortcut variables to simplify commands
                    "public sealed class ConsoleInput : ScriptHookVDotNet {{ public static object Execute() {{ var P = Game.LocalPlayerPed; var V = P.CurrentVehicle; {0}; return null; }} }}";

                CompilerResults compilerResult = compiler.CompileAssemblyFromSource(compilerOptions, string.Format(Template, _input));

                if (!compilerResult.Errors.HasErrors)
                {
                    return compilerResult.CompiledAssembly.GetType("ConsoleInput").GetMethod("Execute");
                }

                var errors = new StringBuilder();

                errors.AppendLine($"Couldn't compile input expression: {TextFormatting.EscapeTokens(capturedInput)}");

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

                Print(Log.Level.Error, errors.ToString());
                return null;
            });

            _compilerTask = newCompilerTask;
        }

        private void UpdateCommandCandidates()
        {
            _commandCandidates.Clear();
            _selectedCandidateIndex = -1;
            if (string.IsNullOrWhiteSpace(_input))
            {
                return;
            }

            foreach (var space in _commands.Keys)
            {
                foreach (var cmd in _commands[space])
                {
                    var paramList = cmd.MethodInfo.GetParameters();
                    string paramStr = string.Join(", ", paramList.Select(p => p.ParameterType.Name + " " + p.Name));
                    string displayName = $"{cmd.Name}({paramStr})";

                    if (displayName.Contains(_input))
                    {
                        _commandCandidates.Add(displayName);
                    }
                }
            }

            if (_commandCandidates.Count > 0)
            {
                _selectedCandidateIndex = 0;
            }
        }

        private void ResetCursorBlinking()
        {
            _lastCursorBlinkTick = 0;

            // Because we set _lastCursorBlinkTick to 0; the state will be flipped next tick.
            _cursorVisible = false;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public string GetClipboardContent()
        {
            string output = string.Empty;
            Exception threadEx = null;

            var t = new Thread(() =>
            {
                try
                {
                    output = Clipboard.GetText();
                }
                catch (Exception ex)
                {
                    threadEx = ex;
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            if (threadEx != null)
            {
                return string.Empty;
            }

            return output;
        }

        public void SetClipboardContent(string str)
        {
            var t = new Thread(() =>
            {
                Clipboard.SetText(str);
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }

        private static unsafe void DrawRect(float x, float y, int width, int height, Color color)
        {
            float w = (float)width / BaseWidth;
            float h = (float)height / BaseHeight;

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
            NativeFunc.Invoke(0xCD015E5BB0D96A57 /* END_TEXT_COMMAND_DISPLAY_TEXT */, x / BaseWidth, y / BaseHeight);
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
    }

    public static class TextFormatting
    {
        public static string EscapeTokens(string str)
        {
            StringBuilder sb = new();

            int start = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '~')
                {
                    sb.Append(str.Substring(start, i - start));

                    sb.Append('\\');
                    sb.Append(str[i]);

                    start = i + 1;
                }
            }

            sb.Append(str.Substring(start));

            return sb.ToString();
        }

        public static unsafe float GetTextLength(string text)
        {
            NativeFunc.Invoke(0x66E0276CC5F6B9DA /* SET_TEXT_FONT */, 0);
            NativeFunc.Invoke(0x07C837F9A01C34C9 /* SET_TEXT_SCALE */, 0.35f, 0.35f);
            NativeFunc.Invoke(0x54CE8AC98E120CAB /* BEGIN_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT */, NativeMemory.CellEmailBcon);
            NativeFunc.PushLongString(text, 98); // 99 byte string chunks don't process properly in END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT
            return *(float*)NativeFunc.Invoke(0x85F061DA64ED2F67 /* END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT */, true);
        }

        public static float GetMarginLength()
        {
            float len1 = GetTextLength("A");
            float len2 = GetTextLength("AA");
            return len1 - (len2 - len1); // [Margin][A] - [A] = [Margin]
        }

        public static string LongestCommonPrefix(List<string> strs, string input)
        {
            if (strs == null || strs.Count == 0)
            {
                return input;
            }

            string prefix = strs[0];
            for (int i = 1; i < strs.Count; i++)
            {
                int j = 0;
                while (j < prefix.Length && j < strs[i].Length && prefix[j] == strs[i][j])
                {
                    j++;
                }

                prefix = prefix.Substring(0, j);
                if (prefix.Length == input.Length)
                {
                    break;
                }
            }

            return prefix;
        }
    }

    public sealed class ConsoleCommand : Attribute
    {
        public ConsoleCommand()
            : this(string.Empty)
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

    public class KeyManager
    {
        public class KeyBinding
        {
            public Keys Combined { get; }
            public Action Action { get; }

            public KeyBinding(Keys combined, Action action)
            {
                Combined = combined;
                Action = action;
            }
        }

        private readonly Dictionary<Keys, List<KeyBinding>> _bindings = new();

        private const Keys KeyCodeMask = (Keys)0xFF;
        private const Keys ModMask = Keys.Control | Keys.Shift | Keys.Alt;

        public void Register(Keys combined, Action action)
        {
            var key = combined & KeyCodeMask;

            if (!_bindings.TryGetValue(key, out var list))
            {
                list = new List<KeyBinding>();
                _bindings[key] = list;
            }

            list.Add(new KeyBinding(combined, action));
        }

        public bool TryHandle(Keys input)
        {
            var key = input & KeyCodeMask;
            var mods = input & ModMask;

            if (!_bindings.TryGetValue(key, out var list))
            {
                return false;
            }

            KeyBinding best = null;
            int bestScore = -1;

            foreach (var binding in list)
            {
                var requiredMods = binding.Combined & ModMask;

                if ((mods & requiredMods) != requiredMods)
                {
                    continue;
                }

                int score = CountBits((uint)requiredMods);

                if (score > bestScore)
                {
                    bestScore = score;
                    best = binding;
                }
            }

            if (best == null)
            {
                return false;
            }

            best.Action();
            return true;
        }

        private static int CountBits(uint value)
        {
            int count = 0;
            while (value != 0)
            {
                value &= value - 1; // clear lowest set bit
                count++;
            }

            return count;
        }
    }
}
