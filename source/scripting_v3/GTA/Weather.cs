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

    internal static class WeatherExtensions
    {
        internal static string GetName(this Weather weather) => WeatherHelpers.GetName(weather);
        internal static uint GetHash(this Weather weather) => WeatherHelpers.GetHash(weather);
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

        internal static string GetName(Weather weather) => s_weatherNames[(int)weather];
        internal static string GetName(int weather) => s_weatherNames[weather];

        internal static uint GetHash(Weather weather) => s_weatherHashes[(int)weather];
        internal static uint GetHash(int weather) => s_weatherHashes[weather];
    }
}
