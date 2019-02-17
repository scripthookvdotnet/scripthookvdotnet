//
// Copyright (C) 2007-2010 SlimDX Group
// 
// Permission is hereby granted, free  of charge, to any person obtaining a copy of this software  and
// associated  documentation  files (the  "Software"), to deal  in the Software  without  restriction,
// including  without  limitation  the  rights  to use,  copy,  modify,  merge,  publish,  distribute,
// sublicense, and/or sell  copies of the  Software,  and to permit  persons to whom  the Software  is
// furnished to do so, subject to the following conditions:
// 
// The  above  copyright  notice  and this  permission  notice shall  be included  in  all  copies  or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS",  WITHOUT WARRANTY OF  ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT  LIMITED  TO  THE  WARRANTIES  OF  MERCHANTABILITY,  FITNESS  FOR  A   PARTICULAR  PURPOSE  AND
// NONINFRINGEMENT.  IN  NO  EVENT SHALL THE  AUTHORS  OR COPYRIGHT HOLDERS  BE LIABLE FOR  ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,  OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace GTA.Math
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Vector2 : IEquatable<Vector2>
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
		/// Initializes a new instance of the <see cref="Vector2"/> class.
		/// </summary>
		/// <param name="x">Initial value for the X component of the vector.</param>
		/// <param name="y">Initial value for the Y component of the vector.</param>
		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Returns this vector with a magnitude of 1.
		/// </summary>
		public Vector2 Normalized => Normalize(new Vector2(X, Y));

		/// <summary>
		/// Returns a null vector. (0,0)
		/// </summary>
		public static Vector2 Zero => new Vector2(0.0f, 0.0f);

		/// <summary>
		/// The X unit <see cref="Vector2"/> (1, 0).
		/// </summary>
		public static Vector2 UnitX => new Vector2(1.0f, 0.0f);

		/// <summary>
		/// The Y unit <see cref="Vector2"/> (0, 1).
		/// </summary>
		public static Vector2 UnitY => new Vector2(0.0f, 1.0f);

		/// <summary>
		/// Returns the up vector. (0,1)
		/// </summary>
		public static Vector2 Up => new Vector2(0.0f, 1.0f);

		/// <summary>
		/// Returns the down vector. (0,-1)
		/// </summary>
		public static Vector2 Down => new Vector2(0.0f, -1.0f);

		/// <summary>
		/// Returns the right vector. (1,0)
		/// </summary>
		public static Vector2 Right => new Vector2(1.0f, 0.0f);

		/// <summary>
		/// Returns the left vector. (-1,0)
		/// </summary>
		public static Vector2 Left => new Vector2(-1.0f, 0.0f);

		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>The value of the X or Y component, depending on the index.</value>
		/// <param name="index">The index of the component to access. Use 0 for the X component and 1 for the Y component.</param>
		/// <returns>The value of the component at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 1].</exception>
		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return X;
					case 1: return Y;
				}

				throw new ArgumentOutOfRangeException("index", "Indices for Vector2 run from 0 to 1, inclusive.");
			}

			set
			{
				switch (index)
				{
					case 0: X = value; break;
					case 1: Y = value; break;
					default: throw new ArgumentOutOfRangeException("index", "Indices for Vector2 run from 0 to 1, inclusive.");
				}
			}
		}

		/// <summary>
		/// Calculates the length of the vector.
		/// </summary>
		/// <returns>The length of the vector.</returns>
		public float Length()
		{
			return (float)System.Math.Sqrt((X * X) + (Y * Y));
		}

		/// <summary>
		/// Calculates the squared length of the vector.
		/// </summary>
		/// <returns>The squared length of the vector.</returns>
		public float LengthSquared()
		{
			return (X * X) + (Y * Y);
		}

		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		void Normalize()
		{
			float length = Length();
			if (length == 0) return;

			float num = 1 / length;
			X *= num;
			Y *= num;
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="position">The second vector to calculate the distance to.</param>
		/// <returns>The distance to the other vector.</returns>
		float DistanceTo(Vector2 position)
		{
			return (position - this).Length();
		}
		/// <summary>
		/// Calculates the squared distance between two vectors.
		/// </summary>
		/// <param name="position">The second vector to calculate the squared distance to.</param>
		/// <returns>The squared distance to the other vector.</returns>
		float DistanceToSquared(Vector2 position)
		{
			return DistanceSquared(position, this);
		}
		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="position1">The first vector to calculate the distance to the second vector.</param>
		/// <param name="position2">The second vector to calculate the distance to the first vector.</param>
		/// <returns>The distance between the two vectors.</returns>
		public static float Distance(Vector2 position1, Vector2 position2)
		{
			return (position1 - position2).Length();
		}
		/// <summary>
		/// Calculates the squared distance between two vectors.
		/// </summary>
		/// <param name="position1">The first vector to calculate the squared distance to the second vector.</param>
		/// <param name="position2">The second vector to calculate the squared distance to the first vector.</param>
		/// <returns>The squared distance between the two vectors.</returns>
		public static float DistanceSquared(Vector2 position1, Vector2 position2)
		{
			return (position1 - position2).LengthSquared();
		}

		/// <summary>
		/// Returns the angle in degrees between from and to.
		/// The angle returned is always the acute angle between the two vectors.
		/// </summary>
		public static float Angle(Vector2 from, Vector2 to)
		{
			return System.Math.Abs(SignedAngle(from, to));
		}
		/// <summary>
		/// Returns the signed angle in degrees between from and to.
		/// </summary>
		public static float SignedAngle(Vector2 from, Vector2 to)
		{
			return (float)((System.Math.Atan2(to.Y, to.X) - System.Math.Atan2(from.Y, from.X)) * (180.0 / System.Math.PI));
		}
		/// <summary>
		/// Converts a vector to a heading.
		/// </summary>
		float ToHeading()
		{
			return (float)((System.Math.Atan2(X, -Y) + System.Math.PI) * (180.0 / System.Math.PI));
		}

		/// <summary>
		/// Returns a new normalized vector with random X and Y components.
		/// </summary>
		public static Vector2 RandomXY()
		{
			Vector2 v;
			double radian = Random.Instance.NextDouble() * 2 * System.Math.PI;
			v.X = (float)(System.Math.Cos(radian));
			v.Y = (float)(System.Math.Sin(radian));
			v.Normalize();
			return v;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="left">The first vector to add.</param>
		/// <param name="right">The second vector to add.</param>
		/// <returns>The sum of the two vectors.</returns>
		public static Vector2 Add(Vector2 left, Vector2 right) => new Vector2(left.X + right.X, left.Y + right.Y);

		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="left">The first vector to subtract.</param>
		/// <param name="right">The second vector to subtract.</param>
		/// <returns>The difference of the two vectors.</returns>
		public static Vector2 Subtract(Vector2 left, Vector2 right) => new Vector2(left.X - right.X, left.Y - right.Y);

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector2 Multiply(Vector2 value, float scale) => new Vector2(value.X * scale, value.Y * scale);

		/// <summary>
		/// Multiplies a vector with another by performing component-wise multiplication.
		/// </summary>
		/// <param name="left">The first vector to multiply.</param>
		/// <param name="right">The second vector to multiply.</param>
		/// <returns>The multiplied vector.</returns>
		public static Vector2 Multiply(Vector2 left, Vector2 right) => new Vector2(left.X * right.X, left.Y * right.Y);

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector2 Divide(Vector2 value, float scale) => new Vector2(value.X / scale, value.Y / scale);

		/// <summary>
		/// Reverses the direction of a given vector.
		/// </summary>
		/// <param name="value">The vector to negate.</param>
		/// <returns>A vector facing in the opposite direction.</returns>
		public static Vector2 Negate(Vector2 value) => new Vector2(-value.X, -value.Y);

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>The clamped value.</returns>
		public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
		{
			float x = value.X;
			x = (x > max.X) ? max.X : x;
			x = (x < min.X) ? min.X : x;

			float y = value.Y;
			y = (y > max.Y) ? max.Y : y;
			y = (y < min.Y) ? min.Y : y;

			return new Vector2(x, y);
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
		public static Vector2 Lerp(Vector2 start, Vector2 end, float amount)
		{
			Vector2 vector;

			vector.X = start.X + ((end.X - start.X) * amount);
			vector.Y = start.Y + ((end.Y - start.Y) * amount);

			return vector;
		}

		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		/// <param name="vector">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
		public static Vector2 Normalize(Vector2 vector)
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
		public static float Dot(Vector2 left, Vector2 right) => (left.X * right.X + left.Y * right.Y);

		/// <summary>
		/// Returns the reflection of a vector off a surface that has the specified normal. 
		/// </summary>
		/// <param name="vector">The source vector.</param>
		/// <param name="normal">Normal of the surface.</param>
		/// <returns>The reflected vector.</returns>
		/// <remarks>Reflect only gives the direction of a reflection off a surface, it does not determine 
		/// whether the original vector was close enough to the surface to hit it.</remarks>
		public static Vector2 Reflect(Vector2 vector, Vector2 normal)
		{
			Vector2 result;
			float dot = ((vector.X * normal.X) + (vector.Y * normal.Y));

			result.X = vector.X - ((2.0f * dot) * normal.X);
			result.Y = vector.Y - ((2.0f * dot) * normal.Y);

			return result;
		}

		/// <summary>
		/// Returns a vector containing the smallest components of the specified vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>A vector containing the smallest components of the source vectors.</returns>
		public static Vector2 Minimize(Vector2 left, Vector2 right)
		{
			Vector2 vector;
			vector.X = (left.X < right.X) ? left.X : right.X;
			vector.Y = (left.Y < right.Y) ? left.Y : right.Y;
			return vector;
		}
		/// <summary>
		/// Returns a vector containing the largest components of the specified vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>A vector containing the largest components of the source vectors.</returns>
		public static Vector2 Maximize(Vector2 left, Vector2 right)
		{
			Vector2 vector;
			vector.X = (left.X > right.X) ? left.X : right.X;
			vector.Y = (left.Y > right.Y) ? left.Y : right.Y;
			return vector;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="left">The first vector to add.</param>
		/// <param name="right">The second vector to add.</param>
		/// <returns>The sum of the two vectors.</returns>
		public static Vector2 operator +(Vector2 left, Vector2 right) => new Vector2(left.X + right.X, left.Y + right.Y);

		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="left">The first vector to subtract.</param>
		/// <param name="right">The second vector to subtract.</param>
		/// <returns>The difference of the two vectors.</returns>
		public static Vector2 operator -(Vector2 left, Vector2 right) => new Vector2(left.X - right.X, left.Y - right.Y);

		/// <summary>
		/// Reverses the direction of a given vector.
		/// </summary>
		/// <param name="value">The vector to negate.</param>
		/// <returns>A vector facing in the opposite direction.</returns>
		public static Vector2 operator -(Vector2 value) => new Vector2(-value.X, -value.Y);

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="vector">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector2 operator *(Vector2 vector, float scale) => new Vector2(vector.X * scale, vector.Y * scale);

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="vector">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector2 operator *(float scale, Vector2 vector) => new Vector2(vector.X * scale, vector.Y * scale);

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="vector">The vector to scale.</param>public public static 
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector2 operator /(Vector2 vector, float scale) => new Vector2(vector.X / scale, vector.Y / scale);

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Vector2 left, Vector2 right) => Equals(left, right);

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(Vector2 left, Vector2 right) => !Equals(left, right);

		/// <summary>
		/// Converts the value of the object to its equivalent string representation.
		/// </summary>
		/// <returns>The string representation of the value of this instance.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", X, Y);
		}

		/// <summary>
		/// Converts the value of the object to its equivalent string representation.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>The string representation of the value of this instance.</returns>
		public string ToString(string format)
		{
			if (format == null)
				return ToString();

			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", X.ToString(format, CultureInfo.CurrentCulture), Y.ToString(format, CultureInfo.CurrentCulture));
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (X.GetHashCode() * 397) ^ Y.GetHashCode();
			}
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object. 
		/// </summary>
		/// <param name="obj">Object to make the comparison with.</param>
		/// <returns><c>true</c> if the current instance is equal to the specified object; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
				return false;

			return Equals((Vector2)(obj));
		}
		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to the specified object. 
		/// </summary>
		/// <param name="other">Object to make the comparison with.</param>
		/// <returns><c>true</c> if the current instance is equal to the specified object; <c>false</c> otherwise.</returns>
		public bool Equals(Vector2 other) => (X == other.X && Y == other.Y);
	}
}