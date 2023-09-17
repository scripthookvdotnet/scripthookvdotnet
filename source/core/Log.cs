//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.IO;

namespace SHVDN
{
	public static class Log
	{
		public enum Level
		{
			Error,
			Warning,
			Info,
			Debug,
		}

		private static string FilePath => Path.ChangeExtension(typeof(ScriptDomain).Assembly.Location, ".log");

		internal static string FileName => Path.GetFileName(FilePath);

		public static void Clear()
		{
			try
			{
				File.WriteAllText(FilePath, string.Empty);
			}
			catch
			{
				// Ignore exceptions
			}
		}

		public static void Message(Level level, params string[] message)
		{
			WriteToFile(level, message);
			WriteToConsole(level, message);
		}

		internal static void WriteToFile(Level level, params string[] message)
		{
			try
			{
				using (var fs = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read))
				{
					using (var sw = new StreamWriter(fs))
					{
						sw.Write(string.Concat("[", DateTime.Now.ToString("HH:mm:ss"), "] "));

						switch (level)
						{
							case Level.Info:
								sw.Write("[INFO] ");
								break;
							case Level.Error:
								sw.Write("[ERROR] ");
								break;
							case Level.Warning:
								sw.Write("[WARNING] ");
								break;
							case Level.Debug:
								sw.Write("[DEBUG] ");
								break;
						}

						foreach (string str in message)
						{
							sw.Write(str);
						}

						sw.WriteLine();
					}
				}
			}
			catch (Exception ex)
			{
				WriteToConsole(Level.Error, "Failed to write to log file: ", ex.ToString());
			}
		}

		internal static void WriteToConsole(Level level, params string[] message)
		{
			var console = AppDomain.CurrentDomain.GetData("Console") as Console;

			if (console == null)
			{
				return;
			}

			switch (level)
			{
				case Level.Error:
					console.PrintError(string.Join(string.Empty, message));
					break;
				case Level.Warning:
					console.PrintWarning(string.Join(string.Empty, message));
					break;
			}
		}
	}
}
