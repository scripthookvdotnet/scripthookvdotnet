using System;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	public struct Model : IEquatable<Model>, INativeValue
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
		public Model(VehicleHash hash) : this((int)hash)
		{
		}
		public Model(WeaponHash hash) : this((int)hash)
		{
		}

		public ulong NativeValue
		{
			get
			{
				return (ulong)Hash;
			}
			set
			{
				Hash = unchecked((int)value);
			}
		}

		/// <summary>
		/// Gets the hash for this <see cref="Model"/>.
		/// </summary>
		public int Hash { get; private set; }

		/// <summary>
		/// Returns true if this <see cref="Model"/> is valid.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Model"/> is valid; otherwise, <c>false</c>.
		/// </value>
		public bool IsValid
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_MODEL_VALID, Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is in the cd image.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is in the cd image; otherwise, <c>false</c>.
		/// </value>
		public bool IsInCdImage
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_MODEL_IN_CDIMAGE, Hash);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is loaded so it can be spawned.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Model"/> is loaded; otherwise, <c>false</c>.
		/// </value>
		public bool IsLoaded
		{
			get
			{
				return Function.Call<bool>(Native.Hash.HAS_MODEL_LOADED, Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether the collision for this <see cref="Model"/> is loaded.
		/// </summary>
		/// <value>
		/// <c>true</c> if the collision is loaded; otherwise, <c>false</c>.
		/// </value>
		public bool IsCollisionLoaded
		{
			get
			{
				return Function.Call<bool>(Native.Hash.HAS_COLLISION_FOR_MODEL_LOADED, Hash);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a bicycle.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a bicycle; otherwise, <c>false</c>.
		/// </value>
		public bool IsBicycle
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BICYCLE, Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a motorbike.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a motorbike; otherwise, <c>false</c>.
		/// </value>
		public bool IsBike
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BIKE, Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a boat.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a boat; otherwise, <c>false</c>.
		/// </value>
		public bool IsBoat
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BOAT, Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a car.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a car; otherwise, <c>false</c>.
		/// </value>
		public bool IsCar
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_CAR, Hash);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is an amphibious car.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is an amphibious car; otherwise, <c>false</c>.
		/// </value>
		public bool IsAmphibiousCar
		{
			get
			{
				if (Game.Version >= GameVersion.v1_0_944_2_Steam)
				{
					return Function.Call<bool>((Native.Hash)0x633F6F44A537EBB6, Hash);
				}

				return false;
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a blimp.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a blimp; otherwise, <c>false</c>.
		/// </value>
		public bool IsBlimp
		{
			get
			{
				return MemoryAccess.IsModelABlimp(Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a cargobob.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a cargobob; otherwise, <c>false</c>.
		/// </value>
		public bool IsCargobob
		{
			get
			{
				VehicleHash hash = (VehicleHash)Hash;
				return hash == VehicleHash.Cargobob || hash == VehicleHash.Cargobob2 || hash == VehicleHash.Cargobob3 || hash == VehicleHash.Cargobob4;
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a helicopter.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a helicopter; otherwise, <c>false</c>.
		/// </value>
		public bool IsHelicopter
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_HELI, Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a jet ski.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a jet ski; otherwise, <c>false</c>.
		/// </value>
		public bool IsJetSki
		{
			get
			{
				return Function.Call<bool>(Native.Hash._IS_THIS_MODEL_A_JETSKI, Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a ped.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a ped; otherwise, <c>false</c>.
		/// </value>
		public bool IsPed
		{
			get
			{
				return MemoryAccess.IsModelAPed(Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a plane.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a plane; otherwise, <c>false</c>.
		/// </value>
		public bool IsPlane
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_PLANE, Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a prop.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a prop; otherwise, <c>false</c>.
		/// </value>
		public bool IsProp
		{
			get
			{
				return IsValid && !IsPed && !IsVehicle;
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a quad bike.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a quadbike; otherwise, <c>false</c>.
		/// </value>
		public bool IsQuadBike
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_QUADBIKE, Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is an amphibious quad bike.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is an amphibious quadbike; otherwise, <c>false</c>.
		/// </value>
		public bool IsAmphibiousQuadBike
		{
			get
			{
				if (Game.Version >= GameVersion.v1_0_944_2_Steam)
				{
					return MemoryAccess.IsModelAnAmphibiousQuadBike(Hash);
				}

				return false;
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a train.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a train; otherwise, <c>false</c>.
		/// </value>
		public bool IsTrain
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_TRAIN, Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a trailer.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a trailer; otherwise, <c>false</c>.
		/// </value>
		public bool IsTrailer
		{
			get
			{
				return MemoryAccess.IsModelATrailer(Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a vehicle.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is a vehicle; otherwise, <c>false</c>.
		/// </value>
		public bool IsVehicle
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_MODEL_A_VEHICLE, Hash);
			}
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is an amphibious vehicle.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Model"/> is an amphibious vehicle; otherwise, <c>false</c>.
		/// </value>
		public bool IsAmphibiousVehicle
		{
			get
			{
				if (Game.Version >= GameVersion.v1_0_944_2_Steam)
				{
					return IsAmphibiousCar || IsAmphibiousQuadBike;
				}

				return false;
			}
		}

		/// <summary>
		/// Gets the dimensions of this <see cref="Model"/>.
		/// </summary>
		/// <returns>
		/// rearBottomLeft is the minimum dimensions, which contains the rear bottom left relative offset from the origin of the model,
		///  frontTopRight is the maximum dimensions, which contains the front top right relative offset from the origin of the model.
		/// </returns>
		public (Vector3 rearBottomLeft, Vector3 frontTopRight) Dimensions
		{
			get
			{
				NativeVector3 min, max;
				unsafe
				{
					Function.Call(Native.Hash.GET_MODEL_DIMENSIONS, Hash, &min, &max);
				}

				return (min, max);
			}
		}

		/// <summary>
		/// Attempt to load this <see cref="Model"/> into memory.
		/// </summary>
		public void Request()
		{
			Function.Call(Native.Hash.REQUEST_MODEL, Hash);
		}
		/// <summary>
		/// Attempt to load this <see cref="Model"/> into memory for a given period of time.
		/// </summary>
		/// <param name="timeout">The time (in milliseconds) before giving up trying to load this <see cref="Model"/></param>
		/// <returns><c>true</c> if this <see cref="Model"/> is loaded; otherwise, <c>false</c></returns>
		public bool Request(int timeout)
		{
			Request();

			DateTime endtime = timeout >= 0 ? DateTime.UtcNow + new TimeSpan(0, 0, 0, 0, timeout) : DateTime.MaxValue;

			while (!IsLoaded)
			{
				Script.Yield();
				Request();

				if (DateTime.UtcNow >= endtime)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Attempt to load this <see cref="Model"/>'s collision into memory.
		/// </summary>
		public void RequestCollision()
		{
			Function.Call(Native.Hash.REQUEST_COLLISION_FOR_MODEL, Hash);
		}
		/// <summary>
		/// Attempt to load this <see cref="Model"/>'s collision into memory for a given period of time.
		/// </summary>
		/// <param name="timeout">The time (in milliseconds) before giving up trying to load this <see cref="Model"/></param>
		/// <returns><c>true</c> if this <see cref="Model"/>'s collision is loaded; otherwise, <c>false</c></returns>
		public bool RequestCollision(int timeout)
		{
			Request();

			DateTime endtime = timeout >= 0 ? DateTime.UtcNow + new TimeSpan(0, 0, 0, 0, timeout) : DateTime.MaxValue;

			while (!IsLoaded)
			{
				Script.Yield();
				RequestCollision();

				if (DateTime.UtcNow >= endtime)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Frees this <see cref="Model"/> from memory.
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
			return obj != null && Equals((Model)obj);
		}

		public override int GetHashCode()
		{
			return Hash;
		}
		public override string ToString()
		{
			return "0x" + Hash.ToString("X");
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
		public static implicit operator Model(VehicleHash source)
		{
			return new Model(source);
		}
		public static implicit operator Model(WeaponHash source)
		{
			return new Model(source);
		}

		public static implicit operator int(Model source)
		{
			return source.Hash;
		}
		public static implicit operator PedHash(Model source)
		{
			return (PedHash)source.Hash;
		}
		public static implicit operator VehicleHash(Model source)
		{
			return (VehicleHash)source.Hash;
		}
		public static implicit operator WeaponHash(Model source)
		{
			return (WeaponHash)source.Hash;
		}

		public static bool operator ==(Model left, Model right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Model left, Model right)
		{
			return !left.Equals(right);
		}
	}
}
