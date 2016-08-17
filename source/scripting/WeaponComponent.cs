using System;
using GTA.Native;

namespace GTA
{
	public class WeaponComponent
	{
		#region Fields
		protected readonly Ped _owner;
		protected readonly Weapon _weapon;
		protected readonly WeaponComponentHash _component;
		#endregion

		internal WeaponComponent(Ped owner, Weapon weapon, WeaponComponentHash component)
		{
			_owner = owner;
			_weapon = weapon;
			_component = component;
		}

		public WeaponComponentHash ComponentHash
		{
			get
			{
				return _component;
			}
		}

		public static implicit operator WeaponComponentHash(WeaponComponent weaponComponent)
		{
			return weaponComponent.ComponentHash;
		}

		public virtual bool Active
		{
			get
			{
				return Function.Call<bool>(Native.Hash.HAS_PED_GOT_WEAPON_COMPONENT, _owner.Handle, _weapon.Hash, _component);
			}
			set
			{
				if (value)
				{
					Function.Call(Native.Hash.GIVE_WEAPON_COMPONENT_TO_PED, _owner.Handle, _weapon.Hash, _component);
				}
				else
				{
					Function.Call(Native.Hash.REMOVE_WEAPON_COMPONENT_FROM_PED, _owner.Handle, _weapon.Hash, _component);
				}
			}
		}

		public virtual string DisplayName
		{
			get
			{
				return GetComponentDisplayNameFromHash(_weapon.Hash, _component);
			}
		}

		public virtual string LocalizedName
		{
			get
			{
				return Game.GetGXTEntry(DisplayName);
			}
		}

		public static string GetComponentDisplayNameFromHash(WeaponHash hash, WeaponComponentHash component)
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
	}
	public class InvalidWeaponComponent : WeaponComponent
	{
		internal InvalidWeaponComponent(): base(null, null, WeaponComponentHash.Invalid)
		{
		}

		public override bool Active
		{
			get
			{
				return false;
			}
			set
			{
				//Setter doesn't need to do anything for the invalid component
			}
		}

		public override string DisplayName
		{
			get
			{
				return "WCT_INVALID";
			}
		}

		public override string LocalizedName
		{
			get
			{
				return Game.GetGXTEntry(DisplayName);
			}
		}

	}


}