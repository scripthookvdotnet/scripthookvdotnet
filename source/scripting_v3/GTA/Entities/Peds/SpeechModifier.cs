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

    internal static class SpeechModifierExtensions
    {
        internal static string GetName(this SpeechModifier value) => SpeechModifierHelpers.GetName(value);
    }

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

        internal static readonly int s_modiferCount = s_speechModifierNames.Length;
        internal static string GetName(SpeechModifier modifier) => s_speechModifierNames[(int)modifier];
    }
}
