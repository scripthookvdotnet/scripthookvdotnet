//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace GTA.Chrono
{
    internal static class FormattingHelpers
    {
        // Map the log2(value) to a power of 10.
        private static readonly long[] Log2ToPow10ForU64 = new long[64]
        {
            1,  1,  1,  2,  2,  2,  3,  3,  3,  4,  4,  4,  4,  5,  5,  5,
            6,  6,  6,  7,  7,  7,  7,  8,  8,  8,  9,  9,  9,  10, 10, 10,
            10, 11, 11, 11, 12, 12, 12, 13, 13, 13, 13, 14, 14, 14, 15, 15,
            15, 16, 16, 16, 16, 17, 17, 17, 18, 18, 18, 19, 19, 19, 19, 20
        };

        // Read the associated power of 10.
        private static readonly ulong[] PowersOf10 = new ulong[21]
        {
            0, // unused entry to avoid needing to subtract
            0,
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
            1000000000,
            10000000000,
            100000000000,
            1000000000000,
            10000000000000,
            100000000000000,
            1000000000000000,
            10000000000000000,
            100000000000000000,
            1000000000000000000,
            10000000000000000000,
        };

        // Based on do_count_digits from https://github.com/fmtlib/fmt/blob/662adf4f33346ba9aba8b072194e319869ede54a/include/fmt/format.h#L1124
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountDigits(ulong value)
        {
            Debug.Assert(Log2ToPow10ForU64.Length == 64);
            var index = (uint)Log2ToPow10ForU64[BitOperations.Log2(value)];

            Debug.Assert((index + 1) <= PowersOf10.Length);
            ulong powerOf10 = PowersOf10[BitOperations.Log2(index)];

            // Return the number of digits based on the power of 10, shifted by 1 if it falls below the threshold.
            bool lessThan = value < powerOf10;
            unsafe
            {
                // while arbitrary bools may be non-0/1, comparison operators are expected to return 0/1
                return (int)(index - *(byte*)&lessThan);
            }
        }

        // Algorithm based on https://lemire.me/blog/2021/06/03/computing-the-number-of-digits-of-an-integer-even-faster.
        private static readonly long[] FastCountDigitTable = new long[32]
        {
            4294967296,
            8589934582,
            8589934582,
            8589934582,
            12884901788,
            12884901788,
            12884901788,
            17179868184,
            17179868184,
            17179868184,
            21474826480,
            21474826480,
            21474826480,
            21474826480,
            25769703776,
            25769703776,
            25769703776,
            30063771072,
            30063771072,
            30063771072,
            34349738368,
            34349738368,
            34349738368,
            34349738368,
            38554705664,
            38554705664,
            38554705664,
            41949672960,
            41949672960,
            41949672960,
            42949672960,
            42949672960,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CountDigits(uint value)
        {
            Debug.Assert(FastCountDigitTable.Length == 32, "Every result of CountDigits(value) needs a long entry in the table.");

            long tableValue = FastCountDigitTable[BitOperations.Log2(value)];
            return (int)((value + tableValue) >> 32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CountDecimalTrailingZeros(uint value, out uint valueWithoutTrailingZeros)
        {
            int zeroCount = 0;

            if (value != 0)
            {
                while (true)
                {
                    uint temp = value / 10;
                    if (value != (temp * 10))
                    {
                        break;
                    }

                    value = temp;
                    zeroCount++;
                }
            }

            valueWithoutTrailingZeros = value;
            return zeroCount;
        }
    }
}
