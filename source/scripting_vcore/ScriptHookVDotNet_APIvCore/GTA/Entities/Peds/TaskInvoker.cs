//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public class TaskInvoker
	{
		#region Fields
		readonly Ped _ped;
		#endregion

		internal TaskInvoker(Ped ped)
		{
			_ped = ped;
		}

		public void AchieveHeading(float heading, int timeout = 0)
		{
			Function.Call(Hash.TASK_ACHIEVE_HEADING, _ped.Handle, heading, timeout);
		}

		public void AimAt(Entity target, int duration)
		{
			Function.Call(Hash.TASK_AIM_GUN_AT_ENTITY, _ped.Handle, target.Handle, duration, 0);
		}

		public void AimAt(Vector3 target, int duration)
		{
			Function.Call(Hash.TASK_AIM_GUN_AT_COORD, _ped.Handle, target.X, target.Y, target.Z, duration, 0, 0);
		}

		public void Arrest(Ped ped)
		{
			Function.Call(Hash.TASK_ARREST_PED, _ped.Handle, ped.Handle);
		}

		public void ChatTo(Ped ped)
		{
			Function.Call(Hash.TASK_CHAT_TO_PED, _ped.Handle, ped.Handle, 16, 0f, 0f, 0f, 0f, 0f);
		}

		public void Jump()
		{
			Function.Call(Hash.TASK_JUMP, _ped.Handle, true);
		}

		public void Climb()
		{
			Function.Call(Hash.TASK_CLIMB, _ped.Handle, true);
		}

		public void ClimbLadder()
		{
			Function.Call(Hash.TASK_CLIMB_LADDER, _ped.Handle, 1);
		}

		public void Cower(int duration)
		{
			Function.Call(Hash.TASK_COWER, _ped.Handle, duration);
		}

		public void ChaseWithGroundVehicle(Ped target)
		{
			Function.Call(Hash.TASK_VEHICLE_CHASE, _ped.Handle, target.Handle);
		}

		public void ChaseWithHelicopter(Ped target, Vector3 offset)
		{
			Function.Call(Hash.TASK_HELI_CHASE, _ped.Handle, target.Handle, offset.X, offset.Y, offset.Z);
		}

		public void ChaseWithPlane(Ped target, Vector3 offset)
		{
			Function.Call(Hash.TASK_PLANE_CHASE, _ped.Handle, target.Handle, offset.X, offset.Y, offset.Z);
		}

		public void CruiseWithVehicle(Vehicle vehicle, float speed, DrivingStyle style = DrivingStyle.Normal)
		{
			Function.Call(Hash.TASK_VEHICLE_DRIVE_WANDER, _ped.Handle, vehicle.Handle, speed, style);
		}

		public void DriveTo(Vehicle vehicle, Vector3 target, float radius, float speed, DrivingStyle style = DrivingStyle.Normal)
		{
			Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, _ped.Handle, vehicle.Handle, target.X, target.Y, target.Z, speed, style, radius);
		}

		public void EnterAnyVehicle(VehicleSeat seat = VehicleSeat.Any, int timeout = -1, float speed = 1f, EnterVehicleFlags flag = EnterVehicleFlags.None)
		{
			Function.Call(Hash.TASK_ENTER_VEHICLE, _ped.Handle, 0, timeout, seat, speed, flag, 0);
		}

		public void EnterVehicle(Vehicle vehicle, VehicleSeat seat = VehicleSeat.Any, int timeout = -1, float speed = 1f, EnterVehicleFlags flag = EnterVehicleFlags.None)
		{
			Function.Call(Hash.TASK_ENTER_VEHICLE, _ped.Handle, vehicle.Handle, timeout, seat, speed, flag, 0);
		}

		public static void EveryoneLeaveVehicle(Vehicle vehicle)
		{
			Function.Call(Hash.TASK_EVERYONE_LEAVE_VEHICLE, vehicle.Handle);
		}

		public void FightAgainst(Ped target)
		{
			Function.Call(Hash.TASK_COMBAT_PED, _ped.Handle, target.Handle, 0, 16);
		}

		public void FightAgainst(Ped target, int duration)
		{
			Function.Call(Hash.TASK_COMBAT_PED_TIMED, _ped.Handle, target.Handle, duration, 0);
		}

		public void FightAgainstHatedTargets(float radius)
		{
			Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, _ped.Handle, radius, 0);
		}

		public void FightAgainstHatedTargets(float radius, int duration)
		{
			Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED_TIMED, _ped.Handle, radius, duration, 0);
		}

		public void FleeFrom(Ped ped, int duration = -1)
		{
			Function.Call(Hash.TASK_SMART_FLEE_PED, _ped.Handle, ped.Handle, 100f, duration, 0, 0);
		}

		public void FleeFrom(Vector3 position, int duration = -1)
		{
			Function.Call(Hash.TASK_SMART_FLEE_COORD, _ped.Handle, position.X, position.Y, position.Z, 100f, duration, 0, 0);
		}

		public void FollowPointRoute(params Vector3[] points)
		{
			FollowPointRoute(1f, points);
		}

		public void FollowPointRoute(float movementSpeed, params Vector3[] points)
		{
			Function.Call(Hash.TASK_FLUSH_ROUTE);

			foreach (var point in points)
			{
				Function.Call(Hash.TASK_EXTEND_ROUTE, point.X, point.Y, point.Z);
			}

			Function.Call(Hash.TASK_FOLLOW_POINT_ROUTE, _ped.Handle, movementSpeed, 0);
		}

		public void FollowToOffsetFromEntity(Entity target, Vector3 offset, float movementSpeed, int timeout = -1, float distanceToFollow = 10f, bool persistFollowing = true)
		{
			Function.Call(Hash.TASK_FOLLOW_TO_OFFSET_OF_ENTITY, _ped.Handle, target.Handle, offset.X, offset.Y, offset.Z, movementSpeed, timeout, distanceToFollow, persistFollowing);
		}

		public void GoTo(Entity target, Vector3 offset = default(Vector3), int timeout = -1)
		{
			Function.Call(Hash.TASK_GOTO_ENTITY_OFFSET_XY, _ped.Handle, target.Handle, timeout, offset.X, offset.Y, offset.Z, 1f, true);
		}

		public void GoTo(Vector3 position, int timeout = -1)
		{
			Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 1f, timeout, 0f, 0, 0f);
		}

		public void GoStraightTo(Vector3 position, int timeout = -1, float targetHeading = 0f, float distanceToSlide = 0f)
		{
			Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 1f, timeout, targetHeading, distanceToSlide);
		}

		public void GuardCurrentPosition()
		{
			Function.Call(Hash.TASK_GUARD_CURRENT_POSITION, _ped.Handle, 15f, 10f, true);
		}

		public void HandsUp(int duration)
		{
			Function.Call(Hash.TASK_HANDS_UP, _ped.Handle, duration, 0, -1, false);
		}

		public void LandPlane(Vector3 startPosition, Vector3 touchdownPosition, Vehicle plane = null)
		{
			if (plane == null)
			{
				plane = _ped.CurrentVehicle;
			}
			Function.Call(Hash.TASK_PLANE_LAND, _ped.Handle, plane.NativeValue, startPosition.X, startPosition.Y, startPosition.Z, touchdownPosition.X, touchdownPosition.Y, touchdownPosition.Z);
		}

		public void LeaveVehicle(LeaveVehicleFlags flags = LeaveVehicleFlags.None)
		{
			Function.Call(Hash.TASK_LEAVE_ANY_VEHICLE, _ped.Handle, 0, flags);
		}

		public void LeaveVehicle(Vehicle vehicle, bool closeDoor)
		{
			LeaveVehicle(vehicle, closeDoor ? LeaveVehicleFlags.None : LeaveVehicleFlags.LeaveDoorOpen);
		}

		public void LeaveVehicle(Vehicle vehicle, LeaveVehicleFlags flags)
		{
			Function.Call(Hash.TASK_LEAVE_VEHICLE, _ped.Handle, vehicle.Handle, flags);
		}

		public void LookAt(Entity target, int duration = -1)
		{
			Function.Call(Hash.TASK_LOOK_AT_ENTITY, _ped.Handle, target.Handle, duration, 0, 2);
		}

		public void LookAt(Vector3 position, int duration = -1)
		{
			Function.Call(Hash.TASK_LOOK_AT_COORD, _ped.Handle, position.X, position.Y, position.Z, duration, 0, 2);
		}

		public void ParachuteTo(Vector3 position)
		{
			Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, _ped.Handle, position.X, position.Y, position.Z);
		}

		public static void UpdateParachuteTarget(Ped ped, Vector3 position)
		{
			Function.Call(Hash.SET_PARACHUTE_TASK_TARGET, ped.Handle, position.X, position.Y, position.Z);
		}

		public void ParkVehicle(Vehicle vehicle, Vector3 position, float heading, float radius = 20.0f, bool keepEngineOn = false)
		{
			Function.Call(Hash.TASK_VEHICLE_PARK, _ped.Handle, vehicle.Handle, position.X, position.Y, position.Z, heading, 1, radius, keepEngineOn);
		}

		public void PerformSequence(TaskSequence sequence)
		{
			if (!sequence.IsClosed)
			{
				sequence.Close(false);
			}

			ClearAll();
			//_ped.BlockPermanentEvents = true;

			Function.Call(Hash.TASK_PERFORM_SEQUENCE, _ped.Handle, sequence.Handle);
		}

		public void PlayAnimation(string animDict, string animName)
		{
			PlayAnimation(animDict, animName, 8f, -8f, -1, AnimationFlags.None, 0f);
		}

		public void PlayAnimation(string animDict, string animName, float speed, int duration, float playbackRate)
		{
			PlayAnimation(animDict, animName, speed, -speed, duration, AnimationFlags.None, playbackRate);
		}

		public void PlayAnimation(string animDict, string animName, float blendInSpeed, int duration, AnimationFlags flags)
		{
			PlayAnimation(animDict, animName, blendInSpeed, -8f, duration, flags, 0f);
		}

		public void PlayAnimation(string animDict, string animName, float blendInSpeed, float blendOutSpeed, int duration, AnimationFlags flags, float playbackRate)
		{
			Function.Call(Hash.REQUEST_ANIM_DICT, animDict);

			DateTime endtime = DateTime.UtcNow + new TimeSpan(0, 0, 0, 0, 1000);

			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, animDict))
			{
				Script.Yield();

				if (DateTime.UtcNow >= endtime)
				{
					return;
				}
			}

			Function.Call(Hash.TASK_PLAY_ANIM, _ped.Handle, animDict, animName, blendInSpeed, blendOutSpeed, duration, flags, playbackRate, 0, 0, 0);
		}

		public void RappelFromHelicopter()
		{
			Function.Call(Hash.TASK_RAPPEL_FROM_HELI, _ped.Handle, 0x41200000);
		}

		public void ReactAndFlee(Ped ped)
		{
			Function.Call(Hash.TASK_REACT_AND_FLEE_PED, _ped.Handle, ped.Handle);
		}

		public void ReloadWeapon()
		{
			Function.Call(Hash.TASK_RELOAD_WEAPON, _ped.Handle, true);
		}

		public void RunTo(Vector3 position, bool ignorePaths = false, int timeout = -1)
		{
			if (ignorePaths)
			{
				Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 4f, timeout, 0f, 0f);
			}
			else
			{
				Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 4f, timeout, 0f, 0, 0f);
			}
		}

		public void ShootAt(Ped target, int duration = -1, FiringPattern pattern = FiringPattern.Default)
		{
			Function.Call(Hash.TASK_SHOOT_AT_ENTITY, _ped.Handle, target.Handle, duration, pattern);
		}

		public void ShootAt(Vector3 position, int duration = -1, FiringPattern pattern = FiringPattern.Default)
		{
			Function.Call(Hash.TASK_SHOOT_AT_COORD, _ped.Handle, position.X, position.Y, position.Z, duration, pattern);
		}

		public void ShuffleToNextVehicleSeat(Vehicle vehicle = null)
		{
			if (vehicle == null)
			{
				vehicle = _ped.CurrentVehicle;
			}
			Function.Call(Hash.TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT, _ped.Handle, vehicle.NativeValue);
		}

		public void Skydive()
		{
			Function.Call(Hash.TASK_SKY_DIVE, _ped.Handle);
		}

		public void SlideTo(Vector3 position, float heading)
		{
			Function.Call(Hash.TASK_PED_SLIDE_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, heading, 0.7f);
		}

		public void StandStill(int duration)
		{
			Function.Call(Hash.TASK_STAND_STILL, _ped.Handle, duration);
		}

		public void StartScenario(string name, float heading)
		{
			Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, _ped.Handle, name, 0, 1);
		}

		public void StartScenario(string name, Vector3 position, float heading)
		{
			Function.Call(Hash.TASK_START_SCENARIO_AT_POSITION, _ped.Handle, name, position.X, position.Y, position.Z, heading, 0, 0, 1);
		}

		/// <summary>Tells the <see cref="Ped"/> to perform a task when in a <see cref="Vehicle"/> against another <see cref="Vehicle"/>.</summary>
		/// <param name="vehicle">The <see cref="Vehicle"/> to use to achieve the task.</param>
		/// <param name="target">The target <see cref="Vehicle"/>.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task.</param>
		/// <param name="drivingFlags">The driving flags for the task.</param>
		/// <param name="targetReachedDist">The distance at which the AI thinks the target has been reached and the car stops. To pick default value, the parameter can be passed in as <c>-1</c>.</param>
		/// <param name="straightLineDist">The distance at which the AI switches to heading for the target directly instead of following the nodes. To pick default value, the parameter can be passed in as <c>-1</c>.</param>
		/// <param name="driveAgainstTraffic">if set to <see langword="true" />, allows the car to drive on the opposite side of the road into incoming traffic.</param>
		public void StartVehicleMission(Vehicle vehicle, Vehicle target, VehicleMissionType missionType, float cruiseSpeed, VehicleDrivingFlags drivingFlags, float targetReachedDist, float straightLineDist, bool driveAgainstTraffic = true)
		{
			Function.Call(Hash.TASK_VEHICLE_MISSION, _ped.Handle, vehicle.Handle, target.Handle, missionType, cruiseSpeed, drivingFlags, targetReachedDist, straightLineDist, driveAgainstTraffic);
		}

		/// <summary>Tells the <see cref="Ped"/> to target another ped with a vehicle.</summary>
		/// <param name="vehicle">The <see cref="Vehicle"/> to use to achieve the task.</param>
		/// <param name="target">The target <see cref="Ped"/>.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task.</param>
		/// <param name="drivingFlags">The driving flags for the task.</param>
		/// <param name="targetReachedDist">The distance at which the AI thinks the target has been reached and the car stops. To pick default value, the parameter can be passed in as <c>-1</c>.</param>
		/// <param name="straightLineDist">The distance at which the AI switches to heading for the target directly instead of following the nodes. To pick default value, the parameter can be passed in as <c>-1</c>.</param>
		/// <param name="driveAgainstTraffic">if set to <see langword="true" />, allows the car to drive on the opposite side of the road into incoming traffic.</param>
		public void StartVehicleMission(Vehicle vehicle, Ped target, VehicleMissionType missionType, float cruiseSpeed, VehicleDrivingFlags drivingFlags, float targetReachedDist, float straightLineDist, bool driveAgainstTraffic = true)
		{
			Function.Call(Hash.TASK_VEHICLE_MISSION_PED_TARGET, _ped.Handle, vehicle.Handle, target.Handle, missionType, cruiseSpeed, drivingFlags, targetReachedDist, straightLineDist, driveAgainstTraffic);
		}

		/// <summary>Tells the <see cref="Ped"/> to target a coord with a <see cref="Vehicle"/>.</summary>
		/// <param name="vehicle">The <see cref="Vehicle"/> to use to achieve the task.</param>
		/// <param name="target">The target coordinates.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task.</param>
		/// <param name="drivingFlags">The driving flags for the task.</param>
		/// <param name="targetReachedDist">The distance at which the AI thinks the target has been reached and the car stops. To pick default value, the parameter can be passed in as <c>-1</c>.</param>
		/// <param name="straightLineDist">The distance at which the AI switches to heading for the target directly instead of following the nodes. To pick default value, the parameter can be passed in as <c>-1</c>.</param>
		/// <param name="driveAgainstTraffic">if set to <see langword="true" />, allows the car to drive on the opposite side of the road into incoming traffic.</param>
		public void StartVehicleMission(Vehicle vehicle, Vector3 target, VehicleMissionType missionType, float cruiseSpeed, VehicleDrivingFlags drivingFlags, float targetReachedDist, float straightLineDist, bool driveAgainstTraffic = true)
		{
			Function.Call(Hash.TASK_VEHICLE_MISSION_COORS_TARGET, _ped.Handle, vehicle.Handle, target.X, target.Y, target.Z, missionType, cruiseSpeed, drivingFlags, targetReachedDist, straightLineDist, driveAgainstTraffic);
		}

		/// <summary>Gives the helicopter a mission.</summary>
		/// <param name="heli">The helicopter.</param>
		/// <param name="target">The target <see cref="Vehicle"/>.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task.</param>
		/// <param name="targetReachedDist">distance (in meters) at which heli thinks it's arrived. Also used as the hover distance for <see cref="VehicleMissionType.Attack"/> and <see cref="VehicleMissionType.Circle"/></param>
		/// <param name="flightHeight">The Z coordinate the heli tries to maintain (i.e. 30 == 30 meters above sea level).</param>
		/// <param name="minHeightAboveTerrain">The height in meters that the heli will try to stay above terrain (ie 20 == always tries to stay at least 20 meters above ground).</param>
		/// <param name="heliOrientation">The orientation the heli tries to be in (<c>0f</c> to <c>360f</c>). Use <c>-1f</c> if not bothered. <c>-1f</c> Should be used in 99% of the times.</param>
		/// <param name="slowDownDistance">In general, get more control with big number and more dynamic with smaller. Setting to <c>-1</c> means use default tuning(<c>100</c>).</param>
		/// <param name="missionFlags">The heli mission flags for the task.</param>
		public void StartHeliMission(Vehicle heli, Vehicle target, VehicleMissionType missionType, float cruiseSpeed, float targetReachedDist, int flightHeight, int minHeightAboveTerrain, float heliOrientation = -1f, float slowDownDistance = -1f, HeliMissionFlags missionFlags = HeliMissionFlags.None)
		{
			Function.Call(Hash.TASK_HELI_MISSION, _ped.Handle, heli.Handle, target.Handle, 0, 0f, 0f, 0f, missionType, cruiseSpeed, targetReachedDist, heliOrientation, flightHeight, minHeightAboveTerrain, slowDownDistance, missionFlags);
		}

		/// <summary>Gives the helicopter a mission.</summary>
		/// <param name="heli">The helicopter.</param>
		/// <param name="target">The target <see cref="Ped"/>.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task.</param>
		/// <param name="targetReachedDist">distance (in meters) at which heli thinks it's arrived. Also used as the hover distance for <see cref="VehicleMissionType.Attack"/> and <see cref="VehicleMissionType.Circle"/></param>
		/// <param name="flightHeight">The Z coordinate the heli tries to maintain (i.e. 30 == 30 meters above sea level).</param>
		/// <param name="minHeightAboveTerrain">The height in meters that the heli will try to stay above terrain (ie 20 == always tries to stay at least 20 meters above ground).</param>
		/// <param name="heliOrientation">The orientation the heli tries to be in (<c>0f</c> to <c>360f</c>). Use <c>-1f</c> if not bothered. <c>-1f</c> Should be used in 99% of the times.</param>
		/// <param name="slowDownDistance">In general, get more control with big number and more dynamic with smaller. Setting to <c>-1</c> means use default tuning(<c>100</c>).</param>
		/// <param name="missionFlags">The heli mission flags for the task.</param>
		public void StartHeliMission(Vehicle heli, Ped target, VehicleMissionType missionType, float cruiseSpeed, float targetReachedDist, int flightHeight, int minHeightAboveTerrain, float heliOrientation = -1f, float slowDownDistance = -1f, HeliMissionFlags missionFlags = HeliMissionFlags.None)
		{
			Function.Call(Hash.TASK_HELI_MISSION, _ped.Handle, heli.Handle, 0, target.Handle, 0f, 0f, 0f, missionType, cruiseSpeed, targetReachedDist, heliOrientation, flightHeight, minHeightAboveTerrain, slowDownDistance, missionFlags);
		}

		/// <summary>Gives the helicopter a mission.</summary>
		/// <param name="heli">The helicopter.</param>
		/// <param name="target">The target coodinate.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task.</param>
		/// <param name="targetReachedDist">distance (in meters) at which heli thinks it's arrived. Also used as the hover distance for <see cref="VehicleMissionType.Attack"/> and <see cref="VehicleMissionType.Circle"/></param>
		/// <param name="flightHeight">The Z coordinate the heli tries to maintain (i.e. 30 == 30 meters above sea level).</param>
		/// <param name="minHeightAboveTerrain">The height in meters that the heli will try to stay above terrain (ie 20 == always tries to stay at least 20 meters above ground).</param>
		/// <param name="heliOrientation">The orientation the heli tries to be in (<c>0f</c> to <c>360f</c>). Use <c>-1f</c> if not bothered. <c>-1f</c> Should be used in 99% of the times.</param>
		/// <param name="slowDownDistance">In general, get more control with big number and more dynamic with smaller. Setting to <c>-1</c> means use default tuning(<c>100</c>).</param>
		/// <param name="missionFlags">The heli mission flags for the task.</param>
		public void StartHeliMission(Vehicle heli, Vector3 target, VehicleMissionType missionType, float cruiseSpeed, float targetReachedDist, int flightHeight, int minHeightAboveTerrain, float heliOrientation = -1f, float slowDownDistance = -1f, HeliMissionFlags missionFlags = HeliMissionFlags.None)
		{
			Function.Call(Hash.TASK_HELI_MISSION, _ped.Handle, heli.Handle, 0, 0, target.X, target.Y, target.Z, missionType, cruiseSpeed, targetReachedDist, heliOrientation, flightHeight, minHeightAboveTerrain, slowDownDistance, missionFlags);
		}

		/// <summary>Gives the plane a mission.</summary>
		/// <param name="plane">The helicopter.</param>
		/// <param name="target">The target <see cref="Vehicle"/>.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task.</param>
		/// <param name="targetReachedDist">distance (in meters) at which heli thinks it's arrived. Also used as the hover distance for <see cref="VehicleMissionType.Attack"/> and <see cref="VehicleMissionType.Circle"/></param>
		/// <param name="flightHeight">The Z coordinate the heli tries to maintain (i.e. 30 == 30 meters above sea level).</param>
		/// <param name="minHeightAboveTerrain">The height in meters that the heli will try to stay above terrain (ie 20 == always tries to stay at least 20 meters above ground).</param>
		/// <param name="planeOrientation">The orientation the plane tries to be in (<c>0f</c> to <c>360f</c>). Use <c>-1f</c> if not bothered. <c>-1f</c> Should be used in 99% of the times.</param>
#pragma warning disable CS1573
		// More rearch needed for the parameter "precise". Even one of the leaked source didn't have the info for "bPrecise".
		public void StartPlaneMission(Vehicle plane, Vehicle target, VehicleMissionType missionType, float cruiseSpeed, float targetReachedDist, int flightHeight, int minHeightAboveTerrain, float planeOrientation = -1f, bool precise = true)
#pragma warning restore CS1573
		{
			Function.Call(Hash.TASK_PLANE_MISSION, _ped.Handle, plane.Handle, target.Handle, 0, 0f, 0f, 0f, missionType, cruiseSpeed, targetReachedDist, planeOrientation, flightHeight, minHeightAboveTerrain, precise);
		}

		/// <summary>Gives the plane a mission.</summary>
		/// <param name="plane">The helicopter.</param>
		/// <param name="target">The target <see cref="Ped"/>.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task.</param>
		/// <param name="targetReachedDist">distance (in meters) at which heli thinks it's arrived. Also used as the hover distance for <see cref="VehicleMissionType.Attack"/> and <see cref="VehicleMissionType.Circle"/></param>
		/// <param name="flightHeight">The Z coordinate the heli tries to maintain (i.e. 30 == 30 meters above sea level).</param>
		/// <param name="minHeightAboveTerrain">The height in meters that the heli will try to stay above terrain (ie 20 == always tries to stay at least 20 meters above ground).</param>
		/// <param name="planeOrientation">The orientation the plane tries to be in (<c>0f</c> to <c>360f</c>). Use <c>-1f</c> if not bothered. <c>-1f</c> Should be used in 99% of the times.</param>
#pragma warning disable CS1573
		public void StartPlaneMission(Vehicle plane, Ped target, VehicleMissionType missionType, float cruiseSpeed, float targetReachedDist, int flightHeight, int minHeightAboveTerrain, float planeOrientation = -1f, bool precise = true)
#pragma warning restore CS1573
		{
			Function.Call(Hash.TASK_PLANE_MISSION, _ped.Handle, plane.Handle, 0, target.Handle, 0f, 0f, 0f, missionType, cruiseSpeed, targetReachedDist, planeOrientation, flightHeight, minHeightAboveTerrain, precise);
		}

		/// <summary>Gives the plane a mission.</summary>
		/// <param name="plane">The plane.</param>
		/// <param name="target">The target coodinate.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task.</param>
		/// <param name="targetReachedDist">distance (in meters) at which heli thinks it's arrived. Also used as the hover distance for <see cref="VehicleMissionType.Attack"/> and <see cref="VehicleMissionType.Circle"/></param>
		/// <param name="flightHeight">The Z coordinate the heli tries to maintain (i.e. 30 == 30 meters above sea level).</param>
		/// <param name="minHeightAboveTerrain">The height in meters that the heli will try to stay above terrain (ie 20 == always tries to stay at least 20 meters above ground).</param>
		/// <param name="planeOrientation">The orientation the plane tries to be in (<c>0f</c> to <c>360f</c>). Use <c>-1f</c> if not bothered. <c>-1f</c> Should be used in 99% of the times.</param>
#pragma warning disable CS1573
		public void StartPlaneMission(Vehicle plane, Vector3 target, VehicleMissionType missionType, float cruiseSpeed, float targetReachedDist, int flightHeight, int minHeightAboveTerrain, float planeOrientation = -1f, bool precise = true)
#pragma warning restore CS1573
		{
			Function.Call(Hash.TASK_PLANE_MISSION, _ped.Handle, plane.Handle, 0, 0, target.X, target.Y, target.Z, missionType, cruiseSpeed, targetReachedDist, planeOrientation, flightHeight, minHeightAboveTerrain, precise);
		}

		/// <summary>Gives the boat a mission.</summary>
		/// <param name="boat">The boat.</param>
		/// <param name="target">The target <see cref="Vehicle"/>.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task.</param>
		/// <param name="drivingFlags">The driving flags for the task.</param>
		/// <param name="targetReachedDist">distance (in meters) at which boat thinks it's arrived. Also used as the hover distance for <see cref="VehicleMissionType.Attack"/> and <see cref="VehicleMissionType.Circle"/></param>
		/// <param name="missionFlags">The boat mission flags for the task.</param>
		public void StartBoatMission(Vehicle boat, Vehicle target, VehicleMissionType missionType, float cruiseSpeed, VehicleDrivingFlags drivingFlags, float targetReachedDist, BoatMissionFlags missionFlags)
		{
			Function.Call(Hash.TASK_BOAT_MISSION, _ped.Handle, boat.Handle, target.Handle, 0, 0f, 0f, 0f, missionType, cruiseSpeed, drivingFlags, targetReachedDist, missionFlags);
		}

		/// <summary>Gives the boat a mission.</summary>
		/// <param name="boat">The boat.</param>
		/// <param name="target">The target <see cref="Ped"/>.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task.</param>
		/// <param name="drivingFlags">The driving flags for the task.</param>
		/// <param name="targetReachedDist">distance (in meters) at which boat thinks it's arrived. Also used as the hover distance for <see cref="VehicleMissionType.Attack"/> and <see cref="VehicleMissionType.Circle"/></param>
		/// <param name="missionFlags">The boat mission flags for the task.</param>
		public void StartBoatMission(Vehicle boat, Ped target, VehicleMissionType missionType, float cruiseSpeed, VehicleDrivingFlags drivingFlags, float targetReachedDist, BoatMissionFlags missionFlags)
		{
			Function.Call(Hash.TASK_BOAT_MISSION, _ped.Handle, boat.Handle, 0, target.Handle, 0f, 0f, 0f, missionType, cruiseSpeed, drivingFlags, targetReachedDist, missionFlags);
		}

		/// <summary>Gives the boat a mission.</summary>
		/// <param name="boat">The boat.</param>
		/// <param name="target">The target coordinate.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task.</param>
		/// <param name="drivingFlags">The driving flags for the task.</param>
		/// <param name="targetReachedDist">distance (in meters) at which boat thinks it's arrived. Also used as the hover distance for <see cref="VehicleMissionType.Attack"/> and <see cref="VehicleMissionType.Circle"/></param>
		/// <param name="missionFlags">The boat mission flags for the task.</param>
		public void StartBoatMission(Vehicle boat, Vector3 target, VehicleMissionType missionType, float cruiseSpeed, VehicleDrivingFlags drivingFlags, float targetReachedDist, BoatMissionFlags missionFlags)
		{
			Function.Call(Hash.TASK_BOAT_MISSION, _ped.Handle, boat.Handle, 0, 0, target.X, target.Y, target.Z, missionType, cruiseSpeed, drivingFlags, targetReachedDist, missionFlags);
		}

		public void SwapWeapon()
		{
			Function.Call(Hash.TASK_SWAP_WEAPON, _ped.Handle, false);
		}

		public void TurnTo(Entity target, int duration = -1)
		{
			Function.Call(Hash.TASK_TURN_PED_TO_FACE_ENTITY, _ped.Handle, target.Handle, duration);
		}

		public void TurnTo(Vector3 position, int duration = -1)
		{
			Function.Call(Hash.TASK_TURN_PED_TO_FACE_COORD, _ped.Handle, position.X, position.Y, position.Z, duration);
		}

		public void UseParachute()
		{
			Function.Call(Hash.TASK_PARACHUTE, _ped.Handle, true);
		}

		public void UseMobilePhone()
		{
			Function.Call(Hash.TASK_USE_MOBILE_PHONE, _ped.Handle, true);
		}

		public void UseMobilePhone(int duration)
		{
			Function.Call(Hash.TASK_USE_MOBILE_PHONE_TIMED, _ped.Handle, duration);
		}

		public void PutAwayParachute()
		{
			Function.Call(Hash.TASK_PARACHUTE, _ped.Handle, false);
		}

		public void PutAwayMobilePhone()
		{
			Function.Call(Hash.TASK_USE_MOBILE_PHONE, _ped.Handle, false);
		}

		public void VehicleChase(Ped target)
		{
			Function.Call(Hash.TASK_VEHICLE_CHASE, _ped.Handle, target.Handle);
		}

		public void VehicleShootAtPed(Ped target)
		{
			Function.Call(Hash.TASK_VEHICLE_SHOOT_AT_PED, _ped.Handle, target.Handle, 20f);
		}

		public void Wait(int duration)
		{
			Function.Call(Hash.TASK_PAUSE, _ped.Handle, duration);
		}

		public void WanderAround()
		{
			Function.Call(Hash.TASK_WANDER_STANDARD, _ped.Handle, 0, 0);
		}

		public void WanderAround(Vector3 position, float radius)
		{
			Function.Call(Hash.TASK_WANDER_IN_AREA, _ped.Handle, position.X, position.Y, position.Z, radius, 0, 0);
		}

		public void WarpIntoVehicle(Vehicle vehicle, VehicleSeat seat)
		{
			Function.Call(Hash.TASK_WARP_PED_INTO_VEHICLE, _ped.Handle, vehicle.Handle, seat);
		}

		public void WarpOutOfVehicle(Vehicle vehicle)
		{
			Function.Call(Hash.TASK_LEAVE_VEHICLE, _ped.Handle, vehicle.Handle, 16);
		}

		public void ClearAll()
		{
			Function.Call(Hash.CLEAR_PED_TASKS, _ped.Handle);
		}

		public void ClearAllImmediately()
		{
			Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, _ped.Handle);
		}

		public void ClearLookAt()
		{
			Function.Call(Hash.TASK_CLEAR_LOOK_AT, _ped.Handle);
		}

		public void ClearSecondary()
		{
			Function.Call(Hash.CLEAR_PED_SECONDARY_TASK, _ped.Handle);
		}

		public void ClearAnimation(string animSet, string animName)
		{
			Function.Call(Hash.STOP_ANIM_TASK, _ped.Handle, animSet, animName, -4f);
		}
	}
}
