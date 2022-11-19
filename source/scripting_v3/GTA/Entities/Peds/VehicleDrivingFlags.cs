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
		StopForVehicles = 1,
		StopForPeds = 2,
		SwerveAroundAllVehicles = 4,
		SteerAroundStationaryVehicles = 8,
		SteerAroundPeds = 16,
		SteerAroundObjects = 32,
		/// <summary>Don't steer around the player ped even if <see cref="SteerAroundPeds"/> is set.</summary>
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
		UseShortCutLinks = 262144,
		ChangeLanesAroundObstructions = 524288,
		UseSwitchedOffNodes = 2097152,
		DriveBySight = 4194304,
		/// <summary>
		/// Only works for planes using <c>MISSION_GOTO</c>, will cause them to drive along the ground instead of fly.
		/// </summary>
		PlaneTaxiMode = 8388608,
		/// <summary>
		/// Force to go to the target directly instead of following the nodes regardless of the distance config for driving or vehicle mission tasks at which the ai switches to heading for the target directly.
		/// </summary>
		ForceStraightLine = 16777216,
		UseStringPullingAtJunctions = 33554432,
		TryToAvoidHighways = 536870912,
		ForceJoinInRoadDirection = 1073741824,
		StopAtDestination = 2147483648,

		[Obsolete("VehicleDrivingFlags.UseBlinkers is obsolete because it is incorrect, please use VehicleDrivingFlags.GoOffRoadWhenAvoiding instead.")]
		UseBlinkers = 256,
		[Obsolete("VehicleDrivingFlags.FollowTraffic is obsolete because  it has less obvious name, please use VehicleDrivingFlags.StopForVehicles instead.")]
		FollowTraffic = 1,
		[Obsolete("VehicleDrivingFlags.YieldToPeds is obsolete, please use VehicleDrivingFlags.StopForPeds instead.")]
		YieldToPeds = 2,
		[Obsolete("VehicleDrivingFlags.AvoidVehicles is obsolete because has less obvious name, please use VehicleDrivingFlags.SwerveAroundAllVehicles instead.")]
		AvoidVehicles = 4,
		[Obsolete("VehicleDrivingFlags.AvoidEmptyVehicles is obsolete because it is inaccurate, please use VehicleDrivingFlags.SteerAroundStationaryVehicles instead.")]
		AvoidEmptyVehicles = 8, // Even with this flag set, the ped may avoid vehicles that have the drivers when it is stationary
		[Obsolete("VehicleDrivingFlags.AvoidPeds is obsolete because it has less obvious name, please use VehicleDrivingFlags.SteerAroundPeds instead.")]
		AvoidPeds = 16,
		[Obsolete("VehicleDrivingFlags.AvoidObjects is obsolete because it has less obvious name, please use VehicleDrivingFlags.SteerAroundObjects instead.")]
		AvoidObjects = 32,
		[Obsolete("VehicleDrivingFlags.AllowMedianCrossing is obsolete because it is inaccurate, please use VehicleDrivingFlags.UseShortCutLinks instead.")]
		AllowMedianCrossing = 262144,
		[Obsolete("VehicleDrivingFlags.IgnorePathFinding is obsolete because it has less obvious name, please use VehicleDrivingFlags.ForceStraightLine instead.")]
		IgnorePathFinding = 16777216,
	}
}
