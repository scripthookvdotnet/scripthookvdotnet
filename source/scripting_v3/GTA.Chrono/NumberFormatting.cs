//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace GTA.Chrono
{
    internal static class NumberFormatting
    {
        // Optimizations using "TwoDigits" inspired by:
        // https://engineering.fb.com/2013/03/15/developer-tools/three-optimization-tips-for-c/
        private static readonly char[] TwoDigitsChars =
            ("00010203040506070809" +
             "10111213141516171819" +
             "20212223242526272829" +
             "30313233343536373839" +
             "40414243444546474849" +
             "50515253545556575859" +
             "60616263646566676869" +
             "70717273747576777879" +
             "80818283848586878889" +
             "90919293949596979899").ToArray();

        // Probably slower than the internal method `Number.WriteTwoDigits` in .NET 9, which `DateTime` uses when
        // `ToString` is called, but way faster than the internal method of .NET Framework 4.8
        // `DateTimeFormat.FormatDigits(StringBuilder outputBuffer, int value, int len, bool overrideLengthLimit)`,
        // which `DateTime` uses when `ToString` is called in .NET Framework 4.8 (more than 2x faster).
        internal static unsafe void WriteTwoDigits(uint value, char* ptr)
        {
            unsafe
            {
                int offsetFromDigitsCharsArray = (int)(value * 2);
                ptr[0] = TwoDigitsChars[offsetFromDigitsCharsArray];
                ptr[1] = TwoDigitsChars[offsetFromDigitsCharsArray + 1];
            }
        }

        internal static unsafe void WriteFourDigits(uint value, char* ptr)
        {
            uint quotient = MathHelpers.DivRem(value, 100, out uint remainder);

            WriteTwoDigits(quotient, ptr);
            WriteTwoDigits(remainder, ptr + 2);
        }

        internal static unsafe void WriteDigits(uint value, char* ptr, int count)
        {
            unsafe
            {
                char* cur;
                for (cur = ptr + count - 1; cur > ptr; cur--)
                {
                    uint temp = '0' + value;
                    value /= 10;
                    *cur = (char)(temp - (value * 10));
                }

                Debug.Assert(value < 10);
                Debug.Assert(cur == ptr);
                *cur = (char)('0' + value);
            }
        }

        internal static unsafe void WriteDigits(ulong value, char* ptr, int count)
        {
            char* cur;
            for (cur = ptr + count - 1; cur > ptr; cur--)
            {
                ulong temp = '0' + value;
                value /= 10;
                *cur = (char)(temp - (value * 10));
            }

            Debug.Assert(value < 10);
            Debug.Assert(cur == ptr);
            *cur = (char)('0' + value);
        }
    }
}
