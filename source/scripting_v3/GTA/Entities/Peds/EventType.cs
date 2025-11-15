//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    /// <summary>
    /// An enumeration of all possible event types except for network and error events.
    /// </summary>
    public enum EventType
    {
        AcquaintancePedDislike,
        AcquaintancePedHate,
        AcquaintancePedLike,
        AcquaintancePedRespect,
        AcquaintancePedWanted,
        AcquaintancePedDead,
        Agitated,
        AgitatedAction,
        EncroachingPed,
        CallForCover,
        CarUndriveable,
        ClimbLadderOnRoute,
        ClimbNavmeshOnRoute,
        CombatTaunt,
        CommunicateEvent,
        CopCarBeingStolen,
        CrimeReported,
        Damage,
        DeadPedFound,
        Death,
        DraggedOutCar,
        DummyConversion,
        Explosion,
        ExplosionHeard,
        FireNearby,
        FlushTasks,
        FootStepHeard,
        GetOutOfWater,
        GivePedTask,
        GunAimedAt,
        HelpAmbientFriend,
        InjuredCryForHelp,
        CrimeCryForHelp,
        InAir,
        InWater,
        /// <summary>
        /// Not available in 1.0.1737.6 or earlier.
        /// </summary>
        Incapacitated,
        LeaderEnteredCarAsDriver,
        LeaderEnteredCover,
        LeaderExitedCarAsDriver,
        LeaderHolsteredWeapon,
        LeaderLeftCover,
        LeaderUnholsteredWeapon,
        MeleeAction,
        MustLeaveBoat,
        NewTask,
        None,
        ObjectCollision,
        OnFire,
        OpenDoor,
        ShovePed,
        PedCollisionWithPed,
        PedCollisionWithPlayer,
        PedEnteredMyVehicle,
        PedJackingMyVehicle,
        PedOnCarRoof,
        PedToChase,
        PedToFlee,
        PlayerCollisionWithPed,
        PlayerLockOnTarget,
        PotentialBeWalkedInto,
        PotentialBlast,
        PotentialGetRunOver,
        PotentialWalkIntoFire,
        PotentialWalkIntoObject,
        PotentialWalkIntoVehicle,
        ProvidingCover,
        RadioTargetPosition,
        RanOverPed,
        ReactionCombatVictory,
        ReactionEnemyPed,
        ReactionInvestigateDeadPed,
        ReactionInvestigateThreat,
        RequestHelpWithConfrontation,
        RespondedToThreat,
        Revived,
        ScriptCommand,
        /// <summary>
        /// Not available in 1.0.1737.6 or earlier.
        /// </summary>
        ShockingBrokenGlass,
        ShockingCarAlarm,
        ShockingCarChase,
        ShockingCarCrash,
        ShockingBicycleCrash,
        ShockingCarPileUp,
        ShockingCarOnCar,
        ShockingDangerousAnimal,
        ShockingDeadBody,
        ShockingDrivingOnPavement,
        ShockingBicycleOnPavement,
        ShockingEngineRevved,
        ShockingExplosion,
        ShockingFire,
        ShockingGunFight,
        ShockingGunshotFired,
        ShockingHelicopterOverhead,
        ShockingParachuterOverhead,
        ShockingPedKnockedIntoByPlayer,
        ShockingHornSounded,
        ShockingInDangerousVehicle,
        ShockingInjuredPed,
        ShockingMadDriver,
        ShockingMadDriverExtreme,
        ShockingMadDriverBicycle,
        ShockingMugging,
        ShockingNonViolentWeaponAimedAt,
        ShockingPedRunOver,
        ShockingPedShot,
        ShockingPlaneFlyBy,
        ShockingPotentialBlast,
        ShockingPropertyDamage,
        ShockingRunningPed,
        ShockingRunningStampede,
        ShockingSeenCarStolen,
        ShockingSeenConfrontation,
        ShockingSeenGangFight,
        ShockingSeenInsult,
        ShockingSeenMeleeAction,
        ShockingSeenNiceCar,
        ShockingSeenPedKilled,
        ShockingSeenVehicleTowed,
        ShockingSeenWeaponThreat,
        ShockingSeenWeirdPed,
        ShockingSeenWeirdPedApproaching,
        ShockingSiren,
        ShockingStudioBomb,
        ShockingVisibleWeapon,
        ShotFired,
        ShotFiredBulletImpact,
        ShotFiredWhizzedBy,
        FriendlyAimedAt,
        FriendlyFireNearMiss,
        ShoutBlockingLos,
        ShoutTargetPosition,
        StaticCountReachedMax,
        StuckInAir,
        SuspiciousActivity,
        Switch2NmTask,
        UnidentifiedPed,
        VehicleCollision,
        VehicleDamageWeapon,
        VehicleOnFire,
        WhistlingHeard,
        Disturbance,
        EntityDamaged,
        EntityDestroyed,
        Writhe,
        HurtTransition,
        PlayerUnableToEnterVehicle,
        ScenarioForceAction,
        StatValueChanged,
        PlayerDeath,
        PedSeenDeadPed,
        Invalid = -1,
    }

    internal static class EventTypeExtensions
    {
        internal static int GetValue(this EventType type)
        {
            int value = (int)type;

            if (Game.FileVersion >= VersionConstsForGameVersion.v1_0_1868_0)
            {
                return value;
            }

            if (type is EventType.Incapacitated or EventType.ShockingBrokenGlass)
            {
                ThrowHelper.ThrowArgumentException(
                    $"{nameof(EventType)}.{type} is not available in game versions prior to v1.0.1868.0.",
                    nameof(type));
            }

            if (value >= (int)EventType.ShockingCarAlarm)
            {
                return value - 2;
            }

            if (value >= (int)EventType.LeaderEnteredCarAsDriver)
            {
                return value - 1;
            }

            return value;
        }
    }
}
