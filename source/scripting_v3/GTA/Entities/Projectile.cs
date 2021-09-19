//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//
using System;
using SHVDN;

namespace GTA
{
	public class Projectile : Prop
	{
		internal Projectile(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Gets the <see cref="Ped"/> this <see cref="Projectile"/> belongs to. Can be <see langword="null" />.
		/// </summary>
		public Ped Owner
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.ProjectileOwnerOffset == 0)
				{
					return null;
				}

				IntPtr pedAddress = SHVDN.NativeMemory.ReadAddress(address + SHVDN.NativeMemory.ProjectileOwnerOffset);

				if (pedAddress == IntPtr.Zero)
					return null;

				return new Ped(SHVDN.NativeMemory.GetEntityHandleFromAddress(pedAddress));
			}
		}

		/// <summary>
		/// Gets the <see cref="WeaponHash"/> this <see cref="Projectile"/> was fired with.
		/// </summary>
		public WeaponHash WeaponHash
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.ProjectileAmmoInfoOffset == 0)
				{
					return (WeaponHash)0;
				}

				return (WeaponHash)SHVDN.NativeMemory.ReadInt32(address + SHVDN.NativeMemory.ProjectileAmmoInfoOffset + 0x8);
			}
		}

		/// <summary>
		/// Explodes this <see cref="Projectile"/>. Note that calling this method does not necessarily delete this <see cref="Projectile"/> due to the weapon configuration.
		/// </summary>
		public void Explode()
		{
			var address = MemoryAddress;
			if (address == IntPtr.Zero)
				return;

			SHVDN.NativeMemory.ExplodeProjectile(address);
		}
	}
}
