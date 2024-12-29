//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Runtime.CompilerServices;

namespace GTA.Chrono
{
    internal static class BitOperations
    {
        private static readonly byte[] Log2DeBruijn = new byte[32]
        {
            00, 09, 01, 10, 13, 21, 02, 29,
            11, 14, 16, 18, 22, 25, 03, 30,
            08, 12, 20, 28, 15, 17, 24, 07,
            19, 27, 23, 06, 26, 05, 04, 31
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Log2(uint value)
        {
            // The 0->0 contract is fulfilled by setting the LSB to 1.
            // Log(1) is 0, and setting the LSB for values > 1 does not change the log2 result.
            value |= 1;

            // Bothering to call `lzcnt` code from C++ code or calling asm code in C# code isn't worth the effort,
            // because calling `lzcnt` saves only 25% of the time compared to the software fallback.
            return Log2SoftwareFallback(value);
        }

        public static int Log2(ulong value)
        {
            uint hi = (uint)(value >> 32);

            if (hi == 0)
            {
                return Log2((uint)value);
            }

            return 32 + Log2(hi);
        }

        private static int Log2SoftwareFallback(uint value)
        {
            // No `AggressiveInlining` due to large method size.
            // Has conventional contract 0->0 (`Log(0)` is undefined).

            // Fill trailing zeros with ones, eg `00010010` becomes `00011111`
            value |= value >> 01;
            value |= value >> 02;
            value |= value >> 04;
            value |= value >> 08;
            value |= value >> 16;

            // `uint.MaxValue >> 27` is always in range [0 - 31] so there's no need to let the JIT perform bound
            // checks, really, but pinning the array takes more time than accepting bound checks if we access
            // the array only couple times after pinning it.
            return Log2DeBruijn[(value * 0x07C4ACDDu) >> 27];
        }
    }
}
