//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA.Chrono
{
	public readonly struct GameClockDateTime : IEquatable<GameClockDateTime>, IComparable<GameClockDateTime>, IComparable, Timelike<GameClockDateTime>
	{
		public GameClockDateTime(GameClockDate date, GameClockTime time) : this()
		{
			_date = date;
			_time = time;
		}

		internal readonly GameClockDate _date;
		internal readonly GameClockTime _time;

		public static readonly GameClockDateTime MaxValue = new(GameClockDate.MaxValue, GameClockTime.MaxValue);
		public static readonly GameClockDateTime MinValue = new(GameClockDate.MinValue, GameClockTime.MinValue);

		public readonly GameClockDate Date => _date;
		public readonly GameClockTime Time => _time;

		public readonly int Hour => _time.Hour;

		public (bool isPM, int hour) Hour12 => _time.Hour12;

		public bool GetHour12(out int hour) => _time.GetHour12(out hour);

		public readonly int Minute => _time.Minute;

		public readonly int Second => _time.Second;

		public int SecondsFromMidnight => _time.SecondsFromMidnight;

		public readonly GameClockDateTime WithHour(int hour) => new(Date, _time.WithHour(hour));

		public readonly GameClockDateTime WithMinute(int minute) => new(Date, _time.WithMinute(minute));

		public readonly GameClockDateTime WithSecond(int second) => new(Date, _time.WithSecond(second));

		public static GameClockDateTime operator +(GameClockDateTime dateTime, GameClockDuration duration)
		{
			if (!dateTime.TryAdd(duration, out GameClockDateTime result))
			{
				throw new ArgumentOutOfRangeException(nameof(duration));
			}

			return result;
		}

		public GameClockDateTime Add(GameClockDuration duration) => this + duration;

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

		public GameClockDateTime AddMonths(int months)
			=> AddMonths((long)months);
		public GameClockDateTime AddMonths(long months)
			=> new(_date.AddMonths(months), _time);

		public bool TryAddMonths(int months, out GameClockDateTime dateTime)
			=> TryAddMonths((long)months, out dateTime);
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

		public static GameClockDateTime operator -(GameClockDateTime dateTime, GameClockDuration duration)
		{
			if (!dateTime.TrySubtract(duration, out GameClockDateTime result))
			{
				throw new ArgumentOutOfRangeException(nameof(duration));
			}

			return result;
		}

		public GameClockDateTime Subtract(GameClockDuration duration) => this - duration;

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

		public GameClockDateTime SubtractMonths(int months)
			=> SubtractMonths((long)months);
		public GameClockDateTime SubtractMonths(long months)
		{
			if (!TrySubtractMonths(months, out GameClockDateTime dateTime))
			{
				throw new ArgumentOutOfRangeException(nameof(months));
			}

			return dateTime;
		}

		public bool TrySubtractMonths(int months, out GameClockDateTime dateTime)
			=> TrySubtractMonths((long)months, out dateTime);
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

		public readonly void Deconstruct(out GameClockDate date, out GameClockTime time)
		{
			date = _date;
			time = _time;
		}

		public bool Equals(GameClockDateTime other)
		{
			return _date == other._date && _time == other._time;
		}
		public override bool Equals(object obj)
		{
			if (obj is GameClockDateTime duration)
			{
				return Equals(duration);
			}

			return false;
		}

		public static bool operator ==(GameClockDateTime left, GameClockDateTime right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(GameClockDateTime left, GameClockDateTime right)
		{
			return !left.Equals(right);
		}

		public int CompareTo(object value)
		{
			if (value == null) return 1;
			if (!(value is GameClockDateTime otherDateTime))
				throw new ArgumentException();
	
			return CompareTo(otherDateTime);
		}

		public int CompareTo(GameClockDateTime value)
		{
			int dateCompRes = _date.CompareTo(value.Date);
			if (dateCompRes != 0)
			{
				return dateCompRes;
			}

			return _time.CompareTo(value.Time);
		}

		public static bool operator <(GameClockDateTime dt1, GameClockDateTime dt2) => dt1.CompareTo(dt2) < 0;
		public static bool operator <=(GameClockDateTime dt1, GameClockDateTime dt2) => dt1.CompareTo(dt2) <= 0;

		public static bool operator >(GameClockDateTime dt1, GameClockDateTime dt2) => dt1.CompareTo(dt2) > 0;
		public static bool operator >=(GameClockDateTime dt1, GameClockDateTime dt2) => dt1.CompareTo(dt2) >= 0;

		public override int GetHashCode()
		{
			return _date.GetHashCode() + 17 * _time.GetHashCode();
		}
	}
}
