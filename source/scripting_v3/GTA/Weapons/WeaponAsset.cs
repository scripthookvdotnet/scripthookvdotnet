//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	public struct WeaponAsset : IEquatable<WeaponAsset>, INativeValue
	{
		public WeaponAsset(int hash) : this()
		{
			Hash = hash;
		}
		public WeaponAsset(uint hash) : this((int)hash)
		{
		}
		public WeaponAsset(WeaponHash hash) : this((int)hash)
		{
		}

		/// <summary>
		/// Gets the hash for this <see cref="WeaponAsset"/>.
		/// </summary>
		public int Hash
		{
			get; private set;
		}

		/// <summary>
		/// Gets the slot hash for this <see cref="WeaponAsset"/>.
		/// </summary>
		/// <remarks>
		/// The slot hash must be unique in a weapon inventory of a <see cref="Ped"/> , so a <see cref="Ped"/> cannot have multiple weapon items with the same slot hash in their inventory.
		/// </remarks>
		public int SlotHash => Function.Call<int>(Native.Hash.GET_WEAPONTYPE_SLOT, (uint)Hash);

		/// <summary>
		/// Gets the native representation of this <see cref="WeaponAsset"/>.
		/// </summary>
		public ulong NativeValue
		{
			get => (ulong)Hash;
			set => Hash = unchecked((int)value);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="WeaponAsset"/> is valid as a weapon or a ammo hash.
		/// </summary>
		public bool IsValid => Function.Call<bool>(Native.Hash.IS_WEAPON_VALID, Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="WeaponAsset"/> is valid as a weapon hash.
		/// </summary>
		public bool IsValidAsWeaponHash => SHVDN.NativeMemory.IsHashValidAsWeaponHash((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="WeaponAsset"/> is loaded so it can be spawned.
		/// </summary>
		public bool IsLoaded => Function.Call<bool>(Native.Hash.HAS_WEAPON_ASSET_LOADED, Hash);

		/// <summary>
		/// Attempts to load this <see cref="WeaponAsset"/> into memory.
		/// </summary>
		public void Request()
		{
			Function.Call(Native.Hash.REQUEST_WEAPON_ASSET, Hash, 31, 0);
		}
		/// <summary>
		/// Attempts to load this <see cref="WeaponAsset"/> into memory for a given period of time.
		/// </summary>
		/// <param name="timeout">The time (in milliseconds) before giving up trying to load this <see cref="WeaponAsset"/>.</param>
		/// <returns><see langword="true" /> if this <see cref="WeaponAsset"/> is loaded; otherwise, <see langword="false" />.</returns>
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
		/// Tells the game we have finished using this <see cref="WeaponAsset"/> and it can be freed from memory.
		/// </summary>
		public void MarkAsNoLongerNeeded()
		{
			Function.Call(Native.Hash.REMOVE_WEAPON_ASSET, Hash);
		}

		/// <summary>
		/// Gets the display name label string for this <see cref="WeaponAsset"/>.
		/// </summary>
		/// <remarks>
		/// Returns <see cref="string.Empty"/> if this <see cref="WeaponAsset"/> is not valid as a weapon hash.
		/// </remarks>
		public string DisplayName => IsValidAsWeaponHash ? Weapon.GetDisplayNameFromHash((WeaponHash)Hash) : string.Empty;

		/// <summary>
		/// Gets the localized human name for this <see cref="WeaponAsset"/>.
		/// </summary>
		/// <remarks>
		/// Returns <see cref="string.Empty"/> if this <see cref="WeaponAsset"/> is not valid as a weapon hash.
		/// </remarks>
		public string HumanName => IsValidAsWeaponHash ? Weapon.GetHumanNameFromHash((WeaponHash)Hash) : string.Empty;

		public bool Equals(WeaponAsset weaponAsset)
		{
			return Hash == weaponAsset.Hash;
		}
		public override bool Equals(object obj)
		{
			if (obj is WeaponAsset asset)
			{
				return Equals(asset);
			}

			return false;
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

		public static implicit operator InputArgument(WeaponAsset value)
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
