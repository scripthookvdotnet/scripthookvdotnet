//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum VehicleDrivingFlags : uint
	{
		None = 0,
		FollowTraffic = 1,
		YieldToPeds = 2,
		AvoidVehicles = 4,
		AvoidEmptyVehicles = 8,
		AvoidPeds = 16,
		AvoidObjects = 32,
		StopAtTrafficLights = 128,
		UseBlinkers = 256,
		AllowGoingWrongWay = 512,
		Reverse = 1024,
		AllowMedianCrossing = 262144,
		DriveBySight = 4194304,
		IgnorePathFinding = 16777216,
		TryToAvoidHighways = 536870912,
		StopAtDestination = 2147483648,
	}
}
