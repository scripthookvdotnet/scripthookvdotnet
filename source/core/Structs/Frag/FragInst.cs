using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0xC0)]
    internal unsafe struct FragInst
    {
        [FieldOffset(0x68)]
        internal FragCacheEntry* FragCacheEntry;
        [FieldOffset(0x78)]
        internal GtaFragType* GtaFragType;
        [FieldOffset(0xB8)]
        internal uint UnkType;

        internal FragPhysicsLod* GetAppropriateFragPhysicsLod()
        {
            FragPhysicsLodGroup* fragPhysicsLodGroup = GtaFragType->FragPhysicsLODGroup;
            if (fragPhysicsLodGroup == null)
            {
                return null;
            }
            switch (UnkType)
            {
                case 0:
                case 1:
                case 2:
                    return fragPhysicsLodGroup->GetFragPhysicsLodByIndex((int)UnkType);
                default:
                    return fragPhysicsLodGroup->GetFragPhysicsLodByIndex(0);
            }
        }
    }
}
