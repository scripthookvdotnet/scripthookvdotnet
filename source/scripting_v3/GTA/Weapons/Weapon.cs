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
		readonly Ped _owner;
		WeaponComponentCollection _components;
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

		/// <summary>
		/// Gets the hash for this <see cref="Weapon"/>.
		/// </summary>
		public WeaponHash Hash
		{
			get;
		}

		/// <summary>
		/// Gets the slot hash for this <see cref="Weapon"/>.
		/// </summary>
		/// <remarks>
		/// The slot hash must be unique in a weapon inventory of a <see cref="Ped"/>, so a <see cref="Ped"/> cannot have multiple <see cref="Weapon"/> items with the same slot hash in their inventory.
		/// </remarks>
		public int SlotHash => Function.Call<int>(Native.Hash.GET_WEAPONTYPE_SLOT, (uint)Hash);

		/// <summary>
		/// Gets the display name label string for this <see cref="Weapon"/>.
		/// </summary>
		public string DisplayName => GetDisplayNameFromHash(Hash);

		/// <summary>
		/// Gets the localized human name for this <see cref="Weapon"/>.
		/// </summary>
		public string LocalizedName => Game.GetLocalizedString((int)SHVDN.NativeMemory.GetHumanNameHashOfWeaponInfo((uint)Hash));

		public bool IsPresent => Hash == WeaponHash.Unarmed || Function.Call<bool>(Native.Hash.HAS_PED_GOT_WEAPON, _owner.Handle, (uint)Hash);

		public Model Model => new(Function.Call<int>(Native.Hash.GET_WEAPONTYPE_MODEL, (uint)Hash));

		public WeaponTint Tint
		{
			get => Function.Call<WeaponTint>(Native.Hash.GET_PED_WEAPON_TINT_INDEX, _owner.Handle, (uint)Hash);
			set => Function.Call(Native.Hash.SET_PED_WEAPON_TINT_INDEX, _owner.Handle, (uint)Hash, (int)value);
		}

		/// <summary>
		/// Gets the number of available color tints for this <see cref="Weapon"/>.
		/// </summary>
		public int TintCount => Function.Call<int>(Native.Hash.GET_WEAPON_TINT_COUNT, (uint)Hash);

		public WeaponGroup Group => Function.Call<WeaponGroup>(Native.Hash.GET_WEAPONTYPE_GROUP, (uint)Hash);

		/// <summary>
		/// Gets or sets the amount of ammo for this weapon.
		/// If this weapon is using special ammo, this property will return the value for it.
		/// </summary>
		/// <remarks>
		/// Will return 1 if <see cref="Hash"/> is <see cref="WeaponHash.Unarmed"/> instead of 0.
		/// </remarks>
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
		/// <summary>
		/// Gets or sets the current amount of ammo in a clip.
		/// </summary>
		/// <remarks>
		/// Will return 1 if <see cref="Hash"/> is <see cref="WeaponHash.Unarmed"/> instead of 0.
		/// </remarks>
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

		/// <summary>
		/// Gets or sets the max amount of ammo this weapon can have.
		/// </summary>
		/// <remarks>
		/// Will return 1 if <see cref="Hash"/> is <see cref="WeaponHash.Unarmed"/> instead of 0.
		/// </remarks>
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
		/// <summary>
		/// Gets or sets the max amount of ammo this weapon can have in a clip.
		/// </summary>
		/// <remarks>
		/// Will return 1 if <see cref="Hash"/> is <see cref="WeaponHash.Unarmed"/> instead of 0.
		/// </remarks>
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

		public int DefaultClipSize => Function.Call<int>(Native.Hash.GET_WEAPON_CLIP_SIZE, (uint)Hash);


		/// <summary>
		/// Sets whether this ped will not consume the current ammo this weapon is using from the weapon ammo inventory.
		/// </summary>
		/// <remarks>
		/// Despite the interface, setting this value globally affects any of the weapons that uses the ammo the current weapon is using
		/// as <c>SET_PED_INFINITE_AMMO</c> modifies a of member of weapon ammo item in <c>CPedInventory</c> of the owner <see cref="Ped"/>.
		/// </remarks>
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
		/// <summary>
		/// Sets whether this ped will not consume any ammo in any clips or that of the weapon ammo inventory of the owner <see cref="Ped"/>.
		/// </summary>
		/// <remarks>
		/// Despite the interface, setting this value globally affects all of the weapons of the owner <see cref="Ped"/>.
		/// </remarks>
		public bool InfiniteAmmoClip
		{
			set => Function.Call(Native.Hash.SET_PED_INFINITE_AMMO_CLIP, _owner.Handle, value);
		}

		public bool CanUseOnParachute => Function.Call<bool>(Native.Hash.CAN_USE_WEAPON_ON_PARACHUTE, (uint)Hash);

		public WeaponComponentCollection Components => _components ?? (_components = new WeaponComponentCollection(_owner, this));

		public static implicit operator WeaponHash(Weapon weapon)
		{
			return weapon.Hash;
		}

		/// <summary>
		/// Gets the display name label string for the <see cref="WeaponHash"/>.
		/// </summary>
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
					if (!Function.Call<bool>(Native.Hash.GET_DLC_WEAPON_DATA, i, &data))
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

		/// <summary>
		/// Gets the localized human name for the <see cref="WeaponHash"/>.
		/// </summary>
		public static string GetHumanNameFromHash(WeaponHash hash) => Game.GetLocalizedString((int)SHVDN.NativeMemory.GetHumanNameHashOfWeaponInfo((uint)hash));

		public static WeaponHash[] GetAllWeaponHashesForHumanPeds()
		{
			return SHVDN.NativeMemory.GetAllWeaponHashesForHumanPeds().Select(x => (WeaponHash)x).ToArray();
		}

		public static Model[] GetAllModels()
		{
			return SHVDN.NativeMemory.WeaponModels.Select(x => new Model(x)).ToArray();
		}
	}
}
