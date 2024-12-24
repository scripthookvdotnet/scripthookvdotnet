using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace GTA.Chrono
{
    internal static class FormattingHelpers
    {
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
