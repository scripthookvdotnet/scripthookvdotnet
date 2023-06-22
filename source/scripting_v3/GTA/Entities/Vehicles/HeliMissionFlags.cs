//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum HeliMissionFlags
	{
		AttainRequestedOrientation = 1,
		DontModifyOrientation = 2,
		DontModifyPitch = 4,
		DontModifyThrottle = 8,
		DontModifyRoll = 16,
		LandOnArrival = 32,
		DontDoAvoidance = 64,
		StartEngineImmediately = 128,
		ForceHeightMapAvoidance = 256,
		DontClampProbesToDestination = 512,
		EnableTimeslicingWhenPossible = 1024,
		CircleOppositeDirection = 2048,
		MaintainHeightAboveTerrain = 4096,
		IgnoreHiddenEntitiesDuringLand = 8192,
		DisableAllHeightMapAvoidance = 16384,

		None = 0,
		HeightMapOnlyAvoidance = ForceHeightMapAvoidance | DontDoAvoidance
	}
}
