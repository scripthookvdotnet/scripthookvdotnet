//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SHVDN
{
    internal static class Ini
    {

        private static Keys[] s_reloadKeyBinding = { Keys.None };
        private static Keys[] s_consoleKeyBinding = { Keys.F4 };

        // We can't make this readonly as otherwise our LockHelper does not work
        private static object s_lock = new();

        public static Keys[] ReloadKeyBinding => LockHelper.ReadLocked<Keys[]>(ref s_reloadKeyBinding, ref s_lock);
        public static Keys[] ConsoleKeyBinding => LockHelper.ReadLocked<Keys[]>(ref s_consoleKeyBinding, ref s_lock);


        // We don't need to use a lock here for these, as they iare only used during initialization.
        public static bool AutoLoadScripts { get; private set; }
            = true;

        public static string ScriptPath { get; private set; }
            = "scripts";

        public static uint ScriptTimeoutThreshold { get; private set; }
            = 5000;

        public static void Load(out List<LogMessageInfo> pendingLogMessageInfo)
        {
            try
            {
                lock (s_lock)
                {
                    pendingLogMessageInfo = LoadInternal();
                }
            }
            catch (Exception ex)
            {
                Log.Message(Log.Level.Error, "Failed to load config: ", ex.ToString());
                pendingLogMessageInfo = new List<LogMessageInfo>();
            }
        }

        private static List<LogMessageInfo> LoadInternal()
        {
            // This code is protected via try-catch and inside a lock when called from `Load()`.

            List<LogMessageInfo> pendingLogMessageInfo = new();

            string[] config = File.ReadAllLines(Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".ini"));

            foreach (string rawLine in config)
            {
                // Perform some very basic key/value parsing
                string line = rawLine.Trim();

                if (line.StartsWith(";") || line.StartsWith("#") || line.StartsWith("//"))
                {
                    continue;
                }

                string[] data = line.Split('=');
                if (data.Length != 2)
                {
                    continue;
                }

                // May fail to parse without trimming whitespaces
                string keyStr = data[0].Trim();
                string valueStr = data[1].Trim();

                ValueTuple<Keys[], InvalidKeysFoundError> keyCombinationResult;

                if (string.Equals(keyStr, "ReloadKeyBinding", StringComparison.OrdinalIgnoreCase))
                {
                    keyCombinationResult = ParseKeyBinding(valueStr);
                    InvalidKeysFoundError invalidKeysError = keyCombinationResult.Item2;
                    if (invalidKeysError != null)
                    {
                        LogKeyBindingParseError(valueStr, invalidKeysError, pendingLogMessageInfo, s_reloadKeyBinding);
                    }
                    else
                    {
                        s_reloadKeyBinding = keyCombinationResult.Item1;
                    }

                    continue;
                }
                if (string.Equals(keyStr, "ConsoleKeyBinding", StringComparison.OrdinalIgnoreCase))
                {
                    keyCombinationResult = ParseKeyBinding(valueStr);
                    InvalidKeysFoundError invalidKeysError = keyCombinationResult.Item2;

                    if (invalidKeysError != null)
                    {
                        LogKeyBindingParseError(valueStr, invalidKeysError, pendingLogMessageInfo, s_consoleKeyBinding);
                    }
                    else
                    {
                        s_consoleKeyBinding = keyCombinationResult.Item1;
                    }

                    continue;
                }

                if (string.Equals(keyStr, "ScriptTimeoutThreshold", StringComparison.OrdinalIgnoreCase))
                {
                    if (uint.TryParse(valueStr, out uint outVal))
                    {
                        ScriptTimeoutThreshold = outVal;
                    }

                    continue;
                }
                if (string.Equals(keyStr, "ScriptsLocation", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptPath = valueStr.Trim('"');

                    continue;
                }

                if (string.Equals(keyStr, "AutoLoadScripts", StringComparison.OrdinalIgnoreCase))
                {
                    if (bool.TryParse(valueStr, out bool outVal))
                    {
                        AutoLoadScripts = outVal;
                    }

                    continue;
                }
            }

            return pendingLogMessageInfo;
        }

        private static ValueTuple<Keys[], InvalidKeysFoundError> ParseKeyBinding(string input)
        {
            InvalidKeysFoundErrorBuilder errorBuilder = null;

            string[] inputSubstrings = input.Split('+');

            List<Keys> resultKeyBinding = new(inputSubstrings.Length);

            for (int i = 0; i < inputSubstrings.Length; i++)
            {
                string keyStr = inputSubstrings[i].Trim();

                if (Enum.TryParse(keyStr, true, out Keys parsedKey))
                {
                    resultKeyBinding.Add(parsedKey);
                }
                else
                {
                    if (errorBuilder == null)
                    {
                        errorBuilder = new InvalidKeysFoundErrorBuilder();
                    }

                    errorBuilder.Add(i, keyStr);
                }
            }

            InvalidKeysFoundError error = errorBuilder?.Build();

            return new ValueTuple<Keys[], InvalidKeysFoundError>(resultKeyBinding.ToArray(), error);
        }

        private static void LogKeyBindingParseError(string rawInput, InvalidKeysFoundError invalidKeysFoundError, List<LogMessageInfo> messageInfoBuffer, Keys[] defaultKeyBinding)
        {
            ScriptHookVDotNet.AppendPendingMessageForConsole(messageInfoBuffer,
                Log.Level.Error,
                $"InvalidKeysFoundError: Failed to parse a key binding from the string \"{rawInput}\". See {Log.FileName} for details."
            );

            StringBuilder errorLineBuilder = new();
            bool hasAlreadyFoundFirstUnparsedString = false;

            foreach (var unparsedInfo in invalidKeysFoundError.GetUnparsedStrSequence())
            {
                if (hasAlreadyFoundFirstUnparsedString)
                {
                    errorLineBuilder.Append(Environment.NewLine);
                }
                else
                {
                    hasAlreadyFoundFirstUnparsedString = true;
                }
                errorLineBuilder.Append(
                    string.Format("    Found an invalid key name \"{0}\" at index {1}", unparsedInfo.UnparsedString, unparsedInfo.Position)
                );
            }

            Log.WriteToFile(Log.Level.Error,
                string.Format(
                    "InvalidKeysFoundError: Failed to parse a key binding from the string \"{0}\":{1}{2}{3}{4}{5}{6}",
                    rawInput,
                    Environment.NewLine,
                    Environment.NewLine,
                    errorLineBuilder.ToString(),
                    Environment.NewLine,
                    Environment.NewLine,
                    "    Fallbacked to \"" + BuildKeyCombinationString(defaultKeyBinding) + "\""
                )
            );
        }

        internal static string BuildKeyCombinationString(Keys[] keyBinding)
        {
            if (keyBinding == null)
            {
                throw new ArgumentNullException(nameof(keyBinding));
            }

            if (keyBinding.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder strBuilder = new();
            bool hasAlreadyFoundFirstElement = false;

            foreach (Keys key in keyBinding)
            {
                if (hasAlreadyFoundFirstElement)
                {
                    strBuilder.Append(" + ");
                }
                else
                {
                    hasAlreadyFoundFirstElement = true;
                }

                strBuilder.Append(typeof(Keys).GetEnumName(key));
            }

            return strBuilder.ToString();
        }
    }
}
