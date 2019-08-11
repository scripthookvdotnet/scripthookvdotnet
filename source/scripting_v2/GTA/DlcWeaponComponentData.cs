using GTA.Native;
using System;
using System.Runtime.InteropServices;

namespace GTA
{
	[StructLayout(LayoutKind.Explicit, Size = 0x110)]
	internal unsafe struct DlcWeaponComponentData
	{
		[FieldOffset(0x00)] private int attachBone;//the bone on the gun to attach the component to

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
					return SHVDN.NativeMemory.PtrToStringUTF8(new IntPtr(ptr));
				}
			}
		}
	}
}
