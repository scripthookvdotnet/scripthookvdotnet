//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// An enumeration of known combat attributes (combat behavior flags) for the <see cref="Ped"/>,
	/// which is used to set or unset the combat attributes on a <c>CPedIntelligence</c> of <c>CPed</c>.
	/// </summary>
	/// <remarks>
	/// You can check if names of this enum are included in the exe by searching the dumped exe for hashed values
	/// of names like <c>BF_[enum name]</c> without case conversion (for example, search the dumped exe for 0x61C7368E,
	/// which is the hashed value of <c>BF_CanUseCover</c>).
	/// Although there is a enum whose hash is 0x0E8E7201 and where contains most of the member listed in this enum
	/// <see cref="CombatAttributes"/> in the exe, the enum name for 0x0E8E7201 is unknown.
	/// </remarks>
	public enum CombatAttributes
	{
		/// <summary>
		/// AI will only use cover if this is set.
		/// </summary>
		CanUseCover,
		/// <summary>
		/// AI will only use <see cref="Vehicle"/>s if this is set.
		/// </summary>
		CanUseVehicles,
		/// <summary>
		/// AI will only driveby from a <see cref="Vehicle"/> if this is set.
		/// </summary>
		CanDoDrivebys,
		/// <summary>
		/// Will be forced to stay in a <see cref="Vehicle"/> if this isn't set.
		/// </summary>
		CanLeaveVehicle,
		/// <summary>
		/// The <see cref="Ped"/> can make decisions on whether to strafe or not based on distance to destination,
		/// recent bullet events, etc.
		/// </summary>
		CanUseDynamicStrafeDecisions,
		/// <summary>
		/// <see cref="Ped"/> will always fight upon getting threat response task.
		/// </summary>
		AlwaysFight,
		/// <summary>
		/// If in combat and in a <see cref="Vehicle"/>, the <see cref="Ped"/> will flee rather than attacking.
		/// </summary>
		FleeWhilstInVehicle,
		/// <summary>
		/// <see cref="Ped"/>s will scan for and react to dead <see cref="Ped"/>s found
		/// </summary>
		WillScanForDeadPeds = 9,
		/// <summary>
		/// The <see cref="Ped"/> will seek cover only.
		/// </summary>
		JustSeekCover = 11,
		/// <summary>
		/// <see cref="Ped"/> will only blind fire when in cover.
		/// </summary>
		BlindFireWhenInCover,
		/// <summary>
		/// <see cref="Ped"/> may advance.
		/// </summary>
		Aggressive,
		/// <summary>
		/// <see cref="Ped"/> can investigate events such as distant gunfire, footsteps, explosions etc.
		/// </summary>
		CanInvestigate,
		/// <summary>
		/// <see cref="Ped"/> can use a radio to call for backup (happens after a reaction).
		/// </summary>
		HasRadio,
		/// <summary>
		/// <see cref="Ped"/> will always flee upon getting threat response task.
		/// </summary>
		AlwaysFlee = 17,
		/// <summary>
		/// <see cref="Ped"/> can do unarmed taunts in <see cref="Vehicle"/>.
		/// </summary>
		CanTauntInVehicle = 20,
		/// <summary>
		/// <see cref="Ped"/> will be able to chase their targets if both are on foot
		/// and the target is running away.
		/// </summary>
		CanChaseTargetOnFoot = 21,
		/// <summary>
		/// <see cref="Ped"/> can drag injured <see cref="Ped"/>s to safety.
		/// </summary>
		WillDragInjuredPedsToSafety = 22,
		/// <summary>
		/// <see cref="Ped"/> will require LOS to the target it is aiming at before shooting.
		/// </summary>
		RequiresLosToShoot,
		/// <summary>
		/// <see cref="Ped"/> is allowed to use proximity based fire rate
		/// (increasing fire rate at closer distances).
		/// </summary>
		UseProximityFiringRate,
		/// <summary>
		/// Normally <see cref="Ped"/>s can switch briefly to a secondary target in combat,
		/// setting this will prevent that.
		/// </summary>
		DisableSecondaryTarget,
		/// <summary>
		/// This will disable the flinching combat entry reactions for <see cref="Ped"/>s,
		/// instead only playing the turn and aim anims.
		/// </summary>
		DisableEntryReactions,
		/// <summary>
		/// Force <see cref="Ped"/> to be 100% accurate in all situations.
		/// </summary>
		PerfectAccuracy,
		/// <summary>
		/// If we don't have cover and can't see our target it's possible we will advance,
		/// even if the target is in cover.
		/// </summary>
		CanUseFrustratedAdvance,
		/// <summary>
		/// This will have the <see cref="Ped"/> move to defensive areas and within attack windows
		/// before performing the cover search.
		/// </summary>
		MoveToLocationBeforeCoverSearch,
		/// <summary>
		/// <see cref="Ped"/> will try to maintain a min distance to the target, even if using defensive areas
		/// (currently only for cover finding + usage).
		/// </summary>
		MaintainMinDistanceToTarget = 31,
		/// <summary>
		/// Allows <see cref="Ped"/> to use steamed variations of peeking anims.
		/// </summary>
		CanUsePeekingVariations = 34,
		/// <summary>
		/// Disables pinned down behaviors.
		/// </summary>
		DisablePinnedDown = 35,
		/// <summary>
		/// Disables pinning down others.
		/// </summary>
		DisablePinDownOthers,
		/// <summary>
		/// Disables bullet reactions.
		/// </summary>
		DisableBulletReactions = 38,
		/// <summary>
		/// Allows <see cref="Ped"/> to bust the player.
		/// </summary>
		CanBust,
		/// <summary>
		/// This <see cref="Ped"/> is ignored by other <see cref="Ped"/>s when wanted.
		/// </summary>
		IgnoredByOtherPedsWhenWanted,
		/// <summary>
		/// <see cref="Ped"/> is allowed to "jack" <see cref="Vehicle"/>s when needing to chase a target in combat.
		/// </summary>
		CanCommandeerVehicles,
		/// <summary>
		/// <see cref="Ped"/> is allowed to flank.
		/// </summary>
		CanFlank,
		/// <summary>
		/// <see cref="Ped"/> will switch to advance if they can't find cover.
		/// </summary>
		SwitchToAdvanceIfCantFindCover,
		/// <summary>
		/// <see cref="Ped"/> will switch to defensive if they are in cover.
		/// </summary>
		SwitchToDefensiveIfInCover,
		/// <summary>
		/// <see cref="Ped"/> will clear their primary defensive area when it is reached.
		/// </summary>
		ClearPrimaryDefensiveAreaWhenReached,
		/// <summary>
		/// <see cref="Ped"/> is allowed to fight armed <see cref="Ped"/>s when not armed.
		/// </summary>
		CanFightArmedPedsWhenNotArmed,
		/// <summary>
		/// <see cref="Ped"/> is not allowed to use tactical points if set to use defensive movement (will only use cover).
		/// </summary>
		EnableTacticalPointsWhenDefensive,
		/// <summary>
		/// <see cref="Ped"/> cannot adjust cover arcs when testing cover safety (atm done on corner cover points
		/// when <see cref="Ped"/> usingdefensive area + no LOS).
		/// </summary>
		DisableCoverArcAdjustments,
		/// <summary>
		/// <see cref="Ped"/> may use reduced accuracy with large number of enemies attacking the same local player target.
		/// </summary>
		UseEnemyAccuracyScaling,
		/// <summary>
		/// <see cref="Ped"/> is allowed to charge the enemy position.
		/// </summary>
		CanCharge,
		/// <summary>
		/// Use the vehicle attack mission during combat (only works on driver).
		/// </summary>
		UseVehicleAttack = 52,
		/// <summary>
		/// Use the vehicle attack mission during combat if the <see cref="Vehicle"/> has mounted guns (only works on driver).
		/// </summary>
		UseVehicleAttackIfVehicleHasMountedGuns,
		/// <summary>
		/// Always equip best weapon in combat.
		/// </summary>
		AlwaysEquipBestWeapon,
		/// <summary>
		/// Ignores in water at depth visibility check.
		/// </summary>
		CanSeeUnderwaterPeds,
		/// <summary>
		/// Will prevent this <see cref="Ped"/> from aiming at any AI targets that are in helicopters.
		/// </summary>
		DisableAimAtAITargetsInHelis,
		/// <summary>
		/// Disables <see cref="Ped"/>s seeking due to no clear line of sight.
		/// </summary>
		DisableSeekDueToLineOfSight,
		/// <summary>
		/// To be used when releasing missions <see cref="Ped"/>s if we don't want them fleeing from combat
		/// (mission <see cref="Ped"/>s already prevent flee).
		/// </summary>
		DisableFleeFromCombat,
		/// <summary>
		/// Disables target changes during vehicle pursuit.
		/// </summary>
		DisableTargetChangesDuringVehiclePursuit,
		/// <summary>
		/// <see cref="Ped"/> may throw a smoke grenade at player loitering in combat.
		/// </summary>
		CanThrowSmokeGrenade,
		NonMissionPedsFleeFromThisPedUnlessArmed,
		/// <summary>
		/// Will clear a set defensive area if that area cannot be reached.
		/// </summary>
		ClearAreaSetDefensiveIfDefensiveCannotBeReached,
		FleesFromInvincibleOpponents,
		/// <summary>
		/// Disable block from pursue during vehicle chases.
		/// </summary>
		DisableBlockFromPursueDuringVehicleChase,
		/// <summary>
		/// Disable spin out during vehicle chases.
		/// </summary>
		DisableSpinOutDuringVehicleChase,
		/// <summary>
		/// Disable cruise in front during block during vehicle chases.
		/// </summary>
		DisableCruiseInFrontDuringBlockDuringVehicleChase,
		/// <summary>
		/// Makes it more likely that the <see cref="Ped"/> will continue targeting a target with blocked los for a few seconds.
		/// </summary>
		CanIgnoreBlockedLosWeighting,
		/// <summary>
		/// Disables the react to buddy shot behaviour.
		/// </summary>
		DisableReactToBuddyShot,
		/// <summary>
		/// Prefer pathing using navmesh over road nodes.
		/// </summary>
		PreferNavmeshDuringVehicleChase,
		/// <summary>
		/// Ignore road edges when avoiding.
		/// </summary>
		AllowedToAvoidOffroadDuringVehicleChase,
		/// <summary>
		/// Permits <see cref="Ped"/> to charge a target outside the assigned defensive area.
		/// </summary>
		PermitChargeBeyondDefensiveArea,
		/// <summary>
		/// This <see cref="Ped"/> will switch to an RPG if target is in a <see cref="Vehicle"/>,
		/// otherwise will use alternate weapon.
		/// </summary>
		UseRocketsAgainstVehiclesOnly,
		/// <summary>
		/// Disables <see cref="Ped"/>s moving to a tactical point without clear los.
		/// </summary>
		DisableTacticalPointsWithoutClearLos,
		/// <summary>
		/// Disables pull alongside during vehicle chase.
		/// </summary>
		DisablePullAlongsideDuringVehicleChase,
		SetDisableShoutTargetPositionOnCombatStart = 76,
		DisableRespondedToThreatBroadcast,
		/// <summary>
		/// If set on a <see cref="Ped"/>, they will not flee when all random <see cref="Ped"/>s flee is
		/// set to <see langword="true"/>
		/// (they are still able to flee due to other reasons).
		/// </summary>
		DisableAllRandomsFlee,
		/// <summary>
		/// This <see cref="Ped"/> will send out a script DeadPedSeenEvent when they see a dead <see cref="Ped"/>.
		/// </summary>
		WillGenerateDeadPedSeenScriptEvents,
		/// <summary>
		/// This will use the receiving <see cref="Ped"/>s sense range rather than the range supplied to the communicate event.
		/// </summary>
		UseMaxSenseRangeWhenReceivingEvents,
		/// <summary>
		/// When aiming from a <see cref="Vehicle"/> the <see cref="Ped"/> will only aim at targetson his side
		/// of the <see cref="Vehicle"/>.
		/// </summary>
		RestrictInVehicleAimingToCurrentSide,
		/// <summary>
		/// LOS to the target is blocked we return to our default position and direction until we have LOS (no aiming)
		/// </summary>
		UseDefaultBlockedLosPositionAndDirection,
		RequiresLosToAim,
		/// <summary>
		/// <see cref="Ped"/>s flying aircraft will prefer to target other aircraft over entities on the ground.
		/// </summary>
		PreferAirCombatWhenInAircraft = 85,
		/// <summary>
		/// Allow <see cref="Ped"/>s flying aircraft to use dog fighting behaviours.
		/// </summary>
		AllowDogFighting,
		/// <summary>
		/// This will make the weight of targets who aircraft vehicles be reduced greatly compared to targets
		/// on foot or in ground based vehicles.
		/// </summary>
		PreferNonAircraftTargets,
		/// <summary>
		/// When <see cref="Ped"/>s are tasked to go to combat, they keep searching for a known target for a while
		/// before forcing an unknown one.
		/// </summary>
		PreferKnownTargetsWhenCombatClosestTarget,
		/// <summary>
		/// Only allow mounted weapons to fire if within the correct attack angle (default 25-degree cone).
		/// On a flag in order to keep exiting behaviour and only fix in specific cases.
		/// </summary>
		ForceCheckAttackAngleForMountedGuns,
		/// <summary>
		/// Blocks the firing state for passenger-controlled mounted weapons.
		/// Existing flags <see cref="UseVehicleAttack"/> and <see cref="UseVehicleAttackIfVehicleHasMountedGuns"/>
		/// only work for drivers.
		/// </summary>
		BlockFireForVehiclePassengerMountedGuns,
	}
}
