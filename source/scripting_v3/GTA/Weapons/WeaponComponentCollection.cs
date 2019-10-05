//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;
using System.Collections.Generic;
using System.Linq;

namespace GTA
{
	public sealed class WeaponComponentCollection
	{
		#region Fields
		readonly Ped _owner;
		readonly Weapon _weapon;
		readonly Dictionary<WeaponComponentHash, WeaponComponent> _weaponComponents = new Dictionary<WeaponComponentHash, WeaponComponent>();
		readonly WeaponComponentHash[] _components;
		readonly static WeaponComponent _invalidComponent = new WeaponComponent(null, null, WeaponComponentHash.Invalid);
		#endregion

		internal WeaponComponentCollection(Ped owner, Weapon weapon)
		{
			_owner = owner;
			_weapon = weapon;
			_components = GetComponentsFromHash(weapon.Hash);
		}

		public WeaponComponent this[int index]
		{
			get
			{
				WeaponComponent component = null;
				if (index >= 0 && index < Count)
				{
					WeaponComponentHash componentHash = _components[index];

					if (!_weaponComponents.TryGetValue(componentHash, out component))
					{
						component = new WeaponComponent(_owner, _weapon, componentHash);
						_weaponComponents.Add(componentHash, component);
					}
					return component;
				}
				else
				{
					return _invalidComponent;
				}
			}
		}

		public WeaponComponent this[WeaponComponentHash componentHash]
		{
			get
			{
				if (_components.Contains(componentHash))
				{
					WeaponComponent component = null;
					if (!_weaponComponents.TryGetValue(componentHash, out component))
					{
						component = new WeaponComponent(_owner, _weapon, componentHash);
						_weaponComponents.Add(componentHash, component);
					}

					return component;
				}
				else
				{
					return _invalidComponent;
				}
			}
		}

		public int Count => _components.Length;

		public IEnumerator<WeaponComponent> GetEnumerator()
		{
			WeaponComponent[] AllComponents = new WeaponComponent[Count];
			for (int i = 0; i < Count; i++)
			{
				AllComponents[i] = this[_components[i]];
			}
			return (AllComponents as IEnumerable<WeaponComponent>).GetEnumerator();
		}

		public WeaponComponent GetClipComponent(int index)
		{
			foreach (var component in this)
			{
				if (component.AttachmentPoint == WeaponAttachmentPoint.Clip ||
					component.AttachmentPoint == WeaponAttachmentPoint.Clip2)
				{
					if (index-- == 0)
					{
						return component;
					}
				}
			}
			return _invalidComponent;
		}

		public int ClipVariationsCount
		{
			get
			{
				int count = 0;
				foreach (var component in this)
				{
					if (component.AttachmentPoint == WeaponAttachmentPoint.Clip ||
					component.AttachmentPoint == WeaponAttachmentPoint.Clip2)
					{
						count++;
					}
				}
				return count;
			}
		}

		public WeaponComponent GetScopeComponent(int index)
		{
			foreach (var component in this)
			{
				if (component.AttachmentPoint == WeaponAttachmentPoint.Scope ||
					component.AttachmentPoint == WeaponAttachmentPoint.Scope2)
				{
					if (index-- == 0)
					{
						return component;
					}
				}
			}
			return _invalidComponent;
		}

		public int ScopeVariationsCount
		{
			get
			{
				int count = 0;
				foreach (var component in this)
				{
					if (component.AttachmentPoint == WeaponAttachmentPoint.Scope ||
					component.AttachmentPoint == WeaponAttachmentPoint.Scope2)
					{
						count++;
					}
				}
				return count;
			}
		}

		public WeaponComponent GetSuppressorComponent()
		{
			foreach (var component in this)
			{
				if (component.AttachmentPoint == WeaponAttachmentPoint.Supp ||
					component.AttachmentPoint == WeaponAttachmentPoint.Supp2)
				{
					return component;
				}
			}
			return _invalidComponent;
		}

		public WeaponComponent GetFlashLightComponent()
		{
			foreach (var component in this)
			{
				if (component.AttachmentPoint == WeaponAttachmentPoint.FlashLaser ||
					component.AttachmentPoint == WeaponAttachmentPoint.FlashLaser2)
				{
					return component;
				}
			}
			return _invalidComponent;
		}

		public WeaponComponent GetLuxuryFinishComponent()
		{
			foreach (var component in this)
			{
				if (component.AttachmentPoint == WeaponAttachmentPoint.GunRoot)
				{
					return component;
				}
			}
			return _invalidComponent;
		}

		static WeaponComponentHash[] GetComponentsFromHash(WeaponHash hash)
		{
			switch (hash)
			{
				case WeaponHash.Pistol:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.PistolClip01,
						WeaponComponentHash.PistolClip02,
						WeaponComponentHash.AtPiFlsh,
						WeaponComponentHash.AtPiSupp02,
						WeaponComponentHash.PistolVarmodLuxe,
					};
				case WeaponHash.CombatPistol:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.CombatPistolClip01,
						WeaponComponentHash.CombatPistolClip02,
						WeaponComponentHash.AtPiFlsh,
						WeaponComponentHash.AtPiSupp,
						WeaponComponentHash.CombatPistolVarmodLowrider,
					};
				case WeaponHash.APPistol:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.APPistolClip01,
						WeaponComponentHash.APPistolClip02,
						WeaponComponentHash.AtPiFlsh,
						WeaponComponentHash.AtPiSupp,
						WeaponComponentHash.APPistolVarmodLuxe,
					};
				case WeaponHash.MicroSMG:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.MicroSMGClip01,
						WeaponComponentHash.MicroSMGClip02,
						WeaponComponentHash.AtPiFlsh,
						WeaponComponentHash.AtScopeMacro,
						WeaponComponentHash.AtArSupp02,
						WeaponComponentHash.MicroSMGVarmodLuxe,
					};
				case WeaponHash.SMG:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.SMGClip01,
						WeaponComponentHash.SMGClip02,
						WeaponComponentHash.SMGClip03,
						WeaponComponentHash.AtArFlsh,
						WeaponComponentHash.AtPiSupp,
						WeaponComponentHash.AtScopeMacro02,
						WeaponComponentHash.AtArAfGrip,
						WeaponComponentHash.SMGVarmodLuxe,
					};
				case WeaponHash.AssaultRifle:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.AssaultRifleClip01,
						WeaponComponentHash.AssaultRifleClip02,
						WeaponComponentHash.AssaultRifleClip03,
						WeaponComponentHash.AtArAfGrip,
						WeaponComponentHash.AtArFlsh,
						WeaponComponentHash.AtScopeMacro,
						WeaponComponentHash.AtArSupp02,
						WeaponComponentHash.AssaultRifleVarmodLuxe,
					};
				case WeaponHash.CarbineRifle:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.CarbineRifleClip01,
						WeaponComponentHash.CarbineRifleClip02,
						WeaponComponentHash.CarbineRifleClip03,
						WeaponComponentHash.AtRailCover01,
						WeaponComponentHash.AtArAfGrip,
						WeaponComponentHash.AtArFlsh,
						WeaponComponentHash.AtScopeMedium,
						WeaponComponentHash.AtArSupp,
						WeaponComponentHash.CarbineRifleVarmodLuxe,
					};
				case WeaponHash.AdvancedRifle:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.AdvancedRifleClip01,
						WeaponComponentHash.AdvancedRifleClip02,
						WeaponComponentHash.AtArFlsh,
						WeaponComponentHash.AtScopeSmall,
						WeaponComponentHash.AtArSupp,
						WeaponComponentHash.AdvancedRifleVarmodLuxe,
					};
				case WeaponHash.MG:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.MGClip01,
						WeaponComponentHash.MGClip02,
						WeaponComponentHash.AtScopeSmall02,
						WeaponComponentHash.AtArAfGrip,
						WeaponComponentHash.MGVarmodLowrider,
					};
				case WeaponHash.CombatMG:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.CombatMGClip01,
						WeaponComponentHash.CombatMGClip02,
						WeaponComponentHash.AtArAfGrip,
						WeaponComponentHash.AtScopeMedium,
						WeaponComponentHash.CombatMGVarmodLowrider,
					};
				case WeaponHash.PumpShotgun:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.AtSrSupp,
						WeaponComponentHash.AtArFlsh,
						WeaponComponentHash.PumpShotgunVarmodLowrider,
					};
				case WeaponHash.AssaultShotgun:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.AssaultShotgunClip01,
						WeaponComponentHash.AssaultShotgunClip02,
						WeaponComponentHash.AtArAfGrip,
						WeaponComponentHash.AtArFlsh,
						WeaponComponentHash.AtArSupp,
					};
				case WeaponHash.SniperRifle:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.SniperRifleClip01,
						WeaponComponentHash.AtScopeLarge,
						WeaponComponentHash.AtScopeMax,
						WeaponComponentHash.AtArSupp02,
						WeaponComponentHash.SniperRifleVarmodLuxe,
					};
				case WeaponHash.HeavySniper:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.HeavySniperClip01,
						WeaponComponentHash.AtScopeLarge,
						WeaponComponentHash.AtScopeMax,
					};
				case WeaponHash.GrenadeLauncher:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.AtArAfGrip,
						WeaponComponentHash.AtArFlsh,
						WeaponComponentHash.AtScopeSmall,
					};
				case WeaponHash.Minigun:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.MinigunClip01,
					};
				case WeaponHash.AssaultSMG:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.AssaultSMGClip01,
						WeaponComponentHash.AssaultSMGClip02,
						WeaponComponentHash.AtArFlsh,
						WeaponComponentHash.AtScopeMacro,
						WeaponComponentHash.AtArSupp02,
						WeaponComponentHash.AssaultSMGVarmodLowrider,
					};
				case WeaponHash.BullpupShotgun:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.AtArAfGrip,
						WeaponComponentHash.AtArFlsh,
						WeaponComponentHash.AtArSupp02,
					};
				case WeaponHash.Pistol50:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.Pistol50Clip01,
						WeaponComponentHash.Pistol50Clip02,
						WeaponComponentHash.AtPiFlsh,
						WeaponComponentHash.AtArSupp02,
						WeaponComponentHash.Pistol50VarmodLuxe,
					};
				case WeaponHash.CombatPDW:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.CombatPDWClip01,
						WeaponComponentHash.CombatPDWClip02,
						WeaponComponentHash.CombatPDWClip03,
						WeaponComponentHash.AtArFlsh,
						WeaponComponentHash.AtScopeSmall,
						WeaponComponentHash.AtArAfGrip,
					};
				case WeaponHash.SawnOffShotgun:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.SawnoffShotgunVarmodLuxe,
					};
				case WeaponHash.BullpupRifle:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.BullpupRifleClip01,
						WeaponComponentHash.BullpupRifleClip02,
						WeaponComponentHash.AtArFlsh,
						WeaponComponentHash.AtScopeSmall,
						WeaponComponentHash.AtArSupp,
						WeaponComponentHash.AtArAfGrip,
						WeaponComponentHash.BullpupRifleVarmodLow,
					};
				case WeaponHash.SNSPistol:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.SNSPistolClip01,
						WeaponComponentHash.SNSPistolClip02,
						WeaponComponentHash.SNSPistolVarmodLowrider,
					};
				case WeaponHash.SpecialCarbine:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.SpecialCarbineClip01,
						WeaponComponentHash.SpecialCarbineClip02,
						WeaponComponentHash.SpecialCarbineClip03,
						WeaponComponentHash.AtArFlsh,
						WeaponComponentHash.AtScopeMedium,
						WeaponComponentHash.AtArSupp02,
						WeaponComponentHash.AtArAfGrip,
						WeaponComponentHash.SpecialCarbineVarmodLowrider,
					};
				case WeaponHash.KnuckleDuster:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.KnuckleVarmodPimp,
						WeaponComponentHash.KnuckleVarmodBallas,
						WeaponComponentHash.KnuckleVarmodDollar,
						WeaponComponentHash.KnuckleVarmodDiamond,
						WeaponComponentHash.KnuckleVarmodHate,
						WeaponComponentHash.KnuckleVarmodLove,
						WeaponComponentHash.KnuckleVarmodPlayer,
						WeaponComponentHash.KnuckleVarmodKing,
						WeaponComponentHash.KnuckleVarmodVagos,
					};
				case WeaponHash.MachinePistol:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.MachinePistolClip01,
						WeaponComponentHash.MachinePistolClip02,
						WeaponComponentHash.MachinePistolClip03,
						WeaponComponentHash.AtPiSupp,
					};
				case WeaponHash.SwitchBlade:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.SwitchbladeVarmodVar1,
						WeaponComponentHash.SwitchbladeVarmodVar2,
					};
				case WeaponHash.Revolver:
					return new WeaponComponentHash[]
					{
						WeaponComponentHash.RevolverClip01,
						WeaponComponentHash.RevolverVarmodBoss,
						WeaponComponentHash.RevolverVarmodGoon,
					};
			}

			WeaponComponentHash[] result = null;

			for (int i = 0, count = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPONS); i < count; i++)
			{
				unsafe
				{
					DlcWeaponData weaponData;
					if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_DATA, i, &weaponData))
					{
						if (weaponData.Hash == hash)
						{
							result = new WeaponComponentHash[Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPON_COMPONENTS, i)];

							for (int j = 0; j < result.Length; j++)
							{
								DlcWeaponComponentData componentData;
								if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_COMPONENT_DATA, i, j, &componentData))
								{
									result[j] = componentData.Hash;
								}
								else
								{
									result[j] = WeaponComponentHash.Invalid;
								}
							}
							return result;
						}
					}
				}
			}


			if (result == null)
			{
				result = new WeaponComponentHash[0];
			}

			return result;
		}
	}
}
