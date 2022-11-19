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
		/// <summary>Don't steer around the player ped even if <see cref="AvoidPeds"/> is set.</summary>
		DontSteerAroundPlayerPed = 64,
		StopAtTrafficLights = 128,
		/// <summary>
		/// Make the <see cref="Ped"/> prefer to go off the road rather than enter oncoming lanes when avoiding (steering around) obstacles if the correct lanes are full.
		/// Even if this value is set, the <see cref="Ped"/> will try to steer around obstacles by entering other correct lanes if the correct lanes are not full. 
		/// </summary>
		GoOffRoadWhenAvoiding = 256,
		/// <summary>
		/// Allow the <see cref="Ped"/> to drive into the oncoming traffic if the correct lanes are full.
		/// Even if this value is set, the <see cref="Ped"/> will try to reach the correct lanes again as soon as possible. 
		/// </summary>
		AllowGoingWrongWay = 512,
		Reverse = 1024,
		/// <summary>
		/// If pathfinding fails, cruise randomly instead of going on a straight line.
		/// </summary>
		UseWanderFallbackInsteadOfStraightLine = 2048,
		AvoidRestrictedAreas = 4096,
		/// <summary>
		/// Only works when the car mission is set to MISSION_CRUISE.
		/// </summary>
		PreventBackgroundPathfinding = 8192,
		/// <summary>
		/// Limit the speed based on the road speed if the max cruise speed for driving tasks exceeds the road speed. Only works when the car mission is set to MISSION_CRUISE.
		/// </summary>
		AdjustCruiseSpeedBasedOnRoadSpeed = 16384,
		AllowMedianCrossing = 262144,
		ChangeLanesAroundObstructions = 524288,
		DriveBySight = 4194304,
		/// <summary>
		/// Only works for planes using <c>MISSION_GOTO</c>, will cause them to drive along the ground instead of fly.
		/// </summary>
		PlaneTaxiMode = 8388608,
		IgnorePathFinding = 16777216,
		UseStringPullingAtJunctions = 33554432,
		TryToAvoidHighways = 536870912,
		ForceJoinInRoadDirection = 1073741824,
		StopAtDestination = 2147483648,

		[Obsolete("VehicleDrivingFlags.UseBlinkers is obsolete because it is incorrect, please use VehicleDrivingFlags.GoOffRoadWhenAvoiding instead.")]
		UseBlinkers = 256,
	}
}
