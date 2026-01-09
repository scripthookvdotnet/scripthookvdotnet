using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct YscScriptTableItem
    {
        [FieldOffset(0x0)]
        internal YscScriptHeader* Header;
        [FieldOffset(0xC)]
        internal int Hash;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsLoaded()
        {
            return Header != null;
        }
    }
}
