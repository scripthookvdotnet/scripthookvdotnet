//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text;

namespace SHVDN
{
	/// <summary>
	/// The interface for tasks that must be run on the main thread (e.g. calling native functions) because of thread local storage (TLS).
	/// </summary>
	public interface IScriptTask
	{
		void Run();
	}

	public sealed class ScriptDomain : MarshalByRefObject, IDisposable
	{
		// Debugger.IsAttached does not detect a Visual Studio debugger
		[SuppressUnmanagedCodeSecurity]
		[DllImport("Kernel32.dll")]
		internal static extern bool IsDebuggerPresent();

		[DllImport("kernel32.dll")]
		private static extern uint GetCurrentThreadId();

		private int _executingThreadId = Thread.CurrentThread.ManagedThreadId;
		private Script _executingScript = null;
		private List<IntPtr> _pinnedStrings = new();
		private List<Script> _runningScripts = new();
		private Queue<IScriptTask> _taskQueue = new();
		private Dictionary<string, int> _scriptInstances = new();
		private SortedList<string, Tuple<string, Type>> _scriptTypes = new();
		private bool _recordKeyboardEvents = true;
		private bool[] _keyboardState = new bool[256];
		private List<Assembly> _scriptApis = new List<Assembly>();

		private unsafe delegate* unmanaged[Cdecl]<IntPtr> _getTlsContext;
		private unsafe delegate* unmanaged[Cdecl]<IntPtr, void> _setTlsContext;

		private IntPtr _tlsContextOfMainThread;
		private uint _gameMainThreadIdUnmanaged;

		/// <summary>
		/// The byte array that contains "CELL_EMAIL_BCON", which is used when SHVDN logs unhandled exceptions
		/// or old scripting SDK/API version warning via a feed ticker.
		/// </summary>
		/// <remarks>
		/// This is not a unmanaged pointer to a pinned string since this member would not be frequently used.
		/// </remarks>
		private static byte[] s_cellEmailBconByteStr = Encoding.ASCII.GetBytes("CELL_EMAIL_BCON\0");

		internal unsafe void InitTlsFunctionPointers(IntPtr getTlsContextFunc, IntPtr setTlsContextFunc)
		{
			_getTlsContext = (delegate* unmanaged[Cdecl]<IntPtr>)getTlsContextFunc;
			_setTlsContext = (delegate* unmanaged[Cdecl]<IntPtr, void>)setTlsContextFunc;
		}

		internal unsafe void SetTlsContextOfGameMainThread(IntPtr tlsAddr)
		{
			_tlsContextOfMainThread = tlsAddr;
		}

		internal unsafe void SetGameMainThreadId(uint threadId)
		{
			_gameMainThreadIdUnmanaged = threadId;
		}

		internal void InitNativeNemoryMembers()
		{
			Log.Message(Log.Level.Debug, "Initializing NativeMemory members...");

			// We have a lot of offset and function address data to initialize, so initialize them in parallel
			// (except for path find data, which has a little amount of data)
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(NativeMemory.PathFind).TypeHandle);
			Parallel.Invoke(
				() => { System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(NativeMemory).TypeHandle); },
				() => { System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(NativeMemory.Vehicle).TypeHandle); },
				() => { System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(NativeMemory.Ped).TypeHandle); }
			);
		}

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

		/// <summary>
		/// Gets the list of currently running scripts in this script domain. This is used by the console implementation.
		/// </summary>
		public Script[] RunningScripts => _runningScripts.ToArray();
		/// <summary>
		/// Gets the currently executing script or <see langword="null" /> if there is none.
		/// </summary>
		public static Script ExecutingScript => CurrentDomain != null ? CurrentDomain._executingScript : null;

		/// <summary>
		/// Gets or sets the value how long script can execute in one tick without getting terminated after the tick ends.
		/// </summary>
		public uint ScriptTimeoutThreshold { get; set; }

		/// <summary>
		/// Gets the dictionary of deprecated script names.
		/// </summary>
		private Dictionary<int, List<string>> DeprecatedScriptAssemblyNamesPerApiVersion { get; set; } = new();
		/// <summary>
		/// Gets the value that indicates whether the script domain should warn of deprecated scripts with a ticker.
		/// </summary>
		public bool ShouldWarnOfScriptsBuiltAgainstDeprecatedApiWithTicker { get; set; }

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
				if (!Regex.IsMatch(Path.GetFileName(apiPath), @"^ScriptHookVDotNet\d\.dll$"))
				{
					continue;
				}

				Log.Message(Log.Level.Debug, "Loading API from ", apiPath, " ...");

				try
				{
					_scriptApis.Add(Assembly.LoadFrom(apiPath));
				}
				catch (Exception ex)
				{
					Log.Message(Log.Level.Error, "Unable to load ", Path.GetFileName(apiPath), ": ", ex.ToString());
				}
			}
			// Sort the api list by major version so the order is guaranteed to be sorted in ascending order regardless of how Directory.EnumerateFiles enumerates
			// as long as all of major versions are unique
			_scriptApis.Sort((x, y) => x.GetName().Version.Major.CompareTo(y.GetName().Version.Major));
		}

		~ScriptDomain()
		{
			DisposeUnmanagedResource();
		}
		public void Dispose()
		{
			DisposeUnmanagedResource();
			GC.SuppressFinalize(this);
		}

		private void DisposeUnmanagedResource()
		{
			// Need to free native strings when disposing the script domain
			CleanupStrings();
			// Need to free unmanaged resources in NativeMemory
			NativeMemory.DisposeUnmanagedResources();
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
			{
				scriptPath = Path.Combine(Path.GetDirectoryName(basePath), scriptPath);
			}

			scriptPath = Path.GetFullPath(scriptPath);

			// Create application and script domain for all the scripts to reside in
			int hashCodeForAppDomain = 17;
			hashCodeForAppDomain = hashCodeForAppDomain * 23 + scriptPath.GetHashCode();
			hashCodeForAppDomain = hashCodeForAppDomain * 23 + Environment.TickCount.GetHashCode();
			string name = "SHVDN_ScriptDomain_" + hashCodeForAppDomain.ToString("X");
			var setup = new AppDomainSetup();
			setup.ShadowCopyFiles = "true"; // Copy assemblies to another location than locking the file, so the original assembly files can be updated while the domain is still loaded (the copied assemblies will be locked)
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
		private bool LoadScriptsFromSource(string filename)
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
			{
				scriptApi = ScriptDomain.CurrentDomain._scriptApis.FirstOrDefault(x => x.GetName().Version.Major == apiVersion);
			}

			// Reference the oldest scripting API that is not deprecated by default to stay compatible with existing scripts
			scriptApi ??= _scriptApis.First(x => !IsApiVersionDeprecated(x.GetName().Version));
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

			CompilerResults compilerResult = compiler.CompileAssemblyFromFile(compilerOptions, filename);

			if (!compilerResult.Errors.HasErrors)
			{
				Log.Message(Log.Level.Debug, "Successfully compiled ", Path.GetFileName(filename), ".");
				return LoadScriptsFromAssembly(compilerResult.CompiledAssembly, filename);
			}

			var errors = new System.Text.StringBuilder();

			foreach (System.CodeDom.Compiler.CompilerError error in compilerResult.Errors)
			{
				errors.Append("   at line ");
				errors.Append(error.Line);
				errors.Append(": ");
				errors.Append(error.ErrorText);
				errors.AppendLine();
			}

			Log.Message(Log.Level.Error, "Failed to compile ", Path.GetFileName(filename), " using API version ", scriptApi.GetName().Version.ToString(3), " with ", compilerResult.Errors.Count.ToString(), " error(s):", Environment.NewLine, errors.ToString());
			return false;
		}
		/// <summary>
		/// Loads scripts from the specified assembly file.
		/// </summary>
		/// <param name="filename">The path to the assembly file to load.</param>
		/// <returns><see langword="true" /> on success, <see langword="false" /> otherwise</returns>
		private bool LoadScriptsFromAssembly(string filename)
		{
			if (!IsManagedAssembly(filename))
			{
				return false;
			}

			string fileNameWithoutPath = Path.GetFileName(filename);
			Log.Message(Log.Level.Debug, "Loading assembly ", fileNameWithoutPath, " ...");

			Assembly assembly = null;

			try
			{
				// Note: This loads the assembly only the first time and afterwards returns the already loaded assembly!
				assembly = Assembly.LoadFrom(filename);
			}
			catch (Exception ex)
			{
				Log.Message(Log.Level.Error, "Unable to load ", fileNameWithoutPath, ": ", ex.ToString());
				return false;
			}

			// Show the warning "Resolving API version 0.0.0" if the script reference a version-less SHVDN
			AssemblyName shvdnAssembly = assembly.GetReferencedAssemblies().FirstOrDefault(x => x.Name == "ScriptHookVDotNet");
			if (shvdnAssembly != null && shvdnAssembly.Version == new Version(0, 0, 0, 0))
			{
				Log.Message(Log.Level.Warning, "Resolving API version 0.0.0 referenced in " + fileNameWithoutPath, ".");
			}

			return LoadScriptsFromAssembly(assembly, filename);
		}
		/// <summary>
		/// Loads scripts from the specified assembly object.
		/// </summary>
		/// <param name="filename">The path to the file associated with this assembly.</param>
		/// <param name="assembly">The assembly to load.</param>
		/// <returns><see langword="true" /> on success, <see langword="false" /> otherwise</returns>
		private bool LoadScriptsFromAssembly(Assembly assembly, string filename)
		{
			int count = 0;
			Version apiVersion = null;

			try
			{
				// Find all script types in the assembly
				foreach (Type type in assembly.GetTypes().Where(x => IsSubclassOf(x, "GTA.Script")))
				{
					count++;

					// This function builds a composite key of all dependencies of a script
					string BuildComparisonString(Type a, string b)
					{
						b = a.FullName + "%%" + b;
						foreach (CustomAttributeData attribute in a.GetCustomAttributesData().Where(x => x.AttributeType.FullName == "GTA.RequireScript"))
						{
							var dependency = attribute.ConstructorArguments[0].Value as Type;
							// Ignore circular dependencies
							if (dependency != null && !b.Contains("%%" + dependency.FullName))
							{
								b = BuildComparisonString(dependency, b);
							}
						}

						return b;
					}

					string key = BuildComparisonString(type, string.Empty);
					key = assembly.GetName().Name + "-" + assembly.GetName().Version + key;

					if (_scriptTypes.TryGetValue(key, out Tuple<string, Type> scriptType))
					{
						Log.Message(Log.Level.Warning, "The script name ", type.FullName, " already exists and was loaded from ", Path.GetFileName(scriptType.Item1), ". Ignoring occurrence loaded from ", Path.GetFileName(filename), ".");
						continue; // Skip types that were already added previously are ignored
					}

					_scriptTypes.Add(key, new Tuple<string, Type>(filename, type));

					if (apiVersion == null) // Check API version for one of the types (should be the same for all)
					{
						apiVersion = GetBaseTypeVersion(type, "GTA.Script");
					}

					if (apiVersion == new Version(0, 0, 0, 0))
					{
						Log.Message(Log.Level.Warning, "Resolving API version 0.0.0 referenced in " + assembly.GetName(), ".");
					}
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

			Log.Message(Log.Level.Info, "Found ", count.ToString(), " script(s) in ", Path.GetFileName(filename), (apiVersion != null ? " resolved to API version " + apiVersion.ToString(3) : string.Empty), ".");

			if (apiVersion != null && IsApiVersionDeprecated(apiVersion))
			{
				AddScriptAssemblyNameBuiltAgainstApiVersion(apiVersion.Major, Path.GetFileName(filename));
			}

			return count != 0;
		}

		/// <summary>
		/// Creates an instance of a script.
		/// </summary>
		/// <param name="scriptType">The type of the script to instantiate.</param>
		/// <returns>The script instance or <see langword="null" /> in case of failure.</returns>
		public Script InstantiateScript(Type scriptType)
		{
			if (Thread.CurrentThread.ManagedThreadId != _executingThreadId)
			{
				return null; // This must only be called in the main thread (since changing 'executingScript' during 'DoTick' of another script would break)
			}

			if (scriptType.IsAbstract || !IsSubclassOf(scriptType, "GTA.Script"))
			{
				return null;
			}

			Log.Message(Log.Level.Debug, "Instantiating script ", scriptType.FullName, " ...");

			var script = new Script();
			// Keep track of current script, so it can be restored down below
			Script previousScript = _executingScript;

			_executingScript = script;

			// Create a name for the new script instance
			if (_scriptInstances.ContainsKey(scriptType.FullName))
			{
				int instanceIndex = _scriptInstances[scriptType.FullName] + 1;
				_scriptInstances[scriptType.FullName] = instanceIndex;

				script.Name = scriptType.FullName + instanceIndex.ToString();
			}
			else
			{
				_scriptInstances.Add(scriptType.FullName, 0);

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
				{
					Log.Message(Log.Level.Error, "Please check the following site for support on the issue: ", supportURL);
				}

				return null;
			}

			_runningScripts.Add(script);

			// Restore previously executing script
			_executingScript = previousScript;

			return script;
		}

		/// <summary>
		/// Loads and starts all scripts.
		/// </summary>
		public void Start()
		{
			if (_scriptTypes.Count != 0 || _runningScripts.Count != 0)
			{
				return; // Cannot start script domain if scripts are already running
			}

			Log.Message(Log.Level.Debug, "Loading scripts from ", ScriptPath, " ...");

			if (!Directory.Exists(ScriptPath))
			{
				Log.Message(Log.Level.Warning, "Failed to reload scripts because the ", ScriptPath, " directory is missing.");
				return;
			}

			// Find all script files and assemblies in the specified script directory
			var sourceFiles = new List<string>();
			var assemblyFiles = new List<string>();

			try
			{
				sourceFiles.AddRange(Directory.GetFiles(ScriptPath, "*.vb", SearchOption.AllDirectories));
				sourceFiles.AddRange(Directory.GetFiles(ScriptPath, "*.cs", SearchOption.AllDirectories));

				assemblyFiles.AddRange(Directory.GetFiles(ScriptPath, "*.dll", SearchOption.AllDirectories)
					.Where(x => IsManagedAssembly(x)));
			}
			catch (Exception ex)
			{
				Log.Message(Log.Level.Error, "Failed to reload scripts: ", ex.ToString());
			}

			// Filter out non-script assemblies
			for (int i = 0; i < assemblyFiles.Count; i++)
			{
				try
				{
					var assemblyName = AssemblyName.GetAssemblyName(assemblyFiles[i]);

					if (!assemblyName.Name.StartsWith("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}

					// Delete copies of SHVDN, since these can cause issues with the assembly binder loading multiple copies
					File.Delete(assemblyFiles[i]);

					assemblyFiles.RemoveAt(i--);
				}
				catch (Exception ex)
				{
					Log.Message(Log.Level.Warning, "Ignoring assembly file ", Path.GetFileName(assemblyFiles[i]), " because of exception: ", ex.ToString());

					assemblyFiles.RemoveAt(i--);
				}
			}

			foreach (string filename in sourceFiles)
			{
				LoadScriptsFromSource(filename);
			}

			foreach (string filename in assemblyFiles)
			{
				LoadScriptsFromAssembly(filename);
			}

			if (DeprecatedScriptAssemblyNamesPerApiVersion.Count > 0)
			{
				WarnOfScriptsUsingDeprecatedApi();
			}

			// Instantiate scripts after they were all loaded, so that dependencies are launched with the right ordering
			foreach (Type type in _scriptTypes.Values.Select(x => x.Item2))
			{
				// Start the script unless script does not want a default instance
				if (GetScriptAttribute(type, "NoDefaultInstance") is bool NoDefaultInstance && NoDefaultInstance)
				{
					continue;
				}

				InstantiateScript(type)?.Start(!(GetScriptAttribute(type, "NoScriptThread") is bool NoScriptThread) || !NoScriptThread);
			}

			void WarnOfScriptsUsingDeprecatedApi()
			{
				if (ShouldWarnOfScriptsBuiltAgainstDeprecatedApiWithTicker)
				{
					int scriptCountUsingDeprecatedApi = DeprecatedScriptAssemblyNamesPerApiVersion.Values.Aggregate(0, (result, current) => result + current.Count);

					PostTickerToFeed(
						message: $"~o~WARNING~s~: {scriptCountUsingDeprecatedApi.ToString()} scripts are using the v2 API (ScriptHookVDotNet2.dll), which is deprecated and not actively supported. See the console outputs or the log file for more details.",
						isImportant: true,
						cacheMessage: false
					);
				}

				foreach (KeyValuePair<int, List<string>> apiVersionAndScriptNameDict in DeprecatedScriptAssemblyNamesPerApiVersion)
				{
					int apiVersion = apiVersionAndScriptNameDict.Key;
					int scriptAssemblyCount = apiVersionAndScriptNameDict.Value.Count;

					string apiVersionString = apiVersion != 0 ? $"{apiVersion.ToString()}.x" : "0.x or 1.x (fallbacked to the v2 API)";

					Log.Message(Log.Level.Warning, $"Found {scriptAssemblyCount.ToString()} script(s) resolved to the deprecated API version {apiVersionString} (ScriptHookVDotNet2.dll). The v2 API is no longer actively supported. Please report to the authors who developed some of the deprecated scripts. The list of script names:");
					foreach (string scriptName in apiVersionAndScriptNameDict.Value)
					{
						Log.Message(Log.Level.Warning, scriptName);
					}
				}
			}
		}
		/// <summary>
		/// Loads and starts all scripts in the specified file.
		/// </summary>
		/// <param name="filename"></param>
		public void StartScripts(string filename)
		{
			filename = Path.GetFullPath(filename);

			bool isAssembly = Path.GetExtension(filename).Equals(".dll", StringComparison.OrdinalIgnoreCase);
			if (isAssembly ? !LoadScriptsFromAssembly(filename) : !LoadScriptsFromSource(filename))
			{
				return;
			}

			// Instantiate only those scripts that are from the this assembly
			foreach (Type type in _scriptTypes.Values.Where(x => x.Item1 == filename).Select(x => x.Item2))
			{
				// Make sure there are no others instances of this script
				_runningScripts.RemoveAll(x => x.Filename == filename && x.ScriptInstance.GetType() == type);

				// Start the script unless script does not want a default instance
				if (GetScriptAttribute(type, "NoDefaultInstance") is bool NoDefaultInstance && NoDefaultInstance)
				{
					continue;
				}

				InstantiateScript(type)?.Start(!(GetScriptAttribute(type, "NoScriptThread") is bool NoScriptThread) || !NoScriptThread);
			}
		}
		/// <summary>
		/// Aborts all running scripts.
		/// </summary>
		public void Abort()
		{
			foreach (Script script in _runningScripts)
			{
				script.Abort();
			}

			_scriptTypes.Clear();
			_runningScripts.Clear();
		}
		/// <summary>
		/// Aborts all running scripts from the specified file.
		/// </summary>
		/// <param name="filename"></param>
		public void AbortScripts(string filename)
		{
			filename = Path.GetFullPath(filename);

			foreach (Script script in _runningScripts.Where(x => filename.Equals(x.Filename, StringComparison.OrdinalIgnoreCase)))
			{
				script.Abort();
			}
		}

		/// <summary>
		/// Execute a script task in this script domain with the tls context of the main thread of the exe.
		/// You must use this method when you call native functions or functions that may access some
		/// resources of the tls context of the main thread, such as a <c>rage::sysMemAllocator</c>.
		/// </summary>
		/// <param name="task">The task to execute.</param>
		public void ExecuteTaskWithGameThreadTlsContext(IScriptTask task)
		{
			if (_gameMainThreadIdUnmanaged == GetCurrentThreadId())
			{
				// Request came from the main thread of the exe, so can just execute it right away
				task.Run();
			}
			else
			{
				unsafe
				{
					IntPtr tlsContextOfScriptThread = _getTlsContext();
					_setTlsContext(_tlsContextOfMainThread);

					try
					{
						task.Run();
					}
					finally
					{
						// Need to revert TLS context to the real one of the script thread
						_setTlsContext(tlsContextOfScriptThread);
					}
				}
			}
		}

		/// <summary>
		/// Execute a script task in this script domain.
		/// </summary>
		/// <param name="task">The task to execute.</param>
		public void ExecuteTaskInScriptDomainThread(IScriptTask task)
		{
			if (Thread.CurrentThread.ManagedThreadId == _executingThreadId)
			{
				// Request came from the script domain thread, so can just execute it right away
				task.Run();
			}
			else
			{
				_taskQueue.Enqueue(task);
				SignalAndWait(_executingScript._waitEvent, _executingScript._continueEvent);
			}
		}

		/// <summary>
		/// Gets the key down status of the specified key.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns><see langword="true" /> if the key is currently pressed or <see langword="false" /> otherwise</returns>
		public bool IsKeyPressed(Keys key)
		{
			return _keyboardState[(int)key];
		}
		/// <summary>
		/// Pauses or resumes handling of keyboard events in this script domain.
		/// </summary>
		/// <param name="pause"><see langword="true" /> to pause or <see langword="false" /> to resume</param>
		public void PauseKeyEvents(bool pause)
		{
			_recordKeyboardEvents = !pause;
		}

		/// <summary>
		/// Main execution logic of the script domain.
		/// </summary>
		internal void DoTick()
		{
			// Execute running scripts
			for (int i = 0; i < _runningScripts.Count; i++)
			{
				Script script = _runningScripts[i];

				// Ignore terminated scripts
				if (!script.IsRunning || script.IsPaused)
				{
					continue;
				}

				_executingScript = script;

				int startTimeTickCount = Environment.TickCount;
				try
				{
					if (script.IsUsingThread)
					{
						// Resume script thread and execute any incoming tasks from it
						SignalAndWait(script._continueEvent, script._waitEvent);
						while (_taskQueue.Count > 0)
						{
							_taskQueue.Dequeue().Run();
							SignalAndWait(script._continueEvent, script._waitEvent);
						}
					}
					else
					{
						script.DoTick();
					}
				}
				catch (Exception ex)
				{
					HandleUnhandledException(script, new UnhandledExceptionEventArgs(ex, true));

					// Stop script in case of an unhandled exception during task execution
					script.Abort();

					_executingScript = null;
					continue;
				}

				_executingScript = null;

				// Tolerate long execution time if a debugger is attached since some script may be debugged using breakpoints
				if ((uint)(Environment.TickCount - startTimeTickCount) < ScriptTimeoutThreshold || IsDebuggerPresent())
				{
					continue;
				}

				Log.Message(Log.Level.Error, $"Blocking script! Script {script.Name} (file name: {Path.GetFileName(script.Filename)}) was terminated because it caused the game to freeze too long.");

				// Wait operation above timed out, which means that the script did not send any task for some time, so abort it
				script.Abort();
				continue;
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
			_keyboardState[(int)e.KeyCode] = status;

			if (!_recordKeyboardEvents)
			{
				return;
			}

			var eventInfo = new Tuple<bool, KeyEventArgs>(status, e);

			foreach (Script script in _runningScripts)
			{
				script._keyboardEvents.Enqueue(eventInfo);
			}
		}

		/// <summary>
		/// Free memory for all pinned strings.
		/// </summary>
		private void CleanupStrings()
		{
			foreach (IntPtr handle in _pinnedStrings)
			{
				Marshal.FreeCoTaskMem(handle);
			}

			_pinnedStrings.Clear();
		}
		/// <summary>
		/// Pins the memory of a string so that it can be used in native calls without worrying about the GC invalidating its pointer.
		/// </summary>
		/// <param name="str">The string to pin to a fixed pointer.</param>
		/// <returns>A pointer to the pinned memory containing the string.</returns>
		public IntPtr PinString(string str)
		{
			IntPtr handle = StringMarshal.StringToCoTaskMemUtf8(str);

			if (handle == IntPtr.Zero)
			{
				return NativeMemory.NullString;
			}

			_pinnedStrings.Add(handle);
			return handle;
		}

		/// <summary>
		/// Finds the script object representing the specified <paramref name="scriptInstance"/> object.
		/// </summary>
		/// <param name="scriptInstance">The 'GTA.Script' instance to check.</param>
		public Script LookupScript(object scriptInstance)
		{
			if (scriptInstance == null)
			{
				return null;
			}

			// Return matching script in running script list if one is found
			Script script = _runningScripts.FirstOrDefault(x => x.ScriptInstance == scriptInstance);

			// Otherwise return the executing script, since during constructor execution the running script list was not yet updated
			if (script != null || _executingScript == null || _executingScript.ScriptInstance != null)
			{
				return script;
			}

			// Handle the case where a script creates a custom instance of a script class that is not managed by SHVDN
			// These may attempt to set events, but are not allowed to do so, since SHVDN will never call them, so just return null
			if (!_executingScript.Name.Contains(scriptInstance.GetType().FullName))
			{
				Log.Message(Log.Level.Warning, "A script tried to use a custom script instance of type ", scriptInstance.GetType().FullName, " that was not instantiated by ScriptHookVDotNet.");
				return null;
			}

			script = _executingScript;

			return script;
		}
		public string LookupScriptFilename(Type scriptType)
		{
			return _scriptTypes.Values.FirstOrDefault(x => x.Item2 == scriptType)?.Item1 ?? string.Empty;
		}

		/// <summary>
		/// Checks if the script has a 'GTA.ScriptAttributes' attribute with the specified argument attached to it and returns it.
		/// </summary>
		/// <param name="scriptType">The script type to check for the attribute.</param>
		/// <param name="name">The named argument to search.</param>
		private static object GetScriptAttribute(Type scriptType, string name)
		{
			CustomAttributeData attribute = scriptType.GetCustomAttributesData().FirstOrDefault(x => x.AttributeType.FullName == "GTA.ScriptAttributes");

			if (attribute == null)
			{
				return null;
			}

			foreach (CustomAttributeNamedArgument arg in attribute.NamedArguments)
			{
				if (arg.MemberName == name)
				{
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

		private static void SignalAndWait(SemaphoreSlim toSignal, SemaphoreSlim toWaitOn)
		{
			toSignal.Release();
			toWaitOn.Wait();
		}

		private static bool IsSubclassOf(Type type, string baseTypeName)
		{
			for (Type t = type.BaseType; t != null; t = t.BaseType)
			{
				if (t.FullName == baseTypeName)
				{
					return true;
				}
			}

			return false;
		}

		private static Version GetBaseTypeVersion(Type type, string baseTypeName)
		{
			for (Type t = type.BaseType; t != null; t = t.BaseType)
			{
				if (t.FullName == baseTypeName)
				{
					return t.Assembly.GetName().Version;
				}
			}

			return null;
		}

		private static bool IsManagedAssembly(string filename)
		{
			try
			{
				using Stream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
				if (file.Length < 64)
				{
					return false;
				}

				using var bin = new BinaryReader(file);
				// PE header starts at offset 0x3C (60). Its a 4 byte header.
				file.Position = 0x3C;
				uint offset = bin.ReadUInt32();
				if (offset == 0)
				{
					offset = 0x80;
				}

				// Ensure there is at least enough room for the following structures:
				//     24 byte PE Signature & Header
				//     28 byte Standard Fields         (24 bytes for PE32+)
				//     68 byte NT Fields               (88 bytes for PE32+)
				// >= 128 byte Data Dictionary Table
				if (offset > file.Length - 256)
				{
					return false;
				}

				// Check the PE signature. Should equal 'PE\0\0'.
				file.Position = offset;
				if (bin.ReadUInt32() != 0x00004550)
				{
					return false;
				}

				// Read PE magic number from Standard Fields to determine format.
				file.Position += 20;
				ushort peFormat = bin.ReadUInt16();
				if (peFormat != 0x10b /* PE32 */ && peFormat != 0x20b /* PE32Plus */)
				{
					return false;
				}

				// Read the 15th Data Dictionary RVA field which contains the CLI header RVA.
				// When this is non-zero then the file contains CLI data otherwise not.
				file.Position = offset + (peFormat == 0x10b ? 232 : 248);
				return bin.ReadUInt32() != 0;
			}
			catch
			{
				// This is likely not a valid assembly if any IO exceptions occur during reading
				return false;
			}
		}

		private static Assembly HandleResolve(object sender, ResolveEventArgs args)
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
				// No warning will be printed from here. Once one script that references a version-less SHVDN is resolved, SHVDN will not execute this block when SHVDN resolves other script that reference a version-less SHVDN.
				if (assemblyName.Version == bestVersion)
				{
					return CurrentDomain._scriptApis.FirstOrDefault(x => x.GetName().Version.Major == 2);
				}

				Assembly compatibleApi = null;

				foreach (Assembly api in CurrentDomain._scriptApis)
				{
					Version apiVersion = api.GetName().Version;

					// Find the newest compatible scripting API version
					if (assemblyName.Version.Major != apiVersion.Major || apiVersion < assemblyName.Version || apiVersion <= bestVersion)
					{
						continue;
					}

					bestVersion = apiVersion;
					compatibleApi = api;
				}

				// Write a warning message if no compatible scripting API version was found
				if (compatibleApi == null)
				{
					Log.Message(Log.Level.Warning, "Unable to resolve API version ", assemblyName.Version.ToString(3),
						args.RequestingAssembly != null ? " referenced in " + args.RequestingAssembly.GetName().Name : string.Empty, ".");
				}

				return compatibleApi;
			}

			// Try to resolve referenced assemblies that the assembly loader failed to find by itself (e.g. because they are in a subdirectory of the scripts directory)
			if (CurrentDomain == null)
			{
				return null;
			}

			string filename = Directory
				.GetFiles(CurrentDomain.ScriptPath, "*.dll", SearchOption.AllDirectories)
				.FirstOrDefault(x => x.EndsWith(assemblyName.Name + ".dll", StringComparison.OrdinalIgnoreCase));
			if (filename != null)
			{
				return Assembly.LoadFrom(filename);
			}

			return null;
		}

		internal static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			Log.Message(Log.Level.Error, $"Caught unhandled exception:", Environment.NewLine, args.ExceptionObject.ToString());

			if (sender is not Script script)
			{
				return;
			}

			Log.Message(Log.Level.Error, "The exception was thrown while executing the script ", script.Name, ".");

			if (GetScriptAttribute(script.ScriptInstance.GetType(), "SupportURL") is string supportURL)
			{
				Log.Message(Log.Level.Error, "Please check the following site for support on the issue: ", supportURL);
			}

			// Show a notification with the script crash information
			ScriptDomain domain = ScriptDomain.CurrentDomain;
			if (domain != null && domain._executingScript != null)
			{
				PostTickerToFeed(
					message: "~r~Unhandled exception~s~ in script \"~h~" + script.Name + "~h~\"!~n~~n~~r~" + args.ExceptionObject.GetType().Name + "~s~ " + ((Exception)args.ExceptionObject).StackTrace.Split('\n').FirstOrDefault().Trim(),
					isImportant: true,
					cacheMessage: false
					);
			}
		}

		private static void PostTickerToFeed(string message, bool isImportant, bool cacheMessage = true)
		{
			unsafe
			{
				fixed (byte* cellEmailBconStrPtr = s_cellEmailBconByteStr)
				{
					NativeFunc.Invoke(0x202709F4C58A0424 /* BEGIN_TEXT_COMMAND_THEFEED_POST */, (ulong)cellEmailBconStrPtr);
					NativeFunc.PushLongString(message);
					NativeFunc.Invoke(0x2ED7843F8F801023 /* END_TEXT_COMMAND_THEFEED_POST_TICKER */, isImportant, cacheMessage);
				}
			}
		}

		private void AddScriptAssemblyNameBuiltAgainstApiVersion(int apiVersion, string fileName)
		{
			if (!DeprecatedScriptAssemblyNamesPerApiVersion.TryGetValue(apiVersion, out List<string> list))
			{
				DeprecatedScriptAssemblyNamesPerApiVersion[apiVersion] = new List<string>() { fileName };
				return;
			}

			list.Add(fileName);
			return;
		}

		private static bool IsApiVersionDeprecated(Version apiVersion) => apiVersion.Major < 3;
	}
}
