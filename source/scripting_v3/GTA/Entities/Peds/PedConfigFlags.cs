namespace GTA
{
	/// <summary>
	/// An enumeration of known config flags for <see cref="Ped"/>.
	/// </summary>
	/// <remarks>
	/// You can check if names of this enum are included in the exe by searching the dumped exe for hashed values of names like <c>CPED_CONFIG_FLAG_[enum name]</c> without case conversion
	/// (for example, search the dumped exe for 0x583B5E2D, which is the hashed value of <c>CPED_CONFIG_FLAG_AllowMedicsToReviveMe</c>).
	/// </remarks>
	public enum PedConfigFlags
	{
		CreatedByFactory = 0,
		NoCriticalHits = 2,
		DrownsInWater = 3,
		DrownsInSinkingVehicle = 4,
		DiesInstantlyWhenSwimming = 5,
		UpperBodyDamageAnimsOnly = 7,
		NeverLeavesGroup = 13,
		DoesntDropWeaponsWhenDead = 14,
		KeepTasksAfterCleanUp = 16,
		BlockNonTemporaryEvents = 17,
		WaitingForScriptBrainToLoad = 19,
		/// <summary>
		/// If the <see cref="Ped"/> dies medics will be dispatched, false by default for mission <see cref="Ped"/>s, the <see cref="Ped"/> wont be attended.
		/// </summary>
		/// <remarks>
		/// Despite the "correct" enum name whose hash 0x583B5E2D (for <c>CPED_CONFIG_FLAG_AllowMedicsToReviveMe</c>) is present in the exe, medics cannot revive <see cref="Ped"/>s.
		/// </remarks>
		AllowMedicsToReviveMe = 20,
		MoneyHasBeenGivenByScript = 21,
		NotAllowedToCrouch = 22,
		IgnoreSeenMelee = 24,
		/// <summary>
		/// Script can stop <see cref="Ped"/>s automatically getting out of car when it's upside down or undrivable, defaults to true.
		/// </summary>
		GetOutUndriveableVehicle = 29,
		DontStoreAsPersistent = 31,
		/// <summary>
		/// The <see cref="Ped"/> will fly through the vehicle windscreen upon a forward impact at high velocity.
		/// </summary>
		WillFlyThroughWindscreen = 32,
		DieWhenRagdoll = 33,
		/// <summary>
		/// The <see cref="Ped"/> has a helmet (the PedHelmetComponent has put the helmet on the <see cref="Ped"/> via "put on" animations).
		/// </summary>
		HasHelmet = 34,
		UseHelmet = 35,
		/// <summary>
		/// Disable the <see cref="Ped"/> taking off his helmet automatically.
		/// </summary>
		DontTakeOffHelmet = 36,
		HideInCutscene = 37,
		DisableEvasiveDives = 39,
		DontInfluenceWantedLevel = 42,
		/// <remarks>
		/// The original name is <c>CPED_CONFIG_FLAG_DisablePlayerLockon</c> in the exe, but this enum uses the corrected name.
		/// </remarks>
		DisablePlayerLockOn = 43,
		/// <remarks>
		/// The original name is <c>CPED_CONFIG_FLAG_DisableLockonToRandomPeds</c> in the exe, but this enum uses the corrected name.
		/// </remarks>
		DisableLockOnToRandomPeds = 44,
		/// <remarks>
		/// The original name is <c>CPED_CONFIG_FLAG_AllowLockonToFriendlyPlayers</c> in the exe, but this enum uses the corrected name.
		/// </remarks>
		AllowLockOnToFriendlyPlayers = 45,
		PedBeingDeleted = 47,
		BlockWeaponSwitching = 48,
		ConstrainToNavMesh = 56,
		IsFiring = 58,
		WasFiring = 59,
		IsStanding = 60,
		WasStanding = 61,
		InVehicle = 62,
		OnMount = 63,
		AttachedToVehicle = 64,
		IsSwimming = 65,
		WasSwimming = 66,
		IsSkiing = 67,
		IsSitting = 68,
		KilledByStealth = 69,
		KilledByTakedown = 70,
		/// <remarks>
		/// The original name is <c>CPED_CONFIG_FLAG_Knockedout</c> in the exe, but this enum uses the corrected name.
		/// </remarks>,
		KnockedOut = 71,
		ClearRadarBlipOnDeath = 72,
		UsingCoverPoint = 75,
		IsInTheAir = 76,
		KnockedUpIntoAir = 77,
		IsAimingGun = 78,
		HasJustLeftCar = 79,
		CurrLeftFootCollNM = 81,
		PrevLeftFootCollNM = 82,
		CurrRightFootCollNM = 83,
		PrevRightFootCollNM = 84,
		DontActivateRagdollFromAnyPedImpact = 89,
		ForcePedLoadCover = 93,
		VaultFromCover = 97,
		UsingCrouchedPedCapsule = 99,
		ForcedAim = 101,
		OpenDoorArmIK = 104,
		ForceReload = 105,
		DontActivateRagdollFromVehicleImpact = 106,
		DontActivateRagdollFromBulletImpact = 107,
		DontActivateRagdollFromExplosions = 108,
		DontActivateRagdollFromFire = 109,
		DontActivateRagdollFromElectrocution = 110,
		IsBeingDraggedToSafety = 111,
		/// <summary>
		/// Will keep the <see cref="Ped"/>s weapon holstered until they shoot or change weapons.
		/// </summary>
		KeepWeaponHolsteredUnlessFired = 113,
		/// <summary>
		/// If set, a <see cref="Ped"/> will escape a burning vehicle they are inside, defaults to true.
		/// </summary>
		GetOutBurningVehicle = 116,
		BumpedByPlayer = 117,
		/// <summary>
		/// If set, a <see cref="Ped"/> will escape a burning vehicle they are inside, defaults to true.
		/// </summary>
		RunFromFiresAndExplosions = 118,
		/// <summary>
		/// If set, the <see cref="Ped"/> will be given the same boost a player gets in the targeting scoring system.
		/// </summary>
		TreatAsPlayerDuringTargeting = 119,
		IsHandCuffed = 120,
		IsAnkleCuffed = 121,
		/// <summary>
		/// Disable melee for a <see cref="Ped"/> (only supported for player right now).
		/// </summary>
		DisableMelee = 122,
		/// <summary>
		/// Disable unarmed driveby taunts for a <see cref="Ped"/>
		/// </summary>
		DisableUnarmedDrivebys = 123,
		/// <summary>
		/// MP only - if the <see cref="Ped"/> is tazed or rubber bulleted in a vehicle and a <see cref="Ped"/> jacks them, the jacker will only pull the <see cref="Ped"/> out.
		/// </summary>
		JustGetsPulledOutWhenElectrocuted = 124,
		/// <summary>
		/// If set, the <see cref="Ped"/> won't hotwire a law enforcement vehicle.
		/// </summary>
		WillNotHotwireLawEnforcementVehicle = 126,
		/// <summary>
		/// If set, the <see cref="Ped"/> will play commandeering anims rather than jacking if available.
		/// </summary>
		WillCommandeerRatherThanJack = 127,
		/// <summary>
		/// If set, the <see cref="Ped"/> can be agitated.
		/// </summary>
		CanBeAgitated = 128,
		ForcePedToFaceLeftInCover = 129,
		/// <summary>
		/// If set <see cref="Ped"/> will turn to face right in cover.
		/// </summary>
		ForcePedToFaceRightInCover = 130,
		BlockPedFromTurningInCover = 131,
		/// <summary>
		/// Ped keeps their relationship group when the mission is cleaned up or they are marked as no longer needed.
		/// </summary>
		KeepRelationshipGroupAfterCleanUp = 132,
		/// <summary>
		/// Ped will loop the try locked door anim when they get to the door in order for them to automatically be dragged along.
		/// </summary>
		ForcePedToBeDragged = 133,
		/// <summary>
		/// Ped doesn't react when being jacked.
		/// </summary>
		PreventPedFromReactingToBeingJacked = 134,
		IsScuba = 135,
		WillArrestRatherThanJack = 136,
		/// <summary>
		/// We must be further away before <see cref="Ped"/> population remove the <see cref="Ped"/> when it is dead.
		/// </summary>
		RemoveDeadExtraFarAway = 137,
		RidingTrain = 138,
		/// <summary>
		/// If set, the <see cref="Ped"/> arrest task completed successfully.
		/// </summary>
		ArrestResult = 139,
		/// <summary>
		/// True allows the <see cref="Ped"/> to attack <see cref="Ped"/>s they are friendly with.
		/// </summary>
		CanAttackFriendly = 140,
		/// <summary>
		/// MP only, if set the <see cref="Ped"/> will be allowed to jack any player <see cref="Ped"/>s, regardless of relationship.
		/// </summary>
		WillJackAnyPlayer = 141,
		/// <summary>
		/// MP only, True if this player will jack hated players rather than try to steal a car (cops arresting crims).
		/// </summary>
		WillJackWantedPlayersRatherThanStealCar = 144,
		/// <summary>
		/// If this flag is set on a <see cref="Ped"/> it will not scan for or climb ladders.
		/// </summary>
		DisableLadderClimbing = 146,
		StairsDetected = 147,
		SlopeDetected = 148,
		HelmetHasBeenShot = 149,
		/// <summary>
		/// If set the <see cref="Ped"/> should cower instead of fleeing.
		/// </summary>
		CowerInsteadOfFlee = 150,
		/// <summary>
		/// If set the <see cref="Ped"/> will be allowed to ragdoll when the vehicle they are in gets turned upside down if the seat supports it.
		/// </summary>
		CanActivateRagdollWhenVehicleUpsideDown = 151,
		/// <summary>
		/// If set the <see cref="Ped"/> will respond to cries for help even if not friends with the injured <see cref="Ped"/>.
		/// </summary>
		AlwaysRespondToCriesForHelp = 152,
		/// <summary>
		/// If set the <see cref="Ped"/> will not create a blood pool when dead.
		/// </summary>
		DisableBloodPoolCreation = 153,
		/// <summary>
		/// If set, the <see cref="Ped"/> will be fixed if there is no collision around.
		/// </summary>
		ShouldFixIfNoCollision = 154,
		/// <summary>
		/// If set, the <see cref="Ped"/> can perform arrests on <see cref="Ped"/>s that can be arrested.
		/// </summary>
		CanPerformArrest = 155,
		/// <summary>
		/// If set, the <see cref="Ped"/> can uncuff <see cref="Ped"/>s that are handcuffed.
		/// </summary>
		CanPerformUncuff = 156,
		/// <summary>
		/// If set, the <see cref="Ped"/> can be arrested.
		/// </summary>
		CanBeArrested = 157,
		MoverConstrictedByOpposingCollisions = 158,
		/// <summary>
		/// When true, Prefer the front seat when getting in a car with buddies.
		/// </summary>
		PlayerPreferFrontSeatMP = 159,
		DontActivateRagdollFromImpactObject = 160,
		DontActivateRagdollFromMelee = 161,
		DontActivateRagdollFromWaterJet = 162,
		DontActivateRagdollFromDrowning = 163,
		DontActivateRagdollFromFalling = 164,
		DontActivateRagdollFromRubberBullet = 165,
		IsInjured = 166,
		/// <summary>
		/// When true, will follow the player around if in their group but wont enter vehicles.
		/// </summary>
		DontEnterVehiclesInPlayersGroup = 167,
		SwimmingTasksRunning = 168,
		/// <summary>
		/// Disable all melee taunts for this particular <see cref="Ped"/>.
		/// </summary>
		PreventAllMeleeTaunts = 169,
		/// <summary>
		/// Will force the <see cref="Ped"/> to use the direct entry point for any vehicle they try to enter, or warp in.
		/// </summary>
		ForceDirectEntry = 170,
		/// <summary>
		/// This <see cref="Ped"/> will always see approaching vehicles (even from behind).
		/// </summary>
		AlwaysSeeApproachingVehicles = 171,
		/// <summary>
		/// This <see cref="Ped"/> can dive away from approaching vehicles.
		/// </summary>
		CanDiveAwayFromApproachingVehicles = 172,
		/// <summary>
		/// Will allow player to interrupt a <see cref="Ped"/>s scripted entry/exit task as if they had triggered it themselves.
		/// </summary>
		AllowPlayerToInterruptVehicleEntryExit = 173,
		/// <summary>
		/// This <see cref="Ped"/> will only attack cops if the player is wanted.
		/// </summary>
		OnlyAttackLawIfPlayerIsWanted = 174,
		PlayerInContactWithKinematicPed = 175,
		PlayerInContactWithSomethingOtherThanKinematicPed = 176,
		/// <summary>
		/// If set the <see cref="Ped"/> will not get in as part of the jack.
		/// </summary>
		PedsJackingMeDontGetIn = 177,
		/// <summary>
		/// AI <see cref="Ped"/>s only, will not early out of anims, default behaviour is to exit as early as possible.
		/// </summary>
		PedIgnoresAnimInterruptEvents = 179,
		/// <summary>
		/// Any targeting LoS checks will fail if any materials with 'see through' materials found.
		/// </summary>
		IsInCustody = 180,
		/// <summary>
		/// Setting this on an armed or buddy <see cref="Ped"/> will make him more likely to perform an nm reaction when bumped by a player, friendly vehicle or ragdolling <see cref="Ped"/>.
		/// </summary>
		ForceStandardBumpReactionThresholds = 181,
		/// <summary>
		/// If set on a <see cref="Ped"/>, law <see cref="Ped"/>s will only attack if the local player is wanted.
		/// </summary>
		LawWillOnlyAttackIfPlayerIsWanted = 182,
		IsAgitated = 183,
		/// <summary>
		/// Prevents passenger from auto shuffling over to drivers seat if it becomes free.
		/// </summary>
		PreventAutoShuffleToDriversSeat = 184,
		/// <summary>
		/// When enabled, the <see cref="Ped"/> will continually set the kinematic mode reset flag when stationary.
		/// </summary>
		UseKinematicModeWhenStationary = 185,
		EnableWeaponBlocking = 186,
		HasHurtStarted = 187,
		/// <summary>
		/// Set to disable the combat hurt mode.
		/// </summary>
		DisableHurt = 188,
		/// <summary>
		/// Should this player <see cref="Ped"/> periodically generate shocking events for being weird.
		/// </summary>
		PlayerIsWeird = 189,
		WarpIntoLeadersVehicle = 192,
		/// <summary>
		/// Do nothing when on foot, by default.
		/// </summary>
		DoNothingWhenOnFootByDefault = 193,
		UsingScenario = 194,
		VisibleOnScreen = 195,
		ActivateOnSwitchFromLowPhysicsLod = 197,
		/// <summary>
		/// Peds with this flag set won't be allowed to reactivate their ragdoll when hit by another ragdoll.
		/// </summary>
		DontActivateRagdollOnPedCollisionWhenDead = 198,
		/// <summary>
		/// Peds with this flag set won't be allowed to reactivate their ragdoll when hit by a vehicle.
		/// </summary>
		DontActivateRagdollOnVehicleCollisionWhenDead = 199,
		/// <summary>
		/// True if we've ever been in non-melee combat.
		/// </summary>
		HasBeenInArmedCombat = 200,
		UseDiminishingAmmoRate = 201,
		/// <summary>
		/// True if the <see cref="Ped"/> never steer around other <see cref="Ped"/>s.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_CONFIG_FLAG_Avoidance_Ignore_All</c> in the exe.
		/// </remarks>
		AvoidanceIgnoreAll = 202,
		/// <summary>
		/// True if other <see cref="Ped"/>s never steer around the <see cref="Ped"/>.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_CONFIG_FLAG_Avoidance_Ignored_by_All</c> in the exe.
		/// </remarks>
		AvoidanceIgnoredbyAll = 203,
		/// <summary>
		/// True if the <see cref="Ped"/> steer around other <see cref="Ped"/>s that are members of group 1.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_CONFIG_FLAG_Avoidance_Ignore_Group1</c> in the exe.
		/// </remarks>
		AvoidanceIgnoreGroup1 = 204,
		/// <summary>
		/// True if the <see cref="Ped"/> are members of avoidance group 1.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_CONFIG_FLAG_Avoidance_Member_of_Group1</c> in the exe.
		/// </remarks>
		AvoidanceMemberofGroup1 = 205,
		/// <summary>
		/// Ped is forced to use specific seat index set by <c>SET_PED_GROUP_MEMBER_PASSENGER_INDEX</c>.
		/// </summary>
		ForcedToUseSpecificGroupSeatIndex = 206,
		LowPhysicsLodMayPlaceOnNavMesh = 207,
		/// <summary>
		/// If set, <see cref="Ped"/> will ignore explosion events.
		/// </summary>
		DisableExplosionReactions = 208,
		/// <summary>
		/// Set when player switches to an ai <see cref="Ped"/> and keeps the scripted task of the ai <see cref="Ped"/>, if unset we won't check for interrupts or time out.
		/// </summary>
		WaitingForPlayerControlInterrupt = 210,
		/// <summary>
		/// If set, <see cref="Ped"/> will stay in cover (won't come out to fire or move out during combat).
		/// </summary>
		ForcedToStayInCover = 211,
		/// <summary>
		/// Does the <see cref="Ped"/> generate sound events?
		/// </summary>
		GeneratesSoundEvents = 212,
		/// <summary>
		/// Does the <see cref="Ped"/> have the ability to respond to sound events?
		/// </summary>
		ListensToSoundEvents = 213,
		/// <summary>
		/// Ped can be targeting inside a vehicle.
		/// </summary>
		AllowToBeTargetedInAVehicle = 214,
		/// <summary>
		/// When exiting a vehicle, the <see cref="Ped"/> will wait for the direct entry point to be clear before exiting.
		/// </summary>
		WaitForDirectEntryPointToBeFreeWhenExiting = 215,
		/// <summary>
		/// Force the skydive exit if we're exiting the vehicle.
		/// </summary>
		ForceExitToSkyDive = 217,
		AllowPedInVehiclesOverrideTaskFlags = 219,
		/// <summary>
		/// Disable the skydive exit if we're exiting the vehicle.
		/// </summary>
		DisableExitToSkyDive = 221,
		ScriptHasDisabledCollision = 222,
		UseAmbientModelScaling = 223,
		DisablePotentialToBeWalkedIntoResponse = 225,
		/// <summary>
		/// This <see cref="Ped"/> will not avoid other <see cref="Ped"/>s whilst navigating.
		/// </summary>
		DisablePedAvoidance = 226,
		/// <summary>
		/// When the <see cref="Ped"/> dies, it will ragdoll instead of potentially choosing an animated death.
		/// </summary>
		ForceRagdollUponDeath = 227,
		/// <summary>
		/// Disable panic in vehicle.
		/// </summary>
		DisablePanicInVehicle = 229,
		/// <summary>
		/// Allow the <see cref="Ped"/> to detach trailers from vehicles.
		/// </summary>
		AllowedToDetachTrailer = 230,
		AllowBlockDeadPedRagdollActivation = 235,
		IsHoldingProp = 236,
		/// <summary>
		/// ForceSkin character cloth on creation when flag is set.
		/// </summary>
		ForceSkinCharacterCloth = 240,
		/// <summary>
		/// Player will leave the engine on when exiting a vehicle normally.
		/// </summary>
		LeaveEngineOnWhenExitingVehicles = 241,
		/// <summary>
		/// tells taskmobile phone to not texting animations.  Currently don't play these in MP.
		/// </summary>
		PhoneDisableTextingAnimations = 242,
		/// <summary>
		/// tells taskmobile phone to not talking animations.  Currently don't play these in MP.
		/// </summary>
		PhoneDisableTalkingAnimations = 243,
		/// <summary>
		/// tells taskmobile phone to not camera animations.  Currently don't play these in SP.
		/// </summary>
		PhoneDisableCameraAnimations = 244,
		/// <summary>
		/// Stops the <see cref="Ped"/> from accidentally firing his weapon when shot.
		/// </summary>
		DisableBlindFiringInShotReactions = 245,
		/// <summary>
		/// This makes it so that OTHER <see cref="Ped"/>s are allowed to take cover at points that would otherwise be rejected due to proximity.
		/// </summary>
		AllowNearbyCoverUsage = 246,
		/// <summary>
		/// If the <see cref="Ped"/> is a law enforcement <see cref="Ped"/> then we will NOT quit combat due to a target player no longer having a wanted level.
		/// </summary>
		CanAttackNonWantedPlayerAsLaw = 249,
		/// <summary>
		/// If set, the <see cref="Ped"/> will take damage if the car they are in crashes.
		/// </summary>
		WillTakeDamageWhenVehicleCrashes = 250,
		/// <summary>
		/// If this ai <see cref="Ped"/> is driving the vehicle, if the player taps to enter, they will enter as a rear passenger, if they hold, they'll jack the driver.
		/// </summary>
		AICanDrivePlayerAsRearPassenger = 251,
		/// <summary>
		/// If a friendly player is driving the vehicle, if the player taps to enter, they will enter as a passenger, if they hold, they'll jack the driver.
		/// </summary>
		PlayerCanJackFriendlyPlayers = 252,
		OnStairs = 253,
		/// <summary>
		/// If this ai <see cref="Ped"/> is driving the vehicle, allow players to get in passenger seats.
		/// </summary>
		AIDriverAllowFriendlyPassengerSeatEntry = 255,
		ParentCarIsBeingRemoved = 256,
		AllowMissionPedToUseInjuredMovement = 257,
		/// <summary>
		/// Don't use certain seats (like hanging on the side of a vehicle).
		/// </summary>
		PreventUsingLowerPrioritySeats = 261,
		JustLeftVehicleNeedsReset = 262,
		/// <summary>
		/// If set, teleport if <see cref="Ped"/> is not in the leader's vehicle before TaskEnterVehicle::m_SecondsBeforeWarpToLeader.
		/// </summary>
		TeleportToLeaderVehicle = 268,
		/// <summary>
		/// Don't walk extra far around weird <see cref="Ped"/>s like trevor.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_CONFIG_FLAG_Avoidance_Ignore_WeirdPedBuffer</c> in the exe.
		/// </remarks>
		AvoidanceIgnoreWeirdPedBuffer = 269,
		OnStairSlope = 270,
		/// <summary>
		/// Don't add a blip for this cop.
		/// </summary>
		DontBlipCop = 272,
		/// <summary>
		/// Kill the <see cref="Ped"/> if it becomes trap<see cref="Ped"/> and cannot get up.
		/// </summary>
		KillWhenTrapped = 275,
		EdgeDetected = 276,
		AlwaysWakeUpPhysicsOfIntersectedPeds = 277,
		/// <summary>
		/// If set, the <see cref="Ped"/> will avoid tear gas.
		/// </summary>
		AvoidTearGas = 279,
		StoppedSpeechUponFreezing = 280,
		/// <summary>
		/// If set, CPed::DAMAGED_GOTOWRITHE will no longer get set.  In particular, tazer hits wil no longer kill the <see cref="Ped"/> in one hit.
		/// </summary>
		DisableGoToWritheWhenInjured = 281,
		/// <summary>
		/// If set the <see cref="Ped"/> will only use their forced seat index if the vehicle they're entering is a heli as part of a group.
		/// </summary>
		OnlyUseForcedSeatWhenEnteringHeliInGroup = 282,
		ThrownFromVehicleDueToExhaustion = 283,
		/// <summary>
		/// Disables weird <see cref="Ped"/> events.
		/// </summary>
		DisableWeirdPedEvents = 285,
		/// <summary>
		/// This <see cref="Ped"/> should charge if in combat right away, for use by scripts, cleared once <see cref="Ped"/> charges.
		/// </summary>
		ShouldChargeNow = 286,
		RagdollingOnBoat = 287,
		HasBrandishedWeapon = 288,
		PedHasBeenSeen = 291,
		PedIsInReusePool = 292,
		/// <summary>
		/// This <see cref="Ped"/> should ignore shocking events.
		/// </summary>
		DisableShockingEvents = 294,
		MovedUsingLowLodPhysicsSinceLastActive = 295,
		/// <summary>
		/// If true, the <see cref="Ped"/> will not react to a <see cref="Ped"/> standing on the roof.
		/// </summary>
		NeverReactToPedOnRoof = 296,
		/// <summary>
		/// If true, the <see cref="Ped"/> will not react to <see cref="Ped"/>s driving on pavement.
		/// </summary>
		DisableShockingDrivingOnPavementEvents = 299,
		DisablePedConstraints = 301,
		/// <summary>
		/// If set, <see cref="Ped"/> will peek once before firing in cover. Cleared upon peeking.
		/// </summary>
		ForceInitialPeekInCover = 302,
		/// <summary>
		/// If true, disable followers jumping out of cars after their group leader.
		/// </summary>
		DisableJumpingFromVehiclesAfterLeader = 305,
		DontActivateRagdollFromPlayerPedImpact = 306,
		DontActivateRagdollFromAiRagdollImpact = 307,
		DontActivateRagdollFromPlayerRagdollImpact = 308,
		DisableQuadrupedSpring = 309,
		IsInCluster = 310,
		/// <summary>
		/// Set this for a <see cref="Ped"/> to be ignored by the auto opened doors when checking to see if the door should be opened.
		/// </summary>
		IgnoredByAutoOpenDoors = 312,
		PreferInjuredGetup = 313,
		/// <summary>
		/// Purposely ignore the melee active combatant role and push them into a support or inactive combatant role.
		/// </summary>
		ForceIgnoreMeleeActiveCombatant = 314,
		/// <summary>
		/// If set, <see cref="Ped"/> will ignore sound events generated by entities it can't see.
		/// </summary>
		CheckLoSForSoundEvents = 315,
		JackedAbandonedCar = 316,
		/// <summary>
		/// If set, <see cref="Ped"/> can play FRIEND_FOLLOWED_BY_PLAYER lines.
		/// </summary>
		CanSayFollowedByPlayerAudio = 317,
		/// <summary>
		/// If set, the <see cref="Ped"/> will activate ragdoll much more easily on contact with the player.
		/// </summary>
		ActivateRagdollFromMinorPlayerContact = 318,
		/// <summary>
		/// Ped has cloth collision bounds.
		/// </summary>
		HasClothCollisionBounds = 321,
		HasHighHeels = 322,
		/// <summary>
		/// If set on a non-law <see cref="Ped"/> that has law like behavior (i.e. security) then that <see cref="Ped"/> will not use the law like behaviors/logic.
		/// </summary>
		DontBehaveLikeLaw = 324,
		SpawnedAtScenario = 325,
		/// <summary>
		/// If set, police will not perform the CTaskShockingPoliceInvestigate behavior on the <see cref="Ped"/>.
		/// </summary>
		DisablePoliceInvestigatingBody = 326,
		/// <summary>
		/// If set, the <see cref="Ped"/> will no longer shoot while writhing.
		/// </summary>
		DisableWritheShootFromGround = 327,
		/// <summary>
		/// If set the <see cref="Ped"/> will only just the warp entry points if there are no animated entry points available.
		/// </summary>
		LowerPriorityOfWarpSeats = 328,
		/// <summary>
		/// If set the <see cref="Ped"/> can't be talked to.
		/// </summary>
		DisableTalkTo = 329,
		/// <summary>
		/// If set the <see cref="Ped"/> will not be blip<see cref="Ped"/> by the wanted system.
		/// </summary>
		DontBlip = 330,
		IsSwitchingWeapon = 331,
		/// <summary>
		/// If set, the <see cref="Ped"/> will ignore leg IK request restrictions for non-player <see cref="Ped"/>s.
		/// </summary>
		IgnoreLegIkRestrictions = 332,
		JackedOutOfMyVehicle = 334,
		WentIntoCombatAfterBeingJacked = 335,
		DontActivateRagdollForVehicleGrab = 336,
		/// <summary>
		/// If set, the <see cref="Ped"/> will timeslice it's DoNothing Task when computing default task.
		/// </summary>
		AllowTaskDoNothingTimeslicing = 339,
		ForcedToStayInCoverDueToPlayerSwitch = 340,
		/// <summary>
		/// If set, the <see cref="Ped"/> will not be allowed to jack any other players (not synced).
		/// </summary>
		NotAllowedToJackAnyPlayers = 342,
		KilledByStandardMelee = 344,
		/// <summary>
		/// If set, the <see cref="Ped"/> will always exit the train when it stops at a station.
		/// </summary>
		AlwaysLeaveTrainUponArrival = 345,
		/// <summary>
		/// If set, Only allow <see cref="Ped"/> to writhe from weapon damage, not from other stuff, like small vehicle impacts.
		/// </summary>
		OnlyWritheFromWeaponDamage = 347,
		EquipJetpack = 349,
		ScriptHasCompletelyDisabledCollision = 351,
		/// <summary>
		/// Don't do distance from camera culling of the deep surface check, needed for detecting snow, mud, etc.
		/// </summary>
		ForceDeepSurfaceCheck = 356,
		/// <summary>
		/// Disable deep surface anims to prevent them slowing <see cref="Ped"/> down.
		/// </summary>
		DisableDeepSurfaceAnims = 357,
		/// <summary>
		/// If set the <see cref="Ped"/> will not be blip<see cref="Ped"/> by the wanted system, this will not be synced and be set on clones so the behaviour can be changed per player.
		/// </summary>
		DontBlipNotSynced = 358,
		/// <summary>
		/// Query only, see if the <see cref="Ped"/> is ducking in a vehicle.
		/// </summary>
		IsDuckingInVehicle = 359,
		/// <summary>
		/// If set the <see cref="Ped"/> will not automatically shuffle to the turret seat when it becomes free.
		/// </summary>
		PreventAutoShuffleToTurretSeat = 360,
		/// <summary>
		/// Disables the ignore events based on interior status check which normally has <see cref="Ped"/>s inside ignore events from outside.
		/// </summary>
		DisableEventInteriorStatusCheck = 361,
		HasReserveParachute = 362,
		UseReserveParachute = 363,
		/// <summary>
		/// If the <see cref="Ped"/> this is set on is in combat then any dislike feeling they have towards other <see cref="Ped"/>s will be treated as a hate feeling.
		/// </summary>
		TreatDislikeAsHateWhenInCombat = 364,
		/// <summary>
		/// Law with this set will only update the WL is the target player is seen. This includes on combat initialization as well as during normal LOS checks (ignoring "last known position" reports).
		/// </summary>
		OnlyUpdateTargetWantedIfSeen = 365,
		/// <summary>
		/// Allows the <see cref="Ped"/> to auto shuffle to the driver seat of a vehicle if the driver is dead (law and MP <see cref="Ped"/>s would do this normally).
		/// </summary>
		AllowAutoShuffleToDriversSeat = 366,
		DontActivateRagdollFromSmokeGrenade = 367,
		/// <summary>
		/// If set prevents the <see cref="Ped"/> from reacting to silenced bullets fired from network clone <see cref="Ped"/>s (use for <see cref="Ped"/>s where stealth kills are important).
		/// </summary>
		PreventReactingToSilencedCloneBullets = 372,
		/// <summary>
		/// Blocks <see cref="Ped"/> from creating the injured cry for help events (run over, tazed or melee would usually do this).
		/// </summary>
		DisableInjuredCryForHelpEvents = 373,
		/// <summary>
		/// Prevents <see cref="Ped"/>s riding trains from getting off them.
		/// </summary>
		NeverLeaveTrain = 374,
		/// <summary>
		/// Prevents <see cref="Ped"/> dropping jetpack when they die.
		/// </summary>
		DontDropJetpackOnDeath = 375,
		/// <summary>
		/// Prevents <see cref="Ped"/> from auto-equipping helmets when entering a bike (includes quad bikes).
		/// </summary>
		DisableAutoEquipHelmetsInBikes = 380,
		/// <summary>
		/// Prevents <see cref="Ped"/> from auto-equipping helmets when entering an aircraft.
		/// </summary>
		DisableAutoEquipHelmetsInAircraft = 381,
		PreferNoPriorityRemoval = 384,
		IsClimbingLadder = 388,
		/// <summary>
		/// Flag to indicate that player has no shoes(used for first person aiming camera).
		/// </summary>
		HasBareFeet = 389,
		/// <summary>
		/// It will force the <see cref="Ped"/> to abandon its vehicle (when using TaskGoToPointAnyMeans) if it is unable to get back to road.
		/// </summary>
		GoOnWithoutVehicleIfItIsUnableToGetBackToRoad = 391,
		/// <summary>
		/// This will block health pickups from being created when the <see cref="Ped"/> dies.
		/// </summary>
		BlockDroppingHealthSnacksOnDeath = 392,
		ResetLastVehicleOnVehicleExit = 393,
		/// <summary>
		/// Forces threat response to melee actions from non friend to friend <see cref="Ped"/>s.
		/// </summary>
		ForceThreatResponseToNonFriendToFriendMeleeActions = 394,
		/// <summary>
		/// Do not respond to random <see cref="Ped"/>s damage.
		/// </summary>
		DontRespondToRandomPedsDamage = 395,
		/// <summary>
		/// Shares the same logic of PCF_OnlyUpdateTargetWantedIfSeen but will continue to check even after the initial WL is set.
		/// </summary>
		AllowContinuousThreatResponseWantedLevelUpdates = 396,
		/// <summary>
		/// The target loss response will not be reset to exit task on cleanup if this is set.
		/// </summary>
		KeepTargetLossResponseOnCleanup = 397,
		/// <summary>
		/// Similar to DontDragMeOutCar except it will still allow AI to drag the <see cref="Ped"/> out of a vehicle.
		/// </summary>
		PlayersDontDragMeOutOfCar = 398,
		/// <summary>
		/// Whenever the <see cref="Ped"/> starts shooting while going to a point, it trigger a responded to threat broadcast.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_CONFIG_FLAG_BroadcastRepondedToThreatWhenGoingToPointShooting</c> in the exe, but this enum uses the corrected name.
		/// </remarks>,
		BroadcastRespondedToThreatWhenGoingToPointShooting = 399,
		/// <summary>
		/// If this is set then IsFriendlyWith will ignore the <see cref="Ped"/> type checks (i.e. two PEDTYPE_COP <see cref="Ped"/>s are not automatically friendly).
		/// </summary>
		IgnorePedTypeForIsFriendlyWith = 400,
		/// <summary>
		/// Any non friendly <see cref="Ped"/> will be considered as hated instead when in combat.
		/// </summary>
		TreatNonFriendlyAsHateWhenInCombat = 401,
		/// <summary>
		/// Suppresses "LeaderExistedCarAsDriver" events. Ped won't exit vehicle if leader isn't in it as well.
		/// </summary>
		DontLeaveVehicleIfLeaderNotInVehicle = 402,
		/// <summary>
		/// Allow melee reaction to come through even if proof is on.
		/// </summary>
		AllowMeleeReactionIfMeleeProofIsOn = 404,
		/// <summary>
		/// If this is set, <see cref="Ped"/> won't be instantly killed if vehicle is blown up. Instead, they will take normal explosive damage and be forced to exit the vehicle if they're still alive.
		/// </summary>
		UseNormalExplosionDamageWhenBlownUpInVehicle = 407,
		/// <summary>
		/// Blocks locking on of the vehicle that the <see cref="Ped"/> is inside.
		/// </summary>
		DisableHomingMissileLockForVehiclePedInside = 408,
		/// <summary>
		/// Disable taking off the scuba gear. Same as PRF_DisableTakeOffScubaGear but on a config flag.
		/// </summary>
		DisableTakeOffScubaGear = 409,
		/// <summary>
		/// Melee fist weapons (ie knuckle duster) won't apply relative health damage scaler (MeleeRightFistTargetHealthDamageScaler in weapon info).
		/// </summary>
		IgnoreMeleeFistWeaponDamageMult = 410,
		/// <summary>
		/// Law <see cref="Ped"/>s will be triggered to flee if player triggers an appropriate event (even if <see cref="Ped"/> is not wanted) instead of entering combat. NB: Only synced over the network when set on players.
		/// </summary>
		LawPedsCanFleeFromNonWantedPlayer = 411,
		ForceBlipSecurityPedsIfPlayerIsWanted = 412,
		/// <summary>
		/// Don't use nav mesh for navigating to scenario points. DLC Hack for yachts.
		/// </summary>
		UseGoToPointForScenarioNavigation = 414,
		/// <summary>
		/// Don't clear local <see cref="Ped"/>'s wanted level when remote <see cref="Ped"/> in the same car has his wanted level cleared by script.
		/// </summary>
		DontClearLocalPassengersWantedLevel = 415,
		/// <summary>
		/// Block auto weapon swaps for weapon pickups.
		/// </summary>
		BlockAutoSwapOnWeaponPickups = 416,
		/// <summary>
		/// Increase AI targeting score for <see cref="Ped"/>s with this flag.
		/// </summary>
		ThisPedIsATargetPriorityForAI = 417,
		/// <summary>
		/// Indicates <see cref="Ped"/> is using switch helmet visor up/down anim.
		/// </summary>
		IsSwitchingHelmetVisor = 418,
		/// <summary>
		/// Indicates <see cref="Ped"/> is using switch helmet visor up/down anim.
		/// </summary>
		ForceHelmetVisorSwitch = 419,
		/// <summary>
		/// Overrides <see cref="Ped"/> footstep particle effects with the overriden footstep effect.
		/// </summary>
		UseOverrideFootstepPtFx = 421,
		/// <summary>
		/// Disables vehicle combat.
		/// </summary>
		DisableVehicleCombat = 422,
		TreatAsFriendlyForTargetingAndDamage = 423,
		/// <summary>
		/// Allows transition into bike alternate animations (PI menu option).
		/// </summary>
		AllowBikeAlternateAnimations = 424,
		TreatAsFriendlyForTargetingAndDamageNonSynced = 425,
		/// <summary>
		/// Use Franklin's alternate lock picking animations for forced entry.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_RESET_FLAG_UseLockpickVehicleEntryAnimations</c> in the exe.
		/// </remarks>
		UseLockPickVehicleEntryAnimations = 426,
		/// <summary>
		/// When set, player will be able to sprint inside interiors even if it is tagged to prevent it.
		/// </summary>
		IgnoreInteriorCheckForSprinting = 427,
		/// <summary>
		/// When set, swat helicopters will spawn within last spotted location instead of actual <see cref="Ped"/> location (and target is a player).
		/// </summary>
		SwatHeliSpawnWithinLastSpottedLocation = 428,
		/// <summary>
		/// Prevents <see cref="Ped"/> from playing start engine anims (and turning engine on).
		/// </summary>
		DisableStartEngine = 429,
		/// <summary>
		/// Makes <see cref="Ped"/> ignore being on fire (fleeing, reacting to CEventOnFire event).
		/// </summary>
		IgnoreBeingOnFire = 430,
		/// <summary>
		/// Disables turret seat and activity seat preference for vehicle entry for local player.
		/// </summary>
		DisableTurretOrRearSeatPreference = 431,
		/// <summary>
		/// Will not spawn wanted helicopters to chase after this target.
		/// </summary>
		DisableWantedHelicopterSpawning = 432,
		/// <summary>
		/// Will only create aimed at events if player is within normal perception of the target.
		/// </summary>
		UseTargetPerceptionForCreatingAimedAtEvents = 433,
		/// <summary>
		/// Will prevent homing lockon on the <see cref="Ped"/>.
		/// </summary>
		/// <remarks>
		/// The original name is <c>CPED_CONFIG_FLAG_DisableHomingMissileLockon</c> in the exe, but this enum uses the corrected name.
		/// </remarks>,
		DisableHomingMissileLockOn = 434,
		/// <summary>
		/// Ignore max number of active support combatants and let <see cref="Ped"/> join them as such.
		/// </summary>
		ForceIgnoreMaxMeleeActiveSupportCombatants = 435,
		/// <summary>
		/// Will try to stay within set defensive area while driving a vehicle.
		/// </summary>
		StayInDefensiveAreaWhenInVehicle = 436,
		/// <summary>
		/// Will prevent the <see cref="Ped"/> from communicating target position to all other friendly <see cref="Ped"/>s.
		/// </summary>
		DontShoutTargetPosition = 437,
		/// <summary>
		/// Will apply full headshot damage, regardless if <see cref="Ped"/> has a helmet (or armored one).
		/// </summary>
		DisableHelmetArmor = 438,
		/// <summary>
		/// Will ignore the friendly fire setting set by NETWORK_SET_FRIENDLY_FIRE_OPTION when checking if <see cref="Ped"/> can be damaged.
		/// </summary>
		IgnoreNetSessionFriendlyFireCheckForAllowDamage = 442,
		/// <summary>
		/// Will make <see cref="Ped"/> stay in combat even if the player hes targeting starts being attacked by cops.
		/// </summary>
		DontLeaveCombatIfTargetPlayerIsAttackedByPolice = 443,
		/// <summary>
		/// Will check when entering a vehicle if it is locked before warping.
		/// </summary>
		CheckLockedBeforeWarp = 444,
		/// <summary>
		/// Will prevent a player from shuffling across to make room if another player is entering from the same side.
		/// </summary>
		DontShuffleInVehicleToMakeRoom = 445,
		/// <summary>
		/// Will give the <see cref="Ped"/> a weapon to use once their weapon is removed for getups.
		/// </summary>
		GiveWeaponOnGetup = 446,
		/// <summary>
		/// Ped fired projectiles will ignore the vehicle they are in.
		/// </summary>
		DontHitVehicleWithProjectiles = 447,
		/// <summary>
		/// Will prevent <see cref="Ped"/> from forcing entry into cars that are open from TryLockedDoor state.
		/// </summary>
		DisableForcedEntryForOpenVehiclesFromTryLockedDoor = 448,
		/// <summary>
		/// his <see cref="Ped"/> will fire rockets that explode when close to its target, and won't affect it.
		/// </summary>
		FiresDummyRockets = 449,
		/// <summary>
		/// This <see cref="Ped"/> has created a decoy.
		/// </summary>
		HasEstablishedDecoy = 452,
		/// <summary>
		/// Will prevent dispatched helicopters from landing and dropping off <see cref="Ped"/>s.
		/// </summary>
		BlockDispatchedHelicoptersFromLanding = 453,
		/// <summary>
		/// Will prevent <see cref="Ped"/>s from crying for help when shot with the stun gun.
		/// </summary>
		DontCryForHelpOnStun = 454,
		/// <summary>
		/// If set, the <see cref="Ped"/> may be incapacitated.
		/// </summary>
		CanBeIncapacitated = 456,
		/// <summary>
		/// If set, we wont set a new target after a melee attack.
		/// </summary>
		DontChangeTargetFromMelee = 458,
		/// <summary>
		/// Prevents a dead <see cref="Ped"/> from sinking.
		/// </summary>
		RagdollFloatsIndefinitely = 460,
		/// <summary>
		/// Blocks electric weapon damage.
		/// </summary>
		BlockElectricWeaponDamage = 461,
	}
}
