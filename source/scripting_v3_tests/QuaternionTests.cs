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
    public class QuaternionTests
    {
        private const float Cos15DegFloat = 0.96592582f;
        private const float Cos22_5DegFloat = 0.92387953f;
        private const float Cos30DegFloat = 0.86602540f;
        private const float Cos45DegFloat = 0.70710678f;
        private const float Cos60DegFloat = 0.5f;
        private const float Cos67_5DegFloat = 0.38268343f;

        private const float Sin15DegFloat = 0.2588190f;
        private const float Sin22_5DegFloat = 0.38268343f;
        private const float Sin30DegFloat = 0.5f;
        private const float Sin45DegFloat = 0.70710678f;
        private const float Sin60DegFloat = 0.86602540f;
        private const float Sin67_5DegFloat = 0.92387953f;

        private const float SqrtOfThree = 1.73205080f;

        private const float DEG_2_RAD = (float)((System.Math.PI / 180.0));
        public static TheoryData<Vector3, Quaternion> LookRotation_1arg_overload_Data =>
            new TheoryData<Vector3, Quaternion>
            {
                {
                   new Vector3(0f, 1f, 0f),
                   Quaternion.Identity
                },
                {
                    new Vector3(-1f, 0f, 0f),
                    new Quaternion(0f, 0f, Sin45DegFloat, Cos45DegFloat)
                },
                {
                    new Vector3(0f, -1f, 0f),
                    new Quaternion(0f, 0f, 1f, 0f)
                },
                {
                    new Vector3(1f, 0f, 0f),
                    new Quaternion(0f, 0f, -Sin45DegFloat, Cos45DegFloat)
                },
                // this should almost match the rotation rotated by 30 degrees around the Z axis from the identity
                {
                    new Vector3(-Cos60DegFloat, Sin60DegFloat, 0f),
                    new Quaternion(0f, 0f, Sin15DegFloat, Cos15DegFloat)
                },
                // this should almost match the rotation rotated by -135 degrees around the Z axis from the identity
                {
                    new Vector3(Cos45DegFloat, -Sin45DegFloat, 0f),
                    new Quaternion(0f, 0f, Cos22_5DegFloat, -Sin22_5DegFloat)
                },
            };

        public static TheoryData<Vector3, Vector3, Quaternion> LookRotation_2_args_overload_Data =>
            new TheoryData<Vector3, Vector3, Quaternion>
            {
                {
                    new Vector3(0f, 1f, 0f),
                    new Vector3(0f, 0f, 1f),
                    Quaternion.Identity
                },
                // this should almost match the rotation rotated by 30 degrees around the X axis from the identity
                {
                    new Vector3(0f, Cos30DegFloat, Sin30DegFloat),
                    new Vector3(0f, -Sin30DegFloat, Cos30DegFloat),
                    new Quaternion(Sin15DegFloat, 0f, 0f, Cos15DegFloat)
                },
                // this should almost match the rotation rotated by 45 degrees around the Z axis then 45 degrees around
                // the X axis from the identity
                {
                    new Vector3(-0.5f, 0.5f, 0.7071068f),
                    new Vector3(0.5f, -0.5f, 0.7071068f),
                    new Quaternion(0.3535534f, 0.1464466f, 0.3535534f, 0.8535534f)
                }
            };

        public static TheoryData<Vector3> Euler_with_YXZ_RotOrder_data =>
            new TheoryData<Vector3>
            {
                {
                    new Vector3(0f, 0f, 0f)
                },
                {
                    new Vector3(30f, 0f, 0f)
                },
                {
                    new Vector3(0f, 45f, 0f)
                },
                {
                    new Vector3(0f, 0f, 60f)
                },
                {
                    new Vector3(75f, 0f, 30f)
                },
                {
                    new Vector3(0f, 15f, 45f)
                },
                {
                    new Vector3(90f, 120f, 0f)
                },
                {
                    new Vector3(60f, 120f, 180f)
                },
            };

        [Theory]
        [MemberData(nameof(LookRotation_1arg_overload_Data))]
        public void LookRotation_1_args_overload_returns_quat_that_rotates_from_identity_to_look_dir_with_world_up(
            Vector3 forward, Quaternion expected)
        {
            Quaternion actual = Quaternion.LookRotation(forward);
            EqualsApprox(actual, expected, 1e-5f);
        }

        [Theory]
        [MemberData(nameof(LookRotation_2_args_overload_Data))]
        public void LookRotation_2_args_overload_returns_quat_that_rotates_from_identity_to_look_dir(
            Vector3 forward, Vector3 up, Quaternion expected)
        {
            Quaternion actual = Quaternion.LookRotation(forward, up);
            EqualsApprox(actual, expected, 1e-5f);
        }

        [Theory]
        [MemberData(nameof(Euler_with_YXZ_RotOrder_data))]
        public void
            Euler_with_Vector_arg_overload_returns_the_same_val_as_Euler_with_3_float_args_overload
        (
            Vector3 rot
        )
        {
            Quaternion ret_EulerWith1VectorOverload = Quaternion.Euler(rot);
            Quaternion ret_EulerWith3FloatOverload = Quaternion.Euler(rot.Z, rot.X, rot.Y);

            StrictEquals(ret_EulerWith1VectorOverload, ret_EulerWith3FloatOverload);
        }

        [Theory]
        [MemberData(nameof(Euler_with_YXZ_RotOrder_data))]
        public void
            Euler_with_Vector_and_EulerRotationOrder_args_overload_returns_the_same_val_as_Euler_with_3_floats_and_EulerRotationOrder_args_overload
        (
            Vector3 rot
        )
        {
            foreach (EulerRotationOrder rotOrder in (EulerRotationOrder[])Enum.GetValues(typeof(EulerRotationOrder)))
            {
                Quaternion ret_EulerWith1VectorAndRotOrderOverload = Quaternion.Euler(rot, rotOrder);
                Quaternion ret_EulerWith3FloatAndRotOrderOverload = Quaternion.Euler(rot.Z, rot.X, rot.Y, rotOrder);

                StrictEquals(ret_EulerWith1VectorAndRotOrderOverload, ret_EulerWith3FloatAndRotOrderOverload);
            }
        }

        [Theory]
        [MemberData(nameof(Euler_with_YXZ_RotOrder_data))]
        public void
            Euler_with_Vector_and_EulerRotationOrder_args_overload_returns_almost_the_same_val_as_Euler_with_Vector_and_rot_order_args_overload_with_rot_order_ZXY_passed
        (
            Vector3 rot
        )
        {
            Quaternion ret_EulerWith3FloatOverload = Quaternion.Euler(rot.Z, rot.X, rot.Y);
            Quaternion ret_EulerWith3FloatAndRotOrderOverload = Quaternion.Euler(rot.Z, rot.X, rot.Y, EulerRotationOrder.ZXY);

            EqualsApprox(ret_EulerWith3FloatOverload, ret_EulerWith3FloatAndRotOrderOverload, 1e-6f);
        }

        [Theory]
        [MemberData(nameof(Euler_with_YXZ_RotOrder_data))]
        public void Euler_with_3_float_args_overload_returns_the_same_val_as_RotationYawPitchRoll(Vector3 rot)
        {
            Quaternion ret_Euler = Quaternion.Euler(rot.Z, rot.X, rot.Y);
            Quaternion ret_RotationYawPitchRoll = Quaternion.RotationYawPitchRoll(
                rot.Z * DEG_2_RAD, rot.X * DEG_2_RAD, rot.Y * DEG_2_RAD);

            StrictEquals(ret_Euler, ret_RotationYawPitchRoll);
        }

        private static void StrictEquals(Quaternion left, Quaternion right)
        {
            Assert.True(left.X == right.X &&
                        left.Y == right.Y &&
                        left.Z == right.Z &&
                        left.W == right.W,
                $"StrictEquals assert failed. left: {left.ToString()}, right: {right.ToString()}");
        }

        private static void EqualsApprox(Quaternion left, Quaternion right, float tolerance)
        {
            Assert.True(System.Math.Abs(left.X - right.X) <= tolerance &&
                        System.Math.Abs(left.Y - right.Y) <= tolerance &&
                        System.Math.Abs(left.Z - right.Z) <= tolerance &&
                        System.Math.Abs(left.W - right.W) <= tolerance,
                $"EqualsApprox assert failed. left: {left.ToString()}, right: {right.ToString()}");
        }
    }
}
