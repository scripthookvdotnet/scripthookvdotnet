using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0xB8)]
    internal struct PedPersonality
    {
        [FieldOffset(0x7C)]
        internal bool IsMale;
        [FieldOffset(0x7D)]
        internal bool IsHuman;
        [FieldOffset(0x7F)]
        internal bool IsGang;
    }
}
