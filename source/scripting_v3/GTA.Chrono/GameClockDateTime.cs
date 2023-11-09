//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA.Chrono
{
	/// <summary>
	/// Represents a combined game clock date and time with the second precision.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <c>With*</c> methods can be convenient to change a single component of a date, but they must be used with
	/// some care. Examples to watch out for:
	/// <list type="bullet">
	/// <item>
	/// <description>
	/// <see cref="WithYear"/> changes the year component of a year-month-day value. Do not use this method if you
	/// want the ordinal to stay the same after changing the year, of if you want the week and weekday values to stay
	/// the same.
	/// </description>
	/// </item>
	/// <item>
	/// <description>
	/// Do not combine two <c>With*</c> methods to change two components of the date. For example to change both the
	/// year and month components of a date. This could fail because an intermediate value does not exist, while the
	/// final date would be valid.
	/// </description>
	/// </item>
	/// </list>
	/// For more complex changes to a date, it is best to create a new <see cref="GameClockDateTime"/> value instead of
	/// altering an existing date.
	/// </para>
	/// </remarks>
	public readonly struct GameClockDateTime : IEquatable<GameClockDateTime>, IComparable<GameClockDateTime>, IComparable, Datelike<GameClockDateTime>, Timelike<GameClockDateTime>
	{
		public GameClockDateTime(GameClockDate date, GameClockTime time) : this()
		{
			_date = date;
			_time = time;
		}

		internal readonly GameClockDate _date;
		internal readonly GameClockTime _time;

		/// <summary>
		/// Gets the largest possible value of <see cref="GameClockDateTime"/>.
		/// </summary>
		/// <value>
		/// The largest possible value of <see cref="GameClockTime"/>, which is <c>+2147483647-12-31T23:59:59</c>
		/// (23:59:59 on December 31, 2147483647 CE).
		/// </value>
		public static readonly GameClockDateTime MaxValue = new(GameClockDate.MaxValue, GameClockTime.MaxValue);
		/// <summary>
		/// Gets the smallest possible value of <see cref="GameClockDateTime"/>.
		/// </summary>
		/// <value>
		/// The smallest possible value of <see cref="GameClockTime"/>, which is <c>-2147483648-01-01T00:00:00</c>
		/// (00:00:00 on January 1, -2147483648 BCE).
		/// </value>
		public static readonly GameClockDateTime MinValue = new(GameClockDate.MinValue, GameClockTime.MinValue);

		/// <summary>
		/// Returns the date component of this instance.
		/// </summary>
		public readonly GameClockDate Date => _date;
		/// <summary>
		/// Returns the time component of this instance.
		/// </summary>
		public readonly GameClockTime Time => _time;

		/// <summary>
		/// Returns the year part of this <see cref="GameClockDateTime"/>.
		/// The returned value is an integer in the range of <see langword="int"/>.
		/// </summary>
		public readonly int Year => _date.Year;

		/// <summary>
		/// Returns the day of the year represented by this <see cref="GameClockDateTime"/> starting from 1.
		/// The returned value is an integer between 1 and 366.
		/// </summary>
		public readonly int DayOfYear => _date.DayOfYear;

		/// <summary>
		/// Returns the day of the year represented by this <see cref="GameClockDateTime"/> starting from 0.
		/// The returned value is an integer between 0 and 365 (the same as <see cref="DayOfYear"/> minus 1).
		/// </summary>
		public readonly int ZeroBasedDayOfYear => _date.ZeroBasedDayOfYear;

		/// <summary>
		/// Returns the day of the week represented by this <see cref="GameClockDateTime"/> in
		/// <see cref="System.DayOfWeek"/>.
		/// The returned value is an integer between 0 and 6, where 0 indicates Sunday, 1 indicates Monday, 2 indicates
		/// Tuesday, 3 indicates Wednesday, 4 indicates Thursday, 5 indicates Friday, and 6 indicates Saturday.
		/// </summary>
		public readonly DayOfWeek DayOfWeek => _date.DayOfWeek;

		/// <summary>
		/// Returns the day of the week for ISO 8601 represented by this <see cref="GameClockDateTime"/> in
		/// <see cref="Chrono.IsoDayOfWeek"/>.
		/// The returned value is an integer between 0 and 6, where 0 indicates Monday, 1 indicates Tuesday,
		/// 2 indicates Wednesday, 3 indicates Thursday, 4 indicates Friday, 5 indicates Saturday, and 6 indicates
		/// Sunday.
		/// </summary>
		public readonly IsoDayOfWeek IsoDayOfWeek => _date.IsoDayOfWeek;

		/// <summary>
		/// Returns the month part of this <see cref="GameClockDateTime"/>.
		/// The returned value is an integer between 1 and 12.
		/// </summary>
		public readonly int Month => _date.Month;

		/// <summary>
		/// Returns the zero-based month part of this <see cref="GameClockDateTime"/>.
		/// The returned value is an integer between 0 and 11 (the same as <see cref="Month"/> minus 1).
		/// </summary>
		public readonly int ZeroBasedMonth => _date.ZeroBasedMonth;

		/// <summary>
		/// Returns the day-of-month part of this <see cref="GameClockDateTime"/>.
		/// The returned value is an integer between 1 and 31.
		/// </summary>
		public readonly int Day => _date.Day;

		/// <summary>
		/// Returns the zero-based day-of-month part of this <see cref="GameClockDateTime"/>.
		/// The returned value is an integer between 0 and 30 (the same as <see cref="Day"/> minus 1).
		/// </summary>
		public readonly int ZeroBasedDay => _date.ZeroBasedDay;

		/// <summary>
		/// Gets the hour component of the time represented by this instance.
		/// </summary>
		public readonly int Hour => _time.Hour;

		/// <summary>
		/// Gets the hour number from 1 to 12 of the time represented by this instance with a boolean flag, which is
		/// <see langword="false"/> for AM and <see langword="true"/> for PM.
		/// </summary>
		public (bool isPM, int hour) Hour12 => _time.Hour12;

		/// <returns>
		/// <see langword="true"/> if the time represented by this instance is in from midnight to noon (where
		/// <see cref="Hour"/> is between 0 and 11); otherwise, <see langword="false"/>.
		/// </returns>
		public bool GetHour12(out int hour) => _time.GetHour12(out hour);

		/// <summary>
		/// Gets the minute component of the time represented by this instance.
		/// </summary>
		public readonly int Minute => _time.Minute;

		/// <summary>
		/// Gets the second component of the time represented by this instance.
		/// </summary>
		public readonly int Second => _time.Second;

		/// <summary>
		/// Gets the number of seconds past the last midnight.
		/// </summary>
		public int SecondsFromMidnight => _time.SecondsFromMidnight;

		/// <summary>
		/// Makes a new <see cref="GameClockDateTime"/> with the year number changed, while keeping the same month and
		/// day.
		/// </summary>
		/// <param name="year">The new year.</param>
		/// <returns>
		/// An object whose value is the date time represented by this instance but the year is the specified
		/// <paramref name="year"/>.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The date is February 29 in a leap year but the specified <paramref name="year"/> is a non-leap one.
		/// </exception>
		public GameClockDateTime WithYear(int year) => new(_date.WithYear(year), _time);

		/// <summary>
		/// Makes a new <see cref="GameClockDateTime"/> with the month number (starting from 1) changed.
		/// </summary>
		/// <param name="month">The new month.</param>
		/// <returns>
		/// An object whose value is the date time represented by this instance but the month is the specified
		/// <paramref name="month"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified <paramref name="month"/> is invalid (not an integer between 1 and 12).
		/// </exception>
		public GameClockDateTime WithMonth(int month) => new(_date.WithMonth(month), _time);

		/// <summary>
		/// Makes a new <see cref="GameClockDateTime"/> with the month number (starting from 0) changed.
		/// </summary>
		/// <param name="month">The new month.</param>
		/// <returns>
		/// An object whose value is the date time represented by this instance but the month is the specified
		/// <paramref name="month"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified <paramref name="month"/> is invalid (not an integer between 0 and 11).
		/// </exception>
		public GameClockDateTime WithZeroBasedMonth(int month) => new(_date.WithZeroBasedMonth(month), _time);

		/// <summary>
		/// Makes a new <see cref="GameClockDateTime"/> with the day of month (starting from 1) changed.
		/// </summary>
		/// <param name="day">The new day of month.</param>
		/// <returns>
		/// An object whose value is the date time represented by this instance but the day of month is the specified
		/// <paramref name="day"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified <paramref name="day"/> is invalid.
		/// </exception>
		public GameClockDateTime WithDay(int day) => new(_date.WithDay(day), _time);

		/// <summary>
		/// Makes a new <see cref="GameClockDateTime"/> with the day of month (starting from 0) changed.
		/// </summary>
		/// <param name="day">The new day of month.</param>
		/// <returns>
		/// An object whose value is the date time represented by this instance but the day of month is the specified
		/// <paramref name="day"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified <paramref name="day"/> is invalid.
		/// </exception>
		public GameClockDateTime WithZeroBasedDay(int day) => new(_date.WithZeroBasedDay(day), _time);

		/// <summary>
		/// Makes a new <see cref="GameClockDateTime"/> with the day of year (starting from 1) changed.
		/// </summary>
		/// <param name="dayOfYear">The day of year.</param>
		/// <returns>
		/// An object whose value is the date time represented by this instance but the day of year is
		/// <paramref name="dayOfYear"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified <paramref name="dayOfYear"/> is invalid.
		/// </exception>
		public GameClockDateTime WithDayOfYear(int dayOfYear) => new(_date.WithDayOfYear(dayOfYear), _time);

		/// <summary>
		/// Makes a new <see cref="GameClockDateTime"/> with the day of year (starting from 0) changed.
		/// </summary>
		/// <param name="dayOfYear">The day of year.</param>
		/// <returns>
		/// An object whose value is the date time represented by this instance but the day of year is
		/// <paramref name="dayOfYear"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified <paramref name="dayOfYear"/> is invalid.
		/// </exception>
		public GameClockDateTime WithZeroBasedDayOfYear(int dayOfYear)
			=> new(_date.WithZeroBasedDayOfYear(dayOfYear), _time);

		/// <summary>
		/// Makes a new <see cref="GameClockDateTime"/> with the hour number changed.
		/// </summary>
		/// <param name="hour">The new hour.</param>
		/// <returns>
		/// An object whose value is the date time represented by this instance but the hour is the specified
		/// <paramref name="hour"/>.
		/// </returns>
		public readonly GameClockDateTime WithHour(int hour) => new(_date, _time.WithHour(hour));

		/// <summary>
		/// Makes a new <see cref="GameClockDateTime"/> with the minute number changed.
		/// </summary>
		/// <param name="minute">The new minute.</param>
		/// <returns>
		/// An object whose value is the date time represented by this instance but the minute is the specified
		/// <paramref name="minute"/>.
		/// </returns>
		public readonly GameClockDateTime WithMinute(int minute) => new(_date, _time.WithMinute(minute));

		/// <summary>
		/// Makes a new <see cref="GameClockDateTime"/> with the second number changed.
		/// </summary>
		/// <param name="second">The new second.</param>
		/// <returns>
		/// An object whose value is the date time represented by this instance but the second is the specified
		/// <paramref name="second"/>.
		/// </returns>
		public readonly GameClockDateTime WithSecond(int second) => new(_date, _time.WithSecond(second));

		/// <summary>
		/// Makes a new <see cref="GameClockDateTime"/> from a <see cref="DateTime"/>.
		/// This does not perform any time zone conversions, so a <see cref="DateTime"/> with a
		/// <see cref="DateTime.Kind"/> of <see cref="DateTimeKind.Utc"/> will still represent the same year/month/day.
		/// </summary>
		public static GameClockDateTime FromSystemDateTime(DateTime dateTime)
		{
			return GameClockDate.FromYmd(dateTime.Year, dateTime.Month, dateTime.Day)
				.AndHms(dateTime.Hour, dateTime.Minute, dateTime.Second);
		}

		/// <summary>
		/// Adds a specified duration to a specified date time, yielding a new date time.
		/// </summary>
		/// <param name="dateTime">The date time value to add.</param>
		/// <param name="duration">The duration value to add.</param>
		/// <returns>
		/// An object that is the sum of the values of <paramref name="dateTime"/> and <paramref name="duration"/>.
		/// </returns>
		/// <remarks>
		/// The addition wraps around and ignores integral number of days.
		/// </remarks>
		public static GameClockDateTime operator +(GameClockDateTime dateTime, GameClockDuration duration)
		{
			if (!dateTime.TryAdd(duration, out GameClockDateTime result))
			{
				throw new ArgumentOutOfRangeException(nameof(duration));
			}

			return result;
		}

		/// <summary>
		/// Returns a new <see cref="GameClockDateTime"/> that adds the specified <see cref="GameClockDuration"/> to
		/// the value of this instance.
		/// </summary>
		/// <param name="duration">
		/// A positive or negative game clock duration.
		/// </param>
		/// <returns>
		/// An object whose value is the sum of the date time represented by this instance and
		/// <paramref name="duration"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The resulting <see cref="GameClockDateTime"/> is less than <see cref="MinValue"/> or greater than
		/// <see cref="MaxValue"/>.
		/// </exception>
		public GameClockDateTime Add(GameClockDuration duration) => this + duration;

		/// <summary>
		/// Tries to return a new <see cref="GameClockDateTime"/> that adds the specified
		/// <see cref="GameClockDuration"/> to the value of this instance. .
		/// </summary>
		/// <param name="duration">
		/// A positive or negative game clock duration.
		/// </param>
		/// <param name="dateTime">
		/// When this method returns, contains the result <see cref="GameClockDateTime"/>, or an undefined value on
		/// failure.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the resulting date time is between <see cref="MinValue"/> and
		/// <see cref="MaxValue"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public bool TryAdd(GameClockDuration duration, out GameClockDateTime dateTime)
		{
			GameClockTime time = _time.OverflowingAddSigned(duration, out long wrappedDays);

			if (!_date.TryAdd(GameClockDuration.FromDays(wrappedDays), out GameClockDate date))
			{
				dateTime = default;
				return false;
			}

			dateTime = new GameClockDateTime(date, time);
			return true;
		}

		/// <summary>
		/// Adds a duration in <paramref name="months"/> to the date time.
		/// </summary>
		/// <param name="months">The number of months to add. Can be negative.</param>
		/// <returns>
		/// An object whose value is the sum of the date represented by this instance and <paramref name="months"/> and
		/// the time component of this instance.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The resulting <see cref="GameClockDateTime"/> is less than <see cref="MinValue"/> or greater than
		/// <see cref="MaxValue"/>.
		/// </exception>
		/// <remarks>
		/// Uses the last day of the month if the day does not exist in the resulting month.
		/// </remarks>
		public GameClockDateTime AddMonths(long months)
			=> new(_date.AddMonths(months), _time);

		/// <summary>
		/// Tries to add a duration in <paramref name="months"/> to the date time.
		/// </summary>
		/// <param name="months">The number of months to add. Can be negative.</param>
		/// <param name="dateTime">
		/// When this method returns, contains the result <see cref="GameClockDateTime"/>, or an undefined value on
		/// failure.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the resulting date time is between <see cref="MinValue"/> and
		/// <see cref="MaxValue"/>; otherwise, <see langword="false"/>.
		/// </returns>
		/// <remarks>
		/// Uses the last day of the month if the day does not exist in the resulting month.
		/// </remarks>
		public bool TryAddMonths(long months, out GameClockDateTime dateTime)
		{
			if (_date.TryAddMonths(months, out GameClockDate date))
			{
				dateTime = default;
				return false;
			}

			dateTime = new GameClockDateTime(date, _time);
			return true;
		}

		/// <summary>
		/// Subtracts a specified duration from a specified date time and returns a new time.
		/// </summary>
		/// <param name="dateTime">The date time value to subtract from.</param>
		/// <param name="duration">The duration value to subtract.</param>
		/// <returns>
		/// An object whose value is the value of <paramref name="dateTime"/> minus the value of
		/// <paramref name="duration"/>.
		/// </returns>
		/// <remarks>
		/// The subtraction wraps around and ignores integral number of days.
		/// </remarks>
		public static GameClockDateTime operator -(GameClockDateTime dateTime, GameClockDuration duration)
		{
			if (!dateTime.TrySubtract(duration, out GameClockDateTime result))
			{
				throw new ArgumentOutOfRangeException(nameof(duration));
			}

			return result;
		}

		/// <summary>
		/// Returns a new <see cref="GameClockDateTime"/> that subtracts the specified <see cref="GameClockDuration"/>
		/// to the value of this instance.
		/// </summary>
		/// <param name="duration">
		/// A positive or negative game clock duration.
		/// </param>
		/// <returns>
		/// An object whose value is the date time represented by this instance minus the duration represented
		/// by <paramref name="duration"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The resulting <see cref="GameClockDateTime"/> is less than <see cref="MinValue"/> or greater than
		/// <see cref="MaxValue"/>.
		/// </exception>
		public GameClockDateTime Subtract(GameClockDuration duration) => this - duration;

		/// <summary>
		/// Tries to return a new <see cref="GameClockDateTime"/> that subtracts the specified
		/// <see cref="GameClockDuration"/> to the value of this instance.
		/// </summary>
		/// <param name="duration">
		/// A positive or negative game clock duration.
		/// </param>
		/// <param name="dateTime">
		/// When this method returns, contains the result <see cref="GameClockDateTime"/>, or an undefined value on
		/// failure.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the resulting date time is between <see cref="MinValue"/> and
		/// <see cref="MaxValue"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public bool TrySubtract(GameClockDuration duration, out GameClockDateTime dateTime)
		{
			GameClockTime time = _time.OverflowingSubtractSigned(duration, out long wrappedDays);

			if (!_date.TrySubtract(GameClockDuration.FromDays(wrappedDays), out GameClockDate date))
			{
				dateTime = default;
				return false;
			}

			dateTime = new GameClockDateTime(date, time);
			return true;
		}

		public GameClockDateTime SubtractMonths(long months)
		{
			if (!TrySubtractMonths(months, out GameClockDateTime dateTime))
			{
				throw new ArgumentOutOfRangeException(nameof(months));
			}

			return dateTime;
		}

		public bool TrySubtractMonths(long months, out GameClockDateTime dateTime)
		{
			if (_date.TrySubtractMonths(months, out GameClockDate date))
			{
				dateTime = default;
				return false;
			}

			dateTime = new GameClockDateTime(date, _time);
			return true;
		}

		/// <summary>
		/// Deconstructs a time into game clock date and time components.
		/// </summary>
		/// <param name="date">The game clock date component.</param>
		/// <param name="time">The game clock time component.</param>
		public readonly void Deconstruct(out GameClockDate date, out GameClockTime time)
		{
			date = _date;
			time = _time;
		}

		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified <see cref="GameClockDateTime"/>
		/// object.
		/// </summary>
		/// <param name="other">An object to compare with this instance.</param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="other"/> represents the same game clock date time as this
		/// instance; otherwise, <see langword="false"/>.
		/// </returns>
		public bool Equals(GameClockDateTime other)
		{
			return _date == other._date && _time == other._time;
		}
		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="obj"/> is a <see cref="GameClockDateTime"/> object that
		/// represents the same game clock date time as the current <see cref="GameClockDateTime"/> structure;
		/// otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is GameClockDateTime duration)
			{
				return Equals(duration);
			}

			return false;
		}

		/// <summary>
		/// Indicates whether two <see cref="GameClockDateTime"/> instances are equal.
		/// </summary>
		/// <param name="left">The game clock date time to compare.</param>
		/// <param name="right">The second game date time to compare.</param>
		/// <returns>
		/// <see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are equal;
		/// otherwise, <see langword="false"/>.
		/// </returns>
		public static bool operator ==(GameClockDateTime left, GameClockDateTime right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Indicates whether two <see cref="GameClockDateTime"/> instances are not equal.
		/// </summary>
		/// <param name="left">The game clock date time to compare.</param>
		/// <param name="right">The second game clock date time to compare.</param>
		/// <returns>
		/// <see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are not equal;
		/// otherwise, <see langword="false"/>.
		/// </returns>
		public static bool operator !=(GameClockDateTime left, GameClockDateTime right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Compares the value of this instance to a specified object that contains a specified
		/// <see cref="GameClockDateTime"/> value, and returns an integer that indicates whether this instance is
		/// earlier than, the same as, or later than the specified <see cref="GameClockDateTime"/> value.
		/// </summary>
		/// <param name="value">A boxed object to compare, or <see langword="null"/>.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and the value parameter. Less than zero if
		/// this instance is earlier than value. Zero if this instance is the same as value. Greater than zero if this
		/// instance is later than value.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// <paramref name="value"/> is not a <see cref="GameClockDateTime"/>.
		/// </exception>
		public int CompareTo(object value)
		{
			if (value == null) return 1;
			if (value is not GameClockDateTime otherDateTime)
				throw new ArgumentException();
	
			return CompareTo(otherDateTime);
		}

		/// <summary>
		/// Compares the value of this instance to a specified <see cref="GameClockDateTime"/> value and indicates
		/// whether this instance is earlier than, the same as, or later than the specified
		/// <see cref="GameClockDateTime"/>
		/// value.
		/// </summary>
		/// <param name="value">The object to compare to the current instance.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and the value parameter.
		/// <list type="bullet">
		/// <item>
		/// <description>Less than zero if this instance is earlier  than <paramref name="value"/>.</description>
		/// </item>
		/// <item>
		/// <description>Zero if this instance is the same as <paramref name="value"/>.</description>
		/// </item>
		/// <item>
		/// <description>Greater than zero if this instance is later than <paramref name="value"/>.</description>
		/// </item>
		/// </list>
		/// </returns>
		public int CompareTo(GameClockDateTime value)
		{
			int dateCompRes = _date.CompareTo(value._date);
			if (dateCompRes != 0)
			{
				return dateCompRes;
			}

			return _time.CompareTo(value._time);
		}

		/// <summary>
		/// Indicates whether a specified <see cref="GameClockDateTime"/> is earlier than another specified
		/// <see cref="GameClockDateTime"/>.
		/// </summary>
		/// <param name="dt1">The first game clock date time to compare.</param>
		/// <param name="dt2">The second game clock date time to compare.</param>
		/// <returns>
		/// <see langword="true"/> if the value of <paramref name="dt1"/> is earlier than the value of
		/// <paramref name="dt2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool operator <(GameClockDateTime dt1, GameClockDateTime dt2) => dt1.CompareTo(dt2) < 0;
		/// <summary>
		/// Indicates whether a specified <see cref="GameClockDateTime"/> is earlier than or equal to another specified
		/// <see cref="GameClockDateTime"/>.
		/// </summary>
		/// <param name="dt1">The first game clock date time to compare.</param>
		/// <param name="dt2">The second game clock date time to compare.</param>
		/// <returns>
		/// <see langword="true"/> if the value of <paramref name="dt1"/> is earlier than or equal to the value of
		/// <paramref name="dt2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool operator <=(GameClockDateTime dt1, GameClockDateTime dt2) => dt1.CompareTo(dt2) <= 0;

		/// <summary>
		/// Indicates whether a specified <see cref="GameClockDateTime"/> is later than another specified
		/// <see cref="GameClockDateTime"/>.
		/// </summary>
		/// <param name="dt1">The first game clock date to compare.</param>
		/// <param name="dt2">The second game clock date to compare.</param>
		/// <returns>
		/// <see langword="true"/> if the value of <paramref name="dt1"/> is later than the value of
		/// <paramref name="dt2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool operator >(GameClockDateTime dt1, GameClockDateTime dt2) => dt1.CompareTo(dt2) > 0;
		/// <summary>
		/// Indicates whether a specified <see cref="GameClockDateTime"/> is later than or equal to another specified
		/// <see cref="GameClockDateTime"/>.
		/// </summary>
		/// <param name="dt1">The first game clock date time to compare.</param>
		/// <param name="dt2">The second game clock date time to compare.</param>
		/// <returns>
		/// <see langword="true"/> if the value of <paramref name="dt1"/> is later than or equal to the value of
		/// <paramref name="dt2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool operator >=(GameClockDateTime dt1, GameClockDateTime dt2) => dt1.CompareTo(dt2) >= 0;

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return _date.GetHashCode() + 17 * _time.GetHashCode();
		}
	}
}
