#pragma once

#include "Weapon.hpp"

namespace GTA
{
	ref class Ped;

	public ref class WeaponCollection sealed
	{
	public:
		property Weapon ^Current
		{
			Weapon ^get();
		}

		property Weapon ^default[Native::WeaponHash]
		{
			Weapon ^get(Native::WeaponHash hash);
		}

		void Drop();
		Weapon ^Give(Native::WeaponHash hash, int ammoCount, bool equipNow, bool isAmmoLoaded);
		bool Select(Weapon ^weapon);
		void Remove(Weapon ^weapon);
		void RemoveAll();

	internal:
		WeaponCollection(Ped ^owner);

	private:
		Ped ^mOwner;
		System::Collections::Generic::Dictionary<Native::WeaponHash, Weapon^> ^mWeapons;
	};
}
