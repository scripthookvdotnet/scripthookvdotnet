//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA.Chrono
{
    internal static class IntExtensions
    {
        /// <summary>
        /// Calculates the least non-negative remainder of <paramref name="lhs"/> (mod <paramref name="rhs"/>).
        /// This is done as if by the Euclidean division algorithm.
        /// </summary>
        public static int RemEuclid(this int lhs, int rhs)
        {
            int r = lhs % rhs;
            if (r < 0)
            {
                // If `rhs` is not `int.MinValue`, then `r + Math.Abs(rhs)` will not overflow and is clearly equivalent,
                // because `r` is negative.
                // Otherwise, `rhs` is `int.MinValue`, then we have `r - int.MinValue`, which is what we wanted (and will
                // not overflow for negative `r`).
                // We can't use Math.Abs(int) for int.MinValue, because it throws OverflowException for int.MinValue.
                return (rhs < 0) ? (r - rhs) : (r + rhs);
            }
            else
            {
                return r;
            }
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
            long r = lhs % rhs;
            if (r < 0)
            {
                // If `rhs` is not `int.MinValue`, then `r + Math.Abs(rhs)` will not overflow and is clearly equivalent,
                // because `r` is negative.
                // Otherwise, `rhs` is `int.MinValue`, then we have `r - int.MinValue`, which is what we wanted (and will
                // not overflow for negative `r`).
                // We can't use Math.Abs(int) for int.MinValue, because it throws OverflowException for int.MinValue.
                return (rhs < 0) ? (r - rhs) : (r + rhs);
            }
            else
            {
                return r;
            }
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
}
