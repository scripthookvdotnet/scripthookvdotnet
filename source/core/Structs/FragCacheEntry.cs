using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct FragCacheEntry
    {
        [FieldOffset(0x178)]
        internal CrSkeleton* crSkeleton;
    }
}
