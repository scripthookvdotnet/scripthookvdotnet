//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using Xunit;
using GTA.Chrono;
using System;
using System.Collections.Generic;

namespace ScriptHookVDotNet_APIv3_Tests
{
    public class GameClockTimeTests
    {
        [Fact]
        public void MaxValue()
        {
            VerifyGameClockTimeHms(GameClockTime.MaxValue, 23, 59, 59);
            VerifyGameClockTimeSecsFromMidnight(GameClockTime.MaxValue, 86399);
        }

        [Fact]
        public void MinValue()
        {
            VerifyGameClockTimeHms(GameClockTime.MinValue, 0, 0, 0);
            VerifyGameClockTimeSecsFromMidnight(GameClockTime.MinValue, 0);
        }

        [Theory]
        [InlineData(3, 5, 7)]
        [InlineData(11, 30, 30)]
        [InlineData(17, 45, 50)]
        public static void FromHms_returns_a_GameClockTime_for_valid_hms(int hour, int minute, int second)
        {
            GameClockTime time = GameClockTime.FromHms(hour, minute, second);
            VerifyGameClockTimeHms(time, hour, minute, second);
        }

        [Theory]
        [InlineData(-1, 5, 7)]
        [InlineData(24, 5, 7)]
        [InlineData(3, -1, 7)]
        [InlineData(3, 60, 7)]
        [InlineData(3, 5, -1)]
        [InlineData(3, 5, 60)]
        public static void FromHms_with_invalid_hms_fails_to_construct_a_GameClockTime(int hour, int minute,
            int second)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => GameClockTime.FromHms(hour, minute, second));
        }

        [Theory]
        [InlineData(3, 5, 7, 0)]
        [InlineData(3, 5, 7, 23)]
        public static void WithHour_returns_original_time_but_with_passed_hour(int hour, int minute, int second,
            int targetHour)
        {
            GameClockTime origTime = GameClockTime.FromHms(hour, minute, second);

            GameClockTime actualTime = origTime.WithHour(targetHour);

            VerifyGameClockTimeHms(actualTime, targetHour, minute, second);
        }

        [Theory]
        [InlineData(3, 5, 7, int.MinValue)]
        [InlineData(3, 5, 7, -1)]
        [InlineData(3, 5, 7, 24)]
        [InlineData(3, 5, 7, int.MaxValue)]
        public static void WithHour_with_invalid_hour_fails_to_construct_GameClockTime(int hour, int minute,
            int second, int targetHour)
        {
            GameClockTime origTime = GameClockTime.FromHms(hour, minute, second);

            Assert.Throws<ArgumentOutOfRangeException>(() => origTime.WithHour(targetHour));
        }

        [Theory]
        [InlineData(3, 5, 7, 0)]
        [InlineData(3, 5, 7, 59)]
        public static void WithMinute_returns_original_time_but_with_passed_minute(int hour, int minute, int second,
            int targetMin)
        {
            GameClockTime origTime = GameClockTime.FromHms(hour, minute, second);

            GameClockTime actualTime = origTime.WithMinute(targetMin);

            VerifyGameClockTimeHms(actualTime, hour, targetMin, second);
        }

        [Theory]
        [InlineData(3, 5, 7, int.MinValue)]
        [InlineData(3, 5, 7, -1)]
        [InlineData(3, 5, 7, 60)]
        [InlineData(3, 5, 7, int.MaxValue)]
        public static void WithMinute_with_invalid_minute_fails_to_construct_GameClockTime(int hour, int minute,
            int second, int targetMin)
        {
            GameClockTime origTime = GameClockTime.FromHms(hour, minute, second);

            Assert.Throws<ArgumentOutOfRangeException>(() => origTime.WithMinute(targetMin));
        }

        [Theory]
        [InlineData(3, 5, 7, 0)]
        [InlineData(3, 5, 7, 59)]
        public static void WithSecond_returns_original_time_but_with_passed_second(int hour, int minute, int second,
            int targetSec)
        {
            GameClockTime origTime = GameClockTime.FromHms(hour, minute, second);

            GameClockTime actualTime = origTime.WithSecond(targetSec);

            VerifyGameClockTimeHms(actualTime, hour, minute, targetSec);
        }

        [Theory]
        [InlineData(3, 5, 7, int.MinValue)]
        [InlineData(3, 5, 7, -1)]
        [InlineData(3, 5, 7, 60)]
        [InlineData(3, 5, 7, int.MaxValue)]
        public static void WithSecond_with_invalid_second_fails_to_construct_GameClockTime(int hour, int minute,
            int second, int targetSec)
        {
            GameClockTime origTime = GameClockTime.FromHms(hour, minute, second);

            Assert.Throws<ArgumentOutOfRangeException>(() => origTime.WithSecond(targetSec));
        }

        public static TheoryData<GameClockTime, GameClockDuration, GameClockTime> Add_Duration_Data =>
            new TheoryData<GameClockTime, GameClockDuration, GameClockTime>
            {
                { GameClockTime.FromHms(3, 4, 59), GameClockDuration.Zero, GameClockTime.FromHms(3, 4, 59) },
                { GameClockTime.FromHms(3, 4, 59), GameClockDuration.FromSeconds(-1), GameClockTime.FromHms(3, 4, 58) },
                { GameClockTime.FromHms(3, 4, 59), GameClockDuration.FromSeconds(1), GameClockTime.FromHms(3, 5, 0) },
                { GameClockTime.FromHms(3, 4, 59), GameClockDuration.FromSeconds(2), GameClockTime.FromHms(3, 5, 1) },
                { GameClockTime.FromHms(3, 4, 59), GameClockDuration.FromSeconds(86399), GameClockTime.FromHms(3, 4, 58) }, // overwrap
				{ GameClockTime.FromHms(3, 4, 59), GameClockDuration.FromSeconds(-86399), GameClockTime.FromHms(3, 5, 0) },
                { GameClockTime.FromHms(3, 4, 59), GameClockDuration.FromDays(1), GameClockTime.FromHms(3, 4, 59) },
                { GameClockTime.FromHms(3, 4, 59), GameClockDuration.FromDays(-1), GameClockTime.FromHms(3, 4, 59) },
                { GameClockTime.FromHms(3, 4, 59), GameClockDuration.FromDays(12345), GameClockTime.FromHms(3, 4, 59) },
            };

        [Theory]
        [MemberData(nameof(Add_Duration_Data))]
        public static void Addition_of_GameClockDuration_returns_added_time_and_overwraps_by_day(
            GameClockTime origTime, GameClockDuration duration, GameClockTime expected)
        {
            GameClockTime actual = origTime + duration;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Add_Duration_Data))]
        public static void Subtraction_of_GameClockDuration_returns_the_same_time_as_addition_of_negated_duration(
            GameClockTime origTime, GameClockDuration duration, GameClockTime expected)
        {
            GameClockTime actual = origTime - (-duration);

            Assert.Equal(expected, actual);
        }

        public static TheoryData<GameClockTime, GameClockDuration, GameClockTime, long> Overflowing_Add_Duration_Data =>
            new TheoryData<GameClockTime, GameClockDuration, GameClockTime, long>
            {
                { GameClockTime.FromHms(3, 4, 5), GameClockDuration.Zero, GameClockTime.FromHms(3, 4, 5), 0 },
                { GameClockTime.FromHms(3, 4, 5), GameClockDuration.FromHours(7), GameClockTime.FromHms(10, 4, 5), 0 },
                { GameClockTime.FromHms(3, 4, 5), GameClockDuration.FromHours(-11), GameClockTime.FromHms(16, 4, 5), -1 },
                { GameClockTime.FromHms(3, 4, 5), GameClockDuration.FromHours(27), GameClockTime.FromHms(6, 4, 5), 1 },
                { GameClockTime.FromHms(3, 4, 5), GameClockDuration.FromDays(1), GameClockTime.FromHms(3, 4, 5), 1 },
                { GameClockTime.FromHms(3, 4, 5), GameClockDuration.FromWeeks(-1), GameClockTime.FromHms(3, 4, 5), -7 },
            };

        [Theory]
        [MemberData(nameof(Overflowing_Add_Duration_Data))]
        public static void Overflowing_addition_of_GameClockDuration_returns_added_time_and_wrapped_days(
            GameClockTime origTime, GameClockDuration duration, GameClockTime expectedTime, long expectedWrappedDays)
        {
            GameClockTime actualTime = origTime.OverflowingAddSigned(duration, out long actualWrappedDays);

            Assert.Equal(expectedTime, actualTime);
            Assert.Equal(expectedWrappedDays, actualWrappedDays);
        }

        [Theory]
        [MemberData(nameof(Overflowing_Add_Duration_Data))]
        public static void Overflowing_subtraction_of_GameClockDuration_returns_the_same_time_as_overflowing_addition_of_negated_duration_and_negated_wrapped_days
            (GameClockTime origTime, GameClockDuration duration, GameClockTime expectedTime,
            long expectedWrappedDaysForOverflowingAddSigned)
        {
            GameClockTime actualTime = origTime.OverflowingSubtractSigned(-duration, out long actualWrappedDaysNegated);

            Assert.Equal(expectedTime, actualTime);
            Assert.Equal(expectedWrappedDaysForOverflowingAddSigned, -actualWrappedDaysNegated);
        }

        public static TheoryData<GameClockTime, GameClockTime, GameClockDuration> SignedDurationSince_Data =>
            new TheoryData<GameClockTime, GameClockTime, GameClockDuration>
            {
                { GameClockTime.FromHms(3, 5, 7), GameClockTime.FromHms(3, 5, 7), GameClockDuration.Zero },
                { GameClockTime.FromHms(3, 5, 7), GameClockTime.FromHms(3, 5, 4), GameClockDuration.FromSeconds(3) },
                { GameClockTime.FromHms(3, 5, 7), GameClockTime.FromHms(3, 4, 7), GameClockDuration.FromSeconds(60) },
                { GameClockTime.FromHms(3, 5, 7), GameClockTime.FromHms(2, 4, 6), GameClockDuration.FromSeconds(3600 + 60 + 1) },
                { GameClockTime.FromHms(12, 0, 0), GameClockTime.FromHms(0, 0, 0), GameClockDuration.FromSeconds(3600 * 12) },
                { GameClockTime.FromHms(23, 59, 59), GameClockTime.FromHms(0, 0, 0), GameClockDuration.FromSeconds(3600 * 23 + 60 * 59 + 1 * 59) },
            };

        [Theory]
        [MemberData(nameof(SignedDurationSince_Data))]
        public static void Signed_duration_since_another_time_returns_signed_duration(GameClockTime laterTime,
            GameClockTime earlierTime, GameClockDuration expectedDurationNonNegative)
        {
            GameClockDuration posDuration = laterTime.SignedDurationSince(earlierTime);
            GameClockDuration negDuration = earlierTime.SignedDurationSince(laterTime);

            Assert.Equal(expectedDurationNonNegative.WholeSeconds, posDuration.WholeSeconds);
            Assert.Equal(expectedDurationNonNegative, posDuration);
            Assert.Equal(-expectedDurationNonNegative, negDuration);
        }

        public static TheoryData<GameClockTime, string> ToString_Data =>
            new()
            {
                { GameClockTime.FromHms(0, 0, 0), "00:00:00" },
                { GameClockTime.FromHms(1, 2, 3), "01:02:03" },
                { GameClockTime.FromHms(13, 14, 15), "13:14:15" },
                { GameClockTime.FromHms(23, 59, 59), "23:59:59" },
            };

        [Theory]
        [MemberData(nameof(ToString_Data))]
        public void ToString_with_no_params_returns_string_with_2_digit_hour_min_and_sec_concatenated_with_colon_separator(
            GameClockTime date, string expected)
        {
            string actual = date.ToString();

            Assert.Equal(expected, actual);
        }

        private static void VerifyGameClockTimeHms(GameClockTime gameClockTime, int hour, int minute, int second)
        {
            Assert.Equal(hour, gameClockTime.Hour);
            Assert.Equal(minute, gameClockTime.Minute);
            Assert.Equal(second, gameClockTime.Second);
        }

        private static void VerifyGameClockTimeSecsFromMidnight(GameClockTime gameClockTime, int secs)
        {
            Assert.Equal(secs, gameClockTime.SecondsFromMidnight);
        }
    }
}
