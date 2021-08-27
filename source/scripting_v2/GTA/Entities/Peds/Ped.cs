//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using GTA.NaturalMotion;
using System;

namespace GTA
{
	public sealed class Ped : Entity
	{
		#region Fields
		Tasks _tasks;
		NaturalMotion.Euphoria _euphoria;
		WeaponCollection _weapons;

		internal static readonly string[] _speechModifierNames = {
			"SPEECH_PARAMS_STANDARD",
			"SPEECH_PARAMS_ALLOW_REPEAT",
			"SPEECH_PARAMS_BEAT",
			"SPEECH_PARAMS_FORCE",
			"SPEECH_PARAMS_FORCE_FRONTEND",
			"SPEECH_PARAMS_FORCE_NO_REPEAT_FRONTEND",
			"SPEECH_PARAMS_FORCE_NORMAL",
			"SPEECH_PARAMS_FORCE_NORMAL_CLEAR",
			"SPEECH_PARAMS_FORCE_NORMAL_CRITICAL",
			"SPEECH_PARAMS_FORCE_SHOUTED",
			"SPEECH_PARAMS_FORCE_SHOUTED_CLEAR",
			"SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL",
			"SPEECH_PARAMS_FORCE_PRELOAD_ONLY",
			"SPEECH_PARAMS_MEGAPHONE",
			"SPEECH_PARAMS_HELI",
			"SPEECH_PARAMS_FORCE_MEGAPHONE",
			"SPEECH_PARAMS_FORCE_HELI",
			"SPEECH_PARAMS_INTERRUPT",
			"SPEECH_PARAMS_INTERRUPT_SHOUTED",
			"SPEECH_PARAMS_INTERRUPT_SHOUTED_CLEAR",
			"SPEECH_PARAMS_INTERRUPT_SHOUTED_CRITICAL",
			"SPEECH_PARAMS_INTERRUPT_NO_FORCE",
			"SPEECH_PARAMS_INTERRUPT_FRONTEND",
			"SPEECH_PARAMS_INTERRUPT_NO_FORCE_FRONTEND",
			"SPEECH_PARAMS_ADD_BLIP",
			"SPEECH_PARAMS_ADD_BLIP_ALLOW_REPEAT",
			"SPEECH_PARAMS_ADD_BLIP_FORCE",
			"SPEECH_PARAMS_ADD_BLIP_SHOUTED",
			"SPEECH_PARAMS_ADD_BLIP_SHOUTED_FORCE",
			"SPEECH_PARAMS_ADD_BLIP_INTERRUPT",
			"SPEECH_PARAMS_ADD_BLIP_INTERRUPT_FORCE",
			"SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED",
			"SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CLEAR",
			"SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CRITICAL",
			"SPEECH_PARAMS_SHOUTED",
			"SPEECH_PARAMS_SHOUTED_CLEAR",
			"SPEECH_PARAMS_SHOUTED_CRITICAL",
		};
		#endregion

		public Ped(int handle) : base(handle)
		{
		}

		public void Clone()
		{
			Clone(0.0F);
		}
		public void Clone(float heading)
		{
			Function.Call(Hash.CLONE_PED, Handle, heading, false, false);
		}

		public void Kill()
		{
			Health = 0;
		}

		#region Styling

		public bool IsHuman => Function.Call<bool>(Hash.IS_PED_HUMAN, Handle);

		public bool IsCuffed => Function.Call<bool>(Hash.IS_PED_CUFFED, Handle);

		public bool CanWearHelmet
		{
			set => Function.Call(Hash.SET_PED_HELMET, Handle, value);
		}

		public bool IsWearingHelmet => Function.Call<bool>(Hash.IS_PED_WEARING_HELMET, Handle);

		public void ClearBloodDamage()
		{
			Function.Call(Hash.CLEAR_PED_BLOOD_DAMAGE, Handle);
		}

		public void ResetVisibleDamage()
		{
			Function.Call(Hash.RESET_PED_VISIBLE_DAMAGE, Handle);
		}

		public void GiveHelmet(bool canBeRemovedByPed, HelmetType helmetType, int textureIndex)
		{
			Function.Call(Hash.GIVE_PED_HELMET, Handle, !canBeRemovedByPed, (int)helmetType, textureIndex);
		}

		public void RemoveHelmet(bool instantly)
		{
			Function.Call(Hash.REMOVE_PED_HELMET, Handle, instantly);
		}

		public Gender Gender => Function.Call<bool>(Hash.IS_PED_MALE, Handle) ? Gender.Male : Gender.Female;

		public float Sweat
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (value > 100)
				{
					value = 100;
				}

				Function.Call(Hash.SET_PED_SWEAT, Handle, value);
			}
		}

		public float WetnessHeight
		{
			set => Function.Call<float>(Hash.SET_PED_WETNESS_HEIGHT, Handle, value);
		}

		public void RandomizeOutfit()
		{
			Function.Call(Hash.SET_PED_RANDOM_COMPONENT_VARIATION, Handle, false);
		}

		public void SetDefaultClothes()
		{
			Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Handle);
		}

		#endregion

		#region Configuration

		public int Armor
		{
			get => Function.Call<int>(Hash.GET_PED_ARMOUR, Handle);
			set => Function.Call(Hash.SET_PED_ARMOUR, Handle, value);
		}

		public int Money
		{
			get => Function.Call<int>(Hash.GET_PED_MONEY, Handle);
			set => Function.Call(Hash.SET_PED_MONEY, Handle, value);
		}

		public override int MaxHealth
		{
			get => Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Handle);
			set => Function.Call(Hash.SET_PED_MAX_HEALTH, Handle, value);
		}

		public bool IsPlayer => Function.Call<bool>(Hash.IS_PED_A_PLAYER, Handle);

		public bool GetConfigFlag(int flagID)
		{
			return Function.Call<bool>(Hash.GET_PED_CONFIG_FLAG, Handle, flagID, true);
		}

		public void SetConfigFlag(int flagID, bool value)
		{
			Function.Call(Hash.SET_PED_CONFIG_FLAG, Handle, flagID, value);
		}

		public void ResetConfigFlag(int flagID)
		{
			Function.Call(Hash.SET_PED_RESET_FLAG, Handle, flagID, true);
		}

		public int GetBoneIndex(Bone BoneID)
		{
			return Function.Call<int>(Hash.GET_PED_BONE_INDEX, Handle, (int)BoneID);
		}

		public Vector3 GetBoneCoord(Bone BoneID)
		{
			return GetBoneCoord(BoneID, Vector3.Zero);
		}
		public Vector3 GetBoneCoord(Bone BoneID, Vector3 Offset)
		{
			return Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, Handle, (int)BoneID, Offset.X, Offset.Y, Offset.Z);
		}

		#endregion

		#region Tasks

		public bool IsIdle => !IsInjured && !IsRagdoll && !IsInAir && !IsOnFire && !IsDucking && !IsGettingIntoAVehicle && !IsInCombat && !IsInMeleeCombat && !(IsInVehicle() && !IsSittingInVehicle());

		public bool IsProne => Function.Call<bool>(Hash.IS_PED_PRONE, Handle);

		public bool IsGettingUp => Function.Call<bool>(Hash.IS_PED_GETTING_UP, Handle);

		public bool IsDiving => Function.Call<bool>(Hash.IS_PED_DIVING, Handle);

		public bool IsJumping => Function.Call<bool>(Hash.IS_PED_JUMPING, Handle);

		public bool IsFalling => Function.Call<bool>(Hash.IS_PED_FALLING, Handle);

		public bool IsVaulting => Function.Call<bool>(Hash.IS_PED_VAULTING, Handle);

		public bool IsClimbing => Function.Call<bool>(Hash.IS_PED_CLIMBING, Handle);

		public bool IsWalking => Function.Call<bool>(Hash.IS_PED_WALKING, Handle);

		public bool IsRunning => Function.Call<bool>(Hash.IS_PED_RUNNING, Handle);

		public bool IsSprinting => Function.Call<bool>(Hash.IS_PED_SPRINTING, Handle);

		public bool IsStopped => Function.Call<bool>(Hash.IS_PED_STOPPED, Handle);

		public bool IsSwimming => Function.Call<bool>(Hash.IS_PED_SWIMMING, Handle);

		public bool IsSwimmingUnderWater => Function.Call<bool>(Hash.IS_PED_SWIMMING_UNDER_WATER, Handle);

		public bool IsDucking
		{
			get => Function.Call<bool>(Hash.IS_PED_DUCKING, Handle);
			set => Function.Call(Hash.SET_PED_DUCKING, Handle, value);
		}

		public bool IsHeadtracking(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_PED_HEADTRACKING_ENTITY, Handle, entity);
		}

		public bool AlwaysKeepTask
		{
			set => Function.Call(Hash.SET_PED_KEEP_TASK, Handle, value);
		}

		public bool BlockPermanentEvents
		{
			set => Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, Handle, value);
		}

		public Tasks Task => _tasks ?? (_tasks = new Tasks(this));

		public int TaskSequenceProgress => Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, Handle);

		#endregion

		#region Euphoria & Ragdoll

		public bool IsRagdoll => Function.Call<bool>(Hash.IS_PED_RAGDOLL, Handle);

		public bool CanRagdoll
		{
			get => Function.Call<bool>(Hash.CAN_PED_RAGDOLL, Handle);
			set => Function.Call(Hash.SET_PED_CAN_RAGDOLL, Handle, value);
		}

		public Euphoria Euphoria => _euphoria ?? (_euphoria = new Euphoria(this));

		#endregion

		#region Weapon Interaction

		public int Accuracy
		{
			get => Function.Call<int>(Hash.GET_PED_ACCURACY, Handle);
			set => Function.Call(Hash.SET_PED_ACCURACY, Handle, value);
		}

		public int ShootRate
		{
			set => Function.Call(Hash.SET_PED_SHOOT_RATE, Handle, value);
		}

		public FiringPattern FiringPattern
		{
			set => Function.Call(Hash.SET_PED_FIRING_PATTERN, Handle, (int)value);
		}

		public WeaponCollection Weapons => _weapons ?? (_weapons = new WeaponCollection(this));

		public bool CanSwitchWeapons
		{
			set => Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, Handle, value);
		}

		#endregion

		#region Vehicle Interaction

		public bool IsOnBike => Function.Call<bool>(Hash.IS_PED_ON_ANY_BIKE, Handle);

		public bool IsOnFoot => Function.Call<bool>(Hash.IS_PED_ON_FOOT, Handle);

		public bool IsInSub => Function.Call<bool>(Hash.IS_PED_IN_ANY_SUB, Handle);

		public bool IsInTaxi => Function.Call<bool>(Hash.IS_PED_IN_ANY_TAXI, Handle);

		public bool IsInTrain => Function.Call<bool>(Hash.IS_PED_IN_ANY_TRAIN, Handle);

		public bool IsInHeli => Function.Call<bool>(Hash.IS_PED_IN_ANY_HELI, Handle);

		public bool IsInPlane => Function.Call<bool>(Hash.IS_PED_IN_ANY_PLANE, Handle);

		public bool IsInFlyingVehicle => Function.Call<bool>(Hash.IS_PED_IN_FLYING_VEHICLE, Handle);

		public bool IsInBoat => Function.Call<bool>(Hash.IS_PED_IN_ANY_BOAT, Handle);

		public bool IsInPoliceVehicle => Function.Call<bool>(Hash.IS_PED_IN_ANY_POLICE_VEHICLE, Handle);

		public bool IsGettingIntoAVehicle => Function.Call<bool>(Hash.IS_PED_GETTING_INTO_A_VEHICLE, Handle);

		public bool IsJumpingOutOfVehicle => Function.Call<bool>(Hash.IS_PED_JUMPING_OUT_OF_VEHICLE, Handle);

		public bool IsTryingToEnterALockedVehicle => Function.Call<bool>(Hash.IS_PED_TRYING_TO_ENTER_A_LOCKED_VEHICLE, Handle);

		public bool CanBeDraggedOutOfVehicle
		{
			set => Function.Call(Hash.SET_PED_CAN_BE_DRAGGED_OUT, Handle, value);
		}

		public bool CanBeKnockedOffBike
		{
			set => Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, Handle, value);
		}

		public bool CanFlyThroughWindscreen
		{
			get => Function.Call<bool>(Hash.GET_PED_CONFIG_FLAG, Handle, 32, true);
			set => Function.Call(Hash.SET_PED_CONFIG_FLAG, Handle, 32, value);
		}

		public bool IsInVehicle()
		{
			return Function.Call<bool>(Hash.IS_PED_IN_ANY_VEHICLE, Handle, 0);
		}
		public bool IsInVehicle(Vehicle vehicle)
		{
			return Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, Handle, vehicle.Handle, 0);
		}

		public bool IsSittingInVehicle()
		{
			return Function.Call<bool>(Hash.IS_PED_SITTING_IN_ANY_VEHICLE, Handle);
		}
		public bool IsSittingInVehicle(Vehicle vehicle)
		{
			return Function.Call<bool>(Hash.IS_PED_SITTING_IN_VEHICLE, Handle, vehicle.Handle);
		}

		public void SetIntoVehicle(Vehicle vehicle, VehicleSeat seat)
		{
			Function.Call(Hash.SET_PED_INTO_VEHICLE, Handle, vehicle.Handle, (int)seat);
		}

		public Vehicle LastVehicle => Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_IN, Handle, true);

		public Vehicle CurrentVehicle => IsInVehicle() ? Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_IN, Handle, false) : null;

		public Vehicle GetVehicleIsTryingToEnter()
		{
			return Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_TRYING_TO_ENTER, Handle);
		}

		public VehicleSeat SeatIndex
		{
			get
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.SeatIndexOffset == 0)
				{
					return VehicleSeat.None;
				}

				int seatIndex = (sbyte)SHVDN.NativeMemory.ReadByte(address + SHVDN.NativeMemory.SeatIndexOffset);
				return (seatIndex >= 0 && IsInVehicle()) ? (VehicleSeat)(seatIndex - 1) : VehicleSeat.None;
			}
		}

		#endregion

		#region Driving

		public float DrivingSpeed
		{
			set => Function.Call(Hash.SET_DRIVE_TASK_CRUISE_SPEED, Handle, value);
		}

		public float MaxDrivingSpeed
		{
			set => Function.Call(Hash.SET_DRIVE_TASK_MAX_CRUISE_SPEED, Handle, value);
		}

		public DrivingStyle DrivingStyle
		{
			set => Function.Call(Hash.SET_DRIVE_TASK_DRIVING_STYLE, Handle, (int)value);
		}

		#endregion

		#region Jacking

		public bool IsJacking => Function.Call<bool>(Hash.IS_PED_JACKING, Handle);

		public bool IsBeingJacked => Function.Call<bool>(Hash.IS_PED_BEING_JACKED, Handle);

		public bool StaysInVehicleWhenJacked
		{
			set => Function.Call(Hash.SET_PED_STAY_IN_VEHICLE_WHEN_JACKED, Handle, value);
		}

		public Ped GetJacker()
		{
			return Function.Call<Ped>(Hash.GET_PEDS_JACKER, Handle);
		}

		public Ped GetJackTarget()
		{
			return Function.Call<Ped>(Hash.GET_JACK_TARGET, Handle);
		}

		#endregion

		#region Parachuting

		public bool IsInParachuteFreeFall => Function.Call<bool>(Hash.IS_PED_IN_PARACHUTE_FREE_FALL, Handle);

		#endregion

		#region Combat

		public bool IsEnemy
		{
			set => Function.Call(Hash.SET_PED_AS_ENEMY, Handle, value);
		}

		public bool IsPriorityTargetForEnemies
		{
			set => Function.Call(Hash.SET_ENTITY_IS_TARGET_PRIORITY, Handle, value, 0);
		}

		public bool IsFleeing => Function.Call<bool>(Hash.IS_PED_FLEEING, Handle);

		public bool IsInjured => Function.Call<bool>(Hash.IS_PED_INJURED, Handle);

		public bool IsInCombat => Function.Call<bool>(Hash.IS_PED_IN_COMBAT, Handle);

		public bool IsInMeleeCombat => Function.Call<bool>(Hash.IS_PED_IN_MELEE_COMBAT, Handle);

		public bool IsShooting => Function.Call<bool>(Hash.IS_PED_SHOOTING, Handle);

		public bool IsReloading => Function.Call<bool>(Hash.IS_PED_RELOADING, Handle);

		public bool IsDoingDriveBy => Function.Call<bool>(Hash.IS_PED_DOING_DRIVEBY, Handle);

		public bool IsGoingIntoCover => Function.Call<bool>(Hash.IS_PED_GOING_INTO_COVER, Handle);

		public bool IsAimingFromCover => Function.Call<bool>(Hash.IS_PED_AIMING_FROM_COVER, Handle);

		public bool IsBeingStunned => Function.Call<bool>(Hash.IS_PED_BEING_STUNNED, Handle);

		public bool IsBeingStealthKilled => Function.Call<bool>(Hash.IS_PED_BEING_STEALTH_KILLED, Handle);

		public bool IsPerformingStealthKill => Function.Call<bool>(Hash.IS_PED_PERFORMING_STEALTH_KILL, Handle);

		public bool IsInCover()
		{
			return IsInCover(false);
		}
		public bool IsInCover(bool expectUseWeapon)
		{
			return Function.Call<bool>(Hash.IS_PED_IN_COVER, Handle, expectUseWeapon);
		}

		public bool IsInCoverFacingLeft => Function.Call<bool>(Hash.IS_PED_IN_COVER_FACING_LEFT, Handle);

		public bool CanBeTargetted
		{
			set => Function.Call(Hash.SET_PED_CAN_BE_TARGETTED, Handle, value);
		}

		public bool CanBeShotInVehicle
		{
			set => Function.Call(Hash.SET_PED_CAN_BE_SHOT_IN_VEHICLE, Handle, value);
		}

		public bool WasKilledByStealth => Function.Call<bool>(Hash.WAS_PED_KILLED_BY_STEALTH, Handle);

		public bool WasKilledByTakedown => Function.Call<bool>(Hash.WAS_PED_KILLED_BY_TAKEDOWN, Handle);

		public Ped GetMeleeTarget()
		{
			return Function.Call<Ped>(Hash.GET_MELEE_TARGET_FOR_PED, Handle);
		}

		public bool IsInCombatAgainst(Ped target)
		{
			return Function.Call<bool>(Hash.IS_PED_IN_COMBAT, Handle, target);
		}

		public Entity GetKiller()
		{
			return Function.Call<Entity>(Hash._GET_PED_KILLER, Handle);
		}

		#endregion

		#region Damaging

		public bool CanWrithe
		{
			get => !GetConfigFlag(281);
			set => SetConfigFlag(281, !value);
		}

		public bool CanSufferCriticalHits
		{
			get
			{
				var address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.PedSuffersCriticalHitOffset == 0)
				{
					return false;
				}

				return !SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.PedSuffersCriticalHitOffset, 2);
			}
			set => Function.Call(Hash.SET_PED_SUFFERS_CRITICAL_HITS, Handle, value);
		}

		public bool AlwaysDiesOnLowHealth
		{
			set => Function.Call(Hash.SET_PED_DIES_WHEN_INJURED, Handle, value);
		}

		public bool DiesInstantlyInWater
		{
			set => Function.Call(Hash.SET_PED_DIES_INSTANTLY_IN_WATER, Handle, value);
		}

		public bool DrownsInWater
		{
			set => Function.Call(Hash.SET_PED_DIES_IN_WATER, Handle, value);
		}

		public bool DrownsInSinkingVehicle
		{
			set => Function.Call(Hash.SET_PED_DIES_IN_SINKING_VEHICLE, Handle, value);
		}

		public bool DropsWeaponsOnDeath
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.PedDropsWeaponsWhenDeadOffset == 0)
				{
					return false;
				}

				return !SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.PedDropsWeaponsWhenDeadOffset, 14);
			}
			set => Function.Call(Hash.SET_PED_DROPS_WEAPONS_WHEN_DEAD, Handle, value);
		}

		public void ApplyDamage(int damageAmount)
		{
			Function.Call(Hash.APPLY_DAMAGE_TO_PED, Handle, damageAmount, true);
		}

		public Vector3 GetLastWeaponImpactCoords()
		{
			var outCoords = new OutputArgument();
			if (Function.Call<bool>(Hash.GET_PED_LAST_WEAPON_IMPACT_COORD, Handle, outCoords))
				return outCoords.GetResult<Vector3>();
			return Vector3.Zero;
		}

		#endregion

		#region Relationship

		public Relationship GetRelationshipWithPed(Ped ped)
		{
			return (Relationship)Function.Call<int>(Hash.GET_RELATIONSHIP_BETWEEN_PEDS, Handle, ped.Handle);
		}

		public int RelationshipGroup
		{
			get => Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, Handle);
			set => Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, Handle, value);
		}

		#endregion

		#region Group

		public bool IsInGroup => Function.Call<bool>(Hash.IS_PED_IN_GROUP, Handle);

		public void LeaveGroup()
		{
			Function.Call(Hash.REMOVE_PED_FROM_GROUP, Handle);
		}

		public bool NeverLeavesGroup
		{
			set => Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, Handle, value);
		}

		public PedGroup CurrentPedGroup => IsInGroup ? Function.Call<PedGroup>(Hash.GET_PED_GROUP_INDEX, Handle, false) : null;

		#endregion

		#region Speech & Animation

		public bool CanPlayGestures
		{
			set => Function.Call(Hash.SET_PED_CAN_PLAY_GESTURE_ANIMS, Handle, value);
		}

		public string Voice
		{
			set => Function.Call(Hash.SET_AMBIENT_VOICE_NAME, Handle, value);
		}

		public string MovementAnimationSet
		{
			set
			{
				Function.Call(Hash.REQUEST_ANIM_SET, value);
				var endtime = DateTime.UtcNow + new TimeSpan(0, 0, 0, 0, 1000);

				while (!Function.Call<bool>(Hash.HAS_ANIM_SET_LOADED, value))
				{
					Script.Yield();

					if (DateTime.UtcNow >= endtime)
					{
						return;
					}
				}

				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, value, 1.0f);
			}
		}

		#endregion
	}
}
