using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct CrSkeletonData
    {
        [FieldOffset(0x10)]
        internal PgHashMap boneHashMap;
        [FieldOffset(0x20)]
        internal CrBoneData* boneData;
        [FieldOffset(0x5E)]
        internal ushort boneCount;

        /// <summary>
        /// Gets the bone index from specified bone id. Note that bone indexes are sequential values and bone ids are not sequential ones.
        /// </summary>
        public int GetBoneIndexByBoneId(int boneId)
        {
            if (boneHashMap.elementCount == 0)
            {
                if (boneId < boneCount)
                {
                    return boneId;
                }

                return -1;
            }

            if (boneHashMap.bucketCount == 0)
            {
                return -1;
            }

            if (boneHashMap.Get((uint)boneId, out int returnBoneId))
            {
                return returnBoneId;
            }

            return -1;
        }

        /// <summary>
        /// Gets the bone id from specified bone index. Note that bone indexes are sequential values and bone ids are not sequential ones.
        /// </summary>
        internal int GetBoneIdByIndex(int boneIndex)
        {
            if (boneIndex < 0 || boneIndex >= boneCount)
            {
                return -1;
            }

            return ((CrBoneData*)((ulong)boneData + (uint)sizeof(CrBoneData) * (uint)boneIndex))->boneId;
        }

        /// <summary>
        /// Gets the next sibling bone index of specified bone index.
        /// </summary>
        internal void GetNextSiblingBoneIndexAndId(int boneIndex, out int nextSiblingBoneIndex, out int nextSiblingBoneId)
        {
            if (boneIndex < 0 || boneIndex >= boneCount)
            {
                nextSiblingBoneIndex = -1;
                nextSiblingBoneId = -1;
                return;
            }

            var crBoneData = ((CrBoneData*)((ulong)boneData + (uint)sizeof(CrBoneData) * (uint)boneIndex));
            ushort nextSiblingBoneIndexFetched = crBoneData->nextSiblingBoneIndex;
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
        internal void GetParentBoneIndexAndId(int boneIndex, out int parentBoneIndex, out int parentBoneId)
        {
            if (boneIndex < 0 || boneIndex >= boneCount)
            {
                parentBoneIndex = -1;
                parentBoneId = -1;
                return;
            }

            var crBoneData = ((CrBoneData*)((ulong)boneData + (uint)sizeof(CrBoneData) * (uint)boneIndex));
            ushort nextParentBoneIndexFetched = crBoneData->parentBoneIndex;
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
        internal string GetBoneName(int boneIndex)
        {
            if (boneIndex < 0 || boneIndex >= boneCount)
            {
                return null;
            }

            return ((CrBoneData*)((ulong)boneData + (uint)sizeof(CrBoneData) * (uint)boneIndex))->Name;
        }
    }
}
