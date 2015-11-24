#pragma once

#include "WeaponHashes.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Ped;
	ref class Prop;
	#pragma endregion

	public enum class WeaponTint
	{
		Normal = 0,
		Green = 1,
		Gold = 2,
		Pink = 3,
		Army = 4,
		LSPD = 5,
		Orange = 6,
		Platinum = 7
	};
	
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
		property WeaponTint Tint
		{
			void set(WeaponTint value);
			WeaponTint get();
		}

	internal:
		Weapon();
		Weapon(Ped ^owner, Native::WeaponHash hash);

	private:
		Ped ^_owner;
		Native::WeaponHash _hash;
	};

	public ref class WeaponCollection sealed
	{
	public:
		property Weapon ^Current
		{
			Weapon ^get();
		}
		property Prop ^CurrentWeaponObject
		{
			Prop ^get();
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
		Ped ^_owner;
		System::Collections::Generic::Dictionary<Native::WeaponHash, Weapon ^> ^_weapons;
	};
}
