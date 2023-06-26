//
// Copyright (C) 2015 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	/// <summary>
	/// Represents the global game clock.
	/// </summary>
	public static class Clock
	{
		/// <summary>
		/// Gets or sets a value indicating whether the in-game clock is paused.
		/// </summary>
		public static bool IsClockPaused
		{
			get => SHVDN.NativeMemory.IsClockPaused;
			set => Function.Call(Hash.PAUSE_CLOCK, value);
		}

		/// <summary>
		/// Gets or sets the day of month starting from 1 to 31.
		/// The max value is guaranteed to be 31  regardless of the month.
		/// </summary>
		public static int Day
		{
			get => Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH);
			set => Function.Call(Hash.SET_CLOCK_DATE, value, MonthZero, Year);
		}

		/// <summary>
		/// Gets or sets the day of month starting from 1.
		/// </summary>
		public static int Month
		{
			get => Function.Call<int>(Hash.GET_CLOCK_MONTH + 1);
			set => Function.Call(Hash.SET_CLOCK_DATE, Day, value - 1, Year);
		}

		/// <summary>
		/// Gets or sets the day of month starting from 0.
		/// </summary>
		public static int MonthZero
		{
			get => Function.Call<int>(Hash.GET_CLOCK_MONTH);
			set => Function.Call(Hash.SET_CLOCK_DATE, Day, value, Year);
		}
		/// <summary>
		/// Gets or sets the year number from 0 to 9999.
		/// </summary>
		/// <value>
		/// The current year number.
		/// </value>
		public static int Year
		{
			get => Function.Call<int>(Hash.GET_CLOCK_YEAR);
			set => Function.Call(Hash.SET_CLOCK_DATE, Day, MonthZero, value);
		}

		/// <summary>
		/// Gets the day of the week.
		/// </summary>
		/// <remarks>
		/// Returns the cached value, not the value calculated by <see cref="Day"/>, <see cref="Month"/>, and <see cref="Year"/>.
		/// If some of them is modified without updating the cached value for the day of week by direct memory editing,
		/// this property will return a incorrect value.
		/// </remarks>
		public static DayOfWeek DayOfWeek => Function.Call<DayOfWeek>(Hash.GET_CLOCK_DAY_OF_WEEK);

		/// <summary>
		/// Gets or sets the hour number from 0 to 23.
		/// </summary>
		/// <value>
		/// The current hour number.
		/// </value>
		public static int Hour
		{
			get => Function.Call<int>(Hash.GET_CLOCK_HOURS);
			set => Function.Call(Hash.SET_CLOCK_TIME, value, Minute, Second);
		}
		/// <summary>
		/// Gets or sets the minute number from 0 to 59.
		/// </summary>
		/// <value>
		/// The current minute number.
		/// </value>
		public static int Minute
		{
			get => Function.Call<int>(Hash.GET_CLOCK_MINUTES);
			set => Function.Call(Hash.SET_CLOCK_TIME, Hour, value, Second);
		}
		/// <summary>
		/// Gets or sets the second number from 0 to 59.
		/// </summary>
		/// <value>
		/// The current second number.
		/// </value>
		public static int Second
		{
			get => Function.Call<int>(Hash.GET_CLOCK_SECONDS);
			set => Function.Call(Hash.SET_CLOCK_TIME, Hour, Minute, value);
		}

		/// <summary>
		/// Gets or sets the current time of day in the GTA World.
		/// </summary>
		/// <value>
		/// The current time of day.
		/// </value>
		public static TimeSpan TimeOfDay
		{
			get => new (Hour, Minute, Second);
			set => Function.Call(Hash.SET_CLOCK_TIME, value.Hours, value.Minutes, value.Seconds);
		}

		/// <summary>
		/// Gets or sets how many milliseconds in the real world one game minute takes.
		/// </summary>
		/// <value>
		/// The milliseconds one game minute takes in the real world.
		/// </value>
		public static int MillisecondsPerGameMinute
		{
			get => Function.Call<int>(Hash.GET_MILLISECONDS_PER_GAME_MINUTE);
			set => SHVDN.NativeMemory.MillisecondsPerGameMinute = value;
		}

		private static int[] s_firstDaysOfWeekForNonLeapYear = new int[12] { 0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5 };
		private static int[] s_firstDaysOfWeekForLeapYear = new int[12] { 6, 2, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5 };

		/// <summary>
		/// Returns an indication whether the specified year is a leap year.
		/// Calculates in the same way as the game does.
		/// </summary>
		public static bool IsLeapYear(int year)
		{
			return (year % 4) == 0 && year != 100 * (year / 100) || year == 400 * (year / 400);
		}

		/// <param name="day">
		/// The day number from 1 to 31.
		/// The max value is the same regardless of the month number.
		/// </param>
		/// <param name="month">
		/// The month number from 1 to 12.
		/// </param>
		/// <param name="year">
		/// The year number from 1 to 9999.
		/// </param>
		/// <inheritdoc cref="GetDayOfWeekMonthZero(int, int, int)"/>
		public static DayOfWeek GetDayOfWeek(int day, int month, int year) => GetDayOfWeekMonthZero(day, month - 1, year);

		/// <summary>
		/// Gets the day of the week.
		/// Calculates in the same way as the game does.
		/// </summary>
		/// <param name="day">
		/// The day number from 1 to 31.
		/// The max value is the same regardless of the month number.
		/// </param>
		/// <param name="month">
		/// The month number from 0 to 11.
		/// </param>
		/// <param name="year">
		/// The year number from 1 to 9999.
		/// </param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Throws when one of the arguments is out of the range.
		/// </exception>
		public static DayOfWeek GetDayOfWeekMonthZero(int day, int month, int year)
		{
			if (day < 1 || day > 31)
			{
				throw new ArgumentOutOfRangeException(nameof(day));
			}
			if (month < 0 || month > 11)
			{
				throw new ArgumentOutOfRangeException(nameof(month));
			}
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException(nameof(year));
			}

			int century = year % 100;
			int firstDayOfWeek = s_firstDaysOfWeekForNonLeapYear[month];
			int unk1 = 2 * (3 - (year - century) / 100 % 4);
			int unk2 = century + century / 4;
			if (IsLeapYear(year))
			{
				firstDayOfWeek = s_firstDaysOfWeekForLeapYear[month];
			}

			return (DayOfWeek)((day + unk1 + firstDayOfWeek + unk2) % 7);
		}
	}
}
