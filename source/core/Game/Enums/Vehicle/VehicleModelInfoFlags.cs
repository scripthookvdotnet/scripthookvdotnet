//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

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
