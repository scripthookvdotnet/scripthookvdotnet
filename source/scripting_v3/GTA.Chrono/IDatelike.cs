//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA.Chrono
{
    /// <summary>
    /// Defines the common set of methods for date component.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Methods such as <see cref="Year"/>, <see cref="Month"/>, <see cref="Day"/>, <see cref="DayOfWeek"/>, and
    /// <see cref="IsoDayOfWeek"/> can be used to get basic information about the date. The <c>With*</c> methods can
    /// change the date.
    /// </para>
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
    /// For more complex changes to a date, it is best to create a new <see cref="IDatelike{T}"/> value instead of
    /// altering an existing date.
    /// </para>
    /// </remarks>
    public interface IDatelike<T>
    {
        /// <summary>
        /// Returns the year part of this interface. The returned value is an integer in the range of <see langword="int"/>.
        /// </summary>
        public int Year { get; }

        /// <summary>
        /// Returns the month part of this interface. The returned value is an integer between 1 and 12.
        /// </summary>
        public int Month { get; }

        /// <summary>
        /// Returns the zero-based month part of this interface.
        /// The returned value is an integer between 0 and 11 (the same as <see cref="Month"/> minus 1).
        /// </summary>
        public int Month0 { get; }

        /// <summary>
        /// Returns the day-of-month part of this interface. The returned value is an integer between 1 and 31.
        /// </summary>
        public int Day { get; }

        /// <summary>
        /// Returns the zero-based day-of-month part of this interface.
        /// The returned value is an integer between 0 and 30 (the same as <see cref="Day"/> minus 1).
        /// </summary>
        public int Day0 { get; }

        /// <summary>
        /// Returns the day of the year represented by this interface starting from 1.
        /// The returned value is an integer between 1 and 366.
        /// </summary>
        public int DayOfYear { get; }

        /// <summary>
        /// Returns the day of the year represented by this interface starting from 0.
        /// The returned value is an integer between 0 and 365 (the same as <see cref="DayOfYear"/> minus 1).
        /// </summary>
        public int DayOfYear0 { get; }

        /// <summary>
        /// Returns the day of the week represented by this interface in <see cref="System.DayOfWeek"/>.
        /// The returned value is an integer between 0 and 6, where 0 indicates Sunday, 1 indicates Monday, 2 indicates
        /// Tuesday, 3 indicates Wednesday, 4 indicates Thursday, 5 indicates Friday, and 6 indicates Saturday.
        /// </summary>
        public DayOfWeek DayOfWeek { get; }

        /// <summary>
        /// Returns the day of the week for ISO 8601 represented by this interface in <see cref="GTA.Chrono.IsoDayOfWeek"/>.
        /// The returned value is an integer between 0 and 6, where 0 indicates Monday, 1 indicates Tuesday,
        /// 2 indicates Wednesday, 3 indicates Thursday, 4 indicates Friday, 5 indicates Saturday, and 6 indicates
        /// Sunday.
        /// </summary>
        public IsoDayOfWeek IsoDayOfWeek { get; }

        /// <summary>
        /// Makes a new <see cref="IDatelike{T}"/> with the year number changed, while keeping the same month and day.
        /// </summary>
        /// <param name="year">The new year.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the year is the specified
        /// <paramref name="year"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The date is February 29 in a leap year but the specified <paramref name="year"/> is a non-leap one.
        /// </exception>
        /// <remarks>
        /// Do not use this method if you want the ordinal to stay the same after changing the year, or if you want the
        /// week and weekday values to stay the same.
        /// </remarks>
        public T WithYear(int year);

        /// <summary>
        /// Makes a new <see cref="IDatelike{T}"/> with the month number (starting from 1) changed.
        /// </summary>
        /// <param name="month">The new month.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the month is the specified
        /// <paramref name="month"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified <paramref name="month"/> is invalid (not an integer between 1 and 12).
        /// </exception>
        public T WithMonth(int month);

        /// <summary>
        /// Makes a new <see cref="IDatelike{T}"/> with the month number (starting from 0) changed.
        /// </summary>
        /// <param name="month0">The new month.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the month is the specified
        /// <paramref name="month0"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified <paramref name="month0"/> is invalid (not an integer between 0 and 11).
        /// </exception>
        public T WithMonth0(int month0);

        /// <summary>
        /// Makes a new <see cref="IDatelike{T}"/> with the day of month (starting from 1) changed.
        /// </summary>
        /// <param name="day">The new day of month.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the day of month is the specified
        /// <paramref name="day"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified <paramref name="day"/> is invalid.
        /// </exception>
        public T WithDay(int day);

        /// <summary>
        /// Makes a new <see cref="IDatelike{T}"/> with the day of month (starting from 0) changed.
        /// </summary>
        /// <param name="day0">The new day of month.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the day of month is the specified
        /// <paramref name="day0"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified <paramref name="day0"/> is invalid.
        /// </exception>
        public T WithDay0(int day0);

        /// <summary>
        /// Makes a new <see cref="IDatelike{T}"/> with the day of year (starting from 1) changed.
        /// </summary>
        /// <param name="dayOfYear">The day of year.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the day of year is
        /// <paramref name="dayOfYear"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified <paramref name="dayOfYear"/> is invalid.
        /// </exception>
        public T WithDayOfYear(int dayOfYear);

        /// <summary>
        /// Makes a new <see cref="IDatelike{T}"/> with the day of year (starting from 0) changed.
        /// </summary>
        /// <param name="dayOfYear0">The day of year.</param>
        /// <returns>
        /// An object whose value is the date represented by this instance but the day of year is
        /// <paramref name="dayOfYear0"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified <paramref name="dayOfYear0"/> is invalid.
        /// </exception>
        public T WithDayOfYear0(int dayOfYear0);
    }
}
