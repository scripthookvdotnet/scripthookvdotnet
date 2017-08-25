#pragma once

#include "Model.hpp"
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

	public enum class WeaponGroup : System::UInt32
	{
		Unarmed = 0xA00FC1E4,
		Melee = 0xD49321D4,
		Pistol = 0x18D5FA97,
		SMG = 0xC6E9A5C5,
		AssaultRifle = 0x39D5C192,
		MG = 0x451B04BC,
		Shotgun = 0x33431399,
		Sniper = 0xB7BBD827,
		Heavy = 0xA27A4F9F,
		Thrown = 0x5C4C5883,
		PetrolCan = 0x5F1BE07C,
	};
	
	public ref class Weapon sealed
	{
	public:	
		property Native::WeaponHash Hash
		{
			Native::WeaponHash get();
		}

		property System::String ^Name
		{
			System::String ^get();
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
		property bool CanUseOnParachute
		{
			bool get();
		}
		property int DefaultClipSize
		{
			int get();
		}
		property WeaponGroup Group
		{
			WeaponGroup get();
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
		property GTA::Model Model
		{
			GTA::Model get();
		}
		property WeaponTint Tint
		{
			void set(WeaponTint value);
			WeaponTint get();
		}
		property int MaxComponents
		{
			int get();
		}
		Native::WeaponComponent GetComponent(int index);
		void SetComponent(Native::WeaponComponent component, bool on);
		bool IsComponentActive(Native::WeaponComponent component);
		System::String ^ComponentName(Native::WeaponComponent component);

		static System::String ^GetDisplayNameFromHash(Native::WeaponHash Hash);
		static array<Native::WeaponComponent> ^GetComponentsFromHash(Native::WeaponHash Hash);
		static System::String ^GetComponentDisplayNameFromHash(Native::WeaponHash Hash, Native::WeaponComponent component);

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
		property Weapon ^BestWeapon
		{
			Weapon ^get();
		}
		property Weapon ^default[Native::WeaponHash]
		{
			Weapon ^get(Native::WeaponHash hash);
		}

		void Drop();
		Weapon ^Give(Native::WeaponHash hash, int ammoCount, bool equipNow, bool isAmmoLoaded);
		bool HasWeapon(Native::WeaponHash weaponHash);
		bool IsWeaponValid(Native::WeaponHash hash);
		bool Select(Weapon ^weapon);
		bool Select(Native::WeaponHash weaponHash);
		bool Select(Native::WeaponHash weaponHash, bool equipNow);
		void Remove(Weapon ^weapon);
		void Remove(Native::WeaponHash weaponHash);
		void RemoveAll();

	internal:
		WeaponCollection(Ped ^owner);

	private:
		Ped ^_owner;
		System::Collections::Generic::Dictionary<System::UInt32, Weapon ^> ^_weapons;
	};
}
