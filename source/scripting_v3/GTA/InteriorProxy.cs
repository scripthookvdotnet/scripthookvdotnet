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
		/// Gets the current <see cref="InteriorInstance"/> this <see cref="InteriorProxy"/> is using.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if this <see cref="InteriorProxy"/> is not using any <see cref="InteriorInstance"/>.</remarks>
		public InteriorInstance CurrentInteriorInstance
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
					return null;

				var interiorInstHandle = SHVDN.NativeMemory.GetAssociatedInteriorInstHandleFromInteriorProxy(Handle);
				return interiorInstHandle != 0 ? new InteriorInstance(interiorInstHandle) : null;
			}
		}

		/// <summary>
		/// Gets the model this <see cref="InteriorProxy"/> will load.
		/// </summary>
		public Model Model
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
					return null;

				return new Model(SHVDN.NativeMemory.ReadInt32(address + 0xE4));
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="InteriorProxy"/> will behave as if interior is not loaded completely.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="InteriorProxy"/> will behave as if interior is not loaded completely; otherwise, <see langword="false" />.
		/// </value>
		public bool IsDisabled => Function.Call<bool>(Hash.IS_INTERIOR_DISABLED, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="InteriorProxy"/> will only load a few elements of the interior.
		/// Doors can be loaded and the collision is not necessarily completely disabled (e.g. collisions for bullets and projectiles can work).
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="InteriorProxy"/> will only load a few elements of the interior; otherwise, <see langword="false" />.
		/// </value>
		public bool IsCapped => Function.Call<bool>(Hash.IS_INTERIOR_CAPPED, Handle);

		/// <summary>
		/// Refreshs the current <see cref="InteriorInstance"/> if loaded. Does not change the memory address or handle of the <see cref="InteriorInstance"/>.
		/// </summary>
		public void Refresh()
		{
			Function.Call(Hash.REFRESH_INTERIOR, Handle);
		}

		/// <summary>
		/// Disables the interior, making <see cref="InteriorProxy"/> behave as if interior is not loaded completely. Does not prevent from having a <see cref="InteriorInstance"/>.
		/// Does nothing if the player <see cref="Ped"/> is in this <see cref="InteriorProxy"/>.
		/// </summary>
		public void Disable(bool toggle)
		{
			Function.Call(Hash.DISABLE_INTERIOR, Handle, toggle);
		}

		/// <summary>
		/// Caps the interior so this <see cref="InteriorProxy"/> will only load a few elements of the interior.
		/// Does nothing if the player <see cref="Ped"/> is in this <see cref="InteriorProxy"/>.
		/// </summary>
		public void Cap(bool toggle)
		{
			Function.Call(Hash.CAP_INTERIOR, Handle, toggle);
		}

		/// <summary>
		/// Makes this <see cref="InteriorProxy"/> keep the <see cref="InteriorProxy"/> this <see cref="InteriorProxy"/> is loaded.
		/// </summary>
		public void PinInMemory()
		{
			Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, Handle);
		}

		/// <summary>
		/// Lets this <see cref="InteriorProxy"/> free the <see cref="InteriorProxy"/> this <see cref="InteriorProxy"/> is loaded.
		/// </summary>
		public void UnpinFromMemory()
		{
			Function.Call(Hash.UNPIN_INTERIOR, Handle);
		}

		static public InteriorProxy GetInteriorProxyAt(Vector3 position)
		{
			return FromHandle(Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z));
		}

		/// <summary>
		/// Gets the <see cref="InteriorProxy"/> if the gameplay camera is in a interior.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if the gameplay camera is not in any interior space.</remarks>
		static public InteriorProxy GetInteriorProxyFromGameplayCam()
		{
			var interiorInstHandle = SHVDN.NativeMemory.GetInteriorProxyHandleFromGameplayCam();
			return interiorInstHandle != 0 ? new InteriorProxy(interiorInstHandle) : null;
		}

		/// <summary>
		/// Determines if this <see cref="InteriorProxy"/> exists.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="InteriorProxy"/> exists; otherwise, <see langword="false" />.</returns>
		public bool Exists()
		{
			return SHVDN.NativeMemory.InteriorProxyHandleExists(Handle);
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
