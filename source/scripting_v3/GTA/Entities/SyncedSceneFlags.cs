//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
    /// <summary>
    /// An enumeration of possible flags used for the synchronized scene task `<c>CTaskSynchronizedScene</c>`.
    /// </summary>
    [Flags]
    public enum SyncedSceneFlags
    {
        None = 0,
        /// <summary>
        /// When this flag is set, the task will run in kinematic physics mode (other entities will collide, and be
        /// pushed out of the way).
        /// </summary>
        UseKinematicPhysics = 1,
        /// <summary>
        /// When this flag is set, The task will do a tag synchronized blend out with the movement behaviour of
        /// the <see cref="Ped"/>.
        /// </summary>
        TagSyncOut = 2,
        /// <summary>
        /// When this flag is set, the scene will not be interrupted by AI events like falling, entering water / etc.
        /// Also blocks all weapon reactions / etc.
        /// </summary>
        DontInterrupt = 4,
        /// <summary>
        /// When this flag is set, the entire scene will be stopped if this task is aborted.
        /// </summary>
        OnAbortStopScene = 8,
        /// <summary>
        /// When this flag is set, the task will end if the <see cref="Ped"/> takes weapon damage.
        /// </summary>
        AbortOnWeaponDamage = 16,
        /// <summary>
        /// When this flag is set, the task will not update the mover. Can be used to include vehicles in a synced
        /// scene when the scene is attached to them.
        /// </summary>
        BlockMoverUpdate = 32,
        /// <summary>
        /// When this flag is set, the tasks with anims shorter than the scene will loop while the scene is playing.
        /// </summary>
        LoopWithinScene = 64,
        /// <summary>
        /// When this flag is set, the task will preserve its velocity on clean up (must be using physics).
        /// </summary>
        PreserveVelocity = 128,
        /// <summary>
        /// When this flag is set, the <see cref="Ped"/> capsule will attempt to expand to cover the skeleton during
        /// playback.
        /// </summary>
        ExpandPedCapsuleFromSkeleton = 256,
        /// <summary>
        /// When this flag is set, the <see cref="Ped"/> will switch to ragdoll and fall down on making contact with
        /// a physical object (other than flat ground).
        /// </summary>
        ActivateRagdollOnCollision = 512,
        /// <summary>
        /// When this flag is set, the <see cref="Ped"/>'s weapon will be hidden while the scene is playing.
        /// </summary>
        HideWeapon = 1024,
        /// <summary>
        /// When this flag is set, the task will end if the <see cref="Ped"/> dies.
        /// </summary>
        AbortOnDeath = 2048,
        /// <summary>
        /// When running the scene on a vehicle, allow the scene to abort if the vehicle takes a heavy collision from
        /// another vehicle.
        /// </summary>
        VehicleAbortOnLargeImpact = 4096,
        /// <summary>
        /// When running the scene on a vehicle, allow player <see cref="Ped"/>s to enter the vehicle.
        /// </summary>
        VehicleAllowPlayerEntry = 8192,
        /// <summary>
        /// When this flag is set, process the attachments at the start of the scene.
        /// </summary>
        ProcessAttachmentsOnStart = 16384,
        /// <summary>
        /// When this flag is set, a non-<see cref="Ped"/> entity will be returned to their starting position if the
        /// scene finishes early.
        /// </summary>
        NetOnEarlyNonPedStopReturnToStart = 32768,
        /// <summary>
        /// When this flag is set, the <see cref="Ped"/> will be automatically set out of their <see cref="Vehicle"/>
        /// at the start of the scene.
        /// </summary>
        SetPedOutOfVehicleAtStart = 65536,
        /// <summary>
        /// When this flag is set, the attachment checks done in `<c>CNetworkSynchronisedScenes::Update</c>` when
        /// pending start will be disregarded.
        /// </summary>
        NetDisregardAttachmentChecks = 131072,
    }
}
