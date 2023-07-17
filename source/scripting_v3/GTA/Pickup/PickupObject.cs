//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//
using System;

namespace GTA
{
	/// <summary>
	/// Represents a pickup object, which is for a <c>CPickup</c>.
	/// </summary>
	public sealed class PickupObject : Prop
	{
		internal PickupObject(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Get a <see cref="PickupObject"/> instance by its handle.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns>
		/// A <see cref="PickupObject"/> if the handle is for pickup object; otherwise, <see langword="null"/>.
		/// </returns>
		public static new PickupObject FromHandle(int handle)
		{
			IntPtr address = SHVDN.NativeMemory.GetEntityAddress(handle);
			if (address == IntPtr.Zero)
			{
				return null;
			}

			// We have to use some of Rockstar's RTTI info to ensure if this CObject is CPickup
			// Native functions that take a pickup object handle verify this in the same way
			// (fetches an address from the associated handle then tests if the class id hash
			// is the same as 0xAD2BCC1A (the joaat hash of the name "CPickup")
			const uint cPickupNameHash = 0xAD2BCC1A;
			if (SHVDN.NativeMemory.GetRageClassId(address) != cPickupNameHash)
			{
				return null;
			}

			return new PickupObject(handle);
		}
	}
}
