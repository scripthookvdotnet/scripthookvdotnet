//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	public sealed class WeaponComponent
	{
		#region Fields
		readonly Ped _owner;
		readonly Weapon _weapon;
		#endregion

		internal WeaponComponent(Ped owner, Weapon weapon, WeaponComponentHash component)
		{
			_owner = owner;
			_weapon = weapon;
			ComponentHash = component;
		}

		public bool Active
		{
			get => Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, _owner.Handle, _weapon.Hash, ComponentHash);
			set => Function.Call(value ? Hash.GIVE_WEAPON_COMPONENT_TO_PED : Hash.REMOVE_WEAPON_COMPONENT_FROM_PED, _owner.Handle, _weapon.Hash, ComponentHash);
		}

		public string DisplayName => GetComponentDisplayNameFromHash(_weapon.Hash, ComponentHash);

		public string LocalizedName => Game.GetLocalizedString(DisplayName);

		public WeaponComponentHash ComponentHash
		{
			get;
		}

		public WeaponAttachmentPoint AttachmentPoint => GetAttachmentPoint(_weapon.Hash, ComponentHash);

		public static implicit operator WeaponComponentHash(WeaponComponent weaponComponent)
		{
			return weaponComponent.ComponentHash;
		}

		static string GetComponentDisplayNameFromHash(WeaponHash hash, WeaponComponentHash component)
		{
			if (hash == WeaponHash.KnuckleDuster)
			{
				switch (component)
				{
					case WeaponComponentHash.KnuckleVarmodBase:
						return "WT_KNUCKLE";
					case WeaponComponentHash.KnuckleVarmodPimp:
						return "WCT_KNUCK_02";
					case WeaponComponentHash.KnuckleVarmodBallas:
						return "WCT_KNUCK_BG";
					case WeaponComponentHash.KnuckleVarmodDollar:
						return "WCT_KNUCK_DLR";
					case WeaponComponentHash.KnuckleVarmodDiamond:
						return "WCT_KNUCK_DMD";
					case WeaponComponentHash.KnuckleVarmodHate:
						return "WCT_KNUCK_HT";
					case WeaponComponentHash.KnuckleVarmodLove:
						return "WCT_KNUCK_LV";
					case WeaponComponentHash.KnuckleVarmodPlayer:
						return "WCT_KNUCK_PC";
					case WeaponComponentHash.KnuckleVarmodKing:
						return "WCT_KNUCK_SLG";
					case WeaponComponentHash.KnuckleVarmodVagos:
						return "WCT_KNUCK_VG";
				}
			}
			else
			{
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
						return "WCT_SCOPE_MAC";
					case WeaponComponentHash.AtScopeMacro02:
						return "WCT_SCOPE_MAC";
					case WeaponComponentHash.AtScopeSmall:
						return "WCT_SCOPE_SML";
					case WeaponComponentHash.AtScopeSmall02:
						return "WCT_SCOPE_SML";
					case WeaponComponentHash.AtScopeMedium:
						return "WCT_SCOPE_MED";
					case WeaponComponentHash.AtScopeLarge:
						return "WCT_SCOPE_LRG";
					case WeaponComponentHash.AtScopeMax:
						return "WCT_SCOPE_MAX";
					case WeaponComponentHash.AtPiSupp:
					case WeaponComponentHash.AtArSupp:
					case WeaponComponentHash.AtArSupp02:
					case WeaponComponentHash.AtSrSupp:
						return "WCT_SUPP";
					case WeaponComponentHash.PistolClip01:
					case WeaponComponentHash.CombatPistolClip01:
					case WeaponComponentHash.APPistolClip01:
					case WeaponComponentHash.MicroSMGClip01:
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
					case WeaponComponentHash.AtScopeLargeFixedZoom:
						return "WCT_SCOPE_LRG";
					case WeaponComponentHash.AtPiSupp02:
						return "WCT_SUPP";
					case WeaponComponentHash.AssaultRifleVarmodLuxe:
					case WeaponComponentHash.CarbineRifleVarmodLuxe:
					case WeaponComponentHash.PistolVarmodLuxe:
					case WeaponComponentHash.SMGVarmodLuxe:
					case WeaponComponentHash.MicroSMGVarmodLuxe:
					case (WeaponComponentHash)0x161E9241:
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
					case WeaponComponentHash.SwitchbladeVarmodBase:
						return "WCT_SB_BASE";
					case WeaponComponentHash.SwitchbladeVarmodVar1:
						return "WCT_SB_VAR1";
					case WeaponComponentHash.SwitchbladeVarmodVar2:
						return "WCT_SB_VAR2";
					case WeaponComponentHash.RevolverClip01:
						return "WCT_CLIP1";
					case WeaponComponentHash.RevolverVarmodBoss:
						return "WCT_REV_VARB";
					case WeaponComponentHash.RevolverVarmodGoon:
						return "WCT_REV_VARG";
					case WeaponComponentHash.SMGClip03:
					case WeaponComponentHash.AssaultRifleClip03:
					case WeaponComponentHash.HeavyShotgunClip03:
						return "WCT_CLIP_DRM";
					case WeaponComponentHash.CarbineRifleClip03:
						return "WCT_CLIP_BOX";
				}
			}
			string result = "WCT_INVALID";

			for (int i = 0, count = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPONS); i < count; i++)
			{
				unsafe
				{
					DlcWeaponData weaponData;
					if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_DATA, i, &weaponData))
					{
						if (weaponData.Hash == hash)
						{
							int maxComp = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPON_COMPONENTS, i);

							for (int j = 0; j < maxComp; j++)
							{
								DlcWeaponComponentData componentData;
								if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_COMPONENT_DATA, i, j, &componentData))
								{
									if (componentData.Hash == component)
									{
										return componentData.DisplayName;
									}
								}
							}
							break;
						}
					}
				}
			}

			return result;
		}

		static WeaponAttachmentPoint GetAttachmentPoint(WeaponHash hash, WeaponComponentHash componentHash)
		{
			switch (hash)
			{
				case WeaponHash.Pistol:
					switch (componentHash)
					{
						case WeaponComponentHash.PistolClip01:
						case WeaponComponentHash.PistolClip02:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtPiFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtPiSupp02:
							return WeaponAttachmentPoint.Supp;

						case WeaponComponentHash.PistolVarmodLuxe:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.CombatPistol:
					switch (componentHash)
					{
						case WeaponComponentHash.CombatPistolClip01:
						case WeaponComponentHash.CombatPistolClip02:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtPiFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtPiSupp:
							return WeaponAttachmentPoint.Supp;

						case WeaponComponentHash.CombatPistolVarmodLowrider:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.APPistol:
					switch (componentHash)
					{
						case WeaponComponentHash.APPistolClip01:
						case WeaponComponentHash.APPistolClip02:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtPiFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtPiSupp:
							return WeaponAttachmentPoint.Supp;

						case WeaponComponentHash.APPistolVarmodLuxe:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.MicroSMG:
					switch (componentHash)
					{
						case WeaponComponentHash.MicroSMGClip01:
						case WeaponComponentHash.MicroSMGClip02:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtPiFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtScopeMacro:
							return WeaponAttachmentPoint.Scope;

						case WeaponComponentHash.AtArSupp02:
							return WeaponAttachmentPoint.Supp;

						case WeaponComponentHash.MicroSMGVarmodLuxe:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.SMG:
					switch (componentHash)
					{
						case WeaponComponentHash.SMGClip01:
						case WeaponComponentHash.SMGClip02:
						case WeaponComponentHash.SMGClip03:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtArFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtScopeMacro02:
							return WeaponAttachmentPoint.Scope;

						case WeaponComponentHash.AtPiSupp:
							return WeaponAttachmentPoint.Supp;

						case WeaponComponentHash.SMGVarmodLuxe:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.AssaultRifle:
					switch (componentHash)
					{
						case WeaponComponentHash.AssaultRifleClip01:
						case WeaponComponentHash.AssaultRifleClip02:
						case WeaponComponentHash.AssaultRifleClip03:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtArAfGrip:
							return WeaponAttachmentPoint.Grip;

						case WeaponComponentHash.AtArFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtScopeMacro:
							return WeaponAttachmentPoint.Scope;

						case WeaponComponentHash.AtArSupp02:
							return WeaponAttachmentPoint.Supp;

						case WeaponComponentHash.AssaultRifleVarmodLuxe:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.CarbineRifle:
					switch (componentHash)
					{
						case WeaponComponentHash.CarbineRifleClip01:
						case WeaponComponentHash.CarbineRifleClip02:
						case WeaponComponentHash.CarbineRifleClip03:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtRailCover01:
							return WeaponAttachmentPoint.Rail;

						case WeaponComponentHash.AtArAfGrip:
							return WeaponAttachmentPoint.Grip;

						case WeaponComponentHash.AtArFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtScopeMedium:
							return WeaponAttachmentPoint.Scope;

						case WeaponComponentHash.AtArSupp:
							return WeaponAttachmentPoint.Supp;

						case WeaponComponentHash.CarbineRifleVarmodLuxe:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.AdvancedRifle:
					switch (componentHash)
					{
						case WeaponComponentHash.AdvancedRifleClip01:
						case WeaponComponentHash.AdvancedRifleClip02:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtArFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtScopeSmall:
							return WeaponAttachmentPoint.Scope;

						case WeaponComponentHash.AtArSupp:
							return WeaponAttachmentPoint.Supp;

						case WeaponComponentHash.AdvancedRifleVarmodLuxe:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}

				case WeaponHash.MG:
					switch (componentHash)
					{
						case WeaponComponentHash.MGClip01:
						case WeaponComponentHash.MGClip02:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtScopeSmall02:
							return WeaponAttachmentPoint.Scope;

						case WeaponComponentHash.MGVarmodLowrider:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.CombatMG:
					switch (componentHash)
					{
						case WeaponComponentHash.CombatMGClip01:
						case WeaponComponentHash.CombatMGClip02:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtArAfGrip:
							return WeaponAttachmentPoint.Grip;

						case WeaponComponentHash.AtScopeMedium:
							return WeaponAttachmentPoint.Scope;

						case WeaponComponentHash.CombatMGVarmodLowrider:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.PumpShotgun:
					switch (componentHash)
					{
						case WeaponComponentHash.PumpShotgunClip01:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtArFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtArSupp:
							return WeaponAttachmentPoint.Supp;

						case WeaponComponentHash.PumpShotgunVarmodLowrider:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.AssaultShotgun:
					switch (componentHash)
					{
						case WeaponComponentHash.AssaultShotgunClip01:
						case WeaponComponentHash.AssaultShotgunClip02:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtArAfGrip:
							return WeaponAttachmentPoint.Grip;

						case WeaponComponentHash.AtArFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtArSupp:
							return WeaponAttachmentPoint.Supp;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.SniperRifle:
					switch (componentHash)
					{
						case WeaponComponentHash.SniperRifleClip01:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtArSupp02:
							return WeaponAttachmentPoint.Supp;

						case WeaponComponentHash.AtScopeLarge:
						case WeaponComponentHash.AtScopeMax:
							return WeaponAttachmentPoint.Scope;

						case WeaponComponentHash.SniperRifleVarmodLuxe:
							return WeaponAttachmentPoint.GunRoot;
						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.HeavySniper:
					switch (componentHash)
					{
						case WeaponComponentHash.HeavySniperClip01:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtScopeLarge:
						case WeaponComponentHash.AtScopeMax:
							return WeaponAttachmentPoint.Scope;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.GrenadeLauncher:
					switch (componentHash)
					{
						case WeaponComponentHash.GrenadeLauncherClip01:
							return WeaponAttachmentPoint.Grip;

						case WeaponComponentHash.AtArAfGrip:
							return WeaponAttachmentPoint.Grip;

						case WeaponComponentHash.AtArFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtScopeSmall:
							return WeaponAttachmentPoint.Scope;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.Minigun:
					switch (componentHash)
					{
						case WeaponComponentHash.MinigunClip01:
							return WeaponAttachmentPoint.Clip;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.AssaultSMG:
					switch (componentHash)
					{
						case WeaponComponentHash.AssaultSMGClip01:
						case WeaponComponentHash.AssaultSMGClip02:
							return WeaponAttachmentPoint.Clip;


						case WeaponComponentHash.AtArFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtScopeMacro:
							return WeaponAttachmentPoint.Scope;

						case WeaponComponentHash.AtArSupp02:
							return WeaponAttachmentPoint.Supp;

						case WeaponComponentHash.AssaultSMGVarmodLowrider:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.BullpupShotgun:
					switch (componentHash)
					{
						case WeaponComponentHash.BullpupShotgunClip01:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtArAfGrip:
							return WeaponAttachmentPoint.Grip;

						case WeaponComponentHash.AtArFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtArSupp02:
							return WeaponAttachmentPoint.Supp;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.Pistol50:
					switch (componentHash)
					{
						case WeaponComponentHash.Pistol50Clip01:
						case WeaponComponentHash.Pistol50Clip02:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.AtPiFlsh:
							return WeaponAttachmentPoint.FlashLaser;

						case WeaponComponentHash.AtArSupp02:
							return WeaponAttachmentPoint.Supp;

						case WeaponComponentHash.Pistol50VarmodLuxe:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}
				case WeaponHash.SawnOffShotgun:
					switch (componentHash)
					{
						case WeaponComponentHash.SawnoffShotgunClip01:
							return WeaponAttachmentPoint.Clip;

						case WeaponComponentHash.SawnoffShotgunVarmodLuxe:
							return WeaponAttachmentPoint.GunRoot;

						default:
							return WeaponAttachmentPoint.Invalid;
					}

			}
			for (int i = 0, count = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPONS); i < count; i++)
			{
				unsafe
				{
					DlcWeaponData weaponData;
					if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_DATA, i, &weaponData))
					{
						if (weaponData.Hash == hash)
						{
							int maxComp = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPON_COMPONENTS, i);

							for (int j = 0; j < maxComp; j++)
							{
								DlcWeaponComponentData componentData;
								if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_COMPONENT_DATA, i, j, &componentData))
								{
									if (componentData.Hash == componentHash)
									{
										return componentData.AttachmentPoint;
									}
								}
							}
							break;
						}
					}
				}
			}
			return WeaponAttachmentPoint.Invalid;
		}
	}
}
