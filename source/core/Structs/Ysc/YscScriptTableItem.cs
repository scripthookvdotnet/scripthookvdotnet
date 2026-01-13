using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct YscScriptTableItem
    {
        [FieldOffset(0x0)]
        public YscScriptHeader* Header;

        [FieldOffset(0xC)]
        public int Hash;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsLoaded()
        {
            return Header != null;
        }
    }
}
