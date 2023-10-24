using Xunit;
using GTA.Chrono;
using System;

namespace ScriptHookVDotNet_APIv3_Tests
{
	public class GameClockDateTests
	{
		[Fact]
		public void Equivalent_FromYmd_AndHms_DateTimeCtor()
		{
			const int Year = 2015;
			const int Month = 4;
			const int Day = 14;

			const int Hour = 12;
			const int Min = 0;
			const int Sec = 0;

			GameClockDate date = GameClockDate.FromYmd(Year, Month, Day);
			GameClockTime time = GameClockTime.FromHms(Hour, Min, Sec);

			Assert.Equal(new GameClockDateTime(date, time), date.AndHms(Hour, Min, Sec));
		}
	}
}
