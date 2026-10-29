//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct FragCacheEntry
    {
        [FieldOffset(0x178)]
        public CrSkeleton* InitialSkeleton;
    }
}
