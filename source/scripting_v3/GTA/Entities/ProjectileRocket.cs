//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using GTA.Math;
using SHVDN;

namespace GTA
{
    /// <summary>
    /// Represents a rocket projectile, which is for `<c>CProjectileRocket</c>`.
    /// </summary>
    public sealed class ProjectileRocket : Projectile
    {
        internal ProjectileRocket(int handle) : base(handle)
        {
        }

        /// <summary>
        /// Gets or sets the homing target.
        /// </summary>
        public Entity Target
        {
            get
            {
                IntPtr address = NativeMemory.GetEntityAddress(Handle);
                if (address == IntPtr.Zero)
                {
                    return null;
                }

                return Entity.FromHandle(NativeMemory.GetTargetEntityOfCProjectileRocket(address));
            }

            set
            {
                IntPtr address = NativeMemory.GetEntityAddress(Handle);
                if (address == IntPtr.Zero)
                {
                    return;
                }

                IntPtr targetAddress = IntPtr.Zero;
                if (value != null)
                {
                    targetAddress = NativeMemory.GetEntityAddress(value.Handle);
                    if (targetAddress == IntPtr.Zero)
                    {
                        return;
                    }
                }

                NativeMemory.AssignToFwRegdRef(address + NativeMemory.ProjectileRocketTargetOffset, targetAddress);
            }
        }

        /// <summary>
        /// Gets or sets the cached homing target position when the projectile is not homing <see cref="Target"/>
        /// accurately for <see cref="IsAccurate"/> being not set.
        /// </summary>
        public Vector3 CachedTargetPosition
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketCachedTargetPosOffset == 0)
                {
                    return Vector3.Zero;
                }

                return new Vector3(MemDataMarshal.ReadVector3(address + NativeMemory.ProjectileRocketCachedTargetPosOffset));
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketCachedTargetPosOffset == 0)
                {
                    return;
                }

                MemDataMarshal.WriteVector3(address + NativeMemory.ProjectileRocketCachedTargetPosOffset,
                    value.ToInternalFVector3());
            }
        }

        /// <summary>
        /// Gets or sets the launch direction.
        /// The <see cref="ProjectileRocket"/> lerps to this direction first until it lerps close enough.
        /// </summary>
        public Vector3 LaunchDirection
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketLaunchDirOffset == 0)
                {
                    return Vector3.Zero;
                }

                return new Vector3(MemDataMarshal.ReadVector3(address + NativeMemory.ProjectileRocketLaunchDirOffset));
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketLaunchDirOffset == 0)
                {
                    return;
                }

                MemDataMarshal.WriteVector3(address + NativeMemory.ProjectileRocketLaunchDirOffset,
                    value.ToInternalFVector3());
            }
        }

        /// <summary>
        /// Gets or sets the flight model input of this <see cref="ProjectileRocket"/> as a <see cref="Vector3"/>.
        /// </summary>
        /// <remarks>
        /// If you are accessing subset of flight model input values and concern potential performance loss when
        /// the property has to access 2 cache lines, use individual flight model input such as
        /// <see cref="FlightModelInputPitch"/>. You might want to note that the address of
        /// <see cref="FlightModelInputYaw"/> can be in a different cache line from one where
        /// <see cref="FlightModelInputPitch"/> and <see cref="FlightModelInputRoll"/> are.
        /// </remarks>
        public Vector3 FlightModelInput
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketFlightModelInputPitchOffset == 0)
                {
                    return Vector3.Zero;
                }

                return new Vector3(MemDataMarshal.ReadVector3(address + NativeMemory.ProjectileRocketFlightModelInputPitchOffset));
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketFlightModelInputPitchOffset == 0)
                {
                    return;
                }

                MemDataMarshal.WriteVector3(address + NativeMemory.ProjectileRocketFlightModelInputPitchOffset,
                    value.ToInternalFVector3());
            }
        }

        /// <summary>
        /// Gets or sets the pitch of flight model input of this <see cref="ProjectileRocket"/>.
        /// </summary>
        public float FlightModelInputPitch
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketFlightModelInputPitchOffset == 0)
                {
                    return 0;
                }

                return MemDataMarshal.ReadFloat(address + NativeMemory.ProjectileRocketFlightModelInputPitchOffset);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketFlightModelInputPitchOffset == 0)
                {
                    return;
                }

                MemDataMarshal.WriteFloat(address + NativeMemory.ProjectileRocketFlightModelInputPitchOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the roll of flight model input of this <see cref="ProjectileRocket"/>.
        /// </summary>
        public float FlightModelInputRoll
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketFlightModelInputRollOffset == 0)
                {
                    return 0;
                }

                return MemDataMarshal.ReadFloat(address + NativeMemory.ProjectileRocketFlightModelInputRollOffset);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketFlightModelInputRollOffset == 0)
                {
                    return;
                }

                MemDataMarshal.WriteFloat(address + NativeMemory.ProjectileRocketFlightModelInputRollOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the yaw of flight model input of this <see cref="ProjectileRocket"/>.
        /// </summary>
        public float FlightModelInputYaw
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketFlightModelInputYawOffset == 0)
                {
                    return 0;
                }

                return MemDataMarshal.ReadFloat(address + NativeMemory.ProjectileRocketFlightModelInputYawOffset);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketFlightModelInputYawOffset == 0)
                {
                    return;
                }

                MemDataMarshal.WriteFloat(address + NativeMemory.ProjectileRocketFlightModelInputYawOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the time before homing in seconds.
        /// </summary>
        public float TimeBeforeHoming
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketTimeBeforeHomingOffset == 0)
                {
                    return 0;
                }

                return MemDataMarshal.ReadFloat(address + NativeMemory.ProjectileRocketTimeBeforeHomingOffset);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketTimeBeforeHomingOffset == 0)
                {
                    return;
                }

                MemDataMarshal.WriteFloat(address + NativeMemory.ProjectileRocketTimeBeforeHomingOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the time before homing stops ignoring angle break in seconds.
        /// If set to zero (or less) and the <see cref="ProjectileRocket"/> is homing, it should only home
        /// <see cref="Target"/> is within a certain angle threshold.
        /// </summary>
        public float TimeBeforeHomingAngleBreak
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketTimeBeforeHomingAngleBreakOffset == 0)
                {
                    return 0;
                }

                return MemDataMarshal.ReadFloat(address + NativeMemory.ProjectileRocketTimeBeforeHomingAngleBreakOffset);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketTimeBeforeHomingAngleBreakOffset == 0)
                {
                    return;
                }

                MemDataMarshal.WriteFloat(address + NativeMemory.ProjectileRocketTimeBeforeHomingAngleBreakOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the speed this <see cref="ProjectileRocket"/> launched and should maintain in m/s.
        /// </summary>
        public float LauncherSpeed
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketLauncherSpeedOffset == 0)
                {
                    return 0;
                }

                return MemDataMarshal.ReadFloat(address + NativeMemory.ProjectileRocketLauncherSpeedOffset);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketLauncherSpeedOffset == 0)
                {
                    return;
                }

                MemDataMarshal.WriteFloat(address + NativeMemory.ProjectileRocketLauncherSpeedOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the time since launch that in seconds. If this value is less than the `SeparationTime` of
        /// the `<c>CAmmoRocketInfo</c>` that this <see cref="ProjectileRocket"/> uses,
        /// the <see cref="ProjectileRocket"/> will receive a small gravity force instead of the thrust force.
        /// </summary>
        public float TimeSinceLaunch
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketTimeSinceLaunchOffset == 0)
                {
                    return 0;
                }

                return MemDataMarshal.ReadFloat(address + NativeMemory.ProjectileRocketTimeSinceLaunchOffset);
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketTimeSinceLaunchOffset == 0)
                {
                    return;
                }

                MemDataMarshal.WriteFloat(address + NativeMemory.ProjectileRocketTimeSinceLaunchOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets whether this <see cref="ProjectileRocket"/> is accurately homing, which is set when the player
        /// locks on a <see cref="Entity"/> and fires the weapon.
        /// </summary>
        public bool IsAccurate
        {
            get => GetProjectileRocketFlag(0);
            set => SetProjectileRocketFlag(0, value);
        }

        /// <summary>
        /// Gets or sets whether this <see cref="ProjectileRocket"/> is lerping to the launch direction, which is set
        /// in `<c>CProjectileRocket::Fire</c>`.
        /// </summary>
        public bool LerpToLaunchDirection
        {
            get => GetProjectileRocketFlag(1);
            set => SetProjectileRocketFlag(1, value);
        }

        /// <summary>
        /// Gets or sets whether this <see cref="ProjectileRocket"/> should apply thrust.
        /// Sets to <see langword="false"/> when the <see cref="ProjectileRocket"/> goes into water if
        /// `<c>CProjectileRocket::ProcessPhysics</c>` is controlling the physics and <c>`ThrustUnderwater`</c> is
        /// not set in `<c>ProjectileFlags</c>` of the projectile's <c>`CAmmoRocketInfo`</c>.
        /// </summary>
        public bool ApplyThrust
        {
            get => GetProjectileRocketFlag(2);
            set => SetProjectileRocketFlag(2, value);
        }

        /// <summary>
        /// Gets or sets whether this <see cref="ProjectileRocket"/> is locked on with an on-foot homing weapon
        /// that fired the <see cref="ProjectileRocket"/>. `<c>OnFootHoming</c>` should be set in
        /// `<c>WeaponFlags</c>` of `<c>CWeaponInfo</c>` (in a `weapons.meta`), so the internal functions of
        /// `<c>CProjectileRocket</c>` can work to home <see cref="Target"/> as intended.
        /// </summary>
        public bool OnFootHomingWeaponLockedOn
        {
            get => GetProjectileRocketFlag(3);
            set => SetProjectileRocketFlag(3, value);
        }

        /// <summary>
        /// Gets or sets whether this <see cref="ProjectileRocket"/> was homing <see cref="Target"/> at any time.
        /// </summary>
        public bool WasHoming
        {
            get => GetProjectileRocketFlag(4);
            set => SetProjectileRocketFlag(4, value);
        }

        /// <summary>
        /// Gets or sets whether this <see cref="ProjectileRocket"/> has stopped homing <see cref="Target"/>.
        /// </summary>
        public bool StopHoming
        {
            get => GetProjectileRocketFlag(5);
            set => SetProjectileRocketFlag(5, value);
        }

        /// <summary>
        /// <para>
        /// Gets whether this <see cref="ProjectileRocket"/> is redirected to a <see cref="Projectile"/> attractor
        /// whose `<c>CAmmoProjectileInfo</c>` (or one of its subclasses `<c>CAmmoRocketInfo</c>` or
        /// `<c>CAmmoThrownInfo</c>`) has `<c>HomingAttractor</c>` flag in `<c>ProjectileFlags</c>`.
        /// </para>
        /// <para>
        /// Not available in v1.0.1032.1 and earlier versions.
        /// </para>
        /// </summary>
        public bool IsRedirected
        {
            get => GetProjectileRocketFlag(6);
            // Maybe we should have our `OnFootHomingWeaponLockedOn` set true in our setter when
            // `CWeaponInfo::GetIsOnFootHoming()` (where the instance is retrieved by
            // `CProjectile::m_uWeaponFiredFromHash`) returns true, just like how `CProjectileRocket::SetIsRedirected`
            // does.
        }

        /*
         * We're 100% sure we should not mess with `m_bTorpHasBeenOutOfWater` (for 8th bit, assumed to exist since
         * b2189 where Kosatka is introduced). It switches on when the projectile gets in and out of water, but both
         * happens only if both `ThrustUnderwater` and `UseGravityOutOfWater` are set in `ProjectileFlags` of
         * the projectile's `CAmmoRocketInfo`.
         */

        /// <summary>
        /// Gets or sets the cached direction toward <see cref="Target"/>.
        /// </summary>
        public Vector3 CachedDirection
        {
            get
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketCachedDirectionOffset == 0)
                {
                    return Vector3.Zero;
                }

                return new Vector3(MemDataMarshal.ReadVector3(address + NativeMemory.ProjectileRocketCachedDirectionOffset));
            }
            set
            {
                IntPtr address = MemoryAddress;
                if (address == IntPtr.Zero || NativeMemory.ProjectileRocketCachedDirectionOffset == 0)
                {
                    return;
                }

                MemDataMarshal.WriteVector3(address + NativeMemory.ProjectileRocketCachedDirectionOffset,
                    value.ToInternalFVector3());
            }
        }

        /// <summary>
        /// Get a <see cref="ProjectileRocket"/> instance by its handle.
        /// </summary>
        /// <param name="handle">The handle to test.</param>
        /// <returns>
        /// A <see cref="ProjectileRocket"/> if the handle is assigned for a <see cref="ProjectileRocket"/>;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        public static new ProjectileRocket FromHandle(int handle)
        {
            IntPtr address = SHVDN.NativeMemory.GetEntityAddress(handle);
            if (address == IntPtr.Zero
                || (EntityTypeInternal)SHVDN.MemDataMarshal.ReadByte(address + 0x28) != EntityTypeInternal.Object)
            {
                return null;
            }

            // `CObject::GetAsCProjectileRocket` should return the same address if the vfunc is overridden by
            // `CProjectileRocket`'s implementation and it should return false otherwise
            return SHVDN.NativeMemory.GetAsCProjectileRocket(address) != address ? null : new ProjectileRocket(handle);
        }

        private bool GetProjectileRocketFlag(byte bit)
        {
            IntPtr address = MemoryAddress;
            if (address == IntPtr.Zero || NativeMemory.ProjectileRocketFlagsOffset == 0)
            {
                return false;
            }

            return MemDataMarshal.IsBitSet(address + NativeMemory.ProjectileRocketFlagsOffset, bit);
        }
        private void SetProjectileRocketFlag(byte bit, bool value)
        {
            IntPtr address = MemoryAddress;
            if (address == IntPtr.Zero || NativeMemory.ProjectileRocketFlagsOffset == 0)
            {
                return;
            }

            MemDataMarshal.SetBit(address + NativeMemory.ProjectileRocketFlagsOffset, bit, value);
        }
    }
}
