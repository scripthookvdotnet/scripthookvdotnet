#include "Weapon.hpp"
#include "Ped.hpp"
#include "Prop.hpp"
#include "Native.hpp"
#include "Game.hpp"

namespace GTA
{
	using namespace System::Collections::Generic;

	Weapon::Weapon() : _hash(Native::WeaponHash::Unarmed)
	{
	}
	Weapon::Weapon(Ped ^owner, Native::WeaponHash hash) : _owner(owner), _hash(hash)
	{
	}

	Native::WeaponHash Weapon::Hash::get()
	{
		return _hash;
	}
	System::String ^Weapon::GetDisplayNameFromHash(Native::WeaponHash hash)
	{
		switch (hash)
		{
		case Native::WeaponHash::Pistol:
			return "WTT_PIST";
		case Native::WeaponHash::CombatPistol:
			return "WTT_PIST_CBT";
		case Native::WeaponHash::APPistol:
			return "WTT_PIST_AP";
		case Native::WeaponHash::SMG:
			return "WTT_SMG";
		case Native::WeaponHash::MicroSMG:
			return "WTT_SMG_MCR";
		case Native::WeaponHash::AssaultRifle:
			return "WTT_RIFLE_ASL";
		case Native::WeaponHash::CarbineRifle:
			return "WTT_RIFLE_CBN";
		case Native::WeaponHash::AdvancedRifle:
			return "WTT_RIFLE_ADV";
		case Native::WeaponHash::MG:
			return "WTT_MG";
		case Native::WeaponHash::CombatMG:
			return "WTT_MG_CBT";
		case Native::WeaponHash::PumpShotgun:
			return "WTT_SG_PMP";
		case Native::WeaponHash::SawnOffShotgun:
			return "WTT_SG_SOF";
		case Native::WeaponHash::AssaultShotgun:
			return "WTT_SG_ASL";
		case Native::WeaponHash::HeavySniper:
			return "WTT_SNIP_HVY";
		case Native::WeaponHash::SniperRifle:
			return "WTT_SNIP_RIF";
		case Native::WeaponHash::GrenadeLauncher:
			return "WTT_GL";
		case Native::WeaponHash::RPG:
			return "WTT_RPG";
		case Native::WeaponHash::Minigun:
			return "WTT_MINIGUN";
		case Native::WeaponHash::AssaultSMG:
			return "WTT_SMG_ASL";
		case Native::WeaponHash::BullpupShotgun:
			return "WTT_SG_BLP";
		case Native::WeaponHash::Pistol50:
			return "WTT_PIST_50";
		case Native::WeaponHash::Bottle:
			return "WTT_BOTTLE";
		case Native::WeaponHash::Gusenberg:
			return "WTT_GUSENBERG";
		case Native::WeaponHash::SNSPistol:
			return "WTT_SNSPISTOL";
		case Native::WeaponHash::VintagePistol:
			return "WTT_VPISTOL";
		case Native::WeaponHash::Dagger:
			return "WTT_DAGGER";
		case Native::WeaponHash::FlareGun:
			return "WTT_FLAREGUN";
		case Native::WeaponHash::Musket:
			return "WTT_MUSKET";
		case Native::WeaponHash::Firework:
			return "WTT_FWRKLNCHR";
		case Native::WeaponHash::MarksmanRifle:
			return "WTT_HMKRIFLE";
		case Native::WeaponHash::HeavyShotgun:
			return "WTT_HVYSHOT";
		case Native::WeaponHash::ProximityMine:
			return "WTT_PRXMINE";
		case Native::WeaponHash::HomingLauncher:
			return "WTT_HOMLNCH";
		case Native::WeaponHash::CombatPDW:
			return "WTT_COMBATPDW";
		case Native::WeaponHash::KnuckleDuster:
			return "WTT_KNUCKLE";
		case Native::WeaponHash::MarksmanPistol:
			return "WTT_MKPISTOL";
		case Native::WeaponHash::Machete:
			return "WTT_MACHETE";
		case Native::WeaponHash::MachinePistol:
			return "WTT_MCHPIST";
		case Native::WeaponHash::Flashlight:
			return "WTT_FLASHLIGHT";
		case Native::WeaponHash::DoubleBarrelShotgun:
			return "WTT_DBSHGN";
		case Native::WeaponHash::CompactRifle:
			return "WTT_CMPRIFLE";
		case Native::WeaponHash::SwitchBlade:
			return "WTT_SWBLADE";
		case Native::WeaponHash::Revolver:
			return "WTT_REVOLVER";
		}
		long long* Data = new long long[39];
		int* Ptr = (int*)Data;//Fix the native call as it doesnt accept long long* as input arg
		int max = Native::Function::Call<int>(Native::Hash::GET_NUM_DLC_WEAPONS);
		int HashInt = static_cast<int>(hash);
		for (int i = 0; i < max; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::GET_DLC_WEAPON_DATA, i, Ptr))
			{
				if (*(int*)(&(Data[1])) == HashInt)
				{

					System::String ^Result = gcnew System::String((const char*)(&Data[23]));
					delete Data;
					return Result;
				}
			}
		}
		delete Data;
		return "";

	}
	System::String ^Weapon::Name::get()
	{
		return Game::GetGXTEntry(GetDisplayNameFromHash(Hash));
	}
	int Weapon::Ammo::get()
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		else if (!IsPresent)
		{
			return 0;
		}

		return Native::Function::Call<int>(Native::Hash::GET_AMMO_IN_PED_WEAPON, _owner->Handle, static_cast<int>(Hash));
	}
	void Weapon::Ammo::set(int value)
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return;
		}

		if (IsPresent)
		{
			Native::Function::Call(Native::Hash::SET_PED_AMMO, _owner->Handle, static_cast<int>(Hash), value);
		}
		else
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_TO_PED, _owner->Handle, static_cast<int>(Hash), value, false, true);
		}
	}
	int Weapon::AmmoInClip::get()
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		else if (!IsPresent)
		{
			return 0;
		}

		int value = 0;
		Native::Function::Call(Native::Hash::GET_AMMO_IN_CLIP, _owner->Handle, static_cast<int>(Hash), &value);

		return value;
	}
	void Weapon::AmmoInClip::set(int value)
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return;
		}

		if (IsPresent)
		{
			Native::Function::Call(Native::Hash::SET_AMMO_IN_CLIP, _owner->Handle, static_cast<int>(Hash), value);
		}
		else
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_TO_PED, _owner->Handle, static_cast<int>(Hash), value, true, true);
		}
	}
	bool Weapon::CanUseOnParachute::get()
	{
		return Native::Function::Call<bool>(Native::Hash::CAN_USE_WEAPON_ON_PARACHUTE, static_cast<int>(Hash));
	}
	int Weapon::DefaultClipSize::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_WEAPON_CLIP_SIZE, static_cast<int>(Hash));
	}
	WeaponGroup Weapon::Group::get()
	{
		return static_cast<WeaponGroup>(Native::Function::Call<int>(Native::Hash::GET_WEAPONTYPE_GROUP, static_cast<int>(Hash)));
	}
	void Weapon::InfiniteAmmo::set(bool value)
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return;
		}

		Native::Function::Call(Native::Hash::SET_PED_INFINITE_AMMO, _owner->Handle, value, static_cast<int>(Hash));
	}
	void Weapon::InfiniteAmmoClip::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_INFINITE_AMMO_CLIP, _owner->Handle, value);
	}
	bool Weapon::IsPresent::get()
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return true;
		}

		return Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, _owner->Handle, static_cast<int>(Hash));
	}
	int Weapon::MaxAmmo::get()
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}

		int value = 0;
		Native::Function::Call(Native::Hash::GET_MAX_AMMO, _owner->Handle, static_cast<int>(Hash), &value);

		return value;
	}
	int Weapon::MaxAmmoInClip::get()
	{
		if (Hash == Native::WeaponHash::Unarmed)
		{
			return 1;
		}
		else if (!IsPresent)
		{
			return 0;
		}

		return Native::Function::Call<int>(Native::Hash::GET_MAX_AMMO_IN_CLIP, _owner->Handle, static_cast<int>(Hash), true);
	}
	GTA::Model Weapon::Model::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_WEAPONTYPE_MODEL, static_cast<int>(Hash));
	}
	void Weapon::Tint::set(WeaponTint value)
	{
		Native::Function::Call(Native::Hash::SET_PED_WEAPON_TINT_INDEX, _owner, static_cast<int>(Hash), static_cast<int>(value));
	}
	WeaponTint Weapon::Tint::get()
	{
		return static_cast<WeaponTint>(Native::Function::Call<int>(Native::Hash::GET_PED_WEAPON_TINT_INDEX, _owner, static_cast<int>(Hash)));
	}
	int Weapon::MaxComponents::get()
	{
		return GetComponentsFromHash(Hash)->Length;
	}
	Native::WeaponComponent Weapon::GetComponent(int index)
	{
		if (index >= MaxComponents)
		{
			return Native::WeaponComponent::Invalid;
		}
		return GetComponentsFromHash(Hash)[index];
	}
	void Weapon::SetComponent(Native::WeaponComponent component, bool on)
	{
		if (on)
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_COMPONENT_TO_PED, _owner, static_cast<int>(Hash), static_cast<int>(component));
		}
		else
		{
			Native::Function::Call(Native::Hash::REMOVE_WEAPON_COMPONENT_FROM_PED, _owner, static_cast<int>(Hash), static_cast<int>(component));
		}
	}
	bool Weapon::IsComponentActive(Native::WeaponComponent component)
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON_COMPONENT, _owner, static_cast<int>(Hash), static_cast<int>(component));
	}
	System::String ^Weapon::ComponentName(Native::WeaponComponent component)
	{
		return Game::GetGXTEntry(GetComponentDisplayNameFromHash(Hash, component));
	}
	System::String ^Weapon::GetComponentDisplayNameFromHash(Native::WeaponHash hash, Native::WeaponComponent component)
	{
		if (hash == Native::WeaponHash::KnuckleDuster)
		{
			switch (component)
			{
			case Native::WeaponComponent::KnuckleVarmodBase:
				return "WT_KNUCKLE";
			case Native::WeaponComponent::KnuckleVarmodPimp:
				return "WCT_KNUCK_02";
			case Native::WeaponComponent::KnuckleVarmodBallas:
				return "WCT_KNUCK_BG";
			case Native::WeaponComponent::KnuckleVarmodDollar:
				return "WCT_KNUCK_DLR";
			case Native::WeaponComponent::KnuckleVarmodDiamond:
				return "WCT_KNUCK_DMD";
			case Native::WeaponComponent::KnuckleVarmodHate:
				return "WCT_KNUCK_HT";
			case Native::WeaponComponent::KnuckleVarmodLove:
				return "WCT_KNUCK_LV";
			case Native::WeaponComponent::KnuckleVarmodPlayer:
				return "WCT_KNUCK_PC";
			case Native::WeaponComponent::KnuckleVarmodKing:
				return "WCT_KNUCK_SLG";
			case Native::WeaponComponent::KnuckleVarmodVagos:
				return "WCT_KNUCK_VG";
			}
		}
		else
		{
			switch (component)
			{
			case Native::WeaponComponent::AtRailCover01:
				return "WCT_RAIL";
			case Native::WeaponComponent::AtArAfGrip:
				return "WCT_GRIP";
			case Native::WeaponComponent::AtPiFlsh:
				return "WCT_FLASH";
			case Native::WeaponComponent::AtArFlsh:
				return "WCT_FLASH";
			case Native::WeaponComponent::AtScopeMacro:
				return "WCT_SCOPE_MAC";
			case Native::WeaponComponent::AtScopeMacro02:
				return "WCT_SCOPE_MAC";
			case Native::WeaponComponent::AtScopeSmall:
				return "WCT_SCOPE_SML";
			case Native::WeaponComponent::AtScopeSmall02:
				return "WCT_SCOPE_SML";
			case Native::WeaponComponent::AtScopeMedium:
				return "WCT_SCOPE_MED";
			case Native::WeaponComponent::AtScopeLarge:
				return "WCT_SCOPE_LRG";
			case Native::WeaponComponent::AtScopeMax:
				return "WCT_SCOPE_MAX";
			case Native::WeaponComponent::AtPiSupp:
				return "WCT_SUPP";
			case Native::WeaponComponent::AtArSupp:
				return "WCT_SUPP";
			case Native::WeaponComponent::AtArSupp02:
				return "WCT_SUPP";
			case Native::WeaponComponent::AtSrSupp:
				return "WCT_SUPP";
			case Native::WeaponComponent::PistolClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::PistolClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::CombatPistolClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::CombatPistolClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::APPistolClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::APPistolClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::MicroSMGClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::MicroSMGClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::SMGClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::SMGClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::AssaultRifleClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::AssaultRifleClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::CarbineRifleClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::CarbineRifleClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::AdvancedRifleClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::AdvancedRifleClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::MGClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::MGClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::CombatMGClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::CombatMGClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::AssaultShotgunClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::AssaultShotgunClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::SniperRifleClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::HeavySniperClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::MinigunClip01:
				return "WCT_CLIP2";
			case Native::WeaponComponent::AssaultSMGClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::AssaultSMGClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::Pistol50Clip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::Pistol50Clip02:
				return "WCT_CLIP2";
			case static_cast<Native::WeaponComponent>(0x0BAAB157) :
				return "WCT_CLIP1";
			case static_cast<Native::WeaponComponent>(0x5AF49386) :
				return "WCT_CLIP1";
			case static_cast<Native::WeaponComponent>(0x6CBF371B) :
				return "WCT_CLIP2";
			case static_cast<Native::WeaponComponent>(0xCAEBD246) :
				return "WCT_CLIP1";
			case static_cast<Native::WeaponComponent>(0xE1C5FFFA) :
				return "WCT_CLIP2";
			case static_cast<Native::WeaponComponent>(0xF8955D89) :
				return "WCT_CLIP1";
			case static_cast<Native::WeaponComponent>(0x3E7E6956) :
				return "WCT_CLIP2";
			case Native::WeaponComponent::SNSPistolClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::SNSPistolClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::VintagePistolClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::VintagePistolClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::HeavyShotgunClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::MarksmanRifleClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::HeavyShotgunClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::MarksmanRifleClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::AtScopeLargeFixedZoom:
				return "WCT_SCOPE_LRG";
			case Native::WeaponComponent::AtPiSupp02:
				return "WCT_SUPP";
			case Native::WeaponComponent::CombatPDWClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::CombatPDWClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::MarksmanPistolClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::MachinePistolClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::MachinePistolClip02:
				return "WCT_CLIP2";
			case Native::WeaponComponent::AssaultRifleVarmodLuxe:
				return "WCT_VAR_GOLD";
			case Native::WeaponComponent::AdvancedRifleVarmodLuxe:
				return "WCT_VAR_METAL";
			case Native::WeaponComponent::CarbineRifleVarmodLuxe:
				return "WCT_VAR_GOLD";
			case Native::WeaponComponent::APPistolVarmodLuxe:
				return "WCT_VAR_METAL";
			case Native::WeaponComponent::PistolVarmodLuxe:
				return "WCT_VAR_GOLD";
			case Native::WeaponComponent::Pistol50VarmodLuxe:
				return "WCT_VAR_SIL";
			case Native::WeaponComponent::HeavyPistolVarmodLuxe:
				return "WCT_VAR_WOOD";
			case Native::WeaponComponent::SMGVarmodLuxe:
				return "WCT_VAR_GOLD";
			case Native::WeaponComponent::MicroSMGVarmodLuxe:
				return "WCT_VAR_GOLD";
			case Native::WeaponComponent::SawnoffShotgunVarmodLuxe:
				return "WCT_VAR_METAL";
			case Native::WeaponComponent::SniperRifleVarmodLuxe:
				return "WCT_VAR_WOOD";
			case static_cast<Native::WeaponComponent>(0x161E9241) :
				return "WCT_VAR_GOLD";
			case Native::WeaponComponent::AssaultSMGVarmodLowrider:
				return "WCT_VAR_GOLD";
			case Native::WeaponComponent::BullpupRifleVarmodLow:
				return "WCT_VAR_METAL";
			case Native::WeaponComponent::CombatMGVarmodLowrider:
				return "WCT_VAR_ETCHM";
			case Native::WeaponComponent::CombatPistolVarmodLowrider:
				return "WCT_VAR_GOLD";
			case Native::WeaponComponent::MGVarmodLowrider:
				return "WCT_VAR_GOLD";
			case Native::WeaponComponent::PumpShotgunVarmodLowrider:
				return "WCT_VAR_GOLD";
			case Native::WeaponComponent::SNSPistolVarmodLowrider:
				return "WCT_VAR_WOOD";
			case Native::WeaponComponent::SpecialCarbineVarmodLowrider:
				return "WCT_VAR_ETCHM";
			case Native::WeaponComponent::SwitchbladeVarmodBase:
				return "WCT_SB_BASE";
			case Native::WeaponComponent::SwitchbladeVarmodVar1:
				return "WCT_SB_VAR1";
			case Native::WeaponComponent::SwitchbladeVarmodVar2:
				return "WCT_SB_VAR2";
			case Native::WeaponComponent::RevolverClip01:
				return "WCT_CLIP1";
			case Native::WeaponComponent::RevolverVarmodBoss:
				return "WCT_REV_VARB";
			case Native::WeaponComponent::RevolverVarmodGoon:
				return "WCT_REV_VARG";
			}
		}
		long long* Data = new long long[39];
		int* Ptr = (int*)Data;//Fix the native call as it doesnt accept long long* as input arg
		int max = Native::Function::Call<int>(Native::Hash::GET_NUM_DLC_WEAPONS);
		int HashInt = static_cast<int>(hash);
		for (int i = 0; i < max; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::GET_DLC_WEAPON_DATA, i, Ptr))
			{
				if (*(int*)(&(Data[1])) == HashInt)
				{
					int maxComp = Native::Function::Call<int>(Native::Hash::GET_NUM_DLC_WEAPON_COMPONENTS, i);
					for (int j = 0; j < maxComp; j++)
					{
						if (Native::Function::Call<bool>(Native::Hash::GET_DLC_WEAPON_COMPONENT_DATA, i, j, Ptr))
						{
							if (component == static_cast<Native::WeaponComponent>(*(int*)(&Data[3])))
							{
								System::String ^Result = gcnew System::String((const char*)(&Data[6]));
								delete Data;
								return Result;
							}
						}
					}
					
				}
			}
		}
		delete Data;
		return "WCT_INVALID";

	}


	array<Native::WeaponComponent> ^Weapon::GetComponentsFromHash(Native::WeaponHash hash)
	{
		switch (hash)
		{
		case Native::WeaponHash::Pistol:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::PistolClip01,
					Native::WeaponComponent::PistolClip02,
					Native::WeaponComponent::AtPiFlsh,
					Native::WeaponComponent::AtPiSupp02,
					Native::WeaponComponent::PistolVarmodLuxe,
			};
		}
		case Native::WeaponHash::CombatPistol:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::CombatPistolClip01,
					Native::WeaponComponent::CombatPistolClip02,
					Native::WeaponComponent::AtPiFlsh,
					Native::WeaponComponent::AtPiSupp,
					Native::WeaponComponent::CombatPistolVarmodLowrider,
			};
		}
		case Native::WeaponHash::APPistol:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::APPistolClip01,
					Native::WeaponComponent::APPistolClip02,
					Native::WeaponComponent::AtPiFlsh,
					Native::WeaponComponent::AtPiSupp,
					Native::WeaponComponent::APPistolVarmodLuxe,
			};
		}
		case Native::WeaponHash::MicroSMG:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::MicroSMGClip01,
					Native::WeaponComponent::MicroSMGClip02,
					Native::WeaponComponent::AtPiFlsh,
					Native::WeaponComponent::AtScopeMacro,
					Native::WeaponComponent::AtArSupp02,
					Native::WeaponComponent::MicroSMGVarmodLuxe,
			};
		}
		case Native::WeaponHash::SMG:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::SMGClip01,
					Native::WeaponComponent::SMGClip02,
					Native::WeaponComponent::AtArFlsh,
					Native::WeaponComponent::AtPiSupp,
					Native::WeaponComponent::AtScopeMacro02,
					Native::WeaponComponent::AtArAfGrip,
					Native::WeaponComponent::SMGVarmodLuxe,
			};
		}
		case Native::WeaponHash::AssaultRifle:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::AssaultRifleClip01,
					Native::WeaponComponent::AssaultRifleClip02,
					Native::WeaponComponent::AtArAfGrip,
					Native::WeaponComponent::AtArFlsh,
					Native::WeaponComponent::AtScopeMacro,
					Native::WeaponComponent::AtArSupp02,
					Native::WeaponComponent::AssaultRifleVarmodLuxe,
			};
		}
		case Native::WeaponHash::CarbineRifle:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::CarbineRifleClip01,
					Native::WeaponComponent::CarbineRifleClip02,
					Native::WeaponComponent::AtRailCover01,
					Native::WeaponComponent::AtArAfGrip,
					Native::WeaponComponent::AtArFlsh,
					Native::WeaponComponent::AtScopeMedium,
					Native::WeaponComponent::AtArSupp,
					Native::WeaponComponent::CarbineRifleVarmodLuxe,
			};
		}
		case Native::WeaponHash::AdvancedRifle:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::AdvancedRifleClip01,
					Native::WeaponComponent::AdvancedRifleClip02,
					Native::WeaponComponent::AtArFlsh,
					Native::WeaponComponent::AtScopeSmall,
					Native::WeaponComponent::AtArSupp,
					Native::WeaponComponent::AdvancedRifleVarmodLuxe,
			};
		}
		case Native::WeaponHash::MG:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::MGClip01,
					Native::WeaponComponent::MGClip02,
					Native::WeaponComponent::AtScopeSmall02,
					Native::WeaponComponent::AtArAfGrip,
					Native::WeaponComponent::MGVarmodLowrider,
			};
		}
		case Native::WeaponHash::CombatMG:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::CombatMGClip01,
					Native::WeaponComponent::CombatMGClip02,
					Native::WeaponComponent::AtArAfGrip,
					Native::WeaponComponent::AtScopeMedium,
					Native::WeaponComponent::CombatMGVarmodLowrider,
			};
		}
		case Native::WeaponHash::PumpShotgun:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::AtSrSupp,
					Native::WeaponComponent::AtArFlsh,
					Native::WeaponComponent::PumpShotgunVarmodLowrider,
			};
		}
		case Native::WeaponHash::AssaultShotgun:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::AssaultShotgunClip01,
					Native::WeaponComponent::AssaultShotgunClip02,
					Native::WeaponComponent::AtArAfGrip,
					Native::WeaponComponent::AtArFlsh,
					Native::WeaponComponent::AtArSupp,
			};
		}
		case Native::WeaponHash::SniperRifle:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::SniperRifleClip01,
					Native::WeaponComponent::AtScopeLarge,
					Native::WeaponComponent::AtScopeMax,
					Native::WeaponComponent::AtArSupp02,
					Native::WeaponComponent::SniperRifleVarmodLuxe,
			};
		}
		case Native::WeaponHash::HeavySniper:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::HeavySniperClip01,
					Native::WeaponComponent::AtScopeLarge,
					Native::WeaponComponent::AtScopeMax,
			};
		}
		case Native::WeaponHash::GrenadeLauncher:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::AtArAfGrip,
					Native::WeaponComponent::AtArFlsh,
					Native::WeaponComponent::AtScopeSmall,
			};
		}
		case Native::WeaponHash::Minigun:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::MinigunClip01,
			};
		}
		case Native::WeaponHash::AssaultSMG:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::AssaultSMGClip01,
					Native::WeaponComponent::AssaultSMGClip02,
					Native::WeaponComponent::AtArFlsh,
					Native::WeaponComponent::AtScopeMacro,
					Native::WeaponComponent::AtArSupp02,
					Native::WeaponComponent::AssaultSMGVarmodLowrider,
			};
		}
		case Native::WeaponHash::BullpupShotgun:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::AtArAfGrip,
					Native::WeaponComponent::AtArFlsh,
					Native::WeaponComponent::AtArSupp02,
			};
		}
		case Native::WeaponHash::Pistol50:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::Pistol50Clip01,
					Native::WeaponComponent::Pistol50Clip02,
					Native::WeaponComponent::AtPiFlsh,
					Native::WeaponComponent::AtArSupp02,
					Native::WeaponComponent::Pistol50VarmodLuxe,
			};
		}
		case Native::WeaponHash::CombatPDW:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::CombatPDWClip01,
					Native::WeaponComponent::CombatPDWClip02,
					Native::WeaponComponent::AtArFlsh,
					Native::WeaponComponent::AtScopeSmall,
					Native::WeaponComponent::AtArAfGrip,
			};
		}
		case Native::WeaponHash::SawnOffShotgun:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::SawnoffShotgunVarmodLuxe,
			};
		}
		case Native::WeaponHash::BullpupRifle:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::BullpupRifleClip01,
					Native::WeaponComponent::BullpupRifleClip02,
					Native::WeaponComponent::AtArFlsh,
					Native::WeaponComponent::AtScopeSmall,
					Native::WeaponComponent::AtArSupp,
					Native::WeaponComponent::AtArAfGrip,
					Native::WeaponComponent::BullpupRifleVarmodLow,
			};
		}
		case Native::WeaponHash::SNSPistol:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::SNSPistolClip01,
					Native::WeaponComponent::SNSPistolClip02,
					Native::WeaponComponent::SNSPistolVarmodLowrider,
			};
		}
		case Native::WeaponHash::SpecialCarbine:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::SpecialCarbineClip01,
					Native::WeaponComponent::SpecialCarbineClip02,
					Native::WeaponComponent::AtArFlsh,
					Native::WeaponComponent::AtScopeMedium,
					Native::WeaponComponent::AtArSupp02,
					Native::WeaponComponent::AtArAfGrip,
					Native::WeaponComponent::SpecialCarbineVarmodLowrider,
			};
		}
		case Native::WeaponHash::KnuckleDuster:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::KnuckleVarmodPimp,
					Native::WeaponComponent::KnuckleVarmodBallas,
					Native::WeaponComponent::KnuckleVarmodDollar,
					Native::WeaponComponent::KnuckleVarmodDiamond,
					Native::WeaponComponent::KnuckleVarmodHate,
					Native::WeaponComponent::KnuckleVarmodLove,
					Native::WeaponComponent::KnuckleVarmodPlayer,
					Native::WeaponComponent::KnuckleVarmodKing,
					Native::WeaponComponent::KnuckleVarmodVagos,
			};
		}
		case Native::WeaponHash::MachinePistol:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::MachinePistolClip01,
					Native::WeaponComponent::MachinePistolClip02,
					Native::WeaponComponent::AtPiSupp,
			};
		}
		case Native::WeaponHash::SwitchBlade:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::SwitchbladeVarmodVar1,
					Native::WeaponComponent::SwitchbladeVarmodVar2,
			};
		}
		case Native::WeaponHash::Revolver:
		{
			return gcnew array<Native::WeaponComponent>{
				Native::WeaponComponent::RevolverClip01,
					Native::WeaponComponent::RevolverVarmodBoss,
					Native::WeaponComponent::RevolverVarmodGoon,
			};
		}
		}
		long long* Data = new long long[39];
		int* Ptr = (int*)Data;//Fix the native call as it doesnt accept long*
		int max = Native::Function::Call<int>(Native::Hash::GET_NUM_DLC_WEAPONS);
		int HashInt = static_cast<int>(hash);
		for (int i = 0; i < max; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::GET_DLC_WEAPON_DATA, i, Ptr))
			{
				if (*(int*)(&(Data[1])) == HashInt)
				{
					int maxComp = Native::Function::Call<int>(Native::Hash::GET_NUM_DLC_WEAPON_COMPONENTS, i);
					array<Native::WeaponComponent> ^Components = gcnew array<Native::WeaponComponent>(maxComp);
					for (int j = 0; j < maxComp; j++)
					{
						if (Native::Function::Call<bool>(Native::Hash::GET_DLC_WEAPON_COMPONENT_DATA, i, j, Ptr))
						{
							Components[j] = static_cast<Native::WeaponComponent>(*(int*)(&Data[3]));
						}
						else
						{
							Components[j] = Native::WeaponComponent::Invalid;
						}
					}
					delete Data;
					return Components;
				}
			}
		}
		delete Data;
		return gcnew array<Native::WeaponComponent>(0);
	}



	WeaponCollection::WeaponCollection(Ped ^owner) : _owner(owner), _weapons(gcnew Dictionary<System::UInt32, Weapon ^>())
	{
	}

	Weapon ^WeaponCollection::Current::get()
	{
		unsigned int hash;
		Native::WeaponHash thash;

		Native::Function::Call(Native::Hash::GET_CURRENT_PED_WEAPON, _owner->Handle, &hash, true);
		thash = static_cast<Native::WeaponHash>(hash);

		if (_weapons->ContainsKey(hash))
		{
			return _weapons->default[hash];
		}

		Weapon ^weapon = gcnew Weapon(_owner, thash);
		_weapons->Add(hash, weapon);

		return weapon;
	}
	Prop ^WeaponCollection::CurrentWeaponObject::get()
	{
		if (Current->Hash != Native::WeaponHash::Unarmed)
		{
			return Native::Function::Call<Prop^>(Native::Hash::_0x3B390A939AF0B5FC, _owner->Handle);
		}

		return nullptr;
	}
	Weapon ^WeaponCollection::BestWeapon::get()
	{
		System::UInt32 hash = 0;
		Native::WeaponHash thash;

		hash = Native::Function::Call<int>(Native::Hash::GET_BEST_PED_WEAPON, _owner->Handle, 0);
		thash = static_cast<Native::WeaponHash>(hash);

		if (_weapons->ContainsKey(hash))
		{
			return _weapons->default[hash];
		}

		Weapon ^weapon = gcnew Weapon(_owner, thash);
		_weapons->Add(hash, weapon);

		return weapon;
	}
	Weapon ^WeaponCollection::default::get(Native::WeaponHash hash)
	{
		Weapon ^weapon;
		System::UInt32 uintHash = static_cast<System::UInt32>(hash);

		if (_weapons->TryGetValue(uintHash, weapon))
		{
			return weapon;
		}

		if (!Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, _owner->Handle, static_cast<int>(hash), 0))
		{
			return nullptr;
		}

		weapon = gcnew Weapon(_owner, hash);
		_weapons->Add(uintHash, weapon);

		return weapon;
	}

	void WeaponCollection::Drop()
	{
		Native::Function::Call(Native::Hash::SET_PED_DROPS_WEAPON, _owner);
	}
	Weapon ^WeaponCollection::Give(Native::WeaponHash hash, int ammoCount, bool equipNow, bool isAmmoLoaded)
	{
		Weapon ^weapon;
		System::UInt32 uintHash = static_cast<System::UInt32>(hash);

		if (!_weapons->TryGetValue(static_cast<System::UInt32>(hash), weapon))
		{
			weapon = gcnew Weapon(_owner, hash);
			_weapons->Add(uintHash, weapon);
		}

		if (weapon->IsPresent)
		{
			Select(weapon);
		}
		else
		{
			Native::Function::Call(Native::Hash::GIVE_WEAPON_TO_PED, _owner->Handle, static_cast<int>(weapon->Hash), ammoCount, equipNow, isAmmoLoaded);
		}

		return weapon;
	}
	bool WeaponCollection::HasWeapon(Native::WeaponHash weaponHash)
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, _owner->Handle, static_cast<int>(weaponHash));
	}
	bool WeaponCollection::IsWeaponValid(Native::WeaponHash hash)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_WEAPON_VALID, static_cast<int>(hash));
	}
	bool WeaponCollection::Select(Weapon ^weapon)
	{
		if (!weapon->IsPresent)
		{
			return false;
		}

		Native::Function::Call(Native::Hash::SET_CURRENT_PED_WEAPON, _owner->Handle, static_cast<int>(weapon->Hash), true);

		return true;
	}
	bool WeaponCollection::Select(Native::WeaponHash weaponHash)
	{
		return WeaponCollection::Select(weaponHash, true);
	}
	bool WeaponCollection::Select(Native::WeaponHash weaponHash, bool equipNow)
	{
		if (!Native::Function::Call<bool>(Native::Hash::HAS_PED_GOT_WEAPON, _owner->Handle, static_cast<int>(weaponHash)))
		{
			return false;
		}

		Native::Function::Call(Native::Hash::SET_CURRENT_PED_WEAPON, _owner->Handle, static_cast<int>(weaponHash), equipNow);

		return true;
	}
	void WeaponCollection::Remove(Weapon ^weapon)
	{
		System::UInt32 hash = static_cast<System::UInt32>(weapon->Hash);

		if (_weapons->ContainsKey(hash))
		{
			_weapons->Remove(hash);
		}

		WeaponCollection::Remove(weapon->Hash);
	}
	void WeaponCollection::Remove(Native::WeaponHash weaponHash)
	{
		Native::Function::Call(Native::Hash::REMOVE_WEAPON_FROM_PED, _owner->Handle, static_cast<int>(weaponHash));
	}
	void WeaponCollection::RemoveAll()
	{
		Native::Function::Call(Native::Hash::REMOVE_ALL_PED_WEAPONS, _owner->Handle, true);

		_weapons->Clear();
	}
}