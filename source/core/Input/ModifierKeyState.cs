//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace SHVDN
{
    [Flags]
    public enum ModifierKeyState
    {
        None = 0,
        Shift = 0x10000,
        Control = 0x20000,
        Alt = 0x40000,
    }
}
