//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using GTA.Math;
using InlineIL;
using static InlineIL.IL.Emit;

namespace GTA.Native
{
	public interface INativeValue
	{
		ulong NativeValue
		{
			get; set;
		}
	}

	internal static class NativeHelper<T>
	{
		static class CastCache<TFrom>
		{
			internal static readonly Func<TFrom, T> Cast;

			static CastCache()
			{
				ParameterExpression paramExp = Expression.Parameter(typeof(TFrom));
				UnaryExpression convertExp = Expression.Convert(paramExp, typeof(T));
				Cast = Expression.Lambda<Func<TFrom, T>>(convertExp, paramExp).Compile();
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
			return CastCache<TFrom>.Cast(from);
		}

		internal static T PtrToStructure(IntPtr ptr)
		{
			return _ptrToStrFunc(ptr);
		}
	}
	internal static class InstanceCreator<T1, TInstance>
	{
		internal static Func<T1, TInstance> Create;

		static InstanceCreator()
		{
			ConstructorInfo constructorInfo = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
				new[] { typeof(T1) }, null);
			ParameterExpression arg1Exp = Expression.Parameter(typeof(T1));

			NewExpression newExp = Expression.New(constructorInfo, arg1Exp);
			var lambdaExp = Expression.Lambda<Func<T1, TInstance>>(newExp, arg1Exp);
			Create = lambdaExp.Compile();
		}
	}
	internal static class InstanceCreator<T1, T2, TInstance>
	{
		internal static Func<T1, T2, TInstance> Create;

		static InstanceCreator()
		{
			ConstructorInfo constructorInfo = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
				new[] { typeof(T1), typeof(T2) }, null);
			ParameterExpression arg1Exp = Expression.Parameter(typeof(T1));
			ParameterExpression arg2Exp = Expression.Parameter(typeof(T2));

			NewExpression newExp = Expression.New(constructorInfo, arg1Exp, arg2Exp);
			var lambdaExp = Expression.Lambda<Func<T1, T2, TInstance>>(newExp, arg1Exp, arg2Exp);
			Create = lambdaExp.Compile();
		}
	}
	internal static class InstanceCreator<T1, T2, T3, TInstance>
	{
		internal static Func<T1, T2, T3, TInstance> Create;

		static InstanceCreator()
		{
			ConstructorInfo constructor = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
				new[] { typeof(T1), typeof(T2), typeof(T3) }, null);
			ParameterExpression arg1 = Expression.Parameter(typeof(T1));
			ParameterExpression arg2 = Expression.Parameter(typeof(T2));
			ParameterExpression arg3 = Expression.Parameter(typeof(T3));

			NewExpression newExp = Expression.New(constructor, arg1, arg2, arg3);
			var lambdaExp = Expression.Lambda<Func<T1, T2, T3, TInstance>>(newExp, arg1, arg2, arg3);
			Create = lambdaExp.Compile();
		}
	}

	#region Functions
	/// <summary>
	/// An input argument passed to a script function.
	/// </summary>
	public sealed class InputArgument
	{
		internal ulong _data;

		/// <summary>
		/// Initializes a new instance of the <see cref="InputArgument"/> class to a script function input argument.
		/// </summary>
		/// <param name="value">The pointer value.</param>
		public InputArgument(ulong value)
		{
			_data = value;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="InputArgument"/> class to a script function input argument.
		/// </summary>
		/// <param name="value">The value.</param>
		public InputArgument(IntPtr value)
		{
			unsafe
			{
				_data = (ulong)value.ToInt64();
			}
		}
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
			// "new InputArgument(value ? 1 : 0)" calls InputArgument constructor using object parameter, not ulong one
			return value ? new InputArgument(1) : new InputArgument(0);
		}
		public static implicit operator InputArgument(byte value)
		{
			return new InputArgument(value);
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
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(int value)
		{
			return new InputArgument((ulong)value);
		}
		public static implicit operator InputArgument(uint value)
		{
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(long value)
		{
			return new InputArgument((ulong)value);
		}
		public static implicit operator InputArgument(ulong value)
		{
			return new InputArgument(value);
		}
		public static implicit operator InputArgument(float value)
		{
			unsafe
			{
				ulong ulongValue = 0;
				*(float*)&ulongValue = value;
				return new InputArgument(ulongValue);
			}
		}
		public static implicit operator InputArgument(double value)
		{
			unsafe
			{
				//Native functions don't consider any arguments as double, so convert double values to float ones
				ulong ulongValue = 0;
				*(float*)&ulongValue = (float)value;
				return new InputArgument(ulongValue);
			}
		}
		public static implicit operator InputArgument(Enum value)
		{
			// Note: The value will be boxed if the original value is a concrete enum
			Type enumDataType = Enum.GetUnderlyingType(value.GetType());
			ulong ulongValue = 0;

			if (enumDataType == typeof(int))
			{
				ulongValue = (ulong)Convert.ToInt32(value);
			}
			else if (enumDataType == typeof(uint))
			{
				ulongValue = Convert.ToUInt32(value);
			}
			else if (enumDataType == typeof(long))
			{
				ulongValue = (ulong)Convert.ToInt64(value);
			}
			else if (enumDataType == typeof(ulong))
			{
				ulongValue = Convert.ToUInt64(value);
			}
			else if (enumDataType == typeof(short))
			{
				ulongValue = (ulong)Convert.ToInt16(value);
			}
			else if (enumDataType == typeof(ushort))
			{
				ulongValue = Convert.ToUInt16(value);
			}
			else if (enumDataType == typeof(byte))
			{
				ulongValue = Convert.ToByte(value);
			}
			else if (enumDataType == typeof(sbyte))
			{
				ulongValue = (ulong)Convert.ToSByte(value);
			}

			return new InputArgument(ulongValue);
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
		#endregion
	}

	/// <summary>
	/// An output argument passed to a script function.
	/// </summary>
	public class OutputArgument : IDisposable
	{
		#region Fields
		bool _disposed = false;
		internal IntPtr _storage = IntPtr.Zero;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="OutputArgument"/> class for script functions that output data into pointers.
		/// Allocates 24 bytes so the instance can allocate for <see cref="Vector3"/>.
		/// </summary>
		public OutputArgument()
		{
			_storage = Marshal.AllocCoTaskMem(24);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="OutputArgument"/> class for script functions that output data into pointers.
		/// Allocates a block of memory of specified size.
		/// </summary>
		public OutputArgument(int size)
		{
			_storage = Marshal.AllocCoTaskMem(size);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="OutputArgument"/> class with an initial value for script functions
		/// that require the pointer to data instead of the actual data.
		/// </summary>
		/// <param name="value">The value to set the data of this <see cref="OutputArgument"/> to.</param>
		/// <remarks>
		/// This constructor only supports <see langword="null"/>, <see cref="string"/>, and classes and structs that implements <see cref="INativeValue"/>.
		/// Otherwise, this constructor throws an exception.
		/// </remarks>
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
			{
				return;
			}

			Marshal.FreeCoTaskMem(_storage);
			_storage = IntPtr.Zero;
			_disposed = true;
		}

		/// <summary>
		/// Allocates a <see cref="OutputArgument"/> instance where the storage has a block of memory of the specified struct type.
		/// </summary>
		public static OutputArgument Alloc<T>() where T : unmanaged
		{
			unsafe
			{
				return new OutputArgument(sizeof(T));
			}
		}
		/// <summary>
		/// Initializes a <see cref="OutputArgument"/> instance with the specified bittable struct.
		/// </summary>
		public static OutputArgument Init<T>(T value) where T : unmanaged
		{
			unsafe
			{
				var outArg = new OutputArgument(sizeof(T));
				*(T*)outArg._storage = value;

				return outArg;
			}
		}
		/// <summary>
		/// Initializes a <see cref="OutputArgument"/> instance with the specified bittable struct.
		/// </summary>
		public static OutputArgument Init<T>(ref T value) where T : unmanaged
		{
			unsafe
			{
				var outArg = new OutputArgument(sizeof(T));
				// since the byte size is the same between the passed struct and the strage in the OutputArgument instance,
				// it is safe to initalize the storage with the passed value
				*(T*)(void*)outArg._storage = *(T*)AsPointer(ref value);

				return outArg;
			}
		}

		/// <summary>
		/// Gets the value of data stored in this <see cref="OutputArgument"/>.
		/// Do not use this method for custom struct types that are not primitives or defined in the SDK/API.
		/// Use <see cref="GetResultAsBittableStruct{T}"/> for custom struct types instead.
		/// </summary>
		/// <exception cref="InvalidCastException">Thrown when failed to get the result for unsupported type.</exception>
		/// <exception cref="NullReferenceException">Thrown when the internal storage is already disposed.</exception>
		public T GetResult<T>()
		{
			unsafe
			{
				if (_storage == IntPtr.Zero)
				{
					// throw an exception from a dedicated method so JIT can inline the body of GetResult
					// this path won't be visited unless users don't care about if the instance is disposed
					ThrowNullReferenceExceptionIfDisposed();
				}

				return Function.ReturnValueFromResultAddress<T>((ulong*)_storage.ToPointer());
			}

			static void ThrowNullReferenceExceptionIfDisposed() => throw new NullReferenceException();
		}

		/// <summary>
		/// Gets the value of data stored in this <see cref="OutputArgument"/> as a bittable struct without any conversion.
		/// Do not use this method for scripting types defined in the SDK/API such as <see cref="Vector3"/>.
		/// Use <see cref="GetResult{T}"/> for scipting types instead.
		/// </summary>
		/// <exception cref="NullReferenceException">Thrown when the internal storage is already disposed.</exception>
		public T GetResultAsBittableStruct<T>() where T : unmanaged => Marshal.PtrToStructure<T>(_storage);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable IDE0060 // Remove unused parameter
		private static unsafe void* AsPointer<T>(ref T value)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			Ldarg(nameof(value));
			Conv_U();
			return IL.ReturnPointer();
		}
	}

	/// <summary>
	/// A static class which handles script function execution.
	/// </summary>
	public static class Function
	{
		private const int MaxArgCount = 63;

		/// <summary>
		/// Calls the specified native script function and returns its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the native script function.</param>
		/// <param name="arguments">A list of input and output arguments to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
		public static T Call<T>(Hash hash, params InputArgument[] arguments)
		{
			unsafe
			{
				int argCount = arguments.Length <= MaxArgCount ? arguments.Length : MaxArgCount;
				ulong* argPtr = stackalloc ulong[argCount];

				for (int i = 0; i < argCount; ++i)
				{
					argPtr[i] = arguments[i]?._data ?? 0;
				}

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}

		#region Call with Return Value Overloads with Normal InputArgument Paramaters
		/// <summary>
		/// Calls the specified native script function and returns its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <returns>The return value of the native.</returns>
		public static T Call<T>(Hash hash)
		{
			unsafe
			{
				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, null, 0);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument">The input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
		public static T Call<T>(Hash hash, InputArgument argument)
		{
			unsafe
			{
				const int argCount = 1;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
		public static T Call<T>(Hash hash, InputArgument argument0, InputArgument argument1)
		{
			unsafe
			{
				const int argCount = 2;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
		public static T Call<T>(Hash hash, InputArgument argument0, InputArgument argument1, InputArgument argument2)
		{
			unsafe
			{
				const int argCount = 3;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
		public static T Call<T>(Hash hash, InputArgument argument0, InputArgument argument1, InputArgument argument2, InputArgument argument3)
		{
			unsafe
			{
				const int argCount = 4;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <param name="argument10">The 11th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;
				argPtr[10] = argument10?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <param name="argument10">The 11th input or output argument to pass to the native script function.</param>
		/// <param name="argument11">The 12th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;
				argPtr[10] = argument10?._data ?? 0;
				argPtr[11] = argument11?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <param name="argument10">The 11th input or output argument to pass to the native script function.</param>
		/// <param name="argument11">The 12th input or output argument to pass to the native script function.</param>
		/// <param name="argument12">The 13th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;
				argPtr[10] = argument10?._data ?? 0;
				argPtr[11] = argument11?._data ?? 0;
				argPtr[12] = argument12?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <param name="argument10">The 11th input or output argument to pass to the native script function.</param>
		/// <param name="argument11">The 12th input or output argument to pass to the native script function.</param>
		/// <param name="argument12">The 13th input or output argument to pass to the native script function.</param>
		/// <param name="argument13">The 14th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;
				argPtr[10] = argument10?._data ?? 0;
				argPtr[11] = argument11?._data ?? 0;
				argPtr[12] = argument12?._data ?? 0;
				argPtr[13] = argument13?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <param name="argument10">The 11th input or output argument to pass to the native script function.</param>
		/// <param name="argument11">The 12th input or output argument to pass to the native script function.</param>
		/// <param name="argument12">The 13th input or output argument to pass to the native script function.</param>
		/// <param name="argument13">The 14th input or output argument to pass to the native script function.</param>
		/// <param name="argument14">The 15th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;
				argPtr[10] = argument10?._data ?? 0;
				argPtr[11] = argument11?._data ?? 0;
				argPtr[12] = argument12?._data ?? 0;
				argPtr[13] = argument13?._data ?? 0;
				argPtr[14] = argument14?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <param name="argument10">The 11th input or output argument to pass to the native script function.</param>
		/// <param name="argument11">The 12th input or output argument to pass to the native script function.</param>
		/// <param name="argument12">The 13th input or output argument to pass to the native script function.</param>
		/// <param name="argument13">The 14th input or output argument to pass to the native script function.</param>
		/// <param name="argument14">The 15th input or output argument to pass to the native script function.</param>
		/// <param name="argument15">The 16th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;
				argPtr[10] = argument10?._data ?? 0;
				argPtr[11] = argument11?._data ?? 0;
				argPtr[12] = argument12?._data ?? 0;
				argPtr[13] = argument13?._data ?? 0;
				argPtr[14] = argument14?._data ?? 0;
				argPtr[15] = argument15?._data ?? 0;

				ulong* res = SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
				return ReturnValueFromNativeIfNotNull<T>(res);
			}
		}
		#endregion

		static unsafe T ReturnValueFromNativeIfNotNull<T>(ulong* result)
		{
			// The result will be null when this method is called from a thread other than the main thread
			if (result == null)
			{
				ThrowInvalidOperationExceptionForInvalidNativeCall();
			}

			return ReturnValueFromResultAddress<T>(result);
		}
		// have to create this method to let JIT inline ReturnValueFromNativeIfNotNull
		static void ThrowInvalidOperationExceptionForInvalidNativeCall() => throw new InvalidOperationException("Native.Function.Call can only be called from the main thread.");

		internal static unsafe T ReturnValueFromResultAddress<T>(ulong* result)
		{
			// The Entity class is abstract and can't be instantiated with our InstanceCreator class as expected
			if (IsKnownClassTypeAssignableFromINativeValueAndNotPoolObject(typeof(T)) || typeof(T).IsValueType || typeof(T).IsEnum || (typeof(T) != typeof(Entity) && typeof(T).IsSubclassOf(typeof(PoolObject))))
			{
				return ObjectFromNative<T>(result);
			}
			else
			{
				return (T)ObjectFromNative(typeof(T), result);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsKnownClassTypeAssignableFromINativeValueAndNotPoolObject(Type type)
		{
			return type == typeof(Player) || type == typeof(Scaleform) || type == typeof(InteriorProxy);
		}

		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="arguments">A list of input and output arguments to pass to the native script function.</param>
		public static void Call(Hash hash, params InputArgument[] arguments)
		{
			unsafe
			{
				int argCount = arguments.Length <= MaxArgCount ? arguments.Length : MaxArgCount;
				ulong* argPtr = stackalloc ulong[argCount];

				for (int i = 0; i < argCount; ++i)
				{
					argPtr[i] = arguments[i]?._data ?? 0;
				}

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}

		#region void Call Overloads with Normal InputArgument Paramaters
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		public static void Call(Hash hash)
		{
			unsafe
			{
				SHVDN.NativeFunc.Invoke((ulong)hash, null, 0);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The input or output argument to pass to the native script function.</param>
		public static void Call(Hash hash, InputArgument argument0)
		{
			unsafe
			{
				const int argCount = 1;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		public static void Call(Hash hash, InputArgument argument0, InputArgument argument1)
		{
			unsafe
			{
				const int argCount = 2;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		public static void Call(Hash hash, InputArgument argument0, InputArgument argument1, InputArgument argument2)
		{
			unsafe
			{
				const int argCount = 3;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		public static void Call(Hash hash, InputArgument argument0, InputArgument argument1, InputArgument argument2, InputArgument argument3)
		{
			unsafe
			{
				const int argCount = 4;
				ulong* argPtr = stackalloc ulong[argCount];

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <param name="argument10">The 11th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;
				argPtr[10] = argument10?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <param name="argument10">The 11th input or output argument to pass to the native script function.</param>
		/// <param name="argument11">The 12th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;
				argPtr[10] = argument10?._data ?? 0;
				argPtr[11] = argument11?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <param name="argument10">The 11th input or output argument to pass to the native script function.</param>
		/// <param name="argument11">The 12th input or output argument to pass to the native script function.</param>
		/// <param name="argument12">The 13th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;
				argPtr[10] = argument10?._data ?? 0;
				argPtr[11] = argument11?._data ?? 0;
				argPtr[12] = argument12?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <param name="argument10">The 11th input or output argument to pass to the native script function.</param>
		/// <param name="argument11">The 12th input or output argument to pass to the native script function.</param>
		/// <param name="argument12">The 13th input or output argument to pass to the native script function.</param>
		/// <param name="argument13">The 14th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;
				argPtr[10] = argument10?._data ?? 0;
				argPtr[11] = argument11?._data ?? 0;
				argPtr[12] = argument12?._data ?? 0;
				argPtr[13] = argument13?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <param name="argument10">The 11th input or output argument to pass to the native script function.</param>
		/// <param name="argument11">The 12th input or output argument to pass to the native script function.</param>
		/// <param name="argument12">The 13th input or output argument to pass to the native script function.</param>
		/// <param name="argument13">The 14th input or output argument to pass to the native script function.</param>
		/// <param name="argument14">The 15th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;
				argPtr[10] = argument10?._data ?? 0;
				argPtr[11] = argument11?._data ?? 0;
				argPtr[12] = argument12?._data ?? 0;
				argPtr[13] = argument13?._data ?? 0;
				argPtr[14] = argument14?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		/// <summary>
		/// Calls the specified native script function and ignores its return value.
		/// </summary>
		/// <param name="hash">The hashed name of the script function.</param>
		/// <param name="argument0">The 1st input or output argument to pass to the native script function.</param>
		/// <param name="argument1">The 2nd input or output argument to pass to the native script function.</param>
		/// <param name="argument2">The 3rd input or output argument to pass to the native script function.</param>
		/// <param name="argument3">The 4th input or output argument to pass to the native script function.</param>
		/// <param name="argument4">The 5th input or output argument to pass to the native script function.</param>
		/// <param name="argument5">The 6th input or output argument to pass to the native script function.</param>
		/// <param name="argument6">The 7th input or output argument to pass to the native script function.</param>
		/// <param name="argument7">The 8th input or output argument to pass to the native script function.</param>
		/// <param name="argument8">The 9th input or output argument to pass to the native script function.</param>
		/// <param name="argument9">The 10th input or output argument to pass to the native script function.</param>
		/// <param name="argument10">The 11th input or output argument to pass to the native script function.</param>
		/// <param name="argument11">The 12th input or output argument to pass to the native script function.</param>
		/// <param name="argument12">The 13th input or output argument to pass to the native script function.</param>
		/// <param name="argument13">The 14th input or output argument to pass to the native script function.</param>
		/// <param name="argument14">The 15th input or output argument to pass to the native script function.</param>
		/// <param name="argument15">The 16th input or output argument to pass to the native script function.</param>
		/// <returns>The return value of the native.</returns>
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

				argPtr[0] = argument0?._data ?? 0;
				argPtr[1] = argument1?._data ?? 0;
				argPtr[2] = argument2?._data ?? 0;
				argPtr[3] = argument3?._data ?? 0;
				argPtr[4] = argument4?._data ?? 0;
				argPtr[5] = argument5?._data ?? 0;
				argPtr[6] = argument6?._data ?? 0;
				argPtr[7] = argument7?._data ?? 0;
				argPtr[8] = argument8?._data ?? 0;
				argPtr[9] = argument9?._data ?? 0;
				argPtr[10] = argument10?._data ?? 0;
				argPtr[11] = argument11?._data ?? 0;
				argPtr[12] = argument12?._data ?? 0;
				argPtr[13] = argument13?._data ?? 0;
				argPtr[14] = argument14?._data ?? 0;
				argPtr[15] = argument15?._data ?? 0;

				SHVDN.NativeFunc.Invoke((ulong)hash, argPtr, argCount);
			}
		}
		#endregion

		/// <summary>
		/// Converts a managed object to a native value.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>A native value representing the input <paramref name="value"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ulong ObjectToNative(object value)
		{
			if (value is null)
			{
				return 0;
			}

			if (value is string valueString)
			{
				return (ulong)SHVDN.ScriptDomain.CurrentDomain.PinString(valueString).ToInt64();
			}

			if (value is INativeValue nativeValue)
			{
				return nativeValue.NativeValue;
			}

			ThrowExceptionForObjectToNative(value);
			return 0;
		}
		static void ThrowExceptionForObjectToNative(object value) => throw new InvalidCastException(string.Concat("Unable to cast object of type '", value.GetType(), "' to native value"));

		/// <summary>
		/// Converts a native value to a managed object of a value type.
		/// </summary>
		/// <typeparam name="T">The return type. The type should be a value type.</typeparam>
		/// <param name="value">The native value to convert.</param>
		/// <returns>A managed object representing the input <paramref name="value"/>.</returns>
		internal static unsafe T ObjectFromNative<T>(ulong* value)
		{
			if (typeof(T) == typeof(bool))
			{
				// Return proper boolean values (true if non-zero and false if zero)
				bool valueBool = *value != 0;
				return NativeHelper<T>.PtrToStructure(new IntPtr(&valueBool));
			}
			if (typeof(T) == typeof(IntPtr)) // Has to be before 'IsPrimitive' check
			{
				return InstanceCreator<long, T>.Create((long)(*value));
			}

			if (typeof(T) == typeof(Math.Vector2))
			{
				float* data = (float*)value;
				return InstanceCreator<float, float, T>.Create(data[0], data[2]);

			}
			if (typeof(T) == typeof(Math.Vector3))
			{
				float* data = (float*)value;
				return InstanceCreator<float, float, float, T>.Create(data[0], data[2], data[4]);
			}

			if (typeof(T) == typeof(Model) || typeof(T) == typeof(WeaponAsset) || typeof(T) == typeof(RelationshipGroup) || typeof(T) == typeof(ShapeTestHandle))
			{
				return InstanceCreator<int, T>.Create((int)*value);
			}

			if (typeof(T).IsPrimitive)
			{
				return NativeHelper<T>.PtrToStructure(new IntPtr(value));
			}
			if (typeof(T).IsEnum)
			{
				return NativeHelper<T>.Convert(*value);
			}

			if (IsKnownClassTypeAssignableFromINativeValueAndNotPoolObject(typeof(T)) || (typeof(T).IsSubclassOf(typeof(PoolObject)) && !typeof(T).IsAbstract))
			{
				return InstanceCreator<int, T>.Create((int)*value);
			}

			throw new InvalidCastException(string.Concat("Unable to cast native value to object of type '", typeof(T), "'"));
		}

		/// <summary>
		/// Converts a native value to a managed object of a reference type.
		/// </summary>
		/// <param name="type">The type to convert to. The type should be a reference type.</param>
		/// <param name="value">The native value to convert.</param>
		/// <returns>A managed object representing the input <paramref name="value"/>.</returns>
		internal static unsafe object ObjectFromNative(Type type, ulong* value)
		{
			if (type == typeof(string))
			{
				return SHVDN.StringMarshal.PtrToStringUtf8(new IntPtr((char*)*value));
			}

			if (type == typeof(Entity))
			{
				return Entity.FromHandle((int)*value);
			}
			if (typeof(INativeValue).IsAssignableFrom(type))
			{
				// Edge case. Warning: Requires classes implementing 'INativeValue' to repeat all constructor work in the setter of 'NativeValue'
				var result = (INativeValue)(System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type));
				result.NativeValue = *value;

				return result;
			}

			throw new InvalidCastException(string.Concat("Unable to cast native value to object of type '", type.FullName, "'"));
		}
	}
	#endregion

	#region Global Variables
	/// <summary>
	/// A value class which handles access to global script variables.
	/// </summary>
	public struct GlobalVariable
	{
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
			IntPtr address = SHVDN.NativeMemory.GetGlobalPtr(index);

			if (address == IntPtr.Zero)
			{
				throw new IndexOutOfRangeException($"The index {index.ToString()} does not correspond to an existing global variable.");
			}

			return new GlobalVariable(address);
		}

		/// <summary>
		/// Gets the native memory address of the <see cref="GlobalVariable"/>.
		/// </summary>
		public IntPtr MemoryAddress
		{
			get;
		}

		/// <summary>
		/// Gets the value stored in the <see cref="GlobalVariable"/>.
		/// </summary>
		public unsafe T Read<T>()
		{
			if (typeof(T) == typeof(string))
			{
				return (T)(object)SHVDN.StringMarshal.PtrToStringUtf8(MemoryAddress);
			}
			else
			{
				return Function.ReturnValueFromResultAddress<T>((ulong*)(MemoryAddress.ToPointer()));
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
				float* data = (float*)(MemoryAddress.ToPointer());

				data[0] = val.X;
				data[2] = val.Y;
				return;
			}
			if (typeof(T) == typeof(Math.Vector3))
			{
				var val = (Math.Vector3)(object)(value);
				float* data = (float*)(MemoryAddress.ToPointer());

				data[0] = val.X;
				data[2] = val.Y;
				data[4] = val.Z;
				return;
			}

			if (typeof(T) == typeof(bool))
			{
				*(ulong*)(MemoryAddress.ToPointer()) = NativeHelper<ulong>.Convert(value);
			}
			else if (typeof(T) == typeof(double))
			{
				*(ulong*)(MemoryAddress.ToPointer()) = 0; // padding
				*(float*)(MemoryAddress.ToPointer()) = NativeHelper<float>.Convert(value);
			}
			else if (typeof(T).IsPrimitive)
			{
				if (typeof(T) == typeof(IntPtr))
				{
					*(long*)(MemoryAddress.ToPointer()) = NativeHelper<long>.Convert(value);
				}
				else
				{
					*(ulong*)(MemoryAddress.ToPointer()) = NativeHelper<ulong>.Convert(value);
				}
			}
			else
			{
				*(ulong*)(MemoryAddress.ToPointer()) = Function.ObjectToNative(value);
			}
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
				throw new ArgumentException("The string maximum size should be one of 8, 16, 24, 32 or 64.", nameof(maxSize));
			}

			// Null-terminate string
			value += '\0';

			// Write UTF-8 string to memory
			int size = Encoding.UTF8.GetByteCount(value);

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
				throw new ArgumentOutOfRangeException(nameof(itemSize), "The item size for an array must be positive.");
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
				throw new ArgumentOutOfRangeException(nameof(itemSize), "The item size for an array must be positive.");
			}

			int count = Read<int>();

			// Globals are stored in pages that hold a maximum of 65536 items
			if (count < 1 || count >= 65536 / itemSize)
			{
				throw new InvalidOperationException("The variable does not seem to be an array.");
			}

			if (index < 0 || index >= count)
			{
				throw new IndexOutOfRangeException($"The index {index.ToString()} was outside the array bounds.");
			}

			return new GlobalVariable(MemoryAddress + 8 + (8 * itemSize * index));
		}
	}
	#endregion
}
