using System.Runtime.InteropServices;

namespace SHVDN
{
	[StructLayout(LayoutKind.Explicit, Size = 0x28)] // total size 40 bytes
	public struct ScrWeaponHudStats
    {
        // Internally, these fields are stored as 'scrValue's.
        // For HUD stats, they represent signed 8-bit integers (s8), aka 'char'.
        [FieldOffset(0x00)] public int Damage;
		[FieldOffset(0x08)] public int Speed;
		[FieldOffset(0x10)] public int Capacity;
		[FieldOffset(0x18)] public int Accuracy;
		[FieldOffset(0x20)] public int Range;

		public ScrWeaponHudStats(int damage, int speed, int capacity, int accuracy, int range)
		{
            Damage = damage;
            Speed = speed;
            Capacity = capacity;
            Accuracy = accuracy;
            Range = range;
        }
	}
}
