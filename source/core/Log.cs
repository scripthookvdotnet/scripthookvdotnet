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
using System.Reflection;

namespace SHVDN
{
	public static class Log
	{
		public enum Level
		{
			Info,
			Error,
			Warning
		}

		static string FilePath => Path.ChangeExtension(typeof(ScriptDomain).Assembly.Location, ".log");

		public static void Clear()
		{
			try { File.WriteAllText(FilePath, string.Empty); } catch { }
		}

		public static void Message(Level level, params string[] message)
		{
			WriteToConsole(level, message);
			try { WriteToFile(level, message); } catch { }
		}

		static void WriteToFile(Level level, params string[] message)
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
					}

					foreach (string str in message)
						sw.Write(str);
					sw.WriteLine();
				}
			}
		}
		static void WriteToConsole(Level level, params string[] message)
		{
			// The console lives in the default application domain, so always call into that
			var DefaultDomain = (AppDomain)AppDomain.CurrentDomain.GetData("DefaultDomain");
			if (DefaultDomain != null)
			{
				DefaultDomain.SetData("WriteToConsole.level", level);
				DefaultDomain.SetData("WriteToConsole.message", message);
				DefaultDomain.DoCallBack(() =>
				{
					var level2 = (Level)AppDomain.CurrentDomain.GetData("WriteToConsole.level");
					var message2 = (string[])AppDomain.CurrentDomain.GetData("WriteToConsole.message");
					WriteToConsole(level2, message2);
				});
				return;
			}

			switch (level)
			{
				case Level.Info:
					SHVDN.Console.PrintInfo(string.Join(string.Empty, message));
					break;
				case Level.Error:
					SHVDN.Console.PrintError(string.Join(string.Empty, message));
					break;
				case Level.Warning:
					SHVDN.Console.PrintWarning(string.Join(string.Empty, message));
					break;
			}
		}
	}
}
