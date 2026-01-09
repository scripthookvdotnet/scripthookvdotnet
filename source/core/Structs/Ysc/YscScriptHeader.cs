using System;
using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct YscScriptHeader
    {
        [FieldOffset(0x10)]
        internal byte** CodeBlocksOffset;
        [FieldOffset(0x1C)]
        internal int CodeLength;
        [FieldOffset(0x24)]
        internal int LocalCount;
        [FieldOffset(0x2C)]
        internal int NativeCount;
        [FieldOffset(0x30)]
        internal long* LocalOffset;
        [FieldOffset(0x40)]
        internal long* NativeOffset;
        [FieldOffset(0x58)]
        internal int NameHash;

        internal int CodePageCount()
        {
            return (CodeLength + 0x3FFF) >> 14;
        }

        internal int GetCodePageSize(int page)
        {
            return (page < 0 || page >= CodePageCount() ? 0 : (page == CodePageCount() - 1) ? CodeLength & 0x3FFF : 0x4000);
        }

        internal IntPtr GetCodePageAddress(int page)
        {
            return new IntPtr(CodeBlocksOffset[page]);
        }
    }
}
