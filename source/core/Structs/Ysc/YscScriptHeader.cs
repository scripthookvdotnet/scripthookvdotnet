using System;
using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct YscScriptHeader
    {
        [FieldOffset(0x10)]
        internal byte** codeBlocksOffset;
        [FieldOffset(0x1C)]
        internal int codeLength;
        [FieldOffset(0x24)]
        internal int localCount;
        [FieldOffset(0x2C)]
        internal int nativeCount;
        [FieldOffset(0x30)]
        internal long* localOffset;
        [FieldOffset(0x40)]
        internal long* nativeOffset;
        [FieldOffset(0x58)]
        internal int nameHash;

        internal int CodePageCount()
        {
            return (codeLength + 0x3FFF) >> 14;
        }

        internal int GetCodePageSize(int page)
        {
            return (page < 0 || page >= CodePageCount() ? 0 : (page == CodePageCount() - 1) ? codeLength & 0x3FFF : 0x4000);
        }

        internal IntPtr GetCodePageAddress(int page)
        {
            return new IntPtr(codeBlocksOffset[page]);
        }
    }
}
