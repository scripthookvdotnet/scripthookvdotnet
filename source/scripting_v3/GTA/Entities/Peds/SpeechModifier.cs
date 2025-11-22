//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    public enum SpeechModifier
    {
        Standard,
        AllowRepeat,
        Beat,
        Force,
        ForceFrontend,
        ForceNoRepeatFrontend,
        ForceNormal,
        ForceNormalClear,
        ForceNormalCritical,
        ForceShouted,
        ForceShoutedClear,
        ForceShoutedCritical,
        ForcePreloadOnly,
        Megaphone,
        Helicopter,
        ForceMegaphone,
        ForceHelicopter,
        Interrupt,
        InterruptShouted,
        InterruptShoutedClear,
        InterruptShoutedCritical,
        InterruptNoForce,
        InterruptFrontend,
        InterruptNoForceFrontend,
        AddBlip,
        AddBlipAllowRepeat,
        AddBlipForce,
        AddBlipShouted,
        AddBlipShoutedForce,
        AddBlipInterrupt,
        AddBlipInterruptForce,
        ForcePreloadOnlyShouted,
        ForcePreloadOnlyShoutedClear,
        ForcePreloadOnlyShoutedCritical,
        Shouted,
        ShoutedClear,
        ShoutedCritical,
    }

    /// <summary>
    /// Provides <see langword="internal"/> extension methods for <see cref="SpeechModifier"/>.
    /// </summary>
    internal static class SpeechModifierExtensions
    {
        /// <summary>
        /// Gets the internal name for the specified <see cref="SpeechModifier"/>.
        /// </summary>
        /// <param name="value">The modifier.</param>
        /// <returns>The internal name.</returns>
        internal static string GetInternalName(this SpeechModifier value) => SpeechModifierHelpers.GetInternalName(value);
    }

    /// <summary>
    /// Provides <see langword="internal"/> helpers for <see cref="SpeechModifier"/>.
    /// </summary>
    internal static class SpeechModifierHelpers
    {
        private static readonly string[] s_speechModifierNames = {
            "SPEECH_PARAMS_STANDARD",
            "SPEECH_PARAMS_ALLOW_REPEAT",
            "SPEECH_PARAMS_BEAT",
            "SPEECH_PARAMS_FORCE",
            "SPEECH_PARAMS_FORCE_FRONTEND",
            "SPEECH_PARAMS_FORCE_NO_REPEAT_FRONTEND",
            "SPEECH_PARAMS_FORCE_NORMAL",
            "SPEECH_PARAMS_FORCE_NORMAL_CLEAR",
            "SPEECH_PARAMS_FORCE_NORMAL_CRITICAL",
            "SPEECH_PARAMS_FORCE_SHOUTED",
            "SPEECH_PARAMS_FORCE_SHOUTED_CLEAR",
            "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL",
            "SPEECH_PARAMS_FORCE_PRELOAD_ONLY",
            "SPEECH_PARAMS_MEGAPHONE",
            "SPEECH_PARAMS_HELI",
            "SPEECH_PARAMS_FORCE_MEGAPHONE",
            "SPEECH_PARAMS_FORCE_HELI",
            "SPEECH_PARAMS_INTERRUPT",
            "SPEECH_PARAMS_INTERRUPT_SHOUTED",
            "SPEECH_PARAMS_INTERRUPT_SHOUTED_CLEAR",
            "SPEECH_PARAMS_INTERRUPT_SHOUTED_CRITICAL",
            "SPEECH_PARAMS_INTERRUPT_NO_FORCE",
            "SPEECH_PARAMS_INTERRUPT_FRONTEND",
            "SPEECH_PARAMS_INTERRUPT_NO_FORCE_FRONTEND",
            "SPEECH_PARAMS_ADD_BLIP",
            "SPEECH_PARAMS_ADD_BLIP_ALLOW_REPEAT",
            "SPEECH_PARAMS_ADD_BLIP_FORCE",
            "SPEECH_PARAMS_ADD_BLIP_SHOUTED",
            "SPEECH_PARAMS_ADD_BLIP_SHOUTED_FORCE",
            "SPEECH_PARAMS_ADD_BLIP_INTERRUPT",
            "SPEECH_PARAMS_ADD_BLIP_INTERRUPT_FORCE",
            "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED",
            "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CLEAR",
            "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CRITICAL",
            "SPEECH_PARAMS_SHOUTED",
            "SPEECH_PARAMS_SHOUTED_CLEAR",
            "SPEECH_PARAMS_SHOUTED_CRITICAL",
        };

        /// <summary>
        /// Gets the count of known <see cref="SpeechModifier"/>s.
        /// </summary>
        internal static readonly int s_modiferCount = s_speechModifierNames.Length;

        /// <summary>
        /// Returns the name of a <see cref="SpeechModifier"/> aligned to the game build.
        /// </summary>
        /// <param name="speechModifier">The <see cref="SpeechModifier"/> to correct.</param>
        /// <returns>The corrected integer value based on game build.</returns>
        internal static string GetInternalName(SpeechModifier speechModifier) => s_speechModifierNames[(int)speechModifier];
    }
}
