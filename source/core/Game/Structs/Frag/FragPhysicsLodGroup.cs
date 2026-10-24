using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct FragPhysicsLodGroup
    {
        [FieldOffset(0x10)]
        private fixed ulong _fragPhysicsLODAddresses[3];

        public FragPhysicsLod* GetFragPhysicsLodByIndex(int index) => (FragPhysicsLod*)((ulong*)_fragPhysicsLODAddresses[index]);
    }
}
