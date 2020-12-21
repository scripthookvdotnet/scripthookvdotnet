//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using SHVDN;

namespace GTA
{
	public sealed class Vehicle : Entity
	{
		#region Fields
		VehicleDoorCollection _doors;
		VehicleModCollection _mods;
		VehicleWheelCollection _wheels;
		VehicleWindowCollection _windows;
		#endregion

		internal Vehicle(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Repair all damage to this <see cref="Vehicle"/> instantaneously.
		/// </summary>
		public void Repair()
		{
			Function.Call(Hash.SET_VEHICLE_FIXED, Handle);
		}

		/// <summary>
		/// Explode this <see cref="Vehicle"/> instantaneously.
		/// </summary>
		public void Explode()
		{
			Function.Call(Hash.EXPLODE_VEHICLE, Handle, true, false);
		}

		/// <summary>
		/// Determines if this <see cref="Vehicle"/> exists.
		/// </summary>
		/// <returns><c>true</c> if this <see cref="Vehicle"/> exists; otherwise, <c>false</c></returns>
		public new bool Exists()
		{
			return EntityType == EntityType.Vehicle;
		}

		#region Styling

		public bool IsConvertible => Function.Call<bool>(Hash.IS_VEHICLE_A_CONVERTIBLE, Handle, 0);
		public bool IsBig => Function.Call<bool>(Hash.IS_BIG_VEHICLE, Handle);
		public bool HasBulletProofGlass => SHVDN.NativeMemory.HasVehicleFlag(Model.Hash, NativeMemory.VehicleFlag2.HasBulletProofGlass);
		public bool HasLowriderHydraulics => Game.Version >= GameVersion.v1_0_505_2_Steam && SHVDN.NativeMemory.HasVehicleFlag(Model.Hash, NativeMemory.VehicleFlag2.HasLowriderHydraulics);
		public bool HasDonkHydraulics => Game.Version >= GameVersion.v1_0_505_2_Steam && SHVDN.NativeMemory.HasVehicleFlag(Model.Hash, NativeMemory.VehicleFlag2.HasLowriderDonkHydraulics);
		public bool HasParachute => Game.Version >= GameVersion.v1_0_505_2_Steam && Function.Call<bool>(Hash._GET_VEHICLE_HAS_PARACHUTE, Handle);
		public bool HasRocketBoost => Game.Version >= GameVersion.v1_0_944_2_Steam && Function.Call<bool>(Hash._GET_HAS_ROCKET_BOOST, Handle);


		public float DirtLevel
		{
			get => Function.Call<float>(Hash.GET_VEHICLE_DIRT_LEVEL, Handle);
			set => Function.Call(Hash.SET_VEHICLE_DIRT_LEVEL, Handle, value);
		}

		public VehicleModCollection Mods => _mods ?? (_mods = new VehicleModCollection(this));

		public VehicleWheelCollection Wheels => _wheels ?? (_wheels = new VehicleWheelCollection(this));

		public VehicleWindowCollection Windows => _windows ?? (_windows = new VehicleWindowCollection(this));

		public void Wash()
		{
			DirtLevel = 0f;
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

		#endregion

		#region Configuration

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> was stolen.
		/// </summary>
		public bool IsStolen
		{
			get => Function.Call<bool>(Hash.IS_VEHICLE_STOLEN, Handle);
			set => Function.Call(Hash.SET_VEHICLE_IS_STOLEN, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> is wanted by the police.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> is wanted by the police; otherwise, <c>false</c>.
		/// </value>
		public bool IsWanted
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.IsWantedOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.IsWantedOffset, 3);
			}
			set => Function.Call(Hash.SET_VEHICLE_IS_WANTED, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> needs to be hotwired to start.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> needs to be hotwired to start; otherwise, <c>false</c>.
		/// </value>
		public bool NeedsToBeHotwired
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.NeedsToBeHotwiredOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.NeedsToBeHotwiredOffset, 2);
			}
			set => Function.Call(Hash.SET_VEHICLE_NEEDS_TO_BE_HOTWIRED, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> was previously owned by a <see cref="Player"/>.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Vehicle"/> was previously owned by a <see cref="Player"/>; otherwise, <c>false</c>.
		/// </value>
		public bool PreviouslyOwnedByPlayer
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.PreviouslyOwnedByPlayerOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.PreviouslyOwnedByPlayerOffset, 1);
			}
			set => Function.Call(Hash.SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER, Handle, value);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Vehicle"/> allows <see cref="Ped"/>s to rappel.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Vehicle"/> allows <see cref="Ped"/>s to rappel; otherwise, <c>false</c>.
		/// </value>
		public bool AllowRappel => Game.Version >= GameVersion.v1_0_757_2_Steam
			? Function.Call<bool>(Hash._DOES_VEHICLE_ALLOW_RAPPEL, Handle)
			: SHVDN.NativeMemory.HasVehicleFlag(Model.Hash, NativeMemory.VehicleFlag1.AllowsRappel);

		/// <summary>
		/// Gets a value indicating whether <see cref="Ped"/>s can stand on this <see cref="Vehicle"/> regardless of <see cref="Vehicle"/>s speed.
		/// </summary>
		/// <value>
		/// <c>true</c> if <see cref="Ped"/>s can stand on this <see cref="Vehicle"/> regardless of <see cref="Vehicle"/>s speed; otherwise, <c>false</c>.
		/// </value>
		public bool CanStandOnTop => SHVDN.NativeMemory.HasVehicleFlag(Model.Hash, NativeMemory.VehicleFlag1.CanStandOnTop);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Vehicle"/> can jump.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Vehicle"/> can jump; otherwise, <c>false</c>.
		/// </value>
		public bool CanJump => Game.Version >= GameVersion.v1_0_944_2_Steam && Function.Call<bool>(Hash._GET_CAN_VEHICLE_JUMP, Handle);

		/// <summary>
		/// Gets the display name of this <see cref="Vehicle"/>.
		/// <remarks>Use <see cref="Game.GetLocalizedString(string)"/> to get the localized name.</remarks>
		/// </summary>
		public string DisplayName => GetModelDisplayName(base.Model);
		/// <summary>
		/// Gets the localized name of this <see cref="Vehicle"/>
		/// </summary>
		public string LocalizedName => Game.GetLocalizedString(DisplayName);

		/// <summary>
		/// Gets the display name of this <see cref="Vehicle"/>s <see cref="VehicleClass"/>.
		/// <remarks>Use <see cref="Game.GetLocalizedString(string)"/> to get the localized class name.</remarks>
		/// </summary>
		public string ClassDisplayName => GetClassDisplayName(ClassType);
		/// <summary>
		/// Gets the localized name of this <see cref="Vehicle"/>s <see cref="VehicleClass"/>.
		/// </summary>
		public string ClassLocalizedName => Game.GetLocalizedString(ClassDisplayName);

		/// <summary>
		/// Gets the class of this <see cref="Vehicle"/>.
		/// </summary>
		public VehicleClass ClassType => Function.Call<VehicleClass>(Hash.GET_VEHICLE_CLASS, Handle);

		public float LodMultiplier
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x1204 : 0x11F4; // untested
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x1224 : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x1264 : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x1274 : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x12A8 : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x12C8 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x1328 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x1368 : offset;

				return SHVDN.NativeMemory.ReadFloat(address + offset);
			}
			set => Function.Call(Hash.SET_VEHICLE_LOD_MULTIPLIER, Handle, value);
		}

		public HandlingData HandlingData
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.HandlingDataOffset == 0)
				{
					return new HandlingData(IntPtr.Zero);
				}

				return new HandlingData(SHVDN.NativeMemory.ReadAddress(MemoryAddress + SHVDN.NativeMemory.HandlingDataOffset));
			}
		}

		#endregion

		#region Health

		/// <summary>
		/// Gets or sets this <see cref="Vehicle"/>s body health.
		/// </summary>
		public float BodyHealth
		{
			get => Function.Call<float>(Hash.GET_VEHICLE_BODY_HEALTH, Handle);
			set => Function.Call(Hash.SET_VEHICLE_BODY_HEALTH, Handle, value);
		}

		/// <summary>
		/// Gets or sets this <see cref="Vehicle"/> engine health.
		/// </summary>
		public float EngineHealth
		{
			get => Function.Call<float>(Hash.GET_VEHICLE_ENGINE_HEALTH, Handle);
			set => Function.Call(Hash.SET_VEHICLE_ENGINE_HEALTH, Handle, value);
		}

		/// <summary>
		/// Gets or sets this <see cref="Vehicle"/> petrol tank health.
		/// </summary>
		public float PetrolTankHealth
		{
			get => Function.Call<float>(Hash.GET_VEHICLE_PETROL_TANK_HEALTH, Handle);
			set => Function.Call(Hash.SET_VEHICLE_PETROL_TANK_HEALTH, Handle, value);
		}

		/// <summary>
		/// Gets or sets the engine health for this heli.
		/// </summary>
		public float HeliEngineHealth
		{
			get
			{
				if (!Model.IsHelicopter)
				{
					return 0.0f;
				}

				return Function.Call<float>(Hash._GET_HELI_ENGINE_HEALTH, Handle);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || !Model.IsHelicopter)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x1754 : 0x1744; // untested
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x1774 : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x1824 : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x1854 : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x18F4 : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x19F8 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x1AB8 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x1AF8 : offset;

				SHVDN.NativeMemory.WriteFloat(address + offset, value);
			}
		}

		/// <summary>
		/// Gets or sets the main rotor health for this heli.
		/// </summary>
		public float HeliMainRotorHealth
		{
			get
			{
				if (!Model.IsHelicopter)
				{
					return 0.0f;
				}

				return Function.Call<float>(Hash.GET_HELI_MAIN_ROTOR_HEALTH, Handle);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || !Model.IsHelicopter)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x174C : 0x173C; // untested
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x176C : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x181C : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x184C : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x18EC : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x19F0 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x1AB0 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x1AF0 : offset;

				SHVDN.NativeMemory.WriteFloat(address + offset, value);
			}
		}

		/// <summary>
		/// Gets or sets the tail rotor health for this heli.
		/// </summary>
		public float HeliTailRotorHealth
		{
			get
			{
				if (!Model.IsHelicopter)
				{
					return 0.0f;
				}

				return Function.Call<float>(Hash.GET_HELI_TAIL_ROTOR_HEALTH, Handle);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || !Model.IsHelicopter)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x1750 : 0x1740; // untested
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x1770 : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x1820 : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x1850 : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x18F0 : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x19F4 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x1AB4 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x1AF4 : offset;

				SHVDN.NativeMemory.WriteFloat(address + offset, value);
			}
		}

		#endregion

		#region Radio

		/// <summary>
		/// Turns this <see cref="Vehicle"/>s radio on or off
		/// </summary>
		public bool IsRadioEnabled
		{
			set => Function.Call(Hash.SET_VEHICLE_RADIO_ENABLED, Handle, value);
		}

		/// <summary>
		/// Sets this <see cref="Vehicle"/>s radio station.
		/// </summary>
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

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/>s engine is running.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Vehicle"/>s engine is running; otherwise, <c>false</c>.
		/// </value>
		public bool IsEngineRunning
		{
			get => Function.Call<bool>(Hash.GET_IS_VEHICLE_ENGINE_RUNNING, Handle);
			set => Function.Call(Hash.SET_VEHICLE_ENGINE_ON, Handle, value, true);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/>s engine is currently starting.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Vehicle"/>s engine is starting; otherwise, <c>false</c>.
		/// </value>
		public bool IsEngineStarting
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.IsEngineStartingOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.IsEngineStartingOffset, 5);
			}
		}

		public bool CanEngineDegrade
		{
			set => Function.Call(Hash.SET_VEHICLE_ENGINE_CAN_DEGRADE, Handle, value);
		}

		/// <summary>
		/// Gets the engine temperature of this <see cref="Vehicle"/>.
		/// </summary>
		public float EngineTemperature
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.EngineTemperatureOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.EngineTemperatureOffset);
			}
		}

		public float EnginePowerMultiplier
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x9D0 : 0x9C0; // untested
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x9F0 : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0xA18 : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0xA28 : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0xA48 : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0xA70 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0xAC0 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0xAE8 : offset;

				return SHVDN.NativeMemory.ReadFloat(address + offset);
			}
			set => Function.Call(Hash.MODIFY_VEHICLE_TOP_SPEED, Handle, value);
		}

		public float EngineTorqueMultiplier
		{
			set => Function.Call(Hash._SET_VEHICLE_ENGINE_TORQUE_MULTIPLIER, Handle, value);
		}

		/// <summary>
		/// Gets or sets this <see cref="Vehicle"/> oil level.
		/// If this value is above zero, this value decreases instead of <see cref="EngineHealth"/> when the engine emits black smoke.
		/// </summary>
		public float OilLevel
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x76C : 0x75C;
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x78C : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x7AC : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x7BC : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x7D8 : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x7E8 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x838 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x858 : offset;

				return SHVDN.NativeMemory.ReadFloat(address + offset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x76C : 0x75C;
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x78C : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x7AC : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x7BC : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x7D8 : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x7E8 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x838 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x858 : offset;

				SHVDN.NativeMemory.WriteFloat(address + offset, value);
			}
		}

		/// <summary>
		/// Gets the oil volume of this <see cref="Vehicle"/>.
		/// </summary>
		public float OilVolume
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return HandlingData.OilVolume;
			}
		}

		/// <summary>
		/// Gets or sets this <see cref="Vehicle"/> fuel level.
		/// </summary>
		public float FuelLevel
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.FuelLevelOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.FuelLevelOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.FuelLevelOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.FuelLevelOffset, value);
			}
		}

		/// <summary>
		/// Gets the petrol tank volume of this <see cref="Vehicle"/>.
		/// </summary>
		public float PetrolTankVolume
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return HandlingData.PetrolTankVolume;
			}
		}

		#endregion

		#region Performance & Driving

		/// <summary>
		/// Gets or sets the gears value of this <see cref="Vehicle"/>.
		/// </summary>
		public int Gears
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x7A0 : 0x790;
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x7C0 : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x7E0 : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x7F0 : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x810 : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x830 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x870 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x890 : offset;

				return SHVDN.NativeMemory.ReadInt32(address + offset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x7A0 : 0x790;
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x7C0 : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x7E0 : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x7F0 : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x810 : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x830 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x870 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x890 : offset;

				SHVDN.NativeMemory.WriteInt32(address + offset, value);
			}
		}

		public int HighGear
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.HighGearOffset == 0)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadByte(address + SHVDN.NativeMemory.HighGearOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.HighGearOffset == 0)
				{
					return;
				}

				if (Game.Version >= GameVersion.v1_0_1604_0_Steam)
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

		/// <summary>
		/// Gets or sets the next gear value of this <see cref="Vehicle"/>.
		/// </summary>
		public int NextGear
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.NextGearOffset == 0)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadByte(address + SHVDN.NativeMemory.NextGearOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.NextGearOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteByte(address + SHVDN.NativeMemory.NextGearOffset, (byte)value);
			}
		}

		/// <summary>
		/// Gets or sets the current gear this <see cref="Vehicle"/> is using.
		/// </summary>
		public int CurrentGear
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.GearOffset == 0)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadByte(address + SHVDN.NativeMemory.GearOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.GearOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteByte(address + SHVDN.NativeMemory.GearOffset, (byte)value);
			}
		}

		/// <summary>
		/// Gets or sets the current turbo value of this <see cref="Vehicle"/>.
		/// </summary>
		public float Turbo
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.TurboOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.TurboOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.TurboOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.TurboOffset, value);
			}
		}

		/// <summary>
		/// Gets or sets the current clutch of this <see cref="Vehicle"/>.
		/// </summary>
		public float Clutch
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.ClutchOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.ClutchOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.ClutchOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.ClutchOffset, value);
			}
		}

		/// <summary>
		/// Gets or sets the current throttle of this <see cref="Vehicle"/>.
		/// </summary>
		public float Throttle
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.AccelerationOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.AccelerationOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.AccelerationOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.AccelerationOffset, value);
			}
		}

		/// <summary>
		/// Gets or sets the current brake power of this <see cref="Vehicle"/>.
		/// </summary>
		public float BrakePower
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.BrakePowerOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.BrakePowerOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.BrakePowerOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.BrakePowerOffset, value);
			}
		}

		/// <summary>
		/// Gets or sets the current throttle power of this <see cref="Vehicle"/>.
		/// </summary>
		public float ThrottlePower
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.ThrottlePowerOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.ThrottlePowerOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.ThrottlePowerOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.ThrottlePowerOffset, value);
			}
		}

		/// <summary>
		/// Gets the maximum brake power of this <see cref="Vehicle"/>.
		/// </summary>
		public float MaxBraking => Function.Call<float>(Hash.GET_VEHICLE_MAX_BRAKING, Handle);

		/// <summary>
		/// Gets the maximum traction of this <see cref="Vehicle"/>.
		/// </summary>
		public float MaxTraction => Function.Call<float>(Hash.GET_VEHICLE_MAX_TRACTION, Handle);

		/// <summary>
		/// Gets the speed the drive wheels are turning at, This is the value used for the dashboard speedometers(after being converted to mph).
		/// </summary>
		public float WheelSpeed
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.WheelSpeedOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.WheelSpeedOffset);
			}
		}

		/// <summary>
		/// Sets this <see cref="Vehicle"/>s forward speed.
		/// </summary>
		/// <value>
		/// The forward speed in m/s.
		/// </value>
		public float ForwardSpeed
		{
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

		/// <summary>
		/// Gets or sets the blades speed for this heli.
		/// </summary>
		public float HeliBladesSpeed
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || !Model.IsHelicopter)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x1740 : 0x1730; // untested
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x1760 : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x1810 : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x1840 : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x18E0 : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x19E4 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x1AA8 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x1AE4 : offset;

				return SHVDN.NativeMemory.ReadFloat(address + offset);
			}
			set
			{
				if (!Model.IsHelicopter)
				{
					return;
				}

				Function.Call(Hash.SET_HELI_BLADES_SPEED, Handle, value);
			}
		}

		/// <summary>
		/// Gets or sets the current RPM of this <see cref="Vehicle"/>.
		/// </summary>
		/// <value>
		/// The current RPM between <c>0.0f</c> and <c>1.0f</c>.
		/// </value>
		public float CurrentRPM
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.CurrentRPMOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.CurrentRPMOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.CurrentRPMOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.CurrentRPMOffset, value);
			}
		}

		/// <summary>
		/// Gets the acceleration of this <see cref="Vehicle"/>.
		/// </summary>
		public float Acceleration
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.AccelerationOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.AccelerationOffset);
			}
		}

		/// <summary>
		/// Gets or sets the steering angle of this <see cref="Vehicle"/>.
		/// </summary>
		/// <value>
		/// The steering angle in degrees.
		/// </value>
		public float SteeringAngle
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.SteeringAngleOffset == 0)
				{
					return 0.0f;
				}

				return (float)(SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.SteeringAngleOffset) * (180.0 / System.Math.PI));
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.SteeringAngleOffset == 0)
				{
					return;
				}

				// Need to add 10 degrees to the value for it to apply correctly for some reason
				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.SteeringAngleOffset, (float)((value + (value > 0 ? 10 : -10)) * (System.Math.PI / 180.0)));
			}
		}

		/// <summary>
		/// Gets or sets the steering scale of this <see cref="Vehicle"/>.
		/// </summary>
		public float SteeringScale
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.SteeringScaleOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.SteeringScaleOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.SteeringScaleOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.SteeringScaleOffset, value);
			}
		}

		#endregion

		#region Alarm

		/// <summary>
		/// Sets a value indicating whether this <see cref="Vehicle"/> has an alarm set.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> has an alarm set; otherwise, <c>false</c>.
		/// </value>
		public bool IsAlarmSet
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.AlarmTimeOffset == 0)
				{
					return false;
				}

				return (ushort)SHVDN.NativeMemory.ReadInt16(address + SHVDN.NativeMemory.AlarmTimeOffset) == ushort.MaxValue; //The alarm is set when the value is 0xFFFF
			}
			set => Function.Call(Hash.SET_VEHICLE_ALARM, Handle, value);
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Vehicle"/> is sounding its alarm.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> is sounding its alarm; otherwise, <c>false</c>.
		/// </value>
		public bool IsAlarmSounding => Function.Call<bool>(Hash.IS_VEHICLE_ALARM_ACTIVATED, Handle);

		/// <summary>
		/// Gets or sets time left before this <see cref="Vehicle"/> alarm stops.
		/// If greater than zero, the vehicle alarm will be sounding.
		/// the value is up to 65534.
		/// </summary>
		/// <value>
		/// The time left before this <see cref="Vehicle"/> alarm stops.
		/// </value>
		public int AlarmTimeLeft
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.AlarmTimeOffset == 0)
				{
					return 0;
				}

				ushort alarmTime = (ushort)SHVDN.NativeMemory.ReadInt16(address + SHVDN.NativeMemory.AlarmTimeOffset);
				return alarmTime != ushort.MaxValue ? alarmTime : 0;
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || (ushort)value == ushort.MaxValue || SHVDN.NativeMemory.AlarmTimeOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteInt16(address + SHVDN.NativeMemory.AlarmTimeOffset, (short)value);
			}
		}

		/// <summary>
		/// Starts sounding the alarm on this <see cref="Vehicle"/>.
		/// </summary>
		public void StartAlarm()
		{
			Function.Call(Hash.START_VEHICLE_ALARM, Handle);
		}

		#endregion

		#region Siren & Horn

		/// <summary>
		/// Gets a value indicating whether this <see cref="Vehicle"/> has a siren.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> has a siren; otherwise, <c>false</c>.
		/// </value>
		public bool HasSiren => Bones.Contains("siren1");

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its siren turned on.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> has its siren turned on; otherwise, <c>false</c>.
		/// </value>
		public bool IsSirenActive
		{
			get => Function.Call<bool>(Hash.IS_VEHICLE_SIREN_ON, Handle);
			set => Function.Call(Hash.SET_VEHICLE_SIREN, Handle, value);
		}
		/// <summary>
		/// Sets a value indicating whether the siren on this <see cref="Vehicle"/> plays sounds.
		/// </summary>
		/// <value>
		/// <c>true</c> if the siren on this <see cref="Vehicle"/> plays sounds; otherwise, <c>false</c>.
		/// </value>
		public bool IsSirenSilent
		{
			set => Function.Call(Hash._SET_DISABLE_VEHICLE_SIREN_SOUND, Handle, value);
		}

		/// <summary>
		/// Sounds the horn on this <see cref="Vehicle"/>.
		/// </summary>
		/// <param name="duration">The duration in milliseconds to sound the horn for.</param>
		public void SoundHorn(int duration)
		{
			Function.Call(Hash.START_VEHICLE_HORN, Handle, duration, Game.GenerateHash("HELDDOWN"), 0);
		}

		#endregion

		#region Lights

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its lights on.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> has its lights on; otherwise, <c>false</c>.
		/// </value>
		public bool AreLightsOn
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

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its high beams on.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> has its high beams on; otherwise, <c>false</c>.
		/// </value>
		public bool AreHighBeamsOn
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

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its interior lights on.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> has its interior lights on; otherwise, <c>false</c>.
		/// </value>
		public bool IsInteriorLightOn
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.IsInteriorLightOnOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.IsInteriorLightOnOffset, 6);
			}
			set => Function.Call(Hash.SET_VEHICLE_INTERIORLIGHT, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its search light on.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> has its search light on; otherwise, <c>false</c>.
		/// </value>
		public bool IsSearchLightOn
		{
			get => Function.Call<bool>(Hash.IS_VEHICLE_SEARCHLIGHT_ON, Handle);
			set => Function.Call(Hash.SET_VEHICLE_SEARCHLIGHT, Handle, value, 0);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its taxi light on.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> has its taxi light on; otherwise, <c>false</c>.
		/// </value>
		public bool IsTaxiLightOn
		{
			get => Function.Call<bool>(Hash.IS_TAXI_LIGHT_ON, Handle);
			set => Function.Call(Hash.SET_TAXI_LIGHTS, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its left indicator light on.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> has its left indicator light on; otherwise, <c>false</c>.
		/// </value>
		public bool IsLeftIndicatorLightOn
		{
			set => Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, Handle, true, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its right indicator light on.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> has its right indicator light on; otherwise, <c>false</c>.
		/// </value>
		public bool IsRightIndicatorLightOn
		{
			set => Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, Handle, false, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its brake light on.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> has its brake light on; otherwise, <c>false</c>.
		/// </value>
		public bool AreBrakeLightsOn
		{
			set => Function.Call(Hash.SET_VEHICLE_BRAKE_LIGHTS, Handle, value);
		}

		public float LightsMultiplier
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x90C : 0x8FC; // untested
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x92C : offset;
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x954 : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x964 : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x984 : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x9A4 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x9F4 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0xA14 : offset;

				return SHVDN.NativeMemory.ReadFloat(address + offset);
			}
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

		public bool IsLeftHeadLightBroken
		{
			get => Function.Call<bool>(Hash.GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED, Handle);
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x77C : 0x76C;
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x79C : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x7BC : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x7CC : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x7EC : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x80C : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x84C : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x86C : offset;

				if (value)
				{
					SHVDN.NativeMemory.SetBit(address + offset, 0);
				}
				else
				{
					SHVDN.NativeMemory.ClearBit(address + offset, 0);
				}
			}
		}

		public bool IsRightHeadLightBroken
		{
			get => Function.Call<bool>(Hash.GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED, Handle);
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x77C : 0x76C;
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x79C : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x7BC : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x7CC : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x7EC : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x80C : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x84C : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x86C : offset;

				if (value)
				{
					SHVDN.NativeMemory.SetBit(address + offset, 1);
				}
				else
				{
					SHVDN.NativeMemory.ClearBit(address + offset, 1);
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
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x83B : 0x82B; // untested
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x85B : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x883 : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x893 : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x8B3 : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x8D3 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x923 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x943 : offset;

				return !SHVDN.NativeMemory.IsBitSet(address + offset, 6);
			}
			set => Function.Call(Hash.SET_VEHICLE_WHEELS_CAN_BREAK, Handle, value);
		}

		public bool CanBeVisiblyDamaged
		{
			set => Function.Call(Hash.SET_VEHICLE_CAN_BE_VISIBLY_DAMAGED, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Vehicle"/> drops money when destroyed.
		/// Only works when the vehicle model is a car, quad bikes or trikes (strictly when the internal vehicle class is CAutomobile or derived class from CAutomobile).
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Vehicle"/> drops money when destroyed; otherwise, <c>false</c>.
		/// </value>
		public bool DropsMoneyOnExplosion
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				var model = Model;

				// check if the vehicle is CAutomobile or a derived class from it
				if (model.IsCar || model.IsQuadBike ||
					(Game.Version >= GameVersion.v1_0_944_2_Steam && model.IsAmphibiousCar) ||
					(Game.Version >= GameVersion.v1_0_944_2_Steam && model.IsAmphibiousQuadBike))
				{
					int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x1319 : 0x12F9;
					offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x1349 : offset;
					offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x13B9 : offset;
					offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x1409 : offset;
					offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x1459 : offset;
					offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x1539 : offset;

					return SHVDN.NativeMemory.IsBitSet(address + offset, 1);
				}

				return false;
			}
			set => Function.Call(Hash.SET_VEHICLE_DROPS_MONEY_WHEN_BLOWN_UP, Handle, value);
		}

		public void ApplyDamage(Vector3 position, float damageAmount, float radius)
		{
			Function.Call(Hash.SET_VEHICLE_DAMAGE, position.X, position.Y, position.Z, damageAmount, radius);
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
			set => Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED, Handle, value);
		}

		public VehicleLandingGearState LandingGearState
		{
			get => Function.Call<VehicleLandingGearState>(Hash.GET_LANDING_GEAR_STATE, Handle);
			set
			{
				int state = 0;
				switch (value)
				{
					case VehicleLandingGearState.Deploying:
						state = 0;
						break;
					case VehicleLandingGearState.Retracting:
						state = 1;
						break;
					case VehicleLandingGearState.Deployed:
						state = 2;
						break;
					case VehicleLandingGearState.Retracted:
						state = 3;
						break;
					case VehicleLandingGearState.Broken:
						state = 4;
						break;
					default:
						return;
				}

				Function.Call(Hash.CONTROL_LANDING_GEAR, Handle, state);
			}
		}

		public VehicleDoorCollection Doors => _doors ?? (_doors = new VehicleDoorCollection(this));

		#endregion

		#region Burnout

		public bool IsInBurnout => Function.Call<bool>(Hash.IS_VEHICLE_IN_BURNOUT, Handle);

		public bool IsBurnoutForced
		{
			set => Function.Call<bool>(Hash.SET_VEHICLE_BURNOUT, Handle, value);
		}

		/// <summary>
		/// Sets a value indicating whether the Handbrake on this <see cref="Vehicle"/> is forced on.
		/// </summary>
		/// <value>
		///   <c>true</c> if the Handbrake on this <see cref="Vehicle"/> is forced on; otherwise, <c>false</c>.
		/// </value>
		public bool IsHandbrakeForcedOn
		{
			set => Function.Call(Hash.SET_VEHICLE_HANDBRAKE, Handle, value);
		}

		#endregion

		#region Occupants

		public Ped Driver => GetPedOnSeat(VehicleSeat.Driver);

		public Ped GetPedOnSeat(VehicleSeat seat)
		{
			var ped = new Ped(Function.Call<int>(Hash.GET_PED_IN_VEHICLE_SEAT, Handle, seat));
			return ped.Exists() ? ped : null;
		}

		public Ped[] Occupants
		{
			get
			{
				Ped driver = Driver;

				if (driver == null)
				{
					return Passengers;
				}

				var result = new Ped[PassengerCount + 1];
				result[0] = driver;

				for (int i = 0, j = 0, seats = PassengerCapacity; i < seats && j < result.Length; i++)
				{
					if (!IsSeatFree((VehicleSeat)i))
					{
						result[j++ + 1] = GetPedOnSeat((VehicleSeat)i);
					}
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

				for (int i = 0, j = 0, seats = PassengerCapacity; i < seats && j < result.Length; i++)
				{
					if (!IsSeatFree((VehicleSeat)i))
					{
						result[j++] = GetPedOnSeat((VehicleSeat)i);
					}
				}

				return result;
			}
		}

		public int PassengerCount => Function.Call<int>(Hash.GET_VEHICLE_NUMBER_OF_PASSENGERS, Handle);

		public int PassengerCapacity => Function.Call<int>(Hash.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, Handle);

		public Ped CreatePedOnSeat(VehicleSeat seat, Model model)
		{
			if (!IsSeatFree(seat))
			{
				throw new ArgumentException("The VehicleSeat selected was not free", nameof(seat));
			}

			if (!model.IsPed || !model.Request(1000))
			{
				return null;
			}

			return new Ped(Function.Call<int>(Hash.CREATE_PED_INSIDE_VEHICLE, Handle, 26, model.Hash, seat, 1, 1));
		}

		public Ped CreateRandomPedOnSeat(VehicleSeat seat)
		{
			if (!IsSeatFree(seat))
			{
				throw new ArgumentException("The VehicleSeat selected was not free", nameof(seat));
			}

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

		public bool IsSeatFree(VehicleSeat seat)
		{
			return Function.Call<bool>(Hash.IS_VEHICLE_SEAT_FREE, Handle, seat);
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
			Vector3 currentPosition = Position;
			NativeVector3 newPosition;
			float heading;
			long unkn;

			for (int i = 1; i < 40; i++)
			{
				unsafe
				{
					Function.Call(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING, currentPosition.X, currentPosition.Y, currentPosition.Z, i, &newPosition, &heading, &unkn, 1, 0x40400000, 0);
				}

				if (!Function.Call<bool>(Hash.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY, newPosition.X, newPosition.Y, newPosition.Z, 5.0f, 5.0f, 5.0f, 0))
				{
					Position = newPosition;
					PlaceOnGround();
					Heading = heading;
					break;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether peds can use this <see cref="Vehicle"/> for cover.
		/// </summary>
		/// <value>
		///   <c>true</c> if peds can use this <see cref="Vehicle"/> for cover; otherwise, <c>false</c>.
		/// </value>
		public bool ProvidesCover
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				// Unsure of the exact version this switched, but all others in the rangs are the same
				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x83C : 0x82C;
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0x85C : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0x884 : offset;
				offset = Game.Version >= GameVersion.v1_0_1103_2_Steam ? 0x894 : offset;
				offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x8B4 : offset;
				offset = Game.Version >= GameVersion.v1_0_1290_1_Steam ? 0x8D4 : offset;
				offset = Game.Version >= GameVersion.v1_0_1604_0_Steam ? 0x924 : offset;
				offset = Game.Version >= GameVersion.v1_0_2060_0_Steam ? 0x944 : offset;

				return SHVDN.NativeMemory.IsBitSet(address + offset, 2);
			}
			set => Function.Call(Hash.SET_VEHICLE_PROVIDES_COVER, Handle, value);
		}

		#endregion

		#region Towing

		/// <summary>
		/// Gets a value indicating whether this <see cref="Vehicle"/> has forks.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Vehicle"/> has forks; otherwise, <c>false</c>.
		/// </value>
		public bool HasForks => Bones.Contains("forks");

		public bool HasTowArm => Bones.Contains("tow_arm");

		public float TowArmPosition
		{
			set => Function.Call(Hash.SET_VEHICLE_TOW_TRUCK_ARM_POSITION, Handle, value);
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

			if (vehicle != null)
			{
				Function.Call(Hash.DETACH_VEHICLE_FROM_TOW_TRUCK, Handle, vehicle.Handle);
			}
		}

		public Vehicle TowedVehicle
		{
			get
			{
				var veh = new Vehicle(Function.Call<int>(Hash.GET_ENTITY_ATTACHED_TO_TOW_TRUCK, Handle));
				return veh.Exists() ? veh : null;
			}
		}

		#endregion

		#region Carbobob

		public bool HasBombBay => Bones.Contains("door_hatch_l") && Bones.Contains("door_hatch_r");

		public void OpenBombBay()
		{
			if (HasBombBay)
			{
				Function.Call(Hash.OPEN_BOMB_BAY_DOORS, Handle);
			}
		}

		public void CloseBombBay()
		{
			if (HasBombBay)
			{
				Function.Call(Hash.CLOSE_BOMB_BAY_DOORS, Handle);
			}
		}

		public void SetHeliYawPitchRollMult(float mult)
		{
			if (Model.IsHelicopter && mult >= 0.0f && mult <= 1.0f)
			{
				Function.Call(Hash._SET_HELICOPTER_ROLL_PITCH_YAW_MULT, Handle, mult);
			}
		}

		public void DropCargobobHook(CargobobHook hook)
		{
			if (Model.IsCargobob)
			{
				Function.Call(Hash.CREATE_PICK_UP_ROPE_FOR_CARGOBOB, Handle, hook);
			}
		}

		public void RetractCargobobHook()
		{
			if (Model.IsCargobob)
			{
				Function.Call(Hash.REMOVE_PICK_UP_ROPE_FOR_CARGOBOB, Handle);
			}
		}

		public bool IsCargobobHookActive()
		{
			if (Model.IsCargobob)
			{
				return Function.Call<bool>(Hash.DOES_CARGOBOB_HAVE_PICK_UP_ROPE, Handle) || Function.Call<bool>(Hash.DOES_CARGOBOB_HAVE_PICKUP_MAGNET, Handle);
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
						return Function.Call<bool>(Hash.DOES_CARGOBOB_HAVE_PICK_UP_ROPE, Handle);
					case CargobobHook.Magnet:
						return Function.Call<bool>(Hash.DOES_CARGOBOB_HAVE_PICKUP_MAGNET, Handle);
				}
			}

			return false;
		}

		public void CargoBobMagnetGrabVehicle()
		{
			if (IsCargobobHookActive(CargobobHook.Magnet))
			{
				Function.Call(Hash.SET_CARGOBOB_PICKUP_MAGNET_ACTIVE, Handle, true);
			}
		}

		public void CargoBobMagnetReleaseVehicle()
		{
			if (IsCargobobHookActive(CargobobHook.Magnet))
			{
				Function.Call(Hash.SET_CARGOBOB_PICKUP_MAGNET_ACTIVE, Handle, false);
			}
		}

		#endregion

		public static string GetModelDisplayName(Model vehicleModel)
		{
			return Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, vehicleModel.Hash);
		}
		public static string GetClassDisplayName(VehicleClass vehicleClass)
		{
			return "VEH_CLASS_" + ((int)vehicleClass).ToString();
		}

		public static VehicleClass GetModelClass(Model vehicleModel)
		{
			return Function.Call<VehicleClass>(Hash.GET_VEHICLE_CLASS_FROM_NAME, vehicleModel.Hash);
		}

		public static int[] GetAllModelValues()
		{
			var allModels = new List<int>();
			for (int i = 0; i < 0x20; i++)
			{
				allModels.AddRange(SHVDN.NativeMemory.VehicleModels[i].ToArray());
			}
			return allModels.ToArray();
		}

		public static VehicleHash[] GetAllModels()
		{
			var allModels = new List<VehicleHash>();
			for (int i = 0; i < 0x20; i++)
			{
				allModels.AddRange(Array.ConvertAll(SHVDN.NativeMemory.VehicleModels[i].ToArray(), item => (VehicleHash)item));
			}
			return allModels.ToArray();
		}
		public static VehicleHash[] GetAllModelsOfClass(VehicleClass vehicleClass)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.VehicleModels[(int)vehicleClass].ToArray(), item => (VehicleHash)item);
		}
	}
}
