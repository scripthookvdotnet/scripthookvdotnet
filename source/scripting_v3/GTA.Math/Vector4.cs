//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA.Math
{
    /// <summary>
    /// Represents a vector with four single-precision floating-point values that can be used to represent 4D
    /// coordinates or any other quadruplet of numeric values.
    /// </summary>
    /// <remarks>
    /// Guaranteed to be a 16-byte aligned struct, which has the same memory layout as <c>rage::Vector4</c> and
    /// <c>rage::Vec4V</c>. You can use this struct to read these kinds of data using memory dereference.
    /// </remarks>
    public struct Vector4 : IEquatable<Vector4>, IFormattable
    {
        /// <summary>
        /// Gets or sets the X component of the vector.
        /// </summary>
        /// <value>The X component of the vector.</value>
        public float X;

        /// <summary>
        /// Gets or sets the Y component of the vector.
        /// </summary>
        /// <value>The Y component of the vector.</value>
        public float Y;

        /// <summary>
        /// Gets or sets the Z component of the vector.
        /// </summary>
        /// <value>The Z component of the vector.</value>
        public float Z;

        /// <summary>
        /// Gets or sets the W component of the vector.
        /// </summary>
        /// <value>The W component of the vector.</value>
        public float W;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> class.
        /// </summary>
        /// <param name="x">Initial value for the X component of the vector.</param>
        /// <param name="y">Initial value for the Y component of the vector.</param>
        /// <param name="z">Initial value for the Z component of the vector.</param>
        /// <param name="w">Initial value for the W component of the vector.</param>
        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Returns this vector with a magnitude of 1.
        /// </summary>
        public readonly Vector4 Normalized => Normalize(new Vector4(X, Y, Z, W));

        /// <summary>
        /// Returns a zero vector, a vector with all components set to <c>0</c>.
        /// </summary>
        public static Vector4 Zero => new(0f, 0f, 0f, 0f);

        /// <summary>
        /// Returns a one vector, a vector with all components set to <c>1</c>.
        /// </summary>
        public static Vector4 One => new(1f, 1f, 1f, 1f);

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the X or Y component, depending on the index.</value>
        /// <param name="index">
        /// The index of the component to access.
        /// Use 0 for the X component, 1 for the Y component, 2 for the Z component, and 3 for the W component.
        /// </param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 3].</exception>
        public float this[int index]
        {
            readonly get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    case 3:
                        return W;
                }

                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index), "Indices for Vector4 run from 0 to 3, inclusive.");
                return 0f;
            }

            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    case 3:
                        W = value;
                        break;
                    default:
                        ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index), "Indices for Vector4 run from 0 to 3, inclusive.");
                        break;
                }
            }
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public readonly float Length()
        {
            return (float)System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
        }

        /// <summary>
        /// Calculates the squared length of the vector.
        /// </summary>
        /// <returns>The squared length of the vector.</returns>
        public readonly float LengthSquared()
        {
            return (X * X) + (Y * Y) + (Z * Z) + (W * W);
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        public void Normalize()
        {
            float length = Length();
            if (length == 0)
            {
                return;
            }

            float num = 1 / length;
            X *= num;
            Y *= num;
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static Vector4 Clamp(Vector4 value, Vector4 min, Vector4 max)
        {
            float x = value.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            float y = value.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            float z = value.Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;

            float w = value.W;
            w = (w > max.W) ? max.W : w;
            w = (w < min.W) ? min.W : w;

            return new Vector4(x, y, z, w);
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="start">Start vector.</param>
        /// <param name="end">End vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The linear interpolation of the two vectors.</returns>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned.
        /// </remarks>
        public static Vector4 Lerp(Vector4 start, Vector4 end, float amount)
        {
            Vector4 vector = default;

            vector.X = start.X + ((end.X - start.X) * amount);
            vector.Y = start.Y + ((end.Y - start.Y) * amount);
            vector.Z = start.Z + ((end.Z - start.Z) * amount);
            vector.W = start.W + ((end.W - start.W) * amount);

            return vector;
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        public static Vector4 Normalize(Vector4 vector)
        {
            vector.Normalize();
            return vector;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static float Dot(Vector4 left, Vector4 right)
            => (left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W);

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>A vector containing the smallest components of the source vectors.</returns>
        public static Vector4 Min(Vector4 left, Vector4 right)
        {
            Vector4 vector = default;;
            vector.X = (left.X < right.X) ? left.X : right.X;
            vector.Y = (left.Y < right.Y) ? left.Y : right.Y;
            vector.Z = (left.Z < right.Z) ? left.Z : right.Z;
            vector.W = (left.W < right.W) ? left.W : right.W;
            return vector;
        }
        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>A vector containing the largest components of the source vectors.</returns>
        public static Vector4 Max(Vector4 left, Vector4 right)
        {
            Vector4 vector = default;;
            vector.X = (left.X > right.X) ? left.X : right.X;
            vector.Y = (left.Y > right.Y) ? left.Y : right.Y;
            vector.Z = (left.Z > right.Z) ? left.Z : right.Z;
            vector.W = (left.W > right.W) ? left.W : right.W;
            return vector;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static Vector4 operator +(Vector4 left, Vector4 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static Vector4 operator -(Vector4 left, Vector4 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static Vector4 operator -(Vector4 value) => new(-value.X, -value.Y, -value.Z, -value.Z);

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4 operator *(Vector4 vector, float scale) => new(vector.X * scale, vector.Y * scale, vector.Z * scale, vector.Z * scale);

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4 operator *(float scale, Vector4 vector) => vector * scale;

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4 operator /(Vector4 vector, float scale) => new(vector.X / scale, vector.Y / scale, vector.Z / scale, vector.W / scale);

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(Vector4 left, Vector4 right) => left.Equals(right);

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(Vector4 left, Vector4 right) => !left.Equals(right);

        /// <summary>
        /// Converts a <see cref="Vector3"/> to a <see cref="Vector4"/> explicitly.
        /// </summary>
        public static explicit operator Vector4(Vector3 vector) => new(vector.X, vector.Y, vector.Z, 0);

        /// <summary>
        /// Converts a <see cref="Vector4"/> to a <see cref="Vector3"/> explicitly.
        /// </summary>
        public static explicit operator Vector3(Vector4 vector) => new(vector.X, vector.Y, vector.Z);

        /// <summary>
        /// Converts a <see cref="Vector2"/> to a <see cref="Vector4"/> explicitly.
        /// </summary>
        public static explicit operator Vector4(Vector2 vector) => new(vector.X, vector.Y, 0, 0);

        /// <summary>
        /// Converts a <see cref="Vector4"/> to a <see cref="Vector2"/> explicitly.
        /// </summary>
        public static explicit operator Vector2(Vector4 vector) => new(vector.X, vector.Y);

        /// <summary>
        /// Converts the value of the object to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance.</returns>
        public override readonly string ToString()
        {
            return $"X:{X.ToString()} Y:{Y.ToString()} Z:{Z.ToString()} W:{W.ToString()}";
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

            return $"X:{X.ToString(format)} Y:{Y.ToString(format)} Z:{Z.ToString(format)} W:{W.ToString(format)}";
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
            return $"X:{X.ToString(format, provider)} Y:{Y.ToString(format, provider)} Z:{Z.ToString(format, provider)} W:{W.ToString(format, provider)}";
        }

        public readonly bool Equals(Vector4 other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);

        public override readonly bool Equals(object obj) => obj is Vector4 other && Equals(other);

        public override readonly int GetHashCode()
        {
            unchecked
            {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ W.GetHashCode();
                return hashCode;
            }
        }
    }
}
