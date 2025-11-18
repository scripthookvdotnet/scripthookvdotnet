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
    /// Non-public extension class for <see cref="Weather"/>.
    /// </summary>
    internal static class WeatherExtensions
    {
        internal static string GetInternalName(this Weather weather) => WeatherHelpers.GetInternalName(weather);
        internal static uint GetNameHash(this Weather weather) => WeatherHelpers.GetNameHash(weather);
    }

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

        internal static int s_weatherCount = s_weatherNames.Length;

        internal static string GetInternalName(Weather weather) => s_weatherNames[(int)weather];
        internal static string GetInternalName(int weather) => s_weatherNames[weather];

        internal static uint GetNameHash(Weather weather) => s_weatherHashes[(int)weather];
        internal static uint GetNameHash(int weather) => s_weatherHashes[weather];
    }
}
