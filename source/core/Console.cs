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
using GTA.Native;

namespace GTA
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

	internal class ConsoleScript : Script
	{
		#region StaticFields
		int _page;
		int _cursorPos;
		int _commandPos;
		string _input;
		DateTime _lastClosed;
		LinkedList<string> _lines;
		List<string> _commandHistory;
		Regex _getEachWordRegex = new Regex(@"[^\W_]+", RegexOptions.Compiled);

		Task<Assembly> _compilerTask;
		ConcurrentQueue<string[]> _outputQueue;
		Dictionary<string, List<Tuple<ConsoleCommand, MethodInfo>>> _commands;

		System.CodeDom.Compiler.CodeDomProvider _compiler;
		System.CodeDom.Compiler.CompilerParameters _compilerOptions;

		Keys PageUpKey;
		Keys PageDownKey;
		Keys ToggleKey;

		static int DefaultFont = 0; //Chalet London :>

		static readonly Color InputColor = Color.White;
		static readonly Color InputColorBusy = Color.DarkGray;
		static readonly Color OutputColor = Color.White;
		static readonly Color PrefixColor = Color.FromArgb(255, 52, 152, 219);
		static readonly Color BackgroundColor = Color.FromArgb(200, Color.Black);
		static readonly Color AltBackgroundColor = Color.FromArgb(200, 52, 73, 94);

		const int VersionWidth = 50;
		const int InputHeight = 20;
		const float DefaultScale = 0.35f;

		static string UpdateCheckUserAgent = "scripthookvdotnet"; //Oh my dear github-api, why you do this...
		static string UpdateCheckUrl = "https://api.github.com/repos/crosire/scripthookvdotnet/releases/latest";
		static string UpdateCheckPattern = "\"tag_name\":\"v(.*?)\"";

		static string CompileTemplate = "using System; using GTA; using GTA.Native; using Console = GTA.Console;" +
			" public class ConsoleInput : GTA.DefaultConsoleCommands {{ public static Object Execute(){{ {0}; return null; }} }}";

		//TODO Temporary UI Stuff
		const int WIDTH = 1280;
		const int HEIGHT = 720;
		#endregion

		internal ConsoleScript()
		{
			Tick += OnTick;
			KeyDown += OnKeyDown;

			_outputQueue = new ConcurrentQueue<string[]>();
			_commands = new Dictionary<String, List<Tuple<ConsoleCommand, MethodInfo>>>();

			_input = "";
			_page = 1;
			_cursorPos = 0;
			_commandPos = -1;
			_lines = new LinkedList<String>();
			_commandHistory = new List<String>();

			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			Info("--- Community Script Hook V .NET {0} ---", version);
			Info("--- Type \"Help()\" to print an overview of available commands ---");

			//Start a update check Task
			Task.Factory.StartNew(DoUpdateCheck);

			RegisterCommands(typeof(DefaultConsoleCommands), true);

			String assemblyPath = Assembly.GetExecutingAssembly().Location;
			String assemblyFilename = Path.GetFileNameWithoutExtension(assemblyPath);
			ScriptSettings settings = ScriptSettings.Load(Path.ChangeExtension(assemblyPath, ".ini"));

			ToggleKey = settings.GetValue<Keys>("Console", "ToggleKey", Keys.F3);
			PageDownKey = settings.GetValue<Keys>("Console", "PageDown", Keys.PageDown);
			PageUpKey = settings.GetValue<Keys>("Console", "PageUp", Keys.PageUp);
		}

		internal bool IsOpen { get; private set; }

		[DllImport("user32.dll")]
		private static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState,
		[Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
		StringBuilder receivingBuffer,
		int bufferSize, uint flags);

		private string GetCharsFromKeys(Keys keys, bool shift, bool alt)
		{
			var buf = new StringBuilder(256);
			var keyboardState = new byte[256];

			if (shift)
				keyboardState[(int)Keys.ShiftKey] = 0xff;

			if (alt)
			{
				keyboardState[(int)Keys.ControlKey] = 0xff;
				keyboardState[(int)Keys.Menu] = 0xff;
			}

			ToUnicode((uint)keys, 0, keyboardState, buf, 256, 0);
			return buf.ToString();
		}

		private void SetControlsEnabled(bool enabled)
		{
			Native.Function.Call(Native.Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
			for (int i = 1; i <= 6; i++)
			{
				Native.Function.Call(Native.Hash.ENABLE_CONTROL_ACTION, 0, i, enabled);
			}
		}

		internal void RegisterCommand(ConsoleCommand command, System.Reflection.MethodInfo methodInfo)
		{
			RegisterCommand(command, methodInfo, false);
		}
		internal void RegisterCommand(ConsoleCommand command, System.Reflection.MethodInfo methodInfo, bool defaultCommand)
		{
			File.WriteAllText("console.log", (command == null) + ":" + (methodInfo == null));

			command.Name = defaultCommand ? methodInfo.Name : methodInfo.DeclaringType.FullName + "." + methodInfo.Name; //TODO FIX
			command.Namespace = defaultCommand ? "Default Commands" : methodInfo.DeclaringType.FullName;

			foreach (var args in methodInfo.GetParameters())
			{
				command.ConsoleArgs.Add(new ConsoleArg(args.ParameterType.Name, args.Name));
			}

			if (!_commands.ContainsKey(command.Namespace))
			{
				_commands[command.Namespace] = new List<Tuple<ConsoleCommand, MethodInfo>>();
			}

			_commands[command.Namespace].Add(new Tuple<ConsoleCommand, MethodInfo>(command, methodInfo));
		}
		internal void RegisterCommands(Type type)
		{
			RegisterCommands(type, false);
		}
		internal void RegisterCommands(Type type, bool defaultCommands)
		{
			foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
			{
				foreach (var attribute in method.GetCustomAttributes<ConsoleCommand>(true))
				{
					RegisterCommand(attribute, method, defaultCommands);
				}
			}
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

		internal void Info(string msg, params object[] args)
		{
			if (args.Length > 0)
			{
				msg = String.Format(msg, args);
			}

			AddLines("[~b~INFO~w~] ", msg.Split('\n'));
		}
		internal void Error(string msg, params object[] args)
		{
			if (args.Length > 0)
			{
				msg = String.Format(msg, args);
			}

			AddLines("[~r~ERROR~w~]", msg.Split('\n'), "~r~");
		}
		internal void Warn(string msg, params object[] args)
		{
			if (args.Length > 0)
			{
				msg = String.Format(msg, args);
			}

			AddLines("[~o~WARN~w~] ", msg.Split('\n'));
		}
		internal void Debug(string msg, params object[] args)
		{
			if (args.Length > 0)
			{
				msg = String.Format(msg, args);
			}

			AddLines("[~c~DEBUG~w~] ", msg.Split('\n'), "~c~");
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
			Info(help.ToString());
		}

		internal void Clear()
		{
			_lines.Clear();
		}

		private void AddLines(string prefix, string[] msgs)
		{
			AddLines(prefix, msgs, "~w~");
		}
		private void AddLines(string prefix, string[] msgs, string textColor)
		{
			for (int i = 0; i < msgs.Length; i++)
			{
				msgs[i] = $"~c~[{DateTime.Now.ToString("HH:mm:ss")}] ~w~{prefix} {textColor}{msgs[i]}"; //Add proper styling
			}

			_outputQueue.Enqueue(msgs);
		}

		internal void OnTick(object sender, EventArgs e)
		{
			DateTime now = DateTime.UtcNow;

			string[] outputLines;
			if (_outputQueue.TryDequeue(out outputLines))
			{
				foreach (String outputLine in outputLines)
				{
					_lines.AddFirst(outputLine);
				}
			}

			if (_compilerTask != null)
			{
				if (_compilerTask.IsCompleted)
				{
					Assembly compileResult = _compilerTask.Result;
					if (compileResult != null)
					{
						Type type = compileResult.GetType("ConsoleInput");
						object result = type.GetMethod("Execute").Invoke(null, null);
						if (result != null)
							Info($"[Return Value: {result}]");
					}
					ClearInput();
					_compilerTask = null;
				}
			}


			//Hack so the input gets blocked long enogh
			if (_lastClosed > now)
			{
				if (Native.Function.Call<bool>(Native.Hash._IS_INPUT_DISABLED, 2))
					SetControlsEnabled(false);
				return;
			}

			if (!IsOpen)
				return;
			if (Native.Function.Call<bool>(Native.Hash._IS_INPUT_DISABLED, 2))
				SetControlsEnabled(false);

			DrawRect(0, 0, WIDTH, HEIGHT / 3, BackgroundColor);
			DrawRect(0, HEIGHT / 3, WIDTH, InputHeight, AltBackgroundColor);
			DrawRect(0, HEIGHT / 3 + InputHeight, 80, InputHeight, AltBackgroundColor);

			DrawText(0, HEIGHT / 3, "$>", DefaultScale, DefaultFont, PrefixColor);
			DrawText(25, HEIGHT / 3, _input, DefaultScale, DefaultFont, _compilerTask == null ? InputColor : InputColorBusy);
			DrawText(5, HEIGHT / 3 + InputHeight, "Page " + _page + "/" + (_lines.Count == 0 ? 0 : ((_lines.Count + 16 - 1) / 16)), DefaultScale, DefaultFont, InputColor); //TODO Nicer way for page-max

			if (now.Millisecond < 500)
			{
				float length = GetTextLength(_input.Substring(0, _cursorPos), DefaultScale, DefaultFont);
				DrawText(25 + (length * WIDTH) - 4, HEIGHT / 3, "~w~~h~|~w~", DefaultScale, DefaultFont, InputColor);
			}

			//We can't get the n-th, so let's do it with a counter
			//page = 1 -. start: 0 end: 15
			//page = 2 -. start: 16 end: 31
			int start = (_page - 1) * 16; //0
			int end = System.Math.Min(_lines.Count, _page * 16 - 1); //12

			int i = 0;
			foreach (String line in _lines)
			{
				if (i >= start && i <= end)
				{
					DrawText(2, (float)((15 - (i % 16)) * 14), line, DefaultScale, DefaultFont, OutputColor);
				}
				i++;
			}
		}
		internal void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == ToggleKey)
			{
				IsOpen = !IsOpen;
				SetControlsEnabled(false);
				if (!IsOpen)
					_lastClosed = DateTime.UtcNow.AddMilliseconds(200); //Hack so the input gets blocked long enogh
				return;
			}

			if (!IsOpen)
				return;

			if (e.KeyCode == PageUpKey)
			{
				PageUp();
				return;
			}
			else if (e.KeyCode == PageDownKey)
			{
				PageDown();
				return;
			}
			else if (e.Control)
			{
				switch (e.KeyCode)
				{
					case Keys.Left:
						BackwardWord();
						return;
					case Keys.Right:
						ForwardWord();
						return;
					case Keys.A:
						MoveCursorToStartOfLine();
						return;
					case Keys.B:
						MoveCursorLeft();
						return;
					case Keys.E:
						MoveCursorToEndOfLine();
						return;
					case Keys.F:
						MoveCursorRight();
						return;
					case Keys.L:
						Clear();
						return;
					case Keys.N:
						GoDownCommandList();
						return;
					case Keys.P:
						GoUpCommandList();
						return;
					case Keys.V:
						PasteClipboard();
						return;
				}
			}
			else if (e.Alt)
			{
				switch (e.KeyCode)
				{
					case Keys.B:
						BackwardWord();
						return;
					case Keys.F:
						ForwardWord();
						return;
				}
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
					MoveCursorLeft();
					break;
				case Keys.Right:
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
					SetControlsEnabled(false);
					_lastClosed = DateTime.UtcNow.AddMilliseconds(200); //Hack so the input gets blocked long enogh
					break;
				default:
					AddToInput(GetCharsFromKeys(e.KeyCode, e.Shift, e.Alt));
					break;
			}
		}

		private void AddToInput(string input)
		{
			if (String.IsNullOrEmpty(input))
			{
				return;
			}

			_input = _input.Insert(_cursorPos, input);
			_cursorPos++;
		}
		private void AddClipboardContent()
		{
			String text = Clipboard.GetText();
			text = text.Replace("\n", ""); //TODO Keep this?

			AddToInput(text);
		}
		private void ClearInput()
		{
			_input = "";
			_cursorPos = 0;
		}
		private Assembly CompileInput()
		{
			string inputStr = String.Format(CompileTemplate, _input);

			_compiler = new Microsoft.CSharp.CSharpCodeProvider();
			_compilerOptions = new System.CodeDom.Compiler.CompilerParameters();
			_compilerOptions.GenerateInMemory = true;
			_compilerOptions.IncludeDebugInformation = true;
			_compilerOptions.ReferencedAssemblies.Add("System.dll");
			_compilerOptions.ReferencedAssemblies.Add(typeof(Script).Assembly.Location);

			foreach (Script script in ScriptDomain.CurrentDomain.RunningScripts)
			{
				if (!String.IsNullOrEmpty(script.Filename))
				{
					_compilerOptions.ReferencedAssemblies.Add(script.Filename);
				}
			}

			System.CodeDom.Compiler.CompilerResults compilerResult = _compiler.CompileAssemblyFromSource(_compilerOptions, inputStr);

			if (!compilerResult.Errors.HasErrors)
			{
				return compilerResult.CompiledAssembly;
			}
			else
			{
				Error($"Couldn't compile input-string: {_input}");

				StringBuilder errors = new StringBuilder();

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

				Error(errors.ToString());
				return null;
			}
		}
		private void ExecuteInput()
		{
			if (String.IsNullOrEmpty(_input))
			{
				return;
			}

			_commandPos = -1;

			if (_commandHistory.Count == 0 || _commandHistory[_commandHistory.Count - 1] != _input)
			{
				_commandHistory.Add(_input);
			}

			if (_compilerTask != null)
			{
				Error("Can't compile input - Compiler is busy");
				return;
			}

			_compilerTask = Task.Factory.StartNew(new Func<Assembly>(CompileInput));
		}

		private void PasteClipboard()
		{
			Thread thread = new Thread(AddClipboardContent);
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
		}

		private void MoveCursorLeft()
		{
			if (_cursorPos > 0)
				_cursorPos--;
		}
		private void MoveCursorRight()
		{
			if (_cursorPos < _input.Length)
				_cursorPos++;
		}
		private void MoveCursorToStartOfLine()
		{
			_cursorPos = 0;
		}
		private void MoveCursorToEndOfLine()
		{
			_cursorPos = _input.Length;
		}
		private void ForwardWord()
		{
			Match match = _getEachWordRegex.Match(_input, _cursorPos);

			if (match.Success)
			{
				_cursorPos = match.Index + match.Length;
			}
			else
			{
				_cursorPos = _input.Length;
			}
		}
		private void BackwardWord()
		{
			var lastMatch = _getEachWordRegex.Matches(_input).Cast<Match>().Where(x => x.Index < _cursorPos).LastOrDefault();

			if (lastMatch != null)
			{
				_cursorPos = lastMatch.Index;
			}
			else
			{
				_cursorPos = 0;
			}
		}
		private void RemoveCharLeft()
		{
			if (_input.Length > 0 && _cursorPos > 0)
			{
				_input = _input.Remove(_cursorPos - 1, 1);
				_cursorPos--;
			}
		}
		private void RemoveCharRight()
		{
			if (_input.Length > 0 && _cursorPos < _input.Length)
			{
				_input = _input.Remove(_cursorPos, 1);
			}
		}
		private void PageUp()
		{
			if (_page + 1 <= ((_lines.Count + 16 - 1) / 16))
				_page++;
		}
		private void PageDown()
		{
			if (_page - 1 >= 1)
				_page--;
		}
		private void GoUpCommandList()
		{
			if (_commandHistory.Count == 0)
				return;
			if (_commandPos >= _commandHistory.Count - 1)
				return;
			_commandPos++;
			_input = _commandHistory[_commandHistory.Count - _commandPos - 1];
		}
		private void GoDownCommandList()
		{
			if (_commandHistory.Count == 0)
				return;
			if (_commandPos <= 0)
				return;
			_commandPos--;
			_input = _commandHistory[_commandHistory.Count - _commandPos - 1];
		}

		private void DrawRect(float x, float y, int width, int height, Color color)
		{
			float w = (float)(width) / WIDTH;
			float h = (float)(height) / HEIGHT;
			float xNew = (x / WIDTH) + w * 0.5f;
			float yNew = (y / HEIGHT) + h * 0.5f;

			Native.Function.Call(Native.Hash.DRAW_RECT, xNew, yNew, w, h, color.R, color.G, color.B, color.A);
		}
		private void DrawText(float x, float y, string text, float scale, int font, Color color)
		{
			float xNew = (x / WIDTH);
			float yNew = (y / HEIGHT);

			Native.Function.Call(Native.Hash.SET_TEXT_FONT, font);
			Native.Function.Call(Native.Hash.SET_TEXT_SCALE, scale, scale);
			Native.Function.Call(Native.Hash.SET_TEXT_COLOUR, color.R, color.G, color.B, color.A);
			Native.Function.Call(Native.Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, MemoryAccess.CellEmailBcon);
			Native.Function.PushLongString(text);
			Native.Function.Call(Native.Hash.END_TEXT_COMMAND_DISPLAY_TEXT, xNew, yNew);
		}

		private float GetTextLength(string text, float scale, int font) //TODO Maybe implement somewhere else?
		{
			Native.Function.Call(Native.Hash._BEGIN_TEXT_COMMAND_WIDTH, MemoryAccess.CellEmailBcon);
			Native.Function.PushLongString(text);
			Native.Function.Call(Native.Hash.SET_TEXT_FONT, font);
			Native.Function.Call(Native.Hash.SET_TEXT_SCALE, scale, scale);

			return Native.Function.Call<float>(Native.Hash._END_TEXT_COMMAND_GET_WIDTH, 1);
		}

		private void DoUpdateCheck()
		{
			Version curVersion = Assembly.GetExecutingAssembly().GetName().Version;
			WebClient webclient = new WebClient();
			webclient.Headers.Add("user-agent", UpdateCheckUserAgent);

			try
			{
				String response = webclient.DownloadString(UpdateCheckUrl);
				if (!String.IsNullOrEmpty(response))
				{
					Regex regex = new Regex(UpdateCheckPattern);
					Match match = regex.Match(response);

					if (match.Success)
					{
						Version fetchedVersion = new Version(match.Groups[1].Value);
						if (fetchedVersion > curVersion)
							Info("There is a new SHV.NET Version available: ~y~v{0}", fetchedVersion);
						else
							Info("You're running on the latest SHV.NET Version");
						return;
					}
				}
			}
			catch (Exception e)
			{
				Warn("SHV.NET Update-Check failed: {0}", e.Message);
				return;
			}
			Warn("SHV.NET Update-Check failed");

			webclient.Dispose();
		}
	}

	public class DefaultConsoleCommands
	{
		[ConsoleCommand("Prints the default Help-Output")]
		public static void Help()
		{
			ScriptDomain.CurrentDomain.Console.PrintHelpString();
		}

		[ConsoleCommand("Prints the Help-Output for the specific command")]
		public static void Help(string command) //TODO Add all commands
		{

		}

		[ConsoleCommand("Loads scripts from a file")]
		public static void Load(string filename)
		{
			String basedirectory = ScriptDomain.CurrentDomain.AppDomain.BaseDirectory;

			if (!File.Exists(Path.Combine(basedirectory, filename)))
			{
				string[] files = Directory.GetFiles(basedirectory, filename, SearchOption.AllDirectories);

				if (files.Length != 1)
				{
					Console.Error("The file '" + filename + "' was not found in '" + basedirectory + "'");
					return;
				}

				Console.Warn("The file '" + filename + "' was not found in '" + basedirectory + "', loading from '" + Path.GetDirectoryName(files[0].Substring(basedirectory.Length + 1)) + "' instead");

				filename = files[0].Substring(basedirectory.Length + 1);
			}
			else
			{
				filename = Path.Combine(basedirectory, filename);
			}

			filename = Path.GetFullPath(filename);

			String extension = Path.GetExtension(filename).ToLower();

			if (extension != ".cs" && extension != ".vb" && extension != ".dll")
			{
				Console.Error("The file '" + filename + "' was not recognized as a script file");
				return;
			}

			foreach (var script in ScriptDomain.CurrentDomain.RunningScripts)
			{
				if (filename.Equals(script.Filename, StringComparison.OrdinalIgnoreCase) && script._running)
				{
					Console.Error("The script is already running");
					return;
				}
			}

			ScriptDomain.CurrentDomain.StartScript(filename);
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
			AbortAllScripts();
			ScriptDomain.CurrentDomain.StartAllScripts();
		}

		[ConsoleCommand("List all loaded scripts")]
		public static void List()
		{
			var scripts = new List<Script>(ScriptDomain.CurrentDomain.RunningScripts);
			scripts.Remove(ScriptDomain.CurrentDomain.Console);

			if (scripts.Count == 0)
			{
				Console.Info("There are no scripts loaded");
				return;
			}

			String basedirectory = ScriptDomain.CurrentDomain.AppDomain.BaseDirectory;

			Console.Info("---");
			foreach (var script in scripts)
			{
				String filename = script.Filename;

				if (filename.StartsWith(basedirectory, StringComparison.OrdinalIgnoreCase))
				{
					filename = filename.Substring(basedirectory.Length + 1);
				}

				Console.Info("   " + filename + ": " + script.Name + (script._running ? " ~g~[running]" : " ~r~[aborted]"));
			}
			Console.Info("---");
		}

		[ConsoleCommand("Aborts all scripts from the specified file")]
		public static void Abort(string filename)
		{
			String basedirectory = ScriptDomain.CurrentDomain.AppDomain.BaseDirectory;

			filename = Path.Combine(basedirectory, filename);

			if (!File.Exists(filename))
			{
				Console.Error("The file '" + filename + "' was not found");
				return;
			}

			ScriptDomain.CurrentDomain.AbortScript(filename);
		}

		[ConsoleCommand("Aborts all scripts found in the script folder")]
		public static void AbortAllScripts()
		{
			ScriptDomain.CurrentDomain.AbortAllScriptsExceptConsole();
		}

		[ConsoleCommand("Clears the console output")]
		public static void Clear()
		{
			ScriptDomain.CurrentDomain.Console.Clear();
		}
	}

	public struct Console
	{
		//TODO Maybe change the structure, we basically just create delegate methods?
		public static void Info(string msg, params object[] args)
		{
			ScriptDomain.CurrentDomain.Console.Info(msg, args);
		}
		public static void Error(string msg, params object[] args)
		{
			ScriptDomain.CurrentDomain.Console.Error(msg, args);
		}
		public static void Warn(string msg, params object[] args)
		{
			ScriptDomain.CurrentDomain.Console.Warn(msg, args);
		}
		public static void Debug(string msg, params object[] args)
		{
			ScriptDomain.CurrentDomain.Console.Debug(msg, args);
		}
	}
}
