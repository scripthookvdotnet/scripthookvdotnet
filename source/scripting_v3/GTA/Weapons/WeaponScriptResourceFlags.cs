//
// Copyright (C) 2025 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
    [Flags]
    public enum WeaponScriptResourceFlags : uint
    {
        RequestBaseAnims = 1,
        RequestCoverAnims = 2,
        RequestMeleeAnims = 4,
        RequestMotionAnims = 8,
        RequestStealthAnims = 16,
        RequestAllMovementVariationAnims = 32,

        RequestAllAnims = 31,
    }
}
