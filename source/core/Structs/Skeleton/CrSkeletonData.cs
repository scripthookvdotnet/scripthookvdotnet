using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct CrSkeletonData
    {
        [FieldOffset(0x10)]
        public PgHashMap BoneHashMap;
        [FieldOffset(0x20)]
        public CrBoneData* BoneData;
        [FieldOffset(0x5E)]
        public ushort BoneCount;

        /// <summary>
        /// Gets the bone index from specified bone id. Note that bone indexes are sequential values and bone ids are not sequential ones.
        /// </summary>
        public int GetBoneIndexByBoneId(int boneId)
        {
            if (BoneHashMap.ElementCount == 0)
            {
                if (boneId < BoneCount)
                {
                    return boneId;
                }

                return -1;
            }

            if (BoneHashMap.BucketCount == 0)
            {
                return -1;
            }

            if (BoneHashMap.Get((uint)boneId, out int returnBoneId))
            {
                return returnBoneId;
            }

            return -1;
        }

        /// <summary>
        /// Gets the bone id from specified bone index. Note that bone indexes are sequential values and bone ids are not sequential ones.
        /// </summary>
        public int GetBoneIdByIndex(int boneIndex)
        {
            if (boneIndex < 0 || boneIndex >= BoneCount)
            {
                return -1;
            }

            return ((CrBoneData*)((ulong)BoneData + (uint)sizeof(CrBoneData) * (uint)boneIndex))->BoneId;
        }

        /// <summary>
        /// Gets the next sibling bone index of specified bone index.
        /// </summary>
        public void GetNextSiblingBoneIndexAndId(int boneIndex, out int nextSiblingBoneIndex, out int nextSiblingBoneId)
        {
            if (boneIndex < 0 || boneIndex >= BoneCount)
            {
                nextSiblingBoneIndex = -1;
                nextSiblingBoneId = -1;
                return;
            }

            var crBoneData = ((CrBoneData*)((ulong)BoneData + (uint)sizeof(CrBoneData) * (uint)boneIndex));
            ushort nextSiblingBoneIndexFetched = crBoneData->NextSiblingBoneIndex;
            if (nextSiblingBoneIndexFetched == 0xFFFF)
            {
                nextSiblingBoneIndex = -1;
                nextSiblingBoneId = -1;
                return;
            }

            int nextSiblingBoneIdFetched = GetBoneIdByIndex(nextSiblingBoneIndexFetched);
            if (nextSiblingBoneIndexFetched == 0xFFFF)
            {
                nextSiblingBoneIndex = -1;
                nextSiblingBoneId = -1;
                return;
            }

            nextSiblingBoneIndex = nextSiblingBoneIndexFetched;
            nextSiblingBoneId = nextSiblingBoneIdFetched;
        }

        /// <summary>
        /// Gets the next parent bone index of specified bone index.
        /// </summary>
        public void GetParentBoneIndexAndId(int boneIndex, out int parentBoneIndex, out int parentBoneId)
        {
            if (boneIndex < 0 || boneIndex >= BoneCount)
            {
                parentBoneIndex = -1;
                parentBoneId = -1;
                return;
            }

            var crBoneData = ((CrBoneData*)((ulong)BoneData + (uint)sizeof(CrBoneData) * (uint)boneIndex));
            ushort nextParentBoneIndexFetched = crBoneData->ParentBoneIndex;
            if (nextParentBoneIndexFetched == 0xFFFF)
            {
                parentBoneIndex = -1;
                parentBoneId = -1;
                return;
            }

            int nextParentBoneIdFetched = GetBoneIdByIndex(nextParentBoneIndexFetched);
            if (nextParentBoneIdFetched == 0xFFFF)
            {
                parentBoneIndex = -1;
                parentBoneId = -1;
                return;
            }

            parentBoneIndex = nextParentBoneIndexFetched;
            parentBoneId = nextParentBoneIdFetched;
        }

        /// <summary>
        /// Gets the bone name string from specified bone index.
        /// </summary>
        public string GetBoneName(int boneIndex)
        {
            if (boneIndex < 0 || boneIndex >= BoneCount)
            {
                return null;
            }

            return ((CrBoneData*)((ulong)BoneData + (uint)sizeof(CrBoneData) * (uint)boneIndex))->Name;
        }
    }
}
