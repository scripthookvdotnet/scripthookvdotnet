//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    public enum CameraShake
    {
        Hand,
        SmallExplosion,
        MediumExplosion,
        LargeExplosion,
        Jolt,
        Vibrate,
        RoadVibration,
        Drunk,
        SkyDiving,
        FamilyDrugTrip,
        DeathFail,
        Wobbly,
    }

    internal static class CameraShakeExtensions
    {
        internal static string GetName(this CameraShake value) => CameraShakeHelpers.GetName(value);
    }

    internal static class CameraShakeHelpers
    {
        private static readonly string[] s_shakeNames =
        {
            "HAND_SHAKE",
            "SMALL_EXPLOSION_SHAKE",
            "MEDIUM_EXPLOSION_SHAKE",
            "LARGE_EXPLOSION_SHAKE",
            "JOLT_SHAKE",
            "VIBRATE_SHAKE",
            "ROAD_VIBRATION_SHAKE",
            "DRUNK_SHAKE",
            "SKY_DIVING_SHAKE",
            "FAMILY5_DRUG_TRIP_SHAKE",
            "DEATH_FAIL_IN_EFFECT_SHAKE",
            "WOBBLY_SHAKE",
        };

        internal static int s_shakeCount = s_shakeNames.Length;

        internal static string GetName(CameraShake shake) => s_shakeNames[(int)shake];
    }
}
