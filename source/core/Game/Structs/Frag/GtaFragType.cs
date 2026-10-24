//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct GtaFragType
    {
        [FieldOffset(0x30)]
        public FragDrawable* FragDrawable;

        [FieldOffset(0xF0)]
        public FragPhysicsLodGroup* FragPhysicsLODGroup;
    }
}
