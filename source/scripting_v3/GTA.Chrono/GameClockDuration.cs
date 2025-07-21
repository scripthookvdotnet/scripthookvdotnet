//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GTA.Chrono
{
    /// <summary>
    /// Represents a fixed length of game clock time with the millisecond precision.
    /// </summary>
    public readonly struct GameClockDuration : IEquatable<GameClockDuration>, IComparable<GameClockDuration>,
        IComparable
    {
        private readonly long _secs;

        internal GameClockDuration(long seconds) : this()
        {
            _secs = seconds;
        }

        /// The number of seconds in a minute.
        const long SecsPerMinute = 60;

        /// The number of seconds in an hour.
        const long SecsPerHour = 3600;

        /// The number of (non-leap) seconds in days.
        const long SecsPerDay = 86_400;

        /// The number of (non-leap) seconds in a week.
        const long SecsPerWeek = 604_800;

        const long LeapYearCountOfInt32 = 1041529570;
        const long NonLeapYearCountOfInt32 = 3253437726;

        /// <summary>
        /// The number of days elapsed since January 1st, the -2147483648 year until December 31st, the 2147483647 year,
        /// which will result in 1_568_704_592_609 days. Subtracted by 1 because 1 day is taken for the min date value.
        /// </summary>
        const long DayCountUInt32YearsLaterSinceInt32MinValueYear = (LeapYearCountOfInt32 * 366)
            + (NonLeapYearCountOfInt32 * 365) - 1;

        /// <summary>
        /// The same value as 135_536_076_801_503_999 seconds.
        /// </summary>
        const long MaxSecDifference = (DayCountUInt32YearsLaterSinceInt32MinValueYear) * SecsPerDay
                                      + 23 * SecsPerHour + 59 * SecsPerMinute + 59;

        const long MinSecDifference = -((DayCountUInt32YearsLaterSinceInt32MinValueYear) * SecsPerDay
                                        + 23 * SecsPerHour + 59 * SecsPerMinute + 59);

        /// <summary>
        /// Represents the zero <see cref="GameClockDuration"/> value. This field is read-only.
        /// </summary>
        public static GameClockDuration Zero = new(0);

        /// <summary>
        /// Represents the maximum <see cref="GameClockDuration"/> value, which can represent the duration from
        /// <see cref="GameClockDateTime.MinValue"/> to <see cref="GameClockDateTime.MaxValue"/>.
        /// This field is read-only.
        /// </summary>
        public static GameClockDuration MaxValue = new(MaxSecDifference);

        /// <summary>
        /// Represents the maximum <see cref="GameClockDuration"/> value, which can represent the duration from
        /// <see cref="GameClockDateTime.MaxValue"/> to <see cref="GameClockDateTime.MinValue"/>.
        /// This field is read-only.
        /// </summary>
        public static GameClockDuration MinValue = new(MinSecDifference);

        /// <summary>
        /// Gets the hours component of the time interval represented by the current <see cref="GameClockDuration"/>
        /// structure.
        /// </summary>
        /// <value>
        /// The hour component of the current <see cref="GameClockDuration"/> structure.
        /// The return value ranges from -23 through 23.
        /// </value>
        public int Hours => (int)((_secs / SecsPerHour) % 24);

        /// <summary>
        /// Gets the minutes component of the time interval represented by the current <see cref="GameClockDuration"/>
        /// structure.
        /// </summary>
        /// <value>
        /// The minute component of the current <see cref="GameClockDuration"/> structure.
        /// The return value ranges from -59 through 59.
        /// </value>
        public int Minutes => (int)((_secs / SecsPerMinute) % 60);

        /// <summary>
        /// Gets the seconds component of the time interval represented by the current <see cref="GameClockDuration"/>
        /// structure.
        /// </summary>
        /// <value>
        /// The second component of the current <see cref="GameClockDuration"/> structure.
        /// The return value ranges from -59 through 59.
        /// </value>
        public int Seconds => (int)(_secs % 60);

        /// <summary>
        /// Gets the value of the current <see cref="GameClockDuration"/> structure expressed in whole and fractional
        /// weeks.
        /// </summary>
        /// <value>The total number of weeks represented by this instance.</value>
        public double TotalWeeks => (double)_secs / SecsPerWeek;

        /// <summary>
        /// Gets the value of the current <see cref="GameClockDuration"/> structure expressed in whole and fractional
        /// days.
        /// </summary>
        /// <value>The total number of days represented by this instance.</value>
        public double TotalDays => (double)_secs / SecsPerDay;

        /// <summary>
        /// Gets the value of the current <see cref="GameClockDuration"/> structure expressed in whole and fractional
        /// hours.
        /// </summary>
        /// <value>The total number of hours represented by this instance.</value>
        public double TotalHours => (double)_secs / SecsPerHour;

        /// <summary>
        /// Gets the value of the current <see cref="GameClockDuration"/> structure expressed in whole and fractional
        /// minutes.
        /// </summary>
        /// <value>The total number of minutes represented by this instance.</value>
        public double TotalMinutes => (double)_secs / SecsPerMinute;

        /// <summary>
        /// Gets the value of the current <see cref="GameClockDuration"/> structure expressed in whole weeks.
        /// </summary>
        /// <value>The number of whole weeks represented by this instance.</value>
        public long WholeWeeks => _secs / SecsPerWeek;

        /// <summary>
        /// Gets the value of the current <see cref="GameClockDuration"/> structure expressed in whole days.
        /// </summary>
        /// <value>The number of whole days represented by this instance.</value>
        public long WholeDays => _secs / SecsPerDay;

        /// <summary>
        /// Gets the value of the current <see cref="GameClockDuration"/> structure expressed in whole hours.
        /// </summary>
        /// <value>The number of whole hours represented by this instance.</value>
        public long WholeHours => _secs / SecsPerHour;

        /// <summary>
        /// Gets the value of the current <see cref="GameClockDuration"/> structure expressed in whole minutes.
        /// </summary>
        /// <value>The number of whole minutes represented by this instance.</value>
        public long WholeMinutes => _secs / SecsPerMinute;

        /// <summary>
        /// Gets the value of the current <see cref="GameClockDuration"/> structure expressed in whole seconds.
        /// </summary>
        /// <value>The number of whole seconds represented by this instance.</value>
        public long WholeSeconds => _secs;

        private static void ThrowOutOfRange_TooLongDuration()
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(null,
                "GameClockDuration overflowed because the duration is too long.");
        }

        private static void ThrowIfOverflowedFromInt32YearMonthDay(long secs)
        {
            if (secs < MinSecDifference || secs > MaxSecDifference)
            {
                ThrowOutOfRange_TooLongDuration();
            }
        }

        /// <summary>
        /// Compares the value of this instance to a specified object that contains a specified
        /// <see cref="GameClockDuration"/> value, and returns an integer that indicates whether this instance is
        /// less than, the same as, or greater than the specified <see cref="GameClockDuration"/> value.
        /// </summary>
        /// <param name="value">A boxed object to compare, or <see langword="null"/>.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and the value parameter. Less than zero if
        /// this instance is less than value. Zero if this instance is the same as value. Greater than zero if this
        /// instance is greater than value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is not a <see cref="GameClockDuration"/>.
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is GameClockDuration otherDuration))
                throw new ArgumentException();

            long t = (otherDuration)._secs;
            if (_secs > t) return 1;
            if (_secs < t) return -1;
            return 0;
        }

        /// <summary>
        /// Compares the value of this instance to a specified <see cref="GameClockTime"/> value and indicates whether
        /// this instance is less than, the same as, or greater than the specified <see cref="GameClockTime"/> value.
        /// </summary>
        /// <param name="value">The object to compare to the current instance.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and the value parameter.
        /// <list type="bullet">
        /// <item>
        /// <description>Less than zero if this instance is less than <paramref name="value"/>.</description>
        /// </item>
        /// <item>
        /// <description>Zero if this instance is the same as <paramref name="value"/>.</description>
        /// </item>
        /// <item>
        /// <description>Greater than zero if this instance is greater than <paramref name="value"/>.</description>
        /// </item>
        /// </list>
        /// </returns>
        public int CompareTo(GameClockDuration value)
        {
            long t = value._secs;
            if (_secs > t) return 1;
            if (_secs < t) return -1;
            return 0;
        }

        /// <summary>
        /// Returns a <see cref="GameClockDuration"/> that represents a specified number of weeks.
        /// </summary>
        /// <param name="weeks">A number of weeks.</param>
        /// <returns>An object that represents <paramref name="weeks"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="weeks"/> is not between -224100656035 and 224100656035.
        /// </exception>
        public static GameClockDuration FromWeeks(long weeks)
        {
            const long minWeeks = MinSecDifference / SecsPerWeek;
            const long maxWeeks = MaxSecDifference / SecsPerWeek;
            ThrowHelper.CheckArgumentRange(nameof(weeks), weeks, minWeeks, maxWeeks);

            return new GameClockDuration(weeks * SecsPerWeek);
        }

        /// <summary>
        /// Returns a <see cref="GameClockDuration"/> that represents a specified number of days.
        /// </summary>
        /// <param name="days">A number of days.</param>
        /// <returns>An object that represents <paramref name="days"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="days"/> is not between -1568704592245 and 1568704592245.
        /// </exception>
        public static GameClockDuration FromDays(long days)
        {
            const long minDays = MinSecDifference / SecsPerDay;
            const long maxDays = MaxSecDifference / SecsPerDay;
            ThrowHelper.CheckArgumentRange(nameof(days), days, minDays, maxDays);

            return new GameClockDuration(days * SecsPerDay);
        }

        /// <summary>
        /// Returns a <see cref="GameClockDuration"/> that represents a specified number of hours.
        /// </summary>
        /// <param name="hours">A number of hours.</param>
        /// <returns>An object that represents <paramref name="hours"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="hours"/> is not between -37648910213903 and 37648910213903.
        /// </exception>
        public static GameClockDuration FromHours(long hours)
        {
            const long minHours = MinSecDifference / SecsPerHour;
            const long maxHours = MaxSecDifference / SecsPerHour;
            ThrowHelper.CheckArgumentRange(nameof(hours), hours, minHours, maxHours);

            return new GameClockDuration(hours * SecsPerHour);
        }

        /// <summary>
        /// Returns a <see cref="GameClockDuration"/> that represents a specified number of minutes.
        /// </summary>
        /// <param name="minutes">A number of minutes.</param>
        /// <returns>An object that represents <paramref name="minutes"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="minutes"/> is not between -2258934612834239 and 2258934612834239.
        /// </exception>
        public static GameClockDuration FromMinutes(long minutes)
        {
            const long minMinutes = MinSecDifference / SecsPerMinute;
            const long maxMinutes = MaxSecDifference / SecsPerMinute;
            ThrowHelper.CheckArgumentRange(nameof(minutes), minutes, minMinutes, maxMinutes);

            return new GameClockDuration(minutes * SecsPerMinute);
        }

        /// <summary>
        /// Returns a <see cref="GameClockDuration"/> that represents a specified number of seconds.
        /// </summary>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>An object that represents <paramref name="seconds"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="seconds"/> is not between -135536076770054399 and 135536076770054399.
        /// </exception>
        public static GameClockDuration FromSeconds(long seconds)
        {
            const long minSeconds = MinSecDifference;
            const long maxSeconds = MaxSecDifference;
            ThrowHelper.CheckArgumentRange(nameof(seconds), seconds, minSeconds, maxSeconds);

            return new GameClockDuration(seconds);
        }

        public static GameClockDuration FromTimeSpan(TimeSpan timeSpan)
            => new GameClockDuration(timeSpan.Ticks / TimeSpan.TicksPerSecond);

        public GameClockDuration Abs() => (_secs < 0) ? new GameClockDuration(-_secs) : this;

        /// <summary>
        /// Returns the specified instance of <see cref="GameClockDuration"/>.
        /// </summary>
        /// <param name="d">The duration to return.</param>
        /// <returns>The time interval specified by <paramref name="d"/>.</returns>
        public static GameClockDuration operator +(GameClockDuration d) => d;

        /// <summary>
        /// Adds two specified <see cref="GameClockDuration"/> instances.
        /// </summary>
        /// <param name="d1">The first game clock duration to add.</param>
        /// <param name="d2">The second game clock duration to add.</param>
        /// <returns>
        /// An object whose value is the sum of the values of <paramref name="d1"/> and <paramref name="d2"/>.
        /// </returns>
        public static GameClockDuration operator +(GameClockDuration d1, GameClockDuration d2)
        {
            long result = d1._secs + d2._secs;
            ThrowIfOverflowedFromInt32YearMonthDay(result);

            return new GameClockDuration(result);
        }

        /// <summary>
        /// Returns a <see cref="GameClockDuration"/> whose value is the negated value of the specified instance.
        /// </summary>
        /// <param name="d">The duration to be negated.</param>
        /// <returns>
        /// An object that has the same numeric value as this instance, but the opposite sign.
        /// </returns>
        public static GameClockDuration operator -(GameClockDuration d) => new(-d._secs);

        /// <summary>
        /// Subtracts a specified <see cref="GameClockDuration"/> from another specified
        /// <see cref="GameClockDuration"/>.
        /// </summary>
        /// <param name="d1">The minuend.</param>
        /// <param name="d2">The subtrahend.</param>
        /// <returns>
        /// An object whose value is the result of the value of <paramref name="d1"/> minus the value of
        /// <paramref name="d2"/>.
        /// </returns>
        public static GameClockDuration operator -(GameClockDuration d1, GameClockDuration d2)
        {
            long result = d1._secs - d2._secs;
            ThrowIfOverflowedFromInt32YearMonthDay(result);

            return new GameClockDuration(result);
        }

        /// <summary>
        /// Returns a new <see cref="GameClockDuration"/> object whose value is the result of multiplying the specified
        /// <paramref name="duration"/> instance and the specified <paramref name="factor"/>.
        /// </summary>
        /// <param name="duration">The value to be multiplied.</param>
        /// <param name="factor">The value to be multiplied by.</param>
        /// <returns>
        /// A new <see cref="GameClockDuration"/> representing the result of multiplying <paramref name="duration"/>
        /// by <paramref name="factor"/>.
        /// </returns>
        public static GameClockDuration operator *(GameClockDuration duration, long factor)
        {
            long result = duration._secs * factor;
            if (duration._secs != 0 && result / duration._secs != factor)
            {
                // overflow, throw ArgumentOutOfRangeException if overflowed to avoid the exception type surprise.
                ThrowOutOfRange_TooLongDuration();
            }

            ThrowIfOverflowedFromInt32YearMonthDay(result);

            return new GameClockDuration(result);
        }

        public static GameClockDuration operator *(GameClockDuration duration, double factor)
        {
            if (double.IsNaN(factor))
            {
                ThrowHelper.ThrowArgumentException_Arg_CannotBeNaN(nameof(factor));
            }

            long durationSecs = duration._secs;
            double resultDouble = durationSecs * factor;
            if (resultDouble > -9007199254740992 && resultDouble < 9007199254740992)
            {
                // The result is within the safe integers out of f64, just use the result calculated as f64.
                // We want to calculate the result as a f64 for performance reasons as long as the result fall
                // within the safe integers out of f64 (calculation as System.Decimal takes more than 10x time).
                return IntervalFromF64Seconds(resultDouble);
            }

            // Fall back to decimal arithmetic, so the calculation 100% will not have any rounding errors in the
            // integral part.
            // Let the code throw an exception even if the product as a double was an infinity.
            return MultiplySecondsInDecimal(durationSecs, factor);

            static GameClockDuration MultiplySecondsInDecimal(long durationSecs, double divisor)
            {
                try
                {
                    // Throw ArgumentOutOfRangeException in the catch block if OverflowException is thrown for decimal
                    // arithmetics or too large divisor to match what FromDecimalSecondsInternal throws for too large
                    // or small results.
                    decimal divisorDecimal = (decimal)divisor;

                    // Throw ArgumentOutOfRangeException if the divisor is zero as decimal to avoid an unintended
                    // DivideByZeroException. we would not expect such exception when trying to divide an integer by
                    // a floating-point value.
                    // Since decimal's smallest positive value is `1e-28m` and `Decimal.Ceiling(1 / 1e-28m)` is larger
                    // than the number of whole seconds of GameClockDuration.MaxValue, we can assume that division by
                    // an arbitrary double smaller than 1e-28m will result in a value larger than the max seconds.
                    if (divisorDecimal == 0)
                    {
                        ThrowOutOfRange_TooLongDuration();
                    }

                    return FromDecimalSecondsInternal(durationSecs * divisorDecimal);
                }
                catch (OverflowException)
                {
                    ThrowOutOfRange_TooLongDuration();
                    return default;
                }
            }
        }

        /// <summary>
        /// Returns a new <see cref="GameClockDuration"/> object whose value is the result of multiplying the
        /// specified <paramref name="factor"/> and the specified <paramref name="duration"/> instance.
        /// </summary>
        /// <param name="factor">The value to be multiplied by.</param>
        /// <param name="duration">The value to be multiplied.</param>
        /// <returns>
        /// A new object that represents the value of the specified <paramref name="factor"/> multiplied by the value
        /// of the specified <paramref name="duration"/> instance.
        /// </returns>
        public static GameClockDuration operator *(long factor, GameClockDuration duration)
            => duration * factor;

        /// <summary>
        /// Returns a new <see cref="GameClockDuration"/> object whose value is the result of multiplying the
        /// specified <paramref name="factor"/> and the specified <paramref name="duration"/> instance.
        /// </summary>
        /// <param name="factor">The value to be multiplied by.</param>
        /// <param name="duration">The value to be multiplied.</param>
        /// <returns>
        /// A new object that represents the value of the specified <paramref name="factor"/> multiplied by the value
        /// of the specified <paramref name="duration"/> instance.
        /// </returns>
        public static GameClockDuration operator *(double factor, GameClockDuration duration)
            => duration * factor;

        /// <summary>
        /// Returns a new TimeSpan object whose value is the result of dividing the specified
        /// <paramref name="duration"/> by the specified <paramref name="divisor"/>.
        /// </summary>
        /// <param name="duration">Dividend or the value to be divided.</param>
        /// <param name="divisor">The value to be divided by.</param>
        /// <returns>
        /// A new object that represents the value of <paramref name="duration"/> divided by the value of
        /// <paramref name="divisor"/>.
        /// </returns>
        public static GameClockDuration operator /(GameClockDuration duration, long divisor)
            => new GameClockDuration(duration._secs / divisor);

        /// <summary>
        /// Returns a new TimeSpan object whose value is the result of dividing the specified
        /// <paramref name="duration"/> by the specified <paramref name="divisor"/>.
        /// </summary>
        /// <param name="duration">Dividend or the value to be divided.</param>
        /// <param name="divisor">The value to be divided by.</param>
        /// <returns>
        /// A new object that represents the value of <paramref name="duration"/> divided by the value of
        /// <paramref name="divisor"/>.
        /// </returns>
        public static GameClockDuration operator /(GameClockDuration duration, double divisor)
        {
            if (double.IsNaN(divisor))
            {
                ThrowHelper.ThrowArgumentException_Arg_CannotBeNaN(nameof(divisor));
            }

            long durationSecs = duration._secs;
            double resultDouble = durationSecs / divisor;
            if (resultDouble > -9007199254740992 && resultDouble < 9007199254740992)
            {
                // The result is within the safe integers out of f64, just use the result calculated as f64.
                // We want to calculate the result as a f64 for performance reasons as long as the result fall
                // within the safe integers out of f64 (calculation as System.Decimal takes more than 10x time).
                return IntervalFromF64Seconds(resultDouble);
            }

            // Fall back to decimal arithmetic, so the calculation 100% will not have any rounding errors in the
            // integral part.
            // Let the code throw an exception even if the product as a double was an infinity.
            return DivideSecondsInDecimal(durationSecs, divisor);

            static GameClockDuration DivideSecondsInDecimal(long durationSecs, double divisor)
            {
                try
                {
                    // Throw ArgumentOutOfRangeException in the catch block if OverflowException is thrown for decimal
                    // arithmetics or too large divisor to match what FromDecimalSecondsInternal throws for too large
                    // or small results.
                    decimal divisorDecimal = (decimal)divisor;

                    // Throw ArgumentOutOfRangeException if the divisor is zero as decimal to avoid an unintended
                    // DivideByZeroException. we would not expect such exception when trying to divide an integer by
                    // a floating-point value.
                    // Since decimal's smallest positive value is `1e-28m` and `Decimal.Ceiling(1 / 1e-28m)` is larger
                    // than the number of whole seconds of GameClockDuration.MaxValue, we can assume that division by
                    // an arbitrary double smaller than 1e-28m will result in a value larger than the max seconds.
                    if (divisorDecimal == 0)
                    {
                        ThrowOutOfRange_TooLongDuration();
                    }

                    return FromDecimalSecondsInternal(durationSecs / divisorDecimal);
                }
                catch (OverflowException)
                {
                    ThrowOutOfRange_TooLongDuration();
                    return default;
                }
            }
        }

        /// <summary>
        /// Returns a new Double value that's the result of dividing <paramref name="d1"/> by <paramref name="d2"/>.
        /// </summary>
        /// <param name="d1">The dividend or the value to be divided.</param>
        /// <param name="d2">The value to be divided by.</param>
        /// <returns>
        /// A new value that represents result of dividing <paramref name="d1"/> by the value of
        /// <paramref name="d2"/>.
        /// </returns>
        public static double operator /(GameClockDuration d1, GameClockDuration d2) => d1._secs / d2._secs;

        private static GameClockDuration IntervalFromF64Seconds(double secs)
        {
            // Use the same rounding mode as how floating-point values are rounded when they can't be exactly
            // represented in .NET, so users wouldn't get surprised as much as when the rounding mode is set to
            // `MidpointRounding.AwayFromZero` (Round to nearest, ties away from zero).
            // Do not check the second boundary, as this method is supposed to be called within the boundary.
            return new GameClockDuration((long)System.Math.Round(secs, MidpointRounding.ToEven));
        }

        private static GameClockDuration FromDecimalSecondsInternal(decimal secs)
        {
            if (secs < MinSecDifference || secs > MaxSecDifference)
            {
                ThrowOutOfRange_TooLongDuration();
            }

            // Use `MidpointRounding.ToEven` (round to nearest, ties to even) so the rounding mode will match how
            // floating-point values are rounded when they can't be exactly represented in .NET. The standard ECMA-335
            // restricts the rounding mode for floating-point values to “round to the nearest number,”
            // (roundTiesToEven) in Common Language Infrastructure (CLI), which is defined in I.12.1.3 in 6th edition
            // of ECMA-335.
            return new GameClockDuration((long)decimal.Round(secs, MidpointRounding.ToEven));
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="GameClockDuration"/>
        /// object.
        /// </summary>
        /// <param name="value">An object to compare with this instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> represents the same game clock duration as this
        /// instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(GameClockDuration value)
        {
            return _secs == value._secs;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="value">An object to compare with this instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is a <see cref="GameClockDuration"/> object that
        /// represents the same game clock duration as the current <see cref="GameClockDuration"/> structure;
        /// otherwise, false.
        /// </returns>
        public override bool Equals(object value)
        {
            if (value is GameClockDuration duration)
            {
                return Equals(duration);
            }

            return false;
        }

        public override string ToString() => ToStringInternal();

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ToStringInternal()
        {
            unsafe
            {
                // large enough for any standard string format for `GameClockDuration` (e.g. "-1568704592609:23:59:59"
                // for the min value)
                const int destSize = 24;
                char* dest = stackalloc char[destSize];
                TryFormatStandard(this, dest, destSize, out int charsWritten);

                return new string(dest, 0, charsWritten);
            }
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe bool TryFormatStandard(GameClockDuration duration, char* dest,
            int destSizeInElemCount, out int charsWritten)
        {
            bool secsValIsNegative = duration._secs < 0;
            ulong absSecs = (ulong)(secsValIsNegative ? -duration._secs : duration._secs);

            // start with "-HH:MM:SS" if the value is negative or else "HH:MM:SS", and adjust as necessary
            int requiredOutputLength = secsValIsNegative ? 9 : 8;

            ulong totalMinsRemaining = MathHelpers.DivRem(absSecs, SecsPerMinute, out ulong secs);
            Debug.Assert(secs < 60);

            ulong totalHoursRemaining = 0, mins = 0;
            if (totalMinsRemaining > 0)
            {
                totalHoursRemaining = MathHelpers.DivRem(totalMinsRemaining, 60 /* minutes per hour */, out mins);
                Debug.Assert(mins < 60);
            }

            ulong days = 0, hours = 0;
            if (totalHoursRemaining > 0)
            {
                days = MathHelpers.DivRem(totalHoursRemaining, 24 /* hours per days */, out hours);
                Debug.Assert(hours < 24);
            }

            int dayDigits = 0;
            if (days > 0)
            {
                dayDigits = FormattingHelpers.CountDigits(days);
                Debug.Assert(dayDigits <= 13);
                requiredOutputLength += dayDigits + 1; // for the leading `D:`
            }

            if (destSizeInElemCount < requiredOutputLength)
            {
                charsWritten = 0;
                return false;
            }

            char* p = dest;
            if (secsValIsNegative)
            {
                *p++ = '-';
            }

            // Write day and separator, if necessary
            if (dayDigits > 0)
            {
                NumberFormatting.WriteDigits(days, p, dayDigits);
                p += dayDigits;
                *p++ = ':';
            }

            // Write "HH:MM:SS"
            NumberFormatting.WriteTwoDigits((uint)hours, p);
            p += 2;
            *p++ = ':';
            NumberFormatting.WriteTwoDigits((uint)mins, p);
            p += 2;
            *p++ = ':';
            NumberFormatting.WriteTwoDigits((uint)secs, p);
            p += 2;
            Debug.Assert(p - dest == requiredOutputLength);

            charsWritten = requiredOutputLength;
            return true;
        }

        /// <summary>
        /// Indicates whether two <see cref="GameClockDuration"/> instances are equal.
        /// </summary>
        /// <param name="left">The first game clock duration to compare.</param>
        /// <param name="right">The second game clock duration to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(GameClockDuration left, GameClockDuration right)
        {
            return left.Equals(right);
        }
        /// <summary>
        /// Indicates whether two <see cref="GameClockDuration"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first game clock duration to compare.</param>
        /// <param name="right">The second game clock duration to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(GameClockDuration left, GameClockDuration right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Indicates whether a specified <see cref="GameClockDuration"/> is less than another specified
        /// <see cref="GameClockDuration"/>.
        /// </summary>
        /// <param name="left">The first game clock duration to compare.</param>
        /// <param name="right">The second game clock duration to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="left"/> is less than the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <(GameClockDuration left, GameClockDuration right) => left._secs < right._secs;
        /// <summary>
        /// Indicates whether a specified <see cref="GameClockDuration"/> is less than or equal to another specified
        /// <see cref="GameClockDuration"/>.
        /// </summary>
        /// <param name="left">The first game clock duration to compare.</param>
        /// <param name="right">The second game clock duration to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="left"/> is less than or equal to the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <=(GameClockDuration left, GameClockDuration right) => left._secs <= right._secs;

        /// <summary>
        /// Indicates whether a specified <see cref="GameClockDuration"/> is greater than another specified
        /// <see cref="GameClockDuration"/>.
        /// </summary>
        /// <param name="left">The first game clock duration to compare.</param>
        /// <param name="right">The second game clock duration to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >(GameClockDuration left, GameClockDuration right) => left._secs > right._secs;
        /// <summary>
        /// Indicates whether a specified <see cref="GameClockDuration"/> is greater than or equal to another specified
        /// <see cref="GameClockDuration"/>.
        /// </summary>
        /// <param name="left">The first game clock duration to compare.</param>
        /// <param name="right">The second game clock duration to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="left"/> is greater than or equal to the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >=(GameClockDuration left, GameClockDuration right) => left._secs >= right._secs;

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return _secs.GetHashCode();
        }
    }
}
