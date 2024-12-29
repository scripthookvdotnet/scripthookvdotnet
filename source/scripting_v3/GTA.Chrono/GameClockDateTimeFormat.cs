//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Runtime.CompilerServices;

namespace GTA.Chrono
{
    internal static class GameClockDateTimeFormat
    {
        private const char HyphenForDateSeparator = '-';
        private const char MinusSign = HyphenForDateSeparator;

        private const int RequiredStrLenForDateWithNonNegative4DigitYear = 10;

        private const char ColonForTimeSeparator = ':';

        internal static unsafe bool TryFormatDateS(GameClockDate date, char* dest, int destLengthInElemCount,
            out int charsWritten)
        {
            int year = date.Year;

            if (year is < -9999 or > 9999)
            {
                return FormatStringForDateWithMoreThan4DigitYear(date, dest, destLengthInElemCount, out charsWritten);
            }

            if (year >= 0)
            {
                if (destLengthInElemCount < RequiredStrLenForDateWithNonNegative4DigitYear)
                {
                    charsWritten = 0;
                    return false;
                }

                FormatStringForDateWithNonNegative4DigitYear(date, dest);
                charsWritten = RequiredStrLenForDateWithNonNegative4DigitYear;
                return true;
            }

            return TryFormatDateSNegative4DigitYear(date, dest, destLengthInElemCount, out charsWritten);
        }

        // Negative year values would be way less common than the year values between 0 and 9999, so no aggressive
        // inlining
        private static unsafe bool TryFormatDateSNegative4DigitYear(GameClockDate date, char* dest,
            int destLengthInElemCount, out int charsWritten)
        {
            if (destLengthInElemCount < RequiredStrLenForDateWithNonNegative4DigitYear + 1)
            {
                charsWritten = 0;
                return false;
            }

            FormatStringForDateWithNegative4DigitYear(date, dest);
            charsWritten = RequiredStrLenForDateWithNonNegative4DigitYear + 1;
            return true;
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe bool TryFormatTimeS(GameClockTime time, char* dest, int destLengthInElemCount,
            out int charsWritten)
        {
            if (destLengthInElemCount < 8)
            {
                charsWritten = 0;
                return false;
            }
            time.Deconstruct(out int hour, out int minute, out int second);

            NumberFormatting.WriteTwoDigits((uint)hour, dest);
            dest[2] = ColonForTimeSeparator;

            NumberFormatting.WriteTwoDigits((uint)minute, dest + 3);
            dest[5] = ColonForTimeSeparator;

            NumberFormatting.WriteTwoDigits((uint)second, dest + 6);

            charsWritten = 8;
            return true;
        }

        /// <summary>
        /// Writes characters that represents a human-readable date string, assuming the year is between 0 and 9999.
        /// <paramref name="dest"/> must have 10 elements to write.
        /// </summary>
        /// <param name="date">The date to create a date string. the year must non-negative and less than 10000.</param>
        /// <param name="dest">The destination pointer. Must have 10 elements to write.</param>
        /// <returns>
        /// A human-readable date string where the year part has 4 digit characters, such as "2008-04-29".
        /// </returns>
        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void FormatStringForDateWithNonNegative4DigitYear(GameClockDate date, char* dest)
        {
            NumberFormatting.WriteFourDigits((uint)date.Year, dest);
            WriteMonthDayStrWithLeadingHyphen(date, dest + 4);
        }

        /// <summary>
        /// Writes characters that represents a human-readable date string, assuming the year is between -9999 and -1.
        /// <paramref name="dest"/> must have 11 elements to write.
        /// </summary>
        /// <param name="date">The date to create a date string. the year must negative and more than 0.</param>
        /// <param name="dest">The destination pointer. Must have 11 elements to write.</param>
        /// <returns>
        /// A human-readable date string where the year part has 4 digit characters, such as "2008-04-29".
        /// </returns>
        [SkipLocalsInit]
        private static unsafe void FormatStringForDateWithNegative4DigitYear(GameClockDate date, char* dest)
        {
            dest[0] = MinusSign;
            NumberFormatting.WriteFourDigits((uint)-date.Year, dest + 1);
            WriteMonthDayStrWithLeadingHyphen(date, dest + 5);
        }

        /// <summary>
        /// Writes characters that represents a human-readable date string, assuming the year is between 0 and 9999.
        /// <paramref name="dest"/> must have 11 elements to write a date string with 5 digit positive year and must
        /// have 17 elements to write any date string.
        /// </summary>
        /// <param name="date">The date to create a date string. the year must non-negative and less than 10000.</param>
        /// <param name="dest">
        /// The destination pointer. Must have 11 elements to write a date string with 5 digit positive year and must
        /// have 17 elements to write any date string.
        /// </param>
        /// <param name="destLengthInElemCount">
        /// The length in element count that tells how many elements can be written from <paramref name="dest"/>.
        /// </param>
        /// <param name="charsWritten">
        /// The actual written characters.
        /// </param>
        /// <returns>
        /// A human-readable date string, such as "12008-04-29".
        /// </returns>
        [SkipLocalsInit]
        // Year values more than 9999 would be way less common than the year values between 0 and 9999, so no aggressive
        // inlining
        private static unsafe bool FormatStringForDateWithMoreThan4DigitYear(GameClockDate date, char* dest,
            int destLengthInElemCount, out int charsWritten)
        {
            const int MinDigitsForYearPart = 4;
            const int RequiredStrLenWithoutYearPart = 6;

            int year = date.Year;
            bool yearIsNegative = year < 0;
            // We can't use `System.Math.Abs(int)`, because it throws an `OverflowException` for int.MinValue,
            // but negating and then casting int.MinValue to uint doesn't cause any problems for `WriteDigits`.
            uint absYear = (uint)(yearIsNegative ? -year : year);

            int yearDigitCount = System.Math.Max(FormattingHelpers.CountDigits(absYear), MinDigitsForYearPart);
            int requiredLen = yearIsNegative
                ? yearDigitCount + RequiredStrLenWithoutYearPart + 1
                : yearDigitCount + RequiredStrLenWithoutYearPart;

            if (destLengthInElemCount < requiredLen)
            {
                charsWritten = 0;
                return false;
            }

            int i;
            if (yearIsNegative)
            {
                dest[0] = MinusSign;
                i = 1;
            }
            else
            {
                i = 0;
            }

            NumberFormatting.WriteDigits(absYear, dest + i, yearDigitCount);
            i += yearDigitCount;
            WriteMonthDayStrWithLeadingHyphen(date, dest + i);
            i += 6;

            charsWritten = i;
            return true;
        }

        /// <summary>
        /// Writes characters that represents a human-readable month-day string with a leading hyphen.
        /// <paramref name="ptr"/> must have 6 elements to write.
        /// </summary>
        /// <param name="date">The date to create a month-day string with a leading hyphen.</param>
        /// <param name="ptr">The destination pointer. Must have 6 elements to write.</param>
        /// <returns>
        /// A human-readable month-day string with a leading hyphen, such as "-04-29".
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void WriteMonthDayStrWithLeadingHyphen(GameClockDate date, char* ptr)
        {
            ptr[0] = HyphenForDateSeparator;
            NumberFormatting.WriteTwoDigits((uint)date.Month, ptr + 1);
            ptr[3] = HyphenForDateSeparator;
            NumberFormatting.WriteTwoDigits((uint)date.Day, ptr + 4);
        }
    }
}
