namespace GTA
{
    /// <summary>
    /// An enumeration of known reset flags for <see cref="Ped"/>, which will be required to set or get every frame you need to set or get.
    /// </summary>
    /// <remarks>
    /// You can check if names of this enum are included in the exe by searching the dumped exe for hashed values of names like <c>CPED_RESET_FLAG_[enum name]</c> without case conversion
    /// (for example, search the dumped exe for 0x49F290D0, which is the hashed value of <c>CPED_RESET_FLAG_DisablePlayerJumping</c>).
    /// </remarks>
    public enum PedResetFlagToggles
    {
        /// <summary>
        /// Decides whether to IK rotate body to match ground angle.
        /// </summary>
        FallenDown,
        /// <summary>
        /// Forces <see cref="Ped"/> to stop rendering this frame (for example - drive task can stop
        /// <see cref="Ped"/>s rendering inside buses).
        /// </summary>
        DontRenderThisFrame,
        IsDrowning,
        PedHitWallLastFrame,
        UsingMobilePhone,
        /// <summary>
        /// Completely disables processing of on-foot movement anim blending for this frame.
        /// </summary>
        BlockMovementAnims,
        /// <summary>
        /// Zeroes out all inputs to movement system this frame, causing <see cref="Ped"/> to stop moving.
        /// </summary>
        ZeroDesiredMoveBlendRatios,
        /// <summary>
        /// If this is set, then `<c>TaskSimpleMoveDoNothing</c>` will not reset desired move blend ratio to zero
        /// this frame.
        /// </summary>
        DontChangeMbrInSimpleMoveDoNothing,
        /// <summary>
        /// Whether this <see cref="Ped"/> is following a route of some sort - used to let simplest goto task pull
        /// the <see cref="Ped"/> back onto their current route segment.
        /// </summary>
        FollowingRoute,
        /// <summary>
        /// Whether the <see cref="Ped"/> is cornering via a spline curve, which will take them off their route line
        /// segment.
        /// </summary>
        TakingRouteSplineCorner,
        Wandering,
        /// <summary>
        /// Indicates if the <see cref="Ped"/> needs to call process physics for (main) tasks this frame.
        /// </summary>
        ProcessPhysicsTasks,
        /// <summary>
        /// Indicates if the <see cref="Ped"/> needs to call `<c>ProcessPreRender2</c>` for tasks.
        /// </summary>
        ProcessPreRender2,
        SetLastMatrixDone,
        /// <summary>
        /// Set when <see cref="Ped"/> fires any weapon, so script can check the flag.
        /// </summary>
        FiringWeapon,
        /// <summary>
        /// Set if the <see cref="Ped"/> is likely to be searching for cover, used by the navmesh to load coverpoints
        /// around interested <see cref="Ped"/>s.
        /// </summary>
        SearchingForCover,
        /// <summary>
        /// Set if the <see cref="Ped"/> wants to keep their current cover point this frame,
        /// otherwise it gets released.
        /// </summary>
        KeepCoverPoint,
        /// <summary>
        /// If the <see cref="Ped"/> is climbing, shimmying or on a ladder. Stops `<c>ProcessProbes()</c>` from
        /// happening.
        /// </summary>
        IsClimbing,
        /// <summary>
        /// Is the <see cref="Ped"/> jumping.
        /// </summary>
        IsJumping,
        /// <summary>
        /// Is landing after being in the air.
        /// </summary>
        IsLanding,
        /// <summary>
        /// Flag AI can set to make <see cref="Ped"/> get culled further away (used for roadblock cops, criminals...)
        /// </summary>
        CullExtraFarAway,
        DontActivateRagdollFromAnyPedImpactReset,
        ForceScriptControlledRagdoll,
        /// <summary>
        /// For use by tasks - puts the <see cref="Ped"/> into Kinematic physics mode.
        /// In this mode the <see cref="Ped"/> will push other objects our of the way, but not be physically affected
        /// by them.
        /// </summary>
        TaskUseKinematicPhysics,
        /// <summary>
        /// Stops weapon switch processing.
        /// </summary>
        TemporarilyBlockWeaponSwitching,
        /// <summary>
        /// Stops clamping the foot IK. Maybe the clamps should be parameters of the IK manager, but apparently that is to be refactored in future
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DoNotClampFootIk</c>`.
        /// </remarks>
        DoNotClampFootIK,
        /// <summary>
        /// `<c>TASK_SMART_FLEE</c>`, or `<c>TASK_COMPLEX_LEAVE_CAR_AND_FLEE</c>`
        /// </summary>
        // ReSharper disable once InconsistentNaming
        MoveBlend_bFleeTaskRunning,
        /// <summary>
        /// `<c>TASK_GUN</c>` or `<c>TASK_USE_COVER</c>`
        /// </summary>
        IsAiming,
        /// <summary>
        /// `<c>TASK_COMPLEX_GUN</c>` (only)
        /// </summary>
        // ReSharper disable once InconsistentNaming
        MoveBlend_bTaskComplexGunRunning,
        /// <summary>
        /// `<c>TASK_COMPLEX_MELEE</c>`
        /// </summary>
        // ReSharper disable once InconsistentNaming
        MoveBlend_bMeleeTaskRunning,
        /// <summary>
        /// `<c>TASK_COMPLEX_SEARCH_FOR_PED_ON_FOOT</c>`
        /// </summary>
        // ReSharper disable once InconsistentNaming
        MoveBlend_bCopSearchTaskRunning,
        /// <summary>
        /// <see cref="Ped"/> is patrolling in a <see cref="Vehicle"/>, likely meaning they are swat or police.
        /// </summary>
        PatrollingInVehicle,
        RaiseVelocityChangeLimit,
        DimTargetReticule,
        /// <summary>
        /// Whether this <see cref="Ped"/> is walking around another <see cref="Ped"/> (one frame latency).
        /// </summary>
        IsWalkingRoundPlayer,
        GestureAnimsAllowed,
        /// <summary>
        /// Blocks viseme anims from playing.
        /// </summary>
        VisemeAnimsBlocked,
        /// <summary>
        /// Blocks new ambient idles from starting.
        /// </summary>
        AmbientAnimsBlocked,
        KnockedToTheFloorByPlayer,
        RandomisePointsDuringNavigation,
        Prevent180SkidTurns,
        IsOnAssistedMovementRoute,
        /// <summary>
        /// Indicates if the <see cref="Ped"/> should apply the velocity directly to the physics collider or go through
        /// the force solver.
        /// </summary>
        ApplyVelocityDirectly,
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DisablePlayerLockon</c>`.
        /// </remarks>
        DisablePlayerLockOn,
        /// <summary>
        /// If <see langword="true"/>, will reset the temp anim group when not ragdolling.
        /// </summary>
        ResetMoveGroupAfterRagdoll,
        /// <summary>
        /// Allow the <see cref="Ped"/> to rotate around freely.
        /// </summary>
        DisablePedConstraints,
        /// <summary>
        /// Disables player jumping if <see langword="true"/>. Reset in `<c>ResetPostPhysics</c>`, as it will be set
        /// via script.
        /// </summary>
        /// <remarks>
        /// Does not disable climbing.
        /// </remarks>
        DisablePlayerJumping,
        /// <summary>
        /// Disables player vaulting/climbing if <see langword="true"/>.
        /// </summary>
        /// <remarks>
        /// Does not disable auto-vault, but you can disable it with <see cref="DisablePlayerAutoVaulting"/>.
        /// </remarks>
        DisablePlayerVaulting,
        /// <summary>
        /// Disable the code that pushes <see cref="Ped"/>s which fall asleep in the air.
        /// </summary>
        DisableAsleepImpulse,
        ForcePostCameraAIUpdate,
        ForcePostCameraAnimUpdate,
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_ePostCameraAnimUpdateUseZeroTimestep</c>`.
        /// </remarks>
        PostCameraAnimUpdateUseZeroTimestep,
        CollideWithGlassRagdoll,
        CollideWithGlassWeapon,
        SyncDesiredHeadingToCurrentHeading,
        /// <summary>
        /// Don't freeze the <see cref="Ped"/> for not having bounds loaded around it.
        /// </summary>
        AllowUpdateIfNoCollisionLoaded,
        InternalWalkingRndPlayer,
        /// <summary>
        /// Setting Bomb (firing weapon is set also).
        /// </summary>
        PlacingCharge,
        /// <summary>
        /// Disable upper body animation tasks such as shove <see cref="Ped"/> and open door anims.
        /// </summary>
        ScriptDisableSecondaryAnimationTasks,
        SearchingForClimb,
        SearchingForDoors,
        WanderingStoppedForOtherPed,
        /// <summary>
        /// Suppresses AI generating fire events, so civilians won't be shocked or react, for use in a shooting range
        /// for example.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_SupressGunfireEvents</c>`.
        /// </remarks>
        SuppressGunfireEvents,
        /// <summary>
        /// Currently just for mounts, but could be expanded to anything with stamina.
        /// </summary>
        InfiniteStamina,
        /// <summary>
        /// Stops ragdoll and NM behaviors triggering from weapon damage unless the <see cref="Ped"/> has died.
        /// </summary>
        /// <remarks>
        /// Does not block explosion reactions.
        /// </remarks>
        BlockWeaponReactionsUnlessDead,
        /// <summary>
        /// Forces player to fire even if they aren't pressing fire.
        /// </summary>
        ForcePlayerFiring,
        /// <summary>
        /// Set when exiting the cover state saying if the <see cref="Ped"/> is facing left.
        /// </summary>
        InCoverFacingLeft,
        /// <summary>
        /// If set the <see cref="Ped"/> will go into peeking if they are already in cover.
        /// </summary>
        ForcePeekFromCover,
        /// <summary>
        /// If set the <see cref="Ped"/> will not be allowed to change their crouch state.
        /// </summary>
        NotAllowedToChangeCrouchState,
        /// <summary>
        /// Forces a <see cref="Ped"/> to strafe.
        /// </summary>
        ForcePedToStrafe,
        /// <summary>
        /// Forces a <see cref="Ped"/> to use the melee strafing anims when strafing.
        /// </summary>
        ForceMeleeStrafingAnims,
        /// <summary>
        /// To be used by scripts - puts the <see cref="Ped"/> into Kinematic physics mode.
        /// In this mode, the <see cref="Ped"/> will push other physical objects our of the way, but not be physically
        /// affected by them.
        /// </summary>
        UseKinematicPhysics,
        /// <summary>
        /// Clears the player's lock-on target next frame.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_ClearLockonTarget</c>`.
        /// </remarks>
        ClearLockOnTarget,
        /// <summary>
        /// Activates can <see cref="Ped"/> see hated <see cref="Ped"/> generating events even when blocking of
        /// non-temp events is on.
        /// </summary>
        CanPedSeeHatedPedBeingUsed,
        /// <summary>
        /// Makes the <see cref="Ped"/> perform an instant blend to aim if starting a gun task this frame.
        /// </summary>
        InstantBlendToAim,
        /// <summary>
        /// Forces the <see cref="Ped"/> to use an improved idle turning system that should help him turn to face more
        /// quickly.
        /// </summary>
        ForceImprovedIdleTurns,
        /// <summary>
        /// Set when damage is inflicted by this <see cref="Ped"/> on another <see cref="Ped"/>.
        /// </summary>
        HitPedWithWeapon,
        /// <summary>
        /// Forces a <see cref="Ped"/> to the scripted camera heading instead of gameplay.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_ForcePedToUseScripCamHeading</c>`.
        /// </remarks>
        ForcePedToUseScriptCamHeading,
        /// <summary>
        /// Makes the capsule physics push the <see cref="Ped"/> out of the ground even when extracting Z.
        /// </summary>
        ProcessProbesWhenExtractingZ,
        /// <summary>
        /// Should the <see cref="Ped"/> keep their desired cover point this frame.
        /// </summary>
        KeepDesiredCoverPoint,
        /// <summary>
        /// Whether the <see cref="Ped"/> has already processed slowing down for this corner.
        /// </summary>
        HasProcessedCornering,
        /// <summary>
        /// Set when the <see cref="Ped"/> standing capsule hits the forklift forks.
        /// </summary>
        StandingOnForkliftForks,
        /// <summary>
        /// <see cref="Ped"/> is running the reaction task this frame.
        /// </summary>
        AimWeaponReactionRunning,
        /// <summary>
        /// <see cref="Ped"/> is in contact with `<c>GTA_FOLIAGE_TYPE</c>` bounds.
        /// </summary>
        InContactWithFoliage,
        /// <summary>
        /// <see cref="Ped"/> will always collide with explosions, even when collision is off.
        /// </summary>
        ForceExplosionCollisions,
        /// <summary>
        /// When checking LOS against targets, this <see cref="Ped"/> will ignore their cover (if the cover exists).
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_IgnoreTargetsCoverForLOS</c>`.
        /// </remarks>
        IgnoreTargetsCoverForLos,
        /// <summary>
        /// <see cref="Ped"/> should not play animated damager reactions while this flag is set.
        /// </summary>
        BlockAnimatedWeaponReactions,
        /// <summary>
        /// Removes the <see cref="Ped"/> capsule from the physics simulation.
        /// </summary>
        DisablePedCapsule,
        /// <summary>
        /// Force the crouch flag to return <see langword="true"/> while in cover.
        /// </summary>
        DisableCrouchWhileInCover,
        /// <summary>
        /// Adds extra 2m onto the radius other <see cref="Ped"/>s use to avoid this <see cref="Ped"/> during local
        /// steering.
        /// </summary>
        IncreasedAvoidanceRadius,

        // `UNUSED_REPLACE_ME` reserves 90 in `ePedResetFlags`, but we *definitely* don't need that member to be
        // defined in this enum.

        /// <summary>
        /// Forces the <see cref="Ped"/> to apply forces to frags as if running on contact,
        /// to guarantee <see cref="Ped"/>s will smash through frag objects when playing custom anims.
        /// Can be set by level designers to force the <see cref="Ped"/> to smash more easily through frags.
        /// </summary>
        ForceRunningSpeedForFragSmashing = 91,
        /// <summary>
        /// While flagged, any mover animation will be applied to the offset of the attachment.
        /// </summary>
        EnableMoverAnimationWhileAttached,
        /// <summary>
        /// While flagged, The time delay before a player can fire after aiming is 0.
        /// </summary>
        NoTimeDelayBeforeShot,
        /// <summary>
        /// Informs climb code the <see cref="Ped"/> is doing an auto-vault.
        /// </summary>
        SearchingForAutoVaultClimb,
        /// <summary>
        /// Extends the range of a <see cref="Ped"/>s weapons to 250m.
        /// </summary>
        ExtraLongWeaponRange,
        /// <summary>
        /// Forces the player to only use direct access when entering <see cref="Vehicle"/>s.
        /// </summary>
        ForcePlayerToEnterVehicleThroughDirectDoorOnly,
        /// <summary>
        /// Can be set by AI tasks on the main task tree to disable a <see cref="Ped"/> getting cull from a <see cref="Vehicle"/>.
        /// </summary>
        TaskCullExtraFarAway,
        /// <summary>
        /// Set the entire time `<c>CTaskVault</c>` is running.
        /// </summary>
        IsVaulting,
        /// <summary>
        /// Set the entire time `<c>CTaskParachute</c>` is running.
        /// </summary>
        IsParachuting,
        /// <summary>
        /// If set, this will prevent the <see cref="Ped"/> from slowing down to take corners this frame.
        /// </summary>
        SuppressSlowingForCorners,
        /// <summary>
        /// Disables processing of probes.
        /// </summary>
        DisableProcessProbes,
        /// <summary>
        /// If this flag is set on a <see cref="Ped"/>, it will not attempt to auto-vault.
        /// </summary>
        DisablePlayerAutoVaulting,
        DisableGaitReduction,
        ExitVehicleTaskFinishedThisFrame,
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_RequiresLegIk</c>`.
        /// </remarks>
        RequiresLegIK,
        /// <summary>
        /// If set then the <see cref="Ped"/> is jay walking and a <see cref="Vehicle"/> is allowed to run him over.
        /// </summary>
        JayWalking,
        /// <summary>
        /// <see cref="Ped"/> will use bullet penetration code.
        /// </summary>
        UseBulletPenetration,
        /// <summary>
        /// Force all attackers to target the head of this <see cref="Ped"/>.
        /// </summary>
        ForceAimAtHead,
        /// <summary>
        /// In a scenario and not moving. Informs avoidance code that the <see cref="Ped"/> is not going anywhere and
        /// should be steered around rather than waited for. Needs to be set on the <see cref="Ped"/> before they can
        /// handle scenario gesture clip sets.
        /// </summary>
        /// <remarks>
        /// `<c>CTaskUseScenario::ProcessPreFSM()</c>` also sets this flag when the squared magnitude/length of
        /// the <see cref="Ped"/> velocity is less than 0.010000001 (internal hex representation: `<c>0x3C23D70B</c>`,
        /// the result of the expression `<c>0.1f * 0.1f</c>`).
        /// </remarks>
        IsInStationaryScenario,
        /// <summary>
        /// Stop weapon equipping.
        /// </summary>
        TemporarilyBlockWeaponEquipping,
        /// <summary>
        /// `<c>TASK_AIM_GUN_FROM_COVER_OUTRO</c>`
        /// </summary>
        CoverOutroRunning,
        /// <summary>
        /// Any targeting LoS checks will fail if any materials with 'see through' materials found.
        /// </summary>
        DisableSeeThroughChecksWhenTargeting,
        /// <summary>
        /// Putting on helmet. You should not set this flag, only query it.
        /// </summary>
        PuttingOnHelmet,
        /// <summary>
        /// Allows goto task to apply heading in order to pull a <see cref="Ped"/> back onto their route.
        /// </summary>
        AllowPullingPedOntoRoute,
        /// <summary>
        /// Allows attachment offsets to be updated from animated velocities.
        /// </summary>
        ApplyAnimatedVelocityWhilstAttached,
        /// <summary>
        /// `<c>TASK_ENTER_COVER</c>` : `<c>State_PlayAIEntryAnim</c>`
        /// </summary>
        AICoverEntryRunning,
        /// <summary>
        /// The <see cref="Ped"/> is entering threat response after panic exiting a scenario.
        /// </summary>
        ResponseAfterScenarioPanic,
        /// <summary>
        /// <see cref="Ped"/> is near a non-vehicle door.
        /// </summary>
        IsNearDoor,
        DisableTorsoSolver,
        /// <summary>
        /// When set, the <see cref="Ped"/> will play panic animations if in a vehicle.
        /// </summary>
        PanicInVehicle,
        /// <summary>
        /// Turn off dynamic adjustments to <see cref="Ped"/> capsules.
        /// </summary>
        DisableDynamicCapsuleRadius,
        /// <summary>
        /// Is currently in a rappel task.
        /// </summary>
        IsRappelling,
        /// <summary>
        /// When this <see cref="Ped"/> goes to `<c>CTaskThreatResponse</c>`, play the flee transition but not
        /// the reaction clip if fleeing.
        /// </summary>
        SkipReactInReactAndFlee,
        /// <summary>
        /// Will prevent this <see cref="Ped"/> from being a part of any other <see cref="Ped"/>s target list.
        /// </summary>
        CannotBeTargeted,
        /// <summary>
        /// <see cref="Ped"/> is in pure fall state (i.e. no parachuting, landing etc. included).
        /// </summary>
        IsFalling,
        /// <summary>
        /// Forces this <see cref="Ped"/> to the injured state after being stunned.
        /// </summary>
        ForceInjuryAfterStunned,
        /// <summary>
        /// The <see cref="Ped"/> has entered the hurt state this frame.
        /// </summary>
        HurtThisFrame,
        /// <summary>
        /// Prevent the <see cref="Ped"/> from shooting a weapon.
        /// </summary>
        BlockWeaponFire,
        /// <summary>
        /// Set the <see cref="Ped"/> capsule radius based on skeleton.
        /// </summary>
        ExpandPedCapsuleFromSkeleton,
        /// <summary>
        /// Toggle the weapon laser sight off for this frame.
        /// </summary>
        DisableWeaponLaserSight,
        /// <summary>
        /// Set when <see cref="Ped"/> gets set out of <see cref="Vehicle"/>.
        /// </summary>
        PedExitedVehicleThisFrame,
        /// <summary>
        /// <see cref="Ped"/> is searching for drop down.
        /// </summary>
        SearchingForDropDown,
        /// <summary>
        /// <see cref="Ped"/> should use tighter turn settings in human locomotion motion task.
        /// </summary>
        UseTighterTurnSettings,
        /// <summary>
        /// Disable the arm solver this frame.
        /// </summary>
        DisableArmSolver,
        DisableHeadSolver,
        DisableLegSolver,
        DisableTorsoReactSolver,
        ForcePreCameraAIUpdate,
        /// <summary>
        /// Set when <see cref="Ped"/>s require calls to `<c>ProcessMoveSignals()</c>`, for AI timeslicing to work
        /// with `<c>Move</c>`.
        /// </summary>
        TasksNeedProcessMoveSignalCalls,
        ShootFromGround,
        /// <summary>
        /// Set when a <see cref="Ped"/> is moving in an area where collisions with fixed geometry are unlikely.
        /// The <see cref="Ped"/>'s physics will not be forced to activate.
        /// </summary>
        NoCollisionMovementMode,
        /// <summary>
        /// <see cref="Ped"/> is near top of a ladder.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_IsNearLaddder</c>`.
        /// </remarks>
        IsNearLadder,
        SkipAimingIdleIntro,
        /// <summary>
        /// Set this for a <see cref="Ped"/> to be ignored by the auto opened doors when checking to see if the door
        /// should be opened.
        /// </summary>
        IgnoredByAutoOpenDoors,
        /// <summary>
        /// <see cref="Ped"/> should not play Ik damager reactions while this flag is set.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_BlockIkWeaponReactions</c>`.
        /// </remarks>
        BlockIKWeaponReactions,
        /// <summary>
        /// <see cref="Ped"/> was just created this frame.
        /// </summary>
        FirstPhysicsUpdate,
        /// <summary>
        /// <see cref="Ped"/> was spawned this frame by ambient population.
        /// </summary>
        SpawnedThisFrameByAmbientPopulation,
        DisableRootSlopeFixupSolver,
        /// <summary>
        /// Temporarily suspend any melee actions this frame (does not include hit reactions).
        /// Use <see cref="PedConfigFlagToggles.DisableMelee"/> to turn it off completely.
        /// </summary>
        SuspendInitiatedMeleeActions,
        /// <summary>
        /// Prevents <see cref="Ped"/> from getting the in air event the next frame.
        /// </summary>
        SuppressInAirEvent,
        /// <summary>
        /// If set, allows the <see cref="Ped"/> to have tasks incompatible with its current motion
        /// (the <see cref="Ped"/> will skip `<c>CheckTasksAreCompatibleWithMotion</c>` in that case).
        /// </summary>
        AllowTasksIncompatibleWithMotion,
        /// <summary>
        /// `<c>TASK_ENTER_VEHICLE</c>` or `<c>TASK_EXIT_VEHICLE</c>`
        /// </summary>
        IsEnteringOrExitingVehicle,

        // `CPED_RESET_FLAG_PlayerOnHorse` reserves 153, but that reset flag not used in that source code as you can
        // expect by the flag name and how GTA V doesn't feature horses at all, and we're sure the flag won't be used
        // in GTA V forever.

        /// <summary>
        /// <see cref="Ped"/> is running `<c>TASK_GUN</c>` and task's state is `<c>State_Aim</c>`.
        /// </summary>
        HasGunTaskWithAimingState = 154,
        /// <summary>
        /// This will suppress any melee action that is considered lethal (`<c>RA_IS_LETHAL</c>`, defined in
        /// `<c>action_table.meta</c>`).
        /// </summary>
        SuppressLethalMeleeActions,
        InstantBlendToAimFromScript,
        IsStillOnBicycle,
        IsSittingAndCycling,
        IsStandingAndCycling,
        IsDoingCoverAimOutro,
        ApplyCoverWeaponBlockingOffsets,
        IsInLowCover,
        /// <summary>
        /// Blocks ambient idles and base animations from playing.
        /// </summary>
        AmbientIdleAndBaseAnimsBlocked,
        /// <summary>
        /// If set, <see cref="Ped"/> will use alternative aiming/firing anims
        /// </summary>
        UseAlternativeWhenBlock,
        /// <summary>
        /// If set, the <see cref="Ped"/> will always force probe for being in water when in low LOD mode.
        /// </summary>
        ForceLowLodWaterCheck,
        /// <summary>
        /// If set, scale the head of the <see cref="Ped"/> to 0.001.
        /// </summary>
        MakeHeadInvisible,
        /// <summary>
        /// Don't auto run when set.
        /// </summary>
        NoAutoRunWhenFiring,
        /// <summary>
        /// Ignore certain aspects (FOV checks, etc.) of `<c>AffectsPedCore()</c>` on some events while
        /// the <see cref="Ped"/> is playing a scenario exit.
        /// </summary>
        PermitEventDuringScenarioExit,
        /// <summary>
        /// Enables/disables the low-level steering behaviour around <see cref="Vehicle"/>s.
        /// </summary>
        DisableSteeringAroundVehicles,
        /// <summary>
        /// Enables/disables the low-level steering behaviour around <see cref="Ped"/>s.
        /// </summary>
        DisableSteeringAroundPeds,
        /// <summary>
        /// Enables/disables the low-level steering behaviour around <see cref="Prop"/>s.
        /// </summary>
        DisableSteeringAroundObjects,
        /// <summary>
        /// Enables/disables the low-level steering behaviour around nav mesh edges.
        /// </summary>
        DisableSteeringAroundNavMeshEdges,
        WantsToEnterVehicleFromCover,
        WantsToEnterCover,
        WantsToEnterVehicleFromAiming,
        CapsuleBeingPushedByVehicle,
        /// <summary>
        /// Disable taking off the parachute pack.
        /// </summary>
        DisableTakeOffParachutePack,
        IsCallingPolice,
        /// <summary>
        /// Forces a combat taunt for <see cref="Ped"/>s using the insult special ability.
        /// This flag is set, they will always use the combat taunt when the special is activated.
        /// </summary>
        /// <remarks>
        /// Although the instance method `<c>IsPedNoticableToPlayer</c>` in the class `<c>CScriptPedAIBlips::AIBlip</c>`,
        /// which `<c>Update()</c>` in the class may call, set this flag as well as some ysc scripts, no game code or
        /// ysc scripts test if this flag is set as of v1.0.2699.0.
        /// </remarks>
        ForceCombatTaunt,
        /// <summary>
        /// The <see cref="Ped"/> will ignore combat taunt events.
        /// </summary>
        IgnoreCombatTaunts,
        /// <summary>
        /// <see langword="true"/> if we've already run this <see cref="Ped"/>'s AI and can skip it from within
        /// `<c>ProcessControl</c>`.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_SkipAiUpdateProcessControl</c>`.
        /// </remarks>
        SkipAIUpdateProcessControl,
        /// <summary>
        /// A reset flag that disables collision and gravity on the <see cref="Ped"/> and drives entity positions and
        /// rotations directly, rather than going through the physics update.
        /// </summary>
        OverridePhysics,
        /// <summary>
        /// <see langword="true"/> if physics was overriden during the last update.
        /// </summary>
        WasPhysicsOverridden,
        /// <summary>
        /// Block any on-foot weapon holding anims.
        /// </summary>
        BlockWeaponHoldingAnims,
        /// <summary>
        /// <see langword="true"/> if the <see cref="Ped"/>'s movement task should not adjust the heading of
        /// the <see cref="Ped"/>.
        /// </summary>
        DisableMoveTaskHeadingAdjustments,
        DisableBodyLookSolver,
        /// <summary>
        /// Will temporarily prevent any takedown from being performed on this <see cref="Ped"/>.
        /// </summary>
        PreventAllMeleeTakedowns,
        /// <summary>
        /// Will temporarily prevent any failed takedowns from being performed on this <see cref="Ped"/>.
        /// </summary>
        PreventFailedMeleeTakedowns,
        IsPedalling,
        /// <summary>
        /// <see cref="Ped"/> should use tighter avoidance settings in navigation task.
        /// </summary>
        UseTighterAvoidanceSettings,
        /// <summary>
        /// <see langword="true"/> if the active task on the main task tree is taking responsibility for the mover
        /// track.
        /// </summary>
        IsHigherPriorityClipControllingPed,
        /// <summary>
        /// Set to <see langword="true"/> if a <see cref="Vehicle"/> is pressing downward on the ragdoll.
        /// </summary>
        VehicleCrushingRagdoll,
        /// <summary>
        /// <see cref="Ped"/> was just activated this frame.
        /// </summary>
        OnActivationUpdate,
        /// <summary>
        /// Set this to disable setting the desired move blend ratio when forcing the motion state.
        /// Useful for forcing a flee start, etc.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_ForceMotionStateLeaveDesiredMBR</c>`.
        /// </remarks>
        ForceMotionStateLeaveDesiredMbr,
        /// <summary>
        /// Disable drop downs for this <see cref="Ped"/>.
        /// </summary>
        DisableDropDowns,
        /// <summary>
        /// <see cref="Ped"/> is in contact with `<c>GTA_FOLIAGE_TYPE</c>` bounds that are large and the player can be hidden.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_InContactWithBIGFoliage</c>`.
        /// </remarks>
        InContactWithBigFoliage,
        DisableTakeOffScubaGear,
        /// <summary>
        /// Tells `<c>CTaskMobilePhone</c>` to blend out move network and prevent it from blending in.
        /// </summary>
        DisableCellphoneAnimations,
        /// <summary>
        /// `<c>TASK_EXIT_VEHICLE</c>`
        /// </summary>
        IsExitingVehicle,
        /// <summary>
        /// Disables combat anims for <see cref="Ped"/>.
        /// </summary>
        DisableActionMode,
        /// <summary>
        /// Equipped weapon changed this frame.
        /// </summary>
        EquippedWeaponChanged,
        /// <summary>
        /// Some part appears to be constrained downwards.
        /// </summary>
        TouchingOverhang,
        /// <summary>
        /// We're standing on something flagged too steep for the player to stand on.
        /// </summary>
        TooSteepForPlayer,
        /// <summary>
        /// Block any secondary scripted task animations playing on this <see cref="Ped"/>.
        /// </summary>
        BlockSecondaryAnim,
        /// <summary>
        /// This <see cref="Ped"/> is running the combat task.
        /// </summary>
        IsInCombat,
        /// <summary>
        /// Will use the <see cref="Ped"/>'s head orientation for perception tests.
        /// </summary>
        UseHeadOrientationForPerception,
        /// <summary>
        /// This <see cref="Ped"/> is running a driveby gun or projectile task.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_IsDoingDriveby</c>`.
        /// </remarks>
        IsDoingDriveBy,
        /// <summary>
        /// This <see cref="Ped"/> is running a cover entry task.
        /// </summary>
        IsEnteringCover,
        /// <summary>
        /// Set to make `<c>CStaticMovementScanner::Scan()</c>` check for collisions as if the <see cref="Ped"/> is
        /// moving.
        /// </summary>
        ForceMovementScannerCheck,
        /// <summary>
        /// If <see langword="true"/>, the player will no longer auto-ragdoll when colliding with objects,
        /// <see cref="Ped"/>s, etc. while jumping.
        /// </summary>
        DisableJumpRagdollOnCollision,
        /// <summary>
        /// Set on the target <see cref="Ped"/> in melee if the player is homing towards them.
        /// </summary>
        IsBeingMeleeHomedByPlayer,
        /// <summary>
        /// This <see cref="Ped"/> should launch the bicycle this frame.
        /// </summary>
        ShouldLaunchBicycleThisFrame,
        /// <summary>
        /// This <see cref="Ped"/> can do a bicycle wheelie.
        /// </summary>
        CanDoBicycleWheelie,
        /// <summary>
        /// If <see langword="true"/>, `<c>ProcessPhysics()</c>` will execute completely for each physics simulation
        /// step.
        /// </summary>
        ForceProcessPhysicsUpdateEachSimStep,
        /// <summary>
        /// Disables collision between the <see cref="Ped"/> capsule and the map (useful in cases where the entity
        /// position is being tightly controlled outside of physics, e.g. by an animation).
        /// </summary>
        DisablePedCapsuleMapCollision,
        /// <summary>
        /// If <see langword="true"/>, motion in vehicle task won't shuffle to the driver seat just because the driver
        /// is injured.
        /// </summary>
        DisableSeatShuffleDueToInjuredDriver,
        DisableParachuting,
        /// <summary>
        /// Tells `<c>CGameWorld::ProcessAfterAllMovement()</c>` to call `<c>ProcessPostMovement</c>` for tasks on
        /// the <see cref="Ped"/>.
        /// </summary>
        ProcessPostMovement,
        /// <summary>
        /// Tells the <see cref="Ped"/> to call `<c>ProcessPostCamera</c>` for tasks.
        /// </summary>
        ProcessPostCamera,
        /// <summary>
        /// Tells the <see cref="Ped"/> to call `<c>ProcessPostPreRender</c>` for tasks.
        /// </summary>
        ProcessPostPreRender,
        PreventBicycleFromLeaningOver,
        KeepParachutePackOnAfterTeleport,
        /// <summary>
        /// The player <see cref="Ped"/> will use the new front melee logic.
        /// </summary>
        DontRaiseFistsWhenLockedOn,
        /// <summary>
        /// This will prefer all melee hit reactions to use body IK hit reactions if ragdoll is not selected.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_PreferMeleeBodyIkHitReaction</c>`.
        /// </remarks>
        PreferMeleeBodyIKHitReaction,
        /// <summary>
        /// Indicates the <see cref="Ped"/> needs to call process physics for motion tasks this frame.
        /// </summary>
        ProcessPhysicsTasksMotion,
        /// <summary>
        /// Indicates the <see cref="Ped"/> needs to call process physics for movement tasks this frame.
        /// </summary>
        ProcessPhysicsTasksMovement,
        /// <summary>
        /// If set, disables friendly responses to gunshots/being aimed at.
        /// </summary>
        DisableFriendlyGunReactAudio,
        DisableAgitationTriggers,
        /// <summary>
        /// If set, force `<c>CTaskReactAndFlee</c>` to use a forward facing flee transition.
        /// </summary>
        ForceForwardTransitionInReactAndFlee,
        /// <summary>
        /// `<c>TASK_ENTER_VEHICLE</c>`
        /// </summary>
        IsEnteringVehicle,
        /// <summary>
        /// If set the <see cref="Ped"/> will not allow the NavMeshTracker update (where the NavMeshTracker is
        /// a `<c>CNavMeshTrackedObject</c>`) to be skipped this frame.
        /// </summary>
        DoNotSkipNavMeshTrackerUpdate,
        /// <summary>
        /// Set to <see langword="true"/> when the ragdoll is lying on top of a <see cref="Vehicle"/> (note: hands,
        /// feet, forearms and shins are not included in the test).
        /// </summary>
        RagdollOnVehicle,
        BlockRagdollActivationInVehicle,
        /// <summary>
        /// If set, disable NM reactions to fast moving water.
        /// </summary>
        DisableNMForRiverRapids,
        /// <summary>
        /// If set, the <see cref="Ped"/> is on the ground writhing and might start shooting from ground.
        /// </summary>
        IsInWrithe,
        /// <summary>
        /// If set, the <see cref="Ped"/> will not go into the still in vehicle pose.
        /// </summary>
        PreventGoingIntoStillInVehicleState,
        /// <summary>
        /// If set, the <see cref="Ped"/> will get in and out of <see cref="Vehicle"/>s faster.
        /// </summary>
        UseFastEnterExitVehicleRates,
        /// <summary>
        /// If set, the <see cref="Ped"/> will not attach to a ground physical when his physics disables (useful for
        /// cutscenes, etc.).
        /// </summary>
        DisableGroundAttachment,
        DisableAgitation,
        DisableTalk,
        InterruptedToQuickStartEngine,
        PedEnteredFromLeftEntry,
        IsDiving,
        DisableVehicleImpacts,
        DeepVehicleImpacts,
        DisablePedCapsuleControl,
        /// <summary>
        /// Uses more expensive slope/stairs detection.
        /// </summary>
        UseProbeSlopeStairsDetection,
        DisableVehicleDamageReactions,
        DisablePotentialBlastReactions,
        /// <summary>
        /// When set alongside open door ik, will only use the left hand.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_OnlyAllowLeftArmDoorIk</c>`.
        /// </remarks>
        OnlyAllowLeftArmDoorIK,
        /// <summary>
        /// When set alongside open door ik, will only use the right hand.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_OnlyAllowRightArmDoorIk</c>`.
        /// </remarks>
        OnlyAllowRightArmDoorIK,
        /// <summary>
        /// When set, `<c>ProcessPedStanding</c>` will get called for each physics step.
        /// </summary>
        ForceProcessPedStandingUpdateEachSimStep,
        /// <summary>
        /// When set, the flashlight on an AI weapon will be turned off.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DisableFlashLight</c>`.
        /// </remarks>
        DisableFlashlight,
        /// <summary>
        /// When set, the <see cref="Ped"/> is doing a combat roll.
        /// </summary>
        DoingCombatRoll,
        DisableBodyRecoilSolver,
        /// <summary>
        /// When set, the <see cref="Ped"/> can abort the exit vehicle anim to go into fall.
        /// </summary>
        CanAbortExitForInAirEvent,
        DisableSprintDamage,
        /// <summary>
        /// When set, the AI <see cref="Ped"/> will enable their flashlight.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_ForceEnableFlashLightForAI</c>`.
        /// </remarks>
        ForceEnableFlashlightForAI,
        IsDoingCoverAimIntro,
        IsAimingFromCover,
        /// <summary>
        /// This <see cref="Ped"/> is waiting for a path request which is now complete, so their tasks must be updated
        /// this frame.
        /// </summary>
        WaitingForCompletedPathRequest,
        DisableCombatAudio,
        DisableCoverAudio,
        PreventBikeFromLeaning,
        InCoverTaskActive,
        /// <summary>
        /// Pushes the <see cref="Ped"/> through the same steep slope tolerances in `<c>CTaskMotionBase</c>` that
        /// the player encounters.
        /// </summary>
        EnableSteepSlopePrevention,
        InsideEnclosedSearchRegion,
        JumpingOutOfVehicle,
        IsTuckedOnBicycleThisFrame,
        /// <summary>
        /// Parallel flag to `<c>ProcessPostMovement</c>`, except this is reset in `<c>PreTask()</c>`, meaning it stays
        /// consistent across timeslicing.
        /// </summary>
        ProcessPostMovementTimeSliced,
        /// <summary>
        /// Player has to press and hold dive button to dive in water.
        /// </summary>
        EnablePressAndReleaseDives,
        /// <summary>
        /// Only allows player to exit vehicle when button is released rather than pressed or held.
        /// </summary>
        OnlyExitVehicleOnButtonRelease,
        IsGoingToStandOnExitedVehicle,
        BlockRagdollFromVehicleFallOff,
        DisableTorsoVehicleSolver,
        IsExitingUpsideDownVehicle,
        IsExitingOnsideVehicle,
        IsExactStopping,
        IsExactStopSettling,
        IsTrainCrushingRagdoll,
        /// <summary>
        /// Scales the <see cref="Ped"/>'s hair down to the specified value.
        /// </summary>
        OverrideHairScale,
        /// <summary>
        /// Considered as a threat as part of player cover search even if they can't see the player.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_ConsiderAsPlayerCoverThreatWithoutLOS</c>`.
        /// </remarks>
        ConsiderAsPlayerCoverThreatWithoutLos,
        /// <summary>
        /// Disables the <see cref="Ped"/> from using custom AI cover entry animations.
        /// </summary>
        BlockCustomAIEntryAnims,
        IgnoreVehicleEntryCollisionTests,
        /// <summary>
        /// Stream action mode anims even if action mode is disabled.
        /// </summary>
        StreamActionModeAnimsIfDisabled,
        /// <summary>
        /// <see cref="Ped"/> requires ragdoll matrix update next frame.
        /// </summary>
        ForceUpdateRagdollMatrix,
        /// <summary>
        /// If set, the <see cref="Ped"/> will not go into the shunt in vehicle pose.
        /// </summary>
        PreventGoingIntoShuntInVehicleState,
        DisableIndependentMoverFrame,
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DoingDrivebyOutro</c>`.
        /// </remarks>
        DoingDriveByOutro,
        BeingElectrocuted,
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DisableUnarmedDrivebys</c>`.
        /// </remarks>
        DisableUnarmedDriveBys,
        TalkingToPlayer,
        /// <summary>
        /// Block ragdoll activations from animated player bumps.
        /// </summary>
        DontActivateRagdollFromPlayerPedImpactReset,
        /// <summary>
        /// Block ragdoll activations from collisions with an AI ragdoll.
        /// </summary>
        DontActivateRagdollFromAiRagdollImpactReset,
        /// <summary>
        /// Block ragdoll activations from collisions with a player ragdoll.
        /// </summary>
        DontActivateRagdollFromPlayerRagdollImpactReset,
        /// <summary>
        /// If set, prevents visemes from playing any additive body animations.
        /// </summary>
        DisableVisemeBodyAdditive,
        /// <summary>
        /// Set when the player capsule is pushing up against this <see cref="Ped"/>s capsule.
        /// </summary>
        CapsuleBeingPushedByPlayerCapsule,
        ForceActionMode,
        ForceUnarmedActionMode,
        /// <summary>
        /// Set when the players capsule is getting repositioned each frame relative to an anim and origin.
        /// </summary>
        UsingMoverExtraction,
        BeingJacked,
        /// <summary>
        /// If set, turn on the voice driven mouth movement.
        /// </summary>
        EnableVoiceDrivenMouthMovement,
        IsReloading,
        /// <summary>
        /// <see cref="Ped"/> should use tighter (shorter) settings for entering <see cref="Vehicle"/>s.
        /// </summary>
        UseTighterEnterVehicleSettings,
        /// <summary>
        /// Set when the player is in the race mode.
        /// </summary>
        InRaceMode,
        /// <summary>
        /// Disable ambient (initial) melee moves.
        /// </summary>
        DisableAmbientMeleeMoves,
        ForceBuoyancyProcessingIfAsleep,
        /// <summary>
        /// Allows the player to trigger the special ability while in a <see cref="Vehicle"/>.
        /// </summary>
        AllowSpecialAbilityInVehicle,
        /// <summary>
        /// Prevents <see cref="Ped"/> from doing in vehicle actions such as starting engine, hotwiring,
        /// closing door etc.
        /// </summary>
        DisableInVehicleActions,
        /// <summary>
        /// Forces <see cref="Ped"/> to blend in steering wheel ik instantly rather than over time.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_ForceInstantSteeringWheelIkBlendIn</c>`.
        /// </remarks>
        ForceInstantSteeringWheelIKBlendIn,
        /// <summary>
        /// Ignores the bonus score for selecting cover that the player can engage the enemy at.
        /// </summary>
        IgnoreThreatEngagePlayerCoverBonus,
        /// <summary>
        /// Blocks triggering of 180 turns in human locomotion this frame.
        /// </summary>
        Block180Turns,
        /// <summary>
        /// Prevents the <see cref="Ped"/> from closing the vehicle door of the car they're in.
        /// </summary>
        DontCloseVehicleDoor,
        /// <summary>
        /// Map collision will not block this <see cref="Ped"/> from being hit by explosions.
        /// </summary>
        SkipExplosionOcclusion,
        /// <summary>
        /// Parallel flag to ProcessPhysicsTasks, except this is reset in PreTask(), meaning it stays consistent across
        /// timeslicing.
        /// </summary>
        ProcessPhysicsTasksTimeSliced,
        /// <summary>
        /// Set when the <see cref="Ped"/> has performed a melee strike and hit any non-<see cref="Ped"/> material.
        /// </summary>
        MeleeStrikeAgainstNonPed,
        /// <summary>
        /// We will not attempt to walk around doors when using arm IK.
        /// </summary>
        IgnoreNavigationForDoorArmIK,
        /// <summary>
        /// Disable aiming while parachuting.
        /// </summary>
        DisableAimingWhileParachuting,
        /// <summary>
        /// Disable hit reaction due to colliding with a <see cref="Ped"/>.
        /// </summary>
        DisablePedCollisionWithPedEvent,
        /// <summary>
        /// Will ignore the vehicle speed threshold and close the door anyway.
        /// </summary>
        IgnoreVelocityWhenClosingVehicleDoor,
        SkipOnFootIdleIntro,
        /// <summary>
        /// Don't walk round objects that the <see cref="Ped"/> collide with whilst moving.
        /// </summary>
        DontWalkRoundObjects,
        DisablePedEnteredMyVehicleEvents,
        /// <summary>
        /// Call `<c>ProcessLeftHandGripIk()</c>` to cancel left hand grip IK, at the right time of the frame.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_CancelLeftHandGripIk</c>`.
        /// </remarks>
        CancelLeftHandGripIK,
        /// <summary>
        /// Will keep reset the static counter when this is set.
        /// </summary>
        ResetMovementStaticCounter,
        /// <summary>
        /// Will allow <see cref="Ped"/> variations to be rendered in <see cref="Vehicle"/>s, even if marked otherwise.
        /// </summary>
        DisableInVehiclePedVariationBlocking,
        /// <summary>
        /// When on a mission this reset flag will slightly reduce the amount of time the player loses control of their
        /// <see cref="Vehicle"/> when hit by an AI <see cref="Ped"/>.
        /// </summary>
        ReduceEffectOfVehicleRamControlLoss,
        /// <summary>
        /// Another flag to disable friendly attack from the player. Set on the opponent you would like it to be
        /// disabled on.
        /// </summary>
        DisablePlayerMeleeFriendlyAttacks,
        MotionPedDoPostMovementIndependentMover,
        /// <summary>
        /// Set when the melee target has been deemed unreachable (AI only).
        /// </summary>
        IsMeleeTargetUnreachable,
        DisableAutoForceOutWhenBlowingUpCar,
        ThrowingProjectile,
        /// <summary>
        /// Scales the <see cref="Ped"/>'s hair up to the specified value.
        /// </summary>
        OverrideHairScaleLarger,
        /// <summary>
        /// Disables ambient dust off animations.
        /// </summary>
        DisableDustOffAnims,
        /// <summary>
        /// This <see cref="Ped"/> will refrain from ever performing a melee hit reaction.
        /// </summary>
        DisableMeleeHitReactions,
        /// <summary>
        /// Blocks viseme anims audio from playing.
        /// </summary>
        VisemeAnimsAudioBlocked,
        /// <summary>
        /// This overrides <see cref="HelmetPropFlags.NotInCar"/> set on any head prop and stops it from being removed
        /// when getting into the <see cref="Vehicle"/>.
        /// </summary>
        AllowHeadPropInVehicle,
        IsInVehicleChase,
        DontQuitMotionAiming,
        /// <summary>
        /// Ensure that the last bound matrices are only updated once per frame.
        /// </summary>
        SetLastBoundMatricesDone,
        /// <summary>
        /// Don't allow the locomotion task to adjust angular velocity coming from animation.
        /// </summary>
        PreserveAnimatedAngularVelocity,
        /// <summary>
        /// Set if the <see cref="Ped"/> should enable open door arm IK.
        /// </summary>
        OpenDoorArmIK,
        /// <summary>
        /// Script set flag, to force use of tighter turn settings in locomotion task.
        /// </summary>
        UseTighterTurnSettingsForScript,
        /// <summary>
        /// Set if the <see cref="Ped"/> should process externally driven DOFs before the pre-camera aAI update.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_ForcePreCameraProcessExternallyDrivenDOFs</c>`.
        /// </remarks>
        ForcePreCameraProcessExternallyDrivenDofs,
        /// <summary>
        /// Ped is waiting for ladder and blocking movement to prevent falling off.
        /// </summary>
        LadderBlockingMovement,
        /// <summary>
        /// If set, turn off the voice driven mouth movement (overrides <see cref="EnableVoiceDrivenMouthMovement"/>).
        /// </summary>
        DisableVoiceDrivenMouthMovement,
        /// <summary>
        /// If set, steer into skids while driving.
        /// </summary>
        SteerIntoSkids,
        /// <summary>
        /// When set, code will ignore the logic that requires the <see cref="Ped"/> to be in
        /// `<c>CTaskHumanLocomotion::State_Moving</c>`.
        /// </summary>
        AllowOpenDoorIkBeforeFullMovement,
        /// <summary>
        /// When set, code will bypass rel settings and allow a homing lock on to this <see cref="Ped"/> when they are
        /// in a <see cref="Vehicle"/>.
        /// </summary>
        AllowHomingMissileLockOnInVehicle,
        AllowCloneForcePostCameraAIUpdate,
        /// <summary>
        /// Force the high heels DOF to be 0.
        /// </summary>
        DisableHighHeels,
        /// <summary>
        /// Force lock on to break for this <see cref="Ped"/>.
        /// </summary>
        BreakTargetLock,
        /// <summary>
        /// Player does not get tired when sprinting.
        /// </summary>
        DontUseSprintEnergy,

        // `CPED_RESET_FLAG_DontChangeHorseMbr` reserves 354, but that reset flag is not queried in any of that source
        // code that aren't involved in horses at all, and we're sure the flag won't be really queried in
        // the game code of GTA V forever.

        /// <summary>
        /// Don't be damaged by touching dangerous material (e.g. electric generator).
        /// </summary>
        DisableMaterialCollisionDamage = 355,
        /// <summary>
        /// Don't target friendly players in MP.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DisableMPFriendlyLockon</c>`.
        /// </remarks>
        DisableMPFriendlyLockOn,
        /// <summary>
        /// Don't melee kill friendly players in MP.
        /// </summary>
        DisableMPFriendlyLethalMeleeActions,
        /// <summary>
        /// If our leader stops, try and seek cover if the <see cref="Ped"/> can.
        /// </summary>
        IfLeaderStopsSeekCover,
        /// <summary>
        /// Indicates that the <see cref="Ped"/> needs to call `<c>ProcessPostPreRenderAfterAttachments</c>` on their
        /// `<c>CPedIntelligence</c>` for tasks.
        /// </summary>
        ProcessPostPreRenderAfterAttachments,
        DoDamageCoughFacial,

        // `CPED_RESET_FLAG_IsUsingJetpack` reserves 361, but the `ENABLE_JETPACK` macro is defined as 0 in
        // final/production builds for any platforms. No `CJetpack` with a `CObject`, no `CJetpackManager`. Period.

        /// <summary>
        /// Use Interior capsule settings.
        /// </summary>
        UseInteriorCapsuleSettings = 362,
        /// <summary>
        /// <see cref="Ped"/> is closing a vehicle door.
        /// </summary>
        IsClosingVehicleDoor,
        /// <summary>
        /// Disable lerping the <see cref="Ped"/> towards the desired heading in the locomotion idle.
        /// </summary>
        DisableIdleExtraHeadingChange,
        /// <summary>
        /// Only allows vehicle weapons to be selected in `<c>CPedWeaponSelector::SelectWeapon</c>`.
        /// </summary>
        OnlySelectVehicleWeapons,
        /// <summary>
        /// Set in `<c>CTaskEnterVehicle::SetPedInSeat_OnEnter</c>` if <see cref="Ped"/> is warping into
        /// a <see cref="Vehicle"/> in multiplayer.
        /// </summary>
        IsWarpingIntoVehicleMP,
        /// <summary>
        /// Forces a <see cref="Ped"/> to remove its helmet.
        /// </summary>
        RemoveHelmet,
        /// <summary>
        /// Returns <see langword="true"/> if <see cref="Ped"/> is removing its helmet.
        /// </summary>
        IsRemovingHelmet,
        GestureAnimsBlockedFromScript,
        /// <summary>
        /// Disable all attempts by this <see cref="Ped"/> to ragdoll.
        /// </summary>
        NeverRagdoll,
        /// <summary>
        /// Disable stuck wall hit animation for the <see cref="Ped"/> this frame.
        /// </summary>
        DisableWallHitAnimation,
        /// <summary>
        /// Play agitated anims in <see cref="Vehicle"/> - overrides normal sit idle.
        /// </summary>
        PlayAgitatedAnimsInVehicle,
        /// <summary>
        /// Returns <see langword="true"/> if <see cref="Ped"/> is shuffling seat.
        /// </summary>
        IsSeatShuffling,
        /// <summary>
        /// <see langword="true"/> if the <see cref="Ped"/> are running `<c>TASK_AIM_AND_THROW_PROJECTILE</c>` as
        /// a subtask of `<c>TASK_AIM_GUN_ON_FOOT</c>`.
        /// </summary>
        IsThrowingProjectileWhileAiming,
        /// <summary>
        /// Set by script command `<c>DISABLE_PLAYER_THROW_GRENADE_WHILE_USING_GUN</c>`.
        /// </summary>
        DisableProjectileThrowsWhileAimingGun,
        /// <summary>
        /// Allows <see cref="Ped"/> in any seat to control radio in multiplayer.
        /// </summary>
        AllowControlRadioInAnySeatInMP,
        /// <summary>
        /// Blocks <see cref="Ped"/> from manually transforming spycar to/from car/sub modes.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DisableSpycarTransformation</c>`.
        /// </remarks>
        DisableSpyCarTransformation,
        /// <summary>
        /// Prevent CTaskQuadLocomotion from blending in idle turns, regardless of desired/current heading
        /// differential.
        /// </summary>
        BlockQuadLocomotionIdleTurns,
        /// <summary>
        /// Blocks <see cref="Ped"/> from headbobbing to radio music in <see cref="Vehicle"/>s.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_BlockHeadbobbingToRadio</c>`.
        /// </remarks>
        BlockHeadBobbingToRadio,
        /// <summary>
        /// Allows us to load and play idle fidgets in `<c>TaskMotionAiming</c>`.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_PlayFPSIdleFidgets</c>`.
        /// </remarks>
        PlayFpsIdleFidgets,
        /// <summary>
        /// When putting a <see cref="Ped"/> directly into cover, the <see cref="Ped"/> will blend in the new cover
        /// anims slowly to prevent a pose pop.
        /// </summary>
        ForceExtraLongBlendInForPedSkipIdleCoverTransition,
        /// <summary>
        /// <see langword="true"/> if FPS idle fidgets are blending out.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_BlendingOutFPSIdleFidgets</c>`.
        /// </remarks>
        BlendingOutFpsIdleFidgets,
        DisableMotionBaseVelocityOverride,
        /// <summary>
        /// Set to <see langword="true"/> when the <see cref="Ped"/> is pressing forward on the left stick in FPS mode so they switch from
        /// Aiming to Swimmimg/Diving motion tasks.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_FPSSwimUseSwimMotionTask</c>`.
        /// </remarks>
        FpsSwimUseSwimMotionTask,
        /// <summary>
        /// Set to <see langword="true"/> when the <see cref="Ped"/> is strafing in water in FPS mode so the <see cref="Ped"/> use the motion aiming task.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_FPSSwimUseAimingMotionTask</c>`.
        /// </remarks>
        FpsSwimUseAimingMotionTask,
        /// <summary>
        /// Set when <see cref="Ped"/> has decided to fire weapon when ready, used in FPS mode.
        /// </summary>
        FiringWeaponWhenReady,
        /// <summary>
        /// <see langword="true"/> if the blind fire task is running.
        /// </summary>
        IsBlindFiring,
        /// <summary>
        /// <see langword="true"/> if the <see cref="Ped"/> is peeking in cover.
        /// </summary>
        IsPeekingFromCover,
        /// <summary>
        /// <see langword="true"/> to bail out of ProcessPreComputeImpacts.
        /// </summary>
        TaskSkipProcessPreComputeImpacts,
        /// <summary>
        /// Don't ever try to lock on to this <see cref="Ped"/> with cinematic aim.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DisableAssistedAimLockon</c>`.
        /// </remarks>
        DisableAssistedAimLockOn,
        /// <summary>
        /// To control enabling of FPS aim IK while using a projectile until it is ready.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_FPSAllowAimIKForThrownProjectile</c>`.
        /// </remarks>
        FpsAllowAimIKForThrownProjectile,
        TriggerRoadRageAnim,
        /// <summary>
        /// Force a pre camera AI and animation update if the <see cref="Ped"/> is the first person camera target
        /// during the pre camera update.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_ForcePreCameraAiAnimUpdateIfFirstPerson</c>`.
        /// </remarks>
        ForcePreCameraAIAnimUpdateIfFirstPerson,
        /// <summary>
        /// Any <see cref="Ped"/> this is set on won't register damage from collisions against other
        /// <see cref="Ped"/>s.
        /// </summary>
        NoCollisionDamageFromOtherPeds,
        /// <summary>
        /// Block camera view mode switching.
        /// </summary>
        BlockCameraSwitching,
        /// <summary>
        /// Negate the capsule's preference for ragdoll triggering death on this <see cref="Ped"/>.
        /// </summary>
        NeverDieFromCapsuleRagdollSettings,
        /// <summary>
        /// <see cref="Ped"/> is in contact with `<c>GTA_DEEP_SURFACE_TYPE</c>` bounds
        /// </summary>
        InContactWithDeepSurface,
        DontSuppressUseNavMeshToNavigateToVehicleDoorWhenVehicleInWater,
        /// <summary>
        /// Add on the <see cref="Ped"/>'s velocity to the projectile's initial velocity.
        /// </summary>
        IncludePedReferenceVelocityWhenFiringProjectiles,
        IsDoingCoverOutroToPeek,
        InstantBlendToAimNoSettle,
        /// <summary>
        /// Force a pre camera animation update if the <see cref="Ped"/> is the first person camera target during
        /// the pre camera update.
        /// </summary>
        ForcePreCameraAnimUpdate,
        /// <summary>
        /// Disables <see cref="HelmetPropFlags.HideInFirstPerson"/> from culling the prop in
        /// `<c>CPedPropsMgr::RenderPropsInternal</c>`.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DisableHelmetCullFPS</c>`.
        /// </remarks>
        DisableHelmetCullFps,
        ShouldIgnoreCoverAutoHeadingCorrection,
        DisableReticuleInCoverThisFrame,
        ForceScriptedCameraLowCoverAngleWhenEnteringCover,
        DisableCameraConstraintFallBackThisFrame,
        /// <summary>
        /// Disables FPS arm IK in `<c>CTaskPlayerOnFoot::IsStateValidForFPSIK</c>`.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DisableFPSArmIK</c>`.
        /// </remarks>
        DisableFpsArmIK,
        /// <summary>
        /// Turn off right arm IK during cover outros in FPS mode when set.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DisableRightArmIKInCoverOutroFPS</c>`.
        /// </remarks>
        DisableRightArmIKInCoverOutroFps,
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DoFPSSprintBreakOut</c>`.
        /// </remarks>
        DoFpsSprintBreakOut,
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DoFPSJumpBreakOut</c>`.
        /// </remarks>
        DoFpsJumpBreakOut,
        IsExitingCover,
        /// <summary>
        /// <see langword="true"/> if running `<c>CTaskWeaponBlocked</c>` in FPS mode.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_WeaponBlockedInFPSMode</c>`.
        /// </remarks>
        WeaponBlockedInFpsMode,
        PoVCameraConstrained,
        /// <summary>
        /// Set by <see cref="TaskInvoker.ClearAllImmediately()"/>.
        /// </summary>
        ScriptClearingPedTasks,
        /// <summary>
        /// <see cref="Ped"/> was jumping on foot with projectile in hand.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_WasFPSJumpingWithProjectile</c>`.
        /// </remarks>
        WasFpsJumpingWithProjectile,
        DisableMeleeWeaponSelection,
        /// <summary>
        /// Slow for corners more aggressively for waypoint playback.
        /// </summary>
        WaypointPlaybackSlowMoreForCorners,
        /// <summary>
        /// <see langword="true"/> while placing a projectile in FPS mode.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_FPSPlacingProjectile</c>`.
        /// </remarks>
        FpsPlacingProjectile,
        /// <summary>
        /// <see cref="Ped"/> will use bullet penetration code when glass material is hit.
        /// </summary>
        UseBulletPenetrationForGlass,
        /// <summary>
        /// Doing a floor plant with a bomb in FPS mode.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_FPSPlantingBombOnFloor</c>`.
        /// </remarks>
        FpsPlantingBombOnFloor,
        /// <summary>
        /// Don't do FPS Aim intro.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_ForceSkipFPSAimIntro</c>`.
        /// </remarks>
        ForceSkipFpsAimIntro,
        /// <summary>
        /// If set on a <see cref="Ped"/> then they are allowed to be pinned by bullets from friendly
        /// <see cref="Ped"/>s.
        /// </summary>
        CanBePinnedByFriendlyBullets,
        /// <summary>
        /// Turn off left arm IK during cover outros in FPS mode when set.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_DisableLeftArmIKInCoverOutroFPS</c>`.
        /// </remarks>
        DisableLeftArmIKInCoverOutroFps,
        /// <summary>
        /// Blocks road blocks with spike strips from spawning if set on the local player <see cref="Ped"/>.
        /// </summary>
        DisableSpikeStripRoadBlocks,
        /// <summary>
        /// Skip aim unholster transition.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_SkipFPSUnHolsterTransition</c>`.
        /// </remarks>
        SkipFpsUnholsterTransition,
        /// <summary>
        /// Trigger the put down helmet fx.
        /// </summary>
        PutDownHelmetFX,
        /// <summary>
        /// Peds marked with this flag will only be able to be hit by the player if the player explicitly presses
        /// the melee button.
        /// </summary>
        IsLowerPriorityMeleeTarget,
        /// <summary>
        /// Disable timeslicing of event scanning.
        /// </summary>
        ForceScanForEventsThisFrame,
        /// <summary>
        /// Set this flag to disable priming when the projectile task starts up until the attack trigger is released
        /// and pressed again.
        /// </summary>
        StartProjectileTaskWithPrimingDisabled,
        /// <summary>
        /// Set if some game code want to perform a second AI/anim update when switching between first person/third person.
        /// </summary>
        /// <remarks>
        /// <para>
        /// `<c>camManager::Update</c>` tests whether the flag is set, and `<c>CTaskCombatRoll::CleanUp()</c>` sets this flag.
        /// </para>
        /// <para>
        /// The canonical name is `<c>CPED_RESET_FLAG_CheckFPSSwitchInCameraUpdate</c>`.
        /// </para>
        /// </remarks>
        CheckFpsSwitchInCameraUpdate,
        /// <summary>
        /// Force <see cref="Ped"/> to auto-equip a helmet on entering aircraft. Overrides
        /// <see cref="PedConfigFlagToggles.DisableAutoEquipHelmetsInAircraft"/> (set in the interaction menu in
        /// GTA Online).
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_ForceAutoEquipHelmetsInAicraft</c>`.
        /// </remarks>
        ForceAutoEquipHelmetsInAircraft,
        /// <summary>
        /// Flag used by replay editor to disable recording specified remote players.
        /// </summary>
        BlockRemotePlayerRecording,
        /// <summary>
        /// Indicates something inflicted damage to the <see cref="Ped"/> in a damage event this frame.
        /// </summary>
        /// <remarks>
        /// `<c>CPedDamageCalculator::ApplyDamageAndComputeResponse</c>` sets this flag only when it processed
        /// a damage response with positive lost in which the victim is the <see cref="Ped"/> this frame.
        /// </remarks>
        InflictedDamageThisFrame,
        /// <summary>
        /// Allow FPS vehicle anims even if FPS camera isn't dominant.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_UseFirstPersonVehicleAnimsIfFPSCamNotDominant</c>`.
        /// </remarks>
        UseFirstPersonVehicleAnimsIfFpsCamNotDominant,
        /// <summary>
        /// Puts the <see cref="Ped"/> in a standing pose on the jetski.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_ForceIntoStandPoseOnJetski</c>`.
        /// </remarks>
        ForceIntoStandPoseOnJetSki,
        /// <summary>
        /// <see cref="Ped"/> is located inside an air defence sphere.
        /// </summary>
        InAirDefenceSphere,
        /// <summary>
        /// This will suppress all takedown melee actions (`<c>RA_IS_TAKEDOWN</c>` or `<c>RA_IS_STEALTH_KILL</c>`,
        /// defined in `<c>`action_table.meta</c>`).
        /// </summary>
        SuppressTakedownMeleeActions,
        /// <summary>
        /// Inverts lookaround controls (right stick / mouse) for this player, for this frame.
        /// </summary>
        InvertLookAroundControls,
        /// <summary>
        /// Allows attacking <see cref="Ped"/> to engage another entity without waiting for its turn (if there's
        /// multiple attackers).
        /// </summary>
        IgnoreCombatManager,
        /// <summary>
        /// Check if there is an active camera blending and use the blended camera frame when compute the FPS camera
        /// relative matrix.
        /// </summary>
        UseBlendedCamerasOnUpdateFpsCameraRelativeMatrix,
        /// <summary>
        /// Forces the <see cref="Ped"/> to perform a dodge and a counter move if it's attacked.
        /// </summary>
        ForceMeleeCounter,
        /// <summary>
        /// Indicates that <see cref="Ped"/> was hit by vehicle melee attack.
        /// </summary>
        WasHitByVehicleMelee,
        /// <summary>
        /// Don't allow <see cref="Ped"/> to use navmesh when navigating in `<c>CTaskEnterVehicle</c>`.
        /// Will use goto-point or bail if they cannot use that.
        /// </summary>
        SuppressNavmeshForEnterVehicleTask,
        DisableShallowWaterBikeJumpOutThisFrame,
        /// <summary>
        /// Disables player combat rolling.
        /// </summary>
        DisablePlayerCombatRoll,
        /// <summary>
        /// Will ignore safe position check on detaching the <see cref="Ped"/>.
        /// </summary>
        IgnoreDetachSafePositionCheck,
        /// <summary>
        /// Prevents the more forgiving MP ladder detection settings from being used, and forces SP settings.
        /// </summary>
        DisableEasyLadderConditions,
        /// <summary>
        /// Makes local player ignore certain scenario spawn restrictions on scenarios that respect this flag.
        /// </summary>
        PlayerIgnoresScenarioSpawnRestrictions,
        /// <summary>
        /// Indicates player is using Drone from Battle DLC.
        /// </summary>
        UsingDrone,
        /// <summary>
        /// Will force the player that killed this <see cref="Ped"/> to get wanted level,
        /// even if he wouldn't otherwise.
        /// </summary>
        ForceWantedLevelWhenKilled,
        /// <summary>
        /// Will use scripted firing position on the clones of this <see cref="Ped"/> on other machines.
        /// </summary>
        UseScriptedWeaponFirePosition,
        /// <summary>
        /// Enable collision on player <see cref="Ped"/> network clones when physics is fixed.
        /// </summary>
        EnableCollisionOnNetworkCloneWhenFixed,
        /// <summary>
        /// Use extended logic for determining damage instigator for ragdoll collisions.
        /// </summary>
        UseExtendedRagdollCollisionCalculator,
        /// <summary>
        /// Prevent the player locking on to friendly players.
        /// </summary>
        /// <remarks>
        /// The canonical name is `<c>CPED_RESET_FLAG_PreventLockonToFriendlyPlayers</c>`.
        /// </remarks>
        PreventLockOnToFriendlyPlayers,
        /// <summary>
        /// Modifies <see cref="AnimationFlags.AbortOnPedMovement"/> to only trigger an abort when movement would be
        /// caused by player input.
        /// </summary>
        OnlyAbortScriptedAnimOnMovementByInput,
        /// <summary>
        /// Prevents stealth takedowns from being preformed on a <see cref="Ped"/>.
        /// </summary>
        PreventAllStealthKills,
        /// <summary>
        /// Prevent <see cref="Ped"/>s from entering a fall task if affected by explosion damage.
        /// </summary>
        BlockFallTaskFromExplosionDamage,
        /// <summary>
        /// Mimics the behaviour like with boss <see cref="Ped"/>s by holding the button for entering the rear seats.
        /// </summary>
        AllowPedRearEntry,
    }
}
