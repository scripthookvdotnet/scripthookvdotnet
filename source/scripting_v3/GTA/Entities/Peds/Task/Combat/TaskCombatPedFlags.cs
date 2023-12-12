//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
    /// <summary>
    /// An enumeration of known flags for the <c>CTaskCombat</c>.
    /// </summary>
    [Flags]
    public enum TaskCombatFlags
    {
        None = 0,
        UseFlinchAimIntro = 16384,
        UseSurprisedAimIntro = 32768,
        /// <summary>
        /// The <see cref="Ped"/> will try to arrest the target if they are a cop, or this flag does not have effect.
        /// </summary>
        ArrestTarget = 65536,
        PreventChangingTarget = 67108864,
        DisableAimIntro = 134217728,
        UseSniperAimIntro = 536870912,
    }
}
