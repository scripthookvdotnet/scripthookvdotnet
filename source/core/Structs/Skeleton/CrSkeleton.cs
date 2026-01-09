using System;
using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct CrSkeleton
    {
        [FieldOffset(0x00)]
        internal CrSkeletonData* SkeletonData;
        // this field has a pointer to one matrix, not a pointer to an array of matrices for all bones
        [FieldOffset(0x8)]
        internal ulong BoneTransformMatrixPtr;
        // object matrices (entity-local space)
        [FieldOffset(0x10)]
        internal ulong BoneObjectMatrixArrayPtr;
        // global matrices (world space)
        [FieldOffset(0x18)]
        internal ulong BoneGlobalMatrixArrayPtr;
        [FieldOffset(0x20)]
        internal int BoneCount;

        public IntPtr GetTransformMatrixAddress()
        {
            return new IntPtr((long)(BoneTransformMatrixPtr));
        }

        public IntPtr GetBoneObjectMatrixAddress(int boneIndex)
        {
            return new IntPtr((long)(BoneObjectMatrixArrayPtr + ((uint)boneIndex * 0x40)));
        }

        public IntPtr GetBoneGlobalMatrixAddress(int boneIndex)
        {
            return new IntPtr((long)(BoneGlobalMatrixArrayPtr + ((uint)boneIndex * 0x40)));
        }
    }
}
