#pragma once

#include "Vector3.hpp"
#include "Entity.hpp"

namespace GTA
{
	using namespace GTA::Math;
	public enum class IntersectOptions
	{
		Everything = -1,
		Map = 1,
		Mission_Entities = 2,
		Peds1 = 12,//4 and 8 both seem to be peds
		Objects = 16,
		Unk1 = 32,
		Unk2 = 64,
		Unk3 = 128,
		Vegetation = 256,
		Unk4 = 512
	};
	public ref class Ray
	{
	public:
		Ray(int Handle);
		static Ray ^Cast(Vector3 source, Vector3 target, IntersectOptions options) {
			return Cast(source, target, options, 7, nullptr);
		}
		static Ray ^Cast(Vector3 source, Vector3 target, IntersectOptions options, int UnkFlags) {
			return Cast(source, target, options, UnkFlags, nullptr);
		}
		static Ray ^Cast(Vector3 source, Vector3 target, IntersectOptions options, int UnkFlags, Entity ^E);

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
	private:
		int mHandle;
		int mStatus;
		int mIntersected;
		Entity ^mHit;
		Vector3 mHitCoord;
		Vector3 mUnk;

	};
}

