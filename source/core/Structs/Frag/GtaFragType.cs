using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct GtaFragType
    {
        [FieldOffset(0x30)]
        public FragDrawable* FragDrawable;

        [FieldOffset(0xF0)]
        public FragPhysicsLodGroup* FragPhysicsLODGroup;
    }
}
