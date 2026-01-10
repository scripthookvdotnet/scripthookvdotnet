using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct CGameScriptResource
    {
        [FieldOffset(0x0)]
        public ulong* vTable;

        [FieldOffset(0x8)]
        public ScriptResourceType ResourceTypeNameIndex;

        [FieldOffset(0xC)]
        public uint CounterOfPool;

        [FieldOffset(0x10)]
        public uint IndexOfPool;

        [FieldOffset(0x18)]
        public CGameScriptResource* Next;

        [FieldOffset(0x20)]
        public CGameScriptResource* Prev;
    }
}
