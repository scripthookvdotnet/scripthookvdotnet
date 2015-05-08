#include "CameraShake.hpp"

namespace GTA
{
	const char *CameraShakeNames::GetShakeName(CameraShake shake)
	{
		return CameraShakeNames::Names[0];
	}

	const char * const CameraShakeNames::Names[] = 
	{ 
		"HAND_SHAKE", 
		"SMALL_EXPLOSION_SHAKE", 
		"MEDIUM_EXPLOSION_SHAKE", 
		"LARGE_EXPLOSION_SHAKE",
		"JOLT_SHAKE",
		"VIBRATE_SHAKE",
		"ROAD_VIBRATION_SHAKE",
		"DRUNK_SHAKE",
		"SKY_DIVING_SHAKE",
		"FAMILY5_DRUG_TRIP_SHAKE",
		"DEATH_FAIL_IN_EFFECT_SHAKE"
	};
}