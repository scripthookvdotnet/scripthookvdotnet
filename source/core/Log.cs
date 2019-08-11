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

		static string FilePath => Path.ChangeExtension(typeof(ScriptDomain).Assembly.Location, ".log");

		public static void Clear()
		{
			try { File.WriteAllText(FilePath, string.Empty); } catch { }
		}

		public static void Message(Level level, params string[] message)
		{
			WriteToFile(level, message);
			WriteToConsole(level, message);
		}

		static void WriteToFile(Level level, params string[] message)
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
							sw.Write(str);
						sw.WriteLine();
					}
				}
			}
			catch (Exception ex)
			{
				WriteToConsole(Level.Error, "Failed to write to log file: ", ex.ToString());
			}
		}
		static void WriteToConsole(Level level, params string[] message)
		{
			Console console = AppDomain.CurrentDomain.GetData("Console") as Console;
			if (console == null)
				return;

			switch (level)
			{
				case Level.Info:
					console.PrintInfo(string.Join(string.Empty, message));
					break;
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
