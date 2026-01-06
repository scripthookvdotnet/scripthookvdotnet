using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0xC0)]
    internal unsafe struct FragInst
    {
        [FieldOffset(0x68)]
        internal FragCacheEntry* fragCacheEntry;
        [FieldOffset(0x78)]
        internal GtaFragType* gtaFragType;
        [FieldOffset(0xB8)]
        internal uint unkType;

        internal FragPhysicsLod* GetAppropriateFragPhysicsLod()
        {
            FragPhysicsLodGroup* fragPhysicsLodGroup = gtaFragType->fragPhysicsLODGroup;
            if (fragPhysicsLodGroup == null)
            {
                return null;
            }
            switch (unkType)
            {
                case 0:
                case 1:
                case 2:
                    return fragPhysicsLodGroup->GetFragPhysicsLodByIndex((int) unkType);
                default:
                    return fragPhysicsLodGroup->GetFragPhysicsLodByIndex(0);
            }
        }
    }
}
