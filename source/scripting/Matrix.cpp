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

#include "Matrix.hpp"
#include "Vector3.hpp"
#include "Quaternion.hpp"

#pragma unmanaged
#include <xmmintrin.h>
extern float _negXor[4];
void matrixmul_sse(const float *a, const float *b, float *r)
{
	__m128  r_line;
	for (int i = 0; i<16; i += 4) {
		r_line = _mm_mul_ps(_mm_loadu_ps(a), _mm_set1_ps(b[i]));
		for (int j = 1; j<4; j++) {
			r_line = _mm_add_ps(_mm_mul_ps(_mm_loadu_ps(&a[j * 4]), _mm_set1_ps(b[i + j])), r_line);
		}
		_mm_storeu_ps(&r[i], r_line);
	}
}
void mdiv_sse(const float *a, const float *b, float *r)
{
	for (int i = 0; i<16; i += 4) {
		_mm_storeu_ps(&r[i], _mm_div_ps(_mm_loadu_ps(&a[i]), _mm_loadu_ps(&b[i])));
	}
}
void mmul_sse(const float *a, const float scale, float *r)
{
	__m128 s_line = _mm_set1_ps(scale);
	for (int i = 0; i<16; i += 4) {
		_mm_storeu_ps(&r[i], _mm_mul_ps(_mm_loadu_ps(&a[i]), s_line));
	}
}
void madd_sse(const float *a, const float *b, float *r)
{
	for (int i = 0; i<16; i += 4) {
		_mm_storeu_ps(&r[i], _mm_add_ps(_mm_loadu_ps(&a[i]), _mm_loadu_ps(&b[i])));
	}
}
void msub_sse(const float *a, const float *b, float *r)
{
	for (int i = 0; i<16; i += 4) {
		_mm_storeu_ps(&r[i], _mm_sub_ps(_mm_loadu_ps(&a[i]), _mm_loadu_ps(&b[i])));
	}
}
void mlerp_sse(const float *a, const float *b, float amount, float *r)
{
	//use lerp(x0, x1, t) = (1 - t)*x0 + t*x1 as to prevent rounding errors
	__m128 s_line1 = _mm_set1_ps(1 - amount);
	__m128 s_line2 = _mm_set1_ps(amount);
	for (int i = 0; i<16; i += 4) {
		_mm_storeu_ps(&r[i], _mm_add_ps(
			_mm_mul_ps(_mm_loadu_ps(&a[i]), s_line1),
			_mm_mul_ps(_mm_loadu_ps(&b[i]), s_line2)));
	}
}
void mneg_sse(const float *a, float *r)
{
	__m128 xorVal = _mm_load_ps(_negXor);
	_mm_storeu_ps(r, _mm_xor_ps(_mm_loadu_ps(a), xorVal));
	_mm_storeu_ps(&r[4], _mm_xor_ps(_mm_loadu_ps(&a[4]), xorVal));
	_mm_storeu_ps(&r[8], _mm_xor_ps(_mm_loadu_ps(&a[8]), xorVal));
	_mm_storeu_ps(&r[12], _mm_xor_ps(_mm_loadu_ps(&a[12]), xorVal));
}
void mtranspose_sse(const float *a, float *r)
{
	__m128 row1 = _mm_loadu_ps(a),
		row2 = _mm_loadu_ps(&a[4]),
		row3 = _mm_loadu_ps(&a[8]),
		row4 = _mm_loadu_ps(&a[12]);
	_MM_TRANSPOSE4_PS(row1, row2, row3, row4);
	_mm_storeu_ps(r, row1);
	_mm_storeu_ps(&r[4], row2);
	_mm_storeu_ps(&r[8], row3);
	_mm_storeu_ps(&r[12], row4);
}
void mtransform_point_sse(const float *matrix, const float pointX, const float pointY, const float pointZ, float *r)
{
	__m128 point = _mm_set_ps(1.0f, pointZ, pointY, pointX);
	point = _mm_add_ps(
		_mm_add_ps(
			_mm_add_ps(
				_mm_mul_ps(_mm_loadu_ps(matrix), _mm_shuffle_ps(point, point, _MM_SHUFFLE(0, 0, 0, 0))),
				_mm_mul_ps(_mm_loadu_ps(&matrix[4]), _mm_shuffle_ps(point, point, _MM_SHUFFLE(1, 1, 1, 1)))),
			_mm_mul_ps(_mm_loadu_ps(&matrix[8]), _mm_shuffle_ps(point, point, _MM_SHUFFLE(2, 2, 2, 2)))),
		_mm_loadu_ps(&matrix[12])
	);
	_mm_storeu_ps(r, point);
}
void minvtransform_point_sse(const float *matrix, const float pointX, const float pointY, const float pointZ, float *r)
{
	__m128 off = _mm_sub_ps(_mm_set_ps(1.0f, pointZ, pointY, pointX), _mm_loadu_ps(&matrix[12]));
	__m128 r1 = _mm_loadu_ps(matrix);
	__m128 r2 = _mm_loadu_ps(&matrix[4]);
	__m128 r3 = _mm_loadu_ps(&matrix[8]);
	__m128 zero = _mm_setzero_ps();
	__m128 lo = _mm_unpacklo_ps(r1, r3);//r0, u0, r1, u1
	__m128 lo2 = _mm_unpacklo_ps(r2, zero);//f0, 0, f1, 0
	off = _mm_add_ps(
		_mm_add_ps(
			_mm_mul_ps(_mm_unpacklo_ps(lo, lo2), _mm_shuffle_ps(off, off, _MM_SHUFFLE(0, 0, 0, 0))),
			_mm_mul_ps(_mm_unpackhi_ps(lo, lo2), _mm_shuffle_ps(off, off, _MM_SHUFFLE(1, 1, 1, 1)))),
		_mm_mul_ps(
			_mm_unpacklo_ps(_mm_unpackhi_ps(r1, r3), _mm_unpackhi_ps(r2, zero)), _mm_shuffle_ps(off, off, _MM_SHUFFLE(2, 2, 2, 2))));
	_mm_storeu_ps(r, off);
}
#pragma managed

namespace GTA
{
	namespace Math
	{
		using namespace System;
		using namespace System::Globalization;

		float Matrix::default::get(int row, int column)
		{
			if (row < 0 || row > 3)
				throw gcnew ArgumentOutOfRangeException("row", "Rows and columns for matrices run from 0 to 3, inclusive.");

			if (column < 0 || column > 3)
				throw gcnew ArgumentOutOfRangeException("column", "Rows and columns for matrices run from 0 to 3, inclusive.");

			int index = row * 4 + column;

			switch (index)
			{
				case 0:  return M11;
				case 1:  return M12;
				case 2:  return M13;
				case 3:  return M14;
				case 4:  return M21;
				case 5:  return M22;
				case 6:  return M23;
				case 7:  return M24;
				case 8:  return M31;
				case 9:  return M32;
				case 10: return M33;
				case 11: return M34;
				case 12: return M41;
				case 13: return M42;
				case 14: return M43;
				case 15: return M44;
			}

			return 0.0f;
		}
		void Matrix::default::set(int row, int column, float value)
		{
			if (row < 0 || row > 3)
				throw gcnew ArgumentOutOfRangeException("row", "Rows and columns for matrices run from 0 to 3, inclusive.");

			if (column < 0 || column > 3)
				throw gcnew ArgumentOutOfRangeException("column", "Rows and columns for matrices run from 0 to 3, inclusive.");

			int index = row * 4 + column;
			switch (index)
			{
				case 0:  M11 = value; break;
				case 1:  M12 = value; break;
				case 2:  M13 = value; break;
				case 3:  M14 = value; break;
				case 4:  M21 = value; break;
				case 5:  M22 = value; break;
				case 6:  M23 = value; break;
				case 7:  M24 = value; break;
				case 8:  M31 = value; break;
				case 9:  M32 = value; break;
				case 10: M33 = value; break;
				case 11: M34 = value; break;
				case 12: M41 = value; break;
				case 13: M42 = value; break;
				case 14: M43 = value; break;
				case 15: M44 = value; break;
			}
		}

		bool Matrix::IsIdentity::get()
		{
			if (M11 != 1.0f || M22 != 1.0f || M33 != 1.0f || M44 != 1.0f)
				return false;

			if (M12 != 0.0f || M13 != 0.0f || M14 != 0.0f ||
				M21 != 0.0f || M23 != 0.0f || M24 != 0.0f ||
				M31 != 0.0f || M32 != 0.0f || M34 != 0.0f ||
				M41 != 0.0f || M42 != 0.0f || M43 != 0.0f)
				return false;

			return true;
		}
		bool Matrix::HasInverse::get()
		{
			return Determinant() != 0.0f;
		}

		Matrix Matrix::FromArray(array<float> ^floatArray)
		{
			if (floatArray->Length != 16)
				throw gcnew Exception("Array must contain 16 items to be converted to a 4*4 Matrix");
			Matrix result;
			System::Runtime::InteropServices::Marshal::Copy(floatArray, 0, IntPtr(&result), 16);
			return result;
		}

		float Matrix::Determinant()
		{
			float temp1 = (M33 * M44) - (M34 * M43);
			float temp2 = (M32 * M44) - (M34 * M42);
			float temp3 = (M32 * M43) - (M33 * M42);
			float temp4 = (M31 * M44) - (M34 * M41);
			float temp5 = (M31 * M43) - (M33 * M41);
			float temp6 = (M31 * M42) - (M32 * M41);

			return ((((M11 * (((M22 * temp1) - (M23 * temp2)) + (M24 * temp3))) - (M12 * (((M21 * temp1) - (M23 * temp4)) + (M24 * temp5)))) + (M13 * (((M21 * temp2) - (M22 * temp4)) + (M24 * temp6)))) - (M14 * (((M21 * temp3) - (M22 * temp5)) + (M23 * temp6))));
		}
		float Det3x3(float M11, float M12, float M13, float M21, float M22, float M23, float M31, float M32, float M33)
		{
			return M11 * (M22 * M33 - M23 * M32) - M12 * (M21 * M33 - M23 * M31) + M13 * (M21 * M32 - M22 * M31);
		}

		void Matrix::Inverse()
		{
			float Det = Determinant();
			if (Det == 0.0f)
				return;
			float invDet = 1.0f / Det;
			float tM11 = Det3x3(M22, M23, M24, M32, M33, M34, M42, M43, M44) * invDet;
			float tM21 = -Det3x3(M21, M23, M24, M31, M33, M34, M41, M43, M44) * invDet;
			float tM31 = Det3x3(M21, M22, M24, M31, M32, M34, M41, M42, M44) * invDet;
			float tM41 = -Det3x3(M21, M22, M23, M31, M32, M33, M41, M42, M43) * invDet;

			float tM12 = -Det3x3(M12, M13, M14, M32, M33, M34, M42, M43, M44) * invDet;
			float tM22 = Det3x3(M11, M13, M14, M31, M33, M34, M41, M43, M44) * invDet;
			float tM32 = -Det3x3(M11, M12, M14, M31, M32, M34, M41, M42, M44) * invDet;
			float tM42 = Det3x3(M11, M12, M13, M31, M32, M33, M41, M42, M43) * invDet;

			float tM13 = Det3x3(M12, M13, M14, M22, M23, M24, M42, M43, M44) * invDet;
			float tM23 = -Det3x3(M11, M13, M14, M21, M23, M24, M41, M43, M44) * invDet;
			float tM33 = Det3x3(M11, M12, M14, M21, M22, M24, M41, M42, M44) * invDet;
			float tM43 = -Det3x3(M11, M12, M13, M21, M22, M23, M41, M42, M43) * invDet;

			float tM14 = -Det3x3(M12, M13, M14, M22, M23, M24, M32, M33, M34) * invDet;
			float tM24 = Det3x3(M11, M13, M14, M21, M23, M24, M31, M33, M34) * invDet;
			float tM34 = -Det3x3(M11, M12, M14, M21, M22, M24, M31, M32, M34) * invDet;
			float tM44 = Det3x3(M11, M12, M13, M21, M22, M23, M31, M32, M33) * invDet;

			M11 = tM11;
			M12 = tM12;
			M13 = tM13;
			M14 = tM14;

			M21 = tM21;
			M22 = tM22;
			M23 = tM23;
			M24 = tM24;

			M31 = tM31;
			M32 = tM32;
			M33 = tM33;
			M34 = tM34;

			M41 = tM41;
			M42 = tM42;
			M43 = tM43;
			M44 = tM44;
		}

		Vector3 Matrix::TransformPoint(Vector3 point)
		{
			pin_ptr<Matrix> pinned = this;
			mtransform_point_sse((float*)pinned, point.X, point.Y, point.Z, (float*)&point);
			return point;
		}

		Vector3 Matrix::InverseTransformPoint(Vector3 point)
		{
			pin_ptr<Matrix> pinned = this;
			minvtransform_point_sse((float*)pinned, point.X, point.Y, point.Z, (float*)&point);
			return point;
		}

		Matrix Matrix::Add(Matrix left, Matrix right)
		{
			madd_sse((float*)&left, (float*)&right, (float*)&left);
			return left;
		}
		Matrix Matrix::Subtract(Matrix left, Matrix right)
		{
			msub_sse((float*)&left, (float*)&right, (float*)&left);
			return left;
		}
		Matrix Matrix::Negate(Matrix matrix)
		{
			mneg_sse((float*)&matrix, (float*)&matrix);
			return matrix;
		}

		Matrix Matrix::Inverse(Matrix matrix)
		{
			matrix.Inverse();
			return matrix;
		}
		Matrix Matrix::Multiply(Matrix left, Matrix right)
		{
			matrixmul_sse((float*)&left, (float*)&right, (float*)&left);
			return left;
		}
		Matrix Matrix::Multiply(Matrix left, float right)
		{
			mmul_sse((float*)&left, right, (float*)&left);
			return left;
		}
		Matrix Matrix::Divide(Matrix left, Matrix right)
		{
			mdiv_sse((float*)&left, (float*)&right, (float*)&left);
			return left;
		}
		Matrix Matrix::Divide(Matrix left, float right)
		{
			mmul_sse((float*)&left, 1.0f / right, (float*)&left);
			return left;
		}
		Matrix Matrix::Lerp(Matrix value1, Matrix value2, float amount)
		{
			mlerp_sse((float*)&value1, (float*)&value2, amount, (float*)&value1);
			return value1;
		}
		Matrix Matrix::RotationX(float angle)
		{
			Matrix result;
			float cos = static_cast<float>(System::Math::Cos(static_cast<double>(angle)));
			float sin = static_cast<float>(System::Math::Sin(static_cast<double>(angle)));

			result.M11 = 1.0f;
			result.M12 = 0.0f;
			result.M13 = 0.0f;
			result.M14 = 0.0f;
			result.M21 = 0.0f;
			result.M22 = cos;
			result.M23 = sin;
			result.M24 = 0.0f;
			result.M31 = 0.0f;
			result.M32 = -sin;
			result.M33 = cos;
			result.M34 = 0.0f;
			result.M41 = 0.0f;
			result.M42 = 0.0f;
			result.M43 = 0.0f;
			result.M44 = 1.0f;

			return result;
		}
		Matrix Matrix::RotationY(float angle)
		{
			Matrix result;
			float cos = static_cast<float>(System::Math::Cos(static_cast<double>(angle)));
			float sin = static_cast<float>(System::Math::Sin(static_cast<double>(angle)));

			result.M11 = cos;
			result.M12 = 0.0f;
			result.M13 = -sin;
			result.M14 = 0.0f;
			result.M21 = 0.0f;
			result.M22 = 1.0f;
			result.M23 = 0.0f;
			result.M24 = 0.0f;
			result.M31 = sin;
			result.M32 = 0.0f;
			result.M33 = cos;
			result.M34 = 0.0f;
			result.M41 = 0.0f;
			result.M42 = 0.0f;
			result.M43 = 0.0f;
			result.M44 = 1.0f;

			return result;
		}
		Matrix Matrix::RotationZ(float angle)
		{
			Matrix result;
			float cos = static_cast<float>(System::Math::Cos(static_cast<double>(angle)));
			float sin = static_cast<float>(System::Math::Sin(static_cast<double>(angle)));

			result.M11 = cos;
			result.M12 = sin;
			result.M13 = 0.0f;
			result.M14 = 0.0f;
			result.M21 = -sin;
			result.M22 = cos;
			result.M23 = 0.0f;
			result.M24 = 0.0f;
			result.M31 = 0.0f;
			result.M32 = 0.0f;
			result.M33 = 1.0f;
			result.M34 = 0.0f;
			result.M41 = 0.0f;
			result.M42 = 0.0f;
			result.M43 = 0.0f;
			result.M44 = 1.0f;

			return result;
		}
		Matrix Matrix::RotationQuaternion(Quaternion quaternion)
		{
			Matrix result;

			float xx = quaternion.X * quaternion.X;
			float yy = quaternion.Y * quaternion.Y;
			float zz = quaternion.Z * quaternion.Z;
			float xy = quaternion.X * quaternion.Y;
			float zw = quaternion.Z * quaternion.W;
			float zx = quaternion.Z * quaternion.X;
			float yw = quaternion.Y * quaternion.W;
			float yz = quaternion.Y * quaternion.Z;
			float xw = quaternion.X * quaternion.W;
			result.M11 = 1.0f - (2.0f * (yy + zz));
			result.M12 = 2.0f * (xy + zw);
			result.M13 = 2.0f * (zx - yw);
			result.M14 = 0.0f;
			result.M21 = 2.0f * (xy - zw);
			result.M22 = 1.0f - (2.0f * (zz + xx));
			result.M23 = 2.0f * (yz + xw);
			result.M24 = 0.0f;
			result.M31 = 2.0f * (zx + yw);
			result.M32 = 2.0f * (yz - xw);
			result.M33 = 1.0f - (2.0f * (yy + xx));
			result.M34 = 0.0f;
			result.M41 = 0.0f;
			result.M42 = 0.0f;
			result.M43 = 0.0f;
			result.M44 = 1.0f;

			return result;
		}
		Matrix Matrix::RotationAxis(Vector3 axis, float angle)
		{
			if (axis.LengthSquared() != 1.0f)
				axis.Normalize();

			Matrix result;
			float x = axis.X;
			float y = axis.Y;
			float z = axis.Z;
			float cos = static_cast<float>(System::Math::Cos(static_cast<double>(angle)));
			float sin = static_cast<float>(System::Math::Sin(static_cast<double>(angle)));
			float xx = x * x;
			float yy = y * y;
			float zz = z * z;
			float xy = x * y;
			float xz = x * z;
			float yz = y * z;

			result.M11 = xx + (cos * (1.0f - xx));
			result.M12 = (xy - (cos * xy)) + (sin * z);
			result.M13 = (xz - (cos * xz)) - (sin * y);
			result.M14 = 0.0f;
			result.M21 = (xy - (cos * xy)) - (sin * z);
			result.M22 = yy + (cos * (1.0f - yy));
			result.M23 = (yz - (cos * yz)) + (sin * x);
			result.M24 = 0.0f;
			result.M31 = (xz - (cos * xz)) + (sin * y);
			result.M32 = (yz - (cos * yz)) - (sin * x);
			result.M33 = zz + (cos * (1.0f - zz));
			result.M34 = 0.0f;
			result.M41 = 0.0f;
			result.M42 = 0.0f;
			result.M43 = 0.0f;
			result.M44 = 1.0f;

			return result;
		}
		Matrix Matrix::RotationYawPitchRoll(float yaw, float pitch, float roll)
		{
			Quaternion quaternion = Quaternion::RotationYawPitchRoll(yaw, pitch, roll);
			return RotationQuaternion(quaternion);
		}
		Matrix Matrix::Translation(float x, float y, float z)
		{
			Matrix result;
			result.M11 = 1.0f;
			result.M12 = 0.0f;
			result.M13 = 0.0f;
			result.M14 = 0.0f;
			result.M21 = 0.0f;
			result.M22 = 1.0f;
			result.M23 = 0.0f;
			result.M24 = 0.0f;
			result.M31 = 0.0f;
			result.M32 = 0.0f;
			result.M33 = 1.0f;
			result.M34 = 0.0f;
			result.M41 = x;
			result.M42 = y;
			result.M43 = z;
			result.M44 = 1.0f;
			return result;
		}
		Matrix Matrix::Translation(Vector3 translation)
		{
			Matrix result;
			result.M11 = 1.0f;
			result.M12 = 0.0f;
			result.M13 = 0.0f;
			result.M14 = 0.0f;
			result.M21 = 0.0f;
			result.M22 = 1.0f;
			result.M23 = 0.0f;
			result.M24 = 0.0f;
			result.M31 = 0.0f;
			result.M32 = 0.0f;
			result.M33 = 1.0f;
			result.M34 = 0.0f;
			result.M41 = translation.X;
			result.M42 = translation.Y;
			result.M43 = translation.Z;
			result.M44 = 1.0f;
			return result;
		}
		Matrix Matrix::Scaling(float x, float y, float z)
		{
			Matrix result;
			result.M11 = x;
			result.M12 = 0.0f;
			result.M13 = 0.0f;
			result.M14 = 0.0f;
			result.M21 = 0.0f;
			result.M22 = y;
			result.M23 = 0.0f;
			result.M24 = 0.0f;
			result.M31 = 0.0f;
			result.M32 = 0.0f;
			result.M33 = z;
			result.M34 = 0.0f;
			result.M41 = 0.0f;
			result.M42 = 0.0f;
			result.M43 = 0.0f;
			result.M44 = 1.0f;
			return result;
		}
		Matrix Matrix::Scaling(Vector3 scaling)
		{
			Matrix result;
			result.M11 = scaling.X;
			result.M12 = 0.0f;
			result.M13 = 0.0f;
			result.M14 = 0.0f;
			result.M21 = 0.0f;
			result.M22 = scaling.Y;
			result.M23 = 0.0f;
			result.M24 = 0.0f;
			result.M31 = 0.0f;
			result.M32 = 0.0f;
			result.M33 = scaling.Z;
			result.M34 = 0.0f;
			result.M41 = 0.0f;
			result.M42 = 0.0f;
			result.M43 = 0.0f;
			result.M44 = 1.0f;
			return result;
		}
		Matrix Matrix::Transpose(Matrix mat)
		{
			mtranspose_sse((float*)&mat, (float*)&mat);
			return mat;
		}

		Matrix Matrix::operator * (Matrix left, Matrix right)
		{
			matrixmul_sse((float*)&left, (float*)&right, (float*)&left);
			return left;
		}
		Matrix Matrix::operator * (Matrix left, float right)
		{
			mmul_sse((float*)&left, right, (float*)&left);
			return left;
		}
		Matrix Matrix::operator * (float right, Matrix left)
		{
			return left * right;
		}
		Matrix Matrix::operator / (Matrix left, Matrix right)
		{
			mdiv_sse((float*)&left, (float*)&right, (float*)&left);
			return left;
		}
		Matrix Matrix::operator / (Matrix left, float right)
		{
			mmul_sse((float*)&left, 1.0f / right, (float*)&left);
			return left;
		}
		Matrix Matrix::operator + (Matrix left, Matrix right)
		{
			madd_sse((float*)&left, (float*)&right, (float*)&left);
			return left;
		}
		Matrix Matrix::operator - (Matrix left, Matrix right)
		{
			msub_sse((float*)&left, (float*)&right, (float*)&left);
			return left;
		}
		Matrix Matrix::operator - (Matrix matrix)
		{
			mneg_sse((float*)&matrix, (float*)&matrix);
			return matrix;
		}
		bool Matrix::operator == (Matrix left, Matrix right)
		{
			return Matrix::Equals(left, right);
		}
		bool Matrix::operator != (Matrix left, Matrix right)
		{
			return !Matrix::Equals(left, right);
		}

		array<float> ^Matrix::ToArray()
		{
			array<float> ^result = gcnew array<float>(16);
			pin_ptr<Matrix> pinned = this;
			System::Runtime::InteropServices::Marshal::Copy(IntPtr(pinned), result, 0, 16);
			return result;
		}
		String ^Matrix::ToString()
		{
			return String::Format(CultureInfo::CurrentCulture, "[[M11:{0} M12:{1} M13:{2} M14:{3}] [M21:{4} M22:{5} M23:{6} M24:{7}] [M31:{8} M32:{9} M33:{10} M34:{11}] [M41:{12} M42:{13} M43:{14} M44:{15}]]",
				M11.ToString(CultureInfo::CurrentCulture), M12.ToString(CultureInfo::CurrentCulture), M13.ToString(CultureInfo::CurrentCulture), M14.ToString(CultureInfo::CurrentCulture),
				M21.ToString(CultureInfo::CurrentCulture), M22.ToString(CultureInfo::CurrentCulture), M23.ToString(CultureInfo::CurrentCulture), M24.ToString(CultureInfo::CurrentCulture),
				M31.ToString(CultureInfo::CurrentCulture), M32.ToString(CultureInfo::CurrentCulture), M33.ToString(CultureInfo::CurrentCulture), M34.ToString(CultureInfo::CurrentCulture),
				M41.ToString(CultureInfo::CurrentCulture), M42.ToString(CultureInfo::CurrentCulture), M43.ToString(CultureInfo::CurrentCulture), M44.ToString(CultureInfo::CurrentCulture));
		}
		int Matrix::GetHashCode()
		{
			return M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() + M14.GetHashCode() +
				M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() + M24.GetHashCode() +
				M31.GetHashCode() + M32.GetHashCode() + M33.GetHashCode() + M34.GetHashCode() +
				M41.GetHashCode() + M42.GetHashCode() + M43.GetHashCode() + M44.GetHashCode();
		}
		bool Matrix::Equals(Object ^value)
		{
			if (value == nullptr || value->GetType() != GetType())
				return false;

			return Equals(safe_cast<Matrix>(value));
		}
		bool Matrix::Equals(Matrix value)
		{
			return (M11 == value.M11 && M12 == value.M12 && M13 == value.M13 && M14 == value.M14 &&
				M21 == value.M21 && M22 == value.M22 && M23 == value.M23 && M24 == value.M24 &&
				M31 == value.M31 && M32 == value.M32 && M33 == value.M33 && M34 == value.M34 &&
				M41 == value.M41 && M42 == value.M42 && M43 == value.M43 && M44 == value.M44);
		}
		bool Matrix::Equals(Matrix %value1, Matrix %value2)
		{
			return (value1.M11 == value2.M11 && value1.M12 == value2.M12 && value1.M13 == value2.M13 && value1.M14 == value2.M14 &&
				value1.M21 == value2.M21 && value1.M22 == value2.M22 && value1.M23 == value2.M23 && value1.M24 == value2.M24 &&
				value1.M31 == value2.M31 && value1.M32 == value2.M32 && value1.M33 == value2.M33 && value1.M34 == value2.M34 &&
				value1.M41 == value2.M41 && value1.M42 == value2.M42 && value1.M43 == value2.M43 && value1.M44 == value2.M44);
		}
	}
}