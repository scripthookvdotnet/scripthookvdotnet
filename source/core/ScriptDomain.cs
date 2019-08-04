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
using System.Windows.Forms;

namespace SHVDN
{
	public interface IScriptTask
	{
		void Run();
	}

	public class ScriptDomain : MarshalByRefObject, IDisposable
	{
		Script executingScript = null;
		List<IntPtr> pinnedStrings = new List<IntPtr>();
		List<Script> runningScripts = new List<Script>();
		Queue<IScriptTask> taskQueue = new Queue<IScriptTask>();
		List<Tuple<string, Type>> scriptTypes = new List<Tuple<string, Type>>();
		bool disposed = false;
		bool recordKeyboardEvents = true;
		bool[] keyboardState = new bool[256];
		Assembly scriptApi = null;

		/// <summary>
		/// Gets the friendly name of this script domain.
		/// </summary>
		public string Name => AppDomain.FriendlyName;
		/// <summary>
		/// Gets the path to the scripting API assembly file.
		/// </summary>
		public string ApiPath { get; private set; }
		/// <summary>
		/// Gets the application domain that is associated with this script domain.
		/// </summary>
		public AppDomain AppDomain { get; private set; } = AppDomain.CurrentDomain;
		
		/// <summary>
		/// Gets the scripting domain for the current application domain.
		/// </summary>
		public static ScriptDomain CurrentDomain { get; private set; }

		/// <summary>
		/// The global list of all existing scripting domains. This is only valid in the main application domain.
		/// </summary>
		public static List<ScriptDomain> Instances = new List<ScriptDomain>();

		/// <summary>
		/// Gets the list of currently running scripts in this script domain. This is used by the console implementation.
		/// </summary>
		public string[] RunningScripts => runningScripts.Select(script => Path.GetFileName(script.Filename) + ": " + script.Name + (script.IsRunning ? " ~g~[running]" : " ~r~[aborted]")).ToArray();
		/// <summary>
		/// Gets the currently executing script or <c>null</c> if there is none.
		/// </summary>
		public static Script ExecutingScript => CurrentDomain != null ? CurrentDomain.executingScript : null;

		/// <summary>
		/// Initializes the script domain inside its application domain.
		/// This constructor is public so that it is found by <see cref="System.AppDomain.CreateInstanceAndUnwrap(string, string)"/>.
		/// </summary>
		/// <param name="apiPath">The path to the scripting API assembly file.</param>
		public ScriptDomain(string apiPath)
		{
			ApiPath = apiPath;
			// Each application domain has its own copy of this static variable, so only need to set it once
			CurrentDomain = this;

			// Attach resolve handler to new domain
			AppDomain.AssemblyResolve += HandleResolve;
			AppDomain.UnhandledException += HandleUnhandledException;

			Log.Message(Log.Level.Info, "Loading API from ", apiPath, " ...");

			// Load API assembly into this script domain
			scriptApi = Assembly.LoadFrom(apiPath);

			Log.Message(Log.Level.Info, "Created new script domain for script API v", scriptApi.GetName().Version.ToString(3), ".");
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
			disposed = true;

			// Need to free native strings when disposing the script domain
			CleanupStrings();
		}

		/// <summary>
		/// Creates a new script domain.
		/// </summary>
		/// <param name="apiPath">The path to the scripting API assembly file.</param>
		/// <param name="scriptPath">The path to the directory containing scripts.</param>
		/// <param name="allowSourceScripts">Whether to allow this script domain to compile and load scripts from source code.</param>
		/// <returns>The script domain or <c>null</c> in case of failure.</returns>
		public static ScriptDomain Load(string apiPath, string scriptPath, bool allowSourceScripts = false)
		{
			// Make absolute path to scrips location
			if (!Path.IsPathRooted(scriptPath))
				scriptPath = Path.Combine(Path.GetDirectoryName(apiPath), scriptPath);
			scriptPath = Path.GetFullPath(scriptPath);

			// Remove handlers first if they already exist, so that the handles are only added once
			AppDomain.CurrentDomain.AssemblyResolve -= HandleResolve;
			AppDomain.CurrentDomain.UnhandledException -= HandleUnhandledException;
			// Need to attach the resolve handler to the current domain too, so that the .NET framework finds this assembly in the ASI file
			AppDomain.CurrentDomain.AssemblyResolve += HandleResolve;
			AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;

			// Create application and script domain for all the scripts to reside in
			var name = "ScriptDomain_" + (apiPath.GetHashCode() ^ scriptPath.GetHashCode() ^ Environment.TickCount).ToString("X");
			var setup = new AppDomainSetup();
			setup.ShadowCopyFiles = "true"; // Copy assemblies into memory rather than locking the file, so they can be updated while the domain is still loaded
			setup.ShadowCopyDirectories = scriptPath; // Only shadow copy files in the scripts directory
			setup.ApplicationBase = scriptPath;

			var appdomain = AppDomain.CreateDomain(name, null, setup, new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted));
			appdomain.InitializeLifetimeService(); // Give the application domain an infinite lifetime

			// Store default domain reference, so it can be used to call back into it (see Log implementation for example)
			appdomain.SetData("DefaultDomain", AppDomain.CurrentDomain);

			ScriptDomain scriptdomain = null;

			try
			{
				scriptdomain = (ScriptDomain)appdomain.CreateInstanceFromAndUnwrap(typeof(ScriptDomain).Assembly.Location, typeof(ScriptDomain).FullName, false, BindingFlags.Default, null, new object[] { apiPath }, null, null);
			}
			catch (Exception ex)
			{
				Log.Message(Log.Level.Error, "Failed to create script domain: ", ex.ToString());
				AppDomain.Unload(appdomain);
				return null;
			}

			scriptdomain.StartAllScripts();

			return scriptdomain;
		}
		/// <summary>
		/// Unloads scripts and destroys an existing script domain.
		/// </summary>
		/// <param name="domain">The script domain to unload.</param>
		public static void Unload(ScriptDomain domain)
		{
			Log.Message(Log.Level.Info, "Unloading script domain ...");

			domain.Abort();
			domain.Dispose();

			try
			{
				AppDomain.Unload(domain.AppDomain);
			}
			catch (Exception ex)
			{
				Log.Message(Log.Level.Error, "Failed to unload script domain: ", ex.ToString());
			}
		}

		/// <summary>
		/// Compiles and load scripts from a C# or VB.NET source code file.
		/// </summary>
		/// <param name="filename">The path to the code file to load.</param>
		/// <returns><c>true</c> on success, <c>false</c> otherwise</returns>
		bool LoadScript(string filename)
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
			compilerOptions.ReferencedAssemblies.Add(scriptApi.Location);
			compilerOptions.ReferencedAssemblies.Add(typeof(ScriptDomain).Assembly.Location);

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
				Log.Message(Log.Level.Info, "Successfully compiled ", Path.GetFileName(filename), ".");
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

				Log.Message(Log.Level.Error, "Failed to compile ", Path.GetFileName(filename), " with ", compilerResult.Errors.Count.ToString(), " error(s):", Environment.NewLine, errors.ToString());
				return false;
			}
		}
		/// <summary>
		/// Loads scripts from the specified assembly file.
		/// </summary>
		/// <param name="filename">The path to the assembly file to load.</param>
		/// <returns><c>true</c> on success, <c>false</c> otherwise</returns>
		bool LoadAssembly(string filename)
		{
			if (!IsManagedAssembly(filename))
				return false;

			Log.Message(Log.Level.Info, "Loading assembly ", Path.GetFileName(filename), " ...");

			Assembly assembly = null;

			try
			{
				assembly = Assembly.Load(File.ReadAllBytes(filename));
			}
			catch (Exception ex)
			{
				Log.Message(Log.Level.Error, "Failed to load assembly ", Path.GetFileName(filename), ":", Environment.NewLine, ex.ToString());
				return false;
			}

			return LoadAssembly(filename, assembly);
		}
		/// <summary>
		/// Loads scripts from the specified assembly object.
		/// </summary>
		/// <param name="filename">The path to the file associated with this assembly.</param>
		/// <param name="assembly">The assembly to load.</param>
		/// <returns><c>true</c> on success, <c>false</c> otherwise</returns>
		bool LoadAssembly(string filename, Assembly assembly)
		{
			int count = 0;
			string name = Path.GetFileName(filename) +
				(Path.GetExtension(filename) == ".dll" ? (" v" + assembly.GetName().Version.ToString(3)) : string.Empty);

			try
			{
				// Find all script types in the assembly
				foreach (var type in assembly.GetTypes().Where(x => x.BaseType.Name == "Script"))
				{
					count++;
					scriptTypes.Add(new Tuple<string, Type>(filename, type));
				}
			}
			catch (ReflectionTypeLoadException ex)
			{
				var fileNotFoundException = (FileNotFoundException)(ex.LoaderExceptions[0]);

				if (ReferenceEquals(fileNotFoundException, null) || fileNotFoundException.Message.IndexOf("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase) < 0)
				{
					Log.Message(Log.Level.Error, "Failed to load assembly ", name, ": ", ex.LoaderExceptions[0].ToString());
				}

				return false;
			}

			Log.Message(Log.Level.Info, "Found ", count.ToString(), " script(s) in ", name, ".");

			return count != 0;
		}

		/// <summary>
		/// Creates an instance of a script.
		/// </summary>
		/// <param name="scriptType">The type of the script to instantiate.</param>
		/// <returns>The script instance or <c>null</c> in case of failure.</returns>
		Script InstantiateScript(Type scriptType)
		{
			if (scriptType.IsAbstract)
				return null;

			Log.Message(Log.Level.Info, "Instantiating script '", scriptType.FullName, "' ...");

			Script script = new Script();
			executingScript = script;

			try
			{
				script.Instance = Activator.CreateInstance(scriptType);
				script.Filename = LookupScriptFilename(scriptType);
				return script;
			}
			catch (MissingMethodException)
			{
				Log.Message(Log.Level.Error, "Failed to instantiate script ", scriptType.FullName, " because no public default constructor was found.");
			}
			catch (TargetInvocationException ex)
			{
				Log.Message(Log.Level.Error, "Failed to instantiate script ", scriptType.FullName, " because constructor threw an exception: ", ex.InnerException.ToString());
			}
			catch (Exception ex)
			{
				Log.Message(Log.Level.Error, "Failed to instantiate script ", scriptType.FullName, ": ", ex.ToString());
			}

			return null;
		}

		static bool SortScripts(ref List<Tuple<string, Type>> scriptTypes)
		{
			//var graph = new Dictionary<Tuple<string, Type>, List<Type>>();

			//foreach (var scriptType in scriptTypes)
			//{
			//	var dependencies = new List<Type>();
			//	foreach (RequireScript attribute in (scriptType.Item2).GetCustomAttributes<RequireScript>(true))
			//		dependencies.Add(attribute._dependency);
			//	graph.Add(scriptType, dependencies);
			//}

			//var result = new List<Tuple<string, Type>>(graph.Count);

			//while (graph.Count > 0)
			//{
			//	Tuple<string, Type> scriptType = null;

			//	foreach (var item in graph)
			//	{
			//		if (item.Value.Count == 0)
			//		{
			//			scriptType = item.Key;
			//			break;
			//		}
			//	}

			//	if (scriptType == null)
			//	{
			//		Log.Message(Log.Level.Error, "Detected a circular script dependency. Aborting ...");
			//		return false;
			//	}

			//	result.Add(scriptType);
			//	graph.Remove(scriptType);

			//	foreach (var item in graph)
			//		item.Value.Remove(scriptType.Item2);
			//}

			//scriptTypes = result;

			return true;
		}

		/// <summary>
		/// Instantiates and starts all loaded scripts. 
		/// </summary>
		public void Start()
		{
			if (runningScripts.Count != 0)
				return; // Scripts are already running, so there is nothing to do.
			if (scriptTypes.Count == 0 || !SortScripts(ref scriptTypes))
				return; // No scripts are loaded.

			foreach (var script in scriptTypes.Select(x => InstantiateScript(x.Item2)).Where(x => x != null))
			{
				script.Start();

				runningScripts.Add(script);
			}
		}
		/// <summary>
		/// Reloads, instantiates and starts all scripts from the specified file.
		/// </summary>
		/// <param name="filename"></param>
		public void StartScripts(string filename)
		{
			filename = Path.GetFullPath(filename);

			int offset = scriptTypes.Count;

			if (Path.GetExtension(filename).Equals(".dll", StringComparison.OrdinalIgnoreCase) ? !LoadAssembly(filename) : !LoadScript(filename))
				return;

			for (int i = offset; i < scriptTypes.Count; i++)
			{
				Script script = InstantiateScript(scriptTypes[i].Item2);

				if (ReferenceEquals(script, null))
					continue;

				script.Start();

				runningScripts.Add(script);
			}
		}
		/// <summary>
		/// Reloads, instantiates and starts all scripts.
		/// </summary>
		public void StartAllScripts()
		{
			string scriptPath = AppDomain.BaseDirectory;

			Log.Message(Log.Level.Info, "Loading scripts from ", scriptPath, " ...");

			if (!Directory.Exists(scriptPath))
			{
				Log.Message(Log.Level.Warning, "Failed to reload scripts because the directory is missing.");
				return;
			}

			// Find all script files and assemblies in the specified script directory
			var filenames = new List<string>();

			try
			{
				filenames.AddRange(Directory.GetFiles(scriptPath, "*.vb", SearchOption.AllDirectories));
				filenames.AddRange(Directory.GetFiles(scriptPath, "*.cs", SearchOption.AllDirectories));

				filenames.AddRange(Directory.GetFiles(scriptPath, "*.dll", SearchOption.AllDirectories).Where(x => IsManagedAssembly(x)));
			}
			catch (Exception ex)
			{
				Log.Message(Log.Level.Error, "Failed to reload scripts: ", ex.ToString());
			}

			// Filter out non-script assemblies like copies of ScriptHookVDotNet
			for (int i = 0; i < filenames.Count; i++)
			{
				try
				{
					var assemblyName = AssemblyName.GetAssemblyName(filenames[i]);

					if (assemblyName.Name.StartsWith("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase))
					{
						Log.Message(Log.Level.Warning, "Ignoring assembly file ", Path.GetFileName(filenames[i]), ".");

						filenames.RemoveAt(i--);
					}
				}
				catch (Exception ex)
				{
					Log.Message(Log.Level.Warning, "Could not check assembly file ", Path.GetFileName(filenames[i]), ": ", ex.ToString());
				}
			}

			foreach (var filename in filenames)
				StartScripts(filename);
		}
		/// <summary>
		/// Aborts all running scripts.
		/// </summary>
		public void Abort()
		{
			Log.Message(Log.Level.Info, "Stopping ", runningScripts.Count.ToString(), " script(s) ...");

			foreach (Script script in runningScripts)
				script.Abort();

			scriptTypes.Clear();
			runningScripts.Clear();
		}
		/// <summary>
		/// Aborts a single running script.
		/// </summary>
		/// <param name="script">The script instance to abort.</param>
		public void AbortScript(object script)
		{
			runningScripts.Single(x => x.Instance == script).Abort();
		}
		/// <summary>
		/// Aborts all running scripts from the specified file.
		/// </summary>
		/// <param name="filename"></param>
		public void AbortScripts(string filename)
		{
			filename = Path.GetFullPath(filename);

			foreach (Script script in runningScripts.Where(x => filename.Equals(x.Filename, StringComparison.OrdinalIgnoreCase)))
				script.Abort();
		}

		/// <summary>
		/// Execute a script task in this script domain.
		/// </summary>
		/// <param name="task">The task to execute.</param>
		public void ExecuteTask(IScriptTask task)
		{
			// Request came from the script thread, so need to pass it to the domain thread and execute there
			taskQueue.Enqueue(task);

			SignalAndWait(executingScript.waitEvent, executingScript.continueEvent);
		}

		/// <summary>
		/// Gets the key down status of the specified key.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns><c>true</c> if the key is currently pressed or <c>false</c> otherwise</returns>
		public bool IsKeyPressed(Keys key)
		{
			return keyboardState[(int)key];
		}
		/// <summary>
		/// Pauses or resumes handling of keyboard events in this script domain.
		/// </summary>
		/// <param name="pause"><c>true</c> to pause or <c>false</c> to resume</param>
		public void PauseKeyboardEvents(bool pause)
		{
			recordKeyboardEvents = !pause;
		}

		/// <summary>
		/// Main execution logic of the script domain.
		/// </summary>
		internal void DoTick()
		{
			// Execute running scripts
			for (int i = 0; i < runningScripts.Count; i++)
			{
				Script script = runningScripts[i];

				// Ignore terminated scripts
				if (!script.IsRunning)
					continue;

				executingScript = script;

				// Resume script thread and execute any incoming tasks from it
				bool finishedInTime = false;
				while ((finishedInTime = SignalAndWait(script.continueEvent, script.waitEvent, 5000)) && taskQueue.Count > 0)
					taskQueue.Dequeue().Run();

				executingScript = null;

				if (!finishedInTime)
				{
					Log.Message(Log.Level.Error, "Script '", script.Name, "' is not responding! Aborting ...");

					// Wait operation above timed out, which means that the script did not send any task for some time, so abort it
					script.Abort();
					continue;
				}
			}

			// Clean up any pinned strings of this frame
			CleanupStrings();
		}
		/// <summary>
		/// Keyboard handling logic of the script domain.
		/// </summary>
		/// <param name="keys">The key that was originated this event and its modifiers.</param>
		/// <param name="status"><c>true</c> on a key down, <c>false</c> on a key up event.</param>
		internal void DoKeyEvent(Keys keys, bool status)
		{
			var e = new KeyEventArgs(keys);

			keyboardState[e.KeyValue] = status;

			if (recordKeyboardEvents)
			{
				var eventinfo = new Tuple<bool, KeyEventArgs>(status, e);

				foreach (Script script in runningScripts)
					script.keyboardEvents.Enqueue(eventinfo);
			}
		}

		/// <summary>
		/// Pins the memory of a string so that it can be used in native calls without worrying about the GC invalidating its pointer.
		/// </summary>
		/// <param name="str">The string to pin to a fixed pointer.</param>
		/// <returns>A pointer to the pinned memory containing the string.</returns>
		public IntPtr PinString(string str)
		{
			IntPtr handle = NativeMemory.StringToCoTaskMemUTF8(str);

			if (handle == IntPtr.Zero)
			{
				return NativeMemory.NullString;
			}
			else
			{
				pinnedStrings.Add(handle);
				return handle;
			}
		}
		/// <summary>
		/// Free memory for all pinned strings.
		/// </summary>
		void CleanupStrings()
		{
			foreach (IntPtr handle in pinnedStrings)
				Marshal.FreeCoTaskMem(handle);
			pinnedStrings.Clear();
		}

		public string LookupScriptFilename(Type scriptType)
		{
			return scriptTypes.FirstOrDefault(x => x.Item2 == scriptType)?.Item1 ?? string.Empty;
		}

		public override object InitializeLifetimeService()
		{
			// Return null to avoid lifetime restriction on the marshaled object.
			return null;
		}

		static void SignalAndWait(SemaphoreSlim toSignal, SemaphoreSlim toWaitOn)
		{
			toSignal.Release();
			toWaitOn.Wait();
		}
		static bool SignalAndWait(SemaphoreSlim toSignal, SemaphoreSlim toWaitOn, int timeout)
		{
			toSignal.Release();
			return toWaitOn.Wait(timeout);
		}

		static bool IsManagedAssembly(string filename)
		{
			using (Stream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				if (file.Length < 64)
					return false;

				using (BinaryReader bin = new BinaryReader(file))
				{
					// PE header starts at offset 0x3C (60). Its a 4 byte header.
					file.Position = 0x3C;
					uint offset = bin.ReadUInt32();
					if (offset == 0)
						offset = 0x80;

					// Ensure there is at least enough room for the following structures:
					//     24 byte PE Signature & Header
					//     28 byte Standard Fields         (24 bytes for PE32+)
					//     68 byte NT Fields               (88 bytes for PE32+)
					// >= 128 byte Data Dictionary Table
					if (offset > file.Length - 256)
						return false;

					// Check the PE signature. Should equal 'PE\0\0'.
					file.Position = offset;
					if (bin.ReadUInt32() != 0x00004550)
						return false;

					// Read PE magic number from Standard Fields to determine format.
					file.Position += 20;
					var peFormat = bin.ReadUInt16();
					if (peFormat != 0x10b /* PE32 */ && peFormat != 0x20b /* PE32Plus */)
						return false;

					// Read the 15th Data Dictionary RVA field which contains the CLI header RVA.
					// When this is non-zero then the file contains CLI data otherwise not.
					file.Position = offset + (peFormat == 0x10b ? 232 : 248);
					return bin.ReadUInt32() != 0;
				}
			}
		}

		static Assembly HandleResolve(object sender, ResolveEventArgs args)
		{
			var assemblyName = new AssemblyName(args.Name);

			// Special case for the main assembly (this is necessary since the .NET framework does not check ASI files for assemblies during lookup)
			if (assemblyName.Name.Equals("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase))
			{
				return typeof(ScriptDomain).Assembly;
			}

			// Handle resolve of the scripting API assembly
			if (CurrentDomain != null && assemblyName.Name.StartsWith("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase))
			{
				var scriptApi = CurrentDomain.scriptApi;

				// Check that the referenced script API assembly version is compatible
				if (assemblyName.Version.Major == scriptApi.GetName().Version.Major)
					return scriptApi;
			}

			return null;
		}
		static internal void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			Log.Message(Log.Level.Error, args.IsTerminating ? "Caught fatal unhandled exception:" : "Caught unhandled exception:", Environment.NewLine, args.ExceptionObject.ToString());

			if (sender == null || !typeof(Script).IsInstanceOfType(sender))
				return;

			var script = (Script)sender;

			Log.Message(Log.Level.Info, "The exception was thrown while executing the script ", script.Name, ".");
		}
	}
}
