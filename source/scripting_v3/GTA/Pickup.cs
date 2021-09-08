//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
	public sealed class Pickup : PoolObject
	{
		public Pickup(int handle) : base(handle)
		{
		}

		/// <summary>
		/// The position of this <see cref="Pickup"/>.
		/// </summary>
		public Vector3 Position => Function.Call<Vector3>(Hash.GET_PICKUP_COORDS, Handle);

		/// <summary>
		/// Gets if this <see cref="Pickup"/> has been collected.
		/// </summary>
		public bool IsCollected => Function.Call<bool>(Hash.HAS_PICKUP_BEEN_COLLECTED, Handle);

		/// <summary>
		/// Determines if the object of this <see cref="Pickup"/> exists.
		/// </summary>
		/// <returns></returns>
		public bool ObjectExists()
		{
			return Function.Call<bool>(Hash.DOES_PICKUP_OBJECT_EXIST, Handle);
		}

		/// <summary>
		/// Destroys this <see cref="Pickup"/>.
		/// </summary>
		public override void Delete()
		{
			Function.Call(Hash.REMOVE_PICKUP, Handle);
		}

		/// <summary>
		/// Determines if this <see cref="Pickup"/> exists.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="Pickup"/> exists; otherwise, <see langword="false" />.</returns>
		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_PICKUP_EXIST, Handle);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same pickup as this <see cref="Pickup"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same pickup as this <see cref="Pickup"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Pickup pickup)
			{
				return Handle == pickup.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="Pickup"/>s refer to the same pickup.
		/// </summary>
		/// <param name="left">The left <see cref="Pickup"/>.</param>
		/// <param name="right">The right <see cref="Pickup"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same pickup as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Pickup left, Pickup right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		/// <summary>
		/// Determines if two <see cref="Pickup"/>s don't refer to the same pickup.
		/// </summary>
		/// <param name="left">The left <see cref="Pickup"/>.</param>
		/// <param name="right">The right <see cref="Pickup"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same pickup as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(Pickup left, Pickup right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts a <see cref="Pickup"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(Pickup value)
		{
			return new InputArgument((ulong)value.Handle);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
