//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;
using System.Collections.Generic;

namespace GTA
{
	public sealed class WeaponCollection
	{
		#region Fields
		readonly Ped owner;
		readonly Dictionary<WeaponHash, Weapon> weapons = new Dictionary<WeaponHash, Weapon>();
		#endregion

		internal WeaponCollection(Ped owner)
		{
			this.owner = owner;
		}

		public Weapon this[WeaponHash hash]
		{
			get
			{
				if (!weapons.TryGetValue(hash, out Weapon weapon))
				{
					if (!Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, owner.Handle, hash, 0))
					{
						return null;
					}

					weapon = new Weapon(owner, hash);
					weapons.Add(hash, weapon);
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
					Function.Call(Hash.GET_CURRENT_PED_WEAPON, owner.Handle, &currentWeapon, true);
				}

				var hash = (WeaponHash)currentWeapon;

				if (weapons.ContainsKey(hash))
				{
					return weapons[hash];
				}
				else
				{
					var weapon = new Weapon(owner, hash);
					weapons.Add(hash, weapon);

					return weapon;
				}
			}
		}

		public Weapon BestWeapon
		{
			get
			{
				WeaponHash hash = Function.Call<WeaponHash>(Hash.GET_BEST_PED_WEAPON, owner.Handle, 0);

				if (weapons.ContainsKey(hash))
				{
					return weapons[hash];
				}
				else
				{
					var weapon = new Weapon(owner, (WeaponHash)hash);
					weapons.Add(hash, weapon);

					return weapon;
				}
			}
		}

		public bool HasWeapon(WeaponHash weaponHash)
		{
			return Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, owner.Handle, weaponHash);
		}

		public bool IsWeaponValid(WeaponHash hash)
		{
			return Function.Call<bool>(Hash.IS_WEAPON_VALID, hash);
		}

		public Prop CurrentWeaponObject
		{
			get
			{
				if (Current.Hash == WeaponHash.Unarmed)
				{
					return null;
				}

				return new Prop(Function.Call<int>(Hash.GET_CURRENT_PED_WEAPON_ENTITY_INDEX, owner.Handle));
			}
		}

		public bool Select(Weapon weapon)
		{
			if (!weapon.IsPresent)
			{
				return false;
			}

			Function.Call(Hash.SET_CURRENT_PED_WEAPON, owner.Handle, weapon.Hash, true);

			return true;
		}
		public bool Select(WeaponHash weaponHash)
		{
			return Select(weaponHash, true);
		}
		public bool Select(WeaponHash weaponHash, bool equipNow)
		{
			if (!Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, owner.Handle, weaponHash))
			{
				return false;
			}

			Function.Call(Hash.SET_CURRENT_PED_WEAPON, owner.Handle, weaponHash, equipNow);

			return true;
		}

		public Weapon Give(WeaponHash weaponHash, int ammoCount, bool equipNow, bool isAmmoLoaded)
		{
			if (!weapons.TryGetValue(weaponHash, out Weapon weapon))
			{
				weapon = new Weapon(owner, weaponHash);
				weapons.Add(weaponHash, weapon);
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
				Function.Call(Hash.GIVE_WEAPON_TO_PED, owner.Handle, weapon.Hash, ammoCount, equipNow, isAmmoLoaded);
			}

			return weapon;
		}

		public Weapon Give(string name, int ammoCount, bool equipNow, bool isAmmoLoaded)
		{
			return Give((WeaponHash)Game.GenerateHash(name), ammoCount, equipNow, isAmmoLoaded);
		}

		public void Drop()
		{
			Function.Call(Hash.SET_PED_DROPS_WEAPON, owner.Handle);
		}

		public void Remove(Weapon weapon)
		{
			WeaponHash hash = weapon.Hash;

			if (weapons.ContainsKey(hash))
			{
				weapons.Remove(hash);
			}

			Remove(weapon.Hash);
		}
		public void Remove(WeaponHash weaponHash)
		{
			Function.Call(Hash.REMOVE_WEAPON_FROM_PED, owner.Handle, weaponHash);
		}

		public void RemoveAll()
		{
			Function.Call(Hash.REMOVE_ALL_PED_WEAPONS, owner.Handle, true);

			weapons.Clear();
		}
	}
}
