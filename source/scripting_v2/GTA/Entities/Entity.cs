//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public abstract class Entity : IEquatable<Entity>, IHandleable, ISpatial
	{
		public Entity(int handle)
		{
			Handle = handle;
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
		/// <returns>
		/// Returns a <see cref="Ped"/> if this handle corresponds to a Ped.
		/// Returns a <see cref="Vehicle"/> if this handle corresponds to a Vehicle.
		/// Returns a <see cref="Prop"/> if this handle corresponds to a Prop.
		/// Returns <see langword="null" /> if no <see cref="Entity"/> exists this the specified <paramref name="handle"/>.
		/// </returns>
		internal static Entity FromHandle(int handle)
		{
			IntPtr address = SHVDN.NativeMemory.GetEntityAddress(handle);
			if (address == IntPtr.Zero)
			{
				return null;
			}

			// Read the same field as GET_ENTITY_TYPE does
			var entityType = (EntityTypeInternal)SHVDN.NativeMemory.ReadByte(address + 0x28);

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
		/// The handle of this <see cref="Entity"/>.
		/// </summary>
		public virtual int Handle
		{
			get;
		}

		/// <summary>
		/// Gets the memory address where the <see cref="Entity"/> is stored in memory.
		/// </summary>
		public unsafe int* MemoryAddress => (int*)SHVDN.NativeMemory.GetEntityAddress(Handle).ToPointer();

		/// <summary>
		/// <para>
		/// Gets a value indicating whether this <see cref="Entity"/> is dead or does not exist.
		/// </para>
		/// <para>
		/// For <see cref="Ped"/>s, use <see cref="Ped.IsInjured"/> unless you specifically need to know they are dead
		/// since this property does not guarantee whether if the <see cref="Ped"/> can start scripted tasks.
		/// </para>
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Entity"/> is dead or does not exist; otherwise, <see langword="false" />.
		/// </value>
		/// <seealso cref="Exists()"/>
		/// <seealso cref="Ped.IsInjured"/>
		public bool IsDead => Function.Call<bool>(Hash.IS_ENTITY_DEAD, Handle);
		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> exists and is alive.
		/// </summary>
		/// <para>
		/// For <see cref="Ped"/>s, use <see cref="Ped.IsInjured"/> unless you specifically need to know they are alive at all
		/// since this property does not guarantee whether if the <see cref="Ped"/> can start scripted tasks.
		/// </para>
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
		public int Alpha
		{
			get => Function.Call<int>(Hash.GET_ENTITY_ALPHA, Handle);
			set => Function.Call(Hash.SET_ENTITY_ALPHA, Handle, value, false);
		}

		/// <summary>
		/// Resets the <see cref="Alpha"/>.
		/// </summary>
		public void ResetAlpha()
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
		/// Set<see cref="Ped.AlwaysKeepTask"/> to<see langword= "true"/> before calling this method.
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
		public bool FreezePosition
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 0x2E, 1);
			}
			set => Function.Call(Hash.FREEZE_ENTITY_POSITION, Handle, value);
		}

		public int GetBoneIndex(string boneName)
		{
			return Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, Handle, boneName);
		}

		public bool HasBone(string boneName)
		{
			return GetBoneIndex(boneName) != -1;
		}

		public Vector3 GetBoneCoord(int boneIndex)
		{
			return Function.Call<Vector3>(Hash._GET_ENTITY_BONE_COORDS, Handle, boneIndex);
		}
		public Vector3 GetBoneCoord(string boneName)
		{
			return Function.Call<Vector3>(Hash._GET_ENTITY_BONE_COORDS, Handle, GetBoneIndex(boneName));
		}

		#endregion

		#region Health

		/// <summary>
		/// Gets or sets the health of this <see cref="Entity"/> as an <see cref="int"/>.
		/// </summary>
		/// <value>
		/// The health as an <see cref="int"/>.
		/// </value>
		/// <remarks>
		/// Since v2.10.11 (the version included in the archive of SHVDN v3.0.0), this property does not subtract by 100 from the original value,
		/// which is actually a breaking change. You should aware of this before using this property.
		/// </remarks>
		public int Health
		{
			get => Function.Call<int>(Hash.GET_ENTITY_HEALTH, Handle);
			set => Function.Call(Hash.SET_ENTITY_HEALTH, Handle, value);
		}

		/// <summary>
		/// Gets or sets the maximum health of this <see cref="Entity"/> as an <see cref="int"/>.
		/// </summary>
		/// <value>
		/// The health as an <see cref="int"/>.
		/// </value>
		/// <remarks>
		/// Since v2.10.11 (the version included in the archive of SHVDN v3.0.0), this property does not subtract by 100 from the original value,
		/// which is actually a breaking change. You should aware of this before using this property.
		/// </remarks>
		public virtual int MaxHealth
		{
			get => Function.Call<int>(Hash.GET_ENTITY_MAX_HEALTH, Handle);
			set => Function.Call(Hash.SET_ENTITY_MAX_HEALTH, Handle, value);
		}

		#endregion

		#region Positioning

		/// <summary>
		/// Gets or sets the position of this <see cref="Entity"/>.
		/// </summary>
		/// <value>
		/// The position in world space.
		/// </value>
		/// <remarks>
		/// If the <see cref="Entity"/> is <see cref="Ped"/> and the <see cref="Ped"/> is in a <see cref="Vehicle"/>, the <see cref="Vehicle"/>'s position will be returned or changed.
		/// </remarks>
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
		/// Gets or sets the rotation of this <see cref="Entity"/> in degrees.
		/// </summary>
		/// <value>
		/// The yaw, pitch, roll rotation values in degrees, where roll represents X rotation, pitch represents Y rotation, yaw represents Z rotation.
		/// The rotation order is pitch, roll, then yaw (in that order).
		/// </value>
		public virtual Vector3 Rotation
		{
			get => Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION, Handle, 2);
			set => Function.Call(Hash.SET_ENTITY_ROTATION, Handle, value.X, value.Y, value.Z, 2, 1);
		}

		/// <summary>
		/// Gets or sets the heading of this <see cref="Entity"/> in degrees.
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
		public Vector3 UpVector => Vector3.Cross(RightVector, ForwardVector);

		/// <summary>
		/// Gets the vector that points to the right of this <see cref="Entity"/>.
		/// </summary>
		public Vector3 RightVector
		{
			get
			{
				const double D2R = 0.01745329251994329576923690768489;
				double num1 = System.Math.Cos(Rotation.Y * D2R);
				double x = num1 * System.Math.Cos(-Rotation.Z * D2R);
				double y = num1 * System.Math.Sin(Rotation.Z * D2R);
				double z = System.Math.Sin(-Rotation.Y * D2R);
				return new Vector3((float)x, (float)y, (float)z);
			}
		}

		/// <summary>
		/// Gets the vector that points in front of this <see cref="Entity"/>.
		/// </summary>
		public Vector3 ForwardVector => Function.Call<Vector3>(Hash.GET_ENTITY_FORWARD_VECTOR, Handle);

		/// <summary>
		/// Gets the position in world coordinates of an offset relative this <see cref="Entity"/>.
		/// </summary>
		/// <param name="offset">The offset from this <see cref="Entity"/>.</param>
		public Vector3 GetOffsetInWorldCoords(Vector3 offset)
		{
			return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, Handle, offset.X, offset.Y, offset.Z);
		}

		/// <summary>
		/// Gets the relative offset of this <see cref="Entity"/> from a world coordinates position.
		/// </summary>
		/// <param name="worldCoords">The world coordinates.</param>
		public Vector3 GetOffsetFromWorldCoords(Vector3 worldCoords)
		{
			return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, Handle, worldCoords.X, worldCoords.Y, worldCoords.Z);
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

		#endregion

		#region Damaging


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
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 5);
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.SetBit(address + 392, 5, value);
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
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 7);
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.SetBit(address + 392, 7, value);
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
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 4);
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.SetBit(address + 392, 4, value);
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
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 11);
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.SetBit(address + 392, 11, value);
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
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 6);
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.SetBit(address + 392, 6, value);
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
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
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
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
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
		public bool IsOnScreen
		{
			get => Function.Call<bool>(Hash.IS_ENTITY_ON_SCREEN, Handle);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is upright within 30f degrees.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> is upright; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>
		/// This property always returned <see langword="false"/> in v2.10.10 or earlier APIs.
		/// </remarks>
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
		/// Ssets a value indicating whether this <see cref="Entity"/> has gravity.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> has gravity; otherwise, <see langword="false" />.
		/// </value>
		public bool HasGravity
		{
			set => Function.Call(Hash.SET_ENTITY_HAS_GRAVITY, Handle, value);
		}

		#endregion

		#region Collision

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> has collision.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> has collision; otherwise, <see langword="false" />.
		/// </value>
		public bool HasCollision
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 0x29, 1);
			}
			set => Function.Call(Hash.SET_ENTITY_COLLISION, Handle, value, false);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> has collided with anything.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> has collided; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>You must call <c>SET_ENTITY_RECORDS_COLLISIONS</c> for this to work.</remarks>
		public bool HasCollidedWithAnything => Function.Call<bool>(Hash.HAS_ENTITY_COLLIDED_WITH_ANYTHING, Handle);

		/// <summary>
		/// Sets the collision between this <see cref="Entity"/> and another <see cref="Entity"/>
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to set collision with</param>
		/// <param name="toggle">if set to <see langword="true" /> the 2 <see cref="Entity"/>s wont collide with each other.</param>
		public void SetNoCollision(Entity entity, bool toggle)
		{
			Function.Call(Hash.SET_ENTITY_NO_COLLISION_ENTITY, Handle, entity.Handle, !toggle);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in a specified area,
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
		/// Determines whether this <see cref="Entity"/> is in a specified angled area.
		/// </summary>
		/// <param name="pos1">The mid-point along a base edge of the rectangle.</param>
		/// <param name="pos2">The mid-point of opposite base edge on the other Z.</param>
		/// <param name="angle">The width. Wrongly named parameter but is kept for existing script compatibilities.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is in the specified angled area; otherwise, <see langword="false" />.
		/// </returns>
		[Obsolete("Entity.IsInArea(Vector3, Vector3, float) is obsolete because it actually tests using an angled area. " +
		          "Call IS_ENTITY_IN_AREA manually to test with an axis aligned area in this API version instead. " +
		          "Use Entity.IsInAngledArea(Vector3, Vector3, float) to test with an angled area instead.")]
		public bool IsInArea(Vector3 pos1, Vector3 pos2, float angle)
		{
			return IsInAngledArea(pos1, pos2, angle);
		}
		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in a specified angled area.
		/// </summary>
		/// <param name="origin">The mid-point along a base edge of the rectangle.</param>
		/// <param name="edge">The mid-point of opposite base edge on the other Z.</param>
		/// <param name="angle">The width. Wrongly named parameter but is kept for existing script compatibilities.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is in the specified angled area; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsInAngledArea(Vector3 origin, Vector3 edge, float angle)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_IN_ANGLED_AREA, Handle, origin.X, origin.Y, origin.Z, edge.X, edge.Y, edge.Z, angle, false, true, false);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in range of a specified position
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="distance">The maximum range.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is in range of the <paramref name="position"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsInRangeOf(Vector3 position, float distance)
		{
			return Vector3.Subtract(Position, position).Length() < distance;
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is near a specified <see cref="Entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to check.</param>
		/// <param name="distance">The max displacement from the <paramref name="entity"/>.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is near the <paramref name="entity"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsNearEntity(Entity entity, Vector3 distance)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_AT_ENTITY, Handle, entity.Handle, distance.X, distance.Y, distance.Z, 0, 1, 0);
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
			return Function.Call<Blip>(Hash.ADD_BLIP_FOR_ENTITY, Handle);
		}

		/// <summary>
		/// Gets the <see cref="Blip"/> attached to this <see cref="Entity"/>.
		/// </summary>
		/// <remarks>Returns <see langword="null" /> if no <see cref="Blip"/>s are attached to this <see cref="Entity"/></remarks>
		public Blip CurrentBlip => Function.Call<Blip>(Hash.GET_BLIP_FROM_ENTITY, Handle);

		#endregion

		#region Attaching

		/// <summary>
		/// Detaches this <see cref="Entity"/> from any <see cref="Entity"/> it may be attached to.
		/// </summary>
		public void Detach()
		{
			Function.Call(Hash.DETACH_ENTITY, Handle, 1, 1);
		}
		/// <summary>
		/// Attaches this <see cref="Entity"/> to a different <see cref="Entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to attach this <see cref="Entity"/> to.</param>
		/// <param name="boneIndex">The bone index to attach. -1 means this <see cref="Entity"/> will attach to the position of the target <see cref="Entity"/>.</param>
		public void AttachTo(Entity entity, int boneIndex)
		{
			AttachTo(entity, boneIndex, Vector3.Zero, Vector3.Zero);
		}
		/// <summary>
		/// Attaches this <see cref="Entity"/> to a different <see cref="Entity"/> bone.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to attach this <see cref="Entity"/> to.</param>
		/// <param name="boneIndex">The bone index to attach. -1 means this <see cref="Entity"/> will attach to the position of the target <see cref="Entity"/>.</param>
		/// <param name="position">The position relative to the entity bone to attach this <see cref="Entity"/> to.</param>
		/// <param name="rotation">The rotation to apply to this <see cref="Entity"/> relative to the entity bone.</param>
		public void AttachTo(Entity entity, int boneIndex, Vector3 position, Vector3 rotation)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Handle, entity.Handle, boneIndex, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 0, 0, 0, 0, 2, 1);
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
		/// <remarks>Returns <see langword="null" /> if this <see cref="Entity"/> isn't attached to any entity.</remarks>
		/// </summary>
		public Entity GetEntityAttachedTo()
		{
			return Function.Call<Entity>(Hash.GET_ENTITY_ATTACHED_TO, Handle);
		}

		#endregion

		#region Forces

		public void ApplyForce(Vector3 direction)
		{
			ApplyForce(direction, Vector3.Zero, ForceType.MaxForceRot2);
		}
		public void ApplyForce(Vector3 direction, Vector3 rotation)
		{
			ApplyForce(direction, rotation, ForceType.MaxForceRot2);
		}
		public void ApplyForce(Vector3 direction, Vector3 rotation, ForceType forceType)
		{
			Function.Call(Hash.APPLY_FORCE_TO_ENTITY, Handle, (int)forceType, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, false, true, true, false, true);
		}
		public void ApplyForceRelative(Vector3 direction)
		{
			ApplyForceRelative(direction, Vector3.Zero, ForceType.MaxForceRot2);
		}
		public void ApplyForceRelative(Vector3 direction, Vector3 rotation)
		{
			ApplyForceRelative(direction, Rotation, ForceType.MaxForceRot2);
		}
		public void ApplyForceRelative(Vector3 direction, Vector3 rotation, ForceType forceType)
		{
			Function.Call(Hash.APPLY_FORCE_TO_ENTITY, Handle, (int)forceType, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, true, true, true, false, true);
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
		}

		/// <summary>
		/// <para>
		/// Destroys this <see cref="Entity"/>.
		/// If this <see cref="Entity"/> is <see cref="Vehicle"/>, the occupants will not be deleted but their tasks will be cleared.
		/// </para>
		/// </summary>
		public void Delete()
		{
			int handle = Handle;
			Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, handle, false, true);
			unsafe
			{
				Function.Call(Hash.DELETE_ENTITY, &handle);
			}
		}

		/// <summary>
		/// Determines if this <see cref="Entity"/> exists.
		/// You should ensure <see cref="Entity"/>s still exist before manipulating them or getting some values for them on every tick, since some native functions may crash the game if invalid entity handles are passed.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="Entity"/> exists; otherwise, <see langword="false" /></returns>.
		/// <seealso cref="IsDead"/>
		public bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_ENTITY_EXIST, Handle);
		}
		/// <summary>
		/// Determines if this <see cref="Entity"/> exists.
		/// You should ensure <see cref="Entity"/>s still exist before manipulating them or getting some values for them on every tick, since some native functions may crash the game if invalid entity handles are passed.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="Entity"/> exists; otherwise, <see langword="false" /></returns>.
		/// <seealso cref="IsDead"/>
		public static bool Exists(Entity entity)
		{
			return entity != null && entity.Exists();
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same entity as this <see cref="Entity"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same entity as this <see cref="Entity"/>; otherwise, <see langword="false" />.</returns>
		public bool Equals(Entity obj)
		{
			return !(obj is null) && Handle == obj.Handle;
		}
		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same entity as this <see cref="Entity"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same entity as this <see cref="Entity"/>; otherwise, <see langword="false" />.</returns>

		public override bool Equals(object obj)
		{
			return obj is not null && obj.GetType() == GetType() && Equals((Entity)obj);
		}

		/// <summary>
		/// Determines if two <see cref="Entity"/>s refer to the same entity.
		/// </summary>
		/// <param name="left">The left <see cref="Entity"/>.</param>
		/// <param name="right">The right <see cref="Entity"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same entity as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Entity left, Entity right)
		{
			return left?.Equals(right) ?? right is null;
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

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
