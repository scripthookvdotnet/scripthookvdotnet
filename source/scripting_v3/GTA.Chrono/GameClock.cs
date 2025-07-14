//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using GTA.Native;

namespace GTA.Chrono
{
    /// <summary>
    /// Represents the global game clock.
    /// </summary>
    public static class GameClock
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
        /// Gets or sets the last game time minutes are added to the clock.
        /// You can use this value to calculate pseudo milliseconds along with <see cref="Game.GameTime"/>
        /// and <see cref="MillisecondsPerGameMinute"/> when the clock is not paused.
        /// You can also set a value to this property to shift the clock minute when the clock is not paused.
        /// </summary>
        /// <remarks>
        /// If <see cref="IsPaused"/> is set to <see langword="true"/>, this value will be updated to
        /// <see cref="Game.GameTime"/> every frame. This property accesses <c>CClock::ms_timeLastMinAdded</c>
        /// internally.
        /// </remarks>
        public static int LastTimeMinAdded
        {
            get => SHVDN.NativeMemory.LastTimeClockTicked;
            set => SHVDN.NativeMemory.LastTimeClockTicked = value;
        }

        /// <summary>
        /// Gets or sets the current date time.
        /// </summary>
        /// <exception cref="InvalidInternalMonthOfGameClockException">
        /// The month starting from 1 is not in the range of 1 to 12 inclusive and therefore the date time cannot be
        /// semantically normalized (can be thrown only from the getter).
        /// </exception>
        /// <remarks>
        /// <para>
        /// Normalizes the day of the date and the time when getting the current value if they are not normalized.
        /// For example, "September 47th, 2013 30:64:90" will be normalized to "October 18th, 2013 04:65:30".
        /// </para>
        /// <para>
        /// The game may get considerably heavier if you set the value to a value with a large year value such as
        /// <c>1e+7</c>.
        /// </para>
        /// </remarks>
        public static GameClockDateTime Now
        {
            get
            {
                int month0 = Month0;
                if (month0 < 0 || month0 > 11)
                {
                    ThrowInvalidInternalMonthOfGameClockException(month0);
                }

                int hour = Hour;
                int minute = Minute;
                int second = Second;
                int dayDiffForNormalizedDate = 0;
                if (second < 0 || second > 59 || minute < 0 || minute > 59 || hour < 0 || hour > 23)
                {
                    dayDiffForNormalizedDate += NormalizeTime(ref second, ref minute, ref hour);
                }

                GameClockTime time = GameClockTime.FromHms(hour, minute, second);

                int year = Year;
                int day = Day + dayDiffForNormalizedDate;
                if (day < 0 || day > GetDaysOfMonth0(month0, year))
                {
                    NormalizeDate(ref year, ref month0, ref day);
                }

                return GameClockDate.FromYmd(year, month0 + 1, day).AndTime(time);
            }
            set
            {
                (GameClockDate date, GameClockTime time) = value;
                (int year, int month, int day) = date;

                SetDate(day, month, year);
                TimeOfDay = time;
            }
        }

        /// <summary>
        /// Gets or sets the current date.
        /// </summary>
        /// <exception cref="InvalidInternalMonthOfGameClockException">
        /// The month starting from 1 is not in the range of 1 to 12 inclusive and therefore the date cannot be
        /// semantically normalized (can be thrown only from the getter).
        /// </exception>
        /// <remarks>
        /// <para>
        /// The getter normalizes the day of the date if it is not normalized.
        /// For example, "September 47th, 2013" will be normalized to "October 17th, 2013".
        /// </para>
        /// <para>
        /// The setter normalizes the time of day if it is not normalized so the getter will be guaranteed to return
        /// the same date right after setting the value.
        /// </para>
        /// <para>
        /// The game may get considerably heavier if you set the value to a value with a large year value such as
        /// <c>1e+7</c>.
        /// </para>
        /// </remarks>
        public static GameClockDate Today
        {
            get => Now.Date;
            set
            {
                (int year, int month, int day) = value;
                SetDate(day, month, year);

                // Normalize the time if it is not normalized so the getter will be guaranteed to return the same date
                // right after setting the value.
                int hour = Hour;
                int minute = Minute;
                int second = Second;
                if (second < 0 || second > 59 || minute < 0 || minute > 59 || hour < 0 || hour > 23)
                {
                    NormalizeTime(ref second, ref minute, ref hour);
                    Hour = hour;
                    Minute = minute;
                    Second = second;
                }
            }
        }

        private static void ThrowInvalidInternalMonthOfGameClockException(int month0)
        {
            throw new InvalidInternalMonthOfGameClockException(month0);
        }

        /// <summary>
        /// Gets or sets the current time.
        /// </summary>
        /// <remarks>
        /// The getter normalizes the time if it is not normalized.
        /// For example, "30:89:72" will be normalized to "7:30:12".
        /// </remarks>
        public static GameClockTime TimeOfDay
        {
            get
            {
                int hour = Hour;
                int minute = Minute;
                int second = Second;
                if (second < 0 || second > 59 || minute < 0 || minute > 59 || hour < 0 || hour > 23)
                {
                    NormalizeTime(ref second, ref minute, ref hour);
                }

                return GameClockTime.FromHms(hour, minute, second);
            }

            set
            {
                (int hour, int minute, int second) = value;
                Hour = hour;
                Minute = minute;
                Second = second;
            }
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
            set => SetDateMonth0(value, Month0, Year);
        }

        /// <summary>
        /// Gets or sets the day of month starting from 1.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value is not between 0 and 11 (in the setter only).
        /// </exception>
        /// <remarks>
        /// <para>
        /// You should not expect the getter to always return a value in the range of 1 to 12.
        /// Although the game uses an array of days in months (12 elements) without array bound checking
        /// and the invalid month will result an unexpected date time, the game having an invalid month for the game
        /// clock will not crash the game.
        /// </para>
        /// <para>
        /// When you do not plan to use this value to draw on the screen,
        /// consider using <see cref="Month0"/> since the game internally uses the zero-based month representation.
        /// </para>
        /// </remarks>
        public static int Month
        {
            get => Month0 + 1;
            set
            {
                ThrowHelper.CheckArgumentRange(nameof(value), value, 1, 12);
                SetDate(Day, value, Year);
            }
        }

        /// <summary>
        /// Gets or sets the day of month starting from 0.
        /// The representation is the same as the game uses for the month.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value is not between 0 and 11 (in the setter only).
        /// </exception>
        /// <remarks>
        /// You should not expect the getter to always return a value in the range of 0 to 11.
        /// Although the game uses an array of days in months (12 elements) without array bound checking
        /// and the invalid month will result an unexpected date time, the game having an invalid month for the game
        /// clock will not crash the game.
        /// </remarks>
        public static int Month0
        {
            get => Function.Call<int>(Hash.GET_CLOCK_MONTH);
            set
            {
                ThrowHelper.CheckArgumentRange(nameof(value), value, 0, 11);
                SetDateMonth0(Day, value, Year);
            }
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
            set => SetDateMonth0(Day, Month0, value);
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
        private static void SetDate(int day, int month, int year) => SetDateMonth0(day, month - 1, year);

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
        private static void SetDateMonth0(int day, int month, int year) => Function.Call(Hash.SET_CLOCK_DATE, day, month, year);

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

        /// <summary>
        /// Normalizes a date just like how the game clock normalizes the date if the day value is not normalized.
        /// Cannot normalize if month0 is not in the range of 0 to 11 since the method can't determine how many days
        /// to add or subtract.
        /// </summary>
        private static void NormalizeDate(ref int year, ref int month0, ref int day)
        {
            while (true)
            {
                int monthDay = GetDaysOfMonth0(month0, year);
                if (day <= monthDay)
                {
                    break;
                }

                day -= monthDay;
                ShiftMonthsAndNormalizeYearAndMonth(ref year, ref month0, 1);
            }
            while (day <= 0)
            {
                if (month0 == 0)
                {
                    month0 = 11;
                    --year;
                }
                else
                {
                    --month0;
                }

                day += GetDaysOfMonth0(month0, year);
            }

            return;

            static void ShiftMonthsAndNormalizeYearAndMonth(ref int year, ref int month0, int diff)
            {
                month0 += diff;
                if (month0 > 11)
                {
                    do
                    {
                        month0 -= 12;
                        year++;
                    }
                    while (month0 > 11);
                }
                else if (month0 < 0)
                {
                    do
                    {
                        month0 += 12;
                        year--;
                    }
                    while (month0 < 0);
                }
            }
        }

        /// <summary>
        /// Normalizes a time in the almost same way from how the game clock normalizes the hour, minute, and second
        /// (which is done by shifting them by one at the same time). Returns the number of day to add for the
        /// normalized date.
        /// </summary>
        private static int NormalizeTime(ref int second, ref int minute, ref int hour)
        {
            if (second >= 0 && second <= 59)
            {
                if (minute < 0 || minute >= 60)
                {
                    return NormalizeMinuteInternal(ref minute, ref hour);
                }
                else
                {
                    // Should not come here, because the game would crash for an invalid hour value less than -1 or
                    // more than 25. "Invalid" minute or second values don't crash the game, but invalid hour values
                    // do. We just don't want to throw an exception, since we could semantically normalize hour values,
                    // which is different from invalid month values.
                    return NormalizeHourInternal(ref hour);
                }
            }

            int dayDiff = 0;
            while (second >= 60)
            {
                second -= 60;
                dayDiff += AddMinuteInternal(ref minute, ref hour, 1);
            }
            while (second < 0)
            {
                second += 60;
                dayDiff += AddMinuteInternal(ref minute, ref hour, -1);
            }

            return dayDiff;
        }

        static int AddMinuteInternal(ref int minute, ref int hour, int diff)
        {
            minute += diff;

            return NormalizeMinuteInternal(ref minute, ref hour);
        }

        static int NormalizeMinuteInternal(ref int minute, ref int hour)
        {
            int dayDiff = 0;

            while (minute >= 60)
            {
                minute -= 60;
                dayDiff += AddHourInternal(ref hour, 1);
            }
            while (minute < 0)
            {
                minute += 60;
                dayDiff += AddHourInternal(ref hour, -1);
            }

            return dayDiff;
        }

        static int AddHourInternal(ref int hour, int diff)
        {
            hour += diff;

            return NormalizeHourInternal(ref hour);
        }

        static int NormalizeHourInternal(ref int hour)
        {
            int dayDiff = 0;

            while (hour >= 24)
            {
                hour -= 24;
                dayDiff++;
            }
            while (hour < 0)
            {
                hour += 24;
                dayDiff--;
            }

            return dayDiff;
        }

        /// <summary>
        /// Get the number of month. Returns 31 if month0 is not in the range of 0 to 11 for smaller code size
        /// (having a statement that throws an exception significantly increases code size).
        /// </summary>
        private static int GetDaysOfMonth0(int month0, int year)
        {
            return month0 switch
            {
                1 => IsLeapYear(year) ? 29 : 28,
                3 or 5 or 8 or 10 => 30,
                _ => 31,
            };
        }

        private static bool IsLeapYear(int year) => year % 4 == 0 && (year % 25 != 0 || year % 16 == 0);
    }
}
