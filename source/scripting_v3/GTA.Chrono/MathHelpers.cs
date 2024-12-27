//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA.Chrono
{
    internal static class MathHelpers
    {
        internal static uint DivRem(uint left, uint right, out uint remainder)
        {
            uint quotient = left / right;
            remainder = left - (quotient * right);
            return quotient;
        }

        internal static ulong DivRem(ulong left, ulong right, out ulong remainder)
        {
            ulong quotient = left / right;
            remainder = left - (quotient * right);
            return quotient;
        }
    }
}
