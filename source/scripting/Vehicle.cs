using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	public enum CargobobHook
	{
		Hook,
		Magnet
	}
	public enum LicensePlateStyle
	{
		BlueOnWhite1 = 3,
		BlueOnWhite2 = 0,
		BlueOnWhite3 = 4,
		YellowOnBlack = 1,
		YellowOnBlue = 2,		
		NorthYankton = 5
	}
	public enum LicensePlateType
	{
		FrontAndRearPlates,
		FrontPlate,
		RearPlate,
		None
	}
	public enum VehicleClass
	{
		Compacts,
		Sedans,
		SUVs,
		Coupes,
		Muscle,
		SportsClassics,
		Sports,
		Super,
		Motorcycles,
		OffRoad,
		Industrial,
		Utility,
		Vans,
		Cycles,
		Boats,
		Helicopters,
		Planes,
		Service,
		Emergency,
		Military,
		Commercial,
		Trains
	}
	public enum VehicleColor
	{
		MetallicBlack,
		MetallicGraphiteBlack,
		MetallicBlackSteel,
		MetallicDarkSilver,
		MetallicSilver,
		MetallicBlueSilver,
		MetallicSteelGray,
		MetallicShadowSilver,
		MetallicStoneSilver,
		MetallicMidnightSilver,
		MetallicGunMetal,
		MetallicAnthraciteGray,
		MatteBlack,
		MatteGray,
		MatteLightGray,
		UtilBlack,
		UtilBlackPoly,
		UtilDarksilver,
		UtilSilver,
		UtilGunMetal,
		UtilShadowSilver,
		WornBlack,
		WornGraphite,
		WornSilverGray,
		WornSilver,
		WornBlueSilver,
		WornShadowSilver,
		MetallicRed,
		MetallicTorinoRed,
		MetallicFormulaRed,
		MetallicBlazeRed,
		MetallicGracefulRed,
		MetallicGarnetRed,
		MetallicDesertRed,
		MetallicCabernetRed,
		MetallicCandyRed,
		MetallicSunriseOrange,
		MetallicClassicGold,
		MetallicOrange,
		MatteRed,
		MatteDarkRed,
		MatteOrange,
		MatteYellow,
		UtilRed,
		UtilBrightRed,
		UtilGarnetRed,
		WornRed,
		WornGoldenRed,
		WornDarkRed,
		MetallicDarkGreen,
		MetallicRacingGreen,
		MetallicSeaGreen,
		MetallicOliveGreen,
		MetallicGreen,
		MetallicGasolineBlueGreen,
		MatteLimeGreen,
		UtilDarkGreen,
		UtilGreen,
		WornDarkGreen,
		WornGreen,
		WornSeaWash,
		MetallicMidnightBlue,
		MetallicDarkBlue,
		MetallicSaxonyBlue,
		MetallicBlue,
		MetallicMarinerBlue,
		MetallicHarborBlue,
		MetallicDiamondBlue,
		MetallicSurfBlue,
		MetallicNauticalBlue,
		MetallicBrightBlue,
		MetallicPurpleBlue,
		MetallicSpinnakerBlue,
		MetallicUltraBlue,
		UtilDarkBlue = 75,
		UtilMidnightBlue,
		UtilBlue,
		UtilSeaFoamBlue,
		UtilLightningBlue,
		UtilMauiBluePoly,
		UtilBrightBlue,
		MatteDarkBlue,
		MatteBlue,
		MatteMidnightBlue,
		WornDarkBlue,
		WornBlue,
		WornLightBlue,
		MetallicTaxiYellow,
		MetallicRaceYellow,
		MetallicBronze,
		MetallicYellowBird,
		MetallicLime,
		MetallicChampagne,
		MetallicPuebloBeige,
		MetallicDarkIvory,
		MetallicChocoBrown,
		MetallicGoldenBrown,
		MetallicLightBrown,
		MetallicStrawBeige,
		MetallicMossBrown,
		MetallicBistonBrown,
		MetallicBeechwood,
		MetallicDarkBeechwood,
		MetallicChocoOrange,
		MetallicBeachSand,
		MetallicSunBleechedSand,
		MetallicCream,
		UtilBrown,
		UtilMediumBrown,
		UtilLightBrown,
		MetallicWhite,
		MetallicFrostWhite,
		WornHoneyBeige,
		WornBrown,
		WornDarkBrown,
		WornStrawBeige,
		BrushedSteel,
		BrushedBlackSteel,
		BrushedAluminium,
		Chrome,
		WornOffWhite,
		UtilOffWhite,
		WornOrange,
		WornLightOrange,
		MetallicSecuricorGreen,
		WornTaxiYellow,
		PoliceCarBlue,
		MatteGreen,
		MatteBrown,
		MatteWhite = 131,
		WornWhite,
		WornOliveArmyGreen,
		PureWhite,
		HotPink,
		Salmonpink,
		MetallicVermillionPink,
		Orange,
		Green,
		Blue,
		MettalicBlackBlue,
		MetallicBlackPurple,
		MetallicBlackRed,
		HunterGreen,
		MetallicPurple,
		MetaillicVDarkBlue,
		ModshopBlack1,
		MattePurple,
		MatteDarkPurple,
		MetallicLavaRed,
		MatteForestGreen,
		MatteOliveDrab,
		MatteDesertBrown,
		MatteDesertTan,
		MatteFoliageGreen,
		DefaultAlloyColor,
		EpsilonBlue,
		PureGold,
		BrushedGold
	}
	public enum VehicleLandingGearState
	{
		Deployed,
		Closing,
		Opening,
		Retracted
	}
	public enum VehicleLockStatus
	{
		None,
		Unlocked,
		Locked,
		LockedForPlayer,
		StickPlayerInside,
		CanBeBrokenInto = 7,
		CanBeBrokenIntoPersist,
		CannotBeTriedToEnter = 10
	}
	public enum VehicleNeonLight
	{
		Left,
		Right,
		Front,
		Back
	}
	public enum VehicleRoofState
	{
		Closed,
		Opening,
		Opened,
		Closing
	}
	public enum VehicleSeat
	{
		None = -3,
		Any,
		Driver,
		Passenger,
		LeftFront = -1,
		RightFront,
		LeftRear,
		RightRear,
		ExtraSeat1,
		ExtraSeat2,
		ExtraSeat3,
		ExtraSeat4,
		ExtraSeat5,
		ExtraSeat6,
		ExtraSeat7,
		ExtraSeat8,
		ExtraSeat9,
		ExtraSeat10,
		ExtraSeat11,
		ExtraSeat12
	}
	public enum VehicleWindowTint
	{
		None,
		PureBlack,
		DarkSmoke,
		LightSmoke,
		Stock,
		Limo,
		Green
	}

	public sealed class Vehicle : Entity
	{
		#region Fields
		VehicleDoorCollection _doors;
		VehicleModCollection _mods;
		VehicleWheelCollection _wheels;
		VehicleWindowCollection _windows;
		#endregion

		public Vehicle(int handle) : base(handle)
		{
		}

		public string DisplayName
		{
			get
			{
				return Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, base.Model.Hash);
			}
		}
		public string FriendlyName
		{
			get
			{
				return Game.GetGXTEntry(DisplayName);
			}
		}
		public VehicleClass ClassType
		{
			get
			{
				return Function.Call<VehicleClass>(Hash.GET_VEHICLE_CLASS, Handle);
			}
		}

		public float BodyHealth
		{
			get
			{
				return Function.Call<float>(Hash.GET_VEHICLE_BODY_HEALTH, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_BODY_HEALTH, Handle, value);
			}
		}
		public float EngineHealth
		{
			get
			{
				return Function.Call<float>(Hash.GET_VEHICLE_ENGINE_HEALTH, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_ENGINE_HEALTH, Handle, value);
			}
		}
		public float PetrolTankHealth
		{
			get
			{
				return Function.Call<float>(Hash.GET_VEHICLE_PETROL_TANK_HEALTH, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_PETROL_TANK_HEALTH, Handle, value);
			}
		}
		public float FuelLevel
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x768 : 0x758;

				return MemoryAccess.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x768 : 0x758;

				MemoryAccess.WriteFloat(MemoryAddress + offset, value);
			}
		}

		public bool IsEngineRunning
		{
			get
			{
				return Function.Call<bool>(Hash._IS_VEHICLE_ENGINE_ON, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_ENGINE_ON, Handle, value, true);
			}
		}
		public bool IsRadioEnabled
		{
			set
			{
				Function.Call(Hash.SET_VEHICLE_RADIO_ENABLED, Handle, value);
			}
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
					Function.Call(Hash.SET_VEH_RADIO_STATION, Game._radioNames[(int)value]);
				}
			}
		}

		public float Speed
		{
			get
			{
				return Function.Call<float>(Hash.GET_ENTITY_SPEED, Handle);
			}
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
				if (MemoryAddress == IntPtr.Zero)
				{
					return 0.0f;
				}

				//old game version hasnt been tested, just following the patterns above for old game ver
				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x9A4 : 0x994;

				return MemoryAccess.ReadFloat(MemoryAddress + offset);
			}
		}
		public float Acceleration
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x7E4 : 0x7D4;

				return MemoryAccess.ReadFloat(MemoryAddress + offset);
			}
		}
		public float CurrentRPM
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 2004 : 1988;

				return MemoryAccess.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 2004 : 1988;

				MemoryAccess.WriteFloat(MemoryAddress + offset, value);
			}
		}

		public int HighGear
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return 0;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x7A6 : 0x796;

				return (int)MemoryAccess.ReadByte(MemoryAddress + offset);
			}
			set
			{
				if (value < 0 || value > byte.MaxValue)
				{
					throw new ArgumentOutOfRangeException("value", "Values must be between 0 and 255, inclusive.");
				}

				if (MemoryAddress == IntPtr.Zero)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x7A6 : 0x796;

				MemoryAccess.WriteByte(MemoryAddress + offset, (byte)value);
			}
		}
		public int CurrentGear
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return 0;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x7A0 : 0x790;

				return (int)MemoryAccess.ReadByte(MemoryAddress + offset);
			}
		}

		public float SteeringAngle
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x8AC : 0x89C;

				return (float)(MemoryAccess.ReadFloat(MemoryAddress + offset) * (180.0 / System.Math.PI));
			}
		}
		public float SteeringScale
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x8A4 : 0x894;

				return MemoryAccess.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x8A4 : 0x894;

				MemoryAccess.WriteFloat(MemoryAddress + offset, value);
			}
		}

		public bool HasForks
		{
			get
			{
				return HasBone("forks");
			}
		}

		public bool HasAlarm
		{
			set
			{
				Function.Call(Hash.SET_VEHICLE_ALARM, Handle, value);
			}
		}
		public bool AlarmActive
		{
			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_ALARM_ACTIVATED, Handle);
			}
		}
		public void StartAlarm()
		{
			Function.Call(Hash.START_VEHICLE_ALARM, Handle);
		}

		public bool HasSiren
		{
			get
			{
				return HasBone("siren1");
			}
		}
		public bool SirenActive
		{
			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_SIREN_ON, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_SIREN, Handle, value);
			}
		}
		public bool IsSirenSilent
		{
			set
			{
				// Sets if the siren is silent actually 
				Function.Call(Hash.DISABLE_VEHICLE_IMPACT_EXPLOSION_ACTIVATION, Handle, value);
			}
		}
		public void SoundHorn(int duration)
		{
			Function.Call(Hash.START_VEHICLE_HORN, Handle, duration, Game.GenerateHash("HELDDOWN"), 0);
		}

		public int Livery
		{
			get
			{
				int modCount = Mods[VehicleModType.Livery].ModCount;

				if (modCount > 0)
				{
					return modCount;
				}

				return Function.Call<int>(Hash.GET_VEHICLE_LIVERY, Handle);
			}
			set
			{
				if (Mods[VehicleModType.Livery].ModCount > 0)
				{
					Mods[VehicleModType.Livery].Index = value;
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
				int modCount = Mods[VehicleModType.Livery].ModCount;

				if (modCount > 0)
				{
					return modCount;
				}

				return Function.Call<int>(Hash.GET_VEHICLE_LIVERY_COUNT, Handle);
			}
		}

		public bool IsWanted
		{
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return false;
				}
				//Unsure of the exact version this switched, but all others in the rangs are the same
				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x84C : 0x83C;

				return (MemoryAccess.ReadByte(memoryAddress + offset) & 8) != 0;
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_IS_WANTED, Handle, value);
			}
		}

		public bool ProvidesCover
		{
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return false;
				}
				//Unsure of the exact version this switched, but all others in the rangs are the same
				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x83C : 0x82C;

				return (MemoryAccess.ReadByte(memoryAddress + offset) & 4) != 0;
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_PROVIDES_COVER, Handle, value);
			}
		}

		public bool DropsMoneyOnExplosion
		{		   
			get
			{
				IntPtr memoryAddress = MemoryAddress;
				if (memoryAddress == IntPtr.Zero)
				{
					return false;
				}
				//Unsure of the exact version this switched or if it switched over a few title updates
				//as its shifted by 0x20 bytes where as rest are only 0x10 bytes
				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0xA98 : 0xA78;

				if (MemoryAccess.ReadInt(memoryAddress + offset) <= 8)
				{
					return (MemoryAccess.ReadByte(memoryAddress + 0x12F9) & 2) != 0;
				}
				return false;
			}
			set
			{
				Function.Call(Hash._SET_VEHICLE_CREATES_MONEY_PICKUPS_WHEN_EXPLODED, Handle, value);
			}
		}

		public bool PreviouslyOwnedByPlayer
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return false;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x844 : 0x834;

				return (MemoryAccess.ReadInt(MemoryAddress + offset) & (1 << 1)) != 0;
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER, Handle, value);
			}
		}

		public bool NeedsToBeHotwired
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return false;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x844 : 0x834;

				return (MemoryAccess.ReadInt(MemoryAddress + offset) & (1 << 2)) != 0;
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_NEEDS_TO_BE_HOTWIRED, Handle, value);
			}
		}

		public bool LightsOn
		{
			get
			{
				var lightState1 = new OutputArgument();
				var lightState2 = new OutputArgument();
				Function.Call(Hash.GET_VEHICLE_LIGHTS_STATE, Handle, lightState1, lightState2);

				return lightState1.GetResult<bool>();
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_LIGHTS, Handle, value ? 3 : 4);
			}
		}
		public bool HighBeamsOn
		{
			get
			{
				var lightState1 = new OutputArgument();
				var lightState2 = new OutputArgument();
				Function.Call(Hash.GET_VEHICLE_LIGHTS_STATE, Handle);

				return lightState2.GetResult<bool>();
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_FULLBEAM, Handle, value);
			}
		}
		public bool InteriorLightOn
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return false;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x841 : 0x831;

				return (MemoryAccess.ReadByte(MemoryAddress + offset) & (1 << 6)) != 0;
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_INTERIORLIGHT, Handle, value);
			}
		}
		public bool SearchLightOn
		{
			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_SEARCHLIGHT_ON, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_SEARCHLIGHT, Handle, value, 0);
			}
		}
		public bool TaxiLightOn
		{
			get
			{
				return Function.Call<bool>(Hash.IS_TAXI_LIGHT_ON, Handle);
			}
			set
			{
				Function.Call(Hash.SET_TAXI_LIGHTS, Handle, value);
			}
		}
		public bool LeftIndicatorLightOn
		{
			set
			{
				Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, Handle, true, value);
			}
		}
		public bool RightIndicatorLightOn
		{
			set
			{
				Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, Handle, false, value);
			}
		}
		public bool HandbrakeOn
		{
			set
			{
				Function.Call(Hash.SET_VEHICLE_HANDBRAKE, Handle, value);
			}
		}
		public bool BrakeLightsOn
		{
			set
			{
				Function.Call(Hash.SET_VEHICLE_BRAKE_LIGHTS, Handle, value);
			}
		}
		public float LightsMultiplier
		{
			set
			{
				Function.Call(Hash.SET_VEHICLE_LIGHT_MULTIPLIER, Handle, value);
			}
		}

		public bool CanBeVisiblyDamaged
		{
			set
			{
				Function.Call(Hash.SET_VEHICLE_CAN_BE_VISIBLY_DAMAGED, Handle, value);
			}
		}

		public bool IsDamaged
		{
			get
			{
				return Function.Call<bool>(Hash._IS_VEHICLE_DAMAGED, Handle);
			}
		}
		public bool IsDriveable
		{
			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_DRIVEABLE, Handle, 0);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, Handle, !value);
			}
		}
		public bool HasRoof
		{
			get
			{
				return Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_ROOF, Handle);
			}
		}
		public bool IsLeftHeadLightBroken
		{
			get
			{
				return Function.Call<bool>(Hash._IS_HEADLIGHT_L_BROKEN, Handle);
			}
			set
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return;
				}

				IntPtr address = MemoryAddress + 1916;
				const int mask = 1 << 0;

				if (value)
				{
					MemoryAccess.WriteByte(address, (byte)(MemoryAccess.ReadByte(address) | mask));
				}
				else
				{
					MemoryAccess.WriteByte(address, (byte)(MemoryAccess.ReadByte(address) & ~mask));
				}
			}
		}
		public bool IsRightHeadLightBroken
		{
			get
			{
				return Function.Call<bool>(Hash._IS_HEADLIGHT_R_BROKEN, Handle);
			}
			set
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return;
				}

				IntPtr address = MemoryAddress + 1916;
				const int mask = 1 << 1;

				if (value)
				{
					MemoryAccess.WriteByte(address, (byte)(MemoryAccess.ReadByte(address) | mask));
				}
				else
				{
					MemoryAccess.WriteByte(address, (byte)(MemoryAccess.ReadByte(address) & ~mask));
				}
			}
		}
		public bool IsRearBumperBrokenOff
		{
			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_BUMPER_BROKEN_OFF, Handle, false);
			}
		}
		public bool IsFrontBumperBrokenOff
		{
			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_BUMPER_BROKEN_OFF, Handle, true);
			}
		}

		public bool IsAxlesStrong
		{
			set
			{
				Function.Call<bool>(Hash.SET_VEHICLE_HAS_STRONG_AXLES, Handle, value);
			}
		}

		public bool CanEngineDegrade
		{
			set
			{
				Function.Call(Hash.SET_VEHICLE_ENGINE_CAN_DEGRADE, Handle, value);
			}
		}
		public float EnginePowerMultiplier
		{
			set
			{
				Function.Call(Hash._SET_VEHICLE_ENGINE_POWER_MULTIPLIER, Handle, value);
			}
		}
		public float EngineTorqueMultiplier
		{
			set
			{
				Function.Call(Hash._SET_VEHICLE_ENGINE_TORQUE_MULTIPLIER, Handle, value);
			}
		}

		public VehicleWindowTint WindowTint
		{
			get
			{
				return Function.Call<VehicleWindowTint>(Hash.GET_VEHICLE_WINDOW_TINT, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_WINDOW_TINT, Handle, value);
			}
		}

		public VehicleColor PrimaryColor
		{
			get
			{
				var color1 = new OutputArgument();
				var color2 = new OutputArgument();
				Function.Call(Hash.GET_VEHICLE_COLOURS, Handle, color1, color2);

				return color1.GetResult<VehicleColor>();
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_COLOURS, Handle, value, SecondaryColor);
			}
		}
		public VehicleColor SecondaryColor
		{
			get
			{
				var color1 = new OutputArgument();
				var color2 = new OutputArgument();
				Function.Call(Hash.GET_VEHICLE_COLOURS, Handle, color1, color2);

				return color2.GetResult<VehicleColor>();
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_COLOURS, Handle, PrimaryColor, value);
			}
		}
		public VehicleColor RimColor
		{
			get
			{
				var color1 = new OutputArgument();
				var color2 = new OutputArgument();
				Function.Call(Hash.GET_VEHICLE_EXTRA_COLOURS, Handle, color1, color2);

				return color2.GetResult<VehicleColor>();
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_EXTRA_COLOURS, Handle, PearlescentColor, value);
			}
		}
		public VehicleColor PearlescentColor
		{
			get
			{
				var color1 = new OutputArgument();
				var color2 = new OutputArgument();
				Function.Call(Hash.GET_VEHICLE_EXTRA_COLOURS, Handle, color1, color2);

				return color1.GetResult<VehicleColor>();
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_EXTRA_COLOURS, Handle, value, RimColor);
			}
		}
		public VehicleColor TrimColor
		{
			get
			{
				var color = new OutputArgument();
				Function.Call((Hash)9012939617897488694uL, Handle, color);

				return color.GetResult<VehicleColor>();
			}
			set
			{
				Function.Call((Hash)17585947422526242585uL, Handle, value);
			}
		}
		public VehicleColor DashboardColor
		{
			get
			{
				var color = new OutputArgument();
				Function.Call((Hash)13214509638265019391uL, Handle, color);

				return color.GetResult<VehicleColor>();
			}
			set
			{
				Function.Call((Hash)6956317558672667244uL, Handle, value);
			}
		}
		public int ColorCombination
		{
			get
			{
				return Function.Call<int>(Hash.GET_VEHICLE_COLOUR_COMBINATION, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_COLOUR_COMBINATION, Handle, value);
			}
		}
		public int ColorCombinationCount
		{
			get
			{
				return Function.Call<int>(Hash.GET_NUMBER_OF_VEHICLE_COLOURS, Handle);
			}
		}
		public Color TireSmokeColor
		{
			get
			{
				var red = new OutputArgument();
				var green = new OutputArgument();
				var blue = new OutputArgument();
				Function.Call(Hash.GET_VEHICLE_TYRE_SMOKE_COLOR, Handle, red, green, blue);

				return Color.FromArgb(red.GetResult<int>(), green.GetResult<int>(), blue.GetResult<int>());
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_TYRE_SMOKE_COLOR, Handle, value.R, value.G, value.B);
			}
		}
		public Color NeonLightsColor
		{
			get
			{
				var red = new OutputArgument();
				var green = new OutputArgument();
				var blue = new OutputArgument();
				Function.Call(Hash._GET_VEHICLE_NEON_LIGHTS_COLOUR, Handle, red, green, blue);

				return Color.FromArgb(red.GetResult<int>(), green.GetResult<int>(), blue.GetResult<int>());
			}
			set
			{
				Function.Call(Hash._SET_VEHICLE_NEON_LIGHTS_COLOUR, Handle, value.R, value.G, value.B);
			}
		}
		public Color CustomPrimaryColor
		{
			get
			{
				var red = new OutputArgument();
				var green = new OutputArgument();
				var blue = new OutputArgument();
				Function.Call(Hash.GET_VEHICLE_CUSTOM_PRIMARY_COLOUR, Handle, red, green, blue);

				return Color.FromArgb(red.GetResult<int>(), green.GetResult<int>(), blue.GetResult<int>());
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_CUSTOM_PRIMARY_COLOUR, Handle, value.R, value.G, value.B);
			}
		}
		public Color CustomSecondaryColor
		{
			get
			{
				var red = new OutputArgument();
				var green = new OutputArgument();
				var blue = new OutputArgument();
				Function.Call(Hash.GET_VEHICLE_CUSTOM_SECONDARY_COLOUR, Handle, red, green, blue);

				return Color.FromArgb(red.GetResult<int>(), green.GetResult<int>(), blue.GetResult<int>());
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_CUSTOM_SECONDARY_COLOUR, Handle, value.R, value.G, value.B);
			}
		}
		public bool IsPrimaryColorCustom
		{
			get
			{
				return Function.Call<bool>(Hash.GET_IS_VEHICLE_PRIMARY_COLOUR_CUSTOM, Handle);
			}
		}
		public bool IsSecondaryColorCustom
		{
			get
			{
				return Function.Call<bool>(Hash.GET_IS_VEHICLE_SECONDARY_COLOUR_CUSTOM, Handle);
			}
		}
		public void ClearCustomPrimaryColor()
		{
			Function.Call(Hash.CLEAR_VEHICLE_CUSTOM_PRIMARY_COLOUR, Handle);
		}
		public void ClearCustomSecondaryColor()
		{
			Function.Call(Hash.CLEAR_VEHICLE_CUSTOM_SECONDARY_COLOUR, Handle);
		}

		public LicensePlateStyle LicensePlateStyle
		{
			get
			{
				return Function.Call<LicensePlateStyle>(Hash.GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, Handle, value);
			}
		}
		public LicensePlateType LicensePlateType
		{
			get
			{
				return Function.Call<LicensePlateType>(Hash.GET_VEHICLE_PLATE_TYPE, Handle);
			}
		}
		public string LicensePlate
		{
			get
			{
				return Function.Call<string>(Hash.GET_VEHICLE_NUMBER_PLATE_TEXT, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT, Handle, value);
			}
		}

		public VehicleLandingGearState LandingGearState
		{
			get
			{
				return Function.Call<VehicleLandingGearState>(Hash._GET_VEHICLE_LANDING_GEAR, Handle);
			}
			set
			{
				Function.Call(Hash._SET_VEHICLE_LANDING_GEAR, Handle, value);
			}
		}
		public VehicleRoofState RoofState
		{
			get
			{
				return Function.Call<VehicleRoofState>(Hash.GET_CONVERTIBLE_ROOF_STATE, Handle);
			}
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
			get
			{
				return Function.Call<VehicleLockStatus>(Hash.GET_VEHICLE_DOOR_LOCK_STATUS, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED, Handle, value);
			}
		}

		public float MaxBraking
		{
			get
			{
				return Function.Call<float>(Hash.GET_VEHICLE_MAX_BRAKING, Handle);
			}
		}
		public float MaxTraction
		{
			get
			{
				return Function.Call<float>(Hash.GET_VEHICLE_MAX_TRACTION, Handle);
			}
		}

		public bool IsOnAllWheels
		{

			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_ON_ALL_WHEELS, Handle);
			}
		}

		public bool IsStopped
		{

			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_STOPPED, Handle);
			}
		}
		public bool IsStoppedAtTrafficLights
		{

			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS, Handle);
			}
		}

		public bool IsStolen
		{
			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_STOLEN, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_IS_STOLEN, Handle, value);
			}
		}

		public bool IsConvertible
		{

			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_A_CONVERTIBLE, Handle, 0);
			}
		}

		public bool IsBurnoutForced
		{
			set
			{
				Function.Call<bool>(Hash.SET_VEHICLE_BURNOUT, Handle, value);
			}
		}
		public bool IsInBurnout
		{
			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_IN_BURNOUT, Handle);
			}
		}

		public Ped Driver
		{
			get
			{
				return GetPedOnSeat(VehicleSeat.Driver);
			}
		}
		public Ped[] Occupants
		{
			get
			{
				Ped driver = Driver;

				if (!Ped.Exists(driver))
				{
					return Passengers;
				}

				var result = new Ped[PassengerCount + 1];
				result[0] = driver;

				for (int i = 0, seats = PassengerCapacity; i < seats && i <= result.Length; i++)
				{
					result[i + 1] = GetPedOnSeat((VehicleSeat)i);
				}

				return result;
			}
		}
		public Ped[] Passengers
		{
			get
			{
				var result = new Ped[PassengerCount];

				if (result.Length == 0)
				{
					return result;
				}

				for (int i = 0, seats = PassengerCapacity; i < seats && i < result.Length; i++)
				{
					result[i] = GetPedOnSeat((VehicleSeat)i);
				}

				return result;
			}
		}
		public int PassengerCapacity
		{
			get
			{
				return Function.Call<int>(Hash.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, Handle);
			}
		}
		public int PassengerCount
		{
			get
			{
				return Function.Call<int>(Hash.GET_VEHICLE_NUMBER_OF_PASSENGERS, Handle);
			}
		}

		public VehicleDoorCollection Doors
		{
			get
			{
				if (_doors == null)
				{
					_doors = new VehicleDoorCollection(this);
				}

				return _doors;
			}
		}
		public VehicleModCollection Mods
		{
			get
			{
				if (_mods == null)
				{
					_mods = new VehicleModCollection(this);
				}

				return _mods;
			}
		}
		public VehicleWheelCollection Wheels
		{
			get
			{
				if (_wheels == null)
				{
					_wheels = new VehicleWheelCollection(this);
				}

				return _wheels;
			}
		}
		public VehicleWindowCollection Windows
		{
			get
			{
				if (_windows == null)
				{
					_windows = new VehicleWindowCollection(this);
				}

				return _windows;
			}
		}

		public bool ExtraExists(int extra)
		{
			return Function.Call<bool>(Hash.DOES_EXTRA_EXIST, Handle, extra);
		}
		public bool IsExtraOn(int extra)
		{
			return Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, Handle, extra);
		}
		public void ToggleExtra(int extra, bool toggle)
		{
			Function.Call(Hash.SET_VEHICLE_EXTRA, Handle, extra, !toggle);
		}

		public Ped GetPedOnSeat(VehicleSeat seat)
		{
			return new Ped(Function.Call<int>(Hash.GET_PED_IN_VEHICLE_SEAT, Handle, seat));
		}
		public bool IsSeatFree(VehicleSeat seat)
		{
			return Function.Call<bool>(Hash.IS_VEHICLE_SEAT_FREE, Handle, seat);
		}

		public void Wash()
		{
			DirtLevel = 0f;
		}
		public float DirtLevel
		{
			get
			{
				return Function.Call<float>(Hash.GET_VEHICLE_DIRT_LEVEL, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_DIRT_LEVEL, Handle, value);
			}
		}

		public bool PlaceOnGround()
		{
			return Function.Call<bool>(Hash.SET_VEHICLE_ON_GROUND_PROPERLY, Handle);
		}
		public void PlaceOnNextStreet()
		{
			Vector3 currentPosition = Position;
			var headingArg = new OutputArgument();
			var newPositionArg = new OutputArgument();

			for (int i = 1; i < 40; i++)
			{
				Function.Call(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING, currentPosition.X, currentPosition.Y, currentPosition.Z, i, newPositionArg, headingArg, new OutputArgument(), 1, 0x40400000, 0);

				var newPosition = newPositionArg.GetResult<Vector3>();

				if (!Function.Call<bool>(Hash.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY, newPosition.X, newPosition.Y, newPosition.Z, 5.0f, 5.0f, 5.0f, 0))
				{
					Position = newPosition;
					PlaceOnGround();
					Heading = headingArg.GetResult<float>();
					break;
				}
			}
		}

		public void Repair()
		{
			Function.Call(Hash.SET_VEHICLE_FIXED, Handle);
		}
		public void Explode()
		{
			Function.Call(Hash.EXPLODE_VEHICLE, Handle, true, false);
		}

		public bool CanTiresBurst
		{
			get
			{
				return Function.Call<bool>(Hash.GET_VEHICLE_TYRES_CAN_BURST, Handle);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_TYRES_CAN_BURST, Handle, value);
			}
		}
		public bool CanWheelsBreak
		{
			set
			{
				Function.Call(Hash.SET_VEHICLE_WHEELS_CAN_BREAK, Handle, value);
			}
		}

		public bool HasBombBay
		{
			get
			{
				return HasBone("door_hatch_l") && HasBone("door_hatch_r");
			}
		}
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
				Function.Call(Hash._CLOSE_VEHICLE_BOMB_BAY, Handle);
			}
		}

		public bool IsNeonLightsOn(VehicleNeonLight light)
		{
			return Function.Call<bool>(Hash._IS_VEHICLE_NEON_LIGHT_ENABLED, Handle, light);
		}
		public void SetNeonLightsOn(VehicleNeonLight light, bool on)
		{
			Function.Call(Hash._SET_VEHICLE_NEON_LIGHT_ENABLED, Handle, light, on);
		}
		public bool HasNeonLights
		{
			get
			{ return Enum.GetValues(typeof(VehicleNeonLight)).Cast<VehicleNeonLight>().Any(HasNeonLight); }
		}
		public bool HasNeonLight(VehicleNeonLight neonLight)
		{
			switch (neonLight)
			{
				case VehicleNeonLight.Left:
					return HasBone("neon_l");
				case VehicleNeonLight.Right:
					return HasBone("neon_r");
				case VehicleNeonLight.Front:
					return HasBone("neon_f");
				case VehicleNeonLight.Back:
					return HasBone("neon_b");
				default:
					return false;
			}
		}

		public void SetHeliYawPitchRollMult(float mult)
		{
			if (Model.IsHelicopter && mult >= 0 && mult <= 1)
			{
				Function.Call(Hash._SET_HELICOPTER_ROLL_PITCH_YAW_MULT, Handle, mult);
			}
		}

		public void DropCargobobHook(CargobobHook hook)
		{
			if (Model.IsCargobob)
			{
				Function.Call(Hash._ENABLE_CARGOBOB_HOOK, Handle, hook);
			}
		}
		public void RetractCargobobHook()
		{
			if (Model.IsCargobob)
			{
				Function.Call(Hash._RETRACT_CARGOBOB_HOOK, Handle);
			}
		}
		public bool IsCargobobHookActive()
		{
			if (Model.IsCargobob)
			{
				return Function.Call<bool>(Hash._IS_CARGOBOB_HOOK_ACTIVE, Handle) || Function.Call<bool>(Hash._IS_CARGOBOB_MAGNET_ACTIVE, Handle);
			}

			return false;
		}
		public bool IsCargobobHookActive(CargobobHook hook)
		{
			if (Model.IsCargobob)
			{
				switch (hook)
				{
					case CargobobHook.Hook:
						return Function.Call<bool>(Hash._IS_CARGOBOB_HOOK_ACTIVE, Handle);
					case CargobobHook.Magnet:
						return Function.Call<bool>(Hash._IS_CARGOBOB_MAGNET_ACTIVE, Handle);
				}
			}

			return false;
		}
		public void CargoBobMagnetGrabVehicle()
		{
			if (IsCargobobHookActive(CargobobHook.Magnet))
			{
				Function.Call(Hash._CARGOBOB_MAGNET_GRAB_VEHICLE, Handle, true);
			}
		}
		public void CargoBobMagnetReleaseVehicle()
		{
			if (IsCargobobHookActive(CargobobHook.Magnet))
			{
				Function.Call(Hash._CARGOBOB_MAGNET_GRAB_VEHICLE, Handle, false);
			}
		}

		public bool HasTowArm
		{
			get
			{
				return HasBone("tow_arm");
			}
		}
		public float TowingCraneRaisedAmount
		{
			set
			{
				Function.Call(Hash._SET_TOW_TRUCK_CRANE_HEIGHT, Handle, value);
			}
		}
		public Vehicle TowedVehicle
		{
			get
			{
				return new Vehicle(Function.Call<int>(Hash.GET_ENTITY_ATTACHED_TO_TOW_TRUCK, Handle));
			}
		}
		public void TowVehicle(Vehicle vehicle, bool rear)
		{
			Function.Call(Hash.ATTACH_VEHICLE_TO_TOW_TRUCK, Handle, vehicle.Handle, rear, 0f, 0f, 0f);
		}
		public void DetachFromTowTruck()
		{
			Function.Call(Hash.DETACH_VEHICLE_FROM_ANY_TOW_TRUCK, Handle);
		}
		public void DetachTowedVehicle()
		{
			Vehicle vehicle = TowedVehicle;

			if (Exists(vehicle))
			{
				Function.Call(Hash.DETACH_VEHICLE_FROM_TOW_TRUCK, Handle, vehicle.Handle);
			}
		}

		public void ApplyDamage(Vector3 position, float damageAmount, float radius)
		{
			Function.Call(Hash.SET_VEHICLE_DAMAGE, position.X, position.Y, position.Z, damageAmount, radius);
		}

		public Ped CreatePedOnSeat(VehicleSeat seat, Model model)
		{
			if (!model.IsPed || !model.Request(1000))
			{
				return null;
			}

			return new Ped(Function.Call<int>(Hash.CREATE_PED_INSIDE_VEHICLE, Handle, 26, model.Hash, seat, 1, 1));
		}
		public Ped CreateRandomPedOnSeat(VehicleSeat seat)
		{
			if (seat == VehicleSeat.Driver)
			{
				return new Ped(Function.Call<int>(Hash.CREATE_RANDOM_PED_AS_DRIVER, Handle, true));
			}
			else
			{
				int pedHandle = Function.Call<int>(Hash.CREATE_RANDOM_PED, 0f, 0f, 0f);
				Function.Call(Hash.SET_PED_INTO_VEHICLE, pedHandle, Handle, seat);

				return new Ped(pedHandle);
			}
		}
	}
}
