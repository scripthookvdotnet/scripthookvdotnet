//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.Runtime.InteropServices;

namespace GTA
{
	[StructLayout(LayoutKind.Explicit, Size = 0x138)]
	internal unsafe struct DlcWeaponData
	{
		[FieldOffset(0x00)] private int validCheck;
		[FieldOffset(0x08)] private int weaponHash;
		[FieldOffset(0x18)] private int weaponCost;
		[FieldOffset(0x20)] private int ammoCost;
		[FieldOffset(0x28)] private int ammoType;
		[FieldOffset(0x30)] private int defaultClipSize;
		[FieldOffset(0x38)] private fixed byte name[0x40];
		[FieldOffset(0x78)] private fixed byte desc[0x40];
		[FieldOffset(0xB8)] private fixed byte simpleDesc[0x40]; // Usually refers to "the " + name
		[FieldOffset(0xF8)] private fixed byte upperCaseName[0x40];

		public WeaponHash Hash => (WeaponHash)weaponHash;

		public string DisplayName
		{
			get
			{
				fixed (byte* ptr = name)
				{
					return SHVDN.StringMarshal.PtrToStringUtf8(new IntPtr(ptr));
				}
			}
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x110)]
	internal unsafe struct DlcWeaponComponentData
	{
		[FieldOffset(0x00)] private int attachBone; // The bone on the gun to attach the component to
		[FieldOffset(0x08)] private int bActiveByDefault;
		[FieldOffset(0x18)] private int componentHash;
		[FieldOffset(0x28)] private int componentCost;
		[FieldOffset(0x30)] private fixed byte name[0x40];
		[FieldOffset(0x70)] private fixed byte desc[0x40];

		public WeaponComponent Hash => (WeaponComponent)componentHash;

		public string DisplayName
		{
			get
			{
				fixed (byte* ptr = name)
				{
					return SHVDN.StringMarshal.PtrToStringUtf8(new IntPtr(ptr));
				}
			}
		}
	}
}
