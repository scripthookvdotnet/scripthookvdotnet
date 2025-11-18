//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    public enum VehicleModType
    {
        /// <summary>
        /// Intended to use with <see cref="VehicleWeaponHandlingData.WeaponVehicleModType"/>, not in <see cref="VehicleMod"/> or <see cref="VehicleModCollection"/>.
        /// </summary>
        None = -1,
        Spoilers,
        FrontBumper,
        RearBumper,
        SideSkirt,
        Exhaust,
        Frame,
        Grille,
        Hood,
        Fender,
        RightFender,
        Roof,
        Engine,
        Brakes,
        Transmission,
        Horns,
        Suspension,
        Armor,
        FrontWheel = 23,
        RearWheel,
        PlateHolder,
        VanityPlates,
        TrimDesign,
        Ornaments,
        Dashboard,
        DialDesign,
        DoorSpeakers,
        Seats,
        SteeringWheels,
        ColumnShifterLevers,
        Plaques,
        Speakers,
        Trunk,
        Hydraulics,
        EngineBlock,
        AirFilter,
        Struts,
        ArchCover,
        Aerials,
        Trim,
        Tank,
        Windows,
        Livery = 48
    }

    /// <summary>
    /// Non-public extension class for <see cref="VehicleModType"/>.
    /// </summary>
    internal static class VehicleModTypeExtensions
    {
        /// <summary>
        /// Returns the value of this <see cref="VehicleModType"/> aligned to the game build.
        /// </summary>
        /// <param name="modType">The <see cref="VehicleModType"/> to correct.</param>
        /// <returns>The corrected integer value based on game version.</returns>
        internal static int GetInternalValue(this VehicleModType modType) => VehicleModTypeHelpers.GetInternalValue(modType);
    }

    internal static class VehicleModTypeHelpers
    {
        internal static int GetInternalValue(VehicleModType modType)
        {
            // This kind of correction was introduced in b393 so return the same value if the game version is earlier than b393
            if (Game.FileVersion < VersionConstsForGameVersion.v1_0_393_2)
            {
                return (int)modType;
            }

            int valueAsInt = (int)modType;

            if (valueAsInt > 24)
            {
                return valueAsInt - 14;
            }

            if (valueAsInt > 10)
            {
                return valueAsInt + 25;
            }

            return valueAsInt;
        }

        internal static VehicleModType FromInternalValue(int value)
        {
            if (Game.FileVersion < VersionConstsForGameVersion.v1_0_393_2)
            {
                return (VehicleModType)value;
            }

            if (value is > 35 and < 50)
            {
                return (VehicleModType)(value - 25);
            }

            if (value is > 10 and < 36)
            {
                return (VehicleModType)(value + 14);
            }

            return (VehicleModType)value;
        }
    }
}
