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

		public int Hash { get; private set; }

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

		public bool IsValid
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_MODEL_VALID, Hash);
			}
		}
		public bool IsInCdImage
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_MODEL_IN_CDIMAGE, Hash);
			}
		}
		public bool IsLoaded
		{
			get
			{
				return Function.Call<bool>(Native.Hash.HAS_MODEL_LOADED, Hash);
			}
		}
		public bool IsCollisionLoaded
		{
			get
			{
				return Function.Call<bool>(Native.Hash.HAS_COLLISION_FOR_MODEL_LOADED, Hash);
			}
		}

		public bool IsBicycle
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BICYCLE, Hash);
			}
		}
		public bool IsBike
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BIKE, Hash);
			}
		}
		public bool IsBoat
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BOAT, Hash);
			}
		}
		public bool IsCar
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_CAR, Hash);
			}
		}
		public bool IsCargobob
		{
			get
			{
				VehicleHash hash = (VehicleHash)Hash;
				return hash == VehicleHash.Cargobob || hash == VehicleHash.Cargobob2 || hash == VehicleHash.Cargobob3 || hash == VehicleHash.Cargobob4;
			}
		}
		public bool IsHelicopter
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_HELI, Hash);
			}
		}
		public bool IsPed
		{
			get
			{
				return MemoryAccess.IsModelAPed(Hash);
			}
		}
		public bool IsPlane
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_PLANE, Hash);
			}
		}
		public bool IsProp
		{
			get
			{
				return IsValid && !IsPed && !IsVehicle;
			}
		}
		public bool IsQuadbike
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_QUADBIKE, Hash);
			}
		}
		public bool IsTrain
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_TRAIN, Hash);
			}
		}
		public bool IsVehicle
		{
			get
			{
				return Function.Call<bool>(Native.Hash.IS_MODEL_A_VEHICLE, Hash);
			}
		}

		public Vector3 GetDimensions()
		{
			Vector3 right, left;
			GetDimensions(out right, out left);

			return Vector3.Subtract(left, right);
		}
		public void GetDimensions(out Vector3 minimum, out Vector3 maximum)
		{
			var minimumArg = new OutputArgument();
			var maximumArg = new OutputArgument();
			Function.Call(Native.Hash.GET_MODEL_DIMENSIONS, Hash, minimumArg, maximumArg);

			minimum = minimumArg.GetResult<Vector3>();
			maximum = maximumArg.GetResult<Vector3>();
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
				{
					return false;
				}
			}

			return true;
		}
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
