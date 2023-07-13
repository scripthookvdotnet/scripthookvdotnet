//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// An enumeration of known combat float attributes for <see cref="Ped"/>,
	/// </summary>
	/// <remarks>
	/// Almost all the initial values can be set for configs that this enum lists with <c>CCombatInfo</c> configs,
	/// which are instantiated from the <c>combatbehaviour.meta</c> file.
	/// </remarks>
	public enum CombatFloatAttributes
	{
		/// <summary>
		/// Chance to blind fire from cover, range is 0.0-1.0 (default is 0.05 for civilians, law doesn't blind fire).
		/// </summary>
		BlindFireChance,
		/// <summary>
		/// How long each burst from cover should last (default is 2.0).
		/// </summary>
		BurstDurationInCover,
		/// <summary>
		/// The maximum distance the <see cref="Ped"/> will try to shoot from (will override weapon range if set to
		/// anything > 0.0, default is -1.0)
		/// </summary>
		MaxShootingDistance,
		/// <summary>
		/// How long to wait, in cover, between firing bursts (&lt; 0.0 will disable firing, unless cover fire is
		/// requested, default is 1.25).
		/// </summary>
		TimeBetweenBurstsInCover,
		/// <summary>
		/// How long to wait before attempting to peek again (default is 10.0).
		/// </summary>
		TimeBetweenPeeks,
		/// <summary>
		/// A chance to strafe to cover, range is 0.0-1.0 (0.0 will force them to run, 1.0 will force strafe and
		/// shoot, default is 1.0).
		/// </summary>
		StrafeWhenMovingChance,
		/// <summary>
		/// A chance to shoot at exactly where the <see cref="Ped"/> points at.
		/// This attribute is for the float value that <see cref="Ped.Accuracy"/> actually reads/writes.
		/// </summary>
		WeaponAccuracy,
		/// <summary>
		/// How well an opponent can melee fight, range is 0.0-1.0.
		/// </summary>
		/// <remarks>
		/// Cannot be set in <c>combatbehaviours.meta</c>.
		/// </remarks>
		FightProficiency,
		/// <summary>
		/// The possibility of a <see cref="Ped"/> walking while strafing rather than jog/run, range is 0.0-1.0
		/// (default is 0.0).
		/// </summary>
		WalkWhenStrafingChance,
		/// <summary>
		/// The speed modifier when driving a heli in combat.
		/// </summary>
		HeliSpeedModifier,
		/// <summary>
		/// The range of the <see cref="Ped"/>'s senses (sight, identification, hearing) when in a heli.
		/// </summary>
		HeliSensesRange,
		/// <summary>
		/// The distance the game will use for cover based behaviour in attack windows Default is -1.0 (disabled),
		/// range is -1.0 to 150.0.
		/// </summary>
		AttackWindowDistanceForCover,
		/// <summary>
		/// How long to stop combat an injured target if there is no other valid target, if target is player in
		/// singleplayer, this will happen indefinitely unless explicitly disabled by setting to 0.0,
		/// default = 10.0 range = 0-50.
		/// </summary>
		TimeToInvalidateInjuredTarget,
		/// <summary>
		/// Min distance the <see cref="Ped"/> will use if <see cref="CombatAttributes.MaintainMinDistanceToTarget"/>
		/// is set, default 5.0.
		/// </summary>
		/// <remarks>
		/// There was the note "(currently only for cover search + usage)" in a official scripting header, but it is
		/// unknown when the word "currently" in the note specifies.
		/// </remarks>
		MinimumDistanceToTarget,
		/// <summary>
		/// The range at which the <see cref="Ped"/> will detect the bullet impact event.
		/// </summary>
		BulletImpactDetectionRange,
		/// <summary>
		/// The threshold at which the <see cref="Ped"/> will perform an aim turn.
		/// </summary>
		AimTurnThreshold,
		OptimalCoverDistance,
		/// <summary>
		/// The speed modifier when driving an automobile in combat.
		/// </summary>
		AutomobileSpeedModifier,
		SpeedToFleeInVehicle,
		/// <summary>
		/// How long to wait before charging a close target hiding in cover.
		/// </summary>
		TriggerChargeTimeFar,
		/// <summary>
		/// How long to wait before charging a distant target hiding in cover.
		/// </summary>
		TriggerChargeTimeNear,
		/// <summary>
		/// Max distance <see cref="Ped"/>s can hear an event from, even if the sound is louder.
		/// </summary>
		MaxDistanceToHearEvents,
		/// <summary>
		/// Max distance <see cref="Ped"/>s can hear an event from, even if the sound is louder if the <see cref="Ped"/>
		/// is using LOS to hear events (<see cref="PedConfigFlags.CheckLoSForSoundEvents"/>).
		/// </summary>
		MaxDistanceToHearEventsUsingLOS,
		/// <summary>
		/// Angle between the rocket and target where lock-on will stop, range is 0.0-1.0, (default is 0.2), the bigger
		/// the number the easier to break lock.
		/// </summary>
		HomingRocketBreakLockAngle,
		/// <summary>
		/// Angle between the rocket and target where lock-on will stop, when rocket is within
		/// <see cref="HomingRocketBreakLockCloseDistance"/>, range is 0.0-1.0, (default is 0.6), the bigger the number
		/// the easier to break lock.
		/// </summary>
		HomingRocketBreakLockAngleClose,
		/// <summary>
		/// Distance at which the game check <see cref="HomingRocketBreakLockAngleClose"/> rather than
		/// <see cref="HomingRocketBreakLockAngle"/>.
		/// </summary>
		HomingRocketBreakLockCloseDistance,
		/// <summary>
		///  Alters homing characteristics defined for the weapon (1.0 is default, &lt;1.0 slow turn rates, &gt;1.0
		///  speed them up.
		/// </summary>
		HomingRocketTurnRateModifier,
		/// <summary>
		/// Sets the time delay between aggressive moves during vehicle chases. -1.0 means use random values,
		/// 0.0 means never.
		/// </summary>
		TimeBetweenAggressiveMovesDuringVehicleChase,
		/// <summary>
		/// Max firing range for a <see cref="Ped"/> in vehicle turret seat.
		/// </summary>
		MaxVehicleTurretFiringRange,
		/// <summary>
		/// Multiplies the weapon damage dealt by the <see cref="Ped"/>, range is 0.0-10.0 (default is 1.0).
		/// </summary>
		WeaponDamageModifier,
		UnarmedDamageModifier,
	}
}
