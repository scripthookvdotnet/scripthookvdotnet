//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public class Tasks
	{
		Ped _ped;

		internal Tasks(Ped ped)
		{
			_ped = ped;
		}

		public void AchieveHeading(float heading)
		{
			AchieveHeading(heading, 0);
		}
		public void AchieveHeading(float heading, int timeout)
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
			Function.Call(Hash.TASK_CHAT_TO_PED, _ped.Handle, ped.Handle, 16, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
		}
		public void Climb()
		{
			Function.Call(Hash.TASK_CLIMB, _ped.Handle, true);
		}
		public void Cower(int duration)
		{
			Function.Call(Hash.TASK_COWER, _ped.Handle, duration);
		}
		public void CruiseWithVehicle(Vehicle vehicle, float speed)
		{
			CruiseWithVehicle(vehicle, speed, 0);
		}
		public void CruiseWithVehicle(Vehicle vehicle, float speed, int drivingstyle)
		{
			Function.Call(Hash.TASK_VEHICLE_DRIVE_WANDER, _ped.Handle, vehicle.Handle, speed, drivingstyle);
		}
		public void DriveTo(Vehicle vehicle, Vector3 position, float radius, float speed)
		{
			DriveTo(vehicle, position, radius, speed, 0);
		}
		public void DriveTo(Vehicle vehicle, Vector3 position, float radius, float speed, int drivingstyle)
		{
			Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, _ped.Handle, vehicle.Handle, position.X, position.Y, position.Z, speed, drivingstyle, radius);
		}
		public void EnterVehicle()
		{
			EnterVehicle(new Vehicle(0), VehicleSeat.Any, -1, 0.0f, 0);
		}
		public void EnterVehicle(Vehicle vehicle, VehicleSeat seat)
		{
			EnterVehicle(vehicle, seat, -1, 0.0f, 0);
		}
		public void EnterVehicle(Vehicle vehicle, VehicleSeat seat, int timeout)
		{
			EnterVehicle(vehicle, seat, timeout, 0.0f, 0);
		}
		public void EnterVehicle(Vehicle vehicle, VehicleSeat seat, int timeout, float speed)
		{
			EnterVehicle(vehicle, seat, timeout, speed, 0);
		}
		public void EnterVehicle(Vehicle vehicle, VehicleSeat seat, int timeout, float speed, int flag)
		{
			Function.Call(Hash.TASK_ENTER_VEHICLE, _ped.Handle, vehicle.Handle, timeout, (int)(seat), speed, flag, 0);
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
		public void FleeFrom(Ped ped)
		{
			FleeFrom(ped, -1);
		}
		public void FleeFrom(Ped ped, int duration)
		{
			Function.Call(Hash.TASK_SMART_FLEE_PED, _ped.Handle, ped.Handle, 100.0f, duration, 0, 0);
		}
		public void FleeFrom(Vector3 position)
		{
			FleeFrom(position, -1);
		}
		public void FleeFrom(Vector3 position, int duration)
		{
			Function.Call(Hash.TASK_SMART_FLEE_COORD, _ped.Handle, position.X, position.Y, position.Z, 100.0f, duration, 0, 0);
		}
		public void FollowPointRoute(params Vector3[] points)
		{
			Function.Call(Hash.TASK_FLUSH_ROUTE);

			foreach (Vector3 point in points)
			{
				Function.Call(Hash.TASK_EXTEND_ROUTE, point.X, point.Y, point.Z);
			}

			Function.Call(Hash.TASK_FOLLOW_POINT_ROUTE, _ped.Handle, 1.0f, 0);
		}
		public void GoTo(Entity target)
		{
			GoTo(target, Vector3.Zero, -1);
		}
		public void GoTo(Entity target, Vector3 offset)
		{
			GoTo(target, offset, -1);
		}
		public void GoTo(Entity target, Vector3 offset, int timeout)
		{
			Function.Call(Hash.TASK_GOTO_ENTITY_OFFSET_XY, _ped.Handle, target.Handle, timeout, offset.X, offset.Y, offset.Z, 1.0f, true);
		}
		public void GoTo(Vector3 position)
		{
			GoTo(position, false, -1);
		}
		public void GoTo(Vector3 position, bool ignorePaths)
		{
			GoTo(position, ignorePaths, -1);
		}
		public void GoTo(Vector3 position, bool ignorePaths, int timeout)
		{
			if (ignorePaths)
			{
				Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 1.0f, timeout, 0.0f /* heading */, 0.0f);
			}
			else
			{
				Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 1.0f, timeout, 0.0f, 0, 0.0f);
			}
		}
		public void FollowToOffsetFromEntity(Entity target, Vector3 offset, int timeout, float stoppingRange)
		{
			FollowToOffsetFromEntity(target, offset, 1.0f, timeout, stoppingRange, true);
		}
		public void FollowToOffsetFromEntity(Entity target, Vector3 offset, float movementSpeed, int timeout, float stoppingRange, bool persistFollowing)
		{
			Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, target.Handle, offset.X, offset.Y, offset.Z, movementSpeed, timeout, stoppingRange, persistFollowing);
		}
		public void GuardCurrentPosition()
		{
			Function.Call(Hash.TASK_GUARD_CURRENT_POSITION, _ped.Handle, 15.0f, 10.0f, true);
		}
		public void HandsUp(int duration)
		{
			Function.Call(Hash.TASK_HANDS_UP, _ped.Handle, duration, 0, -1, false);
		}
		public void Jump()
		{
			Function.Call(Hash.TASK_JUMP, _ped.Handle, true);
		}
		public void LeaveVehicle()
		{
			Function.Call(Hash.TASK_LEAVE_ANY_VEHICLE, _ped.Handle, 0, 0 /* flags */);
		}
		public void LeaveVehicle(Vehicle vehicle, bool closeDoor)
		{
			Function.Call(Hash.TASK_LEAVE_VEHICLE, _ped.Handle, vehicle.Handle, closeDoor ? 0 : 1 << 8);
		}
		public void LeaveVehicle(LeaveVehicleFlags flags)
		{
			Function.Call(Hash.TASK_LEAVE_ANY_VEHICLE, _ped.Handle, 0, (int)(flags));
		}
		public void LeaveVehicle(Vehicle vehicle, LeaveVehicleFlags flags)
		{
			Function.Call(Hash.TASK_LEAVE_VEHICLE, _ped.Handle, vehicle.Handle, (int)(flags));
		}
		public void LookAt(Entity target)
		{
			LookAt(target, -1);
		}
		public void LookAt(Entity target, int duration)
		{
			Function.Call(Hash.TASK_LOOK_AT_ENTITY, _ped.Handle, target.Handle, duration, 0 /* flags */, 2);
		}
		public void LookAt(Vector3 position)
		{
			LookAt(position, -1);
		}
		public void LookAt(Vector3 position, int duration)
		{
			Function.Call(Hash.TASK_LOOK_AT_COORD, _ped.Handle, position.X, position.Y, position.Z, duration, 0 /* flags */, 2);
		}
		public void ParachuteTo(Vector3 position)
		{
			Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, _ped.Handle, position.X, position.Y, position.Z);
		}
		public void ParkVehicle(Vehicle vehicle, Vector3 position, float heading)
		{
			Function.Call(Hash.TASK_VEHICLE_PARK, _ped.Handle, vehicle.Handle, position.X, position.Y, position.Z, heading, 1, 20.0f, false);
		}
		public void ParkVehicle(Vehicle vehicle, Vector3 position, float heading, float radius)
		{
			Function.Call(Hash.TASK_VEHICLE_PARK, _ped.Handle, vehicle.Handle, position.X, position.Y, position.Z, heading, 1, radius, false);
		}
		public void ParkVehicle(Vehicle vehicle, Vector3 position, float heading, float radius, bool keepEngineOn)
		{
			Function.Call(Hash.TASK_VEHICLE_PARK, _ped.Handle, vehicle.Handle, position.X, position.Y, position.Z, heading, 1, radius, keepEngineOn);
		}
		public void PerformSequence(TaskSequence sequence)
		{
			if (!sequence.IsClosed)
			{
				sequence.Close();
			}

			ClearAll();

			_ped.BlockPermanentEvents = true;

			Function.Call(Hash.TASK_PERFORM_SEQUENCE, _ped.Handle, sequence.Handle);
		}
		public void PlayAnimation(string animDict, string animName, float speed, int duration, bool loop, float playbackRate)
		{
			PlayAnimation(animDict, animName, speed, -8.0f, duration, loop ? AnimationFlags.Loop : AnimationFlags.None, playbackRate);
		}
		public void PlayAnimation(string animDict, string animName)
		{
			PlayAnimation(animDict, animName, 8.0f, -8.0f, -1, AnimationFlags.None, 0.0f);
		}
		public void PlayAnimation(string animDict, string animName, float blendInSpeed, int duration, AnimationFlags flags)
		{
			PlayAnimation(animDict, animName, blendInSpeed, -8.0f, duration, flags, 0.0f);
		}
		public void PlayAnimation(string animDict, string animName, float blendInSpeed, float blendOutSpeed, int duration, AnimationFlags flags, float playbackRate)
		{
			Function.Call(Hash.REQUEST_ANIM_DICT, animDict);

			var endtime = DateTime.UtcNow + new TimeSpan(0, 0, 0, 0, 1000);

			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, animDict))
			{
				Script.Yield();

				if (DateTime.UtcNow >= endtime)
				{
					return;
				}
			}

			Function.Call(Hash.TASK_PLAY_ANIM, _ped.Handle, animDict, animName, blendInSpeed, blendOutSpeed, duration, (int)(flags), playbackRate, 0, 0, 0);
		}
		public void PutAwayMobilePhone()
		{
			Function.Call(Hash.TASK_USE_MOBILE_PHONE, _ped.Handle, false);
		}
		public void PutAwayParachute()
		{
			Function.Call(Hash.TASK_PARACHUTE, _ped.Handle, false);
		}
		public void ReactAndFlee(Ped ped)
		{
			Function.Call(Hash.TASK_REACT_AND_FLEE_PED, _ped.Handle, ped.Handle);
		}
		public void ReloadWeapon()
		{
			Function.Call(Hash.TASK_RELOAD_WEAPON, _ped.Handle, true);
		}
		public void RunTo(Vector3 position)
		{
			RunTo(position, false, -1);
		}
		public void RunTo(Vector3 position, bool ignorePaths)
		{
			RunTo(position, ignorePaths, -1);
		}
		public void RunTo(Vector3 position, bool ignorePaths, int timeout)
		{
			if (ignorePaths)
			{
				Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 4.0f, timeout, 0.0f /* heading */, 0.0f);
			}
			else
			{
				Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 4.0f, timeout, 0.0f, 0, 0.0f);
			}
		}
		public void ShootAt(Ped target)
		{
			ShootAt(target, -1, FiringPattern.Default);
		}
		public void ShootAt(Ped target, int duration)
		{
			ShootAt(target, duration, FiringPattern.Default);
		}
		public void ShootAt(Ped target, int duration, FiringPattern pattern)
		{
			Function.Call(Hash.TASK_SHOOT_AT_ENTITY, _ped.Handle, target.Handle, duration, (int)(pattern));
		}
		public void ShootAt(Vector3 position)
		{
			ShootAt(position, -1, FiringPattern.Default);
		}
		public void ShootAt(Vector3 position, int duration)
		{
			ShootAt(position, duration, FiringPattern.Default);
		}
		public void ShootAt(Vector3 position, int duration, FiringPattern pattern)
		{
			Function.Call(Hash.TASK_SHOOT_AT_COORD, _ped.Handle, position.X, position.Y, position.Z, duration, (int)(pattern));
		}
		public void ShuffleToNextVehicleSeat(Vehicle vehicle)
		{
			Function.Call(Hash.TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT, _ped.Handle, vehicle.Handle);
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
		public void StartScenario(string name)
		{
			Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, _ped.Handle, name, 0, 1);
		}
		public void StartScenario(string name, Vector3 position)
		{
			StartScenario(name, position, 0.0f);
		}
		public void StartScenario(string name, Vector3 position, float heading)
		{
			Function.Call(Hash.TASK_START_SCENARIO_AT_POSITION, _ped.Handle, name, position.X, position.Y, position.Z, heading, 0, 0, 1);
		}
		public void SwapWeapon()
		{
			Function.Call(Hash.TASK_SWAP_WEAPON, _ped.Handle, false);
		}
		public void TurnTo(Entity target)
		{
			TurnTo(target, -1);
		}
		public void TurnTo(Entity target, int duration)
		{
			Function.Call(Hash.TASK_TURN_PED_TO_FACE_ENTITY, _ped.Handle, target.Handle, duration);
		}
		public void TurnTo(Vector3 position)
		{
			TurnTo(position, -1);
		}
		public void TurnTo(Vector3 position, int duration)
		{
			Function.Call(Hash.TASK_TURN_PED_TO_FACE_COORD, _ped.Handle, position.X, position.Y, position.Z, duration);
		}
		public void UseMobilePhone()
		{
			Function.Call(Hash.TASK_USE_MOBILE_PHONE, _ped.Handle, true);
		}
		public void UseMobilePhone(int duration)
		{
			Function.Call(Hash.TASK_USE_MOBILE_PHONE_TIMED, _ped.Handle, duration);
		}
		public void UseParachute()
		{
			Function.Call(Hash.TASK_PARACHUTE, _ped.Handle, true);
		}
		public void VehicleChase(Ped target)
		{
			Function.Call(Hash.TASK_VEHICLE_CHASE, _ped.Handle, target.Handle);
		}
		public void VehicleShootAtPed(Ped target)
		{
			Function.Call(Hash.TASK_VEHICLE_SHOOT_AT_PED, _ped.Handle, target.Handle, 20.0f);
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
			Function.Call(Hash.TASK_WANDER_IN_AREA, _ped.Handle, position.X, position.Y, position.Z, radius, 0.0f, 0.0f);
		}
		public void WarpIntoVehicle(Vehicle vehicle, VehicleSeat seat)
		{
			Function.Call(Hash.TASK_WARP_PED_INTO_VEHICLE, _ped.Handle, vehicle.Handle, (int)(seat));
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
			Function.Call(Hash.STOP_ANIM_TASK, _ped.Handle, animSet, animName, -4.0f);
		}
	}
}
