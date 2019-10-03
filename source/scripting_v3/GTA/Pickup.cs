using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public sealed class Pickup : PoolObject, IEquatable<Pickup>
	{
		public Pickup(int handle) : base(handle)
		{
		}

		public Vector3 Position => Function.Call<Vector3>(Hash.GET_PICKUP_COORDS, Handle);

		public bool IsCollected => Function.Call<bool>(Hash.HAS_PICKUP_BEEN_COLLECTED, Handle);

		public override void Delete()
		{
			Function.Call(Hash.REMOVE_PICKUP, Handle);
		}

		public bool ObjectExists()
		{
			return Function.Call<bool>(Hash.DOES_PICKUP_OBJECT_EXIST, Handle);
		}
		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_PICKUP_EXIST, Handle);
		}
		public static bool Exists(Pickup pickup)
		{
			return !ReferenceEquals(pickup, null) && pickup.Exists();
		}

		public bool Equals(Pickup pickup)
		{
			return !ReferenceEquals(pickup, null) && Handle == pickup.Handle;
		}
		public sealed override bool Equals(object obj)
		{
			return !ReferenceEquals(obj, null) && obj.GetType() == GetType() && Equals((Pickup)obj);
		}

		public sealed override int GetHashCode()
		{
			return Handle;
		}

		public static bool operator ==(Pickup left, Pickup right)
		{
			return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);
		}
		public static bool operator !=(Pickup left, Pickup right)
		{
			return !(left == right);
		}
	}
}
