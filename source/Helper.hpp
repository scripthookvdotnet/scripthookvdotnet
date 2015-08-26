#pragma once
#include "Vector3.hpp"
namespace GTA
{
	public ref class Helper
	{
	public:
		static float RadianToDegree(float radians);
		static float DegreeToRadian(float degrees);
		static Math::Vector3 HeadingToDirection(float heading);
		static Math::Vector3 RotationToDirection(Math::Vector3 rotation);
		static float DirectionToHeading(Math::Vector3 direction);
		static Math::Vector3 DirectionToRotation(Math::Vector3 direction, float roll);
		static Math::Vector3 DirectionToRotation(Math::Vector3 direction);
	private:
		static const double _RadianToDegree = 57.295779513082320876798154814105;
	};
}
