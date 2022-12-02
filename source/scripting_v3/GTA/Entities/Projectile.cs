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
		/// Gets the <see cref="Ped"/> this <see cref="Projectile"/> belongs to.
		/// Can be <see langword="null" /> or a <see cref="Ped"/> instance whose handle is for <see cref="Vehicle"/>, which is not valid as a <see cref="Ped"/> instance.
		/// </summary>
		[Obsolete("The Projectile.Owner is obsolete in the v3 API because the actual owner can be a Vehicle, use Projectile.OwnerEntity in the v3 API instead.")]
		public Ped Owner
		{
			get
			{
				var ownerHandle = GetOwnerEntityInternal();
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
				var ownerHandle = GetOwnerEntityInternal();
				return ownerHandle != 0 ? Entity.FromHandle(ownerHandle) : null;
			}
		}

		private int GetOwnerEntityInternal()
		{
			var address = MemoryAddress;
			if (address == IntPtr.Zero || SHVDN.NativeMemory.ProjectileOwnerOffset == 0)
			{
				return 0;
			}

			IntPtr entityAddress = SHVDN.NativeMemory.ReadAddress(address + SHVDN.NativeMemory.ProjectileOwnerOffset);

			if (entityAddress == IntPtr.Zero)
				return 0;

			return SHVDN.NativeMemory.GetEntityHandleFromAddress(entityAddress);
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

		/// <summary>
		/// Get a <see cref="Projectile"/> instance by its handle
		/// </summary>
		/// <param name="handle"></param>
		/// <returns>Null if not found or the entity is not a <see cref="Prop"/>, otherwise, a <see cref="Projectile"/></returns>
		public new static Projectile FromHandle(int handle)
		{
			var memoryAddress = NativeMemory.GetEntityAddress(handle);

			if (memoryAddress == IntPtr.Zero || NativeMemory.ReadByte(memoryAddress + 0x28) != 5)
				return null;

			return new Projectile(handle);
		}
	}
}
