//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;

namespace GTA
{
	// Actually there's CProjectileRocket and CProjectileThrown, but mark this class as sealed until wrapper classes are implemented for either of them
	public sealed class Projectile : Prop
	{
		internal Projectile(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Gets the <see cref="Ped"/> this <see cref="Projectile"/> belongs to.
		/// Can be <see langword="null" /> or a <see cref="Ped"/> instance whose handle is for <see cref="Vehicle"/>, which is not valid as a <see cref="Ped"/> instance.
		/// </summary>
		[Obsolete("The Projectile.Owner is obsolete in the v3 API because the actual owner can be a Vehicle, use Projectile.OwnerEntity instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public Ped Owner
		{
			get
			{
				int ownerHandle = GetOwnerEntityInternal();
				return ownerHandle != 0 ? new Ped(ownerHandle) : null;
			}
		}

		/// <summary>
		/// Gets the <see cref="Entity"/> this <see cref="Projectile"/> belongs to. Can be <see langword="null" />.
		/// </summary>
		public Entity OwnerEntity
		{
			get
			{
				int ownerHandle = GetOwnerEntityInternal();
				return ownerHandle != 0 ? Entity.FromHandle(ownerHandle) : null;
			}
		}

		private int GetOwnerEntityInternal()
		{
			IntPtr address = MemoryAddress;
			if (address == IntPtr.Zero || SHVDN.NativeMemory.ProjectileOwnerOffset == 0)
			{
				return 0;
			}

			IntPtr entityAddress = SHVDN.NativeMemory.ReadAddress(address + SHVDN.NativeMemory.ProjectileOwnerOffset);
			if (entityAddress == IntPtr.Zero)
			{
				return 0;
			}

			return SHVDN.NativeMemory.GetEntityHandleFromAddress(entityAddress);
		}

		/// <summary>
		/// Gets the <see cref="WeaponHash"/> this <see cref="Projectile"/> was fired with.
		/// </summary>
		public WeaponHash WeaponHash
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.ProjectileAmmoInfoOffset == 0)
				{
					return 0;
				}

				return (WeaponHash)SHVDN.NativeMemory.ReadInt32(address + SHVDN.NativeMemory.ProjectileAmmoInfoOffset + 0x8);
			}
		}

		/// <summary>
		/// Explodes this <see cref="Projectile"/>. Note that calling this method does not necessarily delete this <see cref="Projectile"/> due to the weapon configuration.
		/// </summary>
		public void Explode()
		{
			IntPtr address = MemoryAddress;
			if (address == IntPtr.Zero)
			{
				return;
			}

			SHVDN.NativeMemory.ExplodeProjectile(address);
		}

		/// <summary>
		/// Get a <see cref="Projectile"/> instance by its handle
		/// </summary>
		/// <param name="handle"></param>
		/// <returns>Null if not found or the entity is not a <see cref="Prop"/>, otherwise, a <see cref="Projectile"/></returns>
		public static new Projectile FromHandle(int handle)
		{
			IntPtr address = SHVDN.NativeMemory.GetEntityAddress(handle);
			if (address == IntPtr.Zero || SHVDN.NativeMemory.ReadByte(address + 0x28) != 5)
			{
				return null;
			}

			// We can use some of Rockstar's RTTI info to ensure if this CObject is CProjectile or one of its subclasses
			// CProjectileRocket and CProjectileThrown also return the hash of CProjectile in this class id function
			const uint cProjectileNameHash = 0x49DC4195;
			if (SHVDN.NativeMemory.GetRageClassId(address) != cProjectileNameHash)
			{
				return null;
			}

			return new Projectile(handle);
		}
	}
}
