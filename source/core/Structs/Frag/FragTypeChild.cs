using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct FragTypeChild
    {
        [FieldOffset(0x10)]
        internal ushort boneIndex;
        [FieldOffset(0x12)]
        internal ushort boneId;
    }
}
