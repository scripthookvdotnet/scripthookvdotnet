//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;

namespace GTA
{
	/// <summary>
	/// A set of flags to define how <see cref="Ped"/>s should drive vehicles.
	/// </summary>
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
		/// <summary>
		/// Allow the <see cref="Ped"/> to use short cut links (e.g. the 180? turns on the highways without the direction sign).
		/// </summary>
		UseShortCutLinks = 262144,
		/// <summary>
		/// Make the driver change lanes around obstructions.
		/// Without this flag, even small obstacles make the driver completely change lanes.
		/// </summary>
		ChangeLanesAroundObstructions = 524288,
		/// <summary>
		/// Allow the <see cref="Ped"/> to drive on switched off nodes, which are usually located at paths whose colors on the map are darker than roads for driving (e.g. some dirt roads), and on parking lots.
		/// You can check if some nodes are marked as switched off with <c>GET_VEHICLE_NODE_IS_SWITCHED_OFF</c>.
		/// </summary>
		UseSwitchedOffNodes = 2097152,
		/// <summary>
		/// Make <see cref="Ped"/> prefer navigation mesh routes rather than vehicle nodes.
		/// Can be useful if you're going to be primarily driving off road.
		/// </summary>
		PreferNavmeshRoute = 4194304,
		/// <summary>
		/// Only works for planes using <c>MISSION_GOTO</c>, will cause them to drive along the ground instead of fly.
		/// </summary>
		PlaneTaxiMode = 8388608,
		/// <summary>
		/// Force to go to the target directly instead of following the nodes regardless of the distance config for driving or vehicle mission tasks at which the ai switches to heading for the target directly.
		/// </summary>
		ForceStraightLine = 16777216,
		UseStringPullingAtJunctions = 33554432,
		/// <summary>
		/// Avoid the highway unless the <see cref="Ped"/> has to drive on it to achieve the vehicle task.
		/// </summary>
		TryToAvoidHighways = 536870912,
		ForceJoinInRoadDirection = 1073741824,
		StopAtDestination = 2147483648,

		/// <summary>Standard driving mode. stops for cars, peds, and lights, goes around stationary obstructions, and obey lights.</summary>
		DrivingModeStopForVehicles = StopForVehicles | StopForPeds | SteerAroundObjects | SteerAroundStationaryVehicles | StopAtTrafficLights | UseShortCutLinks | ChangeLanesAroundObstructions,
		/// <summary>Like <see cref="DrivingModeStopForVehicles"/>, but doesn't steer around anything in its way - will only wait instead (doesn't deviate an inch).</summary>
		DrivingModeStopForVehiclesStrict = StopForVehicles | StopForPeds | StopAtTrafficLights | UseShortCutLinks,
		/// <summary>Default "alerted" driving mode. Drives around everything, doesn't obey lights.</summary>
		DrivingModeAvoidVehicles = SwerveAroundAllVehicles | SteerAroundObjects | UseShortCutLinks | ChangeLanesAroundObstructions | StopForVehicles,
		/// <summary>Very erratic driving. difference between this and <see cref="DrivingModeAvoidVehicles"/> is that it doesn't use the brakes at ALL to help with steering.</summary>
		DrivingModeAvoidVehiclesReckless = SwerveAroundAllVehicles | SteerAroundObjects | UseShortCutLinks | ChangeLanesAroundObstructions,
		/// <summary>Smashes through everything.</summary>
		DrivingModePloughThrough = UseShortCutLinks,
		/// <summary>Drives normally except for the fact that it ignores lights.</summary>
		DrivingModeStopForVehiclesIgnoreLights = StopForVehicles | SteerAroundStationaryVehicles | StopForPeds | SteerAroundObjects | UseShortCutLinks | ChangeLanesAroundObstructions,
		/// <summary>Try to swerve around everything, but stop for lights if necessary.</summary>
		DrivingModeAvoidVehiclesObeyLights = SwerveAroundAllVehicles | StopAtTrafficLights | SteerAroundObjects | UseShortCutLinks | ChangeLanesAroundObstructions | StopForVehicles,
		/// <summary>Swerve around cars, be careful around peds, and stop for lights.</summary>
		DrivingModeAvoidVehiclesStopForPedsObeyLights = SwerveAroundAllVehicles | StopAtTrafficLights | StopForPeds | SteerAroundObjects | UseShortCutLinks | ChangeLanesAroundObstructions | StopForVehicles,

		[Obsolete("VehicleDrivingFlags.UseBlinkers is obsolete because it is incorrect, use VehicleDrivingFlags.GoOffRoadWhenAvoiding instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		UseBlinkers = GoOffRoadWhenAvoiding,
		[Obsolete("VehicleDrivingFlags.FollowTraffic is obsolete because it has less obvious name, use VehicleDrivingFlags.StopForVehicles instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		FollowTraffic = StopForVehicles,
		[Obsolete("VehicleDrivingFlags.YieldToPeds is obsolete, use VehicleDrivingFlags.StopForPeds instead."),
		 EditorBrowsable(EditorBrowsableState.Never)]
		YieldToPeds = StopForPeds,
		[Obsolete("VehicleDrivingFlags.AvoidVehicles is obsolete because has less obvious name, use VehicleDrivingFlags.SwerveAroundAllVehicles instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		AvoidVehicles = SwerveAroundAllVehicles,
		/// <summary>
		/// Use <see cref="SteerAroundStationaryVehicles"/> instead.
		/// Even with this flag set, the ped may avoid vehicles that have drivers when they are stationary.
		/// </summary>
		[Obsolete("VehicleDrivingFlags.AvoidEmptyVehicles is obsolete because it is inaccurate, use VehicleDrivingFlags.SteerAroundStationaryVehicles instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		AvoidEmptyVehicles = SteerAroundStationaryVehicles,
		[Obsolete("VehicleDrivingFlags.AvoidPeds is obsolete because it has less obvious name, use VehicleDrivingFlags.SteerAroundPeds instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		AvoidPeds = SteerAroundPeds,
		[Obsolete("VehicleDrivingFlags.AvoidObjects is obsolete because it has less obvious name, use VehicleDrivingFlags.SteerAroundObjects instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		AvoidObjects = SteerAroundObjects,
		[Obsolete("VehicleDrivingFlags.AllowMedianCrossing is obsolete because it is inaccurate, use VehicleDrivingFlags.UseShortCutLinks instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		AllowMedianCrossing = UseShortCutLinks,
		[Obsolete("VehicleDrivingFlags.IgnorePathFinding is obsolete because it has less obvious name, use VehicleDrivingFlags.ForceStraightLine instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		IgnorePathFinding = ForceStraightLine,
		[Obsolete("VehicleDrivingFlags.DriveBySight is obsolete because it is inaccurate, use VehicleDrivingFlags.PreferNavmeshRoute instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		DriveBySight = PreferNavmeshRoute,
	}
}
