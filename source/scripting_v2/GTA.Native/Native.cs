//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
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
		static class CastCache<TFrom>
		{
			internal static readonly Func<TFrom, T> Convert;

			static CastCache()
			{
				var paramExp = Expression.Parameter(typeof(TFrom));
				var convertExp = Expression.Convert(paramExp, typeof(T));
				Convert = Expression.Lambda<Func<TFrom, T>>(convertExp, paramExp).Compile();
			}
		}

		static readonly Func<IntPtr, T> _ptrToStrFunc;

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
		public static T Call<T>(Hash hash, params InputArgument[] arguments)
		{
			ulong[] args = new ulong[arguments.Length];
			for (int i = 0; i < arguments.Length; ++i)
			{
				args[i] = arguments[i].data;
			}

			unsafe
			{
				var res = SHVDN.NativeFunc.Invoke((ulong)hash, args);

				// The result will be null when this method is called from a thread other than the main thread
				if (res == null)
				{
					throw new InvalidOperationException("Native.Function.Call can only be called from the main thread.");
				}

				if (typeof(T).IsEnum || typeof(T).IsPrimitive || typeof(T) == typeof(Vector3) || typeof(T) == typeof(Vector2))
				{
					return ObjectFromNative<T>(res);
				}
				else
				{
					return (T)ObjectFromNative(typeof(T), res);
				}
			}
		}
		public static void Call(Hash hash, params InputArgument[] arguments)
		{
			ulong[] args = new ulong[arguments.Length];
			for (int i = 0; i < arguments.Length; ++i)
			{
				args[i] = arguments[i].data;
			}

			unsafe
			{
				SHVDN.NativeFunc.Invoke((ulong)hash, args);
			}
		}

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
				return SHVDN.NativeMemory.PtrToStringUTF8(new IntPtr((char*)*value));
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
