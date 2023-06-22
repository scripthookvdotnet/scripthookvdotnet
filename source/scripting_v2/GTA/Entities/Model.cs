//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
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

		/// <summary>
		/// Gets the hash for this <see cref="Model"/>.
		/// </summary>
		public int Hash
		{
			get; private set;
		}

		/// <summary>
		/// Gets a value that indicates whether this <see cref="Model"/> exists.
		/// This property is practically the same as <see cref="IsInCdImage"/> since an additional check this method does does not work in practice.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Model"/> is valid; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>
		/// <para>The hash of a streamed archetype (MLO is one of them) might only be valid at certain locations on the map.</para>
		/// <para>
		/// You may want to use <see cref="IsInCdImage"/> to avoid a redundant additional check,
		/// where <c>!(~(((modelIndex | 0xFFF0000) &amp; 0xEFFFFFFF) &lt;&lt; 28) &amp; 1)</c> is evaluated (but always evaluated as false in the range of uint32_t)
		/// after checking if the model index is not <c>0xFFFF</c> (which means the index could not found) just like <see cref="IsInCdImage"/> does.
		/// </para>
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
		/// The hash of a streamed archetype (MLO is one of them) might only be valid at certain locations on the map.
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
		/// Gets a value indicating whether this <see cref="Model"/> is a bicycle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a bicycle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsBicycle => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BICYCLE, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a bike (either a motorcycle or a bicycle).
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a bike; otherwise, <see langword="false" />.
		/// </value>
		public bool IsBike => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BIKE, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a boat.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a boat; otherwise, <see langword="false" />.
		/// </value>
		public bool IsBoat => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BOAT, Hash);
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
		/// Gets a value indicating whether this <see cref="Model"/> is a helicopter.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a helicopter; otherwise, <see langword="false" />.
		/// </value>
		public bool IsHelicopter => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_HELI, Hash);
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
		/// Gets a value indicating whether this <see cref="Model"/> is a quad bike and not an amphibious one.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a quad bike and not an amphibious one; otherwise, <see langword="false" />.
		/// </value>
		public bool IsQuadbike => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_QUADBIKE, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a train.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a train; otherwise, <see langword="false" />.
		/// </value>
		public bool IsTrain => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_TRAIN, Hash);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Model"/> is a vehicle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Model"/> is a vehicle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsVehicle => Function.Call<bool>(Native.Hash.IS_MODEL_A_VEHICLE, Hash);

		/// <summary>
		/// <para>Gets the dimensions of this <see cref="Model"/>.</para>
		/// </summary>
		/// <param name="minimum">The rear bottom left relative offset from the origin of the model.</param>
		/// <param name="maximum">The front top right relative offset from the origin of the model.</param>
		public void GetDimensions(out Vector3 minimum, out Vector3 maximum)
		{
			var outMin = new OutputArgument();
			var outMax = new OutputArgument();
			Function.Call(Native.Hash.GET_MODEL_DIMENSIONS, Hash, outMin, outMax);
			minimum = outMin.GetResult<Vector3>();
			maximum = outMax.GetResult<Vector3>();
		}
		/// <summary>
		/// <para>Gets the size of the dimensions of this <see cref="Model"/>.</para>
		/// </summary>
		public Vector3 GetDimensions()
		{
			GetDimensions(out Vector3 min, out Vector3 max);
			return Vector3.Subtract(max, min);
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
			return $"0x{Hash.ToString("X")}";
		}
	}
}
