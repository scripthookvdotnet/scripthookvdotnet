//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System.Collections.Generic;

namespace GTA
{
	public sealed class WeaponCollection
	{
		#region Fields

		private readonly Ped _owner;
		private readonly Dictionary<WeaponHash, Weapon> _weapons = new Dictionary<WeaponHash, Weapon>();
		#endregion

		internal WeaponCollection(Ped owner)
		{
			this._owner = owner;
		}

		public Weapon this[WeaponHash hash]
		{
			get
			{
				if (!_weapons.TryGetValue(hash, out Weapon weapon))
				{
					if (!Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, _owner.Handle, (uint)hash, 0))
					{
						return null;
					}

					weapon = new Weapon(_owner, hash);
					_weapons.Add(hash, weapon);
				}

				return weapon;
			}
		}

		public Weapon Current
		{
			get
			{
				int currentWeapon;
				unsafe
				{
					Function.Call(Hash.GET_CURRENT_PED_WEAPON, _owner.Handle, &currentWeapon, true);
				}

				var hash = (WeaponHash)currentWeapon;

				if (_weapons.ContainsKey(hash))
				{
					return _weapons[hash];
				}
				else
				{
					var weapon = new Weapon(_owner, hash);
					_weapons.Add(hash, weapon);

					return weapon;
				}
			}
		}

		public Weapon BestWeapon
		{
			get
			{
				WeaponHash hash = Function.Call<WeaponHash>(Hash.GET_BEST_PED_WEAPON, _owner.Handle, 0);

				if (_weapons.ContainsKey(hash))
				{
					return _weapons[hash];
				}
				else
				{
					var weapon = new Weapon(_owner, (WeaponHash)hash);
					_weapons.Add(hash, weapon);

					return weapon;
				}
			}
		}

		public bool HasWeapon(WeaponHash weaponHash)
		{
			return Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, _owner.Handle, (uint)weaponHash);
		}

		public bool IsWeaponValid(WeaponHash hash)
		{
			return Function.Call<bool>(Hash.IS_WEAPON_VALID, (uint)hash);
		}

		public Prop CurrentWeaponObject
		{
			get
			{
				if (Current.Hash == WeaponHash.Unarmed)
				{
					return null;
				}

				return new Prop(Function.Call<int>(Hash.GET_CURRENT_PED_WEAPON_ENTITY_INDEX, _owner.Handle));
			}
		}

		public bool Select(Weapon weapon)
		{
			if (!weapon.IsPresent)
			{
				return false;
			}

			Function.Call(Hash.SET_CURRENT_PED_WEAPON, _owner.Handle, (uint)weapon.Hash, true);

			return true;
		}
		public bool Select(WeaponHash weaponHash)
		{
			return Select(weaponHash, true);
		}
		public bool Select(WeaponHash weaponHash, bool equipNow)
		{
			if (!Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, _owner.Handle, (uint)weaponHash))
			{
				return false;
			}

			Function.Call(Hash.SET_CURRENT_PED_WEAPON, _owner.Handle, (uint)weaponHash, equipNow);

			return true;
		}

		public Weapon Give(WeaponHash hash, int ammoCount, bool equipNow, bool isAmmoLoaded)
		{
			if (!_weapons.TryGetValue(hash, out Weapon weapon))
			{
				weapon = new Weapon(_owner, hash);
				_weapons.Add(hash, weapon);
			}

			if (weapon.IsPresent)
			{
				if (equipNow)
				{
					Select(weapon);
				}
			}
			else
			{
				// Set the 4th argument to false for consistency. If 4th argument is set to true when 5th one is set to true, the ped will instantly select the added weapon in any case.
				Function.Call(Hash.GIVE_WEAPON_TO_PED, _owner.Handle, (uint)weapon.Hash, ammoCount, false, equipNow);
			}

			return weapon;
		}

		public Weapon Give(string name, int ammoCount, bool equipNow, bool isAmmoLoaded)
		{
			return Give((WeaponHash)Game.GenerateHash(name), ammoCount, equipNow, isAmmoLoaded);
		}

		public void Drop()
		{
			Function.Call(Hash.SET_PED_DROPS_WEAPON, _owner.Handle);
		}

		public void Remove(Weapon weapon)
		{
			WeaponHash hash = weapon.Hash;

			if (_weapons.ContainsKey(hash))
			{
				_weapons.Remove(hash);
			}

			Remove(weapon.Hash);
		}
		public void Remove(WeaponHash weaponHash)
		{
			Function.Call(Hash.REMOVE_WEAPON_FROM_PED, _owner.Handle, (uint)weaponHash);
		}

		public void RemoveAll()
		{
			Function.Call(Hash.REMOVE_ALL_PED_WEAPONS, _owner.Handle, true);

			_weapons.Clear();
		}
	}
}
