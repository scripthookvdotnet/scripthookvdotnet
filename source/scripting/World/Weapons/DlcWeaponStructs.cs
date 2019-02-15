using System;
using System.Runtime.InteropServices;
using GTA.Native;

namespace GTA
{
	[StructLayout(LayoutKind.Explicit, Size = 0x138)]
	public unsafe struct DlcWeaponData
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

		public bool IsValid
		{
			get { return !Function.Call<bool>(Native.Hash._IS_DLC_DATA_EMPTY, validCheck); }
		}

		public WeaponHash Hash
		{
			get { return (WeaponHash)weaponHash; }
		}

		public string DisplaySimpleDescription
		{
			get
			{
				fixed (byte* ptr = simpleDesc)
				{
					return MemoryAccess.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}
		public string LocalizedSimpleDescription
		{
			get
			{
				return Game.GetLocalizedString(DisplaySimpleDescription);
			}
		}

		public string DisplayDescription
		{
			get
			{
				fixed (byte* ptr = desc)
				{
					return MemoryAccess.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}
		public string LocalizedDescription
		{
			get
			{
				return Game.GetLocalizedString(DisplayDescription);
			}
		}

		public string DisplayName
		{
			get
			{
				fixed (byte* ptr = name)
				{
					return MemoryAccess.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}
		public string LocalizedName
		{
			get
			{
				return Game.GetLocalizedString(DisplayName);
			}
		}


		public string DisplayUpperName
		{
			get
			{
				fixed (byte* ptr = upperCaseName)
				{
					return MemoryAccess.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}
		public string LocalizedUpperName
		{
			get
			{
				return Game.GetLocalizedString(DisplayUpperName);
			}
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x110)]
	public unsafe struct DlcWeaponComponentData
	{
	    [FieldOffset(0x00)] private int attachBone;//the bone on the gun to attach the component to

		[FieldOffset(0x08)] private int bActiveByDefault;

		[FieldOffset(0x18)] private int componentHash;

		[FieldOffset(0x28)] private int componentCost;

		[FieldOffset(0x30)] private fixed byte name [0x40];

		[FieldOffset(0x70)] private fixed byte desc [0x40];

		public WeaponComponentHash Hash
		{
			get { return (WeaponComponentHash)componentHash; }
		}

		public ComponentAttachmentPoint AttachPoint
		{
			get { return (ComponentAttachmentPoint)attachBone; }
		}

		public bool ActiveByDefault
		{
			get { return bActiveByDefault != 0; }
		}
		public string DisplayDescription
		{
			get
			{
				fixed (byte* ptr = desc)
				{
					return MemoryAccess.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}

		public string LocalizedDescription
		{
			get
			{
				return Game.GetLocalizedString(DisplayDescription);
			}
		}

		public string DisplayName
		{
			get
			{
				fixed (byte* ptr = name)
				{
					return MemoryAccess.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}

		public string LocalizedName
		{
			get
			{
				return Game.GetLocalizedString(DisplayName);
			}
		}
	}

}
