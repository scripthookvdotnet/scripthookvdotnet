//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
    public struct WeaponAsset : IEquatable<WeaponAsset>, INativeValue, IScriptStreamingResource
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
        /// The slot hash must be unique in a weapon inventory of a <see cref="Ped"/> , so a <see cref="Ped"/> cannot
        /// have multiple weapon items with the same slot hash in their inventory.
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
        /// Gets a value indicating whether this <see cref="WeaponAsset"/> is valid as a weapon or an ammo hash.
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
        /// <para>
        /// Requests the global streaming loader to load this <see cref="WeaponAsset"/> so it will be eventually loaded
        /// (unless getting interrupted by a <see cref="MarkAsNoLongerNeeded()"/> call of another SHVDN script).
        /// </para>
        /// <para>
        /// You will need to test if the resource is loaded with <see cref="IsLoaded"/> every frame until
        /// the <see cref="WeaponAsset"/> is loaded before you can use it. The game starts loading pending streaming
        /// objects every frame (with `<c>CStreaming::Update()</c>`) before the script update call.
        /// </para>
        /// </summary>
        public void Request() => Request(WeaponScriptResourceFlags.RequestAllAnims);
        /// <summary>
        /// Attempts to load this <see cref="WeaponAsset"/> into memory for a given period of time.
        /// </summary>
        /// <param name="timeout">The time (in milliseconds) before giving up trying to load this
        /// <see cref="WeaponAsset"/>.</param>
        /// <returns><see langword="true"/> if this <see cref="WeaponAsset"/> is loaded; otherwise,
        /// <see langword="false"/>.</returns>
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
        /// Attempts to load this <see cref="WeaponAsset"/> into memory.
        /// </summary>
        /// <param name="weaponResourceFlags">
        /// The set of weapon resource flags that specifies which weapon animations to load in advance.
        /// </param>
        /// <param name="extraWeaponComponentResourceFlags">
        /// The set of weapon component resource flags that specifies which extra weapon components to load in advance
        /// (not for default components).
        /// </param>
        // There's no need to have the first argument non-optional to avoid ambiguous method resolution if there wasn't
        // `Request()` with 0 parameters in v3.6.0. There's occurrences of `REQUEST_WEAPON_ASSET` where only a weapon
        // hash and a set of weapon resource flags (for anims) are explicitly supplied, such as `Prologue1.sc` (or
        // the compiled script for the Windows version `Prologue1.ysc`).
        public void Request(WeaponScriptResourceFlags weaponResourceFlags,
            ExtraWeaponComponentScriptResourceFlags extraWeaponComponentResourceFlags
            = ExtraWeaponComponentScriptResourceFlags.None)
        {
            Function.Call(Native.Hash.REQUEST_WEAPON_ASSET, Hash, (uint)weaponResourceFlags,
                (uint)extraWeaponComponentResourceFlags);
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
        public string DisplayName
            => IsValidAsWeaponHash ? Weapon.GetDisplayNameFromHash((WeaponHash)Hash) : string.Empty;

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
