//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA.Chrono
{
	/// <summary>
	/// GameClockHour, GameClockMinute, GameClockSecond, GameClockDay, GameClockMonth, GameClockYear
	/// </summary>
	public readonly struct GameClockDuration : IEquatable<GameClockDuration>, IComparable<GameClockDuration>, IComparable
	{
		internal GameClockDuration(long seconds) : this()
		{
			_secs = seconds;
		}

		/// The number of seconds in a minute.
		const long SecsPerMinute = 60;
		/// The number of seconds in an hour.
		const long SecsPerHour = 3600;
		/// The number of (non-leap) seconds in days.
		const long SecsPerDay = 86_400;
		/// The number of (non-leap) seconds in a week.
		const long SecsPerWeek = 604_800;

		const long LeapYearCountOfInt32 = 1041529570;
		const long NonLeapYearCountOfInt32 = 3253437725;

		/// <summary>
		/// The maximum integral value within which the square value can exactly represent and correctly compare it
		/// with other values as a <see langword="double"/>. Roughly equivalent to 156.92 weeks.
		/// </summary>
		/// <remarks>
		/// The squared value of this value is
		/// <c>9_007_199_136_250_225</c> which is less than the safe maximum integer in <see langword="double"/>
		/// <c>9_007_199_254_740_991</c> (the same value as <c>Number.MAX_SAFE_INTEGER</c> in JavaScript).
		/// </remarks>
		const int MaxSafeSqrtIntegerOutOfDouble = 94906266;
		/// <summary>
		/// The maximum integral value within which the square value can exactly represent and correctly compare it
		/// with other values as a <see langword="double"/>. Roughly equivalent to -156.92 weeks.
		/// </summary>
		/// <remarks>
		/// The squared value of this value is
		/// <c>-9_007_199_136_250_225</c> which is more than the safe minimum integer in <see langword="double"/>
		/// <c>-9_007_199_254_740_991</c> (the same value as <c>Number.MIN_SAFE_INTEGER</c> in JavaScript).
		/// </remarks>
		const int MinSafeSqrtIntegerOutOfDouble = -94906265;

		/// <summary>
		/// The inverse of <see cref="MinSafeSqrtIntegerOutOfDouble"/> rounded to the nearest double value.
		/// </summary>
		const double InverseOfMaxSafeSqrtIntegerOutOfDouble = 1.0536712127723509e-08;

		/// <summary>
		/// The number of days elapsed in the 0x100000000 (4294967296) year since January 1st, the 0 year
		/// (where year is in the range of <see langword="int"/>).
		/// </summary>
		const long DayCountUInt32YearsLaterSinceInt32MinValueYear = (LeapYearCountOfInt32 * 366)
			+ (NonLeapYearCountOfInt32 * 365);
		const long MaxSecDifference = (DayCountUInt32YearsLaterSinceInt32MinValueYear) * SecsPerDay
			+ 23 * SecsPerHour + 59 * SecsPerMinute + 59;
		const long MinSecDifference = -((DayCountUInt32YearsLaterSinceInt32MinValueYear) * SecsPerDay
			+ 23 * SecsPerHour + 59 * SecsPerMinute + 59);

		/// <summary>
		/// About 17540.8 years
		/// </summary>
		const long SecToOptimizeHigherWeighted = 0x8000000000;

		const long SecToOptimizeLowerWeighted = 0x4000;

		const double InverseOfSecToOptimizeHigherWeighted = 1.8189894035458565e-12;
		const double InverseOfSecToOptimizeLowerWeighted = 6.103515625e-05;

		/// <summary>
		/// Same as 135_536_076_770_054_400 seconds. Can exactly represent as a double floating-point value.
		/// </summary>
		const long MinPositiveSecDifferenceOutOfBound = (DayCountUInt32YearsLaterSinceInt32MinValueYear) * SecsPerDay;
		const long MaxNegativeSecDifferenceOutOfBound = -MinPositiveSecDifferenceOutOfBound;

		public static readonly GameClockDuration Zero = new(0);
		public static readonly GameClockDuration MaxValue = new(MaxSecDifference);
		public static readonly GameClockDuration MinValue = new(MinSecDifference);

		internal readonly long _secs;

		public long Days => _secs / SecsPerDay;

		public int Hours => (int)((_secs / SecsPerHour) % 24);

		public int Minutes => (int)((_secs / SecsPerMinute) % 60);

		public int Seconds => (int)(_secs % 60);

		public double TotalWeeks => (double)_secs / SecsPerWeek;

		public double TotalDays => (double)_secs / SecsPerDay;

		public double TotalHours => (double)_secs / SecsPerHour;

		public double TotalMinutes => (double)_secs / SecsPerMinute;

		public double TotalSeconds => (double)_secs;

		public long ToInt64Seconds() => _secs;

		private static void CheckArgumentRange(string paramName, long value, long minInclusive, long maxInclusive)
		{
			if (value < minInclusive || value > maxInclusive)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(paramName, value, minInclusive, maxInclusive);
			}
		}
		private static void CheckArgumentRange(string paramName, double value, double minInclusive, double maxInclusive)
		{
			if (value < minInclusive || value > maxInclusive)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(paramName, value, minInclusive, maxInclusive);
			}
		}

		private static void ThrowOutOfRange_TooLongDuration()
		{
			throw new ArgumentOutOfRangeException(null, "GameClockDuration overflowed because the duration is too long.");
		}

		private static void ThrowIfOverflowedFromInt32YearMonthDay(long secs)
		{
			if (secs < MinSecDifference || secs > MaxSecDifference)
			{
				ThrowOutOfRange_TooLongDuration();
			}
		}

		public static int Compare(GameClockDuration t1, GameClockDuration t2)
		{
			if (t1._secs > t2._secs) return 1;
			if (t1._secs < t2._secs) return -1;
			return 0;
		}

		public int CompareTo(object value)
		{
			if (value == null) return 1;
			if (!(value is GameClockDuration otherDuration))
				throw new ArgumentException();

			long t = (otherDuration)._secs;
			if (_secs > t) return 1;
			if (_secs < t) return -1;
			return 0;
		}

		public int CompareTo(GameClockDuration value)
		{
			long t = value._secs;
			if (_secs > t) return 1;
			if (_secs < t) return -1;
			return 0;
		}

		public static GameClockDuration FromWeeks(long weeks)
		{
			const long minWeeks = MinSecDifference / SecsPerWeek;
			const long maxWeeks = MaxSecDifference / SecsPerWeek;
			CheckArgumentRange(nameof(weeks), weeks, minWeeks, maxWeeks);

			return new GameClockDuration(weeks * SecsPerWeek);
		}
		public static GameClockDuration FromDays(long days)
		{
			const long minDays = MinSecDifference / SecsPerDay;
			const long maxDays = MaxSecDifference / SecsPerDay;
			CheckArgumentRange(nameof(days), days, minDays, maxDays);

			return new GameClockDuration(days * SecsPerDay);
		}
		public static GameClockDuration FromHours(long hours)
		{
			const long minHours = MinSecDifference / SecsPerHour;
			const long maxHours = MaxSecDifference / SecsPerHour;
			CheckArgumentRange(nameof(hours), hours, minHours, maxHours);

			return new GameClockDuration(hours * SecsPerHour);
		}
		public static GameClockDuration FromMinutes(long minutes)
		{
			const long minMinutes = MinSecDifference / SecsPerMinute;
			const long maxMinutes = MaxSecDifference / SecsPerMinute;
			CheckArgumentRange(nameof(minutes), minutes, minMinutes, maxMinutes);

			return new GameClockDuration(minutes * SecsPerMinute);
		}
		public static GameClockDuration FromSeconds(long seconds)
		{
			const long minSeconds = MinSecDifference;
			const long maxSeconds = MaxSecDifference;
			CheckArgumentRange(nameof(seconds), seconds, minSeconds, maxSeconds);

			return new GameClockDuration(seconds);
		}
		public static GameClockDuration FromTimeSpan(TimeSpan timeSpan)
			=> new GameClockDuration(timeSpan.Ticks / TimeSpan.TicksPerSecond);

		public static GameClockDuration operator +(GameClockDuration d) => d;

		public GameClockDuration Add(GameClockDuration duration) => this + duration;

		public static GameClockDuration operator +(GameClockDuration d1, GameClockDuration d2)
		{
			long result = d1._secs + d2._secs;
			ThrowIfOverflowedFromInt32YearMonthDay(result);

			return new GameClockDuration(result);
		}

		public GameClockDuration Negate() => -this;

		public static GameClockDuration operator -(GameClockDuration d) => new(-d._secs);

		public GameClockDuration Subtract(GameClockDuration duration) => this - duration;

		public static GameClockDuration operator -(GameClockDuration d1, GameClockDuration d2)
		{
			long result = d1._secs - d2._secs;
			ThrowIfOverflowedFromInt32YearMonthDay(result);

			return new GameClockDuration(result);
		}

		public GameClockDuration Multiply(long factor) => this * factor;
		public GameClockDuration Multiply(double factor) => this * factor;

		/// <summary>
		/// Implements the operator * (multiplication).
		/// </summary>
		/// <param name="duration">The left hand side of the operator.</param>
		/// <param name="factor">The right hand side of the operator.</param>
		/// <returns>
		/// A new <see cref="GameClockDuration"/> representing the result of multiplying <paramref name="duration"/>
		/// by <paramref name="factor"/>.
		/// </returns>
		public static GameClockDuration operator *(GameClockDuration duration, long factor)
		{
			long result = duration._secs * factor;
			if (duration._secs != 0 && result / duration._secs != factor)
			{
				// overflow, throw ArgumentOutOfRangeException if overflowed to avoid the exception type surprise.
				ThrowOutOfRange_TooLongDuration();
			}
			ThrowIfOverflowedFromInt32YearMonthDay(result);

			return new GameClockDuration(result);
		}
		public static GameClockDuration operator *(GameClockDuration duration, double factor)
		{
			if (double.IsNaN(factor))
			{
				ThrowHelper.ThrowArgumentException_Arg_CannotBeNaN(nameof(factor));
			}

			// Usual use cases, where both differences are within about 156.92 weeks
			// If both value is within the range where the squared values can exactly represent as double values,
			// we can just calculate the result as a double and convert to long without any rounding errors
			long durationSecs = duration._secs;
			if (durationSecs >= -MaxSafeSqrtIntegerOutOfDouble && durationSecs <= MaxSafeSqrtIntegerOutOfDouble &&
				factor >= MinSafeSqrtIntegerOutOfDouble && factor <= MaxSafeSqrtIntegerOutOfDouble)
			{
				return new GameClockDuration((long)(durationSecs * factor));
			}

			return MultiplyOperatorNonTypicalCases(durationSecs, factor);

			static GameClockDuration MultiplyOperatorNonTypicalCases(long durationSecs, double factor)
			{
				// Calculate the result as a double for performance reasons (decimal calculation takes more than
				// 10x times).
				// The both values are weighted but can exactly calculate the result as a double.
				if ((durationSecs > -SecToOptimizeLowerWeighted && durationSecs < SecToOptimizeLowerWeighted &&
				factor > -SecToOptimizeHigherWeighted && factor < SecToOptimizeHigherWeighted) ||
				(durationSecs > -SecToOptimizeHigherWeighted && durationSecs < SecToOptimizeHigherWeighted &&
				factor > -SecToOptimizeLowerWeighted && factor < SecToOptimizeLowerWeighted))
				{
					return new GameClockDuration((long)(durationSecs * factor));
				}

				// Fall back to decimal arithmetic, so the calculation 100% will not have any rounding errors.
				// Throw ArgumentOutOfRangeException if OverflowException is thrown for decimal arithmetics or too
				// large factor to match what FromDecimalSecondsInternal throws for too large or small results.
				try
				{
					return FromDecimalSecondsInternal(durationSecs * (decimal)factor);
				}
				catch (OverflowException)
				{
					ThrowOutOfRange_TooLongDuration();
					return default;
				}
			}
		}

		public static GameClockDuration operator *(long factor, GameClockDuration duration)
			=> duration * factor;
		public static GameClockDuration operator *(double factor, GameClockDuration duration)
			=> duration * factor;

		public GameClockDuration Divide(long factor) => this / factor;
		public GameClockDuration Divide(double factor) => this / factor;

		public static GameClockDuration operator /(GameClockDuration duration, long divisor)
			=> new GameClockDuration(duration._secs / divisor);
		public static GameClockDuration operator /(GameClockDuration duration, double divisor)
		{
			if (double.IsNaN(divisor))
			{
				ThrowHelper.ThrowArgumentException_Arg_CannotBeNaN(nameof(divisor));
			}

			// Usual use cases, where both differences are within about 156.92 weeks
			// If both value is within the range where the squared values can exactly represent as double values,
			// we can just calculate the result as $a double and convert to long without any rounding errors
			long durationSecs = duration._secs;
			if (durationSecs >= -MaxSafeSqrtIntegerOutOfDouble && durationSecs <= MaxSafeSqrtIntegerOutOfDouble &&
				(divisor < -InverseOfMaxSafeSqrtIntegerOutOfDouble || divisor > InverseOfMaxSafeSqrtIntegerOutOfDouble))
			{
				// No need to check if the result is not a infinity or NaN, and the abs of divisor is large enough to
				// avoid infinities
				return new GameClockDuration((long)(durationSecs / divisor));
			}

			return DivideOperatorNonTypicalCases(durationSecs, divisor);

			static GameClockDuration DivideOperatorNonTypicalCases(long durationSecs, double divisor)
			{
				// Calculate the result as a double for performance reasons (decimal calculation takes more than
				// 10x times).
				// The both values are weighted but can exactly calculate the result as a double.
				if (durationSecs > -SecToOptimizeLowerWeighted && durationSecs < SecToOptimizeLowerWeighted &&
				(divisor < -InverseOfSecToOptimizeHigherWeighted || divisor > InverseOfSecToOptimizeHigherWeighted))
				{
					return new GameClockDuration((long)(durationSecs / divisor));
				}
				if (durationSecs > -SecToOptimizeHigherWeighted && durationSecs < SecToOptimizeHigherWeighted &&
				(divisor < -InverseOfSecToOptimizeLowerWeighted || divisor > InverseOfSecToOptimizeLowerWeighted))
				{
					return new GameClockDuration((long)(durationSecs / divisor));
				}

				// Fall back to decimal arithmetic, so the calculation 100% will not have any rounding errors.
				// Throw ArgumentOutOfRangeException if OverflowException is thrown for decimal arithmetics or too
				// large divisor to match what FromDecimalSecondsInternal throws for too large or small results.
				try
				{
					return FromDecimalSecondsInternal(durationSecs / (decimal)divisor);
				}
				catch (OverflowException)
				{
					ThrowOutOfRange_TooLongDuration();
					return default;
				}
			}
		}
		public static double operator /(GameClockDuration d1, GameClockDuration d2) => d1._secs / d2._secs;

		private static GameClockDuration IntervalFromF64Seconds(double secs)
		{
			// The value (MaxSecDifferenceOutOfBound - 1), which is 135536076770054399, can't exactly represent as
			// a double
			if (secs <= MaxNegativeSecDifferenceOutOfBound || secs >= MinPositiveSecDifferenceOutOfBound ||
				double.IsNaN(secs))
			{
				ThrowOutOfRange_TooLongDuration();
			}

			return new GameClockDuration((long)secs);
		}
		private static GameClockDuration FromDecimalSecondsInternal(decimal secs)
		{
			if (secs <= MaxNegativeSecDifferenceOutOfBound || secs >= MinPositiveSecDifferenceOutOfBound)
			{
				ThrowOutOfRange_TooLongDuration();
			}

			return new GameClockDuration((long)secs);
		}

		public bool Equals(GameClockDuration other)
		{
			return _secs == other._secs;
		}
		public override bool Equals(object obj)
		{
			if (obj is GameClockDuration duration)
			{
				return Equals(duration);
			}

			return false;
		}

		public static bool operator ==(GameClockDuration left, GameClockDuration right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(GameClockDuration left, GameClockDuration right)
		{
			return !left.Equals(right);
		}

		public static bool operator <(GameClockDuration t1, GameClockDuration t2) => t1._secs < t2._secs;
		public static bool operator <=(GameClockDuration t1, GameClockDuration t2) => t1._secs <= t2._secs;

		public static bool operator >(GameClockDuration t1, GameClockDuration t2) => t1._secs > t2._secs;

		public static bool operator >=(GameClockDuration t1, GameClockDuration t2) => t1._secs >= t2._secs;

		public override int GetHashCode()
		{
			return _secs.GetHashCode();
		}
	}
}
