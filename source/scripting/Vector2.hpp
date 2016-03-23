/**
* Copyright (C) 2007-2010 SlimDX Group
*
* Permission is hereby granted, free  of charge, to any person obtaining a copy of this software  and
* associated  documentation  files (the  "Software"), to deal  in the Software  without  restriction,
* including  without  limitation  the  rights  to use,  copy,  modify,  merge,  publish,  distribute,
* sublicense, and/or sell  copies of the  Software,  and to permit  persons to whom  the Software  is
* furnished to do so, subject to the following conditions:
*
* The  above  copyright  notice  and this  permission  notice shall  be included  in  all  copies  or
* substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS",  WITHOUT WARRANTY OF  ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
* NOT  LIMITED  TO  THE  WARRANTIES  OF  MERCHANTABILITY,  FITNESS  FOR  A   PARTICULAR  PURPOSE  AND
* NONINFRINGEMENT.  IN  NO  EVENT SHALL THE  AUTHORS  OR COPYRIGHT HOLDERS  BE LIABLE FOR  ANY CLAIM,
* DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,  OUT
* OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#pragma once

namespace GTA
{
	namespace Math
	{
		[System::SerializableAttribute]
		[System::Runtime::InteropServices::StructLayout(System::Runtime::InteropServices::LayoutKind::Sequential, Pack = 4)]
		public value class Vector2 : System::IEquatable<Vector2>
		{
		public:
			/// <summary>
			/// Gets or sets the X component of the vector.
			/// </summary>
			/// <value>The X component of the vector.</value>
			float X;

			/// <summary>
			/// Gets or sets the Y component of the vector.
			/// </summary>
			/// <value>The Y component of the vector.</value>
			float Y;

			/// <summary>
			/// Initializes a new instance of the <see cref="Vector2"/> class.
			/// </summary>
			/// <param name="x">Initial value for the X component of the vector.</param>
			/// <param name="y">Initial value for the Y component of the vector.</param>
			Vector2(float x, float y);

			/// <summary>
			/// Returns this vector with a magnitude of 1.
			/// </summary>
			property Vector2 Normalized
			{
				Vector2 get();
			}

			/// <summary>
			/// Returns a null vector. (0,0)
			/// </summary>
			static property Vector2 Zero
			{
				Vector2 get()
				{
					return Vector2(0.0f, 0.0f);
				}
			}

			/// <summary>
			/// Returns the up vector. (0,1)
			/// </summary>
			static property Vector2 Up
			{
				Vector2 get()
				{
					return Vector2(0.0f, 1.0f);
				}
			}
			/// <summary>
			/// Returns the down vector. (0,-1)
			/// </summary>
			static property Vector2 Down
			{
				Vector2 get()
				{
					return Vector2(0.0f, -1.0f);
				}
			}
			/// <summary>
			/// Returns the right vector. (1,0)
			/// </summary>
			static property Vector2 Right
			{
				Vector2 get()
				{
					return Vector2(1.0f, 0.0f);
				}
			}
			/// <summary>
			/// Returns the left vector. (-1,0)
			/// </summary>
			static property Vector2 Left
			{
				Vector2 get()
				{
					return Vector2(-1.0f, 0.0f);
				}
			}

			property float default[int]
			{
				float get(int index);
				void set(int index, float value);
			}

			/// <summary>
			/// Calculates the length of the vector.
			/// </summary>
			/// <returns>The length of the vector.</returns>
			float Length();

			/// <summary>
			/// Calculates the squared length of the vector.
			/// </summary>
			/// <returns>The squared length of the vector.</returns>
			float LengthSquared();

			/// <summary>
			/// Converts the vector into a unit vector.
			/// </summary>
			void Normalize();

			/// <summary>
			/// Calculates the distance between two vectors.
			/// </summary>
			/// <param name="position">The second vector to calculate the distance to.</param>
			/// <returns>The distance to the other vector.</returns>
			float DistanceTo(Vector2 position);

			/// <summary>
			/// Calculates the squared distance between two vectors.
			/// </summary>
			/// <param name="position">The second vector to calculate the squared distance to.</param>
			/// <returns>The squared distance to the other vector.</returns>
			float DistanceToSquared(Vector2 position);

			/// <summary>
			/// Calculates the distance between two vectors.
			/// </summary>
			/// <param name="position2">The first vector to calculate the distance to the second vector.</param>
			/// <param name="position2">The second vector to calculate the distance to the first vector.</param>
			/// <returns>The distance between the two vectors.</returns>
			static float Distance(Vector2 position1, Vector2 position2);

			/// <summary>
			/// Calculates the squared distance between two vectors.
			/// </summary>
			/// <param name="position2">The first vector to calculate the squared distance to the second vector.</param>
			/// <param name="position2">The second vector to calculate the squared distance to the first vector.</param>
			/// <returns>The squared distance between the two vectors.</returns>
			static float DistanceSquared(Vector2 position1, Vector2 position2);

			/// <summary>
			/// Returns the angle in degrees between from and to.
			/// The angle returned is always the acute angle between the two vectors.
			/// </summary>
			static float Angle(Vector2 from, Vector2 to);

			/// <summary>
			/// Returns the signed angle in degrees between from and to.
			/// </summary>
			static float SignedAngle(Vector2 from, Vector2 to);

			/// <summary>
			/// Converts a vector to a heading.
			/// </summary>
			float ToHeading();

			/// <summary>
			/// Returns a new normalized vector with random X and Y components.
			/// </summary>
			static Vector2 RandomXY();

			/// <summary>
			/// Adds two vectors.
			/// </summary>
			/// <param name="left">The first vector to add.</param>
			/// <param name="right">The second vector to add.</param>
			/// <returns>The sum of the two vectors.</returns>
			static Vector2 Add(Vector2 left, Vector2 right);

			/// <summary>
			/// Subtracts two vectors.
			/// </summary>
			/// <param name="left">The first vector to subtract.</param>
			/// <param name="right">The second vector to subtract.</param>
			/// <returns>The difference of the two vectors.</returns>
			static Vector2 Subtract(Vector2 left, Vector2 right);

			/// <summary>
			/// Scales a vector by the given value.
			/// </summary>
			/// <param name="value">The vector to scale.</param>
			/// <param name="scale">The amount by which to scale the vector.</param>
			/// <returns>The scaled vector.</returns>
			static Vector2 Multiply(Vector2 value, float scale);

			/// <summary>
			/// Modulates a vector by another.
			/// </summary>
			/// <param name="left">The first vector to modulate.</param>
			/// <param name="right">The second vector to modulate.</param>
			/// <returns>The modulated vector.</returns>
			static Vector2 Modulate(Vector2 left, Vector2 right);

			/// <summary>
			/// Scales a vector by the given value.
			/// </summary>
			/// <param name="value">The vector to scale.</param>
			/// <param name="scale">The amount by which to scale the vector.</param>
			/// <returns>The scaled vector.</returns>
			static Vector2 Divide(Vector2 value, float scale);

			/// <summary>
			/// Reverses the direction of a given vector.
			/// </summary>
			/// <param name="value">The vector to negate.</param>
			/// <returns>A vector facing in the opposite direction.</returns>
			static Vector2 Negate(Vector2 value);

			/// <summary>
			/// Restricts a value to be within a specified range.
			/// </summary>
			/// <param name="value">The value to clamp.</param>
			/// <param name="min">The minimum value.</param>
			/// <param name="max">The maximum value.</param>
			/// <returns>The clamped value.</returns>
			static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max);

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
			static Vector2 Lerp(Vector2 start, Vector2 end, float amount);

			/// <summary>
			/// Converts the vector into a unit vector.
			/// </summary>
			/// <param name="vector">The vector to normalize.</param>
			/// <returns>The normalized vector.</returns>
			static Vector2 Normalize(Vector2 vector);

			/// <summary>
			/// Calculates the dot product of two vectors.
			/// </summary>
			/// <param name="left">First source vector.</param>
			/// <param name="right">Second source vector.</param>
			/// <returns>The dot product of the two vectors.</returns>
			static float Dot(Vector2 left, Vector2 right);

			/// <summary>
			/// Returns the reflection of a vector off a surface that has the specified normal. 
			/// </summary>
			/// <param name="vector">The source vector.</param>
			/// <param name="normal">Normal of the surface.</param>
			/// <returns>The reflected vector.</returns>
			/// <remarks>Reflect only gives the direction of a reflection off a surface, it does not determine 
			/// whether the original vector was close enough to the surface to hit it.</remarks>
			static Vector2 Reflect(Vector2 vector, Vector2 normal);

			/// <summary>
			/// Returns a vector containing the smallest components of the specified vectors.
			/// </summary>
			/// <param name="value1">The first source vector.</param>
			/// <param name="value2">The second source vector.</param>
			/// <returns>A vector containing the smallest components of the source vectors.</returns>
			static Vector2 Minimize(Vector2 value1, Vector2 value2);

			/// <summary>
			/// Returns a vector containing the largest components of the specified vectors.
			/// </summary>
			/// <param name="value1">The first source vector.</param>
			/// <param name="value2">The second source vector.</param>
			/// <returns>A vector containing the largest components of the source vectors.</returns>
			static Vector2 Maximize(Vector2 value1, Vector2 value2);

			/// <summary>
			/// Adds two vectors.
			/// </summary>
			/// <param name="left">The first vector to add.</param>
			/// <param name="right">The second vector to add.</param>
			/// <returns>The sum of the two vectors.</returns>
			static Vector2 operator + (Vector2 left, Vector2 right);

			/// <summary>
			/// Subtracts two vectors.
			/// </summary>
			/// <param name="left">The first vector to subtract.</param>
			/// <param name="right">The second vector to subtract.</param>
			/// <returns>The difference of the two vectors.</returns>
			static Vector2 operator - (Vector2 left, Vector2 right);

			/// <summary>
			/// Reverses the direction of a given vector.
			/// </summary>
			/// <param name="value">The vector to negate.</param>
			/// <returns>A vector facing in the opposite direction.</returns>
			static Vector2 operator - (Vector2 value);

			/// <summary>
			/// Scales a vector by the given value.
			/// </summary>
			/// <param name="vector">The vector to scale.</param>
			/// <param name="scale">The amount by which to scale the vector.</param>
			/// <returns>The scaled vector.</returns>
			static Vector2 operator * (Vector2 vector, float scale);

			/// <summary>
			/// Scales a vector by the given value.
			/// </summary>
			/// <param name="vector">The vector to scale.</param>
			/// <param name="scale">The amount by which to scale the vector.</param>
			/// <returns>The scaled vector.</returns>
			static Vector2 operator * (float scale, Vector2 vector);

			/// <summary>
			/// Scales a vector by the given value.
			/// </summary>
			/// <param name="vector">The vector to scale.</param>
			/// <param name="scale">The amount by which to scale the vector.</param>
			/// <returns>The scaled vector.</returns>
			static Vector2 operator / (Vector2 vector, float scale);

			/// <summary>
			/// Tests for equality between two objects.
			/// </summary>
			/// <param name="left">The first value to compare.</param>
			/// <param name="right">The second value to compare.</param>
			/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
			static bool operator == (Vector2 left, Vector2 right);

			/// <summary>
			/// Tests for inequality between two objects.
			/// </summary>
			/// <param name="left">The first value to compare.</param>
			/// <param name="right">The second value to compare.</param>
			/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
			static bool operator != (Vector2 left, Vector2 right);

			/// <summary>
			/// Converts the value of the object to its equivalent string representation.
			/// </summary>
			/// <returns>The string representation of the value of this instance.</returns>
			virtual System::String ^ToString() override;

			/// <summary>
			/// Returns the hash code for this instance.
			/// </summary>
			/// <returns>A 32-bit signed integer hash code.</returns>
			virtual int GetHashCode() override;

			/// <summary>
			/// Returns a value that indicates whether the current instance is equal to a specified object. 
			/// </summary>
			/// <param name="obj">Object to make the comparison with.</param>
			/// <returns><c>true</c> if the current instance is equal to the specified object; <c>false</c> otherwise.</returns>
			virtual bool Equals(System::Object ^obj) override;

			/// <summary>
			/// Returns a value that indicates whether the current instance is equal to the specified object. 
			/// </summary>
			/// <param name="other">Object to make the comparison with.</param>
			/// <returns><c>true</c> if the current instance is equal to the specified object; <c>false</c> otherwise.</returns>
			virtual bool Equals(Vector2 other);

			/// <summary>
			/// Determines whether the specified object instances are considered equal. 
			/// </summary>
			/// <param name="value1">The first value to compare.</param>
			/// <param name="value2">The second value to compare.</param>
			/// <returns><c>true</c> if <paramref name="value1"/> is the same instance as <paramref name="value2"/> or 
			/// if both are <c>null</c> references or if <c>value1.Equals(value2)</c> returns <c>true</c>; otherwise, <c>false</c>.</returns>
			static bool Equals(Vector2 %value1, Vector2 %value2);
		};
	}
}