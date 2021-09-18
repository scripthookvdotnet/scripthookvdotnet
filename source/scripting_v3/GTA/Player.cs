//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System.Drawing;

namespace GTA
{
	public sealed class Player : INativeValue
	{
		#region Fields
		Ped ped;
		#endregion

		internal Player(int handle)
		{
			Handle = handle;
		}

		public int Handle
		{
			get;
			private set;
		}

		public ulong NativeValue
		{
			get => (ulong)Handle;
			set => Handle = unchecked((int)value);
		}

		/// <summary>
		/// Gets the <see cref="Ped"/> this <see cref="Player"/> is controlling.
		/// </summary>
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

		/// <summary>
		/// Gets the Social Club name of this <see cref="Player"/>.
		/// </summary>
		public string Name => Function.Call<string>(Hash.GET_PLAYER_NAME, Handle);

		/// <summary>
		/// Gets or sets how much money this <see cref="Player"/> has.
		/// <remarks>Only works if current player is <see cref="PedHash.Michael"/>, <see cref="PedHash.Franklin"/> or <see cref="PedHash.Trevor"/></remarks>
		/// </summary>
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

		/// <summary>
		/// Gets or sets the wanted level for this <see cref="Player"/>.
		/// </summary>
		public int WantedLevel
		{
			get => Function.Call<int>(Hash.GET_PLAYER_WANTED_LEVEL, Handle);
			set
			{
				Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Handle, value, false);
				Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Handle, false);
			}
		}

		/// <summary>
		/// Gets or sets the wanted center position for this <see cref="Player"/>.
		/// </summary>
		/// <value>
		/// The place in world coordinates where the police think this <see cref="Player"/> is.
		/// </value>
		public Vector3 WantedCenterPosition
		{
			get => Function.Call<Vector3>(Hash.GET_PLAYER_WANTED_CENTRE_POSITION, Handle);
			set => Function.Call(Hash.SET_PLAYER_WANTED_CENTRE_POSITION, Handle, value.X, value.Y, value.Z);
		}

		/// <summary>
		/// Gets or sets the maximum amount of armor this <see cref="Player"/> can carry.
		/// </summary>
		public int MaxArmor
		{
			get => Function.Call<int>(Hash.GET_PLAYER_MAX_ARMOUR, Handle);
			set => Function.Call(Hash.SET_PLAYER_MAX_ARMOUR, Handle, value);
		}

		/// <summary>
		/// Gets or sets the primary parachute tint for this <see cref="Player"/>.
		/// </summary>
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
			set => Function.Call(Hash.SET_PLAYER_PARACHUTE_TINT_INDEX, Handle, value);
		}
		/// <summary>
		/// Gets or sets the reserve parachute tint for this <see cref="Player"/>.
		/// </summary>
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
			set => Function.Call(Hash.SET_PLAYER_RESERVE_PARACHUTE_TINT_INDEX, Handle, value);
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Player"/> can leave a parachute smoke trail.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> can leave a parachute smoke trail; otherwise, <see langword="false" />.
		/// </value>
		public bool CanLeaveParachuteSmokeTrail
		{
			set => Function.Call(Hash.SET_PLAYER_CAN_LEAVE_PARACHUTE_SMOKE_TRAIL, Handle, value);
		}

		/// <summary>
		/// Gets or sets the color of the parachute smoke trail for this <see cref="Player"/>.
		/// </summary>
		/// <value>
		/// The color of the parachute smoke trail for this <see cref="Player"/>.
		/// </value>
		public Color ParachuteSmokeTrailColor
		{
			get
			{
				int r, g, b;
				unsafe
				{
					Function.Call(Hash.GET_PLAYER_PARACHUTE_SMOKE_TRAIL_COLOR, Handle, &r, &g, &b);
				}
				return Color.FromArgb(r, g, b);
			}
			set => Function.Call(Hash.SET_PLAYER_PARACHUTE_SMOKE_TRAIL_COLOR, Handle, value.R, value.G, value.B);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is dead.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Player"/> is dead; otherwise, <see langword="false" />.
		/// </value>
		public bool IsDead => Function.Call<bool>(Hash.IS_PLAYER_DEAD, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is alive.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Player"/> is alive; otherwise, <see langword="false" />.
		/// </value>
		public bool IsAlive => !IsDead;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is aiming.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Player"/> is aiming; otherwise, <see langword="false" />.
		/// </value>
		public bool IsAiming => Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is climbing.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is climbing; otherwise, <see langword="false" />.
		/// </value>
		public bool IsClimbing => Function.Call<bool>(Hash.IS_PLAYER_CLIMBING, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is riding a train.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is riding a train; otherwise, <see langword="false" />.
		/// </value>
		public bool IsRidingTrain => Function.Call<bool>(Hash.IS_PLAYER_RIDING_TRAIN, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is pressing a horn.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is pressing a horn; otherwise, <see langword="false" />.
		/// </value>
		public bool IsPressingHorn => Function.Call<bool>(Hash.IS_PLAYER_PRESSING_HORN, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is playing.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is playing; otherwise, <see langword="false" />.
		/// </value>
		public bool IsPlaying => Function.Call<bool>(Hash.IS_PLAYER_PLAYING, Handle);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Player"/> is invincible.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is invincible; otherwise, <see langword="false" />.
		/// </value>
		public bool IsInvincible
		{
			get => Function.Call<bool>(Hash.GET_PLAYER_INVINCIBLE, Handle);
			set => Function.Call(Hash.SET_PLAYER_INVINCIBLE, Handle, value);
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Player"/> is ignored by the police.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Player"/> is ignored by the police; otherwise, <see langword="false" />.
		/// </value>
		public bool IgnoredByPolice
		{
			set => Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Handle, value);
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Player"/> is ignored by everyone.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Player"/> is ignored by everyone; otherwise, <see langword="false" />.
		/// </value>
		public bool IgnoredByEveryone
		{
			set => Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Handle, value);
		}

		/// <summary>
		/// Sets a value indicating whether cops will be dispatched for this <see cref="Player"/>
		/// </summary>
		/// <value>
		///   <see langword="true" /> if cops will be dispatched; otherwise, <see langword="false" />.
		/// </value>
		public bool DispatchsCops
		{
			set => Function.Call(Hash.SET_DISPATCH_COPS_FOR_PLAYER, Handle, value);
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Player"/> can use cover.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> can use cover; otherwise, <see langword="false" />.
		/// </value>
		public bool CanUseCover
		{
			set => Function.Call(Hash.SET_PLAYER_CAN_USE_COVER, Handle, value);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> can start a mission.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> can start a mission; otherwise, <see langword="false" />.
		/// </value>
		public bool CanStartMission
		{
			get => Function.Call<bool>(Hash.CAN_PLAYER_START_MISSION, Handle);
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Player"/> can control ragdoll.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> can control ragdoll; otherwise, <see langword="false" />.
		/// </value>
		public bool CanControlRagdoll
		{
			set => Function.Call(Hash.GIVE_PLAYER_RAGDOLL_CONTROL, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Player"/> can control its <see cref="Ped"/>.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> can control its <see cref="Ped"/>; otherwise, <see langword="false" />.
		/// </value>
		public bool CanControlCharacter
		{
			get => Function.Call<bool>(Hash.IS_PLAYER_CONTROL_ON, Handle);
			set => Function.Call(Hash.SET_PLAYER_CONTROL, Handle, value, 0);
		}

		/// <summary>
		/// Attempts to change the <see cref="Model"/> of this <see cref="Player"/>.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> to change this <see cref="Player"/> to.</param>
		/// <returns><see langword="true" /> if the change was successful; otherwise, <see langword="false" />.</returns>
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

		/// <summary>
		/// Gets how long this <see cref="Player"/> can remain sprinting for.
		/// </summary>
		public float RemainingSprintTime => Function.Call<float>(Hash.GET_PLAYER_SPRINT_TIME_REMAINING, Handle);

		/// <summary>
		/// Gets how much sprint stamina this <see cref="Player"/> currently has.
		/// </summary>
		public float RemainingSprintStamina => Function.Call<float>(Hash.GET_PLAYER_SPRINT_STAMINA_REMAINING, Handle);

		/// <summary>
		/// Gets how long this <see cref="Player"/> can stay underwater before they start losing health.
		/// </summary>
		public float RemainingUnderwaterTime => Function.Call<float>(Hash.GET_PLAYER_UNDERWATER_TIME_REMAINING, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is using their special ability.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is using their special ability; otherwise, <see langword="false" />.
		/// </value>
		public bool IsSpecialAbilityActive => Function.Call<bool>(Hash.IS_SPECIAL_ABILITY_ACTIVE, Handle);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Player"/> can use their special ability.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this  <see cref="Player"/> can use their special ability; otherwise, <see langword="false" />.
		/// </value>
		public bool IsSpecialAbilityEnabled
		{
			get => Function.Call<bool>(Hash.IS_SPECIAL_ABILITY_ENABLED, Handle);
			set => Function.Call(Hash.ENABLE_SPECIAL_ABILITY, Handle, value);
		}

		/// <summary>
		/// Charges the special ability for this <see cref="Player"/>.
		/// </summary>
		/// <param name="absoluteAmount">The absolute amount.</param>
		public void ChargeSpecialAbility(int absoluteAmount)
		{
			Function.Call(Hash.SPECIAL_ABILITY_CHARGE_ABSOLUTE, Handle, absoluteAmount, true);
		}

		/// <summary>
		/// Charges the special ability for this <see cref="Player"/>.
		/// </summary>
		/// <param name="normalizedRatio">The amount between <c>0.0f</c> and <c>1.0f</c></param>
		public void ChargeSpecialAbility(float normalizedRatio)
		{
			Function.Call(Hash.SPECIAL_ABILITY_CHARGE_NORMALIZED, Handle, normalizedRatio, true);
		}

		/// <summary>
		/// Refills the special ability for this <see cref="Player"/>.
		/// </summary>
		public void RefillSpecialAbility()
		{
			Function.Call(Hash.SPECIAL_ABILITY_FILL_METER, Handle, 1);
		}

		/// <summary>
		/// Depletes the special ability for this <see cref="Player"/>.
		/// </summary>
		public void DepleteSpecialAbility()
		{
			Function.Call(Hash.SPECIAL_ABILITY_DEPLETE_METER, Handle, 1);
		}

		/// <summary>
		/// Gets the last <see cref="Vehicle"/> this <see cref="Player"/> used.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if the last vehicle doesn't exist.</remarks>
		public Vehicle LastVehicle => Function.Call<Vehicle>(Hash.GET_PLAYERS_LAST_VEHICLE);

		/// <summary>
		/// Determines whether this <see cref="Player"/> is targeting the specified <see cref="Entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to check.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Player"/> is targeting the specified <see cref="Entity"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsTargeting(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Handle, entity.Handle);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is targeting anything.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is targeting anything; otherwise, <see langword="false" />.
		/// </value>
		public bool IsTargetingAnything => Function.Call<bool>(Hash.IS_PLAYER_TARGETTING_ANYTHING, Handle);

		/// <summary>
		/// Gets the <see cref="Entity"/> this <see cref="Player"/> is free aiming.
		/// </summary>
		/// <returns>The <see cref="Entity"/> if this <see cref="Player"/> is free aiming any <see cref="Entity"/>; otherwise, <see langword="null" /></returns>
		public Entity TargetedEntity
		{
			get
			{
				int entityHandle;
				unsafe
				{
					if (Function.Call<bool>(Hash.GET_ENTITY_PLAYER_IS_FREE_AIMING_AT, Handle, &entityHandle))
					{
						return Entity.FromHandle(entityHandle);
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Gets the <see cref="Entity"/> this <see cref="Player"/> is locking on when they are aiming with a firearm using a controller or they are locking on unarmed or with a melee weapon.
		/// </summary>
		/// <returns>The <see cref="Entity"/> if this <see cref="Player"/> is automatically locking on any <see cref="Entity"/>; otherwise, <see langword="null" /></returns>
		public Entity LockedOnEntity
		{
			get
			{
				int entityHandle;
				unsafe
				{
					if (Function.Call<bool>(Hash.GET_PLAYER_TARGET_ENTITY, Handle, &entityHandle))
					{
						return Entity.FromHandle(entityHandle);
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Sets a value indicating whether the player is forced to aim.
		/// </summary>
		/// <value>
		///   <see langword="true" /> to make the player always be aiming; otherwise, <see langword="false" />.
		/// </value>
		public bool ForcedAim
		{
			set => Function.Call(Hash.SET_PLAYER_FORCED_AIM, Handle, value);
		}

		/// <summary>
		/// Prevents this <see cref="Player"/> firing this frame.
		/// </summary>
		public void DisableFiringThisFrame()
		{
			Function.Call(Hash.DISABLE_PLAYER_FIRING, Handle, 0);
		}

		/// <summary>
		/// Sets the run speed multiplier for this <see cref="Player"/> this frame.
		/// </summary>
		/// <param name="mult">The factor - min: <c>0.0f</c>, default: <c>1.0f</c>, max: <c>1.499f</c>.</param>
		public void SetRunSpeedMultThisFrame(float mult)
		{
			if (mult > 1.499f)
			{
				mult = 1.499f;
			}

			Function.Call(Hash.SET_RUN_SPRINT_MULTIPLIER_FOR_PLAYER, Handle, mult);
		}

		/// <summary>
		/// Sets the swim speed multiplier for this <see cref="Player"/> this frame.
		/// </summary>
		/// <param name="mult">The factor - min: <c>0.0f</c>, default: <c>1.0f</c>, max: <c>1.499f</c>.</param>
		public void SetSwimSpeedMultThisFrame(float mult)
		{
			if (mult > 1.499f)
			{
				mult = 1.499f;
			}

			Function.Call(Hash.SET_SWIM_MULTIPLIER_FOR_PLAYER, Handle, mult);
		}

		/// <summary>
		/// Makes this <see cref="Player"/> shoot fire bullets this frame.
		/// </summary>
		public void SetFireAmmoThisFrame()
		{
			Function.Call(Hash.SET_FIRE_AMMO_THIS_FRAME, Handle);
		}

		/// <summary>
		/// Makes this <see cref="Player"/> shoot explosive bullets this frame.
		/// </summary>
		public void SetExplosiveAmmoThisFrame()
		{
			Function.Call(Hash.SET_EXPLOSIVE_AMMO_THIS_FRAME, Handle);
		}

		/// <summary>
		/// Makes this <see cref="Player"/> have an explosive melee attack this frame.
		/// </summary>
		public void SetExplosiveMeleeThisFrame()
		{
			Function.Call(Hash.SET_EXPLOSIVE_MELEE_THIS_FRAME, Handle);
		}

		/// <summary>
		/// Lets this <see cref="Player"/> jump really high this frame.
		/// </summary>
		public void SetSuperJumpThisFrame()
		{
			Function.Call(Hash.SET_SUPER_JUMP_THIS_FRAME, Handle);
		}

		/// <summary>
		/// Blocks this <see cref="Player"/> from entering any <see cref="Vehicle"/> this frame.
		/// </summary>
		public void SetMayNotEnterAnyVehicleThisFrame()
		{
			Function.Call(Hash.SET_PLAYER_MAY_NOT_ENTER_ANY_VEHICLE, Handle);
		}

		/// <summary>
		/// Only lets this <see cref="Player"/> enter a specific <see cref="Vehicle"/> this frame.
		/// </summary>
		/// <param name="vehicle">The <see cref="Vehicle"/> this <see cref="Player"/> is allowed to enter.</param>
		public void SetMayOnlyEnterThisVehicleThisFrame(Vehicle vehicle)
		{
			Function.Call(Hash.SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE, Handle, vehicle.Handle);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same player as this <see cref="Player"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same player as this <see cref="Player"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Player player)
			{
				return Handle == player.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="Player"/>s refer to the same player.
		/// </summary>
		/// <param name="left">The left <see cref="Player"/>.</param>
		/// <param name="right">The right <see cref="Player"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same player as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Player left, Player right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		/// <summary>
		/// Determines if two <see cref="Player"/>s don't refer to the same player.
		/// </summary>
		/// <param name="left">The left <see cref="Player"/>.</param>
		/// <param name="right">The right <see cref="Player"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same player as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(Player left, Player right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts a <see cref="Player"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(Player value)
		{
			return new InputArgument((ulong)value.Handle);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
