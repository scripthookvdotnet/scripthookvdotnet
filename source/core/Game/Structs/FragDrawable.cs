//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct FragDrawable
    {
        [FieldOffset(0x18)]
        public CrSkeletonData* SkeletonData;
    }
}
