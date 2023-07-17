//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System.Linq;

namespace GTA
{
	public sealed class Weapon
	{
		#region Fields

		private readonly Ped _owner;
		#endregion

		internal Weapon()
		{
			Hash = WeaponHash.Unarmed;
		}
		internal Weapon(Ped owner, WeaponHash hash)
		{
			this._owner = owner;
			Hash = hash;
		}

		public WeaponHash Hash
		{
			get;
			private set;
		}

		public string Name
		{
			get => SHVDN.NativeMemory.GetGxtEntryByHash((int)SHVDN.NativeMemory.GetHumanNameHashOfWeaponInfo((uint)Hash));
		}

		public string ComponentName(WeaponComponent component)
		{
			return Game.GetGXTEntry(GetComponentDisplayNameFromHash(Hash, component));
		}

		public bool IsPresent => Hash == WeaponHash.Unarmed || Function.Call<bool>(Native.Hash.HAS_PED_GOT_WEAPON, _owner.Handle, (uint)Hash);

		public Model Model => new Model(Function.Call<int>(Native.Hash.GET_WEAPONTYPE_MODEL, (uint)Hash));

		public WeaponTint Tint
		{
			get => Function.Call<WeaponTint>(Native.Hash.GET_PED_WEAPON_TINT_INDEX, _owner.Handle, (uint)Hash);
			set => Function.Call(Native.Hash.SET_PED_WEAPON_TINT_INDEX, _owner.Handle, (uint)Hash, (int)value);
		}

		public WeaponGroup Group => Function.Call<WeaponGroup>(Native.Hash.GET_WEAPONTYPE_GROUP, (uint)Hash);

		public int Ammo
		{
			get
			{
				if (Hash == WeaponHash.Unarmed)
				{
					return 1;
				}

				if (!IsPresent)
				{
					return 0;
				}

				return Function.Call<int>(Native.Hash.GET_AMMO_IN_PED_WEAPON, _owner.Handle, (uint)Hash);
			}
			set
			{
				if (Hash == WeaponHash.Unarmed)
				{
					return;
				}

				if (IsPresent)
				{
					Function.Call(Native.Hash.SET_PED_AMMO, _owner.Handle, (uint)Hash, value);
				}
				else
				{
					Function.Call(Native.Hash.GIVE_WEAPON_TO_PED, _owner.Handle, (uint)Hash, value, false, true);
				}
			}
		}
		public int AmmoInClip
		{
			get
			{
				if (Hash == WeaponHash.Unarmed)
				{
					return 1;
				}

				if (!IsPresent)
				{
					return 0;
				}

				int ammoInClip;
				unsafe
				{
					Function.Call(Native.Hash.GET_AMMO_IN_CLIP, _owner.Handle, (uint)Hash, &ammoInClip);
				}
				return ammoInClip;
			}
			set
			{
				if (Hash == WeaponHash.Unarmed)
				{
					return;
				}

				if (IsPresent)
				{
					Function.Call(Native.Hash.SET_AMMO_IN_CLIP, _owner.Handle, (uint)Hash, value);
				}
				else
				{
					Function.Call(Native.Hash.GIVE_WEAPON_TO_PED, _owner.Handle, (uint)Hash, value, true, false);
				}
			}
		}

		public int MaxAmmo
		{
			get
			{
				if (Hash == WeaponHash.Unarmed)
				{
					return 1;
				}

				int maxAmmo;
				unsafe
				{
					Function.Call(Native.Hash.GET_MAX_AMMO, _owner.Handle, (uint)Hash, &maxAmmo);
				}
				return maxAmmo;
			}
		}
		public int MaxAmmoInClip
		{
			get
			{
				if (Hash == WeaponHash.Unarmed)
				{
					return 1;
				}

				if (!IsPresent)
				{
					return 0;
				}

				return Function.Call<int>(Native.Hash.GET_MAX_AMMO_IN_CLIP, _owner.Handle, (uint)Hash, true);
			}
		}

		public int MaxComponents => GetComponentsFromHash(Hash).Length;

		public int DefaultClipSize => Function.Call<int>(Native.Hash.GET_WEAPON_CLIP_SIZE, (uint)Hash);

		public bool InfiniteAmmo
		{
			set
			{
				if (Hash == WeaponHash.Unarmed)
				{
					return;
				}

				Function.Call(Native.Hash.SET_PED_INFINITE_AMMO, _owner.Handle, value, (uint)Hash);
			}
		}
		public bool InfiniteAmmoClip
		{
			set => Function.Call(Native.Hash.SET_PED_INFINITE_AMMO_CLIP, _owner.Handle, value);
		}

		public bool CanUseOnParachute => Function.Call<bool>(Native.Hash.CAN_USE_WEAPON_ON_PARACHUTE, (uint)Hash);

		public WeaponComponent GetComponent(int index)
		{
			if (index >= MaxComponents)
			{
				return WeaponComponent.Invalid;
			}

			return GetComponentsFromHash(Hash)[index];
		}

		public void SetComponent(WeaponComponent component, bool on)
		{
			if (on)
			{
				Function.Call(Native.Hash.GIVE_WEAPON_COMPONENT_TO_PED, _owner, (uint)Hash, (uint)component);
			}
			else
			{
				Function.Call(Native.Hash.REMOVE_WEAPON_COMPONENT_FROM_PED, _owner, (uint)Hash, (uint)component);
			}
		}

		public bool IsComponentActive(WeaponComponent component)
		{
			return Function.Call<bool>(Native.Hash.HAS_PED_GOT_WEAPON_COMPONENT, _owner, (uint)Hash, (uint)component);
		}

		public static string GetDisplayNameFromHash(WeaponHash hash)
		{
			// Will be found in this switch statement if the hash is one of the weapon hashes for singleplayer
			switch (hash)
			{
				case WeaponHash.Unarmed:
					return "WT_UNARMED";
				case WeaponHash.Knife:
					return "WT_KNIFE";
				case WeaponHash.Nightstick:
					return "WT_NGTSTK";
				case WeaponHash.Hammer:
					return "WT_HAMMER";
				case WeaponHash.Bat:
					return "WT_BAT";
				case WeaponHash.Crowbar:
					return "WT_CROWBAR";
				case WeaponHash.GolfClub:
					return "WT_GOLFCLUB";
				case WeaponHash.Pistol:
					return "WT_PIST";
				case WeaponHash.CombatPistol:
					return "WT_PIST_CBT";
				case WeaponHash.Pistol50:
					return "WT_PIST_50";
				case WeaponHash.APPistol:
					return "WT_PIST_AP";
				case WeaponHash.StunGun:
					return "WT_STUN";
				case WeaponHash.MicroSMG:
					return "WT_SMG_MCR";
				case WeaponHash.SMG:
					return "WT_SMG";
				case WeaponHash.AssaultSMG:
					return "WT_SMG_ASL";
				case WeaponHash.AssaultRifle:
					return "WT_RIFLE_ASL";
				case WeaponHash.CarbineRifle:
					return "WT_RIFLE_CBN";
				case WeaponHash.AdvancedRifle:
					return "WT_RIFLE_ADV";
				case WeaponHash.MG:
					return "WT_MG";
				case WeaponHash.CombatMG:
					return "WT_MG_CBT";
				case WeaponHash.PumpShotgun:
					return "WT_SG_PMP";
				case WeaponHash.SawnOffShotgun:
					return "WT_SG_SOF";
				case WeaponHash.AssaultShotgun:
					return "WT_SG_ASL";
				case WeaponHash.BullpupShotgun:
					return "WT_SG_BLP";
				case WeaponHash.SniperRifle:
					return "WT_SNIP_RIF";
				case WeaponHash.HeavySniper:
					return "WT_SNIP_HVY";
				case WeaponHash.GrenadeLauncher:
					return "WT_GL";
				case WeaponHash.RPG:
					return "WT_RPG";
				case WeaponHash.Minigun:
					return "WT_MINIGUN";
				case WeaponHash.Grenade:
					return "WT_GNADE";
				case WeaponHash.StickyBomb:
					return "WT_GNADE_STK";
				case WeaponHash.SmokeGrenade:
					return "WT_GNADE_SMK";
				case WeaponHash.BZGas:
					return "WT_BZGAS";
				case WeaponHash.Molotov:
					return "WT_MOLOTOV";
				case WeaponHash.FireExtinguisher:
					return "WT_FIRE";
				case WeaponHash.PetrolCan:
					return "WT_PETROL";
				case WeaponHash.Ball:
					return "WT_BALL";
				case WeaponHash.Flare:
					return "WT_FLARE";
				case WeaponHash.Bottle:
					return "WT_BOTTLE";
				case WeaponHash.Dagger:
					return "WT_DAGGER";
				case WeaponHash.Hatchet:
					return "WT_HATCHET";
				case WeaponHash.Machete:
					return "WT_MACHETE";
				case WeaponHash.KnuckleDuster:
					return "WT_KNUCKLE";
				case WeaponHash.SNSPistol:
					return "WT_SNSPISTOL";
				case WeaponHash.VintagePistol:
					return "WT_VPISTOL";
				case WeaponHash.HeavyPistol:
					return "WT_HVYPISTOL";
				case WeaponHash.MarksmanPistol:
					return "WT_MKPISTOL";
				case WeaponHash.Gusenberg:
					return "WT_GUSENBERG";
				case WeaponHash.MachinePistol:
					return "WT_MCHPIST";
				case WeaponHash.CombatPDW:
					return "WT_COMBATPDW";
				case WeaponHash.SpecialCarbine:
					return "WT_SPCARBINE";
				case WeaponHash.HeavyShotgun:
					return "WT_HVYSHOT";
				case WeaponHash.Musket:
					return "WT_MUSKET";
				case WeaponHash.MarksmanRifle:
					return "WT_MKRIFLE";
				case WeaponHash.Firework:
					return "WT_FWRKLNCHR";
				case WeaponHash.HomingLauncher:
					return "WT_HOMLNCH";
				case WeaponHash.Railgun:
					return "WT_RAILGUN";
				case WeaponHash.ProximityMine:
					return "WT_PRXMINE";
				// there is no WeaponShopItem for the weapon snowballs, so listed here
				case WeaponHash.Snowball:
					return "WT_SNWBALL";
			}

			DlcWeaponData data;
			for (int i = 0, max = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPONS); i < max; i++)
			{
				unsafe
				{
					if (!Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_DATA, i, (int*)&data))
					{
						continue;
					}

					if (data.Hash == hash)
					{
						return data.DisplayName;
					}
				}
			}

			return "WT_INVALID";
		}

		public static string GetComponentDisplayNameFromHash(WeaponHash hash, WeaponComponent component)
		{
			// Will be found in this switch statement if the hash is one of the weapon component hashes for singleplayer
			switch (component)
			{
				case WeaponComponent.Invalid:
					return "WCT_INVALID";
				case WeaponComponent.AtRailCover01:
					return "WCT_RAIL";
				case WeaponComponent.AtArAfGrip:
					return "WCT_GRIP";
				case WeaponComponent.AtPiFlsh:
				case WeaponComponent.AtArFlsh:
					return "WCT_FLASH";
				case WeaponComponent.AtScopeMacro:
				case WeaponComponent.AtScopeMacro02:
					return "WCT_SCOPE_MAC";
				case WeaponComponent.AtScopeSmall:
				case WeaponComponent.AtScopeSmall02:
					return "WCT_SCOPE_SML";
				case WeaponComponent.AtScopeMedium:
					return "WCT_SCOPE_MED";
				case WeaponComponent.AtScopeLarge:
				case WeaponComponent.AtScopeLargeFixedZoom:
					return "WCT_SCOPE_LRG";
				case WeaponComponent.AtScopeMax:
					return "WCT_SCOPE_MAX";
				case WeaponComponent.AtPiSupp:
				case WeaponComponent.AtArSupp:
				case WeaponComponent.AtSrSupp:
					return "WCT_SUPP";
				case WeaponComponent.PistolClip01:
				case WeaponComponent.CombatPistolClip01:
				case WeaponComponent.APPistolClip01:
				case WeaponComponent.MicroSMGClip01:
				case WeaponComponent.SMGClip01:
				case WeaponComponent.AssaultRifleClip01:
				case WeaponComponent.CarbineRifleClip01:
				case WeaponComponent.AdvancedRifleClip01:
				case WeaponComponent.MGClip01:
				case WeaponComponent.CombatMGClip01:
				case WeaponComponent.AssaultShotgunClip01:
				case WeaponComponent.SniperRifleClip01:
				case WeaponComponent.HeavySniperClip01:
				case WeaponComponent.AssaultSMGClip01:
				case WeaponComponent.Pistol50Clip01:
				case (WeaponComponent)0x0BAAB157:
				case (WeaponComponent)0x5AF49386:
				case (WeaponComponent)0xCAEBD246:
				case (WeaponComponent)0xF8955D89:
				case WeaponComponent.SNSPistolClip01:
				case WeaponComponent.VintagePistolClip01:
				case WeaponComponent.HeavyShotgunClip01:
				case WeaponComponent.MarksmanRifleClip01:
				case WeaponComponent.CombatPDWClip01:
				case WeaponComponent.MarksmanPistolClip01:
				case WeaponComponent.MachinePistolClip01:
					return "WCT_CLIP1";
				case WeaponComponent.PistolClip02:
				case WeaponComponent.CombatPistolClip02:
				case WeaponComponent.APPistolClip02:
				case WeaponComponent.MicroSMGClip02:
				case WeaponComponent.SMGClip02:
				case WeaponComponent.AssaultRifleClip02:
				case WeaponComponent.CarbineRifleClip02:
				case WeaponComponent.AdvancedRifleClip02:
				case WeaponComponent.MGClip02:
				case WeaponComponent.CombatMGClip02:
				case WeaponComponent.AssaultShotgunClip02:
				case WeaponComponent.MinigunClip01:
				case WeaponComponent.AssaultSMGClip02:
				case WeaponComponent.Pistol50Clip02:
				case (WeaponComponent)0x6CBF371B:
				case (WeaponComponent)0xE1C5FFFA:
				case (WeaponComponent)0x3E7E6956:
				case WeaponComponent.SNSPistolClip02:
				case WeaponComponent.VintagePistolClip02:
				case WeaponComponent.HeavyShotgunClip02:
				case WeaponComponent.MarksmanRifleClip02:
				case WeaponComponent.CombatPDWClip02:
				case WeaponComponent.MachinePistolClip02:
					return "WCT_CLIP2";
				case WeaponComponent.AssaultRifleVarmodLuxe:
				case WeaponComponent.CarbineRifleVarmodLuxe:
				case WeaponComponent.PistolVarmodLuxe:
				case WeaponComponent.SMGVarmodLuxe:
				case WeaponComponent.MicroSMGVarmodLuxe:
				case WeaponComponent.MarksmanRifleVarmodLuxe:
				case WeaponComponent.AssaultSMGVarmodLowrider:
				case WeaponComponent.CombatPistolVarmodLowrider:
				case WeaponComponent.MGVarmodLowrider:
				case WeaponComponent.PumpShotgunVarmodLowrider:
					return "WCT_VAR_GOLD";
				case WeaponComponent.AdvancedRifleVarmodLuxe:
				case WeaponComponent.APPistolVarmodLuxe:
				case WeaponComponent.SawnoffShotgunVarmodLuxe:
				case WeaponComponent.BullpupRifleVarmodLow:
					return "WCT_VAR_METAL";
				case WeaponComponent.Pistol50VarmodLuxe:
					return "WCT_VAR_SIL";
				case WeaponComponent.HeavyPistolVarmodLuxe:
				case WeaponComponent.SniperRifleVarmodLuxe:
				case WeaponComponent.SNSPistolVarmodLowrider:
					return "WCT_VAR_WOOD";
				case WeaponComponent.CombatMGVarmodLowrider:
				case WeaponComponent.SpecialCarbineVarmodLowrider:
					return "WCT_VAR_ETCHM";
				case WeaponComponent.SMGClip03:
				case WeaponComponent.AssaultRifleClip03:
				case WeaponComponent.HeavyShotgunClip03:
					return "WCT_CLIP_DRM";
				case WeaponComponent.CarbineRifleClip03:
					return "WCT_CLIP_BOX";
			}

			DlcWeaponData data;
			DlcWeaponComponentData componentData;
			for (int i = 0, max = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPONS); i < max; i++)
			{
				unsafe
				{
					if (!Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_DATA, i, (int*)&data))
					{
						continue;
					}

					if (data.Hash != hash)
					{
						continue;
					}

					int maxComp = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPON_COMPONENTS, i);

					for (int j = 0; j < maxComp; j++)
					{
						if (!Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_COMPONENT_DATA, i, j, (int*)&componentData))
						{
							continue;
						}

						if (componentData.Hash == component)
						{
							return componentData.DisplayName;
						}
					}
				}
			}

			return "WCT_INVALID";
		}

		public static WeaponComponent[] GetComponentsFromHash(WeaponHash hash)
		{
			return SHVDN.NativeMemory.GetAllCompatibleWeaponComponentHashes((uint)hash).Select(x => (WeaponComponent)x).ToArray();
		}
	}
}
