//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a interior proxy, which is for <c>CInteriorProxy</c> and is used for native functions for interiors.
	/// </summary>
	public sealed class InteriorProxy : INativeValue, IExistable
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
		/// Returns <see langword="null" /> if no <see cref="InteriorProxy"/> exists this the specified <paramref name="handle"/>
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return null;
				}

				int interiorInstHandle = SHVDN.NativeMemory.GetAssociatedInteriorInstHandleFromInteriorProxy(Handle);
				return interiorInstHandle != 0 ? new InteriorInstance(interiorInstHandle) : null;
			}
		}

		/// <summary>
		/// Gets the MLO model this <see cref="InteriorProxy"/> will load.
		/// Return value has the hashed value of <c>archetypeName</c> in a ymap file.
		/// </summary>
		public Model Model
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return default;
				}

				return new Model(SHVDN.NativeMemory.ReadInt32(address + 0xE4));
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="InteriorProxy"/> will not process, making the interior looks like completely not loaded.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="InteriorProxy"/> will not process, making the interior looks like completely not loaded; otherwise, <see langword="false" />.
		/// </value>
		public bool IsDisabled => Function.Call<bool>(Hash.IS_INTERIOR_DISABLED, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="InteriorProxy"/> is capped to load only the shell objects (usually walls + floor + doors + windows),
		/// prevents most of collisions from loading.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="InteriorProxy"/> is capped to load only the shell objects; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>
		/// Doors can be loaded and the collision is not necessarily completely disabled (e.g. collisions for bullets and projectiles can work).
		/// </remarks>
		public bool IsCapped => Function.Call<bool>(Hash.IS_INTERIOR_CAPPED, Handle);

		/// <summary>
		/// Refreshes the current <see cref="InteriorInstance"/> if loaded.
		/// </summary>
		/// <remarks>
		/// Does not change the memory address or handle of the <see cref="CurrentInteriorInstance"/>.
		/// </remarks>
		public void Refresh()
		{
			Function.Call(Hash.REFRESH_INTERIOR, Handle);
		}

		/// <summary>
		/// Disables the interior, making <see cref="InteriorProxy"/> will not process, making the interior looks like completely not loaded.
		/// </summary>
		/// <remarks>
		/// <para>Does not prevent from having a <see cref="InteriorInstance"/>.</para>
		/// <para>Does nothing if the player <see cref="Ped"/> is in this <see cref="InteriorProxy"/>.</para>
		/// </remarks>
		public void Disable(bool toggle)
		{
			Function.Call(Hash.DISABLE_INTERIOR, Handle, toggle);
		}

		/// <summary>
		/// Caps the interior so this <see cref="InteriorProxy"/> will load only the shell objects (usually walls + floor + doors + windows),
		/// prevents most of collisions from loading.
		/// </summary>
		/// <remarks>Does nothing if the player <see cref="Ped"/> is in this <see cref="InteriorProxy"/>.</remarks>
		public void Cap(bool toggle)
		{
			Function.Call(Hash.CAP_INTERIOR, Handle, toggle);
		}

		/// <summary>
		/// Makes the game keep the <see cref="InteriorProxy"/>.
		/// </summary>
		public void PinInMemory()
		{
			Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, Handle);
		}

		/// <summary>
		/// Lets the game free the <see cref="InteriorProxy"/>.
		/// </summary>
		public void UnpinFromMemory()
		{
			Function.Call(Hash.UNPIN_INTERIOR, Handle);
		}

		/// <summary>
		/// Mark the entity set with the given name in this <see cref="InteriorProxy"/> as being active.
		/// You need to call <see cref="Refresh"/> to apply the changes.
		/// </summary>
		public void ActivateEntitySet(string entitySetName)
			=> Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, Handle, entitySetName);

		/// <summary>
		/// Mark the entity set with the given name in this <see cref="InteriorProxy"/> as being inactive.
		/// You need to call <see cref="Refresh"/> to apply the changes.
		/// </summary>
		public void DeactivateEntitySet(string entitySetName)
			=> Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, Handle, entitySetName);
		/// <summary>
		/// Return <cref langword="true"/> if the entity set with the given name in this interior is marked as active.
		/// </summary>
		public bool IsEntitySetActive(string entitySetName)
			=> Function.Call<bool>(Hash.IS_INTERIOR_ENTITY_SET_ACTIVE, Handle, entitySetName);
		/// <summary>
		/// <para>
		/// Sets tint index for given entity set.
		/// You need to call <see cref="Refresh"/> to apply the changes.
		/// </para>
		/// <para>
		/// Not available in the game versions prior to v1.0.877.1.
		/// </para>
		/// </summary>
		/// <param name="entitySetName">The entity set name.</param>
		/// <param name="index">The tint index. Must be positive.</param>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if the method is called in one of the game versions prior to v1.0.877.1.
		/// </exception>
		public void SetEntitySetTintIndex(string entitySetName, int index)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_877_1_Steam, nameof(InteriorProxy), nameof(SetEntitySetTintIndex));
			Function.Call(Hash.SET_INTERIOR_ENTITY_SET_TINT_INDEX, Handle, entitySetName, index);
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
			int interiorInstHandle = SHVDN.NativeMemory.GetInteriorProxyHandleFromGameplayCam();
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
		/// Determines if an <see cref="object"/> refers to the same interior proxy as this <see cref="InteriorProxy"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true"/> if the <paramref name="obj"/> is the same interior proxy as this <see cref="InteriorProxy"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is InteriorProxy interior)
			{
				return Handle == interior.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="InteriorProxy"/>s refer to the same interior proxy.
		/// </summary>
		/// <param name="left">The left <see cref="InteriorProxy"/>.</param>
		/// <param name="right">The right <see cref="InteriorProxy"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is the same interior proxy as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(InteriorProxy left, InteriorProxy right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="InteriorProxy"/>s don't refer to the same interior proxy.
		/// </summary>
		/// <param name="left">The left <see cref="InteriorProxy"/>.</param>
		/// <param name="right">The right <see cref="InteriorProxy"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is not the same interior proxy as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(InteriorProxy left, InteriorProxy right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts an <see cref="InteriorProxy"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(InteriorProxy value)
		{
			return new InputArgument((ulong)(value?.Handle ?? 0));
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
