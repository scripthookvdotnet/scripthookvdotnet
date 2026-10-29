//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.IO;

namespace SHVDN
{
    // Console expressions inherit these static commands for unqualified calls such as Help().
    public abstract class InbuildCommands
    {
        [ConsoleCommand("Print the default help")]
        public static void Help()
        {
            if (!TryGetConsole(nameof(Help), out Console console))
            {
                return;
            }

            console.Print(Log.Level.Info, "~c~--- Help ---");
            console.Print(Log.Level.Info, "The console accepts ~h~C# expressions~h~ as input and has full access to the scripting API. To print the result of an expression, simply add \"return\" in front of it.");
            console.Print(Log.Level.Info, "You can use \"P\" as a shortcut for the player character and \"V\" for the current vehicle (without the quotes).");
            console.Print(Log.Level.Info, "Example: \"return P.IsInVehicle()\" will print a boolean value indicating whether the player is currently sitting in a vehicle to the console.");
            console.Print(Log.Level.Info, "~c~--- Commands ---");
            console.PrintHelpText();
        }

        [ConsoleCommand("Print the help for a specific command")]
        public static void Help(string command)
        {
            if (!TryGetConsole(nameof(Help), out Console console))
            {
                return;
            }

            console.PrintHelpText(command);
        }

        [ConsoleCommand("Clear the console history and pages")]
        public static void Clear()
        {
            if (!TryGetConsole(nameof(Clear), out Console console))
            {
                return;
            }

            console.Clear();
        }

        [ConsoleCommand("Reload all scripts from the scripts directory")]
        public static void Reload()
        {
            if (!TryGetConsole(nameof(Reload), out Console console))
            {
                return;
            }

            console.Print(Log.Level.Info, "~y~Reloading ...");

            // Force a reload on next tick
            SharedVariables.DidRequestReloadResetFlag = true;
        }

        [ConsoleCommand("Load scripts from a file")]
        public static void Start(string filename)
        {
            if (!TryGetConsole(nameof(Start), out Console console))
            {
                return;
            }

            ScriptDomain domain = ScriptHookVDotNet.Domain;

            if (!Path.IsPathRooted(filename))
            {
                filename = Path.Combine(domain.ScriptPath, filename);
            }

            if (!Path.HasExtension(filename))
            {
                filename += ".dll";
            }

            string ext = Path.GetExtension(filename).ToLower();
            if (!File.Exists(filename) || (ext != ".cs" && ext != ".vb" && ext != ".dll"))
            {
                console.Print(Log.Level.Error, Path.GetFileName(filename) + " is not a script file!");
                return;
            }

            domain.StartScripts(filename);
        }

        [ConsoleCommand("Load all scripts in the scripts folder")]
        public static void StartAllScripts()
        {
            if (!TryGetConsole(nameof(StartAllScripts), out Console console))
            {
                return;
            }

            ScriptDomain domain = ScriptHookVDotNet.Domain;

            console.Print(Log.Level.Info, "~y~Loading all scripts ...");

            domain.Start();
        }

        [ConsoleCommand("Abort all scripts from a file")]
        public static void Abort(string filename)
        {
            if (!TryGetConsole(nameof(Abort), out Console console))
            {
                return;
            }

            ScriptDomain domain = ScriptHookVDotNet.Domain;

            if (!Path.IsPathRooted(filename))
            {
                filename = Path.Combine(domain.ScriptPath, filename);
            }

            if (!Path.HasExtension(filename))
            {
                filename += ".dll";
            }

            string ext = Path.GetExtension(filename).ToLower();
            if (!File.Exists(filename) || (ext != ".cs" && ext != ".vb" && ext != ".dll"))
            {
                console.Print(Log.Level.Error, Path.GetFileName(filename) + " is not a script file!");
                return;
            }

            domain.AbortScripts(filename);
        }

        [ConsoleCommand("Abort all scripts currently running")]
        public static void AbortAll()
        {
            if (!TryGetConsole(nameof(AbortAll), out Console console))
            {
                return;
            }

            ScriptHookVDotNet.Domain.Abort();

            console.Print(Log.Level.Info, "Stopped all running scripts. Use \"Start(filename)\" to start them again.");
        }

        [ConsoleCommand("List all loaded scripts")]
        public static void ListScripts()
        {
            if (!TryGetConsole(nameof(ListScripts), out Console console))
            {
                return;
            }

            console.Print(Log.Level.Info, "~c~--- Loaded Scripts ---");

            foreach (var script in ScriptHookVDotNet.Domain.RunningScripts)
            {
                console.Print(Log.Level.Info, Path.GetFileName(script.Filename) + " ~h~" + script.Name + (script.IsRunning ? (script.IsPaused ? " ~o~[paused]" : " ~g~[running]") : " ~r~[aborted]"));
            }
        }

        private static bool TryGetConsole(string commandName, out Console console)
        {
            console = ScriptHookVDotNet.Console;

            if(console != null)
            {
                return true;
            }

            Log.WriteToFile(Log.Level.Error,
                string.Format(
                    "Could not execute the console command \"{0}\". The console is not loaded. " +
                    "You could report this error to the ScriptHookVDotNet's GitHub repository as the error can be " +
                    "responsible to ScriptHookVDotNet's implementation.",
                    commandName
                )
            );

            return false;
        }

    }
}
