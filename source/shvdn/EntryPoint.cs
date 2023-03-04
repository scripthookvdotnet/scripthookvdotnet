using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;
using mscoree;
using System.Linq;

namespace SHVDN
{
	// 32 bit bool for interop
	struct BOOL
	{
		int value;
		public static implicit operator bool(BOOL b) => b.value != 0;
		public static implicit operator BOOL(bool b) => new() { value = b ? 1 : 0 };
	}
	public unsafe class EntryPoint
	{
		public const string KEY_CLR_INITFUNC = "SHVDN.CLR.InitFuncAddr";
		public const string KEY_CLR_TICKFUNC = "SHVDN.CLR.TickFuncAddr";
		public const string KEY_CLR_KBHFUNC = "SHVDN.CLR.KeyboardHandlerFuncAddr";
		public const string KEY_ASI_GETTLSFUNC = "SHVDN.ASI.GetTlsFuncAddr";
		public const string KEY_ASI_SETTLSFUNC = "SHVDN.ASI.SetTlsFuncAddr";
		public const string KEY_ASI_HMODULE = "SHVDN.ASI.ModuleHandle";
		public const string KEY_ASI_PTRRELOADED = "SHVDN.ASI.PtrGameReloaded";

		delegate void VoidDelegate();
		delegate void KeyboardEventDelegate(uint keycode, bool keydown, bool ctrl, bool shift, bool alt);

		// Used to keep reference as the unmnagaed entry created by
		// Marshal.GetFunctionPointerForDelegate will be destroyed/GC'ed after some timeout.
		static readonly VoidDelegate _initFunc = DoInit;
		static readonly VoidDelegate _tickFunc = DoTick;
		static readonly KeyboardEventDelegate _keyboardFunc = DoKeyboard;

		static Console _console;
		static ScriptDomain _domain;
		static Keys _reloadKey;
		static Keys _consoleKey;
		static bool* _pGameReloaded;

		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
		static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool FreeLibrary(IntPtr hModule);
		static int Main(string[] args)
		{
			try
			{
				var asiPath = Environment.GetEnvironmentVariable("SHVDN_ASI_PATH");
				Debug.Assert(asiPath != null);
				var asiModule = LoadLibrary(asiPath);
				Debug.Assert(asiModule != default);
				ScriptDomain.ModuleSetUp(asiModule);
				ScriptDomain.SetPtr(KEY_CLR_INITFUNC, Marshal.GetFunctionPointerForDelegate(_initFunc));
				ScriptDomain.SetPtr(KEY_CLR_TICKFUNC, Marshal.GetFunctionPointerForDelegate(_tickFunc));
				ScriptDomain.SetPtr(KEY_CLR_KBHFUNC, Marshal.GetFunctionPointerForDelegate(_keyboardFunc));
				Debug.Assert(FreeLibrary(asiModule)); // Reduce the ref count so it can actually get unloaded when SHV reloads
				return 0;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return -1;
			}
		}

		static IList<AppDomain> GetAppDomains()
		{
			IList<AppDomain> _IList = new List<AppDomain>();
			IntPtr enumHandle = IntPtr.Zero;
			ICorRuntimeHost host = new CorRuntimeHost();
			try
			{
				host.EnumDomains(out enumHandle);
				object domain = null;
				while (true)
				{
					host.NextDomain(enumHandle, out domain);
					if (domain == null) break;
					AppDomain appDomain = (AppDomain)domain;
					_IList.Add(appDomain);
				}
				return _IList;
			}
			catch (Exception e)
			{
				Log.Message(Log.Level.Error, "Failed to list AppDomains", e.ToString());
				return null;
			}
			finally
			{
				host.CloseEnum(enumHandle);
				Marshal.ReleaseComObject(host);
			}
		}
		static void DoInit()
		{
			try
			{
				List<string> stashedConsoleCommandHistory = new();

				// Unload previous domains
				foreach (var appDomain in GetAppDomains().Where(x => x.FriendlyName.StartsWith("ScriptDomain")))
				{
					var domain = appDomain.GetData("ScriptDomain") as ScriptDomain;
					var console = appDomain?.GetData("Console") as Console;
					if (domain != null)
					{
						// Stash the command history if console is loaded 
						if (console != null)
						{
							stashedConsoleCommandHistory = console.CommandHistory;
						}
						if (domain.AppDomain.Id == _domain?.AppDomain.Id)
						{
							// Clear remote object
							_domain = null;
							_console = null;
						}
						ScriptDomain.Unload(domain);
					}
				}



				// Clear log from previous runs
				Log.Clear();

				// Load configuration
				var scriptPath = "scripts";

				try
				{
					string[] config = File.ReadAllLines(Path.ChangeExtension(typeof(ScriptDomain).Assembly.Location, ".ini"));

					foreach (var l in config)
					{
						// Perform some very basic key/value parsing
						var line = l.Trim();
						if (line.StartsWith("//"))
							continue;
						string[] data = line.Split('=');
						if (data.Length != 2)
							continue;

						if (data[0] == "ReloadKey")
							Enum.TryParse(data[1], true, out _reloadKey);
						else if (data[0] == "ConsoleKey")
							Enum.TryParse(data[1], true, out _consoleKey);
						else if (data[0] == "ScriptsLocation")
							scriptPath = data[1];
					}
				}
				catch (Exception ex)
				{
					Log.Message(Log.Level.Error, "Failed to load config: ", ex.ToString());
				}

				// Create a separate script domain
				_domain = ScriptDomain.Load(".", scriptPath);
				if (_domain == null)
					return;

				_domain.AppDomain.SetData("ScriptDomain", _domain);

				// Set up TLS, and the asi module to import core functions
				_domain.SetUp(ScriptDomain.AsiModule);

				try
				{
					// Instantiate console inside script domain, so that it can access the scripting API
					_console = (Console)_domain.AppDomain.CreateInstanceFromAndUnwrap(
						typeof(ScriptDomain).Assembly.Location, typeof(Console).FullName);

					// Restore the console command history (set a empty history for the first time)
					_console.CommandHistory = stashedConsoleCommandHistory;

					// Get latest api assembly version
					var version = _domain.AppDomain.GetAssemblies()
						.Where(asm => asm.GetName().Name.StartsWith("ScriptHookVDotNet"))
						.Select(asm => asm.GetName().Version).Max();

					// Print welcome message
					_console.PrintInfo($"~c~--- Community Script Hook V .NET {version} ---");
					_console.PrintInfo("~c~--- Type \"Help()\" to print an overview of available commands ---");

					// Update console pointer in script domain
					_domain.AppDomain.SetData("Console", _console);
					_domain.AppDomain.DoCallBack(new CrossAppDomainDelegate(SetConsole));

					// Add default console commands
					_console.RegisterCommands(typeof(EntryPoint));
				}
				catch (Exception ex)
				{
					Log.Message(Log.Level.Error, "Failed to create console: ", ex.ToString());
				}

				// Start scripts in the newly created domain
				_domain.Start();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "ScriptHookVDotNet error");
			}
		}

		static void DoTick()
		{
			try
			{
				_console?.DoTick();
				_domain?.DoTick();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "ScriptHookVDotNet error");
			}
		}

		static void DoKeyboard(uint keycode, bool keydown, bool ctrl, bool shift, bool alt)
		{

			// Filter out invalid key codes
			if (keycode <= 0 || keycode >= 256)
				return;

			// Convert message into a key event
			var key = (Keys)keycode;
			if (ctrl) key |= Keys.Control;
			if (shift) key |= Keys.Shift;
			if (alt) key |= Keys.Alt;

			if (_console == null)
				return;
			try
			{
				if (_console != null)
				{
					if (keydown && key == _reloadKey)
					{
						// Force a reload
						Reload();
						return;
					}
					if (keydown && key == _consoleKey)
					{
						// Toggle open state
						_console.IsOpen = !_console.IsOpen;
						return;
					}

					// Send key events to console
					_console.DoKeyEvent(key, keydown);

					// Do not send keyboard events to other running scripts when console is open
					if (_console.IsOpen)
						return;
				}

				_domain?.DoKeyEvent(key, keydown);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "ScriptHookVDotNet error");
			}
		}


		[ConsoleCommand("Print the default help")]
		public static void Help()
		{
			_console.PrintInfo("~c~--- Help ---");
			_console.PrintInfo("The console accepts ~h~C# expressions~h~ as input and has full access to the scripting API. To print the result of an expression, simply add \"return\" in front of it.");
			_console.PrintInfo("You can use \"P\" as a shortcut for the player character and \"V\" for the current vehicle (without the quotes).");
			_console.PrintInfo("Example: \"return P.IsInVehicle()\" will print a boolean value indicating whether the player is currently sitting in a vehicle to the console.");
			_console.PrintInfo("~c~--- Commands ---");
			_console.PrintHelpText();
		}

		[ConsoleCommand("Print the help for a specific command")]
		public static void Help(String command)
		{
			_console.PrintHelpText(command);
		}

		[ConsoleCommand("Clear the console history and pages")]
		public static void Clear()
		{
			_console.Clear();
		}

		[ConsoleCommand("Reload all scripts from the scripts directory")]
		public static void Reload()
		{
			_console.PrintInfo("~y~Reloading ...");

			// Force a reload on next tick
			*_pGameReloaded = true;
		}

		[ConsoleCommand("Load scripts from a file")]
		public static void Start(String filename)
		{
			if (!Path.IsPathRooted(filename))
				filename = Path.Combine(_domain.ScriptPath, filename);
			if (!Path.HasExtension(filename))
				filename += ".dll";
			String ext = Path.GetExtension(filename).ToLower();
			if (!File.Exists(filename) || (ext != ".cs" && ext != ".vb" && ext != ".dll"))
			{
				_console.PrintError(Path.GetFileName(filename) + " is not a script file!");
				return;
			}

			_domain.StartScripts(filename);
		}

		[ConsoleCommand("Abort all scripts from a file")]
		public static void Abort(String filename)
		{
			if (!Path.IsPathRooted(filename))
				filename = Path.Combine(_domain.ScriptPath, filename);
			if (!Path.HasExtension(filename))
				filename += ".dll";
			String ext = Path.GetExtension(filename).ToLower();
			if (!File.Exists(filename) || (ext != ".cs" && ext != ".vb" && ext != ".dll"))
			{
				_console.PrintError(Path.GetFileName(filename) + " is not a script file!");
				return;
			}

			_domain.AbortScripts(filename);
		}

		[ConsoleCommand("Abort all scripts currently running")]
		public static void AbortAll()
		{
			_domain.Abort();

			_console.PrintInfo("Stopped all running scripts. Use \"Start(filename)\" to start them again.");
		}

		[ConsoleCommand("List all loaded scripts")]
		public static void ListScripts()
		{
			_console.PrintInfo("~c~--- Loaded Scripts ---");
			foreach (var script in _domain.RunningScripts)

				_console.PrintInfo(Path.GetFileName(script.Filename) + " ~h~" + script.Name + (script.IsRunning ? (script.IsPaused ? " ~o~[paused]" : " ~g~[running]") : " ~r~[aborted]"));
		}

		public static void SetConsole()
		{
			_domain = ScriptDomain.CurrentDomain;
			_pGameReloaded = (bool*)ScriptDomain.GetPtr("SHVDN.ASI.PtrGameReloaded");
			_console = (Console)AppDomain.CurrentDomain.GetData("Console");
			Debug.Assert(_pGameReloaded != null && _console != null && _domain != null);
		}
	}
}
