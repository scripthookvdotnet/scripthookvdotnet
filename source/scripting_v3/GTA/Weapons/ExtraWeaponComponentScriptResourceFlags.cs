//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
    [Flags]
    public enum ExtraWeaponComponentScriptResourceFlags : uint
    {
        None = 0,
        Flash = 1,
        Scope = 2,
        Supp = 4,
        Sclip2 = 8,
        Grip = 16
    }
}
