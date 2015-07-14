#include "WeaponCollection.hpp"
#include "Ped.hpp"
#include "Native.hpp"

namespace GTA
{
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