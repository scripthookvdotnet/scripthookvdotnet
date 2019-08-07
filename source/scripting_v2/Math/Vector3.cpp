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

#include "Vector3.hpp"

namespace GTA
{
	namespace Math
	{
		using namespace System;
		using namespace System::Globalization;

		ref class Random
		{
		public:
			static System::Random ^Instance = gcnew System::Random();
		};

		Vector3::Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		Vector3 Vector3::Normalized::get()
		{
			return Vector3::Normalize(Vector3(X, Y, Z));
		}

		float Vector3::default::get(int index)
		{
			switch (index)
			{
			case 0:
				return X;
			case 1:
				return Y;
			case 2:
				return Z;
			default:
				throw gcnew ArgumentOutOfRangeException("index", "Indices for Vector3 run from 0 to 2, inclusive.");
			}
		}

		void Vector3::default::set(int index, float value)
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
			default:
				throw gcnew ArgumentOutOfRangeException("index", "Indices for Vector3 run from 0 to 2, inclusive.");
			}
		}

		float Vector3::Length()
		{
			return static_cast<float>(System::Math::Sqrt((X*X) + (Y*Y) + (Z*Z)));
		}
		float Vector3::LengthSquared()
		{
			return (X * X) + (Y * Y) + (Z * Z);
		}
		void Vector3::Normalize()
		{
			float length = Length();
			if (length == 0) return;
			float num = 1 / length;
			X *= num;
			Y *= num;
			Z *= num;
		}
		float Vector3::DistanceTo(Vector3 position)
		{
			return (position - *this).Length();
		}
		float Vector3::DistanceToSquared(Vector3 position)
		{
			return DistanceSquared(position, *this);
		}
		float Vector3::DistanceTo2D(Vector3 position)
		{
			Vector3 lhs(X, Y, 0.0f);
			Vector3 rhs(position.X, position.Y, 0.0f);

			return Distance(lhs, rhs);
		}
		float Vector3::DistanceToSquared2D(Vector3 position)
		{
			Vector3 lhs(X, Y, 0.0f);
			Vector3 rhs(position.X, position.Y, 0.0f);

			return DistanceSquared(lhs, rhs);
		}
		float Vector3::Distance(Vector3 position1, Vector3 position2)
		{
			return (position1 - position2).Length();
		}
		float Vector3::DistanceSquared(Vector3 position1, Vector3 position2)
		{
			return (position1 - position2).LengthSquared();
		}
		float Vector3::Distance2D(Vector3 position1, Vector3 position2)
		{
			Vector3 pos1 = Vector3(position1.X, position1.Y, 0);
			Vector3 pos2 = Vector3(position2.X, position2.Y, 0);
			return (pos1 - pos2).Length();
		}
		float Vector3::DistanceSquared2D(Vector3 position1, Vector3 position2)
		{
			Vector3 pos1 = Vector3(position1.X, position1.Y, 0);
			Vector3 pos2 = Vector3(position2.X, position2.Y, 0);
			return (pos1 - pos2).LengthSquared();
		}

		float Vector3::Angle(Vector3 from, Vector3 to)
		{
			double dot = Vector3::Dot(from.Normalized, to.Normalized);
			return (float)(System::Math::Acos((dot)) * (180.0 / System::Math::PI));
		}
		float Vector3::SignedAngle(Vector3 from, Vector3 to, Vector3 planeNormal)
		{
			Vector3 perpVector = Vector3::Cross(planeNormal, from);

			double angle = Vector3::Angle(from, to);
			double dot = Vector3::Dot(perpVector, to);
			if (dot < 0)
			{
				angle *= -1;
			}

			return (float)angle;
		}
		float Vector3::ToHeading()
		{
			return (float)((System::Math::Atan2(X, -Y) + System::Math::PI) * (180.0 / System::Math::PI));
		}

		Vector3 Vector3::Around(float distance)
		{
			return *this + Vector3::RandomXY() * distance;
		}

		Vector3 Vector3::RandomXY()
		{
			Vector3 v;
			double radian = Random::Instance->NextDouble() * 2 * System::Math::PI;

			v.X = (float)(System::Math::Cos(radian));
			v.Y = (float)(System::Math::Sin(radian));
			v.Normalize();
			return v;
		}
		Vector3 Vector3::RandomXYZ()
		{
			Vector3 v;
			double radian = Random::Instance->NextDouble() * 2.0 * System::Math::PI;
			double cosTheta = (Random::Instance->NextDouble() * 2.0) - 1.0;
			double theta = System::Math::Acos(cosTheta);

			v.X = (float)(System::Math::Sin(theta) * System::Math::Cos(radian));
			v.Y = (float)(System::Math::Sin(theta) * System::Math::Sin(radian));
			v.Z = (float)(System::Math::Cos(theta));
			v.Normalize();
			return v;
		}

		Vector3 Vector3::Add(Vector3 left, Vector3 right)
		{
			return Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}
		Vector3 Vector3::Subtract(Vector3 left, Vector3 right)
		{
			return Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}
		Vector3 Vector3::Modulate(Vector3 left, Vector3 right)
		{
			return Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		}
		Vector3 Vector3::Multiply(Vector3 value, float scale)
		{
			return Vector3(value.X * scale, value.Y * scale, value.Z * scale);
		}
		Vector3 Vector3::Divide(Vector3 value, float scale)
		{
			return Vector3(value.X / scale, value.Y / scale, value.Z / scale);
		}
		Vector3 Vector3::Negate(Vector3 value)
		{
			return Vector3(-value.X, -value.Y, -value.Z);
		}
		Vector3 Vector3::Clamp(Vector3 value, Vector3 min, Vector3 max)
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

			return Vector3(x, y, z);
		}
		Vector3 Vector3::Lerp(Vector3 start, Vector3 end, float factor)
		{
			Vector3 vector;

			vector.X = start.X + ((end.X - start.X) * factor);
			vector.Y = start.Y + ((end.Y - start.Y) * factor);
			vector.Z = start.Z + ((end.Z - start.Z) * factor);

			return vector;
		}
		float Vector3::Dot(Vector3 left, Vector3 right)
		{
			return (left.X * right.X + left.Y * right.Y + left.Z * right.Z);
		}
		Vector3 Vector3::Cross(Vector3 left, Vector3 right)
		{
			Vector3 result;
			result.X = left.Y * right.Z - left.Z * right.Y;
			result.Y = left.Z * right.X - left.X * right.Z;
			result.Z = left.X * right.Y - left.Y * right.X;
			return result;
		}
		Vector3 Vector3::Project(Vector3 vector, Vector3 onNormal)
		{
			return onNormal * Dot(vector, onNormal) / Dot(onNormal, onNormal);
		}
		Vector3 Vector3::ProjectOnPlane(Vector3 vector, Vector3 planeNormal)
		{
			return (vector - Project(vector, planeNormal));
		}
		Vector3 Vector3::Reflect(Vector3 vector, Vector3 normal)
		{
			Vector3 result;
			float dot = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);

			result.X = vector.X - ((2.0f * dot) * normal.X);
			result.Y = vector.Y - ((2.0f * dot) * normal.Y);
			result.Z = vector.Z - ((2.0f * dot) * normal.Z);

			return result;
		}
		Vector3 Vector3::Normalize(Vector3 vector)
		{
			vector.Normalize();
			return vector;
		}
		Vector3 Vector3::Minimize(Vector3 left, Vector3 right)
		{
			Vector3 vector;
			vector.X = (left.X < right.X) ? left.X : right.X;
			vector.Y = (left.Y < right.Y) ? left.Y : right.Y;
			vector.Z = (left.Z < right.Z) ? left.Z : right.Z;
			return vector;
		}
		Vector3 Vector3::Maximize(Vector3 left, Vector3 right)
		{
			Vector3 vector;
			vector.X = (left.X > right.X) ? left.X : right.X;
			vector.Y = (left.Y > right.Y) ? left.Y : right.Y;
			vector.Z = (left.Z > right.Z) ? left.Z : right.Z;
			return vector;
		}

		Vector3 Vector3::operator + (Vector3 left, Vector3 right)
		{
			return Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}
		Vector3 Vector3::operator - (Vector3 left, Vector3 right)
		{
			return Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}
		Vector3 Vector3::operator - (Vector3 value)
		{
			return Vector3(-value.X, -value.Y, -value.Z);
		}
		Vector3 Vector3::operator * (Vector3 value, float scale)
		{
			return Vector3(value.X * scale, value.Y * scale, value.Z * scale);
		}
		Vector3 Vector3::operator * (float scale, Vector3 vec)
		{
			return vec * scale;
		}
		Vector3 Vector3::operator / (Vector3 value, float scale)
		{
			return Vector3(value.X / scale, value.Y / scale, value.Z / scale);
		}
		bool Vector3::operator == (Vector3 left, Vector3 right)
		{
			return Vector3::Equals(left, right);
		}
		bool Vector3::operator != (Vector3 left, Vector3 right)
		{
			return !Vector3::Equals(left, right);
		}

		String ^Vector3::ToString()
		{
			return String::Format(CultureInfo::InvariantCulture, "X:{0} Y:{1} Z:{2}", X.ToString(), Y.ToString(), Z.ToString());
		}
		int Vector3::GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() << 2 ^ Z.GetHashCode() >> 2;
		}
		bool Vector3::Equals(Object ^value)
		{
			if (value == nullptr || value->GetType() != GetType())
				return false;

			return Equals(safe_cast<Vector3>(value));
		}
		bool Vector3::Equals(Vector3 value)
		{
			return (X == value.X && Y == value.Y && Z == value.Z);
		}
		bool Vector3::Equals(Vector3 %value1, Vector3 %value2)
		{
			return (value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z);
		}
	}
}