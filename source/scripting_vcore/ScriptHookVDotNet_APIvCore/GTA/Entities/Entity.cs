//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Linq;

namespace GTA
{
	public abstract class Entity : PoolObject, ISpatial
	{
		#region Fields
		EntityBoneCollection _bones;
		EntityDamageRecordCollection _damageRecords;
		#endregion

		internal Entity(int handle) : base(handle)
		{
		}

		private enum EntityTypeInternal
		{
			Building = 1,
			Vehicle = 3,
			Ped = 4,
			Object = 5
		}

		/// <summary>
		/// Creates a new instance of an <see cref="Entity"/> from the given handle.
		/// </summary>
		/// <param name="handle">The entity handle.</param>
		/// <returns>Returns a <see cref="Ped"/> if this handle corresponds to a Ped.
		/// Returns a <see cref="Vehicle"/> if this handle corresponds to a Vehicle.
		/// Returns a <see cref="Prop"/> if this handle corresponds to a Prop.
		/// Returns <see langword="null" /> if no <see cref="Entity"/> exists this the specified <paramref name="handle"/></returns>
		public static Entity FromHandle(int handle)
		{
			var memoryAddress = SHVDN.NativeMemory.GetEntityAddress(handle);
			if (memoryAddress == IntPtr.Zero)
				return null;

			// Read the same field as GET_ENTITY_TYPE does
			var entityType = (EntityTypeInternal)SHVDN.NativeMemory.ReadByte(memoryAddress + 0x28);

			switch (entityType)
			{
				case EntityTypeInternal.Ped:
					return new Ped(handle);
				case EntityTypeInternal.Vehicle:
					return new Vehicle(handle);
				case EntityTypeInternal.Object:
					return new Prop(handle);
			}
			return null;
		}

		/// <summary>
		/// Gets the memory address where the <see cref="Entity"/> is stored in memory.
		/// </summary>
		public IntPtr MemoryAddress => SHVDN.NativeMemory.GetEntityAddress(Handle);

		/// <summary>
		/// Gets the type of the current <see cref="Entity"/>.
		/// </summary>
		public EntityType EntityType
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return EntityType.Invalid;
				}

				// Read the same field as GET_ENTITY_TYPE does
				var entityType = (EntityTypeInternal)SHVDN.NativeMemory.ReadByte(address + 0x28);

				switch (entityType)
				{
					case EntityTypeInternal.Ped:
						return EntityType.Ped;
					case EntityTypeInternal.Vehicle:
						return EntityType.Vehicle;
					case EntityTypeInternal.Object:
						return EntityType.Prop;
				}

				return EntityType.Invalid;
			}
		}

		/// <summary>
		/// Gets or sets the population type of the current <see cref="Entity"/>.
		/// This property can also be used to add or remove <see cref="Entity"/> persistence.
		/// </summary>
		public EntityPopulationType PopulationType
		{
			get => (EntityPopulationType)Function.Call<int>(Hash.GET_ENTITY_POPULATION_TYPE, Handle);
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteByte(address + 0xDA, (byte)((int)value & 0xF));
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is dead or does not exist.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Entity"/> is dead or does not exist; otherwise, <see langword="false" />.
		/// </value>
		/// <seealso cref="Exists"/>
		/// <seealso cref="Ped.IsInjured"/>
		public bool IsDead => Function.Call<bool>(Hash.IS_ENTITY_DEAD, Handle);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> exists and is alive.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Entity"/> exists and is alive; otherwise, <see langword="false" />.
		/// </value>
		public bool IsAlive => !IsDead;

		#region Styling

		/// <summary>
		/// Gets the model of the current <see cref="Entity"/>.
		/// </summary>
		public Model Model => new Model(Function.Call<int>(Hash.GET_ENTITY_MODEL, Handle));

		/// <summary>
		/// Gets or sets how opaque this <see cref="Entity"/> is.
		/// </summary>
		/// <value>
		/// 0 for completely see through, 255 for fully opaque
		/// </value>
		public int Opacity
		{
			get => Function.Call<int>(Hash.GET_ENTITY_ALPHA, Handle);
			set => Function.Call(Hash.SET_ENTITY_ALPHA, Handle, value, false);
		}

		/// <summary>
		/// Resets the <seealso cref="Opacity"/>.
		/// </summary>
		public void ResetOpacity()
		{
			Function.Call(Hash.RESET_ENTITY_ALPHA, Handle);
		}

		#endregion

		#region Configuration

		/// <summary>
		/// Gets or sets the level of detail distance of this <see cref="Entity"/>.
		/// </summary>
		public int LodDistance
		{
			get => Function.Call<int>(Hash.GET_ENTITY_LOD_DIST, Handle);
			set => Function.Call(Hash.SET_ENTITY_LOD_DIST, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is persistent.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is persistent; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>
		/// If this <see cref="Entity"/> is <see cref="Ped"/>, setting to <see langword="true" /> can clear ambient tasks and setting to <see langword="false" /> will clear all tasks immediately.
		/// Use <see cref="Ped.SetIsPersistentNoClearTask(bool)"/> instead if you need to keep assigned tasks.
		/// </remarks>
		public bool IsPersistent
		{
			get => Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, Handle);
			set
			{
				if (value)
				{
					Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, Handle, true, true);
				}
				else
				{
					MarkAsNoLongerNeeded();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is frozen.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is position frozen; otherwise, <see langword="false" />.
		/// </value>
		public bool IsPositionFrozen
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 0x2E, 1);
			}
			set => Function.Call(Hash.FREEZE_ENTITY_POSITION, Handle, value);
		}

		/// <summary>
		/// Gets a collection of the <see cref="EntityBone"/>s in this <see cref="Entity"/>.
		/// </summary>
		public virtual EntityBoneCollection Bones => _bones ?? (_bones = new EntityBoneCollection(this));

		#endregion

		#region Health

		/// <summary>
		/// Gets or sets the health of this <see cref="Entity"/> as an <see cref="int"/>.
		/// <para>Use <see cref="HealthFloat"/> instead if you need to get or set the value precisely, since a health value of a <see cref="Entity"/> are stored as a <see cref="float"/>.</para>
		/// </summary>
		/// <value>
		/// The health as an <see cref="int"/>.
		/// </value>
		/// <seealso cref="HealthFloat"/>
		public int Health
		{
			get => Function.Call<int>(Hash.GET_ENTITY_HEALTH, Handle);
			set => Function.Call(Hash.SET_ENTITY_HEALTH, Handle, value);
		}
		/// <summary>
		/// Gets or sets the maximum health of this <see cref="Entity"/> as an <see cref="int"/>.
		/// <para>Use <see cref="MaxHealthFloat"/> instead if you need to get or set the value precisely, since a max health value of a <see cref="Entity"/> are stored as a <see cref="float"/>.</para>
		/// </summary>
		/// <value>
		/// The maximum health as a <see cref="int"/>.
		/// </value>
		public virtual int MaxHealth
		{
			get => Function.Call<int>(Hash.GET_ENTITY_MAX_HEALTH, Handle);
			set => Function.Call(Hash.SET_ENTITY_MAX_HEALTH, Handle, value);
		}

		/// <summary>
		/// Gets or sets the health of this <see cref="Entity"/> as a <see cref="float"/>.
		/// </summary>
		/// <value>
		/// The health as a <see cref="float"/>.
		/// </value>
		public float HealthFloat
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + 640);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + 640, value);
			}
		}
		/// <summary>
		/// Gets or sets the maximum health of this <see cref="Entity"/> as a <see cref="float"/>.
		/// </summary>
		/// <value>
		/// The maximum health as a <see cref="float"/>.
		/// </value>
		public float MaxHealthFloat
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.EntityMaxHealthOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.EntityMaxHealthOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.EntityMaxHealthOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.EntityMaxHealthOffset, value);
			}
		}

		#endregion

		#region Positioning

		/// <summary>
		/// Gets this <see cref="Entity"/>s matrix which stores position and rotation information.
		/// </summary>
		public Matrix Matrix
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return new Matrix();
				}

				return new Matrix(SHVDN.NativeMemory.ReadMatrix(address + 96));
			}
		}

		/// <summary>
		/// Gets or sets the position of this <see cref="Entity"/>.
		/// If the <see cref="Entity"/> is <see cref="Ped"/> and the <see cref="Ped"/> is in a <see cref="Vehicle"/>, the <see cref="Vehicle"/>'s position will be returned or changed.
		/// </summary>
		/// <value>
		/// The position in world space.
		/// </value>
		public virtual Vector3 Position
		{
			get => Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, Handle, 0);
			set => Function.Call(Hash.SET_ENTITY_COORDS, Handle, value.X, value.Y, value.Z, 0, 0, 0, 1);
		}

		/// <summary>
		/// Sets the position of this <see cref="Entity"/> without any offset.
		/// </summary>
		/// <value>
		/// The position in world space.
		/// </value>
		public Vector3 PositionNoOffset
		{
			set => Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, Handle, value.X, value.Y, value.Z, 1, 1, 1);
		}

		/// <summary>
		/// Gets or sets the rotation of this <see cref="Entity"/>.
		/// </summary>
		/// <value>
		/// The yaw, pitch, roll rotation values.
		/// </value>
		public virtual Vector3 Rotation
		{
			get => Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION, Handle, 2);
			set => Function.Call(Hash.SET_ENTITY_ROTATION, Handle, value.X, value.Y, value.Z, 2, 1);
		}

		/// <summary>
		/// Gets or sets the heading of this <see cref="Entity"/>.
		/// </summary>
		/// <value>
		/// The heading in degrees.
		/// </value>
		public float Heading
		{
			get => Function.Call<float>(Hash.GET_ENTITY_HEADING, Handle);
			set => Function.Call<float>(Hash.SET_ENTITY_HEADING, Handle, value);
		}

		/// <summary>
		/// Gets a value indicating how submersed this <see cref="Entity"/> is, 1.0f means the whole entity is submerged.
		/// </summary>
		public float SubmersionLevel => Function.Call<float>(Hash.GET_ENTITY_SUBMERGED_LEVEL, Handle);

		/// <summary>
		/// Gets how high above ground this <see cref="Entity"/> is.
		/// </summary>
		public float HeightAboveGround => Function.Call<float>(Hash.GET_ENTITY_HEIGHT_ABOVE_GROUND, Handle);

		/// <summary>
		/// Gets or sets the quaternion of this <see cref="Entity"/>.
		/// </summary>
		public Quaternion Quaternion
		{
			get
			{
				float x;
				float y;
				float z;
				float w;
				unsafe
				{
					Function.Call(Hash.GET_ENTITY_QUATERNION, Handle, &x, &y, &z, &w);
				}

				return new Quaternion(x, y, z, w);
			}
			set => Function.Call(Hash.SET_ENTITY_QUATERNION, Handle, value.X, value.Y, value.Z, value.W);
		}

		/// <summary>
		/// Gets the vector that points above this <see cref="Entity"/>.
		/// </summary>
		public Vector3 UpVector
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.RelativeTop;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x80));
			}
		}

		/// <summary>
		/// Gets the vector that points to the right of this <see cref="Entity"/>.
		/// </summary>
		public Vector3 RightVector
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.RelativeRight;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x60));
			}
		}

		/// <summary>
		/// Gets the vector that points in front of this <see cref="Entity"/>.
		/// </summary>
		public Vector3 ForwardVector
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.RelativeFront;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x70));
			}
		}

		/// <summary>
		/// Gets a position directly to the left of this <see cref="Entity"/>.
		/// </summary>
		public Vector3 LeftPosition
		{
			get
			{
				var (rearBottomLeft, _) = Model.Dimensions;
				return GetOffsetPosition(new Vector3(rearBottomLeft.X, 0, 0));
			}
		}

		/// <summary>
		/// Gets a position directly to the right of this <see cref="Entity"/>.
		/// </summary>
		public Vector3 RightPosition
		{
			get
			{
				var (_, frontTopRight) = Model.Dimensions;
				return GetOffsetPosition(new Vector3(frontTopRight.X, 0, 0));
			}
		}

		/// <summary>
		/// Gets a position directly behind this <see cref="Entity"/>.
		/// </summary>
		public Vector3 RearPosition
		{
			get
			{
				var (rearBottomLeft, _) = Model.Dimensions;
				return GetOffsetPosition(new Vector3(0, rearBottomLeft.Y, 0));
			}
		}

		/// <summary>
		/// Gets a position directly in front of this <see cref="Entity"/>.
		/// </summary>
		public Vector3 FrontPosition
		{
			get
			{
				var (_, frontTopRight) = Model.Dimensions;
				return GetOffsetPosition(new Vector3(0, frontTopRight.Y, 0));
			}
		}

		/// <summary>
		/// Gets a position directly above this <see cref="Entity"/>.
		/// </summary>
		public Vector3 AbovePosition
		{
			get
			{
				var (_, frontTopRight) = Model.Dimensions;
				return GetOffsetPosition(new Vector3(0, 0, frontTopRight.Z));
			}
		}

		/// <summary>
		/// Gets a position directly below this <see cref="Entity"/>.
		/// </summary>
		public Vector3 BelowPosition
		{
			get
			{
				var (rearBottomLeft, _) = Model.Dimensions;
				return GetOffsetPosition(new Vector3(0, 0, rearBottomLeft.Z));
			}
		}

		/// <summary>
		/// Gets the position in world coordinates of an offset relative this <see cref="Entity"/>.
		/// </summary>
		/// <param name="offset">The offset from this <see cref="Entity"/>.</param>
		public Vector3 GetOffsetPosition(Vector3 offset)
		{
			return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, Handle, offset.X, offset.Y, offset.Z);
		}

		/// <summary>
		/// Gets the relative offset of this <see cref="Entity"/> from a world coordinates position.
		/// </summary>
		/// <param name="worldCoords">The world coordinates.</param>
		public Vector3 GetPositionOffset(Vector3 worldCoords)
		{
			return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, Handle, worldCoords.X, worldCoords.Y, worldCoords.Z);
		}

		/// <summary>
		/// Gets or sets this <see cref="Entity"/>s speed.
		/// </summary>
		/// <value>
		/// The speed in m/s.
		/// </value>
		public float Speed
		{
			get => Function.Call<float>(Hash.GET_ENTITY_SPEED, Handle);
			set => Velocity = Velocity.Normalized * value;
		}

		/// <summary>
		/// Sets the maximum speed this <see cref="Entity"/> can move at.
		/// </summary>
		public float MaxSpeed
		{
			set => Function.Call(Hash.SET_ENTITY_MAX_SPEED, Handle, value);
		}

		/// <summary>
		/// Gets or sets the velocity of this <see cref="Entity"/>.
		/// </summary>
		public Vector3 Velocity
		{
			get => Function.Call<Vector3>(Hash.GET_ENTITY_VELOCITY, Handle);
			set => Function.Call(Hash.SET_ENTITY_VELOCITY, Handle, value.X, value.Y, value.Z);
		}

		/// <summary>
		/// Gets or sets the rotation velocity of this <see cref="Entity"/> in local space.
		/// </summary>
		public Vector3 RotationVelocity
		{
			get => Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION_VELOCITY, Handle);
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				var angularVelocityInLocalAxes = Quaternion * value;
				SHVDN.NativeMemory.SetEntityAngularVelocity(address, angularVelocityInLocalAxes.X, angularVelocityInLocalAxes.Y, angularVelocityInLocalAxes.Z);
			}
		}

		/// <summary>
		/// Gets or sets the rotation velocity of this <see cref="Entity"/> in world space.
		/// </summary>
		public Vector3 WorldRotationVelocity
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				unsafe
				{
					var returnVectorPtr = SHVDN.NativeMemory.GetEntityAngularVelocity(address);
					return new Vector3(returnVectorPtr[0], returnVectorPtr[1], returnVectorPtr[2]);
				}
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.SetEntityAngularVelocity(address, value.X, value.Y, value.Z);
			}
		}

		#endregion

		#region Damaging

		/// <summary>
		/// Gets a collection of the <see cref="EntityDamageRecord"/>s in this <see cref="Entity"/>.
		/// </summary>
		public EntityDamageRecordCollection DamageRecords => _damageRecords ?? (_damageRecords = new EntityDamageRecordCollection(this));

		/// <summary>
		/// Determines whether this <see cref="Entity"/> has been damaged by a specified <see cref="Entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to check</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> has been damaged by the specified <see cref="Entity"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool HasBeenDamagedBy(Entity entity)
		{
			return Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY, Handle, entity.Handle, 1);
		}
		/// <summary>
		/// Determines whether this <see cref="Entity"/> has been damaged by a specific weapon].
		/// </summary>
		/// <param name="weapon">The weapon to check.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> has been damaged by the specified weapon; otherwise, <see langword="false" />.
		/// </returns>
		public virtual bool HasBeenDamagedBy(WeaponHash weapon)
		{
			return Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_WEAPON, Handle, weapon, 0);
		}
		/// <summary>
		/// Determines whether this <see cref="Entity"/> has been damaged by any weapon.
		/// </summary>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> has been damaged by any weapon; otherwise, <see langword="false" />.
		/// </returns>
		public virtual bool HasBeenDamagedByAnyWeapon()
		{
			return Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_WEAPON, Handle, 0, 2);
		}
		/// <summary>
		/// Determines whether this <see cref="Entity"/> has been damaged by any melee weapon.
		/// </summary>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> has been damaged by any melee weapon; otherwise, <see langword="false" />.
		/// </returns>
		public virtual bool HasBeenDamagedByAnyMeleeWeapon()
		{
			return Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_WEAPON, Handle, 0, 1);
		}

		/// <summary>
		/// Clears the last weapon damage this <see cref="Entity"/> received.
		/// </summary>
		public virtual void ClearLastWeaponDamage()
		{
			Function.Call(Hash.CLEAR_ENTITY_LAST_WEAPON_DAMAGE, Handle);
		}

		#endregion

		#region Fragment Object

		/// <summary>
		/// Returns the number of fragment group of this <see cref="Entity"/>.
		/// </summary>
		public int FragmentGroupCount
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				return SHVDN.NativeMemory.GetFragmentGroupCountFromEntity(address);
			}
		}

		/// <summary>
		/// Determines if this <see cref="Entity"/> is a fragment object.
		/// </summary>
		/// <returns>
		/// <see langword="true" /> if this <see cref="Entity"/> is a fragment object; otherwise, <see langword="false" />.
		/// This will return <see langword="true" /> if this <see cref="Entity"/> is a <see cref="Ped"/> or a <see cref="Vehicle"/>.
		/// </returns>
		public bool IsFragmentObject
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsEntityFragmentObject(address);
			}
		}

		/// <summary>
		/// Detachs a fragment part of this <see cref="Entity"/>. Can create a new <see cref="Entity"/>.
		/// </summary>
		/// <returns>
		///   <para><see langword="true" /> if a new <see cref="Entity"/> is created; otherwise, <see langword="false" />.</para>
		///   <para>Returning <see langword="false" /> does not necessarily mean detaching the part did not change the <see cref="Entity"/> in any ways.
		///   For example, detaching <c>seat_f</c> for <see cref="Vehicle"/> will return <see langword="false" /> but the <see cref="Ped"/> on the front seat will not be able to sit properly.</para>
		/// </returns>
		public bool DetachFragmentPart(int fragmentGroupIndex)
		{
			var address = MemoryAddress;
			if (address == IntPtr.Zero)
			{
				return false;
			}

			return SHVDN.NativeMemory.DetachFragmentPartByIndex(address, fragmentGroupIndex);
		}

		#endregion

		#region Invincibility

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is fire proof.
		/// This <see cref="Entity"/> does not catch fire naturally and <see cref="Ped"/>s do not getting ragdolled for being burned when this property is set to <see langword="true" />.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is fire proof; otherwise, <see langword="false" />.
		/// </value>
		public bool IsFireProof
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 5);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				if (value)
				{
					SHVDN.NativeMemory.SetBit(address + 392, 5);
				}
				else
				{
					SHVDN.NativeMemory.ClearBit(address + 392, 5);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is melee proof.
		/// <see cref="Ped"/>s are not susceptible to the reactions of melee attacks when this property is set to <see langword="true" />.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is melee proof; otherwise, <see langword="false" />.
		/// </value>
		public bool IsMeleeProof
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 7);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				if (value)
				{
					SHVDN.NativeMemory.SetBit(address + 392, 7);
				}
				else
				{
					SHVDN.NativeMemory.ClearBit(address + 392, 7);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is bullet proof.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is bullet proof; otherwise, <see langword="false" />.
		/// </value>
		public bool IsBulletProof
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 4);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				if (value)
				{
					SHVDN.NativeMemory.SetBit(address + 392, 4);
				}
				else
				{
					SHVDN.NativeMemory.ClearBit(address + 392, 4);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is explosion proof.
		/// Explosions cannot add force to this <see cref="Entity"/> and <see cref="Ped"/>s do not getting ragdolled with explosions when this property is set to <see langword="true" />.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is explosion proof; otherwise, <see langword="false" />.
		/// </value>
		public bool IsExplosionProof
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 11);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				if (value)
				{
					SHVDN.NativeMemory.SetBit(address + 392, 11);
				}
				else
				{
					SHVDN.NativeMemory.ClearBit(address + 392, 11);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is collision proof.
		/// Setting this property to <see langword="true" /> only does not prevent this <see cref="Entity"/> from getting ragdolled when another <see cref="Entity"/> collide with this <see cref="Entity"/>.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is collision proof; otherwise, <see langword="false" />.
		/// </value>
		public bool IsCollisionProof
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 6);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				if (value)
				{
					SHVDN.NativeMemory.SetBit(address + 392, 6);
				}
				else
				{
					SHVDN.NativeMemory.ClearBit(address + 392, 6);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is water cannon proof.
		/// <see cref="Ped"/>s does not get ragdolled by the water jet from fire hydrants when this property is set to <see langword="true" />.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is water cannon proof; otherwise, <see langword="false" />.
		/// </value>
		public bool IsWaterCannonProof
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 12);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				if (value)
				{
					SHVDN.NativeMemory.SetBit(address + 392, 12);
				}
				else
				{
					SHVDN.NativeMemory.ClearBit(address + 392, 12);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is steam proof.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is steam proof; otherwise, <see langword="false" />.
		/// </value>
		public bool IsSteamProof
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 15);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				if (value)
				{
					SHVDN.NativeMemory.SetBit(address + 392, 15);
				}
				else
				{
					SHVDN.NativeMemory.ClearBit(address + 392, 15);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is smoke proof.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is smoke proof; otherwise, <see langword="false" />.
		/// </value>
		public bool IsSmokeProof
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 16);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				if (value)
				{
					SHVDN.NativeMemory.SetBit(address + 392, 16);
				}
				else
				{
					SHVDN.NativeMemory.ClearBit(address + 392, 16);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is invincible.
		/// Setting this property to <see langword="true" /> does not prevent <see cref="Ped"/>s from doing the reactions for getting hit with melee attacks.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is invincible; otherwise, <see langword="false" />.
		/// </value>
		public bool IsInvincible
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 8);
			}
			set => Function.Call(Hash.SET_ENTITY_INVINCIBLE, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> can only be damaged by <see cref="Player"/>s.
		/// <see cref="Ped"/>s are not susceptible to the reactions of melee attacks when this property is set to <see langword="true" />, unlike <see cref="IsInvincible"/>.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> can only be damaged by <see cref="Player"/>s; otherwise, <see langword="false" />.
		/// </value>
		public bool IsOnlyDamagedByPlayer
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 10);
			}
			set => Function.Call(Hash.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER, Handle, value);
		}

		#endregion

		#region Status Effects

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is visible.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is visible; otherwise, <see langword="false" />.
		/// </value>
		public bool IsVisible
		{
			get => Function.Call<bool>(Hash.IS_ENTITY_VISIBLE, Handle);
			set => Function.Call(Hash.SET_ENTITY_VISIBLE, Handle, value);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is occluded.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is occluded; otherwise, <see langword="false" />.
		/// </value>
		public bool IsOccluded
		{
			get => Function.Call<bool>(Hash.IS_ENTITY_OCCLUDED, Handle);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is rendered.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is rendered; otherwise, <see langword="false" />.
		/// </value>
		public bool IsRendered
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 176, 4);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is on fire.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is on fire; otherwise, <see langword="false" />.
		/// </value>
		public bool IsOnFire => Function.Call<bool>(Hash.IS_ENTITY_ON_FIRE, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is on screen.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is on screen; otherwise, <see langword="false" />.
		/// </value>
		public bool IsOnScreen => Function.Call<bool>(Hash.IS_ENTITY_ON_SCREEN, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is upright.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is upright; otherwise, <see langword="false" />.
		/// </value>
		public bool IsUpright => Function.Call<bool>(Hash.IS_ENTITY_UPRIGHT, Handle, 30.0f);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is upside down.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is upside down; otherwise, <see langword="false" />.
		/// </value>
		public bool IsUpsideDown => Function.Call<bool>(Hash.IS_ENTITY_UPSIDEDOWN, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is in the air.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Entity"/> is in the air; otherwise, <see langword="false" />.
		/// </value>
		public bool IsInAir => Function.Call<bool>(Hash.IS_ENTITY_IN_AIR, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is in water.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is in water; otherwise, <see langword="false" />.
		/// </value>
		public bool IsInWater => Function.Call<bool>(Hash.IS_ENTITY_IN_WATER, Handle);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> has gravity.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> has gravity; otherwise, <see langword="false" />.
		/// </value>
		public bool HasGravity
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return true;
				}
				address = SHVDN.NativeMemory.ReadAddress(address + 48);
				if (address == IntPtr.Zero)
				{
					return true;
				}
				return !SHVDN.NativeMemory.IsBitSet(address + 26, 4);

			}
			set => Function.Call(Hash.SET_ENTITY_HAS_GRAVITY, Handle, value);
		}

		/// <summary>
		/// Stops all particle effects attached to this <see cref="Entity"/>.
		/// </summary>
		public void RemoveParticleEffects()
		{
			Function.Call(Hash.REMOVE_PARTICLE_FX_FROM_ENTITY, Handle);
		}

		#endregion

		#region Collision

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> has collided with anything.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> has collided; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks><see cref="IsRecordingCollisions"/> must be <see langword="true" /> for this to work.</remarks>
		public bool HasCollided => Function.Call<bool>(Hash.HAS_ENTITY_COLLIDED_WITH_ANYTHING, Handle);

		/// <summary>
		/// Gets the material this <see cref="Entity"/> is pushing up against.
		/// </summary>
		/// <value>
		/// A material hash if this <see cref = "Entity"/> has collision; otherwise, <see cref = "MaterialHash.None"/>.
		/// </value>
		/// <remarks>
		/// <para>This returns <see cref = "MaterialHash.None"/> in some cases, although this enrity is internally considered touched with something.
		/// For example, this returns <see cref = "MaterialHash.None"/> when this <see cref = "Entity"/> is a <see cref = "Ped"/> and this <see cref = "Entity"/> doesn't push none of the touching entities, including buildings.
		/// However, this returns <see cref = "MaterialHash.None"/> when this <see cref = "Entity"/> touches any ragdolled peds.</para>
		/// <para>Note that when this <see cref = "Entity"/> is a this <see cref = "Vehicle"/> and only its wheels touches something, the game will consider the entity touching nothing and this returns <see cref = "MaterialHash.None"/>.</para>
		/// </remarks>
		public MaterialHash MaterialCollidingWith => (MaterialHash)Function.Call<uint>(Hash.GET_LAST_MATERIAL_HIT_BY_ENTITY, Handle);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> has collision.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> has collision; otherwise, <see langword="false" />.
		/// </value>
		public bool IsCollisionEnabled
		{
			get => !Function.Call<bool>(Hash.GET_ENTITY_COLLISION_DISABLED, Handle);
			set => Function.Call(Hash.SET_ENTITY_COLLISION, Handle, value, false);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is recording collisions.
		/// </summary>
		public bool IsRecordingCollisions
		{
			set => Function.Call(Hash.SET_ENTITY_RECORDS_COLLISIONS, Handle, value);
		}

		/// <summary>
		/// Sets the collision between this <see cref="Entity"/> and another <see cref="Entity"/>
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to set collision with</param>
		/// <param name="toggle">if set to <see langword="true" /> the 2 <see cref="Entity"/>s wont collide with each other.</param>
		public void SetNoCollision(Entity entity, bool toggle)
		{
			Function.Call(Hash.SET_ENTITY_NO_COLLISION_ENTITY, Handle, entity.Handle, toggle);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in a specified area
		/// </summary>
		/// <param name="minBounds">The minimum bounds.</param>
		/// <param name="maxBounds">The maximum bounds.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is in the specified area; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsInArea(Vector3 minBounds, Vector3 maxBounds)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_IN_AREA, Handle, minBounds.X, minBounds.Y, minBounds.Z, maxBounds.X, maxBounds.Y, maxBounds.Z);
		}
		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in a specified angled area
		/// </summary>
		/// <param name="origin">The mid-point along a base edge of the rectangle.</param>
		/// <param name="edge">The mid-point of opposite base edge on the other Z.</param>
		/// <param name="angle">The width. Wrongly named parameter but is kept for existing script compatibilities.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is in the specified angled area; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsInAngledArea(Vector3 origin, Vector3 edge, float angle) => IsInAngledArea(origin, edge, angle, true);
		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in a specified angled area.
		/// An angled area is an X-Z oriented rectangle with three parameters: origin, extent, and width.
		/// </summary>
		/// <param name="originEdge">The mid-point along a base edge of the rectangle.</param>
		/// <param name="extentEdge">The mid-point of opposite base edge on the other Z.</param>
		/// <param name="width">The length of the base edge.</param>
		/// <param name="includeZAxis">
		/// If set to <see langword="true" />, the method will also check if the point is in area in Z axis as well as X and Y axes.
		/// If set to <see langword="false" />, the method will only check if the point is in area in X and Y axes.
		/// </param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is in the specified angled area; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsInAngledArea(Vector3 originEdge, Vector3 extentEdge, float width, bool includeZAxis)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_IN_ANGLED_AREA, Handle, originEdge.X, originEdge.Y, originEdge.Z, extentEdge.X, extentEdge.Y, extentEdge.Z, width, false, includeZAxis, 0);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in range of a specified position
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="range">The maximum range.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is in range of the <paramref name="position"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsInRange(Vector3 position, float range)
		{
			return Vector3.Subtract(Position, position).LengthSquared() < range * range;
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is near a specified <see cref="Entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to check.</param>
		/// <param name="bounds">The max displacement from the <paramref name="entity"/>.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is near the <paramref name="entity"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsNearEntity(Entity entity, Vector3 bounds)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_AT_ENTITY, Handle, entity.Handle, bounds.X, bounds.Y, bounds.Z, false, true, false);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is touching an <see cref="Entity"/> with the <see cref="Model"/> <paramref name="model"/>.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> to check</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is touching a <paramref name="model"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsTouching(Model model)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_TOUCHING_MODEL, Handle, model.Hash);
		}
		/// <summary>
		/// Determines whether this <see cref="Entity"/> is touching the <see cref="Entity"/> <paramref name="entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to check.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is touching <paramref name="entity"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsTouching(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_TOUCHING_ENTITY, Handle, entity.Handle);
		}

		#endregion

		#region Blips

		/// <summary>
		/// Creates a <see cref="Blip"/> on this <see cref="Entity"/>.
		/// </summary>
		public Blip AddBlip()
		{
			return new Blip(Function.Call<int>(Hash.ADD_BLIP_FOR_ENTITY, Handle));
		}

		/// <summary>
		/// Gets the <see cref="Blip"/> attached to this <see cref="Entity"/>.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if no <see cref="Blip"/>s are attached to this <see cref="Entity"/></remarks>
		public Blip AttachedBlip
		{
			get
			{
				int handle = Function.Call<int>(Hash.GET_BLIP_FROM_ENTITY, Handle);

				if (Function.Call<bool>(Hash.DOES_BLIP_EXIST, handle))
				{
					return new Blip(handle);
				}

				return null;
			}
		}

		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Blip"/>s attached to this <see cref="Entity"/>.
		/// </summary>
		public Blip[] AttachedBlips
		{
			get => World.GetAllBlips().Where(x => Function.Call<int>(Hash.GET_BLIP_INFO_ID_ENTITY_INDEX, x.NativeValue) == Handle).ToArray();
		}

		#endregion

		#region Attaching

		/// <summary>
		/// Detaches this <see cref="Entity"/> from any <see cref="Entity"/> it may be attached to.
		/// </summary>
		public void Detach()
		{
			Function.Call(Hash.DETACH_ENTITY, Handle, true, true);
		}
		/// <summary>
		/// Attaches this <see cref="Entity"/> to a different <see cref="Entity"/>
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to attach this <see cref="Entity"/> to.</param>
		/// <param name="position">The position relative to the <paramref name="entity"/> to attach this <see cref="Entity"/> to.</param>
		/// <param name="rotation">The rotation to apply to this <see cref="Entity"/> relative to the <paramref name="entity"/></param>
		public void AttachTo(Entity entity, Vector3 position = default, Vector3 rotation = default)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Handle, entity.Handle, -1, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 0, 0, 0, 0, 2, 1);
		}
		/// <summary>
		/// Attaches this <see cref="Entity"/> to a different <see cref="Entity"/>
		/// </summary>
		/// <param name="entityBone">The <see cref="EntityBone"/> to attach this <see cref="Entity"/> to.</param>
		/// <param name="position">The position relative to the <paramref name="entityBone"/> to attach this <see cref="Entity"/> to.</param>
		/// <param name="rotation">The rotation to apply to this <see cref="Entity"/> relative to the <paramref name="entityBone"/></param>
		public void AttachTo(EntityBone entityBone, Vector3 position = default, Vector3 rotation = default)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Handle, entityBone.Owner.Handle, entityBone, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 0, 0, 0, 0, 2, 1);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is attached to any other <see cref="Entity"/>.
		/// </summary>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is attached to another <see cref="Entity"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsAttached()
		{
			return Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, Handle);
		}
		/// <summary>
		/// Determines whether this <see cref="Entity"/> is attached to the specified <see cref="Entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to check if this <see cref="Entity"/> is attached to.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is attached to <paramref name="entity"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsAttachedTo(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_ATTACHED_TO_ENTITY, Handle, entity.Handle);
		}

		/// <summary>
		/// Gets the <see cref="Entity"/> this <see cref="Entity"/> is attached to.
		/// <remarks>returns <see langword="null" /> if this <see cref="Entity"/> isnt attached to any entity</remarks>
		/// </summary>
		public Entity AttachedEntity
		{
			get => FromHandle(Function.Call<int>(Hash.GET_ENTITY_ATTACHED_TO, Handle));
		}

		#endregion

		#region Forces

		/// <summary>
		/// Applies a force to this <see cref="Entity"/>.
		/// </summary>
		/// <param name="direction">The direction to apply the force relative to world coordinates.</param>
		/// <param name="rotation">The offset from the root bone of this <see cref="Entity"/> where the force applies. "rotation" is incorrectly named parameter but is left for scripts that use the method with named parameters.</param>
		/// <param name="forceType">Type of the force to apply.</param>
		public void ApplyForce(Vector3 direction, Vector3 rotation = default, ForceType forceType = ForceType.MaxForceRot2)
		{
			Function.Call(Hash.APPLY_FORCE_TO_ENTITY, Handle, forceType, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, false, true, true, false, true);
		}
		/// <summary>
		/// Applies a force to this <see cref="Entity"/>.
		/// </summary>
		/// <param name="direction">The direction to apply the force relative to this <see cref="Entity"/>s rotation</param>
		/// <param name="rotation">The offset from the root bone of this <see cref="Entity"/> where the force applies. "rotation" is incorrectly named parameter but is left for scripts that use the method with named parameters.</param>
		/// <param name="forceType">Type of the force to apply.</param>
		public void ApplyForceRelative(Vector3 direction, Vector3 rotation = default, ForceType forceType = ForceType.MaxForceRot2)
		{
			Function.Call(Hash.APPLY_FORCE_TO_ENTITY, Handle, forceType, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, true, true, true, false, true);
		}

		#endregion

		/// <summary>
		/// Marks this <see cref="Entity"/> as no longer needed to keep and lets the game delete it when its too far away.
		/// You can still manipulate this <see cref="Entity"/> as long as the <see cref="Entity"/> exists.
		/// </summary>
		public void MarkAsNoLongerNeeded()
		{
			int handle = Handle;
			unsafe
			{
				Function.Call(Hash.SET_ENTITY_AS_NO_LONGER_NEEDED, &handle);
			}
			// Do not set "Handle" property to the value of "handle" again, since engine will have set it to zero, but we still may want to use it otherwise
		}

		/// <summary>
		/// <para>Destroys this <see cref="Entity"/> and sets <see cref="PoolObject.Handle"/> to 0.</para>
		/// <para>
		/// If you need to remove this <see cref="Entity"/> from collections that use <see cref="object.Equals(object)"/> for equality comparison (e.g. <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>),
		/// remove this <see cref="Entity"/> element from these collections before calling this method.
		/// </para>
		/// </summary>
		public override void Delete()
		{
			int handle = Handle;
			Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, handle, false, true);
			unsafe
			{
				Function.Call(Hash.DELETE_ENTITY, &handle);
			}
			Handle = handle; // This will be zero now
		}

		/// <summary>
		/// Determines if this <see cref="Entity"/> exists.
		/// You should ensure <see cref="Entity"/>s still exist before manipulating them or getting some values for them on every tick, since some native functions may crash the game if invalid entity handles are passed.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="Entity"/> exists; otherwise, <see langword="false" /></returns>.
		/// <seealso cref="IsDead"/>
		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_ENTITY_EXIST, Handle);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same entity as this <see cref="Entity"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same entity as this <see cref="Entity"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Entity entity)
			{
				return Handle == entity.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="Entity"/>s refer to the same entity.
		/// </summary>
		/// <param name="left">The left <see cref="Entity"/>.</param>
		/// <param name="right">The right <see cref="Entity"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same entity as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Entity left, Entity right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		/// <summary>
		/// Determines if two <see cref="Entity"/>s don't refer to the same entity.
		/// </summary>
		/// <param name="left">The left <see cref="Entity"/>.</param>
		/// <param name="right">The right <see cref="Entity"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same entity as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(Entity left, Entity right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts an <see cref="Entity"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(Entity value)
		{
			return new InputArgument((ulong)value.Handle);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
