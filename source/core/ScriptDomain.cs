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
using System.IO;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinForms = System.Windows.Forms;

namespace GTA
{
	interface IScriptTask
	{
		void Run();
	}

	internal class ScriptDomain : MarshalByRefObject, IDisposable
	{
		#region Fields
		private int _executingThreadId;
		private Script _executingScript;
		private List<Script> _runningScripts = new List<Script>();
		private Queue<IScriptTask> _taskQueue = new Queue<IScriptTask>();
		private List<IntPtr> _pinnedStrings = new List<IntPtr>();
		private List<Tuple<string, Type>> _scriptTypes = new List<Tuple<string, Type>>();
		private bool _recordKeyboardEvents = true;
		private bool[] _keyboardState = new bool[256];
		private bool disposed = false;
		#endregion

		public ScriptDomain()
		{
			AppDomain = AppDomain.CurrentDomain;
			AppDomain.AssemblyResolve += new ResolveEventHandler(HandleResolve);
			AppDomain.UnhandledException += new UnhandledExceptionEventHandler(HandleUnhandledException);

			CurrentDomain = this;

			_executingThreadId = Thread.CurrentThread.ManagedThreadId;

			Log("[INFO]", "Created new script domain with v", typeof(ScriptDomain).Assembly.GetName().Version.ToString(3), ".");

			Console = new ConsoleScript();
		}
		~ScriptDomain()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			CleanupStrings();

			disposed = true;
		}

		public static Script ExecutingScript => CurrentDomain != null ? CurrentDomain._executingScript : null;
		public static ScriptDomain CurrentDomain { get; private set; }
		public string Name => AppDomain.FriendlyName;
		public AppDomain AppDomain { get; private set; }
		public ConsoleScript Console { get; private set; }
		public Script[] RunningScripts => _runningScripts.ToArray();

		public static ScriptDomain Load(string path)
		{
			if (!Path.IsPathRooted(path))
			{
				path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
			}

			path = Path.GetFullPath(path);

			// Clear log
			string logPath = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".log");

			try
			{
				File.WriteAllText(logPath, string.Empty);
			}
			catch
			{
			}

			// Create AppDomain
			var setup = new AppDomainSetup();
			setup.ApplicationBase = path;
			setup.ShadowCopyFiles = "true";
			setup.ShadowCopyDirectories = path;

			var appdomain = AppDomain.CreateDomain("ScriptDomain_" + (path.GetHashCode() * Environment.TickCount).ToString("X"), null, setup, new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted));
			appdomain.InitializeLifetimeService();

			ScriptDomain scriptdomain = null;

			try
			{
				scriptdomain = (ScriptDomain)(appdomain.CreateInstanceFromAndUnwrap(typeof(ScriptDomain).Assembly.Location, typeof(ScriptDomain).FullName));
			}
			catch (Exception ex)
			{
				Log("[ERROR]", "Failed to create script domain':", Environment.NewLine, ex.ToString());

				AppDomain.Unload(appdomain);

				return null;
			}

			Log("[INFO]", "Loading scripts from '", path, "' ...");

			if (Directory.Exists(path))
			{
				var filenameScripts = new List<string>();
				var filenameAssemblies = new List<string>();

				try
				{
					filenameScripts.AddRange(Directory.GetFiles(path, "*.vb", SearchOption.AllDirectories));
					filenameScripts.AddRange(Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories));
					filenameAssemblies.AddRange(Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories).Where(x => IsManagedAssembly(x)));
				}
				catch (Exception ex)
				{
					Log("[ERROR]", "Failed to reload scripts:", Environment.NewLine, ex.ToString());

					AppDomain.Unload(appdomain);

					return null;
				}

				for (int i = 0; i < filenameAssemblies.Count; i++)
				{
					var filename = filenameAssemblies[i];
					var assemblyName = AssemblyName.GetAssemblyName(filename);

					try
					{
						if (assemblyName.Name.StartsWith("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase))
						{
							Log("[WARNING]", "Removing assembly file '", Path.GetFileName(filename), "'.");

							filenameAssemblies.RemoveAt(i--);

							try
							{
								File.Delete(filename);
							}
							catch (Exception ex)
							{
								Log("[ERROR]", "Failed to delete assembly file:", Environment.NewLine, ex.ToString());
							}
						}
					}
					catch (Exception ex)
					{
						Log("[ERROR]", "Failed to load assembly file '", Path.GetFileName(filename), "':", Environment.NewLine, ex.ToString());
					}
				}

				foreach (string filename in filenameScripts)
				{
					scriptdomain.LoadScript(filename);
				}
				foreach (string filename in filenameAssemblies)
				{
					scriptdomain.LoadAssembly(filename);
				}
			}
			else
			{
				Log("[ERROR]", "Failed to reload scripts because the directory is missing.");
			}

			return scriptdomain;
		}

		private bool LoadScript(string filename)
		{
			var compilerOptions = new System.CodeDom.Compiler.CompilerParameters();
			compilerOptions.CompilerOptions = "/optimize";
			compilerOptions.GenerateInMemory = true;
			compilerOptions.IncludeDebugInformation = true;
			compilerOptions.ReferencedAssemblies.Add("System.dll");
			compilerOptions.ReferencedAssemblies.Add("System.Core.dll");
			compilerOptions.ReferencedAssemblies.Add("System.Drawing.dll");
			compilerOptions.ReferencedAssemblies.Add("System.Windows.Forms.dll");
			compilerOptions.ReferencedAssemblies.Add("System.XML.dll");
			compilerOptions.ReferencedAssemblies.Add("System.XML.Linq.dll");
			compilerOptions.ReferencedAssemblies.Add(typeof(Script).Assembly.Location);

			string extension = Path.GetExtension(filename);
			System.CodeDom.Compiler.CodeDomProvider compiler = null;

			if (extension.Equals(".cs", StringComparison.OrdinalIgnoreCase))
			{
				compiler = new Microsoft.CSharp.CSharpCodeProvider();
				compilerOptions.CompilerOptions += " /unsafe";
			}
			else if (extension.Equals(".vb", StringComparison.OrdinalIgnoreCase))
			{
				compiler = new Microsoft.VisualBasic.VBCodeProvider();
			}
			else
			{
				return false;
			}

			System.CodeDom.Compiler.CompilerResults compilerResult = compiler.CompileAssemblyFromFile(compilerOptions, filename);

			if (!compilerResult.Errors.HasErrors)
			{
				Log("[INFO]", "Successfully compiled '", Path.GetFileName(filename), "'.");

				return LoadAssembly(filename, compilerResult.CompiledAssembly);
			}
			else
			{
				var errors = new System.Text.StringBuilder();

				foreach (System.CodeDom.Compiler.CompilerError error in compilerResult.Errors)
				{
					errors.Append("   at line ");
					errors.Append(error.Line);
					errors.Append(": ");
					errors.Append(error.ErrorText);
					errors.AppendLine();
				}

				Log("[ERROR]", "Failed to compile '", Path.GetFileName(filename), "' with ", compilerResult.Errors.Count.ToString(), " error(s):", Environment.NewLine, errors.ToString());

				return false;
			}
		}
		private bool LoadAssembly(string filename)
		{
			Log("[INFO]", "Loading assembly '", Path.GetFileName(filename), "' ...");

			Assembly assembly = null;

			try
			{
				assembly = Assembly.Load(File.ReadAllBytes(filename));
			}
			catch (Exception ex)
			{
				Log("[ERROR]", "Failed to load assembly '", Path.GetFileName(filename), "':", Environment.NewLine, ex.ToString());

				return false;
			}

			return LoadAssembly(filename, assembly);
		}
		private bool LoadAssembly(string filename, Assembly assembly)
		{
			string version = (Path.GetExtension(filename) == ".dll" ? (" v" + assembly.GetName().Version.ToString(3)) : string.Empty);
			uint count = 0;

			try
			{
				foreach (var type in assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Script))))
				{
					count++;
					_scriptTypes.Add(new Tuple<string, Type>(filename, type));
				}
			}
			catch (ReflectionTypeLoadException ex)
			{
				var fileNotFoundException = (FileNotFoundException)(ex.LoaderExceptions[0]);

				if (ReferenceEquals(fileNotFoundException, null) || fileNotFoundException.Message.IndexOf("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase) < 0)
				{
					Log("[ERROR]", "Failed to load assembly '", Path.GetFileName(filename), version, "':", Environment.NewLine, ex.LoaderExceptions[0].ToString());
				}

				return false;
			}

			Log("[INFO]", "Found ", count.ToString(), " script(s) in '", Path.GetFileName(filename), version, "'.");

			return count != 0;
		}
		public static bool IsManagedAssembly(string fileName)
		{
			using (Stream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			using (BinaryReader binaryReader = new BinaryReader(fileStream))
			{
				if (fileStream.Length < 64)
				{
					return false;
				}

				//PE Header starts @ 0x3C (60). Its a 4 byte header.
				fileStream.Position = 0x3C;
				uint peHeaderPointer = binaryReader.ReadUInt32();
				if (peHeaderPointer == 0)
				{
					peHeaderPointer = 0x80;
				}

				// Ensure there is at least enough room for the following structures:
				//     24 byte PE Signature & Header
				//     28 byte Standard Fields         (24 bytes for PE32+)
				//     68 byte NT Fields               (88 bytes for PE32+)
				// >= 128 byte Data Dictionary Table
				if (peHeaderPointer > fileStream.Length - 256)
				{
					return false;
				}

				// Check the PE signature.  Should equal 'PE\0\0'.
				fileStream.Position = peHeaderPointer;
				uint peHeaderSignature = binaryReader.ReadUInt32();
				if (peHeaderSignature != 0x00004550)
				{
					return false;
				}

				// skip over the PEHeader fields
				fileStream.Position += 20;

				const ushort PE32 = 0x10b;
				const ushort PE32Plus = 0x20b;

				// Read PE magic number from Standard Fields to determine format.
				var peFormat = binaryReader.ReadUInt16();
				if (peFormat != PE32 && peFormat != PE32Plus)
				{
					return false;
				}

				// Read the 15th Data Dictionary RVA field which contains the CLI header RVA.
				// When this is non-zero then the file contains CLI data otherwise not.
				ushort dataDictionaryStart = (ushort)(peHeaderPointer + (peFormat == PE32 ? 232 : 248));
				fileStream.Position = dataDictionaryStart;

				uint cliHeaderRva = binaryReader.ReadUInt32();
				if (cliHeaderRva == 0)
				{
					return false;
				}

				return true;
			}
		}
		public static void Unload(ref ScriptDomain domain)
		{
			Log("[INFO]", "Unloading script domain ...");

			domain.Abort();

			AppDomain appdomain = domain.AppDomain;

			domain.Dispose();

			try
			{
				AppDomain.Unload(appdomain);
			}
			catch (Exception ex)
			{
				Log("[ERROR]", "Failed to unload deleted script domain:", Environment.NewLine, ex.ToString());
			}

			domain = null;
		}
		private Script InstantiateScript(Type scriptType)
		{
			if (!scriptType.IsSubclassOf(typeof(Script)) || scriptType.IsAbstract)
			{
				return null;
			}

			Log("[INFO]", "Instantiating script '", scriptType.FullName, "' ...");

			try
			{
				return (Script)(Activator.CreateInstance(scriptType));
			}
			catch (MissingMethodException)
			{
				Log("[ERROR]", "Failed to instantiate script '", scriptType.FullName, "' because no public default constructor was found.");
			}
			catch (TargetInvocationException ex)
			{
				Log("[ERROR]", "Failed to instantiate script '", scriptType.FullName, "' because constructor threw an exception:", Environment.NewLine, ex.InnerException.ToString());
			}
			catch (Exception ex)
			{
				Log("[ERROR]", "Failed to instantiate script '", scriptType.FullName, "':", Environment.NewLine, ex.ToString());
			}

			string supportURL = GetScriptSupportURL(scriptType);

			if (supportURL != null)
			{
				Log("[INFO]", "Please check the following site for support on the issue: ", supportURL);
			}

			return null;
		}

		internal static bool SortScripts(ref List<Tuple<string, Type>> scriptTypes)
		{
			var graph = new Dictionary<Tuple<string, Type>, List<Type>>();

			foreach (var scriptType in scriptTypes)
			{
				var dependencies = new List<Type>();

				foreach (RequireScript attribute in (scriptType.Item2).GetCustomAttributes<RequireScript>(true))
				{
					dependencies.Add(attribute._dependency);
				}

				graph.Add(scriptType, dependencies);
			}

			var result = new List<Tuple<string, Type>>(graph.Count);

			while (graph.Count > 0)
			{
				Tuple<string, Type> scriptType = null;

				foreach (var item in graph)
				{
					if (item.Value.Count == 0)
					{
						scriptType = item.Key;
						break;
					}
				}

				if (scriptType == null)
				{
					Log("[ERROR]", "Detected a circular script dependency. Aborting ...");
					return false;
				}

				result.Add(scriptType);
				graph.Remove(scriptType);

				foreach (var item in graph)
				{
					item.Value.Remove(scriptType.Item2);
				}
			}

			scriptTypes = result;

			return true;
		}
		public void Start()
		{
			if (_runningScripts.Count != 0)
			{
				return;
			}

			// Start console
			Console.Start();

			// Start script threads
			Log("[INFO]", "Starting ", _scriptTypes.Count.ToString(), " script(s) ...");

			if (_scriptTypes.Count == 0 || !SortScripts(ref _scriptTypes))
			{
				return;
			}

			foreach (var script in _scriptTypes.Select(x => InstantiateScript(x.Item2)).Where(x => x != null))
			{
				script.Start();
			}
		}
		public void StartScript(string filename)
		{
			filename = Path.GetFullPath(filename);

			int offset = _scriptTypes.Count;
			string extension = Path.GetExtension(filename);

			if (extension.Equals(".dll", StringComparison.OrdinalIgnoreCase))
			{
				if (!(IsManagedAssembly(filename) && LoadAssembly(filename)))
				{
					return;
				}
			}
			else if (!LoadScript(filename))
			{
				return;
			}

			Log("[INFO]", "Starting ", (_scriptTypes.Count - offset).ToString(), " script(s) ...");

			for (int i = offset; i < _scriptTypes.Count; i++)
			{
				Script script = InstantiateScript(_scriptTypes[i].Item2);

				if (Object.ReferenceEquals(script, null))
				{
					continue;
				}

				script.Start();
			}
		}
		public void StartAllScripts()
		{
			string basedirectory = CurrentDomain.AppDomain.BaseDirectory;

			if (Directory.Exists(basedirectory))
			{
				var filenameScripts = new List<string>();

				try
				{
					filenameScripts.AddRange(Directory.GetFiles(basedirectory, "*.vb", SearchOption.AllDirectories));
					filenameScripts.AddRange(Directory.GetFiles(basedirectory, "*.cs", SearchOption.AllDirectories));
					filenameScripts.AddRange(Directory.GetFiles(basedirectory, "*.dll", SearchOption.AllDirectories).Where(x => IsManagedAssembly(x)));
				}
				catch (Exception ex)
				{
					Log("[ERROR]", "Failed to reload scripts:", Environment.NewLine, ex.ToString());
				}

				int offset = _scriptTypes.Count;

				foreach (var filename in filenameScripts)
				{
					string extension = Path.GetExtension(filename).ToLower();

					if (extension.Equals(".dll", StringComparison.OrdinalIgnoreCase) ? !LoadAssembly(filename) : !LoadScript(filename))
					{
						continue;
					}
				}

				int TotalScriptCount = _scriptTypes.Count;

				Log("[INFO]", "Starting ", (TotalScriptCount - offset).ToString(), " script(s) ...");

				for (int i = offset; i < TotalScriptCount; i++)
				{
					Script script = InstantiateScript(_scriptTypes[i].Item2);

					if (ReferenceEquals(script, null))
					{
						continue;
					}

					script.Start();
				}
			}
		}
		public void Abort()
		{
			_runningScripts.Remove(Console);

			Log("[INFO]", "Stopping ", _runningScripts.Count.ToString(), " script(s) ...");

			foreach (Script script in _runningScripts)
			{
				script.Abort();
			}

			Console.Abort();

			_scriptTypes.Clear();
			_runningScripts.Clear();
		}
		public void AbortScript(string filename)
		{
			filename = Path.GetFullPath(filename);

			foreach (Script script in _runningScripts.Where(x => filename.Equals(x.Filename, StringComparison.OrdinalIgnoreCase)))
			{
				script.Abort();
			}
		}
		public void AbortAllScriptsExceptConsole()
		{
			foreach (Script script in _runningScripts.Where(x => x != Console))
			{
				script.Abort();
			}

			_scriptTypes.Clear();
			_runningScripts.RemoveAll(x => x != Console);
		}
		public static void OnStartScript(Script script)
		{
			ScriptDomain domain = script._scriptdomain;

			domain._runningScripts.Add(script);

			if (ReferenceEquals(script, domain.Console))
			{
				return;
			}

			domain.Console.RegisterCommands(script.GetType());

			Log("[INFO]", "Started script '", script.Name, "'.");
		}
		public static void OnAbortScript(Script script)
		{
			ScriptDomain domain = script._scriptdomain;

			if (ReferenceEquals(script, domain.Console))
			{
				return;
			}

			domain.Console.UnregisterCommands(script.GetType());

			Log("[INFO]", "Aborted script '", script.Name, "'.");
		}

		public void DoTick()
		{
			// Execute scripts
			for (int i = 0; i < _runningScripts.Count; i++)
			{
				Script script = _runningScripts[i];

				if (!script._running)
				{
					continue;
				}

				_executingScript = script;

				while ((script._running = SignalAndWait(script._continueEvent, script._waitEvent, 5000)) && _taskQueue.Count > 0)
				{
					_taskQueue.Dequeue().Run();
				}

				_executingScript = null;

				if (!script._running)
				{
					Log("[ERROR]", "Script '", script.Name, "' is not responding! Aborting ...");

					OnAbortScript(script);
					continue;
				}
			}

			// Clean up pinned strings
			CleanupStrings();
		}
		public void DoKeyboardMessage(WinForms.Keys key, bool status, bool statusCtrl, bool statusShift, bool statusAlt)
		{
			int keycode = (int)key;

			if (keycode < 0 || keycode >= _keyboardState.Length)
			{
				return;
			}

			_keyboardState[keycode] = status;

			if (_recordKeyboardEvents)
			{
				if (statusCtrl)
				{
					key = key | WinForms.Keys.Control;
				}
				if (statusShift)
				{
					key = key | WinForms.Keys.Shift;
				}
				if (statusAlt)
				{
					key = key | WinForms.Keys.Alt;
				}

				var args = new WinForms.KeyEventArgs(key);
				var eventinfo = new Tuple<bool, WinForms.KeyEventArgs>(status, args);

				if (!ReferenceEquals(Console, null) && Console.IsOpen)
				{
					// Do not send keyboard events to other running scripts when console is open
					Console._keyboardEvents.Enqueue(eventinfo);
				}
				else
				{
					foreach (Script script in _runningScripts)
					{
						script._keyboardEvents.Enqueue(eventinfo);
					}
				}
			}
		}

		public void PauseKeyboardEvents(bool pause)
		{
			_recordKeyboardEvents = !pause;
		}
		public void ExecuteTask(IScriptTask task)
		{
			if (Thread.CurrentThread.ManagedThreadId == _executingThreadId)
			{
				task.Run();
			}
			else
			{
				_taskQueue.Enqueue(task);

				SignalAndWait(ExecutingScript._waitEvent, ExecutingScript._continueEvent);
			}
		}
		public IntPtr PinString(string str)
		{
			IntPtr handle = Native.MemoryAccess.StringToCoTaskMemUTF8(str);

			if (handle == IntPtr.Zero)
			{
				return Native.MemoryAccess.NullString;
			}
			else
			{
				_pinnedStrings.Add(handle);

				return handle;
			}
		}
		private void CleanupStrings()
		{
			foreach (IntPtr handle in _pinnedStrings)
			{
				Marshal.FreeCoTaskMem(handle);
			}

			_pinnedStrings.Clear();
		}
		public string LookupScriptFilename(Script script)
		{
			return LookupScriptFilename(script.GetType());
		}
		public string LookupScriptFilename(Type type)
		{
			return _scriptTypes.FirstOrDefault(x => x.Item2 == type)?.Item1 ?? string.Empty;
		}
		public override object InitializeLifetimeService()
		{
			return null;
		}

		internal bool IsKeyPressed(WinForms.Keys key)
		{
			return _keyboardState[(int)(key)];
		}

		static private void SignalAndWait(SemaphoreSlim toSignal, SemaphoreSlim toWaitOn)
		{
			toSignal.Release();
			toWaitOn.Wait();
		}
		static private bool SignalAndWait(SemaphoreSlim toSignal, SemaphoreSlim toWaitOn, int timeout)
		{
			toSignal.Release();
			return toWaitOn.Wait(timeout);
		}

		static private string GetScriptSupportURL(Type scriptType)
		{
			foreach (ScriptAttributes attribute in scriptType.GetCustomAttributes<ScriptAttributes>(true))
			{
				if (!String.IsNullOrEmpty(attribute.SupportURL))
				{
					return attribute.SupportURL;
				}
			}

			return null;
		}

		static private void Log(string logLevel, params string[] message)
		{
			var datetime = DateTime.Now;
			string logPath = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".log");

			try
			{
				var fs = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.Read);
				var sw = new StreamWriter(fs);

				try
				{
					sw.Write(string.Concat("[", datetime.ToString("HH:mm:ss"), "] ", logLevel, " "));

					foreach (string str in message)
					{
						sw.Write(str);
					}

					sw.WriteLine();
				}
				finally
				{
					sw.Close();
					fs.Close();
				}
			}
			catch (Exception)
			{
			}

			if (ReferenceEquals(CurrentDomain, null))
			{
				return;
			}

			var console = CurrentDomain.Console;

			if (!ReferenceEquals(console, null))
			{
				if (logLevel == "[INFO]")
				{
					console.Info(string.Join(string.Empty, message));
					return;
				}
				if (logLevel == "[ERROR]")
				{
					console.Error(string.Join(string.Empty, message));
					return;
				}
				if (logLevel == "[WARNING]")
				{
					console.Warn(string.Join(string.Empty, message));
					return;
				}
			}
		}
		static private Assembly HandleResolve(Object sender, ResolveEventArgs args)
		{
			var assembly = typeof(Script).Assembly;
			var assemblyName = new AssemblyName(args.Name);

			if (assemblyName.Name.StartsWith("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase))
			{
				if (assemblyName.Version.Major != assembly.GetName().Version.Major)
				{
					Log("[WARNING]", "A script references v", assemblyName.Version.ToString(3), " which may not be compatible with the current v" + assembly.GetName().Version.ToString(3), " and was therefore ignored.");
				}
				else
				{
					return assembly;
				}
			}

			return null;
		}
		static internal void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			if (!args.IsTerminating)
			{
				Log("[ERROR]", "Caught unhandled exception:", Environment.NewLine, args.ExceptionObject.ToString());
			}
			else
			{
				Log("[ERROR]", "Caught fatal unhandled exception:", Environment.NewLine, args.ExceptionObject.ToString());
			}

			if (sender == null || !typeof(Script).IsInstanceOfType(sender))
			{
				return;
			}

			var scriptType = sender.GetType();

			Log("[INFO]", "The exception was thrown while executing the script '", scriptType.FullName, "'.");

			string supportURL = GetScriptSupportURL(scriptType);

			if (supportURL != null)
			{
				Log("[INFO]", "Please check the following site for support on the issue: ", supportURL);
			}
		}
	}
}
