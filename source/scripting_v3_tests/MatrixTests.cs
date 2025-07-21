//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using Xunit;
using GTA.Math;
using System;
using GTA;

namespace ScriptHookVDotNet_APIv3_Tests.Math
{
    public class MatrixTests
    {
        public static TheoryData<Matrix, Matrix> FastInverse_Data =>
            new TheoryData<Matrix, Matrix>
            {
                {
                   new Matrix()
                   {
                       M11 = 1f, M12 = 0f, M13 = 0f, M14 = 0f,
                       M21 = 0f, M22 = 0f, M23 = 1f, M24 = 0f,
                       M31 = 0f, M32 = -1f, M33 = 0f, M34 = 0f,
                       M41 = 10f, M42 = 0f, M43 = 0f, M44 = 1f
                   },
                   new Matrix()
                   {
                       M11 = 1f, M12 = 0f, M13 = 0f, M14 = 0f,
                       M21 = 0f, M22 = 0f, M23 = -1f, M24 = 0f,
                       M31 = 0f, M32 = 1f, M33 = 0f, M34 = 0f,
                       M41 = -10f, M42 = 0f, M43 = 0f, M44 = 1f
                   }
                },
                {
                   new Matrix()
                   {
                       M11 = 1f, M12 = 0f, M13 = 0f, M14 = 0f,
                       M21 = 0, M22 = -0.70710678118f, M23 = 0.70710678118f, M24 = 0f,
                       M31 = 0, M32 = 0.70710678118f, M33 = 0.70710678118f, M34 = 0f,
                       M41 = 100f, M42 = 0f, M43 = -10f, M44 = 1f
                   },
                   new Matrix()
                   {
                       M11 = 1f, M12 = 0f, M13 = 0f, M14 = 0f,
                       M21 = 0f, M22 = -0.70710678118f, M23 = 0.70710678118f, M24 = 0f,
                       M31 = 0f, M32 = 0.70710678118f, M33 = 0.70710678118f, M34 = 0f,
                       M41 = -100f, M42 = 7.0710678f, M43 = 7.0710678f, M44 = 1f
                   }
                },
            };

        public static TheoryData<Matrix> Inverse_And_Fast_Inverse_Comparison_Data =>
            new TheoryData<Matrix>
            {
                {
                   new Matrix()
                   {
                       M11 = 1f, M12 = 0f, M13 = 0f, M14 = 0f,
                       M21 = 0f, M22 = 0f, M23 = 1f, M24 = 0f,
                       M31 = 0f, M32 = -1f, M33 = 0f, M34 = 0f,
                       M41 = 10f, M42 = 0f, M43 = 0f, M44 = 1f
                   }
                },
                {
                   new Matrix()
                   {
                       M11 = 1f, M12 = 0f, M13 = 0f, M14 = 0f,
                       M21 = 0, M22 = -0.70710678118f, M23 = 0.70710678118f, M24 = 0f,
                       M31 = 0, M32 = 0.70710678118f, M33 = 0.70710678118f, M34 = 0f,
                       M41 = 100f, M42 = 0f, M43 = -10f, M44 = 1f
                   }
                },
            };

        [Theory]
        [MemberData(nameof(FastInverse_Data))]
        public void FastInverse_returns_approx_inversed_value(Matrix mat, Matrix expected)
        {
            Matrix actual = mat.FastInverse();
            EqualsApprox(actual, expected, 1e-5f);
        }

        [Theory]
        [MemberData(nameof(FastInverse_Data))]
        public void FastInvert_returns_approx_inversed_value(Matrix mat, Matrix expected)
        {
            Matrix actual = mat;
            actual.FastInvert();
            EqualsApprox(actual, expected, 1e-5f);
        }

        [Theory]
        [MemberData(nameof(Inverse_And_Fast_Inverse_Comparison_Data))]
        public void FastInverse_returns_approx_the_same_value_as_regular_inverse_matrix_if_the_matrix_is_orthogonal_and_no_affine_transform_is_applied(Matrix mat)
        {
            Assert.True(IsOrthonormal(mat, 1e-5f),
                "The matrix must be nearly orthonormal for FastInverse() to calculate the approximately correct value.");

            Matrix fastInverse = mat.FastInverse();
            Matrix regularInverse = mat.Inverse();

            EqualsApprox(fastInverse, regularInverse, 2e-5f);
        }

        private static void EqualsApprox(Matrix left, Matrix right, float tolerance)
        {
            Assert.True(System.Math.Abs(left.M11 - right.M11) <= tolerance &&
                        System.Math.Abs(left.M12 - right.M12) <= tolerance &&
                        System.Math.Abs(left.M13 - right.M13) <= tolerance &&
                        System.Math.Abs(left.M14 - right.M14) <= tolerance &&
                        System.Math.Abs(left.M21 - right.M21) <= tolerance &&
                        System.Math.Abs(left.M22 - right.M22) <= tolerance &&
                        System.Math.Abs(left.M23 - right.M23) <= tolerance &&
                        System.Math.Abs(left.M24 - right.M24) <= tolerance &&
                        System.Math.Abs(left.M31 - right.M31) <= tolerance &&
                        System.Math.Abs(left.M32 - right.M32) <= tolerance &&
                        System.Math.Abs(left.M33 - right.M33) <= tolerance &&
                        System.Math.Abs(left.M34 - right.M34) <= tolerance &&
                        System.Math.Abs(left.M41 - right.M41) <= tolerance &&
                        System.Math.Abs(left.M42 - right.M42) <= tolerance &&
                        System.Math.Abs(left.M43 - right.M43) <= tolerance &&
                        System.Math.Abs(left.M44 - right.M44) <= tolerance,
                $"Assert failed. left: {left.ToString()}, right: {right.ToString()}");
        }

        /// <summary>
        /// Checks if the rotation of matrix is approximately orthonormal. Equivalent to
        /// <c>rage::Matrix34::IsOrthonormal(rage::Matrix33 *this, float error)</c> in GTA5.exe.
        /// </summary>
        private static bool IsOrthonormal(in Matrix m, float tolerance)
        {
            float xScaleSquaredDiffFromOne = System.Math.Abs((new Vector3(m.M11, m.M12, m.M13)).LengthSquared() - 1f);
            if (xScaleSquaredDiffFromOne > tolerance * 2.0f)
            {
                return false;
            }

            float yScaleSquaredDiffFromOne = System.Math.Abs((new Vector3(m.M21, m.M22, m.M23)).LengthSquared() - 1f);
            if (yScaleSquaredDiffFromOne > tolerance * 2.0f)
            {
                return false;
            }
            float zScaleSquaredDiffFromOne = System.Math.Abs((new Vector3(m.M31, m.M32, m.M33)).LengthSquared() - 1f);
            if (zScaleSquaredDiffFromOne > tolerance * 2.0f)
            {
                return false;
            }

            bool areXAndYRotationsNearlyOrthogonal
                = System.Math.Abs(m.M11 * m.M21 + m.M12 * m.M22 + m.M13 * m.M23) <= tolerance;
            if (!areXAndYRotationsNearlyOrthogonal)
            {
                return false;
            }

            bool areXAndZRotationsNearlyOrthogonal
                = System.Math.Abs(m.M11 * m.M31 + m.M12 * m.M32 + m.M13 * m.M33) <= tolerance;
            if (!areXAndZRotationsNearlyOrthogonal)
            {
                return false;
            }

            bool areYAndZRotationsNearlyOrthogonal
                = System.Math.Abs(m.M21 * m.M31 + m.M22 * m.M32 + m.M23 * m.M33) <= tolerance;
            if (!areYAndZRotationsNearlyOrthogonal)
            {
                return false;
            }

            return true;
        }
    }
}
