//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Runtime.InteropServices;
using GTA.Native;

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

		[FieldOffset(0x38)] private fixed byte name [0x40];

		[FieldOffset(0x78)] private fixed byte desc [0x40];

		[FieldOffset(0xB8)] private fixed byte simpleDesc [0x40]; //usually refers to "the " + name

		[FieldOffset(0xF8)] private fixed byte upperCaseName [0x40];

		public bool IsValid => !Function.Call<bool>(Native.Hash.IS_CONTENT_ITEM_LOCKED, validCheck);

		public WeaponHash Hash => (WeaponHash)weaponHash;

		public string DisplaySimpleDescription
		{
			get
			{
				fixed (byte* ptr = simpleDesc)
				{
					return SHVDN.NativeMemory.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}
		public string LocalizedSimpleDescription => Game.GetLocalizedString(DisplaySimpleDescription);

		public string DisplayDescription
		{
			get
			{
				fixed (byte* ptr = desc)
				{
					return SHVDN.NativeMemory.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}
		public string LocalizedDescription => Game.GetLocalizedString(DisplayDescription);

		public string DisplayName
		{
			get
			{
				fixed (byte* ptr = name)
				{
					return SHVDN.NativeMemory.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}
		public string LocalizedName => Game.GetLocalizedString(DisplayName);


		public string DisplayUpperName
		{
			get
			{
				fixed (byte* ptr = upperCaseName)
				{
					return SHVDN.NativeMemory.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}
		public string LocalizedUpperName => Game.GetLocalizedString(DisplayUpperName);
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x110)]
	internal unsafe struct DlcWeaponComponentData
	{
	    [FieldOffset(0x00)] private int attachBone;//the bone on the gun to attach the component to

		[FieldOffset(0x08)] private int bActiveByDefault;

		[FieldOffset(0x18)] private int componentHash;

		[FieldOffset(0x28)] private int componentCost;

		[FieldOffset(0x30)] private fixed byte name [0x40];

		[FieldOffset(0x70)] private fixed byte desc [0x40];

		public WeaponComponentHash Hash => (WeaponComponentHash)componentHash;

		public ComponentAttachmentPoint AttachPoint => (ComponentAttachmentPoint)attachBone;

		public bool ActiveByDefault => bActiveByDefault != 0;
		public string DisplayDescription
		{
			get
			{
				fixed (byte* ptr = desc)
				{
					return SHVDN.NativeMemory.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}

		public string LocalizedDescription => Game.GetLocalizedString(DisplayDescription);

		public string DisplayName
		{
			get
			{
				fixed (byte* ptr = name)
				{
					return SHVDN.NativeMemory.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}

		public string LocalizedName => Game.GetLocalizedString(DisplayName);
	}
}
