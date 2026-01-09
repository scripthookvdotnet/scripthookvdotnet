using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x30)]
    internal struct ScreenInfo
    {
        // these fields should be in pixel coordinates
        [FieldOffset(0x14)]
        internal uint Left;
        [FieldOffset(0x18)]
        internal uint Right;
        [FieldOffset(0x1C)]
        internal uint Top;
        [FieldOffset(0x20)]
        internal uint Bottom;
    }
}
