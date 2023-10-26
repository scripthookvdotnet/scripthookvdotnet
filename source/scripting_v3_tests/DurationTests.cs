using Xunit;
using GTA.Chrono;
using System;

namespace ScriptHookVDotNet_APIv3_Tests
{
	public class DurationTests
	{
		/// The number of seconds in a minute.
		const long SecsPerMinute = 60;
		/// The number of seconds in an hour.
		const long SecsPerHour = 3600;
		/// The number of (non-leap) seconds in days.
		const long SecsPerDay = 86_400;
		/// The number of (non-leap) seconds in a week.
		const long SecsPerWeek = 604_800;

		/// The max safe integer value out of f64 (double) where the gap between the given value and the next up value is
		/// 1.0 or less.
		const double MaxSafeIntegerOutOfF64 = 9007199254740991.0;
		/// The min safe integer value out of f64 (double) where the gap between the given value and the next down value
		/// is 1.0 or less.
		const double MinSafeIntegerOutOfF64 = -9007199254740991.0;

		[Fact]
		public void FromSeconds_ThrowsArgumentOutOfRangeExceptionIfOverflowed()
		{
			GameClockDuration maxDuration = GameClockDuration.MaxValue;

			Assert.Throws<ArgumentOutOfRangeException>(() =>
				GameClockDuration.FromSeconds(GameClockDuration.MaxValue.WholeSeconds + 1)
			);
			Assert.Throws<ArgumentOutOfRangeException>(() => GameClockDuration.FromSeconds(long.MaxValue));

			Assert.Throws<ArgumentOutOfRangeException>(() =>
				GameClockDuration.FromSeconds(GameClockDuration.MinValue.WholeSeconds - 1)
			);
			Assert.Throws<ArgumentOutOfRangeException>(() => GameClockDuration.FromSeconds(long.MinValue));
		}

		[Fact]
		public void MaxMinRelationship()
		{
			Assert.Equal(GameClockDuration.MinValue, -GameClockDuration.MaxValue);
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
		public void NonGenericIComparable_RerurnsPositiveForNullArg()
		{
			GameClockDuration val = GameClockDuration.FromSeconds(5);
			Assert.True(val.CompareTo(null) > 0);
		}

		[Fact]
		public void NonGenericIComparable_ThrowsArgumentExceptionForForWrongTypeArg()
		{
			GameClockDuration val = GameClockDuration.FromSeconds(5);
			Assert.Throws<ArgumentException>(() => val.CompareTo(1) > 0);
		}

		[Fact]
		public void AdditionOperator_AddAnotherValue_ReturnsTheResultIfNotOverflowed()
		{
			GameClockDuration duration = GameClockDuration.FromSeconds(1);
			duration += GameClockDuration.FromSeconds(5);

			Assert.StrictEqual(GameClockDuration.FromSeconds(6), duration);
		}

		[Fact]
		public void NegationOperator_NegatesAndTheSameValueButTheSignIsNegated()
		{
			GameClockDuration d = GameClockDuration.FromSeconds(1);

			Assert.Equal(-d, GameClockDuration.FromSeconds(-1));
		}

		[Fact]
		public void Seconds_ExtractsSecondComponentNotNumOfWholeSeconds()
		{
			GameClockDuration duration = GameClockDuration.FromMinutes(3) + GameClockDuration.FromSeconds(57);
			Assert.Equal(57, duration.Seconds);

			int secondComponent = (duration + GameClockDuration.FromSeconds(3)).Seconds;
			Assert.Equal(0, secondComponent);
		}

		[Fact]
		public void Minutes_ExtractsMinuteComponentNotNumOfWholeMinutes()
		{
			GameClockDuration duration = GameClockDuration.FromHours(4) + GameClockDuration.FromMinutes(58);
			Assert.Equal(58, duration.Minutes);

			int minuteComponent = (duration + GameClockDuration.FromMinutes(2)).Minutes;
			Assert.Equal(0, minuteComponent);
		}

		[Fact]
		public void Hours_ExtractsHourComponentNotNumOfWholeHours()
		{
			GameClockDuration duration = GameClockDuration.FromDays(5) + GameClockDuration.FromHours(23);
			Assert.Equal(23, duration.Hours);

			int hourComponent = (duration + GameClockDuration.FromHours(1)).Hours;
			Assert.Equal(0, hourComponent);
		}

		[Fact]
		public void TotalSeconds_CanReturnNotInTheRangeOfSafeIntegerOutOfF64()
		{
			GameClockDuration maxDuration = GameClockDuration.MaxValue;
			GameClockDuration minDuration = GameClockDuration.MinValue;

			Assert.False(maxDuration.TotalSeconds < MaxSafeIntegerOutOfF64);
			Assert.False(minDuration.TotalSeconds > MinSafeIntegerOutOfF64);
		}

		[Fact]
		public void CanUse_GameClockDate_SignedDurationSince_BetweenMinAndMax()
		{
			GameClockDate maxDate = GameClockDate.MaxValue;
			GameClockDate minDate = GameClockDate.MinValue;

			const long LeapYearCountOfInt32 = 1041529570;
			const long NonLeapYearCountOfInt32 = 3253437725;
			const long DayCountUInt32YearsLaterSinceInt32MinValueYear = (LeapYearCountOfInt32 * 366)
				+ (NonLeapYearCountOfInt32 * 365);

			GameClockDuration durationFromMaxDateToMindate = maxDate.SignedDurationSince(minDate);
			Assert.Equal(GameClockDuration.FromDays(DayCountUInt32YearsLaterSinceInt32MinValueYear),
				durationFromMaxDateToMindate);
			Assert.Throws<ArgumentOutOfRangeException>(() => durationFromMaxDateToMindate +
				GameClockDuration.FromDays(1));

			GameClockDuration durationFromMinDateToMaxdate = minDate.SignedDurationSince(maxDate);
			Assert.Equal(GameClockDuration.FromDays(-DayCountUInt32YearsLaterSinceInt32MinValueYear),
				durationFromMinDateToMaxdate);
			Assert.Throws<ArgumentOutOfRangeException>(() => durationFromMinDateToMaxdate +
				GameClockDuration.FromDays(-1));
		}

		[Fact]
		public void TotalMinutes_ReturnsInTheRangeOfSafeIntegerOutOfF64()
		{
			GameClockDuration maxDuration = GameClockDuration.MaxValue;
			GameClockDuration minDuration = GameClockDuration.MinValue;

			Assert.True(maxDuration.TotalMinutes < MaxSafeIntegerOutOfF64);
			Assert.True(minDuration.TotalMinutes > MinSafeIntegerOutOfF64);
		}

		[Fact]
		public void TotalHours_ReturnsInTheRangeOfSafeIntegerOutOfF64()
		{
			GameClockDuration maxDuration = GameClockDuration.MaxValue;
			GameClockDuration minDuration = GameClockDuration.MinValue;

			Assert.True(maxDuration.TotalHours < MaxSafeIntegerOutOfF64);
			Assert.True(minDuration.TotalHours > MinSafeIntegerOutOfF64);
		}

		[Fact]
		public void TotalDays_ReturnsInTheRangeOfSafeIntegerOutOfF64()
		{
			GameClockDuration maxDuration = GameClockDuration.MaxValue;
			GameClockDuration minDuration = GameClockDuration.MinValue;

			Assert.True(maxDuration.TotalDays < MaxSafeIntegerOutOfF64);
			Assert.True(minDuration.TotalDays > MinSafeIntegerOutOfF64);
		}

		[Fact]
		public void TotalWeeks_ReturnsInTheRangeOfSafeIntegerOutOfF64()
		{
			GameClockDuration maxDuration = GameClockDuration.MaxValue;
			GameClockDuration minDuration = GameClockDuration.MinValue;

			Assert.True(maxDuration.TotalWeeks < MaxSafeIntegerOutOfF64);
			Assert.True(minDuration.TotalWeeks > MinSafeIntegerOutOfF64);
		}

		[Fact]
		public void FromMethods_NegativeArg_ReturnsTheSameValuesAsPassedValuesButWithTimeFactorMultiplied()
		{
			GameClockDuration durationMinus1Sec = GameClockDuration.FromSeconds(-1);
			GameClockDuration durationMinus2Mins = GameClockDuration.FromMinutes(-2);
			GameClockDuration durationMinus3Hours = GameClockDuration.FromHours(-3);
			GameClockDuration durationMinus4Days = GameClockDuration.FromDays(-4);
			GameClockDuration durationMinus5Weeks = GameClockDuration.FromWeeks(-5);

			Assert.Equal(-1, durationMinus1Sec.WholeSeconds);
			Assert.Equal(-2 * SecsPerMinute, durationMinus2Mins.WholeSeconds);
			Assert.Equal(-3 * SecsPerHour, durationMinus3Hours.WholeSeconds);
			Assert.Equal(-4 * SecsPerDay, durationMinus4Days.WholeSeconds);
			Assert.Equal(-5 * SecsPerWeek, durationMinus5Weeks.WholeSeconds);
		}

		[Fact]
		public void AdditionOperator_AddAnotherValue_ThrowsOverflowExceptionIfOverflowed()
		{
			GameClockDuration maxDuration = GameClockDuration.MaxValue;
			GameClockDuration minDuration = GameClockDuration.MinValue;

			GameClockDuration tempResultDuration = GameClockDuration.FromSeconds(0);

			ArgumentOutOfRangeException exMax = Assert.Throws<ArgumentOutOfRangeException>(() => tempResultDuration = maxDuration + GameClockDuration.FromSeconds(1));
			Assert.Equal("GameClockDuration overflowed because the duration is too long.", exMax.Message);
			Assert.StrictEqual(GameClockDuration.FromSeconds(0), tempResultDuration);

			ArgumentOutOfRangeException exMin = Assert.Throws<ArgumentOutOfRangeException>(() => tempResultDuration = minDuration + GameClockDuration.FromSeconds(-1));
			Assert.Equal("GameClockDuration overflowed because the duration is too long.", exMax.Message);
			Assert.StrictEqual(GameClockDuration.FromSeconds(0), tempResultDuration);
		}

		[Fact]
		public void CompareTo_NextGreaterDuration_ReturnsPositiveValue()
		{

		}

		[Fact]
		public void DividesMaxSafeSqrtIntegerByNextGreaterValueOfInverseOfMaxSafeSqrtIntegerUsingDouble()
		{
			var durationLowerWeighted = GameClockDuration.FromSeconds(94906265);
			double NextGreaterValOfInverseOfMaxSafeSqrtIntegerOutOfDouble = 1.0536712127723509e-8.NextUp();

			GameClockDuration actualCalcRes = (durationLowerWeighted /
				NextGreaterValOfInverseOfMaxSafeSqrtIntegerOutOfDouble);

			Assert.StrictEqual((long)(durationLowerWeighted.WholeSeconds /NextGreaterValOfInverseOfMaxSafeSqrtIntegerOutOfDouble), actualCalcRes.WholeSeconds);

			Assert.NotStrictEqual((long)(durationLowerWeighted.WholeSeconds / (decimal)NextGreaterValOfInverseOfMaxSafeSqrtIntegerOutOfDouble), actualCalcRes.WholeSeconds);
		}
		[Fact]
		public void DividesMaxSafeSqrtIntegerByInverseOfMaxSafeSqrtIntegerUsingDecimal()
		{
			var durationLowerWeighted = GameClockDuration.FromSeconds(94906265);
			const double InverseOfMaxSafeSqrtIntegerOutOfDouble = 1.0536712127723509e-08;

			GameClockDuration actualCalcRes = (durationLowerWeighted / InverseOfMaxSafeSqrtIntegerOutOfDouble);

			Assert.NotStrictEqual((long)(durationLowerWeighted.WholeSeconds / InverseOfMaxSafeSqrtIntegerOutOfDouble), actualCalcRes.WholeSeconds);
			Assert.StrictEqual((long)(durationLowerWeighted.WholeSeconds / (decimal)InverseOfMaxSafeSqrtIntegerOutOfDouble), actualCalcRes.WholeSeconds);
		}

		[Fact]
		public void DividesLowestSafeHigherWeightedValueByLowestSafeInverseOfHigherWeightedValueUsingDouble()
		{
			var durationLowerWeighted = GameClockDuration.FromSeconds(0x3FFF);

			double MinOptimizedValueHigherWeighted = 1.8189894035458565e-12.NextUp();

			Assert.StrictEqual((long)(durationLowerWeighted.WholeSeconds / MinOptimizedValueHigherWeighted), (durationLowerWeighted / MinOptimizedValueHigherWeighted).WholeSeconds);
		}

		[Fact]
		public void DividesHighestSafeHigherWeightedValueByLowestSafeInverseOfLowerWeightedValueUsingDouble()
		{
			var durationHigherWeighted = GameClockDuration.FromSeconds(0x7FFFFFFFFF);

			double MinOptimizedValueLowerWeighted = 6.103515625e-05.NextUp();

			Assert.StrictEqual((long)(durationHigherWeighted.WholeSeconds / MinOptimizedValueLowerWeighted), (durationHigherWeighted / MinOptimizedValueLowerWeighted).WholeSeconds);
		}
	}
}
