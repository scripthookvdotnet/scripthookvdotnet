#pragma once

#include "Ped.hpp"

namespace GTA
{
	ref class Ped;
	ref class Weapon;

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

		Weapon ^Give(Native::WeaponHash hash);

	internal:
		WeaponCollection(Ped ^owner);

	private:
		Ped ^pOwner;
		System::Collections::Generic::Dictionary<Native::WeaponHash, Weapon^> ^weapons;
	};
}
