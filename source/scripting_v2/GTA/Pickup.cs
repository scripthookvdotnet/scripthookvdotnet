//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public sealed class Pickup : IEquatable<Pickup>, IHandleable
	{
		public Pickup(int handle)
		{
			Handle = handle;
		}

		public int Handle
		{
			get;
		}

		public Vector3 Position => Function.Call<Vector3>(Hash.GET_PICKUP_COORDS, Handle);

		public bool IsCollected => Function.Call<bool>(Hash.HAS_PICKUP_BEEN_COLLECTED, Handle);

		public bool ObjectExists()
		{
			return Function.Call<bool>(Hash.DOES_PICKUP_OBJECT_EXIST, Handle);
		}

		public void Delete()
		{
			Function.Call(Hash.REMOVE_PICKUP, Handle);
		}

		public bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_PICKUP_EXIST, Handle);
		}
		public static bool Exists(Pickup pickup)
		{
			return pickup != null && pickup.Exists();
		}

		public bool Equals(Pickup obj)
		{
			return obj is not null && Handle == obj.Handle;
		}
		public override bool Equals(object obj)
		{
			return obj is not null && obj.GetType() == GetType() && Equals((Pickup)obj);
		}

		public static bool operator ==(Pickup left, Pickup right)
		{
			return left?.Equals(right) ?? right is null;
		}
		public static bool operator !=(Pickup left, Pickup right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
