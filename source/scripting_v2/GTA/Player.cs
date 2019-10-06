//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Drawing;

namespace GTA
{
	public sealed class Player : IEquatable<Player>, IHandleable
	{
		#region Fields
		Ped ped;
		#endregion

		public Player(int handle)
		{
			Handle = handle;
		}

		public int Handle
		{
			get;
		}

		public Ped Character
		{
			get
			{
				int handle = Function.Call<int>(Hash.GET_PLAYER_PED, Handle);

				if (ped == null || handle != ped.Handle)
				{
					ped = new Ped(handle);
				}

				return ped;
			}
		}

		public string Name => Function.Call<string>(Hash.GET_PLAYER_NAME, Handle);

		public int Money
		{
			get
			{
				int stat;

				switch ((PedHash)Character.Model.Hash)
				{
					case PedHash.Michael:
						stat = Game.GenerateHash("SP0_TOTAL_CASH");
						break;
					case PedHash.Franklin:
						stat = Game.GenerateHash("SP1_TOTAL_CASH");
						break;
					case PedHash.Trevor:
						stat = Game.GenerateHash("SP2_TOTAL_CASH");
						break;
					default:
						return 0;
				}

				int result;
				unsafe
				{
					Function.Call(Hash.STAT_GET_INT, stat, &result, -1);
				}

				return result;
			}
			set
			{
				int stat;

				switch ((PedHash)Character.Model.Hash)
				{
					case PedHash.Michael:
						stat = Game.GenerateHash("SP0_TOTAL_CASH");
						break;
					case PedHash.Franklin:
						stat = Game.GenerateHash("SP1_TOTAL_CASH");
						break;
					case PedHash.Trevor:
						stat = Game.GenerateHash("SP2_TOTAL_CASH");
						break;
					default:
						return;
				}

				Function.Call(Hash.STAT_SET_INT, stat, value, 1);
			}
		}

		public int WantedLevel
		{
			get => Function.Call<int>(Hash.GET_PLAYER_WANTED_LEVEL, Handle);
			set
			{
				Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Handle, value, false);
				Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Handle, false);
			}
		}

		public Vector3 WantedCenterPosition
		{
			get => Function.Call<Vector3>(Hash.GET_PLAYER_WANTED_CENTRE_POSITION, Handle);
			set => Function.Call(Hash.SET_PLAYER_WANTED_CENTRE_POSITION, Handle, value.X, value.Y, value.Z);
		}

		public int MaxArmor
		{
			get => Function.Call<int>(Hash.GET_PLAYER_MAX_ARMOUR, Handle);
			set => Function.Call(Hash.SET_PLAYER_MAX_ARMOUR, Handle, value);
		}

		public Color Color
		{
			get
			{
				int r = 0, g = 0, b = 0;
				unsafe
				{
					Function.Call(Hash.GET_PLAYER_RGB_COLOUR, Handle, &r, &g, &b);
				}
				return Color.FromArgb(r, g, b);
			}
		}

		public ParachuteTint PrimaryParachuteTint
		{
			get
			{
				int result;

				unsafe
				{
					Function.Call(Hash.GET_PLAYER_PARACHUTE_TINT_INDEX, Handle, &result);
				}

				return (ParachuteTint)result;
			}
			set => Function.Call(Hash.SET_PLAYER_PARACHUTE_TINT_INDEX, Handle, (int)value);
		}
		public ParachuteTint ReserveParachuteTint
		{
			get
			{
				int result;
				unsafe
				{
					Function.Call(Hash.GET_PLAYER_RESERVE_PARACHUTE_TINT_INDEX, Handle, &result);
				}

				return (ParachuteTint)result;
			}
			set => Function.Call(Hash.SET_PLAYER_RESERVE_PARACHUTE_TINT_INDEX, Handle, (int)value);
		}

		public bool IsDead => Function.Call<bool>(Hash.IS_PLAYER_DEAD, Handle);

		public bool IsAlive => !IsDead;

		public bool IsAiming => Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING, Handle);

		public bool IsClimbing => Function.Call<bool>(Hash.IS_PLAYER_CLIMBING, Handle);

		public bool IsRidingTrain => Function.Call<bool>(Hash.IS_PLAYER_RIDING_TRAIN, Handle);

		public bool IsPressingHorn => Function.Call<bool>(Hash.IS_PLAYER_PRESSING_HORN, Handle);

		public bool IsPlaying => Function.Call<bool>(Hash.IS_PLAYER_PLAYING, Handle);

		public bool IsInvincible
		{
			get => Function.Call<bool>(Hash.GET_PLAYER_INVINCIBLE, Handle);
			set => Function.Call(Hash.SET_PLAYER_INVINCIBLE, Handle, value);
		}

		public bool IgnoredByPolice
		{
			set => Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Handle, value);
		}

		public bool IgnoredByEveryone
		{
			set => Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Handle, value);
		}

		public bool CanUseCover
		{
			set => Function.Call(Hash.SET_PLAYER_CAN_USE_COVER, Handle, value);
		}

		public bool CanStartMission
		{
			get => Function.Call<bool>(Hash.CAN_PLAYER_START_MISSION, Handle);
		}

		public bool CanControlRagdoll
		{
			set => Function.Call(Hash.GIVE_PLAYER_RAGDOLL_CONTROL, Handle, value);
		}

		public bool CanControlCharacter
		{
			get => Function.Call<bool>(Hash.IS_PLAYER_CONTROL_ON, Handle);
			set => Function.Call(Hash.SET_PLAYER_CONTROL, Handle, value, 0);
		}

		public bool ChangeModel(Model model)
		{
			if (!model.IsInCdImage || !model.IsPed || !model.Request(1000))
			{
				return false;
			}

			Function.Call(Hash.SET_PLAYER_MODEL, Handle, model.Hash);
			model.MarkAsNoLongerNeeded();
			return true;
		}

		public float RemainingSprintTime => Function.Call<float>(Hash.GET_PLAYER_SPRINT_TIME_REMAINING, Handle);

		public float RemainingUnderwaterTime => Function.Call<float>(Hash.GET_PLAYER_UNDERWATER_TIME_REMAINING, Handle);

		public void RefillSpecialAbility()
		{
			Function.Call(Hash.SPECIAL_ABILITY_FILL_METER, Handle, 1);
		}

		public Vehicle LastVehicle => Function.Call<Vehicle>(Hash.GET_PLAYERS_LAST_VEHICLE);

		public bool IsTargetting(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Handle, entity.Handle);
		}

		public bool IsTargettingAnything => Function.Call<bool>(Hash.IS_PLAYER_TARGETTING_ANYTHING, Handle);

		public Entity GetTargetedEntity()
		{
			int entity = 0;
			unsafe
			{
				if (Function.Call<bool>(Hash._GET_AIMED_ENTITY, Handle, &entity))
				{
					return Entity.FromHandle(entity);
				}
			}

			return null;
		}

		public void DisableFiringThisFrame()
		{
			Function.Call(Hash.DISABLE_PLAYER_FIRING, Handle, 0);
		}

		public void SetRunSpeedMultThisFrame(float value)
		{
			Function.Call(Hash.SET_RUN_SPRINT_MULTIPLIER_FOR_PLAYER, Handle, value > 1.499f ? 1.499f : value);
		}

		public void SetSwimSpeedMultThisFrame(float value)
		{
			Function.Call(Hash.SET_SWIM_MULTIPLIER_FOR_PLAYER, Handle, value > 1.499f ? 1.499f : value);
		}

		public void SetFireAmmoThisFrame()
		{
			Function.Call(Hash.SET_FIRE_AMMO_THIS_FRAME, Handle);
		}

		public void SetExplosiveAmmoThisFrame()
		{
			Function.Call(Hash.SET_EXPLOSIVE_AMMO_THIS_FRAME, Handle);
		}

		public void SetExplosiveMeleeThisFrame()
		{
			Function.Call(Hash.SET_EXPLOSIVE_MELEE_THIS_FRAME, Handle);
		}

		public void SetSuperJumpThisFrame()
		{
			Function.Call(Hash.SET_SUPER_JUMP_THIS_FRAME, Handle);
		}

		public void SetMayNotEnterAnyVehicleThisFrame()
		{
			Function.Call(Hash.SET_PLAYER_MAY_NOT_ENTER_ANY_VEHICLE, Handle);
		}

		public void SetMayOnlyEnterThisVehicleThisFrame(Vehicle vehicle)
		{
			Function.Call(Hash.SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE, Handle, vehicle);
		}

		public bool Exists()
		{
			// IHandleable forces us to implement this unfortunately,
			// so we'll implement it explicitly and return true
			return true;
		}

		public bool Equals(Player obj)
		{
			return !(obj is null) && Handle == obj.Handle;
		}
		public override bool Equals(object obj)
		{
			return !(obj is null) && obj.GetType() == GetType() && Equals((Player)obj);
		}

		public static bool operator ==(Player left, Player right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		public static bool operator !=(Player left, Player right)
		{
			return !(left == right);
		}

		public sealed override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
