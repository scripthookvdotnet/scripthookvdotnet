//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using GTA.Math;

namespace GTA.Native
{
	[StructLayout(LayoutKind.Explicit, Size = 0x18)]
	internal struct NativeVector3
	{
		[FieldOffset(0x00)]
		internal float X;
		[FieldOffset(0x08)]
		internal float Y;
		[FieldOffset(0x10)]
		internal float Z;

		internal NativeVector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static explicit operator Vector2(NativeVector3 val) => new Vector2(val.X, val.Y);
		public static explicit operator Vector3(NativeVector3 val) => new Vector3(val.X, val.Y, val.Z);
	}

	internal unsafe static class NativeHelper<T>
	{
		private static class CastCache<TFrom>
		{
			internal static readonly Func<TFrom, T> Convert;

			static CastCache()
			{
				ParameterExpression paramExp = Expression.Parameter(typeof(TFrom));
				UnaryExpression convertExp = Expression.Convert(paramExp, typeof(T));
				Convert = Expression.Lambda<Func<TFrom, T>>(convertExp, paramExp).Compile();
			}
		}

		private static readonly Func<IntPtr, T> _ptrToStrFunc;

		static NativeHelper()
		{
			var ptrToStrMethod = new DynamicMethod("PtrToStructure<" + typeof(T) + ">", typeof(T),
				new Type[] { typeof(IntPtr) }, typeof(NativeHelper<T>), true);

			ILGenerator generator = ptrToStrMethod.GetILGenerator();
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldobj, typeof(T));
			generator.Emit(OpCodes.Ret);

			_ptrToStrFunc = (Func<IntPtr, T>)ptrToStrMethod.CreateDelegate(typeof(Func<IntPtr, T>));
		}

		internal static T Convert<TFrom>(TFrom from)
		{
			return CastCache<TFrom>.Convert(from);
		}

		internal static T PtrToStructure(IntPtr ptr)
		{
			return _ptrToStrFunc(ptr);
		}
	}

	public class InputArgument
	{
		internal ulong data;

		public InputArgument(ulong value)
		{
			data = value;
		}
		public InputArgument(object value)
		{
			data = Function.ObjectToNative(value);
		}

		public InputArgument([MarshalAs(UnmanagedType.U1)] bool value) : this(value ? 1ul : 0ul)
		{
		}
		public InputArgument(int value) : this((uint)value)
		{
		}
		public InputArgument(uint value) : this((ulong)value)
		{
		}
		public InputArgument(float value)
		{
			unsafe
			{
				data = *(uint*)&value;
			}
		}
		public InputArgument(double value) : this((float)value)
		{
		}
		public InputArgument(string value) : this((object)value)
		{
		}

		public InputArgument(Model value) : this((uint)value.Hash)
		{
		}
		public InputArgument(Blip value) : this((object)value)
		{
		}
		public InputArgument(Camera value) : this((object)value)
		{
		}
		public InputArgument(Entity value) : this((object)value)
		{
		}
		public InputArgument(Ped value) : this((object)value)
		{
		}
		public InputArgument(PedGroup value) : this((object)value)
		{
		}
		public InputArgument(Player value) : this((object)value)
		{
		}
		public InputArgument(Prop value) : this((object)value)
		{
		}
		public InputArgument(Vehicle value) : this((object)value)
		{
		}
		public InputArgument(Rope value) : this((object)value)
		{
		}

		public static implicit operator InputArgument([MarshalAs(UnmanagedType.U1)] bool value)
		{
			return value ? new InputArgument(1ul) : new InputArgument(0ul);
		}
		public static implicit operator InputArgument(byte value)
		{
			return new InputArgument((ulong)value);
		}
		public static implicit operator InputArgument(sbyte value)
		{
			return new InputArgument((ulong)value);
		}
		public static implicit operator InputArgument(short value)
		{
			return new InputArgument((ulong)value);
		}
		public static implicit operator InputArgument(ushort value)
		{
			return new InputArgument((ulong)value);
		}
		public static implicit operator InputArgument(int value)
		{
			return new InputArgument((ulong)value);
		}
		public static implicit operator InputArgument(uint value)
		{
			return new InputArgument((ulong)value);
		}
		public static implicit operator InputArgument(float value)
		{
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(double value)
		{
			// Native functions don't consider any arguments as double, so convert double values to float ones
			return new InputArgument((float)value);
		}
		public static implicit operator InputArgument(string value)
		{
			return new InputArgument(value);
		}

		public static unsafe implicit operator InputArgument(bool* value)
		{
			return new InputArgument((ulong)new IntPtr(value).ToInt64());
		}
		public static unsafe implicit operator InputArgument(int* value)
		{
			return new InputArgument((ulong)new IntPtr(value).ToInt64());
		}
		public static unsafe implicit operator InputArgument(uint* value)
		{
			return new InputArgument((ulong)new IntPtr(value).ToInt64());
		}
		public static unsafe implicit operator InputArgument(float* value)
		{
			return new InputArgument((ulong)new IntPtr(value).ToInt64());
		}
		public static unsafe implicit operator InputArgument(sbyte* value)
		{
			return new InputArgument(new string(value));
		}

		public static implicit operator InputArgument(Model value)
		{
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(Blip value)
		{
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(Camera value)
		{
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(Entity value)
		{
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(Ped value)
		{
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(PedGroup value)
		{
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(Player value)
		{
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(Prop value)
		{
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(Vehicle value)
		{
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(Rope value)
		{
			return new InputArgument(value);
		}

		public override string ToString()
		{
			return data.ToString();
		}
	}

	public class OutputArgument : InputArgument, IDisposable
	{
		public OutputArgument() : base(Marshal.AllocCoTaskMem(24))
		{
		}
		public OutputArgument(object initvalue) : this()
		{
			unsafe { *(ulong*)data = Function.ObjectToNative(initvalue); }
		}

		public OutputArgument([MarshalAs(UnmanagedType.U1)] bool initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(byte initvalue) : this((object)(int)initvalue)
		{
		}
		public OutputArgument(sbyte initvalue) : this((object)(int)initvalue)
		{
		}
		public OutputArgument(short initvalue) : this((object)(int)initvalue)
		{
		}
		public OutputArgument(ushort initvalue) : this((object)(int)initvalue)
		{
		}
		public OutputArgument(int initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(uint initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(float initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(double initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(string initvalue) : this((object)initvalue)
		{
		}

		public OutputArgument(Model initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(Blip initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(Camera initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(Entity initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(Ped initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(PedGroup initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(Player initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(Prop initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(Vehicle initvalue) : this((object)initvalue)
		{
		}
		public OutputArgument(Rope initvalue) : this((object)initvalue)
		{
		}

		~OutputArgument()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool disposing)
		{
			if (data != 0)
			{
				Marshal.FreeCoTaskMem((IntPtr)(long)data);
				data = 0;
			}
		}

		public T GetResult<T>()
		{
			unsafe
			{
				if (typeof(T).IsEnum || typeof(T).IsPrimitive || typeof(T) == typeof(Vector3) || typeof(T) == typeof(Vector2))
				{
					return Function.ObjectFromNative<T>((ulong*)data);
				}
				else
				{
					return (T)Function.ObjectFromNative(typeof(T), (ulong*)data);
				}
			}
		}
	}

	public static class Function
	{
		private const int MaxArgCount = 63;

		public static T Call<T>(Hash hash, params InputArgument[] arguments)
		{
			unsafe
			{
				int argCount = arguments.Length <= MaxArgCount ? arguments.Length : MaxArgCount;
				ulong* argPtr = stackalloc ulong[argCount];

				for (int i = 0; i < argCount; ++i)
				{
					argPtr[i] = arguments[i]?.data ?? 0;
				}

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}

		#region Call with Return Value Overloads with Normal InputArgument Paramaters
		public static T Call<T>(Hash hash)
		{
			unsafe
			{
				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, null, 0);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash, InputArgument argument0)
		{
			unsafe
			{
				const int argCount = 1;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash, InputArgument argument0, InputArgument argument1)
		{
			unsafe
			{
				const int argCount = 2;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash, InputArgument argument0, InputArgument argument1, InputArgument argument2)
		{
			unsafe
			{
				const int argCount = 3;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash, InputArgument argument0, InputArgument argument1, InputArgument argument2, InputArgument argument3)
		{
			unsafe
			{
				const int argCount = 4;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4)
		{
			unsafe
			{
				const int argCount = 5;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5)
		{
			unsafe
			{
				const int argCount = 6;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6)
		{
			unsafe
			{
				const int argCount = 7;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7)
		{
			unsafe
			{
				const int argCount = 8;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8)
		{
			unsafe
			{
				const int argCount = 9;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9)
		{
			unsafe
			{
				const int argCount = 10;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9,
			InputArgument argument10)
		{
			unsafe
			{
				const int argCount = 11;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;
				argPtr[10] = argument10?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9,
			InputArgument argument10,
			InputArgument argument11)
		{
			unsafe
			{
				const int argCount = 12;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;
				argPtr[10] = argument10?.data ?? 0;
				argPtr[11] = argument11?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9,
			InputArgument argument10,
			InputArgument argument11,
			InputArgument argument12)
		{
			unsafe
			{
				const int argCount = 13;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;
				argPtr[10] = argument10?.data ?? 0;
				argPtr[11] = argument11?.data ?? 0;
				argPtr[12] = argument12?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9,
			InputArgument argument10,
			InputArgument argument11,
			InputArgument argument12,
			InputArgument argument13)
		{
			unsafe
			{
				const int argCount = 14;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;
				argPtr[10] = argument10?.data ?? 0;
				argPtr[11] = argument11?.data ?? 0;
				argPtr[12] = argument12?.data ?? 0;
				argPtr[13] = argument13?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9,
			InputArgument argument10,
			InputArgument argument11,
			InputArgument argument12,
			InputArgument argument13,
			InputArgument argument14)
		{
			unsafe
			{
				const int argCount = 15;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;
				argPtr[10] = argument10?.data ?? 0;
				argPtr[11] = argument11?.data ?? 0;
				argPtr[12] = argument12?.data ?? 0;
				argPtr[13] = argument13?.data ?? 0;
				argPtr[14] = argument14?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		public static T Call<T>(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9,
			InputArgument argument10,
			InputArgument argument11,
			InputArgument argument12,
			InputArgument argument13,
			InputArgument argument14,
			InputArgument argument15)
		{
			unsafe
			{
				const int argCount = 16;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;
				argPtr[10] = argument10?.data ?? 0;
				argPtr[11] = argument11?.data ?? 0;
				argPtr[12] = argument12?.data ?? 0;
				argPtr[13] = argument13?.data ?? 0;
				argPtr[14] = argument14?.data ?? 0;
				argPtr[15] = argument15?.data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		#endregion

		private static unsafe T ReturnValueFromNativeIfNotNull<T>(ulong* result)
		{
			// The result will be null when this method is called from a thread other than the main thread
			if (result == null)
			{
				ThrowInvalidOperationExceptionForInvalidNativeCall();
			}

			if (typeof(T).IsEnum || typeof(T).IsPrimitive || typeof(T) == typeof(Vector3) || typeof(T) == typeof(Vector2))
			{
				return ObjectFromNative<T>(result);
			}
			else
			{
				return (T)ObjectFromNative(typeof(T), result);
			}
		}
		// have to create this method to let JIT inline ReturnValueFromNativeIfNotNull
		private static void ThrowInvalidOperationExceptionForInvalidNativeCall() => throw new InvalidOperationException("Native.Function.Call can only be called from the main thread.");

		public static void Call(Hash hash, params InputArgument[] arguments)
		{
			unsafe
			{
				int argCount = arguments.Length <= MaxArgCount ? arguments.Length : MaxArgCount;
				ulong* argPtr = stackalloc ulong[argCount];

				for (int i = 0; i < argCount; ++i)
				{
					argPtr[i] = arguments[i]?.data ?? 0;
				}

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}

		#region void Call Overloads with Normal InputArgument Paramaters
		public static void Call(Hash hash)
		{
			unsafe
			{
				SHVDN.NativeFunc.Invoke((ulong)hash, null, 0);
			}
		}
		public static void Call(Hash hash, InputArgument argument0)
		{
			unsafe
			{
				const int argCount = 1;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash, InputArgument argument0, InputArgument argument1)
		{
			unsafe
			{
				const int argCount = 2;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash, InputArgument argument0, InputArgument argument1, InputArgument argument2)
		{
			unsafe
			{
				const int argCount = 3;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash, InputArgument argument0, InputArgument argument1, InputArgument argument2, InputArgument argument3)
		{
			unsafe
			{
				const int argCount = 4;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4)
		{
			unsafe
			{
				const int argCount = 5;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5)
		{
			unsafe
			{
				const int argCount = 6;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6)
		{
			unsafe
			{
				const int argCount = 7;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7)
		{
			unsafe
			{
				const int argCount = 8;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8)
		{
			unsafe
			{
				const int argCount = 9;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9)
		{
			unsafe
			{
				const int argCount = 10;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9,
			InputArgument argument10)
		{
			unsafe
			{
				const int argCount = 11;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;
				argPtr[10] = argument10?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9,
			InputArgument argument10,
			InputArgument argument11)
		{
			unsafe
			{
				const int argCount = 12;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;
				argPtr[10] = argument10?.data ?? 0;
				argPtr[11] = argument11?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9,
			InputArgument argument10,
			InputArgument argument11,
			InputArgument argument12)
		{
			unsafe
			{
				const int argCount = 13;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;
				argPtr[10] = argument10?.data ?? 0;
				argPtr[11] = argument11?.data ?? 0;
				argPtr[12] = argument12?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9,
			InputArgument argument10,
			InputArgument argument11,
			InputArgument argument12,
			InputArgument argument13)
		{
			unsafe
			{
				const int argCount = 14;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;
				argPtr[10] = argument10?.data ?? 0;
				argPtr[11] = argument11?.data ?? 0;
				argPtr[12] = argument12?.data ?? 0;
				argPtr[13] = argument13?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9,
			InputArgument argument10,
			InputArgument argument11,
			InputArgument argument12,
			InputArgument argument13,
			InputArgument argument14)
		{
			unsafe
			{
				const int argCount = 15;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;
				argPtr[10] = argument10?.data ?? 0;
				argPtr[11] = argument11?.data ?? 0;
				argPtr[12] = argument12?.data ?? 0;
				argPtr[13] = argument13?.data ?? 0;
				argPtr[14] = argument14?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		public static void Call(Hash hash,
			InputArgument argument0,
			InputArgument argument1,
			InputArgument argument2,
			InputArgument argument3,
			InputArgument argument4,
			InputArgument argument5,
			InputArgument argument6,
			InputArgument argument7,
			InputArgument argument8,
			InputArgument argument9,
			InputArgument argument10,
			InputArgument argument11,
			InputArgument argument12,
			InputArgument argument13,
			InputArgument argument14,
			InputArgument argument15)
		{
			unsafe
			{
				const int argCount = 16;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?.data ?? 0;
				argPtr[1] = argument1?.data ?? 0;
				argPtr[2] = argument2?.data ?? 0;
				argPtr[3] = argument3?.data ?? 0;
				argPtr[4] = argument4?.data ?? 0;
				argPtr[5] = argument5?.data ?? 0;
				argPtr[6] = argument6?.data ?? 0;
				argPtr[7] = argument7?.data ?? 0;
				argPtr[8] = argument8?.data ?? 0;
				argPtr[9] = argument9?.data ?? 0;
				argPtr[10] = argument10?.data ?? 0;
				argPtr[11] = argument11?.data ?? 0;
				argPtr[12] = argument12?.data ?? 0;
				argPtr[13] = argument13?.data ?? 0;
				argPtr[14] = argument14?.data ?? 0;
				argPtr[15] = argument15?.data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		#endregion

		internal static unsafe ulong ObjectToNative(object value)
		{
			if (value is null)
			{
				return 0;
			}

			if (value is bool valueBool)
			{
				return valueBool ? 1ul : 0ul;
			}
			if (value is int valueInt32)
			{
				// Prevent value from changing memory expression, in case the type is incorrect
				return (uint)valueInt32;
			}
			if (value is uint valueUInt32)
			{
				return valueUInt32;
			}
			if (value is float valueFloat)
			{
				return *(uint*)&valueFloat;
			}
			if (value is double valueDouble)
			{
				valueFloat = (float)valueDouble;
				return *(uint*)&valueFloat;
			}
			if (value is IntPtr valueIntPtr)
			{
				return (ulong)valueIntPtr.ToInt64();
			}
			if (value is string valueString)
			{
				return (ulong)SHVDN.ScriptDomain.CurrentDomain.PinString(valueString).ToInt64();
			}

			// Scripting types
			if (value is Model valueModel)
			{
				return (ulong)valueModel.Hash;
			}
			if (typeof(IHandleable).IsAssignableFrom(value.GetType()))
			{
				return (ulong)((IHandleable)value).Handle;
			}

			throw new InvalidCastException(string.Concat("Unable to cast object of type '", value.GetType(), "' to native value"));
		}

		internal static unsafe T ObjectFromNative<T>(ulong* value)
		{
			if (typeof(T).IsEnum)
			{
				return NativeHelper<T>.Convert(*value);
			}

			if (typeof(T) == typeof(bool))
			{
				// Return proper boolean values (true if non-zero and false if zero)
				bool valueBool = *value != 0;
				return NativeHelper<T>.PtrToStructure(new IntPtr(&valueBool));
			}

			if (typeof(T) == typeof(int) || typeof(T) == typeof(uint) || typeof(T) == typeof(long) || typeof(T) == typeof(ulong) || typeof(T) == typeof(float))
			{
				return NativeHelper<T>.PtrToStructure(new IntPtr(value));
			}

			if (typeof(T) == typeof(double))
			{
				return NativeHelper<T>.Convert(NativeHelper<T>.PtrToStructure(new IntPtr(value)));
			}

			if (typeof(T) == typeof(Vector2) || typeof(T) == typeof(Vector3))
			{
				return NativeHelper<T>.Convert(*(NativeVector3*)value);
			}

			throw new InvalidCastException(string.Concat("Unable to cast native value to object of type '", typeof(T), "'"));
		}

		internal static unsafe object ObjectFromNative(Type type, ulong* value)
		{
			if (type == typeof(string))
			{
				return SHVDN.StringMarshal.PtrToStringUtf8(new IntPtr((char*)*value));
			}

			// Scripting types
			if (type == typeof(Blip))
			{
				return new Blip(*(int*)value);
			}
			if (type == typeof(Camera))
			{
				return new Camera(*(int*)value);
			}
			if (type == typeof(Entity))
			{
				return Entity.FromHandle(*(int*)value);
			}
			if (type == typeof(Ped))
			{
				return new Ped(*(int*)value);
			}
			if (type == typeof(PedGroup))
			{
				return new PedGroup(*(int*)value);
			}
			if (type == typeof(Player))
			{
				return new Player(*(int*)value);
			}
			if (type == typeof(Prop))
			{
				return new Prop(*(int*)value);
			}
			if (type == typeof(Rope))
			{
				return new Rope(*(int*)value);
			}
			if (type == typeof(Vehicle))
			{
				return new Vehicle(*(int*)value);
			}

			throw new InvalidCastException(string.Concat("Unable to cast native value to object of type '", type, "'"));
		}
	}
}
