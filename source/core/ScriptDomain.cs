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
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

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
        internal sealed class ScriptAssemblyInfo
        {
            public Assembly Assembly { get; }
            public string FileName { get; }

            public ScriptAssemblyInfo(Assembly asm, string fileName)
            {
                Assembly = asm;
                FileName = fileName;
            }
        }

        internal sealed class ScriptTypeInfo
        {
            public ScriptAssemblyInfo AssemblyInfo { get; }
            public Type Type { get; }
            public Version TargetApiVersion { get; }

            // We don't validate if the passed `Type` isn't abstract and is a subclass of `GTA.Script`, because
            // these checks aren't that cheap
            public ScriptTypeInfo(ScriptAssemblyInfo assemblyInfo, Type type, Version targetApiVersion)
            {
                AssemblyInfo = assemblyInfo;
                Type = type;
                TargetApiVersion = targetApiVersion;
            }
        }

        public enum AbortScriptMode
        {
            Default,
            Off,
            On,
        }

        internal class ScriptInitOption
        {
            public bool NativeCallResetsTimeout { get; }

            public ScriptInitOption(bool nativeCallResetsTimeout)
            {
                NativeCallResetsTimeout = nativeCallResetsTimeout;
            }
        }

        // Debugger.IsAttached does not detect a Visual Studio debugger
        [SuppressUnmanagedCodeSecurity]
        [DllImport("Kernel32.dll")]
        internal static extern bool IsDebuggerPresent();

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        private static readonly Version s_FirstApiVerThatHasSeparateApiModuleFromAsi = new Version(2, 10, 0, 0);
        private static readonly Version s_FirstVerWhereScriptingAssemblyHasProperVersionInfo = new Version(2, 9, 0, 0);
        private static readonly Version s_LastVerWhereScriptingAssemblyDoesNotHaveProperVersionInfo = new Version(2, 8, 0, 0);
        private static readonly Version s_FirstApiVerWhereNativeCallDoesntResetTimeoutByDefault = new Version(3, 7, 0, 0);

        private static readonly Regex s_ScriptingApiModuleNamePattern
            = new Regex(@"^ScriptHookVDotNet\d\.dll$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex s_ScriptingApiModuleNamePatternWithVersionCapture
            = new Regex(@"^ScriptHookVDotNet(?<ver>\d)\.dll$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static ScriptDomain s_currentDomain;
        // `Dispose` won't be called in this instance because it may be too difficult to call correctly due to how
        // separate `AppDomain`s have different static variables, but it's acceptable since `ReaderWriterLockSlim` uses
        // `EventWaitHandle`, which uses `SafeHandle`.
        private static readonly ReaderWriterLockSlim s_currentDomainPropertyLock = new();

        private readonly int _executingThreadId = Thread.CurrentThread.ManagedThreadId;
        private Script _executingScript = null;
        private readonly List<IntPtr> _pinnedStrings = new();
        private readonly List<Script> _runningScripts = new();
        private readonly ConcurrentQueue<IScriptTask> _taskQueue = new();
        // this is only used in the main thread of `ScriptDomain`, so no lock is needed
        private readonly Dictionary<string, int> _scriptInstances = new();
        private readonly SortedList<string, ScriptTypeInfo> _scriptTypes = new();
        private bool _recordKeyboardEvents = true;
        private bool[] _keyboardState = new bool[256];
        private readonly List<Assembly> _scriptingApiAsms = new List<Assembly>();
        private readonly HashSet<string> _scriptingApiAsmNamesCache = new HashSet<string>();
        private readonly Dictionary<int, Type> _scriptingGtaClassTypesCacheDict = new Dictionary<int, Type>();
        // Intentionally use array over `HashSet` because only 2 or 3 elements will be inserted for sure, where
        // HashSet takes way more time (like 2x or 3x time) to search, at least for `System.Type`.
        private readonly Type[] _scriptingGtaClassTypesCacheArray = Array.Empty<Type>();

        private unsafe delegate* unmanaged[Cdecl]<IntPtr> _getTlsContext;
        private unsafe delegate* unmanaged[Cdecl]<IntPtr, void> _setTlsContext;

        private IntPtr _tlsContextOfMainThread;
        private uint _gameMainThreadIdUnmanaged;

        private bool _areTlsVarsInitialized;

        // Since `ConcurrentQueue` doesn't have `Clear` method in .NET Framework where the head and tail will be set
        // to a new segment instance, we read `_pinnedStrings` sequentially to dispose pinned strings.
        private readonly object _pinnedStrListLock = new();
        private readonly ReaderWriterLockSlim _tlsVariablesLock = new();

        // These locks are used to avoid race conditions, but the code looks so terrible with a lot of lock blocks.
        // If there is a better way to avoid using them a lot by refactoring the code especially on data structures,
        // it would be much appreciated.
        private readonly object _lockForFieldsThatFrequentlyWritten = new();
        private readonly ReaderWriterLockSlim _rwLock = new();

        /// <summary>
        /// The byte array that contains "CELL_EMAIL_BCON", which is used when SHVDN logs unhandled exceptions
        /// or old scripting SDK/API version warning via a feed ticker.
        /// </summary>
        /// <remarks>
        /// This is not a unmanaged pointer to a pinned string since this member would not be frequently used.
        /// </remarks>
        private static byte[] s_cellEmailBconByteStr = Encoding.ASCII.GetBytes("CELL_EMAIL_BCON\0");

        internal unsafe void InitTlsStuffForTlsContextSwitch(IntPtr getTlsContextFunc, IntPtr setTlsContextFunc,
            IntPtr tlsAddr, uint threadId)
        {
            _tlsVariablesLock.EnterWriteLock();
            try
            {
                _getTlsContext = (delegate* unmanaged[Cdecl]<IntPtr>)getTlsContextFunc;
                _setTlsContext = (delegate* unmanaged[Cdecl]<IntPtr, void>)setTlsContextFunc;
                _tlsContextOfMainThread = tlsAddr;
                _gameMainThreadIdUnmanaged = threadId;
                _areTlsVarsInitialized = true;
            }
            finally
            {
                _tlsVariablesLock.ExitWriteLock();
            }
        }

        internal bool IsTlsStuffInitialized()
        {
            _tlsVariablesLock.EnterReadLock();
            try
            {
                return _areTlsVarsInitialized;
            }
            finally
            {
                _tlsVariablesLock.ExitReadLock();
            }
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
        public static ScriptDomain CurrentDomain
        {
            get
            {
                s_currentDomainPropertyLock.EnterReadLock();
                try
                {
                    return s_currentDomain;
                }
                finally
                {
                    s_currentDomainPropertyLock.ExitReadLock();
                }
            }
            set
            {
                s_currentDomainPropertyLock.EnterWriteLock();
                try
                {
                    s_currentDomain = value;
                }
                finally
                {
                    s_currentDomainPropertyLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Gets the list of currently running scripts in this script domain. This is used by the console implementation.
        /// </summary>
        public Script[] RunningScripts
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return _runningScripts.ToArray();
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets the currently executing script or <see langword="null" /> if there is none.
        /// </summary>
        public static Script ExecutingScript
        {
            get
            {
                ScriptDomain dom = CurrentDomain;
                if (dom == null)
                {
                    return null;
                }

                lock (dom._lockForFieldsThatFrequentlyWritten)
                {
                    return dom._executingScript;
                }
            }
        }

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
                if (!s_ScriptingApiModuleNamePattern.IsMatch(Path.GetFileName(apiPath)))
                {
                    continue;
                }

                Log.Message(Log.Level.Debug, "Loading API from ", apiPath, " ...");

                try
                {
                    _scriptingApiAsms.Add(Assembly.LoadFrom(apiPath));
                }
                catch (Exception ex)
                {
                    Log.Message(Log.Level.Error, "Unable to load ", Path.GetFileName(apiPath), ": ", ex.ToString());
                }
            }

            // Sort the api list by major version so the order is guaranteed to be sorted in ascending order regardless of how Directory.EnumerateFiles enumerates
            // as long as all of major versions are unique
            _scriptingApiAsms.Sort((x, y) => x.GetName().Version.Major.CompareTo(y.GetName().Version.Major));
            foreach (Assembly apiAsm in _scriptingApiAsms)
            {
                AssemblyName asmName = apiAsm.GetName();
                _scriptingApiAsmNamesCache.Add(asmName.Name);

                if (TryFindTypeByFullName(apiAsm, "GTA.Script", out Type gtaScriptType))
                {
                    _scriptingGtaClassTypesCacheDict.Add(asmName.Version.Major, gtaScriptType);
                }
                else
                {
                    Log.Message(Log.Level.Error, "Could not find `GTA.Script` type in ",
                        Path.GetFileName(apiAsm.Location), ".", "Report to the developers of SHVDN as this should be " +
                        "a bug.");
                }
            }
            _scriptingGtaClassTypesCacheArray = _scriptingGtaClassTypesCacheDict.Values.ToArray();

            if (_scriptingApiAsms.Count == 0)
            {
                Log.Message(Log.Level.Error, "No scripting API .dll files (\"ScriptHookVDotNet*.dll\") were loaded, " +
                    "and therefore ScriptHookVDotNet can't load any scripts or have the console work including " +
                    "the reload feature except for displaying logs. Make sure *at least* ScriptHookVDotNet3.dll is in" +
                    "the root directory, so the console can work and scripts that are built against only " +
                    "ScriptHookVDotNet3.dll (the v3 API) can work.");

                return;
            }
            else if (!_scriptingGtaClassTypesCacheDict.TryGetValue(3, out Type _))
            {
                Log.Message(Log.Level.Warning, "ScriptHookVDotNet3.dll is not loaded, and therefore ScriptHookVDotNet " +
                    "can't have the console work except for displaying logs. You should make sure the dll file is in " +
                    "the root directory, so the console can work. You can't reload scripts via the console because " +
                    "it is not working properly.");
            }
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
            _tlsVariablesLock.Dispose();
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
            string extension = Path.GetExtension(filename);
            System.CodeDom.Compiler.CodeDomProvider compiler = null;

            string additionalCompilerOptions = string.Empty;
            if (extension.Equals(".cs", StringComparison.OrdinalIgnoreCase))
            {
                compiler = new Microsoft.CSharp.CSharpCodeProvider();
                additionalCompilerOptions = " /unsafe";
            }
            else if (extension.Equals(".vb", StringComparison.OrdinalIgnoreCase))
            {
                compiler = new Microsoft.VisualBasic.VBCodeProvider();
            }
            else
            {
                return false;
            }

            // Support specifying the API version to be used in the file name like "script.3.cs"
            string apiVersionString = Path.GetExtension(Path.GetFileNameWithoutExtension(filename));
            if (!string.IsNullOrEmpty(apiVersionString) && int.TryParse(apiVersionString.Substring(1), out int apiVersion))
            {
                Assembly scriptApi = ScriptDomain.CurrentDomain._scriptingApiAsms.FirstOrDefault(
                    x => x.GetName().Version.Major == apiVersion);

                if (scriptApi == null)
                {
                    string apiVersionStr = "ScriptHookVDotNet" + apiVersion.ToString() + ".dll";
                    Log.Message(Log.Level.Error, "Could not compile ", Path.GetFileName(filename), " because " +
                        "the scripting API with the specified version (", apiVersionStr, ") to compile scripts " +
                        "is not loaded.");
                    return false;
                }

                return CompileScriptsFromAssemblyVersionWithNotatedApi(filename, scriptApi, compiler,
                    additionalCompilerOptions);
            }

            return CompileScriptsFromAssemblyWithFirstNonDeprecatedApiAndLastDeprecatedApi(filename, compiler,
                additionalCompilerOptions);
        }
        private CompilerResults CompileScriptsFromAssembly(string filename, Assembly scriptApi,
            System.CodeDom.Compiler.CodeDomProvider compiler, string additionalCompilerOptions)
        {
            var compilerOptions = new System.CodeDom.Compiler.CompilerParameters();
            compilerOptions.CompilerOptions = "/optimize";
            compilerOptions.CompilerOptions += additionalCompilerOptions;
            compilerOptions.GenerateInMemory = true;
            compilerOptions.IncludeDebugInformation = true;
            compilerOptions.ReferencedAssemblies.Add("System.dll");
            compilerOptions.ReferencedAssemblies.Add("System.Core.dll");
            compilerOptions.ReferencedAssemblies.Add("System.Drawing.dll");
            compilerOptions.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            compilerOptions.ReferencedAssemblies.Add("System.XML.dll");
            compilerOptions.ReferencedAssemblies.Add("System.XML.Linq.dll");
            compilerOptions.ReferencedAssemblies.Add(typeof(ScriptDomain).Assembly.Location);
            compilerOptions.ReferencedAssemblies.Add(scriptApi.Location);

            return compiler.CompileAssemblyFromFile(compilerOptions, filename);
        }
        private bool CompileScriptsFromAssemblyVersionWithNotatedApi(string filename, Assembly scriptApi,
            System.CodeDom.Compiler.CodeDomProvider compiler, string additionalCompilerOptions)
        {
            CompilerResults compilerResults = CompileScriptsFromAssembly(filename, scriptApi, compiler,
                additionalCompilerOptions);

            if (!compilerResults.Errors.HasErrors)
            {
                Log.Message(Log.Level.Debug, "Successfully compiled ", Path.GetFileName(filename), ".");
                return LoadScriptsFromAssembly(compilerResults.CompiledAssembly, filename);
            }

            LogCompilerErrorForRawScript(compilerResults, filename, scriptApi);
            return false;
        }
        private bool CompileScriptsFromAssemblyWithFirstNonDeprecatedApiAndLastDeprecatedApi(string filename,
            System.CodeDom.Compiler.CodeDomProvider compiler, string additionalCompilerOptions)
        {
            // Reference the oldest scripting API that is not deprecated by default to stay compatible with existing scripts
            Assembly firstNonDeprecatedScriptApi = _scriptingApiAsms.FirstOrDefault(x => !IsApiVersionDeprecated(x.GetName().Version));
            Assembly lastDeprecatedScriptApi = _scriptingApiAsms.LastOrDefault(x => IsApiVersionDeprecated(x.GetName().Version));

            bool foundfirstNonDeprecatedScriptApi = firstNonDeprecatedScriptApi != null;
            bool foundLastDeprecatedScriptApi = lastDeprecatedScriptApi != null;

            string scriptFileName = Path.GetFileName(filename);

            if (!foundfirstNonDeprecatedScriptApi && !foundLastDeprecatedScriptApi)
            {
                Log.Message(Log.Level.Error, "Could not compile ", scriptFileName, " because " +
                    "there are not any loaded scripting APIs to compile scripts.");
                return false;
            }

            if (foundfirstNonDeprecatedScriptApi)
            {
                CompilerResults compilerResultsWithFirstNonDeprecatedApi
                    = CompileScriptsFromAssembly(filename, firstNonDeprecatedScriptApi, compiler,
                    additionalCompilerOptions);

                if (!compilerResultsWithFirstNonDeprecatedApi.Errors.HasErrors)
                {
                    string scriptFileNameWithoutExt = Path.GetFileNameWithoutExtension(filename);
                    string extensionStrOfScriptFileName = Path.GetExtension(filename);

                    string scriptFileNameCandidateVersionAnnotated = scriptFileNameWithoutExt + ".2" + extensionStrOfScriptFileName;

                    Log.Message(Log.Level.Debug, "Successfully compiled ", scriptFileName, " using API version ",
                        firstNonDeprecatedScriptApi.GetName().Version.ToString(3), ". " +
                        "If you find the script not working as the author(s) intended, you could annotate an API " +
                        "version by adding a dot and a single-digit number for API version before the extension " +
                        "(e.g. \"", scriptFileNameCandidateVersionAnnotated, "\").");
                    return LoadScriptsFromAssembly(compilerResultsWithFirstNonDeprecatedApi.CompiledAssembly, filename);
                }
                else
                {
                    LogCompilerErrorForRawScript(compilerResultsWithFirstNonDeprecatedApi, filename,
                        firstNonDeprecatedScriptApi);

                    if (!foundLastDeprecatedScriptApi)
                    {
                        return false;
                    }
                    Log.Message(Log.Level.Info, "Fallbacking to the last deprecated API version ",
                        lastDeprecatedScriptApi.GetName().Version.ToString(3), " to compile ",
                        Path.GetFileName(filename), "...");
                }
            }

            if (foundLastDeprecatedScriptApi)
            {
                CompilerResults compilerResultsWithLastDeprecatedApi
                    = CompileScriptsFromAssembly(filename, lastDeprecatedScriptApi, compiler,
                    additionalCompilerOptions);
                if (!compilerResultsWithLastDeprecatedApi.Errors.HasErrors)
                {
                    lastDeprecatedScriptApi.GetName().Version.ToString(3);
                    Log.Message(Log.Level.Debug, "Successfully compiled ", Path.GetFileName(filename),
                        " using deprecated API version ", lastDeprecatedScriptApi.GetName().Version.ToString(3),
                        ". You could let ScriptHookVDotNet compile faster by adding \".",
                        lastDeprecatedScriptApi.GetName().Version.ToString(1), "\" before the extension name of " +
                        "the file name.");
                    return LoadScriptsFromAssembly(compilerResultsWithLastDeprecatedApi.CompiledAssembly, filename);
                }

                LogCompilerErrorForRawScript(compilerResultsWithLastDeprecatedApi, filename,
                    firstNonDeprecatedScriptApi);
                return false;
            }

            return false;
        }
        private void LogCompilerErrorForRawScript(CompilerResults res, string scriptFileName, Assembly scriptApi)
        {
            var errors = new System.Text.StringBuilder();

            foreach (System.CodeDom.Compiler.CompilerError error in res.Errors)
            {
                errors.Append("   at line ");
                errors.Append(error.Line);
                errors.Append(": ");
                errors.Append(error.ErrorText);
                errors.AppendLine();
            }

            Log.Message(Log.Level.Error, "Failed to compile ", Path.GetFileName(scriptFileName), " using API version ",
                scriptApi.GetName().Version.ToString(3), " with ", res.Errors.Count.ToString(),
                " error(s):", Environment.NewLine, errors.ToString());
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
            int scriptTypeCount = 0;
            Version resolvedApiVersion = null;
            ScriptAssemblyInfo asmInfo = new ScriptAssemblyInfo(assembly, filename);

            MakeTargetApisDictPerMajorVerFromAssemblyRefs(assembly, out Dictionary<int, Version> targetApis,
                out int[] missingApiVersions);

            // Don't bother to enumerate the types if the assembly doesn't have any references to scripting APIs
            // because enumerating types is expensive enough for users to notice the difference
            if (targetApis.Count == 0)
            {
                if (missingApiVersions.Length == 1)
                {
                    string missingApiStr = "ScriptHookVDotNet" + missingApiVersions[0].ToString() + ".dll";
                    Log.Message(Log.Level.Error, "Failed to load scripts in ", Path.GetFileName(filename),
                        " because ", missingApiStr, " is missing in the root directory.");
                }
                else if (missingApiVersions.Length > 1)
                {
                    string missingApiVersionsStr = string.Join(", ", missingApiVersions);
                    Log.Message(Log.Level.Error, "Failed to load scripts in ", Path.GetFileName(filename),
                    " because ", missingApiVersionsStr, " are missing in the root directory.");
                }
                else
                {
                    Log.Message(Log.Level.Info, "Found no compatible scripts in ", Path.GetFileName(filename),
                    " but loaded for scripts.");
                }
                return false;
            }
            if (targetApis.Count > 1)
            {
                // The script assembly bothered to use multiple API versions, which can be achieved with the alias
                // feature. Annoying shit because we have to find an appropriate API version per concrete script class
                // 🤮
                return LoadScriptsFromAssemblyMultipleApiVers(assembly, filename, targetApis, asmInfo);
            }

            Version targetApiVersion = targetApis.Values.First();
            try
            {
                scriptTypeCount = RegisterScriptTypesInAssembly(assembly, filename, asmInfo, targetApiVersion, ref resolvedApiVersion);
            }
            catch (ReflectionTypeLoadException ex)
            {
                LogScriptAssemblyLoadingFailureUnlessItWasDueToFailureOfApiAsmResolution(ex, filename);

                return false;
            }
            catch (Exception ex)
            {
                Log.Message(Log.Level.Error, "Failed to load assembly ", Path.GetFileName(filename), ": ", ex.ToString());

                return false;
            }

            if (resolvedApiVersion == new Version(0, 0, 0, 0))
            {
                // shouldn't be null if came this path
                Assembly v2ApiAsm = CurrentDomain._scriptingApiAsms.First(x => x.GetName().Version.Major == 2);

                Log.Message(Log.Level.Info, "Found ", scriptTypeCount.ToString(), " script(s) in ", Path.GetFileName(filename),
                    " resolved to API version " + v2ApiAsm.GetName().Version.ToString(3), " (target API version: v2.8 or earlier).");
            }
            else
            {
                // Script may not have any subclasses of `GTA.Script` even if came here, so `resolvedApiVersion` can be
                // null
                string resolvedApiVerSubstr
                    = (resolvedApiVersion != null
                    ? (" resolved to API version " + resolvedApiVersion.ToString(3))
                    : string.Empty);
                Log.Message(Log.Level.Info, "Found ", scriptTypeCount.ToString(), " script(s) in ", Path.GetFileName(filename),
                    resolvedApiVerSubstr, " (target API version: ", targetApiVersion.ToString(3), ").");
            }

            if (resolvedApiVersion != null && IsApiVersionDeprecated(resolvedApiVersion))
            {
                AddScriptAssemblyNameBuiltAgainstApiVersion(resolvedApiVersion.Major, Path.GetFileName(filename));
            }

            return scriptTypeCount != 0;

            int RegisterScriptTypesInAssembly(Assembly assembly, string filename, ScriptAssemblyInfo asmInfo, Version targetApiVersion, ref Version resolvedApiVersion)
            {
                int scriptTypeCount = 0;
                AssemblyName asmName = assembly.GetName();
                string asmNameStr = asmName.Name;
                Version asmVersion = asmName.Version;

                Type gtaScriptType = _scriptingGtaClassTypesCacheDict[targetApiVersion.Major];
                // Find all script types in the assembly
                foreach (Type type in assembly.GetTypes().Where(x => x.IsSubclassOf(gtaScriptType)))
                {
                    scriptTypeCount++;

                    string key = BuildComparisonStringForDependencyKey(type, string.Empty);
                    key = asmNameStr + "-" + asmVersion + key;

                    // Check API version for one of the types. The API version must be the same among all the types
                    // in the assembly, because API assemblies aren't strongly-named and we already filter out the case
                    // where the script assembly references multiple API assemblies.
                    if (resolvedApiVersion == null)
                    {
                        // return value shouldn't be null, because we already filtered out the types that aren't
                        // subclass of `GTA.Script`
                        resolvedApiVersion = GetBaseTypeVersion(type, gtaScriptType);
                    }

                    // The script is likely to add to script types list, so intentionally use write lock here
                    _rwLock.EnterWriteLock();
                    try
                    {
                        if (_scriptTypes.TryGetValue(key, out ScriptTypeInfo scriptTypeInfo))
                        {
                            Log.Message(Log.Level.Warning, "The script name ", type.FullName, " already exists and was loaded from ", Path.GetFileName(scriptTypeInfo.AssemblyInfo.FileName), ". Ignoring occurrence loaded from ", Path.GetFileName(filename), ".");
                            continue; // Skip types that were already added previously are ignored
                        }

                        _scriptTypes.Add(key, new ScriptTypeInfo(asmInfo, type, targetApiVersion));
                    }
                    finally
                    {
                        _rwLock.ExitWriteLock();
                    }
                }

                return scriptTypeCount;
            }
        }

        private bool LoadScriptsFromAssemblyMultipleApiVers(Assembly assembly, string filename, Dictionary<int, Version> targetApis, ScriptAssemblyInfo asmInfo)
        {
            Log.Message(Log.Level.Info, "Resolving API Versions of the scripts in ", Path.GetFileName(filename),
                ", which has multiple references to different API assemblies...");
            Log.Message(Log.Level.Debug, "For Developers: scripts should not use multiple API versions at the same " +
                "time. Loading multiple API versions may not be supported in future SHVDN versions. " +
                "For users: you could contact the author(s) of ", Path.GetFileName(filename), ", and ask them to use " +
                "only one API version.");

            int scriptTypeCount = 0;
            AssemblyName asmName = assembly.GetName();
            string asmNameStr = asmName.Name;
            Version asmVersion = asmName.Version;
            HashSet<Version> resolvedApiVersionSets = new(targetApis.Count);

            try
            {
                // Find all script types in the assembly
                foreach (Type type in assembly.GetTypes().Where(
                    x => IsSubclassOfOneOfTargetTypes(x, _scriptingGtaClassTypesCacheArray)))
                {
                    scriptTypeCount++;

                    string key = BuildComparisonStringForDependencyKey(type, string.Empty);
                    key = asmNameStr + "-" + asmVersion + key;

                    Version resolvedApiVersion
                        = GetVersionOfOneOfTargetBaseTypes(type, _scriptingGtaClassTypesCacheArray);
                    resolvedApiVersionSets.Add(resolvedApiVersion);

                    Version targetApiVersion = null;
                    if (resolvedApiVersion == new Version(0, 0, 0, 0))
                    {
                        // shouldn't be null if came this path
                        Assembly v2ApiAsm = CurrentDomain._scriptingApiAsms.First(x => x.GetName().Version.Major == 2);
                        Log.Message(Log.Level.Info, "Resolved API Version of the script class name ", type.FullName, ": ", v2ApiAsm.GetName().Version.ToString(3), " (target API version: v2.8 or earlier)");

                        targetApiVersion = s_LastVerWhereScriptingAssemblyDoesNotHaveProperVersionInfo;
                    }
                    else if (targetApis.TryGetValue(resolvedApiVersion.Major, out targetApiVersion))
                    {
                        Log.Message(Log.Level.Info, "Resolved API Version of the script class name ", type.FullName, ": ", resolvedApiVersion.ToString(3), " (target API version: ",
                            targetApiVersion.ToString(3), ")");
                    }
                    else
                    {
                        Log.Message(Log.Level.Info, "Resolved API Version of the script name ", type.FullName, ": ", resolvedApiVersion.ToString(3), " (target API version: unknown)");
                        Log.Message(Log.Level.Warning, "Target API version of ", type.FullName, "is unknown. Contact " +
                            "developers of SHVDN as there may be some bugs if you see this warning.");

                        targetApiVersion = s_LastVerWhereScriptingAssemblyDoesNotHaveProperVersionInfo;
                    }

                    // The script is likely to add to script types list, so intentionally use write lock here
                    _rwLock.EnterWriteLock();
                    try
                    {
                        if (_scriptTypes.TryGetValue(key, out ScriptTypeInfo scriptTypeInfo))
                        {
                            Log.Message(Log.Level.Warning, "The script name ", type.FullName, " already exists and was loaded from ", Path.GetFileName(scriptTypeInfo.AssemblyInfo.FileName), ". Ignoring occurrence loaded from ", Path.GetFileName(filename), ".");
                            continue; // Skip types that were already added previously are ignored
                        }

                        _scriptTypes.Add(key, new ScriptTypeInfo(asmInfo, type, targetApiVersion));
                    }
                    finally
                    {
                        _rwLock.ExitWriteLock();
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                LogScriptAssemblyLoadingFailureUnlessItWasDueToFailureOfApiAsmResolution(ex, filename);

                return false;
            }

            Log.Message(Log.Level.Info, "Found ", scriptTypeCount.ToString(), " script(s) in ", Path.GetFileName(filename), ".");


            foreach (Version resolvedApiVerElem in resolvedApiVersionSets)
            {
                if (IsApiVersionDeprecated(resolvedApiVerElem))
                {
                    AddScriptAssemblyNameBuiltAgainstApiVersion(resolvedApiVerElem.Major, Path.GetFileName(filename));
                    break;
                }
            }

            return scriptTypeCount != 0;
        }

        private void MakeTargetApisDictPerMajorVerFromAssemblyRefs(Assembly asm,
            out Dictionary<int, Version> resolvedApis, out int[] missingApiVersions)
        {
            resolvedApis = new Dictionary<int, Version>(_scriptingApiAsmNamesCache.Count);
            SortedSet<int> missingApiVersionList = new SortedSet<int>();

            foreach (AssemblyName asmName in asm.GetReferencedAssemblies())
            {
                if (_scriptingApiAsmNamesCache.Contains(asmName.Name))
                {
                    Version asmVer = asmName.Version;
                    UpdateDictIfValueDoesNotExistOnKeyOrValueIsOlder(resolvedApis, asmVer);
                    continue;
                }
                string asmNameStr = asmName.Name;
                if (!asmNameStr.StartsWith("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase))
                {
                    // we know the assembly isn't ours now
                    continue;
                }

                if (asmNameStr.Length == "ScriptHookVDotNet".Length)
                {
                    Version verInfoInAsm = asmName.Version;
                    Version targetVerToUseForDict
                        = (verInfoInAsm >= s_FirstVerWhereScriptingAssemblyHasProperVersionInfo)
                        ? verInfoInAsm
                        : s_LastVerWhereScriptingAssemblyDoesNotHaveProperVersionInfo;

                    // There **are** scripts that have references to version-less (0.0.0.0) SHVDN and versioned one
                    // in the *same* assembly, such as "Animation Viewer" by Guadmaz. Therefore, we need to avoid
                    // naively calling `Dictionary.Add` without checking if the key already exists.
                    UpdateDictIfValueDoesNotExistOnKeyOrValueIsOlder(resolvedApis, targetVerToUseForDict);
                }
                else
                {
                    Match nameMatch = s_ScriptingApiModuleNamePatternWithVersionCapture.Match(asmName.Name);
                    if (nameMatch.Success)
                    {
                        int capturedMajorVer = int.Parse(nameMatch.Groups["ver"].Value);
                        missingApiVersionList.Add(capturedMajorVer);
                    }
                }
            }

            missingApiVersions = missingApiVersionList.ToArray();

            static void UpdateDictIfValueDoesNotExistOnKeyOrValueIsOlder(Dictionary<int, Version> dict,
                Version newCandidate)
            {
                int majorVer = newCandidate.Major;
                if (!dict.TryGetValue(majorVer, out Version currentLatestVer) || newCandidate > currentLatestVer)
                {
                    dict[majorVer] = newCandidate;
                }
            }
        }

        private static void LogScriptAssemblyLoadingFailureUnlessItWasDueToFailureOfApiAsmResolution(
            ReflectionTypeLoadException ex, string fileName)
        {
            var fileNotFoundException = ex.LoaderExceptions[0] as FileNotFoundException;
            if (fileNotFoundException == null)
            {
                Log.Message(Log.Level.Error,
                    "Failed to load script assembly ", Path.GetFileName(fileName), ": ", ex.ToString());
            }
            // Filter out failure if unable to resolve SHVDN API, since this was already logged in `HandleResolve`
            else if (!fileNotFoundException.Message.StartsWith("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase))
            {
                Log.Message(Log.Level.Error, "Failed to load script assembly ", Path.GetFileName(fileName),
                    " when searching for script types because there is an assembly that the script tried to load as " +
                    "a dependency but couldn't. Exception message: ", ex.Message.ToString(), Environment.NewLine,
                    "First LoaderException message (which tells what assembly is missing): ",
                    fileNotFoundException.ToString());
            }
        }

        // This function builds a composite key of all dependencies of a script
        private static string BuildComparisonStringForDependencyKey(Type a, string b)
        {
            b = a.FullName + "%%" + b;
            foreach (CustomAttributeData attribute in a.GetCustomAttributesData().Where(
                x => x.AttributeType.FullName == "GTA.RequireScript"))
            {
                var dependency = attribute.ConstructorArguments[0].Value as Type;
                // Ignore circular dependencies
                if (dependency != null && !b.Contains("%%" + dependency.FullName))
                {
                    b = BuildComparisonStringForDependencyKey(dependency, b);
                }
            }

            return b;
        }

        internal static ScriptInitOption GetScriptInitOptionForScriptsBuiltAgainstV2API()
            => new ScriptInitOption(true);

        internal ScriptInitOption BuildScriptInitOptionFromScriptAttribute(Type scriptType)
        {
            Version apiVersion = GetVersionOfOneOfTargetBaseTypes(scriptType, _scriptingGtaClassTypesCacheArray);
            return BuildScriptInitOptionFromScriptAttribute(scriptType, apiVersion);
        }
        internal ScriptInitOption BuildScriptInitOptionFromScriptAttribute(Type scriptType, Version apiVersion)
        {
            if (apiVersion < new Version(3, 0, 0, 0))
            {
                return GetScriptInitOptionForScriptsBuiltAgainstV2API();
            }

            bool nativeCallResetsTimeout = false;

            object nativeCallResetsTimeoutAttr
                = GetScriptAttribute(scriptType, "NativeCallResetsTimeout");
            // checking against our `AbortScriptMode` doesn't work, because the types are different even though
            // the underlying types are the same
            if (nativeCallResetsTimeoutAttr is not null)
            {
                // The cast will always be successful unless the attribute is not an enum with the underlying
                // type of `int`.
                AbortScriptMode AbortModeForNativeCallResetsTimeout = (AbortScriptMode)nativeCallResetsTimeoutAttr;
                // Let the script continue to run after calling a long-blocking native functions only if the API
                // version is 3.6.0.0 or earlier when `AbortsScriptForBlockingNativeFunc` is set to `Default`.
                // This is because the script domain didn't stop the script execution after calling a long-blocking
                // natives in SHVDN v3.6.0.0 or earlier (in short, *only for compatibility reasons*).
                if (AbortModeForNativeCallResetsTimeout == AbortScriptMode.Default
                    && (apiVersion < s_FirstApiVerWhereNativeCallDoesntResetTimeoutByDefault)
                    || AbortModeForNativeCallResetsTimeout == AbortScriptMode.On)
                {
                    nativeCallResetsTimeout = true;
                }
            }
            else
            {
                nativeCallResetsTimeout
                    = (apiVersion < s_FirstApiVerWhereNativeCallDoesntResetTimeoutByDefault) ? true : false;
            }

            return new ScriptInitOption(nativeCallResetsTimeout);
        }
        internal ScriptInitOption BuildScriptInitOptionFromScriptAttributeSafe(Type scriptType)
        {
            if (scriptType.IsAbstract || !IsSubclassOfOneOfTargetTypes(scriptType, _scriptingGtaClassTypesCacheArray))
            {
                return null;
            }
            return BuildScriptInitOptionFromScriptAttribute(scriptType);
        }

        public Script InstantiateScript(Type scriptType)
        {
            if (Thread.CurrentThread.ManagedThreadId != _executingThreadId)
            {
                // This must only be called in the main thread (since changing `_executingScript` during `DoTick` of
                // another script would break)
                return null;
            }
            ScriptInitOption option = BuildScriptInitOptionFromScriptAttributeSafe(scriptType);
            if (option == null)
            {
                option = new ScriptInitOption(false);
            }

            return InstantiateScriptFast(scriptType, option);
        }
        /// <summary>
        /// Creates an instance of a script.
        /// </summary>
        /// <param name="scriptType">The type of the script to instantiate.</param>
        /// <returns>The script instance or <see langword="null" /> in case of failure.</returns>
        internal Script InstantiateScript(Type scriptType, ScriptInitOption option)
        {
            if (Thread.CurrentThread.ManagedThreadId != _executingThreadId)
            {
                // This must only be called in the main thread (since changing `_executingScript` during `DoTick` of
                // another script would break)
                return null;
            }
            if (scriptType.IsAbstract
                || !IsSubclassOfOneOfTargetTypes(scriptType, _scriptingGtaClassTypesCacheArray))
            {
                return null;
            }

            return InstantiateScriptFast(scriptType, option);
        }
        /// <summary>
        /// Creates an instance of a script without testing if the current thread is the main script domain thread or
        /// if <paramref name="scriptType"/> is concreate and inherits `<c>GTA.Script</c>` of one of scripting API
        /// assemblies.
        /// </summary>
        /// <param name="scriptType">The type of the script to instantiate.</param>
        /// <param name="initOption">The initialize option for script instantiation.</param>
        /// <returns>The script instance or <see langword="null"/> in case of failure.</returns>
        internal Script InstantiateScriptFast(Type scriptType, ScriptInitOption initOption)
        {
            Log.Message(Log.Level.Debug, "Instantiating script ", scriptType.FullName, " ...");

            var script = new Script();
            // Keep track of current script, so it can be restored down below
            Script previousScript = null;

            lock (_lockForFieldsThatFrequentlyWritten)
            {
                previousScript = _executingScript;
                _executingScript = script;
            }

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

            script.NativeCallResetsTimeout = initOption.NativeCallResetsTimeout;

            _rwLock.EnterWriteLock();
            try
            {
                _runningScripts.Add(script);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }

            lock (_lockForFieldsThatFrequentlyWritten)
            {
                // Restore previously executing script
                _executingScript = previousScript;
            }

            return script;
        }

        /// <summary>
        /// Loads and starts all scripts.
        /// </summary>
        public void Start()
        {
            int scriptTypesCount = 0;
            int runningScriptCount = 0;

            _rwLock.EnterReadLock();
            try
            {
                scriptTypesCount = _scriptTypes.Count;
                runningScriptCount = _runningScripts.Count;
            }
            finally
            {
                _rwLock.ExitReadLock();
            }

            if (scriptTypesCount != 0 || runningScriptCount != 0)
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

            // Find candidates for instantiation.
            // Instantiate scripts after they were all loaded, so that dependencies are launched with the right ordering.
            // If the rw lock is used in the whole loop, a script will end up indirectly reading `RunningScripts`
            // where the same lock is used, causing a `LockRecursionException` without getting caught and
            // the whole process will crash.
            _rwLock.EnterReadLock();
            List<ScriptTypeInfo> scriptTypesToInstantiate = new(_scriptTypes.Count);
            try
            {
                foreach (ScriptTypeInfo scriptTypeInfo in _scriptTypes.Values)
                {
                    // Start the script unless script does not want a default instance or is abstract
                    if (scriptTypeInfo.Type.IsAbstract
                        || GetScriptAttribute(scriptTypeInfo.Type, "NoDefaultInstance") is bool noDefaultInstance
                        && noDefaultInstance)
                    {
                        continue;
                    }

                    scriptTypesToInstantiate.Add(scriptTypeInfo);
                }
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
            foreach (ScriptTypeInfo scriptTypeInfo in scriptTypesToInstantiate)
            {
                ScriptInitOption initOpt = BuildScriptInitOptionFromScriptAttribute(scriptTypeInfo.Type,
                    scriptTypeInfo.TargetApiVersion);
                Type systemTypeOfScript = scriptTypeInfo.Type;
                InstantiateScriptFast(systemTypeOfScript, initOpt)?
                    .Start(!(GetScriptAttribute(systemTypeOfScript, "NoScriptThread") is bool NoScriptThread)
                            || !NoScriptThread);
            }

            void WarnOfScriptsUsingDeprecatedApi()
            {
                if (ShouldWarnOfScriptsBuiltAgainstDeprecatedApiWithTicker)
                {
                    int scriptCountUsingDeprecatedApi = DeprecatedScriptAssemblyNamesPerApiVersion.Values.Aggregate(0, (result, current) => result + current.Count);

                    PostTickerToFeed(
                        message: $"~s~INFO: ~b~{scriptCountUsingDeprecatedApi.ToString()}~s~ scripts are running but are using the v2 API (ScriptHookVDotNet2.dll), which ~o~is deprecated and not actively supported~s~. It may stop being supported in the future, disabling to run the scripts. You could contact the author(s) or find alternatives to avoid the issue. See the console outputs or the log file for more details.",
                        isImportant: true,
                        cacheMessage: false
                    );
                }

                foreach (KeyValuePair<int, List<string>> apiVersionAndScriptNameDict in DeprecatedScriptAssemblyNamesPerApiVersion)
                {
                    int apiVersion = apiVersionAndScriptNameDict.Key;
                    int scriptAssemblyCount = apiVersionAndScriptNameDict.Value.Count;

                    string apiVersionString = apiVersion != 0 ? $"{apiVersion.ToString()}.x" : "0.x or 1.x (fallbacked to the v2 API)";

                    Log.Message(Log.Level.Info, $"Found {scriptAssemblyCount.ToString()} script(s) resolved to the deprecated API version {apiVersionString} (ScriptHookVDotNet2.dll), though the script(s) are currently running. The v2 API is deprecated and no longer actively supported. It may stop being supported in the future. You could report to the authors who developed some of the scripts that are using the deprecated API, or find alternative scripts to avoid the issue. The list of script names:");
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

            // Find candidates for instantiation. Instantiate only those scripts that are from the this assembly.
            // If the rw lock is used in the whole loop, a script will end up indirectly reading `RunningScripts`
            // where the same lock is used, causing a `LockRecursionException` without getting caught and
            // the whole process will crash.
            List<Type> scriptTypesToInstantiate = new();
            _rwLock.EnterWriteLock();
            try
            {
                foreach (Type type in _scriptTypes.Values.Where(x => x.AssemblyInfo.FileName == filename).Select(x => x.Type))
                {
                    // Make sure there are no others instances of this script
                    Func<Script, bool> filterToRemove = (x => x.Filename == filename && x.ScriptInstance.GetType() == type);
                    foreach (Script scr in _runningScripts.Where(filterToRemove))
                    {
                        scr.Dispose();
                    }
                    _runningScripts.RemoveAll(new Predicate<Script>(filterToRemove));

                    // Start the script unless script does not want a default instance
                    if (GetScriptAttribute(type, "NoDefaultInstance") is bool NoDefaultInstance && NoDefaultInstance)
                    {
                        continue;
                    }

                    scriptTypesToInstantiate.Add(type);
                }
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }

            foreach (Type type in scriptTypesToInstantiate)
            {
                InstantiateScript(type)?.Start(!(GetScriptAttribute(type, "NoScriptThread") is bool NoScriptThread) || !NoScriptThread);
            }
        }
        /// <summary>
        /// Aborts all running scripts.
        /// </summary>
        public void Abort()
        {
            _rwLock.EnterWriteLock();
            try
            {
                foreach (Script script in _runningScripts)
                {
                    script.Abort();
                }
                foreach (Script scr in _runningScripts)
                {
                    scr.Dispose();
                }

                _scriptTypes.Clear();
                _runningScripts.Clear();
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }
        /// <summary>
        /// Aborts all running scripts from the specified file.
        /// </summary>
        /// <param name="filename"></param>
        public void AbortScripts(string filename)
        {
            filename = Path.GetFullPath(filename);

            // we are only interested in the running script list as a lock target in this method
            _rwLock.EnterReadLock();
            try
            {
                foreach (Script script in _runningScripts.Where(x => filename.Equals(x.Filename, StringComparison.OrdinalIgnoreCase)))
                {
                    script.Abort();
                }
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        private bool ResetTimeoutStopwatchOfExecutingScriptIfScriptWantsToResetWhenCallingANativeFunc()
        {
            lock (_lockForFieldsThatFrequentlyWritten)
            {
                if (_executingScript == null)
                {
                    return false;
                }
                if (_executingScript.NativeCallResetsTimeout)
                {
                    _executingScript.StopwatchForTimeout.Reset();
                    return true;
                }

                return false;
            }
        }
        private void ResetTimeoutStopwatchOfExecutingScript()
        {
            lock (_lockForFieldsThatFrequentlyWritten)
            {
                if (_executingScript == null)
                {
                    return;
                }

                _executingScript.StopwatchForTimeout.Reset();
            }
        }
        private void StartTimeoutStopwatchOfExecutingScript()
        {
            lock (_lockForFieldsThatFrequentlyWritten)
            {
                if (_executingScript == null)
                {
                    return;
                }

                _executingScript.StopwatchForTimeout.Start();
            }
        }

        /// <summary>
        /// Execute a script task in this script domain with the tls context of the main thread of the exe.
        /// You must use this method when you call native functions or functions that may access some
        /// resources of the tls context of the main thread, such as a <c>rage::sysMemAllocator</c>.
        /// </summary>
        /// <param name="task">The task to execute.</param>
        public void ExecuteTaskWithGameThreadTlsContext(IScriptTask task, bool forceResetTimeoutStopwatch = false)
        {
            bool timeoutStopwatchHasBeenReset;
            if (forceResetTimeoutStopwatch)
            {
                ResetTimeoutStopwatchOfExecutingScript();
                timeoutStopwatchHasBeenReset = true;
            }
            else
            {
                timeoutStopwatchHasBeenReset
                    = ResetTimeoutStopwatchOfExecutingScriptIfScriptWantsToResetWhenCallingANativeFunc();
            }

            // Store the TLS variables to local variables, so we can perform the `IScriptTask` without the lock
            // and that makes sure the task won't be able to cause a `LockRecursionException` by calling this method
            // again in the task.
            // Since `delegate* unmanaged` is allowed only in unsafe context, we have to use `unsafe` keyword for
            // the entire block.
            unsafe
            {
                uint gameMainThreadIdUnmanaged;
                IntPtr tlsContextOfMainThread;
                delegate* unmanaged[Cdecl]<IntPtr> getTlsContext;
                delegate* unmanaged[Cdecl]<IntPtr, void> setTlsContext;

                #region Read TLS variables with the Lock for Them and Store to Local Variables
                _tlsVariablesLock.EnterReadLock();
                try
                {
                    gameMainThreadIdUnmanaged = _gameMainThreadIdUnmanaged;
                    tlsContextOfMainThread = _tlsContextOfMainThread;
                    getTlsContext = _getTlsContext;
                    setTlsContext = _setTlsContext;
                }
                catch
                {
                    // We should abort the method if reading the TLS variables *should* fail, so we can see the error
                    // in the log when it's predictable.
                    throw;
                }
                finally
                {
                    _tlsVariablesLock.ExitReadLock();
                }
                #endregion

                if (gameMainThreadIdUnmanaged == GetCurrentThreadId())
                {
                    // Request came from the main thread of the exe, so can just execute it right away
                    task.Run();
                }
                else
                {
                    IntPtr tlsContextOfScriptThread = getTlsContext();
                    setTlsContext(tlsContextOfMainThread);

                    try
                    {
                        task.Run();
                    }
                    finally
                    {
                        // Need to revert TLS context to the real one of the script thread
                        setTlsContext(tlsContextOfScriptThread);
                    }
                }
            }

            if (timeoutStopwatchHasBeenReset)
            {
                StartTimeoutStopwatchOfExecutingScript();
            }
        }

        /// <summary>
        /// Execute a script task in this script domain.
        /// </summary>
        /// <param name="task">The task to execute.</param>
        public void ExecuteTaskInScriptDomainThread(IScriptTask task)
        {
            // Timeout stopwatch should always be reset, as an `IScriptTask` that must be executed in the script domain
            // may take time to execute longer than the timeout threshold in poor PC environments but not in good ones.
            ResetTimeoutStopwatchOfExecutingScript();

            if (Thread.CurrentThread.ManagedThreadId == _executingThreadId)
            {
                // Request came from the script domain thread, so can just execute it right away
                task.Run();
            }
            else
            {
                _taskQueue.Enqueue(task);

                Script executingScript = null;
                lock (_lockForFieldsThatFrequentlyWritten)
                {
                    executingScript = _executingScript;
                }

                SignalAndWait(executingScript.WaitEvent, executingScript.ContinueEvent);
            }

            StartTimeoutStopwatchOfExecutingScript();
        }

        /// <summary>
        /// Determines whether the specified key is currently pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns><see langword="true" /> if the key is currently pressed; otherwise, <see langword="false" />.</returns>
        /// <remarks>
        /// Keys with an integer value greater than 255 will throw an <see cref="IndexOutOfRangeException"/>.  
        /// This includes modifier keys such as <see cref="Keys.Control" />, <see cref="Keys.Shift" />, and <see cref="Keys.Alt" />!
        /// </remarks>

        public bool IsKeyPressed(Keys key)
        {
            lock (_lockForFieldsThatFrequentlyWritten)
            {
                return _keyboardState[(int)key];
            }
        }
        /// <summary>
        /// Pauses or resumes handling of keyboard events in this script domain.
        /// </summary>
        /// <param name="pause"><see langword="true" /> to pause or <see langword="false" /> to resume</param>
        public void PauseKeyEvents(bool pause)
        {
            _rwLock.EnterWriteLock();
            try
            {
                _recordKeyboardEvents = !pause;
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Main execution logic of the script domain.
        /// </summary>
        internal void DoTick()
        {
            // Execute running scripts. Running scripts count should be read every time we execute `DoTick` on a script
            // because a script may instantiate additional script instances. Otherwise, the loop will end up skipping
            // newly instantiated scripts one tick, which is different from how this `DoTick` works in between v3.0.0
            // and v3.6.0.
            for (int i = 0; i < GetRunningScriptsCount(); i++)
            {
                // If the rw lock is used in the whole loop, a script will end up indirectly reading `RunningScripts`
                // where the same lock is used, causing a `LockRecursionException` without getting caught and
                // the whole process will crash.
                Script script = null;
                _rwLock.EnterReadLock();
                try
                {
                    script = _runningScripts[i];
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }

                // Ignore terminated scripts
                if (!script.IsRunning || script.IsPaused)
                {
                    continue;
                }

                lock (_lockForFieldsThatFrequentlyWritten)
                {
                    _executingScript = script;
                }

                script.StopwatchForTimeout.Restart();
                try
                {
                    if (script.IsUsingThread)
                    {
                        // Note: this block is the only location where something can be run in script threads in
                        // this method. Anything other than in this block will be run in the main thread of
                        // `ScriptDomain`.

                        // Resume script thread and execute any incoming tasks from it
                        SemaphoreSlim continueEvent = script.ContinueEvent;
                        SemaphoreSlim waitEvent = script.WaitEvent;
                        SignalAndWait(continueEvent, waitEvent);
                        while (_taskQueue.Count > 0)
                        {
                            if (_taskQueue.TryDequeue(out IScriptTask poppedTask))
                            {
                                poppedTask.Run();
                            }
                            SignalAndWait(continueEvent, waitEvent);
                        }
                    }
                    else
                    {
                        script.DoTick();
                    }
                    script.StopwatchForTimeout.Stop();
                }
                catch (Exception ex)
                {
                    HandleUnhandledException(script, new UnhandledExceptionEventArgs(ex, true));

                    // Stop script in case of an unhandled exception during task execution
                    script.Abort();

                    lock (_lockForFieldsThatFrequentlyWritten)
                    {
                        _executingScript = null;
                    }

                    continue;
                }

                uint elapsedTimeForTimeout = 0;
                lock (_lockForFieldsThatFrequentlyWritten)
                {
                    elapsedTimeForTimeout = _executingScript.StopwatchForTimeout.ElapsedMilliseconds;
                    _executingScript = null;
                }

                // Tolerate long execution time if a debugger is attached since some script may be debugged using breakpoints
                if (elapsedTimeForTimeout < ScriptTimeoutThreshold || IsDebuggerPresent())
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

        private int GetRunningScriptsCount()
        {
            int c = 0;
            _rwLock.EnterReadLock();
            try
            {
                c = _runningScripts.Count;
            }
            finally
            {
                _rwLock.ExitReadLock();
            }

            return c;
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
            lock (_lockForFieldsThatFrequentlyWritten)
            {
                _keyboardState[(int)e.KeyCode] = status;
            }

            bool recordKeyEvents = false;
            _rwLock.EnterReadLock();
            try
            {
                recordKeyEvents = _recordKeyboardEvents;
            }
            finally
            {
                _rwLock.ExitReadLock();
            }

            if (!recordKeyEvents)
            {
                return;
            }

            var eventInfo = new Tuple<bool, KeyEventArgs>(status, e);

            // we are only interested in the running script list as a lock target
            _rwLock.EnterReadLock();
            try
            {
                foreach (Script script in _runningScripts)
                {
                    script._keyboardEvents.Enqueue(eventInfo);
                }
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Free memory for all pinned strings.
        /// </summary>
        private void CleanupStrings()
        {
            lock (_pinnedStrListLock)
            {
                foreach (IntPtr handle in _pinnedStrings)
                {
                    Marshal.FreeCoTaskMem(handle);
                }

                _pinnedStrings.Clear();
            }
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

            lock (_pinnedStrListLock)
            {
                _pinnedStrings.Add(handle);
            }

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
            Script script = null;
            _rwLock.EnterReadLock();
            try
            {
                script = _runningScripts.FirstOrDefault(x => x.ScriptInstance == scriptInstance);
            }
            finally
            {
                _rwLock.ExitReadLock();
            }

            Script executingScript = null;
            lock (_lockForFieldsThatFrequentlyWritten)
            {
                executingScript = _executingScript;
            }

            // Otherwise return the executing script, since during constructor execution the running script list was not yet updated
            if (script != null || executingScript == null || executingScript.ScriptInstance != null)
            {
                return script;
            }

            // Handle the case where a script creates a custom instance of a script class that is not managed by SHVDN
            // These may attempt to set events, but are not allowed to do so, since SHVDN will never call them, so just return null
            if (!executingScript.Name.Contains(scriptInstance.GetType().FullName))
            {
                Log.Message(Log.Level.Warning, "A script tried to use a custom script instance of type ", scriptInstance.GetType().FullName, " that was not instantiated by ScriptHookVDotNet.");
                return null;
            }

            script = executingScript;

            return script;
        }
        public string LookupScriptFilename(Type scriptType)
        {
            _rwLock.EnterReadLock();
            try
            {
                return _scriptTypes.Values.FirstOrDefault(x => x.Type == scriptType)?.AssemblyInfo.FileName ?? string.Empty;
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
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
        private static bool IsSubclassOfOneOfTargetTypes(Type typeToTest, Type[] targetBaseTypes)
        {
            for (Type t = typeToTest.BaseType; t != null; t = t.BaseType)
            {
                foreach (Type targetBaseType in targetBaseTypes)
                {
                    if (t == targetBaseType)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool TryFindTypeByFullName(Assembly asm, string fullTypeName, out Type type)
        {
            type = null;
            foreach (Type t in asm.GetTypes())
            {
                if (t.FullName == fullTypeName)
                {
                    type = t;
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
        private static Version GetBaseTypeVersion(Type typeToTest, Type targetBaseType)
        {
            for (Type t = typeToTest.BaseType; t != null; t = t.BaseType)
            {
                if (t == targetBaseType)
                {
                    return t.Assembly.GetName().Version;
                }
            }

            return null;
        }
        private static Version GetVersionOfOneOfTargetBaseTypes(Type typeToTest, Type[] targetBaseTypes)
        {
            for (Type t = typeToTest.BaseType; t != null; t = t.BaseType)
            {
                foreach (Type targetBaseType in targetBaseTypes)
                {
                    if (t == targetBaseType)
                    {
                        return t.Assembly.GetName().Version;
                    }
                }
            }

            return null;
        }

        private static bool IsManagedAssembly(string fileName)
        {
            try
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
            if (assemblyName.Name.Equals("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase)
                && assemblyName.Version >= s_FirstApiVerThatHasSeparateApiModuleFromAsi)
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
                    return CurrentDomain._scriptingApiAsms.FirstOrDefault(x => x.GetName().Version.Major == 2);
                }

                Assembly compatibleApi = null;

                foreach (Assembly api in CurrentDomain._scriptingApiAsms)
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

            Log.Message(Log.Level.Error, $"The exception was thrown while executing the script {script.Name} from \"{script.Filename}\".");

            if (GetScriptAttribute(script.ScriptInstance.GetType(), "SupportURL") is string supportURL)
            {
                Log.Message(Log.Level.Error, "Please check the following site for support on the issue: ", supportURL);
            }

            // Show a notification with the script crash information
            ScriptDomain domain = ScriptDomain.CurrentDomain;
            if (domain == null)
            {
                return;
            }

            lock (domain._lockForFieldsThatFrequentlyWritten)
            {
                if (domain._executingScript != null)
                {
                    PostTickerToFeed(
                        message: "~r~Unhandled exception~s~ in script \"~h~" + script.Name + "~h~\"!~n~~n~~r~"
                            + args.ExceptionObject.GetType().Name + "~s~ "
                            + ((Exception)args.ExceptionObject).StackTrace.Split('\n').FirstOrDefault().Trim(),
                        isImportant: true,
                        cacheMessage: false
                    );
                }
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
