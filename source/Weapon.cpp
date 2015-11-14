#include "Weapon.hpp"
#include "Ped.hpp"
#include "Prop.hpp"
#include "Native.hpp"

namespace GTA
{
	using namespace System::Collections::Generic;

	Weapon::Weapon() : _hash(Native::WeaponHash::Unarmed)
	{
	}
	Weapon::Weapon(Ped ^owner, Native::WeaponHash hash) : _owner(owner), _hash(hash)
	{
	}

	Native::WeaponHash Weapon::Hash::get()
	{
		return _hash;
	}
	int Weapon::Ammo::get()
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		else if (!IsPresent)
		{
			return 0;
		}

		return Native::Function::Call<int>(Native::Hash::GET_AMMO_IN_PED_WEAPON, _owner->Handle, static_cast<int>(Hash));
	}
	void Weapon::Ammo::set(int value)
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return;
		}

		if (IsPresent)
		{
			Native::Function::Call(Native::Hash::SET_PED_AMMO, _owner->Handle, static_cast<int>(Hash), value);
		}
		else
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_TO_PED, _owner->Handle, static_cast<int>(Hash), value, false, true);
		}
	}
	int Weapon::AmmoInClip::get()
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		else if (!IsPresent)
		{
			return 0;
		}

		int value = 0;
		Native::Function::Call(Native::Hash::GET_AMMO_IN_CLIP, _owner->Handle, static_cast<int>(Hash), &value);

		return value;
	}
	void Weapon::AmmoInClip::set(int value)
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return;
		}

		if (IsPresent)
		{
			Native::Function::Call(Native::Hash::SET_AMMO_IN_CLIP, _owner->Handle, static_cast<int>(Hash), value);
		}
		else
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_TO_PED, _owner->Handle, static_cast<int>(Hash), value, true, true);
		}
	}
	void Weapon::InfiniteAmmo::set(bool value)
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return;
		}

		Native::Function::Call(Native::Hash::SET_PED_INFINITE_AMMO, _owner->Handle, value, static_cast<int>(Hash));
	}
	void Weapon::InfiniteAmmoClip::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_INFINITE_AMMO_CLIP, _owner->Handle, value);
	}
	bool Weapon::IsPresent::get()
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return true;
		}

		return Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, _owner->Handle, static_cast<int>(Hash));
	}
	int Weapon::MaxAmmo::get()
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}

		int value = 0;
		Native::Function::Call(Native::Hash::GET_MAX_AMMO, _owner->Handle, static_cast<int>(Hash), &value);

		return value;
	}
	int Weapon::MaxAmmoInClip::get()
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		else if (!IsPresent)
		{
			return 0;
		}

		return Native::Function::Call<int>(Native::Hash::GET_MAX_AMMO_IN_CLIP, _owner->Handle, static_cast<int>(Hash), true);
	}
	void Weapon::Tint::set(WeaponTint value)
	{
		Native::Function::Call(Native::Hash::SET_PED_WEAPON_TINT_INDEX, _owner, static_cast<int>(Hash), static_cast<int>(value));
	}
	WeaponTint Weapon::Tint::get()
	{
		return static_cast<WeaponTint>(Native::Function::Call<int>(Native::Hash::GET_PED_WEAPON_TINT_INDEX, _owner, static_cast<int>(Hash)));
	}

	WeaponCollection::WeaponCollection(Ped ^owner) : _owner(owner), _weapons(gcnew Dictionary<Native::WeaponHash, Weapon ^>())
	{
	}

	Weapon ^WeaponCollection::Current::get()
	{
		int hash = 0;
		Native::WeaponHash thash;

		Native::Function::Call(Native::Hash::GET_CURRENT_PED_WEAPON, _owner->Handle, &hash, true);
		thash = static_cast<Native::WeaponHash>(hash);

		if (_weapons->ContainsKey(thash))
		{
			return _weapons->default[thash];
		}

		Weapon ^weapon = gcnew Weapon(_owner, thash);
		_weapons->Add(thash, weapon);

		return weapon;
	}
	Prop ^WeaponCollection::CurrentWeaponObject::get()
	{
		if (Current->Hash != Native::WeaponHash::Unarmed)
		{
			return Native::Function::Call<Prop^>(Native::Hash::_0x3B390A939AF0B5FC, _owner->Handle);
		}

		return nullptr;
	}
	Weapon ^WeaponCollection::default::get(Native::WeaponHash hash)
	{
		Weapon ^weapon;

		if (_weapons->TryGetValue(hash, weapon))
		{
			return weapon;
		}

		if (!Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, _owner->Handle, static_cast<int>(hash), 0))
		{
			return nullptr;
		}

		weapon = gcnew Weapon(_owner, hash);
		_weapons->Add(hash, weapon);

		return weapon;
	}

	void WeaponCollection::Drop()
	{
		Native::Function::Call(Native::Hash::SET_PED_DROPS_WEAPON, _owner);
	}
	Weapon ^WeaponCollection::Give(Native::WeaponHash hash, int ammoCount, bool equipNow, bool isAmmoLoaded)
	{
		Weapon ^weapon;

		if (!_weapons->TryGetValue(hash, weapon))
		{
			weapon = gcnew Weapon(_owner, hash);
			_weapons->Add(hash, weapon);
		}

		if (weapon->IsPresent)
		{
			Select(weapon);
		}
		else
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_TO_PED, _owner->Handle, static_cast<int>(weapon->Hash), ammoCount, equipNow, isAmmoLoaded);
		}

		return weapon;
	}
	bool WeaponCollection::Select(Weapon ^weapon)
	{
		if (!weapon->IsPresent)
		{
			return false;
		}

		Native::Function::Call(Native::Hash::SET_CURRENT_PED_WEAPON, _owner->Handle, static_cast<int>(weapon->Hash), true);

		return true;
	}
	void WeaponCollection::Remove(Weapon ^weapon)
	{
		if (_weapons->ContainsKey(weapon->Hash))
		{
			_weapons->Remove(weapon->Hash);
		}

		Native::Function::Call(Native::Hash::REMOVE_WEAPON_FROM_PED, _owner->Handle, static_cast<int>(weapon->Hash));
	}
	void WeaponCollection::RemoveAll()
	{
		Native::Function::Call(Native::Hash::REMOVE_ALL_PED_WEAPONS, _owner->Handle, true);

		_weapons->Clear();
	}
}