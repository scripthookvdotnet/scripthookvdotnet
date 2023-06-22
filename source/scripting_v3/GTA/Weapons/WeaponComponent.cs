//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System.Linq;

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
			get
			{
				if (_weapon == null || _owner == null || !_owner.Exists())
				{
					return false;
				}

				return Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, _owner.Handle, (uint)_weapon.Hash, (uint)ComponentHash);
			}
			set
			{
				if (_weapon == null || _owner == null || !_owner.Exists())
				{
					return;
				}

				Function.Call(value ? Hash.GIVE_WEAPON_COMPONENT_TO_PED : Hash.REMOVE_WEAPON_COMPONENT_FROM_PED, _owner.Handle, (uint)_weapon.Hash, (uint)ComponentHash);
			}
		}

		public string DisplayName => _weapon != null ? GetComponentDisplayNameFromHash(_weapon.Hash, ComponentHash) : string.Empty;

		public string LocalizedName => _weapon != null ? Game.GetLocalizedString((int)SHVDN.NativeMemory.GetHumanNameHashOfWeaponComponentInfo((uint)ComponentHash)) : string.Empty;

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
			// Will be found in this switch statement if the hash is one of the weapon component hashes for singleplayer
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

			for (int i = 0, count = Function.Call<int>(Hash.GET_NUM_DLC_WEAPONS); i < count; i++)
			{
				unsafe
				{
					DlcWeaponData weaponData;
					if (Function.Call<bool>(Hash.GET_DLC_WEAPON_DATA, i, &weaponData))
					{
						if (weaponData.Hash == hash)
						{
							int maxComp = Function.Call<int>(Hash.GET_NUM_DLC_WEAPON_COMPONENTS, i);

							for (int j = 0; j < maxComp; j++)
							{
								DlcWeaponComponentData componentData;
								if (Function.Call<bool>(Hash.GET_DLC_WEAPON_COMPONENT_DATA, i, j, &componentData))
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

			return "WCT_INVALID";
		}

		static WeaponAttachmentPoint GetAttachmentPoint(WeaponHash hash, WeaponComponentHash componentHash)
		{
			return (WeaponAttachmentPoint)SHVDN.NativeMemory.GetAttachmentPointHash((uint)hash, (uint)componentHash);
		}

		public static WeaponComponentHash[] GetAllHashes()
		{
			return SHVDN.NativeMemory.GetAllWeaponComponentHashes().Select(x => (WeaponComponentHash)x).ToArray();
		}
	}
}
