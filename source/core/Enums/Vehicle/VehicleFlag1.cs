using System;

namespace SHVDN
{
    [Flags]
    public enum VehicleFlag1 : ulong
    {
        Big = 0x2,
        IsVan = 0x20,
        CanStandOnTop = 0x10000000,
        LawEnforcement = 0x80000000,
        EmergencyService = 0x100000000,
        AllowsRappel = 0x8000000000,
        IsElectric = 0x80000000000,
        IsOffroadVehicle = 0x1000000000000,
        IsBus = 0x400000000000000,
    }
}
