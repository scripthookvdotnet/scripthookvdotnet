using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0xB8)]
    internal struct PersonalityData
    {
        [FieldOffset(0x7C)]
        public bool IsMale;

        [FieldOffset(0x7D)]
        public bool IsHuman;

        [FieldOffset(0x7F)]
        public bool IsGang;
    }
}
