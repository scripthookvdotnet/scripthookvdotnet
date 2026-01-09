using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct FragTypeChild
    {
        [FieldOffset(0x10)]
        internal ushort BoneIndex;
        [FieldOffset(0x12)]
        internal ushort BoneId;
    }
}
