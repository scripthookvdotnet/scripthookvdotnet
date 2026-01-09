using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct CGameScriptResource
    {
        [FieldOffset(0x0)]
        internal ulong* vTable;
        [FieldOffset(0x8)]
        internal CScriptResourceTypeNameIndex ResourceTypeNameIndex;
        [FieldOffset(0xC)]
        internal uint CounterOfPool;
        [FieldOffset(0x10)]
        internal uint IndexOfPool;
        [FieldOffset(0x18)]
        internal CGameScriptResource* Next;
        [FieldOffset(0x20)]
        internal CGameScriptResource* Prev;
    }
}
