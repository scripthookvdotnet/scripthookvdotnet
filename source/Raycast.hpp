#pragma once

#include "Vector3.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Entity;
	#pragma endregion

	public value class RaycastResult
	{
	public:
		property int Result
		{
			int get();
		}
		property bool DitHitEntity
		{
			bool get();
		}
		property bool DitHitAnything
		{
			bool get();
		}
		property Entity ^HitEntity
		{
			Entity ^get();
		}
		property Math::Vector3 HitCoords
		{
			Math::Vector3 get();
		}
		property Math::Vector3 SurfaceNormal
		{
			Math::Vector3 get();
		}

	internal:
		RaycastResult(int handle);

	private:
		int mResult;
		bool mDidHit;
		Entity ^mHitEntity;
		Math::Vector3 mHitCoords;
		Math::Vector3 mSurfaceNormal;
	};
}

