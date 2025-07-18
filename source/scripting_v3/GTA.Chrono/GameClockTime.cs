//
// Copyright 2012-2014 The Rust Project Developers
// Copyright 2023 kagikn & contributors
// Licensed under the MIT License.
// Original license:
// https://github.com/chronotope/chrono/blob/a44142528eb8bab32f9e16cb74e84bb060f4a667/LICENSE.txt
//

using System;
using System.Runtime.CompilerServices;

namespace GTA.Chrono
{
    /// <summary>
    /// Represents a game clock time with the second precision.
    /// </summary>
    /// <remarks>
    /// The <see cref="GameClockTime"/> structure does not store the millisecond component. Although the game clock
    /// stores the last time its minute ticked (which can be accessed via <see cref="GameClock.LastTimeMinAdded"/>)
    /// and you can calculate the pseudo-millisecond component, the last time is volatile, and it will be set to
    /// the same value as <see cref="Game.GameTime"/> every frame when the clock is paused.
    /// </remarks>
    public readonly struct GameClockTime : IEquatable<GameClockTime>, IComparable<GameClockTime>, IComparable, ITimelike<GameClockTime>
    {
        private readonly int _secs;

        internal GameClockTime(int secs) : this()
        {
            _secs = secs;
        }

        /// <summary>
        /// Gets the largest possible value of <see cref="GameClockDate"/>.
        /// </summary>
        /// <value>
        /// The largest possible value of <see cref="GameClockTime"/>, which is 23:59:59.
        /// </value>
        public static GameClockTime MaxValue = new(86400 - 1);
        /// <summary>
        /// Gets the smallest possible value of <see cref="GameClockDate"/>.
        /// </summary>
        /// <value>
        /// The smallest possible value of <see cref="GameClockTime"/>, which is 00:00:00 (midnight).
        /// </value>
        public static GameClockTime MinValue = new(0);

        /// <summary>
        /// Gets the hour component of the time represented by this instance.
        /// </summary>
        public int Hour => _secs / 3600;

        /// <summary>
        /// Gets the hour number from 1 to 12 of the time represented by this instance with a boolean flag, which is
        /// <see langword="false"/> for AM and <see langword="true"/> for PM.
        /// </summary>
        public (bool isPM, int hour) Hour12
        {
            get
            {
                bool isPM = GetHour12(out int hour);
                return (isPM, hour);
            }
        }

        /// <returns>
        /// <see langword="true"/> if the time represented by this instance is in from midnight to noon (where
        /// <see cref="Hour"/> is between 0 and 11); otherwise, <see langword="false"/>.
        /// </returns>
        public bool GetHour12(out int hour)
        {
            int hour24 = Hour;
            int hour12 = hour24 % 12;
            if (hour12 == 0)
            {
                hour12 = 12;
            }

            hour = hour12;
            return hour24 >= 12;
        }

        /// <summary>
        /// Gets the minute component of the time represented by this instance.
        /// </summary>
        public int Minute => ((_secs / 60) % 60);

        /// <summary>
        /// Gets the second component of the time represented by this instance.
        /// </summary>
        public int Second => (_secs % 60);

        /// <summary>
        /// Gets the number of seconds past the last midnight.
        /// </summary>
        public int SecondsFromMidnight => Hour * 3600 + Minute * 60 + Second;

        /// <summary>
        /// Makes a new <see cref="GameClockTime"/> from hour, minute, second and microsecond.
        /// </summary>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        /// <returns>The resulting time.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The parameters do not form a valid time.
        /// </exception>
        public static GameClockTime FromHms(int hour, int minute, int second)
        {
            ThrowHelper.CheckArgumentRange(nameof(hour), hour, 0, 23);
            ThrowHelper.CheckArgumentRange(nameof(minute), minute, 0, 59);
            ThrowHelper.CheckArgumentRange(nameof(second), second, 0, 59);

            int secs = hour * 3600 + minute * 60 + second;
            return new GameClockTime(secs);
        }

        /// <summary>
        /// Makes a new <see cref="GameClockTime"/> from the number of seconds since midnight.
        /// </summary>
        /// <param name="seconds">The number of seconds since midnight.</param>
        /// <returns>
        /// The resulting time.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="seconds"/> is not between 0 and 86399.
        /// </exception>
        public static GameClockTime FromSecondsFromMidnight(int seconds)
        {
            ThrowHelper.CheckArgumentRange(nameof(seconds), seconds, 0, 86399);

            return new GameClockTime(seconds);
        }

        /// <summary>
        /// Makes a new <see cref="GameClockTime"/> with the hour number changed.
        /// </summary>
        /// <param name="hour">The new hour.</param>
        /// <returns>
        /// An object whose value is the time represented by this instance but the hour is the specified
        /// <paramref name="hour"/>.
        /// </returns>
        public GameClockTime WithHour(int hour)
        {
            ThrowHelper.CheckArgumentRange(nameof(hour), hour, 0, 23);

            int secs = hour * 3600 + _secs % 3600;
            return new GameClockTime(secs);
        }

        /// <summary>
        /// Makes a new <see cref="GameClockTime"/> with the minute number changed.
        /// </summary>
        /// <param name="minute">The new minute.</param>
        /// <returns>
        /// An object whose value is the time represented by this instance but the minute is the specified
        /// <paramref name="minute"/>.
        /// </returns>
        public GameClockTime WithMinute(int minute)
        {
            ThrowHelper.CheckArgumentRange(nameof(minute), minute, 0, 59);

            int secs = _secs / 3600 * 3600 + minute * 60 + _secs % 60;
            return new GameClockTime(secs);
        }

        /// <summary>
        /// Makes a new <see cref="GameClockTime"/> with the second number changed.
        /// </summary>
        /// <param name="second">The new second.</param>
        /// <returns>
        /// An object whose value is the time represented by this instance but the second is the specified
        /// <paramref name="second"/>.
        /// </returns>
        public GameClockTime WithSecond(int second)
        {
            ThrowHelper.CheckArgumentRange(nameof(second), second, 0, 59);

            int secs = _secs / 60 * 60 + second;
            return new GameClockTime(secs);
        }

        /// <summary>
        /// Adds the specified <see cref="GameClockDuration"/> to the current time, and also returns the integral
        /// number of wrapped days.
        /// </summary>
        /// <param name="duration">The duration to add.</param>
        /// <param name="wrappedDays">
        /// When this method returns, contains the wrapped days.
        /// </param>
        /// <returns>The resulting date.</returns>
        public GameClockTime OverflowingAddSigned(GameClockDuration duration, out long wrappedDays)
        {
            long rhsDurationSecs = duration.WholeSeconds;

            wrappedDays = rhsDurationSecs / 86_400;
            long newSecs = _secs + rhsDurationSecs % 86_400;
            if (newSecs < 0)
            {
                wrappedDays--;
                newSecs += 86_400;
            }
            else
            {
                if (newSecs >= 86_400)
                {
                    wrappedDays++;
                    newSecs -= 86_400;
                }
            }

            return new GameClockTime((int)newSecs);
        }

        /// <summary>
        /// Subtracts the specified <see cref="GameClockDuration"/> from the current time, and also returns
        /// the integral number of wrapped days.
        /// </summary>
        /// <param name="duration">The duration to add.</param>
        /// <param name="wrappedDays">
        /// When this method returns, contains the wrapped days.
        /// </param>
        /// <returns>The resulting date.</returns>
        public GameClockTime OverflowingSubtractSigned(GameClockDuration duration, out long wrappedDays)
        {
            GameClockTime time = OverflowingAddSigned(-duration, out wrappedDays);
            wrappedDays = -wrappedDays;

            return time;
        }

        /// <summary>
        /// Subtracts another <see cref="GameClockTime"/> from the current time and returns a
        /// <see cref="GameClockDuration"/> within +/- 1 day.
        /// </summary>
        /// <param name="value">The time to subtract.</param>
        /// <returns>
        /// A signed duration that is equal to the time represented by this instance minus the time represented by
        /// <paramref name="value"/>.
        /// </returns>
        public GameClockDuration SignedDurationSince(GameClockTime value) => new(_secs - value._secs);

        /// <summary>
        /// Compares the value of this instance to a specified object that contains a specified
        /// <see cref="GameClockTime"/> value, and returns an integer that indicates whether this instance is earlier
        /// than, the same as, or later than the specified <see cref="GameClockTime"/> value.
        /// </summary>
        /// <param name="value">A boxed object to compare, or <see langword="null"/>.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and the value parameter. Less than zero if
        /// this instance is earlier than value. Zero if this instance is the same as value. Greater than zero if this
        /// instance is later than value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is not a <see cref="GameClockTime"/>.
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (value is not GameClockTime otherTime)
                throw new ArgumentException();

            long t = (otherTime)._secs;
            if (_secs > t) return 1;
            if (_secs < t) return -1;
            return 0;
        }
        /// <summary>
        /// Compares the value of this instance to a specified <see cref="GameClockTime"/> value and indicates whether
        /// this instance is earlier than, the same as, or later than the specified <see cref="GameClockTime"/> value.
        /// </summary>
        /// <param name="value">The object to compare to the current instance.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and the value parameter.
        /// <list type="bullet">
        /// <item>
        /// <description>Less than zero if this instance is earlier than <paramref name="value"/>.</description>
        /// </item>
        /// <item>
        /// <description>Zero if this instance is the same as <paramref name="value"/>.</description>
        /// </item>
        /// <item>
        /// <description>Greater than zero if this instance is later than <paramref name="value"/>.</description>
        /// </item>
        /// </list>
        /// </returns>
        public int CompareTo(GameClockTime value)
        {
            long t = value._secs;
            if (_secs > t) return 1;
            if (_secs < t) return -1;
            return 0;
        }

        public override string ToString() => ToStringInternal();

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe string ToStringInternal()
        {
            unsafe
            {
                // this is the minimum number that is large enough to contain any time string
                const int bufferLen = 8;
                char* buffer = stackalloc char[bufferLen];
                GameClockDateTimeFormat.TryFormatTimeS(this, buffer, bufferLen, out int written);
                return new string(buffer, 0, written);
            }
        }

        /// <summary>
        /// Adds a specified duration to a specified time, yielding a new time.
        /// </summary>
        /// <param name="time">The time value to add.</param>
        /// <param name="duration">The duration value to add.</param>
        /// <returns>
        /// An object that is the sum of the values of <paramref name="time"/> and <paramref name="duration"/>.
        /// </returns>
        /// <remarks>
        /// The addition wraps around and ignores integral number of days.
        /// </remarks>
        public static GameClockTime operator +(GameClockTime time, GameClockDuration duration)
            => new GameClockTime((time._secs + 86400 + (int)(duration.WholeSeconds % 86400)) % 86400);

        /// <summary>
        /// Subtracts a specified duration from a specified time and returns a new time.
        /// </summary>
        /// <param name="time">The time value to subtract from.</param>
        /// <param name="duration">The duration value to subtract.</param>
        /// <returns>
        /// An object whose value is the value of <paramref name="time"/> minus the value of
        /// <paramref name="duration"/>.
        /// </returns>
        /// <remarks>
        /// The subtraction wraps around and ignores integral number of days.
        /// </remarks>
        public static GameClockTime operator -(GameClockTime time, GameClockDuration duration)
            => new GameClockTime((time._secs + 86400 - (int)(duration.WholeSeconds % 86400)) % 86400);

        /// <summary>
        /// Subtracts a specified from the current time, yielding a signed duration.
        /// This does not overflow or underflow at all, as all possible output fits in the range of
        /// <see cref="GameClockDuration"/>.
        /// </summary>
        /// <param name="t1">The time value to subtract from (the minuend).</param>
        /// <param name="t2">The time value to subtract (the subtrahend).</param>
        /// <returns>
        /// The signed duration between <paramref name="t1"/> and <paramref name="t2"/>; that is,
        /// <paramref name="t1"/> minus <paramref name="t2"/>.
        /// </returns>
        /// <remarks>
        /// The implementation is a wrapper around <see cref="SignedDurationSince(GameClockTime)"/>.
        /// </remarks>
        public static GameClockDuration operator -(GameClockTime t1, GameClockTime t2) => t1.SignedDurationSince(t2);

        /// <summary>
        /// Deconstructs a time into hour, minute, and second components.
        /// </summary>
        /// <param name="hour">The hour component.</param>
        /// <param name="minute">The minute component.</param>
        /// <param name="second">The second component.</param>
        public void Deconstruct(out int hour, out int minute, out int second)
        {
            hour = Hour;
            minute = Minute;
            second = Second;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="GameClockTime"/>
        /// object.
        /// </summary>
        /// <param name="value">An object to compare with this instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> represents the same game clock time as this instance;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(GameClockTime value)
        {
            return _secs == value._secs;
        }
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="value">An object to compare with this instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is a <see cref="GameClockTime"/> object that represents
        /// the same game clock time as the current <see cref="GameClockTime"/> structure; otherwise, false.
        /// </returns>
        public override bool Equals(object value)
        {
            if (value is GameClockTime duration)
            {
                return Equals(duration);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether two <see cref="GameClockTime"/> instances are equal.
        /// </summary>
        /// <param name="left">The game clock time to compare.</param>
        /// <param name="right">The second game clock time to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(GameClockTime left, GameClockTime right)
        {
            return left.Equals(right);
        }
        /// <summary>
        /// Indicates whether two <see cref="GameClockTime"/> instances are not equal.
        /// </summary>
        /// <param name="left">The game clock time to compare.</param>
        /// <param name="right">The second game clock time to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(GameClockTime left, GameClockTime right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Indicates whether a specified <see cref="GameClockTime"/> is earlier than another specified
        /// <see cref="GameClockTime"/>.
        /// </summary>
        /// <param name="left">The first game clock time to compare.</param>
        /// <param name="right">The second game clock time to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="left"/> is earlier than the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <(GameClockTime left, GameClockTime right) => left._secs < right._secs;
        /// <summary>
        /// Indicates whether a specified <see cref="GameClockTime"/> is earlier than or equal to another specified
        /// <see cref="GameClockTime"/>.
        /// </summary>
        /// <param name="left">The first game clock time to compare.</param>
        /// <param name="right">The second game clock time to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="left"/> is earlier than or equal to the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <=(GameClockTime left, GameClockTime right) => left._secs <= right._secs;

        /// <summary>
        /// Indicates whether a specified <see cref="GameClockTime"/> is later than another specified
        /// <see cref="GameClockTime"/>.
        /// </summary>
        /// <param name="left">The first game clock time to compare.</param>
        /// <param name="right">The second game clock time to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="left"/> is later than the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >(GameClockTime left, GameClockTime right) => left._secs > right._secs;
        /// <summary>
        /// Indicates whether a specified <see cref="GameClockTime"/> is later than or equal to another specified
        /// <see cref="GameClockTime"/>.
        /// </summary>
        /// <param name="left">The first game clock time to compare.</param>
        /// <param name="right">The second game clock time to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="left"/> is later than or equal to the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >=(GameClockTime left, GameClockTime right) => left._secs >= right._secs;

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return _secs.GetHashCode();
        }
    }
}
