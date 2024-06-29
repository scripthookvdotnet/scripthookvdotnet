//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Runtime.InteropServices;
using System.Security;
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
        [SuppressUnmanagedCodeSecurity]
        [DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativeInit@@YAX_K@Z")]
        private static extern void NativeInit(ulong hash);

        /// <summary>
        /// Pushes a function argument on the script function stack.
        /// </summary>
        /// <param name="val">The argument value.</param>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativePush64@@YAX_K@Z")]
        private static extern void NativePush64(ulong val);

        /// <summary>
        /// Executes the script function call.
        /// </summary>
        /// <returns>A pointer to the return value of the call.</returns>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativeCall@@YAPEA_KXZ")]
        private static extern ulong* NativeCall();
        #endregion

        /// <summary>
        /// Internal script task which holds all data necessary for a script function call.
        /// </summary>
        private class NativeTask : IScriptTask
        {
            internal ulong _hash;
            internal ulong[] _arguments;
            internal ulong* _result;

            public void Run()
            {
                _result = InvokeInternal(_hash, _arguments);
            }
        }

        /// <summary>
        /// Internal script task which holds all data necessary for a script function call.
        /// </summary>
        private class NativeTaskPtrArgs : IScriptTask
        {
            internal ulong _hash;
            internal ulong* _argumentPtr;
            internal int _argumentCount;
            internal ulong* _result;

            public void Run()
            {
                _result = InvokeInternal(_hash, _argumentPtr, _argumentCount);
            }
        }

        /// <summary>
        /// Pushes a single string component on the text stack.
        /// </summary>
        /// <param name="str">The string to push.</param>
        private static void PushString(string str)
        {
            ScriptDomain domain = SHVDN.ScriptDomain.CurrentDomain;
            if (domain == null)
            {
                ThrowInvalidOperationException_IllegalScriptingCall();
                return;
            }

            IntPtr strUtf8 = domain.PinString(str);

            ulong strArg = (ulong)strUtf8.ToInt64();
            domain.ExecuteTaskWithGameThreadTlsContext(new NativeTaskPtrArgs
            {
                _hash = 0x6C188BE134E074AA /* ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME */,
                _argumentPtr = &strArg,
                _argumentCount = 1
            });
        }

        /// <summary>
        /// Splits up a string into manageable components and adds them as text components to the current text command.
        /// This requires that a text command that accepts multiple text components is active (e.g. "CELL_EMAIL_BCON").
        /// </summary>
        /// <param name="str">The string to split up.</param>
        /// <param name="maxLengthUtf8">The max byte length per chunk in UTF-8.</param>
        public static void PushLongString(string str, int maxLengthUtf8 = 99)
        {
            PushLongString(str, PushString, maxLengthUtf8);
        }
        /// <summary>
        /// Splits up a string into manageable components and adds them as text components to the current text command.
        /// This requires that a text command that accepts multiple text components is active (e.g. "CELL_EMAIL_BCON").
        /// </summary>
        /// <param name="str">The string to split up.</param>
        /// <param name="startPos">The start pos.</param>
        /// <param name="count">The length to slice string.</param>
        /// <param name="maxLengthUtf8">The max byte length per chunk in UTF-8.</param>
        public static void PushLongString(string str, int startPos, int count, int maxLengthUtf8 = 99)
        {
            PushLongString(str, PushString, maxLengthUtf8);
        }
        /// <summary>
        /// Splits up a string into manageable components and performs an <paramref name="action"/> on them.
        /// </summary>
        /// <param name="str">The string to split up.</param>
        /// <param name="action">The action to perform on the component.</param>
        /// <param name="maxLengthUtf8">The max byte length per chunk in UTF-8.</param>
        public static void PushLongString(string str, Action<string> action, int maxLengthUtf8 = 99)
        {
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
                char chr = str[currentPos];
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
                    const int lowSurrogateStart = 0xD800;
                    const int highSurrogateStart = 0xD800;

                    int temp1 = (int)chr - highSurrogateStart;
                    if (temp1 >= 0 && temp1 <= 0x7ff)
                    {
                        // Found a high surrogate
                        if (currentPos < str.Length - 1)
                        {
                            int temp2 = str[currentPos + 1] - lowSurrogateStart;
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
            {
                action(str);
            }
            else
            {
                action(str.Substring(startPos, str.Length - startPos));
            }
        }
        /// <summary>
        /// Splits up a string into manageable components and performs an <paramref name="action"/> on them.
        /// </summary>
        /// <param name="str">The string to split up.</param>
        /// <param name="action">The action to perform on the component.</param>
        /// <param name="startPos">The start pos.</param>
        /// <param name="count">The length to slice string.</param>
        /// <param name="maxLengthUtf8">The max byte length per chunk in UTF-8.</param>
        public static void PushLongString(string str, Action<string> action, int startPos, int count, int maxLengthUtf8 = 99)
        {
            if (str == null || GetByteCountUTF8(str, startPos, count) <= maxLengthUtf8)
            {
                action(str);
                return;
            }

            int currentUtf8StrLength = 0;
            int currentIndex = startPos;
            int endPos = startPos + count;
            int startPosForStringSlice = startPos;

            while (currentIndex < endPos)
            {
                int codePointSize = 0;

                // Calculate the UTF-8 code point size of the current character
                char chr = str[currentIndex];
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
                    const int lowSurrogateStart = 0xD800;
                    const int highSurrogateStart = 0xD800;

                    int temp1 = (int)chr - highSurrogateStart;
                    if (temp1 >= 0 && temp1 <= 0x7ff)
                    {
                        // Found a high surrogate
                        if (currentIndex < str.Length - 1)
                        {
                            int temp2 = str[currentIndex + 1] - lowSurrogateStart;
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
                    action(str.Substring(startPosForStringSlice, currentIndex - startPosForStringSlice));

                    startPosForStringSlice = currentIndex;
                    currentUtf8StrLength = 0;
                }
                else
                {
                    currentIndex++;
                    currentUtf8StrLength += codePointSize;
                }

                // Additional increment is needed for surrogate
                if (codePointSize == 4)
                {
                    currentIndex++;
                }
            }

            if (startPos == 0)
            {
                action(str);
            }
            else
            {
                action(str.Substring(startPosForStringSlice, str.Length - startPosForStringSlice));
            }
        }

        internal static int GetByteCountUTF8(string s, int index, int count)
        {
            ThrowIfNull(s);
            ThrowIfNegative(index);
            ThrowIfNegative(count);
            ThrowIfGreaterThan(index, s.Length - count);

            unsafe
            {
                fixed (char* pChar = s)
                {
                    return Encoding.UTF8.GetByteCount(pChar + index, count);
                }
            }
        }

        public static void ThrowIfGreaterThan<T>(T value, T other, string paramName = null) where T : IComparable<T>
        {
            if (value.CompareTo(other) > 0)
                ThrowGreater(value, other, paramName);
        }

        public static void ThrowIfNegative(int value, string paramName = null)
        {
            if (value < 0)
                ThrowNegative(value, paramName);
        }

        public static void ThrowIfNull(object argument, string paramName = null)
        {
            if (argument is null)
            {
                ThrowArgNull(paramName);
            }
        }

        private static void ThrowGreater<T>(T value, T other, string paramName) =>
            throw new ArgumentOutOfRangeException(paramName, value, $"'{value}' must be less than or equal to '{other}'.");

        private static void ThrowNegative<T>(T value, string paramName) =>
            throw new ArgumentOutOfRangeException(paramName, value, $"'{value}' must be a non-negative and non-zero value.");

        internal static void ThrowArgNull(string paramName) => throw new ArgumentNullException(paramName);

        /// <summary>
        /// Helper function that converts an array of primitive values to a native stack.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static ulong[] ConvertPrimitiveArguments(object[] args)
        {
            ulong[] result = new ulong[args.Length];
            for (int i = 0; i < args.Length; ++i)
            {
                switch (args[i])
                {
                    case bool valueBool:
                        result[i] = valueBool ? 1ul : 0ul;
                        continue;
                    case byte valueByte:
                        result[i] = (ulong)valueByte;
                        continue;
                    case int valueInt32:
                        result[i] = (ulong)valueInt32;
                        continue;
                    case ulong valueUInt64:
                        result[i] = valueUInt64;
                        continue;
                    case float valueFloat:
                        result[i] = *(ulong*)&valueFloat;
                        continue;
                    case IntPtr valueIntPtr:
                        result[i] = (ulong)valueIntPtr.ToInt64();
                        continue;
                    case string valueString:
                        result[i] = (ulong)ScriptDomain.CurrentDomain.PinString(valueString).ToInt64();
                        continue;
                    default:
                        throw new ArgumentException("Unknown primitive type in native argument list", nameof(args));
                }
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
            ScriptDomain domain = ScriptDomain.CurrentDomain;
            if (domain == null)
            {
                ThrowInvalidOperationException_IllegalScriptingCall();
                return null;
            }

            var task = new NativeTaskPtrArgs { _hash = hash, _argumentPtr = argPtr, _argumentCount = argCount };
            domain.ExecuteTaskWithGameThreadTlsContext(task);

            return task._result;
        }
        /// <summary>
        /// Executes a script function inside the current script domain.
        /// </summary>
        /// <param name="hash">The function has to call.</param>
        /// <param name="args">A list of function arguments.</param>
        /// <returns>A pointer to the return value of the call.</returns>
        public static ulong* Invoke(ulong hash, params ulong[] args)
        {
            ScriptDomain domain = ScriptDomain.CurrentDomain;
            if (domain == null)
            {
                ThrowInvalidOperationException_IllegalScriptingCall();
                return null;
            }

            var task = new NativeTask { _hash = hash, _arguments = args };
            domain.ExecuteTaskWithGameThreadTlsContext(task);

            return task._result;
        }
        public static ulong* Invoke(ulong hash, params object[] args)
        {
            return Invoke(hash, ConvertPrimitiveArguments(args));
        }

        private static void ThrowInvalidOperationException_IllegalScriptingCall()
        {
            throw new InvalidOperationException("Illegal scripting call outside script domain.");
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
            {
                NativePush64(argPtr[i]);
            }

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
            foreach (ulong arg in args)
            {
                NativePush64(arg);
            }

            return NativeCall();
        }
        public static ulong* InvokeInternal(ulong hash, params object[] args)
        {
            return InvokeInternal(hash, ConvertPrimitiveArguments(args));
        }
    }
}
