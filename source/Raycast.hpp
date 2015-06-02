#pragma once

#include "Vector3.hpp"
#include "Entity.hpp"

namespace GTA
{
	using namespace GTA::Math;

	public ref class RayCastResult
	{
	public:

		property int Handle
		{
			int get();
		}
		property bool DidHitEntity
		{
			bool get();
		}
		property Entity ^HitEntity
		{
			Entity ^get();
		}
		property Vector3 HitCoords
		{
			Vector3 get();
		}
		property Vector3 UnkVec
		{
			Vector3 get();
		}
		property int Result
		{
			int get();
		}
		property int Intersected
		{
			int get();
		}
	internal:
		RayCastResult(int Handle);
	private:
		int mHandle;
		int mStatus;
		int mIntersected;
		Entity ^mHit;
		Vector3 mHitCoord;
		Vector3 mUnk;

	};
}

