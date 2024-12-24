//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using Xunit;
using GTA.Chrono;
using System;

namespace ScriptHookVDotNet_APIv3_Tests
{
    public class GameClockDateTimeTests
    {
        [Fact]
        public void MaxValue()
        {
            VerifyGameClockDateTimeYmdHms(GameClockDateTime.MaxValue, int.MaxValue, 12, 31, 23, 59, 59);
        }

        [Fact]
        public void MinValue()
        {
            VerifyGameClockDateTimeYmdHms(GameClockDateTime.MinValue, int.MinValue, 1, 1, 0, 0, 0);
        }

        public static TheoryData<GameClockDateTime, GameClockDuration, GameClockDateTime> Add_Duration_Valid_TestData =>
            new TheoryData<GameClockDateTime, GameClockDuration, GameClockDateTime>
            {
                { YmdHms(2014, 5, 6, 7, 8, 9), GameClockDuration.FromSeconds(3600 + 60 + 1),
                    YmdHms(2014, 5, 6, 8, 9, 10) },
                { YmdHms(2014, 5, 6, 7, 8, 9), GameClockDuration.FromSeconds(-(3600 + 60 + 1)),
                    YmdHms(2014, 5, 6, 6, 7, 8) },
                { YmdHms(2014, 5, 6, 7, 8, 9), GameClockDuration.FromSeconds(86399),
                    YmdHms(2014, 5, 7, 7, 8, 8) },
                { YmdHms(2014, 5, 6, 7, 8, 9), GameClockDuration.FromSeconds(86_400 * 10),
                    YmdHms(2014, 5, 16, 7, 8, 9) },
                { YmdHms(2014, 5, 6, 7, 8, 9), GameClockDuration.FromSeconds(-86_400 * 10),
                    YmdHms(2014, 4, 26, 7, 8, 9) },
            };

        [Theory]
        [MemberData(nameof(Add_Duration_Valid_TestData))]
        public void Addition_of_duration_to_GameClockDateTime_can_change_both_date_and_time(GameClockDateTime dateTime,
            GameClockDuration duration, GameClockDateTime expected)
        {
            GameClockDateTime actual = dateTime + duration;
            bool tryAddSuccessfullyConstructedDateTime = dateTime.TryAdd(duration, out GameClockDateTime actualTryAdd);

            Assert.Equal(expected, actual);

            Assert.True(tryAddSuccessfullyConstructedDateTime);
            Assert.Equal(expected, actualTryAdd);
        }

        [Theory]
        [MemberData(nameof(Add_Duration_Valid_TestData))]
        public void Subtraction_of_duration_from_GameClockDateTime_is_the_same_as_addition_of_negated_duration(
            GameClockDateTime dateTime, GameClockDuration duration, GameClockDateTime expected)
        {
            GameClockDateTime actual = dateTime - (-duration);
            bool trySubtractSuccessfullyConstructedDateTime
                = dateTime.TrySubtract(-duration, out GameClockDateTime actualTrySubtract);

            Assert.Equal(expected, actual);

            Assert.True(trySubtractSuccessfullyConstructedDateTime);
            Assert.Equal(expected, actualTrySubtract);
        }

        const long MaxDayFromMinDate = 1_568_704_592_609;

        public static TheoryData<GameClockDateTime, GameClockDateTime, GameClockDuration> Signed_Duration_Since_Data =>
            new TheoryData<GameClockDateTime, GameClockDateTime, GameClockDuration>
            {
                { YmdHms(2014, 5, 6, 7, 8, 9), YmdHms(2014, 5, 6, 7, 8, 9), GameClockDuration.Zero },
                { YmdHms(2014, 5, 6, 7, 8, 10), YmdHms(2014, 5, 6, 7, 8, 9), GameClockDuration.FromSeconds(1) },
                { YmdHms(2014, 5, 6, 7, 8, 9), YmdHms(2014, 5, 6, 7, 8, 10), GameClockDuration.FromSeconds(-1) },
                { YmdHms(2014, 5, 7, 7, 8, 9), YmdHms(2014, 5, 6, 7, 8, 10), GameClockDuration.FromSeconds(86399) },
                { YmdHms(2001, 9, 9, 1, 46, 39), YmdHms(1970, 1, 1, 0, 0, 0), GameClockDuration.FromSeconds(999_999_999) },
                { GameClockDateTime.MaxValue, GameClockDateTime.MinValue,
                    GameClockDuration.FromDays(MaxDayFromMinDate) + GameClockDuration.FromSeconds(86399) },
                { GameClockDateTime.MinValue, GameClockDateTime.MaxValue,
                    GameClockDuration.FromDays(-MaxDayFromMinDate) + GameClockDuration.FromSeconds(-86399) },
            };

        [Theory]
        [MemberData(nameof(Signed_Duration_Since_Data))]
        public void Signed_duration_since_another_date_time_returns_signed_duration(GameClockDateTime dt1, GameClockDateTime dt2, GameClockDuration expected)
        {
            GameClockDuration actual = dt1.SignedDurationSince(dt2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MaxValue_is_1568704592609_days_and_86399_seconds_later_than_MinValue()
        {
            GameClockDuration maxDaysFromMinYear = GameClockDate.MaxValue.SignedDurationSince(GameClockDate.MinValue);
            GameClockDateTime minDateTimeMidnight = GameClockDate.MinValue.AndHms(0, 0, 0);
            GameClockDateTime maxDateTimeMidnight = minDateTimeMidnight + maxDaysFromMinYear;
            GameClockDateTime maxDatePlus86399Secs = maxDateTimeMidnight + GameClockDuration.FromSeconds(86399);

            Assert.Equal(1568704592609, maxDaysFromMinYear.WholeDays);
            VerifyGameClockDateTimeYmdHms(maxDateTimeMidnight, int.MaxValue, 12, 31, 0, 0, 0);
            VerifyGameClockDateTimeYmdHms(maxDatePlus86399Secs, int.MaxValue, 12, 31, 23, 59, 59);

            Assert.Throws<ArgumentOutOfRangeException>(()
                => maxDateTimeMidnight + GameClockDuration.FromSeconds(86400));
            Assert.Throws<ArgumentOutOfRangeException>(()
                => maxDateTimeMidnight + GameClockDuration.MaxValue);
        }

        [Fact]
        public void MinValue_is_1568704592609_days_and_86399_seconds_earlier_than_MaxValue()
        {
            GameClockDuration minDaysFromMaxYear = GameClockDate.MinValue.SignedDurationSince(GameClockDate.MaxValue);
            GameClockDateTime maxDateTimeRightBeforeMidnight = GameClockDate.MaxValue.AndHms(23, 59, 59);
            GameClockDateTime minDateTimeRightBeforeMidnight = maxDateTimeRightBeforeMidnight + minDaysFromMaxYear;
            GameClockDateTime minDateTime = minDateTimeRightBeforeMidnight - GameClockDuration.FromSeconds(86399);

            Assert.Equal(-1568704592609, minDaysFromMaxYear.WholeDays);
            VerifyGameClockDateTimeYmdHms(minDateTimeRightBeforeMidnight, int.MinValue, 1, 1, 23, 59, 59);
            VerifyGameClockDateTimeYmdHms(minDateTime, int.MinValue, 1, 1, 0, 0, 0);

            Assert.Throws<ArgumentOutOfRangeException>(()
                => minDateTimeRightBeforeMidnight + GameClockDuration.FromSeconds(-86400));
            Assert.Throws<ArgumentOutOfRangeException>(()
                => minDateTimeRightBeforeMidnight + GameClockDuration.MinValue);
        }

        public static TheoryData<GameClockDateTime> ToString_Data =>
            new()
            {
                {
                    YmdHms(1, 1, 1, 0, 0, 0)
                },
                {
                    YmdHms(9999, 12, 31, 23, 59, 59)
                },
                {
                    YmdHms(10000, 1, 1, 13, 14, 15)
                },
                {
                    YmdHms(-1, 1, 1, 23, 26, 27)
                },
                {
                    YmdHms(2147483647, 12, 31, 23, 55, 59)
                },
                {
                    YmdHms(-2147483648, 1, 1, 1, 2, 3)
                },
            };

        [Theory]
        [MemberData(nameof(ToString_Data))]
        public void ToString_with_no_params_returns_string_with_ToString_of_date_and_space_and_ToString_of_time(
            GameClockDateTime dt)
        {
            GameClockDate d = dt.Date;
            string dStr = d.ToString();
            GameClockTime t = dt.Time;
            string tStr = t.ToString();

            string actual = dt.ToString();

            Assert.Equal($"{dStr} {tStr}", actual);
        }

        private static GameClockDateTime YmdHms(int y, int m, int d, int h, int n, int s)
            => GameClockDate.FromYmd(y, m, d).AndHms(h, n, s);

        private static void VerifyGameClockDateTimeYmdHms(GameClockDateTime dateTime, int year, int month, int day, int hour, int minute, int second)
        {
            Assert.Equal(year, dateTime.Year);
            Assert.Equal(month, dateTime.Month);
            Assert.Equal(day, dateTime.Day);

            Assert.Equal(hour, dateTime.Hour);
            Assert.Equal(minute, dateTime.Minute);
            Assert.Equal(second, dateTime.Second);
        }
    }
}
