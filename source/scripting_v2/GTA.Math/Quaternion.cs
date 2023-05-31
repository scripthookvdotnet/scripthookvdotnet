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
using System.Globalization;
using System.Runtime.InteropServices;

namespace GTA.Math
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Quaternion : IEquatable<Quaternion>
	{
		/// <summary>
		/// Gets or sets the X component of the quaternion.
		/// </summary>
		/// <value>The X component of the quaternion.</value>
		public float X;

		/// <summary>
		/// Gets or sets the Y component of the quaternion.
		/// </summary>
		/// <value>The Y component of the quaternion.</value>
		public float Y;

		/// <summary>
		/// Gets or sets the Z component of the quaternion.
		/// </summary>
		/// <value>The Z component of the quaternion.</value>
		public float Z;

		/// <summary>
		/// Gets or sets the W component of the quaternion.
		/// </summary>
		/// <value>The W component of the quaternion.</value>
		public float W;

		/// <summary>
		/// Initializes a new instance of the <see cref="Quaternion"/> structure.
		/// </summary>
		/// <param name="x">The X component of the quaternion.</param>
		/// <param name="y">The Y component of the quaternion.</param>
		/// <param name="z">The Z component of the quaternion.</param>
		/// <param name="w">The W component of the quaternion.</param>
		public Quaternion(float x, float y, float z, float w) : this()
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Quaternion"/> structure.
		/// </summary>
		/// <param name="axis">The axis of rotation.</param>
		/// <param name="angle">The angle of rotation in radians.</param>
		public Quaternion(Vector3 axis, float angle) : this()
		{
			axis = Vector3.Normalize(axis);

			float half = angle * 0.5f;
			float sin = (float)(System.Math.Sin((double)(half)));
			float cos = (float)(System.Math.Cos((double)(half)));

			X = axis.X * sin;
			Y = axis.Y * sin;
			Z = axis.Z * sin;
			W = cos;
		}

		/// <summary>
		/// The identity <see cref="Quaternion"/> (0, 0, 0, 1).
		/// </summary>
		public static Quaternion Identity => new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

		/// <summary>
		/// Gets the axis components of the quaternion.
		/// </summary>
		public Vector3 Axis
		{
			get
			{
				Vector3 axis;
				float length = Length();

				if (length != 0.0f)
				{
					axis.X = X / length;
					axis.Y = Y / length;
					axis.Z = Z / length;
				}
				else
				{
					axis.X = 1.0f;
					axis.Y = 0.0f;
					axis.Z = 0.0f;
				}

				return axis;
			}
		}

		/// <summary>
		/// Gets the angle of the quaternion.
		/// </summary>
		public float Angle => ((System.Math.Abs(W) <= 1.0f) ? 2.0f * (float)(System.Math.Acos(W)) : 0.0f);

		/// <summary>
		/// Calculates the length of the quaternion.
		/// </summary>
		/// <returns>The length of the quaternion.</returns>
		public float Length() => (float)System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));

		/// <summary>
		/// Calculates the squared length of the quaternion.
		/// </summary>
		/// <returns>The squared length of the quaternion.</returns>
		public float LengthSquared() => (X * X) + (Y * Y) + (Z * Z) + (W * W);

		/// <summary>
		/// Converts the quaternion into a unit quaternion.
		/// </summary>
		public void Normalize()
		{
			float length = 1.0f / Length();
			X *= length;
			Y *= length;
			Z *= length;
			W *= length;
		}

		/// <summary>
		/// Conjugates the quaternion.
		/// </summary>
		public void Conjugate()
		{
			X = -X;
			Y = -Y;
			Z = -Z;
		}

		/// <summary>
		/// Conjugates and renormalizes the quaternion.
		/// </summary>
		public void Invert()
		{
			float lengthSq = 1.0f / LengthSquared();
			X = -X * lengthSq;
			Y = -Y * lengthSq;
			Z = -Z * lengthSq;
			W = W * lengthSq;
		}

		/// <summary>
		/// Reverses the direction of a given quaternion.
		/// </summary>
		/// <param name="quaternion">The quaternion to negate.</param>
		/// <returns>A quaternion facing in the opposite direction.</returns>
		public static Quaternion Negate(Quaternion quaternion)
		{
			var result = new Quaternion();
			result.X = -quaternion.X;
			result.Y = -quaternion.Y;
			result.Z = -quaternion.Z;
			result.W = -quaternion.W;
			return result;
		}

		/// <summary>
		/// Adds two quaternions.
		/// </summary>
		/// <param name="left">The first quaternion to add.</param>
		/// <param name="right">The second quaternion to add.</param>
		/// <returns>The sum of the two quaternions.</returns>
		public static Quaternion Add(Quaternion left, Quaternion right)
		{
			var result = new Quaternion();
			result.X = left.X + right.X;
			result.Y = left.Y + right.Y;
			result.Z = left.Z + right.Z;
			result.W = left.W + right.W;
			return result;
		}

		/// <summary>
		/// Subtracts two quaternions.
		/// </summary>
		/// <param name="left">The first quaternion to subtract.</param>
		/// <param name="right">The second quaternion to subtract.</param>
		/// <returns>The difference of the two quaternions.</returns>
		public static Quaternion Subtract(Quaternion left, Quaternion right)
		{
			var result = new Quaternion();
			result.X = left.X - right.X;
			result.Y = left.Y - right.Y;
			result.Z = left.Z - right.Z;
			result.W = left.W - right.W;
			return result;
		}

		/// <summary>
		/// Modulates a quaternion by another.
		/// </summary>
		/// <param name="left">The first quaternion to modulate.</param>
		/// <param name="right">The second quaternion to modulate.</param>
		/// <returns>The modulated quaternion.</returns>
		public static Quaternion Multiply(Quaternion left, Quaternion right)
		{
			Quaternion quaternion;
			float lx = left.X;
			float ly = left.Y;
			float lz = left.Z;
			float lw = left.W;
			float rx = right.X;
			float ry = right.Y;
			float rz = right.Z;
			float rw = right.W;

			quaternion.X = (lx * rw + rx * lw) + (ly * rz) - (lz * ry);
			quaternion.Y = (ly * rw + ry * lw) + (lz * rx) - (lx * rz);
			quaternion.Z = (lz * rw + rz * lw) + (lx * ry) - (ly * rx);
			quaternion.W = (lw * rw) - (lx * rx + ly * ry + lz * rz);

			return quaternion;
		}

		/// <summary>
		/// Scales a quaternion by the given value.
		/// </summary>
		/// <param name="quaternion">The quaternion to scale.</param>
		/// <param name="scale">The amount by which to scale the quaternion.</param>
		/// <returns>The scaled quaternion.</returns>
		public static Quaternion Multiply(Quaternion quaternion, float scale)
		{
			var result = new Quaternion();
			result.X = quaternion.X * scale;
			result.Y = quaternion.Y * scale;
			result.Z = quaternion.Z * scale;
			result.W = quaternion.W * scale;
			return result;
		}

		/// <summary>
		/// Divides a quaternion by another.
		/// </summary>
		/// <param name="left">The first quaternion to divide.</param>
		/// <param name="right">The second quaternion to divide.</param>
		/// <returns>The divided quaternion.</returns>
		public static Quaternion Divide(Quaternion left, Quaternion right)
		{
			return Multiply(left, Invert(right));
		}

		/// <summary>
		/// Converts the quaternion into a unit quaternion.
		/// </summary>
		/// <param name="quaternion">The quaternion to normalize.</param>
		/// <returns>The normalized quaternion.</returns>
		public static Quaternion Normalize(Quaternion quaternion)
		{
			quaternion.Normalize();
			return quaternion;
		}

		/// <summary>
		/// Creates the conjugate of a specified Quaternion.
		/// </summary>
		/// <param name="value">The Quaternion of which to return the conjugate.</param>
		/// <returns>A new Quaternion that is the conjugate of the specified one.</returns>
		public static Quaternion Conjugate(Quaternion value)
		{
			Quaternion ans;

			ans.X = -value.X;
			ans.Y = -value.Y;
			ans.Z = -value.Z;
			ans.W = value.W;

			return ans;
		}

		/// <summary>
		/// Conjugates and renormalizes the quaternion.
		/// </summary>
		/// <param name="quaternion">The quaternion to conjugate and re-normalize.</param>
		/// <returns>The conjugated and renormalized quaternion.</returns>
		public static Quaternion Invert(Quaternion quaternion)
		{
			var result = new Quaternion();
			float lengthSq = 1.0f / ((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y) + (quaternion.Z * quaternion.Z) + (quaternion.W * quaternion.W));

			result.X = -quaternion.X * lengthSq;
			result.Y = -quaternion.Y * lengthSq;
			result.Z = -quaternion.Z * lengthSq;
			result.W = quaternion.W * lengthSq;

			return result;
		}

		/// <summary>
		/// Calculates the dot product of two quaternions.
		/// </summary>
		/// <param name="left">First source quaternion.</param>
		/// <param name="right">Second source quaternion.</param>
		/// <returns>The dot product of the two quaternions.</returns>
		public static float Dot(Quaternion left, Quaternion right) => (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);

		/// <summary>
		/// Performs a linear interpolation between two quaternion.
		/// </summary>
		/// <param name="start">Start quaternion.</param>
		/// <param name="end">End quaternion.</param>
		/// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
		/// <returns>The linear interpolation of the two quaternions.</returns>
		/// <remarks>
		/// This method performs the linear interpolation based on the following formula.
		/// <code>start + (end - start) * amount</code>
		/// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned.
		/// </remarks>
		public static Quaternion Lerp(Quaternion start, Quaternion end, float amount)
		{
			var result = new Quaternion();
			float inverse = 1.0f - amount;
			float dot = (start.X * end.X) + (start.Y * end.Y) + (start.Z * end.Z) + (start.W * end.W);

			if (dot >= 0.0f)
			{
				result.X = (inverse * start.X) + (amount * end.X);
				result.Y = (inverse * start.Y) + (amount * end.Y);
				result.Z = (inverse * start.Z) + (amount * end.Z);
				result.W = (inverse * start.W) + (amount * end.W);
			}
			else
			{
				result.X = (inverse * start.X) - (amount * end.X);
				result.Y = (inverse * start.Y) - (amount * end.Y);
				result.Z = (inverse * start.Z) - (amount * end.Z);
				result.W = (inverse * start.W) - (amount * end.W);
			}

			float invLength = 1.0f / result.Length();

			result.X *= invLength;
			result.Y *= invLength;
			result.Z *= invLength;
			result.W *= invLength;

			return result;
		}

		/// <summary>
		/// Interpolates between two quaternions, using spherical linear interpolation..
		/// </summary>
		/// <param name="start">Start quaternion.</param>
		/// <param name="end">End quaternion.</param>
		/// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
		/// <returns>The spherical linear interpolation of the two quaternions.</returns>
		public static Quaternion Slerp(Quaternion start, Quaternion end, float amount)
		{
			var result = new Quaternion();
			float kEpsilon = (float)(1.192093E-07);
			float opposite;
			float inverse;
			float dot = Dot(start, end);

			if (System.Math.Abs(dot) > (1.0f - kEpsilon))
			{
				inverse = 1.0f - amount;
				opposite = amount * System.Math.Sign(dot);
			}
			else
			{
				float aCos = (float)System.Math.Acos(System.Math.Abs(dot));
				float invSin = (float)(1.0 / System.Math.Sin(aCos));

				inverse = (float)(System.Math.Sin((1.0f - amount) * aCos) * invSin);
				opposite = (float)(System.Math.Sin(amount * aCos) * invSin * System.Math.Sign(dot));
			}

			result.X = (inverse * start.X) + (opposite * end.X);
			result.Y = (inverse * start.Y) + (opposite * end.Y);
			result.Z = (inverse * start.Z) + (opposite * end.Z);
			result.W = (inverse * start.W) + (opposite * end.W);

			return result;
		}

		/// <summary>
		/// Interpolates between two quaternions, using spherical linear interpolation. The parameter /t/ is not clamped.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t"></param>
		public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
		{
			if (a.LengthSquared() == 0.0f)
			{
				if (b.LengthSquared() == 0.0f)
				{
					return Identity;
				}
				return b;
			}
			else if (b.LengthSquared() == 0.0f)
			{
				return a;
			}


			float cosHalfAngle = a.W * b.W + Vector3.Dot(a.Axis, b.Axis);

			if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
			{
				return a;
			}

			if (cosHalfAngle < 0.0f)
			{
				b.X = -b.X;
				b.Y = -b.Y;
				b.Z = -b.Z;
				b.W = -b.W;
				cosHalfAngle = -cosHalfAngle;
			}

			float blendA;
			float blendB;
			if (cosHalfAngle < 0.99f)
			{
				float halfAngle = (float)System.Math.Acos(cosHalfAngle);
				float sinHalfAngle = (float)System.Math.Sin(halfAngle);
				float oneOverSinHalfAngle = 1.0f / sinHalfAngle;
				blendA = (float)System.Math.Sin(halfAngle * (1.0f - t)) * oneOverSinHalfAngle;
				blendB = (float)System.Math.Sin(halfAngle * t) * oneOverSinHalfAngle;
			}
			else
			{
				blendA = 1.0f - t;
				blendB = t;
			}

			var result = new Quaternion(blendA * a.Axis + blendB * b.Axis, blendA * a.W + blendB * b.W);
			return result.LengthSquared() > 0.0f ? Normalize(result) : Identity;
		}

		/// <summary>
		/// Creates a rotation which rotates from fromDirection to toDirection.
		/// </summary>
		public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
		{
			float NormAB = (float)(System.Math.Sqrt(fromDirection.LengthSquared() * fromDirection.LengthSquared()));

			float w = NormAB + Vector3.Dot(fromDirection, toDirection);
			Quaternion result;

			if (w >= 1e-6f * NormAB)
			{
				result = new Quaternion(Vector3.Cross(fromDirection, toDirection), w);
			}
			else
			{
				w = 0.0f;
				result = System.Math.Abs(fromDirection.X) > System.Math.Abs(fromDirection.Y)
					? new Quaternion(-fromDirection.Z, 0.0f, fromDirection.X, w)
					: new Quaternion(0.0f, -fromDirection.Z, fromDirection.Y, w);
			}

			result.Normalize();
			return result;
		}

		/// <summary>
		/// Rotates a rotation from towards to.
		/// </summary>
		/// <param name="from">From Quaternion.</param>
		/// <param name="to">To Quaternion.</param>
		/// <param name ="maxDegreesDelta"></param>
		public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
		{
			float angle = AngleBetween(from, to);
			if (angle == 0.0f)
			{
				return to;
			}
			float t = System.Math.Min(1.0f, maxDegreesDelta / angle);
			return SlerpUnclamped(from, to, t);
		}

		/// <summary>
		/// Returns the angle in degrees between two rotations a and b.
		/// </summary>
		/// <param name="a">The first quaternion to calculate angle.</param>
		/// <param name="b">The second quaternion to calculate angle.</param>
		/// <returns>The angle in degrees between two rotations a and b.</returns>
		public static float AngleBetween(Quaternion a, Quaternion b)
		{
			float dot = Dot(a, b);
			return (float)((System.Math.Acos(System.Math.Min(System.Math.Abs(dot), 1.0f)) * 2.0 * (180.0f / System.Math.PI)));
		}

		/// <summary>
		/// Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
		/// </summary>
		/// <param name="zaxis">Z degrees.</param>
		/// <param name ="xaxis">X degrees.</param>
		/// <param name ="yaxis">Y degrees.</param>
		public static Quaternion Euler(float zaxis, float xaxis, float yaxis)
		{
			float Deg2Rad = (float)((System.Math.PI / 180.0));
			return RotationYawPitchRoll(zaxis * Deg2Rad, xaxis * Deg2Rad, yaxis * Deg2Rad);
		}

		/// <summary>
		/// Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
		/// </summary>
		/// <param name="euler">Euler angles in degrees. euler.X = around X axis, euler.Y = around Y axis, euler.Z = around Z axis</param>
		public static Quaternion Euler(Vector3 euler)
		{
			Vector3 eulerRad = euler * (float)((System.Math.PI / 180.0));
			return RotationYawPitchRoll(eulerRad.Z, eulerRad.X, eulerRad.Y);
		}

		/// <summary>
		/// Creates a quaternion given a rotation and an axis.
		/// </summary>
		/// <param name="axis">The axis of rotation.</param>
		/// <param name="angle">The angle of rotation in radians.</param>
		/// <returns>The newly created quaternion.</returns>
		public static Quaternion RotationAxis(Vector3 axis, float angle)
		{
			var result = new Quaternion();

			axis = Vector3.Normalize(axis);

			float half = angle * 0.5f;
			float sin = (float)(System.Math.Sin((double)(half)));
			float cos = (float)(System.Math.Cos((double)(half)));

			result.X = axis.X * sin;
			result.Y = axis.Y * sin;
			result.Z = axis.Z * sin;
			result.W = cos;

			return result;
		}

		/// <summary>
		/// Creates a quaternion given a rotation matrix.
		/// Will NOT work correctly if there is scaling in the matrix.
		/// </summary>
		/// <param name="matrix">The rotation matrix.</param>
		/// <returns>The newly created quaternion.</returns>
		public static Quaternion RotationMatrix(Matrix matrix)
		{
			var result = new Quaternion();
			float sqrt;
			float half;
			float scale = matrix.M11 + matrix.M22 + matrix.M33;

			if (scale > 0.0f)
			{
				sqrt = (float)System.Math.Sqrt(scale + 1.0f);
				result.W = sqrt * 0.5f;
				sqrt = 0.5f / sqrt;

				result.X = (matrix.M23 - matrix.M32) * sqrt;
				result.Y = (matrix.M31 - matrix.M13) * sqrt;
				result.Z = (matrix.M12 - matrix.M21) * sqrt;
			}
			else if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
			{
				sqrt = (float)System.Math.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
				half = 0.5f / sqrt;

				result.X = 0.5f * sqrt;
				result.Y = (matrix.M12 + matrix.M21) * half;
				result.Z = (matrix.M13 + matrix.M31) * half;
				result.W = (matrix.M23 - matrix.M32) * half;
			}
			else if (matrix.M22 > matrix.M33)
			{
				sqrt = (float)System.Math.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
				half = 0.5f / sqrt;

				result.X = (matrix.M21 + matrix.M12) * half;
				result.Y = 0.5f * sqrt;
				result.Z = (matrix.M32 + matrix.M23) * half;
				result.W = (matrix.M31 - matrix.M13) * half;
			}
			else
			{
				sqrt = (float)System.Math.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
				half = 0.5f / sqrt;

				result.X = (matrix.M31 + matrix.M13) * half;
				result.Y = (matrix.M32 + matrix.M23) * half;
				result.Z = 0.5f * sqrt;
				result.W = (matrix.M12 - matrix.M21) * half;
			}

			return result;
		}

		/// <summary>
		/// Creates a Quaternion from the given yaw, pitch, and roll, in radians.
		/// </summary>
		/// <param name="yaw">The yaw angle, in radians, around the Z-axis.</param>
		/// <param name="pitch">The pitch angle, in radians, around the X-axis.</param>
		/// <param name="roll">The roll angle, in radians, around the Y-axis.</param>
		/// <returns>The newly created quaternion.</returns>
		public static Quaternion RotationYawPitchRoll(float yaw, float pitch, float roll)
		{
			var result = new Quaternion();

			float halfYaw = yaw * 0.5f;
			float sinYaw = (float)(System.Math.Sin((double)(halfYaw)));
			float cosYaw = (float)(System.Math.Cos((double)(halfYaw)));
			float halfPitch = pitch * 0.5f;
			float sinPitch = (float)(System.Math.Sin((double)(halfPitch)));
			float cosPitch = (float)(System.Math.Cos((double)(halfPitch)));
			float halfRoll = roll * 0.5f;
			float sinRoll = (float)(System.Math.Sin((double)(halfRoll)));
			float cosRoll = (float)(System.Math.Cos((double)(halfRoll)));

			result.X = (cosRoll * sinPitch * cosYaw) + (sinRoll * cosPitch * sinYaw);
			result.Y = (sinRoll * cosPitch * cosYaw) - (cosRoll * sinPitch * sinYaw);
			result.Z = (cosRoll * cosPitch * sinYaw) - (sinRoll * sinPitch * cosYaw);
			result.W = (cosRoll * cosPitch * cosYaw) + (sinRoll * sinPitch * sinYaw);

			return result;
		}

		/// <summary>
		/// Reverses the direction of a given quaternion.
		/// </summary>
		/// <param name="quaternion">The quaternion to negate.</param>
		/// <returns>A quaternion facing in the opposite direction.</returns>
		public static Quaternion operator -(Quaternion quaternion)
		{
			var result = new Quaternion();
			result.X = -quaternion.X;
			result.Y = -quaternion.Y;
			result.Z = -quaternion.Z;
			result.W = -quaternion.W;
			return result;
		}

		/// <summary>
		/// Adds two quaternions.
		/// </summary>
		/// <param name="left">The first quaternion to add.</param>
		/// <param name="right">The second quaternion to add.</param>
		/// <returns>The sum of the two quaternions.</returns>
		public static Quaternion operator +(Quaternion left, Quaternion right)
		{
			var result = new Quaternion();
			result.X = left.X + right.X;
			result.Y = left.Y + right.Y;
			result.Z = left.Z + right.Z;
			result.W = left.W + right.W;
			return result;
		}

		/// <summary>
		/// Subtracts two quaternions.
		/// </summary>
		/// <param name="left">The first quaternion to subtract.</param>
		/// <param name="right">The second quaternion to subtract.</param>
		/// <returns>The difference of the two quaternions.</returns>
		public static Quaternion operator -(Quaternion left, Quaternion right)
		{
			var result = new Quaternion();
			result.X = left.X - right.X;
			result.Y = left.Y - right.Y;
			result.Z = left.Z - right.Z;
			result.W = left.W - right.W;
			return result;
		}

		/// <summary>
		/// Multiplies a quaternion by another.
		/// </summary>
		/// <param name="left">The first quaternion to multiply.</param>
		/// <param name="right">The second quaternion to multiply.</param>
		/// <returns>The multiplied quaternion.</returns>
		public static Quaternion operator *(Quaternion left, Quaternion right)
		{
			var quaternion = new Quaternion();
			float lx = left.X;
			float ly = left.Y;
			float lz = left.Z;
			float lw = left.W;
			float rx = right.X;
			float ry = right.Y;
			float rz = right.Z;
			float rw = right.W;

			quaternion.X = (lx * rw + rx * lw) + (ly * rz) - (lz * ry);
			quaternion.Y = (ly * rw + ry * lw) + (lz * rx) - (lx * rz);
			quaternion.Z = (lz * rw + rz * lw) + (lx * ry) - (ly * rx);
			quaternion.W = (lw * rw) - (lx * rx + ly * ry + lz * rz);

			return quaternion;
		}

		/// <summary>
		/// Rotates a point using a quaternion.
		/// </summary>
		/// <param name="rotation">The quaternion to rotate the point with.</param>
		/// <param name="point">The point to rotate.</param>
		/// <returns>The rotated point coordinates.</returns>
		public static Vector3 operator *(Quaternion rotation, Vector3 point)
		{
			var q = new Vector3(rotation.X, rotation.Y, rotation.Z);
			Vector3 t = 2.0f * Vector3.Cross(q, point);
			Vector3 result = point + (rotation.W * t) + Vector3.Cross(q, t);
			return result;
		}

		/// <summary>
		/// Scales a quaternion by the given value.
		/// </summary>
		/// <param name="quaternion">The quaternion to scale.</param>
		/// <param name="scale">The amount by which to scale the quaternion.</param>
		/// <returns>The scaled quaternion.</returns>
		public static Quaternion operator *(Quaternion quaternion, float scale)
		{
			var result = new Quaternion();
			result.X = quaternion.X * scale;
			result.Y = quaternion.Y * scale;
			result.Z = quaternion.Z * scale;
			result.W = quaternion.W * scale;
			return result;
		}

		/// <summary>
		/// Scales a quaternion by the given value.
		/// </summary>
		/// <param name="quaternion">The quaternion to scale.</param>
		/// <param name="scale">The amount by which to scale the quaternion.</param>
		/// <returns>The scaled quaternion.</returns>
		public static Quaternion operator *(float scale, Quaternion quaternion)
		{
			var result = new Quaternion();
			result.X = quaternion.X * scale;
			result.Y = quaternion.Y * scale;
			result.Z = quaternion.Z * scale;
			result.W = quaternion.W * scale;
			return result;
		}

		/// <summary>
		/// Divides a Quaternion by another Quaternion.
		/// </summary>
		/// <param name="left">The source Quaternion.</param>
		/// <param name="right">The divisor.</param>
		/// <returns>The result of the division.</returns>
		public static Quaternion operator /(Quaternion left, Quaternion right)
		{
			var quaternion = new Quaternion();

			float lx = left.X;
			float ly = left.Y;
			float lz = left.Z;
			float lw = left.W;

			// Inverse part.
			float ls = right.X * right.X + right.Y * right.Y +
			           right.Z * right.Z + right.W * right.W;
			float invNorm = 1.0f / ls;

			float rx = -right.X * invNorm;
			float ry = -right.Y * invNorm;
			float rz = -right.Z * invNorm;
			float rw = right.W * invNorm;

			// Multiply part.
			quaternion.X = (lx * rw + rx * lw) + (ly * rz) - (lz * ry);
			quaternion.Y = (ly * rw + ry * lw) + (lz * rx) - (lx * rz);
			quaternion.Z = (lz * rw + rz * lw) + (lx * ry) - (ly * rx);
			quaternion.W = (lw * rw) - (lx * rx + ly * ry + lz * rz);

			return quaternion;
		}

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Quaternion left, Quaternion right) => left.Equals(right);

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(Quaternion left, Quaternion right) => !left.Equals(right);

		/// <summary>
		/// Converts the value of the object to its equivalent string representation.
		/// </summary>
		/// <returns>The string representation of the value of this instance.</returns>
		public override string ToString()
		{
			return $"X:{X.ToString()} Y:{Y.ToString()} Z:{Z.ToString()} W:{W.ToString()}";
		}

		/// <summary>
		/// Converts the value of the object to its equivalent string representation.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>The string representation of the value of this instance.</returns>
		public string ToString(string format)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return $"X:{X.ToString(format, invariantCulture)} Y:{Y.ToString(format, invariantCulture)} Z:{Z.ToString(format, invariantCulture)} W:{W.ToString(format, invariantCulture)}";
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();

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

			return Equals((Quaternion)(obj));
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to the specified object.
		/// </summary>
		/// <param name="other">Object to make the comparison with.</param>
		/// <returns><see langword="true" /> if the current instance is equal to the specified object; <see langword="false" /> otherwise.</returns>
		public bool Equals(Quaternion other) => (X == other.X && Y == other.Y && Z == other.Z && W == other.W);

		/// <summary>
		/// Determines whether the specified object instances are considered equal.
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns><see langword="true" /> if <paramref name="value1"/> is the same instance as <paramref name="value2"/> or
		/// if both are <see langword="null" /> references or if <c>value1.Equals(value2)</c> returns <see langword="true" />; otherwise, <see langword="false" />.</returns>
		public static bool Equals(ref Quaternion value1, ref Quaternion value2) => value1.Equals(value2);
	}
}
