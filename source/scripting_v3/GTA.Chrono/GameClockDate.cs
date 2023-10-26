//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA.Chrono
{
	/// <summary>
	///
	/// </summary>
	public readonly struct GameClockDate : IEquatable<GameClockDate>, IComparable<GameClockDate>, IComparable,
		Datelike<GameClockDate>
	{
		internal GameClockDate(int year, OrdFlags ordFlags) : this()
		{
			_year = year;
			_ordFlags = ordFlags;
		}

		internal readonly int _year;
		internal readonly OrdFlags _ordFlags;

		public static readonly GameClockDate MaxValue = new(int.MaxValue, OrdFlagsForMaxDate);
		public static readonly GameClockDate MinValue = new(int.MinValue, OrdFlagsForMinDate);
		private static readonly OrdFlags OrdFlagsForMaxDate = new(365, YearFlags.FromYear(int.MaxValue));
		private static readonly OrdFlags OrdFlagsForMinDate = new(1, YearFlags.FromYear(int.MinValue));

		public readonly int Year => _year;

		public readonly int DayOfYear => (int)_ordFlags.Ordinal;

		public readonly int ZeroBasedDayOfYear => DayOfYear - 1;

		public readonly DayOfWeek DayOfWeek
		{
			get
			{
				return (DayOfWeek)(((int)IsoDayOfWeek + 1) % 7);
			}
		}

		public readonly IsoDayOfWeek IsoDayOfWeek => _ordFlags.IsoDayOfWeek;

		public readonly int Month => MonthDayFlags.Month;

		public readonly int ZeroBasedMonth => MonthDayFlags.Month - 1;

		public readonly int Day => MonthDayFlags.Day;

		public readonly int ZeroBasedDay => MonthDayFlags.Day - 1;

		internal readonly MonthDayFlags MonthDayFlags => _ordFlags.ToMonthDayFlags();

		static GameClockDate? FromOrdinalAndFlags(int year, int ordinal, YearFlags flags)
		{
			return OrdFlags.New(ordinal, flags) switch
			{
				OrdFlags of => new GameClockDate(year, of),
				_ => null
			};
		}

		static GameClockDate? FromMdf(int year, MonthDayFlags mdf)
		{
			OrdFlags? of = mdf.ToOrdFlags();

			if (of is OrdFlags ofNonNull)
			{
				return new GameClockDate(year, ofNonNull);
			}

			return null;
		}

		static GameClockDate FromMdfUnchecked(int year, MonthDayFlags mdf)
			=> new GameClockDate(year, mdf.ToOrdFlags().GetValueOrDefault());

		public GameClockDateTime AndTime(GameClockTime time)
			=> new(this, time);

		public GameClockDateTime AndHms(int hour, int minute, int second)
		{
			GameClockTime time = GameClockTime.FromHms(hour, minute, second);
			return new GameClockDateTime(this, time);
		}

		public static GameClockDate FromDateTime(DateTime dateTime)
		{
			return FromYmd(dateTime.Year, dateTime.Month, dateTime.Day);
		}

		public static GameClockDate FromYmd(int year, int month, int day)
		{
			YearFlags flags = YearFlags.FromYear(year);

			return (MonthDayFlags.New(month, day, flags)) switch
			{
				MonthDayFlags mdf => FromMdfUnchecked(year, mdf),
				_ => throw new ArgumentOutOfRangeException(nameof(day))
			};
		}

		public static GameClockDate FromOrdinalDate(int year, int ordinal)
		{
			YearFlags flags = YearFlags.FromYear(year);

			return FromOrdinalAndFlags(year, ordinal, flags) switch
			{
				GameClockDate date => date,
				_ => throw new ArgumentOutOfRangeException(nameof(ordinal))
			};
		}

		public static GameClockDate FromIsoWeekDate(int year, int week, IsoDayOfWeek dayOfWeek)
		{
			YearFlags flags = YearFlags.FromYear(year);
			uint nWeeks = flags.IsoWeekCount;

			if (week < 1 || week > nWeeks)
			{
				throw new ArgumentOutOfRangeException(nameof(week));
			}

			int weekOrd = week * 7 + (int)dayOfWeek;
			int delta = flags.CalcIsoWeekDelta();
			if (weekOrd <= delta)
			{
				// ordinal < 1, previous year
				YearFlags prevFlags = YearFlags.FromYear(year - 1);
				var prevOrdFlags = new OrdFlags(weekOrd + prevFlags.DayCount - delta, prevFlags);
				return new GameClockDate(year - 1, prevOrdFlags);
			}

			if (weekOrd <= delta)
			{
				// ordinal < 1, previous year
				YearFlags prevFlags = YearFlags.FromYear(year - 1);
				var prevOrdFlags = new OrdFlags(weekOrd + prevFlags.DayCount - delta, prevFlags);
				return new GameClockDate(year - 1, prevOrdFlags);
			}

			int ordinal = weekOrd - delta;
			int nDays = flags.DayCount;
			if (ordinal <= nDays)
			{
				// this year
				return new GameClockDate(year, new OrdFlags(ordinal, flags));
			}

			// ordinal > nDays, next year
			YearFlags nextFlags = YearFlags.FromYear(year + 1);
			var nextOrdFlags = new OrdFlags(ordinal - nDays, nextFlags);
			return new GameClockDate(year + 1, nextOrdFlags);
		}

		public bool IsLeapYear => _ordFlags.IsLeapYear;

		public static GameClockDate operator +(GameClockDate date, GameClockDuration duration)
		{
			if (!date.TryAdd(duration, out GameClockDate result))
			{
				throw new ArgumentOutOfRangeException(nameof(duration));
			}

			return result;
		}

		public GameClockDate Add(GameClockDuration duration) => this + duration;

		public bool TryAdd(GameClockDuration duration, out GameClockDate date)
		{
			long wholeDays = (long)duration.TotalDays;
			long newOrdinal = DayOfYear + wholeDays;

			if (newOrdinal is > 0 and <= 365)
			{
				OrdFlags newOrdFlags = _ordFlags.WithOrdinalUnchecked((uint)newOrdinal);
				date = new GameClockDate(Year, newOrdFlags);
				return true;
			}

			DivModFloor(Year, 400, out int yearDiv400, out int yearMod400);
			long dayCycleUnclamped = Internals.YearOrdinalToDayCycle(yearMod400, DayOfYear) + wholeDays;
			DivModFloor(dayCycleUnclamped, 146_097L, out long cycleDiv400Y, out long dayCycleClamped);
			cycleDiv400Y += yearDiv400;

			Internals.DayCycleToYearOrdinal((int)dayCycleClamped, out yearMod400, out int ordinal);
			long newYear = cycleDiv400Y * 400 + yearMod400;
			if (newYear < int.MinValue || newYear > int.MaxValue)
			{
				date = default;
				return false;
			}

			var of = new OrdFlags(ordinal, YearFlags.FromYearMod400(yearMod400));
			date = new GameClockDate((int)newYear, of);
			return true;
		}

		public GameClockDate AddMonths(int months)
			=> AddMonths((long)months);
		public GameClockDate AddMonths(long months)
		{
			if (!TryAddMonths(months, out GameClockDate date))
			{
				throw new ArgumentOutOfRangeException(nameof(months));
			}

			return date;
		}

		public bool TryAddMonths(int months, out GameClockDate date)
			=> TryAddMonths((long)months, out date);
		public bool TryAddMonths(long months, out GameClockDate date)
		{
			if (months == 0)
			{
				date = this;
				return true;
			}

			GameClockDate? nullableDate = DiffMonths(months);
			if (nullableDate.HasValue)
			{
				date = nullableDate.GetValueOrDefault();
				return true;
			}
			else
			{
				date = default;
				return false;
			}
		}

		private GameClockDate? DiffMonths(long months)
		{
			long years = months / 12;
			int left = (int)(months % 12);

			// Determine new year (without taking months into account for now). Return null if the new year is not in
			// the range of int32.
			if ((years > 0 && years > (int.MaxValue - Year)) || (years < 0 && years < (int.MinValue - Year)))
			{
				return null;
			}

			int year = (int)(Year + years);
			int month = Month + left;

			if (month <= 0)
			{
				if (year == int.MinValue)
				{
					return null;
				}

				year -= 1;
				month += 12;
			}
			else if (month > 12)
			{
				if (year == int.MaxValue)
				{
					return null;
				}

				year += 1;
				month -= 12;
			}

			// Clamp original day in case new month is shorter
			YearFlags flags = YearFlags.FromYear(year);
			int dayMax = month switch
			{
				2 => (flags.DayCount == 366 ? 29 : 28),
				4 or 6 or 9 or 11 => 30,
				_ => 31,
			};
			int day = Day;
			if (day > dayMax)
			{
				day = dayMax;
			};

			return FromMdfUnchecked(year, new MonthDayFlags(month, day, flags));
		}

		public bool TrySubtract(GameClockDuration duration, out GameClockDate date)
		{
			bool result = TryAdd(-duration, out date);
			return result;
		}

		public GameClockDate SubtractMonths(int months)
			=> SubtractMonths((long)months);
		public GameClockDate SubtractMonths(long months)
		{
			if (!TrySubtractMonths(months, out GameClockDate date))
			{
				throw new ArgumentOutOfRangeException(nameof(months));
			}

			return date;
		}

		public bool TrySubtractMonths(int months, out GameClockDate date)
			=> TrySubtractMonths((long)months, out date);
		public bool TrySubtractMonths(long months, out GameClockDate date)
		{
			// Don't care about long.MinValue as the absolute value is larger than the number of months between
			// GameClockDate.MinValue and GameClockDate.MaxValue. Will return false anyway in such case.
			return TryAddMonths(-months, out date);
		}

		public static GameClockDate operator -(GameClockDate date, GameClockDuration duration)
		{
			if (!date.TrySubtract(duration, out GameClockDate result))
			{
				throw new ArgumentOutOfRangeException(nameof(duration));
			}

			return result;
		}

		public GameClockDate Subtract(GameClockDuration duration) => this - duration;

		public GameClockDate WithYear(int year)
		{
			// we need to operate with `mdf` since we should keep the month and day number as is
			MonthDayFlags mdf = MonthDayFlags;

			YearFlags flags = YearFlags.FromYear(year);
			mdf = mdf.WithFlags(flags);

			GameClockDate? newDate = FromMdf(year, mdf);
			if (newDate == null)
			{
				throw new ArgumentException("cannot create GameClockDate that represents February 29 in a non-leap year.");
			}

			return newDate.GetValueOrDefault();
		}

		public GameClockDate WithMonth(int month)
		{
			MonthDayFlags? newMdf = MonthDayFlags.WithMonth(month);
			if (newMdf == null)
			{
				throw new ArgumentOutOfRangeException();
			}

			return WithMonthDayFlags(newMdf.GetValueOrDefault());
		}

		public GameClockDate WithZeroBasedMonth(int month)
			=> WithMonth(month + 1);

		public GameClockDate WithDay(int day)
		{
			MonthDayFlags? newMdf = MonthDayFlags.WithDay(day);
			if (newMdf == null)
			{
				throw new ArgumentOutOfRangeException();
			}

			return WithMonthDayFlags(newMdf.GetValueOrDefault());
		}

		public GameClockDate WithZeroBasedDay(int day)
			=> WithDay(day + 1);

		public GameClockDate WithDayOfYear(int dayOfYear)
		{
			OrdFlags? of = _ordFlags.WithOrdinal(dayOfYear);
			if (of == null)
			{
				throw new ArgumentOutOfRangeException(nameof(dayOfYear));
			}

			return WithOrdFlags(of.GetValueOrDefault());
		}

		public GameClockDate WithZeroBasedDayOfYear(int dayOfYear)
			=> WithDayOfYear(dayOfYear + 1);

		private GameClockDate WithOrdFlags(OrdFlags of) => new(Year, of);

		private GameClockDate WithMonthDayFlags(MonthDayFlags mdf)
			=> WithOrdFlags(mdf.ToOrdFlags().GetValueOrDefault());

		public GameClockDuration SignedDurationSince(GameClockDate rhs)
		{
			int year1 = Year;
			int year2 = rhs.Year;
			DivModFloor(year1, 400, out int year1Div400, out int year1Mod400);
			DivModFloor(year2, 400, out int year2Div400, out int year2Mod400);

			long cycle1 = (long)Internals.YearOrdinalToDayCycle(year1Mod400, (int)_ordFlags.Ordinal);
			long cycle2 = (long)Internals.YearOrdinalToDayCycle(year2Mod400, (int)rhs._ordFlags.Ordinal);
			return GameClockDuration.FromDays((year1Div400 - year2Div400) * 146_097 + (cycle1 - cycle2));
		}

		public int YearsSince(GameClockDate other)
		{
			int years = Year - other.Year;

			if ((Month << 5 | Day) < (other.Month << 5 | other.Day))
			{
				years -= 1;
			}

			return (years >= 0) switch
			{
				true => years,
				false => throw new ArgumentException(nameof(other)),
			};
		}

		public void Deconstruct(out int year, out int month, out int day)
		{
			year = Year;
			month = Month;
			day = Day;
		}

		public bool Equals(GameClockDate other)
		{
			return _year == other._year && _ordFlags == other._ordFlags;
		}
		public override bool Equals(object obj)
		{
			if (obj is GameClockDate duration)
			{
				return Equals(duration);
			}

			return false;
		}

		public static bool operator ==(GameClockDate left, GameClockDate right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(GameClockDate left, GameClockDate right)
		{
			return !left.Equals(right);
		}

		public int CompareTo(object value)
		{
			if (value == null) return 1;
			if (value is not GameClockDate otherDuration)
				throw new ArgumentException();

			return CompareTo(otherDuration);
		}

		public int CompareTo(GameClockDate value)
		{
			int thisYear = _year;
			int otherYear = value._year;
			if (thisYear > otherYear)
			{
				return 1;
			}
			else if (thisYear < otherYear)
			{
				return -1;
			}

			uint thisOrdinal = _ordFlags.Ordinal;
			uint otherOrdinal = value._ordFlags.Ordinal;
			if (thisOrdinal > otherOrdinal)
			{
				return 1;
			}
			else if (thisOrdinal < otherOrdinal)
			{
				return -1;
			}

			return 0;
		}

		public static bool operator <(GameClockDate d1, GameClockDate d2) => d1.CompareTo(d2) < 0;
		public static bool operator <=(GameClockDate d1, GameClockDate d2) => d1.CompareTo(d2) <= 0;

		public static bool operator >(GameClockDate d1, GameClockDate d2) => d1.CompareTo(d2) > 0;
		public static bool operator >=(GameClockDate d1, GameClockDate d2) => d1.CompareTo(d2) >= 0;

		public override int GetHashCode()
		{
			return _year.GetHashCode() + 17 * _ordFlags.GetHashCode();
		}

		private static void DivModFloor(int val, int div, out int quotient, out int modulo)
		{
			quotient = val.DivEuclid(div);
			modulo = val.RemEuclid(div);
		}

		private static void DivModFloor(long val, long div, out long quotient, out long modulo)
		{
			quotient = val.DivEuclid(div);
			modulo = val.RemEuclid(div);
		}
	}
}
