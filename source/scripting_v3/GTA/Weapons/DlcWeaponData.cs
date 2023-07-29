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
		[FieldOffset(0x00)] int validCheck;
		[FieldOffset(0x08)] int weaponHash;
		[FieldOffset(0x18)] int weaponCost;
		[FieldOffset(0x20)] int ammoCost;
		[FieldOffset(0x28)] int ammoType;
		[FieldOffset(0x30)] int defaultClipSize;
		[FieldOffset(0x38)] fixed byte name[0x40];
		[FieldOffset(0x78)] fixed byte desc[0x40];
		[FieldOffset(0xB8)] fixed byte simpleDesc[0x40]; // Usually refers to "the " + name
		[FieldOffset(0xF8)] fixed byte upperCaseName[0x40];

		public bool IsValid => !Function.Call<bool>(Native.Hash.IS_CONTENT_ITEM_LOCKED, validCheck);

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
		[FieldOffset(0x00)] int attachBone; // The bone on the gun to attach the component to
		[FieldOffset(0x08)] int bActiveByDefault;
		[FieldOffset(0x18)] int componentHash;
		[FieldOffset(0x28)] int componentCost;
		[FieldOffset(0x30)] fixed byte name[0x40];
		[FieldOffset(0x70)] fixed byte desc[0x40];

		public WeaponComponentHash Hash => (WeaponComponentHash)componentHash;

		public WeaponAttachmentPoint AttachmentPoint => (WeaponAttachmentPoint)attachBone;

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
