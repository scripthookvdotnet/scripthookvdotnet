using System;
using GTA.Chrono;
using Xunit;

namespace ScriptHookVDotNet_APIv3_Tests
{
	public class DurationTests
	{
		/// The max safe integer value out of f64 (double) where the gap between the given value and the next up value is
		/// 1.0 or less.
		const double MaxSafeIntegerOutOfF64 = 9007199254740991.0;
		/// The min safe integer value out of f64 (double) where the gap between the given value and the next down value
		/// is 1.0 or less.
		const double MinSafeIntegerOutOfF64 = -9007199254740991.0;

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

			GameClockDuration actualAddMethod = d1.Add(d2);
			GameClockDuration actualAdditionOp = d1 + d2;

			Assert.Equal(expectedDuration, actualAddMethod);
			Assert.Equal(expectedDuration, actualAdditionOp);
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

			GameClockDuration actualSubtractMethod = d1.Subtract(-d2);
			GameClockDuration actualSubtractionOp = d1 - (-d2);

			Assert.Equal(expectedDuration, actualSubtractMethod);
			Assert.Equal(expectedDuration, actualSubtractionOp);
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

		[Fact]
		public void Division_with_double_divisor_uses_a_double_as_intermediate_value_if_abs_of_dividend_is_94906265_or_smaller_and_abs_of_divisor_is_inv_of_dividend_or_larger()
		{
			var durationLowerWeighted = GameClockDuration.FromSeconds(94906265);
			const double NextGreaterValOfInverseOfMaxSafeSqrtIntegerOutOfDouble = 1.0536712197029352e-08;

			GameClockDuration actualCalcRes = (durationLowerWeighted /
				NextGreaterValOfInverseOfMaxSafeSqrtIntegerOutOfDouble);

			Assert.StrictEqual((long)(durationLowerWeighted.WholeSeconds /NextGreaterValOfInverseOfMaxSafeSqrtIntegerOutOfDouble), actualCalcRes.WholeSeconds);

			Assert.NotStrictEqual((long)(durationLowerWeighted.WholeSeconds / (decimal)NextGreaterValOfInverseOfMaxSafeSqrtIntegerOutOfDouble), actualCalcRes.WholeSeconds);
		}

		[Fact]
		public void Division_with_double_divisor_uses_a_double_as_intermediate_value_if_abs_of_dividend_is_smaller_than_0x4000_and_abs_of_divisor_is_larger_than_inv_of_0x8000000000()
		{
			var durationLowerWeighted = GameClockDuration.FromSeconds(0x3FFF);

			double MinOptimizedValueHigherWeighted = 1.8189894035458565e-12.NextUp();

			Assert.StrictEqual((long)(durationLowerWeighted.WholeSeconds / MinOptimizedValueHigherWeighted), (durationLowerWeighted / MinOptimizedValueHigherWeighted).WholeSeconds);
		}

		[Fact]
		public void Division_with_double_divisor_uses_a_double_as_intermediate_value_if_abs_of_dividend_is_smaller_than_0x8000000000_and_abs_of_divisor_is_larger_than_inv_of_0x4000()
		{
			var durationHigherWeighted = GameClockDuration.FromSeconds(0x7FFFFFFFFF);

			double MinOptimizedValueLowerWeighted = 6.103515625e-05.NextUp();

			Assert.StrictEqual((long)(durationHigherWeighted.WholeSeconds / MinOptimizedValueLowerWeighted), (durationHigherWeighted / MinOptimizedValueLowerWeighted).WholeSeconds);
		}

		[Fact]
		public void Division_with_double_divisor_uses_decimal_as_intermediate_value_if_the_division_does_not_fall_in_known_cases_where_the_result_is_100_percent_within_safe_integers_out_of_f64()
		{
			var durationLowerWeighted = GameClockDuration.FromSeconds(94906265);
			double InverseOfMaxSafeSqrtIntegerOutOfDouble = 1.0536712197029352e-08.NextDown();

			GameClockDuration actualCalcRes = (durationLowerWeighted / InverseOfMaxSafeSqrtIntegerOutOfDouble);

			Assert.NotStrictEqual((long)(durationLowerWeighted.WholeSeconds / InverseOfMaxSafeSqrtIntegerOutOfDouble), actualCalcRes.WholeSeconds);
			Assert.StrictEqual((long)(durationLowerWeighted.WholeSeconds / (decimal)InverseOfMaxSafeSqrtIntegerOutOfDouble), actualCalcRes.WholeSeconds);
		}
	}
}
