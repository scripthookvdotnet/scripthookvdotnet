#pragma once

namespace GTA
{
	public enum class CameraShake : int
	{
		Hand = 0,
		SmallExplosion,
		MediumExplosion,
		LargeExplosion,
		Jolt,
		Vibrate,
		RoadVibration,
		Drunk,
		SkyDiving,
		FamilyDrugTrip,
		DeathFail
	};

	private class CameraShakeNames
	{
	public:
		static const char *GetShakeName(CameraShake shake);

	private:
		static const char * const Names[];
	};
}