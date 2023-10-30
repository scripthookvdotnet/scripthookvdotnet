using Xunit;
using GTA.Chrono;
using System;
using System.Collections.Generic;

namespace ScriptHookVDotNet_APIv3_Tests
{
	public class GameClockDateTests
	{
		[Fact]
		public void MaxValue()
		{
			VerifyTimeSpanYmd(GameClockDate.MaxValue, int.MaxValue, 12, 31);
		}

		[Fact]
		public void MinValue()
		{
			VerifyTimeSpanYmd(GameClockDate.MinValue, int.MinValue, 1, 1);
		}

		[Theory]
		[MemberData(nameof(Deconstruct_Method_TestData))]
		public void Can_Deconstruct_Into_Separate_Year_Month_Day_Ints(int year, int month, int day)
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

		private static void VerifyTimeSpanYmd(GameClockDate gameClockDate, int year, int month, int day)
		{
			Assert.Equal(year, gameClockDate.Year);
			Assert.Equal(month, gameClockDate.Month);
			Assert.Equal(day, gameClockDate.Day);
		}
	}
}
