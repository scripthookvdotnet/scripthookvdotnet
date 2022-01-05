//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	public class InteriorProxy : INativeValue, IExistable
	{
		internal InteriorProxy(int handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// Creates a new instance of an <see cref="InteriorProxy"/> from the given handle.
		/// </summary>
		/// <param name="handle">The interior proxy handle.</param>
		/// <returns>
		/// Returns a <see cref="InteriorProxy"/> if this handle corresponds to a <see cref="InteriorProxy"/>.
		/// Returns <see langword="null" /> if no <see cref="Entity"/> exists this the specified <paramref name="handle"/>
		/// </returns>
		public static InteriorProxy FromHandle(int handle) => SHVDN.NativeMemory.InteriorProxyHandleExists(handle) ? new InteriorProxy(handle) : null;

		/// <summary>
		/// The handle of this <see cref="InteriorProxy"/>.
		/// </summary>
		public int Handle
		{
			get; private set;
		}

		/// <summary>
		/// The handle of this <see cref="InteriorProxy"/> translated to a native value.
		/// </summary>
		public ulong NativeValue
		{
			get => (ulong)Handle;
			set => Handle = unchecked((int)value);
		}

		/// <summary>
		/// Gets the memory address where the <see cref="InteriorProxy"/> is stored in memory.
		/// </summary>
		public IntPtr MemoryAddress => SHVDN.NativeMemory.GetInteriorProxyAddress(Handle);

		/// <summary>
		/// Gets or sets the position of this <see cref="AnimatedBuilding"/>.
		/// </summary>
		/// <value>
		/// The position in world space.
		/// </value>
		public Vector3 Position
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
					return Vector3.Zero;

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x70));
			}
		}

		/// <summary>
		/// Determines if this <see cref="InteriorProxy"/> exists.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="InteriorProxy"/> exists; otherwise, <see langword="false" />.</returns>
		public bool Exists()
		{
			return SHVDN.NativeMemory.InteriorProxyHandleExists(Handle);
		}

		static public InteriorProxy GetInteriorProxyAt(Vector3 position)
		{
			return FromHandle(Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z));
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same entity as this <see cref="InteriorProxy"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same entity as this <see cref="InteriorProxy"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Entity entity)
			{
				return Handle == entity.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="InteriorProxy"/>s refer to the same entity.
		/// </summary>
		/// <param name="left">The left <see cref="InteriorProxy"/>.</param>
		/// <param name="right">The right <see cref="InteriorProxy"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same entity as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(InteriorProxy left, InteriorProxy right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		/// <summary>
		/// Determines if two <see cref="Entity"/>s don't refer to the same entity.
		/// </summary>
		/// <param name="left">The left <see cref="Entity"/>.</param>
		/// <param name="right">The right <see cref="Entity"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same entity as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(InteriorProxy left, InteriorProxy right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts an <see cref="Entity"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(InteriorProxy value)
		{
			return new InputArgument((ulong)value.Handle);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
