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
    /// returns plus 1, you can still compare <see cref="Game.Version"/> with a value the enum <see cref="GameVersion"/>
    /// does not define. For example, even if <see cref="VER_1_0_2060_0_STEAM"/> was not defined, you could still
    /// compare <see cref="Game.Version"/> with the internal value of <see cref="VER_1_0_2060_0_STEAM"/>, which is
    /// <c>60</c>.
    /// </remarks>
    public enum GameVersion
    {
        Unknown = 0,

        VER_1_0_335_2_STEAM,
        VER_1_0_335_2_NOSTEAM,
        VER_1_0_350_1_STEAM,
        VER_1_0_350_2_NOSTEAM,
        VER_1_0_372_2_STEAM,
        VER_1_0_372_2_NOSTEAM,
        VER_1_0_393_2_STEAM,
        VER_1_0_393_2_NOSTEAM,
        VER_1_0_393_4_STEAM,
        VER_1_0_393_4_NOSTEAM,
        VER_1_0_463_1_STEAM,
        VER_1_0_463_1_NOSTEAM,
        VER_1_0_505_2_STEAM,
        VER_1_0_505_2_NOSTEAM,
        VER_1_0_573_1_STEAM,
        VER_1_0_573_1_NOSTEAM,
        VER_1_0_617_1_STEAM,
        VER_1_0_617_1_NOSTEAM,
        VER_1_0_678_1_STEAM,
        VER_1_0_678_1_NOSTEAM,
        VER_1_0_757_2_STEAM,
        VER_1_0_757_2_NOSTEAM,
        /// <remarks>
        /// The correct name would be <c>VER_1_0_757_4_STEAM</c> since the Steam v1.0.757.3 does not exist but Steam
        /// v1.0.757.4 does. Script Hook V can log <c>VER_1_0_757_4_STEAM</c> in <c>ScriptHookV.log</c> but cannot
        /// log <c>VER_1_0_757_3_STEAM</c> as well.
        /// </remarks>
        VER_1_0_757_3_STEAM,
        VER_1_0_757_4_NOSTEAM,
        VER_1_0_791_2_STEAM,
        VER_1_0_791_2_NOSTEAM,
        VER_1_0_877_1_STEAM,
        VER_1_0_877_1_NOSTEAM,
        VER_1_0_944_2_STEAM,
        VER_1_0_944_2_NOSTEAM,
        VER_1_0_1011_1_STEAM,
        VER_1_0_1011_1_NOSTEAM,
        VER_1_0_1032_1_STEAM,
        VER_1_0_1032_1_NOSTEAM,
        VER_1_0_1103_2_STEAM,
        VER_1_0_1103_2_NOSTEAM,
        VER_1_0_1180_2_STEAM,
        VER_1_0_1180_2_NOSTEAM,
        VER_1_0_1290_1_STEAM,
        VER_1_0_1290_1_NOSTEAM,
        VER_1_0_1365_1_STEAM,
        VER_1_0_1365_1_NOSTEAM,
        VER_1_0_1493_0_STEAM,
        VER_1_0_1493_0_NOSTEAM,
        VER_1_0_1493_1_STEAM,
        VER_1_0_1493_1_NOSTEAM,
        VER_1_0_1604_0_STEAM,
        VER_1_0_1604_0_NOSTEAM,
        VER_1_0_1604_1_STEAM,
        VER_1_0_1604_1_NOSTEAM,
        /// <summary>
        /// This value also represents the exe version 1.0.1734.0 for Steam version. 1.0.1737.0 basically works in the same way as 1.0.1734.0 but with bug fixes.
        /// </summary>
        VER_1_0_1737_0_STEAM,
        /// <summary>
        /// This value also represents the exe version 1.0.1734.0 for non-Steam version. 1.0.1737.0 basically works in the same way as 1.0.1734.0 but with bug fixes.
        /// </summary>
        VER_1_0_1737_0_NOSTEAM,
        VER_1_0_1737_6_STEAM,
        VER_1_0_1737_6_NOSTEAM,
        VER_1_0_1868_0_STEAM,
        VER_1_0_1868_0_NOSTEAM,
        VER_1_0_1868_1_STEAM,
        VER_1_0_1868_1_NOSTEAM,
        VER_1_0_1868_4_EGS,
        VER_1_0_2060_0_STEAM,
        VER_1_0_2060_0_NOSTEAM,
        VER_1_0_2060_1_STEAM,
        VER_1_0_2060_1_NOSTEAM,
        VER_1_0_2189_0_STEAM,
        VER_1_0_2189_0_NOSTEAM,
        VER_1_0_2215_0_STEAM,
        VER_1_0_2215_0_NOSTEAM,
        VER_1_0_2245_0_STEAM,
        VER_1_0_2245_0_NOSTEAM,
        /// <summary>
        /// This value also represents the exe version 1.0.2372.2 for Steam version. 1.0.2372.2 basically works in the same way as 1.0.2372.0 but with bug fixes.
        /// </summary>
        VER_1_0_2372_0_STEAM,
        /// <summary>
        /// This value also represents the exe version 1.0.2372.2 for non-Steam version. 1.0.2372.2 basically works in the same way as 1.0.2372.0 but with bug fixes.
        /// </summary>
        VER_1_0_2372_0_NOSTEAM,
        VER_1_0_2545_0_STEAM,
        VER_1_0_2545_0_NOSTEAM,
        VER_1_0_2612_1_STEAM,
        VER_1_0_2612_1_NOSTEAM,
        VER_1_0_2628_2_STEAM,
        VER_1_0_2628_2_NOSTEAM,
        VER_1_0_2699_0_STEAM,
        VER_1_0_2699_0_NOSTEAM,
        VER_1_0_2699_16,
        VER_1_0_2802_0,
        VER_1_0_2824_0,
        VER_1_0_2845_0,
        VER_1_0_2944_0,
        VER_1_0_3028_0,
        VER_1_0_3095_0,
        VER_1_0_3179_0,
        VER_1_0_3258_0,
        VER_1_0_3274_0,
        VER_1_0_3323_0,
        VER_1_0_3337_0,
        VER_1_0_3351_0,
        VER_1_0_3407_0,
        VER_1_0_3411_0,
        VER_1_0_3442_0,
        VER_1_0_3504_0,
        VER_1_0_3521_0,
        VER_1_0_3570_0,
    }
}
