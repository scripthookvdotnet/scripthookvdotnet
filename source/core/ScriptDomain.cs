//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SHVDN
{

	/// <summary>
	/// The interface for tasks that must be run on the main thread (e.g. calling native functions) because of thread local storage (TLS).
	/// </summary>
	public interface IScriptTask
	{
		void Run();
	}

	public class ScriptDomain : MarshalByRefObject, IDisposable
	{
		int executingThreadId = Thread.CurrentThread.ManagedThreadId;
		Script executingScript = null;
		List<IntPtr> pinnedStrings = new List<IntPtr>();
		List<Script> runningScripts = new List<Script>();
		Queue<IScriptTask> taskQueue = new Queue<IScriptTask>();
		Dictionary<string, int> scriptInstances = new Dictionary<string, int>();
		SortedList<string, Tuple<string, Type>> scriptTypes = new SortedList<string, Tuple<string, Type>>();
		bool recordKeyboardEvents = true;
		bool[] keyboardState = new bool[256];
		List<Assembly> scriptApis = new List<Assembly>();

		/// <summary>
		/// Gets the friendly name of this script domain.
		/// </summary>
		public string Name => AppDomain.FriendlyName;
		/// <summary>
		/// Gets the path to the directory containing scripts.
		/// </summary>
		public string ScriptPath => AppDomain.BaseDirectory;
		/// <summary>
		/// Gets the application domain that is associated with this script domain.
		/// </summary>
		public AppDomain AppDomain { get; } = AppDomain.CurrentDomain;

		/// <summary>
		/// Gets the scripting domain for the current application domain.
		/// </summary>
		public static ScriptDomain CurrentDomain { get; private set; }

		internal static string CurrentLoadingScriptAssemblyName { get; private set; }

		/// <summary>
		/// Gets the list of currently running scripts in this script domain. This is used by the console implementation.
		/// </summary>
		public Script[] RunningScripts => runningScripts.ToArray();
		/// <summary>
		/// Gets the currently executing script or <see langword="null" /> if there is none.
		/// </summary>
		public static Script ExecutingScript => CurrentDomain != null ? CurrentDomain.executingScript : null;

		/// <summary>
		/// Initializes the script domain inside its application domain.
		/// </summary>
		/// <param name="apiBasePath">The path to the root directory containing the scripting API assemblies.</param>
		private ScriptDomain(string apiBasePath)
		{
			// Each application domain has its own copy of this static variable, so only need to set it once
			CurrentDomain = this;

			// Attach resolve handler to new domain
			AppDomain.AssemblyResolve += HandleResolve;
			AppDomain.UnhandledException += HandleUnhandledException;

			// Load API assemblies into this script domain
			foreach (string apiPath in Directory.EnumerateFiles(apiBasePath, "ScriptHookVDotNet*.dll", SearchOption.TopDirectoryOnly))
			{
				Log.Message(Log.Level.Debug, "Loading API from ", apiPath, " ...");

				try
				{
					scriptApis.Add(Assembly.LoadFrom(apiPath));
				}
				catch (Exception ex)
				{
					Log.Message(Log.Level.Error, "Unable to load ", Path.GetFileName(apiPath), ": ", ex.ToString());
				}
			}
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
			// Need to free native strings when disposing the script domain
			CleanupStrings();
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
		/// Creates a new script domain.
		/// </summary>
		/// <param name="basePath">The path to the application root directory.</param>
		/// <param name="scriptPath">The path to the directory containing scripts.</param>
		/// <returns>The script domain or <see langword="null" /> in case of failure.</returns>
		public static ScriptDomain Load(string basePath, string scriptPath)
		{
			// Make absolute path to scrips location
			if (!Path.IsPathRooted(scriptPath))
				scriptPath = Path.Combine(Path.GetDirectoryName(basePath), scriptPath);
			scriptPath = Path.GetFullPath(scriptPath);

			// Create application and script domain for all the scripts to reside in
			var name = "ScriptDomain_" + (scriptPath.GetHashCode() ^ Environment.TickCount).ToString("X");
			var setup = new AppDomainSetup();
			setup.ShadowCopyFiles = "true"; // Copy assemblies into memory rather than locking the file, so they can be updated while the domain is still loaded
			setup.ShadowCopyDirectories = scriptPath; // Only shadow copy files in the scripts directory
			setup.ApplicationBase = scriptPath;

			var appdomain = AppDomain.CreateDomain(name, null, setup, new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted));
			appdomain.InitializeLifetimeService(); // Give the application domain an infinite lifetime

			// Need to attach the resolve handler to the current domain too, so that the .NET framework finds this assembly in the ASI file
			AppDomain.CurrentDomain.AssemblyResolve += HandleResolve;

			ScriptDomain scriptdomain = null;

			try
			{
				scriptdomain = (ScriptDomain)appdomain.CreateInstanceFromAndUnwrap(typeof(ScriptDomain).Assembly.Location, typeof(ScriptDomain).FullName, false, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { basePath }, null, null);
			}
			catch (Exception ex)
			{
				Log.Message(Log.Level.Error, "Failed to create script domain: ", ex.ToString());
				AppDomain.Unload(appdomain);
			}

			// Remove resolve handler again
			AppDomain.CurrentDomain.AssemblyResolve -= HandleResolve;

			return scriptdomain;
		}

		/// <summary>
		/// Compiles and load scripts from a C# or VB.NET source code file.
		/// </summary>
		/// <param name="filename">The path to the code file to load.</param>
		/// <returns><see langword="true" /> on success, <see langword="false" /> otherwise</returns>
		bool LoadScriptsFromSource(string filename)
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
			compilerOptions.ReferencedAssemblies.Add(typeof(ScriptDomain).Assembly.Location);

			Assembly scriptApi = null;
			// Support specifying the API version to be used in the file name like "script.3.cs"
			string apiVersionString = Path.GetExtension(Path.GetFileNameWithoutExtension(filename));
			if (!string.IsNullOrEmpty(apiVersionString) && int.TryParse(apiVersionString.Substring(1), out int apiVersion))
				scriptApi = CurrentDomain.scriptApis.FirstOrDefault(x => x.GetName().Version.Major == apiVersion);
			// Reference the oldest scripting API by default to stay compatible with existing scripts
			if (scriptApi is null)
				scriptApi = scriptApis.First();
			compilerOptions.ReferencedAssemblies.Add(scriptApi.Location);

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
				Log.Message(Log.Level.Debug, "Successfully compiled ", Path.GetFileName(filename), ".");
				return LoadScriptsFromAssembly(compilerResult.CompiledAssembly, filename);
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
		/// <returns><see langword="true" /> on success, <see langword="false" /> otherwise</returns>
		bool LoadScriptsFromAssembly(string filename)
		{
			if (!IsManagedAssembly(filename))
				return false;

			Log.Message(Log.Level.Debug, "Loading assembly ", Path.GetFileName(filename), " ...");

			Assembly assembly = null;

			try
			{
				// Note: This loads the assembly only the first time and afterwards returns the already loaded assembly!
				ScriptDomain.CurrentLoadingScriptAssemblyName = Path.GetFileName(filename);
				assembly = Assembly.LoadFrom(filename);
			}
			catch (Exception ex)
			{
				Log.Message(Log.Level.Error, "Unable to load ", Path.GetFileName(filename), ": ", ex.ToString());
				return false;
			}

			return LoadScriptsFromAssembly(assembly, filename);
		}
		/// <summary>
		/// Loads scripts from the specified assembly object.
		/// </summary>
		/// <param name="filename">The path to the file associated with this assembly.</param>
		/// <param name="assembly">The assembly to load.</param>
		/// <returns><see langword="true" /> on success, <see langword="false" /> otherwise</returns>
		bool LoadScriptsFromAssembly(Assembly assembly, string filename)
		{
			int count = 0;
			Version apiVersion = null;

			try
			{
				// Find all script types in the assembly
				foreach (var type in assembly.GetTypes().Where(x => IsSubclassOf(x, "GTA.Script")))
				{
					count++;

					// This function builds a composite key of all dependencies of a script
					Func<Type, string, string> BuildComparisonString = null;
					BuildComparisonString = (a, b) => {
						b = a.FullName + "%%" + b;
						foreach (var attribute in a.GetCustomAttributesData().Where(x => x.AttributeType.FullName == "GTA.RequireScript"))
						{
							var dependency = attribute.ConstructorArguments[0].Value as Type;
							// Ignore circular dependencies
							if (dependency != null && !b.Contains("%%" + dependency.FullName))
								b = BuildComparisonString(dependency, b);
						}
						return b;
					};

					var key = BuildComparisonString(type, string.Empty);
					key = assembly.GetName().Name + "-" + assembly.GetName().Version + key;

					if (scriptTypes.ContainsKey(key))
					{
						Log.Message(Log.Level.Warning, "The script name ", type.FullName, " already exists and was loaded from ", Path.GetFileName(scriptTypes[key].Item1), ". Ignoring occurrence loaded from ", Path.GetFileName(filename), ".");
						continue; // Skip types that were already added previously are ignored
					}

					scriptTypes.Add(key, new Tuple<string, Type>(filename, type));

					if (apiVersion == null) // Check API version for one of the types (should be the same for all)
						apiVersion = GetBaseTypeVersion(type, "GTA.Script");
				}
			}
			catch (ReflectionTypeLoadException ex)
			{
				// Filter out failure if unable to resolve SHVDN API, since this was already logged in 'HandleResolve'
				var fileNotFoundException = ex.LoaderExceptions[0] as FileNotFoundException;
				if (fileNotFoundException == null || fileNotFoundException.Message.IndexOf("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase) < 0)
				{
					Log.Message(Log.Level.Error, "Failed to load assembly ", Path.GetFileName(filename), ": ", ex.LoaderExceptions[0].ToString());
				}

				return false;
			}

			Log.Message(Log.Level.Info, "Found ", count.ToString(), " script(s) in ", Path.GetFileName(filename), (apiVersion != null ? " resolved to API " + apiVersion.ToString(3) : string.Empty), ".");

			return count != 0;
		}

		/// <summary>
		/// Creates an instance of a script.
		/// </summary>
		/// <param name="scriptType">The type of the script to instantiate.</param>
		/// <returns>The script instance or <see langword="null" /> in case of failure.</returns>
		public Script InstantiateScript(Type scriptType)
		{
			if (scriptType.IsAbstract || !IsSubclassOf(scriptType, "GTA.Script"))
				return null;

			Log.Message(Log.Level.Debug, "Instantiating script ", scriptType.FullName, " ...");

			Script script = new Script();
			// Keep track of current script, so it can be restored down below
			Script previousScript = executingScript;

			executingScript = script;

			// Create a name for the new script instance
			if (scriptInstances.ContainsKey(scriptType.FullName))
			{
				int instanceIndex = scriptInstances[scriptType.FullName] + 1;
				scriptInstances[scriptType.FullName] = instanceIndex;

				script.Name = scriptType.FullName + instanceIndex.ToString();
			}
			else
			{
				scriptInstances.Add(scriptType.FullName, 0);

				// Do not append instance index to the default instance name
				script.Name = scriptType.FullName;
			}

			script.Filename = LookupScriptFilename(scriptType);

			try
			{
				script.ScriptInstance = Activator.CreateInstance(scriptType);
			}
			catch (MissingMethodException)
			{
				Log.Message(Log.Level.Error, "Failed to instantiate script ", scriptType.FullName, " because no public default constructor was found.");
				return null;
			}
			catch (TargetInvocationException ex)
			{
				Log.Message(Log.Level.Error, "Failed to instantiate script ", scriptType.FullName, " because constructor threw an exception: ", ex.InnerException.ToString());
				return null;
			}
			catch (Exception ex)
			{
				Log.Message(Log.Level.Error, "Failed to instantiate script ", scriptType.FullName, ": ", ex.ToString());

				if (GetScriptAttribute(scriptType, "SupportURL") is string supportURL)
					Log.Message(Log.Level.Error, "Please check the following site for support on the issue: ", supportURL);

				return null;
			}

			runningScripts.Add(script);

			// Restore previously executing script
			executingScript = previousScript;

			return script;
		}

		/// <summary>
		/// Loads and starts all scripts.
		/// </summary>
		public void Start()
		{
			if (scriptTypes.Count != 0 || runningScripts.Count != 0)
				return; // Cannot start script domain if scripts are already running

			Log.Message(Log.Level.Debug, "Loading scripts from ", ScriptPath, " ...");

			if (!Directory.Exists(ScriptPath))
			{
				Log.Message(Log.Level.Warning, "Failed to reload scripts because the ", ScriptPath, " directory is missing.");
				return;
			}

			// Find all script files and assemblies in the specified script directory
			var filenamesSource = new List<string>();
			var filenamesAssembly = new List<string>();

			try
			{
				filenamesSource.AddRange(Directory.GetFiles(ScriptPath, "*.vb", SearchOption.AllDirectories));
				filenamesSource.AddRange(Directory.GetFiles(ScriptPath, "*.cs", SearchOption.AllDirectories));

				filenamesAssembly.AddRange(Directory.GetFiles(ScriptPath, "*.dll", SearchOption.AllDirectories)
					.Where(x => IsManagedAssembly(x)));
			}
			catch (Exception ex)
			{
				Log.Message(Log.Level.Error, "Failed to reload scripts: ", ex.ToString());
			}

			// Filter out non-script assemblies
			for (int i = 0; i < filenamesAssembly.Count; i++)
			{
				try
				{
					var assemblyName = AssemblyName.GetAssemblyName(filenamesAssembly[i]);

					if (assemblyName.Name.StartsWith("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase))
					{
						// Delete copies of ScriptHookVDotNet, since these can cause issues with the assembly binder loading multiple copies
						File.Delete(filenamesAssembly[i]);

						filenamesAssembly.RemoveAt(i--);
					}
				}
				catch (Exception ex)
				{
					Log.Message(Log.Level.Warning, "Ignoring assembly file ", Path.GetFileName(filenamesAssembly[i]), " because of exception: ", ex.ToString());

					filenamesAssembly.RemoveAt(i--);
				}
			}

			foreach (var filename in filenamesSource)
				LoadScriptsFromSource(filename);
			foreach (var filename in filenamesAssembly)
				LoadScriptsFromAssembly(filename);

			// Instantiate scripts after they were all loaded, so that dependencies are launched with the right ordering
			foreach (var type in scriptTypes.Values.Select(x => x.Item2))
			{
				// Start the script unless script does not want a default instance
				if (!(GetScriptAttribute(type, "NoDefaultInstance") is bool NoDefaultInstance) || !NoDefaultInstance)
					InstantiateScript(type)?.Start();
			}
		}
		/// <summary>
		/// Loads and starts all scripts in the specified file.
		/// </summary>
		/// <param name="filename"></param>
		public void StartScripts(string filename)
		{
			filename = Path.GetFullPath(filename);

			if (Path.GetExtension(filename).Equals(".dll", StringComparison.OrdinalIgnoreCase) ?
				!LoadScriptsFromAssembly(filename) : !LoadScriptsFromSource(filename))
				return;

			// Instantiate only those scripts that are from the this assembly
			foreach (var type in scriptTypes.Values.Where(x => x.Item1 == filename).Select(x => x.Item2))
			{
				// Make sure there are no others instances of this script
				runningScripts.RemoveAll(x => x.Filename == filename && x.ScriptInstance.GetType() == type);

				// Start the script unless script does not want a default instance
				if (!(GetScriptAttribute(type, "NoDefaultInstance") is bool NoDefaultInstance) || !NoDefaultInstance)
					InstantiateScript(type)?.Start();
			}
		}
		/// <summary>
		/// Aborts all running scripts.
		/// </summary>
		public void Abort()
		{
			foreach (Script script in runningScripts)
				script.Abort();

			scriptTypes.Clear();
			runningScripts.Clear();
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
			if (Thread.CurrentThread.ManagedThreadId == executingThreadId)
			{
				// Request came from the main thread, so can just execute it right away
				task.Run();
			}
			else
			{
				// Request came from the script thread, so need to pass it to the domain thread and execute there
				taskQueue.Enqueue(task);

				SignalAndWait(executingScript.waitEvent, executingScript.continueEvent);
			}
		}

		/// <summary>
		/// Gets the key down status of the specified key.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns><see langword="true" /> if the key is currently pressed or <see langword="false" /> otherwise</returns>
		public bool IsKeyPressed(Keys key)
		{
			return keyboardState[(int)key];
		}
		/// <summary>
		/// Pauses or resumes handling of keyboard events in this script domain.
		/// </summary>
		/// <param name="pause"><see langword="true" /> to pause or <see langword="false" /> to resume</param>
		public void PauseKeyEvents(bool pause)
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
				if (!script.IsRunning || script.IsPaused)
					continue;

				executingScript = script;

				bool finishedInTime = true;

				try
				{
					// Resume script thread and execute any incoming tasks from it
					while ((finishedInTime = SignalAndWait(script.continueEvent, script.waitEvent, 5000)) && taskQueue.Count > 0)
						taskQueue.Dequeue().Run();
				}
				catch (Exception ex)
				{
					HandleUnhandledException(script, new UnhandledExceptionEventArgs(ex, true));

					// Stop script in case of an unhandled exception during task execution
					script.Abort();
				}

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
		/// <param name="status"><see langword="true" /> on a key down, <see langword="false" /> on a key up event.</param>
		internal void DoKeyEvent(Keys keys, bool status)
		{
			var e = new KeyEventArgs(keys);

			// Only update state of the primary key (without modifiers) here
			keyboardState[(int)e.KeyCode] = status;

			if (recordKeyboardEvents)
			{
				var eventinfo = new Tuple<bool, KeyEventArgs>(status, e);

				foreach (Script script in runningScripts)
					script.keyboardEvents.Enqueue(eventinfo);
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
		/// Finds the script object representing the specified <paramref name="scriptInstance"/> object.
		/// </summary>
		/// <param name="scriptInstance">The 'GTA.Script' instance to check.</param>
		public Script LookupScript(object scriptInstance)
		{
			if (scriptInstance == null)
				return null;

			// Return matching script in running script list if one is found
			var script = runningScripts.FirstOrDefault(x => x.ScriptInstance == scriptInstance);

			// Otherwise return the executing script, since during constructor execution the running script list was not yet updated
			if (script == null && executingScript != null && executingScript.ScriptInstance == null)
			{
				// Handle the case where a script creates a custom instance of a script class that is not managed by SHVDN
				// These may attempt to set events, but are not allowed to do so, since SHVDN will never call them, so just return null
				if (!executingScript.Name.Contains(scriptInstance.GetType().FullName))
				{
					Log.Message(Log.Level.Warning, "A script tried to use a custom script instance of type ", scriptInstance.GetType().FullName, " that was not instantiated by ScriptHookVDotNet.");
					return null;
				}

				script = executingScript;
			}

			return script;
		}
		public string LookupScriptFilename(Type scriptType)
		{
			return scriptTypes.Values.FirstOrDefault(x => x.Item2 == scriptType)?.Item1 ?? string.Empty;
		}

		/// <summary>
		/// Checks if the script has a 'GTA.ScriptAttributes' attribute with the specified argument attached to it and returns it.
		/// </summary>
		/// <param name="scriptType">The script type to check for the attribute.</param>
		/// <param name="name">The named argument to search.</param>
		static object GetScriptAttribute(Type scriptType, string name)
		{
			var attribute = scriptType.GetCustomAttributesData().FirstOrDefault(x => x.AttributeType.FullName == "GTA.ScriptAttributes");

			if (attribute != null)
			{
				foreach (var arg in attribute.NamedArguments)
				{
					if (arg.MemberName == name)
						return arg.TypedValue.Value;
				}
			}

			return null;
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

		static bool IsSubclassOf(Type type, string baseTypeName)
		{
			for (Type t = type.BaseType; t != null; t = t.BaseType)
				if (t.FullName == baseTypeName)
					return true;
			return false;
		}

		static Version GetBaseTypeVersion(Type type, string baseTypeName)
		{
			for (Type t = type.BaseType; t != null; t = t.BaseType)
				if (t.FullName == baseTypeName)
					return t.Assembly.GetName().Version;
			return null;
		}

		static bool IsManagedAssembly(string filename)
		{
			try
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
			catch
			{
				// This is likely not a valid assembly if any IO exceptions occur during reading
				return false;
			}
		}

		static Assembly HandleResolve(object sender, ResolveEventArgs args)
		{
			var assemblyName = new AssemblyName(args.Name);

			// Special case for the main assembly (this is necessary since the .NET framework does not check ASI files for assemblies during lookup, so is unable to load the ScriptDomain type when creating it in a new application domain)
			// Some scripts were written against old SHVDN versions where everything was still in the ASI, so make sure those are not caught here (see also https://github.com/crosire/scripthookvdotnet/releases/tag/v2.10.0)
			if (assemblyName.Name.Equals("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase) && assemblyName.Version >= new Version(2, 10, 0, 0))
			{
				return typeof(ScriptDomain).Assembly;
			}

			// Handle resolve of the scripting API assembly (ScriptHookVDotNet*.dll)
			if (CurrentDomain != null && assemblyName.Name.StartsWith("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase))
			{
				var bestVersion = new Version(0, 0, 0, 0);

				// Some scripts reference a version-less SHVDN, do default those to major version 2
				if (assemblyName.Version == bestVersion)
				{
					// ResolveEventArgs.RequestingAssembly will always be null for some reason
					Log.Message(Log.Level.Warning, "Resolving API version 0.0.0",
						ScriptDomain.CurrentLoadingScriptAssemblyName != null ? " referenced in " + ScriptDomain.CurrentLoadingScriptAssemblyName : string.Empty, ".");

					return CurrentDomain.scriptApis.FirstOrDefault(x => x.GetName().Version.Major == 2);
				}

				Assembly compatibleApi = null;

				foreach (Assembly api in CurrentDomain.scriptApis)
				{
					Version apiVersion = api.GetName().Version;

					// Find the newest compatible scripting API version
					if (assemblyName.Version.Major == apiVersion.Major && apiVersion >= assemblyName.Version && apiVersion > bestVersion)
					{
						bestVersion = apiVersion;
						compatibleApi = api;
					}
				}

				// Write a warning message if no compatible scripting API version was found
				if (compatibleApi == null)
				{
					Log.Message(Log.Level.Warning, "Unable to resolve API version ", assemblyName.Version.ToString(3),
						args.RequestingAssembly != null ? " referenced in " + args.RequestingAssembly.GetName().Name : string.Empty, ".");
				}

				return compatibleApi;
			}

			return null;
		}

		static internal void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			Log.Message(Log.Level.Error, args.IsTerminating ? "Caught fatal unhandled exception:" : "Caught unhandled exception:", Environment.NewLine, args.ExceptionObject.ToString());

			if (sender is Script script)
			{
				Log.Message(Log.Level.Error, "The exception was thrown while executing the script ", script.Name, ".");

				if (GetScriptAttribute(script.ScriptInstance.GetType(), "SupportURL") is string supportURL)
					Log.Message(Log.Level.Error, "Please check the following site for support on the issue: ", supportURL);

				// Show a notification with the script crash information
				var domain = ScriptDomain.CurrentDomain;
				if (domain != null && domain.executingScript != null)
				{
					unsafe
					{
						NativeFunc.Invoke(0x202709F4C58A0424 /*BEGIN_TEXT_COMMAND_THEFEED_POST*/, NativeMemory.CellEmailBcon);
						NativeFunc.PushLongString("~r~Unhandled exception~s~ in script \"~h~" + script.Name + "~h~\"!~n~~n~~r~" + args.ExceptionObject.GetType().Name + "~s~ " + ((Exception)args.ExceptionObject).StackTrace.Split('\n').FirstOrDefault().Trim());
						NativeFunc.Invoke(0x2ED7843F8F801023 /*END_TEXT_COMMAND_THEFEED_POST_TICKER*/, true, true);
					}
				}
			}
		}
	}
}
