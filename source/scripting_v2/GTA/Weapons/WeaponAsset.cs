//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	public struct WeaponAsset : IEquatable<WeaponAsset>
	{
		public WeaponAsset(int weaponHash) : this()
		{
			Hash = weaponHash;
		}
		public WeaponAsset(uint weaponHash) : this((int)weaponHash)
		{
		}
		public WeaponAsset(WeaponHash weaponHash) : this((int)weaponHash)
		{
		}

		public int Hash
		{
			get; private set;
		}

		public bool IsValid => Function.Call<bool>(Native.Hash.IS_WEAPON_VALID, Hash);
		public bool IsLoaded => Function.Call<bool>(Native.Hash.HAS_WEAPON_ASSET_LOADED, Hash);

		public void Request()
		{
			Function.Call(Native.Hash.REQUEST_WEAPON_ASSET, Hash, 31, 0);
		}
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

		public void Dismiss()
		{
			Function.Call(Native.Hash.REMOVE_WEAPON_ASSET, Hash);
		}

		public bool Equals(WeaponAsset obj)
		{
			return Hash == obj.Hash;
		}
		public override bool Equals(object obj)
		{
			return obj is not null && obj.GetType() == GetType() && Equals((WeaponAsset)obj);
		}

		public static bool operator ==(WeaponAsset left, WeaponAsset right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(WeaponAsset left, WeaponAsset right)
		{
			return !left.Equals(right);
		}

		public static implicit operator WeaponAsset(int hash)
		{
			return new WeaponAsset(hash);
		}
		public static implicit operator WeaponAsset(uint hash)
		{
			return new WeaponAsset(hash);
		}
		public static implicit operator WeaponAsset(WeaponHash hash)
		{
			return new WeaponAsset(hash);
		}

		public override int GetHashCode()
		{
			return Hash;
		}

		public override string ToString()
		{
			return "0x" + ((uint)Hash).ToString("X");
		}
	}
}
