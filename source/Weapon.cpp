#include "Weapon.hpp"
#include "Ped.hpp"
#include "Native.hpp"

namespace GTA
{
	Weapon::Weapon(Ped ^owner, Native::WeaponHash hash) : mOwner(owner), mHash(hash)
	{
	}

	Native::WeaponHash Weapon::Hash::get()
	{
		return this->mHash;
	}

	int Weapon::Ammo::get()
	{
		if (this->mHash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		else if (!IsPresent)
		{
			return 0;
		}

		return Native::Function::Call<int>(Native::Hash::GET_AMMO_IN_PED_WEAPON, this->mOwner->Handle, static_cast<int>(this->mHash));
	}
	void Weapon::Ammo::set(int value)
	{
		if (this->mHash == Native::WeaponHash::Unarmed)
		{
			return;
		}

		if (IsPresent)
		{
			Native::Function::Call(Native::Hash::SET_PED_AMMO, this->mOwner->Handle, static_cast<int>(this->mHash), value);
		}
		else
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_TO_PED, this->mOwner->Handle, static_cast<int>(this->mHash), value, false, true);
		}
	}
	int Weapon::AmmoInClip::get()
	{
		if (this->mHash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		else if (!IsPresent)
		{
			return 0;
		}

		int value = 0;
		Native::Function::Call(Native::Hash::GET_AMMO_IN_CLIP, this->mOwner->Handle, static_cast<int>(this->mHash), &value);

		return value;
	}
	void Weapon::AmmoInClip::set(int value)
	{
		if (this->mHash == Native::WeaponHash::Unarmed)
		{
			return;
		}

		if (IsPresent)
		{
			Native::Function::Call(Native::Hash::SET_AMMO_IN_CLIP, this->mOwner->Handle, static_cast<int>(this->mHash), value);
		}
		else
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_TO_PED, this->mOwner->Handle, static_cast<int>(this->mHash), value, true, true);
		}
	}
	void Weapon::InfiniteAmmo::set(bool value)
	{
		if (this->mHash == Native::WeaponHash::Unarmed)
		{
			return;
		}

		Native::Function::Call(Native::Hash::SET_PED_INFINITE_AMMO, this->mOwner->Handle, value, static_cast<int>(this->mHash));
	}
	void Weapon::InfiniteAmmoClip::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_INFINITE_AMMO_CLIP, this->mOwner->Handle, value);
	}
	bool Weapon::IsPresent::get()
	{
		if (this->mHash == Native::WeaponHash::Unarmed)
		{
			return true;
		}

		return Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, this->mOwner->Handle, static_cast<int>(this->mHash));
	}
	int Weapon::MaxAmmo::get()
	{
		if (this->mHash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}

		int value = 0;
		Native::Function::Call(Native::Hash::GET_MAX_AMMO, this->mOwner->Handle, static_cast<int>(this->mHash), &value);

		return value;
	}
	int Weapon::MaxAmmoInClip::get()
	{
		if (this->mHash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		else if (!IsPresent)
		{
			return 0;
		}

		return Native::Function::Call<int>(Native::Hash::GET_MAX_AMMO_IN_CLIP, this->mOwner->Handle, static_cast<int>(this->mHash), true);
	}

	WeaponCollection::WeaponCollection(Ped ^owner) : mOwner(owner), mWeapons(gcnew System::Collections::Generic::Dictionary<Native::WeaponHash, Weapon^>())
	{
	}

	Weapon ^WeaponCollection::Current::get()
	{
		int hash = 0;
		Native::Function::Call(Native::Hash::GET_CURRENT_PED_WEAPON, this->mOwner->Handle, &hash, true);

		Native::WeaponHash thash = static_cast<Native::WeaponHash>(hash);

		if (this->mWeapons->ContainsKey(thash))
		{
			return this->mWeapons->default[thash];
		}

		Weapon ^weapon = gcnew Weapon(this->mOwner, thash);
		this->mWeapons->Add(thash, weapon);

		return weapon;
	}
	Weapon ^WeaponCollection::default::get(Native::WeaponHash hash)
	{
		Weapon ^weapon;

		if (this->mWeapons->TryGetValue(hash, weapon))
		{
			return weapon;
		}

		if (Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, this->mOwner->Handle, static_cast<int>(hash), 0))
		{
			weapon = gcnew Weapon(this->mOwner, hash);
			this->mWeapons->Add(hash, weapon);

			return weapon;
		}

		return nullptr;
	}

	void WeaponCollection::Drop()
	{
		Native::Function::Call(Native::Hash::SET_PED_DROPS_WEAPON, this->mOwner);
	}
	Weapon ^WeaponCollection::Give(Native::WeaponHash hash, int ammoCount, bool equipNow, bool isAmmoLoaded)
	{
		Weapon ^weapon;

		if (!this->mWeapons->TryGetValue(hash, weapon))
		{
			weapon = gcnew Weapon(this->mOwner, hash);
			this->mWeapons->Add(hash, weapon);
		}

		if (weapon->IsPresent)
		{
			this->Select(weapon);
		}
		else
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_TO_PED, this->mOwner->Handle, static_cast<int>(weapon->Hash), ammoCount, equipNow, isAmmoLoaded);
		}

		return weapon;
	}
	bool WeaponCollection::Select(Weapon ^weapon)
	{
		if (!weapon->IsPresent)
		{
			return false;
		}

		Native::Function::Call(Native::Hash::SET_CURRENT_PED_WEAPON, this->mOwner->Handle, static_cast<int>(weapon->Hash), true);

		return true;
	}
	void WeaponCollection::Remove(Weapon ^weapon)
	{
		if (this->mWeapons->ContainsKey(weapon->Hash))
		{
			this->mWeapons->Remove(weapon->Hash);
		}

		Native::Function::Call(Native::Hash::REMOVE_WEAPON_FROM_PED, this->mOwner->Handle, static_cast<int>(weapon->Hash));
	}
	void WeaponCollection::RemoveAll()
	{
		Native::Function::Call(Native::Hash::REMOVE_ALL_PED_WEAPONS, this->mOwner->Handle, true);

		this->mWeapons->Clear();
	}
}