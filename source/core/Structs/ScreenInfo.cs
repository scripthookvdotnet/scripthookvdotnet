using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x30)]
    internal struct ScreenInfo
    {
        // these fields should be in pixel coordinates
        [FieldOffset(0x14)]
        public uint Left;
        [FieldOffset(0x18)]
        public uint Right;
        [FieldOffset(0x1C)]
        public uint Top;
        [FieldOffset(0x20)]
        public uint Bottom;
    }
}
