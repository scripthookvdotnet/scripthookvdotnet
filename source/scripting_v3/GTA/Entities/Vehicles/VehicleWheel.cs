//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GTA
{
	public sealed class VehicleWheel
	{
		#region Fields
		IntPtr _cachedAddress;

		internal static readonly VehicleWheelBoneId[] vehicleWheelBoneIndexTableForNatives = {
			VehicleWheelBoneId.WheelLeftFront,
			VehicleWheelBoneId.WheelRightFront,
			VehicleWheelBoneId.WheelLeftMiddle1,
			VehicleWheelBoneId.WheelRightMiddle1,
			VehicleWheelBoneId.WheelLeftRear,
			VehicleWheelBoneId.WheelRightRear,
			VehicleWheelBoneId.WheelLeftFront,
			VehicleWheelBoneId.WheelLeftRear,
		};

		internal static readonly Dictionary<VehicleWheelBoneId, ScriptVehicleWheelIndex> vehicleScriptWheelIndicesForBoneIds
			= new Dictionary<VehicleWheelBoneId, ScriptVehicleWheelIndex>(10)
		{
			{ VehicleWheelBoneId.WheelLeftFront, ScriptVehicleWheelIndex.CarFrontLeft },
			{ VehicleWheelBoneId.WheelRightFront, ScriptVehicleWheelIndex.CarFrontRight },
			{ VehicleWheelBoneId.WheelLeftRear, ScriptVehicleWheelIndex.CarRearLeft },
			{ VehicleWheelBoneId.WheelRightRear, ScriptVehicleWheelIndex.CarRearRight },
			{ VehicleWheelBoneId.WheelLeftMiddle1, ScriptVehicleWheelIndex.CarMidLeft },
			{ VehicleWheelBoneId.WheelRightMiddle1, ScriptVehicleWheelIndex.CarMidRight },

			// Natives for vehicle wheels don't support the middle 2 wheels or middle 3 wheels
			// Can fetch some correct value even if any value outside 0 to 7 is passed as the wheel id to the natives,
			// but it's kind of a undefined behavior because the array for wheel id has only 8 elements
			{ VehicleWheelBoneId.WheelLeftMiddle2, ScriptVehicleWheelIndex.Invalid },
			{ VehicleWheelBoneId.WheelRightMiddle2, ScriptVehicleWheelIndex.Invalid },
			{ VehicleWheelBoneId.WheelLeftMiddle3, ScriptVehicleWheelIndex.Invalid },
			{ VehicleWheelBoneId.WheelRightMiddle3, ScriptVehicleWheelIndex.Invalid },
		};
		#endregion

		internal VehicleWheel(Vehicle owner, int scriptIndex) : this(owner, (ScriptVehicleWheelIndex)scriptIndex)
		{
		}
		internal VehicleWheel(Vehicle owner, VehicleWheelBoneId boneIndex)
		{
			Vehicle = owner;
			BoneId = boneIndex;

			if (vehicleScriptWheelIndicesForBoneIds.TryGetValue(boneIndex, out ScriptVehicleWheelIndex scriptIndex))
			{
				ScriptIndex = scriptIndex;
			}
			else
			{
				ScriptIndex = ScriptVehicleWheelIndex.Invalid;
			}
		}
		internal VehicleWheel(Vehicle owner, ScriptVehicleWheelIndex index)
		{
			Vehicle = owner;

			#region Index Assignment
			switch ((int)index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					BoneId = vehicleWheelBoneIndexTableForNatives[(int)index];
					ScriptIndex = index;
					break;
				default:
					BoneId = VehicleWheelBoneId.Invalid;
					ScriptIndex = ScriptVehicleWheelIndex.Invalid;
					break;
			}
			#endregion
		}

		internal VehicleWheel(Vehicle owner, VehicleWheelBoneId boneIndex, IntPtr wheelAddress) : this(owner, boneIndex)
		{
			_cachedAddress = wheelAddress;
		}

		/// <summary>
		/// Gets the <see cref="Vehicle"/>this <see cref="VehicleWheel"/> belongs to.
		/// </summary>
		public Vehicle Vehicle
		{
			get;
		}

		/// <summary>
		/// Gets the script wheel index for native functions.
		/// </summary>
		[Obsolete("Use VehicleWheel.BoneId or VehicleWheel.ScriptIndex instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public int Index => (int)ScriptIndex;

		/// <summary>
		/// Gets the script wheel index for native functions.
		/// </summary>
		public ScriptVehicleWheelIndex ScriptIndex
		{
			get;
		}

		/// <summary>
		/// Gets the bone id this <see cref="VehicleWheel"/>.
		/// </summary>
		public VehicleWheelBoneId BoneId
		{
			get;
		}

		/// <summary>
		/// Gets the memory address where this <see cref="VehicleWheel"/> is stored in memory.
		/// </summary>
		public IntPtr MemoryAddress
		{
			get
			{
				if (!IsBoneIdValid(BoneId))
				{
					return IntPtr.Zero;
				}

				// Check if the vehicle is not boat, train, or submarine. This also checks if the vehicle exists (0xFFFFFFFF will be returned if doesn't exist)
				if (!CanVehicleHaveWheels(Vehicle))
				{
					return IntPtr.Zero;
				}

				if (_cachedAddress != IntPtr.Zero)
				{
					return _cachedAddress;
				}

				return GetMemoryAddressInit();
			}
		}

		/// <summary>
		/// Gets the last contact position.
		/// </summary>
		public Vector3 LastContactPosition
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x40));
			}
		}

		/// <summary>
		/// Gets the normal vector of surface this <see cref="VehicleWheel"/> is contacting.
		/// </summary>
		public Vector3 SurfaceNormalVector
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x70));
			}
		}

		/// <summary>
		/// Gets or sets the limit multiplier that affects how much this <see cref="VehicleWheel"/> can turn.
		/// </summary>
		public float SteeringLimitMultiplier
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelSteeringLimitMultiplierOffset == 0)
				{
					return 0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Vehicle.WheelSteeringLimitMultiplierOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelSteeringLimitMultiplierOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Vehicle.WheelSteeringLimitMultiplierOffset, value);
			}
		}

		/// <summary>
		/// Gets or sets the suspension strength of this <see cref="VehicleWheel"/>.
		/// After the value is changed, the physics of the owner <see cref="Vehicle"/> must be activated before the change can apply.
		/// </summary>
		public float SuspensionStrength
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelSuspensionStrengthOffset == 0)
				{
					return 0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Vehicle.WheelSuspensionStrengthOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelSuspensionStrengthOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Vehicle.WheelSuspensionStrengthOffset, value);
			}
		}

		/// <summary>
		/// Gets or sets the temperature of <see cref="VehicleWheel"/>. This value rises when <see cref="Vehicle"/> is drifting, braking, or in burnout.
		/// If this value is kept at <c>59f</c> when <see cref="Vehicle"/> is on burnout for a short time, the tire will burst.
		/// </summary>
		public float Temperature
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelTemperatureOffset == 0)
				{
					return 0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Vehicle.WheelTemperatureOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelTemperatureOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Vehicle.WheelTemperatureOffset, value);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="VehicleWheel"/> is touching any surface.
		/// </summary>
		public bool IsTouchingSurface
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.Vehicle.IsWheelTouchingSurface(address, Vehicle.MemoryAddress);
			}
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="VehicleWheel"/>'s tire is on fire.
		/// </summary>
		public bool IsTireOnFire
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelTouchingFlagsOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.Vehicle.WheelTouchingFlagsOffset, 3);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelTouchingFlagsOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.SetBit(address + SHVDN.NativeMemory.Vehicle.WheelTouchingFlagsOffset, 3, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="VehicleWheel"/> is a steering wheel.
		/// </summary>
		public bool IsSteeringWheel
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelTouchingFlagsOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.Vehicle.WheelTouchingFlagsOffset + 4, 3);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelTouchingFlagsOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.SetBit(address + SHVDN.NativeMemory.Vehicle.WheelTouchingFlagsOffset + 4, 3, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="VehicleWheel"/> is a driving wheel.
		/// </summary>
		public bool IsDrivingWheel
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelTouchingFlagsOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.Vehicle.WheelTouchingFlagsOffset + 4, 4);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelTouchingFlagsOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.SetBit(address + SHVDN.NativeMemory.Vehicle.WheelTouchingFlagsOffset + 4, 4, value);
			}
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="VehicleWheel"/> is punctured.
		/// </summary>
		public bool IsPunctured
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.TireHealthOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Vehicle.WheelHealthOffset) < 1000f;
			}
		}
		/// <summary>
		/// Sets a value indicating whether this <see cref="VehicleWheel"/> is bursted.
		/// </summary>
		public bool IsBursted
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.TireHealthOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Vehicle.TireHealthOffset) <= 0f;
			}
		}
		/// <summary>
		/// Gets or sets the wheel health.
		/// </summary>
		public float Health
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelHealthOffset == 0)
				{
					return 0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Vehicle.WheelHealthOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelHealthOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Vehicle.WheelHealthOffset, value);
			}
		}
		/// <summary>
		/// Gets or sets the tire health.
		/// If <see cref="WearMultiplier" /> is set to exactly <c>0f</c>, the value will default to <c>350f</c> if the value is positive and less than <c>1000f</c>.
		/// </summary>
		public float TireHealth
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.TireHealthOffset == 0)
				{
					return 0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Vehicle.TireHealthOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.TireHealthOffset == 0)
				{
					return;
				}

				// Change value to 0 if the value is negative. IS_VEHICLE_TYRE_BURST returns true only if value is exactly 0 when the 3rd parameter (the bool completely) is a non-zero value.
				if (value < 0f)
				{
					value = 0f;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Vehicle.TireHealthOffset, value);
			}
		}
		/// <summary>
		/// <para>
		/// Gets or sets the value indicating how fast the tires will wear out.
		/// The higher this value is, the greater downforce will be created.
		/// </para>
		/// <para>
		/// Only supported in v1.0.1868.0 and later versions.
		/// Will throw <see cref="GameVersionNotSupportedException"/> if the setter is called in earlier versions (the getter always returns <see langword="false"/> in earlier versions).
		/// </para>
		/// </summary>
		/// <remarks>
		/// This property represents the wear rate property <c>SET_TYRE_WEAR_RATE</c> is supposed to change,
		/// but Script Hook V does never call it and instead calls <c>SET_TYRE_WEAR_RATE_SCALE</c> in versions that
		/// support up to v1.0.2944.0 or some earlier game version (you can actually call <c>SET_TYRE_WEAR_RATE</c>
		/// in RAGE Plugin Hook and FiveM, and you can call <c>GET_TYRE_WEAR_RATE</c> in SHV scripts).
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown when called on a game version prior to v1.0.1868.0.
		/// </exception>
		public float WearMultiplier
		{
			// If you wonder why SET_TYRE_WEAR_RATE and SET_TYRE_WEAR_RATE_SCALE produces the same result in SHV scripts,
			// that's because SHV maps hashes for SET_TYRE_WEAR_RATE wrong in versions that support up to v1.0.2944.0 or
			// a older game version (maps hashes for SET_TYRE_WEAR_RATE_SCALE instead)
			// You can actually call SET_TYRE_WEAR_RATE in RPH and FiveM
			get
			{
				if (Game.Version < GameVersion.v1_0_1868_0_Steam)
				{
					return 0f;
				}

				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.TireWearRateOffset == 0)
				{
					return 0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Vehicle.TireWearRateOffset);
			}
			set
			{
				GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_1868_0_Steam, nameof(VehicleWheel), nameof(WearMultiplier));

				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.TireWearRateOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Vehicle.TireWearRateOffset, value);
			}
		}
		/// <summary>
		/// <para>
		/// Gets or sets the difference in tire grip.
		/// The more this value is, the more traction the wheel loses of traction produced from <see cref="WearMultiplier"/>.
		/// Should be between a wear rate of 1 and 0.
		/// </para>
		/// <para>
		/// Only supported in v1.0.2060.0 and later versions.
		/// Will throw <see cref="GameVersionNotSupportedException"/> if the setter is called in earlier versions (the getter always returns <see langword="false"/> in earlier versions).
		/// </para>
		/// </summary>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown when called on a game version prior to v1.0.2060.0.
		/// </exception>
		public float MaxGripDifferenceDueToWearRate
		{
			get
			{
				if (Game.Version < GameVersion.v1_0_2060_0_Steam)
				{
					return 0f;
				}

				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelMaxGripDiffDueToWearRateOffset == 0)
				{
					return 0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Vehicle.WheelMaxGripDiffDueToWearRateOffset);
			}
			set
			{
				GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_2060_0_Steam, nameof(VehicleWheel), nameof(WearMultiplier));

				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelMaxGripDiffDueToWearRateOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Vehicle.WheelMaxGripDiffDueToWearRateOffset, value);
			}
		}
		/// <summary>
		/// <para>
		/// Gets or sets the value indicating how fast the tires will wear out.
		/// Only affects how fast the tires will wear out and does not affect how strong the downforce will be.
		/// </para>
		/// <para>
		/// Only supported in v1.0.2060.0 and later versions.
		/// Will throw <see cref="GameVersionNotSupportedException"/> if the setter is called in earlier versions (the getter always returns <see langword="false"/> in earlier versions).
		/// </para>
		/// </summary>
		/// <remarks>
		/// This property represents the wear rate scale property, which <c>SET_TYRE_WEAR_RATE_SCALE</c> is supposed to
		/// change, not the the wear rate property, which <c>SET_TYRE_WEAR_RATE</c> is supposed to change.
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown when called on a game version prior to v1.0.2060.0.
		/// </exception>
		public float WearRateScale
		{
			get
			{
				if (Game.Version < GameVersion.v1_0_2060_0_Steam)
				{
					return 0f;
				}

				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.TireWearRateScaleOffset == 0)
				{
					return 0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Vehicle.TireWearRateScaleOffset);
			}
			set
			{
				GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_2060_0_Steam, nameof(VehicleWheel), nameof(WearMultiplier));

				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.TireWearRateScaleOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Vehicle.TireWearRateScaleOffset, value);
			}
		}

		/// <summary>
		/// Fixes this <see cref="VehicleWheel"/>'s tire.
		/// </summary>
		public void Fix()
		{
			// Do what SET_VEHICLE_TYRE_FIXED exactly does
			Fix(false);
		}
		/// <summary>
		/// Fixes this <see cref="VehicleWheel"/>'s tire.
		/// </summary>
		/// <param name="leaveOtherBurstedTiresNotShowing">If set to <see langword="false"/>, bursted tires will appear again just like <c>SET_VEHICLE_TYRE_FIXED</c> does.</param>
		public void Fix(bool leaveOtherBurstedTiresNotShowing)
		{
			IntPtr address = MemoryAddress;
			if (address == IntPtr.Zero)
			{
				return;
			}

			SHVDN.NativeMemory.Vehicle.FixVehicleWheel(address);

			if (leaveOtherBurstedTiresNotShowing)
			{
				return;
			}

			IntPtr customShaderEffectVehicleAddr = SHVDN.NativeMemory.ReadAddress(SHVDN.NativeMemory.ReadAddress(Vehicle.MemoryAddress + 0x48) + 0x20);
			SHVDN.NativeMemory.SetBit(customShaderEffectVehicleAddr + SHVDN.NativeMemory.Vehicle.ShouldShowOnlyVehicleTiresWithPositiveHealthOffset, 1, false);
		}

		/// <summary>
		/// Punctures this <see cref="VehicleWheel"/>'s tire.
		/// </summary>
		/// <param name="damage">How much damage this <see cref="VehicleWheel"/> will take.</param>
		public void Puncture(float damage = 1000f)
		{
			IntPtr address = MemoryAddress;
			if (address == IntPtr.Zero)
			{
				return;
			}

			// Do what SET_VEHICLE_TYRE_BURST exactly does with false (zero) as 3rd parameter
			SHVDN.NativeMemory.Vehicle.PunctureTire(address, damage, Vehicle.MemoryAddress);
		}

		/// <summary>
		/// Bursts this <see cref="VehicleWheel"/>'s tire completely.
		/// </summary>
		public void Burst()
		{
			IntPtr address = MemoryAddress;
			if (address == IntPtr.Zero)
			{
				return;
			}

			// Do what SET_VEHICLE_TYRE_BURST exactly does with true (non-zero) as 3rd parameter and 1000f as 4th parameter
			SHVDN.NativeMemory.Vehicle.BurstTireOnRim(address, Vehicle.MemoryAddress);
		}

		// Property doesn't make much sense for hydraulic suspention raise factor,
		// as SET_HYDRAULIC_SUSPENSION_RAISE_FACTOR implicitly changes some other lowrider control states on a vehicle
		// wheel struct and CVehicle if the native successfully changes the raise factor
		/// <summary>
		/// <para>
		/// Sets the hydraulic suspension raise factor for this wheel.
		/// </para>
		/// <para>
		/// Not available in game versions prior to v1.0.505.2.
		/// </para>
		/// </summary>
		/// <remarks>
		/// Does not support for <see cref="VehicleWheelBoneId.WheelLeftMiddle2"/>, <see cref="VehicleWheelBoneId.WheelRightMiddle2"/>,
		/// <see cref="VehicleWheelBoneId.WheelLeftMiddle3"/>, or <see cref="VehicleWheelBoneId.WheelRightMiddle3"/>.
		/// If called on one of the wheels, this method will throw <see cref="InvalidOperationException"/>.
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown when called on a game version prior to v1.0.505.2.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown when called on a <see cref="VehicleWheel"/> whose <see cref="BoneId"/> is
		/// <see cref="VehicleWheelBoneId.WheelLeftMiddle2"/>, <see cref="VehicleWheelBoneId.WheelRightMiddle2"/>,
		/// <see cref="VehicleWheelBoneId.WheelLeftMiddle3"/>, or <see cref="VehicleWheelBoneId.WheelRightMiddle3"/>.
		/// </exception>
		public void SetHydraulicSuspensionRaiseFactor(float raiseFactor)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_505_2_Steam, nameof(VehicleWheel), nameof(SetHydraulicSuspensionRaiseFactor));
			ThrowIfNotListedInScriptIndex(nameof(SetHydraulicSuspensionRaiseFactor));

			Function.Call(Hash.SET_HYDRAULIC_SUSPENSION_RAISE_FACTOR, Vehicle, (int)ScriptIndex, raiseFactor);
		}
		/// <summary>
		/// <para>
		/// Gets the hydraulic suspension raise factor for this wheel.
		/// </para>
		/// <para>
		/// Currently only available in game versions prior to v1.0.2372.0 (will not be available in game versions prior
		/// to v1.0.505.2 as lowrider state variables are not present in those versions).
		/// </para>
		/// </summary>
		/// <remarks>
		/// Does not support for <see cref="VehicleWheelBoneId.WheelLeftMiddle2"/>, <see cref="VehicleWheelBoneId.WheelRightMiddle2"/>,
		/// <see cref="VehicleWheelBoneId.WheelLeftMiddle3"/>, or <see cref="VehicleWheelBoneId.WheelRightMiddle3"/>.
		/// If called on one of the wheels, this method will throw <see cref="InvalidOperationException"/>.
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown when called on a game version prior to v1.0.2372.0.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown when called on a <see cref="VehicleWheel"/> whose <see cref="BoneId"/> is
		/// <see cref="VehicleWheelBoneId.WheelLeftMiddle2"/>, <see cref="VehicleWheelBoneId.WheelRightMiddle2"/>,
		/// <see cref="VehicleWheelBoneId.WheelLeftMiddle3"/>, or <see cref="VehicleWheelBoneId.WheelRightMiddle3"/>.
		/// </exception>
		public float GetHydraulicSuspensionRaiseFactor()
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_2372_0_Steam, nameof(VehicleWheel), nameof(SetHydraulicSuspensionRaiseFactor));
			ThrowIfNotListedInScriptIndex(nameof(GetHydraulicSuspensionRaiseFactor));

			return Function.Call<float>(Hash.GET_HYDRAULIC_SUSPENSION_RAISE_FACTOR, Vehicle, (int)ScriptIndex);
		}

		// boats, trains, and submarines cannot have wheels
		internal static bool CanVehicleHaveWheels(Vehicle vehicle) => (uint)vehicle.Type <= 0xC;
		private IntPtr GetMemoryAddressInit()
		{
			IntPtr vehicleAddr = Vehicle.MemoryAddress;

			byte wheelIndexOfArrayPtr = SHVDN.NativeMemory.ReadByte(vehicleAddr + SHVDN.NativeMemory.Vehicle.WheelBoneIdToPtrArrayIndexOffset + ((int)BoneId - 11));
			if (wheelIndexOfArrayPtr == 0xFF)
			{
				return IntPtr.Zero;
			}

			IntPtr vehicleWheelArrayAddr = SHVDN.NativeMemory.ReadAddress(vehicleAddr + SHVDN.NativeMemory.Vehicle.WheelPtrArrayOffset);
			_cachedAddress = SHVDN.NativeMemory.ReadAddress(vehicleWheelArrayAddr + 0x8 * wheelIndexOfArrayPtr);

			return _cachedAddress;
		}
		internal static bool IsBoneIdValid(VehicleWheelBoneId boneId)
		{
			switch (boneId)
			{
				case VehicleWheelBoneId.WheelLeftFront:
				case VehicleWheelBoneId.WheelRightFront:
				case VehicleWheelBoneId.WheelLeftRear:
				case VehicleWheelBoneId.WheelRightRear:
				case VehicleWheelBoneId.WheelLeftMiddle1:
				case VehicleWheelBoneId.WheelRightMiddle1:
				case VehicleWheelBoneId.WheelLeftMiddle2:
				case VehicleWheelBoneId.WheelRightMiddle2:
				case VehicleWheelBoneId.WheelLeftMiddle3:
				case VehicleWheelBoneId.WheelRightMiddle3:
					return true;
				default:
					return false;
			}
		}

		private void ThrowIfNotListedInScriptIndex(string methodName)
		{
			switch (BoneId)
			{
				case VehicleWheelBoneId.WheelLeftFront:
				case VehicleWheelBoneId.WheelRightFront:
				case VehicleWheelBoneId.WheelLeftRear:
				case VehicleWheelBoneId.WheelRightRear:
				case VehicleWheelBoneId.WheelLeftMiddle1:
				case VehicleWheelBoneId.WheelRightMiddle1:
					return;
				default:
					throw new InvalidOperationException($"VehicleWheel.{methodName} does not support for wheels of LeftMiddle2, RightMiddle2, LeftMiddle3, or RightMiddle3.");
			}
		}
	}
}
