#pragma once

#include "WeaponHashes.hpp"

namespace GTA
{
	ref class Ped;

	public ref class Weapon sealed
	{
	public:	
		property Native::WeaponHash Hash
		{
			Native::WeaponHash get();
		}
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
		property bool InfiniteAmmo
		{
			void set(bool value);
		}
		property bool InfiniteAmmoClip
		{
			void set(bool value);
		}
		property bool IsPresent
		{
			bool get();
		}
		property int MaxAmmo
		{
			int get();
		}
		property int MaxAmmoInClip
		{
			int get();
		}

	internal:
		Weapon() { }
		Weapon(Ped ^owner, Native::WeaponHash hash);

	private:
		Ped ^mOwner;
		Native::WeaponHash mHash;
	};
}
