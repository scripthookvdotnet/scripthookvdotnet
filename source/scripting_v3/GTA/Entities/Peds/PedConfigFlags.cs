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
    /// Represents a wrapper class for `<c>CPedConfigFlags</c>` (not `<c>ePedConfigFlags</c>`), which contains
    /// relatively static flags to configure <see cref="Ped"/>s abilities, options, and special settings.
    /// </summary>
    public sealed class PedConfigFlags
    {
        #region Fields
        readonly Ped _ped;
        #endregion

        internal PedConfigFlags(Ped ped)
        {
            _ped = ped;
        }

        /// <summary>
        /// Sets the vehicle knock off type that determines how easy this <see cref="Ped"/> can be knocked off
        /// (fall off) a <see cref="Vehicle"/>.
        /// </summary>
        public KnockOffVehicleType KnockOffVehicleType
        {
            get
            {
                if (NativeMemory.Ped.KnockOffVehicleTypeOffset == 0)
                {
                    return KnockOffVehicleType.Default;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return KnockOffVehicleType.Default;
                }

                return (KnockOffVehicleType)MemDataMarshal.ReadUInt32BitField(
                    address + NativeMemory.Ped.KnockOffVehicleTypeOffset, 0, 2
                    );
            }
            set => Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, _ped.Handle, (int)value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Ped"/> leg IK mode.
        /// </summary>
        public PedLegIKMode PedLegIKMode
        {
            get
            {
                if (NativeMemory.Ped.KnockOffVehicleTypeOffset == 0)
                {
                    return PedLegIKMode.Off;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return PedLegIKMode.Off;
                }

                return (PedLegIKMode)MemDataMarshal.ReadUInt32BitField(
                    address + NativeMemory.Ped.KnockOffVehicleTypeOffset, 2, 2
                );
            }
            set => Function.Call(Hash.SET_PED_LEG_IK_MODE, _ped.Handle, (int)value);
        }

        /*
         * There's `unsigned int    nPedGestureMode : 2` defined at between `nPedLegIkMode` and
         * `m_iPassengerIndexToUseInAGroup` in `CPedConfigFlags`, but the game doesn't call the setter at all.
         * For those who want to what `SetPedGestureMode(unsigned int)` expects, it's a `eGestureModes` value.
         * The definition of `eGestureModes`:
         * ```cpp
         * enum eGestureModes
           {
               // if gesturing is enabled*, gestures will blended in and out based on audio
	           GESTURE_MODE_DEFAULT,
               // if gesturing is enabled*, gestures will only be blended in during gesture allow tags
	           GESTURE_MODE_USE_ANIM_ALLOW_TAGS,
               // if gesturing is enabled*, gestures will be blended out during gesture block tags
	           GESTURE_MODE_USE_ANIM_BLOCK_TAGS,
	           GESTURE_MODE_NUM	        // *if the conditions in BlockGestures are met
	        };
           ```
         */

        /// <summary>
        /// Gets or sets the passenger index the <see cref="Ped"/> should want to be in use when they are in
        /// a <see cref="PedGroup"/> as a follower.
        /// If set to <see cref="VehicleSeat.Any"/>, which is the default value when a <see cref="Ped"/> is created,
        /// the group leader <see cref="Ped"/> will decide which seat this <see cref="Ped"/> should be in when
        /// the leader entered a vehicle as a driver (using a task response to an event leader event).
        /// </summary>
        /// <value>
        /// A corresponding <see cref="VehicleSeat"/> value if the internal value is between 0 and 15, or
        /// <see cref="VehicleSeat.Any"/> if the internal value is -1 (which is the default value); otherwise,
        /// <see cref="VehicleSeat.None"/> (e.g. when the internal value is between -16 and -2 or SHVDN could not
        /// fetch the value).
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The passenger index is not one of the members of the passenger seats of <see cref="VehicleSeat"/> to
        /// specify the passenger index or <see cref="VehicleSeat.Any"/> to let the group leader <see cref="Ped"/>
        /// inform this <see cref="Ped"/> of which seat this <see cref="Ped"/> should be in. Only thrown from
        /// the setter.
        /// </exception>
        /// <remarks>
        /// You can force the <see cref="Ped"/> to use the specified passenger seat only by setting
        /// <see cref="PedConfigFlagToggles.ForcedToUseSpecificGroupSeatIndex"/> on.
        /// </remarks>
        public VehicleSeat PassengerIndexToUseInAGroup
        {
            get
            {
                if (NativeMemory.Ped.KnockOffVehicleTypeOffset == 0)
                {
                    return VehicleSeat.None;
                }

                IntPtr address = _ped.MemoryAddress;
                if (address == IntPtr.Zero)
                {
                    return VehicleSeat.None;
                }

                // Needs to right shift by 2 bits if `CPedConfigFlags::nPedGestureMode` is removed
                int val = MemDataMarshal.ReadInt32BitField(
                    address + NativeMemory.Ped.KnockOffVehicleTypeOffset, 6, 5
                );

                // Fast path for the default value
                if (val == -1)
                {
                    return VehicleSeat.Any;
                }
                // Any negative values except for -1 should be considered as not really useful.
                //
                // Any values between 16 and 31 are supposed to be negative considering how signed bit-fields are
                // interpreted in Windows implementation of C++.
                //
                // None of them will make `CTaskEnterVehicle::ShouldLeaveDoorOpenForGroupMembers` and
                // `CTaskMotionInVehicle::ProcessShuffleForGroupMember` go to specialized cases for seat index match
                // (where 0 is the driver seat).
                // `CEventLeaderEnteredCarAsDriver::CreateResponseTask` will have this ped start a `CTaskEnterVehicle`
                // with a negative seat index (which is always invalid in functions outside scripting natives) when
                // the leader enters a vehicle as a driver.
                if (val < 0)
                {
                    return VehicleSeat.None;
                }

                return (VehicleSeat)(val - 1);
            }
            set
            {
                // Do not throw an exception if the value is `VehicleSeat.Any` because it's the default value.
                // While `SET_PED_GROUP_MEMBER_PASSENGER_INDEX` does not set a new value if it's -1 or less,
                // which is the default value, the native can set the internal value to -1 by passing a positive value
                // like 31 or 63 as the 2nd argument.
                if (((int)value < (int)VehicleSeat.Any || (int)value > (int)VehicleSeat.ExtraSeat12))
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(
                        nameof(value),
                        "The value must be one of the members of the passenger seats or `VehicleSeat.Any` " +
                        "(`VehicleSeat.Driver` is also acceptable for implementation convenience)."
                    );
                }

                int valToPass = (int)value;
                // Both of the special cases below use the overflow wrapping behavior, while the internal value is
                // a signed bit-field and signed overflow is undefined in C++. You can test the compiled code of
                // `CPedFlags::SetPassengerIndexToUseInAGroup` just uses the wrapping behavior by passing an int value
                // more than 14 to the native function as the 2nd argument (the native passes what you pass plus 1 to
                // the internal func and the range of `CPedFlags::m_iPassengerIndexToUseInAGroup` is between -16 and
                // 15).
                if (value == VehicleSeat.Any)
                {
                    const int PassengerIndexToResetToDefault = 30;
                    valToPass = PassengerIndexToResetToDefault;
                }
                else if (value == VehicleSeat.Driver)
                {
                    const int PassengerIndexToSetToDriver = 31;
                    valToPass = PassengerIndexToSetToDriver;
                }

                Function.Call(Hash.SET_PED_GROUP_MEMBER_PASSENGER_INDEX, _ped.Handle, valToPass);               
            }
        }

        /*
         * There's `float fClimbRateOverride` defined at between `m_iPassengerIndexToUseInAGroup` and
         * `m_Flags` in `CPedConfigFlags`, but the game doesn't call the setter at all.
         */

        /// <summary>
        /// Gets the value of a config flag toggle on this <see cref="Ped"/>.
        /// </summary>
        public bool GetConfigFlag(PedConfigFlagToggles configFlagToggle)
        {
            return Function.Call<bool>(Hash.GET_PED_CONFIG_FLAG, _ped.Handle, (int)configFlagToggle, false);
        }
        /// <summary>
        /// Sets the value of a config flag toggle on this <see cref="Ped"/>.
        /// </summary>
        public void SetConfigFlag(PedConfigFlagToggles configFlagToggle, bool value)
        {
            Function.Call(Hash.SET_PED_CONFIG_FLAG, _ped.Handle, (int)configFlagToggle, value);
        }
    }
}
