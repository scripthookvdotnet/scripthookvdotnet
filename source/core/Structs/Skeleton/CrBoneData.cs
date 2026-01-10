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
        public ushort NextSiblingBoneIndex;
        [FieldOffset(0x32)]
        public ushort ParentBoneIndex;
        [FieldOffset(0x38)]
        public IntPtr NamePtr;
        [FieldOffset(0x42)]
        public ushort BoneIndex;
        [FieldOffset(0x44)]
        public ushort BoneId;

        public string Name => NamePtr == default ? null : Marshal.PtrToStringAnsi(NamePtr);
    }
}
