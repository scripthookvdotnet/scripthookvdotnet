//
// Copyright (C) 2023 kagikn & contributors
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
		/// Gets or sets a value that indicates whether the in-game clock is paused.
		/// </summary>
		public static bool IsPaused
		{
			get => SHVDN.NativeMemory.IsClockPaused;
			set => Function.Call(Hash.PAUSE_CLOCK, value);
		}

		/// <summary>
		/// Gets or sets the last game time the clock is ticked.
		/// You can use this value to calculate pseudo milliseconds along with <see cref="Game.GameTime"/>
		/// and <see cref="MillisecondsPerGameMinute"/> when the clock is not paused.
		/// You can also set a value to this property to shift the clock minute when the clock is not paused.
		/// </summary>
		/// <remarks>
		/// If <see cref="IsPaused"/> is set to <see langword="true"/>, this value will be updated to <see cref="Game.GameTime"/> every frame.
		/// </remarks>
		public static int LastTimeTicked
		{
			get => SHVDN.NativeMemory.LastTimeClockTicked;
			set => SHVDN.NativeMemory.LastTimeClockTicked = value;
		}

		/// <summary>
		/// Gets or sets the day of month.
		/// The max value varies depending on the current month, but may return a value outside the range
		/// of 1 to the max value if not normalized yet.
		/// </summary>
		/// <remarks>
		/// If you set a value not in the range of 1 to the max value depending on the current month,
		/// the game will normalize the date time when the game updates the clock minute.
		/// </remarks>
		public static int Day
		{
			get => Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH);
			set => SetDateZeroBasedMonth(value, MonthZero, Year);
		}

		/// <summary>
		/// Gets or sets the day of month starting from 1.
		/// </summary>
		/// <remarks>
		/// <para>
		/// You should not set a value not in the range of 1 to 12 to this property.
		/// Doing so may result in an unexpected date time, since the game uses an array of days in months
		/// without array bound checking.
		/// </para>
		/// <para>
		/// When you do not plan to use this value to draw on the screen,
		/// consider using <see cref="MonthZero"/> since the game internally uses the zero-based month representation.
		/// </para>
		/// </remarks>
		public static int Month
		{
			get => MonthZero + 1;
			set => MonthZero = value - 1;
		}

		/// <summary>
		/// Gets or sets the day of month starting from 0.
		/// The representation is the same as the game uses for the month.
		/// </summary>
		/// <remarks>
		/// You should not set a value not in the range of 0 to 11 to this property.
		/// Doing so may result in an unexpected date time, since the game uses an array of days in months
		/// without array bound checking.
		/// </remarks>
		public static int MonthZero
		{
			get => Function.Call<int>(Hash.GET_CLOCK_MONTH);
			set => SetDateZeroBasedMonth(Day, value, Year);
		}
		/// <summary>
		/// Gets or sets the year number (no range limitation).
		/// </summary>
		/// <value>
		/// The current year number.
		/// </value>
		/// <remarks>
		/// The game may get considerably heavier if you set the value to a very large value such as 1e+7.
		/// </remarks>
		public static int Year
		{
			get => Function.Call<int>(Hash.GET_CLOCK_YEAR);
			set => SetDateZeroBasedMonth(Day, MonthZero, value);
		}

		/// <summary>
		/// Sets the current date in the GTA world.
		/// </summary>
		/// <param name="day">
		/// The day (1 through the number of days in <paramref name="month"/>).
		/// If you set another value, the game will normalize the date time when the game updates
		/// the clock minute.
		/// </param>
		/// <param name="month">
		/// The month (1 through 12).
		/// </param>
		/// <param name="year">
		/// The year (no range limitation).
		/// </param>
		/// <remarks>
		/// You should not set a value not in the range of 1 to 12 to this property.
		/// Doing so may result in an unexpected date time, since the game uses an array of days in months
		/// without array bound checking.
		/// On the other hands, you can safely set an arbitrary day value as the game normalizes the day value as a result
		/// when the game updates the clock minute.
		/// </remarks>
		public static void SetDate(int day, int month, int year) => SetDateZeroBasedMonth(day, month - 1, year);

		/// <summary>
		/// Sets the current date in the GTA world.
		/// </summary>
		/// <param name="day">
		/// The day (1 through the number of days in <paramref name="month"/>).
		/// If you set another value, the game will normalize the date time when the game updates
		/// the clock minute.
		/// </param>
		/// <param name="month">
		/// The month (0 through 11).
		/// </param>
		/// <param name="year">
		/// The year (no range limitation).
		/// </param>
		/// <remarks>
		/// You should not set a value not in the range of 0 to 11 to this property.
		/// Doing so may result in an unexpected date time, since the game uses an array of days in months
		/// without array bound checking.
		/// On the other hands, you can safely set an arbitrary day value as the game normalizes the day value as a result
		/// when the game updates the clock minute.
		/// </remarks>
		public static void SetDateZeroBasedMonth(int day, int month, int year) => Function.Call(Hash.SET_CLOCK_DATE, day, month, year);

		/// <summary>
		/// Adds the specified number of hours, minutes, and seconds to the current in-game time.
		/// </summary>
		/// <param name="hours">The number of hours to add to the in-game clock.</param>
		/// <param name="minutes">The number of minutes to add to the in-game clock.</param>
		/// <param name="seconds">The number of seconds to add to the in-game clock.</param>
		public static void AddToCurrentTime(int hours, int minutes, int seconds) => Function.Call(Hash.ADD_TO_CLOCK_TIME, hours, minutes, seconds);

		/// <summary>
		/// Gets the day of the week.
		/// </summary>
		/// <remarks>
		/// Returns the cached value, not the value calculated by <see cref="Day"/>, <see cref="Month"/>, and <see cref="Year"/>.
		/// If some of them is modified without updating the cached value for the day of week by direct memory editing,
		/// this property will return an incorrect value.
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
		/// Gets or sets the current time of day in the GTA world.
		/// </summary>
		/// <value>
		/// The current time of day.
		/// </value>
		/// <remarks>
		/// The resolution of the value is 1 second.
		/// </remarks>
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

		// these 2 arrays were taken from the exe (embedded as 4-byte arrays)
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
		/// The day (1 through the number of days in <paramref name="month"/>).
		/// </param>
		/// <param name="month">
		/// The month (1 through 12).
		/// </param>
		/// <param name="year">
		/// The year (no range limitation).
		/// </param>
		/// <inheritdoc cref="GetDayOfWeekZeroBasedMonth(int, int, int)"/>
		public static DayOfWeek GetDayOfWeek(int day, int month, int year) => GetDayOfWeekZeroBasedMonth(day, month - 1, year);

		/// <summary>
		/// Gets the day of the week.
		/// Calculates in the same way as the game does.
		/// </summary>
		/// <param name="day">
		/// The day (1 through the number of days in <paramref name="month"/>).
		/// </param>
		/// <param name="month">
		/// The month (1 through 12).
		/// </param>
		/// <param name="year">
		/// The year (no range limitation).
		/// </param>
		public static DayOfWeek GetDayOfWeekZeroBasedMonth(int day, int month, int year)
		{
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
