//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public struct Model : IEquatable<Model>, INativeValue, IScriptStreamingResource
	{
		public Model(int hash) : this()
		{
			Hash = hash;
		}
		public Model(string name) : this(Game.GenerateHash(name))
		{
		}
		public Model(PedHash hash) : this((int)hash)
		{
		}
		public Model(WeaponHash hash) : this((int)hash)
		{
		}
		public Model(VehicleHash hash) : this((int)hash)
		{
		}

		/// <summary>
		/// Gets the hash for this <see cref="Model"/>.
		/// </summary>
		public int Hash
		{
			get; private set;
		}

		/// <summary>
		/// Gets the native representation of this <see cref="Model"/>.
		/// </summary>
		public ulong NativeValue
		{
			get => (ulong)Hash;
			set => Hash = unchecked((int)value);
		}

		/// <summary>
		/// Gets a value that indicates whether this <see cref="Model"/> exists.
		/// This property is practically the same as <see cref="IsInCdImage"/> since an additional check this method does does not work in practice.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Model"/> is valid; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>
		/// You may want to use <see cref="IsInCdImage"/> to avoid a redundant additional check,
		/// where <c>!(~(((modelIndex | 0xFFF0000) &amp; 0xEFFFFFFF) &lt;&lt; 28) &amp; 1)</c> is evaluated (but always evaluated as false in the range of uint32_t)
		/// after checking if the model index is not <c>0xFFFF</c> (which means the index could not found) just like <see cref="IsInCdImage"/> does.
		/// </remarks>
		public bool IsValid => Function.Call<bool>(Native.Hash.IS_MODEL_VALID, Hash);
		/// <summary>
		/// Gets a value that indicates whether this <see cref="Model"/> is in the CD image.
		/// This property is practically the same as <see cref="IsValid"/> since its additional check does not work in practice.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is in the CD image; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>
		/// The hash of a streamed archetype might only be valid at certain locations on the map.
		/// </remarks>
		public bool IsInCdImage => Function.Call<bool>(Native.Hash.IS_MODEL_IN_CDIMAGE, Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is loaded so it can be spawned.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Model"/> is loaded; otherwise, <see langword="false" />.
		/// </value>
		public bool IsLoaded => Function.Call<bool>(Native.Hash.HAS_MODEL_LOADED, Hash);
		/// <summary>
		/// Gets a value indicating whether the collision for this <see cref="Model"/> is loaded.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the collision is loaded; otherwise, <see langword="false" />.
		/// </value>
		public bool IsCollisionLoaded => Function.Call<bool>(Native.Hash.HAS_COLLISION_FOR_MODEL_LOADED, Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is an amphibious car.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is an amphibious car; otherwise, <see langword="false" />.
		/// </value>
		public bool IsAmphibiousCar => Game.Version >= GameVersion.v1_0_944_2_Steam && Function.Call<bool>(Native.Hash.IS_THIS_MODEL_AN_AMPHIBIOUS_CAR, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is an amphibious quad bike.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is an amphibious quad bike; otherwise, <see langword="false" />.
		/// </value>
		public bool IsAmphibiousQuadBike => Game.Version >= GameVersion.v1_0_944_2_Steam && Function.Call<bool>(Native.Hash.IS_THIS_MODEL_AN_AMPHIBIOUS_QUADBIKE, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is an amphibious vehicle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is an amphibious vehicle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsAmphibiousVehicle => IsAmphibiousCar || IsAmphibiousQuadBike;
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a animal pedestrian.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a animal pedestrian; otherwise, <see langword="false" />.
		/// </value>
		public bool IsAnimalPed => SHVDN.NativeMemory.IsModelAnAnimalPed(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a bicycle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a bicycle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsBicycle => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BICYCLE, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a big vehicle whose vehicle flag has "FLAG_BIG".
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a big vehicle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsBigVehicle => SHVDN.NativeMemory.HasVehicleFlag(Hash, SHVDN.NativeMemory.VehicleFlag1.Big);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a bike (either a motorcycle or a bicycle).
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a bike; otherwise, <see langword="false" />.
		/// </value>
		public bool IsBike => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BIKE, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a blimp.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a blimp; otherwise, <see langword="false" />.
		/// </value>
		public bool IsBlimp => SHVDN.NativeMemory.IsModelABlimp(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a boat.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a boat; otherwise, <see langword="false" />.
		/// </value>
		public bool IsBoat => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BOAT, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is an emergency vehicle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is an emergency vehicle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsBus => SHVDN.NativeMemory.HasVehicleFlag(Hash, SHVDN.NativeMemory.VehicleFlag1.IsBus);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a car and not an amphibious one.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a car and not an amphibious one; otherwise, <see langword="false" />.
		/// </value>
		public bool IsCar => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_CAR, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a cargobob.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a cargobob; otherwise, <see langword="false" />.
		/// </value>
		public bool IsCargobob => (VehicleHash)Hash == VehicleHash.Cargobob || (VehicleHash)Hash == VehicleHash.Cargobob2 || (VehicleHash)Hash == VehicleHash.Cargobob3 || (VehicleHash)Hash == VehicleHash.Cargobob4;
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a donk car.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a donk car; otherwise, <see langword="false" />.
		/// </value>
		public bool IsDonk => SHVDN.NativeMemory.HasVehicleFlag(Hash, SHVDN.NativeMemory.VehicleFlag2.HasLowriderDonkHydraulics);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is an electric vehicle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is an electric vehicle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsElectricVehicle => SHVDN.NativeMemory.HasVehicleFlag(Hash, SHVDN.NativeMemory.VehicleFlag1.IsElectric);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is an emergency vehicle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is an emergency vehicle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsEmergencyVehicle => SHVDN.NativeMemory.HasVehicleFlag(Hash, SHVDN.NativeMemory.VehicleFlag1.EmergencyService);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a female pedestrian.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a female pedestrian; otherwise, <see langword="false" />.
		/// </value>
		public bool IsFemalePed => SHVDN.NativeMemory.IsModelAFemalePed(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a fragment model.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a fragment model; otherwise, <see langword="false" />.
		/// </value>
		public bool IsFragment => SHVDN.NativeMemory.IsModelAFragment(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a gangster pedestrian.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a gangster pedestrian; otherwise, <see langword="false" />.
		/// </value>
		public bool IsGangPed => SHVDN.NativeMemory.IsModelAGangPed(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a helicopter.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a helicopter; otherwise, <see langword="false" />.
		/// </value>
		public bool IsHelicopter => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_HELI, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a human pedestrian.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a human pedestrian; otherwise, <see langword="false" />.
		/// </value>
		public bool IsHumanPed => SHVDN.NativeMemory.IsModelHumanPed(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a jet ski.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a jet ski; otherwise, <see langword="false" />.
		/// </value>
		public bool IsJetSki => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_JETSKI, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a law enforcement vehicle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a law enforcement vehicle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsLawEnforcementVehicle => SHVDN.NativeMemory.HasVehicleFlag(Hash, SHVDN.NativeMemory.VehicleFlag1.LawEnforcement);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a regular lowrider.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a regular lowrider; otherwise, <see langword="false" />.
		/// </value>
		public bool IsLowrider => SHVDN.NativeMemory.HasVehicleFlag(Hash, SHVDN.NativeMemory.VehicleFlag2.HasLowriderHydraulics);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a movable interior loader (also known as MLO or MILO).
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a car; otherwise, <see langword="false" />.
		/// </value>
		public bool IsMlo => SHVDN.NativeMemory.IsModelAMlo(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a male pedestrian.
		/// Without modding <c>pedpersonality.ymt</c>, returns <see langword="true" /> if the <see cref="Hash"/> is one of the animal hashes.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a male pedestrian; otherwise, <see langword="false" />.
		/// </value>
		public bool IsMalePed => SHVDN.NativeMemory.IsModelAMalePed(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a motorcycle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a motorcycle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsMotorcycle => SHVDN.NativeMemory.IsModelAMotorcycle(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is an off-road vehicle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is an off-road vehicle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsOffRoadVehicle => SHVDN.NativeMemory.HasVehicleFlag(Hash, SHVDN.NativeMemory.VehicleFlag1.IsOffroadVehicle);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a pedestrian.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a pedestrian; otherwise, <see langword="false" />.
		/// </value>
		public bool IsPed => SHVDN.NativeMemory.IsModelAPed(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a plane.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a plane; otherwise, <see langword="false" />.
		/// </value>
		public bool IsPlane => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_PLANE, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a prop.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a prop; otherwise, <see langword="false" />.
		/// </value>
		public bool IsProp => IsValid && !IsPed && !IsVehicle;
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a quad bike and not an amphibious one.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a quad bike and not an amphibious one; otherwise, <see langword="false" />.
		/// </value>
		public bool IsQuadBike => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_QUADBIKE, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a submarine.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is an submarine; otherwise, <see langword="false" />.
		/// </value>
		public bool IsSubmarine => SHVDN.NativeMemory.IsModelASubmarine(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a submarine car.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is an submarine car; otherwise, <see langword="false" />.
		/// </value>
		public bool IsSubmarineCar => SHVDN.NativeMemory.IsModelASubmarineCar(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a tank.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a tank; otherwise, <see langword="false" />.
		/// </value>
		public bool IsTank => SHVDN.NativeMemory.HasVehicleFlag(Hash, SHVDN.NativeMemory.VehicleFlag2.IsTank);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a train.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a train; otherwise, <see langword="false" />.
		/// </value>
		public bool IsTrain => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_TRAIN, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a trailer.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a trailer; otherwise, <see langword="false" />.
		/// </value>
		public bool IsTrailer => SHVDN.NativeMemory.IsModelATrailer(Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a van.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a van; otherwise, <see langword="false" />.
		/// </value>
		public bool IsVan => SHVDN.NativeMemory.HasVehicleFlag(Hash, SHVDN.NativeMemory.VehicleFlag1.IsVan);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a vehicle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a vehicle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsVehicle => Function.Call<bool>(Native.Hash.IS_MODEL_A_VEHICLE, Hash);

		/// <summary>
		/// Gets the dimensions of this <see cref="Model"/>.
		/// </summary>
		/// <returns>
		/// <c>rearBottomLeft</c> is the minimum dimensions, which contains the rear bottom left relative offset from the origin of the model,
		/// <c>frontTopRight</c> is the maximum dimensions, which contains the front top right relative offset from the origin of the model.
		/// </returns>
		/// <remarks>
		/// When you need to fetch dimensions info from large amount of <see cref="Model"/>s in a short time,
		/// it may be better to use <see cref="GetDimensions(out Vector3, out Vector3)"/> instead because creating <see cref="ValueTuple"/> costs much (about 10x) more than
		/// using out parameters in .NET Framework and this property creates a <see cref="ValueTuple"/> instance.
		/// </remarks>
		public (Vector3 rearBottomLeft, Vector3 frontTopRight) Dimensions
		{
			get
			{
				GetDimensions(out Vector3 rearBottomLeft, out Vector3 frontTopRight);
				return (rearBottomLeft, frontTopRight);
			}
		}

		/// <summary>
		/// <para>Gets the dimensions of this <see cref="Model"/>.</para>
		/// <para>
		/// When you need to fetch dimensions info from large amount of <see cref="Model"/>s in a short time, it may be better to use this method instead of <see cref="Dimensions"/>
		/// because creating <see cref="ValueTuple"/> costs much (about 10x) more than using out parameters in .NET Framework.
		/// </para>
		/// </summary>
		/// <param name="min">The minimum offset, a.k.a. the rear bottom left relative offset, from the origin of the model.</param>
		/// <param name="max">The maximum offset, a.k.a. the front top right relative offset, from the origin of the model.</param>
		public void GetDimensions(out Vector3 min, out Vector3 max)
		{
			NativeVector3 retMin, retMax;
			unsafe
			{
				Function.Call(Native.Hash.GET_MODEL_DIMENSIONS, Hash, &retMin, &retMax);
			}

			min = retMin;
			max = retMax;
		}

		/// <summary>
		/// Attempts to load this <see cref="Model"/> into memory.
		/// </summary>
		public void Request()
		{
			Function.Call(Native.Hash.REQUEST_MODEL, Hash);
		}
		/// <summary>
		/// Attempts to load this <see cref="Model"/> into memory for a given period of time.
		/// </summary>
		/// <param name="timeout">The time (in milliseconds) before giving up trying to load this <see cref="Model"/>.</param>
		/// <returns><see langword="true" /> if this <see cref="Model"/> is loaded; otherwise, <see langword="false" />.</returns>
		public bool Request(int timeout)
		{
			Request();

			int startTime = Environment.TickCount;
			int maxElapsedTime = timeout >= 0 ? timeout : int.MaxValue;

			while (!IsLoaded)
			{
				Script.Yield();
				Request();

				if (Environment.TickCount - startTime >= maxElapsedTime)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Attempts to load this <see cref="Model"/>'s collision into memory.
		/// </summary>
		public void RequestCollision()
		{
			Function.Call(Native.Hash.REQUEST_COLLISION_FOR_MODEL, Hash);
		}
		/// <summary>
		/// Attempts to load this <see cref="Model"/>'s collision into memory for a given period of time.
		/// </summary>
		/// <param name="timeout">The time (in milliseconds) before giving up trying to load this <see cref="Model"/>.</param>
		/// <returns><see langword="true" /> if this <see cref="Model"/>'s collision is loaded; otherwise, <see langword="false" />.</returns>
		public bool RequestCollision(int timeout)
		{
			Request();

			int startTime = Environment.TickCount;
			int maxElapsedTime = timeout >= 0 ? timeout : int.MaxValue;

			while (!IsLoaded)
			{
				Script.Yield();
				RequestCollision();

				if (Environment.TickCount - startTime >= maxElapsedTime)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Tells the game we have finished using this <see cref="Model"/> and it can be freed from memory.
		/// </summary>
		public void MarkAsNoLongerNeeded()
		{
			Function.Call(Native.Hash.SET_MODEL_AS_NO_LONGER_NEEDED, Hash);
		}

		public bool Equals(Model model)
		{
			return Hash == model.Hash;
		}
		public override bool Equals(object obj)
		{
			if (obj is Model model)
			{
				return Equals(model);
			}

			return false;
		}

		public static bool operator ==(Model left, Model right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Model left, Model right)
		{
			return !left.Equals(right);
		}

		public static implicit operator int(Model source)
		{
			return source.Hash;
		}
		public static implicit operator PedHash(Model source)
		{
			return (PedHash)source.Hash;
		}
		public static implicit operator WeaponHash(Model source)
		{
			return (WeaponHash)source.Hash;
		}
		public static implicit operator VehicleHash(Model source)
		{
			return (VehicleHash)source.Hash;
		}

		public static implicit operator Model(int source)
		{
			return new Model(source);
		}
		public static implicit operator Model(string source)
		{
			return new Model(source);
		}
		public static implicit operator Model(PedHash source)
		{
			return new Model(source);
		}
		public static implicit operator Model(WeaponHash source)
		{
			return new Model(source);
		}
		public static implicit operator Model(VehicleHash source)
		{
			return new Model(source);
		}

		public static implicit operator InputArgument(Model value)
		{
			return new InputArgument((ulong)value.Hash);
		}

		public override int GetHashCode()
		{
			return Hash;
		}

		public override string ToString()
		{
			return "0x" + Hash.ToString("X");
		}
	}
}
