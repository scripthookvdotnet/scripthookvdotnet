//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GTA
{
    public sealed class Vehicle : Entity
    {
        #region Fields
        VehicleDoorCollection _doors;
        VehicleModCollection _mods;
        VehicleExtraCollection _extras;
        VehicleWheelCollection _wheels;
        VehicleWindowCollection _windows;
        #endregion

        internal Vehicle(int handle) : base(handle)
        {
        }

        /// <summary>
        /// Restores the health of this <see cref="Vehicle"/> and fixes any damage instantaneously.
        /// </summary>
        public void Repair()
        {
            Function.Call(Hash.SET_VEHICLE_FIXED, Handle);
            IsConsideredDestroyed = false;
        }

        /// <summary>
        /// Explodes this <see cref="Vehicle"/>.
        /// </summary>
        public void Explode()
        {
            Function.Call(Hash.EXPLODE_VEHICLE, Handle, true, false);
        }

        /// <summary>
        /// Determines if this <see cref="Vehicle"/> exists.
        /// You should ensure <see cref="Vehicle"/>s still exist before manipulating them or getting some values for them on every tick, since some native functions may crash the game if invalid entity handles are passed.
        /// </summary>
        /// <returns><see langword="true" /> if this <see cref="Vehicle"/> exists; otherwise, <see langword="false" /></returns>
        public new bool Exists()
        {
            return EntityType == EntityType.Vehicle;
        }

        #region Styling

        public bool IsConvertible => Function.Call<bool>(Hash.IS_VEHICLE_A_CONVERTIBLE, Handle, 0);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> is considered a “Big” vehicle.
        /// </summary>
        /// <remarks>
        /// This returns <see langword="true"/> when either of the following conditions is met:
        /// <list type="bullet">
        /// <item>
        /// The <c>&lt;strModelFlags&gt;</c> attribute in the vehicle's <c>handling.xml</c>
        /// contains the <c>IS_BIG</c> flag (<c>0x8</c>) or the <c>IS_BUS</c> flag (<c>0x2</c>).
        /// </item>
        /// <item>The <see cref="Type"/> of this vehicle is <see cref="VehicleType.Plane"/>.</item>
        /// </list>
        /// </remarks>
        public bool IsBig => Function.Call<bool>(Hash.IS_BIG_VEHICLE, Handle);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> has bulletproof glass.
        /// </summary>
        public bool HasBulletProofGlass => SHVDN.NativeMemory.HasVehicleFlag(Model.Hash, SHVDN.VehicleModelInfoFlags.HasBulletProofGlass);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> has Lowrider Hydraulics.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="false"/> prior to <c>v1.0.505.2</c>;
        /// </remarks>
        public bool HasLowriderHydraulics
            => Game.FileVersion >= ExeVersionConsts.v1_0_505_2
            && SHVDN.NativeMemory.HasVehicleFlag(Model.Hash, SHVDN.VehicleModelInfoFlags.HasLowriderHydraulics);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> has Donk Hydraulics.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="false"/> prior to <c>v1.0.505.2</c>;
        /// </remarks>
        public bool HasDonkHydraulics
            => Game.FileVersion >= ExeVersionConsts.v1_0_505_2
            && SHVDN.NativeMemory.HasVehicleFlag(Model.Hash, SHVDN.VehicleModelInfoFlags.HasLowriderDonkHydraulics);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> has a Parachute.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="false"/> prior to <c>v1.0.505.2</c>;
        /// </remarks>
        public bool HasParachute
            => Game.FileVersion >= ExeVersionConsts.v1_0_505_2
            && Function.Call<bool>(Hash.GET_VEHICLE_HAS_PARACHUTE, Handle);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> has a Rocket Boost
        /// </summary>
        /// <remarks>
        /// Returns <see langword="false"/> prior to <c>v1.0.944.2</c>;
        /// </remarks>
        public bool HasRocketBoost
            => Game.FileVersion >= ExeVersionConsts.v1_0_944_2
            && Function.Call<bool>(Hash.GET_HAS_ROCKET_BOOST, Handle);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> has the Parachute deployed.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="false"/> prior to <c>v1.0.1011.1</c>;
        /// </remarks>
        public bool IsParachuteDeployed
            => Game.FileVersion >= ExeVersionConsts.v1_0_1011_1
            && Function.Call<bool>(Hash.IS_VEHICLE_PARACHUTE_DEPLOYED, Handle);

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its rocket boost currently active.
        /// </summary>
        /// <remarks>
        /// Reading always <see langword="false"/> prior to <c>v1.0.1011.1</c>;
        /// Writing to this prior to <c>v1.0.1011.1</c> will throw an error.
        /// </remarks>
        /// <exception cref="GameVersionNotSupportedException">
        /// Thrown if trying to update this value in game versions earlier than <c>v1.0.1011.1</c>;
        /// </exception>
        public bool IsRocketBoostActive
        {
            get => Game.FileVersion >= ExeVersionConsts.v1_0_944_2
                && Function.Call<bool>(Hash.IS_ROCKET_BOOST_ACTIVE, Handle);
            set
            {
                if (Game.FileVersion < ExeVersionConsts.v1_0_944_2)
                {
                    GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_944_2), nameof(Vehicle), nameof(IsRocketBoostActive));
                }

                Function.Call(Hash.SET_ROCKET_BOOST_ACTIVE, Handle, value);
            }
        }

        /// <summary>
        /// Gets or sets the dirt level of this <see cref="Vehicle"/> between <see langword="0.0f"/> (clean) to <see langword="15.0f"/> (dirty).
        /// </summary>
        public float DirtLevel
        {
            get => Function.Call<float>(Hash.GET_VEHICLE_DIRT_LEVEL, Handle);
            set => Function.Call(Hash.SET_VEHICLE_DIRT_LEVEL, Handle, value);
        }

        /// <summary>
        /// Gets or sets the opacity of the EnvEff texture on this <see cref="Vehicle"/> between <see langword="0.0f"/> (transparent) and <see langword="1.0f"/> (opaque).
        /// </summary>
        public float EnvEffLevel
        {
            get => Function.Call<float>(Hash.GET_VEHICLE_ENVEFF_SCALE, Handle);
            set => Function.Call(Hash.SET_VEHICLE_ENVEFF_SCALE, Handle, value);
        }

        /// <summary>
        /// Gets the <see cref="VehicleModCollection"/> for this <see cref="Vehicle"/>.
        /// </summary>
        public VehicleModCollection Mods => _mods ??= new VehicleModCollection(this);

        /// <summary>
        /// Gets the <see cref="VehicleWheelCollection"/> for this <see cref="Vehicle"/>.
        /// </summary>
        public VehicleWheelCollection Wheels => _wheels ??= new VehicleWheelCollection(this);

        /// <summary>
        /// Gets the <see cref="VehicleWindowCollection"/> for this <see cref="Vehicle"/>.
        /// </summary>
        public VehicleWindowCollection Windows => _windows ??= new VehicleWindowCollection(this);

        /// <summary>
        /// Gets the <see cref="VehicleExtraCollection"/> for this <see cref="Vehicle"/>.
        /// </summary>
        public VehicleExtraCollection Extras => _extras ??= new VehicleExtraCollection(this);

        /// <summary>
        /// Washes this <see cref="Vehicle"/> by setting the value of <see cref="DirtLevel"/> to <see langword="0.0f"/>.
        /// </summary>
        public void Wash()
        {
            DirtLevel = 0f;
        }

        /// <summary>
        /// Enables or disables the control of hydraulics for this <see cref="Vehicle"/>.
        /// </summary>
        /// <remarks>
        /// When set to <see langword="true"/>, hydraulics control is enabled, and any lowered hydraulics are raised.
        /// When set to <see langword="false"/>, hydraulics control is disabled, and hydraulics are lowered.
        /// <para>
        /// Prior to <c>v1.0.505.2</c> will throw an error.
        /// </para>
        /// </remarks>
        /// <exception cref="GameVersionNotSupportedException">
        /// Thrown if called in game versions earlier than <c>v1.0.505.2</c>.
        /// </exception>
        public void SetHydraulicsControl(bool toggle)
        {
            GameVersionNotSupportedException.ThrowIfNotSupported(ExeVersionConsts.v1_0_505_2, nameof(Vehicle), nameof(SetHydraulicsControl));

            Function.Call(Hash.SET_HYDRAULICS_CONTROL, Handle, toggle);
        }

        /// <summary>
        /// Overrides this <see cref="Vehicle"/>'s audio game object with another so the vehicle has different various
        /// vehicle sounds such as horn, door, suspension, and start sequences.
        /// </summary>
        /// <param name="gameObjectName">
        /// <para>
        /// The audio game object name. Case-insensitive in ASCII characters since the string will be hashed using
        /// (almost) the same function as <see cref="StringHash.AtStringHash(string, uint)"/> uses.
        /// </para>
        /// <para>
        /// Generally accepts the internal game name of the <see cref="Vehicle"/> to source an audio profile from,
        /// such as "sentinel" or "deluxe". All vanilla vehicles use unique game audio objects as of v1.0.2944.0
        /// because none of them have <c>audioNameHash</c> override in vehicles.meta. For mod vehicles, you might
        /// want to check if <c>audioNameHash</c> is overridden.
        /// </para>
        /// <para>
        /// You can use any names listed as items of any audio type of vehicle, boat, bicycle, aeroplane, helicopter,
        /// or train. You can find one with the term <c>Item type="[some type]"</c> ("[some type]" can be
        /// <c>Vehicle</c>, <c>Boat</c>, <c>Bicycle</c>, <c>Aeroplane</c>, <c>Helicopter</c>, <c>Train</c>)
        /// in <c>game.dat[some number].rel</c> in CodeWalker. "BJXL_ARMENIAN_3" and "STRETCH_MICHAEL_4" are registered
        /// as the vehicle type for 2 ysc scripts for example, and you can use them.
        /// </para>
        /// </param>
        public void ForceUseAudioGameObject(string gameObjectName) => Function.Call(Hash.FORCE_USE_AUDIO_GAME_OBJECT, Handle, gameObjectName);

        #endregion

        #region Extras
        /// <summary>
        /// Determines whether the specified <c>extra</c> is currently enabled on this <see cref="Vehicle"/>.
        /// </summary>
        /// <param name="extra">The extra to check.</param>
        /// <returns>
        /// <see langword="true"/> if the extra is enabled; otherwise, <see langword="false"/>.
        /// </returns>
        [Obsolete("Use Vehicle.Extras[VehicleExtraIndex].Enabled instead!")]
        public bool IsExtraOn(int extra)
        {
            return Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, Handle, extra);
        }

        /// <summary>
        /// Determines whether the specified <c>extra</c> exists on this <see cref="Vehicle"/>.
        /// </summary>
        /// <param name="extra">The extra to check.</param>
        /// <returns>
        /// <see langword="true"/> if the extra exists; otherwise, <see langword="false"/>.
        /// </returns>
        [Obsolete("Use Vehicle.Extras[VehicleExtraIndex].Exists() instead!")]
        public bool ExtraExists(int extra)
        {
            return Function.Call<bool>(Hash.DOES_EXTRA_EXIST, Handle, extra);
        }

        /// <summary>
        /// Enables or disables the specified <c>extra</c> on this <see cref="Vehicle"/>.
        /// </summary>
        /// <param name="extra">The extra to enable or disable.</param>
        /// <param name="toggle"><see langword="true"/> to enable the extra; <see langword="false"/> to disable it.</param>
        [Obsolete("Use Vehicle.Extras[VehicleExtraIndex].Enabled instead!")]
        public void ToggleExtra(int extra, bool toggle)
        {
            Function.Call(Hash.SET_VEHICLE_EXTRA, Handle, extra, !toggle);
        }

        #endregion

        #region Configuration

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is a regular <see cref="VehicleType.Automobile"/>.
        /// </summary>
        public bool IsRegularAutomobile => Type == VehicleType.Automobile;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.AmphibiousAutomobile"/>.
        /// </summary>
        public bool IsAmphibiousAutomobile => Type == VehicleType.AmphibiousAutomobile;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.SubmarineCar"/>.
        /// </summary>
        public bool IsSubmarineCar => Type == VehicleType.SubmarineCar;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> is either a <see cref="VehicleType.Automobile"/>,
        /// a <see cref="VehicleType.AmphibiousAutomobile"/> or a <see cref="VehicleType.SubmarineCar"/>.
        /// </summary>
        public bool IsAutomobile
        {
            get
            {
                VehicleType vehicleType = Type;
                return vehicleType is VehicleType.Automobile or VehicleType.AmphibiousAutomobile or VehicleType.SubmarineCar;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.QuadBike"/>.
        /// </summary>
        public bool IsRegularQuadBike => Type == VehicleType.QuadBike;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.AmphibiousQuadBike"/>.
        /// </summary>
        public bool IsAmphibiousQuadBike => Type == VehicleType.AmphibiousQuadBike;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> is either a <see cref="VehicleType.QuadBike"/> or a <see cref="VehicleType.AmphibiousQuadBike"/>.
        /// </summary>
        public bool IsQuadBike
        {
            get
            {
                VehicleType vehicleType = Type;
                return vehicleType is VehicleType.QuadBike or VehicleType.AmphibiousQuadBike;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> is either a <see cref="VehicleType.AmphibiousAutomobile"/>
        /// or a <see cref="VehicleType.AmphibiousQuadBike"/>.
        /// </summary>
        public bool IsAmphibious
        {
            get
            {
                VehicleType vehicleType = Type;
                return vehicleType is VehicleType.AmphibiousAutomobile or VehicleType.AmphibiousQuadBike;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.Trailer"/>.
        /// </summary>
        public bool IsTrailer => Type == VehicleType.Trailer;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.Plane"/>.
        /// </summary>
        public bool IsPlane => Type == VehicleType.Plane;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.Helicopter"/>.
        /// </summary>
        public bool IsHelicopter => Type == VehicleType.Helicopter;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.Blimp"/>.
        /// </summary>
        public bool IsBlimp => Type == VehicleType.Blimp;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> is either a <see cref="VehicleType.Plane"/>,
        /// a <see cref="VehicleType.Helicopter"/> or a <see cref="VehicleType.Blimp"/>.
        /// </summary>
        public bool IsAircraft
        {
            get
            {
                VehicleType vehicleType = Type;
                return vehicleType is VehicleType.Plane or VehicleType.Helicopter or VehicleType.Blimp;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> is either a <see cref="VehicleType.Helicopter"/>
        /// or a <see cref="VehicleType.Blimp"/>.
        /// </summary>
        private bool IsHeliOrBlimp
        {
            get
            {
                VehicleType vehicleType = Type;
                return ((uint)vehicleType - 8) <= 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> is either a <see cref="VehicleType.Helicopter"/>,
        /// a <see cref="VehicleType.Blimp"/> or a <see cref="VehicleType.Autogyro"/>.
        /// </summary>
        private bool IsRotaryWingAircraft
        {
            get
            {
                VehicleType vehicleType = Type;
                return ((uint)vehicleType - 8) <= 2;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.Motorcycle"/>.
        /// </summary>
        public bool IsMotorcycle => Type == VehicleType.Motorcycle;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.Bicycle"/>.
        /// </summary>
        public bool IsBicycle => Type == VehicleType.Bicycle;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is either a <see cref="VehicleType.Motorcycle"/> or a <see cref="VehicleType.Bicycle"/>.
        /// </summary>
        public bool IsBike
        {
            get
            {
                VehicleType vehicleType = Type;
                return (vehicleType == VehicleType.Motorcycle || vehicleType == VehicleType.Bicycle);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.Boat"/>.
        /// </summary>
        public bool IsBoat => Type == VehicleType.Boat;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.Train"/>.
        /// </summary>
        public bool IsTrain => Type == VehicleType.Train;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Type"/> of this <see cref="Vehicle"/> is <see cref="VehicleType.Submarine"/>.
        /// </summary>
        public bool IsSubmarine => Type == VehicleType.Submarine;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> can pretend it has the same <see cref="Ped"/>s.
        /// Set to <see langword="false"/> to prevent this <see cref="Vehicle"/> from creating new <see cref="Ped"/>s as its occupants.
        /// </summary>
        /// <remarks>
        /// <see cref="Vehicle"/>s do not pretend occupants regardless of this value if <see cref="Entity.PopulationType"/> is set to
        /// <see cref="EntityPopulationType.Permanent"/> or <see cref="EntityPopulationType.Mission"/>.
        /// </remarks>
        public bool CanPretendOccupants
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.DisablePretendOccupantOffset == 0)
                    return false;

                return !SHVDN.MemDataMarshal.IsBitSet(address + SHVDN.NativeMemory.Vehicle.DisablePretendOccupantOffset, 7);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.DisablePretendOccupantOffset == 0)
                    return;

                // SET_DISABLE_PRETEND_OCCUPANTS changes the value only if the population type is set to 6 or 7, so change the value manually
                SHVDN.MemDataMarshal.SetBit(address + SHVDN.NativeMemory.Vehicle.DisablePretendOccupantOffset, 7, !value);
            }
        }

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
        /// <remarks></remarks>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> is wanted by the police; otherwise, <see langword="false" />.
        /// </value>
        public bool IsWanted
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.IsWantedOffset == 0)
                    return false;

                return SHVDN.MemDataMarshal.IsBitSet(address + SHVDN.NativeMemory.Vehicle.IsWantedOffset, 3);
            }
            set => Function.Call(Hash.SET_VEHICLE_IS_WANTED, Handle, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> needs to be hotwired to start.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> needs to be hotwired to start; otherwise, <see langword="false" />.
        /// </value>
        public bool NeedsToBeHotwired
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.NeedsToBeHotwiredOffset == 0)
                    return false;

                return SHVDN.MemDataMarshal.IsBitSet(address + SHVDN.NativeMemory.Vehicle.NeedsToBeHotwiredOffset, 2);
            }
            set => Function.Call(Hash.SET_VEHICLE_NEEDS_TO_BE_HOTWIRED, Handle, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> was previously owned by a <see cref="Player"/>.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="Vehicle"/> was previously owned by a <see cref="Player"/>; otherwise, <see langword="false" />.
        /// </value>
        public bool PreviouslyOwnedByPlayer
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.PreviouslyOwnedByPlayerOffset == 0)
                    return false;

                return SHVDN.MemDataMarshal.IsBitSet(address + SHVDN.NativeMemory.Vehicle.PreviouslyOwnedByPlayerOffset, 1);
            }
            set => Function.Call(Hash.SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER, Handle, value);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> allows <see cref="Ped"/>s to rappel.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="Vehicle"/> allows <see cref="Ped"/>s to rappel; otherwise, <see langword="false" />.
        /// </value>
        public bool AllowRappel => Game.FileVersion >= ExeVersionConsts.v1_0_757_2
            ? Function.Call<bool>(Hash.DOES_VEHICLE_ALLOW_RAPPEL, Handle)
            : SHVDN.NativeMemory.HasVehicleFlag(Model.Hash, SHVDN.VehicleFlags.AllowsRappel);

        /// <summary>
        /// Gets a value indicating whether <see cref="Ped"/>s can stand on this <see cref="Vehicle"/> regardless of <see cref="Vehicle"/>s speed.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if <see cref="Ped"/>s can stand on this <see cref="Vehicle"/> regardless of <see cref="Vehicle"/>s speed; otherwise, <see langword="false" />.
        /// </value>
        public bool CanStandOnTop => SHVDN.NativeMemory.HasVehicleFlag(Model.Hash, SHVDN.VehicleFlags.CanStandOnTop);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> has jumping functionality.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="Vehicle"/> can jump; otherwise, <see langword="false" />.
        /// </value>
        public bool CanJump
            => Game.FileVersion >= ExeVersionConsts.v1_0_944_2
            && Function.Call<bool>(Hash.GET_CAR_HAS_JUMP, Handle);

        /// <summary>
        /// Sets whether the exhaust of this <see cref="Vehicle"/> will make popping noises.
        /// </summary>
        /// <value>
        /// <see langword="true" /> to enable exhaust popping; otherwise, <see langword="false" />.
        /// </value>
        public bool AreExhaustPopsEnabled
        {
            set => Function.Call(Hash.ENABLE_VEHICLE_EXHAUST_POPS, Handle, value);
        }

        /// <summary>
        /// Sets whether this <see cref="Vehicle"/> has reduced grip.
        /// </summary>
        /// <remarks>
        /// This property only affects Automobiles, Helicopters, and Planes.
        /// </remarks>
        /// <value>
        /// <see langword="true"/> to reduce the grip of this <see cref="Vehicle"/>; otherwise, <see langword="false"/>.
        /// </value>
        public bool HasLowerFrictionTires
        {
            set => Function.Call(Hash.SET_VEHICLE_REDUCE_GRIP, Handle, value);
        }

        /// <summary>
        /// Gets the display name of this <see cref="Vehicle"/>.
        /// <remarks>Use <see cref="Game.GetLocalizedString(string)"/> to get the localized name.</remarks>
        /// </summary>
        public string DisplayName => GetModelDisplayName(Model);
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

        /// <summary>
        /// Gets the type of this <see cref="Vehicle"/>.
        /// </summary>
        public VehicleType Type
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.VehicleTypeOffset == 0)
                    return VehicleType.None;

                int vehTypeInt = SHVDN.MemDataMarshal.ReadInt32(address + SHVDN.NativeMemory.Vehicle.VehicleTypeOffset);
                if (vehTypeInt >= 6 && Game.FileVersion < ExeVersionConsts.v1_0_944_2)
                {
                    vehTypeInt += 2;
                }

                return (VehicleType)vehTypeInt;
            }
        }

        /// <summary>
        /// Gets or sets a lod multiplier for this <see cref="Vehicle"/>.
        /// </summary>
        /// <remarks>
        /// When you try to find an appropriate lod multiplier to set, start by using low values (1.1, 1.5, etc.) until the wanted result is achieved.
        /// Large values are not appropriate and will be expensive to draw.
        /// </remarks>
        public float LodMultiplier
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.LodMultiplierOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.LodMultiplierOffset);
            }
            set => Function.Call(Hash.SET_VEHICLE_LOD_MULTIPLIER, Handle, value);
        }

        /// <summary>
        /// Gets the handling data attached to this <see cref="Vehicle"/>.
        /// </summary>
        public HandlingData HandlingData
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.HandlingDataOffset == 0)
                    return new HandlingData(IntPtr.Zero);

                return new HandlingData(SHVDN.MemDataMarshal.ReadAddress(MemoryAddress + SHVDN.NativeMemory.Vehicle.HandlingDataOffset));
            }
        }

        #endregion

        #region Health

        /// <summary>
        /// Gets or sets the body health of this <see cref="Vehicle"/>.
        /// </summary>
        public float BodyHealth
        {
            get => Function.Call<float>(Hash.GET_VEHICLE_BODY_HEALTH, Handle);
            set => Function.Call(Hash.SET_VEHICLE_BODY_HEALTH, Handle, value);
        }

        /// <summary>
        /// Gets or sets this <see cref="Vehicle"/>'s engine health.
        /// <para>
        /// When this value is less than 0.0, the engine will not work.
        /// </para>
        /// <para>
        /// When this value is between -1000.0 and 0.0 exclusive, the engine is on fire and the value will decrease until it reaches -1000.0.
        /// While on fire, burning engine "may" set the petrol tank on fire as well, but there's only a chance of this.
        /// </para>
        /// </summary>
        public float EngineHealth
        {
            get => Function.Call<float>(Hash.GET_VEHICLE_ENGINE_HEALTH, Handle);
            set => Function.Call(Hash.SET_VEHICLE_ENGINE_HEALTH, Handle, value);
        }

        /// <summary>
        /// <para>Gets or sets this <see cref="Vehicle"/> petrol tank health.</para>
        /// <para>
        /// When this value is between -1000.0 and 0.0 exclusive, the petrol tank is on fire and the value will decrease until it reaches -1000.0.
        /// The <see cref="Vehicle"/> will explode when this health reaches -1000.0.
        /// </para>
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
                if (!IsHeliOrBlimp)
                {
                    return 0.0f;
                }

                return Function.Call<float>(Hash.GET_HELI_TAIL_BOOM_HEALTH, Handle);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.HeliTailBoomHealthOffset == 0 || !IsHeliOrBlimp)
                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + SHVDN.NativeMemory.Vehicle.HeliTailBoomHealthOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the main rotor health for this heli.
        /// </summary>
        public float HeliMainRotorHealth
        {
            get
            {
                if (!IsHeliOrBlimp)
                {
                    return 0.0f;
                }

                return Function.Call<float>(Hash.GET_HELI_MAIN_ROTOR_HEALTH, Handle);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.HeliMainRotorHealthOffset == 0 || !IsHeliOrBlimp)
                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + SHVDN.NativeMemory.Vehicle.HeliMainRotorHealthOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the tail rotor health for this heli.
        /// </summary>
        public float HeliTailRotorHealth
        {
            get
            {
                if (!IsHeliOrBlimp)
                {
                    return 0.0f;
                }

                return Function.Call<float>(Hash.GET_HELI_TAIL_ROTOR_HEALTH, Handle);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.HeliTailRotorHealthOffset == 0 || !IsHeliOrBlimp)
                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + SHVDN.NativeMemory.Vehicle.HeliTailRotorHealthOffset, value);
            }
        }

        #endregion

        #region Radio

        /// <summary>
        /// Sets whether the Radio of this <see cref="Vehicle"/> is enabled.
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
                    Function.Call(Hash.SET_VEH_RADIO_STATION, Handle, "OFF");
                }
                else if (Enum.IsDefined(typeof(RadioStation), value))
                {
                    Function.Call(Hash.SET_VEH_RADIO_STATION, Handle, Game.s_radioNames[(int)value]);
                }
            }
        }

        #endregion

        #region Engine

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/>s engine is running.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="Vehicle"/>s engine is running; otherwise, <see langword="false" />.
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
        /// <see langword="true" /> if this <see cref="Vehicle"/>s engine is starting; otherwise, <see langword="false" />.
        /// </value>
        public bool IsEngineStarting
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.IsEngineStartingOffset == 0)
                    return false;

                return SHVDN.MemDataMarshal.IsBitSet(address + SHVDN.NativeMemory.Vehicle.IsEngineStartingOffset, 5);
            }
        }

        /// <summary>
        /// Sets the value indicating whether the aircraft engine of this <see cref="Vehicle"/> can degrade.
        /// </summary>
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
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.EngineTemperatureOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.EngineTemperatureOffset);
            }
        }

        public float EnginePowerMultiplier
        {
            [Obsolete("The getter of Vehicle.EnginePowerMultiplier is obsolete since MODIFY_VEHICLE_TOP_SPEED does not store the passed value " +
                "as the 2nd argument to any of vehicle members in v1.0.887.1 or earlier. This property returns zero in those versions."),
            EditorBrowsable(EditorBrowsableState.Never)]
            get
            {
                if (Game.Version < GameVersion.v1_0_944_2_Steam)
                {
                    return 0.0f;
                }

                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.EnginePowerMultiplierOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.EnginePowerMultiplierOffset);
            }
            set => Function.Call(Hash.MODIFY_VEHICLE_TOP_SPEED, Handle, value);
        }

        public float EngineTorqueMultiplier
        {
            set => Function.Call(Hash.SET_VEHICLE_CHEAT_POWER_INCREASE, Handle, value);
        }

        /// <summary>
        /// Gets or sets this <see cref="Vehicle"/> oil level.
        /// If this value is above zero, this value decreases instead of <see cref="EngineHealth"/> when the engine emits black smoke.
        /// </summary>
        public float OilLevel
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.OilLevelOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.OilLevelOffset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.OilLevelOffset == 0)

                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + SHVDN.NativeMemory.Vehicle.OilLevelOffset, value);
            }
        }

        /// <summary>
        /// Gets the oil volume of this <see cref="Vehicle"/>.
        /// </summary>
        public float OilVolume
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address))
                    return 0.0f;

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
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.FuelLevelOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.FuelLevelOffset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.FuelLevelOffset == 0)

                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + SHVDN.NativeMemory.Vehicle.FuelLevelOffset, value);
            }
        }

        /// <summary>
        /// Gets the petrol tank volume of this <see cref="Vehicle"/>.
        /// </summary>
        public float PetrolTankVolume
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address))
                    return 0.0f;

                return HandlingData.PetrolTankVolume;
            }
        }

        #endregion

        #region Performance & Driving

        /// <summary>
        /// Gets or sets the gears value of this <see cref="Vehicle"/>.
        /// </summary>
        [Obsolete("Use Vehicle.HighGear for the high gear value and Vehicle.CurrentGear for the current gear value instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public int Gears
        {
            get => HighGear;
            set => HighGear = value;
        }

        /// <summary>
        /// Gets or sets the high gear value of this <see cref="Vehicle"/>.
        /// The highest acceptable value is 10 since v1.0.1604.0 and is 7 in earlier game versions, so you cannot
        /// corrupt the memory region for this <see cref="Vehicle"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the passed value is too high.</exception>
        public int HighGear
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.HighGearOffset == 0)
                    return 0;

                return SHVDN.MemDataMarshal.ReadByte(address + SHVDN.NativeMemory.Vehicle.HighGearOffset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.HighGearOffset == 0)

                    return;

                if (Game.FileVersion >= ExeVersionConsts.v1_0_1604_0)
                {
                    if (value > 10)
                    {
                        ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value), "Values must be between 0 and 10, inclusive.");
                    }
                }
                else if (value > 7)
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value), "Values must be between 0 and 7, inclusive.");
                }

                SHVDN.MemDataMarshal.WriteByte(address + SHVDN.NativeMemory.Vehicle.HighGearOffset, (byte)value);
            }
        }

        /// <summary>
        /// Gets or sets the next gear value of this <see cref="Vehicle"/>.
        /// </summary>
        public int NextGear
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.NextGearOffset == 0)
                    return 0;

                return SHVDN.MemDataMarshal.ReadByte(address + SHVDN.NativeMemory.Vehicle.NextGearOffset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.NextGearOffset == 0)

                    return;

                SHVDN.MemDataMarshal.WriteByte(address + SHVDN.NativeMemory.Vehicle.NextGearOffset, (byte)value);
            }
        }

        /// <summary>
        /// Gets or sets the current gear this <see cref="Vehicle"/> is using.
        /// </summary>
        public int CurrentGear
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.GearOffset == 0)
                    return 0;

                return SHVDN.MemDataMarshal.ReadByte(address + SHVDN.NativeMemory.Vehicle.GearOffset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.GearOffset == 0)

                    return;

                SHVDN.MemDataMarshal.WriteByte(address + SHVDN.NativeMemory.Vehicle.GearOffset, (byte)value);
            }
        }

        /// <summary>
        /// Gets or sets the current turbo value of this <see cref="Vehicle"/>.
        /// </summary>
        /// <remarks>
        /// Affects the engine performance only when <see cref="VehicleToggleModType.Turbo"/> is installed.
        /// </remarks>
        public float Turbo
        {
            get => SHVDN.NativeMemory.Vehicle.GetTurbo(Handle);
            set => SHVDN.NativeMemory.Vehicle.SetTurbo(Handle, value);
        }

        /// <summary>
        /// Gets or sets the current clutch of this <see cref="Vehicle"/>.
        /// </summary>
        public float Clutch
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.ClutchOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.ClutchOffset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.ClutchOffset == 0)

                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + SHVDN.NativeMemory.Vehicle.ClutchOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the current throttle of this <see cref="Vehicle"/>.
        /// </summary>
        public float Throttle
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.AccelerationOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.AccelerationOffset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.AccelerationOffset == 0)

                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + SHVDN.NativeMemory.Vehicle.AccelerationOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the current brake power of this <see cref="Vehicle"/>.
        /// </summary>
        public float BrakePower
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.BrakePowerOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.BrakePowerOffset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.BrakePowerOffset == 0)

                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + SHVDN.NativeMemory.Vehicle.BrakePowerOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the current throttle power of this <see cref="Vehicle"/>.
        /// </summary>
        public float ThrottlePower
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.ThrottlePowerOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.ThrottlePowerOffset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.ThrottlePowerOffset == 0)

                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + SHVDN.NativeMemory.Vehicle.ThrottlePowerOffset, value);
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
        /// Reduces this <see cref="Vehicle"/>'s current grip level.
        /// Only works on Automobiles, Helicopters, and Planes.
        /// Only supported in game versions 1.0.1604.0 or later.
        /// </summary>
        /// <param name="level">The level from 0-3 to reduce grip by, with 0 being no reduction and 3 being maximum reduction.</param>
        public void SetReducedGripLevel(int level)
        {
            GameVersionNotSupportedException.ThrowIfNotSupported(ExeVersionConsts.v1_0_1604_0, nameof(Vehicle), nameof(SetReducedGripLevel));

            Function.Call(Hash.SET_VEHICLE_REDUCE_GRIP_LEVEL, Handle, level);
        }

        /// <summary>
        /// Gets the speed the drive wheels are turning at.
        /// This is the value used for the dashboard speedometers (after being converted to mph).
        /// </summary>
        public float WheelSpeed
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.WheelSpeedOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.WheelSpeedOffset);
            }
        }

        /// <summary>
        /// Sets this <see cref="Vehicle"/>s forward speed in m/s.
        /// </summary>
        /// <remarks>
        /// If this <see cref="IsTrain"/> for this <see cref="Vehicle"/> returns <c>true</c>,
        /// both the speed and the cruise speed are set to the passed <c>value</c>.
        /// </remarks>
        public float ForwardSpeed
        {
            set
            {
                if (IsTrain)
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
        /// Gets or sets the speed of the Heli Blades if <see cref="IsRotaryWingAircraft"/> returns <c>true</c>.
        /// </summary>
        public float HeliBladesSpeed
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.HeliBladesSpeedOffset == 0 || !IsRotaryWingAircraft)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.HeliBladesSpeedOffset);
            }
            set
            {
                if (!IsRotaryWingAircraft)
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
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.CurrentRpmOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.CurrentRpmOffset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.CurrentRpmOffset == 0)

                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + SHVDN.NativeMemory.Vehicle.CurrentRpmOffset, value);
            }
        }

        /// <summary>
        /// Gets the acceleration of this <see cref="Vehicle"/>.
        /// </summary>
        public float Acceleration
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.AccelerationOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.AccelerationOffset);
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
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.SteeringAngleOffset == 0)
                    return 0.0f;

                return (float)(SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.SteeringAngleOffset) * (180.0 / System.Math.PI));
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.SteeringAngleOffset == 0)

                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + SHVDN.NativeMemory.Vehicle.SteeringAngleOffset, (float)(value * (System.Math.PI / 180.0)));
            }
        }

        /// <summary>
        /// Gets or sets the steering scale of this <see cref="Vehicle"/>.
        /// </summary>
        public float SteeringScale
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.SteeringScaleOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.SteeringScaleOffset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.SteeringScaleOffset == 0)

                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + SHVDN.NativeMemory.Vehicle.SteeringScaleOffset, value);
            }
        }

        #endregion

        #region Alarm

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> has an alarm set.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has an alarm set; otherwise, <see langword="false" />.
        /// </value>
        public bool IsAlarmSet
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.AlarmTimeOffset == 0)
                    return false;

                return (ushort)SHVDN.MemDataMarshal.ReadInt16(address + SHVDN.NativeMemory.Vehicle.AlarmTimeOffset) == ushort.MaxValue; //The alarm is set when the value is 0xFFFF
            }
            set => Function.Call(Hash.SET_VEHICLE_ALARM, Handle, value);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> is sounding its alarm.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> is sounding its alarm; otherwise, <see langword="false" />.
        /// </value>
        public bool IsAlarmSounding => Function.Call<bool>(Hash.IS_VEHICLE_ALARM_ACTIVATED, Handle);

        /// <summary>
        /// Gets or sets time left before this <see cref="Vehicle"/> alarm stops.
        /// If greater than zero, the vehicle alarm will be sounding.
        /// The highest acceptable value is 65534.
        /// </summary>
        /// <value>
        /// The time left before this <see cref="Vehicle"/> alarm stops.
        /// </value>
        public int AlarmTimeLeft
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.AlarmTimeOffset == 0)
                    return 0;

                ushort alarmTime = (ushort)SHVDN.MemDataMarshal.ReadInt16(address + SHVDN.NativeMemory.Vehicle.AlarmTimeOffset);
                return alarmTime != ushort.MaxValue ? alarmTime : 0;
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || (ushort)value == ushort.MaxValue || SHVDN.NativeMemory.Vehicle.AlarmTimeOffset == 0)

                    return;

                SHVDN.MemDataMarshal.WriteInt16(address + SHVDN.NativeMemory.Vehicle.AlarmTimeOffset, (short)value);
            }
        }

        /// <summary>
        /// Starts the vehicle alarm, if possible.
        /// </summary>
        /// <remarks>
        /// The alarm will sound only when all of the following conditions are met:
        /// - <see cref="Vehicle.Type"/> is <see cref="VehicleType.Automobile"/>
        /// - <see cref="IsAlarmSet"/> is <see langword="true"/>
        /// - The vehicle is not considered destroyed (<see cref="IsConsideredDestroyed"/> is <see langword="false"/>)
        /// </remarks>
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
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has a siren; otherwise, <see langword="false" />.
        /// </value>
        public bool HasSiren => SHVDN.NativeMemory.Vehicle.HasSiren(Handle);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> can use a siren.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> can activate a siren; otherwise, <see langword="false" />.
        /// </value>
        public bool CanUseSiren
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.CanUseSirenOffset == 0)
                    return false;

                return SHVDN.MemDataMarshal.IsBitSet(address + SHVDN.NativeMemory.Vehicle.CanUseSirenOffset, 1);
            }
        }

        /// <summary>
        /// Sets the value that determines this <see cref="Vehicle"/> can use siren if a given <see langword="bool"/> is <see langword="false"/>
        /// or the <see langword="bool"/> is <see langword="true"/> and <see cref="CanUseSiren"/> returns <see langword="true"/> on the <see cref="Vehicle"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns <see langword="true"/> if the supplied value can be set.</returns>
        public bool TrySetCanUseSiren(bool value)
        {
            if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.CanUseSirenOffset == 0)
                return false;

            if (value && !CanUseSiren)
            {
                return false;
            }

            SHVDN.MemDataMarshal.SetBit(address + SHVDN.NativeMemory.Vehicle.CanUseSirenOffset, 1, value);
            return true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its siren turned on.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="Vehicle"/> has its siren turned on; otherwise, <see langword="false" />.
        /// </value>
        public bool IsSirenActive
        {
            get => Function.Call<bool>(Hash.IS_VEHICLE_SIREN_ON, Handle);
            set => Function.Call(Hash.SET_VEHICLE_SIREN, Handle, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the siren on this <see cref="Vehicle"/> is muted.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the siren on this <see cref="Vehicle"/> is muted; otherwise, <see langword="false" />.
        /// </value>
        public bool IsSirenSilent
        {
            get => SHVDN.NativeMemory.Vehicle.HasMutedSirens(Handle);
            set => Function.Call(Hash.SET_VEHICLE_HAS_MUTED_SIRENS, Handle, value);
        }

        /// <summary>
        /// Sets a value indicating whether this <see cref="Vehicle"/> has its horn enabled.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="Vehicle"/> has its horn enabled; otherwise, <see langword="false" />.
        /// </value>
        public bool IsHornEnabled
        {
            set => Function.Call(Hash.SET_HORN_ENABLED, Handle, value);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> has its horn turned on.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="Vehicle"/> has its horn turned on; otherwise, <see langword="false" />.
        /// </value>
        public bool IsHornActive
        {
            get => Function.Call<bool>(Hash.IS_HORN_ACTIVE, Handle);
        }

        /// <summary>
        /// Sounds the horn on this <see cref="Vehicle"/>.
        /// </summary>
        /// <param name="duration">The duration in milliseconds to sound the horn for.</param>
        public void SoundHorn(int duration)
        {
            Function.Call(Hash.START_VEHICLE_HORN, Handle, duration, StringHash.AtStringHash("HELDDOWN"), 0);
        }

        #endregion

        #region Lights

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its lights on.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has its lights on; otherwise, <see langword="false" />.
        /// </value>
        public bool AreLightsOn
        {
            get
            {
                bool areLightsOn;
                unsafe
                {
                    Function.Call(Hash.GET_VEHICLE_LIGHTS_STATE, Handle, &areLightsOn, InputArgument.DiscardPtr64);
                }

                return areLightsOn;
            }
            [Obsolete("The setter of Vehicle.AreLightsOn is obsolete. Use Vehicle.SetScriptedLightSetting instead."),
            EditorBrowsable(EditorBrowsableState.Never)]
            set => Function.Call(Hash.SET_VEHICLE_LIGHTS, Handle, value ? 3 : 4);
        }

        /// <summary>
        /// Sets scripted vehicle light setting.
        /// </summary>
        /// <param name="lightSetting">
        /// The scripted light setting to set.
        /// If set to <see cref="ScriptedVehicleLightSetting.SetVehicleLightsOn"/>
        /// or <see cref="ScriptedVehicleLightSetting.SetVehicleLightsOff"/>, the method will also change non-scripted light states
        /// (for <see cref="ScriptedVehicleLightSetting.SetVehicleLightsOff"/>, in a bit incomplete way).
        /// </param>
        public void SetScriptedLightSetting(ScriptedVehicleLightSetting lightSetting)
        {
            Function.Call(Hash.SET_VEHICLE_LIGHTS, Handle, (int)lightSetting);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its high beams on.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has its high beams on; otherwise, <see langword="false" />.
        /// </value>
        public bool AreHighBeamsOn
        {
            get
            {
                bool highbeamState;
                unsafe
                {
                    Function.Call(Hash.GET_VEHICLE_LIGHTS_STATE, Handle, InputArgument.DiscardPtr64, &highbeamState);
                }

                return highbeamState;
            }
            set => Function.Call(Hash.SET_VEHICLE_FULLBEAM, Handle, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its interior lights on.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has its interior lights on; otherwise, <see langword="false" />.
        /// </value>
        public bool IsInteriorLightOn
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.IsInteriorLightOnOffset == 0)
                    return false;

                return SHVDN.MemDataMarshal.IsBitSet(address + SHVDN.NativeMemory.Vehicle.IsInteriorLightOnOffset, 6);
            }
            set => Function.Call(Hash.SET_VEHICLE_INTERIORLIGHT, Handle, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its search light on.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has its search light on; otherwise, <see langword="false" />.
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
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has its taxi light on; otherwise, <see langword="false" />.
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
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has its left indicator light on; otherwise, <see langword="false" />.
        /// </value>
        public bool IsLeftIndicatorLightOn
        {
            get => SHVDN.MemDataMarshal.IsBitSet(MemoryAddress + SHVDN.NativeMemory.Vehicle.IsInteriorLightOnOffset, 0);
            set => Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, Handle, true, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its right indicator light on.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has its right indicator light on; otherwise, <see langword="false" />.
        /// </value>
        public bool IsRightIndicatorLightOn
        {
            get => SHVDN.MemDataMarshal.IsBitSet(MemoryAddress + SHVDN.NativeMemory.Vehicle.IsInteriorLightOnOffset, 1);
            set => Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, Handle, false, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> has its brake light on.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has its brake light on; otherwise, <see langword="false" />.
        /// </value>
        public bool AreBrakeLightsOn
        {
            set => Function.Call(Hash.SET_VEHICLE_BRAKE_LIGHTS, Handle, value);
        }

        public float LightsMultiplier
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.LightsMultiplierOffset == 0)
                    return 0.0f;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.LightsMultiplierOffset);
            }
            set => Function.Call(Hash.SET_VEHICLE_LIGHT_MULTIPLIER, Handle, value);
        }
        #endregion

        #region Damaging

        /// <summary>
        /// <para>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> is considered destroyed.
        /// Will be set to <see langword="true"/> when <see cref="Vehicle"/>s are exploded or sinking for a short time.
        /// </para>
        /// <para>
        /// <see cref="Entity.IsDead"/> will return <see langword="true"/> and <see cref="IsDriveable"/> will return <see langword="false"/> if this value is set to <see langword="true"/>.
        /// Does not affect if this <see cref="Vehicle"/> will rendered scorched.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Many features of <see cref="Vehicle"/> will be disabled when this value is set to <see langword="true"/>.
        /// For example, <see cref="Ped"/>s cannot enter <see cref="Vehicle"/>s considered destroyed or start the engines of them. <see cref="Ped"/>s cannot use weapons of them.
        /// The player cannot unflip <see cref="Vehicle"/>s considered destroyed.
        /// </remarks>
        public bool IsConsideredDestroyed
        {
            get
            {
                // Return true if the entity does not exist, just like IS_ENTITY_DEAD will return true in the same condition
                if (!TryGetMemoryAddress(out IntPtr address))
                    return true;

                return (SHVDN.MemDataMarshal.ReadByte(address + 0xD8) & 7) == 3;
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))

                    return;

                int targetValue = SHVDN.MemDataMarshal.ReadByte(address + 0xD8) & 0xF8;

                if (value)
                {
                    targetValue |= 3;
                }

                SHVDN.MemDataMarshal.WriteByte(address + 0xD8, (byte)targetValue);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> has any damage decals.
        /// </summary>
        public bool HasDamageDecals => Function.Call<bool>(Hash.GET_DOES_VEHICLE_HAVE_DAMAGE_DECALS, Handle);


        /// <inheritdoc cref="Vehicle.HasDamageDecals"/>
        [Obsolete("Use Vehicle.HasDamageDecals instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsDamaged => Function.Call<bool>(Hash.GET_DOES_VEHICLE_HAVE_DAMAGE_DECALS, Handle);

        /// <summary>
        /// Gets the value that indicates whether this <see cref="Vehicle"/> is driveable.
        /// For the setter, it behaves in the same way as <see cref="IsUndriveable"/> except that this setter negates the value.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Vehicle"/> is not destroyed (<see cref="IsConsideredDestroyed"/>
        /// returns <see langword="false"/>) and both <see cref="PetrolTankHealth"/> and <see cref="EngineHealth"/> are
        /// greater than 0.0f; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsDriveable
        {
            get => Function.Call<bool>(Hash.IS_VEHICLE_DRIVEABLE, Handle, 0);
            [Obsolete("The setter of Vehicle.IsDriveable is obsolete because SET_VEHICLE_UNDRIVEABLE sets a value to" +
                "an dedicated flag that IS_VEHICLE_DRIVEABLE does not access. Use Vehicle.IsUndriveable instead.")
                , EditorBrowsable(EditorBrowsableState.Never)]
            set => Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, Handle, !value);
        }

        /// <summary>
        /// Sets an internal flag which prevents the engine from being started.
        /// </summary>
        public bool IsUndriveable
        {
            set => Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, Handle, value);
        }

        public bool IsLeftHeadLightBroken
        {
            get => Function.Call<bool>(Hash.GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED, Handle);
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.IsHeadlightDamagedOffset == 0)

                    return;

                SHVDN.MemDataMarshal.SetBit(address + SHVDN.NativeMemory.Vehicle.IsHeadlightDamagedOffset, 0, value);
            }
        }

        public bool IsRightHeadLightBroken
        {
            get => Function.Call<bool>(Hash.GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED, Handle);
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.IsHeadlightDamagedOffset == 0)

                    return;

                SHVDN.MemDataMarshal.SetBit(address + SHVDN.NativeMemory.Vehicle.IsHeadlightDamagedOffset, 1, value);
            }
        }

        /// <summary>
        /// Indicates whether the rear bumper of this <see cref="Vehicle"/> is detached.
        /// </summary>
        public bool IsRearBumperBrokenOff => Function.Call<bool>(Hash.IS_VEHICLE_BUMPER_BROKEN_OFF, Handle, false);

        /// <summary>
        /// Indicates whether the front bumper of this <see cref="Vehicle"/> is detached.
        /// </summary>
        public bool IsFrontBumperBrokenOff => Function.Call<bool>(Hash.IS_VEHICLE_BUMPER_BROKEN_OFF, Handle, true);

        /// <summary>
        /// Sets a value indicating whether this <see cref="Vehicle"/> has strong axles, meaning they won't break as easily.
        /// </summary>
        public bool IsAxlesStrong
        {
            set => Function.Call<bool>(Hash.SET_VEHICLE_HAS_STRONG_AXLES, Handle, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Tires of this <see cref="Vehicle"/> can burst.
        /// </summary>
        public bool CanTiresBurst
        {
            get => Function.Call<bool>(Hash.GET_VEHICLE_TYRES_CAN_BURST, Handle);
            set => Function.Call(Hash.SET_VEHICLE_TYRES_CAN_BURST, Handle, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Tires of this <see cref="Vehicle"/> can break.
        /// </summary>
        public bool CanWheelsBreak
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.CanWheelBreakOffset == 0)
                    return false;

                return !SHVDN.MemDataMarshal.IsBitSet(address + SHVDN.NativeMemory.Vehicle.CanWheelBreakOffset, 6);
            }
            set => Function.Call(Hash.SET_VEHICLE_WHEELS_CAN_BREAK, Handle, value);
        }

        /// <summary>
        /// Sets a value indicating whether this <see cref="Vehicle"/> can be visibly damaged.
        /// </summary>
        public bool CanBeVisiblyDamaged
        {
            set => Function.Call(Hash.SET_VEHICLE_CAN_BE_VISIBLY_DAMAGED, Handle, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vehicle"/> drops money when destroyed.
        /// Only works if the vehicle model is a car, quad bikes or trikes (strictly if the internal vehicle class is CAutomobile or derived class from CAutomobile).
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="Vehicle"/> drops money when destroyed; otherwise, <see langword="false" />.
        /// </value>
        public bool DropsMoneyOnExplosion
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.DropsMoneyWhenBlownUpOffset == 0)
                    return false;

                // Check if the vehicle class is CAutomobile or a subclass of it
                if ((uint)Type <= 10)
                {
                    return SHVDN.MemDataMarshal.IsBitSet(address + SHVDN.NativeMemory.Vehicle.DropsMoneyWhenBlownUpOffset, 1);
                }

                return false;
            }
            set => Function.Call(Hash.SET_VEHICLE_DROPS_MONEY_WHEN_BLOWN_UP, Handle, value);
        }

        /// <summary>
        /// Applies damage deformation to this <see cref="Vehicle"/>.
        /// You might want to pass <paramref name="damage"/> and <paramref name="deformation"/> where the product is
        /// larger than 5000 to see an observable result.
        /// </summary>
        /// <param name="position">The coordinates where the damage is applied to the <see cref="Vehicle"/>.</param>
        /// <param name="damage">
        /// Scales how much damage is applied to the <see cref="Vehicle"/>.
        /// Not known whether the outcome will be exactly the same if this parameter and <paramref name="deformation"/>
        /// are passed in the reversed order.
        /// </param>
        /// <param name="deformation">
        /// Sets how much the vehicle is deformed by (not exactly a radius parameter).
        /// Not known whether the outcome will be exactly the same if this parameter and <paramref name="deformation"/>
        /// are passed in the reversed order.
        /// </param>
        /// <param name="localDamage">
        /// If <see langword="true"/>, this method will apply the damage local to the <see cref="Vehicle"/>'s space.
        /// If <see langword="false"/>, this method will apply the damage in the world space.
        /// You would likely set this parameter to <see langword="true"/> in most cases.
        /// </param>
        /// <remarks>
        /// Does not deal damage to any health values.
        /// </remarks>
        public void ApplyDamageDeformation(Vector3 position, float damage, float deformation, bool localDamage)
        {
            Function.Call(Hash.SET_VEHICLE_DAMAGE, Handle, position.X, position.Y, position.Z, damage, deformation, localDamage);
        }
        [Obsolete("Use ApplyDamageDeformation instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public void ApplyDamage(Vector3 position, float damageAmount, float radius)
        {
            Function.Call(Hash.SET_VEHICLE_DAMAGE, Handle, position.X, position.Y, position.Z, damageAmount, radius);
        }

        /// <summary>
        /// Returns <see langword="true"/> if the stuck timer of this <see cref="Vehicle"/> for a passed stuck type
        /// is more than a passed time value.
        /// </summary>
        /// <param name="stuckType">The stuck type to test.</param>
        /// <param name="requiredTime">
        /// <para>
        /// The minimum required time for the stuck timer for the <paramref name="stuckType"/> to surpass so
        /// this method can return <see langword="true"/>.
        /// </para>
        /// </param>
        /// <returns><see langword="true"/> if the stuck timer for the passed stuck time is more than
        /// <paramref name="requiredTime"/>, otherwise, <see langword="false"/>. If the stuck timer is the same as
        /// <paramref name="requiredTime"/>, this method will return <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="stuckType"/> is <see cref="VehicleStuckType.ResetAll"/>.
        /// </exception>
        /// <remarks>
        /// <para>
        /// <see cref="Vehicle"/> stuck timers increase when the position delta of <see cref="Vehicle"/> are not too
        /// large and they meet their certain conditions, and reset to zero otherwise. The max value of the stuck
        /// timers are `<c>65535</c>` and they clamp to `65535` when the addition of the time step in milliseconds to
        /// current stuck timer values are `65535` or more, so no overflows will not occur for them.
        /// </para>
        /// <para>
        /// The position delta limit from the last stuck position except for <see cref="VehicleStuckType.Jammed"/> is
        /// for non-watercraft <see cref="Vehicle"/>s are `<c>2f</c>` and the one for watercraft ones are `<c>1f</c>`.
        /// When the <see cref="Vehicle"/> is a watercraft and this method calculates a position delta for the 3 stuck
        /// timers (not for the jammed one), the Z-axis delta is multiplied by `<c>0.2f</c>`. How the method does
        /// determine if the <see cref="Vehicle"/> is a watercraft is described in the paragraph below.
        /// The one for <see cref="VehicleStuckType.Jammed"/> from the last jammed position is `<c>3f</c>` for any
        /// <see cref="Vehicle"/>s.
        /// The last jammed position for <see cref="VehicleStuckType.Jammed"/> is different from the one for any other
        /// stuck types, and they update independently of each other and they do only when the position deltas are too
        /// large to consider the <see cref="Vehicle"/> stucked or jammed.
        /// </para>
        /// <para>
        /// If the <see cref="VehicleType"/> of <see cref="Vehicle"/> is <see cref="VehicleType.Boat"/>,
        /// <see cref="VehicleType.Submarine"/>, <see cref="VehicleType.SubmarineCar"/>,
        /// <see cref="VehicleType.AmphibiousAutomobile"/>, or <see cref="VehicleType.AmphibiousQuadBike"/>, or if
        /// the <see cref="Vehicle"/> has a <see cref="SeaPlaneHandlingData"/> and is touching water at all,
        /// this method treats <see cref="Vehicle"/> as a watercraft. However, if the <see cref="VehicleType"/> of
        /// <see cref="Vehicle"/> is <see cref="VehicleType.SubmarineCar"/>,
        /// <see cref="VehicleType.AmphibiousAutomobile"/>, or <see cref="VehicleType.AmphibiousQuadBike"/> but it is
        /// not touching water at all, this method does not treat <see cref="Vehicle"/> as a watercraft.
        /// </para>
        /// <para>
        /// the method always returns <see langword="false"/> when `<c>65535</c>` is passed to
        /// <paramref name="requiredTime"/>.
        /// </para>
        /// </remarks>
        // Since the native internally clamps the value to 65535 if the passed `requiredTime` arg is more than
        // 65535 and internal stuck timer fields in `CVehicleDamage` are also `u16` values (same as `unsigned short`
        // when the game code base is built on Windows), we use `ushort` for our`requiredTime` arg.
        // If Rockstar changes the type to `u32`, we could add another overload.
        public bool IsStuckTimerUp(VehicleStuckType stuckType, ushort requiredTime)
        {
            // `ResetAll` is only for `RESET_VEHICLE_STUCK_TIMER`
            if (stuckType is VehicleStuckType.ResetAll)
            {
                ThrowHelper.ThrowArgumentException("`VehicleStuckType.ResetAll` is not supported.", nameof(stuckType));
            }

            return Function.Call<bool>(Hash.IS_VEHICLE_STUCK_TIMER_UP, Handle, stuckType, requiredTime);
        }

        /// <summary>
        /// Resets a stuck timer of this <see cref="Vehicle"/>.
        /// </summary>
        /// <param name="stuckType">
        /// The stuck type to specify a stuck timer to reset. <see cref="VehicleStuckType.ResetAll"/> resets
        /// all of the 4 stuck timers.
        /// </param>
        public void ResetVehicleStuckTimer(VehicleStuckType stuckType)
            => Function.Call(Hash.RESET_VEHICLE_STUCK_TIMER, Handle, stuckType);

        #endregion

        #region Doors & Locks

        /// <summary>
        /// Gets whether this <see cref="Vehicle"/> has a roof.
        /// </summary>
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

        /// <summary>
        /// Gets whether this <see cref="Vehicle"/> is performing a burnout.
        /// </summary>
        public bool IsInBurnout => Function.Call<bool>(Hash.IS_VEHICLE_IN_BURNOUT, Handle);

        public bool IsBurnoutForced
        {
            set => Function.Call<bool>(Hash.SET_VEHICLE_BURNOUT, Handle, value);
        }

        /// <summary>
        /// Sets a value indicating whether the Handbrake on this <see cref="Vehicle"/> is forced on.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if the Handbrake on this <see cref="Vehicle"/> is forced on; otherwise, <see langword="false" />.
        /// </value>
        public bool IsHandbrakeForcedOn
        {
            set => Function.Call(Hash.SET_VEHICLE_HANDBRAKE, Handle, value);
        }

        #endregion

        #region Occupants

        /// <summary>
        /// Gets the <see cref="Ped"/> occupying the driver seat, or <see langword="null"/> if the seat is unoccupied.
        /// </summary>
        public Ped Driver => GetPedOnSeat(VehicleSeat.Driver);

        /// <summary>
        /// Gets the <see cref="Ped"/> occupying the specified <see cref="VehicleSeat"/>, or <see langword="null"/> if the seat is unoccupied.
        /// </summary>
        public Ped GetPedOnSeat(VehicleSeat seat)
        {
            int handle = Function.Call<int>(Hash.GET_PED_IN_VEHICLE_SEAT, Handle, (int)seat);
            return handle != 0 ? new Ped(handle) : null;
        }


        /// <summary>
        /// Gets an array of all <see cref="Ped"/>s in this <see cref="Vehicle"/>.
        /// </summary>
        /// <remarks>
        /// Returns an empty array if there are no occupants.
        /// If this <see cref="Vehicle"/> has a <see cref="Driver"/>, the driver will be at index 0.
        /// Remaining occupants are ordered according to their seat assignment.
        /// </remarks>
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

        /// <summary>
        /// Gets an array of all passengers in this <see cref="Vehicle"/>.
        /// </summary>
        /// <remarks>
        /// Returns an empty array if there are no passengers.
        /// The occupants are ordered according to their seat assignment.
        /// </remarks>
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

        /// <summary>
        /// Gets the number of passenger <see cref="Ped"/>s, excluding the <see cref="Driver"/>.
        /// </summary>
        public int PassengerCount => Function.Call<int>(Hash.GET_VEHICLE_NUMBER_OF_PASSENGERS, Handle, false, true);

        /// <summary>
        /// Gets the number of passengers this <see cref="Vehicle"/> can carry, excluding the <see cref="Driver"/>.
        /// </summary>
        public int PassengerCapacity => Function.Call<int>(Hash.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, Handle);

        /// <summary>
        /// Creates a <see cref="Ped"/> on the specified seat.
        /// </summary>
        /// <param name="seat">The seat the new <see cref="Ped"/> will be spawned.</param>
        /// <param name="model">The model for the new <see cref="Ped"/>.</param>
        /// <exception cref="ArgumentException">Another <see cref="Ped"/> already occupies <paramref name="seat"/> of the <see cref="Vehicle"/>.</exception>
        /// <remarks>Returns <see langword="null"/> if <paramref name="model"/> is not for a <see cref="Ped"/> or it cannot be loaded within one second.</remarks>
        public Ped CreatePedOnSeat(VehicleSeat seat, Model model)
        {
            if (!IsSeatFree(seat))
            {
                ThrowHelper.ThrowArgumentException("The VehicleSeat selected was not free", nameof(seat));
            }

            if (!model.IsPed || !model.Request(1000))
            {
                return null;
            }

            return new Ped(Function.Call<int>(Hash.CREATE_PED_INSIDE_VEHICLE, Handle, 26, model.Hash, (int)seat, 1, 1));
        }

        /// <summary>
        /// Creates a random <see cref="Ped"/> on the specified seat.
        /// </summary>
        /// <param name="seat">The seat the new <see cref="Ped"/> will be spawned.</param>
        /// <exception cref="ArgumentException">Another <see cref="Ped"/> already occupies <paramref name="seat"/> of the <see cref="Vehicle"/>.</exception>
        public Ped CreateRandomPedOnSeat(VehicleSeat seat)
        {
            if (!IsSeatFree(seat))
            {
                ThrowHelper.ThrowArgumentException("The VehicleSeat selected was not free", nameof(seat));
            }

            if (seat == VehicleSeat.Driver)
            {
                return new Ped(Function.Call<int>(Hash.CREATE_RANDOM_PED_AS_DRIVER, Handle, true));
            }

            int pedHandle = Function.Call<int>(Hash.CREATE_RANDOM_PED, 0f, 0f, 0f);
            Function.Call(Hash.SET_PED_INTO_VEHICLE, pedHandle, Handle, (int)seat);

            return new Ped(pedHandle);
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="VehicleSeat"/> is unoccupied.
        /// </summary>
        /// <param name="seat">The seat to check.</param>
        /// <returns>
        /// <see langword="true"/> if the seat is unoccupied; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSeatFree(VehicleSeat seat)
        {
            return Function.Call<bool>(Hash.IS_VEHICLE_SEAT_FREE, Handle, (int)seat);
        }

        #endregion

        #region Positioning

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> is considered as stopped.
        /// </summary>
        public bool IsStopped => Function.Call<bool>(Hash.IS_VEHICLE_STOPPED, Handle);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> is considered stopped.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="true"/> when the <see cref="Driver"/> <c>CVehicleIntelligence</c> is waiting for traffic lights; otherwise, <see langword="false"/>.
        /// </remarks>
        public bool IsStoppedAtTrafficLights => Function.Call<bool>(Hash.IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS, Handle);

        /// <summary>
        /// Gets a value indicating whether all <see cref="Wheels"/> of this vehicle are in contact with anything.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="true"/> when the all <see cref="Wheels"/> are in contact with anything; otherwise, <see langword="false"/>.
        /// </remarks>
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
        ///   <see langword="true" /> if peds can use this <see cref="Vehicle"/> for cover; otherwise, <see langword="false" />.
        /// </value>
        public bool ProvidesCover
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.ProvidesCoverOffset == 0)
                    return false;

                return SHVDN.MemDataMarshal.IsBitSet(address + SHVDN.NativeMemory.Vehicle.ProvidesCoverOffset, 2);
            }
            set => Function.Call(Hash.SET_VEHICLE_PROVIDES_COVER, Handle, value);
        }

        #endregion

        #region Weapons

        /// <summary>
        /// <para>
        /// Gets the current restricted ammo count for a particular vehicle weapon index on this <see cref="Vehicle"/>.
        /// When the restricted ammo count is set positive, the game will count down with every fire and prevent firing at 0.
        /// </para>
        /// <para>
        /// Not available in v1.0.877.1 or earlier game versions due to absence of members for restricted
        /// ammo counts in <c>CVehicle</c>.
        /// Currently not available in v1.0.944.2 due to absence of <c>GET_VEHICLE_WEAPON_RESTRICTED_AMMO</c>.
        /// </para>
        /// </summary>
        /// <param name="vehicleWeaponIndex">
        /// The weapon index, corresponds to each weapon slot in the vehicle's handling.meta.
        /// The valid range is between 0 and 5 since the game version v1.0.1180.2, between 0 and 4 in the game version between v1.0.944.2 and v1.0.1103.2.
        /// you can see what weapon hashes the weapon sub handling data specifies with <see cref="VehicleWeaponHandlingData.WeaponHash"/>.
        /// </param>
        /// <returns>
        /// The current restricted ammo count for specified weapon index if the <see cref="Vehicle"/> exists and the weapon index is valid
        /// (can be a negative value other than -1); otherwise, -1, which is the same as the default value set when the <see cref="Vehicle"/> just spawned.
        /// </returns>
        /// <exception cref="GameVersionNotSupportedException">
        /// Thrown if called in game versions earlier than <c>v1.0.1011.1</c>.
        /// </exception>
        public int GetRestrictedAmmoCount(int vehicleWeaponIndex)
        {
            if (Game.FileVersion < ExeVersionConsts.v1_0_1011_1)
            {
                GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_1011_1), nameof(Vehicle), nameof(GetRestrictedAmmoCount));
            }

            return Function.Call<int>(Hash.GET_VEHICLE_WEAPON_RESTRICTED_AMMO, Handle, vehicleWeaponIndex);
        }
        /// <summary>
        /// <para>
        /// Sets the current restricted ammo count for a particular vehicle weapon index on this <see cref="Vehicle"/>.
        /// </para>
        /// <para>
        /// Not available in v1.0.877.1 or earlier game versions due to absence of members for restricted
        /// ammo counts in <c>CVehicle</c>.
        /// </para>
        /// </summary>
        /// <param name="vehicleWeaponIndex">
        /// The weapon index, corresponds to each weapon slot in the vehicle's handling.meta.
        /// The valid range is between 0 and 5 since the game version v1.0.1180.2, between 0 and 4 in the game version between v1.0.944.2 and v1.0.1103.2.
        /// you can see what weapon hashes the weapon sub handling data specifies with <see cref="VehicleWeaponHandlingData.WeaponHash"/>.
        /// </param>
        /// <param name="ammoCount">
        /// When set positive, will count down with every fire and prevent firing at 0.
        /// Set -1 (or another negative value) to disable restricted ammo, which will result in the same result as when the <see cref="Vehicle"/> just spawned.
        /// </param>
        /// <exception cref="GameVersionNotSupportedException">
        /// Thrown if called in game versions earlier than <c>v1.0.944.2</c>.
        /// </exception>
        public void SetRestrictedAmmoCount(int vehicleWeaponIndex, int ammoCount)
        {
            if (Game.FileVersion < ExeVersionConsts.v1_0_944_2)
            {
                GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_944_2), nameof(Vehicle), nameof(SetRestrictedAmmoCount));
            }

            Function.Call(Hash.SET_VEHICLE_WEAPON_RESTRICTED_AMMO, Handle, vehicleWeaponIndex, ammoCount);
        }

        /// <summary>
        /// Gets or sets the current bomb ammo count on this <see cref="Vehicle"/>, which does not make the game prevent from using bombs
        /// but can be read/write across scripts.
        /// <para>
        /// Not available in v1.0.1103.2 or earlier game versions.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Unlike restricted vehicle ammo (which is game code fired), this is script-fired and manually decremented,
        /// and only stored in vehicle code for network sync purposes. Therefore, you need to manage this property in your scripts on your own.
        /// </remarks>
        public int BombAmmoCount
        {
            get
            {
                if (Game.FileVersion < ExeVersionConsts.v1_0_1180_2)
                {
                    GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_1180_2), nameof(Vehicle), nameof(GetRestrictedAmmoCount));
                }

                return Function.Call<int>(Hash.GET_VEHICLE_BOMB_AMMO, Handle);
            }
            set
            {
                if (Game.FileVersion < ExeVersionConsts.v1_0_1180_2)
                {
                    GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_1180_2), nameof(Vehicle), nameof(GetRestrictedAmmoCount));
                }

                Function.Call(Hash.SET_VEHICLE_BOMB_AMMO, Handle, value);
            }
        }

        /// <summary>
        /// Gets or sets the current countermeasure ammo count on this <see cref="Vehicle"/>, which does not make the game prevent from using bombs
        /// but can be read/write across scripts.
        /// <para>
        /// Not available in v1.0.1103.2 or earlier game versions.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Unlike restricted vehicle ammo (which is game code fired), this is script-fired and manually decremented,
        /// and only stored in vehicle code for network sync purposes. Therefore, you need to manage this property in your scripts on your own.
        /// </remarks>
        public int CountermeasureAmmoCount
        {
            get
            {
                if (Game.FileVersion < ExeVersionConsts.v1_0_1180_2)
                {
                    GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_1180_2), nameof(Vehicle), nameof(GetRestrictedAmmoCount));
                }

                return Function.Call<int>(Hash.GET_VEHICLE_COUNTERMEASURE_AMMO, Handle);
            }
            set
            {
                if (Game.FileVersion < ExeVersionConsts.v1_0_1180_2)
                {
                    GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_1180_2), nameof(Vehicle), nameof(GetRestrictedAmmoCount));
                }

                Function.Call(Hash.SET_VEHICLE_COUNTERMEASURE_AMMO, Handle, value);
            }
        }

        #endregion

        #region Vehicle Gadgets

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> has forks.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has forks; otherwise, <see langword="false" />.
        /// </value>
        public bool HasForks => Bones.Contains("forks");

        #region Cargobob

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> has a bomb bay.
        /// </summary>
        /// <returns>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has a bomb bay; otherwise, <see langword="false" />.
        /// </returns>
        public bool HasBombBay => Bones.Contains("door_hatch_l") && Bones.Contains("door_hatch_r");

        /// <summary>
        /// Opens the bomb bay of this <see cref="Vehicle"/>.
        /// </summary>
        public void OpenBombBay() => Function.Call(Hash.OPEN_BOMB_BAY_DOORS, Handle);

        /// <summary>
        /// Closes the bomb bay of this <see cref="Vehicle"/>.
        /// </summary>
        public void CloseBombBay() => Function.Call(Hash.CLOSE_BOMB_BAY_DOORS, Handle);

        /// <summary>
        /// Sets the <see cref="VehicleType.Helicopter"/> control lagging scalar, values are clamped between <see langword="0.0f"/> and <see langword="1.0f"/>
        /// </summary>
        /// <remarks>
        /// With smaller values, controlling Yaw, Pitch and Roll takes longer to blend to the desired input.
        /// </remarks>
        public void SetHeliYawPitchRollMult(float mult)
            => Function.Call(Hash.SET_HELI_CONTROL_LAGGING_RATE_SCALAR, Handle, mult);

        /// <summary>
        /// Creates a <see cref="CargobobHook"/> pickup hook if this <see cref="Vehicle"/> is a Cargobob.
        /// </summary>
        public void DropCargobobHook(CargobobHook hook)
            => Function.Call(Hash.CREATE_PICK_UP_ROPE_FOR_CARGOBOB, Handle, (int)hook);

        /// <summary>
        /// Removes the pickup hook if this <see cref="Vehicle"/> is a Cargobob.
        /// </summary>
        public void RetractCargobobHook()
            => Function.Call(Hash.REMOVE_PICK_UP_ROPE_FOR_CARGOBOB, Handle);

        /// <summary>
        /// Gets a value indicating whether this Cargobob <see cref="Vehicle"/> currently has a <see cref="CargobobHook.Hook"/> or
        /// <see cref="CargobobHook.Magnet"/> gadget.
        /// </summary>
        public bool IsCargobobHookActive()
            => Function.Call<bool>(Hash.DOES_CARGOBOB_HAVE_PICK_UP_ROPE, Handle) || Function.Call<bool>(Hash.DOES_CARGOBOB_HAVE_PICKUP_MAGNET, Handle);

        /// <summary>
        /// Gets a value indicating whether this Cargobob <see cref="Vehicle"/> currently has the specified <see cref="CargobobHook"/> gadget active.
        /// </summary>
        /// <param name="hook">The <see cref="CargobobHook"/> to check for.</param>
        /// <returns><see langword="true"/> if this Cargobob has the specified hook active; otherwise, <see langword="false"/></returns>
        public bool IsCargobobHookActive(CargobobHook hook)
        {
            switch (hook)
            {
                case CargobobHook.Hook:
                    return Function.Call<bool>(Hash.DOES_CARGOBOB_HAVE_PICK_UP_ROPE, Handle);
                case CargobobHook.Magnet:
                    return Function.Call<bool>(Hash.DOES_CARGOBOB_HAVE_PICKUP_MAGNET, Handle);
            }

            return false;
        }

        /// <summary>
        /// Activates the <see cref="CargobobHook.Magnet"/> of this Cargobob.
        /// </summary>
        public void CargoBobMagnetGrabVehicle()
        {
            if (IsCargobobHookActive(CargobobHook.Magnet))
            {
                Function.Call(Hash.SET_CARGOBOB_PICKUP_MAGNET_ACTIVE, Handle, true);
            }
        }

        /// <summary>
        /// Disables the <see cref="CargobobHook.Magnet"/> of this Cargobob.
        /// </summary>
        public void CargoBobMagnetReleaseVehicle()
        {
            if (IsCargobobHookActive(CargobobHook.Magnet))
            {
                Function.Call(Hash.SET_CARGOBOB_PICKUP_MAGNET_ACTIVE, Handle, false);
            }
        }

        #endregion

        #region Towing

        /// <summary>
        /// Gets a value indicating whether this <see cref="Vehicle"/> has tow arms.
        /// </summary>
        /// <remarks>
        /// Works by checking whether <see cref="Entity.Bones"/> of this <see cref="Vehicle"/> has the <c>"tow_arm"</c> bone.
        /// </remarks>
        /// <value>
        ///   <see langword="true" /> if this <see cref="Vehicle"/> has tow arms; otherwise, <see langword="false" />.
        /// </value>
        public bool HasTowArm => Bones.Contains("tow_arm");

        /// <summary>
        /// Sets the desired tow arm position of this <see cref="Vehicle"/>
        /// </summary>
        /// <remarks>
        /// <see langword="0.0f"/>: fully lowered; <see langword="1.0f"/>: fully raised.
        /// </remarks>
        public float TowArmPosition
        {
            set => Function.Call(Hash.SET_VEHICLE_TOW_TRUCK_ARM_POSITION, Handle, value);
        }

        /// <summary>
        /// Attaches a vehicle to this tow truck <see cref="Vehicle"/> if this <see cref="Vehicle"/> has
        /// a tow arm gadget.
        /// </summary>
        /// <param name="vehicle">The <see cref="Vehicle"/> to tow.</param>
        /// <param name="attachPointOffset">
        /// The offset from the base position of <paramref name="vehicle"/>.
        /// </param>
        /// <remarks>
        /// This method does not reposition the vehicles.
        /// Try and position the vehicle around 0.5m behind the truck if necessary.
        /// </remarks>
        public void TowVehicle(Vehicle vehicle, Vector3 attachPointOffset)
        {
            Function.Call(Hash.ATTACH_VEHICLE_TO_TOW_TRUCK, Handle, vehicle, -1,
                attachPointOffset.X, attachPointOffset.Y, attachPointOffset.Z);
        }
        /// <summary>
        /// Attaches a vehicle to this tow truck <see cref="Vehicle"/> if this <see cref="Vehicle"/> has
        /// a tow arm gadget.
        /// </summary>
        /// <param name="vehicleBone">The bone that belong to the <see cref="Vehicle"/> to tow.</param>
        /// <param name="attachPointOffset">
        /// The offset from the position of <paramref name="vehicleBone"/>.
        /// </param>
        /// <remarks>
        /// This method does not reposition the vehicles.
        /// Try and position the vehicle around 0.5m behind the truck if necessary.
        /// </remarks>
        public void TowVehicle(EntityBone vehicleBone, Vector3 attachPointOffset)
        {
            Function.Call(Hash.ATTACH_VEHICLE_TO_TOW_TRUCK, Handle, vehicleBone.Owner, vehicleBone.Index,
                attachPointOffset.X, attachPointOffset.Y, attachPointOffset.Z);
        }
        [Obsolete("Vehicle.TowVehicle(Vehicle, bool) is obsolete because the bone index parameter is incorrectly used " +
            "as a bool parameter. Use one of the other overload instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public void TowVehicle(Vehicle vehicle, bool rear)
        {
            Function.Call(Hash.ATTACH_VEHICLE_TO_TOW_TRUCK, Handle, vehicle.Handle, rear, 0f, 0f, 0f);
        }

        /// <summary>
        /// Detaches specified vehicle from any tow truck it might be attached through, loops through all vehicles so could be expensive.
        /// If you know the tow truck <see cref="Vehicle"/> that tows this <see cref="Vehicle"/>, you should call <see cref="DetachTowedVehicle"/> on the tow truck.
        /// </summary>
        /// <remarks>
        /// Although <c>DETACH_VEHICLE_FROM_ANY_TOW_TRUCK</c> returns a bool value that indicates whether the <see cref="Vehicle"/>
        /// has been detached from a tow truck, this method returns nothing in favor of the compatibility for scripts
        /// built against v3.6.0 or earlier versions.
        /// </remarks>
        public void DetachFromTowTruck()
        {
            Function.Call(Hash.DETACH_VEHICLE_FROM_ANY_TOW_TRUCK, Handle);
        }

        /// <summary>
        /// Detach the towed <see cref="Vehicle"/> to from this tow truck <see cref="Vehicle"/>.
        /// </summary>
        public void DetachTowedVehicle()
        {
            Vehicle vehicle = TowedVehicle;

            if (vehicle != null)
            {
                Function.Call(Hash.DETACH_VEHICLE_FROM_TOW_TRUCK, Handle, vehicle.Handle);
            }
        }

        /// <summary>
        /// Gets the <see cref="Vehicle"/> this tow truck <see cref="Vehicle"/> is towing.
        /// </summary>
        /// <returns>
        /// A <see cref="Vehicle"/> if this <see cref="Vehicle"/> is towing one; otherwise, <see langword="null"/>.
        /// </returns>
        public Vehicle TowedVehicle
        {
            get
            {
                int handle = Function.Call<int>(Hash.GET_ENTITY_ATTACHED_TO_TOW_TRUCK, Handle);
                return handle != 0 ? new Vehicle(handle) : null;
            }
        }

        #endregion

        #region Trailer Attach Point

        /// <summary>
        /// Gets the trailer <see cref="Vehicle"/> atleast one trailer attach point of this <see cref="Vehicle"/> is attached to.
        /// </summary>
        /// <returns>
        /// A trailer <see cref="Vehicle"/> if this <see cref="Vehicle"/> has a trailer attach point (gadget) that has
        /// a trailer <see cref="Vehicle"/>; otherwise, <see langword="null"/>.
        /// </returns>
        public Vehicle TrailerVehicle
        {
            get
            {
                unsafe
                {
                    int trailerHandle;
                    return Function.Call<bool>(Hash.GET_VEHICLE_TRAILER_VEHICLE, Handle, &trailerHandle)
                        ? new Vehicle(trailerHandle)
                        : null;
                }
            }
        }

        /// <summary>
        /// Attaches this <see cref="Vehicle"/> to a trailer using a trailer attach point instantly.
        /// </summary>
        /// <remarks>
        /// Requires a <c>CVehicleTrailerAttachPoint</c> to successfully attach the <see cref="Vehicle"/> to a trailer.
        /// </remarks>
        public void AttachToTrailer(Vehicle trailer, float inverseMassScale = 1f)
            => Function.Call(Hash.ATTACH_VEHICLE_TO_TRAILER, Handle, trailer, inverseMassScale);

        /// <summary>
        /// Detaches this <see cref="Vehicle"/> from a trailer.
        /// </summary>
        public void DetachFromTrailer()
            => Function.Call(Hash.DETACH_VEHICLE_FROM_TRAILER, Handle);

        /// <summary>
        /// Gets a value indicating whether this (truck) <see cref="Vehicle"/> is attached to a trailer <see cref="Vehicle"/>.
        /// </summary>
        public bool IsAttachedToTrailer => Function.Call<bool>(Hash.IS_VEHICLE_ATTACHED_TO_TRAILER, Handle);

        #endregion

        #region Trailer Legs

        /// <summary>
        /// Instantly raises the trailers legs, useful when attaching a trailer in script to prevent popping.
        /// </summary>
        public void SetTrailerLegsRaised() => Function.Call(Hash.SET_TRAILER_LEGS_RAISED, Handle);

        /// <summary>
        /// Instantly lowers the trailers legs of this <see cref="Vehicle"/>.
        /// <para>
        /// Currently not available in the game version earlier than v1.0.1103.2 (technically possible to backport the
        /// feature for the earlier versions).
        /// </para>
        /// </summary>
        /// <exception cref="GameVersionNotSupportedException">
        /// Thrown if called in game versions earlier than <c>v1.0.1103.2</c>.
        /// </exception>
        public void SetTrailerLegsLowered()
        {
            GameVersionNotSupportedException.ThrowIfNotSupported(ExeVersionConsts.v1_0_1103_2, nameof(Vehicle), nameof(SetTrailerLegsLowered));
            Function.Call(Hash.SET_TRAILER_LEGS_LOWERED, Handle);
        }

        #endregion

        #endregion

        /// <summary>
        /// Attaches this <see cref="Vehicle"/> on to the back of a trailer.
        /// This limits the mass of the vehicle put onto the trailer to reduce physics issues.
        /// </summary>
        /// <param name="trailer">The trailer <see cref="Vehicle"/>.</param>
        /// <param name="offset">
        /// The offset of this <see cref="Vehicle"/>. Typically left as the same value as <see cref="Vector3.Zero"/>.
        /// </param>
        /// <param name="trailerOffset">
        /// The offset of <paramref name="trailer"/>
        /// </param>
        /// <param name="rotation">
        /// The rotation in <see cref="EulerRotationOrder.YXZ"/>.
        /// </param>
        /// <param name="physicalStrength">
        /// The physical strength. Typically left as -1f.
        /// </param>
        public void AttachOnToTrailer(Vehicle trailer, Vector3 offset, Vector3 trailerOffset, Vector3 rotation, float physicalStrength)
        {
            Function.Call(Hash.ATTACH_VEHICLE_ON_TO_TRAILER, Handle, trailer, offset.X, offset.Y, offset.Z,
                trailerOffset.X, trailerOffset.Y, trailerOffset.Z, rotation.X, rotation.Y, rotation.Z, physicalStrength);
        }

        #region Task

        /// <summary>
        /// Checks if this <see cref="Vehicle"/> is being brought to a halt.
        /// Currently only supported in v1.0.1493.0.
        /// </summary>
        public bool IsBeingBroughtToHalt
            => Game.FileVersion >= ExeVersionConsts.v1_0_1493_0
            && Function.Call<bool>(Hash.IS_VEHICLE_BEING_BROUGHT_TO_HALT, Handle);

        /// <summary>
        /// Starts the task to decelerate this <see cref="Vehicle"/> until it comes to rest, possibly in an unphysically short distance.
        /// </summary>
        /// <param name="stoppingDistance">The distance from the initial coords at which the vehicle should come to rest.</param>
        /// <param name="timeToStopFor">The length of time in seconds to hold the car at rest after stopping.</param>
        /// <param name="controlVerticalVelocity">
        /// If <see langword="false" />, the task allows gravity to act normally in the Z direction.
        /// If <see langword="true" />,  the task will also arrest the car's vertical velocity.
        /// </param>
        public void BringToHalt(float stoppingDistance, int timeToStopFor, bool controlVerticalVelocity = false)
        {
            Function.Call(Hash.BRING_VEHICLE_TO_HALT, Handle, stoppingDistance, timeToStopFor, controlVerticalVelocity);
        }

        /// <summary>
        /// Stops bringing this <see cref="Vehicle"/> to a halt.
        /// Currently only supported in v1.0.1103.2.
        /// </summary>
        public void StopBringingToHalt()
        {
            if (Game.FileVersion < ExeVersionConsts.v1_0_1103_2)
            {
                GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_1103_2), nameof(Vehicle), nameof(StopBringingToHalt));
            }

            Function.Call(Hash.STOP_BRINGING_VEHICLE_TO_HALT, Handle);
        }

        /// <summary>
        /// Gets the active <see cref="VehicleMissionType"/> of this <see cref="Vehicle"/>.
        /// </summary>
        public VehicleMissionType GetActiveMissionType()
        {
            return Function.Call<VehicleMissionType>(Hash.GET_ACTIVE_VEHICLE_MISSION_TYPE, Handle);
        }
        #endregion

        #region Special Abilities

        /// <summary>
        /// Open the vehicle's parachute (if any)
        /// </summary>
        /// <param name="allowPlayerToCancel">Whether to allow the player to cancel parachuting before the vehicle lands</param>
        public void StartParachuting(bool allowPlayerToCancel)
        {
            if (Game.FileVersion < ExeVersionConsts.v1_0_944_2)
            {
                GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_944_2), nameof(Vehicle), nameof(StartParachuting));
            }

            if (HasParachute)
            {
                Function.Call((Hash)0x0BFFB028B3DD0A97, Handle, allowPlayerToCancel);
            }
        }

        /// <summary>
        /// Completely enables or disables this <see cref="Vehicle"/>'s flight mode, used for <see cref="VehicleHash.Deluxo"/> and <see cref="VehicleHash.Oppressor2"/>.
        /// Only available in v1.0.1290.1 or later.
        /// </summary>
        /// <param name="allowed">Whether to allow the <see cref="Vehicle"/> to switch to flight mode or not.</param>
        public void SetSpecialFlightModeAllowed(bool allowed)
        {
            GameVersionNotSupportedException.ThrowIfNotSupported(ExeVersionConsts.v1_0_1290_1, nameof(Vehicle), nameof(SetSpecialFlightModeAllowed));

            Function.Call(Hash.SET_SPECIAL_FLIGHT_MODE_ALLOWED, Handle, allowed);
        }

        /// <summary>
        /// Gets or sets the current ratio indicating how much the <see cref="Vehicle"/> is transformed to hover mode for special flight mode,
        /// which is used for <see cref="VehicleHash.Deluxo"/> and <see cref="VehicleHash.Oppressor2"/>.
        /// Only available in v1.0.1290.1 or later.
        /// </summary>
        /// <value>The target ratio indicating how much the <see cref="Vehicle"/> will be transformed into hover mode from <c>0f</c> to <c>1f</c>.</value>
        public float SpecialFlightModeTargetRatio
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.SpecialFlightTargetRatioOffset == 0)
                    return 0f;

                return SHVDN.MemDataMarshal.ReadFloat(MemoryAddress + SHVDN.NativeMemory.Vehicle.SpecialFlightTargetRatioOffset);
            }
            set
            {
                if (Game.FileVersion < ExeVersionConsts.v1_0_1290_1)
                {
                    GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_1290_1), nameof(Vehicle), nameof(SpecialFlightModeTargetRatio));
                }

                Function.Call(Hash.SET_SPECIAL_FLIGHT_MODE_TARGET_RATIO, Handle, value);
            }
        }

        /// <summary>
        /// Gets or sets the current ratio indicating how much the <see cref="Vehicle"/> is transformed to hover mode for special flight mode,
        /// which is used for <see cref="VehicleHash.Deluxo"/> and <see cref="VehicleHash.Oppressor2"/>.
        /// Only available in v1.0.1290.1 or later.
        /// </summary>
        /// <value>The current ratio indicating how much the <see cref="Vehicle"/> is transformed to hover mode from <c>0f</c> to <c>1f</c>.</value>
        public float SpecialFlightModeCurrentRatio
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.SpecialFlightCurrentRatioOffset == 0)
                    return 0f;

                return SHVDN.MemDataMarshal.ReadFloat(MemoryAddress + SHVDN.NativeMemory.Vehicle.SpecialFlightCurrentRatioOffset);
            }
            set
            {
                if (Game.FileVersion < ExeVersionConsts.v1_0_1290_1)
                {
                    GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_1290_1), nameof(Vehicle), nameof(SpecialFlightModeCurrentRatio));
                }

                Function.Call(Hash.SET_SPECIAL_FLIGHT_MODE_RATIO, Handle, value);
            }
        }

        /// <summary>
        /// Gets or sets the ratio indicating how much wings are deployed for the special flight mode,
        /// which is used for <see cref="VehicleHash.Deluxo"/> and <see cref="VehicleHash.Oppressor2"/>.
        /// Only available in v1.0.1290.1 or later.
        /// </summary>
        /// <value>The ratio indicating how much wings are deployed for the special flight mode from <c>0f</c> to <c>1f</c>.</value>
        public float SpecialFlightModeWingRatio
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.SpecialFlightWingRatioOffset == 0)
                    return 0f;

                return SHVDN.MemDataMarshal.ReadFloat(MemoryAddress + SHVDN.NativeMemory.Vehicle.SpecialFlightWingRatioOffset);
            }
            set
            {
                if (Game.FileVersion < ExeVersionConsts.v1_0_1290_1)
                {
                    GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_1290_1), nameof(Vehicle), nameof(SpecialFlightModeWingRatio));
                }

                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.SpecialFlightAreWingsDisabledOffset == 0)


                    return;

                SHVDN.MemDataMarshal.WriteFloat(MemoryAddress + SHVDN.NativeMemory.Vehicle.SpecialFlightWingRatioOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether wings are enabled for the special flight mode,
        /// which is used for <see cref="VehicleHash.Deluxo"/> and <see cref="VehicleHash.Oppressor2"/>.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="false"/> prior to <c>v1.0.1290.1</c>;
        /// </remarks>
        /// <exception cref="GameVersionNotSupportedException">
        /// Thrown if called in game versions earlier than <c>v1.0.1290.1</c>.
        /// </exception>
        public bool AreWingsEnabledForSpecialFlightMode
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.SpecialFlightAreWingsDisabledOffset == 0)
                    return false;

                return SHVDN.MemDataMarshal.ReadByte(MemoryAddress + SHVDN.NativeMemory.Vehicle.SpecialFlightAreWingsDisabledOffset) != 0;
            }
            set
            {
                if (Game.FileVersion < ExeVersionConsts.v1_0_1290_1)
                {
                    GameVersionNotSupportedException.ThrowIfNotSupported((ExeVersionConsts.v1_0_1290_1), nameof(Vehicle), nameof(AreWingsEnabledForSpecialFlightMode));
                }

                Function.Call(Hash.SET_DISABLE_HOVER_MODE_FLIGHT, Handle, !value);
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
        public static string GetModelMakeName(Model vehicleModel)
        {
            return SHVDN.NativeMemory.GetVehicleMakeName(vehicleModel.Hash);
        }

        public static VehicleClass GetModelClass(Model vehicleModel)
        {
            return Function.Call<VehicleClass>(Hash.GET_VEHICLE_CLASS_FROM_NAME, vehicleModel.Hash);
        }
        public static VehicleType GetModelType(Model vehicleModel)
        {
            return (VehicleType)SHVDN.NativeMemory.GetVehicleType(vehicleModel);
        }

        /// <summary>
        /// Gets an array of all model values.
        /// </summary>
        public static int[] GetAllModelValues()
        {
            var allModels = new List<int>();
            for (int i = 0; i < 0x20; i++)
            {
                allModels.AddRange(SHVDN.NativeMemory.VehicleModels[i].ToArray());
            }
            return allModels.ToArray();
        }

        /// <summary>
        /// Gets an array of all <see cref="VehicleHash"/>es.
        /// </summary>
        public static VehicleHash[] GetAllModels()
        {
            var allModels = new List<VehicleHash>();
            for (int i = 0; i < 0x20; i++)
            {
                allModels.AddRange(Array.ConvertAll(SHVDN.NativeMemory.VehicleModels[i].ToArray(), item => (VehicleHash)item));
            }
            return allModels.ToArray();
        }

        /// <summary>
        /// Gets an array of all loaded <see cref="VehicleHash"/>es that is appropriate to spawn as ambient vehicles.
        /// All the model hashes of the elements are loaded and the <see cref="Vehicle"/>s with the model hashes can be spawned immediately.
        /// </summary>
        public static VehicleHash[] GetAllLoadedModelsAppropriateForAmbientVehicles()
        {
            return SHVDN.NativeMemory.GetLoadedAppropriateVehicleHashes()
                .Select(x => (VehicleHash)x)
                .ToArray();
        }

        /// <summary>
        /// Gets an array of all <see cref="VehicleHash"/>es whose <see cref="VehicleClass"/>es belong to the specified one.
        /// </summary>
        public static VehicleHash[] GetAllModelsOfClass(VehicleClass vehicleClass)
        {
            return Array.ConvertAll(SHVDN.NativeMemory.VehicleModels[(int)vehicleClass].ToArray(), item => (VehicleHash)item);
        }

        /// <summary>
        /// Gets an array of all <see cref="VehicleHash"/>es whose <see cref="VehicleType"/>es belong to the specified one.
        /// </summary>
        public static VehicleHash[] GetAllModelsOfType(VehicleType vehicleType)
        {
            return Array.ConvertAll(SHVDN.NativeMemory.VehicleModelsGroupedByType[(int)vehicleType].ToArray(), item => (VehicleHash)item);
        }
    }
}
