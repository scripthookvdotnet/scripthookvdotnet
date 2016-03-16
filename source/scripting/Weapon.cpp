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
	bool Weapon::CanUseOnParachute::get()
	{
		return Native::Function::Call<bool>(Native::Hash::CAN_USE_WEAPON_ON_PARACHUTE, static_cast<int>(Hash));
	}
	int Weapon::DefaultClipSize::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_WEAPON_CLIP_SIZE, static_cast<int>(Hash));
	}
	WeaponGroup Weapon::Group::get()
	{
		return static_cast<WeaponGroup>(Native::Function::Call<int>(Native::Hash::GET_WEAPONTYPE_GROUP, static_cast<int>(Hash)));
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
	GTA::Model Weapon::Model::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_WEAPONTYPE_MODEL, static_cast<int>(Hash));
	}
	void Weapon::Tint::set(WeaponTint value)
	{
		Native::Function::Call(Native::Hash::SET_PED_WEAPON_TINT_INDEX, _owner, static_cast<int>(Hash), static_cast<int>(value));
	}
	WeaponTint Weapon::Tint::get()
	{
		return static_cast<WeaponTint>(Native::Function::Call<int>(Native::Hash::GET_PED_WEAPON_TINT_INDEX, _owner, static_cast<int>(Hash)));
	}

	WeaponCollection::WeaponCollection(Ped ^owner) : _owner(owner), _weapons(gcnew Dictionary<System::UInt32, Weapon ^>())
	{
	}

	Weapon ^WeaponCollection::Current::get()
	{
		unsigned int hash;
		Native::WeaponHash thash;

		Native::Function::Call(Native::Hash::GET_CURRENT_PED_WEAPON, _owner->Handle, &hash, true);
		thash = static_cast<Native::WeaponHash>(hash);

		if (_weapons->ContainsKey(hash))
		{
			return _weapons->default[hash];
		}

		Weapon ^weapon = gcnew Weapon(_owner, thash);
		_weapons->Add(hash, weapon);

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
	Weapon ^WeaponCollection::BestWeapon::get()
	{
		System::UInt32 hash = 0;
		Native::WeaponHash thash;

		hash = Native::Function::Call<int>(Native::Hash::GET_BEST_PED_WEAPON, _owner->Handle, 0);
		thash = static_cast<Native::WeaponHash>(hash);

		if (_weapons->ContainsKey(hash))
		{
			return _weapons->default[hash];
		}

		Weapon ^weapon = gcnew Weapon(_owner, thash);
		_weapons->Add(hash, weapon);

		return weapon;
	}
	Weapon ^WeaponCollection::default::get(Native::WeaponHash hash)
	{
		Weapon ^weapon;
		System::UInt32 uintHash = static_cast<System::UInt32>(hash);

		if (_weapons->TryGetValue(uintHash, weapon))
		{
			return weapon;
		}

		if (!Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, _owner->Handle, static_cast<int>(hash), 0))
		{
			return nullptr;
		}

		weapon = gcnew Weapon(_owner, hash);
		_weapons->Add(uintHash, weapon);

		return weapon;
	}

	void WeaponCollection::Drop()
	{
		Native::Function::Call(Native::Hash::SET_PED_DROPS_WEAPON, _owner);
	}
	Weapon ^WeaponCollection::Give(Native::WeaponHash hash, int ammoCount, bool equipNow, bool isAmmoLoaded)
	{
		Weapon ^weapon;
		System::UInt32 uintHash = static_cast<System::UInt32>(hash);

		if (!_weapons->TryGetValue(static_cast<System::UInt32>(hash), weapon))
		{
			weapon = gcnew Weapon(_owner, hash);
			_weapons->Add(uintHash, weapon);
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
	bool WeaponCollection::HasWeapon(Native::WeaponHash weaponHash)
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, _owner->Handle, static_cast<int>(weaponHash));
	}
	bool WeaponCollection::IsWeaponValid(Native::WeaponHash hash)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_WEAPON_VALID, static_cast<int>(hash));
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
	bool WeaponCollection::Select(Native::WeaponHash weaponHash)
	{
		return WeaponCollection::Select(weaponHash, true);
	}
	bool WeaponCollection::Select(Native::WeaponHash weaponHash, bool equipNow)
	{
		if (!Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, _owner->Handle, static_cast<int>(weaponHash)))
		{
			return false;
		}

		Native::Function::Call(Native::Hash::SET_CURRENT_PED_WEAPON, _owner->Handle, static_cast<int>(weaponHash), equipNow);

		return true;
	}
	void WeaponCollection::Remove(Weapon ^weapon)
	{
		System::UInt32 hash = static_cast<System::UInt32>(weapon->Hash);

		if (_weapons->ContainsKey(hash))
		{
			_weapons->Remove(hash);
		}

		WeaponCollection::Remove(weapon->Hash);
	}
	void WeaponCollection::Remove(Native::WeaponHash weaponHash)
	{
		Native::Function::Call(Native::Hash::REMOVE_WEAPON_FROM_PED, _owner->Handle, static_cast<int>(weaponHash));
	}
	void WeaponCollection::RemoveAll()
	{
		Native::Function::Call(Native::Hash::REMOVE_ALL_PED_WEAPONS, _owner->Handle, true);

		_weapons->Clear();
	}
}