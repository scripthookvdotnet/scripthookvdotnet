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
				{ GameClockDate.FromYmd(2014, 5, 6).AndHms(7, 8, 9), GameClockDuration.FromSeconds(3600 + 60 + 1),
					GameClockDate.FromYmd(2014, 5, 6).AndHms(8, 9, 10) },
				{ GameClockDate.FromYmd(2014, 5, 6).AndHms(7, 8, 9), GameClockDuration.FromSeconds(-(3600 + 60 + 1)),
					GameClockDate.FromYmd(2014, 5, 6).AndHms(6, 7, 8) },
				{ GameClockDate.FromYmd(2014, 5, 6).AndHms(7, 8, 9), GameClockDuration.FromSeconds(86399),
					GameClockDate.FromYmd(2014, 5, 7).AndHms(7, 8, 8) },
				{ GameClockDate.FromYmd(2014, 5, 6).AndHms(7, 8, 9), GameClockDuration.FromSeconds(86_400 * 10),
					GameClockDate.FromYmd(2014, 5, 16).AndHms(7, 8, 9) },
				{ GameClockDate.FromYmd(2014, 5, 6).AndHms(7, 8, 9), GameClockDuration.FromSeconds(-86_400 * 10),
					GameClockDate.FromYmd(2014, 4, 26).AndHms(7, 8, 9) },
			};

		[Theory]
		[MemberData(nameof(Add_Duration_Valid_TestData))]
		public void Addition_of_duration_to_GameClockDateTime_can_change_both_date_and_time(GameClockDateTime dateTime,
			GameClockDuration duration, GameClockDateTime expected)
		{
			GameClockDateTime actualAddMethod = dateTime.Add(duration);
			GameClockDateTime actualAdditionOp = dateTime + duration;
			bool tryAddSuccessfullyConstructedDateTime = dateTime.TryAdd(duration, out GameClockDateTime actualTryAdd);

			Assert.Equal(expected, actualAddMethod);
			Assert.Equal(expected, actualAdditionOp);

			Assert.True(tryAddSuccessfullyConstructedDateTime);
			Assert.Equal(expected, actualTryAdd);
		}

		[Theory]
		[MemberData(nameof(Add_Duration_Valid_TestData))]
		public void Subtraction_of_duration_from_GameClockDateTime_is_the_same_as_addition_of_negated_duration(
			GameClockDateTime dateTime, GameClockDuration duration, GameClockDateTime expected)
		{
			GameClockDateTime actualSubtractMethod = dateTime.Subtract(-duration);
			GameClockDateTime actualSubtractionOp = dateTime - (-duration);
			bool trySubtractSuccessfullyConstructedDateTime
				= dateTime.TrySubtract(-duration, out GameClockDateTime actualTrySubtract);

			Assert.Equal(expected, actualSubtractMethod);
			Assert.Equal(expected, actualSubtractionOp);

			Assert.True(trySubtractSuccessfullyConstructedDateTime);
			Assert.Equal(expected, actualTrySubtract);
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
