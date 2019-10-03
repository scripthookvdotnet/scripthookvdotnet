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
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Runtime.InteropServices;
using GTA.Math;

namespace GTA.Native
{
	internal static class NativeHelper
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
	}
	internal unsafe static class NativeHelperGeneric<T>
	{
		static class CastCache<From>
		{
			internal static readonly Func<From, T> Convert;

			static CastCache()
			{
				var paramExp = Expression.Parameter(typeof(From));
				var convertExp = Expression.Convert(paramExp, typeof(T));
				Convert = Expression.Lambda<Func<From, T>>(convertExp, paramExp).Compile();
			}
		}

		private static readonly Func<IntPtr, T> _ptrToStrFunc;

		static NativeHelperGeneric()
		{
			var ptrToStrMethod = new DynamicMethod("PtrToStructure<" + typeof(T) + ">", typeof(T),
				new Type[] { typeof(IntPtr) }, typeof(NativeHelperGeneric<T>), true);

			ILGenerator generator = ptrToStrMethod.GetILGenerator();
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldobj, typeof(T));
			generator.Emit(OpCodes.Ret);

			_ptrToStrFunc = (Func<IntPtr, T>)ptrToStrMethod.CreateDelegate(typeof(Func<IntPtr, T>));
		}

		internal static T ObjectFromNativeGeneric(ulong *value)
		{
			if (typeof(T).IsEnum)
			{
				return Convert(*value);
			}

			// Fundamental types
			if (typeof(T) == typeof(bool))
			{
				return PtrToStructure(new IntPtr(value));
			}
			if (typeof(T) == typeof(int))
			{
				return PtrToStructure(new IntPtr(value));
			}
			if (typeof(T) == typeof(uint))
			{
				return PtrToStructure(new IntPtr(value));
			}
			if (typeof(T) == typeof(long))
			{
				return PtrToStructure(new IntPtr(value));
			}
			if (typeof(T) == typeof(ulong))
			{
				return PtrToStructure(new IntPtr(value));
			}
			if (typeof(T) == typeof(float))
			{
				return PtrToStructure(new IntPtr(value));
			}
			if (typeof(T) == typeof(double))
			{
				return Convert(PtrToStructure(new IntPtr(value)));
			}

			// Math types
			if (typeof(T) == typeof(Vector2) || typeof(T) == typeof(Vector3))
			{
				return Convert(*(NativeHelper.NativeVector3*)value);
			}

			throw new InvalidCastException(string.Concat("Unable to cast native value to object of type '", typeof(T), "'"));
		}

		internal static T Convert<T1>(T1 from)
		{
			return CastCache<T1>.Convert(from);
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
		public InputArgument(int value) : this((ulong)value)
		{
		}
		public InputArgument(uint value) : this((ulong)value)
		{
		}
		public InputArgument(float value) : this((ulong)BitConverter.ToUInt32(BitConverter.GetBytes(value), 0))
		{
		}
		public InputArgument(double value) : this((float)value)
		{
		}
		public InputArgument(string value) : this((object)value)
		{
		}

		public InputArgument(Model value) : this((ulong)value.Hash)
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
			Marshal.FreeCoTaskMem((IntPtr)(long)data);
		}

		public T GetResult<T>()
		{
			unsafe
			{
				var type = typeof(T);
				if (type.IsEnum || type.IsPrimitive || type == typeof(Vector3) || type == typeof(Vector2))
					return NativeHelperGeneric<T>.ObjectFromNativeGeneric((ulong*)data);
				else
					return (T)Function.ObjectFromNative(type, (ulong*)data);
			}
		}
	}

	public static class Function
	{
		public static T Call<T>(Hash hash, params InputArgument[] arguments)
		{
			ulong[] args = new ulong[arguments.Length];
			for (int i = 0; i < arguments.Length; ++i)
				args[i] = arguments[i].data;

			var task = new SHVDN.NativeFunc((ulong)hash, args);

			SHVDN.ScriptDomain.CurrentDomain.ExecuteTask(task);

			unsafe
			{
				// The result will be null when this method is called from a thread other than the main thread 
				if (task.Result == null)
					throw new InvalidOperationException("Native.Function.Call can only be called from the main thread.");

				var type = typeof(T);
				if (type.IsEnum || type.IsPrimitive || type == typeof(Vector3) || type == typeof(Vector2))
					return NativeHelperGeneric<T>.ObjectFromNativeGeneric(task.Result);
				else
					return (T)ObjectFromNative(type, task.Result);
			}
		}
		public static void Call(Hash hash, params InputArgument[] arguments)
		{
			ulong[] args = new ulong[arguments.Length];
			for (int i = 0; i < arguments.Length; ++i)
				args[i] = arguments[i].data;

			var task = new SHVDN.NativeFunc((ulong)hash, args);

			SHVDN.ScriptDomain.CurrentDomain.ExecuteTask(task);
		}

		internal static ulong ObjectToNative(object value)
		{
			if (ReferenceEquals(value, null))
			{
				return 0;
			}

			var type = value.GetType();

			// Fundamental types
			if (type == typeof(bool))
			{
				return (bool)value ? 1ul : 0ul;
			}
			if (type == typeof(int))
			{
				return (ulong)(int)value;
			}
			if (type == typeof(uint))
			{
				return (uint)value;
			}
			if (type == typeof(float))
			{
				return BitConverter.ToUInt32(BitConverter.GetBytes((float)value), 0);
			}
			if (type == typeof(double))
			{
				return BitConverter.ToUInt32(BitConverter.GetBytes((float)(double)value), 0);
			}
			if (type == typeof(IntPtr))
			{
				return (ulong)((IntPtr)value).ToInt64();
			}
			if (type == typeof(string))
			{
				return (ulong)SHVDN.ScriptDomain.CurrentDomain.PinString((string)value).ToInt64();
			}

			// Scripting types
			if (type == typeof(Model))
			{
				return (ulong)((Model)value).Hash;
			}
			if (typeof(IHandleable).IsAssignableFrom(type))
			{
				return (ulong)((IHandleable)value).Handle;
			}

			throw new InvalidCastException(string.Concat("Unable to cast object of type '", type.FullName, "' to native value"));
		}
		internal static unsafe object ObjectFromNative(Type type, ulong* value)
		{
			if (type == typeof(string))
			{
				if (*value != 0)
				{
					int length = 0;
					for (byte* end = (byte*)*value; *end++ != 0; length++)
						continue;

					var bytes = new byte[length];

					Marshal.Copy((IntPtr)(long)*value, bytes, 0, length);

					return Encoding.UTF8.GetString(bytes);
				}
				else
				{
					return string.Empty;
				}
			}

			int handle = *(int*)value;

			// Scripting types
			if (type == typeof(Blip))
			{
				return new Blip(handle);
			}
			if (type == typeof(Camera))
			{
				return new Camera(handle);
			}
			if (type == typeof(Entity))
			{
				return Entity.FromHandle(handle);
			}
			if (type == typeof(Ped))
			{
				return new Ped(handle);
			}
			if (type == typeof(PedGroup))
			{
				return new PedGroup(handle);
			}
			if (type == typeof(Player))
			{
				return new Player(handle);
			}
			if (type == typeof(Prop))
			{
				return new Prop(handle);
			}
			if (type == typeof(Rope))
			{
				return new Rope(handle);
			}
			if (type == typeof(Vehicle))
			{
				return new Vehicle(handle);
			}

			throw new InvalidCastException(string.Concat("Unable to cast native value to object of type '", type.FullName, "'"));
		}

		internal static int GetUtf8CodePointSize(string str, int index)
		{
			uint chr = str[index];

			if (chr < 0x80)
			{
				return 1;
			}
			if (chr < 0x800)
			{
				return 2;
			}
			if (chr < 0x10000)
			{
				return 3;
			}
			#region Surrogate check
			const int HighSurrogateStart = 0xD800;
			const int LowSurrogateStart = 0xD800;

			var temp1 = (int)chr - HighSurrogateStart;
			if (temp1 < 0 || temp1 > 0x7ff)
			{
				return 0;
			}
			// Found a high surrogate
			if (index < str.Length - 1)
			{
				var temp2 = str[index + 1] - LowSurrogateStart;
				if (temp2 >= 0 && temp2 <= 0x3ff)
				{
					// Found a low surrogate
					return 4;
				}

				return 0;
			}
			else
			{
				return 0;
			}
			#endregion
		}
		internal static void PushLongString(string str, int maxLengthUtf8 = 99)
		{
			if (maxLengthUtf8 <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(maxLengthUtf8));
			}

			int size = Encoding.UTF8.GetByteCount(str);

			if (size <= maxLengthUtf8)
			{
				Call(Hash._ADD_TEXT_COMPONENT_STRING, str);
				return;
			}

			int currentUtf8StrLength = 0;
			int startPos = 0;
			int currentPos;

			for (currentPos = 0; currentPos < str.Length; currentPos++)
			{
				int codePointSize = GetUtf8CodePointSize(str, currentPos);

				if (currentUtf8StrLength + codePointSize > maxLengthUtf8)
				{
					Call(Hash._ADD_TEXT_COMPONENT_STRING, str.Substring(startPos, currentPos - startPos));

					currentUtf8StrLength = 0;
					startPos = currentPos;
				}
				else
				{
					currentUtf8StrLength += codePointSize;
				}

				//if the code point size is 4, additional increment is needed
				if (codePointSize == 4)
				{
					currentPos++;
				}
			}

			Call(Hash._ADD_TEXT_COMPONENT_STRING, str.Substring(startPos, str.Length - startPos));
		}
	}
}
