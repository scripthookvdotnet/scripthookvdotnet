//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Linq;

namespace GTA
{
    public enum Weather
    {
        Unknown = -1,
        ExtraSunny,
        Clear,
        Clouds,
        Smog,
        Foggy,
        Overcast,
        Raining,
        ThunderStorm,
        Clearing,
        Neutral,
        Snowing,
        Blizzard,
        Snowlight,
        Christmas,
        Halloween,
    }

    /// <summary>
    /// Provides <see langword="internal"/> extension methods for <see cref="Weather"/>.
    /// </summary>
    internal static class WeatherExtensions
    {
        /// <summary>
        /// Returns the internal name of this <see cref="Weather"/>.
        /// </summary>
        /// <param name="weather">The weather to resolve.</param>
        /// <returns>The corresponding internal name.</returns>
        internal static string GetInternalName(this Weather weather) => WeatherHelpers.GetInternalName(weather);

        /// <summary>
        /// Returns the hash of this <see cref="Weather"/>.
        /// </summary>
        /// <param name="weather">The weather to resolve.</param>
        /// <returns>The corresponding <see cref="StringHash.AtStringHash(string, uint)"/></returns>
        internal static uint GetNameHash(this Weather weather) => WeatherHelpers.GetNameHash(weather);
    }

    /// <summary>
    /// Provides <see langword="internal"/> helpers for <see cref="Weather"/>.
    /// </summary>
    internal static class WeatherHelpers
    {
        private static readonly string[] s_weatherNames = {
            "EXTRASUNNY",
            "CLEAR",
            "CLOUDS",
            "SMOG",
            "FOGGY",
            "OVERCAST",
            "RAIN",
            "THUNDER",
            "CLEARING",
            "NEUTRAL",
            "SNOW",
            "BLIZZARD",
            "SNOWLIGHT",
            "XMAS",
            "HALLOWEEN"
        };

        private static readonly uint[] s_weatherHashes = s_weatherNames
            .Select(name => StringHash.AtStringHash(name))
            .ToArray();


        /// <summary>
        /// Gets the count of known <see cref="Weather"/> types.
        /// </summary>
        internal static readonly int s_weatherCount = s_weatherNames.Length;

        /// <summary>
        /// Returns the internal name for the specified <see cref="Weather"/>.
        /// </summary>
        /// <param name="weather">The weather to resolve.</param>
        /// <returns>The corresponding internal name.</returns>
        internal static string GetInternalName(Weather weather) => s_weatherNames[(int)weather];

        /// <inheritdoc cref="GetInternalName(Weather)"/>
        internal static string GetInternalName(int weather) => s_weatherNames[weather];

        /// <summary>
        /// Returns the hash for the specified <see cref="Weather"/>.
        /// </summary>
        /// <param name="weather">The weather to resolve.</param>
        /// <returns>The corresponding <see cref="StringHash.AtStringHash(string, uint)"/></returns>
        internal static uint GetNameHash(Weather weather) => s_weatherHashes[(int)weather];

        /// <inheritdoc cref="GetNameHash(Weather)"/>
        internal static uint GetNameHash(int weather) => s_weatherHashes[weather];
    }
}
