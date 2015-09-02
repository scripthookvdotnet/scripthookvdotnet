#pragma once

#include "WeaponHashes.hpp"

namespace GTA
{
	ref class Ped;
	ref class Prop;

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
		property Prop ^CurrentWeaponObject
		{
			Prop ^get();
		}

	internal:
		WeaponCollection(Ped ^owner);

	private:
		Ped ^mOwner;
		System::Collections::Generic::Dictionary<Native::WeaponHash, Weapon^> ^mWeapons;
	};
}
