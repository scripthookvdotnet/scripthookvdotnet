using System.Runtime.InteropServices;

namespace SHVDN
{
	[StructLayout(LayoutKind.Explicit, Size = 0x28)] // total size 40 bytes
	public struct ScrWeaponHudStats
    {
		[FieldOffset(0x00)] public byte Damage;
		[FieldOffset(0x08)] public byte Speed;
		[FieldOffset(0x10)] public byte Capacity;
		[FieldOffset(0x18)] public byte Accuracy;
		[FieldOffset(0x20)] public byte Range;

		public ScrWeaponHudStats(int damage, int speed, int capacity, int accuracy, int range)
		{
            Damage = (byte)damage;
            Speed = (byte)speed;
            Capacity = (byte)capacity;
            Accuracy = (byte)accuracy;
            Range = (byte)range;
        }
	}
}
