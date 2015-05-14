#include "Weapon.hpp"
#include "Native.hpp"

namespace GTA
{
	Weapon::Weapon(Ped ^owner, Native::WeaponHash hash)
	{
		this->pOwner = owner;
		this->hash = hash;
	}

	int Weapon::Ammo::get()
	{
		if (this->hash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		if (this->IsPresent)
		{
			return Native::Function::Call<int>(Native::Hash::GET_AMMO_IN_PED_WEAPON, this->pOwner->Handle, static_cast<int>(this->hash));
		}
		return 0;
	}
	void Weapon::Ammo::set(int value)
	{
		if (this->hash == Native::WeaponHash::Unarmed)
		{
			return;
		}
		if (this->IsPresent)
		{
			Native::Function::Call(Native::Hash::SET_PED_AMMO, this->pOwner->Handle, static_cast<int>(this->hash), value);
		}
		else
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_TO_PED, this->pOwner->Handle, static_cast<int>(this->hash), value, false, true);
		}
	}
	int Weapon::AmmoInClip::get()
	{
		if (this->hash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		if (this->IsPresent)
		{
			int value = 0;
			Native::Function::Call(Native::Hash::GET_AMMO_IN_CLIP, this->pOwner->Handle, static_cast<int>(this->hash), &value);
			return value;
		}
		return 0;
	}
	void Weapon::AmmoInClip::set(int value)
	{
		if (this->hash == Native::WeaponHash::Unarmed)
		{
			return;
		}
		if (this->IsPresent)
		{
			Native::Function::Call(Native::Hash::SET_AMMO_IN_CLIP, this->pOwner->Handle, static_cast<int>(this->hash), value);
		}
		else
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_TO_PED, this->pOwner->Handle, static_cast<int>(this->hash), value, true, true);
		}
	}
	int Weapon::MaxAmmo::get()
	{
		if (this->hash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		int value = 0;
		Native::Function::Call(Native::Hash::GET_MAX_AMMO, this->pOwner->Handle, static_cast<int>(this->hash), &value);
		return value;
	}
	int Weapon::MaxAmmoInClip::get()
	{
		if (this->hash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		if (this->IsPresent)
		{
			return Native::Function::Call<int>(Native::Hash::GET_MAX_AMMO_IN_CLIP, this->pOwner->Handle, static_cast<int>(this->hash), true);
		}
		return 0;
	}
	bool Weapon::IsPresent::get()
	{
		if (this->hash == Native::WeaponHash::Unarmed)
		{
			return true;
		}
		return Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, this->pOwner->Handle, static_cast<int>(this->hash));
	}
	Native::WeaponHash Weapon::Hash::get()
	{
		return this->hash;
	}
}