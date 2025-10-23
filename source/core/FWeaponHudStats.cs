using System.Runtime.InteropServices;

namespace SHVDN
{
	[StructLayout(LayoutKind.Explicit, Size = 0x28)] // total size 40 bytes
	public struct FWeaponHudStats
	{
		[FieldOffset(0x00)] public byte Damage;
		[FieldOffset(0x08)] public byte Speed;
		[FieldOffset(0x10)] public byte Capacity;
		[FieldOffset(0x18)] public byte Accuracy;
		[FieldOffset(0x20)] public byte Range;

		public FWeaponHudStats(int damage, int speed, int capacity, int accuracy, int range)
		{
			Damage = ClampToByte(damage);
			Speed = ClampToByte(speed);
			Capacity = ClampToByte(capacity);
			Accuracy = ClampToByte(accuracy);
			Range = ClampToByte(range);
		}

		private static byte ClampToByte(int value) => (byte)(value < 0 ? 0 : (value > 255 ? 255 : value));
	}
}
