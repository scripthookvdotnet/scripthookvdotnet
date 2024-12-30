//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using GTA.Chrono;
using Xunit;

namespace ScriptHookVDotNet_APIv3_Tests
{
    public class GameClockDurationTests
    {
        /// The max safe integer value out of f64 (double) where the gap between the given value and the next up value is
        /// 1.0 or less.
        const double MaxSafeIntegerOutOfF64 = 9007199254740991.0;
        /// The min safe integer value out of f64 (double) where the gap between the given value and the next down value
        /// is 1.0 or less.
        const double MinSafeIntegerOutOfF64 = -9007199254740991.0;

        private const long SecsPerHour = 3600;
        private const long SecsPerMinute = 60;

        [Fact]
        public void FromSeconds_with_the_arg_1_does_not_return_the_same_duration_as_zero()
        {
            GameClockDuration actual = GameClockDuration.FromSeconds(1);

            Assert.NotEqual(GameClockDuration.Zero, actual);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(-1, 1, 0)]
        [InlineData(-1, -1, -2)]
        public void Addition_of_two_duration_returns_the_same_duration_as_one_with_the_sum_of_two_second_values(
            long secs1, long secs2, long expected)
        {
            GameClockDuration d1 = GameClockDuration.FromSeconds(secs1);
            GameClockDuration d2 = GameClockDuration.FromSeconds(secs2);
            GameClockDuration expectedDuration = GameClockDuration.FromSeconds(expected);

            GameClockDuration actual = d1 + d2;

            Assert.Equal(expectedDuration, actual);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(-1, 1, 0)]
        [InlineData(-1, -1, -2)]
        public void Subtraction_of_duration_from_GameClockDuration_is_the_same_as_addition_of_negated_duration(
            long secs1, long secs2, long expected)
        {
            GameClockDuration d1 = GameClockDuration.FromSeconds(secs1);
            GameClockDuration d2 = GameClockDuration.FromSeconds(secs2);
            GameClockDuration expectedDuration = GameClockDuration.FromSeconds(expected);

            GameClockDuration actual = d1 - (-d2);

            Assert.Equal(expectedDuration, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(31)]
        [InlineData(-31)]
        public void Negation_of_duration_returns_the_same_duration_but_with_opposite_sign(
            long secs)
        {
            GameClockDuration actualDurationSecs = -GameClockDuration.FromSeconds(secs);
            GameClockDuration expectedDurationSecs = GameClockDuration.FromSeconds(-secs);
            GameClockDuration actualDurationDays = -GameClockDuration.FromDays(secs);
            GameClockDuration expectedDurationDays = GameClockDuration.FromDays(-secs);

            Assert.Equal(expectedDurationSecs, actualDurationSecs);
            Assert.Equal(actualDurationDays, expectedDurationDays);
        }

        [Fact]
        public void FromSeconds_with_too_large_or_small_value_fails()
        {
            GameClockDuration maxDuration = GameClockDuration.MaxValue;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                GameClockDuration.FromSeconds(maxDuration.WholeSeconds + 1)
            );
            Assert.Throws<ArgumentOutOfRangeException>(() => GameClockDuration.FromSeconds(long.MaxValue));

            GameClockDuration minDuration = GameClockDuration.MinValue;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                GameClockDuration.FromSeconds(minDuration.WholeSeconds - 1)
            );
            Assert.Throws<ArgumentOutOfRangeException>(() => GameClockDuration.FromSeconds(long.MinValue));
        }

        [Fact]
        public void Whole_seconds_of_max_value_is_135536076801503999_seconds()
        {
            long wholeSecsOfMaxDuration = GameClockDuration.MaxValue.WholeSeconds;

            Assert.Equal(135536076801503999, wholeSecsOfMaxDuration);
        }

        [Fact]
        public void Whole_seconds_of_min_value_is_minus_135536076801503999_seconds()
        {
            long wholeSecsOfMinDuration = GameClockDuration.MinValue.WholeSeconds;

            Assert.Equal(-135536076801503999, wholeSecsOfMinDuration);
        }

        [Fact]
        public void FromDays_returns_the_same_value_FromSeconds_with_the_same_arg_but_multiplied_by_86400()
        {
            GameClockDuration actual1Day = GameClockDuration.FromDays(1);
            GameClockDuration expected86400Secs = GameClockDuration.FromSeconds(86400);

            Assert.Equal(expected86400Secs, actual1Day);

            GameClockDuration actual10DaysMinus1000Secs
                = GameClockDuration.FromDays(10) - GameClockDuration.FromSeconds(1000);
            GameClockDuration expected863000Secs = GameClockDuration.FromSeconds(863_000);

            Assert.Equal(expected863000Secs, actual10DaysMinus1000Secs);

            GameClockDuration actual10DaysMinus1000000Secs
                = GameClockDuration.FromDays(10) - GameClockDuration.FromSeconds(1_000_000);
            GameClockDuration expectedMinus136000Secs = GameClockDuration.FromSeconds(-136_000);

            Assert.Equal(expectedMinus136000Secs, actual10DaysMinus1000000Secs);

            GameClockDuration actual3DaysPlus70Secs_ButWholeSecsNegated
                = -(GameClockDuration.FromDays(3) + GameClockDuration.FromSeconds(70));
            GameClockDuration expectedMinus4Days_Plus86400SecsMinus70Secs
                = GameClockDuration.FromDays(-4) + GameClockDuration.FromSeconds(86_400 - 70);

            Assert.Equal(expectedMinus4Days_Plus86400SecsMinus70Secs, actual3DaysPlus70Secs_ButWholeSecsNegated);
        }

        public static TheoryData<long, long> WholeDays_Valid_Secs_TestData =>
            new TheoryData<long, long>
            {
                { 86_399, 0 },
                { 86_400, 1 },
                { 86_401, 1 },
                { -86_399, 0 },
                { -86_400, -1 },
                { -86_401, -1 },
            };

        [Theory]
        [MemberData(nameof(WholeDays_Valid_Secs_TestData))]
        public void WholeDays_returns_only_whole_days_and_discards_fractional_days(long secs, long expectedDays)
        {
            GameClockDuration actualDuration = GameClockDuration.FromSeconds(secs);

            Assert.Equal(expectedDays, actualDuration.WholeDays);
        }

        [Fact]
        public void Whole_days_of_max_value_is_1568704592609_days()
        {
            long wholeDaysOfMaxDuration = GameClockDuration.MaxValue.WholeDays;

            Assert.Equal(1568704592609, wholeDaysOfMaxDuration);
        }

        [Fact]
        public void Whole_days_of_min_value_is_negative_1568704592609_days()
        {
            long wholeDaysOfMinDuration = GameClockDuration.MinValue.WholeDays;

            Assert.Equal(-1568704592609, wholeDaysOfMinDuration);
        }

        [Fact]
        public void CompareTo()
        {
            GameClockDuration duration1 = GameClockDuration.FromSeconds(30);
            GameClockDuration duration2 = GameClockDuration.FromSeconds(30);
            GameClockDuration duration3 = GameClockDuration.FromSeconds(40);

            Assert.True(duration1.CompareTo(duration2) == 0);
            Assert.True(duration1.CompareTo(duration3) < 0);
            Assert.True(duration3.CompareTo(duration2) > 0);
        }

        [Fact]
        public void NonGenericIComparable_CompareTo()
        {
            GameClockDuration duration1 = GameClockDuration.FromSeconds(30);
            GameClockDuration duration2 = GameClockDuration.FromSeconds(30);
            GameClockDuration duration3 = GameClockDuration.FromSeconds(40);

            var i_duration1 = (IComparable)duration1;
            var i_duration3 = (IComparable)duration3;

            Assert.True(i_duration1.CompareTo(duration2) == 0);
            Assert.True(i_duration1.CompareTo(duration3) < 0);
            Assert.True(i_duration3.CompareTo(duration2) > 0);
        }

        [Fact]
        public void NonGenericIComparable_returns_positive_for_null_arg()
        {
            GameClockDuration val = GameClockDuration.FromSeconds(5);
            Assert.True(val.CompareTo(null) > 0);
        }

        [Fact]
        public void NonGenericIComparable_with_obj_other_than_duration_fails()
        {
            GameClockDuration val = GameClockDuration.FromSeconds(5);
            Assert.Throws<ArgumentException>(() => val.CompareTo(1) > 0);
        }

        [Fact]
        public void Seconds_extracts_second_component_not_num_of_whole_seconds()
        {
            GameClockDuration duration = GameClockDuration.FromMinutes(3) + GameClockDuration.FromSeconds(57);
            Assert.Equal(57, duration.Seconds);

            int secondComponent = (duration + GameClockDuration.FromSeconds(3)).Seconds;
            Assert.Equal(0, secondComponent);
        }

        [Fact]
        public void Minutes_extracts_minute_component_not_num_of_whole_minutes()
        {
            GameClockDuration duration = GameClockDuration.FromHours(4) + GameClockDuration.FromMinutes(58);
            Assert.Equal(58, duration.Minutes);

            int minuteComponent = (duration + GameClockDuration.FromMinutes(2)).Minutes;
            Assert.Equal(0, minuteComponent);
        }

        [Fact]
        public void Hours_extracts_hour_component_not_num_of_whole_hours()
        {
            GameClockDuration duration = GameClockDuration.FromDays(5) + GameClockDuration.FromHours(23);
            Assert.Equal(23, duration.Hours);

            int hourComponent = (duration + GameClockDuration.FromHours(1)).Hours;
            Assert.Equal(0, hourComponent);
        }

        [Fact]
        public void TotalMinutes_returns_a_certain_value_in_the_range_of_safe_integer_out_of_f64()
        {
            GameClockDuration maxDuration = GameClockDuration.MaxValue;
            GameClockDuration minDuration = GameClockDuration.MinValue;

            Assert.True(maxDuration.TotalMinutes < MaxSafeIntegerOutOfF64);
            Assert.True(minDuration.TotalMinutes > MinSafeIntegerOutOfF64);
        }

        [Fact]
        public void TotalHours_returns_a_certain_value_in_the_range_of_safe_integer_out_of_f64()
        {
            GameClockDuration maxDuration = GameClockDuration.MaxValue;
            GameClockDuration minDuration = GameClockDuration.MinValue;

            Assert.True(maxDuration.TotalHours < MaxSafeIntegerOutOfF64);
            Assert.True(minDuration.TotalHours > MinSafeIntegerOutOfF64);
        }

        [Fact]
        public void TotalDays__returns_a_certain_value_in_the_range_of_safe_integer_out_of_f64()
        {
            GameClockDuration maxDuration = GameClockDuration.MaxValue;
            GameClockDuration minDuration = GameClockDuration.MinValue;

            Assert.True(maxDuration.TotalDays < MaxSafeIntegerOutOfF64);
            Assert.True(minDuration.TotalDays > MinSafeIntegerOutOfF64);
        }

        [Fact]
        public void TotalWeeks__returns_a_certain_value_in_the_range_of_safe_integer_out_of_f64()
        {
            GameClockDuration maxDuration = GameClockDuration.MaxValue;
            GameClockDuration minDuration = GameClockDuration.MinValue;

            Assert.True(maxDuration.TotalWeeks < MaxSafeIntegerOutOfF64);
            Assert.True(minDuration.TotalWeeks > MinSafeIntegerOutOfF64);
        }

        public static TheoryData<GameClockDuration, long, GameClockDuration> Multiplication_with_long_factor_Valid_Data =>
            new TheoryData<GameClockDuration, long, GameClockDuration>
            {
                { GameClockDuration.Zero, long.MaxValue, GameClockDuration.Zero },
                { GameClockDuration.Zero, long.MinValue, GameClockDuration.Zero },
                { GameClockDuration.FromSeconds(1), 0, GameClockDuration.Zero },
                { GameClockDuration.FromSeconds(1), 1, GameClockDuration.FromSeconds(1) },
                { GameClockDuration.FromSeconds(1), 3600, GameClockDuration.FromHours(1) },
                { GameClockDuration.FromSeconds(1), -3600, -GameClockDuration.FromHours(1) },
                { -GameClockDuration.FromSeconds(1), 3600, -GameClockDuration.FromHours(1) },
                { GameClockDuration.FromSeconds(1) + GameClockDuration.FromDays(1), 5,
                    GameClockDuration.FromSeconds(5) + GameClockDuration.FromDays(5) },
                { GameClockDuration.FromSeconds(45), -4, -GameClockDuration.FromMinutes(3) },
                { GameClockDuration.FromSeconds(-45), 4, -GameClockDuration.FromMinutes(3) },
            };

        [Theory]
        [MemberData(nameof(Multiplication_with_long_factor_Valid_Data))]
        public void Multiplication_with_long_factor_performs_against_the_internal_long_sec_value(
            GameClockDuration duration, long factor, GameClockDuration expected)
        {
            GameClockDuration actual = duration * factor;

            Assert.Equal(expected, actual);
        }

        public static TheoryData<GameClockDuration, GameClockDuration> Abs_Method_Test_Data =>
            new TheoryData<GameClockDuration, GameClockDuration>
            {
                { GameClockDuration.Zero, GameClockDuration.Zero },
                { GameClockDuration.FromSeconds(1), GameClockDuration.FromSeconds(1) },
                { GameClockDuration.FromSeconds(-1), GameClockDuration.FromSeconds(1) },
                { GameClockDuration.FromMinutes(2), GameClockDuration.FromMinutes(2) },
                { GameClockDuration.FromMinutes(-2), GameClockDuration.FromMinutes(2) },
                { GameClockDuration.FromHours(3), GameClockDuration.FromHours(3) },
                { GameClockDuration.FromHours(-3), GameClockDuration.FromHours(3) },
                { GameClockDuration.FromDays(4), GameClockDuration.FromDays(4) },
                { GameClockDuration.FromDays(-4), GameClockDuration.FromDays(4) },
                { GameClockDuration.FromWeeks(5), GameClockDuration.FromWeeks(5) },
                { GameClockDuration.FromWeeks(-5), GameClockDuration.FromWeeks(5) },
            };

        [Theory]
        [MemberData(nameof(Abs_Method_Test_Data))]
        public void Abs_returns_the_duration_but_with_the_positive_sign(
            GameClockDuration duration, GameClockDuration expected)
        {
            GameClockDuration actual = duration.Abs();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiplication_with_Infinity_factor_fails()
        {
            Assert.Throws<ArgumentOutOfRangeException>(()
                => GameClockDuration.FromSeconds(1) * double.PositiveInfinity);
            Assert.Throws<ArgumentOutOfRangeException>(()
                => GameClockDuration.FromSeconds(-1) * double.PositiveInfinity);
            Assert.Throws<ArgumentOutOfRangeException>(()
                => GameClockDuration.FromSeconds(1) * double.NegativeInfinity);
            Assert.Throws<ArgumentOutOfRangeException>(()
                => GameClockDuration.FromSeconds(-1) * double.NegativeInfinity);
        }

        [Fact]
        public void Multiplication_with_NaN_factor_fails()
        {
            Assert.Throws<ArgumentException>(() => GameClockDuration.Zero * double.NaN);
            Assert.Throws<ArgumentException>(() => GameClockDuration.FromSeconds(1) * double.NaN);
            Assert.Throws<ArgumentException>(() => GameClockDuration.FromSeconds(-1) * double.NaN);
        }

        [Fact]
        public void Multiplication_of_double_factor_returns_zero_duration_if_latter_factor_is_zero_or_duration_is_zero()
        {
            GameClockDuration actualDurationOf1SecsMultipliedByZero
                = GameClockDuration.FromSeconds(1) * 0.0;
            GameClockDuration actualDurationOfZeroMultipliedByMinDouble
                = GameClockDuration.Zero * double.MinValue;
            GameClockDuration actualDurationOfZeroMultipliedByMaxDouble
                = GameClockDuration.Zero * double.MaxValue;

            Assert.Equal(0, actualDurationOf1SecsMultipliedByZero.WholeSeconds);
            Assert.Equal(0, actualDurationOfZeroMultipliedByMinDouble.WholeSeconds);
            Assert.Equal(0, actualDurationOfZeroMultipliedByMaxDouble.WholeSeconds);
        }

        public static TheoryData<GameClockDuration, double, GameClockDuration> Multiplication_double_factor_Simple_Case_Data =>
            new TheoryData<GameClockDuration, double, GameClockDuration>
            {
                { GameClockDuration.FromSeconds(1), 0.0, GameClockDuration.Zero },
                { GameClockDuration.FromSeconds(1), 1.0, GameClockDuration.FromSeconds(1) },
                { GameClockDuration.FromSeconds(1), 3600.0, GameClockDuration.FromHours(1) },
                { GameClockDuration.FromSeconds(1), -3600.0, -GameClockDuration.FromHours(1) },
                { -GameClockDuration.FromSeconds(1), 3600.0, -GameClockDuration.FromHours(1) },
                { GameClockDuration.FromSeconds(2), 2.75, GameClockDuration.FromSeconds(6) },
                { GameClockDuration.FromSeconds(45), -4.5, -GameClockDuration.FromSeconds(202) },
                { GameClockDuration.FromSeconds(-45), 4.5, -GameClockDuration.FromSeconds(202) },
            };

        [Theory]
        [MemberData(nameof(Multiplication_double_factor_Simple_Case_Data))]
        public void Multiplication_by_double_factor_simple_cases_where_the_intermediate_value_is_a_double(GameClockDuration duration, double factor, GameClockDuration expected)
        {
            GameClockDuration actual = (duration * factor);

            Assert.Equal(expected, actual);
        }

        public static TheoryData<GameClockDuration, double, GameClockDuration> Multiplication_double_factor_Complex_Case_Data =>
            new TheoryData<GameClockDuration, double, GameClockDuration>
            {
                { GameClockDuration.FromSeconds(2020050260302305), 1.375,
                    GameClockDuration.FromSeconds(2777569107915670) },
                { GameClockDuration.FromSeconds(2020050260302302), 1.25,
                    GameClockDuration.FromSeconds(2525062825377878) },
                { GameClockDuration.FromSeconds(8000050260300301), 1.125,
                    GameClockDuration.FromSeconds(9000056542837839) },

            };

        [Theory]
        [MemberData(nameof(Multiplication_double_factor_Complex_Case_Data))]
        public void Multiplication_by_double_factor_uses_a_double_as_intermediate_value_and_rounds_with_nearest_even_midpoint_rounding_mode_if__the_abs_of_product_as_a_double_is_94906265_or_smaller(GameClockDuration duration, double factor, GameClockDuration expected)
        {
            GameClockDuration actual = (duration * factor);

            // Note: this test will fail if the results as both double and decimal fall in the same value
            Assert.Equal(expected, actual);
            Assert.NotEqual(duration.WholeSeconds * (decimal)factor, (decimal)actual.WholeSeconds);
        }

        public static TheoryData<GameClockDuration, double, GameClockDuration> Multiplication_double_factor_Decimal_Case_Data =>
            new TheoryData<GameClockDuration, double, GameClockDuration>
            {
				// `94906266.0 * 94906266.0` produces the same result `9007199326062756.0` as `94906266.0M * 94906266.0M`,
				// where the gap between the next value is only 2.0. Hence, we test against `94906267.0` as this is the
				// min value where the results will be different between double and decimal.
				{ GameClockDuration.FromSeconds(94906265), 94906267.0, GameClockDuration.FromSeconds(9007199326062755) },
                { GameClockDuration.FromSeconds(94906267), 94906267.0, GameClockDuration.FromSeconds(9007199515875289) },
                { GameClockDuration.FromSeconds(379625061), 34906267.0, GameClockDuration.FromSeconds(13251293739157287) },
                { GameClockDuration.FromSeconds(549755813889), 16385, GameClockDuration.FromSeconds(9007749010571265) },
                { GameClockDuration.FromSeconds(16385), -549755813889, -GameClockDuration.FromSeconds(9007749010571265) },
            };

        [Theory]
        [MemberData(nameof(Multiplication_double_factor_Decimal_Case_Data))]
        public void Multiplication_by_double_factor_uses_decimal_as_intermediate_value_if_the_abs_of_product_as_a_double_is_9007199254740992_or_larger(
            GameClockDuration duration, double factor, GameClockDuration expected)
        {
            GameClockDuration actual = duration * factor;

            // Note: this test will fail if the results as both double and decimal fall in the same value
            Assert.Equal(expected, actual);
            Assert.NotEqual(expected.WholeSeconds, (long)(duration.WholeSeconds * factor));
        }

        public static TheoryData<GameClockDuration, long, GameClockDuration> Division_with_long_divisor_Valid_Data =>
            new TheoryData<GameClockDuration, long, GameClockDuration>
                    {
                { GameClockDuration.Zero, long.MaxValue, GameClockDuration.Zero },
                { GameClockDuration.Zero, long.MinValue, GameClockDuration.Zero },
                { GameClockDuration.FromSeconds(123_456_789), 1, GameClockDuration.FromSeconds(123_456_789) },
                { GameClockDuration.FromSeconds(123_456_789), -1, -GameClockDuration.FromSeconds(123_456_789) },
                { -GameClockDuration.FromSeconds(123_456_789), -1, GameClockDuration.FromSeconds(123_456_789) },
                { -GameClockDuration.FromSeconds(123_456_789), 1, -GameClockDuration.FromSeconds(123_456_789) },
                { GameClockDuration.FromSeconds(1), 2, GameClockDuration.Zero },
                { GameClockDuration.FromSeconds(1), 2, GameClockDuration.Zero },
                { GameClockDuration.FromSeconds(50), 51, GameClockDuration.Zero },
                { GameClockDuration.FromSeconds(50), 49, GameClockDuration.FromSeconds(1) },
                { GameClockDuration.FromSeconds(2), 2, GameClockDuration.FromSeconds(1) },
                { GameClockDuration.FromSeconds(3), 2, GameClockDuration.FromSeconds(1) },
                { GameClockDuration.FromSeconds(60), 7, GameClockDuration.FromSeconds(8) },
            };

        [Theory]
        [MemberData(nameof(Division_with_long_divisor_Valid_Data))]
        public void Division_with_long_divisor_performs_against_the_internal_long_sec_value(
            GameClockDuration duration, long divisor, GameClockDuration expected)
        {
            GameClockDuration actual = duration / divisor;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Division_with_zero_divisor_fails()
        {
            Assert.Throws<DivideByZeroException>(() => GameClockDuration.Zero / 0);
            Assert.Throws<DivideByZeroException>(() => GameClockDuration.FromSeconds(1) / 0);
            Assert.Throws<DivideByZeroException>(() => GameClockDuration.FromSeconds(-1) / 0);

            Assert.Throws<ArgumentOutOfRangeException>(() => GameClockDuration.Zero / 0.0);
            Assert.Throws<ArgumentOutOfRangeException>(() => GameClockDuration.FromSeconds(1) / 0.0);
            Assert.Throws<ArgumentOutOfRangeException>(() => GameClockDuration.FromSeconds(-1) / 0.0);
        }

        [Fact]
        public void Division_with_NaN_divisor_fails()
        {
            Assert.Throws<ArgumentException>(() => GameClockDuration.Zero / double.NaN);
            Assert.Throws<ArgumentException>(() => GameClockDuration.FromSeconds(1) / double.NaN);
            Assert.Throws<ArgumentException>(() => GameClockDuration.FromSeconds(-1) / double.NaN);
        }

        public static TheoryData<GameClockDuration, double, GameClockDuration> Division_double_divisor_Simple_Case_Data =>
            new TheoryData<GameClockDuration, double, GameClockDuration>
            {
                { GameClockDuration.Zero, GameClockDuration.MaxValue.WholeSeconds, GameClockDuration.Zero },
                { GameClockDuration.Zero, GameClockDuration.MinValue.WholeSeconds, GameClockDuration.Zero },
                { GameClockDuration.FromSeconds(2), 2.0, GameClockDuration.FromSeconds(1) },
                { GameClockDuration.FromSeconds(3), 2.0, GameClockDuration.FromSeconds(2) },
                { GameClockDuration.FromSeconds(-5), 2.0, GameClockDuration.FromSeconds(-2) },
                { GameClockDuration.FromSeconds(5), -2.0, GameClockDuration.FromSeconds(-2) },
                { GameClockDuration.FromSeconds(-5), -2.0, GameClockDuration.FromSeconds(2) },
                { GameClockDuration.FromSeconds(80), 7.5, GameClockDuration.FromSeconds(11) },
                { GameClockDuration.FromSeconds(-45), 4.5, GameClockDuration.FromSeconds(-10) },
                { GameClockDuration.FromSeconds(180), 9.8, GameClockDuration.FromSeconds(18) },
            };

        [Theory]
        [MemberData(nameof(Division_double_divisor_Simple_Case_Data))]
        public void Division_by_double_divisor_simple_cases_where_the_intermediate_value_is_a_double(GameClockDuration duration, double factor, GameClockDuration expected)
        {
            GameClockDuration actual = (duration / factor);

            Assert.Equal(expected.WholeSeconds, actual.WholeSeconds);
        }

        public static TheoryData<GameClockDuration, double, GameClockDuration> Division_double_divisor_Complex_Case_Data =>
            new TheoryData<GameClockDuration, double, GameClockDuration>
            {
                { GameClockDuration.FromSeconds(1020050260302302), 0.875,
                    GameClockDuration.FromSeconds(1165771726059774) },
                { GameClockDuration.FromSeconds(1270150260342703), 0.75,
                    GameClockDuration.FromSeconds(1693533680456937) },
                { GameClockDuration.FromSeconds(60153266842715), 0.72132e-2,
                    GameClockDuration.FromSeconds(8339331620184522) },
            };

        [Theory]
        [MemberData(nameof(Division_double_divisor_Complex_Case_Data))]
        public void Division_by_double_divisor_uses_a_double_as_intermediate_value_and_rounds_with_nearest_even_midpoint_rounding_mode_if_abs_of_quotient_as_a_double_is_94906265_or_smaller(GameClockDuration duration, double factor, GameClockDuration expected)
        {
            GameClockDuration actual = (duration / factor);

            Assert.Equal(expected, actual);
            Assert.NotEqual(duration.WholeSeconds / (decimal)factor, actual.WholeSeconds);
        }

        public static TheoryData<GameClockDuration, double, GameClockDuration> Division_double_divisor_Decimal_Case_Data =>
            new TheoryData<GameClockDuration, double, GameClockDuration>
            {
				// `94906266.0 * 94906266.0` produces the same result `9007199326062756.0` as `94906266.0M * 94906266.0M`,
				// where the gap between the next value is only 2.0. Hence, we test against `94906267.0` as this is the
				// min value where the results will be different between double and decimal.
				{ GameClockDuration.FromSeconds(94906265), 1.0 / 94906267.0, GameClockDuration.FromSeconds(9007199326062798) },
                { GameClockDuration.FromSeconds(94906267), 1.0 / 94906267.0, GameClockDuration.FromSeconds(9007199515875332) },
                { GameClockDuration.FromSeconds(379625061), 1.0 / 34906267.0, GameClockDuration.FromSeconds(13251293739157267) },
                { GameClockDuration.FromSeconds(549755813889), 1.0 / 16385.0, GameClockDuration.FromSeconds(9007749010571270) },
                { GameClockDuration.FromSeconds(16385), 1.0 / -549755813889.0, GameClockDuration.FromSeconds(-9007749010571254) },
            };

        [Theory]
        [MemberData(nameof(Division_double_divisor_Decimal_Case_Data))]
        public void Division_by_double_divisor_uses_decimal_as_intermediate_value_if_the_abs_of_quotient_as_a_double_is_9007199254740992_or_larger(
            GameClockDuration duration, double factor, GameClockDuration expected)
        {
            GameClockDuration actual = duration / factor;

            // Note: this test will fail if the results as both double and decimal fall in the same value
            Assert.Equal(expected.WholeSeconds, actual.WholeSeconds);
            Assert.NotEqual(expected.WholeSeconds, (long)(duration.WholeSeconds / factor));
        }

        public static TheoryData<GameClockDuration, string> ToString_NonNegative_Less_Than_86400_Secs_Data =>
            new()
            {
                { GameClockDuration.FromSeconds(0), "00:00:00" },
                { GameClockDuration.FromSeconds(1), "00:00:01" },
                { GameClockDuration.FromMinutes(33), "00:33:00" },
                { GameClockDuration.FromHours(15), "15:00:00" },
                {
                    GameClockDuration.FromSeconds(23 * SecsPerHour + 59 * SecsPerMinute + 59),
                    "23:59:59"
                },
            };

        [Theory]
        [MemberData(nameof(ToString_NonNegative_Less_Than_86400_Secs_Data))]
        public void
        ToString_with_no_params_returns_string_with_2_digit_hour_min_and_sec_concatenated_with_colon_separator_if_duration_has_0_days_and_is_not_negative(
            GameClockDuration date, string expected)
        {
            string actual = date.ToString();

            Assert.Equal(expected, actual);
        }

        public static TheoryData<GameClockDuration, string> ToString_Negative_More_Than_86400_Secs_Data =>
            new()
            {
                { -GameClockDuration.FromSeconds(1), "-00:00:01" },
                { -GameClockDuration.FromMinutes(22), "-00:22:00" },
                { -GameClockDuration.FromHours(19), "-19:00:00" },
                {
                    -GameClockDuration.FromSeconds(23 * SecsPerHour + 59 * SecsPerMinute + 59),
                    "-23:59:59"
                },
            };

        [Theory]
        [MemberData(nameof(ToString_Negative_More_Than_86400_Secs_Data))]
        public void
        ToString_with_no_params_returns_string_with_neg_sign_2_digit_hour_min_and_sec_concatenated_with_colon_separator_if_duration_has_0_days_and_is_negative(
            GameClockDuration date, string expected)
        {
            string actual = date.ToString();

            Assert.Equal(expected, actual);
        }

        public static TheoryData<GameClockDuration, string> ToString_NonNegative_86400_Or_More_Secs_Data =>
            new()
            {
                { GameClockDuration.FromDays(1), "1:00:00:00" },
                { GameClockDuration.FromDays(14) + GameClockDuration.FromSeconds(5), "14:00:00:05" },
                { GameClockDuration.FromDays(190) + GameClockDuration.FromMinutes(37), "190:00:37:00" },
                { GameClockDuration.MaxValue, "1568704592609:23:59:59" },
            };

        [Theory]
        [MemberData(nameof(ToString_NonNegative_86400_Or_More_Secs_Data))]
        public void
            ToString_with_no_params_returns_string_with_no_padding_whole_days_2_digit_hour_min_and_sec_concatenated_with_colon_separator_if_duration_has_1_or_more_days_and_is_not_negative(
                GameClockDuration date, string expected)
        {
            string actual = date.ToString();

            Assert.Equal(expected, actual);
        }

        public static TheoryData<GameClockDuration, string> ToString_Negative_86400_Or_Less_Secs_Data =>
            new()
            {
                { -GameClockDuration.FromDays(1), "-1:00:00:00" },
                { -(GameClockDuration.FromDays(1000) + GameClockDuration.FromHours(5)), "-1000:05:00:00" },
                { GameClockDuration.MinValue, "-1568704592609:23:59:59" },
            };

        [Theory]
        [MemberData(nameof(ToString_Negative_86400_Or_Less_Secs_Data))]
        public void
            ToString_with_no_params_returns_string_with_neg_sign_no_padding_whole_days_2_digit_hour_min_and_sec_concatenated_with_colon_separator_if_duration_has_1_or_more_days_and_is_negative(
                GameClockDuration date, string expected)
        {
            string actual = date.ToString();

            Assert.Equal(expected, actual);
        }
    }
}
