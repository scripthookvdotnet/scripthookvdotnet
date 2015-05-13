#pragma once

#include "WeaponHashes.hpp"
#include "Ped.hpp"

namespace GTA
{
	ref class Ped;

	public ref class Weapon sealed
	{
	public:	

		property int Ammo
		{
			int get();
			void set(int value);
		}
		property int AmmoInClip
		{
			int get();
			void set(int value);
		}
		property int MaxAmmo
		{
			int get();
		}
		property int MaxAmmoInClip
		{
			int get();
		}
		property bool IsPresent
		{
			bool get();
		}

		void Select();
		void Remove();

	internal:
		Weapon(Ped ^owner, Native::WeaponHash hash);
		Weapon() { }

	private:
		Ped ^pOwner;
		Native::WeaponHash hash;
	};
}
