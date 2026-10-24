using System;
using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct YscScriptHeader
    {
        [FieldOffset(0x10)]
        public byte** CodeBlocksOffset;

        [FieldOffset(0x1C)]
        public int CodeLength;

        [FieldOffset(0x24)]
        public int LocalCount;

        [FieldOffset(0x2C)]
        public int NativeCount;

        [FieldOffset(0x30)]
        public long* LocalOffset;

        [FieldOffset(0x40)]
        public long* NativeOffset;

        [FieldOffset(0x58)]
        public int NameHash;

        public int CodePageCount()
        {
            return (CodeLength + 0x3FFF) >> 14;
        }

        public int GetCodePageSize(int page)
        {
            return (page < 0 || page >= CodePageCount() ? 0 : (page == CodePageCount() - 1) ? CodeLength & 0x3FFF : 0x4000);
        }

        public IntPtr GetCodePageAddress(int page)
        {
            return new IntPtr(CodeBlocksOffset[page]);
        }
    }
}
