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

    /// <summary>
    /// Provides  <see langword="internal"/> extension methods for <see cref="CameraShake"/>.
    /// </summary>
    internal static class CameraShakeExtensions
    {
        /// <summary>
        /// Returns the name of this <see cref="CameraShake"/> aligned to the game build.
        /// </summary>
        /// <param name="value">The <see cref="CameraShake"/> to correct.</param>
        /// <returns>The corrected integer value based on game build.</returns>
        internal static string GetInternalName(this CameraShake value) => CameraShakeHelpers.GetInternalName(value);
    }

    /// <summary>
    /// Provides <see langword="internal"/> helpers for <see cref="CameraShake"/>.
    /// </summary>
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

        /// <summary>
        /// Gets the count of known <see cref="CameraShake"/>s.
        /// </summary>
        internal static readonly int s_shakeCount = s_shakeNames.Length;

        /// <summary>
        /// Returns the name of a <see cref="CameraShake"/> aligned to the game build.
        /// </summary>
        /// <param name="shake">The <see cref="CameraShake"/> to correct.</param>
        /// <returns>The corrected integer value based on game build.</returns>
        internal static string GetInternalName(CameraShake shake) => s_shakeNames[(int)shake];
    }
}
