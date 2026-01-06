using System;
using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x50)]
    internal struct CrBoneData
    {
        // Rotation (quaternion) is between 0x0 - 0x10
        // Translation (vector3) is between 0x10 - 0x1C
        // Scale (vector3?) is between 0x20 - 0x2C
        [FieldOffset(0x30)]
        internal ushort nextSiblingBoneIndex;
        [FieldOffset(0x32)]
        internal ushort parentBoneIndex;
        [FieldOffset(0x38)]
        internal IntPtr namePtr;
        [FieldOffset(0x42)]
        internal ushort boneIndex;
        [FieldOffset(0x44)]
        internal ushort boneId;

        internal string Name => namePtr == default ? null : Marshal.PtrToStringAnsi(namePtr);
    }
}
