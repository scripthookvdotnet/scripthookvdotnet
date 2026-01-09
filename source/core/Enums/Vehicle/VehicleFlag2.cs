using System;

namespace SHVDN
{
    [Flags]
    public enum VehicleFlag2 : ulong
    {
        IsTank = 0x200,
        HasBulletProofGlass = 0x1000,
        HasLowriderHydraulics = 0x80000000000000,
        HasLowriderDonkHydraulics = 0x800000000000000,
    }
}
