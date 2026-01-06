using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct GtaFragType
    {
        [FieldOffset(0x30)]
        internal FragDrawable* fragDrawable;
        [FieldOffset(0xF0)]
        internal FragPhysicsLodGroup* fragPhysicsLODGroup;
    }
}
