namespace GTA
{
	/// <summary>
	/// An enumeration of known reset flags for <see cref="Ped"/>, which will be required to set or get every frame you need to set or get.
	/// </summary>
	/// <remarks>
	/// You can check if names of this enum are included in the exe by searching the dumped exe for hashed values of names like <c>CPED_RESET_FLAG_[enum name]</c> without case conversion
	/// (for example, search the dumped exe for 0x49F290D0, which is the hashed value of <c>CPED_RESET_FLAG_DisablePlayerJumping</c>).
	/// </remarks>
	public enum PedResetFlags
	{
		/// <summary>
		/// Disable jumping (exclusive from climbing).
		/// </summary>
		DisablePlayerJumping = 46,
		/// <summary>
		/// Disable climbing / vaulting.
		/// </summary>
		/// <remarks>
		/// Does not disable auto-vault, but you can disable it with <see cref="DisablePlayerAutoVaulting"/>.
		/// </remarks>
		DisablePlayerVaulting = 47,
		/// <summary>
		/// Don't freeze the <see cref="Ped"/> for not having bounds loaded around it.
		/// </summary>
		AllowUpdateIfNoCollisionLoaded = 55,
		/// <summary>
		/// Suppresses AI generating fire events, so civilians won't be shocked or react, for use in a shooting range for example.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_RESET_FLAG_SupressGunfireEvents</c> in the exe, but this enum uses the corrected name.
		/// </remarks>
		SuppressGunfireEvents = 62,
		/// <summary>
		/// Currently just for mounts, but could be expanded to anything with stamina.
		/// </summary>
		InfiniteStamina = 63,
		/// <summary>
		/// Stops the <see cref="Ped"/> from reacting to damage events (such as shots / fires, etc).
		/// The <see cref="Ped"/> will still take damage while this flag is active.
		/// </summary>
		/// <remarks>
		/// Does not block explosion reactions.
		/// </remarks>
		BlockWeaponReactionsUnlessDead = 64,
		/// <summary>
		/// Forces player to fire even if they aren't pressing fire
		/// </summary>
		ForcePlayerFiring = 65,
		/// <summary>
		/// Forces an actor that is in cover to continue (or start if they haven't yet) peeking
		/// </summary>
		ForcePeekFromCover = 67,
		/// <summary>
		/// Forces an actor to strafe
		/// </summary>
		ForcePedToStrafe = 69,
		/// <summary>
		/// Enables kinematic physics mode on The <see cref="Ped"/>.
		/// This stops other physical objects from pushing The <see cref="Ped"/> around, and causes the <see cref="Ped"/>
		/// to push any physical objects out of its way when it moves into them.
		/// </summary>
		UseKinematicPhysics = 71,
		/// <summary>
		/// Clear the players lock on target next frame
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_RESET_FLAG_ClearLockonTarget</c> in the exe, but this enum uses the corrected name.
		/// </remarks>
		ClearLockOnTarget = 72,
		/// <summary>
		/// forces the <see cref="Ped"/> to the scripted camera heading instead of gameplay.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_RESET_FLAG_ForcePedToUseScripCamHeading</c> in the exe, but this enum uses the corrected name.
		/// </remarks>
		ForcePedToUseScriptCamHeading = 77,
		/// <summary>
		/// When doing LOS checks to other <see cref="Ped"/>s we won't use the cover vantage position as the "target" position.
		/// </summary>
		IgnoreTargetsCoverForLOS = 85,
		/// <summary>
		/// Force the crouch flag to return true while in cover.
		/// </summary>
		DisableCrouchWhileInCover = 88,
		/// <summary>
		/// Forces the <see cref="Ped"/> to apply forces to frags as if running on contact,
		/// to guarantee <see cref="Ped"/>s will smash through frag objects when playing custom anims.
		/// </summary>
		ForceRunningSpeedForFragSmashing = 91,
		/// <summary>
		/// Force the bullets gun range to increase to 250m.
		/// </summary>
		ExtraLongWeaponRange = 95,
		/// <summary>
		/// Forces the player to only use direct access when entering vehicles.
		/// </summary>
		ForcePlayerToEnterVehicleThroughDirectDoorOnly = 96,
		/// <summary>
		/// Disable the <see cref="Ped"/> getting cull from a vehicle from pretend occupants.
		/// </summary>
		TaskCullExtraFarAway = 97,
		/// <summary>
		/// If this flag is set on a <see cref="Ped"/>, it will not attempt to auto-vault.
		/// </summary>
		DisablePlayerAutoVaulting = 102,
		/// <summary>
		/// If this flag is set on a <see cref="Ped"/>, it will use the bullet shoot through code.
		/// </summary>
		UseBulletPenetration = 107,
		/// <summary>
		/// Force all attackers to target the head of the <see cref="Ped"/>.
		/// </summary>
		ForceAimAtHead = 108,
		/// <summary>
		/// Inform avoidance code that the <see cref="Ped"/> isn't going anywhere and should be steered around rather than waited for
		/// </summary>
		IsInStationaryScenario = 109,
		/// <summary>
		/// Any targeting LoS checks will fail if any materials with 'see through' materials found.
		/// </summary>
		DisableSeeThroughChecksWhenTargeting = 112,
		/// <summary>
		/// When set, the <see cref="Ped"/> is putting on a helmet. DONT SET THIS only query it.
		/// </summary>
		PuttingOnHelmet = 113,
		/// <summary>
		/// When set, the <see cref="Ped"/> will play panic animations if in a vehicle.
		/// </summary>
		PanicInVehicle = 120,
		/// <summary>
		/// Forces the <see cref="Ped"/> to the injured state after being stunned.
		/// </summary>
		ForceInjuryAfterStunned = 126,
		/// <summary>
		/// Prevent the <see cref="Ped"/> from shooting a weapon.
		/// </summary>
		BlockWeaponFire = 128,
		/// <summary>
		/// Set the <see cref="Ped"/> capsule radius based on skeleton.
		/// </summary>
		ExpandPedCapsuleFromSkeleton = 129,
		/// <summary>
		/// Toggle the weapon laser sight off for this frame.
		/// </summary>
		DisableWeaponLaserSight = 130,
		/// <summary>
		/// Temporarily suspend any melee actions this frame (does not include hit reactions). Use PCF_DisableMelee to turn it off completely.
		/// </summary>
		SuspendInitiatedMeleeActions = 149,
		/// <summary>
		/// Prevents the <see cref="Ped"/> from getting the in air event the next frame.
		/// </summary>
		SuppressInAirEvent = 150,
		/// <summary>
		/// If set, allows the <see cref="Ped"/> to have tasks incompatible with its current motion.
		/// </summary>
		AllowTasksIncompatibleWithMotion = 151,
		/// <summary>
		/// This will suppress any melee action that is considered lethal (RA_IS_LETHAL, defined in action_table.meta).
		/// </summary>
		SuppressLethalMeleeActions = 155,
		/// <summary>
		/// Don't auto run when this flag is set.
		/// </summary>
		NoAutoRunWhenFiring = 167,
		/// <summary>
		/// Don't let the <see cref="Ped"/> take navmesh edges into account when performing their low-level steering/avoidance code.
		/// </summary>
		DisableSteeringAroundNavMeshEdges = 172,
		/// <summary>
		/// Disable taking off the parachute pack
		/// </summary>
		DisableTakeOffParachutePack = 177,
		/// <summary>
		/// If the <see cref="Ped"/> has the INSULT special ability, and this flag is set, he will always use the combat taunt when the special is activated.
		/// </summary>
		ForceCombatTaunt = 179,
		/// <summary>
		/// The <see cref="Ped"/> will ignore combat taunts
		/// </summary>
		IgnoreCombatTaunts = 180,
		/// <summary>
		/// Will temporarily prevent any takedown from being performed on the <see cref="Ped"/>.
		/// </summary>
		PreventAllMeleeTakedowns = 187,
		/// <summary>
		/// Will temporarily prevent any failed takedown from being performed on the <see cref="Ped"/>.
		/// </summary>
		PreventFailedMeleeTakedowns = 188,
		/// <summary>
		/// Will temporarily force min avoidance on the <see cref="Ped"/>.
		/// Will brush other <see cref="Ped"/>s but avoid a bit.
		/// </summary>
		UseTighterAvoidanceSettings = 190,
		/// <summary>
		/// Disable drop downs for the <see cref="Ped"/>.
		/// </summary>
		DisableDropDowns = 195,
		/// <summary>
		/// Disable taking off the scuba gear.
		/// </summary>
		DisableTakeOffScubaGear = 197,
		/// <summary>
		/// Disable combat anims for the <see cref="Ped"/>.
		/// </summary>
		DisableActionMode = 200,
		/// <summary>
		/// Use the <see cref="Ped"/>'s head orientation for perception tests.
		/// </summary>
		UseHeadOrientationForPerception = 206,
		/// <summary>
		/// The player will no longer auto-ragdoll when colliding with something while jumping.
		/// </summary>
		DisableJumpRagdollOnCollision = 210,
		/// <summary>
		/// Disable parachuting for the <see cref="Ped"/>.
		/// </summary>
		DisableParachuting = 217,
		/// <summary>
		/// Keep the parachute pack on after a teleport.
		/// </summary>
		KeepParachutePackOnAfterTeleport = 222,
		/// <summary>
		/// Whether or not you want the player <see cref="Ped"/> to use the new front melee logic.
		/// </summary>
		DontRaiseFistsWhenLockedOn = 223,
		/// <summary>
		/// This will prefer all melee hit reactions to use body ik hit reactions if ragdoll is not selected
		/// </summary>
		PreferMeleeBodyIkHitReaction = 224,
		/// <summary>
		/// If set, disables friendly responses to gunshots/being aimed at.
		/// </summary>
		DisableFriendlyGunReactAudio = 227,
		/// <summary>
		/// Disables agitation triggers.
		/// </summary>
		DisableAgitationTriggers = 228,
		/// <summary>
		/// Disable NM reactions to fast moving water for the <see cref="Ped"/>.
		/// </summary>
		DisableNMForRiverRapids = 234,
		/// <summary>
		/// If set, the <see cref="Ped"/> will not go into the still in vehicle pose.
		/// </summary>
		PreventGoingIntoStillInVehicleState = 236,
		/// <summary>
		/// If set, the <see cref="Ped"/> will get in and out of vehicles faster (same as in multiplayer).
		/// </summary>
		UseFastEnterExitVehicleRates = 237,
		/// <summary>
		/// Disables agitation.
		/// </summary>
		DisableAgitation = 239,
		/// <summary>
		/// Disables talking.
		/// </summary>
		DisableTalk = 240,
		/// <summary>
		/// Uses more expensive slope/stairs detection.
		/// </summary>
		UseProbeSlopeStairsDetection = 247,
		/// <summary>
		/// Disables vehicle damage reactions.
		/// </summary>
		DisableVehicleDamageReactions = 248,
		/// <summary>
		/// Disables potential blast reactions.
		/// </summary>
		DisablePotentialBlastReactions = 249,
		/// <summary>
		/// When set along side open door ik, will only use the left hand.
		/// </summary>
		OnlyAllowLeftArmDoorIk = 250,
		/// <summary>
		/// When set along side open door ik, will only use the left hand.
		/// </summary>
		OnlyAllowRightArmDoorIk = 251,
		/// <summary>
		/// When set, the flash light on a Ai weapon will be turned off.
		/// </summary>
		DisableFlashLight = 253,
		/// <summary>
		/// When set, the AI <see cref="Ped"/> will enable their flash light.
		/// </summary>
		ForceEnableFlashLightForAI = 258,
		/// <summary>
		/// Disables combat audio.
		/// </summary>
		DisableCombatAudio = 262,
		/// <summary>
		/// Disables cover audio.
		/// </summary>
		DisableCoverAudio = 263,
		/// <summary>
		/// Player has to press and hold dive button to dive in water.
		/// </summary>
		EnablePressAndReleaseDives = 271,
		/// <summary>
		/// Only allows player to exit vehicle when button is released rather than pressed or held.
		/// </summary>
		OnlyExitVehicleOnButtonRelease = 272,
		/// <summary>
		/// Considered as a threat as part of player cover search even if they can't see the player.
		/// </summary>
		ConsiderAsPlayerCoverThreatWithoutLOS = 282,
		/// <summary>
		/// Disables the <see cref="Ped"/> from using custom ai cover entry animations.
		/// </summary>
		BlockCustomAIEntryAnims = 283,
		/// <summary>
		/// Ignore the vehicle entry collision tests for this <see cref="Ped"/>.
		/// </summary>
		IgnoreVehicleEntryCollisionTests = 284,
		/// <summary>
		/// If set, the <see cref="Ped"/> will not go into the shunt in vehicle pose.
		/// </summary>
		PreventGoingIntoShuntInVehicleState = 287,
		/// <summary>
		/// If set, turn on the voice driven mouth movement
		/// </summary>
		EnableVoiceDrivenMouthMovement = 302,
		/// <summary>
		/// To have <see cref="Ped"/>ds do better vehicle entries when in a group and interfered by other <see cref="Ped"/>s, use carefully though.
		/// </summary>
		UseTighterEnterVehicleSettings = 304,
		/// <summary>
		/// Set when the player is in the races to make the player more interesting to look at.
		/// </summary>
		InRaceMode = 305,
		/// <summary>
		/// Disable ambient (initial) melee moves.
		/// </summary>
		DisableAmbientMeleeMoves = 306,
		/// <summary>
		/// Allows the player to trigger the special ability while in a vehicle.
		/// </summary>
		AllowSpecialAbilityInVehicle = 308,
		/// <summary>
		/// Prevents the <see cref="Ped"/> from doing in vehicle actions like closing door, hotwiring, starting engine, putting on helmet etc.
		/// </summary>
		DisableInVehicleActions = 309,
		/// <summary>
		/// Forces the <see cref="Ped"/> to blend in steering wheel ik instantly rather than over time.
		/// </summary>
		ForceInstantSteeringWheelIkBlendIn = 310,
		/// <summary>
		/// Ignores the bonus score for selecting cover that the player can engage the enemy at.
		/// </summary>
		IgnoreThreatEngagePlayerCoverBonus = 311,
		/// <summary>
		/// Prevents the the <see cref="Ped"/> from closing the vehicle door of the car they're inside.
		/// </summary>
		DontCloseVehicleDoor = 313,
		/// <summary>
		/// Explosions can't be blocked by map collision when damaging the <see cref="Ped"/>.
		/// </summary>
		SkipExplosionOcclusion = 314,
		/// <summary>
		/// Set when the <see cref="Ped"/> has performed a melee strike and hit any non <see cref="Ped"/> material.
		/// </summary>
		MeleeStrikeAgainstNonPed = 316,
		/// <summary>
		/// We will not attempt to walk around doors when using arm IK.
		/// </summary>
		IgnoreNavigationForDoorArmIK = 317,
		/// <summary>
		/// Disable aiming while parachuting.
		/// </summary>
		DisableAimingWhileParachuting = 318,
		/// <summary>
		/// Disable hit reaction due to colliding with a <see cref="Ped"/>.
		/// </summary>
		DisablePedCollisionWithPedEvent = 319,
		/// <summary>
		/// Will ignore the vehicle speed threshold and close the door anyway.
		/// </summary>
		IgnoreVelocityWhenClosingVehicleDoor = 320,
		/// <summary>
		/// Skip idle intro.
		/// </summary>
		SkipOnFootIdleIntro = 321,
		/// <summary>
		/// Don't walk round objects that we collide with whilst moving.
		/// </summary>
		DontWalkRoundObjects = 322,
		/// <summary>
		/// Disable the <see cref="Ped"/> entered my vehicle events.
		/// </summary>
		DisablePedEnteredMyVehicleEvents = 323,
		/// <summary>
		/// Will allow the <see cref="Ped"/> variations to be rendered in vehicles, even if marked otherwise.
		/// </summary>
		DisableInVehiclePedVariationBlocking = 326,
		/// <summary>
		/// When on a mission this reset flag will slightly reduce the amount of time the player loses control of their vehicle when hit by an AI <see cref="Ped"/>.
		/// </summary>
		ReduceEffectOfVehicleRamControlLoss = 327,
		/// <summary>
		/// Another flag to disable friendly attack from the player. Set on the opponent you would like it to be disabled on.
		/// </summary>
		DisablePlayerMeleeFriendlyAttacks = 328,
		/// <summary>
		/// Set when the melee target has been deemed unreachable (AI only).
		/// </summary>
		IsMeleeTargetUnreachable = 330,
		/// <summary>
		/// Disable automatically forcing player to exit a vehicle in a network game when blowing up vehicle.
		/// </summary>
		DisableAutoForceOutWhenBlowingUpCar = 331,
		/// <summary>
		/// Disable ambient dust off animations.
		/// </summary>
		DisableDustOffAnims = 334,
		/// <summary>
		/// The <see cref="Ped"/> will refrain from ever performing a melee hit reaction.
		/// </summary>
		DisableMeleeHitReactions = 335,
		/// <summary>
		/// This overrides PV_FLAG_NOT_IN_CAR (which is used in 3rd argument of <c>GIVE_PED_HELMET</c>) set on any head prop and stops it from being removed when getting into the vehicle.
		/// </summary>
		AllowHeadPropInVehicle = 337,
		DontQuitMotionAiming = 339,
		/// <summary>
		/// Reset version of PCF_OpenDoorArmIK, which sets if the <see cref="Ped"/> should enable open door arm IK.
		/// </summary>
		OpenDoorArmIK = 342,
		/// <summary>
		/// Force use of tighter turn settings in locomotion task.
		/// </summary>
		UseTighterTurnSettingsForScript = 343,
		/// <summary>
		/// If set, turn off the voice driven mouth movement (overrides EnableVoiceDrivenMouthMovement).
		/// </summary>
		DisableVoiceDrivenMouthMovement = 346,
		/// <summary>
		/// If set, steer into skids while driving.
		/// </summary>
		SteerIntoSkids = 347,
		/// <summary>
		/// When set, code will ignore the logic that requires the <see cref="Ped"/> to be in CTaskHumanLocomotion::State_Moving.
		/// </summary>
		AllowOpenDoorIkBeforeFullMovement = 348,
		/// <summary>
		/// When set, code will bypass rel settings and allow a homing lock on to the <see cref="Ped"/> when they are in a vehicle.
		/// </summary>
		AllowHomingMissileLockOnInVehicle = 349,
		AllowCloneForcePostCameraAIUpdate = 350,
		/// <summary>
		/// Force the high heels DOF to be 0.
		/// </summary>
		DisableHighHeels = 351,
		/// <summary>
		/// Player does not get tired when sprinting.
		/// </summary>
		DontUseSprintEnergy = 353,
		/// <summary>
		/// Don't be damaged by touching dangerous material (e.g. electric generator).
		/// </summary>
		DisableMaterialCollisionDamage = 355,
		/// <summary>
		/// Don't target friendly players in MP.
		/// </summary>
		DisableMPFriendlyLockon = 356,
		/// <summary>
		/// Don't melee kill friendly players in MP.
		/// </summary>
		DisableMPFriendlyLethalMeleeActions = 357,
		/// <summary>
		/// If our leader stops, try and seek cover if we can.
		/// </summary>
		IfLeaderStopsSeekCover = 358,
		/// <summary>
		/// Use Interior capsule settings.
		/// </summary>
		UseInteriorCapsuleSettings = 362,
		/// <summary>
		/// The <see cref="Ped"/> is closing a vehicle door.
		/// </summary>
		IsClosingVehicleDoor = 363,
		/// <summary>
		/// Disable stuck wall hit animation for the <see cref="Ped"/> this frame.
		/// </summary>
		DisableWallHitAnimation = 371,
		/// <summary>
		/// When set, the <see cref="Ped"/> will play panic animations if in a vehicle.
		/// </summary>
		PlayAgitatedAnimsInVehicle = 372,
		/// <summary>
		/// The <see cref="Ped"/> is shuffling seat.
		/// </summary>
		IsSeatShuffling = 373,
		/// <summary>
		/// Allows ped in any seat to control the radio (in MP only).
		/// </summary>
		AllowControlRadioInAnySeatInMP = 376,
		/// <summary>
		/// Blocks the <see cref="Ped"/> from manually transforming spy car to/from car/sub modes.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_RESET_FLAG_DisableSpycarTransformation</c> in the exe, but this enum uses the corrected name.
		/// </remarks>
		DisableSpyCarTransformation = 377,
		/// <summary>
		/// Blocks the <see cref="Ped"/> from head bobbing to radio music in vehicles.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_RESET_FLAG_BlockHeadbobbingToRadio</c> in the exe, but this enum uses the corrected name.
		/// </remarks>
		BlockHeadBobbingToRadio = 379,
		/// <summary>
		/// When putting a <see cref="Ped"/> directly into cover, the <see cref="Ped"/> will blend in the new cover anims slowly to prevent a pose pop.
		/// </summary>
		ForceExtraLongBlendInForPedSkipIdleCoverTransition = 381,
		/// <summary>
		/// Don't ever try to lock on to the <see cref="Ped"/> with cinematic aim.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_RESET_FLAG_DisableAssistedAimLockon</c> in the exe, but this enum uses the corrected name.
		/// </remarks>
		DisableAssistedAimLockOn = 390,
		/// <summary>
		/// Any <see cref="Ped"/>s with this flag set on won't register damage from collisions against other <see cref="Ped"/>s.
		/// </summary>
		NoCollisionDamageFromOtherPeds = 394,
		/// <summary>
		/// For thing that inherit from boats only.
		/// </summary>
		DontSuppressUseNavMeshToNavigateToVehicleDoorWhenVehicleInWater = 398,
		/// <summary>
		/// If true it avoids playing the settle anim when aiming.
		/// </summary>
		InstantBlendToAimNoSettle = 401,
		/// <summary>
		/// For first person mode, when the player enters low cover, will orientate camera to face left or right rather than into cover.
		/// </summary>
		ForceScriptedCameraLowCoverAngleWhenEnteringCover = 406,
		DisableMeleeWeaponSelection = 417,
		/// <summary>
		/// Allows <see cref="Ped"/>s following waypoint recordings to slow down more for corners. (Achieves same effect as the EWAYPOINT_SUPPRESS_EXACTSTOP flag, which is passed into TASK_FOLLOW_WAYPOINT_RECORDING).
		/// </summary>
		WaypointPlaybackSlowMoreForCorners = 418,
		/// <summary>
		/// <see cref="Ped"/> will use bullet penetration code when glass material is hit.
		/// </summary>
		UseBulletPenetrationForGlass = 420,
		/// <summary>
		/// If set on a <see cref="Ped"/> then they are allowed to be pinned by bullets from friendly <see cref="Ped"/>s.
		/// </summary>
		CanBePinnedByFriendlyBullets = 423,
		/// <summary>
		/// Blocks road blocks with spike strips from spawning.
		/// </summary>
		DisableSpikeStripRoadBlocks = 425,
		/// <summary>
		/// <see cref="Ped"/>s marked with this flag will only be able to be hit by the player if the player explicitly presses the melee button.
		/// </summary>
		IsLowerPriorityMeleeTarget = 428,
		/// <summary>
		/// Disable timeslicing of event scanning this frame.
		/// </summary>
		ForceScanForEventsThisFrame = 429,
		/// <summary>
		/// Forces <see cref="Ped"/> to auto-equip a helmet when entering an aircraft. Overrides PCF_DisableAutoEquipHelmetsInAicraft which is set in the interaction menu.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_RESET_FLAG_ForceAutoEquipHelmetsInAicraft</c> in the exe, but this enum uses the corrected name.
		/// </remarks>
		ForceAutoEquipHelmetsInAircraft = 432,
		/// <summary>
		/// Flag used by replay editor to disable recording specified remote players.
		/// </summary>
		BlockRemotePlayerRecording = 433,
		/// <summary>
		/// allow FPS vehicle anims even if FPS camera isn't dominant.
		/// </summary>
		UseFirstPersonVehicleAnimsIfFPSCamNotDominant = 435,
		/// <summary>
		/// allow FPS vehicle anims even if FPS camera isn't dominant.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_RESET_FLAG_ForceIntoStandPoseOnJetski</c> in the exe, but this enum uses the capital-corrected name.
		/// </remarks>
		ForceIntoStandPoseOnJetSki = 436,
		/// <summary>
		/// This will suppress all takedown melee actions (RA_IS_TAKEDOWN or RA_IS_STEALTH_KILL, defined in action_table.meta)
		/// </summary>
		SuppressTakedownMeleeActions = 438,
		/// <summary>
		/// Inverts lookaround controls (right stick / mouse) for this player.
		/// </summary>
		InvertLookAroundControls = 439,
		/// <summary>
		/// Allows attacking <see cref="Ped"/>s to engage another entity without waiting for its turn (if there's multiple attackers).
		/// </summary>
		IgnoreCombatManager = 440,
		/// <summary>
		/// Check if there is an active camera blending and use the blended camera frame when compute the FPS camera relative matrix.
		/// </summary>
		UseBlendedCamerasOnUpdateFpsCameraRelativeMatrix = 441,
		/// <summary>
		/// Forces the <see cref="Ped"/> to perform a dodge and a counter move if it's attacked.
		/// </summary>
		ForceMeleeCounter = 442,
		/// <summary>
		/// Suppress navmesh navigation in TaskEnterVehicle. Will use gotopoint or bail if cant use that.
		/// </summary>
		SuppressNavmeshForEnterVehicleTask = 444,
		/// <summary>
		/// Prevents the <see cref="Ped"/> from jumping out of the vehicle in shallow water if the bike is submerged.
		/// </summary>
		DisableShallowWaterBikeJumpOutThisFrame = 445,
		/// <summary>
		/// Prevents the player from performing a combat roll.
		/// </summary>
		DisablePlayerCombatRoll = 446,
		/// <summary>
		/// Will ignore safe position check on detaching the <see cref="Ped"/>.
		/// </summary>
		IgnoreDetachSafePositionCheck = 447,
		/// <summary>
		/// Prevents the more forgiving MP ladder detection settings from being used, and forces SP settings.
		/// </summary>
		DisableEasyLadderConditions = 448,
		/// <summary>
		/// Makes local player ignore certain scenario spawn restrictions on scenarios that respect this flag.
		/// </summary>
		PlayerIgnoresScenarioSpawnRestrictions = 449,
		/// <summary>
		/// Indicates player is using Drone from Battle DLC.
		/// </summary>
		UsingDrone = 450,
		/// <summary>
		/// Will use scripted firing position on the clones of the <see cref="Ped"/> on other machines.
		/// </summary>
		UseScriptedWeaponFirePosition = 452,
		/// <summary>
		/// Use extended logic for determining damage instigator for ragdoll collisions.
		/// </summary>
		UseExtendedRagdollCollisionCalculator = 454,
		/// <summary>
		/// Prevent the player locking on to friendly players.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_RESET_FLAG_PreventLockonToFriendlyPlayers</c> in the exe, but this enum uses the capital-corrected name.
		/// </remarks>
		PreventLockOnToFriendlyPlayers = 455,
		/// <summary>
		/// Modifies AF_ABORT_ON_PED_MOVEMENT to only trigger an abort when movement would be caused by player input.
		/// </summary>
		OnlyAbortScriptedAnimOnMovementByInput = 456,
		/// <summary>
		/// Prevents stealth take downs from being preformed on a <see cref="Ped"/>.
		/// </summary>
		PreventAllStealthKills = 457,
		/// <summary>
		/// Prevents <see cref="Ped"/>s from entering a fall task if affected by explosion damage.
		/// </summary>
		BlockFallTaskFromExplosionDamage = 458,
		/// <summary>
		/// Mimics the behaviour of boss <see cref="Ped"/>s by holding the button for entering the rear seats.
		/// </summary>
		AllowPedRearEntry = 459,
	}
}
