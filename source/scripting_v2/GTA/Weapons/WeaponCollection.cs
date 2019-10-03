using System.Collections.Generic;
using GTA.Native;

namespace GTA
{
	public sealed class WeaponCollection
	{
		Ped owner;
		readonly Dictionary<WeaponHash, Weapon> _weapons = new Dictionary<WeaponHash, Weapon>();

		internal WeaponCollection(Ped owner)
		{
			this.owner = owner;
		}

		public Weapon this[WeaponHash hash]
		{
			get
			{
				if (!_weapons.TryGetValue(hash, out Weapon weapon))
				{
					if (!Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, owner.Handle, (uint)hash, 0))
						return null;

					weapon = new Weapon(owner, hash);
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
					Function.Call(Hash.GET_CURRENT_PED_WEAPON, owner.Handle, &currentWeapon, true);
				}

				WeaponHash hash = (WeaponHash)currentWeapon;

				if (_weapons.ContainsKey(hash))
				{
					return _weapons[hash];
				}
				else
				{
					var weapon = new Weapon(owner, hash);
					_weapons.Add(hash, weapon);

					return weapon;
				}
			}
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
		public Weapon BestWeapon
		{
			get
			{
				WeaponHash hash = Function.Call<WeaponHash>(Hash.GET_BEST_PED_WEAPON, owner.Handle, 0);

				if (_weapons.ContainsKey(hash))
				{
					return _weapons[hash];
				}
				else
				{
					var weapon = new Weapon(owner, (WeaponHash)hash);
					_weapons.Add(hash, weapon);

					return weapon;
				}
			}
		}

		public bool HasWeapon(WeaponHash weaponHash)
		{
			return Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, owner.Handle, (uint)weaponHash);
		}
		public bool IsWeaponValid(WeaponHash hash)
		{
			return Function.Call<bool>(Hash.IS_WEAPON_VALID, (uint)hash);
		}

		public Weapon Give(WeaponHash hash, int ammoCount, bool equipNow, bool isAmmoLoaded)
		{
			Weapon weapon = null;

			if (!_weapons.TryGetValue(hash, out weapon))
			{
				weapon = new Weapon(owner, hash);
				_weapons.Add(hash, weapon);
			}

			if (weapon.IsPresent)
			{
				Select(weapon);
			}
			else
			{
				Function.Call(Hash.GIVE_WEAPON_TO_PED, owner.Handle, (uint)weapon.Hash, ammoCount, equipNow, isAmmoLoaded);
			}

			return weapon;
		}
		public bool Select(Weapon weapon)
		{
			if (!weapon.IsPresent)
			{
				return false;
			}

			Function.Call(Hash.SET_CURRENT_PED_WEAPON, owner.Handle, (uint)weapon.Hash, true);

			return true;
		}
		public bool Select(WeaponHash weaponHash)
		{
			return Select(weaponHash, true);
		}
		public bool Select(WeaponHash weaponHash, bool equipNow)
		{
			if (!Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, owner.Handle, (uint)weaponHash))
				return false;

			Function.Call(Hash.SET_CURRENT_PED_WEAPON, owner.Handle, (uint)weaponHash, equipNow);

			return true;
		}

		public void Drop()
		{
			Function.Call(Hash.SET_PED_DROPS_WEAPON, owner.Handle);
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
			Function.Call(Hash.REMOVE_WEAPON_FROM_PED, owner.Handle, (uint)weaponHash);
		}
		public void RemoveAll()
		{
			Function.Call(Hash.REMOVE_ALL_PED_WEAPONS, owner.Handle, true);

			_weapons.Clear();
		}
	}
}
