#include "WeaponCollection.hpp"
#include "Native.hpp"
#include "Weapon.hpp"

namespace GTA
{
	WeaponCollection::WeaponCollection(Ped ^owner)
	{
		this->pOwner = owner;
		this->weapons = gcnew System::Collections::Generic::Dictionary<Native::WeaponHash, Weapon^>();
	}

	Weapon ^WeaponCollection::Current::get()
	{
		int hash = 0;
		Native::Function::Call(Native::Hash::GET_CURRENT_PED_WEAPON, this->pOwner->Handle, &hash, true);
		Native::WeaponHash wHash = (Native::WeaponHash)hash;
		if (this->weapons->ContainsKey(wHash))
		{
			return this->weapons->default[wHash];
		}
		Weapon ^weapon = gcnew Weapon(this->pOwner, (Native::WeaponHash)hash);
		this->weapons->Add(wHash, weapon);
		return weapon;
	}
	Weapon ^WeaponCollection::default::get(Native::WeaponHash hash)
	{
		Weapon ^weapon;
		if (this->weapons->TryGetValue(hash, weapon))
		{
			return weapon;
		}
		bool present = Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, this->pOwner->Handle, static_cast<int>(hash), 0);
		if (present)
		{
			weapon = gcnew Weapon(this->pOwner, (Native::WeaponHash)hash);
			this->weapons->Add(hash, weapon);
			return weapon;
		}
		return nullptr;
	}

	void WeaponCollection::Drop()
	{
		Native::Function::Call(Native::Hash::SET_PED_DROPS_WEAPON, this->pOwner);
	}
	Weapon ^WeaponCollection::Give(Native::WeaponHash hash, int ammoCount, bool equipNow, bool isAmmoLoaded)
	{
		Weapon ^weapon;
		if (!this->weapons->TryGetValue(hash, weapon))
		{
			weapon = gcnew Weapon(this->pOwner, hash);
			this->weapons->Add(hash, weapon);
		}
		if (weapon->IsPresent)
		{
			this->Select(weapon);
		}
		else
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_TO_PED, this->pOwner->Handle, static_cast<int>(weapon->Hash), ammoCount, equipNow, isAmmoLoaded);
		}
		return weapon;
	}
	bool WeaponCollection::Select(Weapon ^weapon)
	{
		if (weapon->IsPresent)
		{
			Native::Function::Call(Native::Hash::SET_CURRENT_PED_WEAPON, this->pOwner->Handle, static_cast<int>(weapon->Hash), true);
			return true;
		}
		return false;
	}
	void WeaponCollection::Remove(Weapon ^weapon)
	{
		if (this->weapons->ContainsKey(weapon->Hash))
		{
			this->weapons->Remove(weapon->Hash);
		}
		Native::Function::Call(Native::Hash::REMOVE_WEAPON_FROM_PED, this->pOwner->Handle, static_cast<int>(weapon->Hash));
	}
	void WeaponCollection::RemoveAll()
	{
		Native::Function::Call(Native::Hash::REMOVE_ALL_PED_WEAPONS, this->pOwner->Handle, true);
		this->weapons->Clear();
	}
}