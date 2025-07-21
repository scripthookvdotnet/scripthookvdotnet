//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    /// <summary>
    /// An enumeration of game versions.
    /// </summary>
    /// <remarks>
    /// The values of this enum <see cref="GameVersion"/> are defined only for convenience.
    /// Since the value of this enum <see cref="GameVersion"/> match what Script Hook V's `<c>getGameVersion()</c>`
    /// returns, you can still compare <see cref="Game.Version"/> with a value the enum <see cref="GameVersion"/> does
    /// not define. For example, even if <see cref="v1_0_2060_0_Steam"/> was not defined, you could still compare
    /// <see cref="Game.Version"/> with the internal value of <see cref="v1_0_2060_0_Steam"/>, which is <c>59</c>.
    /// </remarks>
    public enum GameVersion
    {
        Unknown = -1,

        v1_0_335_2_Steam,
        v1_0_335_2_NoSteam,
        v1_0_350_1_Steam,
        v1_0_350_2_NoSteam,
        v1_0_372_2_Steam,
        v1_0_372_2_NoSteam,
        v1_0_393_2_Steam,
        v1_0_393_2_NoSteam,
        v1_0_393_4_Steam,
        v1_0_393_4_NoSteam,
        v1_0_463_1_Steam,
        v1_0_463_1_NoSteam,
        v1_0_505_2_Steam,
        v1_0_505_2_NoSteam,
        v1_0_573_1_Steam,
        v1_0_573_1_NoSteam,
        v1_0_617_1_Steam,
        v1_0_617_1_NoSteam,
        v1_0_678_1_Steam,
        v1_0_678_1_NoSteam,
        v1_0_757_2_Steam,
        v1_0_757_2_NoSteam,
        /// <remarks>
        /// The correct name would be <c>v1_0_757_4_Steam</c> since the Steam v1.0.757.3 does not exist but Steam
        /// v1.0.757.4 does. Script Hook V can log <c>VER_1_0_757_4_STEAM</c> in <c>ScriptHookV.log</c> but cannot
        /// log <c>VER_1_0_757_3_STEAM</c> as well.
        /// </remarks>
        v1_0_757_3_Steam,
        v1_0_757_4_NoSteam,
        v1_0_791_2_Steam,
        v1_0_791_2_NoSteam,
        v1_0_877_1_Steam,
        v1_0_877_1_NoSteam,
        v1_0_944_2_Steam,
        v1_0_944_2_NoSteam,
        v1_0_1011_1_Steam,
        v1_0_1011_1_NoSteam,
        v1_0_1032_1_Steam,
        v1_0_1032_1_NoSteam,
        v1_0_1103_2_Steam,
        v1_0_1103_2_NoSteam,
        v1_0_1180_2_Steam,
        v1_0_1180_2_NoSteam,
        v1_0_1290_1_Steam,
        v1_0_1290_1_NoSteam,
        v1_0_1365_1_Steam,
        v1_0_1365_1_NoSteam,
        v1_0_1493_0_Steam,
        v1_0_1493_0_NoSteam,
        v1_0_1493_1_Steam,
        v1_0_1493_1_NoSteam,
        v1_0_1604_0_Steam,
        v1_0_1604_0_NoSteam,
        v1_0_1604_1_Steam,
        v1_0_1604_1_NoSteam,
        /// <summary>
        /// This value also represents the exe version 1.0.1734.0 for Steam version. 1.0.1737.0 basically works in the same way as 1.0.1734.0 but with bug fixes.
        /// </summary>
        v1_0_1737_0_Steam,
        /// <summary>
        /// This value also represents the exe version 1.0.1734.0 for non-Steam version. 1.0.1737.0 basically works in the same way as 1.0.1734.0 but with bug fixes.
        /// </summary>
        v1_0_1737_0_NoSteam,
        v1_0_1737_6_Steam,
        v1_0_1737_6_NoSteam,
        v1_0_1868_0_Steam,
        v1_0_1868_0_NoSteam,
        v1_0_1868_1_Steam,
        v1_0_1868_1_NoSteam,
        v1_0_1868_4_EGS,
        v1_0_2060_0_Steam,
        v1_0_2060_0_NoSteam,
        v1_0_2060_1_Steam,
        v1_0_2060_1_NoSteam,
        v1_0_2189_0_Steam,
        v1_0_2189_0_NoSteam,
        v1_0_2215_0_Steam,
        v1_0_2215_0_NoSteam,
        v1_0_2245_0_Steam,
        v1_0_2245_0_NoSteam,
        /// <summary>
        /// This value also represents the exe version 1.0.2372.2 for Steam version. 1.0.2372.2 basically works in the same way as 1.0.2372.0 but with bug fixes.
        /// </summary>
        v1_0_2372_0_Steam,
        /// <summary>
        /// This value also represents the exe version 1.0.2372.2 for non-Steam version. 1.0.2372.2 basically works in the same way as 1.0.2372.0 but with bug fixes.
        /// </summary>
        v1_0_2372_0_NoSteam,
        v1_0_2545_0_Steam,
        v1_0_2545_0_NoSteam,
        v1_0_2612_1_Steam,
        v1_0_2612_1_NoSteam,
        v1_0_2628_2_Steam,
        v1_0_2628_2_NoSteam,
        v1_0_2699_0_Steam,
        v1_0_2699_0_NoSteam,
        v1_0_2699_16,
        v1_0_2802_0,
        v1_0_2824_0,
        v1_0_2845_0,
        v1_0_2944_0,
        v1_0_3028_0,
        v1_0_3095_0,
        v1_0_3179_0,
        v1_0_3258_0,
        v1_0_3274_0,
        v1_0_3323_0,
        v1_0_3337_0,
        v1_0_3351_0,
        V1_0_3407_0,
        V1_0_3411_0,
        V1_0_3442_0,
        V1_0_3504_0,
        V1_0_3521_0,
        V1_0_3570_0,
    }
}
