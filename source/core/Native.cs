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
using System.Text;
using System.Runtime.InteropServices;

namespace GTA
{
	namespace Native
	{
		public interface INativeValue
		{
			ulong NativeValue { get; set; }
		}

		#region Functions
		/// <summary>
		/// An input argument passed to a script function.
		/// </summary>
		public class InputArgument
		{
			internal ulong _data;

			/// <summary>
			/// Initializes a new instance of the <see cref="InputArgument"/> class and converts a managed object to a script function input argument.
			/// </summary>
			/// <param name="value">The object to convert.</param>
			public InputArgument(object value)
			{
				unsafe
				{
					_data = Function.ObjectToNative(value);
				}
			}

			/// <summary>
			/// Converts the internal value of the argument to its equivalent string representation.
			/// </summary>
			public override string ToString()
			{
				return _data.ToString();
			}

			#region Implicit Conversion Operators
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

			// INativeValue types
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
			public static implicit operator InputArgument(Blip value)
			{
				return new InputArgument(value.Handle);
			}
			public static implicit operator InputArgument(Camera value)
			{
				return new InputArgument(value.Handle);
			}
			public static implicit operator InputArgument(Checkpoint value)
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
			#endregion
		}

		/// <summary>
		/// An output argument passed to a script function.
		/// </summary>
		public class OutputArgument : IDisposable
		{
			#region Fields
			private bool _disposed = false;
			internal IntPtr _storage = IntPtr.Zero;
			#endregion

			/// <summary>
			/// Initializes a new instance of the <see cref="OutputArgument"/> class for script functions that output data into pointers.
			/// </summary>
			public OutputArgument()
			{
				_storage = Marshal.AllocCoTaskMem(24);
			}
			/// <summary>
			/// Initializes a new instance of the <see cref="OutputArgument"/> class with an initial value for script functions that require the pointer to data instead of the actual data.
			/// </summary>
			/// <param name="initvalue">The value to set the data of this <see cref="OutputArgument"/> to.</param>
			public OutputArgument(object value) : this()
			{
				unsafe
				{
					*(ulong*)(_storage) = Function.ObjectToNative(value);
				}
			}

			/// <summary>
			/// Frees the unmanaged resources associated with this <see cref="OutputArgument"/>.
			/// </summary>
			~OutputArgument()
			{
				Dispose(false);
			}

			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}
			protected virtual void Dispose(bool disposing)
			{
				if (_disposed)
					return;

				Marshal.FreeCoTaskMem(_storage);

				_disposed = true;
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

		/// <summary>
		/// A static class which handles script function execution.
		/// </summary>
		public static class Function
		{
			[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativeInit@@YAX_K@Z")]
			static extern void NativeInit(ulong hash);
			[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativePush64@@YAX_K@Z")]
			static extern void NativePush64(ulong val);
			[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?nativeCall@@YAPEA_KXZ")]
			static unsafe extern ulong* NativeCall();

			/// <summary>
			/// Internal script task responsible for executing the script function.
			/// </summary>
			internal unsafe class NativeTask : IScriptTask
			{
				internal ulong Hash;
				internal ulong* Result;
				internal InputArgument[] Arguments;

				public void Run()
				{
					NativeInit(Hash);

					foreach (var argument in Arguments)
					{
						NativePush64(argument._data);
					}

					Result = NativeCall();
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
				var task = new NativeTask();
				task.Hash = (ulong)hash;
				task.Arguments = arguments;

				ScriptDomain.CurrentDomain.ExecuteTask(task);

				unsafe { return (T)(ObjectFromNative(typeof(T), task.Result)); }
			}
			/// <summary>
			/// Calls the specified native script function and ignores its return value.
			/// </summary>
			/// <param name="hash">The hashed name of the script function.</param>
			/// <param name="arguments">A list of input and output arguments to pass to the native script function.</param>
			public static void Call(Hash hash, params InputArgument[] arguments)
			{
				var task = new NativeTask();
				task.Hash = (ulong)hash;
				task.Arguments = arguments;

				ScriptDomain.CurrentDomain.ExecuteTask(task);
			}

			/// <summary>
			/// Converts a managed object to a native value.
			/// </summary>
			/// <param name="value">The object to convert.</param>
			/// <returns>A native value representing the input <paramref name="value"/>.</returns>
			internal static unsafe ulong ObjectToNative(object value)
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
				if (type == typeof(double))
				{
					return BitConverter.ToUInt32(BitConverter.GetBytes((float)((double)value)), 0);
				}

				if (type == typeof(string))
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
			/// <summary>
			/// Converts a native value to a managed object.
			/// </summary>
			/// <param name="type">The type to convert to.</param>
			/// <param name="value">The native value to convert.</param>
			/// <returns>A managed object representing the input <paramref name="value"/>.</returns>
			internal static unsafe object ObjectFromNative(Type type, ulong* value)
			{
				if (type.IsEnum)
				{
					type = Enum.GetUnderlyingType(type);
				}

				if (type == typeof(bool))
				{
					return *(int*)(value) != 0;
				}
				if (type == typeof(int))
				{
					return *(int*)(value);
				}
				if (type == typeof(uint))
				{
					return *(uint*)(value);
				}
				if (type == typeof(long))
				{
					return *(long*)(value);
				}
				if (type == typeof(ulong))
				{
					return *(value);
				}
				if (type == typeof(float))
				{
					return *(float*)(value);
				}
				if (type == typeof(double))
				{
					return (double)(*(float*)(value));
				}

				if (type == typeof(string))
				{
					var address = (char*)(*value);

					if (address == null)
					{
						return String.Empty;
					}

					return MemoryAccess.PtrToStringUTF8(new IntPtr(address));
				}

				if (type == typeof(IntPtr))
				{
					return new IntPtr((long)(value));
				}

				if (type == typeof(Math.Vector2))
				{
					var data = (float*)(value);

					return new Math.Vector2(data[0], data[2]);

				}
				if (type == typeof(Math.Vector3))
				{
					var data = (float*)(value);

					return new Math.Vector3(data[0], data[2], data[4]);

				}

				if (typeof(INativeValue).IsAssignableFrom(type))
				{
					// Warning: Requires classes implementing 'INativeValue' to repeat all constructor work in the setter of 'NativeValue'
					var result = (INativeValue)(System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type));
					result.NativeValue = *value;

					return result;
				}

				throw new InvalidCastException(String.Concat("Unable to cast native value to object of type '", type.FullName, "'"));
			}
		}
		#endregion

		#region Global Variables
		/// <summary>
		/// A value class which handles access to global script variables.
		/// </summary>
		public struct GlobalVariable
		{
			[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?getGlobalPtr@@YAPEA_KH@Z")]
			static extern IntPtr GetGlobalPtr(int index);

			/// <summary>
			/// Initializes a new instance of the <see cref="GlobalVariable"/> class with a variable address.
			/// </summary>
			/// <param name="address">The memory address of the global variable.</param>
			private GlobalVariable(IntPtr address) : this()
			{
				MemoryAddress = address;
			}

			/// <summary>
			/// Gets the global variable at the specified index.
			/// </summary>
			/// <param name="index">The index of the global variable.</param>
			/// <returns>A <see cref="GlobalVariable"/> instance representing the global variable.</returns>
			public static GlobalVariable Get(int index)
			{
				IntPtr address = GetGlobalPtr(index);

				if (address == IntPtr.Zero)
				{
					throw new IndexOutOfRangeException($"The index {index} does not correspond to an existing global variable.");
				}

				return new GlobalVariable(address);
			}

			/// <summary>
			/// Gets the native memory address of the <see cref="GlobalVariable"/>.
			/// </summary>
			public IntPtr MemoryAddress { get; private set; }

			/// <summary>
			/// Gets the value stored in the <see cref="GlobalVariable"/>.
			/// </summary>
			public unsafe T Read<T>()
			{
				if (typeof(T) == typeof(string))
				{
					return (T)(object)MemoryAccess.PtrToStringUTF8(MemoryAddress);
				}
				else
				{
					return (T)(Function.ObjectFromNative(typeof(T), (ulong*)(MemoryAddress.ToPointer())));
				}
			}

			/// <summary>
			/// Set the value stored in the <see cref="GlobalVariable"/>.
			/// </summary>
			/// <param name="value">The new value to assign to the <see cref="GlobalVariable"/>.</param>
			public unsafe void Write<T>(T value)
			{
				if (typeof(T) == typeof(string))
				{
					throw new InvalidOperationException("Cannot write string values via 'Write<string>', use 'WriteString' instead.");
				}

				if (typeof(T) == typeof(Math.Vector2))
				{
					var val = (Math.Vector2)(object)value;
					var data = (float*)(MemoryAddress.ToPointer());

					data[0] = val.X;
					data[2] = val.Y;
					return;
				}
				if (typeof(T) == typeof(Math.Vector3))
				{
					var val = (Math.Vector3)(object)(value);
					var data = (float*)(MemoryAddress.ToPointer());

					data[0] = val.X;
					data[2] = val.Y;
					data[4] = val.Z;
					return;
				}

				*(ulong*)(MemoryAddress.ToPointer()) = Function.ObjectToNative(value);
			}
			/// <summary>
			/// Set the value stored in the <see cref="GlobalVariable"/> to a string.
			/// </summary>
			/// <param name="value">The string to set the <see cref="GlobalVariable"/> to.</param>
			/// <param name="maxSize">The maximum size of the string. Can be found for a given global variable by checking the decompiled scripts from the game.</param>
			public unsafe void WriteString(string value, int maxSize)
			{
				if (maxSize % 8 != 0 || maxSize <= 0 || maxSize > 64)
				{
					throw new ArgumentException("The string maximum size should be one of 8, 16, 24, 32 or 64.", "maxSize");
				}

				// Null-terminate string
				value += '\0';

				// Write UTF-8 string to memory
				var size = Encoding.UTF8.GetByteCount(value);

				if (size >= maxSize)
				{
					size = maxSize - 1;
				}

				Marshal.Copy(Encoding.UTF8.GetBytes(value), 0, MemoryAddress, size);
			}

			/// <summary>
			/// Set the value of a specific bit of the <see cref="GlobalVariable"/> to true.
			/// </summary>
			/// <param name="index">The zero indexed bit of the <see cref="GlobalVariable"/> to set.</param>
			public unsafe void SetBit(int index)
			{
				if (index < 0 || index > 63)
				{
					throw new IndexOutOfRangeException("The bit index has to be between 0 and 63");
				}

				*(ulong*)(MemoryAddress.ToPointer()) |= (1u << index);
			}
			/// <summary>
			/// Set the value of a specific bit of the <see cref="GlobalVariable"/> to false.
			/// </summary>
			/// <param name="index">The zero indexed bit of the <see cref="GlobalVariable"/> to clear.</param>
			public unsafe void ClearBit(int index)
			{
				if (index < 0 || index > 63)
				{
					throw new IndexOutOfRangeException("The bit index has to be between 0 and 63");
				}

				*(ulong*)(MemoryAddress.ToPointer()) &= ~(1u << index);
			}
			/// <summary>
			/// Gets a value indicating whether a specific bit of the <see cref="GlobalVariable"/> is set.
			/// </summary>
			/// <param name="index">The zero indexed bit of the <see cref="GlobalVariable"/> to check.</param>
			public unsafe bool IsBitSet(int index)
			{
				if (index < 0 || index > 63)
				{
					throw new IndexOutOfRangeException("The bit index has to be between 0 and 63");
				}

				return ((*(ulong*)(MemoryAddress.ToPointer()) >> index) & 1) != 0;
			}

			/// <summary>
			/// Gets the <see cref="GlobalVariable"/> stored at a given offset in a global structure.
			/// </summary>
			/// <param name="index">The index the <see cref="GlobalVariable"/> is stored in the structure. For example the Y component of a Vector3 is at index 1.</param>
			/// <returns>The <see cref="GlobalVariable"/> at the index given.</returns>
			public unsafe GlobalVariable GetStructField(int index)
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
			public unsafe GlobalVariable[] GetArray(int itemSize)
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
			public unsafe GlobalVariable GetArrayItem(int index, int itemSize)
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
					throw new IndexOutOfRangeException($"The index {index} was outside the array bounds.");
				}

				return new GlobalVariable(MemoryAddress + 8 + (8 * itemSize * index));
			}
		}
		#endregion
	}
}
