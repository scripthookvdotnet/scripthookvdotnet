using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0xC0)]
    internal unsafe struct FragInst
    {
        [FieldOffset(0x68)]
        public FragCacheEntry* FragCacheEntry;

        [FieldOffset(0x78)]
        public GtaFragType* GtaFragType;

        [FieldOffset(0xB8)]
        public uint Guid;

        public FragPhysicsLod* GetAppropriateFragPhysicsLod()
        {
            FragPhysicsLodGroup* fragPhysicsLodGroup = GtaFragType->FragPhysicsLODGroup;
            if (fragPhysicsLodGroup == null)
            {
                return null;
            }
            switch (Guid)
            {
                case 0:
                case 1:
                case 2:
                    return fragPhysicsLodGroup->GetFragPhysicsLodByIndex((int)Guid);
                default:
                    return fragPhysicsLodGroup->GetFragPhysicsLodByIndex(0);
            }
        }
    }
}
