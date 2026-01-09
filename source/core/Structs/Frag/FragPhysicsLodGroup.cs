using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct FragPhysicsLodGroup
    {
        [FieldOffset(0x10)]
        internal fixed ulong FragPhysicsLODAddresses[3];

        internal FragPhysicsLod* GetFragPhysicsLodByIndex(int index) => (FragPhysicsLod*)((ulong*)FragPhysicsLODAddresses[index]);
    }
}
