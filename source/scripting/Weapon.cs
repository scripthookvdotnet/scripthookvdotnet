using System;
using System.Runtime.InteropServices;
using GTA.Native;

namespace GTA
{
	public enum WeaponTint
	{
		Normal,
		Green,
		Gold,
		Pink,
		Army,
		LSPD,
		Orange,
		Platinum
	}
	public enum WeaponGroup : uint
	{
		Unarmed = 2685387236u,
		Melee = 3566412244u,
		Pistol = 416676503u,
		SMG = 3337201093u,
		AssaultRifle = 3352383570u,
		MG = 1159398588u,
		Shotgun = 860033945u,
		Sniper = 3082541095u,
		Heavy = 2725924767u,
		Thrown = 1548507267u,
		PetrolCan = 1595662460u
	}
	public enum WeaponComponent : uint
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

	public sealed class Weapon
	{
		#region Fields
		Ped _owner;
		#endregion

		internal Weapon()
		{
			Hash = WeaponHash.Unarmed;
		}
		internal Weapon(Ped owner, WeaponHash hash)
		{
			_owner = owner;
			Hash = hash;
		}

		public WeaponHash Hash { get; private set; }
		public static implicit operator WeaponHash(Weapon weapon)
		{
			return weapon.Hash;
		}

		public bool IsPresent
		{
			get
			{
				if (Hash == WeaponHash.Unarmed)
				{
					return true;
				}

				return Function.Call<bool>(Native.Hash.HAS_PED_GOT_WEAPON, _owner.Handle, Hash);
			}
		}

		public string Name
		{
			get
			{
				return Game.GetGXTEntry(GetDisplayNameFromHash(Hash));
			}
		}
		public Model Model
		{
			get
			{
				return new Model(Function.Call<int>(Native.Hash.GET_WEAPONTYPE_MODEL, Hash));
			}
		}
		public WeaponTint Tint
		{
			get
			{
				return Function.Call<WeaponTint>(Native.Hash.GET_PED_WEAPON_TINT_INDEX, _owner.Handle, Hash);
			}
			set
			{
				Function.Call(Native.Hash.SET_PED_WEAPON_TINT_INDEX, _owner.Handle, Hash, value);
			}
		}
		public WeaponGroup Group
		{
			get
			{
				return Function.Call<WeaponGroup>(Native.Hash.GET_WEAPONTYPE_GROUP, Hash);
			}
		}

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

				return Function.Call<int>(Native.Hash.GET_AMMO_IN_PED_WEAPON, _owner.Handle, Hash);
			}
			set
			{
				if (Hash == WeaponHash.Unarmed)
				{
					return;
				}

				if (IsPresent)
				{
					Function.Call(Native.Hash.SET_PED_AMMO, _owner.Handle, Hash, value);
				}
				else
				{
					Function.Call(Native.Hash.GIVE_WEAPON_TO_PED, _owner.Handle, Hash, value, false, true);
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

				var ammoInClip = new OutputArgument();
				Function.Call(Native.Hash.GET_AMMO_IN_CLIP, _owner.Handle, Hash, ammoInClip);

				return ammoInClip.GetResult<int>();
			}
			set
			{
				if (Hash == WeaponHash.Unarmed)
				{
					return;
				}

				if (IsPresent)
				{
					Function.Call(Native.Hash.SET_AMMO_IN_CLIP, _owner.Handle, Hash, value);
				}
				else
				{
					Function.Call(Native.Hash.GIVE_WEAPON_TO_PED, _owner.Handle, Hash, value, true, false);
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

				var maxAmmo = new OutputArgument();
				Function.Call(Native.Hash.GET_MAX_AMMO, _owner.Handle, Hash, maxAmmo);

				return maxAmmo.GetResult<int>();
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

				return Function.Call<int>(Native.Hash.GET_MAX_AMMO_IN_CLIP, _owner.Handle, Hash, true);
			}
		}
		public int DefaultClipSize
		{
			get
			{
				return Function.Call<int>(Native.Hash.GET_WEAPON_CLIP_SIZE, Hash);
			}
		}
		public bool InfiniteAmmo
		{
			set
			{
				if (Hash == WeaponHash.Unarmed)
				{
					return;
				}

				Function.Call(Native.Hash.SET_PED_INFINITE_AMMO, _owner.Handle, value, Hash);
			}
		}
		public bool InfiniteAmmoClip
		{
			set
			{
				Function.Call(Native.Hash.SET_PED_INFINITE_AMMO_CLIP, _owner.Handle, value);
			}
		}

		public bool CanUseOnParachute
		{
			get
			{
				return Function.Call<bool>(Native.Hash.CAN_USE_WEAPON_ON_PARACHUTE, Hash);
			}
		}

		public int MaxComponents
		{
			get
			{
				return GetComponentsFromHash(Hash).Length;
			}
		}
		public WeaponComponent GetComponent(int index)
		{
			var components = GetComponentsFromHash(Hash);

			if (index >= components.Length)
			{
				return WeaponComponent.Invalid;
			}

			return components[index];
		}
		public string GetComponentName(WeaponComponent component)
		{
			return Game.GetGXTEntry(GetComponentDisplayNameFromHash(Hash, component));
		}
		public void SetComponent(WeaponComponent component, bool active)
		{
			if (active)
			{
				Function.Call(Native.Hash.GIVE_WEAPON_COMPONENT_TO_PED, _owner.Handle, Hash, component);
			}
			else
			{
				Function.Call(Native.Hash.REMOVE_WEAPON_COMPONENT_FROM_PED, _owner.Handle, Hash, component);
			}
		}
		public bool IsComponentActive(WeaponComponent component)
		{
			return Function.Call<bool>(Native.Hash.HAS_PED_GOT_WEAPON_COMPONENT, _owner.Handle, Hash, component);
		}

		public static string GetDisplayNameFromHash(WeaponHash hash)
		{
			switch (hash)
			{
				case WeaponHash.Pistol:
					return "WTT_PIST";
				case WeaponHash.CombatPistol:
					return "WTT_PIST_CBT";
				case WeaponHash.APPistol:
					return "WTT_PIST_AP";
				case WeaponHash.SMG:
					return "WTT_SMG";
				case WeaponHash.MicroSMG:
					return "WTT_SMG_MCR";
				case WeaponHash.AssaultRifle:
					return "WTT_RIFLE_ASL";
				case WeaponHash.CarbineRifle:
					return "WTT_RIFLE_CBN";
				case WeaponHash.AdvancedRifle:
					return "WTT_RIFLE_ADV";
				case WeaponHash.MG:
					return "WTT_MG";
				case WeaponHash.CombatMG:
					return "WTT_MG_CBT";
				case WeaponHash.PumpShotgun:
					return "WTT_SG_PMP";
				case WeaponHash.SawnOffShotgun:
					return "WTT_SG_SOF";
				case WeaponHash.AssaultShotgun:
					return "WTT_SG_ASL";
				case WeaponHash.HeavySniper:
					return "WTT_SNIP_HVY";
				case WeaponHash.SniperRifle:
					return "WTT_SNIP_RIF";
				case WeaponHash.GrenadeLauncher:
					return "WTT_GL";
				case WeaponHash.RPG:
					return "WTT_RPG";
				case WeaponHash.Minigun:
					return "WTT_MINIGUN";
				case WeaponHash.AssaultSMG:
					return "WTT_SMG_ASL";
				case WeaponHash.BullpupShotgun:
					return "WTT_SG_BLP";
				case WeaponHash.Pistol50:
					return "WTT_PIST_50";
				case WeaponHash.Bottle:
					return "WTT_BOTTLE";
				case WeaponHash.Gusenberg:
					return "WTT_GUSENBERG";
				case WeaponHash.SNSPistol:
					return "WTT_SNSPISTOL";
				case WeaponHash.VintagePistol:
					return "WTT_VPISTOL";
				case WeaponHash.Dagger:
					return "WTT_DAGGER";
				case WeaponHash.FlareGun:
					return "WTT_FLAREGUN";
				case WeaponHash.Musket:
					return "WTT_MUSKET";
				case WeaponHash.Firework:
					return "WTT_FWRKLNCHR";
				case WeaponHash.MarksmanRifle:
					return "WTT_HMKRIFLE";
				case WeaponHash.HeavyShotgun:
					return "WTT_HVYSHOT";
				case WeaponHash.ProximityMine:
					return "WTT_PRXMINE";
				case WeaponHash.HomingLauncher:
					return "WTT_HOMLNCH";
				case WeaponHash.CombatPDW:
					return "WTT_COMBATPDW";
				case WeaponHash.KnuckleDuster:
					return "WTT_KNUCKLE";
				case WeaponHash.MarksmanPistol:
					return "WTT_MKPISTOL";
				case WeaponHash.Machete:
					return "WTT_MACHETE";
				case WeaponHash.MachinePistol:
					return "WTT_MCHPIST";
				case WeaponHash.Flashlight:
					return "WTT_FLASHLIGHT";
				case WeaponHash.DoubleBarrelShotgun:
					return "WTT_DBSHGN";
				case WeaponHash.CompactRifle:
					return "WTT_CMPRIFLE";
				case WeaponHash.SwitchBlade:
					return "WTT_SWBLADE";
				case WeaponHash.Revolver:
					return "WTT_REVOLVER";
			}

			IntPtr data = Marshal.AllocCoTaskMem(39 * 8);
			string result = string.Empty;

			for (int i = 0, count = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPONS); i < count; i++)
			{
				if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_DATA, i, data))
				{
					if (MemoryAccess.ReadInt(data + 8) == (int)hash)
					{
						result = MemoryAccess.ReadString(data + 23 * 8);
						break;
					}
				}
			}

			Marshal.FreeCoTaskMem(data);

			return result;
		}
		public static WeaponComponent[] GetComponentsFromHash(WeaponHash hash)
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
					case WeaponComponent.AtArSupp:
					case WeaponComponent.AtArSupp02:
					case WeaponComponent.AtSrSupp:
						return "WCT_SUPP";
					case WeaponComponent.PistolClip01:
					case WeaponComponent.CombatPistolClip01:
					case WeaponComponent.APPistolClip01:
					case WeaponComponent.MicroSMGClip01:
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
					case WeaponComponent.AtScopeLargeFixedZoom:
						return "WCT_SCOPE_LRG";
					case WeaponComponent.AtPiSupp02:
						return "WCT_SUPP";
					case WeaponComponent.AssaultRifleVarmodLuxe:
					case WeaponComponent.CarbineRifleVarmodLuxe:
					case WeaponComponent.PistolVarmodLuxe:
					case WeaponComponent.SMGVarmodLuxe:
					case WeaponComponent.MicroSMGVarmodLuxe:
					case (WeaponComponent)0x161E9241:
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
					case WeaponComponent.SMGClip03:
					case WeaponComponent.AssaultRifleClip03:
					case WeaponComponent.HeavyShotgunClip03:
						return "WCT_CLIP_DRM";
					case WeaponComponent.CarbineRifleClip03:
						return "WCT_CLIP_BOX";
				}
			}

			IntPtr data = Marshal.AllocCoTaskMem(39 * 8);
			string result = string.Empty;

			for (int i = 0, count = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPONS); i < count; i++)
			{
				if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_DATA, i, data))
				{
					if (MemoryAccess.ReadInt(data + 8) == (int)hash)
					{
						int maxComp = Function.Call<int>(Native.Hash.GET_NUM_DLC_WEAPON_COMPONENTS, i);

						for (int j = 0; j < maxComp; j++)
						{
							if (Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_COMPONENT_DATA, i, j, data))
							{
								if (MemoryAccess.ReadInt(data + 3 * 8) == (int)component)
								{
									result = MemoryAccess.ReadString(data + 6 * 8);
									break;
								}
							}
						}
						break;
					}
				}
			}

			Marshal.FreeCoTaskMem(data);

			return result;
		}
	}
}
