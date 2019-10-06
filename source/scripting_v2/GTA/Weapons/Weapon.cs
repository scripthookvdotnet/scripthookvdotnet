//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;

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
			switch (hash)
			{
				case WeaponHash.Pistol:
					return "WT_PIST";
				case WeaponHash.CombatPistol:
					return "WT_PIST_CBT";
				case WeaponHash.APPistol:
					return "WT_PIST_AP";
				case WeaponHash.SMG:
					return "WT_SMG";
				case WeaponHash.MicroSMG:
					return "WT_SMG_MCR";
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
				case WeaponHash.HeavySniper:
					return "WT_SNIP_HVY";
				case WeaponHash.SniperRifle:
					return "WT_SNIP_RIF";
				case WeaponHash.GrenadeLauncher:
					return "WT_GL";
				case WeaponHash.RPG:
					return "WT_RPG";
				case WeaponHash.Minigun:
					return "WT_MINIGUN";
				case WeaponHash.AssaultSMG:
					return "WT_SMG_ASL";
				case WeaponHash.BullpupShotgun:
					return "WT_SG_BLP";
				case WeaponHash.Pistol50:
					return "WT_PIST_50";
				case WeaponHash.Bottle:
					return "WT_BOTTLE";
				case WeaponHash.Gusenberg:
					return "WT_GUSENBERG";
				case WeaponHash.SNSPistol:
					return "WT_SNSPISTOL";
				case WeaponHash.VintagePistol:
					return "TT_VPISTOL";
				case WeaponHash.Dagger:
					return "WT_DAGGER";
				case WeaponHash.FlareGun:
					return "WT_FLAREGUN";
				case WeaponHash.Musket:
					return "WT_MUSKET";
				case WeaponHash.Firework:
					return "WT_FWRKLNCHR";
				case WeaponHash.MarksmanRifle:
					return "WT_HMKRIFLE";
				case WeaponHash.HeavyShotgun:
					return "WT_HVYSHOT";
				case WeaponHash.ProximityMine:
					return "WT_PRXMINE";
				case WeaponHash.HomingLauncher:
					return "WT_HOMLNCH";
				case WeaponHash.CombatPDW:
					return "WT_COMBATPDW";
				case WeaponHash.KnuckleDuster:
					return "WT_KNUCKLE";
				case WeaponHash.MarksmanPistol:
					return "WT_MKPISTOL";
				case WeaponHash.Machete:
					return "WT_MACHETE";
				case WeaponHash.MachinePistol:
					return "WT_MCHPIST";
				case WeaponHash.Flashlight:
					return "WT_FLASHLIGHT";
				case WeaponHash.DoubleBarrelShotgun:
					return "WT_DBSHGN";
				case WeaponHash.CompactRifle:
					return "WT_CMPRIFLE";
				case WeaponHash.SwitchBlade:
					return "WT_SWBLADE";
				case WeaponHash.Revolver:
					return "WT_REVOLVER";
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
			if (hash == WeaponHash.KnuckleDuster)
			{
				switch (component)
				{
					case WeaponComponent.KnuckleVarmodBase:
						return "WT_KNUCKLE";
					case WeaponComponent.KnuckleVarmodPimp:
						return "WCT_KNUCK_02";
					case WeaponComponent.KnuckleVarmodBallas:
						return "WCT_KNUCK_BG";
					case WeaponComponent.KnuckleVarmodDollar:
						return "WCT_KNUCK_DLR";
					case WeaponComponent.KnuckleVarmodDiamond:
						return "WCT_KNUCK_DMD";
					case WeaponComponent.KnuckleVarmodHate:
						return "WCT_KNUCK_HT";
					case WeaponComponent.KnuckleVarmodLove:
						return "WCT_KNUCK_LV";
					case WeaponComponent.KnuckleVarmodPlayer:
						return "WCT_KNUCK_PC";
					case WeaponComponent.KnuckleVarmodKing:
						return "WCT_KNUCK_SLG";
					case WeaponComponent.KnuckleVarmodVagos:
						return "WCT_KNUCK_VG";
				}
			}
			else
			{
				switch (component)
				{
					case WeaponComponent.AtRailCover01:
						return "WCT_RAIL";
					case WeaponComponent.AtArAfGrip:
						return "WCT_GRIP";
					case WeaponComponent.AtPiFlsh:
						return "WCT_FLASH";
					case WeaponComponent.AtArFlsh:
						return "WCT_FLASH";
					case WeaponComponent.AtScopeMacro:
						return "WCT_SCOPE_MAC";
					case WeaponComponent.AtScopeMacro02:
						return "WCT_SCOPE_MAC";
					case WeaponComponent.AtScopeSmall:
						return "WCT_SCOPE_SML";
					case WeaponComponent.AtScopeSmall02:
						return "WCT_SCOPE_SML";
					case WeaponComponent.AtScopeMedium:
						return "WCT_SCOPE_MED";
					case WeaponComponent.AtScopeLarge:
						return "WCT_SCOPE_LRG";
					case WeaponComponent.AtScopeMax:
						return "WCT_SCOPE_MAX";
					case WeaponComponent.AtPiSupp:
						return "WCT_SUPP";
					case WeaponComponent.AtArSupp:
						return "WCT_SUPP";
					case WeaponComponent.AtArSupp02:
						return "WCT_SUPP";
					case WeaponComponent.AtSrSupp:
						return "WCT_SUPP";
					case WeaponComponent.PistolClip01:
						return "WCT_CLIP1";
					case WeaponComponent.PistolClip02:
						return "WCT_CLIP2";
					case WeaponComponent.CombatPistolClip01:
						return "WCT_CLIP1";
					case WeaponComponent.CombatPistolClip02:
						return "WCT_CLIP2";
					case WeaponComponent.APPistolClip01:
						return "WCT_CLIP1";
					case WeaponComponent.APPistolClip02:
						return "WCT_CLIP2";
					case WeaponComponent.MicroSMGClip01:
						return "WCT_CLIP1";
					case WeaponComponent.MicroSMGClip02:
						return "WCT_CLIP2";
					case WeaponComponent.SMGClip01:
						return "WCT_CLIP1";
					case WeaponComponent.SMGClip02:
						return "WCT_CLIP2";
					case WeaponComponent.AssaultRifleClip01:
						return "WCT_CLIP1";
					case WeaponComponent.AssaultRifleClip02:
						return "WCT_CLIP2";
					case WeaponComponent.CarbineRifleClip01:
						return "WCT_CLIP1";
					case WeaponComponent.CarbineRifleClip02:
						return "WCT_CLIP2";
					case WeaponComponent.AdvancedRifleClip01:
						return "WCT_CLIP1";
					case WeaponComponent.AdvancedRifleClip02:
						return "WCT_CLIP2";
					case WeaponComponent.MGClip01:
						return "WCT_CLIP1";
					case WeaponComponent.MGClip02:
						return "WCT_CLIP2";
					case WeaponComponent.CombatMGClip01:
						return "WCT_CLIP1";
					case WeaponComponent.CombatMGClip02:
						return "WCT_CLIP2";
					case WeaponComponent.AssaultShotgunClip01:
						return "WCT_CLIP1";
					case WeaponComponent.AssaultShotgunClip02:
						return "WCT_CLIP2";
					case WeaponComponent.SniperRifleClip01:
						return "WCT_CLIP1";
					case WeaponComponent.HeavySniperClip01:
						return "WCT_CLIP1";
					case WeaponComponent.MinigunClip01:
						return "WCT_CLIP2";
					case WeaponComponent.AssaultSMGClip01:
						return "WCT_CLIP1";
					case WeaponComponent.AssaultSMGClip02:
						return "WCT_CLIP2";
					case WeaponComponent.Pistol50Clip01:
						return "WCT_CLIP1";
					case WeaponComponent.Pistol50Clip02:
						return "WCT_CLIP2";
					case (WeaponComponent)0x0BAAB157:
						return "WCT_CLIP1";
					case (WeaponComponent)0x5AF49386:
						return "WCT_CLIP1";
					case (WeaponComponent)0x6CBF371B:
						return "WCT_CLIP2";
					case (WeaponComponent)0xCAEBD246:
						return "WCT_CLIP1";
					case (WeaponComponent)0xE1C5FFFA:
						return "WCT_CLIP2";
					case (WeaponComponent)0xF8955D89:
						return "WCT_CLIP1";
					case (WeaponComponent)0x3E7E6956:
						return "WCT_CLIP2";
					case WeaponComponent.SNSPistolClip01:
						return "WCT_CLIP1";
					case WeaponComponent.SNSPistolClip02:
						return "WCT_CLIP2";
					case WeaponComponent.VintagePistolClip01:
						return "WCT_CLIP1";
					case WeaponComponent.VintagePistolClip02:
						return "WCT_CLIP2";
					case WeaponComponent.HeavyShotgunClip01:
						return "WCT_CLIP1";
					case WeaponComponent.MarksmanRifleClip01:
						return "WCT_CLIP1";
					case WeaponComponent.HeavyShotgunClip02:
						return "WCT_CLIP2";
					case WeaponComponent.MarksmanRifleClip02:
						return "WCT_CLIP2";
					case WeaponComponent.AtScopeLargeFixedZoom:
						return "WCT_SCOPE_LRG";
					case WeaponComponent.AtPiSupp02:
						return "WCT_SUPP";
					case WeaponComponent.CombatPDWClip01:
						return "WCT_CLIP1";
					case WeaponComponent.CombatPDWClip02:
						return "WCT_CLIP2";
					case WeaponComponent.MarksmanPistolClip01:
						return "WCT_CLIP1";
					case WeaponComponent.MachinePistolClip01:
						return "WCT_CLIP1";
					case WeaponComponent.MachinePistolClip02:
						return "WCT_CLIP2";
					case WeaponComponent.AssaultRifleVarmodLuxe:
						return "WCT_VAR_GOLD";
					case WeaponComponent.AdvancedRifleVarmodLuxe:
						return "WCT_VAR_METAL";
					case WeaponComponent.CarbineRifleVarmodLuxe:
						return "WCT_VAR_GOLD";
					case WeaponComponent.APPistolVarmodLuxe:
						return "WCT_VAR_METAL";
					case WeaponComponent.PistolVarmodLuxe:
						return "WCT_VAR_GOLD";
					case WeaponComponent.Pistol50VarmodLuxe:
						return "WCT_VAR_SIL";
					case WeaponComponent.HeavyPistolVarmodLuxe:
						return "WCT_VAR_WOOD";
					case WeaponComponent.SMGVarmodLuxe:
						return "WCT_VAR_GOLD";
					case WeaponComponent.MicroSMGVarmodLuxe:
						return "WCT_VAR_GOLD";
					case WeaponComponent.SawnoffShotgunVarmodLuxe:
						return "WCT_VAR_METAL";
					case WeaponComponent.SniperRifleVarmodLuxe:
						return "WCT_VAR_WOOD";
					case (WeaponComponent)0x161E9241:
						return "WCT_VAR_GOLD";
					case WeaponComponent.AssaultSMGVarmodLowrider:
						return "WCT_VAR_GOLD";
					case WeaponComponent.BullpupRifleVarmodLow:
						return "WCT_VAR_METAL";
					case WeaponComponent.CombatMGVarmodLowrider:
						return "WCT_VAR_ETCHM";
					case WeaponComponent.CombatPistolVarmodLowrider:
						return "WCT_VAR_GOLD";
					case WeaponComponent.MGVarmodLowrider:
						return "WCT_VAR_GOLD";
					case WeaponComponent.PumpShotgunVarmodLowrider:
						return "WCT_VAR_GOLD";
					case WeaponComponent.SNSPistolVarmodLowrider:
						return "WCT_VAR_WOOD";
					case WeaponComponent.SpecialCarbineVarmodLowrider:
						return "WCT_VAR_ETCHM";
					case WeaponComponent.SwitchbladeVarmodBase:
						return "WCT_SB_BASE";
					case WeaponComponent.SwitchbladeVarmodVar1:
						return "WCT_SB_VAR1";
					case WeaponComponent.SwitchbladeVarmodVar2:
						return "WCT_SB_VAR2";
					case WeaponComponent.RevolverClip01:
						return "WCT_CLIP1";
					case WeaponComponent.RevolverVarmodBoss:
						return "WCT_REV_VARB";
					case WeaponComponent.RevolverVarmodGoon:
						return "WCT_REV_VARG";
				}
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
			switch (hash)
			{
				case WeaponHash.Pistol:
					return new[] {
						WeaponComponent.PistolClip01,
						WeaponComponent.PistolClip02,
						WeaponComponent.AtPiFlsh,
						WeaponComponent.AtPiSupp02,
						WeaponComponent.PistolVarmodLuxe };
				case WeaponHash.CombatPistol:
					return new[] {
						WeaponComponent.CombatPistolClip01,
						WeaponComponent.CombatPistolClip02,
						WeaponComponent.AtPiFlsh,
						WeaponComponent.AtPiSupp,
						WeaponComponent.CombatPistolVarmodLowrider };
				case WeaponHash.APPistol:
					return new[] {
						WeaponComponent.APPistolClip01,
						WeaponComponent.APPistolClip02,
						WeaponComponent.AtPiFlsh,
						WeaponComponent.AtPiSupp,
						WeaponComponent.APPistolVarmodLuxe };
				case WeaponHash.MicroSMG:
					return new[] {
						WeaponComponent.MicroSMGClip01,
						WeaponComponent.MicroSMGClip02,
						WeaponComponent.AtPiFlsh,
						WeaponComponent.AtScopeMacro,
						WeaponComponent.AtArSupp02,
						WeaponComponent.MicroSMGVarmodLuxe };
				case WeaponHash.SMG:
					return new[] {
						WeaponComponent.SMGClip01,
						WeaponComponent.SMGClip02,
						WeaponComponent.AtArFlsh,
						WeaponComponent.AtPiSupp,
						WeaponComponent.AtScopeMacro02,
						WeaponComponent.AtArAfGrip,
						WeaponComponent.SMGVarmodLuxe };
				case WeaponHash.AssaultRifle:
					return new[] {
						WeaponComponent.AssaultRifleClip01,
						WeaponComponent.AssaultRifleClip02,
						WeaponComponent.AtArAfGrip,
						WeaponComponent.AtArFlsh,
						WeaponComponent.AtScopeMacro,
						WeaponComponent.AtArSupp02,
						WeaponComponent.AssaultRifleVarmodLuxe };
				case WeaponHash.CarbineRifle:
					return new[] {
						WeaponComponent.CarbineRifleClip01,
						WeaponComponent.CarbineRifleClip02,
						WeaponComponent.AtRailCover01,
						WeaponComponent.AtArAfGrip,
						WeaponComponent.AtArFlsh,
						WeaponComponent.AtScopeMedium,
						WeaponComponent.AtArSupp,
						WeaponComponent.CarbineRifleVarmodLuxe };
				case WeaponHash.AdvancedRifle:
					return new[] {
						WeaponComponent.AdvancedRifleClip01,
						WeaponComponent.AdvancedRifleClip02,
						WeaponComponent.AtArFlsh,
						WeaponComponent.AtScopeSmall,
						WeaponComponent.AtArSupp,
						WeaponComponent.AdvancedRifleVarmodLuxe };
				case WeaponHash.MG:
					return new[] {
						WeaponComponent.MGClip01,
						WeaponComponent.MGClip02,
						WeaponComponent.AtScopeSmall02,
						WeaponComponent.AtArAfGrip,
						WeaponComponent.MGVarmodLowrider };
				case WeaponHash.CombatMG:
					return new[] {
						WeaponComponent.CombatMGClip01,
						WeaponComponent.CombatMGClip02,
						WeaponComponent.AtArAfGrip,
						WeaponComponent.AtScopeMedium,
						WeaponComponent.CombatMGVarmodLowrider };
				case WeaponHash.PumpShotgun:
					return new[] {
						WeaponComponent.AtSrSupp,
						WeaponComponent.AtArFlsh,
						WeaponComponent.PumpShotgunVarmodLowrider };
				case WeaponHash.AssaultShotgun:
					return new[] {
						WeaponComponent.AssaultShotgunClip01,
						WeaponComponent.AssaultShotgunClip02,
						WeaponComponent.AtArAfGrip,
						WeaponComponent.AtArFlsh,
						WeaponComponent.AtArSupp };
				case WeaponHash.SniperRifle:
					return new[] {
						WeaponComponent.SniperRifleClip01,
						WeaponComponent.AtScopeLarge,
						WeaponComponent.AtScopeMax,
						WeaponComponent.AtArSupp02,
						WeaponComponent.SniperRifleVarmodLuxe };
				case WeaponHash.HeavySniper:
					return new[] {
						WeaponComponent.HeavySniperClip01,
						WeaponComponent.AtScopeLarge,
						WeaponComponent.AtScopeMax };
				case WeaponHash.GrenadeLauncher:
					return new[] {
						WeaponComponent.AtArAfGrip,
						WeaponComponent.AtArFlsh,
						WeaponComponent.AtScopeSmall };
				case WeaponHash.Minigun:
					return new[] {
						WeaponComponent.MinigunClip01 };
				case WeaponHash.AssaultSMG:
					return new[] {
						WeaponComponent.AssaultSMGClip01,
						WeaponComponent.AssaultSMGClip02,
						WeaponComponent.AtArFlsh,
						WeaponComponent.AtScopeMacro,
						WeaponComponent.AtArSupp02,
						WeaponComponent.AssaultSMGVarmodLowrider };
				case WeaponHash.BullpupShotgun:
					return new[] {
						WeaponComponent.AtArAfGrip,
						WeaponComponent.AtArFlsh,
						WeaponComponent.AtArSupp02 };
				case WeaponHash.Pistol50:
					return new[] {
						WeaponComponent.Pistol50Clip01,
						WeaponComponent.Pistol50Clip02,
						WeaponComponent.AtPiFlsh,
						WeaponComponent.AtArSupp02,
						WeaponComponent.Pistol50VarmodLuxe };
				case WeaponHash.CombatPDW:
					return new[] {
						WeaponComponent.CombatPDWClip01,
						WeaponComponent.CombatPDWClip02,
						WeaponComponent.AtArFlsh,
						WeaponComponent.AtScopeSmall,
						WeaponComponent.AtArAfGrip };
				case WeaponHash.SawnOffShotgun:
					return new[] {
						WeaponComponent.SawnoffShotgunVarmodLuxe };
				case WeaponHash.BullpupRifle:
					return new[] {
						WeaponComponent.BullpupRifleClip01,
						WeaponComponent.BullpupRifleClip02,
						WeaponComponent.AtArFlsh,
						WeaponComponent.AtScopeSmall,
						WeaponComponent.AtArSupp,
						WeaponComponent.AtArAfGrip,
						WeaponComponent.BullpupRifleVarmodLow };
				case WeaponHash.SNSPistol:
					return new[] {
						WeaponComponent.SNSPistolClip01,
						WeaponComponent.SNSPistolClip02,
						WeaponComponent.SNSPistolVarmodLowrider };
				case WeaponHash.SpecialCarbine:
					return new[] {
						WeaponComponent.SpecialCarbineClip01,
						WeaponComponent.SpecialCarbineClip02,
						WeaponComponent.AtArFlsh,
						WeaponComponent.AtScopeMedium,
						WeaponComponent.AtArSupp02,
						WeaponComponent.AtArAfGrip,
						WeaponComponent.SpecialCarbineVarmodLowrider };
				case WeaponHash.KnuckleDuster:
					return new[] {
						WeaponComponent.KnuckleVarmodPimp,
						WeaponComponent.KnuckleVarmodBallas,
						WeaponComponent.KnuckleVarmodDollar,
						WeaponComponent.KnuckleVarmodDiamond,
						WeaponComponent.KnuckleVarmodHate,
						WeaponComponent.KnuckleVarmodLove,
						WeaponComponent.KnuckleVarmodPlayer,
						WeaponComponent.KnuckleVarmodKing,
						WeaponComponent.KnuckleVarmodVagos };
				case WeaponHash.MachinePistol:
					return new[] {
						WeaponComponent.MachinePistolClip01,
						WeaponComponent.MachinePistolClip02,
						WeaponComponent.AtPiSupp };
				case WeaponHash.SwitchBlade:
					return new[] {
						WeaponComponent.SwitchbladeVarmodVar1,
						WeaponComponent.SwitchbladeVarmodVar2 };
				case WeaponHash.Revolver:
					return new[] {
						WeaponComponent.RevolverClip01,
						WeaponComponent.RevolverVarmodBoss,
						WeaponComponent.RevolverVarmodGoon };
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
							WeaponComponent[] components = new WeaponComponent[maxComp];

							for (int j = 0; j < maxComp; j++)
							{
								if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_COMPONENT_DATA, i, j, (int*)(void*)&componentData))
								{
									components[j] = (WeaponComponent)componentData.Hash;
								}
								else
								{
									components[j] = WeaponComponent.Invalid;
								}
							}
						}
					}
				}
			}

			return new WeaponComponent[0];
		}
	}
}
