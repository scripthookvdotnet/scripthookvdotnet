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
		#region ScriptHookV Imports
		/// <summary>
		/// Initializes the stack for a new script function call.
		/// </summary>
		/// <param name="hash">The function hash to call.</param>
		[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativeInit@@YAX_K@Z")]
		static extern void NativeInit(ulong hash);

		/// <summary>
		/// Pushes a function argument on the script function stack.
		/// </summary>
		/// <param name="val">The argument value.</param>
		[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativePush64@@YAX_K@Z")]
		static extern void NativePush64(ulong val);

		/// <summary>
		/// Executes the script function call.
		/// </summary>
		/// <returns>A pointer to the return value of the call.</returns>
		[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativeCall@@YAPEA_KXZ")]
		static unsafe extern ulong* NativeCall();
		#endregion

		/// <summary>
		/// Internal script task which holds all data necessary for a script function call.
		/// </summary>
		class NativeTask : IScriptTask
		{
			internal ulong Hash;
			internal ulong[] Arguments;
			internal unsafe ulong* Result;

			public void Run()
			{
				Result = InvokeInternal(Hash, Arguments);
			}
		}

		/// <summary>
		/// Internal script task which holds all data necessary for a script function call.
		/// </summary>
		class NativeTaskPtrArgs : IScriptTask
		{
			internal ulong Hash;
			internal ulong* ArgumentPtr;
			internal int ArgumentCount;
			internal unsafe ulong* Result;

			public void Run()
			{
				Result = InvokeInternal(Hash, ArgumentPtr, ArgumentCount);
			}
		}

		/// <summary>
		/// Pushes a single string component on the text stack.
		/// </summary>
		/// <param name="str">The string to push.</param>
		static void PushString(string str)
		{
			var domain = SHVDN.ScriptDomain.CurrentDomain;
			if (domain == null)
			{
				throw new InvalidOperationException("Illegal scripting call outside script domain.");
			}

			IntPtr strUtf8 = domain.PinString(str);

			var strArg = (ulong)strUtf8.ToInt64();
			domain.ExecuteTask(new NativeTaskPtrArgs {
				Hash = 0x6C188BE134E074AA /*ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME*/,
				ArgumentPtr = &strArg,
				ArgumentCount = 1
			});
		}

		/// <summary>
		/// Splits up a spring into manageable components and pushes them on the text stack.
		/// </summary>
		/// <param name="str">The string to split up.</param>
		public static void PushLongString(string str)
		{
			PushLongString(str, PushString);
		}
		/// <summary>
		/// Splits up a string into manageable components and performs an <paramref name="action"/> on them.
		/// </summary>
		/// <param name="str">The string to split up.</param>
		/// <param name="action">The action to perform on the component.</param>
		public static void PushLongString(string str, Action<string> action)
		{
			const int maxLengthUtf8 = 99;

			if (str == null || Encoding.UTF8.GetByteCount(str) <= maxLengthUtf8)
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

				// Calculate the UTF-8 code point size of the current character
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

					startPos = currentPos;
					currentUtf8StrLength = 0;
				}
				else
				{
					currentPos++;
					currentUtf8StrLength += codePointSize;
				}

				// Additional increment is needed for surrogate
				if (codePointSize == 4)
				{
					currentPos++;
				}
			}

			if (startPos == 0)
				action(str);
			else
				action(str.Substring(startPos, str.Length - startPos));
		}

		/// <summary>
		/// Helper function that converts an array of primitive values to a native stack.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		static ulong[] ConvertPrimitiveArguments(object[] args)
		{
			var result = new ulong[args.Length];
			for (int i = 0; i < args.Length; ++i)
			{
				if (args[i] is bool valueBool)
				{
					result[i] = valueBool ? 1ul : 0ul;
					continue;
				}
				if (args[i] is byte valueByte)
				{
					result[i] = (ulong)valueByte;
					continue;
				}
				if (args[i] is int valueInt32)
				{
					result[i] = (ulong)valueInt32;
					continue;
				}
				if (args[i] is ulong valueUInt64)
				{
					result[i] = valueUInt64;
					continue;
				}
				if (args[i] is float valueFloat)
				{
					result[i] = *(ulong*)&valueFloat;
					continue;
				}
				if (args[i] is IntPtr valueIntPtr)
				{
					result[i] = (ulong)valueIntPtr.ToInt64();
					continue;
				}
				if (args[i] is string valueString)
				{
					result[i] = (ulong)ScriptDomain.CurrentDomain.PinString(valueString).ToInt64();
					continue;
				}

				throw new ArgumentException("Unknown primitive type in native argument list", nameof(args));
			}

			return result;
		}

		/// <summary>
		/// Executes a script function inside the current script domain.
		/// </summary>
		/// <param name="hash">The function has to call.</param>
		/// <param name="argPtr">A pointer of function arguments.</param>
		/// <param name="argCount">The length of <paramref name="argPtr" />.</param>
		/// <returns>A pointer to the return value of the call.</returns>
		public static ulong* Invoke(ulong hash, ulong* argPtr, int argCount)
		{
			var domain = ScriptDomain.CurrentDomain;
			if (domain == null)
			{
				throw new InvalidOperationException("Illegal scripting call outside script domain.");
			}

			var task = new NativeTaskPtrArgs { Hash = hash, ArgumentPtr = argPtr, ArgumentCount = argCount };
			domain.ExecuteTask(task);

			return task.Result;
		}
		/// <summary>
		/// Executes a script function inside the current script domain.
		/// </summary>
		/// <param name="hash">The function has to call.</param>
		/// <param name="args">A list of function arguments.</param>
		/// <returns>A pointer to the return value of the call.</returns>
		public static ulong* Invoke(ulong hash, params ulong[] args)
		{
			var domain = ScriptDomain.CurrentDomain;
			if (domain == null)
			{
				throw new InvalidOperationException("Illegal scripting call outside script domain.");
			}

			var task = new NativeTask { Hash = hash, Arguments = args };
			domain.ExecuteTask(task);

			return task.Result;
		}
		public static ulong* Invoke(ulong hash, params object[] args)
		{
			return Invoke(hash, ConvertPrimitiveArguments(args));
		}

		/// <summary>
		/// Executes a script function immediately. This may only be called from the main script domain thread.
		/// </summary>
		/// <param name="hash">The function has to call.</param>
		/// <param name="argPtr">A pointer of function arguments.</param>
		/// <param name="argCount">The length of <paramref name="argPtr" />.</param>
		/// <returns>A pointer to the return value of the call.</returns>
		public static ulong* InvokeInternal(ulong hash, ulong* argPtr, int argCount)
		{
			NativeInit(hash);
			for (int i = 0; i < argCount; i++)
				NativePush64(argPtr[i]);
			return NativeCall();
		}
		/// <summary>
		/// Executes a script function immediately. This may only be called from the main script domain thread.
		/// </summary>
		/// <param name="hash">The function has to call.</param>
		/// <param name="args">A list of function arguments.</param>
		/// <returns>A pointer to the return value of the call.</returns>
		public static ulong* InvokeInternal(ulong hash, params ulong[] args)
		{
			NativeInit(hash);
			foreach (var arg in args)
				NativePush64(arg);
			return NativeCall();
		}
		public static ulong* InvokeInternal(ulong hash, params object[] args)
		{
			return InvokeInternal(hash, ConvertPrimitiveArguments(args));
		}
	}
}
