//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SHVDN
{
	/// <summary>
	/// Class responsible for executing script functions.
	/// </summary>
	public static unsafe class NativeFunc
	{
		internal class NativeTask : IScriptTask
		{
			[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativeInit@@YAX_K@Z")]
			static extern void NativeInit(ulong hash);
			[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativePush64@@YAX_K@Z")]
			static extern void NativePush64(ulong val);
			[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativeCall@@YAPEA_KXZ")]
			static unsafe extern ulong* NativeCall();

			internal ulong Hash;
			internal ulong[] Arguments;
			internal ulong* Result;

			public void Run()
			{
				NativeInit(Hash);
				foreach (var arg in Arguments)
					NativePush64(arg);
				Result = NativeCall();
			}
		}

		static void PushString(string str)
		{
			var domain = SHVDN.ScriptDomain.CurrentDomain;
			if (domain == null)
			{
				throw new InvalidOperationException("Illegal scripting call outside script domain.");
			}

			IntPtr strUtf8 = domain.PinString(str);

			domain.ExecuteTask(new NativeTask {
				Hash = 0x6C188BE134E074AAul /*_ADD_TEXT_COMPONENT_STRING*/,
				Arguments = new ulong[] { (ulong)strUtf8.ToInt64() }
			});
		}
		public static void PushLongString(string str)
		{
			PushLongString(str, PushString);
		}
		public static void PushLongString(string str, Action<string> action)
		{
			const int maxLengthUtf8 = 99;

			int size = Encoding.UTF8.GetByteCount(str);
			if (size <= maxLengthUtf8)
			{
				action(str);
				return;
			}

			int startPos = 0;
			int currentPos = 0;
			int currentUtf8StrLength = 0;

			while (currentPos < str.Length)
			{
				int codePointSize = 0;

				var chr = str[currentPos];
				if (chr < 0x80)
				{
					codePointSize = 1;
				}
				else if (chr < 0x800)
				{
					codePointSize = 2;
				}
				else if (chr < 0x10000)
				{
					codePointSize = 3;
				}
				else
				{
					#region Surrogate check
					const int LowSurrogateStart = 0xD800;
					const int HighSurrogateStart = 0xD800;

					var temp1 = (int)chr - HighSurrogateStart;
					if (temp1 >= 0 && temp1 <= 0x7ff)
					{
						// Found a high surrogate
						if (currentPos < str.Length - 1)
						{
							var temp2 = str[currentPos + 1] - LowSurrogateStart;
							if (temp2 >= 0 && temp2 <= 0x3ff)
							{
								// Found a low surrogate
								codePointSize = 4;
							}
						}
					}
					#endregion
				}

				if (currentUtf8StrLength + codePointSize > maxLengthUtf8)
				{
					action(str.Substring(startPos, currentPos - startPos));

					currentUtf8StrLength = 0;
					startPos = currentPos;
				}
				else
				{
					currentUtf8StrLength += codePointSize;
				}

				currentPos++;

				// Additional increment is needed for surrogate
				if (codePointSize == 4)
				{
					currentPos++;
				}
			}

			action(str.Substring(startPos, str.Length - startPos));
		}

		public static ulong* Invoke(ulong hash, params ulong[] args)
		{
			var domain = SHVDN.ScriptDomain.CurrentDomain;
			if (domain == null)
			{
				throw new InvalidOperationException("Illegal scripting call outside script domain.");
			}

			var task = new NativeTask { Hash = hash, Arguments = args };
			domain.ExecuteTask(task);

			return task.Result;
		}
		public static ulong* InvokeInternal(ulong hash, params ulong[] args)
		{
			NativeInit(hash);
			foreach (var arg in args)
				NativePush64(arg);
			return NativeCall();
		}
	}
}
