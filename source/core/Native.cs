/**
 * Copyright (C) 2015 crosire
 *
 * This software is  provided 'as-is', without any express  or implied  warranty. In no event will the
 * authors be held liable for any damages arising from the use of this software.
 * Permission  is granted  to anyone  to use  this software  for  any  purpose,  including  commercial
 * applications, and to alter it and redistribute it freely, subject to the following restrictions:
 *
 *   1. The origin of this software must not be misrepresented; you must not claim that you  wrote the
 *      original  software. If you use this  software  in a product, an  acknowledgment in the product
 *      documentation would be appreciated but is not required.
 *   2. Altered source versions must  be plainly  marked as such, and  must not be  misrepresented  as
 *      being the original software.
 *   3. This notice may not be removed or altered from any source distribution.
 */

using System;
using System.Runtime.InteropServices;
using ScriptHookWrapper;

namespace GTA
{
    namespace Native
    {
        public interface INativeValue
        {
            ulong NativeValue { get; set; }
        }

        #region Functions
        public class InputArgument
        {
            internal ulong _data;

            public InputArgument(object value)
            {
                _data = Function.ObjectToNative(value);
            }

            public override string ToString()
            {
                return _data.ToString();
            }

            // Value types
            public static implicit operator InputArgument(bool value)
            {
                return new InputArgument(value);
            }
            public static implicit operator InputArgument(byte value)
            {
                return new InputArgument((int)(value));
            }
            public static implicit operator InputArgument(sbyte value)
            {
                return new InputArgument((int)(value));
            }
            public static implicit operator InputArgument(short value)
            {
                return new InputArgument((int)(value));
            }
            public static implicit operator InputArgument(ushort value)
            {
                return new InputArgument((int)(value));
            }
            public static implicit operator InputArgument(int value)
            {
                return new InputArgument(value);
            }
            public static implicit operator InputArgument(uint value)
            {
                return new InputArgument(value);
            }
            public static implicit operator InputArgument(long value)
            {
                return new InputArgument(value);
            }
            public static implicit operator InputArgument(ulong value)
            {
                return new InputArgument(value);
            }
            public static implicit operator InputArgument(float value)
            {
                return new InputArgument(value);
            }
            public static implicit operator InputArgument(double value)
            {
                return new InputArgument((float)(value));
            }
            public static implicit operator InputArgument(Enum value)
            {
                return new InputArgument(value);
            }

            //INativeValue types (Unfortunately, implicit interface conversion isn't allowed in C#)
            public static implicit operator InputArgument(Model value)
            {
                return new InputArgument(value.Hash);
            }
            public static implicit operator InputArgument(RelationshipGroup value)
            {
                return new InputArgument(value.Hash);
            }
            public static implicit operator InputArgument(WeaponAsset value)
            {
                return new InputArgument(value.Hash);
            }
            //PoolObject types
            public static implicit operator InputArgument(Blip value)
            {
                return new InputArgument(value.Handle);
            }
            public static implicit operator InputArgument(Camera value)
            {
                return new InputArgument(value.Handle);
            }
            public static implicit operator InputArgument(CheckPoint value)
            {
                return new InputArgument(value.Handle);
            }
            public static implicit operator InputArgument(Entity value)
            {
                return new InputArgument(value.Handle);
            }
            public static implicit operator InputArgument(PedGroup value)
            {
                return new InputArgument(value.Handle);
            }
            public static implicit operator InputArgument(Pickup value)
            {
                return new InputArgument(value.Handle);
            }
            public static implicit operator InputArgument(Player value)
            {
                return new InputArgument(value.Handle);
            }
            public static implicit operator InputArgument(Rope value)
            {
                return new InputArgument(value.Handle);
            }
            public static implicit operator InputArgument(Scaleform value)
            {
                return new InputArgument(value.Handle);
            }

            // String types
            public static implicit operator InputArgument(string value)
            {
                return new InputArgument(value);
            }
            public static unsafe implicit operator InputArgument(char* value)
            {
                return new InputArgument(new string(value));
            }

            // Pointer types
            public static implicit operator InputArgument(IntPtr value)
            {
                return new InputArgument(value);
            }
            public static unsafe implicit operator InputArgument(void* value)
            {
                return new InputArgument(new IntPtr(value));
            }

            public static implicit operator InputArgument(OutputArgument value)
            {
                return new InputArgument(value._storage);
            }
        }

        public class OutputArgument
        {
            internal IntPtr _storage = Marshal.AllocCoTaskMem(24);
            private bool disposed;

            /// <summary>
            /// Initializes a new instance of the <see cref="OutputArgument"/> class for natives that output data into pointers.
            /// </summary>
            public OutputArgument()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="OutputArgument"/> class with an initial value for natives that require the pointer to data instead of the actual data.
            /// </summary>
            /// <param name="initvalue">The value to set the data of this <see cref="OutputArgument"/> to.</param>
            public OutputArgument(object value) : this()
            {
                unsafe
                {
                    *(ulong*)(_storage) = Function.ObjectToNative(value);
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            protected virtual void Dispose(bool disposing)
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                    }
                    Marshal.FreeCoTaskMem(_storage);
                    disposed = true;
                }
            }
            ~OutputArgument()
            {
                Dispose(false);
            }

            /// <summary>
            /// Gets the value of data stored in this <see cref="OutputArgument"/>.
            /// </summary>
            public T GetResult<T>()
            {
                unsafe
                {
                    return (T)(Function.ObjectFromNative(typeof(T), (ulong*)(_storage.ToPointer())));
                }
            }
        }

        public sealed class Function
        {
            internal unsafe struct NativeTask : IScriptTask
            {
                internal ulong _hash;
                internal ulong* _result;
                internal InputArgument[] _arguments;

                public void Run()
                {
					Wrapper.NativeInit(_hash);

                    foreach (var argument in _arguments)
                    {
						Wrapper.NativePush64(argument._data);
                    }

					_result = Wrapper.NativeCall();
				}
            }

            /// <summary>
            /// Calls the specified native script function and returns its return value.
            /// </summary>
            /// <param name="hash">The hashed name of the native script function.</param>
            /// <param name="arguments">A list of input and output arguments to pass to the native script function.</param>
            /// <returns>The return value of the native</returns>
            public static T Call<T>(Hash hash, params InputArgument[] arguments)
            {
                return Call<T>((ulong)hash, arguments);
            }
            /// <summary>
            /// Calls the specified native script function and ignores its return value.
            /// </summary>
            /// <param name="hash">The hashed name of the script function.</param>
            /// <param name="arguments">A list of input and output arguments to pass to the native script function.</param>
            public static void Call(Hash hash, params InputArgument[] arguments)
            {
                Call<int>((ulong)hash, arguments);
            }

            internal static T Call<T>(ulong hash, params InputArgument[] arguments)
            {
                var task = new NativeTask();
                task._hash = hash;
                task._arguments = arguments;

                ScriptDomain.CurrentDomain.ExecuteTask(task);

                unsafe { return (T)(ObjectFromNative(typeof(T), task._result)); }
            }

            internal static ulong ObjectToNative(object value)
            {
                if (ReferenceEquals(value, null))
                {
                    return 0;
                }

                var type = value.GetType();

                if (type.IsEnum)
                {
                    value = Convert.ChangeType(value, type = Enum.GetUnderlyingType(type));
                }

                if (type == typeof(bool))
                {
                    return (ulong)(((bool)value) ? 1 : 0);
                }
                if (type == typeof(int))
                {
                    return (ulong)((int)value);
                }
                if (type == typeof(uint))
                {
                    return (uint)value;
                }
                if (type == typeof(long))
                {
                    return (ulong)((long)value);
                }
                if (type == typeof(ulong))
                {
                    return (ulong)value;
                }
                if (type == typeof(float))
                {
                    return BitConverter.ToUInt32(BitConverter.GetBytes((float)value), 0);
                }
                if (type == typeof(Double))
                {
                    return BitConverter.ToUInt32(BitConverter.GetBytes((float)((double)value)), 0);
                }

                if (type == typeof(String))
                {
                    return (ulong)ScriptDomain.CurrentDomain.PinString((string)(value)).ToInt64();
                }

                if (type == typeof(IntPtr))
                {
                    return (ulong)((IntPtr)value).ToInt64();
                }

                if (typeof(INativeValue).IsAssignableFrom(type))
                {
                    return ((INativeValue)value).NativeValue;
                }

                throw new InvalidCastException(String.Concat("Unable to cast object of type '", type.FullName, "' to native value"));
            }
            internal static unsafe object ObjectFromNative(Type type, ulong* value)
            {
                if (type.IsEnum)
                {
                    type = Enum.GetUnderlyingType(type);
                }

                if (type == typeof(bool))
                {
                    unsafe
                    {
                        return *(int*)(value) != 0;
                    }
                }
                if (type == typeof(int))
                {
                    unsafe
                    {
                        return *(int*)(value);
                    }

                }
                if (type == typeof(uint))
                {
                    unsafe
                    {
                        return *(uint*)(value);
                    }

                }
                if (type == typeof(long))
                {
                    unsafe
                    {
                        return *(long*)(value);
                    }

                }
                if (type == typeof(ulong))
                {
                    unsafe
                    {
                        return *(value);
                    }

                }
                if (type == typeof(float))
                {
                    unsafe
                    {
                        return *(float*)(value);
                    }

                }
                if (type == typeof(double))
                {
                    unsafe
                    {
                        return (double)(*(float*)(value));
                    }
   
                }

                if (type == typeof(string))
                {
                    var address = (char*)(value);

                    if (address == null)
                    {
                        return String.Empty;
                    }

                    return StringFromNativeUtf8(new IntPtr(address));
                }

                if (type == typeof(IntPtr))
                {
                    unsafe
                    {
                        return new IntPtr((long)(value));
                    }
                }

                if (type == typeof(Math.Vector2))
                {
                    unsafe
                    {
                        var data = (float*)(value);

                        return new Math.Vector2(data[0], data[2]);
                    }
                }
                if (type == typeof(Math.Vector3))
                {
                    unsafe
                    {
                        var data = (float*)(value);

                        return new Math.Vector3(data[0], data[2], data[4]);
                    }
                }

                if (typeof(INativeValue).IsAssignableFrom(type))
                {
                    unsafe
                    {
                        // Warning: Requires classes implementing 'INativeValue' to repeat all constructor work in the setter of 'NativeValue'
                        var result = (INativeValue)(System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type));
                        result.NativeValue = *value;

                        return result;
                    }

                }

                throw new InvalidCastException(String.Concat("Unable to cast native value to object of type '", type.FullName, "'"));
            }

            internal static string StringFromNativeUtf8(IntPtr nativeUtf8)
            {
                unsafe
                {
                    byte* address = (byte*)nativeUtf8.ToPointer();
                    int len = 0;

                    while (*(address + len) != 0)
                        ++len;

                    if (len == 0)
                        return string.Empty;

                    byte[] buffer = new byte[len];
                    Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);
                    return System.Text.Encoding.UTF8.GetString(buffer);
                }
            }
        }
        #endregion

        #region Global Variables
        public struct GlobalVariable
        {
            private IntPtr _address;

            private GlobalVariable(IntPtr address) : this()
            {
                _address = address;
            }

            /// <summary>
            /// Gets the global variable at the specified index.
            /// </summary>
            /// <param name="index">The index of the global variable.</param>
            /// <returns>A <see cref="GlobalVariable"/> instance representing the global variable.</returns>
            public static GlobalVariable Get(int index)
            {
				IntPtr address = Wrapper.GetGlobalPtr(index);

			    if (address == IntPtr.Zero)
			    {
			    	throw new IndexOutOfRangeException(String.Format("The index {0} does not correspond to an existing global variable.", index));
			    }

			    return new GlobalVariable(address);
		    }
            /// <summary>
            /// Gets the native memory address of the <see cref="GlobalVariable"/>.
            /// </summary>
            public IntPtr MemoryAddress
            {
                get
                {
                    return _address;
                }
            }

            /// <summary>
            /// Gets the value stored in the <see cref="GlobalVariable"/>.
            /// </summary>
            public T Read<T>()
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)Function.StringFromNativeUtf8(_address);
                }

                unsafe
                {
                    return (T)(Function.ObjectFromNative(typeof(T), (ulong*)(_address.ToPointer())));
                }
            }
            /// <summary>
            /// Set the value stored in the <see cref="GlobalVariable"/>.
            /// </summary>
            /// <param name="value">The new value to assign to the <see cref="GlobalVariable"/>.</param>
            public void Write<T>(T value)
            {
                if (typeof(T) == typeof(string))
                {
                    throw new InvalidOperationException("Cannot write string values via 'Write<string>', use 'WriteString' instead.");
                }

                if (typeof(T) == typeof(Math.Vector2))
                {
                    var val = (Math.Vector2)(object)value;
                    unsafe
                    {
                        var data = (float*)(_address.ToPointer());

                        data[0] = val.X;
                        data[2] = val.Y;
                        return;
                    }
                }
                if (typeof(T) == typeof(Math.Vector3))
                {
                    var val = (Math.Vector3)(object)(value);
                    unsafe
                    {
                        var data = (float*)(_address.ToPointer());

                        data[0] = val.X;
                        data[2] = val.Y;
                        data[4] = val.Z;
                        return;
                    }
                }
                unsafe
                {
                    *(ulong*)(_address.ToPointer()) = Function.ObjectToNative(value);
                }
            }

            /// <summary>
            /// Set the value stored in the <see cref="GlobalVariable"/> to a string.
            /// </summary>
            /// <param name="value">The string to set the <see cref="GlobalVariable"/> to.</param>
            /// <param name="maxSize">The maximum size of the string. Can be found for a given global variable by checking the decompiled scripts from the game.</param>
            public void WriteString(string value, int maxSize)
            {
                if (maxSize % 8 != 0 || maxSize <= 0 || maxSize > 64)
                {
                    throw new ArgumentException("The string maximum size should be one of 8, 16, 24, 32 or 64.", "maxSize");
                }

                var size = System.Text.Encoding.UTF8.GetByteCount(value);

                if (size >= maxSize)
                {
                    size = maxSize - 1;
                }

                Marshal.Copy(System.Text.Encoding.UTF8.GetBytes(value), 0, _address, size);
                unsafe { ((char*)(_address.ToPointer()))[size] = '\0'; }
            }
            /// <summary>
            /// Set the value of a specific bit of the <see cref="GlobalVariable"/> to true.
            /// </summary>
            /// <param name="index">The zero indexed bit of the <see cref="GlobalVariable"/> to set.</param>
            public void SetBit(int index)
            {
                if (index < 0 || index > 63)
                {
                    throw new IndexOutOfRangeException("The bit index has to be between 0 and 63");
                }

                unsafe { *(ulong*)(_address.ToPointer()) |= (1u << index); }
            }
            /// <summary>
            /// Set the value of a specific bit of the <see cref="GlobalVariable"/> to false.
            /// </summary>
            /// <param name="index">The zero indexed bit of the <see cref="GlobalVariable"/> to clear.</param>
            public void ClearBit(int index)
            {
                if (index < 0 || index > 63)
                {
                    throw new IndexOutOfRangeException("The bit index has to be between 0 and 63");
                }

               unsafe { *(ulong*)(_address.ToPointer()) &= ~(1u << index); }
            }
            /// <summary>
            /// Gets a value indicating whether a specific bit of the <see cref="GlobalVariable"/> is set.
            /// </summary>
            /// <param name="index">The zero indexed bit of the <see cref="GlobalVariable"/> to check.</param>
            public bool IsBitSet(int index)
            {
                if (index < 0 || index > 63)
                {
                    throw new IndexOutOfRangeException("The bit index has to be between 0 and 63");
                }

                unsafe { return ((*(ulong*)(_address.ToPointer()) >> index) & 1) != 0; }
            }

            /// <summary>
            /// Gets the <see cref="GlobalVariable"/> stored at a given offset in a global structure.
            /// </summary>
            /// <param name="index">The index the <see cref="GlobalVariable"/> is stored in the structure. For example the Y component of a Vector3 is at index 1.</param>
            /// <returns>The <see cref="GlobalVariable"/> at the index given.</returns>
            public GlobalVariable GetStructField(int index)
            {
                if (index < 0)
                {
                    throw new IndexOutOfRangeException("The structure item index cannot be negative.");
                }

                return new GlobalVariable(MemoryAddress + (8 * index));
            }

            /// <summary>
            /// Returns an array of all <see cref="GlobalVariable"/>s in a global array.
            /// </summary>
            /// <param name="itemSize">The number of items stored in each array index. For example an array of Vector3s takes up 3 items.</param>
            /// <returns>The array of <see cref="GlobalVariable"/>s.</returns>
            public GlobalVariable[] GetArray(int itemSize)
            {
                if (itemSize <= 0)
                {
                    throw new ArgumentOutOfRangeException("itemSize", "The item size for an array must be positive.");
                }

                int count = Read<int>();

                // Globals are stored in pages that hold a maximum of 65536 items
                if (count < 1 || count >= 65536 / itemSize)
                {
                    throw new InvalidOperationException("The variable does not seem to be an array.");
                }
            
                var result = new GlobalVariable[count];

                for (int i = 0; i < count; i++)
                {
                    result[i] = new GlobalVariable(MemoryAddress + 8 + (8 * itemSize * i));
                }

                return result;
            }
            /// <summary>
            /// Gets the <see cref="GlobalVariable"/> stored at a specific index in a global array.
            /// </summary>
            /// <param name="index">The array index.</param>
            /// <param name="itemSize">The number of items stored in each array index. For example an array of Vector3s takes up 3 items.</param>
            /// <returns>The <see cref="GlobalVariable"/> at the index given.</returns>
            public GlobalVariable GetArrayItem(int index, int itemSize)
            {
                if (itemSize <= 0)
                {
                    throw new ArgumentOutOfRangeException("itemSize", "The item size for an array must be positive.");
                }

                int count = Read<int>();

                // Globals are stored in pages that hold a maximum of 65536 items
                if (count < 1 || count >= 65536 / itemSize)
                {
                    throw new InvalidOperationException("The variable does not seem to be an array.");
                }

                if (index < 0 || index >= count)
                {
                    throw new IndexOutOfRangeException(String.Format("The index {0} was outside the array bounds.", index));
                }

                return new GlobalVariable(MemoryAddress + 8 + (8 * itemSize * index));
            }
        }
        #endregion
    }
}
