using System;

namespace ScriptHookVDotNet_APIv3_Tests
{
	internal static class DoubleExtensions
	{
		/// <summary>
		/// Returns the least number greater than a given value.
		/// Returns the same value as how <c>Double.BitIncrement(double)</c> returns in .NET 7+ and how
		/// <c>f64::next_up(self)</c> returns in Rust (in nightly API).
		/// </summary>
		internal static double NextUp(this double x)
		{
			long bits = BitConverter.DoubleToInt64Bits(x);

			if (((bits >> 32) & 0x7FF00000) >= 0x7FF00000)
			{
				// NaN returns NaN
				// -Infinity returns double.MinValue
				// +Infinity returns +Infinity
				return (bits == unchecked((long)(0xFFF00000_00000000))) ? double.MinValue : x;
			}

			if (bits == unchecked((long)(0x80000000_00000000)))
			{
				// -0.0 returns double.Epsilon
				return double.Epsilon;
			}

			// Negative values need to be decremented
			// Positive values need to be incremented

			bits += ((bits < 0) ? -1 : +1);
			return BitConverter.Int64BitsToDouble(bits);
		}

		/// <summary>
		/// Returns the greatest number less than a given value.
		/// Returns the same value as how <c>Double.BitDecrement(double)</c> returns in .NET 7+ and how
		/// <c>f64::next_down(self)</c> returns in Rust (in nightly API).
		/// </summary>
		internal static double NextDown(this double x)
		{
			long bits = BitConverter.DoubleToInt64Bits(x);

			if (((bits >> 32) & 0x7FF00000) >= 0x7FF00000)
			{
				// NaN returns NaN
				// -Infinity returns -Infinity
				// +Infinity returns double.MaxValue
				return (bits == 0x7FF00000_00000000) ? double.MaxValue : x;
			}

			if (bits == 0x00000000_00000000)
			{
				// +0.0 returns -double.Epsilon
				return -double.Epsilon;
			}

			// Negative values need to be incremented
			// Positive values need to be decremented

			bits += ((bits < 0) ? +1 : -1);
			return BitConverter.Int64BitsToDouble(bits);
		}
	}
}
