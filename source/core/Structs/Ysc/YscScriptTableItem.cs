using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct YscScriptTableItem
    {
        [FieldOffset(0x0)]
        internal YscScriptHeader* header;
        [FieldOffset(0xC)]
        internal int hash;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsLoaded()
        {
            return header != null;
        }
    }
}
