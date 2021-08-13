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
	}
}
