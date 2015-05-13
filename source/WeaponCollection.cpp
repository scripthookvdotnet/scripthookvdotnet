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
		return gcnew Weapon(this->pOwner, (Native::WeaponHash)hash);
	}
	Weapon ^WeaponCollection::default::get(Native::WeaponHash hash)
	{
		Weapon ^weapon;
		if (this->weapons->TryGetValue(hash, weapon))
		{
			return weapon;
		}
		bool present = Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, this->pOwner->Handle, static_cast<int>(hash));
		if (present)
		{
			return gcnew Weapon(this->pOwner, (Native::WeaponHash)hash);
		}
		return nullptr;
	}
	Weapon ^WeaponCollection::Give(Native::WeaponHash hash)
	{
		Weapon ^weapon;
		if (!this->weapons->TryGetValue(hash, weapon))
		{
			weapon = gcnew Weapon(this->pOwner, hash);
		}
		weapon->Select();
		return weapon;
	}
}