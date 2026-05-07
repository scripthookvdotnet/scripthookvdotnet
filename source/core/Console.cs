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
        private int _selectionAnchor = -1;
        private float _lengthUntilSelectionStart = -1f;
        private float _lengthUntilSelectionEnd = -1f;
        private int _cursorPos = 0;
        private int _lastCursorBlinkTick = 0;
        private bool _cursorVisible = false;
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
        private List<string> _commandCandidates = new();
        private bool _hideCandidates = false;
        private int _selectedCandidateIndex = -1;

        // We need a lock because tick calls and keyboard events are fired on different threads, even if we don't use
        // a dedicated thread in order to avoid a fiber from SHV
        private readonly object _lock = new();

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
        private static readonly Color s_selectionColor = Color.FromArgb(200, 78, 165, 217);

        [DllImport("user32.dll")]
        private static extern int ToUnicode(
            uint virtualKeyCode, uint scanCode, byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer, int bufferSize, uint flags);

        private KeyManager _keyManager = new();


        public Console()
        {
            // Register Key Actions
            _keyManager.Register(Keys.PageUp, PageUp);
            _keyManager.Register(Keys.PageDown, PageDown);
            _keyManager.Register(Keys.Back, BackwardDeleteChar);
            _keyManager.Register(Keys.Back | Keys.Alt, BackwardKillWord);

            _keyManager.Register(Keys.Delete, () =>
            {
                if (HasSelection)
                {
                    DeleteSelection();
                    return;
                }

                ForwardDeleteChar();
            });

            _keyManager.Register(Keys.Left, () => MoveCursorLeft(false));
            _keyManager.Register(Keys.Left | Keys.Control, () => BackwardWord(false));

            _keyManager.Register(Keys.Left | Keys.Shift, () => MoveCursorLeft(true));
            _keyManager.Register(Keys.Left | Keys.Control | Keys.Shift, () => BackwardWord(true));

            _keyManager.Register(Keys.Right, () => MoveCursorRight(false));
            _keyManager.Register(Keys.Right | Keys.Control, () => ForwardWord(false));

            _keyManager.Register(Keys.Right | Keys.Shift, () => MoveCursorRight(true));
            _keyManager.Register(Keys.Right | Keys.Control | Keys.Shift, () => ForwardWord(true));

            _keyManager.Register(Keys.Insert | Keys.Shift, AddClipboardContent);

            _keyManager.Register(Keys.Home, () => MoveCursorToBegOfLine(false));
            _keyManager.Register(Keys.Home | Keys.Shift, () => MoveCursorToBegOfLine(true));

            _keyManager.Register(Keys.End, () => MoveCursorToEndOfLine(false));
            _keyManager.Register(Keys.End | Keys.Shift, () => MoveCursorToEndOfLine(true));

            _keyManager.Register(Keys.Up, () =>
            {
                lock (_lock)
                {
                    if (_hideCandidates || _commandCandidates.Count == 0)
                    {
                        GoUpCommandList();
                        return;
                    }

                    NextCandidate();
                }
            });

            _keyManager.Register(Keys.Down, () =>
            {
                lock (_lock)
                {
                    if (_hideCandidates || _commandCandidates.Count == 0)
                    {
                        GoDownCommandList();
                        return;
                    }

                    PreviousCandidate();
                }
            });

            _keyManager.Register(Keys.Enter, CompileExpression);

            _keyManager.Register(Keys.Escape, () =>
            {
                lock (_lock)
                {
                    if (_hideCandidates || _commandCandidates.Count == 0)
                    {
                        Close();
                        return;
                    }

                    _hideCandidates = true;
                }          
            });

            _keyManager.Register(Keys.Tab, CompleteCandidate);

            _keyManager.Register(Keys.A | Keys.Control, SelectAll);
            _keyManager.Register(Keys.B | Keys.Control, () => MoveCursorLeft(false));
            _keyManager.Register(Keys.B | Keys.Alt, () => MoveCursorRight(false));
            _keyManager.Register(Keys.C | Keys.Control, CopySelection);
            _keyManager.Register(Keys.D | Keys.Alt, KillWord);

            _keyManager.Register(Keys.D | Keys.Control, () => {
                if (HasSelection)
                {
                    DeleteSelection();
                    return;
                }

                ForwardDeleteChar();
            });

            _keyManager.Register(Keys.E | Keys.Control, () => MoveCursorToEndOfLine(false));
            _keyManager.Register(Keys.F | Keys.Control, () => MoveCursorRight(false));
            _keyManager.Register(Keys.F | Keys.Alt, () => ForwardWord(false));

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
            get
            {
                lock (_lock)
                {
                    return _isOpen;
                }
            }
            set
            {
                DisableControlsThisFrame();

                lock (_lock)
                {
                    _isOpen = value;
                    if (_isOpen)
                    {
                        return;
                    }

                    _lastClosedTickCount = Environment.TickCount + 200; // Hack so the input gets blocked long enough
                    _shouldBlockControls = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the command history. This is used to avoid losing the command history on SHVDN reloading.
        /// </summary>
        public List<string> CommandHistory
        {
            get
            {
                lock (_lock)
                {
                    return _commandHistory;
                }
            }
            set
            {
                lock (_lock)
                {
                    _commandHistory = value;
                }
            }
        }

        /// <summary>
        /// Register the specified method as a console command.
        /// </summary>
        /// <param name="command">The command attribute of the method.</param>
        /// <param name="methodInfo">The method information.</param>
        public void RegisterCommand(ConsoleCommand command, MethodInfo methodInfo)
        {
            command.MethodInfo = methodInfo;

            lock (_lock)
            {
                if (!_commands.ContainsKey(command.Namespace))
                {
                    _commands[command.Namespace] = new List<ConsoleCommand>();
                }

                _commands[command.Namespace].Add(command);
            }
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

                lock (_lock)
                {
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

            lock (_lock)
            {
                if (HasSelection)
                {
                    DeleteSelection();
                }

                _input = _input.Insert(_cursorPos, text);
                _cursorPos += text.Length;
                UpdateCommandCandidates();
            }

            ClearSelection();
            ResetCursorBlinking();
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
            lock (_lock)
            {
                _input = string.Empty;
                _cursorPos = 0;
            }

            ResetCursorBlinking();
        }
        /// <summary>
        /// Clears the console output.
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _lineHistory.Clear();
                _currentPage = 1;
            }
        }

        /// <summary>
        /// Writes a message to the console.
        /// </summary>
        /// <param name="msg">The composite format string.</param>
        public void PrintMessage(string headerStr, string msg)
        {
            AddLines(headerStr + " ", msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }
        /// <summary>
        /// Writes a message to the console.
        /// </summary>
        /// <param name="msg">The composite format string.</param>
        /// <param name="args">The formatting arguments.</param>
        public void PrintMessage(string headerStr, string msg, params object[] args)
        {
            if (args.Length > 0)
            {
                msg = String.Format(msg, args);
            }

            AddLines(headerStr + " ", msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }

        const string DebugMessageHeaderStr = "[~b~DEBUG~w~]";
        const string InfoMessageHeaderStr = "[~b~INFO~w~]";
        const string ErrorMessageHeaderStr = "[~r~ERROR~w~]";
        const string WarningMessageHeaderStr = "[~o~WARNING~w~]";

        /// <summary>
        /// Writes a debug message to the console.
        /// </summary>
        /// <param name="msg">The composite format string.</param>
        public void PrintDebug(string msg) => PrintMessage(DebugMessageHeaderStr, msg);
        /// <summary>
        /// Writes a debug message to the console.
        /// </summary>
        /// <param name="msg">The composite format string.</param>
        /// <param name="args">The formatting arguments.</param>
        public void PrintDebug(string msg, params object[] args) => PrintMessage(DebugMessageHeaderStr, msg, args);
        /// <summary>
        /// Writes an info message to the console.
        /// </summary>
        /// <param name="msg">The composite format string.</param>
        public void PrintInfo(string msg) => PrintMessage(InfoMessageHeaderStr, msg);
        /// <summary>
        /// Writes an info message to the console.
        /// </summary>
        /// <param name="msg">The composite format string.</param>
        /// <param name="args">The formatting arguments.</param>
        public void PrintInfo(string msg, params object[] args) => PrintMessage(InfoMessageHeaderStr, msg, args);
        /// <summary>
        /// Writes an error message to the console.
        /// </summary>
        /// <param name="msg">The composite format string.</param>
        public void PrintError(string msg) => PrintMessage(ErrorMessageHeaderStr, msg);
        /// <summary>
        /// Writes an error message to the console.
        /// </summary>
        /// <param name="msg">The composite format string.</param>
        /// <param name="args">The formatting arguments.</param>
        public void PrintError(string msg, params object[] args) => PrintMessage(ErrorMessageHeaderStr, msg, args);
        /// <summary>
        /// Writes a warning message to the console.
        /// </summary>
        /// <param name="msg">The composite format string.</param>
        public void PrintWarning(string msg) => PrintMessage(WarningMessageHeaderStr, msg);
        /// <summary>
        /// Writes a warning message to the console.
        /// </summary>
        /// <param name="msg">The composite format string.</param>
        /// <param name="args">The formatting arguments.</param>
        public void PrintWarning(string msg, params object[] args) => PrintMessage(WarningMessageHeaderStr, msg, args);

        /// <summary>
        /// Writes the help text for all commands to the console.
        /// </summary>
        public void PrintHelpText()
        {
            var help = new StringBuilder();
            lock (_lock)
            {
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
            }

            PrintInfo(help.ToString());
        }
        /// <summary>
        /// Writes the help text for the specified command to the console.
        /// </summary>
        /// <param name="commandName">The command name to check.</param>
        internal void PrintHelpText(string commandName)
        {
            lock (_lock)
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
        }

        /// <summary>
        /// Main execution logic of the console.
        /// </summary>
        internal void DoTick()
        {
            int nowTickCount = Environment.TickCount;

            // Execute compiled input line script
            Task<MethodInfo> compilerTask = null;
            lock (_lock)
            {
                compilerTask = _compilerTask;
            }
            if (compilerTask != null && compilerTask.IsCompleted)
            {
                if (compilerTask.Result != null)
                {
                    try
                    {
                        object result = compilerTask.Result.Invoke(null, null);
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
                lock (_lock)
                {
                    _compilerTask = null;
                }
                compilerTask = null;
            }

            // Add lines from concurrent queue to history
            if (_outputQueue.TryDequeue(out string[] lines))
            {
                lock (_lock)
                {
                    foreach (string line in lines)
                    {
                        _lineHistory.Add(line);
                    }
                }
            }

            if (!IsOpen)
            {
                int lastClosedTickCount = 0;
                bool shouldBlockControls = false;
                lock (_lock)
                {
                    lastClosedTickCount = _lastClosedTickCount;
                    shouldBlockControls = _shouldBlockControls;
                }
                // Hack so the input gets blocked long enough
                if ((lastClosedTickCount - nowTickCount) > 0)
                {
                    if (shouldBlockControls)
                    {
                        DisableControlsThisFrame();
                    }
                }
                // The console is not open for more than about 24.9 days, calculating the elapsed time with 2 int tick count vars doesn't do the job
                else if (shouldBlockControls)
                {
                    shouldBlockControls = false;
                }
                return; // Nothing more to do here when the console is not open
            }

            // Disable controls while the console is open
            DisableControlsThisFrame();

            string currInput = null;
            int currLineHistCount = 0;
            int currPage = 0;
            int selectionAnchor;
            int cursorPos;
            lock (_lock)
            {
                currInput = _input;
                currLineHistCount = _lineHistory.Count;
                currPage = _currentPage;
                selectionAnchor = _selectionAnchor;
                cursorPos = _cursorPos;
            }

            // Draw background
            DrawRect(0, 0, ConsoleWidth, ConsoleHeight, s_backgroundColor);
            // Draw input field
            DrawRect(0, ConsoleHeight, ConsoleWidth, InputHeight, s_altBackgroundColor);
            DrawRect(0, ConsoleHeight + InputHeight, 80, InputHeight, s_altBackgroundColor);
            // Draw selection
            if (HasSelection)
            {
                (int selectionStart, int selectionEnd) = MinMax(selectionAnchor, cursorPos);

                int x1 = (int)Math.Round(_lengthUntilSelectionStart * ConsoleWidth);
                int x2 = (int)Math.Round(_lengthUntilSelectionEnd * ConsoleWidth);

                int width = Math.Max(0, x2 - x1);

                DrawRect(26 + x1, ConsoleHeight + 2, width, InputHeight - 4, s_selectionColor);
            }
            // Draw input prefix
            DrawText(0, ConsoleHeight, "$>", s_prefixColor);
            // Draw input text
            DrawText(25, ConsoleHeight, currInput, compilerTask == null ? s_inputColor : s_inputColorBusy);
            // Draw page information
            DrawText(5, ConsoleHeight + InputHeight, "Page " + currPage + "/" + System.Math.Max(1, ((currLineHistCount + (LinesPerPage - 1)) / LinesPerPage)), s_inputColor);

            lock (_lock)
            {
                if(_lastCursorBlinkTick + 500 < nowTickCount)
                {
                    _cursorVisible = !_cursorVisible;
                    _lastCursorBlinkTick = nowTickCount;
                }

                // Draw blinking cursor
                if (_cursorVisible)
                {
                    float lengthBetweenInputStartAndCursor = GetTextLength(currInput.Substring(0, _cursorPos)) - GetMarginLength();
                    DrawRect(26 + (lengthBetweenInputStartAndCursor * ConsoleWidth), ConsoleHeight + 2, 2, InputHeight - 4, Color.White);
                }

                // Draw console history text
                int historyOffset = currLineHistCount - (LinesPerPage * currPage);
                int historyLength = historyOffset + LinesPerPage;
                for (int i = System.Math.Max(0, historyOffset); i < historyLength; ++i)
                {
                    DrawText(2, (float)((i - historyOffset) * 14), _lineHistory[i], s_outputColor);
                }

                // Draw command candidates
                if (!_hideCandidates && _commandCandidates.Count > 0)
                {
                    for (int i = 0; i < _commandCandidates.Count && i < 5; i++)
                    {
                        var color = (i == _selectedCandidateIndex) ? Color.Yellow : Color.White;
                        DrawText(25, ConsoleHeight + InputHeight + 16 + i * 16, _commandCandidates[i], color);
                    }
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

                //We only want to show candidates again if the actual input has changed.
                lock (_lock)
                {
                    _hideCandidates = false;
                }
            }
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
                lock (_lock)
                {
                    if (_selectedCandidateIndex >= 0 && _selectedCandidateIndex < _commandCandidates.Count)
                    {
                        string candidate = _commandCandidates[_selectedCandidateIndex];
                        int parenIndex = candidate.IndexOf('(');
                        int closeParenIndex = candidate.IndexOf(')');
                        bool hasParams = (closeParenIndex - parenIndex > 1);

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
                            UpdateCommandCandidates();
                            ResetCursorBlinking();
                            ClearSelection();
                        }
                        else
                        {
                            _input = candidate;
                            _cursorPos = _input.Length;
                            UpdateCommandCandidates();
                            ResetCursorBlinking();
                            ClearSelection();
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

                        string prefix = LongestCommonPrefix(names, _input);

                        if (prefix.Length > _input.Length)
                        {
                            _input = prefix;
                            _cursorPos = _input.Length;
                            UpdateCommandCandidates();
                            ResetCursorBlinking();
                            ClearSelection();
                        }
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
            lock (_lock)
            {
                if (_currentPage < ((_lineHistory.Count + LinesPerPage - 1) / LinesPerPage))
                {
                    _currentPage++;
                }
            }
        }

        private void PageDown()
        {
            lock (_lock)
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                }
            }
        }

        private void GoUpCommandList()
        {
            lock (_lock)
            {
                if (_commandHistory.Count == 0 || _commandPos >= _commandHistory.Count - 1)
                {
                    return;
                }

                _commandPos++;
                _input = _commandHistory[_commandHistory.Count - _commandPos - 1];
                // Reset cursor position to end of input text
                _cursorPos = _input.Length;
                UpdateCommandCandidates();
                ResetCursorBlinking();
                ClearSelection();
            }
        }

        private void GoDownCommandList()
        {
            lock (_lock)
            {
                if (_commandHistory.Count == 0 || _commandPos <= 0)
                {
                    return;
                }

                _commandPos--;
                _input = _commandHistory[_commandHistory.Count - _commandPos - 1];
                _cursorPos = _input.Length;

                UpdateCommandCandidates();
                ResetCursorBlinking();
                ClearSelection();
            }
        }

        /// <summary>
        /// Moves to the end of the next word, just like emacs and GNU readline (does not move to the beginning of the next word like zsh does for forward-word).
        /// Words are composed of letters and digits.
        /// </summary>
        private void ForwardWord(bool moveSelection = false)
        {
            int oldPosition;

            lock (_lock)
            {
                if (_cursorPos >= _input.Length)
                {
                    return;
                }

                oldPosition = _cursorPos;

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

            UpdateSelection(oldPosition, moveSelection);
            ResetCursorBlinking();
        }
        /// <summary>
        /// Moves back to the start of the current or previous word.
        /// Words are composed of letters and digits.
        /// </summary>
        private void BackwardWord(bool moveSelection = false)
        {
            int oldPosition;

            lock (_lock)
            {
                if (_cursorPos == 0)
                {
                    return;
                }

                oldPosition = _cursorPos;

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

            UpdateSelection(oldPosition, moveSelection);
            ResetCursorBlinking();
        }
        /// <summary>
        /// Deletes the character behind the cursor.
        /// </summary>
        private void BackwardDeleteChar()
        {
            if (HasSelection)
            {
                DeleteSelection();
                ResetCursorBlinking();
                return;
            }

            lock (_lock)
            {
                if (_input.Length <= 0 || _cursorPos <= 0)
                {
                    return;
                }

                _input = _input.Remove(_cursorPos - 1, 1);
                _cursorPos--;
                UpdateCommandCandidates();
                ResetCursorBlinking();
                ClearSelection();
            }
        }
        /// <summary>
        /// Deletes the character at point.
        /// </summary>
        private void ForwardDeleteChar()
        {
            lock (_lock)
            {
                if (_input.Length <= 0 || _cursorPos >= _input.Length)
                {
                    return;
                }

                _input = _input.Remove(_cursorPos, 1);
                UpdateCommandCandidates();
                ClearSelection();
            }
        }

        /// <summary>
        /// Kills the text from the cursor to the end of the line.
        /// </summary>
        private void KillLine()
        {
            lock (_lock)
            {
                if (_input.Length <= 0 || _cursorPos <= 0)
                {
                    return;
                }

                KillText(ref _input, 0, _cursorPos);
                _cursorPos = 0;
            }

            ResetCursorBlinking();
        }
        /// <summary>
        /// Kills backward from the cursor to the beginning of the current line.
        /// </summary>
        private void BackwardKillLine()
        {
            lock (_lock)
            {
                if (_input.Length <= 0 || _cursorPos >= _input.Length)
                {
                    return;
                }

                KillText(ref _input, _cursorPos, _input.Length - _cursorPos);
            }
        }
        /// <summary>
        /// Kills from point to the end of the current word, or if between words, to the end of the next word.
        /// Word boundaries are the same as <see cref="ForwardWord"/>.
        /// </summary>
        private void KillWord()
        {
            int origCursorPos = 0;
            lock (_lock)
            {
                origCursorPos = _cursorPos;
            }

            ForwardWord();
            int currCursorPos = 0;
            lock (_lock)
            {
                currCursorPos = _cursorPos;
            }

            if (currCursorPos == origCursorPos)
            {
                return;
            }

            lock (_lock)
            {
                KillText(ref _input, origCursorPos, currCursorPos - origCursorPos);
                _cursorPos = origCursorPos;
            }

            ResetCursorBlinking();
        }
        /// <summary>
        /// Kill the word behind the cursor.
        /// Word boundaries are the same as <see cref="BackwardWord"/>.
        /// </summary>
        private void BackwardKillWord()
        {
            int origCursorPos = 0;
            lock (_lock)
            {
                origCursorPos = _cursorPos;
            }

            BackwardWord();
            int currCursorPos = 0;
            lock (_lock)
            {
                currCursorPos = _cursorPos;
            }

            if (currCursorPos == origCursorPos)
            {
                return;
            }

            lock (_lock)
            {
                KillText(ref _input, currCursorPos, origCursorPos - currCursorPos);
            }
        }
        /// <summary>
        /// Kills the word behind the cursor, using white space as a word boundary.
        /// </summary>
        private void UnixWordRubout()
        {
            lock (_lock)
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
                ResetCursorBlinking();
            }

            // yields exactly the same result as a internal "whitespace" function in bash
            static bool IsRegularWhiteSpaceOrTab(char ch) => ch == ' ' || ch == '\t';
        }

        /// <summary>
        /// Drags the character before the cursor forward over the character at the cursor, moving the cursor forward as well.
        /// If the insertion point is at the end of the line, then this transposes the last two characters of the line.
        /// </summary>
        private void TransposeTwoChars()
        {
            lock (_lock)
            {
                int inputLength = _input.Length;
                if (inputLength < 2)
                {
                    return;
                }

                if (_cursorPos == 0)
                {
                    _input = SwapTwoCharacters(_input, 0);
                    _cursorPos = 2;
                }
                else if (_cursorPos < inputLength)
                {
                    _input = SwapTwoCharacters(_input, _cursorPos - 1);
                    _cursorPos += 1;
                }
                else
                {
                    _input = SwapTwoCharacters(_input, _cursorPos - 2);
                }

                UpdateCommandCandidates();
                ResetCursorBlinking();
                ClearSelection();
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
            int origCursorPos = 0;

            lock (_lock)
            {
                if (_input.Length < 3)
                {
                    return;
                }

                origCursorPos = _cursorPos;
            }

            int word2End = 0;
            int word2Beg = 0;
            int word1Beg = 0;
            int word1End = 0;

            ForwardWord();
            lock (_lock)
            {
                word2End = _cursorPos;
            }
            BackwardWord();
            lock (_lock)
            {
                word2Beg = _cursorPos;
            }
            BackwardWord();
            lock (_lock)
            {
                word1Beg = _cursorPos;
            }
            ForwardWord();
            lock (_lock)
            {
                word1End = _cursorPos;
            }

            if ((word1Beg == word2Beg) || (word2Beg < word1End))
            {
                lock (_lock)
                {
                    _cursorPos = origCursorPos;
                }
                return;
            }

            string origInput = null;
            lock (_lock)
            {
                origInput = _input;
            }

            string word1 = origInput.Substring(word1Beg, word1End - word1Beg);
            string word2 = origInput.Substring(word2Beg, word2End - word2Beg);

            // Prevent reallocation of internal array
            var stringBuilder = new StringBuilder(origInput.Length + Math.Max((word1.Length - word2.Length), 0));
            stringBuilder.Append(origInput);

            stringBuilder.Remove(word2Beg, word2.Length);
            stringBuilder.Insert(word2Beg, word1);

            stringBuilder.Remove(word1Beg, word1.Length);
            stringBuilder.Insert(word1Beg, word2);

            lock (_lock)
            {
                _input = stringBuilder.ToString();
                _cursorPos = word2End;
                UpdateCommandCandidates(); 
            }

            ResetCursorBlinking();
            ClearSelection();
        }

        private void KillText(ref string str, int startIndex, int length)
        {
            Clipboard.SetText(str.Substring(startIndex, length));
            str = str.Remove(startIndex, length);
            UpdateCommandCandidates(); 
        }

        private void MoveCursorLeft(bool moveSelection = false)
        {
            int oldPosition;

            lock (_lock)
            {
                oldPosition = _cursorPos;

                if (_cursorPos > 0)
                {
                    _cursorPos--;
                }
            }

            UpdateSelection(oldPosition, moveSelection);
            ResetCursorBlinking();
        }

        private void MoveCursorRight(bool moveSelection = false)
        {
            int oldPosition;

            lock (_lock)
            {
                oldPosition = _cursorPos;

                if (_cursorPos < _input.Length)
                {
                    _cursorPos++;
                }
            }

            UpdateSelection(oldPosition, moveSelection);
            ResetCursorBlinking();
        }

        private void MoveCursorToBegOfLine(bool moveSelection = false)
        {
            int oldPosition;

            lock (_lock)
            {
                oldPosition = _cursorPos;
                _cursorPos = 0;
            }

            UpdateSelection(oldPosition, moveSelection);
            ResetCursorBlinking();
        }

        private void MoveCursorToEndOfLine(bool moveSelection = false)
        {
            int oldPosition;

            lock (_lock)
            {
                oldPosition = _cursorPos;
                _cursorPos = _input.Length;
            }

            UpdateSelection(oldPosition, moveSelection);
            ResetCursorBlinking();
        }

        private void CompileExpression()
        {
            // We need to capture the input for the task action below, so it won't read a stale reference of
            // the console input
            string capturedInput = null;
            lock (_lock)
            {
                capturedInput = _input;

                if (string.IsNullOrEmpty(capturedInput) || _compilerTask != null)
                {
                    return;
                }

                _commandPos = -1;
                if (_commandHistory.LastOrDefault() != capturedInput)
                {
                    _commandHistory.Add(capturedInput);
                }
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

                errors.AppendLine($"Couldn't compile input expression: {capturedInput}");

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

            lock (_lock)
            {
                _compilerTask = newCompilerTask;
            }
        }

        private bool HasSelection
        {
            get
            {
                lock (_lock)
                {
                    return _selectionAnchor >= 0 &&
                       _selectionAnchor != _cursorPos;
                }
            }
        }

        private void SelectAll()
        {
            lock (_lock)
            {
                _cursorPos = 0;
            }

            MoveCursorToEndOfLine(true);
        }

        private void CopySelection()
        {
            if (!HasSelection)
            {
                return;
            }

            string content;
            lock (_lock)
            {
                (int start, int end) = MinMax(_selectionAnchor, _cursorPos);
                content = _input.Substring(start, end - start);
            }

            Clipboard.SetText(content);
        }

        private void DeleteSelection()
        {
            lock (_lock)
            {
                if (!HasSelection)
                {
                    return;
                }

                (int start, int end) = MinMax(_cursorPos, _selectionAnchor);

                int length = end - start;

                _input = _input.Remove(start, length);
                _cursorPos = start;

                ClearSelection();
                UpdateCommandCandidates();
            }
        }

        private void UpdateSelection(int oldCursor, bool moveSelection)
        {
            if (!moveSelection)
            {
                ClearSelection();
                return;
            }

            lock (_lock)
            {
                if (_selectionAnchor < 0)
                {
                    _selectionAnchor = oldCursor;
                }

                (int start, int end) = MinMax(_selectionAnchor, _cursorPos);

                _lengthUntilSelectionStart = GetTextLength(_input.Substring(0, start)) - GetMarginLength();
                _lengthUntilSelectionEnd = GetTextLength(_input.Substring(0, end)) - GetMarginLength();
            }
        }

        private void ClearSelection()
        {
            lock (_lock)
            {
                _lengthUntilSelectionStart = -1f;
                _lengthUntilSelectionEnd = -1f;
                _selectionAnchor = -1;
            }
        }

        private void UpdateCommandCandidates()
        {
            string input = _input;
            _commandCandidates.Clear();
            _selectedCandidateIndex = -1;
            if (string.IsNullOrWhiteSpace(input))
                return;

            lock (_lock)
            {
                foreach (var space in _commands.Keys)
                {
                    foreach (var cmd in _commands[space])
                    {
                        var paramList = cmd.MethodInfo.GetParameters();
                        string paramStr = string.Join(", ", paramList.Select(p => p.ParameterType.Name + " " + p.Name));
                        string displayName = $"{cmd.Name}({paramStr})";

                        if (displayName.Contains(input))
                        {
                            _commandCandidates.Add(displayName);
                        }
                    }
                }
            }
            if (_commandCandidates.Count > 0)
                _selectedCandidateIndex = 0;
        }

        private void ResetCursorBlinking()
        {
            lock (_lock)
            {
                _lastCursorBlinkTick = 0;

                //Because we set _lastCursorBlinkTick to 0; the state will be flipped next tick.
                _cursorVisible = false;
            }
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

        private static float _cachedMarginLength = -1f;

        private static float GetMarginLength()
        {
            if (_cachedMarginLength < 0f)
            {
                float len1 = GetTextLength("A");
                float len2 = GetTextLength("AA");

                _cachedMarginLength = len1 - (len2 - len1); // [Margin][A] - [A] = [Margin]
            }
            return _cachedMarginLength;
        }

        private static string LongestCommonPrefix(List<string> strs, string input)
        {
            if (strs == null || strs.Count == 0)
                return input;

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
                    break; 
            }
            return prefix;
        }

        private (int min, int max) MinMax(int a, int b)
        {
            return a < b ? (a, b) : (b, a);
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
                return false;

            KeyBinding best = null;
            int bestScore = -1;

            foreach (var binding in list)
            {
                var requiredMods = binding.Combined & ModMask;

                if ((mods & requiredMods) != requiredMods)
                    continue;

                int score = CountBits((uint)requiredMods);

                if (score > bestScore)
                {
                    bestScore = score;
                    best = binding;
                }
            }

            if(best == null)
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
