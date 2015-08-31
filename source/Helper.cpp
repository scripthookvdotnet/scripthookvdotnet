#include "Helper.hpp"
namespace GTA
{
	float Helper::RadianToDegree(float radians)
	{
		return radians * _RadianToDegree;
	}
	float Helper::DegreeToRadian(float degrees)
	{
		return degrees / _RadianToDegree;
	}
	Math::Vector3 Helper::HeadingToDirection(float heading)
	{
		heading = DegreeToRadian(heading);
		return Math::Vector3((float)-System::Math::Sin(heading), (float)System::Math::Cos(heading), 0.0f);
	}
	Math::Vector3 Helper::RotationToDirection(Math::Vector3 rotation)
	{
		double rotZ = DegreeToRadian(rotation.Z);
		double rotX = DegreeToRadian(rotation.X);
		double multXY = System::Math::Abs((float)System::Math::Cos(rotX));
		return Math::Vector3((float)-System::Math::Sin(rotZ) * multXY, (float)System::Math::Cos(rotZ) * multXY, (float)System::Math::Sin(rotX));
	}
	float Helper::DirectionToHeading(Math::Vector3 direction)
	{
		direction.Z = 0.0f;
		direction.Normalize();
		return RadianToDegree(-System::Math::Atan2(direction.X, direction.Y));
	}
	Math::Vector3 Helper::DirectionToRotation(Math::Vector3 direction) 
	{
		return DirectionToRotation(direction, 0.0f);
	}
	Math::Vector3 Helper::DirectionToRotation(Math::Vector3 direction, float roll)
	{
		direction.Normalize();
		Math::Vector3 rotation;
		Math::Vector3 v1 = Math::Vector3::Normalize(Math::Vector3(direction.Z, Math::Vector3(direction.X, direction.Y, 0.0f).Length(), 0.0f));
		rotation.X = RadianToDegree(System::Math::Atan2(v1.X, v1.Y));
		rotation.Y = roll;
		rotation.Z = -RadianToDegree(System::Math::Atan2(direction.X, direction.Y));
		return rotation;
	}
}