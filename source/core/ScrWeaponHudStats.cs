using System.Runtime.InteropServices;

namespace SHVDN
{
	[StructLayout(LayoutKind.Explicit, Size = 0x28)] // total size 40 bytes
	public struct ScrWeaponHudStats
    {
        // Internally, each stat is represented by a scrValue union, which occupies 8 bytes on Win64.
        // When populated with data from CWeaponInfo (data is stored as signed 8-bit integers),
        // the values are implicitly converted from s8 to int when assigned to these fields.
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
