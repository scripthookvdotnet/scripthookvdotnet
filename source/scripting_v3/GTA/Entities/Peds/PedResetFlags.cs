//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;
using SHVDN;

namespace GTA
{
    /// <summary>
    /// Represents a wrapper class for `<c>CPedResetFlags</c>` (not `<c>ePedResetFlags</c>`).
    /// The class calls its reset methods every frame, where the members that tracks the remaining numbers of frames
    /// get decremented and most of the other values are reset to appropriate values.
    /// </summary>
    public sealed class PedResetFlags
    {
        #region Fields
        readonly Ped _ped;
        #endregion

        internal PedResetFlags(Ped ped)
        {
            _ped = ped;
        }

        /// <summary>
        /// Gets or sets the number of frame where the <see cref="Ped"/> is getting pushed out of the way by the player
        /// pushing a door.
        /// </summary>
        public byte NumFramesToBeKnockedByDoor
        {
            get
            {
                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return 0;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0;
                }

                return MemDataMarshal.ReadByte(address + NativeMemory.Ped.CPed__PedResetFlagsOffset);
            }
            set
            {
                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                SHVDN.MemDataMarshal.WriteByte(address + NativeMemory.Ped.CPed__PedResetFlagsOffset, value);
            }
        }

        /*
         * There's the following members between `m_nIsInCover` and `m_fEntityZFromGroundZHeight` in
         * `CPedResetFlag`:
         * ```
         * // Rotation modifier, which is used to allow tasks to override the amount of rotation applied from anims.
         * // Resets each frame to 1.
         * float m_fAnimRotationModifier;
         *
         * // Root correction modifier, how much of the root correction is applied to the ped.
         * // Resets to 1 each frame.
         * float m_fRootCorrectionModifer;
         *
         * // Overall control of speed at which movement anims play.
         * // Default is 1.0f.
         * float m_fMoveAnimRate;
         *
         * // Modifier set each frame by script, sets a distance a ped should be between seats to be applied when in
         * // a vehicle.
         * float m_fScriptedScaleBetweenSeatsDefaultDistance;
         * ```
         *
         * `SET_SCRIPTED_ANIM_SEAT_OFFSET` calls `SetScriptedScaleBetweenSeatsDefaultDistance(const float f)`, but
         * There's no `GetScriptedScaleBetweenSeatsDefaultDistance()` calls in the game, and thus there's no need to
         * provide our property for `m_fScriptedScaleBetweenSeatsDefaultDistance` in SHVDN until the game start using
         * the getter method.
         * For `m_fAnimRotationModifier`, `m_fRootCorrectionModifer`, and `m_fMoveAnimRate`, the game doesn't actually
         * call any of the 3 public setter methods at all.
         */

        /// <summary>
        /// Gets or sets the number of frames where the game can snap the height of the ped to the correct distance
        /// above the ground.
        /// </summary>
        public byte NumFramesToSetEntityZFromGround
        {
            get
            {
                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return 0;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 1);
                return MemDataMarshal.ReadByte(address + offset);
            }
            set
            {
                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 1);
                SHVDN.MemDataMarshal.WriteByte(address + offset, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of frames the <see cref="Ped"/> should not accept any IK look ats.
        /// The value takes any number between 0 and 3.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value is not within the range of 0 to 3. Can only be thrown from the setter.
        /// </exception>
        public uint NumFramesNotToAcceptIKLookAts
        {
            get
            {
                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return 0;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 4);
                return MemDataMarshal.ReadUInt32BitField(address + offset, 0, 2);
            }
            set
            {
                ThrowHelper.CheckArgumentRange(nameof(value), value, 0, 3);

                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 4);
                MemDataMarshal.WriteBitFieldAsUInt32(address + offset, value, 0, 2);
            }
        }

        /// <summary>
        /// Gets or sets the number of frames the <see cref="Ped"/> should not accept script IK look ats.
        /// The value takes any number between 0 and 3.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value is not within the range of 0 to 3. Can only be thrown from the setter.
        /// </exception>
        /// <remarks>
        /// This property having a value more than zero does not prevent the <see cref="Ped"/> from looking at
        /// initiated by a script, or by some of the game code that's not called via native functions.
        /// </remarks>
        public uint NumFramesNotToAcceptCodeIKLookAts
        {
            get
            {
                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return 0;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 4);
                return MemDataMarshal.ReadUInt32BitField(address + offset, 2, 2);
            }
            set
            {
                ThrowHelper.CheckArgumentRange(nameof(value), value, 0, 3);

                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 4);
                MemDataMarshal.WriteBitFieldAsUInt32(address + offset, value, 2, 2);
            }
        }

        /// <summary>
        /// Gets or sets the number of frames the <see cref="Ped"/> should be considered to have just left
        /// a <see cref="Vehicle"/>. The value takes any number between 0 and 15.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value is not within the range of 0 to 15. Can only be thrown from the setter.
        /// </exception>
        public uint NumFramesToConsiderJustLeftVehicle
        {
            get
            {
                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return 0;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 4);
                return MemDataMarshal.ReadUInt32BitField(address + offset, 4, 4);
            }
            set
            {
                ThrowHelper.CheckArgumentRange(nameof(value), value, 0, 15);

                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 4);
                MemDataMarshal.WriteBitFieldAsUInt32(address + offset, value, 4, 4);
            }
        }

        /// <summary>
        /// Gets or sets the number of frames the <see cref="Ped"/> should be considered to have just left
        /// a <see cref="Vehicle"/>. The value takes any number between 0 and 3.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value is not within the range of 0 to 3. Can only be thrown from the setter.
        /// </exception>
        public uint NumFramesToConsiderInCover
        {
            get
            {
                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return 0;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 4);
                return MemDataMarshal.ReadUInt32BitField(address + offset, 8, 2);
            }
            set
            {
                ThrowHelper.CheckArgumentRange(nameof(value), value, 0, 3);

                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 4);
                MemDataMarshal.WriteBitFieldAsUInt32(address + offset, value, 8, 2);
            }
        }

        /// <summary>
        /// Gets or sets the absolute Z coordinate of the ground height that determines if the <see cref="Ped"/> should
        /// be snapped to the ground. If close enough and <see cref="NumFramesToSetEntityZFromGround"/> is more than
        /// zero, the <see cref="Ped"/> will be snapped to the ground.
        /// </summary>
        public float EntityZFromGroundZHeight
        {
            get
            {
                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return 0;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 24);
                return MemDataMarshal.ReadFloat(address + offset);
            }
            set
            {
                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 24);
                MemDataMarshal.WriteFloat(address + offset, value);
            }
        }

        /// <summary>
        /// Gets or sets the Z coordinate threshold from the ground height that determines if the <see cref="Ped"/>
        /// should be snapped to the ground. If close enough and <see cref="NumFramesToSetEntityZFromGround"/> is more
        /// than zero, the <see cref="Ped"/> will be snapped to the ground.
        /// </summary>
        public float EntityZFromGroundZThreshold
        {
            get
            {
                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return 0;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return 0;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 28);
                return MemDataMarshal.ReadFloat(address + offset);
            }
            set
            {
                if (NativeMemory.Ped.CPed__PedResetFlagsOffset == 0)
                {
                    return;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return;
                }

                int offset = (NativeMemory.Ped.CPed__PedResetFlagsOffset + 28);
                MemDataMarshal.WriteFloat(address + offset, value);
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the head IK is blocked for both game code and scripts in
        /// this <see cref="Ped"/>.
        /// </summary>
        public bool IsHeadIKBlocked => NumFramesNotToAcceptIKLookAts > 0;

        /// <summary>
        /// Gets a value that indicates whether the head IK is blocked for scripts in this <see cref="Ped"/>.
        /// </summary>
        public bool IsCodeHeadIKBlocked => NumFramesNotToAcceptCodeIKLookAts > 0;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Ped"/> should be considered as have just left
        /// a <see cref="Vehicle"/> by testing if <see cref="NumFramesToConsiderInCover"/> is not zero.
        /// </summary>
        public bool HasJustLeftVehicle => NumFramesToConsiderJustLeftVehicle > 0;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Ped"/> is currently in cover by testing
        /// if <see cref="NumFramesToConsiderInCover"/> is not zero.
        /// </summary>
        /// <remarks>
        /// While how this property is the same as how `<c>bool CPed::GetIsInCover() const</c>` returns true,
        /// it is not the same as how SHVDN's <see cref="Ped.IsInCover"/> returns true, which tests if
        /// a `<c>TaskCover</c>` is running on the <see cref="Ped"/>'s intelligence.
        /// </remarks>
        public bool IsInCover => NumFramesToConsiderInCover > 0;

        /// <summary>
        /// Sets the head IK of this <see cref="Ped"/> as blocked.
        /// </summary>
        /// <remarks>
        /// Sets <see cref="NumFramesNotToAcceptIKLookAts"/> to the max value, which is 3 at least in the game versions
        /// between v1.0.372.2 and v1.0.3179.0.
        /// </remarks>
        public void SetIsHeadIKBlocked()
        {
            NumFramesNotToAcceptIKLookAts = 3;
        }

        /// <summary>
        /// Sets the head IK of this <see cref="Ped"/> as blocked for scripts.
        /// </summary>
        /// <remarks>
        /// Sets <see cref="NumFramesNotToAcceptIKLookAts"/> to the max value, which is 3 at least in the game versions
        /// between v1.0.372.2 and v1.0.3179.0.
        /// </remarks>
        public void SetIsCodeHeadIKBlocked()
        {
            NumFramesNotToAcceptCodeIKLookAts = 3;
        }

        /// <summary>
        /// Sets the Z coordinate of the ground height and the threshold, which determine if the <see cref="Ped"/>
        /// should be snapped to the ground.
        /// </summary>
        /// <param name="height">The absolute Z coordinate of the ground where the <see cref="Ped"/> is on.</param>
        /// <param name="threshold">The threshold to determine if the <see cref="Ped"/> should be snapped.</param>
        /// <remarks>
        /// <see cref="NumFramesToSetEntityZFromGround"/> should be more than zero to snap before calling this method.
        /// If the property gets set to zero in the internal reset function, what you set via this method will also be
        /// reset.
        /// </remarks>
        public void SetEntityZFromGroundZHeight(float height, float threshold = 1.0f)
        {
            EntityZFromGroundZHeight = height;
            EntityZFromGroundZThreshold = threshold;
        }

        /// <summary>
        /// Gets the value of a reset flag toggle on this <see cref="Ped"/>.
        /// You will need to call this method every frame you want to get, since the values of
        /// <see cref="PedConfigFlagToggles"/> are reset every frame.
        /// </summary>
        public bool GetResetFlag(PedResetFlagToggles resetFlag)
        {
            return Function.Call<bool>(Hash.GET_PED_RESET_FLAG, _ped.Handle, (int)resetFlag, false);
        }
        /// <summary>
        /// Sets the value of a reset flag toggle on this <see cref="Ped"/>.
        /// You will need to call this method every frame you want to set, since the values of
        /// <see cref="PedConfigFlagToggles"/> are reset every frame.
        /// </summary>
        public void SetResetFlag(PedResetFlagToggles resetFlag, bool value)
        {
            Function.Call(Hash.SET_PED_RESET_FLAG, _ped.Handle, (int)resetFlag, value);
        }
    }
}
