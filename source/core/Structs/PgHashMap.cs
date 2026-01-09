using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct PgHashMap
    {
        [FieldOffset(0x0)]
        internal ulong* Buckets;
        [FieldOffset(0x8)]
        internal ushort BucketCount;
        [FieldOffset(0xA)]
        internal ushort ElementCount;

        internal ulong GetBucketAddress(int index)
        {
            return Buckets[index];
        }

        internal bool Get(uint hash, out int value)
        {
            ulong* firstEntryAddr = (ulong*)GetBucketAddress((int)(hash % BucketCount));
            for (var hashEntry = (HashEntry*)firstEntryAddr; hashEntry != null; hashEntry = hashEntry->Next)
            {
                if (hash != hashEntry->Hash)
                {
                    continue;
                }

                value = hashEntry->Data;
                return true;
            }

            value = default;
            return false;
        }
    }
}
