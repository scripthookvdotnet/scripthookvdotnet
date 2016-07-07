using System;
using System.Collections.Generic;
using GTA;
using System.Linq;

namespace GTA
{
	public enum WeaponComponentHash : uint
	{
		AdvancedRifleClip01 = 4203716879u,
		AdvancedRifleClip02 = 2395064697u,
		AdvancedRifleVarmodLuxe = 930927479u,
		APPistolClip01 = 834974250u,
		APPistolClip02 = 614078421u,
		APPistolVarmodLuxe = 2608252716u,
		AssaultRifleClip01 = 3193891350u,
		AssaultRifleClip02 = 2971750299u,
		AssaultRifleClip03 = 3689981245u,
		AssaultRifleVarmodLuxe = 1319990579u,
		AssaultSMGClip01 = 2366834608u,
		AssaultSMGClip02 = 3141985303u,
		AssaultSMGVarmodLowrider = 663517359u,
		AssaultShotgunClip01 = 2498239431u,
		AssaultShotgunClip02 = 2260565874u,
		AtArAfGrip = 202788691u,
		AtArFlsh = 2076495324u,
		AtArSupp = 2205435306u,
		AtArSupp02 = 2805810788u,
		AtPiFlsh = 899381934u,
		AtPiSupp = 3271853210u,
		AtPiSupp02 = 1709866683u,
		AtRailCover01 = 1967214384u,
		AtScopeLarge = 3527687644u,
		AtScopeLargeFixedZoom = 471997210u,
		AtScopeMacro = 2637152041u,
		AtScopeMacro02 = 1019656791u,
		AtScopeMax = 3159677559u,
		AtScopeMedium = 2698550338u,
		AtScopeSmall = 2855028148u,
		AtScopeSmall02 = 1006677997u,
		AtSrSupp = 3859329886u,
		BullpupRifleClip01 = 3315675008u,
		BullpupRifleClip02 = 3009973007u,
		BullpupRifleVarmodLow = 2824322168u,
		BullpupShotgunClip01 = 3377353998u,
		CarbineRifleClip01 = 2680042476u,
		CarbineRifleClip02 = 2433783441u,
		CarbineRifleClip03 = 3127044405u,
		CarbineRifleVarmodLuxe = 3634075224u,
		CombatMGClip01 = 3791631178u,
		CombatMGClip02 = 3603274966u,
		CombatMGVarmodLowrider = 2466172125u,
		CombatPDWClip01 = 1125642654u,
		CombatPDWClip02 = 860508675u,
		CombatPDWClip03 = 1857603803u,
		CombatPistolClip01 = 119648377u,
		CombatPistolClip02 = 3598405421u,
		CombatPistolVarmodLowrider = 3328527730u,
		CompactRifleClip01 = 1363085923u,
		CompactRifleClip02 = 1509923832u,
		CompactRifleClip03 = 3322377230u,
		DBShotgunClip01 = 703231006u,
		FireworkClip01 = 3840197261u,
		FlareGunClip01 = 2481569177u,
		FlashlightLight = 3719772431u,
		GrenadeLauncherClip01 = 296639639u,
		GusenbergClip01 = 484812453u,
		GusenbergClip02 = 3939025520u,
		HeavyPistolClip01 = 222992026u,
		HeavyPistolClip02 = 1694090795u,
		HeavyPistolVarmodLuxe = 2053798779u,
		HeavyShotgunClip01 = 844049759u,
		HeavyShotgunClip02 = 2535257853u,
		HeavyShotgunClip03 = 2294798931u,
		HeavySniperClip01 = 1198478068u,
		HomingLauncherClip01 = 4162006335u,
		KnuckleVarmodBallas = 4007263587u,
		KnuckleVarmodBase = 4081463091u,
		KnuckleVarmodDiamond = 2539772380u,
		KnuckleVarmodDollar = 1351683121u,
		KnuckleVarmodHate = 2112683568u,
		KnuckleVarmodKing = 3800804335u,
		KnuckleVarmodLove = 1062111910u,
		KnuckleVarmodPimp = 3323197061u,
		KnuckleVarmodPlayer = 146278587u,
		KnuckleVarmodVagos = 2062808965u,
		MGClip01 = 4097109892u,
		MGClip02 = 2182449991u,
		MGVarmodLowrider = 3604658878u,
		MachinePistolClip01 = 1198425599u,
		MachinePistolClip02 = 3106695545u,
		MachinePistolClip03 = 2850671348u,
		MarksmanPistolClip01 = 3416146413u,
		MarksmanRifleClip01 = 3627761985u,
		MarksmanRifleClip02 = 3439143621u,
		MicroSMGClip01 = 3410538224u,
		MicroSMGClip02 = 283556395u,
		MicroSMGVarmodLuxe = 1215999497u,
		MinigunClip01 = 3370020614u,
		MusketClip01 = 1322387263u,
		Pistol50Clip01 = 580369945u,
		Pistol50Clip02 = 3654528146u,
		Pistol50VarmodLuxe = 2008591151u,
		PistolClip01 = 4275109233u,
		PistolClip02 = 3978713628u,
		PistolVarmodLuxe = 3610841222u,
		PoliceTorchFlashlight = 3315797997u,
		PumpShotgunClip01 = 3513717816u,
		PumpShotgunVarmodLowrider = 2732039643u,
		RPGClip01 = 1319465907u,
		RailgunClip01 = 59044840u,
		RevolverClip01 = 3917905123u,
		RevolverVarmodBoss = 384708672u,
		RevolverVarmodGoon = 2492708877u,
		SMGClip01 = 643254679u,
		SMGClip02 = 889808635u,
		SMGClip03 = 2043113590u,
		SMGVarmodLuxe = 663170192u,
		SNSPistolClip01 = 4169150169u,
		SNSPistolClip02 = 2063610803u,
		SNSPistolVarmodLowrider = 2150886575u,
		SawnoffShotgunClip01 = 3352699429u,
		SawnoffShotgunVarmodLuxe = 2242268665u,
		SniperRifleClip01 = 2613461129u,
		SniperRifleVarmodLuxe = 1077065191u,
		SpecialCarbineClip01 = 3334989185u,
		SpecialCarbineClip02 = 2089537806u,
		SpecialCarbineClip03 = 1801039530u,
		SpecialCarbineVarmodLowrider = 1929467122u,
		SwitchbladeVarmodBase = 2436343040u,
		SwitchbladeVarmodVar1 = 1530822070u,
		SwitchbladeVarmodVar2 = 3885209186u,
		VintagePistolClip01 = 1168357051u,
		VintagePistolClip02 = 867832552u,
		Invalid = 4294967295u
	}

	class WeaponComponentCollection
	{
		#region Fields
		readonly Ped _owner;
		readonly Weapon _weapon;
		readonly Dictionary<WeaponComponentHash, WeaponComponent> _weaponComponents = new Dictionary<WeaponComponentHash, WeaponComponent>();
		readonly WeaponComponentHash _components;
		readonly static InvalidWeaponComponent _invalidComponent = new InvalidWeaponComponent();
		#endregion

		internal WeaponComponentCollection(Ped owner, Weapon weapon)
		{
			_owner = owner;
			_weapon = Weapon;
			_components = GetComponentsFromHash(weapon.Hash);
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
						component = new VehicleMod(_owner, componentHash);
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
		public WeaponComponent this[int index]
		{
			get
			{
				WeaponComponent component = null;
				if (index >= 0 && index < Count)
				{
					WeaponComponentHash hash = _components[index];
				
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

		public int Count
		{
			get
			{
				return _components.Length;
			}
		}

		public IEnumerator<WeaponComponent> GetEnumerator()
		{
			return (_components as IEnumerable<WeaponComponent>).GetEnumerator();
		}

		public static WeaponComponentHash[] GetComponentsFromHash(WeaponHash hash)
		{
			switch (hash)
			{
				case WeaponHash.Pistol:
					return new WeaponComponent[] {
							WeaponComponent.PistolClip01,
							WeaponComponent.PistolClip02,
							WeaponComponent.AtPiFlsh,
							WeaponComponent.AtPiSupp02,
							WeaponComponent.PistolVarmodLuxe,
						};
				case WeaponHash.CombatPistol:
					return new WeaponComponent[] {
							WeaponComponent.CombatPistolClip01,
							WeaponComponent.CombatPistolClip02,
							WeaponComponent.AtPiFlsh,
							WeaponComponent.AtPiSupp,
							WeaponComponent.CombatPistolVarmodLowrider,
						};
				case WeaponHash.APPistol:
					return new WeaponComponent[] {
							WeaponComponent.APPistolClip01,
							WeaponComponent.APPistolClip02,
							WeaponComponent.AtPiFlsh,
							WeaponComponent.AtPiSupp,
							WeaponComponent.APPistolVarmodLuxe,
						};
				case WeaponHash.MicroSMG:
					return new WeaponComponent[] {
							WeaponComponent.MicroSMGClip01,
							WeaponComponent.MicroSMGClip02,
							WeaponComponent.AtPiFlsh,
							WeaponComponent.AtScopeMacro,
							WeaponComponent.AtArSupp02,
							WeaponComponent.MicroSMGVarmodLuxe,
						};
				case WeaponHash.SMG:
					return new WeaponComponent[] {
							WeaponComponent.SMGClip01,
							WeaponComponent.SMGClip02,
							WeaponComponent.SMGClip03,
							WeaponComponent.AtArFlsh,
							WeaponComponent.AtPiSupp,
							WeaponComponent.AtScopeMacro02,
							WeaponComponent.AtArAfGrip,
							WeaponComponent.SMGVarmodLuxe,
						};
				case WeaponHash.AssaultRifle:
					return new WeaponComponent[] {
							WeaponComponent.AssaultRifleClip01,
							WeaponComponent.AssaultRifleClip02,
							WeaponComponent.AssaultRifleClip03,
							WeaponComponent.AtArAfGrip,
							WeaponComponent.AtArFlsh,
							WeaponComponent.AtScopeMacro,
							WeaponComponent.AtArSupp02,
							WeaponComponent.AssaultRifleVarmodLuxe,
						};
				case WeaponHash.CarbineRifle:
					return new WeaponComponent[] {
							WeaponComponent.CarbineRifleClip01,
							WeaponComponent.CarbineRifleClip02,
							WeaponComponent.CarbineRifleClip03,
							WeaponComponent.AtRailCover01,
							WeaponComponent.AtArAfGrip,
							WeaponComponent.AtArFlsh,
							WeaponComponent.AtScopeMedium,
							WeaponComponent.AtArSupp,
							WeaponComponent.CarbineRifleVarmodLuxe,
						};
				case WeaponHash.AdvancedRifle:
					return new WeaponComponent[] {
							WeaponComponent.AdvancedRifleClip01,
							WeaponComponent.AdvancedRifleClip02,
							WeaponComponent.AtArFlsh,
							WeaponComponent.AtScopeSmall,
							WeaponComponent.AtArSupp,
							WeaponComponent.AdvancedRifleVarmodLuxe,
						};
				case WeaponHash.MG:
					return new WeaponComponent[] {
							WeaponComponent.MGClip01,
							WeaponComponent.MGClip02,
							WeaponComponent.AtScopeSmall02,
							WeaponComponent.AtArAfGrip,
							WeaponComponent.MGVarmodLowrider,
						};
				case WeaponHash.CombatMG:
					return new WeaponComponent[] {
							WeaponComponent.CombatMGClip01,
							WeaponComponent.CombatMGClip02,
							WeaponComponent.AtArAfGrip,
							WeaponComponent.AtScopeMedium,
							WeaponComponent.CombatMGVarmodLowrider,
						};
				case WeaponHash.PumpShotgun:
					return new WeaponComponent[] {
							WeaponComponent.AtSrSupp,
							WeaponComponent.AtArFlsh,
							WeaponComponent.PumpShotgunVarmodLowrider,
						};
				case WeaponHash.AssaultShotgun:
					return new WeaponComponent[] {
							WeaponComponent.AssaultShotgunClip01,
							WeaponComponent.AssaultShotgunClip02,
							WeaponComponent.AtArAfGrip,
							WeaponComponent.AtArFlsh,
							WeaponComponent.AtArSupp,
						};
				case WeaponHash.SniperRifle:
					return new WeaponComponent[] {
							WeaponComponent.SniperRifleClip01,
							WeaponComponent.AtScopeLarge,
							WeaponComponent.AtScopeMax,
							WeaponComponent.AtArSupp02,
							WeaponComponent.SniperRifleVarmodLuxe,
						};
				case WeaponHash.HeavySniper:
					return new WeaponComponent[] {
							WeaponComponent.HeavySniperClip01,
							WeaponComponent.AtScopeLarge,
							WeaponComponent.AtScopeMax,
						};
				case WeaponHash.GrenadeLauncher:
					return new WeaponComponent[] {
							WeaponComponent.AtArAfGrip,
							WeaponComponent.AtArFlsh,
							WeaponComponent.AtScopeSmall,
						};
				case WeaponHash.Minigun:
					return new WeaponComponent[] {
							WeaponComponent.MinigunClip01,
						};
				case WeaponHash.AssaultSMG:
					return new WeaponComponent[] {
							WeaponComponent.AssaultSMGClip01,
							WeaponComponent.AssaultSMGClip02,
							WeaponComponent.AtArFlsh,
							WeaponComponent.AtScopeMacro,
							WeaponComponent.AtArSupp02,
							WeaponComponent.AssaultSMGVarmodLowrider,
						};
				case WeaponHash.BullpupShotgun:
					return new WeaponComponent[] {
							WeaponComponent.AtArAfGrip,
							WeaponComponent.AtArFlsh,
							WeaponComponent.AtArSupp02,
						};
				case WeaponHash.Pistol50:
					return new WeaponComponent[] {
							WeaponComponent.Pistol50Clip01,
							WeaponComponent.Pistol50Clip02,
							WeaponComponent.AtPiFlsh,
							WeaponComponent.AtArSupp02,
							WeaponComponent.Pistol50VarmodLuxe,
						};
				case WeaponHash.CombatPDW:
					return new WeaponComponent[] {
							WeaponComponent.CombatPDWClip01,
							WeaponComponent.CombatPDWClip02,
							WeaponComponent.CombatPDWClip03,
							WeaponComponent.AtArFlsh,
							WeaponComponent.AtScopeSmall,
							WeaponComponent.AtArAfGrip,
						};
				case WeaponHash.SawnOffShotgun:
					return new WeaponComponent[] {
							WeaponComponent.SawnoffShotgunVarmodLuxe,
						};
				case WeaponHash.BullpupRifle:
					return new WeaponComponent[] {
							WeaponComponent.BullpupRifleClip01,
							WeaponComponent.BullpupRifleClip02,
							WeaponComponent.AtArFlsh,
							WeaponComponent.AtScopeSmall,
							WeaponComponent.AtArSupp,
							WeaponComponent.AtArAfGrip,
							WeaponComponent.BullpupRifleVarmodLow,
						};
				case WeaponHash.SNSPistol:
					return new WeaponComponent[] {
							WeaponComponent.SNSPistolClip01,
							WeaponComponent.SNSPistolClip02,
							WeaponComponent.SNSPistolVarmodLowrider,
						};
				case WeaponHash.SpecialCarbine:
					return new WeaponComponent[] {
							WeaponComponent.SpecialCarbineClip01,
							WeaponComponent.SpecialCarbineClip02,
							WeaponComponent.SpecialCarbineClip03,
							WeaponComponent.AtArFlsh,
							WeaponComponent.AtScopeMedium,
							WeaponComponent.AtArSupp02,
							WeaponComponent.AtArAfGrip,
							WeaponComponent.SpecialCarbineVarmodLowrider,
						};
				case WeaponHash.KnuckleDuster:
					return new WeaponComponent[] {
							WeaponComponent.KnuckleVarmodPimp,
							WeaponComponent.KnuckleVarmodBallas,
							WeaponComponent.KnuckleVarmodDollar,
							WeaponComponent.KnuckleVarmodDiamond,
							WeaponComponent.KnuckleVarmodHate,
							WeaponComponent.KnuckleVarmodLove,
							WeaponComponent.KnuckleVarmodPlayer,
							WeaponComponent.KnuckleVarmodKing,
							WeaponComponent.KnuckleVarmodVagos,
						};
				case WeaponHash.MachinePistol:
					return new WeaponComponent[] {
							WeaponComponent.MachinePistolClip01,
							WeaponComponent.MachinePistolClip02,
							WeaponComponent.MachinePistolClip03,
							WeaponComponent.AtPiSupp,
						};
				case WeaponHash.SwitchBlade:
					return new WeaponComponent[] {
							WeaponComponent.SwitchbladeVarmodVar1,
							WeaponComponent.SwitchbladeVarmodVar2,
						};
				case WeaponHash.Revolver:
					return new WeaponComponent[] {
							WeaponComponent.RevolverClip01,
							WeaponComponent.RevolverVarmodBoss,
							WeaponComponent.RevolverVarmodGoon,
						};
			}

			IntPtr data = Marshal.AllocCoTaskMem(39 * 8);
			WeaponComponent[] result = null;

			for (int i = 0, count = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPONS); i < count; i++)
			{
				if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_DATA, i, data))
				{
					if (MemoryAccess.ReadInt(data + 8) == (int)hash)
					{
						result = new WeaponComponent[Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPON_COMPONENTS, i)];

						for (int j = 0; j < result.Length; j++)
						{
							if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_COMPONENT_DATA, i, j, data))
							{
								result[j] = (WeaponComponent)MemoryAccess.ReadInt(data + 3 * 8);
							}
							else
							{
								result[j] = WeaponComponent.Invalid;
							}
						}
						break;
					}
				}
			}

			Marshal.FreeCoTaskMem(data);

			if (result == null)
			{
				result = new WeaponComponent[0];
			}

			return result;
		}
	}
}