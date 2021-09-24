//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;
using System.Linq;

namespace GTA
{
	public sealed class Weapon
	{
		#region Fields
		readonly Ped owner;
		#endregion

		internal Weapon()
		{
			Hash = WeaponHash.Unarmed;
		}
		internal Weapon(Ped owner, WeaponHash hash)
		{
			this.owner = owner;
			Hash = hash;
		}

		public WeaponHash Hash
		{
			get;
			private set;
		}

		public string Name
		{
			get => Game.GetGXTEntry(GetDisplayNameFromHash(Hash));
		}

		public string ComponentName(WeaponComponent component)
		{
			return Game.GetGXTEntry(GetComponentDisplayNameFromHash(Hash, component));
		}

		public bool IsPresent => Hash == WeaponHash.Unarmed || Function.Call<bool>(Native.Hash.HAS_PED_GOT_WEAPON, owner.Handle, (uint)Hash);

		public Model Model => new Model(Function.Call<int>(Native.Hash.GET_WEAPONTYPE_MODEL, (uint)Hash));

		public WeaponTint Tint
		{
			get => Function.Call<WeaponTint>(Native.Hash.GET_PED_WEAPON_TINT_INDEX, owner.Handle, (uint)Hash);
			set => Function.Call(Native.Hash.SET_PED_WEAPON_TINT_INDEX, owner.Handle, (uint)Hash, (int)value);
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

				return Function.Call<int>(Native.Hash.GET_AMMO_IN_PED_WEAPON, owner.Handle, (uint)Hash);
			}
			set
			{
				if (Hash == WeaponHash.Unarmed)
				{
					return;
				}

				if (IsPresent)
				{
					Function.Call(Native.Hash.SET_PED_AMMO, owner.Handle, (uint)Hash, value);
				}
				else
				{
					Function.Call(Native.Hash.GIVE_WEAPON_TO_PED, owner.Handle, (uint)Hash, value, false, true);
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
					Function.Call(Native.Hash.GET_AMMO_IN_CLIP, owner.Handle, (uint)Hash, &ammoInClip);
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
					Function.Call(Native.Hash.SET_AMMO_IN_CLIP, owner.Handle, (uint)Hash, value);
				}
				else
				{
					Function.Call(Native.Hash.GIVE_WEAPON_TO_PED, owner.Handle, (uint)Hash, value, true, false);
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
					Function.Call(Native.Hash.GET_MAX_AMMO, owner.Handle, (uint)Hash, &maxAmmo);
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

				return Function.Call<int>(Native.Hash.GET_MAX_AMMO_IN_CLIP, owner.Handle, (uint)Hash, true);
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

				Function.Call(Native.Hash.SET_PED_INFINITE_AMMO, owner.Handle, value, (uint)Hash);
			}
		}
		public bool InfiniteAmmoClip
		{
			set => Function.Call(Native.Hash.SET_PED_INFINITE_AMMO_CLIP, owner.Handle, value);
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
				Function.Call(Native.Hash.GIVE_WEAPON_COMPONENT_TO_PED, owner, (uint)Hash, (uint)component);
			}
			else
			{
				Function.Call(Native.Hash.REMOVE_WEAPON_COMPONENT_FROM_PED, owner, (uint)Hash, (uint)component);
			}
		}

		public bool IsComponentActive(WeaponComponent component)
		{
			return Function.Call<bool>(Native.Hash.HAS_PED_GOT_WEAPON_COMPONENT, owner, (uint)Hash, (uint)component);
		}

		public static string GetDisplayNameFromHash(WeaponHash hash)
		{
			// Will be found in this switch statement if the hash is a weapon hash for singleplayer
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
					return "WT_HMKRIFLE";
				case WeaponHash.Firework:
					return "WT_FWRKLNCHR";
				case WeaponHash.HomingLauncher:
					return "WT_HOMLNCH";
				case WeaponHash.Railgun:
					return "WT_RAILGUN";
				case WeaponHash.ProximityMine:
					return "WT_PRXMINE";
			}

			DlcWeaponData data;
			for (int i = 0, max = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPONS); i < max; i++)
			{
				unsafe
				{
					if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_DATA, i, (int*)(void*)&data))
					{
						if (data.Hash == hash)
						{
							return data.DisplayName;
						}
					}
				}
			}

			return "WT_INVALID";
		}

		public static string GetComponentDisplayNameFromHash(WeaponHash hash, WeaponComponent component)
		{
			// Will be found in this switch statement if the hash is a weapon component hash for singleplayer
			switch (component)
			{
				case WeaponComponentHash.Invalid:
					return "WCT_INVALID";
				case WeaponComponentHash.AtRailCover01:
					return "WCT_RAIL";
				case WeaponComponentHash.AtArAfGrip:
					return "WCT_GRIP";
				case WeaponComponentHash.AtPiFlsh:
				case WeaponComponentHash.AtArFlsh:
					return "WCT_FLASH";
				case WeaponComponentHash.AtScopeMacro:
				case WeaponComponentHash.AtScopeMacro02:
					return "WCT_SCOPE_MAC";
				case WeaponComponentHash.AtScopeSmall:
				case WeaponComponentHash.AtScopeSmall02:
					return "WCT_SCOPE_SML";
				case WeaponComponentHash.AtScopeMedium:
					return "WCT_SCOPE_MED";
				case WeaponComponentHash.AtScopeLarge:
				case WeaponComponentHash.AtScopeLargeFixedZoom:
					return "WCT_SCOPE_LRG";
				case WeaponComponentHash.AtScopeMax:
					return "WCT_SCOPE_MAX";
				case WeaponComponentHash.AtPiSupp:
				case WeaponComponentHash.AtArSupp:
				case WeaponComponentHash.AtSrSupp:
					return "WCT_SUPP";
				case WeaponComponentHash.PistolClip01:
				case WeaponComponentHash.CombatPistolClip01:
				case WeaponComponentHash.APPistolClip01:
				case WeaponComponentHash.MicroSMGClip01:
				case WeaponComponentHash.SMGClip01:
				case WeaponComponentHash.AssaultRifleClip01:
				case WeaponComponentHash.CarbineRifleClip01:
				case WeaponComponentHash.AdvancedRifleClip01:
				case WeaponComponentHash.MGClip01:
				case WeaponComponentHash.CombatMGClip01:
				case WeaponComponentHash.AssaultShotgunClip01:
				case WeaponComponentHash.SniperRifleClip01:
				case WeaponComponentHash.HeavySniperClip01:
				case WeaponComponentHash.AssaultSMGClip01:
				case WeaponComponentHash.Pistol50Clip01:
				case (WeaponComponentHash)0x0BAAB157:
				case (WeaponComponentHash)0x5AF49386:
				case (WeaponComponentHash)0xCAEBD246:
				case (WeaponComponentHash)0xF8955D89:
				case WeaponComponentHash.SNSPistolClip01:
				case WeaponComponentHash.VintagePistolClip01:
				case WeaponComponentHash.HeavyShotgunClip01:
				case WeaponComponentHash.MarksmanRifleClip01:
				case WeaponComponentHash.CombatPDWClip01:
				case WeaponComponentHash.MarksmanPistolClip01:
				case WeaponComponentHash.MachinePistolClip01:
					return "WCT_CLIP1";
				case WeaponComponentHash.PistolClip02:
				case WeaponComponentHash.CombatPistolClip02:
				case WeaponComponentHash.APPistolClip02:
				case WeaponComponentHash.MicroSMGClip02:
				case WeaponComponentHash.SMGClip02:
				case WeaponComponentHash.AssaultRifleClip02:
				case WeaponComponentHash.CarbineRifleClip02:
				case WeaponComponentHash.AdvancedRifleClip02:
				case WeaponComponentHash.MGClip02:
				case WeaponComponentHash.CombatMGClip02:
				case WeaponComponentHash.AssaultShotgunClip02:
				case WeaponComponentHash.MinigunClip01:
				case WeaponComponentHash.AssaultSMGClip02:
				case WeaponComponentHash.Pistol50Clip02:
				case (WeaponComponentHash)0x6CBF371B:
				case (WeaponComponentHash)0xE1C5FFFA:
				case (WeaponComponentHash)0x3E7E6956:
				case WeaponComponentHash.SNSPistolClip02:
				case WeaponComponentHash.VintagePistolClip02:
				case WeaponComponentHash.HeavyShotgunClip02:
				case WeaponComponentHash.MarksmanRifleClip02:
				case WeaponComponentHash.CombatPDWClip02:
				case WeaponComponentHash.MachinePistolClip02:
					return "WCT_CLIP2";
				case WeaponComponentHash.AssaultRifleVarmodLuxe:
				case WeaponComponentHash.CarbineRifleVarmodLuxe:
				case WeaponComponentHash.PistolVarmodLuxe:
				case WeaponComponentHash.SMGVarmodLuxe:
				case WeaponComponentHash.MicroSMGVarmodLuxe:
				case WeaponComponentHash.MarksmanRifleVarmodLuxe:
				case WeaponComponentHash.AssaultSMGVarmodLowrider:
				case WeaponComponentHash.CombatPistolVarmodLowrider:
				case WeaponComponentHash.MGVarmodLowrider:
				case WeaponComponentHash.PumpShotgunVarmodLowrider:
					return "WCT_VAR_GOLD";
				case WeaponComponentHash.AdvancedRifleVarmodLuxe:
				case WeaponComponentHash.APPistolVarmodLuxe:
				case WeaponComponentHash.SawnoffShotgunVarmodLuxe:
				case WeaponComponentHash.BullpupRifleVarmodLow:
					return "WCT_VAR_METAL";
				case WeaponComponentHash.Pistol50VarmodLuxe:
					return "WCT_VAR_SIL";
				case WeaponComponentHash.HeavyPistolVarmodLuxe:
				case WeaponComponentHash.SniperRifleVarmodLuxe:
				case WeaponComponentHash.SNSPistolVarmodLowrider:
					return "WCT_VAR_WOOD";
				case WeaponComponentHash.CombatMGVarmodLowrider:
				case WeaponComponentHash.SpecialCarbineVarmodLowrider:
					return "WCT_VAR_ETCHM";
				case WeaponComponentHash.SMGClip03:
				case WeaponComponentHash.AssaultRifleClip03:
				case WeaponComponentHash.HeavyShotgunClip03:
					return "WCT_CLIP_DRM";
				case WeaponComponentHash.CarbineRifleClip03:
					return "WCT_CLIP_BOX";
			}

			DlcWeaponData data;
			DlcWeaponComponentData componentData;
			for (int i = 0, max = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPONS); i < max; i++)
			{
				unsafe
				{
					if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_DATA, i, (int*)(void*)&data))
					{
						if (data.Hash == hash)
						{
							int maxComp = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPON_COMPONENTS, i);

							for (int j = 0; j < maxComp; j++)
							{
								if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_COMPONENT_DATA, i, j, (int*)(void*)&componentData))
								{
									if (componentData.Hash == component)
									{
										return componentData.DisplayName;
									}
								}
							}
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
