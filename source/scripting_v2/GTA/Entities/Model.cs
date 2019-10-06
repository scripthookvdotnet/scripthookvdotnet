//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public struct Model : IEquatable<Model>
	{
		public Model(int hash)
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

		public int Hash
		{
			get; private set;
		}

		public bool IsValid => Function.Call<bool>(Native.Hash.IS_MODEL_VALID, Hash);
		public bool IsInCdImage => Function.Call<bool>(Native.Hash.IS_MODEL_IN_CDIMAGE, Hash);

		public bool IsLoaded => Function.Call<bool>(Native.Hash.HAS_MODEL_LOADED, Hash);
		public bool IsCollisionLoaded => Function.Call<bool>(Native.Hash.HAS_COLLISION_FOR_MODEL_LOADED, Hash);

		public bool IsBicycle => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BICYCLE, Hash);
		public bool IsBike => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BIKE, Hash);
		public bool IsBoat => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BOAT, Hash);
		public bool IsCar => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_CAR, Hash);
		public bool IsCargobob => (VehicleHash)Hash == VehicleHash.Cargobob || (VehicleHash)Hash == VehicleHash.Cargobob2 || (VehicleHash)Hash == VehicleHash.Cargobob3 || (VehicleHash)Hash == VehicleHash.Cargobob4;
		public bool IsHelicopter => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_HELI, Hash);
		public bool IsPed => SHVDN.NativeMemory.IsModelAPed(Hash);
		public bool IsPlane => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_PLANE, Hash);
		public bool IsQuadbike => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_QUADBIKE, Hash);
		public bool IsTrain => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_TRAIN, Hash);
		public bool IsVehicle => Function.Call<bool>(Native.Hash.IS_MODEL_A_VEHICLE, Hash);

		public void GetDimensions(out Vector3 minimum, out Vector3 maximum)
		{
			var outmin = new OutputArgument();
			var outmax = new OutputArgument();
			Function.Call(Native.Hash.GET_MODEL_DIMENSIONS, Hash, outmin, outmax);
			minimum = outmin.GetResult<Vector3>();
			maximum = outmax.GetResult<Vector3>();
		}
		public Vector3 GetDimensions()
		{
			GetDimensions(out Vector3 min, out Vector3 max);
			return Vector3.Subtract(max, min);
		}

		public void Request()
		{
			Function.Call(Native.Hash.REQUEST_MODEL, Hash);
		}
		public bool Request(int timeout)
		{
			Request();

			DateTime endtime = timeout >= 0 ? DateTime.UtcNow + new TimeSpan(0, 0, 0, 0, timeout) : DateTime.MaxValue;

			while (!IsLoaded)
			{
				Script.Yield();

				if (DateTime.UtcNow >= endtime)
					return false;
			}

			return true;
		}

		public void MarkAsNoLongerNeeded()
		{
			Function.Call(Native.Hash.SET_MODEL_AS_NO_LONGER_NEEDED, Hash);
		}

		public bool Equals(Model obj)
		{
			return Hash == obj.Hash;
		}
		public override bool Equals(object obj)
		{
			return obj != null && obj.GetType() == GetType() && Equals((Model)obj);
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
