//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using GTA.Chrono;
using Xunit;

namespace ScriptHookVDotNet_APIv3_Tests
{
    public class GameClockDateTests
    {
        [Fact]
        public void MaxValue()
        {
            VerifyClockDateYmd(GameClockDate.MaxValue, int.MaxValue, 12, 31);
        }

        [Fact]
        public void MinValue()
        {
            VerifyClockDateYmd(GameClockDate.MinValue, int.MinValue, 1, 1);
        }

        public static TheoryData<int, int, int> Ymd_Successful_Data =>
            new TheoryData<int, int, int>
            {
                { int.MinValue, 1, 1 },
                { 2001, 10, 22 },
                { 2004, 10, 26 },
                { 2008, 4, 29 },
                { 2012, 2, 29 },
                { 2014, 3, 1 },
                { 2014, 3, 31 },
                { int.MaxValue, 12, 31 },
            };

        public static TheoryData<int, int, int> Ymd_Invalid_Data =>
            new TheoryData<int, int, int>
            {
                { 2015, 0, 1 },
                { 2015, 2, 29 },
                { 2023, 3, 0 },
                { 2025, 13, 1 },
            };

        /// <summary>
        /// Invalid YMD data that can block fast, because the month is not between 1 and 12 inclusive or the day is not
        /// between 1 and 31 inclusive.
        /// </summary>
        public static TheoryData<int, int, int> Ymd_Invalid_Data_That_Can_Block_Fast =>
            new TheoryData<int, int, int>
            {
                { 2015, 0, 1 },
                { 2015, 2, 32 },
                { 2023, 3, 0 },
                { 2025, 13, 1 },
            };

        /// <summary>
        /// Invalid YMD data that cannot block fast, because the month is between 1 and 12 inclusive and the day is
        /// between 1 and 31 inclusive.
        /// </summary>
        public static TheoryData<int, int, int> Ymd_Invalid_Data_That_Cannot_Block_Fast =>
            new TheoryData<int, int, int>
            {
                { 2018, 2, 29 },
                { 2015, 2, 30 },
                { 2015, 2, 31 },
                { 2015, 4, 31 },
                { 2015, 6, 31 },
                { 2015, 9, 31 },
                { 2015, 11, 31 },
            };

        [Theory]
        [MemberData(nameof(Ymd_Successful_Data))]
        public void FromYmd_returns_expected_value_for_valid_date(int year, int month, int day)
        {
            GameClockDate date = GameClockDate.FromYmd(year, month, day);
            VerifyClockDateYmd(date, year, month, day);
        }

        [Theory]
        [MemberData(nameof(Ymd_Successful_Data))]
        public void TryFromYmd_returns_true_and_set_expected_value_to_out_date_for_valid_date(int year, int month,
            int day)
        {
            bool constructedValidDate = GameClockDate.TryFromYmd(year, month, day, out GameClockDate date);

            Assert.True(constructedValidDate);
            Assert.NotEqual(default, date);

            VerifyClockDateYmd(date, year, month, day);
        }

        [Theory]
        [MemberData(nameof(Ymd_Invalid_Data))]
        public void FromYmd_throws_ArgumentOutOfRange_for_invalid_date(int year, int month, int day)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => GameClockDate.FromYmd(year, month, day));
        }

        [Theory]
        [MemberData(nameof(Ymd_Invalid_Data_That_Can_Block_Fast))]
        public void MonthDayFlags_New_returns_null_for_invalid_date_whose_month_is_not_between_1_and_12_or_whose_day_is_not_between_1_and_31(
            int year, int month, int day)
        {
            YearFlags yf = YearFlags.FromYear(year);
            MonthDayFlags? mdf = MonthDayFlags.New(month, day, yf);

            Assert.Null(mdf);

            bool constructedValidDate = GameClockDate.TryFromYmd(year, month, day, out _);

            Assert.False(constructedValidDate);
        }

        [Theory]
        [MemberData(nameof(Ymd_Invalid_Data_That_Cannot_Block_Fast))]
        public void MonthDayFlags_New_returns_non_null_for_invalid_date_whose_month_is_between_1_and_12_and_whose_day_is_between_1_and_31(
            int year, int month, int day)
        {
            YearFlags yf = YearFlags.FromYear(year);
            MonthDayFlags? mdf = MonthDayFlags.New(month, day, yf);

            Assert.NotNull(mdf);

            bool constructedValidDate = GameClockDate.TryFromYmd(year, month, day, out _);

            Assert.False(constructedValidDate);
        }

        public static TheoryData<int, int, GameClockDate> Yo_Successful_Data =>
            new TheoryData<int, int, GameClockDate>
            {
                { 2008, 1, GameClockDate.FromYmd(2008, 1, 1) },
                { 2008, 2, GameClockDate.FromYmd(2008, 1, 2) },
                { 2008, 32, GameClockDate.FromYmd(2008, 2, 1) },
                { 2008, 60, GameClockDate.FromYmd(2008, 2, 29) },
                { 2008, 61, GameClockDate.FromYmd(2008, 3, 1) },
                { 2008, 100, GameClockDate.FromYmd(2008, 4, 9) },
                { 2008, 200, GameClockDate.FromYmd(2008, 7, 18) },
                { 2008, 300, GameClockDate.FromYmd(2008, 10, 26) },
                { 2008, 366, GameClockDate.FromYmd(2008, 12, 31) },
                { 2015, 1, GameClockDate.FromYmd(2015, 1, 1) },
                { 2015, 2, GameClockDate.FromYmd(2015, 1, 2) },
                { 2015, 32, GameClockDate.FromYmd(2015, 2, 1) },
                { 2015, 59, GameClockDate.FromYmd(2015, 2, 28) },
                { 2015, 60, GameClockDate.FromYmd(2015, 3, 1) },
                { 2015, 100, GameClockDate.FromYmd(2015, 4, 10) },
                { 2015, 200, GameClockDate.FromYmd(2015, 7, 19) },
                { 2015, 300, GameClockDate.FromYmd(2015, 10, 27) },
                { 2015, 365, GameClockDate.FromYmd(2015, 12, 31) },
            };

        public static TheoryData<int, int> Yo_Invalid_Data =>
            new TheoryData<int, int>
            {
                { 2008, 0 },
                { 2008, 367 },
                { 2015, 0 },
                { 2015, 366 },
            };

        [Theory]
        [MemberData(nameof(Yo_Successful_Data))]
        public void FromOrdinalDate_returns_expected_value_for_valid_date(int year, int ordinal, GameClockDate expectedDate)
        {
            GameClockDate actualDate = GameClockDate.FromOrdinalDate(year, ordinal);
            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [MemberData(nameof(Yo_Successful_Data))]
        public void TryFromOrdinalDate_returns_true_and_set_expected_value_to_out_date_for_valid_date(int year, int ordinal,
            GameClockDate expectedDate)
        {
            bool constructedValidDate = GameClockDate.TryFromOrdinalDate(year, ordinal, out GameClockDate actualDate);

            Assert.True(constructedValidDate);
            Assert.NotEqual(default, actualDate);

            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [MemberData(nameof(Yo_Invalid_Data))]
        public void FromOrdinalDate_throws_ArgumentOutOfRange_for_invalid_date(int year, int ordinal)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => GameClockDate.FromOrdinalDate(year, ordinal));
        }

        [Theory]
        [MemberData(nameof(Yo_Invalid_Data))]
        public void TryFromOrdinalDate_returns_false_and_set_default_value_to_out_date_for_invalid_date(int year, int ordinal)
        {
            bool constructedValidDate = GameClockDate.TryFromOrdinalDate(year, ordinal, out GameClockDate date);

            Assert.False(constructedValidDate);
            Assert.Equal(default, date);
        }

        public static TheoryData<int, int, IsoDayOfWeek, GameClockDate> IsoYwd_Successful_Data =>
            new TheoryData<int, int, IsoDayOfWeek, GameClockDate>
            {
                { 2004, 1, IsoDayOfWeek.Monday, GameClockDate.FromYmd(2003, 12, 29) },
                { 2004, 1, IsoDayOfWeek.Sunday, GameClockDate.FromYmd(2004, 1, 4) },
                { 2004, 2, IsoDayOfWeek.Monday, GameClockDate.FromYmd(2004, 1, 5) },
                { 2004, 2, IsoDayOfWeek.Sunday, GameClockDate.FromYmd(2004, 1, 11) },
                { 2004, 52, IsoDayOfWeek.Monday, GameClockDate.FromYmd(2004, 12, 20) },
                { 2004, 52, IsoDayOfWeek.Sunday, GameClockDate.FromYmd(2004, 12, 26) },
                { 2004, 53, IsoDayOfWeek.Monday, GameClockDate.FromYmd(2004, 12, 27) },
                { 2004, 53, IsoDayOfWeek.Sunday, GameClockDate.FromYmd(2005, 1, 2) },
                { 2011, 1, IsoDayOfWeek.Monday, GameClockDate.FromYmd(2011, 1, 3) },
                { 2011, 1, IsoDayOfWeek.Sunday, GameClockDate.FromYmd(2011, 1, 9) },
                { 2011, 2, IsoDayOfWeek.Monday, GameClockDate.FromYmd(2011, 1, 10) },
                { 2011, 2, IsoDayOfWeek.Sunday, GameClockDate.FromYmd(2011, 1, 16) },
                { 2018, 51, IsoDayOfWeek.Monday, GameClockDate.FromYmd(2018, 12, 17) },
                { 2018, 51, IsoDayOfWeek.Sunday, GameClockDate.FromYmd(2018, 12, 23) },
                { 2018, 52, IsoDayOfWeek.Monday, GameClockDate.FromYmd(2018, 12, 24) },
                { 2018, 52, IsoDayOfWeek.Sunday, GameClockDate.FromYmd(2018, 12, 30) },
            };

        public static TheoryData<int, int, IsoDayOfWeek> IsoYwd_Invalid_Data =>
            new TheoryData<int, int, IsoDayOfWeek>
            {
                { 2004, 0, IsoDayOfWeek.Sunday },
                { 2004, 54, IsoDayOfWeek.Monday },
                { 2011, 0, IsoDayOfWeek.Sunday },
                { 2018, 53, IsoDayOfWeek.Monday },
            };

        [Theory]
        [MemberData(nameof(IsoYwd_Successful_Data))]
        public void FromIsoWeekDate_returns_expected_value_for_valid_date(int year, int week, IsoDayOfWeek weekday, GameClockDate expectedDate)
        {
            GameClockDate actualDate = GameClockDate.FromIsoWeekDate(year, week, weekday);
            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [MemberData(nameof(IsoYwd_Successful_Data))]
        public void TryFromIsoWeekDate_returns_true_and_set_expected_value_to_out_date_for_valid_date(int year, int week,
            IsoDayOfWeek weekday, GameClockDate expectedDate)
        {
            bool constructedValidDate = GameClockDate.TryFromIsoWeekDate(year, week, weekday, out GameClockDate actualDate);

            Assert.True(constructedValidDate);
            Assert.NotEqual(default, actualDate);

            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [MemberData(nameof(IsoYwd_Invalid_Data))]
        public void FromIsoWeekDate_throws_ArgumentOutOfRange_for_invalid_date(int year, int week, IsoDayOfWeek weekday)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => GameClockDate.FromIsoWeekDate(year, week, weekday));
        }

        [Theory]
        [MemberData(nameof(IsoYwd_Invalid_Data))]
        public void TryFromIsoWeekDate_returns_false_and_set_default_value_to_out_date_for_invalid_date(int year, int week,
            IsoDayOfWeek weekday)
        {
            bool constructedValidDate = GameClockDate.TryFromIsoWeekDate(year, week, weekday, out GameClockDate date);

            Assert.False(constructedValidDate);
            Assert.Equal(default, date);
        }

        const long MaxDayFromMinDate = 1_568_704_592_609;

        public static TheoryData<GameClockDate, GameClockDuration, GameClockDate> Add_Duration_Successful_Data =>
            new TheoryData<GameClockDate, GameClockDuration, GameClockDate>
            {
                { GameClockDate.FromYmd(2018, 1, 1), GameClockDuration.Zero, GameClockDate.FromYmd(2018, 1, 1) },
                { GameClockDate.FromYmd(2018, 1, 1), GameClockDuration.FromSeconds(86399), GameClockDate.FromYmd(2018, 1, 1) },
                { GameClockDate.FromYmd(2018, 1, 1), GameClockDuration.FromSeconds(-86399), GameClockDate.FromYmd(2018, 1, 1) },
                { GameClockDate.FromYmd(2018, 1, 1), GameClockDuration.FromDays(1), GameClockDate.FromYmd(2018, 1, 2) },
                { GameClockDate.FromYmd(2018, 1, 1), GameClockDuration.FromDays(-1), GameClockDate.FromYmd(2017, 12, 31) },
                { GameClockDate.FromYmd(2018, 1, 1), GameClockDuration.FromDays(364), GameClockDate.FromYmd(2018, 12, 31) },
                { GameClockDate.FromYmd(2018, 1, 1), GameClockDuration.FromDays(365 * 4 + 1), GameClockDate.FromYmd(2022, 1, 1) },
                { GameClockDate.FromYmd(2018, 1, 1), GameClockDuration.FromDays(365 * 400 + 97), GameClockDate.FromYmd(2418, 1, 1) },
                { GameClockDate.FromYmd(int.MinValue, 1, 1), GameClockDuration.FromDays(MaxDayFromMinDate), GameClockDate.FromYmd(int.MaxValue, 12, 31) },
                { GameClockDate.FromYmd(int.MaxValue, 12, 31), GameClockDuration.FromDays(-MaxDayFromMinDate), GameClockDate.FromYmd(int.MinValue, 1, 1) },
                { GameClockDate.FromYmd(int.MaxValue, 12, 31), GameClockDuration.MinValue, GameClockDate.FromYmd(int.MinValue, 1, 1) },
            };

        public static TheoryData<GameClockDate, GameClockDuration> Add_Duration_Invalid_Data =>
            new TheoryData<GameClockDate, GameClockDuration>
            {
                { GameClockDate.FromYmd(0, 1, 1), GameClockDuration.FromDays(MaxDayFromMinDate) },
                { GameClockDate.MaxValue, GameClockDuration.FromDays(1) },
                { GameClockDate.MinValue, GameClockDuration.FromDays(-1) },
            };

        public static TheoryData<GameClockDate, GameClockDuration> Subtract_Duration_Successful_Data =>
            new TheoryData<GameClockDate, GameClockDuration>
            {
                { GameClockDate.FromYmd(2024, 1, 1), GameClockDuration.Zero },
                { GameClockDate.FromYmd(2024, 1, 1), GameClockDuration.FromSeconds(86399) },
                { GameClockDate.FromYmd(2024, 1, 1), GameClockDuration.FromSeconds(-86399) },
                { GameClockDate.FromYmd(2024, 1, 1), GameClockDuration.FromDays(1) },
                { GameClockDate.FromYmd(2024, 1, 1), GameClockDuration.FromDays(-1) },
                { GameClockDate.FromYmd(2024, 1, 1), GameClockDuration.FromDays(365) },
            };

        [Theory]
        [MemberData(nameof(Add_Duration_Successful_Data))]
        public void Adding_duration_to_date_adds_only_whole_day_of_duration(GameClockDate date, GameClockDuration duration, GameClockDate expected)
        {
            GameClockDate actual = date + duration;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Add_Duration_Successful_Data))]
        public void TryAdd_GameClockDuration_returns_true_and_set_expected_value_to_out_date_within_expected_range(GameClockDate date, GameClockDuration duration, GameClockDate expected)
        {
            bool constructedValidDate = date.TryAdd(duration, out GameClockDate actualDate);

            Assert.True(constructedValidDate);
            Assert.NotEqual(default, actualDate);

            Assert.Equal(expected, actualDate);
        }

        [Theory]
        [MemberData(nameof(GameClockDateTests.Add_Duration_Invalid_Data))]
        public void Adding_excessive_duration_fails(GameClockDate date, GameClockDuration duration)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => date + duration);

            bool constructedValidDate = date.TryAdd(duration, out GameClockDate actualDate);

            Assert.False(constructedValidDate);
            Assert.Equal(default, actualDate);
        }

        [Theory]
        [MemberData(nameof(Subtract_Duration_Successful_Data))]
        public void Subtraction_of_duration_is_the_same_as_addition_of_negated_duration(GameClockDate date, GameClockDuration duration)
        {
            GameClockDate expected = date + (-duration);

            GameClockDate actual = date - duration;

            Assert.Equal(expected, actual);
        }

        const long MaxMonthFromMinDateToMaxDateExclusive = 51_539_607_552;//((long)uint.MaxValue + 1) * 12;

        public static TheoryData<GameClockDate, long, GameClockDate> Diff_Months_Valid_Data =>
            new TheoryData<GameClockDate, long, GameClockDate>
            {
                { GameClockDate.FromYmd(2023, 10, 23), 0, GameClockDate.FromYmd(2023, 10, 23) },
                { GameClockDate.FromYmd(2023, 10, 23), -2050 * 12, GameClockDate.FromYmd(-27, 10, 23) },
                { GameClockDate.FromYmd(2023, 10, 23), 6, GameClockDate.FromYmd(2024, 4, 23) },
                { GameClockDate.FromYmd(2023, 6, 23), -8, GameClockDate.FromYmd(2022, 10, 23) },
                { GameClockDate.FromYmd(2023, 1, 29), 1, GameClockDate.FromYmd(2023, 2, 28) },
                { GameClockDate.FromYmd(2023, 10, 29), 4, GameClockDate.FromYmd(2024, 2, 29) },
                { GameClockDate.FromYmd(2023, 10, 31), 2, GameClockDate.FromYmd(2023, 12, 31) },
                { GameClockDate.FromYmd(2023, 10, 31), -10, GameClockDate.FromYmd(2022, 12, 31) },
                { GameClockDate.FromYmd(2023, 8, 3), 5, GameClockDate.FromYmd(2024, 1, 3) },
                { GameClockDate.FromYmd(2023, 8, 3), -7, GameClockDate.FromYmd(2023, 1, 3) },
                { GameClockDate.MinValue, MaxMonthFromMinDateToMaxDateExclusive - 1, GameClockDate.FromYmd(int.MaxValue, 12, 1) },
                { GameClockDate.MaxValue, -(MaxMonthFromMinDateToMaxDateExclusive - 1), GameClockDate.FromYmd(int.MinValue, 1, 31) },
            };

        public static TheoryData<GameClockDate, long> Diff_Months_Invalid_Data =>
            new TheoryData<GameClockDate, long>
            {
                { GameClockDate.FromYmd(0, 1, 1), MaxMonthFromMinDateToMaxDateExclusive },
                { GameClockDate.MinValue, MaxMonthFromMinDateToMaxDateExclusive },
                { GameClockDate.MaxValue, -MaxMonthFromMinDateToMaxDateExclusive },
            };


        [Theory]
        [MemberData(nameof(Diff_Months_Valid_Data))]
        public void Adding_months_truncates_excess_days_and_does_not_normalize(GameClockDate date, long months, GameClockDate expected)
        {
            GameClockDate actualDate = date.AddMonths(months);

            Assert.Equal(expected, actualDate);
        }

        [Theory]
        [MemberData(nameof(Diff_Months_Valid_Data))]
        public void Subtracting_months_is_the_same_as_adding_negated_months(GameClockDate date, long months, GameClockDate expected)
        {
            GameClockDate actual = date.SubtractMonths(-months);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Diff_Months_Invalid_Data))]
        public void Adding_too_many_months_into_earlier_than_min_or_later_than_max_date_fails(GameClockDate date, long months)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => date.AddMonths(months));

            bool constructedValidDate = date.TryAddMonths(months, out GameClockDate actualDate);

            Assert.False(constructedValidDate);
            Assert.Equal(default, actualDate);
        }

        public static TheoryData<GameClockDate, GameClockDate, GameClockDuration> Signed_Duration_Since_Data =>
            new TheoryData<GameClockDate, GameClockDate, GameClockDuration>
            {
                { GameClockDate.FromYmd(2014, 1, 1), GameClockDate.FromYmd(2014, 1, 1), GameClockDuration.Zero },
                { GameClockDate.FromYmd(2014, 1, 2), GameClockDate.FromYmd(2014, 1, 1), GameClockDuration.FromDays(1) },
                { GameClockDate.FromYmd(2014, 12, 31), GameClockDate.FromYmd(2014, 1, 1), GameClockDuration.FromDays(364) },
                { GameClockDate.FromYmd(2015, 1, 3), GameClockDate.FromYmd(2014, 1, 1), GameClockDuration.FromDays(365 + 2) },
                { GameClockDate.FromYmd(2018, 1, 1), GameClockDate.FromYmd(2014, 1, 1), GameClockDuration.FromDays(365 * 4 + 1) },
                { GameClockDate.FromYmd(2414, 1, 1), GameClockDate.FromYmd(2014, 1, 1), GameClockDuration.FromDays(365 * 400 + 97)  },
                { GameClockDate.MaxValue, GameClockDate.MinValue, GameClockDuration.FromDays(MaxDayFromMinDate) },
                { GameClockDate.MinValue, GameClockDate.MaxValue, GameClockDuration.FromDays(-MaxDayFromMinDate) },
            };

        [Theory]
        [MemberData(nameof(Signed_Duration_Since_Data))]
        public void Can_calculate_signed_duration_from_two_arbitrary_dates(GameClockDate endDate, GameClockDate startDate, GameClockDuration expectedPositive)
        {
            GameClockDuration actualPositive = endDate.SignedDurationSince(startDate);
            GameClockDuration actualNegative = startDate.SignedDurationSince(endDate);

            Assert.Equal(expectedPositive, actualPositive);
            Assert.Equal(-expectedPositive, actualNegative);
        }

        [Theory]
        [MemberData(nameof(Deconstruct_Method_TestData))]
        public void Can_deconstruct_into_separate_year_month_day_ints(int year, int month, int day)
        {
            GameClockDate date = GameClockDate.FromYmd(year, month, day);
            (int obtainedYear, int obtainedMonth, int obtainedDay) = date;

            Assert.Equal(obtainedYear, date.Year);
            Assert.Equal(obtainedMonth, date.Month);
            Assert.Equal(obtainedDay, date.Day);
        }

        public static IEnumerable<object[]> TryAdd_Failure_TestData()
        {
            const long maxDayDiffExclusive = 1_568_704_592_245;
            yield return new object[] { GameClockDate.MinValue, GameClockDuration.FromDays(-1) };
            yield return new object[] { GameClockDate.MaxValue, GameClockDuration.FromDays(1) };
            yield return new object[] { GameClockDate.FromYmd(1, 1, 1),
                GameClockDuration.FromDays(-maxDayDiffExclusive) };
            yield return new object[] { GameClockDate.FromYmd(1, 1, 1),
                GameClockDuration.FromDays(maxDayDiffExclusive) };
        }

        [Theory]
        [MemberData(nameof(TryAdd_Failure_TestData))]
        public void TryAdd_returns_false_when_the_result_is_earlier_than_min_or_later_than_max(GameClockDate date,
            GameClockDuration duration)
        {
            Assert.False(date.TryAdd(duration, out _));
        }

        [Theory]
        [MemberData(nameof(WithOrdinal_Successful_Data))]
        public void WithDayOfYear_with_valid_ordinal_successfully_creates_a_new_GameClockDate(GameClockDate date,
            IEnumerable<int> expectedOrdinals)
        {
            foreach (int ordinal in expectedOrdinals)
            {
                GameClockDate newDate = date.WithDayOfYear(ordinal);
                Assert.Equal(ordinal, newDate.DayOfYear);
            }
        }

        [Theory]
        [MemberData(nameof(WithOrdinal_Failure_Data))]
        public void WithDayOfYear_with_invalid_ordinal_throws_ArgumentOutOfRange(GameClockDate date,
            IEnumerable<int> unexpectedOrdinals)
        {
            foreach (int ordinal in unexpectedOrdinals)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => date.WithDayOfYear(ordinal));
            }
        }

        public static TheoryData<GameClockDate, IEnumerable<int>> WithOrdinal_Successful_Data =>
            new TheoryData<GameClockDate, IEnumerable<int>>
            {
                { GameClockDate.FromYmd(1900, 4, 12), new int[] {1, 60, 61, 186, 365} },
                { GameClockDate.FromYmd(2000, 8, 14), new int[] {1, 99, 137, 211, 365, 366 } },
                { GameClockDate.FromYmd(2001, 10, 22), new int[] {1, 60, 61, 365} },
                { GameClockDate.FromYmd(2004, 10, 26), new int[] {1, 60, 61, 365, 366} },
                { GameClockDate.FromYmd(2008, 4, 29), new int[] {1, 60, 61, 365, 366} },
                { GameClockDate.FromYmd(2013, 9, 17), new int[] {1, 60, 61, 113, 279, 365} },
            };

        public static TheoryData<GameClockDate, IEnumerable<int>> WithOrdinal_Failure_Data =>
            new TheoryData<GameClockDate, IEnumerable<int>>
            {
                { GameClockDate.FromYmd(1900, 4, 12), new int[] {-1, 0, 366, int.MaxValue } },
                { GameClockDate.FromYmd(2000, 8, 14), new int[] {int.MinValue, 367 } },
                { GameClockDate.FromYmd(2008, 4, 29), new int[] { 0, 367 } },
            };

        public static TheoryData<GameClockDate, DayOfWeek> SystemDayOfWeek_Prop_Valid_Data =>
            new TheoryData<GameClockDate, DayOfWeek>
            {
                { GameClockDate.FromYmd(1582, 10, 15), DayOfWeek.Friday },
                { GameClockDate.FromYmd(1875, 5, 20), DayOfWeek.Thursday },
                { GameClockDate.FromYmd(2000, 1, 1), DayOfWeek.Saturday },
            };

        public static TheoryData<GameClockDate, IsoDayOfWeek> IsoDayOfWeek_Prop_Valid_Data =>
            new TheoryData<GameClockDate, IsoDayOfWeek>
            {
                { GameClockDate.FromYmd(1582, 10, 15), IsoDayOfWeek.Friday },
                { GameClockDate.FromYmd(1875, 5, 20), IsoDayOfWeek.Thursday },
                { GameClockDate.FromYmd(2000, 1, 1), IsoDayOfWeek.Saturday },
            };

        [Theory]
        [MemberData(nameof(SystemDayOfWeek_Prop_Valid_Data))]
        public void DayOfYear_returns_expected_value(GameClockDate date,
            DayOfWeek weekday)
        {
            Assert.Equal(weekday, date.DayOfWeek);
        }

        [Theory]
        [MemberData(nameof(IsoDayOfWeek_Prop_Valid_Data))]
        public void IsoDayOfYear_returns_expected_value(GameClockDate date,
            IsoDayOfWeek isoWeekday)
        {
            Assert.Equal(isoWeekday, date.IsoDayOfWeek);
        }

        [Theory]
        [InlineData(-2147483648, true)]
        [InlineData(2008, true)]
        [InlineData(2000, true)]
        [InlineData(1900, false)]
        [InlineData(2013, false)]
        [InlineData(2147483647, false)]
        public void IsLeapYear_Invoke_ReturnsExpected(int year, bool expected)
        {
            GameClockDate date = GameClockDate.FromYmd(year, 2, 28);
            Assert.Equal(expected, date.IsLeapYear);
        }

        public static TheoryData<GameClockDate, string> ToString_Typical_NonNegative_4Digit_Year_Data =>
            new()
            {
                { GameClockDate.FromYmd(0, 1, 1), "0000-01-01" },
                { GameClockDate.FromYmd(30, 9, 17), "0030-09-17" },
                { GameClockDate.FromYmd(700, 6, 30), "0700-06-30" },
                { GameClockDate.FromYmd(1900, 1, 1), "1900-01-01" },
                { GameClockDate.FromYmd(2500, 12, 31), "2500-12-31" },
                { GameClockDate.FromYmd(9999, 12, 31), "9999-12-31" },
            };

        public static TheoryData<GameClockDate, string> ToString_Negative_4Digit_Year_Data =>
            new()
            {
                { GameClockDate.FromYmd(-1, 1, 1), "-0001-01-01" },
                { GameClockDate.FromYmd(-1900, 10, 26), "-1900-10-26" },
                { GameClockDate.FromYmd(-9999, 12, 31), "-9999-12-31" },
            };

        public static TheoryData<GameClockDate, string> ToString_More_Than_4_Digit_Year_Data =>
            new()
            {
                { GameClockDate.FromYmd(10000, 1, 1), "10000-01-01" },
                { GameClockDate.FromYmd(-10000, 12, 31), "-10000-12-31" },
                { GameClockDate.FromYmd(2147483647, 12, 31), "2147483647-12-31" },
                { GameClockDate.FromYmd(-2147483648, 1, 1), "-2147483648-01-01" },
            };

        [Theory]
        [MemberData(nameof(ToString_Typical_NonNegative_4Digit_Year_Data))]
        public void ToString_with_no_params_returns_string_with_4_digit_year_2_digit_month_and_2_digit_day_concatenated_with_hyphen_separator_if_year_is_between_0_and_9999(
            GameClockDate date, string expected)
        {
            string actual = date.ToString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(ToString_Negative_4Digit_Year_Data))]
        public void
        ToString_with_no_params_returns_string_with_leading_neg_sign_and_4_digit_year_if_year_is_between_neg_9999_and_neg_1(
            GameClockDate date, string expected)
        {
            string actual = date.ToString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(ToString_More_Than_4_Digit_Year_Data))]
        public void
            ToString_with_no_params_returns_string_with_more_than_5_digit_year_if_year_is_not_between_neg_9999_and_pos_9999(
                GameClockDate date, string expected)
        {
            string actual = date.ToString();

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> Deconstruct_Method_TestData()
        {
            yield return new object[] { int.MinValue, 1, 1 };
            yield return new object[] { int.MinValue, 10, 22 };
            yield return new object[] { -61474848, 10, 29 };
            yield return new object[] { -2004, 10, 26 };
            yield return new object[] { 1986, 8, 15 };
            yield return new object[] { 1986, 2, 28 };
            yield return new object[] { 1986, 12, 31 };
            yield return new object[] { 2000, 2, 28 };
            yield return new object[] { 2000, 2, 29 };
            yield return new object[] { 2000, 12, 31 };
            yield return new object[] { 1900, 2, 28 };
            yield return new object[] { 1900, 12, 31 };
            yield return new object[] { 83234513, 4, 29 };
            yield return new object[] { int.MaxValue, 9, 17 };
            yield return new object[] { int.MaxValue, 12, 31 };
        }

        private static void VerifyClockDateYmd(GameClockDate gameClockDate, int year, int month, int day)
        {
            Assert.Equal(year, gameClockDate.Year);
            Assert.Equal(month, gameClockDate.Month);
            Assert.Equal(day, gameClockDate.Day);
        }
    }
}
