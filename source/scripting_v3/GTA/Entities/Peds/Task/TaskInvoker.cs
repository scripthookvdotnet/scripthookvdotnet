//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.ComponentModel;

namespace GTA
{
	public sealed class TaskInvoker
	{
		#region Fields
		readonly Ped _ped;
		#endregion

		// this value is unlikely to get changed in future updates as some of the script task natives use this value as a constant float value
		const float DefaultNavmeshFinalHeading = 40000f;

		internal TaskInvoker(Ped ped)
		{
			_ped = ped;
		}

		public void AchieveHeading(float heading, int timeout = 0)
		{
			Function.Call(Hash.TASK_ACHIEVE_HEADING, _ped.Handle, heading, timeout);
		}

		[Obsolete("Use TaskInvoker.AimGunAtEntity for entity targets instead.")]
		public void AimAt(Entity target, int duration)
		{
			Function.Call(Hash.TASK_AIM_GUN_AT_ENTITY, _ped.Handle, target.Handle, duration, 0);
		}
		[Obsolete("Use TaskInvoker.AimGunAtPosition for coordinate targets instead.")]
		public void AimAt(Vector3 target, int duration)
		{
			Function.Call(Hash.TASK_AIM_GUN_AT_COORD, _ped.Handle, target.X, target.Y, target.Z, duration, 0, 0);
		}

		/// <summary>
		/// Tells the <see cref="Ped"/> to aim a gun at an <see cref="Entity"/>.
		/// The <see cref="Ped"/> must equip a weapon where its <c>CWeaponInfo</c> has the <c>"Gun"</c> flag (the RAGE parser will create ones from weapon meta files).
		/// For instance, the <see cref="Ped"/> a task for aiming when they are equipping a pistol or rocket launcher, but not when equipping a melee weapon or thrown weapon.
		/// </summary>
		/// <param name="target">The target <see cref="Entity"/>.</param>
		/// <param name="duration">The duration in milliseconds.</param>
		/// <param name="instantBlendToAim">If <see langword="true"/>, the task will skip the idle transition and instantly blend to the aim pose.</param>
		public void AimGunAtEntity(Entity target, int duration, bool instantBlendToAim = false)
		{
			Function.Call(Hash.TASK_AIM_GUN_AT_ENTITY, _ped.Handle, target.Handle, duration, instantBlendToAim);
		}
		/// <summary>
		/// Tells the <see cref="Ped"/> to aim a gun at the specified position.
		/// The <see cref="Ped"/> must equip a weapon where its <c>CWeaponInfo</c> has the <c>"Gun"</c> flag (the RAGE parser will create ones from weapon meta files).
		/// For instance, the <see cref="Ped"/> a task for aiming when they are equipping a pistol or rocket launcher, but not when equipping a melee weapon or thrown weapon.
		/// </summary>
		/// <param name="target">The target position.</param>
		/// <param name="duration">The duration in milliseconds.</param>
		/// <param name="instantBlendToAim">If <see langword="true"/>, the task will skip the idle transition and instantly blend to the aim pose.</param>
		/// <param name="playAimIntro">If <see langword="true"/>, the task will play the aim intro.</param>
		public void AimGunAtPosition(Vector3 target, int duration, bool instantBlendToAim = false, bool playAimIntro = false)
		{
			Function.Call(Hash.TASK_AIM_GUN_AT_COORD, _ped.Handle, target.X, target.Y, target.Z, duration, instantBlendToAim, playAimIntro);
		}

		public void Arrest(Ped ped)
		{
			Function.Call(Hash.TASK_ARREST_PED, _ped.Handle, ped.Handle);
		}

		public void ChatTo(Ped ped)
		{
			Function.Call(Hash.TASK_CHAT_TO_PED, _ped.Handle, ped.Handle, 16, 0f, 0f, 0f, 0f, 0f);
		}

		/// <inheritdoc cref="Jump(bool, bool)"/>
		public void Jump() => Jump(false, false);
		/// <summary>
		/// Forces the <see cref="Ped"/> to jump.
		/// </summary>
		/// <param name="doSuperJump">
		/// If <see langword="true"/>, the <see cref="Ped"/> will do super jump.
		/// Internally, the super jump and the beast jump flags will be used for a new <c>CTaskJumpVault</c>.
		/// Does nothing in (probably) v1.0.505.2 or earlier game versions.
		/// </param>
		/// <param name="useFullSuperJumpForce">
		/// If <see langword="true"/> and <paramref name="doSuperJump"/> is <see langword="true"/> as well, the super jump height will be doubled.
		/// Internally, the super jump and the beast jump flags will be used for a new <c>CTaskJumpVault</c> (even if <paramref name="doSuperJump"/> is <see langword="false"/>).
		/// Does nothing in (probably) v1.0.505.2 or earlier game versions.
		/// </param>
		public void Jump(bool doSuperJump, bool useFullSuperJumpForce)
		{
			// 2nd arugment is unused
			Function.Call(Hash.TASK_JUMP, _ped.Handle, false, doSuperJump, useFullSuperJumpForce);
		}

		/// <summary>
		/// Tells the <see cref="Ped"/> to perform the climb task (<c>CTaskJumpVault</c>).
		/// </summary>
		/// <remarks>
		/// The <see cref="Ped"/> needs to be positioned and oriented so that a jump will locate an edge for the ped to grab.
		/// If an edge can’t be found, the ped will just do a normal jump and land.
		/// If an edge can be found then the ped will climb and then stand on top of the found edge.
		/// </remarks>
		public void Climb()
		{
			// 2nd arugment is unused
			Function.Call(Hash.TASK_CLIMB, _ped.Handle, true);
		}

		/// <inheritdoc cref="ClimbLadder(bool)"/>
		public void ClimbLadder() => ClimbLadder(true);
		/// <summary>
		/// Tells the <see cref="Ped"/> to perform a climb ladder task (<c>CTaskClimbLadderFully</c>).
		/// </summary>
		/// <remarks>
		/// The task decides whether the <see cref="Ped"/> is supposed to climb or descend by examining which end of the ladder is nearest.
		/// The <see cref="Ped"/> needs to be positioned right next to the ladder they are supposed to use, and should also be facing it.
		/// There are two possibilities for mounting the ladder - at the base of the ladder facing towards the front of it, and at the top of the ladder facing the reverse of the ladder.
		/// If successful, the <see cref="Ped"/> will get on the ladder, climb, and then get off.
		/// </remarks>
		public void ClimbLadder(bool fast)
		{
			Function.Call(Hash.TASK_CLIMB_LADDER, _ped.Handle, fast);
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

		public void CruiseWithVehicle(Vehicle vehicle, float speed, VehicleDrivingFlags drivingFlags)
		{
			Function.Call(Hash.TASK_VEHICLE_DRIVE_WANDER, _ped.Handle, vehicle.Handle, speed, (int)drivingFlags);
		}
		[Obsolete("Use TaskInvoker.CruiseWithVehicle(Vehicle, float, VehicleDrivingFlags) instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public void CruiseWithVehicle(Vehicle vehicle, float speed, DrivingStyle style = DrivingStyle.Normal)
		{
			Function.Call(Hash.TASK_VEHICLE_DRIVE_WANDER, _ped.Handle, vehicle.Handle, speed, (int)style);
		}

		public void DriveTo(Vehicle vehicle, Vector3 target, float speed, VehicleDrivingFlags drivingFlags, float radius)
		{
			Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, _ped.Handle, vehicle.Handle, target.X, target.Y, target.Z, speed, (int)drivingFlags, radius);
		}
		[Obsolete("Use DriveTo(Vehicle, Vector3, float, VehicleDrivingFlags, float) instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public void DriveTo(Vehicle vehicle, Vector3 target, float radius, float speed, DrivingStyle style = DrivingStyle.Normal)
		{
			Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, _ped.Handle, vehicle.Handle, target.X, target.Y, target.Z, speed, (int)style, radius);
		}

		public void EnterAnyVehicle(VehicleSeat seat = VehicleSeat.Any, int timeout = -1, float speed = 1f, EnterVehicleFlags flag = EnterVehicleFlags.None)
		{
			Function.Call(Hash.TASK_ENTER_VEHICLE, _ped.Handle, 0, timeout, (int)seat, speed, (int)flag, 0);
		}

		public void EnterVehicle(Vehicle vehicle, VehicleSeat seat = VehicleSeat.Any, int timeout = -1, float speed = 1f, EnterVehicleFlags flag = EnterVehicleFlags.None)
		{
			EnterVehicle(vehicle, seat, timeout, (PedMoveBlendRatio)speed, flag, null);
		}
		public void EnterVehicle(Vehicle vehicle, VehicleSeat seat, int timeout, PedMoveBlendRatio? moveBlendRatio = null, EnterVehicleFlags flag = EnterVehicleFlags.None, string overriddenClipSet = null)
		{
			float moveBlendRatioArgForNative = 1.0f;
			if (moveBlendRatio.HasValue)
			{
				moveBlendRatioArgForNative = (float)moveBlendRatio.Value;
			}
			Function.Call(Hash.TASK_ENTER_VEHICLE, _ped.Handle, vehicle.Handle, timeout, (int)seat, moveBlendRatioArgForNative, (int)flag, overriddenClipSet);
		}

		public void OpenVehicleDoor(Vehicle vehicle, VehicleSeat seat = VehicleSeat.Any, int timeout = -1, PedMoveBlendRatio? moveBlendRatio = null)
		{
			float moveBlendRatioArgForNative = 2.0f;
			if (moveBlendRatio.HasValue)
			{
				moveBlendRatioArgForNative = (float)moveBlendRatio.Value;
			}
			Function.Call(Hash.TASK_OPEN_VEHICLE_DOOR, _ped.Handle, vehicle.Handle, timeout, (int)seat, moveBlendRatioArgForNative);
		}

		public static void EveryoneLeaveVehicle(Vehicle vehicle)
		{
			Function.Call(Hash.TASK_EVERYONE_LEAVE_VEHICLE, vehicle.Handle);
		}

		/// <summary>
		/// Tells a ped to combat another ped.
		/// </summary>
		public void Combat(Ped target, TaskCombatFlags combatFlags = TaskCombatFlags.None,
			TaskThreatResponseFlags taskThreatResponseFlags = TaskThreatResponseFlags.CanFightArmedPedsWhenNotArmed)
			=> Function.Call(Hash.TASK_COMBAT_PED, _ped.Handle, target.Handle, (int)combatFlags,
				(int)taskThreatResponseFlags);

		/// <summary>
		/// Tells a ped to combat another ped for a timed period.
		/// </summary>
		/// <remarks>
		/// Implicitly specifies <see cref="TaskThreatResponseFlags.CanFightArmedPedsWhenNotArmed"/> for a new
		/// <c>CTaskThreadResponse</c> task.
		/// </remarks>
		public void CombatTimed(Ped target, int time, TaskCombatFlags combatFlags = TaskCombatFlags.None)
			=> Function.Call(Hash.TASK_COMBAT_PED, _ped.Handle, target.Handle, time, (int)combatFlags);

		/// <summary>
		/// Tells the <see cref="Ped"/> to combat hated targets in the area.
		/// </summary>
		/// <remarks>
		/// Hated targets means <see cref="Ped"/>s whose relationships/acquaintances are set to
		/// <see cref="Relationship.Neutral"/>, <see cref="Relationship.Dislike"/> or <see cref="Relationship.Hate"/>
		/// from the <see cref="Ped"/> who will execute the new task toward them.
		/// There must be at least one <see cref="Ped"/> with one of the relationship settings, or the created
		/// <c>CTaskCombatClosestTargetInArea</c> will stop executing immediately.
		/// </remarks>
		public void CombatHatedTargetsInArea(Vector3 position, float radius,
			TaskCombatFlags combatFlags = TaskCombatFlags.None)
			=> Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_IN_AREA, _ped.Handle, position.X, position.Y, position.Z,
				radius, (int)combatFlags);

		/// <summary>
		/// Tells the <see cref="Ped"/> to combat hated targets in the radius about the <see cref="Ped"/>.
		/// </summary>
		/// <remarks>
		/// Hated targets means <see cref="Ped"/>s whose relationships/acquaintances are set to
		/// <see cref="Relationship.Neutral"/>, <see cref="Relationship.Dislike"/> or <see cref="Relationship.Hate"/>
		/// from the <see cref="Ped"/> who will execute the new task toward them.
		/// There must be at least one <see cref="Ped"/> with one of the relationship settings, or the created
		/// <c>CTaskCombatClosestTargetInArea</c> will stop executing immediately.
		/// </remarks>
		public void CombatHatedTargetsAroundPed(float radius, TaskCombatFlags combatFlags = TaskCombatFlags.None)
			=> Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, _ped.Handle, radius, (int)combatFlags);

		/// <summary>
		/// Tells the <see cref="Ped"/> to combat hated targets in the radius about the <see cref="Ped"/> for a time period.
		/// </summary>
		/// <remarks>
		/// Hated targets means <see cref="Ped"/>s whose relationships/acquaintances are set to
		/// <see cref="Relationship.Neutral"/>, <see cref="Relationship.Dislike"/> or <see cref="Relationship.Hate"/>
		/// from the <see cref="Ped"/> who will execute the new task toward them.
		/// There must be at least one <see cref="Ped"/> with one of the relationship settings, or the created
		/// <c>CTaskCombatClosestTargetInArea</c> will stop executing immediately.
		/// </remarks>
		public void CombatHatedTargetsAroundPedTimed(float radius, int time,
			TaskCombatFlags combatFlags = TaskCombatFlags.None)
			=> Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED_TIMED, _ped.Handle, radius, time,
				(int)combatFlags);

		/// <summary>
		/// Puts the <see cref="Ped"/> into melee.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="blendInDuration">
		/// The blend time in second. Zero or negative value will make the <see cref="Ped"/> into melee instantly
		/// from the previous motion such as walking.
		/// To presicely mimic how long general game code blends a <see cref="Ped"/> into melee, use <c>0.25f</c>.
		/// </param>
		/// <param name="strafePhaseSync">
		/// <para>
		/// Typically <c>0f</c> is used, but <c>10f</c> is used in all of the occurrences where <c>8f</c> is used for
		/// <paramref name="blendInDuration"/> in <c>fbi4_prep1.ysc</c>.
		/// </para>
		/// <para>
		/// Not exactly known how this parameter affects the motion strafing task (<c>CTaskMotionStrafing</c>), but
		/// at least this sets a field of <c>CTaskMotionPed</c> in <c>TASK_PUT_PED_DIRECTLY_INTO_MELEE</c>, which is
		/// soon read by a created <c>CTaskMotionStrafing</c> and will be used for a <c>rage::fwMoveNetworkPlayer</c>
		/// that can be accessed from a <c>CTaskMotionStrafing</c> instance.
		/// </para>
		/// </param>
		/// <remarks>
		/// Implicitly sets <see cref="TaskThreatResponseFlags.CanFightArmedPedsWhenNotArmed"/> for the created
		/// <c>CTaskThreatResponse</c> if called on an AI/NPC <see cref="Ped"/>.
		/// </remarks>
		public void PutDirectlyIntoMelee(Ped target, float blendInDuration, float strafePhaseSync)
			=> Function.Call(Hash.TASK_PUT_PED_DIRECTLY_INTO_MELEE, _ped.Handle, target, blendInDuration, -1,
				strafePhaseSync, 0);
		/// <summary>
		/// Puts the <see cref="Ped"/> into melee, but allow to specify the task time that only applies for a player
		/// <see cref="Ped"/>. Not intended to use with an AI/NPC <see cref="Ped"/>.
		/// </summary>
		/// <param name="target">
		/// <inheritdoc cref="PutDirectlyIntoMelee(Ped, float, float)" path="/param[@name='target']"/>
		/// </param>
		/// <param name="blendInDuration">
		/// <inheritdoc cref="PutDirectlyIntoMelee(Ped, float, float)" path="/param[@name='blendInDuration']"/>
		/// </param>
		/// <param name="strafePhaseSync">
		/// <inheritdoc cref="PutDirectlyIntoMelee(Ped, float, float)" path="/param[@name='strafePhaseSync']"/>
		/// </param>
		/// <param name="timeInTask">
		/// The time in seconds for the newly created <c>CTaskMelee</c>.
		/// Only applies when the <see cref="Ped"/> being given the task is a player one.
		/// </param>
		public void PutDirectlyIntoMelee(Ped target, float blendInDuration, float strafePhaseSync, float timeInTask)
			=> Function.Call(Hash.TASK_PUT_PED_DIRECTLY_INTO_MELEE, _ped.Handle, target, blendInDuration, timeInTask,
				strafePhaseSync, 0);
		/// <summary>
		/// Puts the <see cref="Ped"/> into melee, but allow to specify the combat flags that only applies for an
		/// AI/NPC <see cref="Ped"/>. Not intended to use with a player <see cref="Ped"/>.
		/// </summary>
		/// <param name="target">
		/// <inheritdoc cref="PutDirectlyIntoMelee(Ped, float, float)" path="/param[@name='target']"/>
		/// </param>
		/// <param name="blendInDuration">
		/// <inheritdoc cref="PutDirectlyIntoMelee(Ped, float, float)" path="/param[@name='blendInDuration']"/>
		/// </param>
		/// <param name="strafePhaseSync">
		/// <inheritdoc cref="PutDirectlyIntoMelee(Ped, float, float)" path="/param[@name='strafePhaseSync']"/>
		/// </param>
		/// <param name="aiCombatFlags">
		/// The combat flags for the newly created <c>CTaskThreatResponse</c> to use.
		/// Only applies when the <see cref="Ped"/> being given the task is an AI/NPC one.
		/// <see cref="TaskCombatFlags.DisableAimIntro"/> is implicitly set in <c>TASK_PUT_PED_DIRECTLY_INTO_MELEE</c>,
		/// and setting such value has no effect.
		/// </param>
		/// <remarks>
		/// Implicitly sets <see cref="TaskThreatResponseFlags.CanFightArmedPedsWhenNotArmed"/> for the created
		/// <c>CTaskThreatResponse</c> if called on an AI/NPC <see cref="Ped"/>.
		/// </remarks>
		public void PutDirectlyIntoMelee(Ped target, float blendInDuration, float strafePhaseSync, TaskCombatFlags aiCombatFlags)
			=> Function.Call(Hash.TASK_PUT_PED_DIRECTLY_INTO_MELEE, _ped.Handle, target, blendInDuration, -1f,
				strafePhaseSync, (uint)aiCombatFlags);

		[Obsolete("Use TaskInvoker.Combat instead.")]
		public void FightAgainst(Ped target)
		{
			Function.Call(Hash.TASK_COMBAT_PED, _ped.Handle, target.Handle, 0, 16);
		}
		[Obsolete("Use TaskInvoker.CombatTimed instead.")]
		public void FightAgainst(Ped target, int duration)
		{
			Function.Call(Hash.TASK_COMBAT_PED_TIMED, _ped.Handle, target.Handle, duration, 0);
		}
		[Obsolete("Use TaskInvoker.CombatHatedTargetsAroundPed instead.")]
		public void FightAgainstHatedTargets(float radius)
		{
			Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, _ped.Handle, radius, 0);
		}
		[Obsolete("Use TaskInvoker.CombatHatedTargetsAroundPedTimed instead.")]
		public void FightAgainstHatedTargets(float radius, int duration)
		{
			Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED_TIMED, _ped.Handle, radius, duration, 0);
		}

		public void FleeFrom(Ped ped, int duration = -1) => FleeFrom(ped, 100f, duration);
		public void FleeFrom(Ped otherPed, float safeDistance, int duration)
		{
			// 5th argument bPreferPavements and 6th argument bUpdateToNearestHatedPed are unused
			Function.Call(Hash.TASK_SMART_FLEE_PED, _ped.Handle, otherPed.Handle, safeDistance, duration, false, false);
		}

		public void FleeFrom(Vector3 position, int duration = -1) => FleeFrom(position, 100f, duration);
		public void FleeFrom(Vector3 position, float safeDistance, int duration, bool quitIfOutOfRange = false)
		{
			// 7th argument bPreferPavements is unused
			Function.Call(Hash.TASK_SMART_FLEE_COORD, _ped.Handle, position.X, position.Y, position.Z, safeDistance, duration, false, quitIfOutOfRange);
		}

		public void FollowPointRoute(params Vector3[] points)
		{
			FollowPointRoute(1f, points);
		}

		public void FollowPointRoute(float movementSpeed, params Vector3[] points)
		{
			Function.Call(Hash.TASK_FLUSH_ROUTE);

			foreach (Vector3 point in points)
			{
				Function.Call(Hash.TASK_EXTEND_ROUTE, point.X, point.Y, point.Z);
			}

			Function.Call(Hash.TASK_FOLLOW_POINT_ROUTE, _ped.Handle, movementSpeed, 0);
		}

		public void FollowToOffsetFromEntity(Entity target, Vector3 offset, float movementSpeed, int timeout = -1, float distanceToFollow = 10f, bool persistFollowing = true)
		{
			Function.Call(Hash.TASK_FOLLOW_TO_OFFSET_OF_ENTITY, _ped.Handle, target.Handle, offset.X, offset.Y, offset.Z, movementSpeed, timeout, distanceToFollow, persistFollowing);
		}

		/// <summary>
		/// Tells the <see cref="Ped"/> to follow the navmesh to the given coord.
		/// </summary>
		/// <param name="position">The position to go to.</param>
		/// <param name="moveBlendRatio">Specifies how much fast the ped will move. If <see langword="null"/>, the value will default to <c>2f</c>.</param>
		/// <param name="timeBeforeWarp">The time before warping in milliseconds.</param>
		/// <param name="radius">An Unknown radius parameter (possibly the radius for navmesh search) but does not affect the distance where the ped will stop.</param>
		/// <param name="navigationFlags">The navigation flags.</param>
		/// <param name="finalHeading">The final heading that the <see cref="Ped"/> will turn to at the end of the task. Leave <see cref="DefaultNavmeshFinalHeading"/> to leave as is.</param>
		/// <remarks>
		/// Sometimes a path may not be able to be found. This could happen because there simply isn't any way to get there, or maybe a bunch of dynamic objects have blocked the way,
		/// or maybe the destination is too far away. In this case the <see cref="Ped"/> will simply stand still.
		/// To identify when this has happened, you can use <see cref="Ped.GetNavMeshRouteResult()"/>. This will help you find situations where <see cref="Ped"/> cannot get to their target.
		/// </remarks>
		public void FollowNavMeshTo(Vector3 position, PedMoveBlendRatio? moveBlendRatio = null, int timeBeforeWarp = -1, float radius = 0.25f, FollowNavMeshFlags navigationFlags = FollowNavMeshFlags.Default, float finalHeading = DefaultNavmeshFinalHeading)
		{
			float moveBlendRatioArgForNative = 2.0f;
			if (moveBlendRatio.HasValue)
			{
				moveBlendRatioArgForNative = (float)moveBlendRatio.Value;
			}
			Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, moveBlendRatioArgForNative, timeBeforeWarp, radius, (int)navigationFlags, finalHeading);
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
		/// <param name="finalHeading">The final heading that the <see cref="Ped"/> will turn to at the end of the task. Leave <see cref="DefaultNavmeshFinalHeading"/> to leave as is.</param>
		/// <remarks>
		/// Sometimes a path may not be able to be found. This could happen because there simply isn't any way to get there, or maybe a bunch of dynamic objects have blocked the way,
		/// or maybe the destination is too far away. In this case the <see cref="Ped"/> will simply stand still.
		/// To identify when this has happened, you can use <see cref="Ped.GetNavMeshRouteResult()"/>. This will help you find situations where <see cref="Ped"/> cannot get to their target.
		/// </remarks>
		public void FollowNavMeshTo(Vector3 position, PedMoveBlendRatio moveBlendRatio, int timeBeforeWarp, float radius, FollowNavMeshFlags navigationFlags, float slideToCoordHeading, float maxSlopeNavigable, float clampMaxSearchDistance, float finalHeading = DefaultNavmeshFinalHeading)
		{
			Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD_ADVANCED, _ped.Handle, position.X, position.Y, position.Z, moveBlendRatio.Value, timeBeforeWarp, radius, (int)navigationFlags, slideToCoordHeading, maxSlopeNavigable, clampMaxSearchDistance, finalHeading);
		}

		public void GoTo(Entity target, Vector3 offset = default(Vector3), int timeout = -1)
		{
			Function.Call(Hash.TASK_GOTO_ENTITY_OFFSET_XY, _ped.Handle, target.Handle, timeout, offset.X, offset.Y, offset.Z, 1f, true);
		}

		[Obsolete("TaskInvoker.GoTo with the position parameter may not obvious enough to suggest it uses navigation mesh. Use TaskInvoker.FollowNavMeshTo instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public void GoTo(Vector3 position, int timeout = -1)
		{
			Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 1f, timeout, 0f, 0, 0f);
		}

		public void GoStraightTo(Vector3 position, int timeout = -1, float targetHeading = 0f, float distanceToSlide = 0f)
		{
			GoStraightTo(position, timeout, (PedMoveBlendRatio)1f, targetHeading, distanceToSlide);
		}

		/// <summary>
		/// Tells the <see cref="Ped"/> to go to a coord, without using the navmesh.
		/// </summary>
		/// <param name="position">The position to go to.</param>
		/// <param name="moveBlendRatio">Specifies how much fast the ped will move.</param>
		/// <param name="timeBeforeWarp">The time before warping in milliseconds.</param>
		/// <param name="finalHeading">The final heading that the <see cref="Ped"/> will turn to at the end of the task. Set <see cref="DefaultNavmeshFinalHeading"/> to leave the heading as is.</param>
		/// <param name="targetRadius">The target radius.</param>
		public void GoStraightTo(Vector3 position, int timeBeforeWarp, PedMoveBlendRatio moveBlendRatio, float finalHeading, float targetRadius)
		{
			Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, moveBlendRatio.Value, timeBeforeWarp, finalHeading, targetRadius);
		}

		/// <inheritdoc cref="GoToPointAnyMeansExtraParamsWithCruiseSpeed"/>
		public void GoToPointAnyMeans(Vector3 target, PedMoveBlendRatio moveBlendRatio, Vehicle vehicle,
			bool useLongRangeVehiclePathing = false,
			VehicleDrivingFlags drivingFlags = VehicleDrivingFlags.DrivingModeStopForVehicles,
			float maxRangeToShootTargets = -1f)
			=> Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS, _ped.Handle, target.X, target.Y, target.Z, moveBlendRatio,
				vehicle, useLongRangeVehiclePathing, (int)drivingFlags, maxRangeToShootTargets);

		/// <param name="target">
		/// <inheritdoc cref="GoToPointAnyMeansExtraParamsWithCruiseSpeed" path="/param[@name='target']"/>
		/// </param>
		/// <param name="moveBlendRatio">
		/// <inheritdoc cref="GoToPointAnyMeansExtraParamsWithCruiseSpeed" path="/param[@name='moveBlendRatio']"/>
		/// </param>
		/// <param name="vehicle">
		/// <inheritdoc cref="GoToPointAnyMeansExtraParamsWithCruiseSpeed" path="/param[@name='vehicle']"/>
		/// </param>
		/// <param name="useLongRangeVehiclePathing">
		/// <inheritdoc cref="GoToPointAnyMeansExtraParamsWithCruiseSpeed" path="/param[@name='useLongRangeVehiclePathing']"/>
		/// </param>
		/// <param name="drivingFlags">
		/// <inheritdoc cref="GoToPointAnyMeansExtraParamsWithCruiseSpeed" path="/param[@name='drivingFlags']"/>
		/// </param>
		/// <param name="maxRangeToShootTargets">
		/// <inheritdoc cref="GoToPointAnyMeansExtraParamsWithCruiseSpeed" path="/param[@name='maxRangeToShootTargets']"/>
		/// </param>
		/// <param name="extraVehToTargetDistToPreferVeh">
		/// <inheritdoc cref="GoToPointAnyMeansExtraParamsWithCruiseSpeed" path="/param[@name='extraVehToTargetDistToPreferVeh']"/>
		/// </param>
		/// <param name="driveStraightLineDistance">
		/// <inheritdoc cref="GoToPointAnyMeansExtraParamsWithCruiseSpeed" path="/param[@name='driveStraightLineDistance']"/>
		/// </param>
		/// <param name="extraFlags">
		/// <inheritdoc cref="GoToPointAnyMeansExtraParamsWithCruiseSpeed" path="/param[@name='extraFlags']"/>
		/// </param>
		/// <param name="warpTimerMs">
		/// Warps ped to target position if ped gets stuck for this amount of time in milliseconds
		/// (only if <paramref name="warpTimerMs"/> != -1.0).
		/// Only works for <see cref="Ped"/>s on foot or in a car/bike (not aircraft/boats).
		/// <see cref="Ped"/>s will be removed from <see cref="Vehicle"/> on warp.
		/// </param>
		/// <inheritdoc cref="GoToPointAnyMeansExtraParamsWithCruiseSpeed"/>
		///
		public void GoToPointAnyMeansExtraParams(Vector3 target, PedMoveBlendRatio moveBlendRatio, Vehicle vehicle,
			bool useLongRangeVehiclePathing = false,
			VehicleDrivingFlags drivingFlags = VehicleDrivingFlags.DrivingModeStopForVehicles,
			float maxRangeToShootTargets = -1f, float extraVehToTargetDistToPreferVeh = 0f,
			float driveStraightLineDistance = 20f,
			TaskGoToPointAnyMeansFlags extraFlags = TaskGoToPointAnyMeansFlags.Default, float warpTimerMs = -1f)
			=> Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS_EXTRA_PARAMS, _ped.Handle, target.X, target.Y, target.Z,
				moveBlendRatio,
				vehicle, useLongRangeVehiclePathing, (int)drivingFlags, maxRangeToShootTargets,
				extraVehToTargetDistToPreferVeh, driveStraightLineDistance, (int)extraFlags, warpTimerMs);


		/// <summary>
		/// Tells the <see cref="Ped"/> to go to a point by any means.
		/// </summary>
		/// <param name="target">The target point.</param>
		/// <param name="moveBlendRatio">The move blend ratio.</param>
		/// <param name="vehicle">
		/// The vehicle to get to the point.
		/// Set <see langword="null"/> to let the <see cref="Ped"/> use any <see cref="Vehicle"/>s.
		/// If a <see cref="Vehicle"/> instance is set that does not exist in the game, the method will silently fail
		/// without even creating a <c>CTaskGoToPointAnyMeans</c>, which is supposed to be created.
		/// </param>
		/// <param name="useLongRangeVehiclePathing">
		/// If <see langword="true"/>, the created task may use a <c>CTaskVehicleGotoLongRange</c>, which automatically
		/// loads nodes in the background (may be useful to avoid the task not going to a point when you want to get
		/// <see cref="Ped"/>s to a point far from where the player is).
		/// If <see langword="false"/>, the created task will not use a <c>CTaskVehicleGotoLongRange</c>, which may
		/// result in the task not going to a point if the specified point is too far from where the player is.
		/// </param>
		/// <param name="drivingFlags">The driving flags.</param>
		/// <param name="maxRangeToShootTargets">
		/// The max range to shoot targets in meters.
		/// </param>
		/// <param name="extraVehToTargetDistToPreferVeh">
		/// <para>
		/// The distance in meters that partially determines if the <see cref="Ped"/> stops considering when they are
		/// not in a <see cref="Vehicle"/>. The high this value is, the further the <see cref="Ped"/> will stop
		/// considering from <paramref name="target"/>. Roughly speaking, if this parameter is more than the square of
		/// the distance between the <see cref="Ped"/> and <paramref name="target"/>, they will not basically take any
		/// <see cref="Vehicle"/>s.
		/// </para>
		/// <para>
		/// Strictly speaking, the distance between a <see cref="Vehicle"/> and
		/// <paramref name="target"/> and this parameter is less than or equal to the sum of the distance between
		/// the <see cref="Ped"/> and <paramref name="target"/> and the square root, the task will stop considering
		/// the <see cref="Vehicle"/>.<br/>
		/// You can confirm if it is correct that how exactly this parameter works that explained in this document by
		/// searching for <c>"76 04 B0 01 EB 79 0F 28 9A ? ? ? ? F3 0F 59 ED"</c> and inspecting nearby instructions,
		/// where rcx is the pointer to a <c>CTaskGoToPointAnyMeans</c>, rdx is the pointer to a considered
		/// <c>CVehicle</c>, and r8 is the pointer to the <c>CPed</c> who is executing the rcx task.
		/// </para>
		/// </param>
		/// <param name="driveStraightLineDistance">
		/// The distance to target in meters at which the <see cref="Ped"/> will start driving straight instead of
		/// following vehicle nodes.
		/// </param>
		/// <param name="extraFlags">
		/// The extra flags for how the created task should be executed.
		/// </param>
		/// <param name="cruiseSpeed">
		/// The cruise speed in m/s.
		/// </param>
		/// <param name="targetArriveDist">
		/// The distance to target in meters at which a vehicle task will quit.
		/// </param>
		public void GoToPointAnyMeansExtraParamsWithCruiseSpeed(Vector3 target, PedMoveBlendRatio moveBlendRatio,
			Vehicle vehicle, bool useLongRangeVehiclePathing = false,
			VehicleDrivingFlags drivingFlags = VehicleDrivingFlags.DrivingModeStopForVehicles,
			float maxRangeToShootTargets = -1f, float extraVehToTargetDistToPreferVeh = 0f,
			float driveStraightLineDistance = 20f,
			TaskGoToPointAnyMeansFlags extraFlags = TaskGoToPointAnyMeansFlags.Default, float cruiseSpeed = -1f,
			float targetArriveDist = 4f)
			=> Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS_EXTRA_PARAMS_WITH_CRUISE_SPEED, _ped.Handle, target.X,
				target.Y, target.Z, moveBlendRatio,
				vehicle, useLongRangeVehiclePathing, (int)drivingFlags, maxRangeToShootTargets,
				extraVehToTargetDistToPreferVeh, driveStraightLineDistance, (int)extraFlags, cruiseSpeed,
				targetArriveDist);

		public void GuardCurrentPosition()
		{
			Function.Call(Hash.TASK_GUARD_CURRENT_POSITION, _ped.Handle, 15f, 10f, true);
		}

		public void HandsUp(int duration)
		{
			Function.Call(Hash.TASK_HANDS_UP, _ped.Handle, duration, 0, -1, false);
		}

		/// <summary>
		/// Tells the <see cref="Ped"/> in a plane to land.
		/// The <see cref="Ped"/> will try to land in between the <paramref name="startPosition"/> and <paramref name="touchdownPosition"/>.
		/// </summary>
		/// <param name="startPosition">The start position on a runway.</param>
		/// <param name="touchdownPosition">The end position on a runway.</param>
		/// <param name="plane">The plane to land. if <see langword="null"/>, <see cref="Ped.CurrentVehicle"/> will be used as <c>TASK_PLANE_LAND</c> requires a vehicle handle as the 2nd parameter.</param>
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
			Function.Call(Hash.TASK_LEAVE_ANY_VEHICLE, _ped.Handle, 0, (int)flags);
		}

		public void LeaveVehicle(Vehicle vehicle, bool closeDoor)
		{
			LeaveVehicle(vehicle, closeDoor ? LeaveVehicleFlags.None : LeaveVehicleFlags.LeaveDoorOpen);
		}

		public void LeaveVehicle(Vehicle vehicle, LeaveVehicleFlags flags)
		{
			Function.Call(Hash.TASK_LEAVE_VEHICLE, _ped.Handle, vehicle.Handle, (int)flags);
		}

		public void LookAt(Entity target, int duration = -1)
		{
			LookAt(target, duration, LookAtFlags.Default, LookAtPriority.Medium);
		}

		public void LookAt(Entity target, int duration, LookAtFlags lookFlags = LookAtFlags.Default, LookAtPriority priority = LookAtPriority.Medium)
		{
			Function.Call(Hash.TASK_LOOK_AT_ENTITY, _ped.Handle, target.Handle, duration, (int)lookFlags, (int)priority);
		}

		public void LookAt(Vector3 position, int duration = -1)
		{
			LookAt(position, duration, LookAtFlags.Default, LookAtPriority.Medium);
		}

		public void LookAt(Vector3 position, int duration, LookAtFlags lookFlags = LookAtFlags.Default, LookAtPriority priority = LookAtPriority.Medium)
		{
			Function.Call(Hash.TASK_LOOK_AT_COORD, _ped.Handle, position.X, position.Y, position.Z, duration, (int)lookFlags, (int)priority);
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
		/// <param name="position">The center of the space.</param>
		/// <param name="heading">
		/// <para>Heading of the parking space. Can be either positive or negative direction.</para>
		/// <para>Although "radius" is an incorrectly named parameter, the name is retained for scripts that use the method with named parameters.</para>
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
		/// <param name="position">The center of the space.</param>
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
			Function.Call(Hash.TASK_VEHICLE_PARK, _ped.Handle, vehicle.Handle, position.X, position.Y, position.Z, directionDegrees, (int)parkType, toleranceDegrees, keepEngineOn);
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
			PlayAnimationInternal((CrClipDictionary)animDict, animName, 8f, -8f, -1, AnimationFlags.None, 0f, false, AnimationIKControlFlags.None);
		}
		public void PlayAnimation(CrClipAsset crClipAsset)
		{
			PlayAnimationInternal(crClipAsset, 8f, -8f, -1, AnimationFlags.None, 0f, false, AnimationIKControlFlags.None);
		}
		public void PlayAnimation(string animDict, string animName, float speed, int duration, float playbackRate)
		{
			PlayAnimationInternal((CrClipDictionary)animDict, animName, speed, -speed, duration, AnimationFlags.None, playbackRate, false, AnimationIKControlFlags.None);
		}
		public void PlayAnimation(CrClipAsset crClipAsset, AnimationBlendDelta blendSpeed, int duration, float startPhase)
		{
			PlayAnimationInternal(crClipAsset, blendSpeed.Value, -blendSpeed.Value, duration, AnimationFlags.None,
				startPhase, false, AnimationIKControlFlags.None);
		}
		public void PlayAnimation(string animDict, string animName, float blendInSpeed, int duration, AnimationFlags flags)
		{
			PlayAnimationInternal((CrClipDictionary)animDict, animName, blendInSpeed, -8f, duration, flags, 0f, false,
				AnimationIKControlFlags.None);
		}
		public void PlayAnimation(string animDict, string animName, float blendInSpeed, float blendOutSpeed, int duration, AnimationFlags flags, float playbackRate)
		{
			PlayAnimationInternal((CrClipDictionary)animDict, animName, blendInSpeed, blendOutSpeed, duration, flags, playbackRate, false, AnimationIKControlFlags.None);
		}
		public void PlayAnimation(CrClipAsset crClipAsset, AnimationBlendDelta blendInSpeed, AnimationBlendDelta blendOutSpeed, int duration, AnimationFlags flags, float startPhase)
		{
			PlayAnimationInternal(crClipAsset, blendInSpeed.Value, blendOutSpeed.Value, duration, flags, startPhase, false, AnimationIKControlFlags.None);
		}
		public void PlayAnimation(CrClipAsset crClipAsset, AnimationBlendDelta blendInSpeed, AnimationBlendDelta blendOutSpeed, int duration, AnimationFlags flags, float startPhase, bool phaseControlled, AnimationIKControlFlags ikFlags)
		{
			PlayAnimationInternal(crClipAsset, blendInSpeed.Value, -blendOutSpeed.Value, duration, flags, startPhase, phaseControlled, ikFlags);
		}

		private void PlayAnimationInternal(CrClipAsset crClipAsset, float blendInSpeed, float blendOutSpeed, int duration, AnimationFlags flags, float startPhase, bool phaseControlled, AnimationIKControlFlags ikFlags)
		{
			(CrClipDictionary clipDict, string clipName) = crClipAsset;
			PlayAnimationInternal(clipDict, clipName, blendInSpeed, blendOutSpeed, duration, flags, startPhase, phaseControlled, ikFlags);
		}
		private void PlayAnimationInternal(CrClipDictionary clipDict, string animName, float blendInSpeed, float blendOutSpeed, int duration, AnimationFlags flags, float startPhase, bool phaseControlled, AnimationIKControlFlags ikFlags)
		{
			Function.Call(Hash.REQUEST_ANIM_DICT, clipDict);

			int startTime = Environment.TickCount;

			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, clipDict))
			{
				Script.Yield();

				if (Environment.TickCount - startTime >= 1000)
				{
					return;
				}
			}

			// The sign of blend delta doesn't make any difference in TASK_PLAY_ANIM and TASK_PLAY_ANIM_ADVANCED,
			// but R* might add sign checks for blend delta values in the future so we'll have to check the signs of
			// blend delta values as well.
			// The last argument is named bAllowOverrideCloneUpdate and will not be useful in the story mode.
			Function.Call(Hash.TASK_PLAY_ANIM, _ped.Handle, clipDict, animName, blendInSpeed, blendOutSpeed, duration,
				(int)flags, startPhase, phaseControlled, (int)ikFlags, 0);
		}

		/// <summary>
		/// Plays an anim task on the <see cref="Ped"/> with a reposition and reorientation at the beginning.
		/// </summary>
		/// <param name="crClipAsset">
		/// The <see cref="CrClipAsset"/> to find the corresponding clip.
		/// </param>
		/// <param name="position">The initial position in World Coordinates to start the anim at.</param>
		/// <param name="rotation">
		/// The initial rotation (in degrees, format &lt;&lt; pitch, roll, heading &gt;&gt;) to playback the anim from.
		/// </param>
		/// <param name="blendInDelta">The rate at which the task will blend in.</param>
		/// <param name="blendOutDelta">The rate at which the task will blend out.</param>
		/// <param name="timeToPlay">The time to play in milliseconds.</param>
		/// <param name="flags">The animation flags.</param>
		/// <param name="startPhase">The phase to start between 0 and 1.</param>
		/// <param name="rotOrder">The rotation order.</param>
		/// <param name="ikFlags">The IK flags.</param>
		/// <remarks>
		/// <para>
		/// Specifying the task flags both <see cref="AnimationFlags.ExtractInitialOffset"/> and
		/// <see cref="AnimationFlags.OverridePhysics"/> will instruct the task to play the anim using an initial offset
		/// specified by the animator (if one exists). Use this flag to playback synced anims on multiple peds (i.e.
		/// give all peds the same Pos and Rot values and the animation flag
		/// <see cref="AnimationFlags.ExtractInitialOffset"/> and <see cref="AnimationFlags.OverridePhysics"/>)
		/// </para>
		/// <para>
		/// This method does not automatically request <see cref="CrClipDictionary"/> of <paramref name="crClipAsset"/>
		/// which is different from <see cref="PlayAnimation(CrClipAsset)"/>, so you will need to manually request it.
		/// </para>
		/// </remarks>
		public void PlayAnimationAdvanced(CrClipAsset crClipAsset, Vector3 position, Vector3 rotation,
			AnimationBlendDelta? blendInDelta = null, AnimationBlendDelta? blendOutDelta = null, int timeToPlay = -1,
			AnimationFlags flags = AnimationFlags.None, float startPhase = 0f,
			EulerRotationOrder rotOrder = EulerRotationOrder.YXZ,
			AnimationIKControlFlags ikFlags = AnimationIKControlFlags.None)
		{
			(CrClipDictionary clipDict, string clipName) = crClipAsset;
			float blendInDeltaArg = blendInDelta?.Value ?? AnimationBlendDelta.Normal.Value;
			float blendOutDeltaArg = -(blendOutDelta?.Value ?? AnimationBlendDelta.Normal.Value);

			Function.Call(Hash.TASK_PLAY_ANIM_ADVANCED, _ped.Handle, clipDict, clipName, position.X, position.Y, position.Z,
				rotation.X, rotation.Y, rotation.Z, blendInDeltaArg, blendOutDeltaArg, timeToPlay, (int)flags,
				startPhase, (int)rotOrder, (int)ikFlags);
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
			// 2nd parameter is unused
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
			Function.Call(Hash.TASK_SHOOT_AT_ENTITY, _ped.Handle, target.Handle, duration, (uint)pattern);
		}

		public void ShootAt(Vector3 position, int duration = -1, FiringPattern pattern = FiringPattern.Default)
		{
			Function.Call(Hash.TASK_SHOOT_AT_COORD, _ped.Handle, position.X, position.Y, position.Z, duration, (uint)pattern);
		}

		public void ShuffleToNextVehicleSeat(Vehicle vehicle = null)
		{
			if (vehicle == null)
			{
				vehicle = _ped.CurrentVehicle;
			}
			Function.Call(Hash.TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT, _ped.Handle, vehicle);
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
		/// <remarks>
		/// Unlike <see cref="StandStill(int)"/>, if no script (including ysc ones or external ones) owns the <see cref="Ped"/>,
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
		/// <remarks>
		/// Unlike <see cref="Pause(int)"/>, the <see cref="Ped"/> won't stop doing a pause task even if no script
		/// (including ysc ones or external ones) owns the <see cref="Ped"/>, which is possible by creating them or
		/// calling <see cref="Entity.MarkAsMissionEntity(bool)"/>.
		/// </remarks>
		public void StandStill(int duration)
		{
			Function.Call(Hash.TASK_STAND_STILL, _ped.Handle, duration);
		}

		[Obsolete("TaskInvoker.StartScenario is obsolete, use TaskInvoker.StartScenarioInPlace instead.")]
		public void StartScenario(string name, float heading) => StartScenarioInPlace(name);

		[Obsolete("TaskInvoker.StartScenario is obsolete, use TaskInvoker.StartScenarioAtPosition instead.")]
		public void StartScenario(string name, Vector3 position, float heading) => StartScenarioAtPosition(name, position, heading, playIntroClip : false);

		/// <summary>
		/// Puts this <see cref="Ped"/> into the given scenario immediately in place.
		/// </summary>
		/// <param name="scenarioName">The scenario name.</param>
		/// <param name="timeToLeave">
		/// <para>The time in milliseconds since the <see cref="Ped"/> starts the main clip of the scenario before they starts to leave.</para>
		/// <para>If zero, the initiated task will not have the <see cref="Ped"/> leave the scenario by elapsing time.</para>
		/// <para>If positive, the initiated task will have the <see cref="Ped"/> leave the scenario after the specified time elapsed.</para>
		/// <para>
		/// If negative, the initiated task will will not stop the <see cref="Ped"/> leaving the scenario by elapsing time (behaves the same as zero in this way)
		/// but this method will start a <c>CTaskUseScenario</c> task with "idle forever" flag (although it is unknown what difference the flag makes).
		/// </para>
		/// </param>
		/// <param name="playIntroClip">If <see langword="false"/>, the initiated task will skip the enter clip.</param>
		/// <remarks>
		/// This method will not start a <c>CTask</c> and the <see cref="Ped"/> will do nothing for the <c>CTask</c>
		/// if the scenario manager does not have the registered hash for <paramref name="scenarioName"/>.
		/// </remarks>
		public void StartScenarioInPlace(string scenarioName, int timeToLeave = 0, bool playIntroClip = true)
		{
			Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, _ped.Handle, scenarioName, timeToLeave, playIntroClip);
		}

		/// <summary>
		/// Tell this <see cref="Ped"/> to move or warp to the position and heading given, then start the scenario passed.
		/// </summary>
		/// <param name="scenarioName">The scenario name.</param>
		/// <param name="position">The position to put the <see cref="Ped"/> into the given scenario.</param>
		/// <param name="heading">The heading to put the <see cref="Ped"/> into the given scenario.</param>
		/// <param name="timeToLeave">
		/// <para>The time in milliseconds since the <see cref="Ped"/> starts the main clip of the scenario before they starts to leave.</para>
		/// <para>If zero, the initiated task will not have the <see cref="Ped"/> leave the scenario by elapsing time.</para>
		/// <para>If positive, the initiated task will have the <see cref="Ped"/> leave the scenario after the specified time elapsed.</para>
		/// <para>
		/// If negative, the initiated task will will not stop the <see cref="Ped"/> leaving the scenario by elapsing time (behaves the same as zero in this way)
		/// but this method will start a <c>CTaskUseScenario</c> task with "idle forever" flag (although it is unknown what difference the flag makes).
		/// </para>
		/// </param>
		/// <param name="playIntroClip">If <see langword="false"/>, the initiated task will skip the enter clip.</param>
		/// <param name="warp">If <see langword="true"/>, the initiated task will warp the <see cref="Ped"/> rather than tell them to go to the position themselves.</param>
		/// <remarks>
		/// This method will not start a <c>CTask</c> and the <see cref="Ped"/> will do nothing for the <c>CTask</c>
		/// if the scenario manager does not have the registered hash for <paramref name="scenarioName"/>.
		/// </remarks>
		public void StartScenarioAtPosition(string scenarioName, Vector3 position, float heading, int timeToLeave = 0, bool playIntroClip = true, bool warp = true)
		{
			Function.Call(Hash.TASK_START_SCENARIO_AT_POSITION, _ped.Handle, scenarioName, position.X, position.Y, position.Z, heading, timeToLeave, playIntroClip, warp);
		}

		/// <summary>
		/// Tells the <see cref="Ped"/> to perform a task when in a <see cref="Vehicle"/> against another
		/// <see cref="Vehicle"/>.
		/// </summary>
		/// <param name="vehicle">The <see cref="Vehicle"/> to use to achieve the task.</param>
		/// <param name="target">The target <see cref="Vehicle"/>.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task in m/s.</param>
		/// <param name="drivingFlags">The driving flags for the task.</param>
		/// <param name="targetReachedDist">
		/// The distance in meters at which the AI thinks the target has been reached and the car stops.
		/// To pick default value <c>4f</c>, the parameter can be passed in as <c>-1</c> or any other values less than
		/// zero.
		/// </param>
		/// <param name="straightLineDist">
		/// The distance in meters at which the AI switches to heading for the target directly instead of following
		/// the nodes.
		/// The max acceptable value is 255, or the value will be clamp to 255 by the native function
		/// <c>TASK_VEHICLE_MISSION</c>.
		/// To pick default value <c>20f</c>, the parameter can be passed in as <c>-1</c> or any other values less than
		/// zero.
		/// </param>
		/// <param name="driveAgainstTraffic">
		/// if set to <see langword="true"/>, allows the car to drive on the opposite side of the road into incoming traffic.
		/// </param>
		public void StartVehicleMission(Vehicle vehicle, Vehicle target, VehicleMissionType missionType, float cruiseSpeed, VehicleDrivingFlags drivingFlags, float targetReachedDist, float straightLineDist, bool driveAgainstTraffic = true)
		{
			Function.Call(Hash.TASK_VEHICLE_MISSION, _ped.Handle, vehicle.Handle, target.Handle, (int)missionType, cruiseSpeed, (uint)drivingFlags, targetReachedDist, straightLineDist, driveAgainstTraffic);
		}

		/// <summary>
		/// Tells the <see cref="Ped"/> to target another <see cref="Ped"/> with a <see cref="Vehicle"/>.
		/// </summary>
		/// <param name="vehicle">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='vehicle']"
		/// />
		/// </param>
		/// <param name="target">The target <see cref="Ped"/>.</param>
		/// <param name="missionType">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='missionType']"
		/// />
		/// </param>
		/// <param name="cruiseSpeed">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='cruiseSpeed']"
		/// />
		/// </param>
		/// <param name="drivingFlags">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='drivingFlags']"
		/// />
		/// </param>
		/// <param name="targetReachedDist">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='targetReachedDist']"
		/// />
		/// </param>
		/// <param name="straightLineDist">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='straightLineDist']"
		/// />
		/// </param>
		/// <param name="driveAgainstTraffic">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='driveAgainstTraffic']"
		/// />
		/// </param>
		public void StartVehicleMission(Vehicle vehicle, Ped target, VehicleMissionType missionType, float cruiseSpeed, VehicleDrivingFlags drivingFlags, float targetReachedDist, float straightLineDist, bool driveAgainstTraffic = true)
		{
			Function.Call(Hash.TASK_VEHICLE_MISSION_PED_TARGET, _ped.Handle, vehicle.Handle, target.Handle, (int)missionType, cruiseSpeed, (uint)drivingFlags, targetReachedDist, straightLineDist, driveAgainstTraffic);
		}

		/// <summary>
		/// Tells the <see cref="Ped"/> to target a coord with a <see cref="Vehicle"/>.
		/// </summary>
		/// <param name="vehicle">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='vehicle']"
		/// />
		/// </param>
		/// <param name="target">The target coordinates.</param>
		/// <param name="missionType">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='missionType']"
		/// />
		/// </param>
		/// <param name="cruiseSpeed">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='cruiseSpeed']"
		/// />
		/// </param>
		/// <param name="drivingFlags">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='drivingFlags']"
		/// />
		/// </param>
		/// <param name="targetReachedDist">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='targetReachedDist']"
		/// />
		/// </param>
		/// <param name="straightLineDist">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='straightLineDist']"
		/// />
		/// </param>
		/// <param name="driveAgainstTraffic">
		/// <inheritdoc
		/// cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"
		/// path="/param[@name='driveAgainstTraffic']"
		/// />
		/// </param>
		public void StartVehicleMission(Vehicle vehicle, Vector3 target, VehicleMissionType missionType, float cruiseSpeed, VehicleDrivingFlags drivingFlags, float targetReachedDist, float straightLineDist, bool driveAgainstTraffic = true)
		{
			Function.Call(Hash.TASK_VEHICLE_MISSION_COORS_TARGET, _ped.Handle, vehicle.Handle, target.X, target.Y, target.Z, (int)missionType, cruiseSpeed, (uint)drivingFlags, targetReachedDist, straightLineDist, driveAgainstTraffic);
		}
		/// <summary>
		/// Tells a <see cref="Vehicle"/> to escort another <see cref="Entity"/>.
		/// Identical to <see cref="StartVehicleMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, float, bool)"/>
		/// with <see cref="VehicleMissionType.Escort"/>, but allows setting a custom offset.
		/// </summary>
		/// <param name="vehicle">The <see cref="Vehicle"/> to drive.</param>
		/// <param name="escortEntity">The <see cref="Entity"/> to escort.</param>
		/// <param name="escortType">The escort types.</param>
		/// <param name="cruiseSpeed">The cruise speed in meters.</param>
		/// <param name="drivingFlags">The driving flags.</param>
		/// <param name="customOffset">
		/// The distance how far the <see cref="Ped"/> will keep the distance between <paramref name="escortEntity"/>
		/// in meters. If less than zero, the following value will be used as a fallback in the created task:
		/// <list type="bullet">
		/// <item>
		/// <description>
		/// If <paramref name="escortType"/> is either <see cref="VehicleEscortType.Rear"/> or
		/// <see cref="VehicleEscortType.Front"/>, always 5 meters is used as a fallback regardless of the vehicle type
		/// (<see cref="VehicleType"/>). To test if the fall back value is correct for front or rear escort types,
		/// search a dumped exe for <c>0F 28 46 ? F3 0F 10 8B ? ? 00 00 0F 2F 0D ? ? ? ? 0F 29 45 ? 73 08</c>.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// If <paramref name="escortType"/> is either <see cref="VehicleEscortType.Left"/> or
		/// <see cref="VehicleEscortType.Right"/>, one of the following three fallback values will be used and the
		/// vehicle type (<see cref="VehicleType"/>) determines this. Fallback values:
		/// 	<list type="bullet">
		/// 	<item>
		/// 	<description>
		/// 	For helicopters, blimp, and autogyro types, which are <see cref="VehicleType.Helicopter"/>,
		/// 	<see cref="VehicleType.Blimp"/>, and <see cref="VehicleType.Autogyro"/>, 15 meters.
		/// 	</description>
		/// 	</item>
		/// 	<item>
		/// 	<description>
		/// 	For motorcycle type (<see cref="VehicleType.Motorcycle"/>), 1.4 meters. This does not include bicycle
		/// 	type (<see cref="VehicleType.Bicycle"/>).
		/// 	</description>
		/// 	</item>
		/// 	<item>
		/// 	<description>
		/// 	For any vehicle types other than, heli, blimp, motorcycle, and autogyro, 2 meters. Do note that this
		/// 	fallback value applies for plane type <see cref="VehicleType.Plane"/>.
		/// 	</description>
		/// 	</item>
		///     </list>
		/// To test if the fall back value is correct for left or right escort types, search a dumped exe for
		/// <c>0F 57 F6 0F 2F FE 44 0F 28 C6 73 32</c>.
		/// </description>
		/// </item>
		/// </list>
		/// The fallback values are subject to change by game updates.
		/// </param>
		/// <param name="minHeightAboveTerrain">
		/// Only used for helicopters. The height in meters that the heli will try to stay above terrain
		/// (i.e. 20 == always tries to stay at least 20 meters above ground).
		/// </param>
		/// <param name="straightLineDistance">
		/// The distance to target in meters at which the <see cref="Ped"/> will start driving straight instead of
		/// following vehicle nodes.
		/// </param>
		public void VehicleEscort(Vehicle vehicle, Entity escortEntity, VehicleEscortType escortType,
			float cruiseSpeed, VehicleDrivingFlags drivingFlags, float customOffset = -1f,
			int minHeightAboveTerrain = 20, float straightLineDistance = 20f)
		{
			Function.Call(Hash.TASK_VEHICLE_ESCORT, _ped.Handle, vehicle, escortEntity, (int)escortType, cruiseSpeed,
				(int)drivingFlags, customOffset, minHeightAboveTerrain, straightLineDistance);
		}
		/// <summary>
		/// Tells a <see cref="Vehicle"/> to follow another <see cref="Entity"/>.
		/// This task sits sort of in between <see cref="VehicleEscort"/> and <see cref="VehicleChase"/>.
		/// Not as fine-controlled as <see cref="VehicleEscort"/> but not as aggressive as <see cref="VehicleChase"/>.
		/// This task is preferable to <see cref="VehicleEscort"/> when the following vehicle might start off in front
		/// of the thing it's supposed to follow.
		/// </summary>
		/// <param name="vehicle">
		/// The <see cref="Vehicle"/> to use to follow <paramref name="followEntity"/>.
		/// </param>
		/// <param name="followEntity">The <see cref="Entity"/> to follow.</param>
		/// <param name="cruiseSpeed">The cruise speed in m/s.</param>
		/// <param name="drivingFlags">The driving flags.</param>
		/// <param name="followDistance">The follow distance.</param>
		public void VehicleFollow(Vehicle vehicle, Entity followEntity, float cruiseSpeed,
			VehicleDrivingFlags drivingFlags, int followDistance = 20)
		{
			Function.Call(Hash.TASK_VEHICLE_FOLLOW, _ped.Handle, vehicle, followEntity, cruiseSpeed, (int)drivingFlags,
				followDistance);
		}

		/// <summary>
		/// Tells a helicopter to protect another <see cref="Entity"/>.
		/// Identical to <see cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"/>
		/// with <see cref="VehicleMissionType.HeliProtect"/>, but allows setting a custom offset.
		/// </summary>
		public void VehicleHeliProtect(Vehicle heli, Entity protectEntity, float cruiseSpeed,
			VehicleDrivingFlags drivingFlags, float customOffset = -1f, int minHeightAboveTerrain = 20,
			HeliMissionFlags missionFlags = HeliMissionFlags.None)
		{
			Function.Call(Hash.TASK_VEHICLE_HELI_PROTECT, _ped.Handle, heli.Handle, protectEntity, cruiseSpeed,
				(int)drivingFlags, customOffset, minHeightAboveTerrain, (int)missionFlags);
		}

		/// <summary>Gives the helicopter a mission.</summary>
		/// <param name="heli">The helicopter.</param>
		/// <param name="target">The target <see cref="Vehicle"/>.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task in m/s.</param>
		/// <param name="targetReachedDist">
		/// The distance in meters at which heli thinks it's arrived.
		/// Also used as the hover distance for <see cref="VehicleMissionType.Attack"/> and
		/// <see cref="VehicleMissionType.Circle"/>.
		/// To pick default value <c>4f</c>, the parameter can be passed in as <c>-1</c> or any other values less than
		/// zero.
		/// </param>
		/// <param name="flightHeight">
		/// The Z coordinate the heli tries to maintain (i.e. 30 == 30 meters above sea level).
		/// </param>
		/// <param name="minHeightAboveTerrain">
		/// The height in meters that the heli will try to stay above terrain (ie 20 == always tries to stay at least
		/// 20 meters above ground).
		/// </param>
		/// <param name="heliOrientation">
		/// The orientation the heli tries to be in (<c>0f</c> to <c>360f</c>).
		/// Use <c>-1f</c> (or any value less than zero) if not bothered. <c>-1f</c> Should be used in 99% of the times.
		/// </param>
		/// <param name="slowDownDistance">
		/// In general, get more control with big number and more dynamic with smaller.
		/// Setting to <c>-1</c> means use default tuning (<c>100</c>).
		/// </param>
		/// <param name="missionFlags">The heli mission flags for the task.</param>
		public void StartHeliMission(Vehicle heli, Vehicle target, VehicleMissionType missionType, float cruiseSpeed, float targetReachedDist, int flightHeight, int minHeightAboveTerrain, float heliOrientation = -1f, float slowDownDistance = -1f, HeliMissionFlags missionFlags = HeliMissionFlags.None)
		{
			Function.Call(Hash.TASK_HELI_MISSION, _ped.Handle, heli.Handle, target.Handle, 0, 0f, 0f, 0f, (int)missionType, cruiseSpeed, targetReachedDist, heliOrientation, flightHeight, minHeightAboveTerrain, slowDownDistance, (int)missionFlags);
		}

		/// <summary>Gives the helicopter a mission.</summary>
		/// <param name="heli">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='heli']"
		/// />
		/// </param>
		/// <param name="target">The target <see cref="Ped"/>.</param>
		/// <param name="missionType">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='missionType']"
		/// />
		/// </param>
		/// <param name="cruiseSpeed">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='cruiseSpeed']"
		/// />
		/// </param>
		/// <param name="targetReachedDist">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='targetReachedDist']"
		/// />
		/// </param>
		/// <param name="flightHeight">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='flightHeight']"
		/// />
		/// </param>
		/// <param name="minHeightAboveTerrain">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='minHeightAboveTerrain']"
		/// />
		/// </param>
		/// <param name="heliOrientation">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='heliOrientation']"
		/// />
		/// </param>
		/// <param name="slowDownDistance">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='slowDownDistance']"
		/// />
		/// </param>
		/// <param name="missionFlags">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='missionFlags']"
		/// />
		/// </param>
		public void StartHeliMission(Vehicle heli, Ped target, VehicleMissionType missionType, float cruiseSpeed, float targetReachedDist, int flightHeight, int minHeightAboveTerrain, float heliOrientation = -1f, float slowDownDistance = -1f, HeliMissionFlags missionFlags = HeliMissionFlags.None)
		{
			Function.Call(Hash.TASK_HELI_MISSION, _ped.Handle, heli.Handle, 0, target.Handle, 0f, 0f, 0f, (int)missionType, cruiseSpeed, targetReachedDist, heliOrientation, flightHeight, minHeightAboveTerrain, slowDownDistance, (int)missionFlags);
		}

		/// <summary>Gives the helicopter a mission.</summary>
		/// <param name="heli">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='heli']"
		/// />
		/// </param>
		/// <param name="target">The target coordinate.</param>
		/// <param name="missionType">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='missionType']"
		/// />
		/// </param>
		/// <param name="cruiseSpeed">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='cruiseSpeed']"
		/// />
		/// </param>
		/// <param name="targetReachedDist">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='targetReachedDist']"
		/// />
		/// </param>
		/// <param name="flightHeight">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='flightHeight']"
		/// />
		/// </param>
		/// <param name="minHeightAboveTerrain">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='minHeightAboveTerrain']"
		/// />
		/// </param>
		/// <param name="heliOrientation">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='heliOrientation']"
		/// />
		/// </param>
		/// <param name="slowDownDistance">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='slowDownDistance']"
		/// />
		/// </param>
		/// <param name="missionFlags">
		/// <inheritdoc
		/// cref="StartHeliMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, float, HeliMissionFlags)"
		/// path="/param[@name='missionFlags']"
		/// />
		/// </param>
		public void StartHeliMission(Vehicle heli, Vector3 target, VehicleMissionType missionType, float cruiseSpeed, float targetReachedDist, int flightHeight, int minHeightAboveTerrain, float heliOrientation = -1f, float slowDownDistance = -1f, HeliMissionFlags missionFlags = HeliMissionFlags.None)
		{
			Function.Call(Hash.TASK_HELI_MISSION, _ped.Handle, heli.Handle, 0, 0, target.X, target.Y, target.Z, (int)missionType, cruiseSpeed, targetReachedDist, heliOrientation, flightHeight, minHeightAboveTerrain, slowDownDistance, (int)missionFlags);
		}
		/// <summary>
		/// Gives a helicopter a mission to escort another heli at an offset position.
		/// </summary>
		/// <param name="heli">
		/// The helicopter for the <see cref="Ped"/> to escort <paramref name="escortHeli"/>.
		/// </param>
		/// <param name="escortHeli">
		/// The helicopter to escort. If <see cref="Vehicle.Type"/> is a type other than
		/// <see cref="VehicleType.Helicopter"/> on this argument, the method will fail without giving an escort task.
		/// </param>
		/// <param name="offset">
		/// The escort offset.
		/// </param>
		public void HeliEscortHeli(Vehicle heli, Vehicle escortHeli, Vector3 offset)
			=> Function.Call(Hash.TASK_HELI_ESCORT_HELI, _ped.Handle, heli, escortHeli, offset.X, offset.Y, offset.Z);

		/// <summary>Gives a plane a mission.</summary>
		/// <param name="plane">The helicopter.</param>
		/// <param name="target">The target <see cref="Vehicle"/>.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task in m/s.</param>
		/// <param name="targetReachedDist">
		/// Distance in meters at which heli thinks it's arrived.
		/// Also used as the hover distance for <see cref="VehicleMissionType.Attack"/> and
		/// <see cref="VehicleMissionType.Circle"/>.
		/// To pick default value <c>4f</c>, the parameter can be passed in as <c>-1</c> or any other values less than
		/// zero.
		/// </param>
		/// <param name="flightHeight">
		/// The Z coordinate the heli tries to maintain (i.e. 30 == 30 meters above sea level).
		/// </param>
		/// <param name="minHeightAboveTerrain">
		/// The height in meters that the heli will try to stay above terrain
		/// (ie 20 == always tries to stay at least 20 meters above ground).
		/// </param>
		/// <param name="planeOrientation">
		/// The orientation the plane tries to be in (<c>0f</c> to <c>360f</c>). Use <c>-1f</c> if not bothered.
		/// <c>-1f</c> Should be used in 99% of the times.
		/// </param>
		/// <param name="precise">
		/// Specifies whether to tell the plane to move precisely with VTOL.
		/// If <see langword="true"/> and the plane supports VTOL, the plane will use VTOL and set the vertical flight
		/// nozzles position to the position for vertical flight.
		/// If <see langword="true"/> and the plane supports VTOL, the plane will not use VTOL and set the vertical flight
		/// nozzles position to the position for horizontal flight.
		/// If the plane does not support VTOL, this parameter has no effect.
		/// </param>
		public void StartPlaneMission(Vehicle plane, Vehicle target, VehicleMissionType missionType, float cruiseSpeed,
			float targetReachedDist, int flightHeight, int minHeightAboveTerrain, float planeOrientation = -1f,
			bool precise = true)
		{
			Function.Call(Hash.TASK_PLANE_MISSION, _ped.Handle, plane.Handle, target.Handle, 0, 0f, 0f, 0f, (int)missionType, cruiseSpeed, targetReachedDist, planeOrientation, flightHeight, minHeightAboveTerrain, precise);
		}

		/// <summary>
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/summary"
		/// />
		/// </summary>
		/// <param name="plane">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='plane']"
		/// />
		/// </param>
		/// <param name="target">The target <see cref="Ped"/>.</param>
		/// <param name="missionType">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='missionType']"
		/// />
		/// </param>
		/// <param name="cruiseSpeed">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='cruiseSpeed']"
		/// />
		/// </param>
		/// <param name="targetReachedDist">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='targetReachedDist']"
		/// />
		/// </param>
		/// <param name="flightHeight">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='flightHeight']"
		/// />
		/// </param>
		/// <param name="minHeightAboveTerrain">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='minHeightAboveTerrain']"
		/// />
		/// </param>
		/// <param name="planeOrientation">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='planeOrientation']"
		/// />
		/// </param>
		/// <param name="precise">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='precise']"
		/// />
		/// </param>
		public void StartPlaneMission(Vehicle plane, Ped target, VehicleMissionType missionType, float cruiseSpeed,
			float targetReachedDist, int flightHeight, int minHeightAboveTerrain, float planeOrientation = -1f,
			bool precise = true)
		{
			Function.Call(Hash.TASK_PLANE_MISSION, _ped.Handle, plane.Handle, 0, target.Handle, 0f, 0f, 0f, (int)missionType, cruiseSpeed, targetReachedDist, planeOrientation, flightHeight, minHeightAboveTerrain, precise);
		}

		/// <summary>
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/summary"
		/// />
		/// </summary>
		/// <param name="plane">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='plane']"
		/// />
		/// </param>
		/// <param name="target">The target coordinate.</param>
		/// <param name="missionType">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='missionType']"
		/// />
		/// </param>
		/// <param name="cruiseSpeed">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='cruiseSpeed']"
		/// />
		/// </param>
		/// <param name="targetReachedDist">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='targetReachedDist']"
		/// />
		/// </param>
		/// <param name="flightHeight">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='flightHeight']"
		/// />
		/// </param>
		/// <param name="minHeightAboveTerrain">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='minHeightAboveTerrain']"
		/// />
		/// </param>
		/// <param name="planeOrientation">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='planeOrientation']"
		/// />
		/// </param>
		/// <param name="precise">
		/// <inheritdoc
		/// cref="StartPlaneMission(Vehicle, Vehicle, VehicleMissionType, float, float, int, int, float, bool)"
		/// path="/param[@name='precise']"
		/// />
		/// </param>
		public void StartPlaneMission(Vehicle plane, Vector3 target, VehicleMissionType missionType, float cruiseSpeed,
			float targetReachedDist, int flightHeight, int minHeightAboveTerrain, float planeOrientation = -1f,
			bool precise = true)
		{
			Function.Call(Hash.TASK_PLANE_MISSION, _ped.Handle, plane.Handle, 0, 0, target.X, target.Y, target.Z, (int)missionType, cruiseSpeed, targetReachedDist, planeOrientation, flightHeight, minHeightAboveTerrain, precise);
		}
		/// <summary>
		/// Gives plane a task to drive/taxi along the runway on the ground.
		/// </summary>
		/// <param name="plane">The plane to use/give a task.</param>
		/// <param name="position">The target position.</param>
		/// <param name="cruiseSpeed">The cruise speed in m/s.</param>
		/// <param name="targetReachedDist">
		/// The distance in meters at which the plane thinks it's arrived and the task stops executing.
		/// </param>
		public void PlaneTaxi(Vehicle plane, Vector3 position, float cruiseSpeed, float targetReachedDist)
		{
			Function.Call(Hash.TASK_PLANE_TAXI, _ped.Handle, plane, position.X, position.Y, position.Z, cruiseSpeed,
				targetReachedDist);
		}

		/// <summary>Gives the boat a mission.</summary>
		/// <param name="boat">The boat.</param>
		/// <param name="target">The target <see cref="Vehicle"/>.</param>
		/// <param name="missionType">The vehicle mission type.</param>
		/// <param name="cruiseSpeed">The cruise speed for the task in m/s.</param>
		/// <param name="drivingFlags">The driving flags for the task.</param>
		/// <param name="targetReachedDist">
		/// The distance in meters at which boat thinks it's arrived.
		/// Also used as the hover distance for <see cref="VehicleMissionType.Attack"/> and
		/// <see cref="VehicleMissionType.Circle"/>.
		/// To pick default value <c>4f</c>, the parameter can be passed in as <c>-1</c> or any other values less than
		/// zero.
		/// </param>
		/// <param name="missionFlags">The boat mission flags for the task.</param>
		public void StartBoatMission(Vehicle boat, Vehicle target, VehicleMissionType missionType, float cruiseSpeed, VehicleDrivingFlags drivingFlags, float targetReachedDist, BoatMissionFlags missionFlags)
		{
			Function.Call(Hash.TASK_BOAT_MISSION, _ped.Handle, boat.Handle, target.Handle, 0, 0f, 0f, 0f, (int)missionType, cruiseSpeed, (uint)drivingFlags, targetReachedDist, (int)missionFlags);
		}

		/// <summary>Gives the boat a mission.</summary>
		/// <param name="boat">
		/// <inheritdoc
		/// cref="StartBoatMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, BoatMissionFlags)"
		/// path="/param[@name='boat']"
		/// />
		/// </param>
		/// <param name="target">The target <see cref="Ped"/>.</param>
		/// <param name="missionType">
		/// <inheritdoc
		/// cref="StartBoatMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, BoatMissionFlags)"
		/// path="/param[@name='missionType']"
		/// />
		/// </param>
		/// <param name="cruiseSpeed">
		/// <inheritdoc
		/// cref="StartBoatMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, BoatMissionFlags)"
		/// path="/param[@name='cruiseSpeed']"
		/// />
		/// </param>
		/// <param name="drivingFlags">
		/// <inheritdoc
		/// cref="StartBoatMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, BoatMissionFlags)"
		/// path="/param[@name='drivingFlags']"
		/// />
		/// </param>
		/// <param name="targetReachedDist">
		/// <inheritdoc
		/// cref="StartBoatMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, BoatMissionFlags)"
		/// path="/param[@name='targetReachedDist']"
		/// />
		/// </param>
		/// <param name="missionFlags">
		/// <inheritdoc
		/// cref="StartBoatMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, BoatMissionFlags)"
		/// path="/param[@name='missionFlags']"
		/// />
		/// </param>
		public void StartBoatMission(Vehicle boat, Ped target, VehicleMissionType missionType, float cruiseSpeed, VehicleDrivingFlags drivingFlags, float targetReachedDist, BoatMissionFlags missionFlags)
		{
			Function.Call(Hash.TASK_BOAT_MISSION, _ped.Handle, boat.Handle, 0, target.Handle, 0f, 0f, 0f, (int)missionType, cruiseSpeed, (uint)drivingFlags, targetReachedDist, (int)missionFlags);
		}

		/// <summary>Gives the boat a mission.</summary>
		/// <param name="boat">
		/// <inheritdoc
		/// cref="StartBoatMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, BoatMissionFlags)"
		/// path="/param[@name='boat']"
		/// />
		/// </param>
		/// <param name="target">The target coordinate.</param>
		/// <param name="missionType">
		/// <inheritdoc
		/// cref="StartBoatMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, BoatMissionFlags)"
		/// path="/param[@name='missionType']"
		/// />
		/// </param>
		/// <param name="cruiseSpeed">
		/// <inheritdoc
		/// cref="StartBoatMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, BoatMissionFlags)"
		/// path="/param[@name='cruiseSpeed']"
		/// />
		/// </param>
		/// <param name="drivingFlags">
		/// <inheritdoc
		/// cref="StartBoatMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, BoatMissionFlags)"
		/// path="/param[@name='drivingFlags']"
		/// />
		/// </param>
		/// <param name="targetReachedDist">
		/// <inheritdoc
		/// cref="StartBoatMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, BoatMissionFlags)"
		/// path="/param[@name='targetReachedDist']"
		/// />
		/// </param>
		/// <param name="missionFlags">
		/// <inheritdoc
		/// cref="StartBoatMission(Vehicle, Vehicle, VehicleMissionType, float, VehicleDrivingFlags, float, BoatMissionFlags)"
		/// path="/param[@name='missionFlags']"
		/// />
		/// </param>
		public void StartBoatMission(Vehicle boat, Vector3 target, VehicleMissionType missionType, float cruiseSpeed, VehicleDrivingFlags drivingFlags, float targetReachedDist, BoatMissionFlags missionFlags)
		{
			Function.Call(Hash.TASK_BOAT_MISSION, _ped.Handle, boat.Handle, 0, 0, target.X, target.Y, target.Z, (int)missionType, cruiseSpeed, (uint)drivingFlags, targetReachedDist, (int)missionFlags);
		}

		/// <summary>
		/// <para>
		/// Tells a plane with VTOL (like <see cref="VehicleHash.Hydra"/> or <see cref="VehicleHash.Avenger"/>)
		/// to move precisely throughout the world. Will fail for other <see cref="Vehicle"/>s.
		/// </para>
		/// <para>
		/// Not available in the game versions earlier than v1.0.1290.1.
		/// </para>
		/// </summary>
		/// <param name="plane">The plane to apply the task.</param>
		/// <param name="target">The target coordinates.</param>
		/// <param name="flightHeight">
		/// The Z coordinate the heli tries to maintain (i.e. 30 == 30 meters above sea level).
		/// </param>
		/// <param name="minHeightAboveTerrain">
		/// The height in meters that the heli will try to stay above terrain
		/// (ie 20 == always tries to stay at least 20 meters above ground).
		/// </param>
		/// <param name="desiredOrientation">
		/// The orientation the plane tries to be in (<c>0f</c> to <c>360f</c>).
		/// Set <see langword="null"/> to not constrain.
		/// </param>
		/// <param name="autoPilot">
		/// Specifies whether to apply the plane goto task directly to the <see cref="Vehicle"/>, and apply some flags
		/// to allow this task to run with no driver.
		/// </param>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if called in the game versions earlier than v1.0.1290.1.
		/// </exception>
		public void GoToPlanePreciseVtol(Vehicle plane, Vector3 target, int flightHeight, int minHeightAboveTerrain,
			float? desiredOrientation = null, bool autoPilot = false)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_1290_1_Steam, nameof(TaskInvoker),
				nameof(GoToPlanePreciseVtol));

			bool useDesiredOrientation = desiredOrientation.HasValue;
			Function.Call(Hash.TASK_PLANE_GOTO_PRECISE_VTOL, _ped.Handle, plane, target.X, target.Y, target.Z,
				flightHeight, minHeightAboveTerrain, useDesiredOrientation, desiredOrientation ?? 0f,
				autoPilot);
		}

		/// <summary>
		/// <para>
		/// Tells a submarine to goto and stop at the position given.
		/// </para>
		/// <para>
		/// Only available in the game version v1.0.1290.1 or later versions.
		/// </para>
		/// </summary>
		/// <param name="submarine">The submarine to use or directly apply the task.</param>
		/// <param name="position">The target position.</param>
		/// <param name="autoPilot">
		/// If <see langword="true"/>, a <c>CTaskVehicleGoToSubmarine</c> will be directly
		/// applied to the <see cref="Vehicle"/>, and apply some flags to allow this task to run with no driver.
		/// If <see langword="false"/>, a <c>CTaskVehicleGoToSubmarine</c> will be applied as a part of
		/// <c>CTaskControlVehicle</c> in the <see cref="Ped"/>.
		/// </param>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if called in the game versions earlier than v1.0.2189.0.
		/// </exception>
		/// <remarks>
		/// Cannot be used in a <see cref="TaskSequence"/> if <paramref name="autoPilot"/> is <see langword="true"/>,
		/// since <c>TASK_SUBMARINE_GOTO_AND_STOP</c> directly apply the task to the task manager of the
		/// <see cref="Vehicle"/>'s intelligence in such case.
		/// </remarks>
		public void GoToSubmarineAndStop(Vehicle submarine, Vector3 position, bool autoPilot = false)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_2189_0_Steam, nameof(TaskInvoker),
				nameof(GoToSubmarineAndStop));

			Function.Call(Hash.TASK_SUBMARINE_GOTO_AND_STOP, _ped.Handle, submarine, position.X, position.Y, position.Z,
				autoPilot);
		}


		/// <inheritdoc cref="SwapWeapon(bool)"/>
		public void SwapWeapon() => SwapWeapon(false);
		/// <summary>
		/// Tells the <see cref="Ped"/> to swap their weapon.
		/// </summary>
		/// <param name="drawWeapon">If <see langword="true"/>, the <see cref="Ped"/> will start to swap while the current weapon is drawn.</param>
		public void SwapWeapon(bool drawWeapon)
		{
			Function.Call(Hash.TASK_SWAP_WEAPON, _ped.Handle, drawWeapon);
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

		/// <summary>
		/// The <see cref="Ped"/> will chase the target <see cref="Ped"/>'s <see cref="Vehicle"/> with their own
		/// <see cref="Vehicle"/>. Both <see cref="Ped"/>s must be in <see cref="Vehicle"/>s, or the task will abort.
		/// </summary>
		/// <remarks>
		/// Cannot be used in a <see cref="TaskSequence"/> since <c>TASK_VEHICLE_CHASE</c> does not expect the zero
		/// handle for task sequences.
		/// </remarks>
		public void VehicleChase(Ped target)
		{
			Function.Call(Hash.TASK_VEHICLE_CHASE, _ped.Handle, target.Handle);
		}
		/// <summary>
		/// Tells the <see cref="Ped"/> in a heli to chase an <see cref="Entity"/>. The <see cref="Ped"/> must be in
		/// a heli.
		/// </summary>
		/// <remarks>
		/// Cannot be used in a <see cref="TaskSequence"/> since <c>TASK_HELI_CHASE</c> does not expect the zero
		/// handle for task sequences.
		/// </remarks>
		public void HeliChase(Entity target, Vector3 targetOffset)
		{
			Function.Call(Hash.TASK_HELI_CHASE, _ped.Handle, target.Handle, targetOffset.X, targetOffset.Y,
				targetOffset.Z);
		}
		/// <summary>
		/// Tells the <see cref="Ped"/> in a plane to land. The <see cref="Ped"/> will try to land in between the start
		/// and end coords.
		/// </summary>
		/// <remarks>
		/// Cannot be used in a <see cref="TaskSequence"/> since <c>TASK_PLANE_CHASE</c> does not expect the zero
		/// handle for task sequences.
		/// </remarks>
		public void PlaneChase(Vehicle plane, Vector3 runWayStart, Vector3 runWayEnd)
		{
			Function.Call(Hash.TASK_PLANE_CHASE, _ped.Handle, plane.Handle, runWayStart.X, runWayStart.Y,
				runWayStart.Z, runWayEnd.X, runWayEnd.Y, runWayEnd.Z);
		}
		/// <summary>
		/// Tells the <see cref="Ped"/> in a heli to chase an <see cref="Entity"/>. The <see cref="Ped"/> must be in
		/// a heli.
		/// </summary>
		/// <remarks>
		/// Cannot be used in a <see cref="TaskSequence"/> since <c>TASK_PLANE_CHASE</c> does not expect the zero
		/// handle for task sequences.
		/// </remarks>
		public void PlaneChase(Entity target, Vector3 targetOffset)
		{
			Function.Call(Hash.TASK_PLANE_CHASE, _ped.Handle, target.Handle, targetOffset.X, targetOffset.Y,
				targetOffset.Z);
		}

		public void VehicleShootAtPed(Ped target)
		{
			Function.Call(Hash.TASK_VEHICLE_SHOOT_AT_PED, _ped.Handle, target.Handle, 20f);
		}

		[Obsolete("TaskInvoke.Wait is obsolete, use TaskInvoker.Pause instead.")]
		public void Wait(int duration) => Pause(duration);

		/// <summary>
		/// Tells the <see cref="Ped"/> to wander.
		/// </summary>
		public void Wander(float heading = DefaultNavmeshFinalHeading, bool keepMovingWhilstWaitingForFirstPath = false)
		{
			// the 3nd argument is actually a flag value, but only the 1st bit is used as of b2845
			Function.Call(Hash.TASK_WANDER_STANDARD, _ped.Handle, heading, keepMovingWhilstWaitingForFirstPath ? 1 : 0);
		}
		[Obsolete("the overload of TaskInvoker.WanderAround with no parameters is obsolete, use TaskInvoker.Wander instead.")]
		public void WanderAround() => Wander(0, false);

		/// <inheritdoc cref="WanderAround(Vector3, float, float, float)"/>
		public void WanderAround(Vector3 position, float radius) => WanderAround(position, radius, 0, 0);
		/// <summary>
		/// Tells the <see cref="Ped"/> to wander within a certain radius from the given position indefinitely.
		/// </summary>
		/// <param name="position">The center position.</param>
		/// <param name="radius">The max radius the <see cref="Ped"/> can wander around <paramref name="position"/>.</param>
		/// <param name="minTime">
		/// The minimum time for the wait time in seconds before the <see cref="Ped"/> starts wandering.
		/// Must not be negative or more than <paramref name="maxTime"/>.
		/// </param>
		/// <param name="maxTime">
		/// The maximum time for the wait time in seconds before the <see cref="Ped"/> starts wandering.
		/// Must not be negative or less than <paramref name="maxTime"/>.
		/// </param>
		/// <remarks>
		/// The initiated task will put the <see cref="Ped"/> to the wait state if the <see cref="Ped"/> gets interrupted by a <c>CEvent</c> after the interruption.
		/// </remarks>
		public void WanderAround(Vector3 position, float radius, float minTime, float maxTime)
		{
			Function.Call(Hash.TASK_WANDER_IN_AREA, _ped.Handle, position.X, position.Y, position.Z, radius, minTime, maxTime);
		}

		public void WarpIntoVehicle(Vehicle vehicle, VehicleSeat seat)
		{
			Function.Call(Hash.TASK_WARP_PED_INTO_VEHICLE, _ped.Handle, vehicle.Handle, (int)seat);
		}

		public void WarpOutOfVehicle(Vehicle vehicle) => LeaveVehicle(vehicle, LeaveVehicleFlags.WarpOut);

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

		/// <summary>
		/// Attempts to stop a play anim task initiated by <see cref="PlayAnimation(CrClipAsset)"/>.
		/// Does not stop non-scripted animation tasks.
		/// </summary>
		/// <param name="crClipAsset">
		/// The <see cref="CrClipAsset"/> to find the corresponding clip.
		/// </param>
		/// <param name="blendOutDelta">
		/// The blend out delta. if set to <see langword="null"/>, <see cref="AnimationBlendDelta.Normal"/> will be used.
		/// </param>
		public void StopScriptedAnimationTask(CrClipAsset crClipAsset, AnimationBlendDelta? blendOutDelta = null)
		{
			(CrClipDictionary clipDict, string clipName) = crClipAsset;
			float deltaArg = blendOutDelta.HasValue
				? -(float)(blendOutDelta.Value)
				: -(AnimationBlendDelta.Normal.Value);

			Function.Call(Hash.STOP_ANIM_TASK, _ped.Handle, clipDict, clipName, deltaArg);
		}

		[Obsolete("Use StopScriptedAnimationTask instead.")]
		public void ClearAnimation(string animSet, string animName)
		{
			Function.Call(Hash.STOP_ANIM_TASK, _ped.Handle, animSet, animName, -4f);
		}
	}
}
