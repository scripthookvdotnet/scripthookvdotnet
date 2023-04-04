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
			EnterVehicle(vehicle, seat, timeout, speed, flag, null);
		}
		public void EnterVehicle(Vehicle vehicle, VehicleSeat seat, int timeout, PedMoveBlendRatio? moveBlendRatio = null, EnterVehicleFlags flag = EnterVehicleFlags.None, string overriddenClipSet = null)
		{
			float moveBlendRatioArgForNative = 1.0f;
			if (moveBlendRatio.HasValue)
			{
				moveBlendRatioArgForNative = (float)moveBlendRatio.Value;
			}
			Function.Call(Hash.TASK_ENTER_VEHICLE, _ped.Handle, vehicle.Handle, timeout, seat, moveBlendRatioArgForNative, flag, overriddenClipSet);
		}

		public void OpenVehicleDoor(Vehicle vehicle, VehicleSeat seat = VehicleSeat.Any, int timeout = -1, PedMoveBlendRatio? moveBlendRatio = null)
		{
			float moveBlendRatioArgForNative = 2.0f;
			if (moveBlendRatio.HasValue)
			{
				moveBlendRatioArgForNative = (float)moveBlendRatio.Value;
			}
			Function.Call(Hash.TASK_OPEN_VEHICLE_DOOR, _ped.Handle, vehicle.Handle, timeout, seat, moveBlendRatioArgForNative);
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

		public void FleeFrom(Ped target, float safeDistance, int duration, bool preferPavements = false, bool updateTargetToNearestHatedPed = false)
		{
			Function.Call(Hash.TASK_SMART_FLEE_PED, _ped.Handle, target.Handle, safeDistance, duration, preferPavements, updateTargetToNearestHatedPed);
		}

		public void FleeFrom(Vector3 position, int duration = -1)
		{
			Function.Call(Hash.TASK_SMART_FLEE_COORD, _ped.Handle, position.X, position.Y, position.Z, 100f, duration, 0, 0);
		}

		public void FleeFrom(Vector3 position, float safeDistance, int duration, bool preferPavements = false, bool quitIfOutOfRange = false)
		{
			Function.Call(Hash.TASK_SMART_FLEE_COORD, _ped.Handle, position.X, position.Y, position.Z, safeDistance, duration, preferPavements, quitIfOutOfRange);
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

		/// <param name="position">The position to go to.</param>
		/// <param name="moveBlendRatio">Specifies how much fast the ped will move. If <see langword="null"/>, the value will default to <c>2f</c>.</param>
		/// <param name="timeBeforeWarp">The time before warping in milliseconds.</param>
		/// <param name="radius">An Unknown radius parameter (possibly the radius for navmesh search) but does not affect the distance where the ped will stop.</param>
		/// <param name="navigationFlags">The navigation flags.</param>
		/// <param name="finalHeading">The final heading that the <see cref="Ped"/> will turn to at the end of the task. Leave <c>40000f</c> to leave as is.</param>
		/// <inheritdoc cref="FollowNavMeshTo(Vector3, PedMoveBlendRatio, int, float, FollowNavMeshFlags, float, float, float, float)"/>
		public void FollowNavMeshTo(Vector3 position, PedMoveBlendRatio? moveBlendRatio = null, int timeBeforeWarp = -1, float radius = 0.25f, FollowNavMeshFlags navigationFlags = FollowNavMeshFlags.Default, float finalHeading = 40000f)
		{
			float moveBlendRatioArgForNative = 2.0f;
			if (moveBlendRatio.HasValue)
			{
				moveBlendRatioArgForNative = (float)moveBlendRatio.Value;
			}
			Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, moveBlendRatioArgForNative, timeBeforeWarp, radius, navigationFlags, finalHeading);
		}

		/// <summary>
		/// Tells the <see cref="Ped"/> to follow the navmesh to the given coord.
		/// </summary>
		/// <param name="position">The position to go to.</param>
		/// <param name="moveBlendRatio">Specifies how much fast the ped will move.</param>
		/// <param name="timeBeforeWarp">The time before warping in milliseconds.</param>
		/// <param name="radius">An Unknown radius parameter (possibly the radius for navmesh search) but does not affect the distance where the ped will stop.</param>
		/// <param name="navigationFlags">The navigation flags.</param>
		/// <param name="slideToCoordHeading">The slide-to-coord heading in degrees.</param>
		/// <param name="maxSlopeNavigable">
		/// Max slope which this ped can move over (<c>0f</c> = can only move on flat,
		/// <c>45f</c> means cannot move on anything above 1:1 slope, <c>90f</c> means can move on any slope).</param>
		/// <param name="clampMaxSearchDistance">
		/// Clamp the search distance to this value, path-search will not search further than this distance
		/// (value must be between 1 and 255 inclusive).
		/// </param>
		/// <param name="finalHeading">The final heading that the <see cref="Ped"/> will turn to at the end of the task. Leave <c>40000f</c> to leave as is.</param>
		/// <remarks>Sometimes a path may not be able to be found. This could happen because there simply isn't any way to get there, or maybe a bunch of dynamic objects have blocked the way,
		///  or maybe the destination is too far away. In this case the <see cref="Ped"/> will simply stand still.
		///  To identify when this has happened, you can use <see cref="Ped.GetNavMeshRouteResult()"/>. This will help you find situations where <see cref="Ped"/> cannot get to their target.</remarks>
		public void FollowNavMeshTo(Vector3 position, PedMoveBlendRatio moveBlendRatio, int timeBeforeWarp, float radius, FollowNavMeshFlags navigationFlags, float slideToCoordHeading, float maxSlopeNavigable, float clampMaxSearchDistance, float finalHeading = 40000f)
		{
			Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD_ADVANCED, _ped.Handle, position.X, position.Y, position.Z, moveBlendRatio.Value, timeBeforeWarp, radius, navigationFlags, slideToCoordHeading, maxSlopeNavigable, clampMaxSearchDistance, finalHeading);
		}

		public void GoTo(Entity target, Vector3 offset = default(Vector3), int timeout = -1)
		{
			Function.Call(Hash.TASK_GOTO_ENTITY_OFFSET_XY, _ped.Handle, target.Handle, timeout, offset.X, offset.Y, offset.Z, 1f, true);
		}

		[Obsolete("TaskInvoker.GoTo with the position parameter may not obvious enough to suggest it uses navigation mesh. Use TaskInvoker.FollowNavMeshTo instead.")]
		public void GoTo(Vector3 position, int timeout = -1)
		{
			Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 1f, timeout, 0f, 0, 0f);
		}

		public void GoStraightTo(Vector3 position, int timeout = -1, float targetHeading = 0f, float distanceToSlide = 0f)
		{
			GoStraightTo(position, timeout, (PedMoveBlendRatio)1f, targetHeading, distanceToSlide);
		}

		/// <summary>
		/// Tells the <see cref="Ped"/> to go to a coord, without using the navemesh.
		/// </summary>
		/// <param name="position">The position to go to.</param>
		/// <param name="moveBlendRatio">Specifies how much fast the ped will move.</param>
		/// <param name="timeBeforeWarp">The time before warping in milliseconds.</param>
		/// <param name="finalHeading">The final heading that the <see cref="Ped"/> will turn to at the end of the task. Set <c>40000f</c> to leave the heading as is.</param>
		/// <param name="targetRadius">The target radius.</param>
		public void GoStraightTo(Vector3 position, int timeBeforeWarp, PedMoveBlendRatio moveBlendRatio, float finalHeading, float targetRadius)
		{
			Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, moveBlendRatio.Value, timeBeforeWarp, finalHeading, targetRadius);
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
			LookAt(target, duration, LookAtFlags.Default, LookAtPriority.Medium);
		}

		public void LookAt(Entity target, int duration, LookAtFlags lookFlags, LookAtPriority priority = LookAtPriority.Medium)
		{
			Function.Call(Hash.TASK_LOOK_AT_ENTITY, _ped.Handle, target.Handle, duration, lookFlags, priority);
		}

		public void LookAt(Vector3 position, int duration = -1)
		{
			LookAt(position, duration, LookAtFlags.Default, LookAtPriority.Medium);
		}

		public void LookAt(Vector3 position, int duration, LookAtFlags lookFlags, LookAtPriority priority = LookAtPriority.Medium)
		{
			Function.Call(Hash.TASK_LOOK_AT_COORD, _ped.Handle, position.X, position.Y, position.Z, duration, lookFlags, priority);
		}

		public void ParachuteTo(Vector3 position)
		{
			Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, _ped.Handle, position.X, position.Y, position.Z);
		}

		public static void UpdateParachuteTarget(Ped ped, Vector3 position)
		{
			Function.Call(Hash.SET_PARACHUTE_TASK_TARGET, ped.Handle, position.X, position.Y, position.Z);
		}

		/// <summary>
		/// Gives the <see cref="Ped"/> a task to park the specified <see cref="Vehicle"/> in the specified manner.
		/// </summary>
		/// <param name="vehicle">The driven vehicle.</param>
		/// <param name="position">The center of the space</param>
		/// <param name="heading">
		/// <para>Heading of the parking space. Can be either positive or negative direction.</para>
		/// <para><para>Although "radius" is an incorrectly named parameter, the name is retained for scripts that use the method with named parameters.</para></para>
		/// </param>
		/// <param name="radius">
		/// <para>If the vehicle's heading isn't within this amount of <paramref name="heading"/>, the <see cref="Vehicle"/> will back up and try to straighten itself out.</para>
		/// <para></para>
		/// </param>
		/// <param name="keepEngineOn">If <see langword="true"/>, keep the lights on after parking.</param>
		public void ParkVehicle(Vehicle vehicle, Vector3 position, float heading, float radius = 20.0f, bool keepEngineOn = false)
		{
			ParkVehicle(vehicle, position, heading, ParkType.PerpendicularNoseIn, radius, keepEngineOn);
		}
		/// <summary>
		/// Gives the <see cref="Ped"/> a task to park the specified <see cref="Vehicle"/> in the specified manner.
		/// </summary>
		/// <param name="vehicle">The driven vehicle.</param>
		/// <param name="position">The center of the space</param>
		/// <param name="directionDegrees">
		/// Heading of the parking space.
		/// Can be either positive or negative direction--how the <see cref="Vehicle"/> enters the space is determined by <paramref name="parkType"/>.
		/// </param>
		/// <param name="parkType">Style of parking.</param>
		/// <param name="toleranceDegrees">
		/// If the vehicle's heading isn't within this amount of <paramref name="directionDegrees"/>, the <see cref="Vehicle"/> will back up and try to straighten itself out.
		/// </param>
		/// <param name="keepEngineOn">If <see langword="true"/>, keep the lights on after parking.</param>
		public void ParkVehicle(Vehicle vehicle, Vector3 position, float directionDegrees, ParkType parkType, float toleranceDegrees = 20.0f, bool keepEngineOn = false)
		{
			Function.Call(Hash.TASK_VEHICLE_PARK, _ped.Handle, vehicle.Handle, position.X, position.Y, position.Z, directionDegrees, parkType, toleranceDegrees, keepEngineOn);
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
			PlayAnimationInternal(animDict, animName, 8f, -8f, -1, AnimationFlags.None, 0f, AnimationIKControlFlags.None);
		}
		public void PlayAnimation(AnimationDictionary animDict, string animName)
		{
			PlayAnimationInternal(animDict, animName, 8f, -8f, -1, AnimationFlags.None, 0f, AnimationIKControlFlags.None);
		}

		public void PlayAnimation(string animDict, string animName, float speed, int duration, float playbackRate)
		{
			PlayAnimationInternal(animDict, animName, speed, -speed, duration, AnimationFlags.None, playbackRate, AnimationIKControlFlags.None);
		}
		public void PlayAnimation(AnimationDictionary animDict, string animName, AnimationBlendDelta blendSpeed, int duration, float startPhase)
		{
			PlayAnimationInternal(animDict, animName, blendSpeed.Value, -blendSpeed.Value, duration, AnimationFlags.None, startPhase, AnimationIKControlFlags.None);
		}

		public void PlayAnimation(string animDict, string animName, float blendInSpeed, int duration, AnimationFlags flags)
		{
			PlayAnimationInternal(animDict, animName, blendInSpeed, -8f, duration, flags, 0f, AnimationIKControlFlags.None);
		}

		public void PlayAnimation(string animDict, string animName, float blendInSpeed, float blendOutSpeed, int duration, AnimationFlags flags, float playbackRate)
		{
			PlayAnimationInternal(animDict, animName, blendInSpeed, blendOutSpeed, duration, flags, playbackRate, AnimationIKControlFlags.None);
		}

		public void PlayAnimation(AnimationDictionary animDict, string animName, AnimationBlendDelta blendInSpeed, AnimationBlendDelta blendOutSpeed, int duration, AnimationFlags flags, float startPhase)
		{
			PlayAnimationInternal(animDict, animName, blendInSpeed.Value, blendOutSpeed.Value, duration, flags, startPhase, AnimationIKControlFlags.None);
		}

		public void PlayAnimation(AnimationDictionary animDict, string animName, AnimationBlendDelta blendInSpeed, AnimationBlendDelta blendOutSpeed, int duration, AnimationFlags flags, float startPhase, AnimationIKControlFlags ikFlags)
		{
			PlayAnimationInternal(animDict, animName, blendInSpeed.Value, -blendOutSpeed.Value, duration, flags, startPhase, ikFlags);
		}

		private void PlayAnimationInternal(AnimationDictionary animDict, string animName, float blendInSpeed, float blendOutSpeed, int duration, AnimationFlags flags, float startPhase, AnimationIKControlFlags ikFlags)
		{
			Function.Call(Hash.REQUEST_ANIM_DICT, animDict);

			int startTime = Environment.TickCount;

			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, animDict))
			{
				Script.Yield();

				if (Environment.TickCount - startTime >= 1000)
				{
					return;
				}
			}

			// The sign of blend delta doesn't make any difference in TASK_PLAY_ANIM and TASK_PLAY_ANIM_ADVANCED, but R* might add sign checks for blend delta values in the future so we'll have to check the signs of blend delta values as well.
			// The third last argument is named phaseControlled in commands_task.sch.
			// Considering how paparrazo3.ysc calls this function, the third last argument may be useful under a synchronized scene.
			// The last argument is named bAllowOverrideCloneUpdate and will not be useful in the story mode.
			Function.Call(Hash.TASK_PLAY_ANIM, _ped.Handle, animDict, animName, blendInSpeed, blendOutSpeed, duration, flags, startPhase, 0, ikFlags, 0);
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

		/// <summary>
		/// <para>
		/// Tasks the <see cref="Ped"/> to do nothing for the specified amount of miliseconds.
		/// Typically used as a part of <see cref="TaskSequence"/> to add a delay.
		/// <c>CTaskDoNothing</c> will be issued when this method is called as a standalone task and <c>CTaskPause</c> will be issued when this method is called as a part of <see cref="TaskSequence"/>.
		/// </para>
		/// <para>
		/// Some tasks such as <c>CTaskMeleeActionResult</c>, which is caused by doing melee attacks, may stop immediately when this task is issued
		/// as a part of <see cref="TaskSequence"/>, which is different from <see cref="StandStill(int)"/>.
		/// </para>
		/// </summary>
		/// <param name="duration">The duration in milliseconds.</param>
		/// <remarks>Unlike <see cref="StandStill(int)"/>, if no script (including ysc ones or external ones) owns the <see cref="Ped"/>,
		/// which is possible by creating them or calling <see cref="Entity.MarkAsMissionEntity(bool)"/>,
		/// the <see cref="Ped"/> will stop doing a pause task immediately and do an ambient task instead.
		/// </remarks>
		public void Pause(int duration)
		{
			Function.Call(Hash.TASK_PAUSE, _ped.Handle, duration);
		}

		/// <summary>
		/// <para>
		/// Tasks the <see cref="Ped"/> to stand still for the specified amount of miliseconds.
		/// Typically used as a part of <see cref="TaskSequence"/> to add a stand still task (internally <c>CTaskDoNothing</c> will always be issued).
		/// </para>
		/// <para>
		/// Some tasks such as <c>CTaskMeleeActionResult</c>, which is caused by doing melee attacks, may not stop immediately when this task is issued,
		/// which is different from <see cref="Pause(int)"/>.
		/// </para>
		/// </summary>
		/// <param name="duration">The duration in milliseconds.</param>
		/// <remarks>Unlike <see cref="Pause(int)"/>, the <see cref="Ped"/> won't stop doing a pause task even if no script
		/// (including ysc ones or external ones) owns the <see cref="Ped"/>, which is possible by creating them or
		/// calling <see cref="Entity.MarkAsMissionEntity(bool)"/>.
		/// </remarks>
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
