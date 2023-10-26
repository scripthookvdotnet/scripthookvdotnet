//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA.Chrono
{
	public readonly struct GameClockTime : IEquatable<GameClockTime>, IComparable<GameClockTime>, IComparable, Timelike<GameClockTime>
	{
		private readonly int _secs;

		internal GameClockTime(int secs) : this()
		{
			_secs = secs;
		}

		public static readonly GameClockTime MaxValue = new(86400 - 1);
		public static readonly GameClockTime MinValue = new(0);

		public int Hour => _secs / 3600;

		public (bool isPM, int hour) Hour12
		{
			get
			{
				bool isPM = GetHour12(out int hour);
				return (isPM, hour);
			}
		}

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

		public int Minute => ((_secs / 60) % 60);

		public int Second => (_secs % 60);

		public int SecondsFromMidnight => Hour * 3600 + Minute * 60 + Second;

		public static GameClockTime FromHms(int hour, int minute, int second)
		{
			ThrowHelper.CheckArgumentRange(nameof(hour), hour, 0, 23);
			ThrowHelper.CheckArgumentRange(nameof(minute), minute, 0, 59);
			ThrowHelper.CheckArgumentRange(nameof(second), second, 0, 59);

			int secs = hour * 3600 + minute * 60 + second;
			return new GameClockTime(secs);
		}

		public static GameClockTime FromSecondsFromMidnight(int seconds)
		{
			ThrowHelper.CheckArgumentRange(nameof(seconds), seconds, 0, 86399);

			return new GameClockTime(seconds);
		}

		public GameClockTime WithHour(int hour)
		{
			ThrowHelper.CheckArgumentRange(nameof(hour), hour, 0, 23);

			int secs = hour * 3600 + _secs % 3600;
			return new GameClockTime(secs);
		}

		public GameClockTime WithMinute(int minute)
		{
			ThrowHelper.CheckArgumentRange(nameof(minute), minute, 0, 59);

			int secs = _secs / 3600 * 3600 + minute * 60 + _secs % 60;
			return new GameClockTime(secs);
		}

		public GameClockTime WithSecond(int second)
		{
			ThrowHelper.CheckArgumentRange(nameof(second), second, 0, 59);

			int secs = _secs / 60 * 60 + second;
			return new GameClockTime(secs);
		}

		public GameClockTime OverflowingAddSigned(GameClockDuration duration, out long wrappedDays)
		{
			long rhsDurationSecs = duration.WholeSeconds;

			wrappedDays = (int)(rhsDurationSecs / 86_400);
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

		public GameClockTime OverflowingSubtractSigned(GameClockDuration duration, out long wrappedDays)
		{
			GameClockTime time = OverflowingAddSigned(-duration, out wrappedDays);
			wrappedDays = -wrappedDays;

			return time;
		}

		public GameClockDuration SignedDurationSince(GameClockTime rhs) => new(_secs - rhs._secs);

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
		public int CompareTo(GameClockTime value)
		{
			long t = value._secs;
			if (_secs > t) return 1;
			if (_secs < t) return -1;
			return 0;
		}

		public static GameClockTime operator +(GameClockTime time, GameClockDuration duration)
			=> new GameClockTime((time._secs + 86400 + (int)(duration.WholeSeconds % 86400)) % 86400);

		public static GameClockTime operator -(GameClockTime time, GameClockDuration duration)
			=> new GameClockTime((time._secs + 86400 - (int)(duration.WholeSeconds % 86400)) % 86400);

		public void Deconstruct(out int hour, out int minute, out int second)
		{
			hour = Hour;
			minute = Minute;
			second = Second;
		}

		public bool Equals(GameClockTime other)
		{
			return _secs == other._secs;
		}
		public override bool Equals(object obj)
		{
			if (obj is GameClockTime duration)
			{
				return Equals(duration);
			}

			return false;
		}

		public static bool operator ==(GameClockTime left, GameClockTime right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(GameClockTime left, GameClockTime right)
		{
			return !left.Equals(right);
		}

		public static bool operator <(GameClockTime t1, GameClockTime t2) => t1._secs < t2._secs;
		public static bool operator <=(GameClockTime t1, GameClockTime t2) => t1._secs <=t2._secs;

		public static bool operator >(GameClockTime t1, GameClockTime t2) => t1._secs > t2._secs;
		public static bool operator >=(GameClockTime t1, GameClockTime t2) => t1._secs >= t2._secs;

		public override int GetHashCode()
		{
			return _secs.GetHashCode();
		}
	}
}
