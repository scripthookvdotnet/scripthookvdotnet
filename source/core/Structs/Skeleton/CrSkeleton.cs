using System;
using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct CrSkeleton
    {
        [FieldOffset(0x00)]
        internal CrSkeletonData* skeletonData;
        // this field has a pointer to one matrix, not a pointer to an array of matrices for all bones
        [FieldOffset(0x8)]
        internal ulong boneTransformMatrixPtr;
        // object matrices (entity-local space)
        [FieldOffset(0x10)]
        internal ulong boneObjectMatrixArrayPtr;
        // global matrices (world space)
        [FieldOffset(0x18)]
        internal ulong boneGlobalMatrixArrayPtr;
        [FieldOffset(0x20)]
        internal int boneCount;

        public IntPtr GetTransformMatrixAddress()
        {
            return new IntPtr((long)(boneTransformMatrixPtr));
        }

        public IntPtr GetBoneObjectMatrixAddress(int boneIndex)
        {
            return new IntPtr((long)(boneObjectMatrixArrayPtr + ((uint)boneIndex * 0x40)));
        }

        public IntPtr GetBoneGlobalMatrixAddress(int boneIndex)
        {
            return new IntPtr((long)(boneGlobalMatrixArrayPtr + ((uint)boneIndex * 0x40)));
        }
    }
}
