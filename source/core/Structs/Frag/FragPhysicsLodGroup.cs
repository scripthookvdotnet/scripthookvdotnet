using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct FragPhysicsLodGroup
    {
        [FieldOffset(0x10)]
        internal fixed ulong fragPhysicsLODAddresses[3];

        internal FragPhysicsLod* GetFragPhysicsLodByIndex(int index) => (FragPhysicsLod*)((ulong*)fragPhysicsLODAddresses[index]);
    }
}
