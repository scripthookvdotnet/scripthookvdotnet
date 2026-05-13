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
                int codePointSize = GetUtf8CodePointSize(str, currentPos);

                if (currentUtf8StrLength + codePointSize > maxLengthUtf8)
                {
                    int splitPos = FindTokenSafeSplitPosition(str, startPos, currentPos);
                    if (splitPos <= startPos)
                    {
                        splitPos = currentPos;
                    }

                    action(str.Substring(startPos, splitPos - startPos));

                    startPos = splitPos;
                    currentPos = splitPos;
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

        private static int GetUtf8CodePointSize(string str, int index)
        {
            char chr = str[index];
            if (char.IsHighSurrogate(chr) && index < str.Length - 1 && char.IsLowSurrogate(str[index + 1]))
            {
                return 4;
            }

            if (chr < 0x80)
            {
                return 1;
            }

            return chr < 0x800 ? 2 : 3;
        }

        private static int FindTokenSafeSplitPosition(string str, int startPos, int preferredSplitPos)
        {
            int splitPos = preferredSplitPos;
            while (splitPos > startPos && IsInsideRockstarFormatToken(str, splitPos))
            {
                splitPos--;
            }

            return splitPos;
        }

        private static bool IsInsideRockstarFormatToken(string str, int index)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!IsRockstarFormatTokenAt(str, i, out int tokenEnd))
                {
                    continue;
                }

                if (index > i && index <= tokenEnd)
                {
                    return true;
                }

                i = tokenEnd;
            }

            return false;
        }

        private static bool IsRockstarFormatTokenAt(string str, int index, out int tokenEnd)
        {
            tokenEnd = -1;
            if (index < 0 || index >= str.Length || str[index] != '~' || IsEscapedTilde(str, index))
            {
                return false;
            }

            for (int i = index + 1; i < str.Length; i++)
            {
                if (str[i] == '~' && !IsEscapedTilde(str, i))
                {
                    tokenEnd = i;
                    return true;
                }
            }

            return false;
        }

        private static bool IsEscapedTilde(string str, int index)
        {
            int backslashCount = 0;
            for (int i = index - 1; i >= 0 && str[i] == '\\'; i--)
            {
                backslashCount++;
            }

            return (backslashCount & 1) != 0;
        }

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
        /// <param name="argPtr">A pointer of function arguments.</param>
        /// <param name="argCount">The length of <paramref name="argPtr" />.</param>
        /// <returns>A pointer to the return value of the call.</returns>
        public static ulong* InvokeLongBlockingFunc(ulong hash, ulong* argPtr, int argCount)
        {
            ScriptDomain domain = ScriptDomain.CurrentDomain;
            if (domain == null)
            {
                ThrowInvalidOperationException_IllegalScriptingCall();
                return null;
            }

            var task = new NativeTaskPtrArgs { _hash = hash, _argumentPtr = argPtr, _argumentCount = argCount };
            domain.ExecuteTaskWithGameThreadTlsContext(task, true);

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
