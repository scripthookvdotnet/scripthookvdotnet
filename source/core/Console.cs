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
        private int _lastCursorPos = 0;
        private string _lastRenderedCursorInput = string.Empty;
        private int _lastCursorBlinkTick = 0;
        private bool _cursorVisible = false;
        private int _commandPos = -1;
        private int _currentPage = 1;
        private bool _isOpen = false;
        private string _input = string.Empty;
        private string _lastInput = string.Empty;
        private string _lastRenderedInput = string.Empty;
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
        private readonly Dictionary<TextLayoutCacheKey, int> _lineCountCache = new();
        private readonly Dictionary<TextLayoutCacheKey, List<string>> _renderLineCache = new();
        private Size _cachedResolution = Size.Empty;

        // We need a lock because tick calls and keyboard events are fired on different threads, even if we don't use
        // a dedicated thread in order to avoid a fiber from SHV
        private readonly object _lock = new();
        private readonly object _textLayoutCacheLock = new();

        private const int BaseWidth = 1280;
        private const int BaseHeight = 720;
        private const int ConsoleWidth = BaseWidth;
        private const int ConsoleHeight = BaseHeight / 3;
        private const int InputHeight = 20;
        private const int LinesPerPage = 16;
        private const float ConsoleTextScale = 0.35f;
        private const float OutputTextX = 2f;
        private const float OutputLineHeight = 14f;
        private const int TextWrapSafetyBackoff = 2;
        private const int WordWrapSafetyBackoff = 1;
        private const int MaxTextCommandBytes = /* MaxNumberOfSubStringsInPrintCommand */ 4 * 99;
        private const int MaxLineCountCacheEntries = 2048;
        private const int MaxRenderLineCacheEntries = 512;

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

        private KeyManager _keyManager = new();


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
                _input = _input.Insert(_cursorPos, text);
                _cursorPos += text.Length;
                UpdateCommandCandidates();
            }

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
                ClearTextLayoutCaches();
            }
        }

        /// <summary>
        /// Writes a message to the console.
        /// </summary>
        /// <param name="msg">The composite format string.</param>
        public void PrintMessage(string headerStr, string msg)
        {
            AddLines(headerStr + " ", new[] { NormalizeNewlinesForGtaText(msg) });
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

            AddLines(headerStr + " ", new[] { NormalizeNewlinesForGtaText(msg) });
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
            List<string> lineHistorySnapshot;
            List<string> commandCandidatesSnapshot;
            bool hideCandidates;
            int selectedCandidateIndex;

            string inputRender;
            string cursorRender;

            lock (_lock)
            {
                currInput = _input;
                currLineHistCount = _lineHistory.Count;
                currPage = _currentPage;
                lineHistorySnapshot = new List<string>(_lineHistory);
                commandCandidatesSnapshot = new List<string>(_commandCandidates);
                hideCandidates = _hideCandidates;
                selectedCandidateIndex = _selectedCandidateIndex;

                if (_input != _lastInput)
                {
                    _lastInput = _input;
                    _lastRenderedInput = EscapeTokens(_input);

                    _lastCursorPos = -1;
                }

                if (_cursorPos != _lastCursorPos)
                {
                    _lastCursorPos = _cursorPos;
                    _lastRenderedCursorInput = EscapeTokens(_input.Substring(0, _cursorPos));
                }

                inputRender = _lastRenderedInput;
                cursorRender = _lastRenderedCursorInput;
            }

            // Draw background
            DrawRect(0, 0, ConsoleWidth, ConsoleHeight, s_backgroundColor);
            // Draw input field
            DrawRect(0, ConsoleHeight, ConsoleWidth, InputHeight, s_altBackgroundColor);
            DrawRect(0, ConsoleHeight + InputHeight, 80, InputHeight, s_altBackgroundColor);
            // Draw input prefix
            DrawText(0, ConsoleHeight, "$>", s_prefixColor);
            // Draw input text
            DrawText(25, ConsoleHeight, inputRender, compilerTask == null ? s_inputColor : s_inputColorBusy);

            List<string> displayLineHistory = SplitHistoryIntoRenderLines(lineHistorySnapshot, OutputTextX, 0f);
            currLineHistCount = displayLineHistory.Count;
            int totalPages = System.Math.Max(1, ((currLineHistCount + (LinesPerPage - 1)) / LinesPerPage));
            if (currPage > totalPages)
            {
                currPage = totalPages;
                lock (_lock)
                {
                    _currentPage = currPage;
                }
            }

            // Draw page information
            DrawText(5, ConsoleHeight + InputHeight, "Page " + currPage + "/" + totalPages, s_inputColor);

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
                    float lengthBetweenInputStartAndCursor = GetTextLength(cursorRender) - GetMarginLength();
                    DrawRect(26 + (lengthBetweenInputStartAndCursor * ConsoleWidth), ConsoleHeight + 2, 2, InputHeight - 4, Color.White);
                }

                // Draw command candidates
                if (!hideCandidates && commandCandidatesSnapshot.Count > 0)
                {
                    for (int i = 0; i < commandCandidatesSnapshot.Count && i < 5; i++)
                    {
                        var color = (i == selectedCandidateIndex) ? Color.Yellow : Color.White;
                        DrawText(25, ConsoleHeight + InputHeight + 16 + i * 16, commandCandidatesSnapshot[i], color);
                    }
                }
            }

            // Draw console history text after GTA-layout splitting so wrapped output consumes real rows.
            int historyOffset = currLineHistCount - (LinesPerPage * currPage);
            int historyLength = System.Math.Min(currLineHistCount, historyOffset + LinesPerPage);
            for (int i = System.Math.Max(0, historyOffset); i < historyLength; ++i)
            {
                DrawConsoleOutputText(OutputTextX, (float)((i - historyOffset) * OutputLineHeight), displayLineHistory[i], s_outputColor);
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
                        }
                        else
                        {
                            _input = candidate;
                            _cursorPos = _input.Length;
                            UpdateCommandCandidates();
                            ResetCursorBlinking();
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
            List<string> lineHistorySnapshot;
            lock (_lock)
            {
                lineHistorySnapshot = new List<string>(_lineHistory);
            }

            int renderLineCount = GetHistoryRenderLineCount(lineHistorySnapshot, OutputTextX, 0f);
            int totalPages = System.Math.Max(1, ((renderLineCount + LinesPerPage - 1) / LinesPerPage));

            lock (_lock)
            {
                if (_currentPage < totalPages)
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
            }
        }

        /// <summary>
        /// Moves to the end of the next word, just like emacs and GNU readline (does not move to the beginning of the next word like zsh does for forward-word).
        /// Words are composed of letters and digits.
        /// </summary>
        private void ForwardWord()
        {
            lock (_lock)
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

            ResetCursorBlinking();
        }
        /// <summary>
        /// Moves back to the start of the current or previous word.
        /// Words are composed of letters and digits.
        /// </summary>
        private void BackwardWord()
        {
            lock (_lock)
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

            ResetCursorBlinking();
        }
        /// <summary>
        /// Deletes the character behind the cursor.
        /// </summary>
        private void BackwardDeleteChar()
        {
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
        }

        private void KillText(ref string str, int startIndex, int length)
        {
            Clipboard.SetText(str.Substring(startIndex, length));
            str = str.Remove(startIndex, length);
            UpdateCommandCandidates(); 
        }

        private void MoveCursorLeft()
        {
            lock (_lock)
            {
                if (_cursorPos > 0)
                {
                    _cursorPos--;
                }
            }

            ResetCursorBlinking();
        }

        private void MoveCursorRight()
        {
            lock (_lock)
            {
                if (_cursorPos < _input.Length)
                {
                    _cursorPos++;
                }
            }

            ResetCursorBlinking();
        }

        private void MoveCursorToBegOfLine()
        {
            lock (_lock)
            {
                _cursorPos = 0;
            }

            ResetCursorBlinking();
        }

        private void MoveCursorToEndOfLine()
        {
            lock (_lock)
            {
                _cursorPos = _input.Length;
            }

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

                errors.AppendLine($"Couldn't compile input expression: {EscapeTokens(capturedInput)}");

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

        private static string NormalizeNewlinesForGtaText(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return str.Replace("\r\n", "~n~").Replace("\n", "~n~").Replace("\r", "~n~");
        }

        private static string EscapeTokens(string str)
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
            NativeFunc.Invoke(0x07C837F9A01C34C9 /* SET_TEXT_SCALE */, ConsoleTextScale, ConsoleTextScale);
            NativeFunc.Invoke(0xBE6B23FFA53FB442 /* SET_TEXT_COLOUR */, color.R, color.G, color.B, color.A);
            NativeFunc.Invoke(0x25FBB336DF1804CB /* BEGIN_TEXT_COMMAND_DISPLAY_TEXT */, NativeMemory.CellEmailBcon);
            NativeFunc.PushLongString(text, 99);
            NativeFunc.Invoke(0xCD015E5BB0D96A57 /* END_TEXT_COMMAND_DISPLAY_TEXT */, (x / BaseWidth), (y / BaseHeight));
        }

        private static unsafe void DrawConsoleOutputText(float x, float y, string text, Color color)
        {
            NativeFunc.Invoke(0x66E0276CC5F6B9DA /* SET_TEXT_FONT */, 0); // Chalet London :>
            NativeFunc.Invoke(0x07C837F9A01C34C9 /* SET_TEXT_SCALE */, ConsoleTextScale, ConsoleTextScale);
            NativeFunc.Invoke(0xBE6B23FFA53FB442 /* SET_TEXT_COLOUR */, color.R, color.G, color.B, color.A);
            NativeFunc.Invoke(0x63145D9C883A1A70 /* SET_TEXT_WRAP */, x / BaseWidth, 1f);
            NativeFunc.Invoke(0x25FBB336DF1804CB /* BEGIN_TEXT_COMMAND_DISPLAY_TEXT */, NativeMemory.CellEmailBcon);
            // GTA only accepts roughly four variable substrings for END_TEXT_COMMAND_DISPLAY_TEXT.
            // Very long outputs therefore still need several draw calls, which SplitIntoRenderLines provides.
            NativeFunc.PushLongString(text, 99);
            NativeFunc.Invoke(0xCD015E5BB0D96A57 /* END_TEXT_COMMAND_DISPLAY_TEXT */, (x / BaseWidth), (y / BaseHeight));
            NativeFunc.Invoke(0x63145D9C883A1A70 /* SET_TEXT_WRAP */, 0f, 1f);
        }

        private List<string> SplitHistoryIntoRenderLines(List<string> history, float x, float y)
        {
            var displayLines = new List<string>();
            foreach (string line in history)
            {
                displayLines.AddRange(SplitIntoRenderLines(line, x, y));
            }

            return displayLines;
        }

        private int GetHistoryRenderLineCount(List<string> history, float x, float y)
        {
            int count = 0;
            foreach (string line in history)
            {
                count += SplitIntoRenderLines(line, x, y).Count;
            }

            return count;
        }

        private List<string> SplitIntoRenderLines(string text, float x, float y)
        {
            text ??= string.Empty;

            Size resolution = EnsureTextLayoutCacheResolution();
            TextLayoutCacheKey cacheKey = CreateTextLayoutCacheKey(text, x, y, resolution);
            lock (_textLayoutCacheLock)
            {
                if (_renderLineCache.TryGetValue(cacheKey, out List<string> cachedLines))
                {
                    return cachedLines;
                }
            }

            var lines = new List<string>();
            foreach (string paragraph in SplitOnGtaNewlineTokens(text))
            {
                if (paragraph.Length == 0)
                {
                    lines.Add(string.Empty);
                    continue;
                }

                TextFormattingState formattingState = default;
                int startIndex = 0;
                while (startIndex < paragraph.Length)
                {
                    string renderPrefix = formattingState.CreatePrefix();
                    string renderText = renderPrefix + paragraph.Substring(startIndex);
                    int splitIndex = FindSplitIndex(renderText, 1, x, y, resolution);
                    if (splitIndex >= renderText.Length)
                    {
                        lines.Add(renderText);
                        break;
                    }

                    if (splitIndex <= 0)
                    {
                        splitIndex = FindNextPlainTextSplitIndex(renderText, 1);
                    }

                    if (renderPrefix.Length > 0 && splitIndex <= renderPrefix.Length)
                    {
                        splitIndex = FindNextPlainTextSplitIndex(renderText, renderPrefix.Length + 1);
                    }

                    splitIndex = System.Math.Max(1, System.Math.Min(splitIndex, renderText.Length));
                    string segment = renderText.Substring(0, splitIndex);
                    lines.Add(segment);

                    formattingState = GetActiveFormattingState(segment);
                    startIndex += System.Math.Max(1, splitIndex - renderPrefix.Length);
                }
            }

            lock (_textLayoutCacheLock)
            {
                EnforceCacheLimit(_renderLineCache, MaxRenderLineCacheEntries);
                _renderLineCache[cacheKey] = lines;
            }
            return lines;
        }

        private int FindSplitIndex(string text, int targetLine, float x, float y, Size resolution)
        {
            int low = 1;
            int high = 1;
            while (high < text.Length &&
                IsTextCommandSafeLength(text, high) &&
                GetLineCount(text.Substring(0, high), x, y, resolution) <= targetLine)
            {
                low = high + 1;
                high = System.Math.Min(text.Length, high * 2);
            }

            if (high >= text.Length &&
                IsTextCommandSafeLength(text, text.Length) &&
                GetLineCount(text, x, y, resolution) <= targetLine)
            {
                return text.Length;
            }

            high = System.Math.Min(high, text.Length);
            while (low < high)
            {
                int mid = low + ((high - low) / 2);
                if (!IsTextCommandSafeLength(text, mid) ||
                    GetLineCount(text.Substring(0, mid), x, y, resolution) > targetLine)
                {
                    high = mid;
                }
                else
                {
                    low = mid + 1;
                }
            }

            int splitIndex = FindPreviousPlainTextSplitIndex(text, low - 1);
            while (splitIndex > 0 &&
                (!IsTextCommandSafeLength(text, splitIndex) ||
                GetLineCount(text.Substring(0, splitIndex), x, y, resolution) > targetLine))
            {
                splitIndex = FindPreviousPlainTextSplitIndex(text, splitIndex - 1);
            }

            int wordSplitIndex = FindPreviousWordSplitIndex(text, splitIndex);
            if (wordSplitIndex > 0 &&
                IsTextCommandSafeLength(text, wordSplitIndex) &&
                GetLineCount(text.Substring(0, wordSplitIndex), x, y, resolution) <= targetLine)
            {
                return MoveTrailingTokenRunToNextLine(text, BackOffWordSplitIndex(text, wordSplitIndex, WordWrapSafetyBackoff));
            }

            return MoveTrailingTokenRunToNextLine(text, BackOffPlainTextSplitIndex(text, splitIndex, TextWrapSafetyBackoff));
        }

        private int GetLineCount(string text, float x, float y)
        {
            return GetLineCount(text, x, y, EnsureTextLayoutCacheResolution());
        }

        private unsafe int GetLineCount(string text, float x, float y, Size resolution)
        {
            text ??= string.Empty;

            TextLayoutCacheKey cacheKey = CreateTextLayoutCacheKey(text, x, y, resolution);
            lock (_textLayoutCacheLock)
            {
                if (_lineCountCache.TryGetValue(cacheKey, out int cachedCount))
                {
                    return cachedCount;
                }
            }

            float normalizedX = x / BaseWidth;
            float normalizedY = y / BaseHeight;
            NativeFunc.Invoke(0x66E0276CC5F6B9DA /* SET_TEXT_FONT */, 0);
            NativeFunc.Invoke(0x07C837F9A01C34C9 /* SET_TEXT_SCALE */, ConsoleTextScale, ConsoleTextScale);
            NativeFunc.Invoke(0x63145D9C883A1A70 /* SET_TEXT_WRAP */, normalizedX, 1f);
            NativeFunc.Invoke(0x521FB041D93DD0E4 /* BEGIN_TEXT_COMMAND_GET_NUMBER_OF_LINES_FOR_STRING */, NativeMemory.CellEmailBcon);
            NativeFunc.PushLongString(text, 99);
            int lineCount = *(int*)NativeFunc.Invoke(0x9040DFB09BE75706 /* END_TEXT_COMMAND_GET_NUMBER_OF_LINES_FOR_STRING */, normalizedX, normalizedY);
            NativeFunc.Invoke(0x63145D9C883A1A70 /* SET_TEXT_WRAP */, 0f, 1f);

            lineCount = System.Math.Max(1, lineCount);
            lock (_textLayoutCacheLock)
            {
                EnforceCacheLimit(_lineCountCache, MaxLineCountCacheEntries);
                _lineCountCache[cacheKey] = lineCount;
            }

            return lineCount;
        }

        private unsafe Size EnsureTextLayoutCacheResolution()
        {
            int width = BaseWidth;
            int height = BaseHeight;
            ulong* args = stackalloc ulong[2];
            args[0] = (ulong)(&width);
            args[1] = (ulong)(&height);
            NativeFunc.Invoke(0x873C9F3104101DD3 /* GET_ACTUAL_SCREEN_RESOLUTION */, args, 2);

            var resolution = new Size(width, height);
            if (resolution != _cachedResolution)
            {
                _cachedResolution = resolution;
                ClearTextLayoutCaches();
            }

            return resolution;
        }

        private void ClearTextLayoutCaches()
        {
            lock (_textLayoutCacheLock)
            {
                _lineCountCache.Clear();
                _renderLineCache.Clear();
            }
        }

        private static void EnforceCacheLimit<TKey, TValue>(Dictionary<TKey, TValue> cache, int maxEntries)
        {
            if (cache.Count < maxEntries)
            {
                return;
            }

            cache.Clear();
        }

        private static TextLayoutCacheKey CreateTextLayoutCacheKey(string text, float x, float y, Size resolution)
        {
            return new TextLayoutCacheKey(text, x, y, ConsoleTextScale, resolution);
        }

        private static List<string> SplitOnGtaNewlineTokens(string text)
        {
            var result = new List<string>();
            int start = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (IsGtaTokenAt(text, i, out int tokenEnd) &&
                    string.Equals(text.Substring(i + 1, tokenEnd - i - 1), "n", StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(text.Substring(start, i - start));
                    start = tokenEnd + 1;
                    i = tokenEnd;
                }
            }

            result.Add(text.Substring(start));
            return result;
        }

        private static TextFormattingState GetActiveFormattingState(string text)
        {
            TextFormattingState state = default;

            for (int i = 0; i < text.Length; i++)
            {
                if (IsCondensedStartTagAt(text, i))
                {
                    state.IsCondensedActive = true;
                    i += 2;
                    continue;
                }

                if (IsCondensedEndTagAt(text, i))
                {
                    state.IsCondensedActive = false;
                    i += 3;
                    continue;
                }

                if (!IsGtaTokenAt(text, i, out int tokenEnd))
                {
                    continue;
                }

                string tokenContent = text.Substring(i + 1, tokenEnd - i - 1);
                if (IsDefaultColorResetToken(tokenContent))
                {
                    state.ActiveColorToken = string.Empty;
                }
                else if (IsPersistentColorToken(tokenContent))
                {
                    state.ActiveColorToken = text.Substring(i, tokenEnd - i + 1);
                }
                else if (IsBoldToken(tokenContent))
                {
                    state.IsBoldActive = !state.IsBoldActive;
                }
                else if (IsItalicToken(tokenContent))
                {
                    state.IsItalicActive = !state.IsItalicActive;
                }

                i = tokenEnd;
            }

            return state;
        }

        private static bool IsDefaultColorResetToken(string tokenContent)
        {
            return string.Equals(tokenContent, "s", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsPersistentColorToken(string tokenContent)
        {
            if (tokenContent.StartsWith("HUD_COLOUR_", StringComparison.OrdinalIgnoreCase) ||
                tokenContent.StartsWith("HC_", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (tokenContent.Length != 1)
            {
                return false;
            }

            switch (char.ToLowerInvariant(tokenContent[0]))
            {
                case 'r':
                case 'g':
                case 'b':
                case 'f':
                case 'y':
                case 'c':
                case 't':
                case 'o':
                case 'p':
                case 'q':
                case 'm':
                case 'l':
                case 'd':
                case 'v':
                case 'u':
                case 'w':
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsBoldToken(string tokenContent)
        {
            return string.Equals(tokenContent, "h", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(tokenContent, "bold", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsItalicToken(string tokenContent)
        {
            return string.Equals(tokenContent, "italic", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsCondensedStartTagAt(string text, int index)
        {
            return index + 2 < text.Length &&
                text[index] == '<' &&
                char.ToUpperInvariant(text[index + 1]) == 'C' &&
                text[index + 2] == '>';
        }

        private static bool IsCondensedEndTagAt(string text, int index)
        {
            return index + 3 < text.Length &&
                text[index] == '<' &&
                text[index + 1] == '/' &&
                char.ToUpperInvariant(text[index + 2]) == 'C' &&
                text[index + 3] == '>';
        }

        private struct TextFormattingState
        {
            public string ActiveColorToken;
            public bool IsBoldActive;
            public bool IsItalicActive;
            public bool IsCondensedActive;

            public string CreatePrefix()
            {
                if (string.IsNullOrEmpty(ActiveColorToken) && !IsBoldActive && !IsItalicActive && !IsCondensedActive)
                {
                    return string.Empty;
                }

                var prefix = new StringBuilder();
                if (IsCondensedActive)
                {
                    prefix.Append("<C>");
                }

                prefix.Append(ActiveColorToken);
                if (IsBoldActive)
                {
                    prefix.Append("~h~");
                }

                if (IsItalicActive)
                {
                    prefix.Append("~italic~");
                }

                return prefix.ToString();
            }
        }

        private struct TextLayoutCacheKey : IEquatable<TextLayoutCacheKey>
        {
            private readonly string _text;
            private readonly float _x;
            private readonly float _y;
            private readonly float _scale;
            private readonly int _resolutionWidth;
            private readonly int _resolutionHeight;

            public TextLayoutCacheKey(string text, float x, float y, float scale, Size resolution)
            {
                _text = text ?? string.Empty;
                _x = x;
                _y = y;
                _scale = scale;
                _resolutionWidth = resolution.Width;
                _resolutionHeight = resolution.Height;
            }

            public bool Equals(TextLayoutCacheKey other)
            {
                return _x.Equals(other._x) &&
                    _y.Equals(other._y) &&
                    _scale.Equals(other._scale) &&
                    _resolutionWidth == other._resolutionWidth &&
                    _resolutionHeight == other._resolutionHeight &&
                    string.Equals(_text, other._text, StringComparison.Ordinal);
            }

            public override bool Equals(object obj)
            {
                return obj is TextLayoutCacheKey other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = (hash * 31) + (_text?.GetHashCode() ?? 0);
                    hash = (hash * 31) + _x.GetHashCode();
                    hash = (hash * 31) + _y.GetHashCode();
                    hash = (hash * 31) + _scale.GetHashCode();
                    hash = (hash * 31) + _resolutionWidth;
                    hash = (hash * 31) + _resolutionHeight;
                    return hash;
                }
            }
        }

        private static int FindPreviousPlainTextSplitIndex(string text, int index)
        {
            for (int i = System.Math.Min(index, text.Length - 1); i > 0; i--)
            {
                if (IsPlainTextSplitIndex(text, i))
                {
                    return i;
                }
            }

            return 0;
        }

        private static bool IsTextCommandSafeLength(string text, int length)
        {
            int byteCount = 0;
            int end = System.Math.Min(length, text.Length);
            for (int i = 0; i < end; i++)
            {
                char chr = text[i];
                if (chr < 0x80)
                {
                    byteCount += 1;
                }
                else if (chr < 0x800)
                {
                    byteCount += 2;
                }
                else if (char.IsHighSurrogate(chr) && i + 1 < end && char.IsLowSurrogate(text[i + 1]))
                {
                    byteCount += 4;
                    i++;
                }
                else
                {
                    byteCount += 3;
                }

                if (byteCount > MaxTextCommandBytes)
                {
                    return false;
                }
            }

            return true;
        }

        private static int FindNextPlainTextSplitIndex(string text, int index)
        {
            for (int i = System.Math.Max(1, index); i < text.Length; i++)
            {
                if (IsPlainTextSplitIndex(text, i))
                {
                    return i;
                }
            }

            return text.Length;
        }

        private static int FindPreviousWordSplitIndex(string text, int index)
        {
            int searchStart = System.Math.Min(index, text.Length - 1);
            for (int i = searchStart; i > 0; i--)
            {
                if (!IsPlainTextSplitIndex(text, i))
                {
                    continue;
                }

                if (char.IsWhiteSpace(text[i - 1]) || char.IsWhiteSpace(text[i]))
                {
                    return i;
                }
            }

            return 0;
        }

        private static int BackOffPlainTextSplitIndex(string text, int index, int count)
        {
            int splitIndex = index;
            for (int i = 0; i < count; i++)
            {
                int previousSplitIndex = FindPreviousPlainTextSplitIndex(text, splitIndex - 1);
                if (previousSplitIndex <= 0)
                {
                    break;
                }

                splitIndex = previousSplitIndex;
            }

            return splitIndex;
        }

        private static int BackOffWordSplitIndex(string text, int index, int count)
        {
            int splitIndex = index;
            for (int i = 0; i < count; i++)
            {
                int previousSplitIndex = FindPreviousWordSplitIndex(text, splitIndex - 1);
                if (previousSplitIndex <= 0)
                {
                    break;
                }

                splitIndex = previousSplitIndex;
            }

            return splitIndex;
        }

        private static int MoveTrailingTokenRunToNextLine(string text, int index)
        {
            int searchIndex = System.Math.Min(index, text.Length);
            int tokenStart = -1;
            bool foundToken = false;

            while (searchIndex > 0)
            {
                int previousIndex = searchIndex - 1;
                if (char.IsWhiteSpace(text[previousIndex]))
                {
                    searchIndex--;
                    continue;
                }

                if (!TryGetTokenEndingAt(text, previousIndex, out int currentTokenStart))
                {
                    break;
                }

                tokenStart = currentTokenStart;
                searchIndex = currentTokenStart;
                foundToken = true;
            }

            if (!foundToken || tokenStart <= 0)
            {
                return index;
            }

            return tokenStart;
        }

        private static bool TryGetTokenEndingAt(string text, int tokenEnd, out int tokenStart)
        {
            tokenStart = -1;
            if (tokenEnd <= 0 || tokenEnd >= text.Length || text[tokenEnd] != '~' || IsEscapedTilde(text, tokenEnd))
            {
                return false;
            }

            for (int i = tokenEnd - 1; i >= 0; i--)
            {
                if (text[i] != '~' || IsEscapedTilde(text, i))
                {
                    continue;
                }

                tokenStart = i;
                return true;
            }

            return false;
        }

        private static bool IsPlainTextSplitIndex(string text, int index)
        {
            if (index <= 0 || index >= text.Length)
            {
                return index == text.Length;
            }

            if (text[index - 1] == '\\' && text[index] == '~')
            {
                return false;
            }

            return !IsGtaTokenCharacter(text, index - 1) && !IsGtaTokenCharacter(text, index);
        }

        private static bool IsGtaTokenCharacter(string text, int index)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!IsGtaTokenAt(text, i, out int tokenEnd))
                {
                    continue;
                }

                if (index >= i && index <= tokenEnd)
                {
                    return true;
                }

                i = tokenEnd;
            }

            return false;
        }

        private static bool IsGtaTokenAt(string text, int index, out int tokenEnd)
        {
            tokenEnd = -1;
            if (index < 0 || index >= text.Length || text[index] != '~' || IsEscapedTilde(text, index))
            {
                return false;
            }

            for (int i = index + 1; i < text.Length; i++)
            {
                if (text[i] == '~' && !IsEscapedTilde(text, i))
                {
                    tokenEnd = i;
                    return true;
                }
            }

            return false;
        }

        private static bool IsEscapedTilde(string text, int index)
        {
            int backslashCount = 0;
            for (int i = index - 1; i >= 0 && text[i] == '\\'; i--)
            {
                backslashCount++;
            }

            return (backslashCount & 1) != 0;
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
            NativeFunc.Invoke(0x07C837F9A01C34C9 /* SET_TEXT_SCALE */, ConsoleTextScale, ConsoleTextScale);
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
