//
// Copyright (C) 2025 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using GTA.Native;

namespace GTA
{
    /// <summary>
    /// A static collection of dispatch data. This "dispatch" includes but not limited to those for responses of
    /// law enforcements, ambulances, and fires.
    /// </summary>
    public static class DispatchData
    {
        private const int MinWantedLevel = 0;
        private const int MaxWantedLevel = 5;

        /// <summary>
        /// Gets the wanted radius for a specified wanted level. The raduis value can affect the difficulty level of
        /// <see cref="Wanted"/> by the distance between the last spotted position and the current player position, and
        /// by the wanted radius value.
        /// </summary>
        /// <param name="wantedLevel">The wanted level to retrieve the wanted radius.</param>
        /// <returns>The wanted level radius.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="wantedLevel"/> is not between 0 and 5, inclusive.
        /// </exception>
        public static float GetWantedLevelRadius(int wantedLevel)
        {
            // The native doesn't test if the arg is in the value range in final builds and the native indirectly
            // accesses an array of the singleton `CDispatchData` out of bounds if the arg is out of the value range,
            // so we need to test that instead.
            ThrowHelper.CheckArgumentRange(nameof(wantedLevel), wantedLevel, MinWantedLevel, MaxWantedLevel);
            return Function.Call<float>(Hash.GET_WANTED_LEVEL_RADIUS, wantedLevel);
        }

        /// <summary>
        /// Gets the minimum threshold of a specified wanted level at which <see cref="Wanted.CurrentCrimeValue"/> must
        /// be or above so the <see cref="Player"/> will have said wanted level or greater one.
        /// </summary>
        /// <param name="wantedLevel">The wanted level to retrieve the minimum threshold.</param>
        /// <returns>
        /// The minimum threshold of the specified wanted level (the same unit as
        /// <see cref="Wanted.CurrentCrimeValue"/>).
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="wantedLevel"/> is not between 0 and 5, inclusive.
        /// </exception>
        public static int GetWantedLevelThreshold(int wantedLevel)
        {
            // The native doesn't test if the arg is in the value range in final builds and the native indirectly
            // accesses an array of the singleton `CDispatchData` out of bounds if the arg is out of the value range,
            // so we need to test that instead.
            ThrowHelper.CheckArgumentRange(nameof(wantedLevel), wantedLevel, MinWantedLevel, MaxWantedLevel);
            return Function.Call<int>(Hash.GET_WANTED_LEVEL_THRESHOLD, wantedLevel);
        }

        // `GET_WANTED_LEVEL_TIME_TO_ESCAPE` gets a parole value from `CDispatchData::GetParoleDuration()`. However,
        // the parole duration is used only when `CDispatchData::sm_bEnableRadiusEvasion` is `true`, which can't be
        // `true` in final builds. The variable can be set to `true` only by `CWanted::InitWidgets` (or direct memory
        // editing).
    }
}
