//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GTA
{
	public sealed class Vehicle : Entity
	{
		public Vehicle(int handle) : base(handle)
		{
		}

		public void Repair()
		{
			Function.Call(Hash.SET_VEHICLE_FIXED, Handle);
			RemoveDestroyedFlag(Handle);

			void RemoveDestroyedFlag(int vehicleHandle)
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(vehicleHandle);
				if (address == IntPtr.Zero)
				{
					return;
				}

				int targetValue = SHVDN.NativeMemory.ReadByte(address + 0xD8);

				if ((targetValue & 7) == 3)
					targetValue &= 0xF8;

				SHVDN.NativeMemory.WriteByte(address + 0xD8, (byte)targetValue);
			}
		}

		public void Explode()
		{
			Function.Call(Hash.EXPLODE_VEHICLE, Handle, true, false);
		}

		#region Styling

		public bool IsConvertible => Function.Call<bool>(Hash.IS_VEHICLE_A_CONVERTIBLE, Handle, 0);

		public float DirtLevel
		{
			get => Function.Call<float>(Hash.GET_VEHICLE_DIRT_LEVEL, Handle);
			set => Function.Call(Hash.SET_VEHICLE_DIRT_LEVEL, Handle, value);
		}

		public void InstallModKit()
		{
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, Handle, 0);
		}

		public int GetMod(VehicleMod modType)
		{
			return Function.Call<int>(Hash.GET_VEHICLE_MOD, Handle, (int)(modType));
		}
		public void SetMod(VehicleMod modType, int modIndex, bool variations)
		{
			Function.Call(Hash.SET_VEHICLE_MOD, Handle, (int)(modType), modIndex, variations);
		}
		public int GetModCount(VehicleMod modType)
		{
			return Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, Handle, (int)(modType));
		}
		public void ToggleMod(VehicleToggleMod toggleMod, bool toggle)
		{
			Function.Call(Hash.TOGGLE_VEHICLE_MOD, Handle, (int)(toggleMod), toggle);
		}
		public bool IsToggleModOn(VehicleToggleMod toggleMod)
		{
			return Function.Call<bool>(Hash.IS_TOGGLE_MOD_ON, Handle, (int)(toggleMod));
		}
		public string GetModTypeName(VehicleMod modType)
		{
			return Function.Call<string>(Hash.GET_MOD_SLOT_NAME, Handle, (int)(modType));
		}
		public string GetToggleModTypeName(VehicleToggleMod toggleModType)
		{
			return Function.Call<string>(Hash.GET_MOD_SLOT_NAME, Handle, (int)(toggleModType));
		}
		public string GetModName(VehicleMod modType, int modValue)
		{
			return Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, Handle, (int)(modType), modValue);
		}

		public void Wash()
		{
			DirtLevel = 0.0f;
		}

		public bool IsExtraOn(int extra)
		{
			return Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, Handle, extra);
		}

		public bool ExtraExists(int extra)
		{
			return Function.Call<bool>(Hash.DOES_EXTRA_EXIST, Handle, extra);
		}

		public void ToggleExtra(int extra, bool toggle)
		{
			Function.Call(Hash.SET_VEHICLE_EXTRA, Handle, extra, !toggle);
		}

		public string NumberPlate
		{
			get => Function.Call<string>(Hash.GET_VEHICLE_NUMBER_PLATE_TEXT, Handle);
			set => Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT, Handle, value);
		}

		public NumberPlateMounting NumberPlateMounting => (NumberPlateMounting)Function.Call<int>(Hash.GET_VEHICLE_PLATE_TYPE, Handle);

		public NumberPlateType NumberPlateType
		{
			get => (NumberPlateType)Function.Call<int>(Hash.GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, Handle);
			set => Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, Handle, (int)value);
		}

		public VehicleColor PrimaryColor
		{
			get
			{
				int color1, color2;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_COLOURS, Handle, &color1, &color2);
				}

				return (VehicleColor)color1;
			}
			set
			{
				int color1, color2;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_COLOURS, Handle, &color1, &color2);
				}
				Function.Call(Hash.SET_VEHICLE_COLOURS, Handle, (int)value, color2);
			}
		}

		public VehicleColor SecondaryColor
		{
			get
			{
				int color1, color2;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_COLOURS, Handle, &color1, &color2);
				}

				return (VehicleColor)color2;
			}
			set
			{
				int color1, color2;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_COLOURS, Handle, &color1, &color2);
				}
				Function.Call(Hash.SET_VEHICLE_COLOURS, Handle, color1, (int)value);
			}
		}

		public VehicleColor RimColor
		{
			get
			{
				int pearlescentColor, rimColor;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_EXTRA_COLOURS, Handle, &pearlescentColor, &rimColor);
				}
				return (VehicleColor)rimColor;
			}
			set => Function.Call(Hash.SET_VEHICLE_EXTRA_COLOURS, Handle, (int)PearlescentColor, (int)value);
		}

		public VehicleColor PearlescentColor
		{
			get
			{
				int pearlescentColor, rimColor;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_EXTRA_COLOURS, Handle, &pearlescentColor, &rimColor);
				}
				return (VehicleColor)pearlescentColor;
			}
			set => Function.Call(Hash.SET_VEHICLE_EXTRA_COLOURS, Handle, (int)value, (int)RimColor);
		}

		public VehicleColor DashboardColor
		{
			get
			{
				int dashboardColor;
				unsafe
				{
					Function.Call((Hash)0xB7635E80A5C31BFF, Handle, &dashboardColor);
				}
				return (VehicleColor)dashboardColor;
			}
			set => Function.Call((Hash)0x6089CDF6A57F326C, Handle, (int)value);
		}

		public VehicleColor TrimColor
		{
			get
			{
				int trimColor;
				unsafe
				{
					Function.Call((Hash)0x7D1464D472D32136, Handle, &trimColor);
				}
				return (VehicleColor)trimColor;
			}
			set => Function.Call((Hash)0xF40DD601A65F7F19, Handle, (int)value);
		}

		public int ColorCombination
		{
			get => Function.Call<int>(Hash.GET_VEHICLE_COLOUR_COMBINATION, Handle);
			set => Function.Call(Hash.SET_VEHICLE_COLOUR_COMBINATION, Handle, value);
		}

		public int ColorCombinationCount => Function.Call<int>(Hash.GET_NUMBER_OF_VEHICLE_COLOURS, Handle);

		public VehicleWheelType WheelType
		{
			get => (VehicleWheelType)Function.Call<int>(Hash.GET_VEHICLE_WHEEL_TYPE, Handle);
			set => Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, Handle, (int)value);
		}

		public VehicleWindowTint WindowTint
		{
			get => (VehicleWindowTint)Function.Call<int>(Hash.GET_VEHICLE_WINDOW_TINT, Handle);
			set => Function.Call(Hash.SET_VEHICLE_WINDOW_TINT, Handle, (int)value);
		}

		public bool IsPrimaryColorCustom => Function.Call<bool>(Hash.GET_IS_VEHICLE_PRIMARY_COLOUR_CUSTOM, Handle);
		public bool IsSecondaryColorCustom => Function.Call<bool>(Hash.GET_IS_VEHICLE_SECONDARY_COLOUR_CUSTOM, Handle);

		public void ClearCustomPrimaryColor()
		{
			Function.Call(Hash.CLEAR_VEHICLE_CUSTOM_PRIMARY_COLOUR, Handle);
		}
		public void ClearCustomSecondaryColor()
		{
			Function.Call(Hash.CLEAR_VEHICLE_CUSTOM_SECONDARY_COLOUR, Handle);
		}

		public Color CustomPrimaryColor
		{
			get
			{
				int r, g, b;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_CUSTOM_PRIMARY_COLOUR, Handle, &r, &g, &b);
				}
				return Color.FromArgb(r, g, b);
			}
			set => Function.Call(Hash.SET_VEHICLE_CUSTOM_PRIMARY_COLOUR, Handle, value.R, value.G, value.B);
		}
		public Color CustomSecondaryColor
		{
			get
			{
				int r, g, b;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_CUSTOM_SECONDARY_COLOUR, Handle, &r, &g, &b);
				}
				return Color.FromArgb(r, g, b);
			}
			set => Function.Call(Hash.SET_VEHICLE_CUSTOM_SECONDARY_COLOUR, Handle, value.R, value.G, value.B);
		}

		public Color NeonLightsColor
		{
			get
			{
				int r, g, b;
				unsafe
				{
					Function.Call(Hash._GET_VEHICLE_NEON_LIGHTS_COLOUR, Handle, &r, &g, &b);
				}
				return Color.FromArgb(r, g, b);
			}
			set => Function.Call(Hash._SET_VEHICLE_NEON_LIGHTS_COLOUR, Handle, value.R, value.G, value.B);
		}

		public Color TireSmokeColor
		{
			get
			{
				int r, g, b;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_TYRE_SMOKE_COLOR, Handle, &r, &g, &b);
				}
				return Color.FromArgb(r, g, b);
			}
			set => Function.Call(Hash.SET_VEHICLE_TYRE_SMOKE_COLOR, Handle, value.R, value.G, value.B);
		}

		public int Livery
		{
			get
			{
				if (GetModCount(VehicleMod.Livery) >= 1)
				{
					return GetMod(VehicleMod.Livery);
				}
				else
				{
					return Function.Call<int>(Hash.GET_VEHICLE_LIVERY, Handle);
				}
			}
			set
			{
				if (GetModCount(VehicleMod.Livery) >= 1)
				{
					SetMod(VehicleMod.Livery, value, false);
				}
				else
				{
					Function.Call(Hash.SET_VEHICLE_LIVERY, Handle, value);
				}
			}
		}

		public int LiveryCount
		{
			get
			{
				int bennysLiveryCount = GetModCount(VehicleMod.Livery);

				if (bennysLiveryCount > 0)
				{
					return bennysLiveryCount;
				}
				else
				{
					return Function.Call<int>(Hash.GET_VEHICLE_LIVERY_COUNT, Handle);
				}
			}
		}

		#endregion

		#region Configuration

		public bool IsStolen
		{
			get => Function.Call<bool>(Hash.IS_VEHICLE_STOLEN, Handle);
			set => Function.Call(Hash.SET_VEHICLE_IS_STOLEN, Handle, value);
		}

		public bool IsWanted
		{
			set => Function.Call(Hash.SET_VEHICLE_IS_WANTED, Handle, value);
		}

		public bool NeedsToBeHotwired
		{
			get
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.NeedsToBeHotwiredOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.NeedsToBeHotwiredOffset, 2);
			}
			set => Function.Call(Hash.SET_VEHICLE_NEEDS_TO_BE_HOTWIRED, Handle, value);
		}

		public bool PreviouslyOwnedByPlayer
		{
			get
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.PreviouslyOwnedByPlayerOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.PreviouslyOwnedByPlayerOffset, 1);
			}
			set => Function.Call(Hash.SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER, Handle, value);
		}

		public string DisplayName => Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, Model.Hash);
		public string FriendlyName => Game.GetGXTEntry(DisplayName);

		public VehicleClass ClassType => Function.Call<VehicleClass>(Hash.GET_VEHICLE_CLASS, Handle);

		#endregion

		#region Health

		public float BodyHealth
		{
			get => Function.Call<float>(Hash.GET_VEHICLE_BODY_HEALTH, Handle);
			set => Function.Call(Hash.SET_VEHICLE_BODY_HEALTH, Handle, value);
		}

		public float EngineHealth
		{
			get => Function.Call<float>(Hash.GET_VEHICLE_ENGINE_HEALTH, Handle);
			set => Function.Call(Hash.SET_VEHICLE_ENGINE_HEALTH, Handle, value);
		}

		public float PetrolTankHealth
		{
			get => Function.Call<float>(Hash.GET_VEHICLE_PETROL_TANK_HEALTH, Handle);
			set => Function.Call(Hash.SET_VEHICLE_PETROL_TANK_HEALTH, Handle, value);
		}

		#endregion

		#region Radio

		public bool IsRadioEnabled
		{
			set => Function.Call(Hash.SET_VEHICLE_RADIO_ENABLED, Handle, value);
		}

		public RadioStation RadioStation
		{
			set
			{
				if (value == RadioStation.RadioOff)
				{
					Function.Call(Hash.SET_VEH_RADIO_STATION, "OFF");
				}
				else if (Enum.IsDefined(typeof(RadioStation), value))
				{
					Function.Call(Hash.SET_VEH_RADIO_STATION, Game.radioNames[(int)value]);
				}
			}
		}

		#endregion

		#region Engine

		public bool EngineRunning
		{
			get => Function.Call<bool>(Hash._IS_VEHICLE_ENGINE_ON, Handle);
			set => Function.Call(Hash.SET_VEHICLE_ENGINE_ON, Handle, value, true);
		}

		public bool EngineCanDegrade
		{
			set => Function.Call(Hash.SET_VEHICLE_ENGINE_CAN_DEGRADE, Handle, value);
		}

		public float EnginePowerMultiplier
		{
			set => Function.Call(Hash._SET_VEHICLE_ENGINE_POWER_MULTIPLIER, Handle, value);
		}

		public float EngineTorqueMultiplier
		{
			set => Function.Call(Hash._SET_VEHICLE_ENGINE_TORQUE_MULTIPLIER, Handle, value);
		}

		public float FuelLevel
		{
			get
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.FuelLevelOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.FuelLevelOffset);
			}
			set
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.FuelLevelOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.FuelLevelOffset, value);
			}
		}

		#endregion

		#region Performance & Driving

		public int HighGear
		{
			get
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.HighGearOffset == 0)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadByte(address + SHVDN.NativeMemory.HighGearOffset);
			}
			set
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.HighGearOffset == 0)
				{
					return;
				}

				if (Game.Version >= GameVersion.VER_1_0_1604_0_STEAM)
				{
					if (value > 10)
					{
						throw new ArgumentOutOfRangeException("value", "Values must be between 0 and 10, inclusive.");
					}
				}
				else if (value > 7)
				{
					throw new ArgumentOutOfRangeException("value", "Values must be between 0 and 7, inclusive.");
				}

				SHVDN.NativeMemory.WriteByte(address + SHVDN.NativeMemory.HighGearOffset, (byte)value);
			}
		}

		public int CurrentGear
		{
			get
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.GearOffset == 0)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadByte(address + SHVDN.NativeMemory.GearOffset);
			}
		}

		public float MaxBraking => Function.Call<float>(Hash.GET_VEHICLE_MAX_BRAKING, Handle);

		public float MaxTraction => Function.Call<float>(Hash.GET_VEHICLE_MAX_TRACTION, Handle);

		public float Speed
		{
			get => Function.Call<float>(Hash.GET_ENTITY_SPEED, Handle);
			set
			{
				if (Model.IsTrain)
				{
					Function.Call(Hash.SET_TRAIN_SPEED, Handle, value);
					Function.Call(Hash.SET_TRAIN_CRUISE_SPEED, Handle, value);
				}
				else
				{
					Function.Call(Hash.SET_VEHICLE_FORWARD_SPEED, Handle, value);
				}
			}
		}

		public float WheelSpeed
		{
			get
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.WheelSpeedOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.WheelSpeedOffset);
			}
		}

		public float CurrentRPM
		{
			get
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.CurrentRPMOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.CurrentRPMOffset);
			}
			set
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.CurrentRPMOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.CurrentRPMOffset, value);
			}
		}

		public float Acceleration
		{
			get
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.AccelerationOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.AccelerationOffset);
			}
		}

		[Obsolete("Vehicle.Steering is obsolete, please use Vehicle.SteeringScale instead.")]
		public float Steering => SteeringScale;

		public float SteeringAngle
		{
			get
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.SteeringAngleOffset == 0)
				{
					return 0.0f;
				}

				return (float)(SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.SteeringAngleOffset) * (180.0 / System.Math.PI));
			}
			set
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.SteeringAngleOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.SteeringAngleOffset, (float)((value + (value > 0 ? 10 : -10)) * (System.Math.PI / 180.0)));
			}
		}

		public float SteeringScale
		{
			get
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.SteeringScaleOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.SteeringScaleOffset);
			}
			set
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.SteeringScaleOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.SteeringScaleOffset, value);
			}
		}

		#endregion

		#region Alarm

		public bool HasAlarm
		{
			set => Function.Call(Hash.SET_VEHICLE_ALARM, Handle, value);
		}

		public bool AlarmActive => Function.Call<bool>(Hash.IS_VEHICLE_ALARM_ACTIVATED, Handle);

		public void StartAlarm()
		{
			Function.Call(Hash.START_VEHICLE_ALARM, Handle);
		}

		#endregion

		#region Siren & Horn

		public bool HasSiren => HasBone("siren1");

		public bool SirenActive
		{
			get => Function.Call<bool>(Hash.IS_VEHICLE_SIREN_ON, Handle);
			set => Function.Call(Hash.SET_VEHICLE_SIREN, Handle, value);
		}

		public bool IsSirenSilent
		{
			// Sets if the siren is silent actually
			set => Function.Call(Hash.DISABLE_VEHICLE_IMPACT_EXPLOSION_ACTIVATION, Handle, value);
		}

		public void SoundHorn(int duration)
		{
			int heldDownHash = Game.GenerateHash("HELDDOWN");
			Function.Call(Hash.START_VEHICLE_HORN, Handle, duration, heldDownHash, 0);
		}

		#endregion

		#region Lights

		public bool LightsOn
		{
			get
			{
				bool lightState1, lightState2;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_LIGHTS_STATE, Handle, &lightState1, &lightState2);
				}
				return lightState1;
			}
			set => Function.Call(Hash.SET_VEHICLE_LIGHTS, Handle, value ? 3 : 4);
		}

		public bool HighBeamsOn
		{
			get
			{
				bool lightState1, lightState2;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_LIGHTS_STATE, Handle, &lightState1, &lightState2);
				}
				return lightState2;
			}
			set => Function.Call(Hash.SET_VEHICLE_FULLBEAM, Handle, value);
		}

		public bool InteriorLightOn
		{
			set => Function.Call(Hash.SET_VEHICLE_INTERIORLIGHT, Handle, value);
		}

		public bool SearchLightOn
		{
			get => Function.Call<bool>(Hash.IS_VEHICLE_SEARCHLIGHT_ON, Handle);
			set => Function.Call(Hash.SET_VEHICLE_SEARCHLIGHT, Handle, value, 0);
		}

		public bool TaxiLightOn
		{
			get => Function.Call<bool>(Hash.IS_TAXI_LIGHT_ON, Handle);
			set => Function.Call(Hash.SET_TAXI_LIGHTS, Handle, value);
		}

		public bool LeftIndicatorLightOn
		{
			set => Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, Handle, true, value);
		}

		public bool RightIndicatorLightOn
		{
			set => Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, Handle, false, value);
		}

		public bool BrakeLightsOn
		{
			set => Function.Call(Hash.SET_VEHICLE_BRAKE_LIGHTS, Handle, value);
		}

		public bool IsNeonLightsOn(VehicleNeonLight light)
		{
			return Function.Call<bool>(Hash._IS_VEHICLE_NEON_LIGHT_ENABLED, Handle, (int)(light));
		}

		public void SetNeonLightsOn(VehicleNeonLight light, bool on)
		{
			Function.Call(Hash._SET_VEHICLE_NEON_LIGHT_ENABLED, Handle, (int)(light), on);
		}

		public float LightsMultiplier
		{
			set => Function.Call(Hash.SET_VEHICLE_LIGHT_MULTIPLIER, Handle, value);
		}

		#endregion

		#region Damaging

		public bool IsDamaged => Function.Call<bool>(Hash._IS_VEHICLE_DAMAGED, Handle);

		public bool IsDriveable
		{
			get => Function.Call<bool>(Hash.IS_VEHICLE_DRIVEABLE, Handle, 0);
			set => Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, Handle, !value);
		}

		public bool LeftHeadLightBroken
		{
			get => Function.Call<bool>(Hash._IS_HEADLIGHT_L_BROKEN, Handle);
			set
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.IsHeadlightDamagedOffset == 0)
				{
					return;
				}

				if (value)
				{
					SHVDN.NativeMemory.SetBit(address + SHVDN.NativeMemory.IsHeadlightDamagedOffset, 0);
				}
				else
				{
					SHVDN.NativeMemory.ClearBit(address + SHVDN.NativeMemory.IsHeadlightDamagedOffset, 0);
				}
			}
		}

		public bool RightHeadLightBroken
		{
			get => Function.Call<bool>(Hash._IS_HEADLIGHT_R_BROKEN, Handle);
			set
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.IsHeadlightDamagedOffset == 0)
				{
					return;
				}

				if (value)
				{
					SHVDN.NativeMemory.SetBit(address + SHVDN.NativeMemory.IsHeadlightDamagedOffset, 1);
				}
				else
				{
					SHVDN.NativeMemory.ClearBit(address + SHVDN.NativeMemory.IsHeadlightDamagedOffset, 1);
				}
			}
		}

		public bool IsRearBumperBrokenOff => Function.Call<bool>(Hash.IS_VEHICLE_BUMPER_BROKEN_OFF, Handle, false);

		public bool IsFrontBumperBrokenOff => Function.Call<bool>(Hash.IS_VEHICLE_BUMPER_BROKEN_OFF, Handle, true);

		public bool IsAxlesStrong
		{
			set => Function.Call<bool>(Hash.SET_VEHICLE_HAS_STRONG_AXLES, Handle, value);
		}

		public bool CanTiresBurst
		{
			get => Function.Call<bool>(Hash.GET_VEHICLE_TYRES_CAN_BURST, Handle);
			set => Function.Call(Hash.SET_VEHICLE_TYRES_CAN_BURST, Handle, value);
		}

		public bool CanWheelsBreak
		{
			set => Function.Call(Hash.SET_VEHICLE_WHEELS_CAN_BREAK, Handle, value);
		}

		public bool CanBeVisiblyDamaged
		{
			set => Function.Call(Hash.SET_VEHICLE_CAN_BE_VISIBLY_DAMAGED, Handle, value);
		}

		public bool DropsMoneyOnExplosion
		{
			set => Function.Call(Hash._0x068F64F2470F9656, Handle, value);
		}

		public void FixTire(int wheel)
		{
			Function.Call(Hash.SET_VEHICLE_TYRE_FIXED, Handle, wheel);
		}
		public void BurstTire(int wheel)
		{
			Function.Call(Hash.SET_VEHICLE_TYRE_BURST, Handle, wheel, 1, 1000.0f);
		}
		public bool IsTireBurst(int wheel)
		{
			return Function.Call<bool>(Hash.IS_VEHICLE_TYRE_BURST, Handle, wheel, false);
		}

		public void ApplyDamage(Vector3 loc, float damageAmount, float radius)
		{
			Function.Call(Hash.SET_VEHICLE_DAMAGE, loc.X, loc.Y, loc.Z, damageAmount, radius, true);
		}

		#endregion

		#region Doors & Locks

		public bool HasRoof => Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, Handle);

		public VehicleRoofState RoofState
		{
			get => Function.Call<VehicleRoofState>(Hash.GET_CONVERTIBLE_ROOF_STATE, Handle);
			set
			{
				switch (value)
				{
					case VehicleRoofState.Closed:
						Function.Call(Hash.RAISE_CONVERTIBLE_ROOF, Handle, true);
						Function.Call(Hash.RAISE_CONVERTIBLE_ROOF, Handle, false);
						break;
					case VehicleRoofState.Closing:
						Function.Call(Hash.RAISE_CONVERTIBLE_ROOF, Handle, false);
						break;
					case VehicleRoofState.Opened:
						Function.Call(Hash.LOWER_CONVERTIBLE_ROOF, Handle, true);
						Function.Call(Hash.LOWER_CONVERTIBLE_ROOF, Handle, false);
						break;
					case VehicleRoofState.Opening:
						Function.Call(Hash.LOWER_CONVERTIBLE_ROOF, Handle, false);
						break;
				}
			}
		}

		public VehicleLockStatus LockStatus
		{
			get => Function.Call<VehicleLockStatus>(Hash.GET_VEHICLE_DOOR_LOCK_STATUS, Handle);
			set => Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED, Handle, (int)value);
		}

		public VehicleLandingGear LandingGear
		{
			get => (VehicleLandingGear)Function.Call<int>(Hash._GET_VEHICLE_LANDING_GEAR, Handle);
			set => Function.Call(Hash._SET_VEHICLE_LANDING_GEAR, Handle, (int)value);
		}

		public VehicleDoor[] GetDoors()
		{
			System.Collections.Generic.List<VehicleDoor> Doors = new System.Collections.Generic.List<VehicleDoor>();
			if (HasBone("door_dside_f"))
				Doors.Add(VehicleDoor.FrontLeftDoor);
			if (HasBone("door_pside_f"))
				Doors.Add(VehicleDoor.FrontRightDoor);
			if (HasBone("door_dside_r"))
				Doors.Add(VehicleDoor.BackLeftDoor);
			if (HasBone("door_pside_r"))
				Doors.Add(VehicleDoor.BackRightDoor);
			if (HasBone("bonnet"))
				Doors.Add(VehicleDoor.Hood);
			if (HasBone("hood"))
				Doors.Add(VehicleDoor.Trunk);
			return Doors.ToArray();
		}

		public void OpenDoor(VehicleDoor door, bool loose, bool instantly)
		{
			Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, Handle, (int)(door), loose, instantly);
		}

		public void CloseDoor(VehicleDoor door, bool instantly)
		{
			Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, Handle, (int)(door), instantly);
		}

		public bool IsDoorOpen(VehicleDoor door)
		{
			return Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, Handle, (int)(door)) > 0.0f;
		}

		public void BreakDoor(VehicleDoor door)
		{
			Function.Call(Hash.SET_VEHICLE_DOOR_BROKEN, Handle, (int)(door));
		}

		public bool IsDoorBroken(VehicleDoor door)
		{
			return Function.Call<bool>(Hash.IS_VEHICLE_DOOR_DAMAGED, Handle, (int)(door));
		}

		public float GetDoorAngleRatio(VehicleDoor door)
		{
			return Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, Handle, (int)(door));
		}

		public void SetDoorBreakable(VehicleDoor door, bool isBreakable)
		{
			Function.Call(Hash._SET_VEHICLE_DOOR_BREAKABLE, Handle, (int)(door), isBreakable);
		}

		public void FixWindow(VehicleWindow window)
		{
			Function.Call(Hash.FIX_VEHICLE_WINDOW, Handle, (int)(window));
		}

		public void SmashWindow(VehicleWindow window)
		{
			Function.Call(Hash.SMASH_VEHICLE_WINDOW, Handle, (int)(window));
		}

		public void RollUpWindow(VehicleWindow window)
		{
			Function.Call(Hash.ROLL_UP_WINDOW, Handle, (int)(window));
		}

		public void RollDownWindow(VehicleWindow window)
		{
			Function.Call(Hash.ROLL_DOWN_WINDOW, Handle, (int)(window));
		}

		public void RollDownWindows()
		{
			Function.Call(Hash.ROLL_DOWN_WINDOWS, Handle);
		}

		public void RemoveWindow(VehicleWindow window)
		{
			Function.Call(Hash.REMOVE_VEHICLE_WINDOW, Handle, (int)(window));
		}

		#endregion

		#region Burnout

		public bool IsInBurnout()
		{
			return Function.Call<bool>(Hash.IS_VEHICLE_IN_BURNOUT, Handle);
		}

		public bool HandbrakeOn
		{
			set => Function.Call(Hash.SET_VEHICLE_HANDBRAKE, Handle, value);
		}

		#endregion

		#region Occupants

		public Ped Driver => GetPedOnSeat(VehicleSeat.Driver);

		public Ped GetPedOnSeat(VehicleSeat seat)
		{
			return Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, Handle, (int)(seat));
		}

		public Ped[] Occupants
		{
			get
			{
				Ped driver = Driver;

				int arraySize = Entity.Exists(driver) ? PassengerCount + 1 : PassengerCount;
				Ped[] occupantsArray = new Ped[arraySize];
				int occupantIndex = 0;

				if (arraySize == 0)
				{
					return occupantsArray;
				}

				if (Entity.Exists(driver))
				{
					occupantsArray[0] = driver;
					++occupantIndex;
				}

				for (int i = 0, seats = PassengerSeats; i < seats; i++)
				{
					Ped ped = GetPedOnSeat((VehicleSeat)i);

					if (!Entity.Exists(ped))
					{
						continue;
					}

					occupantsArray[occupantIndex] = ped;
					++occupantIndex;

					if (occupantIndex >= arraySize)
					{
						return occupantsArray;
					}
				}

				return occupantsArray;
			}
		}

		public Ped[] Passengers
		{
			get
			{
				var passengersArray = new Ped[PassengerCount];
				int passengerIndex = 0;

				if (passengersArray.Length == 0)
				{
					return passengersArray;
				}

				for (int i = 0, seats = PassengerSeats; i < seats; i++)
				{
					Ped ped = GetPedOnSeat((VehicleSeat)i);

					if (!Entity.Exists(ped))
					{
						continue;
					}

					passengersArray[passengerIndex] = ped;
					++passengerIndex;

					if (passengerIndex >= passengersArray.Length)
					{
						return passengersArray;
					}
				}

				return passengersArray;
			}
		}

		public int PassengerCount => Function.Call<int>(Hash.GET_VEHICLE_NUMBER_OF_PASSENGERS, Handle);

		public int PassengerSeats => Function.Call<int>(Hash.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, Handle);

		public Ped CreatePedOnSeat(VehicleSeat seat, GTA.Model model)
		{
			if (!model.IsPed || !model.Request(1000))
			{
				return null;
			}

			return Function.Call<Ped>(Hash.CREATE_PED_INSIDE_VEHICLE, Handle, 26, model.Hash, (int)seat, 1, 1);
		}

		public Ped CreateRandomPedOnSeat(VehicleSeat seat)
		{
			if (seat == VehicleSeat.Driver)
			{
				return Function.Call<Ped>(Hash.CREATE_RANDOM_PED_AS_DRIVER, Handle, true);
			}
			else
			{
				Ped ped = Function.Call<Ped>(Hash.CREATE_RANDOM_PED, 0.0f, 0.0f, 0.0f);
				Function.Call(Hash.SET_PED_INTO_VEHICLE, ped.Handle, Handle, (int)seat);

				return ped;
			}
		}

		public bool IsSeatFree(VehicleSeat seat)
		{
			return Function.Call<bool>(Hash.IS_VEHICLE_SEAT_FREE, Handle, (int)seat);
		}

		#endregion

		#region Positioning

		public bool IsStopped => Function.Call<bool>(Hash.IS_VEHICLE_STOPPED, Handle);

		public bool IsStoppedAtTrafficLights => Function.Call<bool>(Hash.IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS, Handle);

		public bool IsOnAllWheels => Function.Call<bool>(Hash.IS_VEHICLE_ON_ALL_WHEELS, Handle);

		public bool PlaceOnGround()
		{
			return Function.Call<bool>(Hash.SET_VEHICLE_ON_GROUND_PROPERLY, Handle);
		}

		public void PlaceOnNextStreet()
		{
			Vector3 pos = Position;
			OutputArgument outPos = new OutputArgument();

			for (int i = 1; i < 40; i++)
			{
				float heading;
				float val;
				unsafe
				{
					Function.Call(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING, pos.X, pos.Y, pos.Z, i, outPos, &heading, &val, 1, 0x40400000, 0);
				}
				Vector3 newPos = outPos.GetResult<Vector3>();

				if (!Function.Call<bool>(Hash.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY, newPos.X, newPos.Y, newPos.Z, 5.0f, 5.0f, 5.0f, 0))
				{
					Position = newPos;
					PlaceOnGround();
					Heading = heading;
					break;
				}
			}
		}

		public bool ProvidesCover
		{
			set => Function.Call(Hash.SET_VEHICLE_PROVIDES_COVER, Handle, value);
		}

		#endregion

		#region Towing

		public bool HasForks => HasBone("forks");

		public bool HasTowArm => HasBone("tow_arm");

		public float TowingCraneRaisedAmount
		{
			set => Function.Call(Hash._SET_TOW_TRUCK_CRANE_RAISED, Handle, value);
		}

		public void TowVehicle(Vehicle vehicle, bool rear)
		{
			Function.Call(Hash.ATTACH_VEHICLE_TO_TOW_TRUCK, Handle, vehicle.Handle, rear, 0.0f, 0.0f, 0.0f);
		}

		public void DetachFromTowTruck()
		{
			Function.Call(Hash.DETACH_VEHICLE_FROM_ANY_TOW_TRUCK, Handle);
		}

		public void DetachTowedVehicle()
		{
			Vehicle vehicle = TowedVehicle;

			if (Entity.Exists(vehicle))
			{
				Function.Call(Hash.DETACH_VEHICLE_FROM_TOW_TRUCK, Handle, vehicle.Handle);
			}
		}

		public Vehicle TowedVehicle => Function.Call<Vehicle>(Hash.GET_ENTITY_ATTACHED_TO_TOW_TRUCK, Handle);

		#endregion

		#region Carbobob

		public bool HasBombBay => HasBone("door_hatch_l") && HasBone("door_hatch_r");

		public void OpenBombBay()
		{
			if (HasBombBay)
			{
				Function.Call(Hash._OPEN_VEHICLE_BOMB_BAY, Handle);
			}
		}
		public void CloseBombBay()
		{
			if (HasBombBay)
			{
				Function.Call(Hash._0x3556041742A0DC74, Handle);
			}
		}

		public void SetHeliYawPitchRollMult(float mult)
		{
			if (IsVehicleHeliOrBlimp(Handle) && mult >= 0.0f && mult <= 1.0f)
			{
				Function.Call(Hash._0x6E0859B530A365CC, Handle, mult);
			}

			bool IsVehicleHeliOrBlimp(int handle)
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.VehicleTypeOffsetInCVehicle == 0)
				{
					return false;
				}

				var vehicleTypeValue = (uint)SHVDN.NativeMemory.ReadInt32(address + SHVDN.NativeMemory.VehicleTypeOffsetInCVehicle);
				return (vehicleTypeValue - 8) <= 1;
			}
		}

		public void DropCargobobHook(CargobobHook hookType)
		{
			if (Model.IsCargobob)
			{
				Function.Call(Hash._0x7BEB0C7A235F6F3B, Handle, (int)(hookType));
			}
		}

		public void RemoveCargobobHook()
		{
			if (Model.IsCargobob)
			{
				Function.Call(Hash._0x9768CF648F54C804, Handle);
			}
		}

		public bool IsCargobobHookActive()
		{
			if (Model.IsCargobob)
			{
				return Function.Call<bool>(Hash._0x1821D91AD4B56108, Handle) || Function.Call<bool>(Hash._0x6E08BF5B3722BAC9, Handle);
			}

			return false;
		}
		public bool IsCargobobHookActive(CargobobHook hookType)
		{
			if (Model.IsCargobob)
			{
				switch (hookType)
				{
					case CargobobHook.Hook:
						return Function.Call<bool>(Hash._0x1821D91AD4B56108, Handle);
					case CargobobHook.Magnet:
						return Function.Call<bool>(Hash._0x6E08BF5B3722BAC9, Handle);
				}
			}

			return false;
		}

		public void CargoBobMagnetGrabVehicle()
		{
			if (IsCargobobHookActive(CargobobHook.Magnet))
			{
				Function.Call(Hash._0x9A665550F8DA349B, Handle, true);
			}
		}

		public void CargoBobMagnetReleaseVehicle()
		{
			if (IsCargobobHookActive(CargobobHook.Magnet))
			{
				Function.Call(Hash._0x9A665550F8DA349B, Handle, false);
			}
		}

		#endregion
	}
}
