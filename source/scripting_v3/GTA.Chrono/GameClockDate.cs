//
// Copyright 2012-2014 The Rust Project Developers
// Copyright 2023 kagikn & contributors
// Licensed under the MIT License.
// Original license:
// https://github.com/chronotope/chrono/blob/a44142528eb8bab32f9e16cb74e84bb060f4a667/LICENSE.txt
//

using System;
using System.Runtime.CompilerServices;

namespace GTA.Chrono
{
    /// <summary>
    /// Represents a game clock date, allowing for every <see langword="int"/> year as how the game clock
    /// can represent its year.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <c>With*</c> methods can be convenient to change a single component of a date, but they must be used with
    /// some care. Examples to watch out for:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="WithYear"/> changes the year component of a year-month-day value. Do not use this method if you
    /// want the ordinal to stay the same after changing the year, of if you want the week and weekday values to stay
    /// the same.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Do not combine two <c>With*</c> methods to change two components of the date. For example to change both the
    /// year and month components of a date. This could fail because an intermediate value does not exist, while the
    /// final date would be valid.
    /// </description>
    /// </item>
    /// </list>
    /// For more complex changes to a date, it is best to create a new <see cref="GameClockDate"/> value instead of
    /// altering an existing date.
    /// </para>
    /// <para>
    /// Do not access any methods or properties on <see cref="GameClockDate"/> with the default value. Doing so may
    /// result in undefined behaviors.
    /// </para>
    /// </remarks>
    public readonly struct GameClockDate : IEquatable<GameClockDate>, IComparable<GameClockDate>, IComparable,
        IDatelike<GameClockDate>
    {
        private readonly int _year;
        private readonly OrdFlags _ordFlags;

        internal GameClockDate(int year, OrdFlags ordFlags) : this()
        {
            _year = year;
            _ordFlags = ordFlags;
        }

        private static OrdFlags OrdFlagsForMaxDate = new(365, YearFlags.FromYear(int.MaxValue));
        private static OrdFlags OrdFlagsForMinDate = new(1, YearFlags.FromYear(int.MinValue));

        /// <summary>
        /// Represents the largest possible value of a <see cref="GameClockDate"/>.
        /// </summary>
        /// <value>
        /// The largest possible value of <see cref="GameClockTime"/>, which is December 31, 2147483647 CE.
        /// </value>
        public static GameClockDate MaxValue = new(int.MaxValue, OrdFlagsForMaxDate);
        /// <summary>
        /// Represents the smallest possible value of a <see cref="GameClockDate"/>.
        /// </summary>
        /// <value>
        /// The smallest possible value of <see cref="GameClockTime"/>, which is January 1, -2147483648 BCE.
        /// </value>
        public static GameClockDate MinValue = new(int.MinValue, OrdFlagsForMinDate);

        /// <summary>
        /// Returns the year part of this <see cref="GameClockDate"/>. The returned value is an integer in the range of
        /// <see langword="int"/>.
        /// </summary>
        public int Year => _year;

        /// <summary>
        /// Returns the day of the year represented by this <see cref="GameClockDate"/> starting from 1.
        /// The returned value is an integer between 1 and 366.
        /// </summary>
        public int DayOfYear => (int)_ordFlags.Ordinal;

        /// <summary>
        /// Returns the day of the year represented by this <see cref="GameClockDate"/> starting from 0.
        /// The returned value is an integer between 0 and 365 (the same as <see cref="DayOfYear"/> minus 1).
        /// </summary>
        public int DayOfYear0 => DayOfYear - 1;

        /// <summary>
        /// Returns the day of the week represented by this <see cref="GameClockDate"/> in
        /// <see cref="System.DayOfWeek"/>.
        /// The returned value is an integer between 0 and 6, where 0 indicates Sunday, 1 indicates Monday, 2 indicates
        /// Tuesday, 3 indicates Wednesday, 4 indicates Thursday, 5 indicates Friday, and 6 indicates Saturday.
        /// </summary>
        public DayOfWeek DayOfWeek
        {
            get
            {
                return (DayOfWeek)(((int)IsoDayOfWeek + 1) % 7);
            }
        }

        /// <summary>
        /// Returns the day of the week for ISO 8601 represented by this <see cref="GameClockDate"/> in
        /// <see cref="Chrono.IsoDayOfWeek"/>.
        /// The returned value is an integer between 0 and 6, where 0 indicates Monday, 1 indicates Tuesday,
        /// 2 indicates Wednesday, 3 indicates Thursday, 4 indicates Friday, 5 indicates Saturday, and 6 indicates
        /// Sunday.
        /// </summary>
        public IsoDayOfWeek IsoDayOfWeek => _ordFlags.IsoDayOfWeek;

        /// <summary>
        /// Returns the month part of this <see cref="GameClockDate"/>.
        /// The returned value is an integer between 1 and 12.
        /// </summary>
        public int Month => MonthDayFlags.Month;

        /// <summary>
        /// Returns the zero-based month part of this <see cref="GameClockDate"/>.
        /// The returned value is an integer between 0 and 11 (the same as <see cref="Month"/> minus 1).
        /// </summary>
        public int Month0 => MonthDayFlags.Month - 1;

        /// <summary>
        /// Returns the day-of-month part of this <see cref="GameClockDate"/>.
        /// The returned value is an integer between 1 and 31.
        /// </summary>
        public int Day => MonthDayFlags.Day;

        /// <summary>
        /// Returns the zero-based day-of-month part of this <see cref="GameClockDate"/>.
        /// The returned value is an integer between 0 and 30 (the same as <see cref="Day"/> minus 1).
        /// </summary>
        public int Day0 => MonthDayFlags.Day - 1;

        internal MonthDayFlags MonthDayFlags => _ordFlags.ToMonthDayFlags();

        static GameClockDate? FromOrdinalAndFlags(int year, int ordinal, YearFlags flags)
        {
            return OrdFlags.New(ordinal, flags) switch
            {
                OrdFlags of => new GameClockDate(year, of),
                _ => null
            };
        }

        static GameClockDate? FromMdf(int year, MonthDayFlags mdf)
        {
            OrdFlags? of = mdf.ToOrdFlags();

            if (of is OrdFlags ofNonNull)
            {
                return new GameClockDate(year, ofNonNull);
            }

            return null;
        }

        static GameClockDate FromMdfUnchecked(int year, MonthDayFlags mdf)
            => new GameClockDate(year, mdf.ToOrdFlags().GetValueOrDefault());

        /// <summary>
        /// Makes a new <see cref="GameClockDateTime"/> from the current date and given <see cref="GameClockTime"/>.
        /// </summary>
        public GameClockDateTime AndTime(GameClockTime time)
            => new(this, time);

        /// <summary>
        /// Makes a new <see cref="GameClockDateTime"/> from this <see cref="GameClockDate"/>, hour, minute and second.
        /// </summary>
        public GameClockDateTime AndHms(int hour, int minute, int second)
        {
            GameClockTime time = GameClockTime.FromHms(hour, minute, second);
            return new GameClockDateTime(this, time);
        }

        /// <summary>
        /// Makes a new <see cref="GameClockDate"/> from a <see cref="DateTime"/>.
        /// This does not perform any time zone conversions, so a <see cref="DateTime"/> with a
        /// <see cref="DateTime.Kind"/> of <see cref="DateTimeKind.Utc"/> will still represent the same year/month/day.
        /// </summary>
        public static GameClockDate FromSystemDateTime(DateTime dateTime)
        {
            return FromYmd(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        /// <summary>
        /// Makes a new <see cref="GameClockDate"/> from the calendar date (year, month and day).
        /// </summary>
        /// <param name="year">The year. Any int32 years are valid.</param>
        /// <param name="month">The month of year.</param>
        /// <param name="day">The day of month.</param>
        /// <returns>The resulting date.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameters do not form a valid date.</exception>
        public static GameClockDate FromYmd(int year, int month, int day)
        {
            if (!TryFromYmd(year, month, day, out GameClockDate date))
            {
                ThrowInvalidGregorianMonthDay(year, month, day);
            }

            return date;

            [MethodImpl(MethodImplOptions.NoInlining)]
            static void ThrowInvalidGregorianMonthDay(int year, int month, int day)
            {
                if (month < 1 || month > 12)
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(month), month, 1, 12);
                }

                int dayMax = GetMaxDayOfMonth(month, YearFlags.FromYear(year));
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(day), day, 1, dayMax);
            }
        }

        /// <summary>
        /// Tries to make a new <see cref="GameClockDate"/> from the calendar date (year, month and day).
        /// </summary>
        /// <param name="year">The year. Any int32 years are valid.</param>
        /// <param name="month">The month of year.</param>
        /// <param name="day">The day of month.</param>
        /// <param name="date">
        /// When this method returns, contains the result <see cref="GameClockDate"/>, or an undefined value on failure.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the date is valid and this method successfully created
        /// a <see cref="GameClockDate"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryFromYmd(int year, int month, int day, out GameClockDate date)
        {
            YearFlags flags = YearFlags.FromYear(year);
            MonthDayFlags? mdf = MonthDayFlags.New(month, day, flags);
            if (mdf == null)
            {
                date = default;
                return false;
            }

            MonthDayFlags mdfNonNullable = mdf.GetValueOrDefault();
            if (!mdfNonNullable.IsValid)
            {
                date = default;
                return false;
            }

            date = FromMdfUnchecked(year, mdfNonNullable);
            return true;
        }

        /// <summary>
        /// Makes a new <see cref="GameClockDate"/> from the ordinal date (year and day of the year).
        /// </summary>
        /// <param name="year">The year. Any int32 years are valid.</param>
        /// <param name="ordinal">The day of year.</param>
        /// <returns>The resulting date.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameters do not form a valid date.</exception>
        public static GameClockDate FromOrdinalDate(int year, int ordinal)
        {
            if (!TryFromOrdinalDate(year, ordinal, out GameClockDate date))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(ordinal), ordinal, 1,
                    YearFlags.FromYear(year).DayCount);
            }

            return date;
        }

        /// <summary>
        /// Tries to make a new <see cref="GameClockDate"/> from the ordinal date (year and day of the year).
        /// </summary>
        /// <param name="year">The year. Any int32 years are valid.</param>
        /// <param name="ordinal">The day of year.</param>
        /// <param name="date">
        /// When this method returns, contains the result <see cref="GameClockDate"/>, or an undefined value on failure.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the date is valid and this method successfully created
        /// a <see cref="GameClockDate"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryFromOrdinalDate(int year, int ordinal, out GameClockDate date)
        {
            YearFlags flags = YearFlags.FromYear(year);

            GameClockDate? dateNullable = FromOrdinalAndFlags(year, ordinal, flags);
            if (dateNullable == null)
            {
                date = default;
                return false;
            }

            date = dateNullable.GetValueOrDefault();
            return true;
        }

        /// <summary>
        /// Makes a new <see cref="GameClockDate"/> from the ISO week date (year, week number and day of the week).
        /// The resulting <see cref="GameClockDate"/> may have a different year from the input year.
        /// </summary>
        /// <param name="year">The year. Any int32 years are valid.</param>
        /// <param name="week">The week number.</param>
        /// <param name="dayOfWeek">The day of the <paramref name="week"/>.</param>
        /// <returns>The resulting date.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameters do not form a valid date.</exception>
        public static GameClockDate FromIsoWeekDate(int year, int week, IsoDayOfWeek dayOfWeek)
        {
            if (!TryFromIsoWeekDate(year, week, dayOfWeek, out GameClockDate date))
            {
                Throw_InvalidIsoWeekDate(year, week, dayOfWeek);
            }

            return date;

            [MethodImpl(MethodImplOptions.NoInlining)]
            static void Throw_InvalidIsoWeekDate(int year, int week, IsoDayOfWeek dayOfWeek)
            {
                if (dayOfWeek < IsoDayOfWeek.Monday || dayOfWeek > IsoDayOfWeek.Sunday)
                {
                    ThrowHelper.ArgumentOutOfRangeException_Enum_Value(nameof(dayOfWeek));
                }

                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(week), week, 1,
                    YearFlags.FromYear(year).IsoWeekCount);
            }
        }

        /// <summary>
        /// Tries to make a new <see cref="GameClockDateTime"/> from the ISO week date (year, week number and day of
        /// the week).
        /// </summary>
        /// <param name="year">The year. Any int32 years are valid.</param>
        /// <param name="week">The week number.</param>
        /// <param name="dayOfWeek">The day of the week.</param>
        /// <param name="date">
        /// When this method returns, contains the result <see cref="GameClockDate"/>, or an undefined value on failure.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the date is valid and this method successfully created
        /// a <see cref="GameClockDate"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryFromIsoWeekDate(int year, int week, IsoDayOfWeek dayOfWeek,
            out GameClockDate date)
        {
            YearFlags flags = YearFlags.FromYear(year);
            uint nWeeks = flags.IsoWeekCount;

            if ((dayOfWeek < IsoDayOfWeek.Monday || dayOfWeek > IsoDayOfWeek.Sunday) ||
                (week < 1 || week > nWeeks))
            {
                date = default;
                return false;
            }

            int weekOrd = week * 7 + (int)dayOfWeek;
            int delta = flags.CalcIsoWeekDelta();
            if (weekOrd <= delta)
            {
                // ordinal < 1, previous year
                YearFlags prevFlags = YearFlags.FromYear(year - 1);
                var prevOrdFlags = new OrdFlags(weekOrd + prevFlags.DayCount - delta, prevFlags);
                date = new GameClockDate(year - 1, prevOrdFlags);
                return true;
            }

            int ordinal = weekOrd - delta;
            int nDays = flags.DayCount;
            if (ordinal <= nDays)
            {
                // this year
                date = new GameClockDate(year, new OrdFlags(ordinal, flags));
                return true;
            }

            // ordinal > nDays, next year
            YearFlags nextFlags = YearFlags.FromYear(year + 1);
            var nextOrdFlags = new OrdFlags(ordinal - nDays, nextFlags);
            date = new GameClockDate(year + 1, nextOrdFlags);
            return true;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the year of this <see cref="GameClockDate"/> is a leap one.
        /// </summary>
        public bool IsLeapYear => _ordFlags.IsLeapYear;

        /// <summary>
        /// Adds a specified game clock duration to a specified game clock date, yielding a new date.
        /// Discards the fractional days, rounding to the closest integral number of days towards
        /// <see cref="GameClockDuration.Zero"/>.
        /// </summary>
        /// <param name="date">The date to add.</param>
        /// <param name="duration">The duration to add. Only the whole days will be used.</param>
        /// <returns>
        /// An object whose value is the sum of the <paramref name="date"/> and the whole days of <paramref name="duration"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting <see cref="GameClockDate"/> is less than <see cref="MinValue"/> or greater than
        /// <see cref="MaxValue"/>.
        /// </exception>
        public static GameClockDate operator +(GameClockDate date, GameClockDuration duration)
        {
            if (!date.TryAdd(duration, out GameClockDate result))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(duration));
            }

            return result;
        }

        /// <summary>
        /// Tries to return a new <see cref="GameClockDate"/> that adds the whole days of the specified
        /// <see cref="GameClockDuration"/> to the value of this instance. Discards the fractional days, rounding to
        /// the closest integral number of days towards <see cref="GameClockDuration.Zero"/>.
        /// </summary>
        /// <param name="duration">
        /// A positive or negative game clock duration. Only the whole days will be used.
        /// </param>
        /// <param name="date">
        /// When this method returns, contains the result <see cref="GameClockDate"/>, or an undefined value on failure.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the resulting date is between <see cref="MinValue"/> and <see cref="MaxValue"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryAdd(GameClockDuration duration, out GameClockDate date)
        {
            // dividing int64 secs by a float64 value may result in unintended rounding errors when converting secs
            // into float64.
            long wholeDays = (long)duration.WholeDays;
            long newOrdinal = DayOfYear + wholeDays;

            if (newOrdinal is > 0 and <= 365)
            {
                OrdFlags newOrdFlags = _ordFlags.WithOrdinalUnchecked((uint)newOrdinal);
                date = new GameClockDate(_year, newOrdFlags);
                return true;
            }

            DivModFloor(_year, 400, out int yearDiv400, out int yearMod400);
            long dayCycleUnclamped = Internals.YearOrdinalToDayCycle(yearMod400, DayOfYear) + wholeDays;
            DivModFloor(dayCycleUnclamped, 146_097L, out long cycleDiv400Y, out long dayCycleClamped);
            cycleDiv400Y += yearDiv400;

            Internals.DayCycleToYearOrdinal((int)dayCycleClamped, out yearMod400, out int ordinal);
            long newYear = cycleDiv400Y * 400 + yearMod400;
            if (newYear < int.MinValue || newYear > int.MaxValue)
            {
                date = default;
                return false;
            }

            var of = new OrdFlags(ordinal, YearFlags.FromYearMod400(yearMod400));
            date = new GameClockDate((int)newYear, of);
            return true;
        }

        /// <summary>
        /// Adds a duration in <paramref name="months"/> to the date.
        /// </summary>
        /// <param name="months">The number of months to add. Can be negative.</param>
        /// <returns>
        /// An object whose value is the sum of the date represented by this instance and <paramref name="months"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting <see cref="GameClockDate"/> is less than <see cref="MinValue"/> or greater than
        /// <see cref="MaxValue"/>.
        /// </exception>
        /// <remarks>
        /// Uses the last day of the month if the day does not exist in the resulting month.
        /// </remarks>
        public GameClockDate AddMonths(long months)
        {
            if (!TryAddMonths(months, out GameClockDate date))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(months));
            }

            return date;
        }

        /// <summary>
        /// Tries to add a duration in <paramref name="months"/> to the date.
        /// </summary>
        /// <param name="months">The number of months to add. Can be negative.</param>
        /// <param name="date">
        /// When this method returns, contains the result <see cref="GameClockDate"/>, or an undefined value on failure.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the resulting date is between <see cref="MinValue"/> and <see cref="MaxValue"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// Uses the last day of the month if the day does not exist in the resulting month.
        /// </remarks>
        public bool TryAddMonths(long months, out GameClockDate date)
        {
            if (months == 0)
            {
                date = this;
                return true;
            }

            GameClockDate? nullableDate = DiffMonths(months);
            if (nullableDate.HasValue)
            {
                date = nullableDate.GetValueOrDefault();
                return true;
            }
            else
            {
                date = default;
                return false;
            }
        }

        private GameClockDate? DiffMonths(long months)
        {
            long years = months / 12;
            int left = (int)(months % 12);

            // Determine new year (without taking months into account for now). Return null if the new year is not in
            // the range of int32.
            if ((years > 0 && _year > (int.MaxValue - years)) || (years < 0 && _year < (int.MinValue - years)))
            {
                return null;
            }

            int year = (int)(_year + years);
            int month = Month + left;

            if (month <= 0)
            {
                if (year == int.MinValue)
                {
                    return null;
                }

                year -= 1;
                month += 12;
            }
            else if (month > 12)
            {
                if (year == int.MaxValue)
                {
                    return null;
                }

                year += 1;
                month -= 12;
            }

            // Clamp original day in case new month is shorter
            YearFlags flags = YearFlags.FromYear(year);
            int dayMax = GetMaxDayOfMonth(month, flags);
            int day = Day;
            if (day > dayMax)
            {
                day = dayMax;
            };

            return FromMdfUnchecked(year, new MonthDayFlags(month, day, flags));
        }

        /// <summary>
        /// Tries to return a new <see cref="GameClockDate"/> that subtracts the whole days of the specified
        /// <see cref="GameClockDuration"/> to the value of this instance. Discards the fractional days, rounding to
        /// the closest integral number of days towards <see cref="GameClockDuration.Zero"/>.
        /// </summary>
        /// <param name="duration">
        /// A positive or negative game clock duration. Only the whole days will be used.
        /// </param>
        /// <param name="date">
        /// When this method returns, contains the result <see cref="GameClockDate"/>, or an undefined value on failure.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the resulting date is between <see cref="MinValue"/> and <see cref="MaxValue"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool TrySubtract(GameClockDuration duration, out GameClockDate date)
        {
            bool result = TryAdd(-duration, out date);
            return result;
        }

        /// <summary>
        /// Subtracts a duration in <paramref name="months"/> from the date.
        /// </summary>
        /// <param name="months">The number of months to subtract. Can be negative.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance minus <paramref name="months"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting <see cref="GameClockDate"/> is less than <see cref="MinValue"/> or greater than
        /// <see cref="MaxValue"/>.
        /// </exception>
        /// <remarks>
        /// Uses the last day of the month if the day does not exist in the resulting month.
        /// </remarks>
        public GameClockDate SubtractMonths(long months)
        {
            if (!TrySubtractMonths(months, out GameClockDate date))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(months));
            }

            return date;
        }

        /// <summary>
        /// Tries to subtract a duration in <paramref name="months"/> from the date.
        /// </summary>
        /// <param name="months">The number of months to subtract. Can be negative.</param>
        /// <param name="date">
        /// When this method returns, contains the result <see cref="GameClockDate"/>, or an undefined value on failure.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the resulting date is between <see cref="MinValue"/> and <see cref="MaxValue"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// Uses the last day of the month if the day does not exist in the resulting month.
        /// </remarks>
        public bool TrySubtractMonths(long months, out GameClockDate date)
        {
            // Don't care about long.MinValue as the absolute value is larger than the number of months between
            // GameClockDate.MinValue and GameClockDate.MaxValue. Will return false anyway in such case.
            return TryAddMonths(-months, out date);
        }

        /// <summary>
        /// Subtracts a specified duration from a specified date, yielding a new date.
        /// Discards the fractional days, rounding to the closest integral number of days towards
        /// <see cref="GameClockDuration.Zero"/>.
        /// </summary>
        /// <param name="date">The date to subtract from.</param>
        /// <param name="duration">The duration to subtract. Only the whole days will be used.</param>
        /// <returns>
        /// An object whose value is the <paramref name="date"/> minus the whole days of <paramref name="duration"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting <see cref="GameClockDate"/> is less than <see cref="MinValue"/> or greater than
        /// <see cref="MaxValue"/>.
        /// </exception>
        public static GameClockDate operator -(GameClockDate date, GameClockDuration duration)
        {
            if (!date.TrySubtract(duration, out GameClockDate result))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(duration));
            }

            return result;
        }

        /// <summary>
        /// Subtracts a specified from the current date, yielding a signed duration.
        /// This does not overflow or underflow at all, as all possible output fits in the range of
        /// <see cref="GameClockDuration"/>.
        /// </summary>
        /// <param name="d1">The date value to subtract from (the minuend).</param>
        /// <param name="d2">The date value to subtract (the subtrahend).</param>
        /// <returns>
        /// The signed duration between <paramref name="d1"/> and <paramref name="d2"/>; that is,
        /// <paramref name="d1"/> minus <paramref name="d2"/>.
        /// </returns>
        /// <remarks>
        /// The implementation is a wrapper around <see cref="SignedDurationSince(GameClockDate)"/>.
        /// </remarks>
        public static GameClockDuration operator -(GameClockDate d1, GameClockDate d2) => d1.SignedDurationSince(d2);

        /// <summary>
        /// Makes a new <see cref="GameClockDate"/> with the year number changed, while keeping the same month and day.
        /// </summary>
        /// <param name="year">The new year.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the year is the specified
        /// <paramref name="year"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The date is February 29 in a leap year but the specified <paramref name="year"/> is a non-leap one.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The internal state of this instance is invalid and therefore cannot create a new
        /// <see cref="GameClockDate"/>.
        /// </exception>
        public GameClockDate WithYear(int year)
        {
            // we need to operate with `mdf` since we should keep the month and day number as is
            MonthDayFlags mdf = MonthDayFlags;

            if (mdf.Month == 2 && mdf.Day == 29 && !YearFlags.FromYear(year).IsLeapYear)
            {
                ThrowHelper.ThrowArgumentException("cannot create GameClockDate that represents February 29 in a non-leap year.");
            }

            YearFlags flags = YearFlags.FromYear(year);
            mdf = mdf.WithFlags(flags);

            GameClockDate? newDate = FromMdf(year, mdf);
            if (newDate == null)
            {
                ThrowHelper.ThrowInvalidOperationException("Cannot create a new GameClockDate with a GameClockDate with invalid state (e.g. the default value).");
            }

            return newDate.GetValueOrDefault();
        }

        /// <summary>
        /// Makes a new <see cref="GameClockDate"/> with the month number (starting from 1) changed.
        /// </summary>
        /// <param name="month">The new month.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the month is the specified
        /// <paramref name="month"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified <paramref name="month"/> is invalid (not an integer between 1 and 12).
        /// </exception>
        public GameClockDate WithMonth(int month)
        {
            MonthDayFlags? newMdf = MonthDayFlags.WithMonth(month);
            if (newMdf == null)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(month), month, 1, 12);
            }

            return WithMonthDayFlags(newMdf.GetValueOrDefault());
        }

        /// <summary>
        /// Makes a new <see cref="GameClockDate"/> with the month number (starting from 0) changed.
        /// </summary>
        /// <param name="month0">The new month.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the month is the specified
        /// <paramref name="month0"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified <paramref name="month0"/> is invalid (not an integer between 0 and 11).
        /// </exception>
        public GameClockDate WithMonth0(int month0)
            => WithMonth(month0 + 1);

        /// <summary>
        /// Makes a new <see cref="GameClockDate"/> with the day of month (starting from 1) changed.
        /// </summary>
        /// <param name="day">The new day of month.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the day of month is the specified
        /// <paramref name="day"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified <paramref name="day"/> is invalid.
        /// </exception>
        public GameClockDate WithDay(int day)
        {
            MonthDayFlags? newMdf = MonthDayFlags.WithDay(day);
            if (newMdf == null)
            {
                Throw_InvalidDayNumber(day, MonthDayFlags.Month, _ordFlags);
            }

            return WithMonthDayFlags(newMdf.GetValueOrDefault());

            [MethodImpl(MethodImplOptions.NoInlining)]
            static void Throw_InvalidDayNumber(int day, int month, OrdFlags of)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(day), day, 1, GetMaxDayOfMonth(month, of.Flags));
            }
        }

        /// <summary>
        /// Makes a new <see cref="GameClockDate"/> with the day of month (starting from 0) changed.
        /// </summary>
        /// <param name="day0">The new day of month.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the day of month is the specified
        /// <paramref name="day0"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified <paramref name="day0"/> is invalid.
        /// </exception>
        public GameClockDate WithDay0(int day0)
            => WithDay(day0 + 1);

        /// <summary>
        /// Makes a new <see cref="GameClockDate"/> with the day of year (starting from 1) changed.
        /// </summary>
        /// <param name="dayOfYear">The day of year.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the day of year is
        /// <paramref name="dayOfYear"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified <paramref name="dayOfYear"/> is invalid.
        /// </exception>
        public GameClockDate WithDayOfYear(int dayOfYear)
        {
            OrdFlags? of = _ordFlags.WithOrdinal(dayOfYear);
            if (of == null)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(dayOfYear), dayOfYear, 1, _ordFlags.Flags.DayCount);
            }

            return WithOrdFlags(of.GetValueOrDefault());
        }

        /// <summary>
        /// Makes a new <see cref="GameClockDate"/> with the day of year (starting from 0) changed.
        /// </summary>
        /// <param name="dayOfYear0">The day of year.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the day of year is
        /// <paramref name="dayOfYear0"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified <paramref name="dayOfYear0"/> is invalid.
        /// </exception>
        public GameClockDate WithDayOfYear0(int dayOfYear0)
            => WithDayOfYear(dayOfYear0 + 1);

        private GameClockDate WithOrdFlags(OrdFlags of) => new(_year, of);

        private GameClockDate WithMonthDayFlags(MonthDayFlags mdf)
            => WithOrdFlags(mdf.ToOrdFlags().GetValueOrDefault());

        /// <summary>
        /// Returns a duration subtracted from this instance by <paramref name="value"/>.
        /// This does not throw an exception in any cases, as all possible output fits in the range of
        /// <see cref="GameClockDuration"/>.
        /// </summary>
        /// <param name="value">The date to subtract.</param>
        /// <returns>
        /// A signed duration that is equal to the date represented by this instance minus the date represented by
        /// <paramref name="value"/>.
        /// </returns>
        public GameClockDuration SignedDurationSince(GameClockDate value)
        {
            int year1 = _year;
            int year2 = value._year;
            DivModFloor(year1, 400, out int year1Div400, out int year1Mod400);
            DivModFloor(year2, 400, out int year2Div400, out int year2Mod400);

            long cycle1 = (long)Internals.YearOrdinalToDayCycle(year1Mod400, (int)_ordFlags.Ordinal);
            long cycle2 = (long)Internals.YearOrdinalToDayCycle(year2Mod400, (int)value._ordFlags.Ordinal);
            return GameClockDuration.FromDays(((long)year1Div400 - year2Div400) * 146_097 + (cycle1 - cycle2));
        }

        /// <summary>
        /// Returns the number of whole years from <paramref name="other"/> until this instance.
        /// </summary>
        /// <param name="other">The other date.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="other"/> is later than this instance.
        /// </exception>
        public int YearsSince(GameClockDate other)
        {
            int years = _year - other._year;

            if ((Month << 5 | Day) < (other.Month << 5 | other.Day))
            {
                years -= 1;
            }

            if (years < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(other));
            }
            return years;
        }

        /// <summary>
        /// Deconstructs a time into year, month, and day components.
        /// </summary>
        /// <param name="year">The year component.</param>
        /// <param name="month">The month component.</param>
        /// <param name="day">The day component.</param>
        public void Deconstruct(out int year, out int month, out int day)
        {
            year = _year;
            month = Month;
            day = Day;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="GameClockDate"/>
        /// object.
        /// </summary>
        /// <param name="value">An object to compare with this instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> represents the same game clock date as this instance;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(GameClockDate value)
        {
            return _year == value._year && _ordFlags == value._ordFlags;
        }
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="value">An object to compare with this instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is a <see cref="GameClockDate"/> object that represents
        /// the same game clock date as the current <see cref="GameClockDate"/> structure; otherwise, false.
        /// </returns>
        public override bool Equals(object value)
        {
            if (value is GameClockDate duration)
            {
                return Equals(duration);
            }

            return false;
        }

        public override string ToString() => ToStringInternal();

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe string ToStringInternal()
        {
            unsafe
            {
                // this is the minimum number that is large enough to contain any date string and is multiple of 4
                const int bufferLen = 20;
                char* buffer = stackalloc char[bufferLen];
                GameClockDateTimeFormat.TryFormatDateS(this, buffer, bufferLen, out int written);
                return new string(buffer, 0, written);
            }
        }

        /// <summary>
        /// Indicates whether two <see cref="GameClockDate"/> instances are equal.
        /// </summary>
        /// <param name="left">The game clock date to compare.</param>
        /// <param name="right">The second game date time to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(GameClockDate left, GameClockDate right)
        {
            return left.Equals(right);
        }
        /// <summary>
        /// Indicates whether two <see cref="GameClockDate"/> instances are not equal.
        /// </summary>
        /// <param name="left">The game clock date to compare.</param>
        /// <param name="right">The second game clock date to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(GameClockDate left, GameClockDate right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Compares the value of this instance to a specified object that contains a specified
        /// <see cref="GameClockDate"/> value, and returns an integer that indicates whether this instance is earlier
        /// than, the same as, or later than the specified <see cref="GameClockDate"/> value.
        /// </summary>
        /// <param name="value">A boxed object to compare, or <see langword="null"/>.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and the value parameter. Less than zero if
        /// this instance is earlier than value. Zero if this instance is the same as value. Greater than zero if this
        /// instance is later than value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is not a <see cref="GameClockDate"/>.
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (value is not GameClockDate otherDuration)
                throw new ArgumentException();

            return CompareTo(otherDuration);
        }
        /// <summary>
        /// Compares the value of this instance to a specified <see cref="GameClockDate"/> value and indicates whether
        /// this instance is earlier than, the same as, or later than the specified <see cref="GameClockDate"/> value.
        /// </summary>
        /// <param name="value">The object to compare to the current instance.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and the value parameter.
        /// <list type="bullet">
        /// <item>
        /// <description>Less than zero if this instance is earlier than <paramref name="value"/>.</description>
        /// </item>
        /// <item>
        /// <description>Zero if this instance is the same as <paramref name="value"/>.</description>
        /// </item>
        /// <item>
        /// <description>Greater than zero if this instance is later than <paramref name="value"/>.</description>
        /// </item>
        /// </list>
        /// </returns>
        public int CompareTo(GameClockDate value)
        {
            int thisYear = _year;
            int otherYear = value._year;
            if (thisYear > otherYear)
            {
                return 1;
            }
            else if (thisYear < otherYear)
            {
                return -1;
            }

            uint thisOrdinal = _ordFlags.Ordinal;
            uint otherOrdinal = value._ordFlags.Ordinal;
            if (thisOrdinal > otherOrdinal)
            {
                return 1;
            }
            else if (thisOrdinal < otherOrdinal)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="GameClockDate"/> is earlier than another specified
        /// <see cref="GameClockDate"/>.
        /// </summary>
        /// <param name="left">The first game clock date to compare.</param>
        /// <param name="right">The second game clock date to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="left"/> is earlier than the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <(GameClockDate left, GameClockDate right) => left.CompareTo(right) < 0;
        /// <summary>
        /// Indicates whether a specified <see cref="GameClockDate"/> is earlier than or equal to another specified
        /// <see cref="GameClockDate"/>.
        /// </summary>
        /// <param name="left">The first game clock date to compare.</param>
        /// <param name="right">The second game clock date to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="left"/> is earlier than or equal to the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <=(GameClockDate left, GameClockDate right) => left.CompareTo(right) <= 0;

        /// <summary>
        /// Indicates whether a specified <see cref="GameClockDate"/> is later than another specified
        /// <see cref="GameClockDate"/>.
        /// </summary>
        /// <param name="left">The first game clock date to compare.</param>
        /// <param name="right">The second game clock date to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="left"/> is later than the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >(GameClockDate left, GameClockDate right) => left.CompareTo(right) > 0;
        /// <summary>
        /// Indicates whether a specified <see cref="GameClockDate"/> is later than or equal to another specified
        /// <see cref="GameClockDate"/>.
        /// </summary>
        /// <param name="left">The first game clock date to compare.</param>
        /// <param name="right">The second game clock date to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="left"/> is later than or equal to the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >=(GameClockDate left, GameClockDate right) => left.CompareTo(right) >= 0;

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return _year.GetHashCode() + 17 * _ordFlags.GetHashCode();
        }

        private static int GetMaxDayOfMonth(int month, YearFlags flags)
        {
            return month switch
            {
                2 => (flags.IsLeapYear ? 29 : 28),
                4 or 6 or 9 or 11 => 30,
                _ => 31,
            };
        }

        private static void DivModFloor(int val, int div, out int quotient, out int modulo)
        {
            quotient = val.DivEuclid(div);
            modulo = val.RemEuclid(div);
        }

        private static void DivModFloor(long val, long div, out long quotient, out long modulo)
        {
            quotient = val.DivEuclid(div);
            modulo = val.RemEuclid(div);
        }
    }
}
