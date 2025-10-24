using System.Runtime.InteropServices;

namespace SHVDN
{
	[StructLayout(LayoutKind.Explicit, Size = 0x28)] // total size 40 bytes
	public struct ScrWeaponHudStats
    {
        // Internally in the game (CWeaponInfo), they are stored as signed 8-bit integers (s8)
        // within 'scrValue' unions (8 bytes on Win64, defined in value.h). 
        // When accessed through GET_WEAPON_HUD_STATS, the getter functions read the 'int' 
        // member of the scrValue union, which is why these fields are declared as int here.
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
