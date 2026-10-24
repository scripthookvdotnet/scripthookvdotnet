using System;

namespace SHVDN
{
    [Flags]
    public enum VehicleModelInfoFlags : ulong
    {
        IsTank = 1 << 9,
        HasBulletProofGlass = 1 << 12,
        HasLowriderHydraulics = 1 << 55,
        HasLowriderDonkHydraulics = 1 << 59,
    }
}
