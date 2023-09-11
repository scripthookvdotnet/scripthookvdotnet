//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
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

		private Tasks _tasks;
		private Euphoria _euphoria;
		private WeaponCollection _weapons;

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
		/// <inheritdoc cref="Clone(float)"/>
		public void Clone()
		{
			Clone(0.0f);
		}
		/// <summary>
		/// Spawn an identical clone of this <see cref="Ped"/>.
		/// </summary>
		/// <param name="heading">The direction the clone should be facing.</param>
		public void Clone(float heading)
		{
			Function.Call(Hash.CLONE_PED, Handle, heading, false, false);
		}

		/// <summary>
		/// Kills this <see cref="Ped"/> immediately.
		/// </summary>
		public void Kill()
		{
			Health = 0;
		}

		#region Styling

		/// <summary>
		/// Gets a value indicating whether this <see cref="Ped"/> is human.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Ped"/> is human; otherwise, <see langword="false" />.
		/// </value>
		public bool IsHuman => Function.Call<bool>(Hash.IS_PED_HUMAN, Handle);

		public bool IsCuffed => Function.Call<bool>(Hash.IS_PED_CUFFED, Handle);

		/// <summary>
		/// Sets a value that indicates whether this <see cref="Ped"/> will use a helmet on their own.
		/// </summary>
		public bool CanWearHelmet
		{
			set => Function.Call(Hash.SET_PED_HELMET, Handle, value);
		}

		/// <summary>
		/// Gets a value that indicates whether this <see cref="Ped"/> is wearing a helmet.
		/// </summary>
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

		/// <summary>
		/// Gets the gender of this <see cref="Ped"/>.
		/// </summary>
		public Gender Gender => Function.Call<bool>(Hash.IS_PED_MALE, Handle) ? Gender.Male : Gender.Female;

		/// <summary>
		/// Gets or sets the how much sweat should be rendered on this <see cref="Ped"/>.
		/// </summary>
		/// <value>
		/// The sweat from 0 to 100, 0 being no sweat, 100 being saturated.
		/// </value>
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

		/// <summary>
		/// Sets how high up on this <see cref="Ped"/>s body water should be visible.
		/// </summary>
		/// <value>
		/// The height ranges from 0.0f to 1.99f, 0.0f being no water visible, 1.99f being covered in water.
		/// </value>
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

		/// <summary>
		/// Gets or sets how much armor this <see cref="Ped"/> is wearing as an <see cref="int"/>.
		/// </summary>
		/// <value>
		/// The armor as an <see cref="int"/>.
		/// </value>
		public int Armor
		{
			get => Function.Call<int>(Hash.GET_PED_ARMOUR, Handle);
			set => Function.Call(Hash.SET_PED_ARMOUR, Handle, value);
		}

		/// <summary>
		/// Gets or sets how much money this <see cref="Ped"/> is carrying.
		/// The max value is 65535.
		/// </summary>
		public int Money
		{
			get => Function.Call<int>(Hash.GET_PED_MONEY, Handle);
			set => Function.Call(Hash.SET_PED_MONEY, Handle, value);
		}

		/// <summary>
		/// Gets or sets the maximum health of this <see cref="Ped"/> as an <see cref="int"/>.
		/// </summary>
		/// <value>
		/// The maximum health as an <see cref="int"/>.
		/// </value>
		/// <remarks>
		/// This property (which is overriding <see cref="Entity.MaxHealth"/>) does not subtract the original value by 100 since v2.5, but subtracted in earlier versions
		/// as this overriding property does not exist in those versions.
		/// </remarks>
		public override int MaxHealth
		{
			get => Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Handle);
			set => Function.Call(Hash.SET_PED_MAX_HEALTH, Handle, value);
		}

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is a player <see cref="Ped"/>, who has a <c>CPlayerInfo</c> pointer value.
		/// Returns <see langword="true"/> only on up to one <see cref="Ped"/>.
		/// </summary>
		public bool IsPlayer => Function.Call<bool>(Hash.IS_PED_A_PLAYER, Handle);

		/// <summary>
		/// Gets the config flag bit on this <see cref="Ped"/>.
		/// </summary>
		public bool GetConfigFlag(int flagID)
		{
			return Function.Call<bool>(Hash.GET_PED_CONFIG_FLAG, Handle, flagID, true);
		}

		/// <summary>
		/// Sets the config flag bit on this <see cref="Ped"/>.
		/// </summary>
		public void SetConfigFlag(int flagID, bool value)
		{
			Function.Call(Hash.SET_PED_CONFIG_FLAG, Handle, flagID, value);
		}

		/// <summary>
		/// Do not use this flag as <c>SET_PED_RESET_FLAG</c> uses different flag IDs from the IDs <see cref="GetConfigFlag(int)"/> and <see cref="SetConfigFlag(int, bool)"/> use.
		/// </summary>
		[Obsolete("Ped.ResetConfigFlag is obsolete since SET_PED_RESET_FLAG uses different flag IDs from the IDs GET_PED_CONFIG_FLAG and SET_PED_CONFIG_FLAG use" +
			"and Ped.ResetConfigFlag always set the flag (2nd argument of SET_PED_RESET_FLAG) to true. Call SET_PED_RESET_FLAG on your own.", true)]
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

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is basically lying on the ground.
		/// </summary>
		public bool IsProne => Function.Call<bool>(Hash.IS_PED_PRONE, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is getting up.
		/// </summary>
		public bool IsGettingUp => Function.Call<bool>(Hash.IS_PED_GETTING_UP, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently diving (includes jump launch/clamber phase).
		/// </summary>
		public bool IsDiving => Function.Call<bool>(Hash.IS_PED_DIVING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently jumping.
		/// </summary>
		public bool IsJumping => Function.Call<bool>(Hash.IS_PED_JUMPING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently falling.
		/// </summary>
		public bool IsFalling => Function.Call<bool>(Hash.IS_PED_FALLING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently climbing or vaulting or doing a drop down.
		/// </summary>
		public bool IsVaulting => Function.Call<bool>(Hash.IS_PED_VAULTING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently climbing.
		/// </summary>
		public bool IsClimbing => Function.Call<bool>(Hash.IS_PED_CLIMBING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently walking.
		/// </summary>
		public bool IsWalking => Function.Call<bool>(Hash.IS_PED_WALKING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently running.
		/// </summary>
		public bool IsRunning => Function.Call<bool>(Hash.IS_PED_RUNNING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently sprinting.
		/// </summary>
		public bool IsSprinting => Function.Call<bool>(Hash.IS_PED_SPRINTING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is stood still or in a stationary vehicle.
		/// </summary>
		public bool IsStopped => Function.Call<bool>(Hash.IS_PED_STOPPED, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is swimming.
		/// </summary>
		public bool IsSwimming => Function.Call<bool>(Hash.IS_PED_SWIMMING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is swimming underwater.
		/// </summary>
		public bool IsSwimmingUnderWater => Function.Call<bool>(Hash.IS_PED_SWIMMING_UNDER_WATER, Handle);

		/// <summary>
		/// Gets or sets whether this <see cref="Ped"/> is ducking (crouching).
		/// </summary>
		/// <remarks>
		/// You need to let <see cref="Ped"/>s duck by setting <c>AllowCrouchedMovement</c> to <c>CB_TRUE</c> (and setting <c>AllowStealthMovement</c> to <c>CB_FALSE</c>) in <c>gameconfig.xml</c>
		/// or changing the values for crouching with a script such as Zolika1351's Trainer before this property can return <see langword="true"/> or setting this property to <see langword="true"/> can actually make a <see cref="Ped"/> crouch.
		/// For clarification, changing stance of the player <see cref="Ped"/> with Stance by jedijosh920 does not make this property return <see langword="true"/> as it only changes strafe clipset.
		/// </remarks>
		public bool IsDucking
		{
			get => Function.Call<bool>(Hash.IS_PED_DUCKING, Handle);
			set => Function.Call(Hash.SET_PED_DUCKING, Handle, value);
		}

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is looking at the target entity.
		/// </summary>
		public bool IsHeadtracking(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_PED_HEADTRACKING_ENTITY, Handle, entity);
		}

		/// <summary>
		/// Sets whether this <see cref="Ped"/> keeps their tasks when they are marked as no longer needed by <see cref="Entity.MarkAsNoLongerNeeded"/>.
		/// </summary>
		/// <value>
		/// <para>
		/// If set to <see langword="false" />, this <see cref="Ped"/>'s task will be immediately cleared and start some ambient tasks
		/// (most likely start wandering) when they are marked as no longer needed.
		/// </para>
		/// <para>
		/// If set to <see langword="true" />, this <see cref="Ped"/> will keep their scripted task.
		/// Once this <see cref="Ped"/> has no script tasks, their task will clear and they'll start some ambient tasks (one-time-only).
		/// </para>
		/// </value>
		public bool AlwaysKeepTask
		{
			set => Function.Call(Hash.SET_PED_KEEP_TASK, Handle, value);
		}

		/// <summary>
		/// Sets whether permanent events are blocked for this <see cref="Ped"/>.
		/// <para>
		/// If set to <see langword="true" />, this <see cref="Ped"/> will no longer react to permanent events and will only do as they're told.
		/// For example, the <see cref="Ped"/> will not flee when get shot at and they will not begin combat even if the decision maker for this <see cref="Ped"/> specifies that seeing a hated ped should.
		/// However, the <see cref="Ped"/> will still respond to temporary events like walking around other peds or vehicles even if this property is set to <see langword="true" />.
		/// </para>
		/// </summary>
		/// <value>
		///   <see langword="true" /> if permanent events are blocked; otherwise, <see langword="false" />.
		/// </value>
		public bool BlockPermanentEvents
		{
			set => Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, Handle, value);
		}

		/// <summary>
		/// Opens a list of <see cref="Tasks"/> that this <see cref="Ped"/> can carry out.
		/// </summary>
		public Tasks Task => _tasks ?? (_tasks = new Tasks(this));

		/// <summary>
		/// Gets the stage of the <see cref="TaskSequence"/> this <see cref="Ped"/> is currently executing.
		/// </summary>
		public int TaskSequenceProgress => Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, Handle);

		#endregion

		#region Euphoria & Ragdoll

		/// <summary>
		/// Gets or sets whether this <see cref="Ped"/> is ragdolling.
		/// </summary>
		/// <remarks>
		/// Will return <see langword="false"/> when the <see cref="Ped"/> is getting up or writhing as a part of a ragdoll task.
		/// </remarks>
		public bool IsRagdoll => Function.Call<bool>(Hash.IS_PED_RAGDOLL, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is running a ragdoll task which manages its ragdoll.
		/// </summary>
		/// <remarks>
		/// Will return <see langword="true"/> when <see cref="IsRagdoll"/> returns <see langword="true"/> or the <see cref="Ped"/> is getting up or writhing as a part of a ragdoll task.
		/// </remarks>
		public bool CanRagdoll
		{
			get => Function.Call<bool>(Hash.CAN_PED_RAGDOLL, Handle);
			set => Function.Call(Hash.SET_PED_CAN_RAGDOLL, Handle, value);
		}

		/// <summary>
		/// Opens a list of <see cref="GTA.NaturalMotion.Euphoria"/> Helpers which can be applied to this <see cref="Ped"/>.
		/// </summary>
		public Euphoria Euphoria => _euphoria ?? (_euphoria = new Euphoria(this));

		#endregion

		#region Weapon Interaction

		/// <summary>
		/// Gets or sets how accurate this <see cref="Ped"/>s shooting ability is.
		/// The higher the value of this property is, the more likely it is that this <see cref="Ped"/> will shoot at exactly where they are aiming at.
		/// </summary>
		/// <value>
		/// The accuracy from 0 to 100, 0 being very inaccurate, which means this <see cref="Ped"/> cannot shoot at exactly where they are aiming at,
		/// 100 being perfectly accurate.
		/// </value>
		public int Accuracy
		{
			get => Function.Call<int>(Hash.GET_PED_ACCURACY, Handle);
			set => Function.Call(Hash.SET_PED_ACCURACY, Handle, value);
		}

		/// <summary>
		/// Sets the rate this <see cref="Ped"/> will shoot at.
		/// </summary>
		/// <value>
		/// The shoot rate from 0.0f to 1000.0f, 100.0f is the default value.
		/// </value>
		public int ShootRate
		{
			set => Function.Call(Hash.SET_PED_SHOOT_RATE, Handle, value);
		}

		/// <summary>
		/// Gets of sets the pattern this <see cref="Ped"/> uses to fire weapons.
		/// </summary>
		public FiringPattern FiringPattern
		{
			set => Function.Call(Hash.SET_PED_FIRING_PATTERN, Handle, (int)value);
		}

		/// <summary>
		/// Gets a collection of all this <see cref="Ped"/>s <see cref="Weapon"/>s.
		/// </summary>
		public WeaponCollection Weapons => _weapons ?? (_weapons = new WeaponCollection(this));

		/// <summary>
		/// Sets if this <see cref="Ped"/> can switch between different weapons.
		/// </summary>
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

		/// <summary>
		/// Indicates whether is in a plane or helicopter.
		/// </summary>
		public bool IsInFlyingVehicle => Function.Call<bool>(Hash.IS_PED_IN_FLYING_VEHICLE, Handle);

		public bool IsInBoat => Function.Call<bool>(Hash.IS_PED_IN_ANY_BOAT, Handle);

		public bool IsInPoliceVehicle => Function.Call<bool>(Hash.IS_PED_IN_ANY_POLICE_VEHICLE, Handle);

		/// <summary>
		/// Indicates whether is getting into a <see cref="Vehicle"/> but not sitting in a <see cref="Vehicle"/>.
		/// </summary>
		public bool IsGettingIntoAVehicle => Function.Call<bool>(Hash.IS_PED_GETTING_INTO_A_VEHICLE, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Ped"/> is jumping out of their vehicle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Ped"/> is jumping out of their vehicle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsJumpingOutOfVehicle => Function.Call<bool>(Hash.IS_PED_JUMPING_OUT_OF_VEHICLE, Handle);

		public bool IsTryingToEnterALockedVehicle => Function.Call<bool>(Hash.IS_PED_TRYING_TO_ENTER_A_LOCKED_VEHICLE, Handle);

		public bool CanBeDraggedOutOfVehicle
		{
			set => Function.Call(Hash.SET_PED_CAN_BE_DRAGGED_OUT, Handle, value);
		}

		/// <summary>
		/// Sets that this <see cref="Ped"/> can be knocked off a <see cref="Vehicle"/> (not limited to a bike despite the property name).
		/// </summary>
		public bool CanBeKnockedOffBike
		{
			set => Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, Handle, !value);
		}

		public bool CanFlyThroughWindscreen
		{
			get => Function.Call<bool>(Hash.GET_PED_CONFIG_FLAG, Handle, 32, true);
			set => Function.Call(Hash.SET_PED_CONFIG_FLAG, Handle, 32, value);
		}

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is sitting in or getting out any <see cref="Vehicle"/>.
		/// </summary>
		public bool IsInVehicle()
		{
			return Function.Call<bool>(Hash.IS_PED_IN_ANY_VEHICLE, Handle, 0);
		}
		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is sitting in or getting out the specified <see cref="Vehicle"/>.
		/// </summary>
		public bool IsInVehicle(Vehicle vehicle)
		{
			return Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, Handle, vehicle.Handle, 0);
		}

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is sitting in any <see cref="Vehicle"/>.
		/// </summary>
		public bool IsSittingInVehicle()
		{
			return Function.Call<bool>(Hash.IS_PED_SITTING_IN_ANY_VEHICLE, Handle);
		}
		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is sitting in the specified <see cref="Vehicle"/>.
		/// </summary>
		public bool IsSittingInVehicle(Vehicle vehicle)
		{
			return Function.Call<bool>(Hash.IS_PED_SITTING_IN_VEHICLE, Handle, vehicle.Handle);
		}

		/// <summary>
		/// Sets this <see cref="Ped"/> into the relevant seat of the specified <see cref="Vehicle"/>.
		/// </summary>
		/// <param name="vehicle">The vehicle this <see cref="Ped"/> will be set into.</param>
		/// <param name="seat">The seat to set.</param>
		public void SetIntoVehicle(Vehicle vehicle, VehicleSeat seat)
		{
			Function.Call(Hash.SET_PED_INTO_VEHICLE, Handle, vehicle.Handle, (int)seat);
		}

		/// <summary>
		/// Gets the last <see cref="Vehicle"/> this <see cref="Ped"/> used.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if the last vehicle doesn't exist.</remarks>
		public Vehicle LastVehicle
		{
			get
			{
				unsafe
				{
					var address = new IntPtr(MemoryAddress);
					if (address == IntPtr.Zero)
					{
						return null;
					}

					// Intentionally always create a vehicle instance to avoid unintended NullReferenceException like in v3.6.0 or earlier versions
					// GET_VEHICLE_PED_IS_IN isn't reliable at getting last vehicle since it returns 0 when the ped is going to a door of some vehicle or opening one.
					// Also, the native returns the vehicle's handle the ped is getting in when ped is getting in it (which is not the last vehicle), though the 2nd parameter name is supposed to be "ConsiderEnteringAsInVehicle" as a leaked header suggests.
					return new Vehicle(SHVDN.NativeMemory.Ped.GetLastVehicleHandle(address));
				}
			}
		}

		/// <summary>
		/// Gets the current <see cref="Vehicle"/> this <see cref="Ped"/> is using.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if this <see cref="Ped"/> isn't in a <see cref="Vehicle"/>.</remarks>
		public Vehicle CurrentVehicle
		{
			get
			{
				unsafe
				{
					// In b2699, GET_VEHICLE_PED_IS_IN always returns the last vehicle without checking the driving flag even when the 2nd argument is set to false.
					var address = new IntPtr(MemoryAddress);
					if (address == IntPtr.Zero)
					{
						return null;
					}

					int vehicleHandle = SHVDN.NativeMemory.Ped.GetVehicleHandlePedIsIn(address);
					return vehicleHandle != 0 ? new Vehicle(vehicleHandle) : null;
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="Vehicle"/> this <see cref="Ped"/> is trying to enter.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if this <see cref="Ped"/> isn't trying to enter a <see cref="Vehicle"/>.</remarks>
		public Vehicle GetVehicleIsTryingToEnter()
		{
			return Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_TRYING_TO_ENTER, Handle);
		}

		/// <summary>
		/// Gets the <see cref="VehicleSeat"/> this <see cref="Ped"/> is in.
		/// </summary>
		/// <value>
		/// The <see cref="VehicleSeat"/> this <see cref="Ped"/> is in if this <see cref="Ped"/> is in a <see cref="Vehicle"/>; otherwise, <see cref="VehicleSeat.None"/>.
		/// </value>
		public VehicleSeat SeatIndex
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.SeatIndexOffset == 0)
				{
					return VehicleSeat.None;
				}

				int seatIndex = (sbyte)SHVDN.NativeMemory.ReadByte(address + SHVDN.NativeMemory.Ped.SeatIndexOffset);
				return (seatIndex >= 0 && IsInVehicle()) ? (VehicleSeat)(seatIndex - 1) : VehicleSeat.None;
			}
		}

		#endregion

		#region Driving

		/// <summary>
		/// <para>
		/// Sets the driving speed this <see cref="Ped"/> drives at.
		/// </para>
		/// <para>
		/// this <see cref="Ped"/> must be on a <see cref="Vehicle"/> as a driver and the drive task running on this <see cref="Ped"/> must be active before setting the value can actually affect.
		/// </para>
		/// </summary>
		/// <remarks>
		/// Despite the interface, this actually changes the cruise speed field on <c>CTaskVehicleMissionBase</c>, which is not for <see cref="Ped"/> but for <see cref="Vehicle"/>.
		/// </remarks>
		public float DrivingSpeed
		{
			set => Function.Call(Hash.SET_DRIVE_TASK_CRUISE_SPEED, Handle, value);
		}

		/// <summary>
		/// <para>
		/// Sets the maximum driving speed this <see cref="Ped"/> can drive at.
		/// </para>
		/// <para>
		/// This <see cref="Ped"/> must be on a <see cref="Vehicle"/> as a driver and the drive task running on this <see cref="Ped"/> must be active before setting the value can actually affect.
		/// </para>
		/// </summary>
		/// <remarks>
		/// Despite the interface, this actually changes the maximum cruise speed field on <c>CTaskVehicleMissionBase</c>, which is not for <see cref="Ped"/> but for <see cref="Vehicle"/>.
		/// </remarks>
		public float MaxDrivingSpeed
		{
			set => Function.Call(Hash.SET_DRIVE_TASK_MAX_CRUISE_SPEED, Handle, value);
		}

		/// <summary>
		/// <para>
		/// Sets the the drive tasks driving style.
		/// </para>
		/// <para>
		/// This <see cref="Ped"/> must be on a <see cref="Vehicle"/> as a driver and the drive task running on this <see cref="Ped"/> must be active before setting the value can actually affect.
		/// </para>
		/// </summary>
		/// <remarks>
		/// Despite the interface, this actually changes the driving flags field on <c>CTaskVehicleMissionBase</c>, which is not for <see cref="Ped"/> but for <see cref="Vehicle"/>.
		/// </remarks>
		public DrivingStyle DrivingStyle
		{
			set => Function.Call(Hash.SET_DRIVE_TASK_DRIVING_STYLE, Handle, (int)value);
		}

		#endregion

		#region Jacking

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is in the process of dragging another <see cref="Ped"/> from a <see cref="Vehicle"/>.
		/// </summary>
		public bool IsJacking => Function.Call<bool>(Hash.IS_PED_JACKING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is being dragged from their <see cref="Vehicle"/> by another <see cref="Ped"/>.
		/// </summary>
		public bool IsBeingJacked => Function.Call<bool>(Hash.IS_PED_BEING_JACKED, Handle);

		/// <summary>
		/// Sets a value indicating whether this <see cref="Ped"/> will stay in the vehicle when the driver gets jacked.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if <see cref="Ped"/> stays in vehicle when jacked; otherwise, <see langword="false" />.
		/// </value>
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

		/// <summary>
		/// <para>
		/// Gets a value indicating whether this <see cref="Ped"/> is injured (<see cref="Entity.Health"/> of the <see cref="Ped"/> is lower than the injury threshold in <c>CPed</c>) or does not exist.
		/// </para>
		/// <para>
		/// Since <see cref="Ped"/>s cannot start any scripted tasks if you try to give injured <see cref="Ped"/>s some of them,
		/// this property should be used to determine if the <see cref="Ped"/> is able to do anything in the game (i.e. run scripted tasks) instead of <see cref="Entity.IsDead"/>.
		/// You can reproduce the case where you give some <see cref="Ped"/> a scripted task but it will not start by modifying the injury threshold in <c>CPed</c> and then giving them a scripted task.
		/// <see cref="Entity.IsDead"/> should be used only if you want to specifically know that the <see cref="Ped"/> is dead.
		/// </para>
		/// <para>
		/// Can be safely called to check if <see cref="Ped"/>s exist and are not injured without calling <see cref="Entity.Exists()"/>.
		/// </para>
		/// </summary>
		/// <value>
		///   <see langword="true" /> this <see cref="Ped"/> is injured or does not exist; otherwise, <see langword="false" />.
		/// </value>
		/// <seealso cref="Entity.IsDead"/>
		/// <seealso cref="Entity.Exists()"/>
		/// <remarks>
		/// Since GTA IV, Rockstar Games use the equivalent native function (<c>IS_PED_INJURED</c> in GTA V) to check if some <see cref="Ped"/> is (almost) dead
		/// instead of the equivalent one (<c>IS_ENTITY_DEAD</c> in GTA V) of <see cref="Entity.IsDead"/> in most cases.
		/// </remarks>
		public bool IsInjured => Function.Call<bool>(Hash.IS_PED_INJURED, Handle);

		public bool IsInCombat => Function.Call<bool>(Hash.IS_PED_IN_COMBAT, Handle);

		/// <summary>
		/// Gets a value that indicates whether this <see cref="Ped"/> is in melee combat (doing a melee task, which
		/// is <c>CTaskMelee</c>).
		/// </summary>
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

		/// <summary>
		/// Sets if this <see cref="Ped"/> can take damage inflicted by regular bullets (not stun gun bullets) while in a <see cref="Vehicle"/>.
		/// </summary>
		public bool CanBeShotInVehicle
		{
			set => Function.Call(Hash.SET_PED_CAN_BE_SHOT_IN_VEHICLE, Handle, value);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Ped"/> was killed by a stealth attack.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Ped"/> was killed by stealth; otherwise, <see langword="false" />.
		/// </value>
		public bool WasKilledByStealth => Function.Call<bool>(Hash.WAS_PED_KILLED_BY_STEALTH, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Ped"/> was killed by a takedown.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Ped"/> was killed by a takedown; otherwise, <see langword="false" />.
		/// </value>
		public bool WasKilledByTakedown => Function.Call<bool>(Hash.WAS_PED_KILLED_BY_TAKEDOWN, Handle);

		public Ped GetMeleeTarget()
		{
			return Function.Call<Ped>(Hash.GET_MELEE_TARGET_FOR_PED, Handle);
		}

		public bool IsInCombatAgainst(Ped target)
		{
			return Function.Call<bool>(Hash.IS_PED_IN_COMBAT, Handle, target);
		}

		/// <summary>
		/// Gets the <see cref="Entity"/> that killed this <see cref="Ped"/>.
		/// </summary>
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

		/// <summary>
		/// Gets or Sets whether this <see cref="Ped"/> can suffer critical damage (which deals 1000 times base damages to non-player characters with default weapon configs) when bullets hit this <see cref="Ped"/>'s head bone or its child bones.
		/// If <see langword="false"/>, they will take base damage of weapons when bullets hit their head bone or its child bones, just like when bullets hit a bone other than their head bone, its child bones, or limb bones.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Ped"/> can suffer critical damage; otherwise, <see langword="false" />.
		/// </value>
		public bool CanSufferCriticalHits
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.SuffersCriticalHitOffset == 0)
				{
					return false;
				}

				return !SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.Ped.SuffersCriticalHitOffset, 2);
			}
			set => Function.Call(Hash.SET_PED_SUFFERS_CRITICAL_HITS, Handle, value);
		}

		/// <summary>
		/// Intended to set whether this <see cref="Ped"/> will die when injured, but practically do nothing meaningful.
		/// </summary>
		public bool AlwaysDiesOnLowHealth
		{
			set => Function.Call(Hash.SET_PED_DIES_WHEN_INJURED, Handle, value);
		}

		/// <summary>
		/// Sets whether this <see cref="Ped"/> will die instantly if they find themselves in a body of water.
		/// </summary>
		/// <remarks>
		/// The complete submersion into water does not guarantee this <see cref="Ped"/> will die if this <see cref="Ped"/> is the player one.
		/// </remarks>
		public bool DiesInstantlyInWater
		{
			set => Function.Call(Hash.SET_PED_DIES_INSTANTLY_IN_WATER, Handle, value);
		}

		/// <summary>
		/// Sets whether this <see cref="Ped"/> can take damage for being deep water.
		/// If this <see cref="Ped"/> is the player one, setting to <see langword="false"/> will enable the player to be deep water without taking <c>WEAPON_DROWNING</c> damage.
		/// </summary>
		public bool DrownsInWater
		{
			set => Function.Call(Hash.SET_PED_DIES_IN_WATER, Handle, value);
		}

		/// <summary>
		/// Sets whether this <see cref="Ped"/> can take <c>WEAPON_DROWNING</c> damage in a sinking vehicle.
		/// If this <see cref="Ped"/> is the player one, setting to <see langword="false"/> will enable the player to be in a sinking vehicle without taking <c>WEAPON_DROWNING</c> damage.
		/// </summary>
		public bool DrownsInSinkingVehicle
		{
			set => Function.Call(Hash.SET_PED_DIES_IN_SINKING_VEHICLE, Handle, value);
		}

		/// <summary>
		/// Sets whether this <see cref="Ped"/> will drop the current weapon when they get killed.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if <see cref="Ped"/> drops the current weapon when killed; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>
		/// <see cref="Ped"/>s will drop only their current weapon when they get killed.
		/// </remarks>
		public bool DropsWeaponsOnDeath
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.DropsWeaponsWhenDeadOffset == 0)
				{
					return false;
				}

				return !SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.Ped.DropsWeaponsWhenDeadOffset, 14);
			}
			set => Function.Call(Hash.SET_PED_DROPS_WEAPONS_WHEN_DEAD, Handle, value);
		}

		public void ApplyDamage(int damageAmount)
		{
			Function.Call(Hash.APPLY_DAMAGE_TO_PED, Handle, damageAmount, true);
		}

		/// <summary>
		/// Gets the last position a weapon of this <see cref="Ped"/> was impacted at this frame.
		/// </summary>
		/// <remarks>
		/// This property should be called every frame as the the last valid result lasts only the frame a weapon of this <see cref="Ped"/>
		/// was impacted at and else it returns <see cref="Vector3.Zero"/>.
		/// </remarks>
		public Vector3 GetLastWeaponImpactCoords()
		{
			var outCoords = new OutputArgument();
			if (Function.Call<bool>(Hash.GET_PED_LAST_WEAPON_IMPACT_COORD, Handle, outCoords))
			{
				return outCoords.GetResult<Vector3>();
			}

			return Vector3.Zero;
		}

		#endregion

		#region Relationship

		/// <summary>
		/// Gets the relationship between this <see cref="Ped"/> and <paramref name="ped"/>.
		/// </summary>
		/// <param name="ped"></param>
		/// <remarks>
		/// This property returns <see cref="Relationship.Pedestrians"/> if the relationship is not set.
		/// </remarks>
		public Relationship GetRelationshipWithPed(Ped ped)
		{
			return (Relationship)Function.Call<int>(Hash.GET_RELATIONSHIP_BETWEEN_PEDS, Handle, ped.Handle);
		}

		/// <summary>
		/// Gets or sets the <see cref="RelationshipGroup"/> this <see cref="Ped"/> belongs to.
		/// </summary>
		public int RelationshipGroup
		{
			get => Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, Handle);
			set => Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, Handle, value);
		}

		#endregion

		#region Group

		/// <summary>
		/// Gets if this <see cref="Ped"/> is in a <see cref="PedGroup"/>.
		/// </summary>
		public bool IsInGroup => Function.Call<bool>(Hash.IS_PED_IN_GROUP, Handle);

		public void LeaveGroup()
		{
			Function.Call(Hash.REMOVE_PED_FROM_GROUP, Handle);
		}

		public bool NeverLeavesGroup
		{
			set => Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, Handle, value);
		}

		/// <summary>
		/// Gets the PedGroup this <see cref="Ped"/> is in.
		/// </summary>
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
				int startTime = Environment.TickCount;

				while (!Function.Call<bool>(Hash.HAS_ANIM_SET_LOADED, value))
				{
					Script.Yield();

					if (Environment.TickCount - startTime >= 1000)
					{
						return;
					}
				}

				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Handle, value, 1.0f);
			}
		}

		#endregion
	}
}
