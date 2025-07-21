namespace GTA
{
    /// <summary>
    /// An enumeration of known config flags for <see cref="Ped"/> (represents `<c>ePedConfigFlags</c>`).
    /// </summary>
    /// <remarks>
    /// You can check if names of this enum are included in the exe by searching the dumped exe for hashed values of
    /// names like `<c>CPED_CONFIG_FLAG_[enum name]</c>` without case conversion
    /// (for example, search the dumped exe for <c>0x583B5E2D</c>, which is the hashed value of
    /// `<c>CPED_CONFIG_FLAG_AllowMedicsToReviveMe</c>`).
    /// </remarks>
    public enum PedConfigFlagToggles
    {
        CreatedByFactory,
        /// <summary>
        /// Script can stop <see cref="Ped"/>s being shot when driving <see cref="Vehicle"/> (including exposed ones
        /// like bikes).
        /// </summary>
        CanBeShotInVehicle,
        /// <summary>
        /// <see cref="Ped"/> cannot be killed by a single bullet.
        /// </summary>
        NoCriticalHits,
        /// <summary>
        /// Determines whether this <see cref="Ped"/> can drown in water by taking drown damage.
        /// Can be set with <see cref="Ped.DrownsInWater"/>.
        /// </summary>
        /// <remarks>
        /// Resets to <see langword="true"/> when the <see cref="Ped"/> is marked as no longer needed, which can be done
        /// with <see cref="Entity.MarkAsNoLongerNeeded()"/> unless <see cref="Ped"/> is running a task with the type
        /// <c>TASK_ON_FOOT_FISH</c>.
        /// </remarks>
        DrownsInWater,
        /// <summary>
        /// Determines whether this <see cref="Ped"/> can take damage whilst in a sinking <see cref="Vehicle"/>.
        /// Can be set with <see cref="Ped.DrownsInSinkingVehicle"/>.
        /// </summary>
        DrownsInSinkingVehicle,
        /// <summary>
        /// <see cref="Ped"/> cannot swim and will die as soon as they are required to swim.
        /// Can be set with <see cref="Ped.DiesInstantlyInWater"/>.
        /// </summary>
        /// <remarks>
        /// Resets to <see langword="false"/> when the <see cref="Ped"/> is marked as no longer needed, which can be
        /// done with <see cref="Entity.MarkAsNoLongerNeeded()"/> unless <see cref="Ped"/> is running a task with
        /// the type <c>TASK_ON_FOOT_FISH</c>.
        /// </remarks>
        DiesInstantlyWhenSwimming,
        /// <summary>
        /// If <see langword="true"/>, <see cref="Ped"/> will not take gun damage where the hit bone is
        /// <see cref="Bone.SkelSpine0"/>, <see cref="Bone.SkelSpine1"/>, <see cref="Bone.SkelSpine2"/>, or
        /// <see cref="Bone.SkelSpine3"/> (<c>BONETAG_SPINE0</c>, <c>BONETAG_SPINE1</c>, <c>BONETAG_SPINE2</c>, and
        /// <c>BONETAG_SPINE0</c> of bone tag names respectively).
        /// </summary>
        HasBulletProofVest,
        /// <summary>
        /// Force <see cref="Ped"/> to play only upper body damage anims from weapons.
        /// </summary>
        UpperBodyDamageAnimsOnly,
        /// <summary>
        /// <see cref="Ped"/> will never fall over on skis.
        /// </summary>
        NeverFallOffSkis,
        /// <summary>
        /// If <see langword="true"/>, the player cannot target the <see cref="Ped"/>.
        /// </summary>
        NeverEverTargetThisPed,
        /// <summary>
        /// If <see langword="true"/>, the player targetting system puts the fixed high score on the <see cref="Ped"/>
        /// as a scripted high priority target (though configurable  with <c>PrioScriptedHighPriority</c> in
        /// <c>pedtargetevaluator.ymt</c>), which will make the player will target them easier.
        /// </summary>
        ThisPedIsATargetPriority,
        /// <summary>
        /// If <see langword="true"/>, the player can target the <see cref="Ped"/> without line of sight to them.
        /// </summary>
        TargettableWithNoLos,
        /// <summary>
        /// script can set this so <see cref="Ped"/> will be in players group but not reacting to commands.
        /// </summary>
        DoesntListenToPlayerGroupCommands,
        /// <summary>
        /// If <see langword="true"/>, the <see cref="Ped"/> will not leave their <see cref="PedGroup"/> when
        /// the distance to the leader exceeds the configured separation range.
        /// </summary>
        NeverLeavesGroup,
        DoesntDropWeaponsWhenDead,
        /// <summary>
        /// When the <see cref="Ped"/>s delayed weapon finally loads, set it as the <see cref="Ped"/>s current weapon.
        /// </summary>
        SetDelayedWeaponAsCurrent,
        /// <summary>
        /// If <see langword="true"/> <see cref="Ped"/> will carry on with task even after script <see cref="Ped"/>
        /// cleanup.
        /// </summary>
        KeepTasksAfterCleanUp,
        /// <summary>
        /// Set to block any events that might interrupt the currently running tasks.
        /// Can be set with <see cref="Ped.BlockPermanentEvents"/>.
        /// </summary>
        BlockNonTemporaryEvents,
        HasAScriptBrain,
        WaitingForScriptBrainToLoad,
        /// <summary>
        /// If the <see cref="Ped"/> dies medics will be dispatched, <see langword="false"/> by default for mission
        /// <see cref="Ped"/>s, the <see cref="Ped"/> wont be attended.
        /// </summary>
        /// <remarks>
        /// Despite the "correct" enum name whose hash 0x583B5E2D (for `<c>CPED_CONFIG_FLAG_AllowMedicsToReviveMe</c>`)
        /// is present in the exe, medics cannot revive <see cref="Ped"/>s.
        /// </remarks>
        AllowMedicsToReviveMe,
        /// <summary>
        /// Script can give specific amount of money to <see cref="Ped"/> (script <see cref="Ped"/>s don't drop any
        /// money by default).
        /// </summary>
        MoneyHasBeenGivenByScript,
        /// <summary>
        /// Is this <see cref="Ped"/> allowed to crouch at all?
        /// </summary>
        NotAllowedToCrouch,
        /// <summary>
        /// Script command to control what type of pickups are created when <see cref="Ped"/> dies.
        /// </summary>
        DeathPickupsPersist,
        /// <summary>
        /// Script command so <see cref="Ped"/> does not stop to watch fights.
        /// </summary>
        IgnoreSeenMelee,
        /// <summary>
        /// Script command so missions <see cref="Ped"/>s die if injured.
        /// Can be set with <see cref="Ped.DiesOnLowHealth"/>.
        /// </summary>
        ForceDieIfInjured,
        /// <summary>
        /// Force this <see cref="Ped"/> cannot be carjacked.
        /// </summary>
        DontDragMeOutCar,
        /// <summary>
        /// Script sets this to keep <see cref="Ped"/>s in <see cref="Vehicle"/> when the player steals it.
        /// </summary>
        StayInCarOnJack,
        /// <summary>
        /// Don't fall out <see cref="Vehicle"/> if killed.
        /// </summary>
        ForceDieInCar,
        /// <summary>
        /// script can stop <see cref="Ped"/>s automatically getting out of <see cref="Vehicle"/> when it's upside down
        /// or undrivable (for races and stuff), defaults to <see langword="true"/>.
        /// </summary>
        GetOutUndriveableVehicle,
        /// <summary>
        /// Script can stop <see cref="Ped"/>s automatically leaving boats when they become random chars, after a script
        /// quits.
        /// </summary>
        WillRemainOnBoatAfterMissionEnds,
        /// <summary>
        /// Some <see cref="Ped"/>s (like mission <see cref="Ped"/>s) should not be stored as persistent.
        /// </summary>
        DontStoreAsPersistent,
        /// <summary>
        /// The <see cref="Ped"/> will fly through the <see cref="Vehicle"/> windscreen upon a forward impact at high
        /// velocity.
        /// </summary>
        WillFlyThroughWindscreen,
        DieWhenRagdoll,
        /// <summary>
        /// The <see cref="Ped"/> has a helmet (the PedHelmetComponent has put the helmet on the <see cref="Ped"/> via
        /// "put on" animations).
        /// </summary>
        HasHelmet,
        /// <summary>
        /// will the <see cref="Ped"/> try to put on their helmet?
        /// </summary>
        UseHelmet,
        /// <summary>
        /// The <see cref="Ped"/> will not take off their helmet (if equipped) while this is set.
        /// </summary>
        DontTakeOffHelmet,
        HideInCutscene,
        PedIsEnemyToPlayer,
        DisableEvasiveDives,
        /// <summary>
        /// Generates shocking events as if dead.
        /// </summary>
        PedGeneratesDeadBodyEvents,
        DontAttackPlayerWithoutWantedLevel,
        /// <summary>
        /// Can do any crime against this character and the cops turn a blind eye (no crime reported).
        /// </summary>
        DontInfluenceWantedLevel,
        /// <summary>
        /// If set on the local player <see cref="Ped"/>, the player cannot lock on any <see cref="Ped"/>s at all.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_DisablePlayerLockon</c>` in the exe, but this enum uses
        /// the corrected name.
        /// </remarks>
        DisablePlayerLockOn,
        /// <summary>
        /// In the multiplayer game, if set on a player <see cref="Ped"/>, they cannot lock on other non-player
        /// <see cref="Ped"/>s with the <see cref="EntityPopulationType"/> anything other than
        /// <see cref="EntityPopulationType.Mission"/>.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_DisableLockonToRandomPeds</c>` in the exe, but this enum uses
        /// the corrected name.
        /// </remarks>
        DisableLockOnToRandomPeds,
        /// <summary>
        /// In the multiplayer game, if set on a player <see cref="Ped"/>, they cannot lock on a friendly player
        /// <see cref="Ped"/>s.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_AllowLockonToFriendlyPlayers</c>` in the exe, but this enum uses
        /// the corrected name.
        /// </remarks>
        AllowLockOnToFriendlyPlayers,
        /// <summary>
        /// Disable the horn when the <see cref="Ped"/> dies in a <see cref="Vehicle"/> and has their head against
        /// the steering wheel.
        /// </summary>
        DisableHornAudioWhenDead,
        PedBeingDeleted,
        /// <summary>
        /// Disable weapon switching while this is set.
        /// </summary>
        BlockWeaponSwitching,
        /// <summary>
        /// Disable the behaviour which causes player-group <see cref="Ped"/>s to crouch when the player aims at them.
        /// </summary>
        BlockGroupPedAimedAtResponse,
        /// <summary>
        /// Basically defines whether group <see cref="Ped"/>s will use <see cref="Vehicle"/>s etc. to follow their
        /// leader (<see langword="false"/> by default).
        /// </summary>
        WillFollowLeaderAnyMeans,
        /// <summary>
        /// Set to <see langword="true"/> if the char has ever been blipped, not 100% correct so don't use it on
        /// anything sensitive.
        /// </summary>
        BlippedByScript,
        /// <summary>
        /// Draw this <see cref="Ped"/>s visual field in the stealth radar.
        /// </summary>
        DrawRadarVisualField,
        /// <summary>
        /// Set to <see langword="true"/> to stop the <see cref="Ped"/>s weapon firing on impact when they drop it.
        /// </summary>
        StopWeaponFiringOnImpact,
        /// <summary>
        /// Set to <see langword="true"/> to stop <see cref="Ped"/> scanning for things to fall off when shot by
        /// the player.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_DissableAutoFallOffTests</c>` in the exe, but this enum uses
        /// the corrected name.
        /// </remarks>
        DisableAutoFallOffTests,
        /// <summary>
        /// Forces <see cref="Ped"/>s to steer around dead bodies, the default is <see langword="false"/>.
        /// </summary>
        SteerAroundDeadBodies,
        /// <summary>
        /// Does not use for special handling in game code.
        /// </summary>
        ConstrainToNavMesh,
        /// <summary>
        /// Does not use for special handling in game code.
        /// </summary>
        SyncingAnimatedProps,
        IsFiring,
        WasFiring,
        IsStanding,
        /// <summary>
        /// Was the <see cref="Ped"/> standing last frame.
        /// </summary>
        WasStanding,
        InVehicle,
        OnMount,
        AttachedToVehicle,
        IsSwimming,
        /// <summary>
        /// Was the <see cref="Ped"/> swimming in water last frame.
        /// </summary>
        WasSwimming,
        IsSkiing,
        IsSitting,
        /// <summary>
        /// Determines if this <see cref="Ped"/> was killed by a stealth action.
        /// </summary>
        KilledByStealth,
        /// <summary>
        /// Determines if this <see cref="Ped"/> was killed by a takedown action.
        /// </summary>
        KilledByTakedown,
        /// <summary>
        /// Determines if this <see cref="Ped"/> was finished with a knockout action.
        /// </summary>
        /// <remarks>
        /// The original name is <c>CPED_CONFIG_FLAG_Knockedout</c> in the exe, but this enum uses the corrected name.
        /// </remarks>
        KnockedOut,
        /// <summary>
        /// So <see cref="Ped"/>s automatically given blips will clear them again when they die (mainly used for
        /// <see cref="Ped"/>s recruited into players group).
        /// </summary>
        ClearRadarBlipOnDeath,
        /// <summary>
        /// Train code uses this when grabbing random <see cref="Ped"/>s to get in trains.
        /// </summary>
        JustGotOffTrain,
        /// <summary>
        /// Train code uses this when grabbing random <see cref="Ped"/>s to get in trains.
        /// </summary>
        JustGotOnTrain,
        /// <summary>
        /// Set to <see langword="true"/> when a <see cref="Ped"/> is in process of using a cover point.
        /// </summary>
        UsingCoverPoint,
        IsInTheAir,
        /// <summary>
        /// Has <see cref="Ped"/> been knocked up into the air by a <see cref="Vehicle"/> collision.
        /// </summary>
        KnockedUpIntoAir,
        IsAimingGun,
        /// <summary>
        /// Used by navigation to force scan for <see cref="Vehicle"/>s collisions.
        /// </summary>
        HasJustLeftCar,
        TargetWhenInjuredAllowed,
        /// <summary>
        /// Footprint for left foot collision for NaturalMotion tasks (for the current frame).
        /// </summary>
        CurrLeftFootCollNM,
        /// <summary>
        /// Footprint for left foot collision for NaturalMotion tasks (for the previous frame).
        /// </summary>
        PrevLeftFootCollNM,
        /// <summary>
        /// Footprint for right foot collision for NaturalMotion tasks (for the current frame).
        /// </summary>
        CurrRightFootCollNM,
        /// <summary>
        /// Footprint for right foot collision for NaturalMotion tasks (for the previous frame).
        /// </summary>
        PrevRightFootCollNM,
        /// <summary>
        /// Has this <see cref="Ped"/> been bumped by a <see cref="Vehicle"/> while driving?
        /// </summary>
        HasBeenBumpedInCar,
        /// <summary>
        /// The in-water task has just quit in response to a ladder-climb request.
        /// </summary>
        InWaterTaskQuitToClimbLadder,
        /// <summary>
        /// When using physical 2-handed weapons, both hands have been latched to the gun model (used when sending
        /// ConfigureCharacter to NaturalMotion).
        /// </summary>
        NMTwoHandedWeaponBothHandsConstrained,
        CreatedBloodPoolTimer,
        DontActivateRagdollFromAnyPedImpact,
        GroupPedFailedToEnterCover,
        AlreadyChattedOnPhone,
        AlreadyReactedToPedOnRoof,
        /// <summary>
        /// Set by the script to force a <see cref="Ped"/> to load cover.
        /// </summary>
        ForcePedLoadCover,
        BlockCoweringInCover,
        BlockPeekingInCover,
        /// <summary>
        /// Set when a <see cref="Ped"/> exits a <see cref="Vehicle"/>. The first time they move, they should check for
        /// door obstructions.
        /// </summary>
        JustLeftCarNotCheckedForDoors,
        VaultFromCover,
        AutoConversationLookAts,
        /// <summary>
        /// Set to indicate that the <see cref="Ped"/>'s bounds are in the crouched configuration.
        /// </summary>
        UsingCrouchedPedCapsule,
        /// <summary>
        /// Whether this <see cref="Ped"/> has been investigated (for dead <see cref="Ped"/>s).
        /// </summary>
        HasDeadPedBeenReported,
        /// <summary>
        /// If set, the player <see cref="Ped"/> will always behave like we have the aim trigger pressed.
        /// </summary>
        ForcedAim,
        /// <summary>
        /// Enables/disables the low-level steering behaviour around <see cref="Ped"/>s and <see cref="Prop"/>s.
        /// </summary>
        SteersAroundPeds,
        /// <summary>
        /// Enables/disables the low-level steering behaviour around <see cref="Ped"/>s and <see cref="Prop"/>s.
        /// </summary>
        SteersAroundObjects,
        /// <summary>
        /// Set if the <see cref="Ped"/> should enable open door arm IK.
        /// </summary>
        OpenDoorArmIK,
        /// <summary>
        /// Force a reload of the current weapon.
        /// </summary>
        ForceReload,
        /// <summary>
        /// Blocks ragdoll activation when hit by a <see cref="Vehicle"/>.
        /// </summary>
        DontActivateRagdollFromVehicleImpact,
        /// <summary>
        /// Blocks ragdoll activation when hit by a bullet.
        /// </summary>
        DontActivateRagdollFromBulletImpact,
        /// <summary>
        /// Blocks ragdoll activation when hit by an explosive.
        /// </summary>
        DontActivateRagdollFromExplosions,
        /// <summary>
        /// Blocks ragdoll activation when set on fire.
        /// </summary>
        DontActivateRagdollFromFire,
        /// <summary>
        /// Blocks ragdoll activation when electrocuted.
        /// </summary>
        DontActivateRagdollFromElectrocution,
        /// <summary>
        /// Whether this <see cref="Ped"/> is being dragged to safety.
        /// </summary>
        IsBeingDraggedToSafety,
        /// <summary>
        /// Whether this <see cref="Ped"/> has been dragged to safety.
        /// </summary>
        HasBeenDraggedToSafety,
        /// <summary>
        /// Ignores the creation of the weapon <see cref="Prop"/> unless the gun is shot.
        /// </summary>
        /// <remarks>
        /// This flag does not practically have any effect by setting via scripts, because it can be changed by
        /// a lot of functions of game code such as ones in `<c>CWeaponWheel</c>`, `<c>CPedWeaponManager</c>`,
        /// `<c>CPedWeaponSelector</c>` and many tasks such as `<c>CTaskCombat</c>` and `<c>CTaskMotionSwimming</c>`.
        /// </remarks>
        KeepWeaponHolsteredUnlessFired,
        /// <summary>
        /// Forces a melee knockout state for the victim <see cref="Ped"/>.
        /// </summary>
        ForceScriptControlledKnockout,
        /// <summary>
        /// Forces a <see cref="Ped"/> to fall out of a <see cref="Vehicle"/> when killed.
        /// </summary>
        FallOutOfVehicleWhenKilled,
        /// <summary>
        /// If set, a <see cref="Ped"/> will escape a burning <see cref="Vehicle"/> they are inside, defaults to
        /// <see langword="true"/>.
        /// </summary>
        GetOutBurningVehicle,
        /// <summary>
        /// Whether this <see cref="Ped"/> has been bumped by the player.
        /// </summary>
        BumpedByPlayer,
        /// <summary>
        /// If set, a <see cref="Ped"/> will run away from fires or potential explosions, defaults to
        /// <see langword="true"/>.
        /// </summary>
        RunFromFiresAndExplosions,
        /// <summary>
        /// If set, the <see cref="Ped"/> will be given the same boost a player gets in the targeting scoring system.
        /// </summary>
        TreatAsPlayerDuringTargeting,
        /// <summary>
        /// indicates if the <see cref="Ped"/> is currently handcuffed.
        /// </summary>
        /// <remarks>
        /// The original name is <c>CPED_CONFIG_FLAG_IsHandCuffed</c> in the exe, but this enum uses
        /// the corrected name.
        /// </remarks>
        IsHandcuffed,
        /// <summary>
        /// indicates if the <see cref="Ped"/> is currently ankle cuffed.
        /// </summary>
        IsAnkleCuffed,
        /// <summary>
        /// Disable melee for a <see cref="Ped"/> (only supported for player right now).
        /// </summary>
        /// <remarks>
        /// <para>
        /// Disables starting melee tasks for player <see cref="Ped"/>s from the regular player move on foot task
        /// `<c>CTaskPlayerOnFoot</c>`, disabling player <see cref="Ped"/>s to start aiming another <see cref="Ped"/>
        /// with a melee weapon (including unarmed one) and to perform a melee attack task when they are regularly
        /// moving.
        /// </para>
        /// <para>
        /// Does not disable starting melee tasks from any tasks other than `<c>CTaskPlayerOnFoot</c>`, which means
        /// this flag does not prevent non-player <see cref="Ped"/>s from performing melee tasks at all.
        /// This flag does not prevent player <see cref="Ped"/> from performing melee tasks when already aiming another
        /// <see cref="Ped"/> with a melee weapon or performing a combat task for the same reason, either.
        /// </para>
        /// </remarks>
        DisableMelee,
        /// <summary>
        /// Disable unarmed drive-by taunts for a <see cref="Ped"/>.
        /// </summary>
        /// <remarks>
        /// The original name is <c>CPED_CONFIG_FLAG_DisableUnarmedDrivebys</c> in the exe, but this enum uses
        /// the corrected name.
        /// </remarks>
        DisableUnarmedDriveBys,
        /// <summary>
        /// MP only, if a player <see cref="Ped"/> is electrocuted and marked as arrestable, another player jacking
        /// the other will just pull them out.
        /// </summary>
        JustGetsPulledOutWhenElectrocuted,

        /*
         *  UNUSED_REPLACE_ME takes 125 in `ePedConfigFlags` in `PedFlagsMeta.psc`
         */

        /// <summary>
        /// <see langword="true"/> if the <see cref="Ped"/> will skip hotwiring a law enforcement <see cref="Vehicle"/>
        /// if it needs to be hotwired.
        /// </summary>
        WillNotHotwireLawEnforcementVehicle = 126,
        /// <summary>
        /// MP only, <see langword="true"/> if the <see cref="Ped"/> will try to commandeer a <see cref="Vehicle"/>
        /// rather than jack if possible.
        /// </summary>
        WillCommandeerRatherThanJack,
        /// <summary>
        /// <see langword="true"/> if the <see cref="Ped"/> will respond to agitation events.
        /// </summary>
        CanBeAgitated,
        /// <summary>
        /// If set, <see cref="Ped"/> will turn to face left in cover.
        /// </summary>
        ForcePedToFaceLeftInCover,
        /// <summary>
        /// If set, <see cref="Ped"/> will turn to face right in cover.
        /// </summary>
        ForcePedToFaceRightInCover,
        /// <summary>
        /// If set, <see cref="Ped"/> will not turn in cover, unless one of the force flags is set.
        /// </summary>
        BlockPedFromTurningInCover,
        /// <summary>
        /// Will allow the <see cref="Ped"/> to keep their relationship group after mission cleanup as opposed to going
        /// back to default.
        /// </summary>
        KeepRelationshipGroupAfterCleanUp,
        /// <summary>
        /// Forces the <see cref="Ped"/> to loop try locked door anim in order to be dragged along when
        /// <see cref="Vehicle"/>'s entry point they're trying to go has a door, and it's fully closed.
        /// </summary>
        ForcePedToBeDragged,
        /// <summary>
        /// <see cref="Ped"/> does not react when being jacked.
        /// </summary>
        PreventPedFromReactingToBeingJacked,
        /// <summary>
        /// indicates if the <see cref="Ped"/> is currently equipped for scuba.
        /// </summary>
        IsScuba,
        /// <summary>
        /// For cops arresting <see cref="Ped"/>s in <see cref="Vehicle"/>s.
        /// </summary>
        WillArrestRatherThanJack,
        /// <summary>
        /// We must be further away before <see cref="Ped"/> polulation remove this <see cref="Ped"/> when it is dead.
        /// </summary>
        RemoveDeadExtraFarAway,
        /// <summary>
        /// <see langword="true"/> if the <see cref="Ped"/> is riding a train.
        /// </summary>
        RidingTrain,
        /// <summary>
        /// <see langword="true"/> if the arrest task succeeded.
        /// </summary>
        ArrestResult,
        /// <summary>
        /// <see langword="true"/> allows this <see cref="Ped"/> to attack <see cref="Ped"/>s they are friendly with.
        /// </summary>
        CanAttackFriendly,
        /// <summary>
        /// <see langword="true"/> if this player <see cref="Ped"/> can jack any other player (MP Only).
        /// </summary>
        WillJackAnyPlayer,
        /// <summary>
        /// Whether this <see cref="Ped"/> has been bumped by a player <see cref="Vehicle"/>.
        /// </summary>
        BumpedByPlayerVehicle,
        /// <summary>
        /// Whether this <see cref="Ped"/> has just dodged a player <see cref="Vehicle"/>.
        /// </summary>
        DodgedPlayerVehicle,
        /// <summary>
        /// <see langword="true"/> if this player will jack wanted player passengers rather than try to steal
        /// a <see cref="Vehicle"/> (cops arresting crims)
        /// (MP Only).
        /// </summary>
        WillJackWantedPlayersRatherThanStealCar,
        /// <summary>
        /// If this flag is set on a cap, skip some of the code that would normally make them extra aggressive and
        /// alert.
        /// </summary>
        NoCopWantedAggro,
        /// <summary>
        /// If this flag is set on a <see cref="Ped"/> it will not scan for or climb ladders.
        /// </summary>
        DisableLadderClimbing,
        /// <summary>
        /// If this flag is set on a <see cref="Ped"/> it has detected stairs.
        /// </summary>
        StairsDetected,
        /// <summary>
        /// If this flag is set on a <see cref="Ped"/> it has detected a slope.
        /// </summary>
        SlopeDetected,
        /// <summary>
        /// If this flag is set on a <see cref="Ped"/> it's helmet has been damaged.
        /// </summary>
        HelmetHasBeenShot,
        /// <summary>
        /// If set, the <see cref="Ped"/> will cower in place rather than flee, used. Used for scenarios in confined
        /// spaces.
        /// </summary>
        CowerInsteadOfFlee,
        /// <summary>
        /// If set the <see cref="Ped"/> will be allowed to ragdoll when the <see cref="Vehicle"/> they are in gets
        /// turned upside down if the seat supports it.
        /// </summary>
        /// <remarks>
        /// Resets to <see langword="true"/> when the <see cref="Ped"/> is marked as no longer needed, which can be done
        /// with <see cref="Entity.MarkAsNoLongerNeeded()"/>.
        /// </remarks>
        CanActivateRagdollWhenVehicleUpsideDown,
        /// <summary>
        /// If set, the <see cref="Ped"/> will respond to <c>CEventInjuredCryForHelp</c> regardless if it is allied with
        /// the injured <see cref="Ped"/>.
        /// </summary>
        /// <remarks>
        /// <c>CEventInjuredCryForHelp</c>s can be created by getting hit by a car (strictly by
        /// <c>WEAPON_RAMMED_BY_CAR</c> or <c>WEAPON_RUN_OVER_BY_CAR</c>, getting hit with a non-lethal gun such as
        /// <see cref="WeaponHash.StunGun"/> but not the tranquilizer (weapon hash), getting hit by melee with
        /// the damage amount more than <c>3.0</c> and the victim is not a player.
        /// </remarks>
        AlwaysRespondToCriesForHelp,
        /// <summary>
        /// If set the <see cref="Ped"/> will not create a blood pool when dead.
        /// </summary>
        DisableBloodPoolCreation,
        /// <summary>
        /// If set, the <see cref="Ped"/> will be fixed if there is no collision around.
        /// </summary>
        ShouldFixIfNoCollision,
        /// <summary>
        /// If set, the <see cref="Ped"/> can perform arrests on <see cref="Ped"/> that can be arrested.
        /// </summary>
        CanPerformArrest,
        /// <summary>
        /// If set, the <see cref="Ped"/> can uncuff <see cref="Ped"/>s that are handcuffed.
        /// </summary>
        CanPerformUncuff,
        /// <summary>
        /// If set, the <see cref="Ped"/> may be arrested.
        /// </summary>
        CanBeArrested,
        /// <summary>
        /// If set, the <see cref="Ped"/>'s mover is getting collisions from opposing sides.
        /// </summary>
        MoverConstrictedByOpposingCollisions,
        /// <summary>
        /// When <see langword="true"/>, Prefer the front seat when getting in a <see cref="Vehicle"/> with buddies.
        /// </summary>
        PlayerPreferFrontSeatMP,
        DontActivateRagdollFromImpactObject,
        DontActivateRagdollFromMelee,
        DontActivateRagdollFromWaterJet,
        DontActivateRagdollFromDrowning,
        DontActivateRagdollFromFalling,
        DontActivateRagdollFromRubberBullet,
        /// <summary>
        /// When <see langword="true"/>, the <see cref="Ped"/> will use injured movement anim sets and getup animations.
        /// </summary>
        IsInjured,
        /// <summary>
        /// When <see langword="true"/>, will follow the player around if in their group but won't enter
        /// <see cref="Vehicle"/>s.
        /// </summary>
        DontEnterVehiclesInPlayersGroup,
        /// <summary>
        /// Stronger than <see cref="IsSwimming"/>, persists so long as the tasks are active.
        /// </summary>
        SwimmingTasksRunning,
        /// <summary>
        /// Disable all melee taunts for this particular <see cref="Ped"/>.
        /// </summary>
        PreventAllMeleeTaunts,
        /// <summary>
        /// Will force this <see cref="Ped"/> to use the direct entry point for any <see cref="Vehicle"/> they try to
        /// enter, or warp in.
        /// </summary>
        ForceDirectEntry,
        /// <summary>
        /// This <see cref="Ped"/> will always see approaching <see cref="Vehicle"/>s (even from behind).
        /// </summary>
        AlwaysSeeApproachingVehicles,
        /// <summary>
        /// This <see cref="Ped"/> can dive away from approaching <see cref="Vehicle"/>s.
        /// </summary>
        CanDiveAwayFromApproachingVehicles,
        /// <summary>
        /// Will allow player to interrupt a <see cref="Ped"/>s scripted entry/exit task as if they had triggered it
        /// themselves
        /// </summary>
        AllowPlayerToInterruptVehicleEntryExit,
        /// <summary>
        /// This <see cref="Ped"/> will not attack cops unless the player is wanted.
        /// </summary>
        OnlyAttackLawIfPlayerIsWanted,
        /// <summary>
        /// Gets set to <see langword="true"/> if the player <see cref="Ped"/> is colliding against a <see cref="Ped"/>
        /// in kinematic mode.
        /// </summary>
        PlayerInContactWithKinematicPed,
        /// <summary>
        /// Gets set to <see langword="true"/> if the player <see cref="Ped"/> is colliding against something that is
        /// not a <see cref="Ped"/> in kinematic mode.
        /// </summary>
        PlayerInContactWithSomethingOtherThanKinematicPed,
        /// <summary>
        /// If set any <see cref="Ped"/> jacking this <see cref="Ped"/> will not get in as part of the jack.
        /// </summary>
        PedsJackingMeDontGetIn,
        AdditionalRappellingPed,
        /// <summary>
        /// AI <see cref="Ped"/>s only, will not early out of anims, default behaviour is to exit as early as possible.
        /// </summary>
        PedIgnoresAnimInterruptEvents,
        /// <summary>
        /// Signifies a player is in custody. Not much useful in final/production game builds, because the built exe
        /// does not have code related to arrest or handcuff.
        /// </summary>
        IsInCustody,
        /// <summary>
        /// By default, armed and friendly <see cref="Ped"/>s have increased resistance to being bumped by players and
        /// friendly <see cref="Vehicle"/>s. Setting this flag will make them use the standard thresholds without
        /// multiplying by any values.
        /// </summary>
        ForceStandardBumpReactionThresholds,
        /// <summary>
        /// If set on a player <see cref="Ped"/>, they can only be attacked by law if the player is wanted.
        /// </summary>
        LawWillOnlyAttackIfPlayerIsWanted,
        /// <summary>
        /// If set, this <see cref="Ped"/> is agitated.
        /// </summary>
        IsAgitated,
        /// <summary>
        /// MP only, prevents passenger from auto shuffling over to driver's seat if it becomes free.
        /// </summary>
        PreventAutoShuffleToDriversSeat,
        /// <summary>
        /// When enabled, the <see cref="Ped"/> will continually set the kinematic mode reset flag when stationary.
        /// </summary>
        UseKinematicModeWhenStationary,
        /// <summary>
        /// When enabled, non-player <see cref="Ped"/>s can use WeaponBlocking behaviors.
        /// </summary>
        EnableWeaponBlocking,
        HasHurtStarted,
        /// <summary>
        /// Will prevent the <see cref="Ped"/>s go into hurt combat mode.
        /// </summary>
        DisableHurt,
        /// <summary>
        /// Should this player <see cref="Ped"/> periodically generate shocking events for being weird.
        /// </summary>
        PlayerIsWeird,
        /// <summary>
        /// Has this <see cref="Ped"/> had a phone conversation before.
        /// </summary>
        PedHadPhoneConversation,
        /// <summary>
        /// Indicates <see cref="Ped"/> started crossing the road in case of interruption.
        /// </summary>
        BeganCrossingRoad,
        /// <summary>
        /// Warps into the leader's <see cref="Vehicle"/> of the <see cref="Ped"/>'s <see cref="PedGroup"/> when
        /// starting entering the <see cref="Vehicle"/> instead of normally entering it.
        /// </summary>
        WarpIntoLeadersVehicle,
        /// <summary>
        /// Do nothing when on foot by default (when computing a default task).
        /// </summary>
        /// <remarks>
        /// Another task takes precedence when computing a default task if the <see cref="Ped"/> meet one of
        /// the following criteria in the following order of precedence;
        /// <list type="bullet">
        /// <item><description>
        /// <see cref="Ped"/> is in <see cref="Vehicle"/> (strictly when <see cref="InVehicle"/> is set to
        /// <see langword="true"/>). Performs an appropriate task such as `<c>CTaskPlayerDrive</c>` for the local player
        /// <see cref="Ped"/> and `<c>CTaskPolice</c>` for <see cref="Ped"/>s whose population type is the cop one.
        /// </description></item>
        /// <item><description>
        /// <see cref="Ped"/> is the local player <see cref="Ped"/>. Uses a primary motion task.
        /// </description></item>
        /// <item><description>
        /// <see cref="Ped"/>'s <see cref="PedType"/> is <see cref="PedType.Swat"/> and
        /// the <see cref="EntityPopulationType"/> is not <see cref="EntityPopulationType.Mission"/>.
        /// Uses a `<c>CTaskSwat</c>`.
        /// </description></item>
        /// <item><description>
        /// <see cref="Ped"/>'s <see cref="PedType"/> is <see cref="PedType.Army"/> and
        /// the <see cref="EntityPopulationType"/> is not <see cref="EntityPopulationType.Mission"/>.
        /// Uses a `<c>CTaskArmy</c>`.
        /// </description></item>
        /// <item><description>
        /// <see cref="Ped"/> is in a <see cref="PedGroup"/> as a follower and it has a leader.
        /// Uses a `<c>CTaskFollowLeaderAnyMeans</c>`.
        /// </description></item>
        /// <item><description>
        /// <see cref="Ped"/>'s <see cref="EntityPopulationType"/> is <see cref="EntityPopulationType.Mission"/>.
        /// Uses a `<c>CTaskDoNothing</c>`, which means there is no need to set this flag for mission
        /// <see cref="Ped"/>s unless <see cref="Entity.MarkAsNoLongerNeeded"/> may be called on them later.
        /// </description></item>
        /// </list>
        /// </remarks>
        DoNothingWhenOnFootByDefault,
        /// <summary>
        /// Set when the <see cref="Ped"/> is using a scenario. Call `<c>CPed::UpdateSpatialArrayTypeFlags()</c>` if
        /// changing.
        /// </summary>
        UsingScenario,
        /// <summary>
        /// Set when the <see cref="Ped"/> is visible on screen, as determined by `<c>CPedAILodManager</c>`.
        /// </summary>
        VisibleOnScreen,
        /// <summary>
        /// If <see langword="true"/>, the <see cref="Ped"/> will not collide with other kinematic <see cref="Ped"/>s.
        /// </summary>
        DontCollideWithKinematic,
        /// <summary>
        /// If set, activate physics when switching from low to regular physics LOD.
        /// </summary>
        ActivateOnSwitchFromLowPhysicsLod,
        /// <summary>
        /// Dead <see cref="Ped"/>s with this flag set won't be allowed to reactivate their ragdoll when hit by another
        /// ragdoll.
        /// </summary>
        DontActivateRagdollOnPedCollisionWhenDead,
        /// <summary>
        /// Dead <see cref="Ped"/>s with this flag set won't be allowed to reactivate their ragdoll when hit by
        /// a <see cref="Vehicle"/>.
        /// </summary>
        DontActivateRagdollOnVehicleCollisionWhenDead,
        /// <summary>
        /// Is set if this <see cref="Ped"/> has ever been in armed combat.
        /// </summary>
        HasBeenInArmedCombat,
        /// <summary>
        /// Set for when we want to diminish the ammo at a slower rate. Used specifically in cases where AI do not have
        /// infinite ammo.
        /// </summary>
        UseDiminishingAmmoRate,
        /// <summary>
        /// This <see cref="Ped"/> will not steer around anyone
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_Avoidance_Ignore_All</c>` in the exe.
        /// </remarks>
        AvoidanceIgnoreAll,
        /// <summary>
        /// Other <see cref="Ped"/>s won't steer around this <see cref="Ped"/>.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_Avoidance_Ignored_by_All</c>` in the exe.
        /// </remarks>
        AvoidanceIgnoredByAll,
        /// <summary>
        /// This <see cref="Ped"/> will not steer around <see cref="Ped"/>s marked group 1.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_Avoidance_Ignore_Group1</c>` in the exe.
        /// </remarks>
        AvoidanceIgnoreGroup1,
        /// <summary>
        /// This <see cref="Ped"/> is marked as a member of avoidance group 1.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_Avoidance_Member_of_Group1</c>` in the exe.
        /// </remarks>
        AvoidanceMemberOfGroup1,
        /// <summary>
        /// <see cref="Ped"/> is forced to use specific seat index set by `<c>SET_PED_GROUP_MEMBER_PASSENGER_INDEX</c>`.
        /// </summary>
        ForcedToUseSpecificGroupSeatIndex,
        /// <summary>
        /// If set, <see cref="Ped"/>s in low lod physics will be placed so that their feet rest on the navmesh.
        /// </summary>
        LowPhysicsLodMayPlaceOnNavMesh,
        /// <summary>
        /// If set, <see cref="Ped"/>s will disable all explosion reactions.
        /// </summary>
        DisableExplosionReactions,
        /// <summary>
        /// Whether this <see cref="Ped"/> has just dodged a player.
        /// </summary>
        DodgedPlayer,
        /// <summary>
        /// Set when player switches to an AI <see cref="Ped"/> and keeps the scripted task of the AI <see cref="Ped"/>,
        /// if unset we won't check for interrupts or time out.
        /// </summary>
        WaitingForPlayerControlInterrupt,
        /// <summary>
        /// <see cref="Ped"/> will not move out of cover when set (not even to fire).
        /// </summary>
        ForcedToStayInCover,
        /// <summary>
        /// Does this <see cref="Ped"/> generate sound events?
        /// </summary>
        GeneratesSoundEvents,
        /// <summary>
        /// Does this <see cref="Ped"/> have the ability to respond to sound events?
        /// </summary>
        ListensToSoundEvents,
        /// <summary>
        /// <see cref="Ped"/> can be targeting inside a <see cref="Vehicle"/>.
        /// </summary>
        AllowToBeTargetedInAVehicle,
        /// <summary>
        /// When exiting a <see cref="Vehicle"/>, the <see cref="Ped"/> will wait for the direct entry point to be clear
        /// before exiting.
        /// </summary>
        WaitForDirectEntryPointToBeFreeWhenExiting,
        OnlyRequireOnePressToExitVehicle,
        /// <summary>
        /// Force the skydive exit if we're exiting the <see cref="Vehicle"/>.
        /// </summary>
        ForceExitToSkyDive,
        /// <summary>
        /// Enables/disables the low-level steering behaviour around <see cref="Vehicle"/>s.
        /// </summary>
        SteersAroundVehicles,
        /// <summary>
        /// If set, allow the <see cref="Ped"/> to be set in <see cref="Vehicle"/>s even if the <see cref="Ped"/>'s
        /// TaskData would otherwise disallow it.
        /// </summary>
        AllowPedInVehiclesOverrideTaskFlags,
        /// <summary>
        /// If set, the <see cref="Ped"/> will not enter the leader's <see cref="Vehicle"/>.
        /// </summary>
        DontEnterLeadersVehicle,
        /// <summary>
        /// Disable the skydive exit if we're exiting the <see cref="Vehicle"/>.
        /// </summary>
        DisableExitToSkyDive,
        /// <summary>
        /// Script disabled collision on this <see cref="Ped"/> via <see cref="Entity.IsCollisionEnabled"/>, this leaves
        /// on collision against explosions and weapons. Exists solely to prevent the AI lod reactivate the collision.
        /// </summary>
        ScriptHasDisabledCollision,
        /// <summary>
        /// This <see cref="Ped"/> is drawn randomly scaled from [0.5,1.0]
        /// </summary>
        UseAmbientModelScaling,
        /// <summary>
        /// Hurry away without watching the next time this <see cref="Ped"/> runs <c>CTaskHurryAway</c>.
        /// </summary>
        DontWatchFirstOnNextHurryAway,
        /// <summary>
        /// make <c>EVENT_POTENTIAL_BE_WALKED_INTO</c> not affect this <see cref="Ped"/>.
        /// </summary>
        DisablePotentialToBeWalkedIntoResponse,
        /// <summary>
        /// This <see cref="Ped"/> will not avoid other <see cref="Ped"/>s whilst navigating.
        /// </summary>
        /// <remarks>
        /// Resets to <see langword="false"/> when the <see cref="Ped"/> is marked as no longer needed, which can be
        /// done with <see cref="Entity.MarkAsNoLongerNeeded()"/>.
        /// </remarks>
        DisablePedAvoidance,
        /// <summary>
        /// When the <see cref="Ped"/> dies, it will ragdoll instead of potentially choosing an animated death.
        /// </summary>
        ForceRagdollUponDeath,
        /// <summary>
        /// When <see cref="Ped"/> receives damage any <see cref="Prop"/> glasses could be knocked off.
        /// </summary>
        CanLosePropsOnDamage,
        DisablePanicInVehicle,
        /// <summary>
        /// Allow this <see cref="Ped"/> to detach trailers from <see cref="Vehicle"/>s.
        /// </summary>
        AllowedToDetachTrailer,
        HasShotBeenReactedToFromFront,
        HasShotBeenReactedToFromBack,
        HasShotBeenReactedToFromLeft,
        HasShotBeenReactedToFromRight,
        /// <summary>
        /// If set, the ragdoll activation blocking flags can be used to disable activation of dead <see cref="Ped"/>s.
        /// Otherwise, by default, dead <see cref="Ped"/>s can always activate their ragdolls.
        /// </summary>
        AllowBlockDeadPedRagdollActivation,
        /// <summary>
        /// <see langword="true"/> if the <see cref="Ped"/> is currently holding a <see cref="Prop"/>.
        /// </summary>
        IsHoldingProp,
        /// <summary>
        /// When this <see cref="Ped"/> dies their body will block all pathfinding modes - not just wandering.
        /// </summary>
        BlocksPathingWhenDead,
        /// <summary>
        /// The next time this <see cref="Ped"/> leaves a scenario to perform some script task they will be forced into
        /// their normal scenario exit.
        /// </summary>
        ForcePlayNormalScenarioExitOnNextScriptCommand,
        /// <summary>
        /// The next time this <see cref="Ped"/> leaves a scneario to perform some script task they will be forced into
        /// their immediate (blend out) exit.
        /// </summary>
        ForcePlayImmediateScenarioExitOnNextScriptCommand,
        /// <summary>
        /// Force character cloth to stay skinned immediately after being created. If flag is not set then character
        /// cloth is not forced to be skinned when created.
        /// </summary>
        ForceSkinCharacterCloth,
        /// <summary>
        /// The player will leave the engine on when exiting a <see cref="Vehicle"/> <em>normally</em>.
        /// </summary>
        /// <remarks>
        /// The following factors overwrite the "SwitchEngineOff" value evaluated with this flag, which are evaluated
        /// in the following order;
        /// <list type="bullet">
        /// <item><description>
        /// If the <see cref="Vehicle"/> model (index) is not that of the Oppressor Mk II, then;
        ///     <list type="bullet">
        ///     <item><description>
        ///     The wanted level is 2 or more and the time since last spotted is less than a certain tunable value.
        ///     Overwrites the evaluated value with <see langword="false"/>.
        ///     </description></item>
        ///     <item><description>
        ///     There is an active Action Mode reason. Overwrites the evaluated value with <see langword="false"/>.
        ///     </description></item>
        ///     </list>
        /// </description></item>
        /// <item><description>
        /// The <see cref="Vehicle"/> has the flag that can be set with
        /// `<c>SET_VEHICLE_KEEP_ENGINE_ON_WHEN_ABANDONED</c>`. Overwrites the evaluated value with
        /// <see langword="false"/>.
        /// </description></item>
        /// <item><description>
        /// The <see cref="Vehicle"/> has weapon blades. Overwrites the evaluated value with <see langword="true"/>.
        /// </description></item>
        /// <item><description>
        /// `<c>CVehicleEnterExitFlags::BeJacked</c>` flag is set for the exit <see cref="Vehicle"/> task.
        /// Overwrites the evaluated value with <see langword="false"/>.
        /// </description></item>
        /// <item><description>
        /// The <see cref="Vehicle"/> is using script autopilot. Overwrites the evaluated value with
        /// <see langword="false"/>.
        /// </description></item>
        /// </list>
        /// </remarks>
        LeaveEngineOnWhenExitingVehicles,
        /// <summary>
        /// Tells taskmobile phone to not play texting animations. Currently, don't play these in MP.
        /// </summary>
        PhoneDisableTextingAnimations,
        /// <summary>
        /// Tells taskmobile phone to not play talking animations. Currently, don't play these in MP.
        /// </summary>
        PhoneDisableTalkingAnimations,
        /// <summary>
        /// Tells taskmobile phone to not play camera animations. Currently, don't play these in SP.
        /// </summary>
        PhoneDisableCameraAnimations,
        /// <summary>
        /// Stops the <see cref="Ped"/> from accidentally blind firing its weapon when doing an NM (ragdoll) shot
        /// reaction.
        /// </summary>
        DisableBlindFiringInShotReactions,
        /// <summary>
        /// This makes it so that OTHER <see cref="Ped"/>s are allowed to take cover at points that would otherwise be
        /// rejected due to proximity.
        /// </summary>
        AllowNearbyCoverUsage,
        /// <summary>
        /// <see langword="true"/> if in strafe transition.
        /// </summary>
        InStrafeTransition,
        /// <summary>
        /// If <see langword="false"/>, blocks in-car idle animations from playing.
        /// </summary>
        CanPlayInCarIdles,
        /// <summary>
        /// If the <see cref="Ped"/> is a law enforcement <see cref="Ped"/> then they will ignore the player wanted
        /// level clean check in combat and continue attacking.
        /// </summary>
        CanAttackNonWantedPlayerAsLaw,
        /// <summary>
        /// <see cref="Ped"/> gets damaged when the <see cref="Vehicle"/> they are in crashes.
        /// </summary>
        WillTakeDamageWhenVehicleCrashes,
        /// <summary>
        /// If this AI <see cref="Ped"/> is driving the <see cref="Vehicle"/>, if the player taps to enter, they will
        /// enter as a rear passenger, if they hold, they'll jack the driver.
        /// </summary>
        AICanDrivePlayerAsRearPassenger,
        /// <summary>
        /// If a friendly player is driving the <see cref="Vehicle"/>, if the player taps to enter, they will enter as
        /// a passenger, if they hold, they'll jack the driver.
        /// </summary>
        PlayerCanJackFriendlyPlayers,
        /// <summary>
        /// Are we on stairs?
        /// </summary>
        OnStairs,
        /// <summary>
        /// Simulating the aim button for player until on input detection.
        /// </summary>
        SimulatingAiming,
        /// <summary>
        /// If this AI <see cref="Ped"/> is driving the <see cref="Vehicle"/>, allow players to get in passenger seats.
        /// </summary>
        AIDriverAllowFriendlyPassengerSeatEntry,
        /// <summary>
        /// Set on the target <see cref="Ped"/> if the <see cref="Vehicle"/> they are in is being removed to avoid
        /// an expensive detach check.
        /// </summary>
        ParentCarIsBeingRemoved,
        /// <summary>
        /// Set the target <see cref="Ped"/> to be allowed to use Injured movement clips.
        /// </summary>
        AllowMissionPedToUseInjuredMovement,
        /// <summary>
        /// When <see cref="Ped"/> receives a headshot then a helmet can be knocked off.
        /// </summary>
        CanLoseHelmetOnDamage,
        /// <summary>
        /// When this <see cref="Ped"/> exits a scenario they ignore probe checks against the environment and just pick
        /// an exit clip.
        /// </summary>
        NeverDoScenarioExitProbeChecks,
        /// <summary>
        /// This will suppress the automatic switch to a lower ragdoll LOD when switching to the ragdoll frame after
        /// dying.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_SuppressLowLODRagdollSwitchWhenCorpseSettles</c>` in the exe.
        /// </remarks>
        SuppressLowLodRagdollSwitchWhenCorpseSettles,
        /// <summary>
        /// Don't use certain seats (like hanging on the side of a <see cref="Vehicle"/>).
        /// </summary>
        PreventUsingLowerPrioritySeats,
        /// <summary>
        /// Set when leaving a <see cref="Vehicle"/> and disabling collision with the <see cref="Vehicle"/> exiting to
        /// indicate we need to clear out the disabling.
        /// </summary>
        JustLeftVehicleNeedsReset,
        /// <summary>
        /// If this <see cref="Ped"/> is following the player and stuck in a place where he can't be reached, teleport
        /// when possible.
        /// </summary>
        TeleportIfCantReachPlayer,
        /// <summary>
        /// <see cref="Ped"/> was being jacked/killed but isn't anymore, ensure they're in the seat.
        /// </summary>
        PedsInVehiclePositionNeedsReset,
        /// <summary>
        /// <see cref="Ped"/> is fully in the seat (Set after the position needs reset flag)
        /// </summary>
        PedsFullyInSeat,
        /// <summary>
        /// If this <see cref="Ped"/> is friendly with the player, this will allow the <see cref="Ped"/> to lock on.
        /// </summary>
        AllowPlayerLockOnIfFriendly,
        /// <summary>
        /// Force camera direction for heading test if desired direction is also set
        /// </summary>
        UseCameraHeadingForDesiredDirectionLockOnTest,
        /// <summary>
        /// If set, teleport if <see cref="Ped"/> is not in the leader's <see cref="Vehicle"/> before
        /// `<c>TaskEnterVehicle::m_SecondsBeforeWarpToLeader</c>`, which is set with <c>SecondsBeforeWarpToLeader</c>
        /// of <c>CTaskEnterVehicle__Tunables</c> in <c>vehicletasks.ymt</c>.
        /// </summary>
        TeleportToLeaderVehicle,
        /// <summary>
        /// Don't give weird <see cref="Ped"/>s extra buffer.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_Avoidance_Ignore_WeirdPedBuffer</c>` in the exe.
        /// </remarks>
        AvoidanceIgnoreWeirdPedBuffer,
        /// <summary>
        /// Are we on a stair slope?
        /// </summary>
        OnStairSlope,
        /// <summary>
        /// This <see cref="Ped"/> has gotten up from NM at least once.
        /// </summary>
        HasPlayedNMGetup,
        /// <summary>
        /// Wanted system shouldn't consider this <see cref="Ped"/> when creating blips.
        /// </summary>
        DontBlipCop,
        /// <summary>
        /// Set if the <see cref="Ped"/> spawned at a scenario with extended range.
        /// </summary>
        SpawnedAtExtendedRangeScenario,
        /// <summary>
        /// This <see cref="Ped"/> will walk alongside group leader if they are the first member of the leader's
        /// <see cref="PedGroup"/>, they are close enough to the leader, and the <see cref="PedGroup"/>'s formation is
        /// set up to allow this (such as in the default <c>CPedFormationTypes::FORMATION_LOOSE</c>).
        /// </summary>
        WalkAlongsideLeaderWhenClose,
        /// <summary>
        /// This will kill a mission <see cref="Ped"/> that becomes trapped (like under a cow carcass) and cannot get
        /// up.
        /// </summary>
        KillWhenTrapped,
        /// <summary>
        /// If this flag is set on a <see cref="Ped"/> it has detected an edge.
        /// </summary>
        EdgeDetected,
        /// <summary>
        /// This <see cref="Ped"/> will cause physics to activate on any <see cref="Ped"/> this <see cref="Ped"/>'s
        /// capsule is inside, even if this <see cref="Ped"/> is being attached.
        /// </summary>
        AlwaysWakeUpPhysicsOfIntersectedPeds,
        /// <summary>
        /// This is set to prevent a <see cref="Ped"/> from holstering a loadout weapon equipped during
        /// <c>CPedPopulation::EquipPed</c> as part of the defined <c>CAmbientPedModelVariations</c> loadout.
        /// </summary>
        EquippedAmbientLoadOutWeapon,
        /// <summary>
        /// If set, a <see cref="Ped"/> will avoid tear gas.
        /// </summary>
        AvoidTearGas,
        /// <summary>
        /// Marks that we've already dealt with cleaning up speech audio after becoming frozen.
        /// </summary>
        StoppedSpeechUponFreezing,
        /// <summary>
        /// If set, <c>CPed::DAMAGED_GOTOWRITHE</c> will no longer get set. In particular, tazer hits will no longer
        /// kill this <see cref="Ped"/> in one hit. Also, this flag prevents <see cref="Ped"/>s reacting to fire from
        /// instantly getting killed when they are set not to ragdoll.
        /// </summary>
        DisableGoToWritheWhenInjured,
        /// <summary>
        /// If set this <see cref="Ped"/> will only use their forced seat index if the <see cref="Vehicle"/> they are
        /// entering is a heli as part of a group.
        /// </summary>
        OnlyUseForcedSeatWhenEnteringHeliInGroup,
        /// <summary>
        /// <see cref="Ped"/> got tired and was thrown from bike. Used to scale ragdoll damage for a few seconds after
        /// the dismount.
        /// </summary>
        ThrownFromVehicleDueToExhaustion,
        /// <summary>
        /// This <see cref="Ped"/> will update their enclosed regions.
        /// </summary>
        UpdateEnclosedSearchRegion,
        DisableWeirdPedEvents,
        /// <summary>
        /// This <see cref="Ped"/> should charge if in combat right away, for use by scripts, cleared once
        /// <see cref="Ped"/> charges.
        /// </summary>
        ShouldChargeNow,
        /// <summary>
        /// We don't want ragdolling <see cref="Ped"/>s processing buoyancy when in a boat.
        /// </summary>
        RagdollingOnBoat,
        HasBrandishedWeapon,
        /// <summary>
        /// If <see langword="true"/>, this <see cref="Ped"/> will react to events such as being hit by
        /// a <see cref="Vehicle"/> as a mission <see cref="Ped"/>.
        /// </summary>
        AllowMinorReactionsAsMissionPed,
        /// <summary>
        /// If <see langword="true"/>, this <see cref="Ped"/> will not generate dead body shocking events when dead.
        /// </summary>
        BlockDeadBodyShockingEventsWhenDead,
        /// <summary>
        /// <see langword="true"/> if the <see cref="Ped"/> has be visible to the player.
        /// </summary>
        PedHasBeenSeen,
        /// <summary>
        /// <see langword="true"/> if the <see cref="Ped"/> is currently in the <see cref="Ped"/> reuse pool.
        /// </summary>
        PedIsInReusePool,
        /// <summary>
        /// <see langword="true"/> if the <see cref="Ped"/> was in the reuse pool and then was reused.
        /// </summary>
        PedWasReused,
        /// This <see cref="Ped"/> should ignore shocking events (not generate ones).
        DisableShockingEvents,
        /// <summary>
        /// Set for <see cref="Ped"/>s that have moved using low LOD physics.
        /// </summary>
        MovedUsingLowLodPhysicsSinceLastActive,
        /// <summary>
        /// If <see langword="true"/>, this <see cref="Ped"/> will not react to a <see cref="Ped"/> standing on the
        /// roof.
        /// </summary>
        NeverReactToPedOnRoof,
        /// <summary>
        /// If set this <see cref="Ped"/> will use a flee exit to leave on the next script command.
        /// </summary>
        ForcePlayFleeScenarioExitOnNextScriptCommand,
        /// <summary>
        /// Set for <see cref="Ped"/>s that just bumped into a <see cref="Vehicle"/>.
        /// </summary>
        JustBumpedIntoVehicle,
        /// If <see langword="true"/>, the <see cref="Ped"/> will not react to <see cref="Ped"/>s driving on pavement.
        DisableShockingDrivingOnPavementEvents,
        /// <summary>
        /// This <see cref="Ped"/> should throw a smoke grenade in combat right away, for use by scripts, cleared once
        /// <see cref="Ped"/> throws.
        /// </summary>
        ShouldThrowSmokeNow,
        /// <summary>
        /// Flags the <see cref="Ped"/> to ensure it either does or does not have its control constraints.
        /// </summary>
        DisablePedConstraints,
        /// <summary>
        /// If set, <see cref="Ped"/> will peek once before firing in cover. Cleared upon peeking.
        /// </summary>
        ForceInitialPeekInCover,
        /// <summary>
        /// this <see cref="Ped"/> was created by one of the dispatch systems, usually law enforcement.
        /// </summary>
        CreatedByDispatch,
        /// <summary>
        /// NM state config flag. Set to <see langword="true"/> when the characters support hand has broken from
        /// the weapon.
        /// </summary>
        PointGunLeftHandSupporting,
        /// <summary>
        /// If <see langword="true"/> on a <see cref="PedGroup"/> leader, disable the follower <see cref="Ped"/>s
        /// jumping out of <see cref="Vehicle"/>s after the leader.
        /// </summary>
        DisableJumpingFromVehiclesAfterLeader,
        /// <summary>
        /// Blocks ragdoll activation from animated player <see cref="Ped"/> bumps.
        /// </summary>
        DontActivateRagdollFromPlayerPedImpact,
        /// <summary>
        /// Blocks ragdoll activation from collisions with AI ragdolls.
        /// </summary>
        DontActivateRagdollFromAiRagdollImpact,
        /// <summary>
        /// Blocks ragdoll activation from collisions with a ragdolling player.
        /// </summary>
        DontActivateRagdollFromPlayerRagdollImpact,
        /// <summary>
        /// Use to disable quadruped spring processing when settling from a ragdoll performance.
        /// </summary>
        DisableQuadrupedSpring,
        /// <summary>
        /// This <see cref="Ped"/> is currently in a scenario point cluster.
        /// </summary>
        IsInCluster,
        /// <summary>
        /// If set, <see cref="Ped"/> will shout target position when melee attacked by a player.
        /// </summary>
        ShoutToGroupOnPlayerMelee,
        /// <summary>
        /// Set this for a <see cref="Ped"/> to be ignored by the auto opened doors when checking to see if the door
        /// should be opened.
        /// </summary>
        IgnoredByAutoOpenDoors,
        /// <summary>
        /// Set this during nm tasks to trigger an injured getup when the <see cref="Ped"/> gets up.
        /// </summary>
        PreferInjuredGetup,
        /// <summary>
        /// Purposely ignore the melee active combatant role and push them into a support or inactive combatant role.
        /// </summary>
        ForceIgnoreMeleeActiveCombatant,
        /// <summary>
        /// If set, <see cref="Ped"/> will ignore sound events generated by <see cref="Entity"/>s they can't see.
        /// </summary>
        CheckLoSForSoundEvents,
        /// <summary>
        /// This <see cref="Ped"/> was spawned to steal an ambient <see cref="Vehicle"/> that was left around.
        /// </summary>
        JackedAbandonedCar,
        /// <summary>
        /// If set, <see cref="Ped"/> can play FRIEND_FOLLOWED_BY_PLAYER lines.
        /// </summary>
        CanSayFollowedByPlayerAudio,
        /// <summary>
        /// If set, the <see cref="Ped"/> will activate the NM ragdoll balance as soon as they are touched by the player
        /// (ignoring velocity thresholds).
        /// </summary>
        ActivateRagdollFromMinorPlayerContact,
        /// <summary>
        /// If set, the <see cref="Ped"/> is carrying a portable pickup.
        /// </summary>
        HasPortablePickupAttached,
        /// <summary>
        /// If set, default cloth pose will be applied if is available in the character cloth when the cloth is created.
        /// </summary>
        ForcePoseCharacterCloth,
        /// <summary>
        /// If set, <see cref="Ped"/> will use cloth collision bounds.
        /// </summary>
        HasClothCollisionBounds,
        /// <summary>
        /// Set when the <see cref="Ped"/> has high heels.
        /// </summary>
        HasHighHeels,
        /// <summary>
        /// If set, this force player <see cref="Ped"/> to treat this <see cref="Ped"/> as an ambient target even if
        /// this <see cref="Ped"/>'s  <see cref="EntityPopulationType"/> is set to
        /// <see cref="EntityPopulationType.Mission"/>.
        /// </summary>
        /// <remarks>
        /// With this flag, the player cannot lock on to <see cref="Ped"/>s with <see cref="PedType.Animal"/> or
        /// non-threat <see cref="Ped"/>s as how non-mission <see cref="Ped"/>s can be locked on to from inside
        /// a <see cref="Vehicle"/>. Without this flag, the player cannot lock on to friendly mission <see cref="Ped"/>s
        /// (the player can lock on to mission <see cref="Ped"/>s who are neutral).
        /// </remarks>
        TreatAsAmbientPedForDriverLockOn,
        /// <summary>
        /// </summary>
        /// If set on security <see cref="Ped"/>s, they will not use the law like behaviors/logic (they will not report
        /// wanted position, can attack without wanted level, etc.).
        DontBehaveLikeLaw,
        /// <summary>
        /// If set, the <see cref="Ped"/> was originally spawned at a scenario point.
        /// </summary>
        SpawnedAtScenario,
        /// <summary>
        /// If set, police will not perform the CTaskShockingPoliceInvestigate Behavior on the <see cref="Ped"/>
        /// </summary>
        DisablePoliceInvestigatingBody,
        /// <summary>
        /// If set, the <see cref="Ped"/> will no longer shoot while writhing.
        /// </summary>
        DisableWritheShootFromGround,
        /// <summary>
        /// If set the <see cref="Ped"/> will only just the warp entry points if there are no animated entry points
        /// available.
        /// </summary>
        LowerPriorityOfWarpSeats,
        /// <summary>
        /// If set the <see cref="Ped"/> cannot be talked to.
        /// </summary>
        /// <remarks>
        /// Resets to <see langword="false"/> when the <see cref="Ped"/> is marked as no longer needed, which can be
        /// done with <see cref="Entity.MarkAsNoLongerNeeded()"/>.
        /// </remarks>
        DisableTalkTo,
        /// <summary>
        /// Prevents the wanted system from attaching a blip to a <see cref="Ped"/>
        /// </summary>
        DontBlip,
        /// <summary>
        /// <see cref="Ped"/> is running the swap weapon task.
        /// </summary>
        IsSwitchingWeapon,
        /// <summary>
        /// If set, the <see cref="Ped"/> will ignore leg IK request restrictions for non-player <see cref="Ped"/>s.
        /// </summary>
        IgnoreLegIkRestrictions,
        /// <summary>
        /// If set, the <see cref="Ped"/> will never have their intelligence update time sliced across frames.
        /// </summary>
        ScriptForceNoTimesliceIntelligenceUpdate,
        /// <summary>
        /// If set, this <see cref="Ped"/> has been jacked out of its <see cref="Vehicle"/>.
        /// </summary>
        JackedOutOfMyVehicle,
        /// <summary>
        /// If set, this <see cref="Ped"/> went into combat because of being jacked.
        /// </summary>
        WentIntoCombatAfterBeingJacked,
        /// <summary>
        /// Blocks ragdoll activation when grabbing <see cref="Vehicle"/> doors.
        /// </summary>
        DontActivateRagdollForVehicleGrab,
        /// <summary>
        /// Set the flag for forcing package on character cloth when cloth is created on the <see cref="Ped"/>.
        /// </summary>
        ForcePackageCharacterCloth,
        DontRemoveWithValidOrder,
        /// <summary>
        /// If set, this <see cref="Ped"/> will timeslice it's <c>DoNothing</c> Task when computing default task.
        /// </summary>
        AllowTaskDoNothingTimeslicing,
        ForcedToStayInCoverDueToPlayerSwitch,
        /// <summary>
        /// Set the flag to place character cloth in prone state when cloth is created on the <see cref="Ped"/>.
        /// </summary>
        ForceProneCharacterCloth,
        /// <summary>
        /// If set, the <see cref="Ped"/> will not be allowed to jack any other players (not synced).
        /// </summary>
        NotAllowedToJackAnyPlayers,
        InToStrafeTransition,
        /// <summary>
        /// Killed by standard melee.
        /// </summary>
        KilledByStandardMelee,
        /// <summary>
        /// If set, the <see cref="Ped"/> will always exit the train when it stops at a station.
        /// </summary>
        AlwaysLeaveTrainUponArrival,
        /// <summary>
        /// Set flag to determine that a directed normal exit should be used for new tasks on this scenario
        /// <see cref="Ped"/>.
        /// </summary>
        ForcePlayDirectedNormalScenarioExitOnNextScriptCommand,
        /// <summary>
        /// Only allow <see cref="Ped"/> to writhe from weapon damage, not from other stuff, like small vehicle impacts
        /// </summary>
        OnlyWritheFromWeaponDamage,
        /// <summary>
        /// Flags the <see cref="Ped"/> to use the slo-mo blood vfx instead of the normal ones.
        /// </summary>
        UseSloMoBloodVfx,
        /// <summary>
        /// Equip/put on the jetpack if we have one in our inventory.
        /// </summary>
        EquipJetpack,
        /// <summary>
        /// Don't do threat response when dragged out of a <see cref="Vehicle"/>.
        /// </summary>
        PreventDraggedOutOfCarThreatResponse,
        /// <summary>
        /// Script has completely disabled collision on this <see cref="Ped"/> via
        /// <c>SET_ENTITY_COMPLETELY_DISABLE_COLLISION</c>.
        /// </summary>
        ScriptHasCompletelyDisabledCollision,
        /// <summary>
        /// This <see cref="Ped"/> will not check for navmesh when exiting their scenario.
        /// </summary>
        NeverDoScenarioNavChecks,
        /// <summary>
        /// This <see cref="Ped"/> will expensively probe for a scenario exit location in one frame.
        /// </summary>
        ForceSynchronousScenarioExitChecking,
        /// <summary>
        /// Set <see langword="true"/> in <c>CTaskAimGunOnFoot::Aiming_OnUpdate</c>, <see langword="false"/> in
        /// <c>CTaskAimAndThrowProjectile::CleanUp</c>.
        /// </summary>
        ThrowingGrenadeWhileAiming,
        HeadbobToRadioEnabled,
        /// <summary>
        /// Don't do distance from camera culling of the deep surface check, needed for detecting snow, mud, etc.
        /// </summary>
        ForceDeepSurfaceCheck,
        /// <summary>
        /// Disable deep surface anims to prevent them slowing <see cref="Ped"/> down.
        /// </summary>
        DisableDeepSurfaceAnims,
        /// <summary>
        /// If set the <see cref="Ped"/> will not be blipped by the wanted system, this is not synced over
        /// the network to allow script to individually control a <see cref="Ped"/>s blippedness on different machines.
        /// </summary>
        DontBlipNotSynced,
        /// <summary>
        /// <see cref="Ped"/> is ducking inside a <see cref="Vehicle"/>.
        /// </summary>
        IsDuckingInVehicle,
        /// If set the <see cref="Ped"/> will not automatically shuffle to the turret seat when it becomes free.
        PreventAutoShuffleToTurretSeat,
        /// <summary>
        /// Disables the ignore events based on interior status check which normally has <see cref="Ped"/>s inside
        /// ignore events from outside.
        /// </summary>
        /// <remarks>
        /// Resets to <see langword="false"/> when the <see cref="Ped"/> is marked as no longer needed, which can be
        /// done with <see cref="Entity.MarkAsNoLongerNeeded()"/>.
        /// </remarks>
        DisableEventInteriorStatusCheck,
        /// <summary>
        /// Does <see cref="Ped"/> have a reserve chute that they can deploy.
        /// </summary>
        HasReserveParachute,
        /// <summary>
        /// Use reserve parachute settings.
        /// </summary>
        UseReserveParachute,
        /// <summary>
        /// If this <see cref="Ped"/> is in combat then any <see cref="Ped"/> they dislike they will consider
        /// the relationship hate instead.
        /// </summary>
        TreatDislikeAsHateWhenInCombat,
        /// <summary>
        /// If the target is a player we will only set the wanted level or update the radar if they are seen.
        /// </summary>
        OnlyUpdateTargetWantedIfSeen,
        /// <summary>
        /// Allows this <see cref="Ped"/> to auto shuffle to the driver seat of a <see cref="Vehicle"/> if the driver is
        /// dead (law and MP <see cref="Ped"/>s would do this normally).
        /// </summary>
        AllowAutoShuffleToDriversSeat,
        /// <summary>
        /// Blocks ragdoll activation when damaged by smoke grenade.
        /// </summary>
        DontActivateRagdollFromSmokeGrenade,
        /// <summary>
        /// This <see cref="Ped"/> will attempt to match the speed of the owner while following its current scenario
        /// chain.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_LinkMBRToOwnerOnChain</c>` in the exe.
        /// </remarks>
        LinkMbrToOwnerOnChain,
        /// <summary>
        /// The player has walked into our ambient friend.
        /// </summary>
        AmbientFriendBumpedByPlayer,
        /// <summary>
        /// The player has driven into our ambient friend.
        /// </summary>
        AmbientFriendBumpedByPlayerVehicle,
        /// <summary>
        /// Player is playing the unholster transition in FPS mode.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_InFPSUnholsterTransition</c>` in the exe.
        /// </remarks>
        InFpsUnholsterTransition,
        /// <summary>
        /// Prevents the <see cref="Ped"/> from reacting to silenced bullets fired from network clone
        /// <see cref="Ped"/>s.
        /// </summary>
        PreventReactingToSilencedCloneBullets,
        /// <summary>
        /// Blocks <see cref="Ped"/> from creating the injured cry for help events (run over, tazed or melee would
        /// usually do this).
        /// </summary>
        DisableInjuredCryForHelpEvents,
        /// <summary>
        /// Prevents <see cref="Ped"/>s riding trains from getting off them.
        /// </summary>
        NeverLeaveTrain,
        /// <summary>
        /// When dead, don't drop equipped jetpack.
        /// </summary>
        DontDropJetpackOnDeath,
        /// <summary>
        /// Player is playing the unholster transition in FPS mode.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_UseFPSUnholsterTransitionDuringCombatRoll</c>` in the exe.
        /// </remarks>
        UseFpsUnholsterTransitionDuringCombatRoll,
        /// <summary>
        /// Player is exiting combat roll in FPS mode.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_ExitingFpsCombatRoll</c>` in the exe.
        /// </remarks>
        ExitingFpsCombatRoll,
        /// <summary>
        /// <see langword="true"/> when script is controlling the movement of the player.
        /// </summary>
        ScriptHasControlOfPlayer,
        /// <summary>
        /// <see langword="true"/> when we should be playing idle fidgets for projectiles.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_PlayFPSIdleFidgetsForProjectile</c>` in the exe.
        /// </remarks>
        PlayFpsIdleFidgetsForProjectile,
        /// <summary>
        /// Prevents <see cref="Ped"/> from auto-equipping helmets when entering a bike (includes quad bikes).
        /// </summary>
        DisableAutoEquipHelmetsInBikes,
        /// <summary>
        /// Prevents <see cref="Ped"/> from auto-equipping helmets when entering an aircraft.
        /// </summary>
        DisableAutoEquipHelmetsInAircraft,
        /// <summary>
        /// Was playing getup animations in FPS mode.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_WasPlayingFPSGetup</c>` in the exe.
        /// </remarks>
        WasPlayingFpsGetup,
        /// <summary>
        /// Was playing action result animations in FPS mode.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_WasPlayingFPSMeleeActionResult</c>` in the exe.
        /// </remarks>
        WasPlayingFpsMeleeActionResult,
        /// <summary>
        /// Unless scenario conditions apply, make this <see cref="Ped"/> go through normal deletion but not priority
        /// deletion.
        /// </summary>
        PreferNoPriorityRemoval,
        /// <summary>
        /// <see langword="true"/> when the FPS idle fidgets are aborted because the player fired the gun.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_FPSFidgetsAbortedOnFire</c>` in the exe.
        /// </remarks>
        FpsFidgetsAbortedOnFire,
        /// <summary>
        /// <see langword="true"/> when upper body anims are used during various tasks.
        /// </summary>
        /// <remarks>
        /// The original name is `<c>CPED_CONFIG_FLAG_ForceFPSIKWithUpperBodyAnim</c>` in the exe.
        /// </remarks>
        ForceFpsIKWithUpperBodyAnim,
        /// <summary>
        /// <see langword="true"/> we switch a character in first person mode (in `<c>CGameWorld::ChangePlayerPed</c>`).
        /// </summary>
        SwitchingCharactersInFirstPerson,
        /// <summary>
        /// <see langword="true"/> when the <see cref="Ped"/> is climbing a ladder.
        /// </summary>
        IsClimbingLadder,
        /// <summary>
        /// Flag to indicate that player has no shoes (used for third and first person aiming cameras and
        /// <see cref="Ped"/> follow camera).
        /// </summary>
        HasBareFeet,

        /*
         *  UNUSED_REPLACE_ME_2 takes 390 in `ePedConfigFlags` in `PedFlagsMeta.psc`
         */

        /// <summary>
        /// It will force the <see cref="Ped"/> to abandon its <see cref="Vehicle"/> (when using TaskGoToPointAnyMeans)
        /// if it is unable to get back to road.
        /// </summary>
        GoOnWithoutVehicleIfItIsUnableToGetBackToRoad = 391,
        /// <summary>
        /// Set by script to prevent <see cref="Ped"/>s from dropping snack health pickups on death
        /// (in <c>CPed::CreateDeadPedPickups</c>).
        /// </summary>
        BlockDroppingHealthSnacksOnDeath,
        /// <summary>
        /// Reset the <see cref="Ped"/>'s stored MyVehicle pointer when they leave their <see cref="Vehicle"/>.
        /// </summary>
        ResetLastVehicleOnVehicleExit,
        /// <summary>
        /// Forces threat response to melee actions from non friend to friend <see cref="Ped"/>s.
        /// With this flag, the <see cref="Ped"/> will ignore threat responses to melee actions where they are friendly
        /// with neither of source and target <see cref="Ped"/>s of the threat responses.
        /// </summary>
        ForceThreatResponseToNonFriendToFriendMeleeActions,
        /// <summary>
        /// Do not respond to <see cref="Ped"/>s damage where the inflictor's <see cref="EntityPopulationType"/> is
        /// one of the random types.
        /// </summary>
        DontRespondToRandomPedsDamage,
        /// <summary>
        /// Shares the same logic of <see cref="OnlyUpdateTargetWantedIfSeen"/> but will continue to check even after
        /// the initial wanted level is set.
        /// </summary>
        AllowContinuousThreatResponseWantedLevelUpdates,
        /// <summary>
        /// On mission state cleanup, which can be called with <see cref="Entity.MarkAsNoLongerNeeded"/>,
        /// the <see cref="Ped"/> will not set their target loss response to exit task.
        /// </summary>
        KeepTargetLossResponseOnCleanup,
        /// <summary>
        /// Similar to <see cref="DontDragMeOutCar"/> except it only prevents players from dragging
        /// the <see cref="Ped"/> out and allows AI to still do so.
        /// </summary>
        PlayersDontDragMeOutOfCar,
        /// <summary>
        /// Whenever the <see cref="Ped"/> starts shooting while going to a point, it triggers a responded to threat
        /// broadcast.
        /// </summary>
        /// <remarks>
        /// The original name is <c>CPED_CONFIG_FLAG_BroadcastRepondedToThreatWhenGoingToPointShooting</c> in the exe,
        /// but this enum uses the corrected name.
        /// </remarks>
        BroadcastRespondedToThreatWhenGoingToPointShooting,
        /// <summary>
        /// If this is set then `<c>CPedIntelligence::IsFriendlyWith</c>` will ignore the <see cref="Ped"/> type checks
        /// (i.e. two <see cref="PedType.Cop"/> <see cref="Ped"/>s are not automatically friendly).
        /// </summary>
        /// <remarks>
        /// Strictly, when either of 2 <see cref="Ped"/>s have this flag, neither of them will not automatically
        /// consider the other as friendly even if their <see cref="PedType"/>s are the same and are
        /// <see cref="PedType.Medic"/>, <see cref="PedType.Fire"/>, or <see cref="PedType.Cop"/>.
        /// <see cref="Ped"/>s' <see cref="Relationship"/> (acquaintance) must be set to
        /// <see cref="Relationship.Companion"/> or <see cref="Relationship.Respect"/> towards other <see cref="Ped"/>s,
        /// or must be in the same <see cref="PedGroup"/> as other <see cref="Ped"/>s, so they will be considered
        /// friendly with others (as the usual case).
        /// </remarks>
        IgnorePedTypeForIsFriendlyWith,
        /// <summary>
        /// Any non-friendly <see cref="Ped"/> will be considered as hated instead.
        /// </summary>
        TreatNonFriendlyAsHateWhenInCombat,
        /// <summary>
        /// Suppresses exit vehicle task being created in <c>CEventLeaderExitedCarAsDriver::CreateResponseTask</c>.
        /// <see cref="Ped"/> won't exit <see cref="Vehicle"/> if leader isn't in it as well.
        /// </summary>
        DontLeaveVehicleIfLeaderNotInVehicle,
        /// <summary>
        /// Change <see cref="Ped"/> to ambient pop type on migration.
        /// </summary>
        ChangeFromPermanentToAmbientPopTypeOnMigration,
        /// <summary>
        /// Allow melee reaction to come through even if proof is on.
        /// </summary>
        AllowMeleeReactionIfMeleeProofIsOn,
        /// <summary>
        /// <see cref="Ped"/> is playing lowrider lean animations due to <see cref="Vehicle"/> suspension modification.
        /// </summary>
        UsingLowriderLeans,
        /// <summary>
        /// <see cref="Ped"/> is playing alternate lowrider lean animations (ie arm on window) due to
        /// <see cref="Vehicle"/> suspension modification.
        /// </summary>
        UsingAlternateLowriderLeans,
        /// <summary>
        /// If this is set, the <see cref="Ped"/> won't be instantly killed if <see cref="Vehicle"/> is blown up (from
        /// <c>CAutomobile::BlowUpCar</c> -> <c>KillPedsInVehicle</c>). Instead, they will take normal explosive damage
        /// and be forced to exit the <see cref="Vehicle"/> if they're still alive.
        /// </summary>
        UseNormalExplosionDamageWhenBlownUpInVehicle,
        /// <summary>
        /// Blocks locking on of the <see cref="Vehicle"/> that the <see cref="Ped"/> is inside.
        /// </summary>
        DisableHomingMissileLockForVehiclePedInside,
        /// <summary>
        /// Same as CPED_RESET_FLAG_DisableTakeOffScubaGear but on a config flag.
        /// </summary>
        DisableTakeOffScubaGear,
        /// <summary>
        /// Melee fist weapons (ie knuckle duster) won't apply relative health damage scaler
        /// (<c>m_MeleeRightFistTargetHealthDamageScaler</c> in weapon info).
        /// </summary>
        IgnoreMeleeFistWeaponDamageMult,
        /// <summary>
        /// Law <see cref="Ped"/>s will be triggered to flee if player triggers an appropriate event (even if
        /// <see cref="Ped"/> is not wanted) instead of entering combat. NB: Only synced over the network when set on
        /// players.
        ///
        /// Allows law <see cref="Ped"/> to flee even if <see cref="Ped"/> is not wanted and
        /// <c>CWanted::m_AllRandomsFlee</c> is set.
        /// </summary>
        LawPedsCanFleeFromNonWantedPlayer,
        /// <summary>
        /// Forces security <see cref="Ped"/>s (not cop <see cref="Ped"/>s) to be blipped on the minimap if the player
        /// is wanted. Set on the local player.
        /// </summary>
        ForceBlipSecurityPedsIfPlayerIsWanted,
        /// <summary>
        /// <see cref="Ped"/> is running the swap weapon task and holstering the previous weapon, but has not started
        /// drawing the new one.
        /// </summary>
        IsHolsteringWeapon,
        /// <summary>
        /// Don't use nav mesh for navigating to scenario points. DLC Hack for yachts.
        /// </summary>
        UseGoToPointForScenarioNavigation,
        /// <summary>
        /// Don't clear local <see cref="Ped"/>'s wanted level when remote <see cref="Ped"/> in the same
        /// <see cref="Vehicle"/> has his wanted level cleared by script.
        /// </summary>
        DontClearLocalPassengersWantedLevel,
        /// <summary>
        /// Block auto weapon swaps for weapon pickups.
        /// </summary>
        BlockAutoSwapOnWeaponPickups,
        /// <summary>
        /// Increase AI targeting score for <see cref="Ped"/>s with this flag.
        /// </summary>
        ThisPedIsATargetPriorityForAI,
        /// <summary>
        /// Indicates that <see cref="Ped"/> is playing switch visor up/down anim.
        /// </summary>
        IsSwitchingHelmetVisor,
        /// <summary>
        /// Forces <see cref="Ped"/> to do a visor helmet switch if able to.
        /// </summary>
        ForceHelmetVisorSwitch,
        /// <summary>
        /// Indicates that <see cref="Ped"/> is performing vehicle melee action.
        /// </summary>
        IsPerformingVehicleMelee,
        /// <summary>
        /// Overrides <see cref="Ped"/> footstep particle effects with the global overriden footstep effect, which can
        /// be changed by <c>SET_PARTICLE_FX_FOOT_OVERRIDE_NAME</c>.
        /// </summary>
        UseOverrideFootstepPtFx,
        /// <summary>
        /// Disables vehicle combat.
        /// </summary>
        DisableVehicleCombat,
        /// <summary>
        /// Prevents this <see cref="Ped"/> from being locked on and blocks ability to damage / fire at
        /// <see cref="Ped"/>.
        /// </summary>
        TreatAsFriendlyForTargetingAndDamage,
        /// <summary>
        /// Allows <see cref="Ped"/>s on motorcycles to use the alternate animation set when available.
        /// </summary>
        /// <remarks>
        /// <see cref="Ped"/>s will not use the alternate animation when they have seat overrides set by a script, which
        /// can be set with `<c>SET_PED_IN_VEHICLE_CONTEXT</c>`, or when they are panicked or agitated.
        /// </remarks>
        AllowBikeAlternateAnimations,
        /// <summary>
        /// Prevents this <see cref="Ped"/> from being locked on and blocks ability to damage / fire at
        /// <see cref="Ped"/>. This flag is not synced, o will only work on machine that set it.
        /// </summary>
        TreatAsFriendlyForTargetingAndDamageNonSynced,
        /// <summary>
        /// When set, will attempt to use lockpick animations designed for Franklin in SP mode when breaking into
        /// a <see cref="Vehicle"/> (for forced entry).
        /// </summary>
        UseLockpickVehicleEntryAnimations,
        /// <summary>
        /// When set, player will be able to sprint inside interiors even if it is tagged to prevent it.
        /// </summary>
        IgnoreInteriorCheckForSprinting,
        /// <summary>
        /// When set, swat helicopters will spawn within last spotted location instead of actual <see cref="Ped"/>
        /// location (and target is a player).
        /// </summary>
        SwatHeliSpawnWithinLastSpottedLocation,
        /// <summary>
        /// prevents <see cref="Ped"/> from playing start engine anims (and turning engine on).
        /// </summary>
        DisableStartEngine,
        /// <summary>
        /// makes <see cref="Ped"/> ignore being on fire (fleeing, reacting to `<c>CEventOnFire</c>` event).
        /// </summary>
        IgnoreBeingOnFire,
        /// <summary>
        /// Disables turret seat and activity seat preference for vehicle entry for local player.
        /// </summary>
        DisableTurretOrRearSeatPreference,
        /// <summary>
        /// Will not spawn wanted helicopters to chase after this target.
        /// </summary>
        DisableWantedHelicopterSpawning,
        /// <summary>
        /// Will only create aimed at events if player is within normal perception of the target.
        /// </summary>
        UseTargetPerceptionForCreatingAimedAtEvents,
        /// <summary>
        /// Will prevent homing lockon on this <see cref="Ped"/>.
        /// </summary>
        DisableHomingMissileLockon,
        /// <summary>
        /// Ignore max number of active support combatants and let <see cref="Ped"/> join them as such.
        /// </summary>
        ForceIgnoreMaxMeleeActiveSupportCombatants,
        /// <summary>
        /// Will try to stay within set defensive area while driving a <see cref="Vehicle"/>.
        /// </summary>
        StayInDefensiveAreaWhenInVehicle,
        /// <summary>
        /// Will prevent the <see cref="Ped"/> from communicating target position to all other friendly
        /// <see cref="Ped"/>s.
        /// </summary>
        DontShoutTargetPosition,
        /// <summary>
        /// Will apply full headshot damage, regardless if <see cref="Ped"/> has a helmet (or armored one).
        /// </summary>
        DisableHelmetArmor,
        /// <summary>
        /// Marks a <see cref="Ped"/> that was created by concealed player from marked up scenarios.
        /// </summary>
        CreatedByConcealedPlayer,
        /// <summary>
        /// Synced and permanent version of <see cref="DisablePotentialToBeWalkedIntoResponse"/>.
        /// </summary>
        PermanentlyDisablePotentialToBeWalkedIntoResponse,
        /// <summary>
        /// Will prevent <see cref="Ped"/> from automatically being forced out of <see cref="Vehicle"/> due to weapon
        /// being invalid (e.g. turret seats after going into water).
        /// </summary>
        PreventVehExitDueToInvalidWeapon,
        /// <summary>
        /// Will ignore the friendly fire setting that was set by NETWORK_SET_FRIENDLY_FIRE_OPTION when checking if
        /// <see cref="Ped"/> can be damaged.
        /// </summary>
        IgnoreNetSessionFriendlyFireCheckForAllowDamage,
        /// <summary>
        /// Will make <see cref="Ped"/> stay in combat even if the player has targeting starts being attacked by cops.
        /// </summary>
        DontLeaveCombatIfTargetPlayerIsAttackedByPolice,
        /// <summary>
        /// Will check when entering a <see cref="Vehicle"/> if it is locked before warping.
        /// </summary>
        CheckLockedBeforeWarp,
        /// <summary>
        /// Will prevent a player from shuffling across to make room if another player is entering from the same side.
        /// </summary>
        DontShuffleInVehicleToMakeRoom,
        /// <summary>
        /// Will give the <see cref="Ped"/> a weapon to use once their weapon is removed for getups.
        /// </summary>
        GiveWeaponOnGetup,
        /// <summary>
        /// <see cref="Ped"/> fired projectiles will ignore the <see cref="Vehicle"/> they are in.
        /// </summary>
        DontHitVehicleWithProjectiles,
        /// <summary>
        /// Will prevent <see cref="Ped"/> from forcing entry into <see cref="Vehicle"/>s that are open from
        /// <c>TryLockedDoor</c> state.
        /// </summary>
        DisableForcedEntryForOpenVehiclesFromTryLockedDoor,
        /// <summary>
        /// Does nothing in SP game. In MP game, issues a <c>CEventNetworkFiredDummyProjectile</c> when
        /// <see cref="Ped"/>s with this flag fire projectiles (not limited to rockets).
        /// </summary>
        FiresDummyRockets,
        /// <summary>
        /// Is the <see cref="Ped"/> currently preforming an arrest.
        /// </summary>
        PedIsArresting,
        /// <summary>
        /// Will make this <see cref="Ped"/> a decoy <see cref="Ped"/> that will focus targeting.
        /// </summary>
        IsDecoyPed,
        /// <summary>
        /// This <see cref="Ped"/> has created a decoy.
        /// </summary>
        HasEstablishedDecoy,
        /// <summary>
        /// Will prevent dispatched helicopters from landing and dropping off <see cref="Ped"/>s.
        /// </summary>
        BlockDispatchedHelicoptersFromLanding,
        /// <summary>
        /// Will prevent <see cref="Ped"/>s from crying for help when shot with the stun gun.
        /// </summary>
        DontCryForHelpOnStun,
        /// <summary>
        /// Tranq weapons are handled differently in terms of damage. This triggers that logic.
        /// </summary>
        HitByTranqWeapon,
        /// <summary>
        /// If set, the <see cref="Ped"/> may be incapacitated.
        /// </summary>
        CanBeIncapacitated,
        /// <summary>
        /// If set, we will always behave like we have the aim trigger pressed.
        /// </summary>
        ForcedAimFromArrest,
        /// <summary>
        /// If set, we will not set a new target after a melee attack.
        /// </summary>
        DontChangeTargetFromMelee,
        /// <summary>
        /// Used to disable health regeneration when damaged with the stun gun in MP.
        /// </summary>
        DisableHealthRegenerationWhenStunned,
        /// <summary>
        /// Prevents a dead <see cref="Ped"/> from sinking.
        /// </summary>
        RagdollFloatsIndefinitely,
        /// <summary>
        /// Blocks electric weapon damage.
        /// </summary>
        BlockElectricWeaponDamage,
    }
}
