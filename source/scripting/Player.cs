using GTA.Math;
using GTA.Native;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GTA
{
	public enum ParachuteTint
	{
		None = -1,
		Rainbow,
		Red,
		SeasideStripes,
		WidowMaker,
		Patriot,
		Blue,
		Black,
		Hornet,
		AirFocce,
		Desert,
		Shadow,
		HighAltitude,
		Airbone,
		Sunrise
	}

	public sealed class Player : IEquatable<Player>, IHandleable
	{
		public Player(int handle)
		{
			Handle = handle;
		}

		Ped _ped;

		public int Handle { get; private set; }

		public Ped Character
		{
			get
			{
				int handle = Function.Call<int>(Hash.GET_PLAYER_PED, Handle);

				if (ReferenceEquals(_ped, null) || handle != _ped.Handle)
				{
					_ped = new Ped(handle);
				}

				return _ped;
			}
		}

		public string Name
		{
			get
			{
				return Function.Call<string>(Hash.GET_PLAYER_NAME, Handle);
			}
		}
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

				var result = new OutputArgument();
				Function.Call(Hash.STAT_GET_INT, stat, result, -1);

				return result.GetResult<int>();
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
			get
			{
				return Function.Call<int>(Hash.GET_PLAYER_WANTED_LEVEL, Handle);
			}
			set
			{
				Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Handle, value, false);
				Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Handle, false);
			}
		}
		public Vector3 WantedCenterPosition
		{
			get
			{
				return Function.Call<Vector3>(Hash.GET_PLAYER_WANTED_CENTRE_POSITION, Handle);
			}
			set
			{
				Function.Call(Hash.SET_PLAYER_WANTED_CENTRE_POSITION, Handle, value.X, value.Y, value.Z);
			}
		}

		public int MaxArmor
		{
			get
			{
				return Function.Call<int>(Hash.GET_PLAYER_MAX_ARMOUR, Handle);
			}
			set
			{
				Function.Call(Hash.SET_PLAYER_MAX_ARMOUR, Handle, value);
			}
		}

		public ParachuteTint PrimaryParachuteTint
		{
			get
			{
				var result = new OutputArgument();
				Function.Call(Hash.GET_PLAYER_PARACHUTE_TINT_INDEX, Handle, result);

				return (ParachuteTint)result.GetResult<int>();
			}
			set
			{
				Function.Call(Hash.SET_PLAYER_PARACHUTE_TINT_INDEX, Handle, (int)value);
			}
		}
		public ParachuteTint ReserveParachuteTint
		{
			get
			{
				var result = new OutputArgument();
				Function.Call(Hash.GET_PLAYER_RESERVE_PARACHUTE_TINT_INDEX, Handle, result);

				return (ParachuteTint)result.GetResult<int>();
			}
			set
			{
				Function.Call(Hash.SET_PLAYER_RESERVE_PARACHUTE_TINT_INDEX, Handle, (int)value);
			}
		}

		public bool IsAlive
		{
			get
			{
				return !IsDead;
			}
		}
		public bool IsDead
		{
			get
			{
				return Function.Call<bool>(Hash.IS_PLAYER_DEAD, Handle);
			}
		}
		public bool IsAiming
		{
			get
			{
				return Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING, Handle);
			}
		}
		public bool IsClimbing
		{
			get
			{
				return Function.Call<bool>(Hash.IS_PLAYER_CLIMBING, Handle);
			}
		}
		public bool IsRidingTrain
		{
			get
			{
				return Function.Call<bool>(Hash.IS_PLAYER_RIDING_TRAIN, Handle);
			}
		}
		public bool IsPressingHorn
		{
			get
			{
				return Function.Call<bool>(Hash.IS_PLAYER_PRESSING_HORN, Handle);
			}
		}
		public bool IsPlaying
		{
			get
			{
				return Function.Call<bool>(Hash.IS_PLAYER_PLAYING, Handle);
			}
		}
		public bool IsInvincible
		{
			get
			{
				return Function.Call<bool>(Hash.GET_PLAYER_INVINCIBLE, Handle);
			}
			set
			{
				Function.Call(Hash.SET_PLAYER_INVINCIBLE, Handle, value);
			}
		}

		public bool IgnoredByPolice
		{
			set
			{
				Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Handle, value);
			}
		}
		public bool IgnoredByEveryone
		{
			set
			{
				Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Handle, value);
			}
		}

		public bool CanUseCover
		{
			set
			{
				Function.Call(Hash.SET_PLAYER_CAN_USE_COVER, Handle, value);
			}
		}
		public bool CanStartMission
		{
			get
			{
				return Function.Call<bool>(Hash.CAN_PLAYER_START_MISSION, Handle);
			}
		}

		public bool CanControlRagdoll
		{
			set
			{
				Function.Call(Hash.GIVE_PLAYER_RAGDOLL_CONTROL, Handle, value);
			}
		}
		public bool CanControlCharacter
		{
			get
			{
				return Function.Call<bool>(Hash.IS_PLAYER_CONTROL_ON, Handle);
			}
			set
			{
				Function.Call(Hash.SET_PLAYER_CONTROL, Handle, value, 0);
			}
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

		public int RemainingSprintTime
		{
			get
			{
				return Function.Call<int>(Hash.GET_PLAYER_SPRINT_TIME_REMAINING, Handle);
			}
		}
		public int RemainingUnderwaterTime
		{
			get
			{
				return Function.Call<int>(Hash.GET_PLAYER_UNDERWATER_TIME_REMAINING, Handle);
			}
		}
		public void RefillSpecialAbility()
		{
			Function.Call(Hash.SPECIAL_ABILITY_FILL_METER, Handle, 1);
		}

		public Vehicle LastVehicle
		{
			get
			{
				return new Vehicle(Function.Call<int>(Hash.GET_PLAYERS_LAST_VEHICLE));
			}
		}

		public bool IsTargetting(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Handle, entity.Handle);
		}
		public bool IsTargettingAnything
		{

			get
			{
				return Function.Call<bool>(Hash.IS_PLAYER_TARGETTING_ANYTHING, Handle);
			}
		}
		public Entity GetTargetedEntity()
		{
			var entityArg = new OutputArgument();

			if (Function.Call<bool>(Hash.GET_ENTITY_PLAYER_IS_FREE_AIMING_AT, Handle, entityArg))
			{
				int handle = entityArg.GetResult<int>();

				if (Function.Call<bool>(Hash.DOES_ENTITY_EXIST, handle))
				{
					switch (Function.Call<int>(Hash.GET_ENTITY_TYPE, handle))
					{
						case 1:
							return new Ped(handle);
						case 2:
							return new Vehicle(handle);
						case 3:
							return new Prop(handle);
					}
				}
			}
			return null;
		}

		public void DisableFiringThisFrame()
		{
			Function.Call(Hash.DISABLE_PLAYER_FIRING, Handle, 0);
		}
		public void SetRunSpeedMultThisFrame(float mult)
		{
			if (mult > 1.499f)
			{
				mult = 1.499f;
			}

			Function.Call(Hash.SET_RUN_SPRINT_MULTIPLIER_FOR_PLAYER, Handle, mult);
		}
		public void SetSwimSpeedMultThisFrame(float mult)
		{
			if (mult > 1.499f)
			{
				mult = 1.499f;
			}

			Function.Call(Hash.SET_SWIM_MULTIPLIER_FOR_PLAYER, Handle, mult);
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
			Function.Call(Hash.SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE, Handle, vehicle.Handle);
		}

		public bool Exists()
		{
			return true;
		}

		public bool Equals(Player player)
		{
			return !ReferenceEquals(player, null) && Handle == player.Handle;
		}
		public override bool Equals(object obj)
		{
			return !ReferenceEquals(obj, null) && obj.GetType() == GetType() && Equals((Entity)obj);
		}

		public override int GetHashCode()
		{
			return Handle;
		}

		public static bool operator ==(Player left, Player right)
		{
			return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);
		}
		public static bool operator !=(Player left, Player right)
		{
			return !(left == right);
		}
	}
}
