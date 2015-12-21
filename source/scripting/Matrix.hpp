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
		value class Vector3;
		value class Quaternion;

		/// <summary>
		/// Defines a 4x4 matrix.
		/// </summary>
		/// <unmanaged>D3DXMATRIX</unmanaged>
		[System::SerializableAttribute]
		[System::Runtime::InteropServices::StructLayout(System::Runtime::InteropServices::LayoutKind::Sequential, Pack = 4)]
		public value class Matrix : System::IEquatable < Matrix >
		{
		public:
			/// <summary>
			/// Gets or sets the element of the matrix that exists in the first row and first column. 
			/// </summary>
			float M11;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the first row and second column. 
			/// </summary>
			float M12;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the first row and third column. 
			/// </summary>
			float M13;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the first row and fourth column. 
			/// </summary>
			float M14;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the second row and first column. 
			/// </summary>
			float M21;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the second row and second column. 
			/// </summary>
			float M22;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the second row and third column. 
			/// </summary>
			float M23;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the second row and fourth column. 
			/// </summary>
			float M24;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the third row and first column. 
			/// </summary>
			float M31;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the third row and second column. 
			/// </summary>
			float M32;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the third row and third column. 
			/// </summary>
			float M33;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the third row and fourth column. 
			/// </summary>
			float M34;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the fourth row and first column. 
			/// </summary>
			float M41;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the fourth row and second column. 
			/// </summary>
			float M42;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the fourth row and third column. 
			/// </summary>
			float M43;

			/// <summary>
			/// Gets or sets the element of the matrix that exists in the fourth row and fourth column. 
			/// </summary>
			float M44;

			[System::ComponentModel::Browsable(false)]
			property float default[int, int]
			{
				float get(int row, int column);
				void set(int row, int column, float value);
			}

			/// <summary>
			/// Gets a <see cref="Matrix"/> that represents an identity matrix.
			/// </summary>
			static property Matrix Identity
			{
				Matrix get()
				{
					Matrix result;
					result.M11 = 1.0f;
					result.M22 = 1.0f;
					result.M33 = 1.0f;
					result.M44 = 1.0f;

					return result;
				}
			}

			/// <summary>
			/// Gets a value indicating whether this instance is an identity matrix.
			/// </summary>
			[System::ComponentModel::Browsable(false)]
			property bool IsIdentity
			{
				bool get();
			}
			/// <summary>
			/// Gets a value indicating whether this instance has an inverse matrix.
			/// </summary>
			property bool HasInverse
			{
				bool get();
			}

			/// <summary>
			/// Converts the matrix to an array of floats.
			/// </summary>
			static Matrix FromArray(array<float> ^floatArray);

			/// <summary>
			/// Calculates the determinant of the matrix.
			/// </summary>
			/// <returns>The determinant of the matrix.</returns>
			float Determinant();

			/// <summary>
			/// Calculates the inverse of the matrix if it exists.
			/// </summary>
			void Inverse();

			/// <summary>
			/// Determines the sum of two matrices.
			/// </summary>
			/// <param name="left">The first matrix to add.</param>
			/// <param name="right">The second matrix to add.</param>
			/// <returns>The sum of the two matrices.</returns>
			static Matrix Add(Matrix left, Matrix right);

			/// <summary>
			/// Determines the difference between two matrices.
			/// </summary>
			/// <param name="left">The first matrix to subtract.</param>
			/// <param name="right">The second matrix to subtract.</param>
			/// <returns>The difference between the two matrices.</returns>
			static Matrix Subtract(Matrix left, Matrix right);

			/// <summary>
			/// Determines the product of two matrices.
			/// </summary>
			/// <param name="left">The first matrix to multiply.</param>
			/// <param name="right">The second matrix to multiply.</param>
			/// <returns>The product of the two matrices.</returns>
			static Matrix Multiply(Matrix left, Matrix right);

			/// <summary>
			/// Scales a matrix by the given value.
			/// </summary>
			/// <param name="left">The matrix to scale.</param>
			/// <param name="right">The amount by which to scale.</param>
			/// <returns>The scaled matrix.</returns>
			static Matrix Multiply(Matrix left, float right);

			/// <summary>
			/// Determines the quotient of two matrices.
			/// </summary>
			/// <param name="left">The first matrix to divide.</param>
			/// <param name="right">The second matrix to divide.</param>
			/// <returns>The quotient of the two matrices.</returns>
			static Matrix Divide(Matrix left, Matrix right);

			/// <summary>
			/// Scales a matrix by the given value.
			/// </summary>
			/// <param name="left">The matrix to scale.</param>
			/// <param name="right">The amount by which to scale.</param>
			/// <returns>The scaled matrix.</returns>
			static Matrix Divide(Matrix left, float right);

			/// <summary>
			/// Negates a matrix.
			/// </summary>
			/// <param name="matrix">The matrix to be negated.</param>
			/// <returns>The negated matrix.</returns>
			static Matrix Negate(Matrix matrix);

			/// <summary>
			/// Calculates the inverse of a matrix if it exists.
			/// </summary>
			/// <returns>The inverse of the matrix.</returns>
			static Matrix Inverse(Matrix matrix);

			/// <summary>
			/// Performs a linear interpolation between two matricies.
			/// </summary>
			/// <param name="start">Start matrix.</param>
			/// <param name="end">End matrix.</param>
			/// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
			/// <returns>The linear interpolation of the two matrices.</returns>
			/// <remarks>
			/// This method performs the linear interpolation based on the following formula.
			/// <code>start + (end - start) * amount</code>
			/// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
			/// </remarks>
			static Matrix Lerp(Matrix start, Matrix end, float amount);

			/// <summary>
			/// Creates a matrix that rotates around the x-axis.
			/// </summary>
			/// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin.</param>
			/// <returns>The created rotation matrix.</returns>
			static Matrix RotationX(float angle);

			/// <summary>
			/// Creates a matrix that rotates around the y-axis.
			/// </summary>
			/// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin.</param>
			/// <returns>The created rotation matrix.</returns>
			static Matrix RotationY(float angle);

			/// <summary>
			/// Creates a matrix that rotates around the z-axis.
			/// </summary>
			/// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin.</param>
			/// <returns>The created rotation matrix.</returns>
			static Matrix RotationZ(float angle);

			/// <summary>
			/// Creates a matrix that rotates around an arbitary axis.
			/// </summary>
			/// <param name="axis">The axis around which to rotate.</param>
			/// <param name="angle">Angle of rotation in radians. Angles are measured clockwise when looking along the rotation axis toward the origin.</param>
			/// <returns>The created rotation matrix.</returns>
			static Matrix RotationAxis(Vector3 axis, float angle);

			/// <summary>
			/// Creates a rotation matrix from a quaternion.
			/// </summary>
			/// <param name="rotation">The quaternion to use to build the matrix.</param>
			/// <returns>The created rotation matrix.</returns>
			static Matrix RotationQuaternion(Quaternion rotation);

			/// <summary>
			/// Creates a rotation matrix with a specified yaw, pitch, and roll.
			/// </summary>
			/// <param name="yaw">Yaw around the y-axis, in radians.</param>
			/// <param name="pitch">Pitch around the x-axis, in radians.</param>
			/// <param name="roll">Roll around the z-axis, in radians.</param>
			/// <returns>The created rotation matrix.</returns>
			static Matrix RotationYawPitchRoll(float yaw, float pitch, float roll);

			/// <summary>
			/// Creates a matrix that scales along the x-axis, y-axis, and y-axis.
			/// </summary>
			/// <param name="x">Scaling factor that is applied along the x-axis.</param>
			/// <param name="y">Scaling factor that is applied along the y-axis.</param>
			/// <param name="z">Scaling factor that is applied along the z-axis.</param>
			/// <returns>The created scaling matrix.</returns>
			static Matrix Scaling(float x, float y, float z);

			/// <summary>
			/// Creates a matrix that scales along the x-axis, y-axis, and y-axis.
			/// </summary>
			/// <param name="scale">Scaling factor for all three axes.</param>
			/// <returns>The created scaling matrix.</returns>
			static Matrix Scaling(Vector3 scale);

			/// <summary>
			/// Creates a translation matrix using the specified offsets.
			/// </summary>
			/// <param name="x">X-coordinate offset.</param>
			/// <param name="y">Y-coordinate offset.</param>
			/// <param name="z">Z-coordinate offset.</param>
			/// <returns>The created translation matrix.</returns>
			static Matrix Translation(float x, float y, float z);

			/// <summary>
			/// Creates a translation matrix using the specified offsets.
			/// </summary>
			/// <param name="amount">The offset for all three coordinate planes.</param>
			/// <returns>The created translation matrix.</returns>
			static Matrix Translation(Vector3 amount);

			/// <summary>
			/// Calculates the transpose of the specified matrix.
			/// </summary>
			/// <param name="matrix">The matrix whose transpose is to be calculated.</param>
			/// <returns>The transpose of the specified matrix.</returns>
			static Matrix Transpose(Matrix matrix);

			/// <summary>
			/// Negates a matrix.
			/// </summary>
			/// <param name="matrix">The matrix to negate.</param>
			/// <returns>The negated matrix.</returns>
			static Matrix operator - (Matrix matrix);

			/// <summary>
			/// Adds two matricies.
			/// </summary>
			/// <param name="left">The first matrix to add.</param>
			/// <param name="right">The second matrix to add.</param>
			/// <returns>The sum of the two matricies.</returns>
			static Matrix operator + (Matrix left, Matrix right);

			/// <summary>
			/// Subtracts two matricies.
			/// </summary>
			/// <param name="left">The first matrix to subtract.</param>
			/// <param name="right">The second matrix to subtract.</param>
			/// <returns>The difference between the two matricies.</returns>
			static Matrix operator - (Matrix left, Matrix right);

			/// <summary>
			/// Divides two matricies.
			/// </summary>
			/// <param name="left">The first matrix to divide.</param>
			/// <param name="right">The second matrix to divide.</param>
			/// <returns>The quotient of the two matricies.</returns>
			static Matrix operator / (Matrix left, Matrix right);

			/// <summary>
			/// Scales a matrix by a given value.
			/// </summary>
			/// <param name="left">The matrix to scale.</param>
			/// <param name="right">The amount by which to scale.</param>
			/// <returns>The scaled matrix.</returns>
			static Matrix operator / (Matrix left, float right);

			/// <summary>
			/// Multiplies two matricies.
			/// </summary>
			/// <param name="left">The first matrix to multiply.</param>
			/// <param name="right">The second matrix to multiply.</param>
			/// <returns>The product of the two matricies.</returns>
			static Matrix operator * (Matrix left, Matrix right);

			/// <summary>
			/// Scales a matrix by a given value.
			/// </summary>
			/// <param name="left">The matrix to scale.</param>
			/// <param name="right">The amount by which to scale.</param>
			/// <returns>The scaled matrix.</returns>
			static Matrix operator * (Matrix left, float right);

			/// <summary>
			/// Scales a matrix by a given value.
			/// </summary>
			/// <param name="right">The matrix to scale.</param>
			/// <param name="left">The amount by which to scale.</param>
			/// <returns>The scaled matrix.</returns>
			static Matrix operator * (float left, Matrix right);

			/// <summary>
			/// Tests for equality between two objects.
			/// </summary>
			/// <param name="left">The first value to compare.</param>
			/// <param name="right">The second value to compare.</param>
			/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
			static bool operator == (Matrix left, Matrix right);

			/// <summary>
			/// Tests for inequality between two objects.
			/// </summary>
			/// <param name="left">The first value to compare.</param>
			/// <param name="right">The second value to compare.</param>
			/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
			static bool operator != (Matrix left, Matrix right);

			/// <summary>
			/// Converts the matrix to an array of floats.
			/// </summary>
			array<float> ^ToArray();
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
			virtual bool Equals(Matrix other);

			/// <summary>
			/// Determines whether the specified object instances are considered equal. 
			/// </summary>
			/// <param name="value1"></param>
			/// <param name="value2"></param>
			/// <returns><c>true</c> if <paramref name="value1"/> is the same instance as <paramref name="value2"/> or 
			/// if both are <c>null</c> references or if <c>value1.Equals(value2)</c> returns <c>true</c>; otherwise, <c>false</c>.</returns>
			static bool Equals(Matrix %value1, Matrix %value2);
		};
	}
}