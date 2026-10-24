//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct YscScriptTableItem
    {
        [FieldOffset(0x0)]
        public YscScriptHeader* Header;

        [FieldOffset(0xC)]
        public int Hash;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsLoaded()
        {
            return Header != null;
        }
    }
}
