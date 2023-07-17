//
// Copyright (C) 2007-2010 SlimDX Group
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Runtime.InteropServices;

namespace GTA.Math
{
	internal sealed class Random
	{
		internal static readonly System.Random Instance = new System.Random();
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Vector3 : IEquatable<Vector3>
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
		/// Initializes a new instance of the <see cref="Vector3"/> class.
		/// </summary>
		/// <param name="x">Initial value for the X component of the vector.</param>
		/// <param name="y">Initial value for the Y component of the vector.</param>
		/// <param name="z">Initial value for the Z component of the vector.</param>
		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		internal Vector3(SHVDN.NativeMemory.FVector3 value) : this(value.X, value.Y, value.Z)
		{
		}

		/// <summary>
		/// Returns this vector with a magnitude of 1.
		/// </summary>
		public Vector3 Normalized => Normalize(new Vector3(X, Y, Z));

		/// <summary>
		/// Returns a null vector. (0,0,0)
		/// </summary>
		public static Vector3 Zero => new(0.0f, 0.0f, 0.0f);

		/// <summary>
		/// Returns the world Up vector. (0,0,1)
		/// </summary>
		public static Vector3 WorldUp => new(0.0f, 0.0f, 1.0f);

		/// <summary>
		/// Returns the world Down vector. (0,0,-1)
		/// </summary>
		public static Vector3 WorldDown => new(0.0f, 0.0f, -1.0f);

		/// <summary>
		/// Returns the world North vector. (0,1,0)
		/// </summary>
		public static Vector3 WorldNorth => new(0.0f, 1.0f, 0.0f);

		/// <summary>
		/// Returns the world South vector. (0,-1,0)
		/// </summary>
		public static Vector3 WorldSouth => new(0.0f, -1.0f, 0.0f);

		/// <summary>
		/// Returns the world East vector. (1,0,0)
		/// </summary>
		public static Vector3 WorldEast => new(1.0f, 0.0f, 0.0f);

		/// <summary>
		/// Returns the world West vector. (-1,0,0)
		/// </summary>
		public static Vector3 WorldWest => new(-1.0f, 0.0f, 0.0f);

		/// <summary>
		/// Returns the relative Right vector. (1,0,0)
		/// </summary>
		public static Vector3 RelativeRight => new(1.0f, 0.0f, 0.0f);

		/// <summary>
		/// Returns the relative Left vector. (-1,0,0)
		/// </summary>
		public static Vector3 RelativeLeft => new(-1.0f, 0.0f, 0.0f);

		/// <summary>
		/// Returns the relative Front vector. (0,1,0)
		/// </summary>
		public static Vector3 RelativeFront => new(0.0f, 1.0f, 0.0f);

		/// <summary>
		/// Returns the relative Back vector. (0,-1,0)
		/// </summary>
		public static Vector3 RelativeBack => new(0.0f, -1.0f, 0.0f);

		/// <summary>
		/// Returns the relative Top vector. (0,0,1)
		/// </summary>
		public static Vector3 RelativeTop => new(0.0f, 0.0f, 1.0f);

		/// <summary>
		/// Returns the relative Bottom vector as used. (0,0,-1)
		/// </summary>
		public static Vector3 RelativeBottom => new(0.0f, 0.0f, -1.0f);

		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>The value of the X, Y or Z component, depending on the index.</value>
		/// <param name="index">The index of the component to access. Use 0 for the X component, 1 for the Y component and 2 for the Z component.</param>
		/// <returns>The value of the component at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 2].</exception>
		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return X;
					case 1: return Y;
					case 2: return Z;
				}

				throw new ArgumentOutOfRangeException("index", "Indices for Vector3 run from 0 to 2, inclusive.");
			}

			set
			{
				switch (index)
				{
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					default: throw new ArgumentOutOfRangeException("index", "Indices for Vector3 run from 0 to 2, inclusive.");
				}
			}
		}

		/// <summary>
		/// Calculates the length of the vector.
		/// </summary>
		/// <returns>The length of the vector.</returns>
		public float Length() => (float)(System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z)));

		/// <summary>
		/// Calculates the squared length of the vector.
		/// </summary>
		/// <returns>The squared length of the vector.</returns>
		public float LengthSquared() => (X * X) + (Y * Y) + (Z * Z);

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
			Z *= num;
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="position">The second vector to calculate the distance to.</param>
		/// <returns>The distance to the other vector.</returns>
		public float DistanceTo(Vector3 position) => (position - this).Length();

		/// <summary>
		/// Calculates the squared distance between two vectors.
		/// </summary>
		/// <param name="position">The second vector to calculate the distance to.</param>
		/// <returns>The distance to the other vector.</returns>
		public float DistanceToSquared(Vector3 position) => DistanceSquared(position, this);

		/// <summary>
		/// Calculates the distance between two vectors, ignoring the Z-component.
		/// </summary>
		/// <param name="position">The second vector to calculate the distance to.</param>
		/// <returns>The distance to the other vector.</returns>
		public float DistanceTo2D(Vector3 position)
		{
			var lhs = new Vector3(X, Y, 0.0f);
			var rhs = new Vector3(position.X, position.Y, 0.0f);

			return Distance(lhs, rhs);
		}

		/// <summary>
		/// Calculates the squared distance between two vectors, ignoring the Z-component.
		/// </summary>
		/// <param name="position">The second vector to calculate the squared distance to.</param>
		/// <returns>The distance to the other vector.</returns>
		public float DistanceToSquared2D(Vector3 position)
		{
			var lhs = new Vector3(X, Y, 0.0f);
			var rhs = new Vector3(position.X, position.Y, 0.0f);

			return DistanceSquared(lhs, rhs);
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="position1">The first vector to calculate the distance to the second vector.</param>
		/// <param name="position2">The second vector to calculate the distance to the first vector.</param>
		/// <returns>The distance between the two vectors.</returns>
		public static float Distance(Vector3 position1, Vector3 position2) => (position1 - position2).Length();

		/// <summary>
		/// Calculates the squared distance between two vectors.
		/// </summary>
		/// <param name="position1">The first vector to calculate the squared distance to the second vector.</param>
		/// <param name="position2">The second vector to calculate the squared distance to the first vector.</param>
		/// <returns>The squared distance between the two vectors.</returns>
		public static float DistanceSquared(Vector3 position1, Vector3 position2) => (position1 - position2).LengthSquared();

		/// <summary>
		/// Calculates the distance between two vectors, ignoring the Z-component.
		/// </summary>
		/// <param name="position1">The first vector to calculate the distance to the second vector.</param>
		/// <param name="position2">The second vector to calculate the distance to the first vector.</param>
		/// <returns>The distance between the two vectors.</returns>
		public static float Distance2D(Vector3 position1, Vector3 position2)
		{
			var pos1 = new Vector3(position1.X, position1.Y, 0);
			var pos2 = new Vector3(position2.X, position2.Y, 0);

			return (pos1 - pos2).Length();
		}

		/// <summary>
		/// Calculates the squared distance between two vectors, ignoring the Z-component.
		/// </summary>
		/// <param name="position1">The first vector to calculate the squared distance to the second vector.</param>
		/// <param name="position2">The second vector to calculate the squared distance to the first vector.</param>
		/// <returns>The squared distance between the two vectors.</returns>
		public static float DistanceSquared2D(Vector3 position1, Vector3 position2)
		{
			var pos1 = new Vector3(position1.X, position1.Y, 0);
			var pos2 = new Vector3(position2.X, position2.Y, 0);

			return (pos1 - pos2).LengthSquared();
		}

		/// <summary>
		/// Returns the angle in degrees between from and to.
		/// The angle returned is always the acute angle between the two vectors.
		/// </summary>
		public static float Angle(Vector3 from, Vector3 to)
		{
			double dot = Dot(from.Normalized, to.Normalized);
			return (float)(System.Math.Acos((dot)) * (180.0 / System.Math.PI));
		}

		/// <summary>
		/// Returns the signed angle in degrees between from and to.
		/// </summary>
		public static float SignedAngle(Vector3 from, Vector3 to, Vector3 planeNormal)
		{
			Vector3 perpVector = Cross(planeNormal, from);

			float angle = Angle(from, to);
			float dot = Dot(perpVector, to);
			if (dot < 0)
			{
				angle *= -1;
			}

			return angle;
		}

		/// <summary>
		/// Converts a vector to a heading.
		/// </summary>
		public float ToHeading() => (float)((System.Math.Atan2(X, -Y) + System.Math.PI) * (180.0 / System.Math.PI));

		/// <summary>
		/// Creates a random vector inside the circle around this position.
		/// </summary>
		public Vector3 Around(float distance) => this + (RandomXY() * distance);

		/// <summary>
		/// Returns a new normalized vector with random X and Y components.
		/// </summary>
		public static Vector3 RandomXY()
		{
			Vector3 v = Zero;
			double radian = Random.Instance.NextDouble() * 2 * System.Math.PI;

			v.X = (float)(System.Math.Cos(radian));
			v.Y = (float)(System.Math.Sin(radian));
			v.Normalize();
			return v;
		}

		/// <summary>
		/// Returns a new normalized vector with random X, Y and Z components.
		/// </summary>
		public static Vector3 RandomXYZ()
		{
			Vector3 v = Zero;
			double radian = Random.Instance.NextDouble() * 2.0 * System.Math.PI;
			double cosTheta = (Random.Instance.NextDouble() * 2.0) - 1.0;
			double theta = System.Math.Acos(cosTheta);

			v.X = (float)(System.Math.Sin(theta) * System.Math.Cos(radian));
			v.Y = (float)(System.Math.Sin(theta) * System.Math.Sin(radian));
			v.Z = (float)(System.Math.Cos(theta));
			v.Normalize();
			return v;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="left">The first vector to add.</param>
		/// <param name="right">The second vector to add.</param>
		/// <returns>The sum of the two vectors.</returns>
		public static Vector3 Add(Vector3 left, Vector3 right) => new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="left">The first vector to subtract.</param>
		/// <param name="right">The second vector to subtract.</param>
		/// <returns>The difference of the two vectors.</returns>
		public static Vector3 Subtract(Vector3 left, Vector3 right) => new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector3 Multiply(Vector3 value, float scale) => new Vector3(value.X * scale, value.Y * scale, value.Z * scale);

		/// <summary>
		/// Modulates a vector by another.
		/// </summary>
		/// <param name="left">The first vector to modulate.</param>
		/// <param name="right">The second vector to modulate.</param>
		/// <returns>The multiplied vector.</returns>
		public static Vector3 Modulate(Vector3 left, Vector3 right) => new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector3 Divide(Vector3 value, float scale) => new Vector3(value.X / scale, value.Y / scale, value.Z / scale);

		/// <summary>
		/// Reverses the direction of a given vector.
		/// </summary>
		/// <param name="value">The vector to negate.</param>
		/// <returns>A vector facing in the opposite direction.</returns>
		public static Vector3 Negate(Vector3 value) => new Vector3(-value.X, -value.Y, -value.Z);

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>The clamped value.</returns>
		public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
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

			return new Vector3(x, y, z);
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
		public static Vector3 Lerp(Vector3 start, Vector3 end, float amount)
		{
			Vector3 vector = Zero;

			vector.X = start.X + ((end.X - start.X) * amount);
			vector.Y = start.Y + ((end.Y - start.Y) * amount);
			vector.Z = start.Z + ((end.Z - start.Z) * amount);

			return vector;
		}

		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		/// <param name="vector">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
		public static Vector3 Normalize(Vector3 vector)
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
		public static float Dot(Vector3 left, Vector3 right) => (left.X * right.X + left.Y * right.Y + left.Z * right.Z);

		/// <summary>
		/// Calculates the cross product of two vectors.
		/// </summary>
		/// <param name="left">First source vector.</param>
		/// <param name="right">Second source vector.</param>
		/// <returns>The cross product of the two vectors.</returns>
		public static Vector3 Cross(Vector3 left, Vector3 right)
		{
			Vector3 result = Zero;
			result.X = left.Y * right.Z - left.Z * right.Y;
			result.Y = left.Z * right.X - left.X * right.Z;
			result.Z = left.X * right.Y - left.Y * right.X;
			return result;
		}
		/// <summary>
		/// Projects a vector onto another vector.
		/// </summary>
		/// <param name="vector">The vector to project.</param>
		/// <param name="onNormal">Vector to project onto, does not assume it is normalized.</param>
		/// <returns>The projected vector.</returns>
		public static Vector3 Project(Vector3 vector, Vector3 onNormal) => onNormal * Dot(vector, onNormal) / Dot(onNormal, onNormal);

		/// <summary>
		/// Projects a vector onto a plane defined by a normal orthogonal to the plane.
		/// </summary>
		/// <param name="vector">The vector to project.</param>
		/// <param name="planeNormal">Normal of the plane,  does not assume it is normalized.</param>
		/// <returns>The Projection of vector onto plane.</returns>
		public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal) => (vector - Project(vector, planeNormal));

		/// <summary>
		/// Returns the reflection of a vector off a surface that has the specified normal.
		/// </summary>
		/// <param name="vector">The vector to project onto the plane.</param>
		/// <param name="normal">Normal of the surface.</param>
		/// <returns>The reflected vector.</returns>
		/// <remarks>Reflect only gives the direction of a reflection off a surface, it does not determine
		/// whether the original vector was close enough to the surface to hit it.</remarks>
		public static Vector3 Reflect(Vector3 vector, Vector3 normal)
		{
			Vector3 result = Zero;
			float dot = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);

			result.X = vector.X - ((2.0f * dot) * normal.X);
			result.Y = vector.Y - ((2.0f * dot) * normal.Y);
			result.Z = vector.Z - ((2.0f * dot) * normal.Z);

			return result;
		}

		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors.
		/// </summary>
		/// <param name="value1">The first source vector.</param>
		/// <param name="value2">The second source vector.</param>
		/// <returns>A vector containing the smallest components of the source vectors.</returns>
		public static Vector3 Minimize(Vector3 value1, Vector3 value2)
		{
			Vector3 vector = Zero;
			vector.X = (value1.X < value2.X) ? value1.X : value2.X;
			vector.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
			vector.Z = (value1.Z < value2.Z) ? value1.Z : value2.Z;
			return vector;
		}

		/// <summary>
		/// Returns a vector containing the largest components of the specified vectors.
		/// </summary>
		/// <param name="value1">The first source vector.</param>
		/// <param name="value2">The second source vector.</param>
		/// <returns>A vector containing the largest components of the source vectors.</returns>
		public static Vector3 Maximize(Vector3 value1, Vector3 value2)
		{
			Vector3 vector = Zero;
			vector.X = (value1.X > value2.X) ? value1.X : value2.X;
			vector.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
			vector.Z = (value1.Z > value2.Z) ? value1.Z : value2.Z;
			return vector;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="left">The first vector to add.</param>
		/// <param name="right">The second vector to add.</param>
		/// <returns>The sum of the two vectors.</returns>
		public static Vector3 operator +(Vector3 left, Vector3 right) => new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="left">The first vector to subtract.</param>
		/// <param name="right">The second vector to subtract.</param>
		/// <returns>The difference of the two vectors.</returns>
		public static Vector3 operator -(Vector3 left, Vector3 right) => new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

		/// <summary>
		/// Reverses the direction of a given vector.
		/// </summary>
		/// <param name="vector">The vector to negate.</param>
		/// <returns>A vector facing in the opposite direction.</returns>
		public static Vector3 operator -(Vector3 vector) => new Vector3(-vector.X, -vector.Y, -vector.Z);

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="vector">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector3 operator *(Vector3 vector, float scale) => new Vector3(vector.X * scale, vector.Y * scale, vector.Z * scale);

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="vector">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector3 operator *(float scale, Vector3 vector) => vector * scale;

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="vector">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector3 operator /(Vector3 vector, float scale)
		{
			float invScale = 1.0f / scale;
			return new Vector3(vector.X * invScale, vector.Y * invScale, vector.Z * invScale);
		}

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Vector3 left, Vector3 right) => left.Equals(right);

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(Vector3 left, Vector3 right) => !left.Equals(right);

		/// <summary>
		/// Converts a Vector3 to a Vector2 implicitly.
		/// </summary>
		public static implicit operator Vector2(Vector3 vector) => new Vector2(vector.X, vector.Y);

		/// <summary>
		/// Converts the matrix to an array of floats.
		/// </summary>
		internal float[] ToArray() => new[] { X, Y, Z };

		/// <summary>
		/// Converts the value of the object to its equivalent string representation.
		/// </summary>
		/// <returns>The string representation of the value of this instance.</returns>
		public override string ToString() => $"X:{X.ToString()} Y:{Y.ToString()} Z:{Z.ToString()}";

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = X.GetHashCode();
				hashCode = (hashCode * 397) ^ Y.GetHashCode();
				hashCode = (hashCode * 397) ^ Z.GetHashCode();
				return hashCode;
			}
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">Object to make the comparison with.</param>
		/// <returns><see langword="true" /> if the current instance is equal to the specified object; <see langword="false" /> otherwise.</returns>
		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((Vector3)obj);
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to the specified object.
		/// </summary>
		/// <param name="other">Object to make the comparison with.</param>
		/// <returns><see langword="true" /> if the current instance is equal to the specified object; <see langword="false" /> otherwise.</returns>
		public bool Equals(Vector3 other) => (X == other.X && Y == other.Y && Z == other.Z);

		/// <summary>
		/// Determines whether the specified object instances are considered equal.
		/// </summary>
		/// <param name="value1">The first value to compare.</param>
		/// <param name="value2">The second value to compare.</param>
		/// <returns><see langword="true" /> if <paramref name="value1"/> is the same instance as <paramref name="value2"/> or
		/// if both are <see langword="null" /> references or if <c>value1.Equals(value2)</c> returns <see langword="true" />; otherwise, <see langword="false" />.</returns>
		public static bool Equals(ref Vector3 value1, ref Vector3 value2) => value1.Equals(value2);

		internal SHVDN.NativeMemory.FVector3 ToInternalFVector3() => new SHVDN.NativeMemory.FVector3(X, Y, Z);
	}
}
