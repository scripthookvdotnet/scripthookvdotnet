//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

internal static class IntExtensions
{
	/// <summary>
	/// Calculates the least non-negative remainder of <paramref name="lhs"/> (mod <paramref name="rhs"/>).
	/// This is done as if by the Euclidean division algorithm.
	/// </summary>
	public static int RemEuclid(this int lhs, int rhs)
	{
		int remainder = lhs % rhs;
		if (remainder >= 0)
		{
			return remainder;
		}

		int wrappedAbsRhs = rhs;
		if (wrappedAbsRhs != int.MinValue)
		{
			wrappedAbsRhs = Math.Abs(wrappedAbsRhs);
		}

		return unchecked(remainder + wrappedAbsRhs);
	}

	public static int DivEuclid(this int lhs, int rhs)
	{
		int q = lhs / rhs;
		if (lhs % rhs < 0)
		{
			return (rhs > 0) ? (q - 1) : (q + 1);
		}

		return q;
	}

	/// <summary>
	/// Calculates the least non-negative remainder of <paramref name="lhs"/> (mod <paramref name="rhs"/>).
	/// This is done as if by the Euclidean division algorithm.
	/// </summary>
	public static long RemEuclid(this long lhs, long rhs)
	{
		long remainder = lhs % rhs;
		if (remainder >= 0)
		{
			return remainder;
		}

		long wrappedAbsRhs = rhs;
		if (wrappedAbsRhs != long.MinValue)
		{
			wrappedAbsRhs = Math.Abs(wrappedAbsRhs);
		}

		return unchecked(remainder + wrappedAbsRhs);
	}

	public static long DivEuclid(this long lhs, long rhs)
	{
		long q = lhs / rhs;
		if (lhs % rhs < 0)
		{
			return (rhs > 0) ? (q - 1) : (q + 1);
		}

		return q;
	}
}
