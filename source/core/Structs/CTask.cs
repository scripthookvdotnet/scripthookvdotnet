using System;
using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x38)]
    internal struct CTask
    {
        [FieldOffset(0x34)]
        internal ushort taskTypeIndex;
    }
}
