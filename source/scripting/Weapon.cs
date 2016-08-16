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

	public sealed class Weapon
	{
		#region Fields
		Ped _owner;
		WeaponComponentCollection _components;
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
		public string DisplayName
		{
			get
			{
				return  GetDisplayNameFromHash(Hash);
			}
		}
		public string LocalizedName
		{
			get
			{
				return Game.GetGXTEntry(DisplayName);
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

				int ammoInClip;
				unsafe
				{
					Function.Call(Native.Hash.GET_AMMO_IN_CLIP, _owner.Handle, Hash, &ammoInClip);
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

			    int maxAmmo;
				unsafe
				{
					Function.Call(Native.Hash.GET_MAX_AMMO, _owner.Handle, Hash, &maxAmmo);
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

		public WeaponComponentCollection Components
		{
			get
			{
				if (_components == null)
				{
					_components = new WeaponComponentCollection(_owner, this);
				}
				return _components;
			}
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

	}
}
