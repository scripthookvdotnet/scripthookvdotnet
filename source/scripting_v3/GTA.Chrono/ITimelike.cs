//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA.Chrono
{
    /// <summary>
    /// Defines the common set of methods for time component.
    /// </summary>
    public interface ITimelike<T>
    {
        /// <summary>
        /// Gets the hour component of the time represented by this interface.
        /// </summary>
        public int Hour { get; }

        /// <summary>
        /// Gets the hour number from 1 to 12 of the time represented by this interface with a boolean flag, which is
        /// <see langword="false"/> for AM and <see langword="true"/> for PM.
        /// </summary>
        public (bool isPM, int hour) Hour12 { get; }

        /// <summary>
        /// Gets the minute component of the time represented by this interface.
        /// </summary>
        public int Minute { get; }

        /// <summary>
        /// Gets the second component of the time represented by this interface.
        /// </summary>
        public int Second { get; }

        /// <summary>
        /// Gets the number of seconds past the last midnight.
        /// </summary>
        public int SecondsFromMidnight { get; }

        /// <returns>
        /// <see langword="true"/> if the time represented by this interface is in from midnight to noon (where
        /// <see cref="Hour"/> is between 0 and 11); otherwise, <see langword="false"/>.
        /// </returns>
        public bool GetHour12(out int hour);

        /// <summary>
        /// Makes a new <see cref="ITimelike{T}"/> with the hour number changed.
        /// </summary>
        /// <param name="hour">The new hour.</param>
        /// <returns>
        /// An object whose value is the time represented by this interface but the hour is the specified
        /// <paramref name="hour"/>.
        /// </returns>
        public T WithHour(int hour);

        /// <summary>
        /// Makes a new <see cref="ITimelike{T}"/> with the minute number changed.
        /// </summary>
        /// <param name="minute">The new minute.</param>
        /// <returns>
        /// An object whose value is the time represented by this interface but the minute is the specified
        /// <paramref name="minute"/>.
        /// </returns>
        public T WithMinute(int minute);

        /// <summary>
        /// Makes a new <see cref="ITimelike{T}"/> with the second number changed.
        /// </summary>
        /// <param name="second">The new second.</param>
        /// <returns>
        /// An object whose value is the time represented by this interface but the second is the specified
        /// <paramref name="second"/>.
        /// </returns>
        public T WithSecond(int second);
    }
}
