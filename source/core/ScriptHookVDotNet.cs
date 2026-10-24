//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;

namespace SHVDN
{
    public static class ScriptHookVDotNet
    {
        private static Console s_console = null;

        // We use this domain to prevent from the keyboard thread reading stale values, and to protect against race
        // condition during reload. Do note that static variables are not shared between `AppDomain`s.
        private static object s_variablesLockForMainDomain = new();
        private static object s_variablesLockForScriptDomain = new();

        // Although static variables are not shared between `AppDomain`s, we need to use a lock because the keyboard
        // message thread may want to request the script domain to instant reload.
        public static Console Console
        {
            get => LockHelper.ReadLocked<Console>(ref s_console, ref s_variablesLockForMainDomain);
            private set => LockHelper.WriteLocked<Console>(ref s_console, value, ref s_variablesLockForMainDomain);
        }

        public static ScriptDomain Domain { get; private set; }
            = ScriptDomain.CurrentDomain;

        public static void Init(IntPtr getTlsContextFunc, IntPtr setTlsContextFunc, IntPtr gameThreadTlsContext, uint gameThreadId)
        {
            ScriptDomain domain = null;
            Console console = null;
            List<string> stashedConsoleCommandHistory = new();

            // Unload previous domain (this unloads all script assemblies too)
            lock (s_variablesLockForMainDomain)
            {
                domain = Domain;

                if (domain != null)
                {
                    console = (Console)domain.AppDomain.GetData("Console");

                    // Stash the command history if console is loaded
                    if (console != null)
                    {
                        stashedConsoleCommandHistory = console.CommandHistory;
                        Console = null;
                    }

                    ScriptDomain.Unload(domain);
                    Domain = null;
                }
                else
                {
                    console = Console;
                }
            }

            // Clear log from previous runs
            Log.Clear();

            // Load configuration
            Ini.Load(out List<LogMessageInfo> pendingLogMessageInfo);

            // Create a separate script domain
            domain = ScriptDomain.Load(".", Ini.ScriptPath);
            if (domain == null)
            {
                return;
            }

            lock (s_variablesLockForMainDomain)
            {
                Domain = domain;
            }

            domain.ScriptTimeoutThreshold = Ini.ScriptTimeoutThreshold;

            // Set functions for Thread Local Storage (TLS), so scripts can do tasks that need variables in the TLS of the main thread in their script thread
            domain.InitTlsStuffForTlsContextSwitch(getTlsContextFunc, setTlsContextFunc, gameThreadTlsContext, gameThreadId);

            try
            {
                // Initialize and scan memory at a predictable point
                domain.InitNativeMemoryMembers();
            }
            catch (Exception ex)
            {
                Log.Message(Log.Level.Error, "Failed to initialize native memory members: ", ex.ToString());
            }

            try
            {
                // Instantiate console inside script domain, so that it can access the scripting API
                console = (Console)domain.AppDomain.CreateInstanceFromAndUnwrap(typeof(Console).Assembly.Location, typeof(Console).FullName);

                // Restore the console command history (set a empty history for the first time)
                console.CommandHistory = stashedConsoleCommandHistory;

                // Print welcome message
                var version = typeof(ScriptHookVDotNet).Assembly.GetName().Version;
                console.Print(Log.Level.Info, $"~c~--- Community Script Hook V .NET {version} ---");
                console.Print(Log.Level.Info, "~c~--- Type \"Help()\" to print an overview of available commands ---");

                SendPendingMessagesToConsole(console, pendingLogMessageInfo);

                // Update console pointer in script domain
                domain.AppDomain.SetData("Console", console);
                domain.AppDomain.DoCallBack(new CrossAppDomainDelegate(AssignConsoleFromDomain));

                // Add default console commands
                console.RegisterCommands(typeof(InbuildCommands));
            }
            catch (Exception ex)
            {
                Log.Message(Log.Level.Error, "Failed to create console: ", ex.ToString());
            }

            Console = console;

            if (Ini.AutoLoadScripts)
            {
                // Start scripts in the newly created domain
                domain.Start();
            }
            else
            {
                Log.Message(Log.Level.Debug, "AutoLoadScripts is set to false, skipping auto loading scripts. " +
                    "You can start scripts with specific console commands such as `StartAllScripts()`.");
            }
        }

        public static void Tick()
        {
            lock (s_variablesLockForMainDomain)
            {
                Console?.DoTick();
                Domain?.DoTick();
            }
        }

        public static void KeyboardMessage(ulong keycode, bool keydown, bool ctrl, bool shift, bool alt)
        {
            // Filter out invalid key codes
            if (keycode <= 0 || keycode >= 256)
            {
                return;
            }

            // Protect against race condition during reload.
            // Also prevent from the keyboard thread reading stale values such as key bindings.
            lock (s_variablesLockForMainDomain)
            {
                ScriptDomain scriptDomain = Domain;

                // If the TLS stuff is not initialized, the console or some script can call a native function without swapping
                // TLS address, leading the whole process to crash
                if (scriptDomain == null || !scriptDomain.IsTlsStuffInitialized())
                {
                    return;
                }

                Keyboard.OnMessage(keycode, keydown, ctrl, shift, alt);
            }
        }

        public static void AssignConsoleFromDomain()
        {
            lock (s_variablesLockForScriptDomain)
            {
                Console = (Console)AppDomain.CurrentDomain.GetData("Console");
            }
        }

        internal static void AppendPendingMessageForConsole(List<LogMessageInfo> messageInfoBuffer, Log.Level level, string input)
        {
            messageInfoBuffer.Add(new LogMessageInfo(level, input));
        }

        private static void SendPendingMessagesToConsole(Console console, List<LogMessageInfo> pendingMessageInfo)
        {
            if (console == null)
            {
                return;
            }

            foreach (LogMessageInfo messageInfo in pendingMessageInfo)
            {
                // Don't use Log.WriteToConsole here, we want to make sure that log strings will print via
                // the passed console instance
                switch (messageInfo.Level)
                {
                    case Log.Level.Error:
                        console.Print(Log.Level.Error, messageInfo.Message);
                        break;
                    case Log.Level.Warning:
                        console.Print(Log.Level.Warning, messageInfo.Message);
                        break;
                }
            }
        }
    }
}
