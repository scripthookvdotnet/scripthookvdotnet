using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace SHVDN
{
    // Note: actually this struct is supposed to point the same struct type as `FwBasePool` in this source code
    // file, but needs to be careful when refactoring.
    [StructLayout(LayoutKind.Explicit)]
    internal struct FwScriptGuidPool
    {
        // The max count value should be at least 3072 as long as ScriptHookV is installed.
        // Without ScriptHookV, the default value is hardcoded and may be different between different game versions (the value is 300 in b372 and 700 in b2824).
        // The default value (when running without ScriptHookV) can be found by searching the dumped exe or the game memory with "D7 A8 11 73" (0x7311A8D7).
        [FieldOffset(0x10)]
        public uint MaxCount;

        [FieldOffset(0x14)]
        public int ItemSize;

        [FieldOffset(0x18)]
        public int FirstEmptySlot;

        [FieldOffset(0x1C)]
        public int EmptySlots;

        [FieldOffset(0x20)]
        public uint ItemCount;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull()
        {
            return MaxCount - (ItemCount & 0x3FFFFFFF) <= 256;
        }
    }
}
