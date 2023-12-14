//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA.Math
{
    /// <summary>
    /// Represents a plane in 3D space.
    /// </summary>
    /// <remarks>
    /// Do not try to directly read <c>rage::Vector4</c> and <c>rage::Vec4V</c> using this struct.
    /// Doing so will result in incorrect values. Instead, use read them as <see cref="Vector4"/> and then convert to
    /// <see cref="Plane"/> using the explicit cast.
    /// </remarks>
    public struct Plane : IEquatable<Plane>, IFormattable
    {
        /// <summary>
        /// Gets or sets the X component of the vector.
        /// </summary>
        /// <value>The X component of the vector.</value>
        public Vector3 Normal;

        /// <summary>
        /// Gets or sets the distance component of the vector.
        /// </summary>
        /// <value>The distance component of the vector.</value>
        public float D;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class.
        /// </summary>
        /// <param name="normal">Initial value for the normal component of the vector.</param>
        /// <param name="d">Initial value for the distance component of the vector.</param>
        public Plane(Vector3 normal, float d)
        {
            Normal = normal;
            D = d;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class.
        public Plane(Vector3 normal, Vector3 point)
        {
            Normal = normal;
            D = Vector3.Dot(normal, point);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class.
        /// The resulting plane goes through the given three points.
        /// The points go around counter-clockwise as you look down on the top surface of the plane, which is the same
        /// way as the <c>TPlane</c> constructors with 3 <c>TVector</c> points in Unreal Engine 5, and
        /// <c>rage::Vector4::ComputePlane</c> in GTA V (the name is not included in production builds).
        /// </summary>
        /// <param name="a">The first point in the plane.</param>
        /// <param name="b">The second point in the plane.</param>
        /// <param name="c">The third point in the plane.</param>
        public Plane(Vector3 a, Vector3 b, Vector3 c)
        {
            Normal = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            D = Vector3.Dot(Normal, a);
        }

        /// <summary>
        /// Returns this vector with a magnitude of 1.
        /// </summary>
        public readonly Plane Normalized => Normalize(new Plane(Normal, D));

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        public void Normalize()
        {
            float length = Normal.Length();
            if (length == 0)
            {
                return;
            }

            float num = 1 / length;
            Normal *= num;
            D *= num;
        }

        /// <summary>
        /// Converts the vector into a unit plane.
        /// </summary>
        /// <param name="plane">The plane to normalize.</param>
        /// <returns>The normalized plane.</returns>
        public static Plane Normalize(Plane plane)
        {
            plane.Normalize();
            return plane;
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(Plane left, Plane right) => left.Equals(right);

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(Plane left, Plane right) => !left.Equals(right);

        /// <summary>
        /// Converts a <see cref="Vector4"/> to a <see cref="Vector3"/> explicitly.
        /// </summary>
        public static explicit operator Plane(Vector4 vector) => new(new Vector3(vector.X, vector.Y, vector.Z), vector.W);

        /// <summary>
        /// Converts a <see cref="Vector3"/> to a <see cref="Vector4"/> explicitly.
        /// </summary>
        public static explicit operator Vector4(Plane plane) => new(plane.Normal.X, plane.Normal.Y, plane.Normal.Z, plane.D);

        /// <summary>
        /// Converts the value of the object to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance.</returns>
        public override readonly string ToString()
        {
            return $"Normal:{Normal.ToString()} D:{D.ToString()}";
        }

        /// <summary>
        /// Converts the value of the object to its equivalent string representation.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>The string representation of the value of this instance.</returns>
        public readonly string ToString(string format)
        {
            if (format == null)
            {
                return ToString();
            }

            return $"Normal:{Normal.ToString(format)} D:{D.ToString(format)}";
        }

        /// <summary>
        /// Returns the string representation of the current instance using the specified format string to format
        /// individual elements and the specified format provider to define culture-specific formatting.
        /// </summary>
        /// <param name="format">
        /// A standard or custom numeric format string that defines the format of individual elements.
        /// </param>
        /// <param name="provider">
        /// A format provider that supplies culture-specific formatting information.
        /// </param>
        /// <returns>The string representation of the value of this instance.</returns>
        public readonly string ToString(string format, IFormatProvider provider)
        {
            return $"Normal:{Normal.ToString(format, provider)} D:{D.ToString(format, provider)}";
        }

        public readonly bool Equals(Plane other) => Normal.Equals(other.Normal) && D.Equals(other.D);

        public override readonly bool Equals(object obj) => obj is Plane other && Equals(other);

        public override readonly int GetHashCode()
        {
            unchecked
            {
                int hashCode = Normal.GetHashCode();
                hashCode = (hashCode * 397) ^ D.GetHashCode();
                return hashCode;
            }
        }
    }
}
