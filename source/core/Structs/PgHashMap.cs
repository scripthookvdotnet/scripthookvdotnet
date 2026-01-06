using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct PgHashMap
    {
        [FieldOffset(0x0)]
        internal ulong* buckets;
        [FieldOffset(0x8)]
        internal ushort bucketCount;
        [FieldOffset(0xA)]
        internal ushort elementCount;

        internal ulong GetBucketAddress(int index)
        {
            return buckets[index];
        }

        internal bool Get(uint hash, out int value)
        {
            ulong* firstEntryAddr = (ulong*)GetBucketAddress((int)(hash % bucketCount));
            for (var hashEntry = (HashEntry*)firstEntryAddr; hashEntry != null; hashEntry = hashEntry->next)
            {
                if (hash != hashEntry->hash)
                {
                    continue;
                }

                value = hashEntry->data;
                return true;
            }

            value = default;
            return false;
        }
    }
}
