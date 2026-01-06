using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct CGameScriptResource
    {
        [FieldOffset(0x0)]
        internal ulong* vTable;
        [FieldOffset(0x8)]
        internal CScriptResourceTypeNameIndex resourceTypeNameIndex;
        [FieldOffset(0xC)]
        internal uint counterOfPool;
        [FieldOffset(0x10)]
        internal uint indexOfPool;
        [FieldOffset(0x18)]
        internal CGameScriptResource* next;
        [FieldOffset(0x20)]
        internal CGameScriptResource* prev;
    }
}
