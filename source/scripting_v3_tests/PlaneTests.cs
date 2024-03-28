//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using Xunit;
using GTA.Math;
using System;

namespace ScriptHookVDotNet_APIv3_Tests.Math
{
    public class PlaneTests
    {
        public static TheoryData<Plane, Vector3, float> DistanceTo_Data =>
            new TheoryData<Plane, Vector3, float>
            {
                { new Plane(new Vector3(0f, 0f, 1f), 1f), new Vector3(1f, 1f, 10f), 9f },
                { new Plane(new Vector3(0f, 0f, 1f), -1f), new Vector3(1f, 1f, 10f), 11f },
                { new Plane(new Vector3(0f, 0f, 1f), 10f), new Vector3(1f, 1f, 10f), 0f },
            };

        [Theory]
        [MemberData(nameof(DistanceTo_Data))]
        public void DistanceTo_returns_signed_distance(Plane plane, Vector3 point, float expected)
        {
            float actual = plane.DistanceTo(point);
            Assert.Equal(actual, expected, 1e-5f);
        }

        public static TheoryData<Plane, Vector3, Vector3> ProjectPoint_Data =>
            new TheoryData<Plane, Vector3, Vector3>
            {
                {
                    new Plane(new Vector3(0f, 0f, 1f), 1f), new Vector3(1f, 0f, 0f), new Vector3(1f, 0f, 1f)
                },
                {
                    new Plane(new Vector3(0f, 0f, 1f), 1f), new Vector3(-1f, 0f, 0f), new Vector3(-1f, 0f, 1f)
                },
                {
                    new Plane(new Vector3(0f, 0f, 1f), 1f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 1f)
                },
                {
                    new Plane(new Vector3(0f, 0f, 1f), 1f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 1f)
                },
                {
                    new Plane(new Vector3(0f, 0f, 1f), -1f), new Vector3(1f, 0f, 0f), new Vector3(1f, 0f, -1f)
                },
                {
                    new Plane(new Vector3(0f, 1f, 0f), 1f), new Vector3(1f, 0f, 0f), new Vector3(1f, 1f, 0f)
                },
                {
                    new Plane(new Vector3(0f, 1f, 0f), 1f), new Vector3(-1f, 0f, 0f), new Vector3(-1f, 1f, 0f)
                },
                {
                    new Plane(new Vector3(0f, 1f, 0f), 1f), new Vector3(0f, 0f, 1f), new Vector3(0f, 1f, 1f)
                },
                {
                    new Plane(new Vector3(0f, 1f, 0f), 1f), new Vector3(0f, 0f, -1f), new Vector3(0f, 1f, -1f)
                },
            };

        [Theory]
        [MemberData(nameof(ProjectPoint_Data))]
        public void Can_find_ortho_projection_point_on_plane_using_point_that_is_not_on_plane(Plane plane, Vector3 point,
            Vector3 expected)
        {
            Vector3 actual = plane.ProjectPoint(point);
            EqualsApprox(actual, expected, 1e-5f);
        }

        public static TheoryData<Plane, Plane, Plane, Vector3> Intersect3_Successful_Data =>
            new TheoryData<Plane, Plane, Plane, Vector3>
            {
                {
                    new Plane(new Vector3(1f, 0f, 0f), 1f), new Plane(new Vector3(0f, 1f, 0f), 1f),
                    new Plane(new Vector3(0f, 0f, 1f), 1f), new Vector3(1f, 1f, 1f)
                },
                {
                    new Plane(new Vector3(0f, 1f, 0f), 1f), new Plane(new Vector3(0f, 0f, 1f), 1f),
                    new Plane(new Vector3(1f, 0f, 0f), 1f), new Vector3(1f, 1f, 1f)
                },
                {
                    new Plane(new Vector3(1f, 0f, 0f), 1f), new Plane(new Vector3(0f, 1f, 0f), 1f),
                    new Plane(new Vector3(0f, 0f, 1f), 1f), new Vector3(1f, 1f, 1f)
                },
                {
                    new Plane(new Vector3(0f, 1f, 0f), 1f), new Plane(new Vector3(0f, 0f, 1f), 1f),
                    new Plane(new Vector3(1f, 0f, 0f), 1f), new Vector3(1f, 1f, 1f)
                },
            };

        [Theory]
        [MemberData(nameof(PlaneTests.Intersect3_Successful_Data))]
        public void Intersect3_successfully_calculates_intersection_if_determinant_of_3_planes_is_nonzero(
            Plane a, Plane b, Plane c, Vector3 expected)
        {
            bool foundIntersection = Plane.Intersect3(a, b, c, out Vector3 actual);
            Assert.True(foundIntersection);
            EqualsApprox(actual, expected, 1e-5f);
        }

        public static TheoryData<Plane, Vector3, Vector3> IntersectsRay_Failed_Before_Start_Point_Data =>
            new TheoryData<Plane, Vector3, Vector3>
            {
                {
                    new Plane(new Vector3(0f, 0f, 1f), 1f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -1f)
                },
                {
                    new Plane(new Vector3(1f, 0f, 0f), 1000f), new Vector3(500f, 0f, 0f), new Vector3(-1f, 0f, 0f)
                },
            };

        [Theory]
        [MemberData(nameof(IntersectsRay_Failed_Before_Start_Point_Data))]
        public void IntersectsRay_fails_if_the_intersect_point_is_before_start_point(
            Plane plane, Vector3 from, Vector3 dir)
        {
            // Make sure if the normal of plane and the direction vector are not parallel, so we can make sure that
            // `Plane.IntersectsRay` fails for the intersect point being before `from` (the start point)
            float dot = Vector3.Dot(plane.Normal, dir);
            Assert.True(System.Math.Abs(dot) >= 1e-5);

            bool foundIntersection = plane.IntersectsRay(from, dir, out _);
            Assert.False(foundIntersection);
        }

        private static void EqualsApprox(Vector3 left, Vector3 right, float tolerance)
        {
            Assert.True(System.Math.Abs(left.X - right.X) <= tolerance &&
                        System.Math.Abs(left.Y - right.Y) <= tolerance &&
                        System.Math.Abs(left.Z - right.Z) <= tolerance,
                $"Assert failed. left: {left.ToString()}, right: {right.ToString()}");
        }

        /// <summary>
        /// Calculates the closest point.
        /// </summary>
        /// <param name="point">The point to test that is not on the plane.</param>
        /// <param name="planeNormal">The normal of the plane.</param>
        /// <param name="planePoint">A point on the plane.</param>
        /// <returns>The closest point on the plane.</returns>
        private static Vector3 CalcPointOnPlane(Vector3 point, Vector3 planeNormal, Vector3 planePoint)
        {
            float dist = Vector3.Dot(planeNormal, point - planePoint);

            Vector3 res = default;
            res.X = point.X - planeNormal.X * dist;
            res.Y = point.Y - planeNormal.Y * dist;
            res.Z = point.Z - planeNormal.Z * dist;

            return res;
        }
    }
}
