//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
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
		/// <returns>
		/// Returns a <see cref="Ped"/> if this handle corresponds to a Ped.
		/// Returns a <see cref="Vehicle"/> if this handle corresponds to a Vehicle.
		/// Returns a <see cref="Prop"/> if this handle corresponds to a Prop.
		/// Returns <see langword="null" /> if no <see cref="Entity"/> exists this the specified <paramref name="handle"/>.
		/// </returns>
		public static Entity FromHandle(int handle)
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
				IntPtr address = MemoryAddress;
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteByte(address + 0xDA, (byte)((int)value & 0xF));
			}
		}

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
		/// <seealso cref="Exists"/>
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

		#region Streaming

		/// <summary>
		/// Gets a value that indicates whether this <see cref="Entity"/> has a drawable object.
		/// <see cref="PlayAnimation(string, ClipDictionary, AnimationBlendDelta, bool, bool, bool, float, AnimationFlags)"/>
		/// and <c>PLAY_SYNCHRONIZED_ENTITY_ANIM</c> require the <see cref="Entity"/> to have a drawable.
		/// You can use this property to check that the entity has a drawable before attempting to play the anim.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if this <see cref="Entity"/> has a drawable object; otherwise, <see langword="false"/>.
		/// </value>
		public bool HasDrawable => Function.Call<bool>(Hash.DOES_ENTITY_HAVE_DRAWABLE, Handle);

		/// <summary>
		/// Gets a value that indicates whether this <see cref="Entity"/> has a skeleton.
		/// <see cref="HasAnimationEventFired(int)"/> requires the <see cref="Entity"/> to have a skeleton.
		/// You can use this property to check that the entity has a skeleton before calling that method.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if this <see cref="Entity"/> has a skeleton; otherwise, <see langword="false"/>.
		/// </value>
		public bool HasSkeleton
		{
			get
			{
				return (int)Game.Version >= (int)GameVersion.v1_0_2699_0_Steam
					? Function.Call<bool>(Hash.DOES_ENTITY_HAVE_SKELETON, Handle)
					: SHVDN.NativeMemory.EntityHasSkeleton(Handle);
			}
		}

		/// <summary>
		/// Gets a value that indicates whether this <see cref="Entity"/> has an animation director.
		/// <see cref="HasAnimationEventFired(int)"/> requires the <see cref="Entity"/> to have an animation director.
		/// You can use this property to check that the entity has an animation director before calling that method.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if this <see cref="Entity"/> has an animation director; otherwise, <see langword="false"/>.
		/// </value>
		/// <remarks>
		/// Currently only available in v1.0.2699.0 or later.
		/// </remarks>
		public bool HasAnimationDirector => Function.Call<bool>(Hash.DOES_ENTITY_HAVE_ANIM_DIRECTOR, Handle);

		/// <summary>
		/// Gets a value that indicates whether this <see cref="Entity"/> is owned by a SHVDN script including the console.
		/// When this property returns <see langword="true"/>, you can successfully call <see cref="MarkAsNoLongerNeeded"/>.
		/// </summary>
		/// <remarks>
		/// Strictly speaking, this property returns true if the <c>CGameScriptHandler</c> associated with this
		/// <see cref="Entity"/> matches the one of the SHVDN runtime script. Although this property calls
		/// <c>DOES_ENTITY_BELONG_TO_THIS_SCRIPT</c>, ScriptHookVDotNet uses a shared <c>GtaThread</c> since
		/// and the SHVDN runtime script registers only one script via Script Hook V and thus the SHVDN runtime uses
		/// only one <c>CGameScriptHandler</c> instance for all SHVDN scripts (which is held by the <c>GtaThread</c>
		/// of SHVDN runtime script). This behavior is very similar to how RAGE Plugin Hook executes its plugins in how
		/// game resources are managed (not in how .NET assemblies are managed, however).
		/// </remarks>
		// The canonical name of 2nd argument is bDeadCheck in DOES_ENTITY_BELONG_TO_THIS_SCRIPT, but it has no effect
		public bool IsOwnedByShvdnScript => Function.Call<bool>(Hash.DOES_ENTITY_BELONG_TO_THIS_SCRIPT, Handle, true);
		/// <summary>
		/// Gets a value that indicates whether this <see cref="Entity"/> is owned by a script including ysc scripts
		/// or external scripts other than SHVDN.
		/// </summary>
		public bool IsOwnedByAnyScript => OwnerScriptName != null;
		/// <summary>
		/// Gets the script name of the <c>scrThread</c> that owns this <see cref="Entity"/>.
		/// Although you can get distinct names of ysc scripts with this property, you should not except that you can
		/// get a distinct name for external scripts, since both Script Hook V and RAGE Plugin Hook use shared script
		/// names for all scripts/plugins.
		/// </summary>
		/// <remarks>
		/// This property will return <see langword="null"/> if no script owns this <see cref="Entity"/>.
		/// If some Script Hook V script (including SHVDN) owns this <see cref="Entity"/>, this property should return
		/// "audiotest" (or "AudioTest").
		/// If some RAGE Plugin Hook plugin owns this <see cref="Entity"/>, this property should return "RagePluginHook".
		/// </remarks>
		public string OwnerScriptName
		{
			get
			{
				// Get the string address so we can make sure if GET_ENTITY_SCRIPT returns a valid string address
				// Unfortunately, Fucntion.Call returns the empty string even if the address is null for compatibility
				// reasons in v3 and v2, while Marshal.PtrToStringUTF8 in .NET Core/5+ returns null in this case
				// The 2nd argument InstanceId (a pointer value) is eventually always unused, so just set to null
				IntPtr strAddr = Function.Call<IntPtr>(Hash.GET_ENTITY_SCRIPT, Handle, null);
				if (strAddr == IntPtr.Zero)
				{
					return null;
				}

				return SHVDN.StringMarshal.PtrToStringUtf8(strAddr);
			}
		}

		#endregion

		#region Styling

		/// <summary>
		/// Gets the model of the current <see cref="Entity"/>.
		/// </summary>
		public Model Model => new(Function.Call<int>(Hash.GET_ENTITY_MODEL, Handle));

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
		/// Resets the <see cref="Opacity"/>.
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
		/// Set <see cref="Ped.KeepTaskWhenMarkedAsNoLongerNeeded"/> to <see langword="true"/> before calling this method or use <see cref="Ped.SetIsPersistentNoClearTask(bool)"/> instead if you need to keep assigned tasks.
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
				IntPtr address = MemoryAddress;
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
		/// </summary>
		/// <value>
		/// The health as an <see cref="int"/>.
		/// </value>
		/// <remarks>
		/// Use <see cref="HealthFloat"/> instead if you need to get or set the value precisely, since a health value of a <see cref="Entity"/> are stored as a <see cref="float"/>.
		/// You should note <see cref="HealthFloat"/> does not care about the 16-bit unsigned integer value for the max health of the player ped(s) on <c>CPlayerInfo</c> even if the <see cref="Entity"/> is the player <see cref="Ped"/>s, however.
		/// </remarks>
		/// <seealso cref="HealthFloat"/>
		public int Health
		{
			get => Function.Call<int>(Hash.GET_ENTITY_HEALTH, Handle);
			set => Function.Call(Hash.SET_ENTITY_HEALTH, Handle, value);
		}
		/// <summary>
		/// Gets or sets the maximum health of this <see cref="Entity"/> as an <see cref="int"/>.
		/// </summary>
		/// <value>
		/// The maximum health as a <see cref="int"/>.
		/// </value>
		/// <remarks>
		/// Use <see cref="MaxHealthFloat"/> instead if you need to get or set the value precisely, since a max health value of a <see cref="Entity"/> are stored as a <see cref="float"/>.
		/// </remarks>
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
		/// <remarks>
		/// This property does not check <see cref="MaxHealth"/> (for any <see cref="Entity"/>s that are not player <see cref="Ped"/>s) or <see cref="Player.MaxHealth"/> (for player <see cref="Ped"/>s) values.
		/// </remarks>
		public float HealthFloat
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + 640);
			}
			set
			{
				IntPtr address = MemoryAddress;
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
		/// <remarks>
		/// <para>
		/// This method also set value to the 16-bit unsigned integer field for the max health of the player ped(s) on <c>CPlayerInfo</c> to prevent them from having inconsistent max health values.
		/// while the base implementation of <see cref="MaxHealth"/> does not care about it at all since there is <see cref="Ped.MaxHealth"/>, where the max health of the player ped(s) on <c>CPlayerInfo</c> is considered.
		/// </para>
		/// <para>
		/// You should not set a value larger than <c>65535.0</c> or a negative value for the player ped(s) as the game uses the 16-bit unsigned integer value for the max health of the player ped(s) on <c>CPlayerInfo</c>
		/// and it is used when respawning and in <c>SET_ENTITY_MAX_HEALTH</c> as the max limit.
		/// Setting a value larger than <c>65535.0</c> will result in the overflow of the 16-bit unsigned integer value for the max health of <c>CPlayerInfo</c>.
		/// </para>
		/// </remarks>
		public float MaxHealthFloat
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.EntityMaxHealthOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.EntityMaxHealthOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.EntityMaxHealthOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.EntityMaxHealthOffset, value);

				// Only needed for setter as GET_ENTITY_HEALTH doesn't use the the uint16_t max health value of CPlayerInfo in any cases
				// SET_ENTITY_HEALTH ignores the float max health value of CPhysical and instead uses the max health value of CPlayerInfo for the player ped
				// SET_ENTITY_MAX_HEALTH sets both values for player peds as thw health display on HUD uses the float max health value of CPhysical
				#region Special Max Health Treatment for Player Ped
				if (SHVDN.NativeMemory.Ped.UnkStateOffset == 0 || SHVDN.NativeMemory.PedPlayerInfoOffset == 0)
				{
					return;
				}

				var entityType = (EntityTypeInternal)SHVDN.NativeMemory.ReadByte(address + 0x28);
				if (entityType != EntityTypeInternal.Ped)
				{
					return;
				}

				byte unkPedState = SHVDN.NativeMemory.ReadByte(address + SHVDN.NativeMemory.Ped.UnkStateOffset);
				if (unkPedState != 2)
				{
					return;
				}


				IntPtr cPlayerInfo = SHVDN.NativeMemory.ReadAddress(address + SHVDN.NativeMemory.PedPlayerInfoOffset);
				if (cPlayerInfo == IntPtr.Zero)
				{
					return;
				}

				// SET_ENTITY_MAX_HEALTH doesn't care about overflow either
				SHVDN.NativeMemory.WriteUInt16(address + SHVDN.NativeMemory.CPlayerInfoMaxHealthOffset, (ushort)System.Math.Ceiling(value));
				#endregion
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return new Matrix();
				}

				return new Matrix(SHVDN.NativeMemory.ReadMatrix(address + 96));
			}
		}

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
		/// The rotation order is <see cref="EulerRotationOrder.YXZ"/>.
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
				IntPtr address = MemoryAddress;
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
				IntPtr address = MemoryAddress;
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
				IntPtr address = MemoryAddress;
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
				Model.GetDimensions(out Vector3 rearBottomLeft, out _);
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
				Model.GetDimensions(out _, out Vector3 frontTopRight);
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
				Model.GetDimensions(out Vector3 rearBottomLeft, out _);
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
				Model.GetDimensions(out _, out Vector3 frontTopRight);
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
				Model.GetDimensions(out _, out Vector3 frontTopRight);
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
				Model.GetDimensions(out Vector3 rearBottomLeft, out _);
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
		[Obsolete("Entity.RotationVelocity is obsolete because GET_ENTITY_ROTATION_VELOCITY returns the world angular velocity with local to world conversion applied. Use Entity.LocalRotationVelocity instead.")]
		public Vector3 RotationVelocity
		{
			get => Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION_VELOCITY, Handle);
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				Vector3 angularVelocityInLocalAxes = Quaternion * value;
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				unsafe
				{
					float* returnVectorPtr = SHVDN.NativeMemory.GetEntityAngularVelocity(address);
					return new Vector3(returnVectorPtr[0], returnVectorPtr[1], returnVectorPtr[2]);
				}
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.SetEntityAngularVelocity(address, value.X, value.Y, value.Z);
			}
		}

		/// <summary>
		/// Gets or sets the rotation velocity of this <see cref="Entity"/> in local space.
		/// </summary>
		public Vector3 LocalRotationVelocity
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				Quaternion quaternionInverted = Quaternion;
				quaternionInverted.Invert();
				unsafe
				{
					float* returnVectorPtr = SHVDN.NativeMemory.GetEntityAngularVelocity(address);
					return (quaternionInverted * new Vector3(returnVectorPtr[0], returnVectorPtr[1], returnVectorPtr[2]));
				}
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				Vector3 angularVelocityWorldSpace = Quaternion * value;
				SHVDN.NativeMemory.SetEntityAngularVelocity(address, angularVelocityWorldSpace.X, angularVelocityWorldSpace.Y, angularVelocityWorldSpace.Z);
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
			return Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_WEAPON, Handle, (uint)weapon, 0);
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
				IntPtr address = MemoryAddress;
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsEntityFragmentObject(address);
			}
		}

		/// <summary>
		/// Detaches a fragment part of this <see cref="Entity"/>. Can create a new <see cref="Entity"/>.
		/// </summary>
		/// <returns>
		///   <para><see langword="true" /> if a new <see cref="Entity"/> is created; otherwise, <see langword="false" />.</para>
		///   <para>Returning <see langword="false" /> does not necessarily mean detaching the part did not change the <see cref="Entity"/> in any ways.
		///   For example, detaching <c>seat_f</c> for <see cref="Vehicle"/> will return <see langword="false" /> but the <see cref="Ped"/> on the front seat will not be able to sit properly.</para>
		/// </returns>
		public bool DetachFragmentPart(int fragmentGroupIndex)
		{
			IntPtr address = MemoryAddress;
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 5);
			}
			set
			{
				IntPtr address = MemoryAddress;
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 7);
			}
			set
			{
				IntPtr address = MemoryAddress;
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 4);
			}
			set
			{
				IntPtr address = MemoryAddress;
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 11);
			}
			set
			{
				IntPtr address = MemoryAddress;
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 6);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.SetBit(address + 392, 6, value);
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 12);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.SetBit(address + 392, 12, value);
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 15);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.SetBit(address + 392, 15, value);
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
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 392, 16);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.SetBit(address + 392, 16, value);
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
				IntPtr address = MemoryAddress;
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
				IntPtr address = MemoryAddress;
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
		public bool IsOccluded => Function.Call<bool>(Hash.IS_ENTITY_OCCLUDED, Handle);

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
				IntPtr address = MemoryAddress;
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
		/// Gets a value indicating whether this <see cref="Entity"/> is upright within 30f degrees.
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
		/// Gets an upright value for this <see cref="Entity"/> between 1.0 being upright and -1.0 being upside down.
		/// </summary>
		/// <value>
		/// The upright value between 1.0 being upright and -1.0 being upside down.
		/// </value>
		public float UprightValue => Function.Call<float>(Hash.GET_ENTITY_UPRIGHT_VALUE, Handle);

		/// <summary>
		/// Checks if this <see cref="Entity"/> is upright within a define angle limit.
		/// </summary>
		public bool IsUprightWithin(float angleToVerticalLimit = 90f) => Function.Call<bool>(Hash.IS_ENTITY_UPRIGHT, Handle, angleToVerticalLimit);

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
				IntPtr address = MemoryAddress;
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
		/// Gets a value indicating whether this <see cref="Entity"/> has collided with a <see cref="Building"/> or an <see cref="AnimatedBuilding"/>.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Entity"/> has collided; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks><see cref="IsRecordingCollisions"/> must be <see langword="true" /> for this to work.</remarks>
		public bool HasCollidedWithBuildingOrAnimatedBuilding => SHVDN.NativeMemory.HasEntityCollidedWithBuildingOrAnimatedBuilding(Handle);

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
		/// Gets the <see cref="Vehicle"/> this <see cref="Entity"/> has collided with.
		/// </summary>
		public Vehicle VehicleCollidingWith
		{
			get
			{
				int vehicleHandle = SHVDN.NativeMemory.GetVehicleHandleEntityIsCollidingWith(Handle);
				return vehicleHandle != 0 ? new Vehicle(vehicleHandle) : null;
			}
		}

		/// <summary>
		/// Gets the <see cref="Ped"/> this <see cref="Entity"/> has collided with.
		/// </summary>
		public Ped PedCollidingWith
		{
			get
			{
				int pedHandle = SHVDN.NativeMemory.GetPedHandleEntityIsCollidingWith(Handle);
				return pedHandle != 0 ? new Ped(pedHandle) : null;
			}
		}

		/// <summary>
		/// Gets the <see cref="Prop"/> this <see cref="Entity"/> has collided with.
		/// </summary>
		public Prop PropCollidingWith
		{
			get
			{
				int propHandle = SHVDN.NativeMemory.GetPropHandleEntityIsCollidingWith(Handle);
				return propHandle != 0 ? new Prop(propHandle) : null;
			}
		}

		/// <summary>
		/// Gets the physical <see cref="Entity"/> this <see cref="Entity"/> has collided with from the last collision record,
		/// where a <see cref="Building"/> or an <see cref="AnimatedBuilding"/> can be stored instead of a physical entity.
		/// </summary>
		/// <param name="entity">
		/// When this method returns, contains the physical <see cref="Entity"/> this <see cref="Entity"/> has collided with,
		/// if the last collision record exists on this <see cref="Entity"/> and the collision record has a has a physical <see cref="Entity"/> address as a target;
		/// otherwise, <see langword="null"/>.
		/// This parameter is passed uninitialized.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the last collision record exists on this <see cref="Entity"/> and the collision record has a has a physical <see cref="Entity"/> address as a target;
		/// otherwise, <see langword="false"/>.
		/// </returns>
		public bool TryGetPhysicalEntityFromLastCollisionRecord(out Entity entity)
		{
			entity = FromHandle(SHVDN.NativeMemory.GetPhysicalEntityHandleFromLastCollisionEntryOfEntity(Handle));
			return entity is not null;
		}

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
		/// This value must be set to <see langword="true"/> before this <see cref="Entity"/> can record their collision records and you can properly fetch some of the records,
		/// via properties such as <see cref="HasCollided"/> and <see cref="TryGetPhysicalEntityFromLastCollisionRecord(out Entity)"/>.
		/// </summary>
		public bool IsRecordingCollisions
		{
			get => SHVDN.NativeMemory.EntityRecordsCollision(Handle);
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
		/// Determines whether this <see cref="Entity"/> is in a specified area.
		/// </summary>
		/// <param name="minBounds">The minimum bounds.</param>
		/// <param name="maxBounds">The maximum bounds.</param>
		/// <returns>
		/// <see langword="true" /> if this <see cref="Entity"/> is in the specified area; otherwise, <see langword="false" />.
		/// </returns>
		/// <remarks>
		/// This overload tests with a 2D area defined in X and Y axes for compatibility built against v3.6.0 or earlier,
		/// while most tests of IS_ENTITY_IN_AREA are done with 3D areas in ysc scripts.
		/// When you test with a 3D area, you need to use <see cref="IsInArea(Vector3, Vector3, bool, PedTransportMode)"/>.
		/// </remarks>
		public bool IsInArea(Vector3 minBounds, Vector3 maxBounds)
			=> IsInArea(minBounds, maxBounds, false, PedTransportMode.Any);
		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in a specified area.
		/// </summary>
		/// <param name="minCoords">
		/// A coordinates that defines the axis aligned area to test along with <paramref name="maxCoords"/>.
		/// Despite the name, does not have to be lower than <paramref name="maxCoords"/> in all components.
		/// </param>
		/// <param name="maxCoords">
		/// A coordinates that defines the axis aligned area to test along with <paramref name="minCoords"/>.
		/// Despite the name, does not have to be higher than <paramref name="maxCoords"/> in all components.
		/// </param>
		/// <param name="do3DCheck">
		/// If set to <see langword="true"/>, the method will test if the <see cref="Entity"/> is in an 3D area,
		/// which means both coordinates will be also considered in z axis.
		/// If set to <see langword="false"/>, the method will only check if the point is in area in X and Y axes.
		/// </param>
		/// <param name="transportMode">
		/// The transport mode constraint so the test can be passed.
		/// </param>
		/// <returns>
		/// <see langword="true" /> if this <see cref="Entity"/> is in the specified area; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsInArea(Vector3 minCoords, Vector3 maxCoords, bool do3DCheck,
			PedTransportMode transportMode = PedTransportMode.Any)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_IN_AREA, Handle, minCoords.X, minCoords.Y, minCoords.Z, maxCoords.X,
				maxCoords.Y, maxCoords.Z, false /* HighlightArea (unused in public builds) */, do3DCheck,
				(int)transportMode);
		}
		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in a specified angled area.
		/// </summary>
		/// <param name="origin">The mid-point along a base edge of the rectangle.</param>
		/// <param name="edge">The mid-point of opposite base edge on the other Z.</param>
		/// <param name="angle">The width. Wrongly named parameter but is kept for existing script compatibilities.</param>
		/// <returns>
		///   <see langword="true"/> if this <see cref="Entity"/> is in the specified angled area; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsInAngledArea(Vector3 origin, Vector3 edge, float angle)
			=> IsInAngledArea(origin, edge, angle, true, PedTransportMode.Any);
		/// <summary>
		/// <inheritdoc
		/// cref="IsInAngledArea(Vector3, Vector3, float, bool, PedTransportMode)"
		/// path="/summary"
		/// />
		/// </summary>
		/// <param name="originEdge">
		/// <inheritdoc
		/// cref="IsInAngledArea(Vector3, Vector3, float, bool, PedTransportMode)"
		/// path="/param[@name='originEdge']"
		/// />
		/// </param>
		/// <param name="extentEdge">
		/// <inheritdoc
		/// cref="IsInAngledArea(Vector3, Vector3, float, bool, PedTransportMode)"
		/// path="/param[@name='extentEdge']"
		/// />
		/// </param>
		/// <param name="width">
		/// <inheritdoc
		/// cref="IsInAngledArea(Vector3, Vector3, float, bool, PedTransportMode)"
		/// path="/param[@name='width']"
		/// />
		/// </param>
		/// <param name="includeZAxis">
		/// <inheritdoc
		/// cref="IsInAngledArea(Vector3, Vector3, float, bool, PedTransportMode)"
		/// path="/param[@name='do3DCheck']"
		/// />
		/// </param>
		/// <returns>
		/// <inheritdoc
		/// cref="IsInAngledArea(Vector3, Vector3, float, bool, PedTransportMode)"
		/// path="/returns"
		/// />
		/// </returns>
		public bool IsInAngledArea(Vector3 originEdge, Vector3 extentEdge, float width, bool includeZAxis)
		 => IsInAngledArea(originEdge, extentEdge, width, includeZAxis, PedTransportMode.Any);
		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in a specified angled area.
		/// An angled area is an X-Z oriented rectangle with three parameters: origin, extent, and width.
		/// </summary>
		/// <param name="originEdge">The mid-point along a base edge of the rectangle.</param>
		/// <param name="extentEdge">The mid-point of opposite base edge on the other Z.</param>
		/// <param name="width">The length of the base edge.</param>
		/// <param name="do3DCheck">
		/// If set to <see langword="true"/>, the method will also check if the point is in area in Z axis as well as
		/// X and Y axes.
		/// If set to <see langword="false"/>, the method will only check if the point is in area in X and Y axes.
		/// </param>
		/// <param name="transportMode">
		/// The transport mode constraint so the test can be passed.
		/// </param>
		/// <returns>
		/// <see langword="true" /> if this <see cref="Entity"/> is in the specified angled area;
		/// otherwise, <see langword="false" />.
		/// </returns>
		public bool IsInAngledArea(Vector3 originEdge, Vector3 extentEdge, float width, bool do3DCheck,
			PedTransportMode transportMode = PedTransportMode.Any)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_IN_ANGLED_AREA, Handle, originEdge.X, originEdge.Y, originEdge.Z,
				extentEdge.X, extentEdge.Y, extentEdge.Z, width, false /* HighlightArea (unused in public builds) */,
				do3DCheck, (int)transportMode);
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

		/// <inheritdoc cref="IsNearEntity(Entity, Vector3, bool, PedTransportMode)"/>
		public bool IsNearEntity(Entity entity, Vector3 bounds)
			=> IsNearEntity(entity, bounds, true, PedTransportMode.Any);
		/// <summary>
		/// Determines whether this <see cref="Entity"/> is near a specified <see cref="Entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to check.</param>
		/// <param name="bounds">The max displacement from the <paramref name="entity"/>.</param>
		/// <param name="do3DCheck">
		/// If set to <see langword="true"/>, the method will also check if the point is in area in Z axis as well as
		/// X and Y axes.
		/// If set to <see langword="false"/>, the method will only check if the point is in area in X and Y axes.
		/// </param>
		/// <param name="transportMode">
		/// The transport mode constraint so the test can be passed.
		/// </param>
		/// <returns>
		/// <see langword="true" /> if this <see cref="Entity"/> is near the <paramref name="entity"/>;
		/// otherwise, <see langword="false" />.
		/// </returns>
		public bool IsNearEntity(Entity entity, Vector3 bounds, bool do3DCheck,
			PedTransportMode transportMode = PedTransportMode.Any)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_AT_ENTITY, Handle, entity.Handle, bounds.X, bounds.Y, bounds.Z,
				false /* HighlightArea (unused in public builds) */, do3DCheck, (int)transportMode);
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
		/// <remarks>Returns <see langword="null" /> if no <see cref="Blip"/>s are attached to this <see cref="Entity"/></remarks>
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
		public Blip[] AttachedBlips => World.GetAllBlips().Where(x => Function.Call<int>(Hash.GET_BLIP_INFO_ID_ENTITY_INDEX, x.NativeValue) == Handle).ToArray();

		#endregion

		#region Attaching

		/// <summary>
		/// Detaches this <see cref="Entity"/> from any <see cref="Entity"/> it may be attached to.
		/// </summary>
		public void Detach() => Detach(true, true);
		/// <summary>
		/// Detaches this <see cref="Entity"/> from any <see cref="Entity"/> it may be attached to.
		/// </summary>
		/// <param name="applyVelocity">
		/// If <see langword="true"/> and this <see cref="Entity"/> is not a <see cref="Ped"/>,
		/// this <see cref="Entity"/> will detach while apply velocity of the parent <see cref="Entity"/>.
		/// </param>
		/// <param name="noCollisionUntilClear">
		/// If <see langword="true"/> and this <see cref="Entity"/> is not a <see cref="Vehicle"/>,
		/// this method will set states to disable collision between this <see cref="Entity"/> and the parent <see cref="Entity"/> until they are clear of one another.
		/// </param>
		public void Detach(bool applyVelocity, bool noCollisionUntilClear)
		{
			Function.Call(Hash.DETACH_ENTITY, Handle, applyVelocity, noCollisionUntilClear);
		}

		/// <inheritdoc cref="AttachTo(Entity, Vector3, Vector3, bool, bool, bool, bool, EulerRotationOrder, bool, bool)"/>
		public void AttachTo(Entity entity, Vector3 position = default, Vector3 rotation = default)
			=> AttachTo(entity, position, rotation, false, false, false, false, EulerRotationOrder.YXZ);
		/// <inheritdoc cref="AttachTo(EntityBone, Vector3, Vector3, bool, bool, bool, bool, EulerRotationOrder, bool, bool)"/>
		public void AttachTo(EntityBone entityBone, Vector3 position = default, Vector3 rotation = default)
			=> AttachTo(entityBone, position, rotation, false, false, false, false, EulerRotationOrder.YXZ);
		/// <summary>
		/// Attaches this <see cref="Entity"/> to a different <see cref="Entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to attach this <see cref="Entity"/> to.</param>
		/// <param name="offset">
		/// The offset relative to the <paramref name="entity"/> to attach this <see cref="Entity"/> to.
		/// If <paramref name="attachOffsetIsRelative"/> is <see langword="true"/>, the offset will be used as offset
		/// in world space.
		/// </param>
		/// <param name="rotation">The rotation to apply to this <see cref="Entity"/> relative to the <paramref name="entity"/>.</param>
		/// <param name="detachWhenDead">
		/// If <see langword="true"/> and this <see cref="Entity"/> is a <see cref="Ped"/>, the <see cref="Ped"/>
		/// will be detached when they are dead.
		/// </param>
		/// <param name="detachWhenRagdoll">
		/// If <see langword="true"/> and this <see cref="Entity"/> is a <see cref="Ped"/>, the <see cref="Ped"/>
		/// will be detached when they ragdolls.
		/// </param>
		/// <param name="activeCollisions">
		/// Specifies whether the collision of this <see cref="Entity"/> will be left activated for other
		/// <see cref="Entity"/>s with colliders. This <see cref="Entity"/> will not collide with static collisions
		/// such as map collisions, which are part of <see cref="Building"/>s or <see cref="AnimatedBuilding"/>s, or
		/// those of <see cref="Entity"/>s with no working colliders.
		/// </param>
		/// <param name="useBasicAttachIfPed">
		/// If <see langword="true"/> this method forces a path, even for <see cref="Ped"/>s, that will use all three rotation components
		/// This parameter does not have effect if this <see cref="Entity"/> is a <see cref="Vehicle"/> or <see cref="Prop"/>.
		/// </param>
		/// <param name="rotationOrder">The rotation order.</param>
		/// <param name="attachOffsetIsRelative">
		/// <para>
		/// Specifies whether <paramref name="offset"/> is in the local space of <paramref name="entity"/>
		/// rather than world space.
		/// </para>
		/// <para>
		/// Always set to <see langword="true"/> in all occurrences of <c>ATTACH_ENTITY_TO_ENTITY</c> in ysc scripts
		/// as of v1.0.2944.0.
		/// </para>
		/// </param>
		/// <param name="markAsNoLongerNeededWhenDetached">
		/// <para>
		/// If <see langword="true"/> and the game version is v1.0.1493.0 or later, the game marks as no longer
		/// needed when this <see cref="Entity"/> gets detached from <paramref name="entity"/>.
		/// This <see cref="Entity"/> must be owned by the SHVDN runtime to get marked as no longer needed
		/// when this <see cref="Entity"/> gets detached, or the <see cref="Entity"/> will just get detached.
		/// </para>
		/// <para>
		/// Always set to <see langword="false"/> in all occurrences of <c>ATTACH_ENTITY_TO_ENTITY</c> in ysc scripts
		/// as of v1.0.2944.0.
		/// </para>
		/// </param>
		public void AttachTo(Entity entity, Vector3 offset, Vector3 rotation, bool detachWhenDead = false,
			bool detachWhenRagdoll = false, bool activeCollisions = false, bool useBasicAttachIfPed = false,
			EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ, bool attachOffsetIsRelative = true,
			bool markAsNoLongerNeededWhenDetached = false)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Handle, entity, -1, offset.X, offset.Y, offset.Z,
				rotation.X, rotation.Y, rotation.Z, detachWhenDead, detachWhenRagdoll, activeCollisions, useBasicAttachIfPed,
				(int)rotationOrder, attachOffsetIsRelative, markAsNoLongerNeededWhenDetached);
		}

		/// <param name="entityBone">The <see cref="EntityBone"/> to attach this <see cref="Entity"/> to.</param>
		/// <param name="offset">
		/// <inheritdoc
		/// cref="AttachTo(Entity, Vector3, Vector3, bool, bool, bool, bool, EulerRotationOrder, bool, bool)"
		/// path="/param[@name='offset']"
		/// />
		/// </param>
		/// <param name="rotation">
		/// <inheritdoc
		/// cref="AttachTo(Entity, Vector3, Vector3, bool, bool, bool, bool, EulerRotationOrder, bool, bool)"
		/// path="/param[@name='rotation']"
		/// />
		/// </param>
		/// <param name="detachWhenDead">
		/// <inheritdoc
		/// cref="AttachTo(Entity, Vector3, Vector3, bool, bool, bool, bool, EulerRotationOrder, bool, bool)"
		/// path="/param[@name='detachWhenDead']"
		/// />
		/// </param>
		/// <param name="detachWhenRagdoll">
		/// <inheritdoc
		/// cref="AttachTo(Entity, Vector3, Vector3, bool, bool, bool, bool, EulerRotationOrder, bool, bool)"
		/// path="/param[@name='detachWhenRagdoll']"
		/// />
		/// </param>
		/// <param name="activeCollisions">
		/// <inheritdoc
		/// cref="AttachTo(Entity, Vector3, Vector3, bool, bool, bool, bool, EulerRotationOrder, bool, bool)"
		/// path="/param[@name='activeCollisions']"
		/// />
		/// </param>
		/// <param name="useBasicAttachIfPed">
		/// <inheritdoc
		/// cref="AttachTo(Entity, Vector3, Vector3, bool, bool, bool, bool, EulerRotationOrder, bool, bool)"
		/// path="/param[@name='useBasicAttachIfPed']"
		/// />
		/// </param>
		/// <param name="rotationOrder">
		/// <inheritdoc
		/// cref="AttachTo(Entity, Vector3, Vector3, bool, bool, bool, bool, EulerRotationOrder, bool, bool)"
		/// path="/param[@name='rotationOrder']"
		/// />
		/// </param>
		/// <param name="attachOffsetIsRelative">
		/// <inheritdoc
		/// cref="AttachTo(Entity, Vector3, Vector3, bool, bool, bool, bool, EulerRotationOrder, bool, bool)"
		/// path="/param[@name='attachOffsetIsRelative']"
		/// />
		/// </param>
		/// <param name="markAsNoLongerNeededWhenDetached">
		/// <inheritdoc
		/// cref="AttachTo(Entity, Vector3, Vector3, bool, bool, bool, bool, EulerRotationOrder, bool, bool)"
		/// path="/param[@name='markAsNoLongerNeededWhenDetached']"
		/// />
		/// </param>
		///
		///	<inheritdoc
		/// cref="AttachTo(Entity, Vector3, Vector3, bool, bool, bool, bool, EulerRotationOrder, bool, bool)"
		/// />
		public void AttachTo(EntityBone entityBone, Vector3 offset, Vector3 rotation, bool detachWhenDead = false,
			bool detachWhenRagdoll = false, bool activeCollisions = false, bool useBasicAttachIfPed = false,
			EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ, bool attachOffsetIsRelative = true,
			bool markAsNoLongerNeededWhenDetached = false)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Handle, entityBone.Owner, entityBone.Index, offset.X, offset.Y,
				offset.Z, rotation.X, rotation.Y, rotation.Z, detachWhenDead, detachWhenRagdoll, activeCollisions,
				useBasicAttachIfPed, (int)rotationOrder, attachOffsetIsRelative, markAsNoLongerNeededWhenDetached);
		}

		/// <summary>
		/// Attaches this <see cref="Entity"/> to a different <see cref="Entity"/>.
		/// </summary>
		/// <param name="boneOfThisEntity">
		/// The <see cref="EntityBone"/> to attach to <paramref name="boneOfSecondEntity"/>.
		/// </param>
		/// <param name="boneOfSecondEntity">
		/// The <see cref="EntityBone"/> to attach this <see cref="Entity"/> to that belongs to another <see cref="Entity"/>.
		/// </param>
		/// <param name="activeCollisions">
		/// If <see langword="true"/> this <see cref="Entity"/> collide with other <see cref="Entity"/>s (does not
		/// collide with map collision or static <see cref="Prop"/>s that do not have physics/colliders).
		/// </param>
		/// <param name="useBasicAttachIfPed">
		/// Specifies whether the method forces a path, even for <see cref="Ped"/>s, that will use all three rotation
		/// components, just like how <see cref="Vehicle"/>s and <see cref="Prop"/>s will be placed.
		/// if <see langword="false"/>, pitch will not work and roll will only work on negative numbers for <see cref="Ped"/>s.
		/// If this <see cref="Entity"/> is a <see cref="Vehicle"/>s or <see cref="Prop"/>, setting this parameter
		/// to <see langword="true"/> has no effect as they use all rotations by default.
		/// </param>
		/// <remarks>
		/// Both <see cref="Entity"/>s must have skeletons if they exist in the game. Otherwise, this method will throw
		/// a <see cref="InvalidOperationException"/> or <see cref="ArgumentException"/> so the method can prevent the
		/// game from getting crashed for trying to access the null <c>rage::crSkeleton</c> in a virtual function of
		/// <c>CEntity</c>.
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if called in the game versions earlier than v1.0.791.2.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown if both of the entities exist but this <see cref="Entity"/> does not have a skeleton.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Thrown if both of the entities exist but the <see cref="Entity"/> of <paramref name="boneOfSecondEntity"/>
		/// does not have a skeleton.
		/// </exception>
		public void AttachBoneTo(EntityBone boneOfThisEntity, EntityBone boneOfSecondEntity,
			bool activeCollisions = true, bool useBasicAttachIfPed = false)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_791_2_Steam, nameof(Entity),
				nameof(AttachBoneTo));

			Entity secondEntity = boneOfSecondEntity.Owner;

			if (!ThisEntityAndSecondEntityExist(secondEntity))
			{
				return;
			}

			ThrowExceptionIfEitherOfEntityDoesNotHaveSkeleton(secondEntity);

			Function.Call(Hash.ATTACH_ENTITY_BONE_TO_ENTITY_BONE, Handle, boneOfThisEntity.Index, secondEntity,
				boneOfSecondEntity.Index, activeCollisions, useBasicAttachIfPed);
		}

		/// <summary>
		/// Attaches this <see cref="Entity"/> to a different <see cref="Entity"/> assuming that the bone is facing
		/// along the y axis.
		/// </summary>
		/// <param name="boneOfThisEntity">
		/// The <see cref="EntityBone"/> to attach to <paramref name="boneOfSecondEntity"/>.
		/// </param>
		/// <param name="boneOfSecondEntity">
		/// The <see cref="EntityBone"/> to attach this <see cref="Entity"/> to that belongs to another <see cref="Entity"/>.
		/// </param>
		/// <param name="activeCollisions">
		/// If <see langword="true"/> this <see cref="Entity"/> collide with other <see cref="Entity"/>s (does not
		/// collide with map collision or static <see cref="Prop"/>s that do not have physics/colliders).
		/// </param>
		/// <param name="useBasicAttachIfPed">
		/// Specifies whether the method forces a path, even for <see cref="Ped"/>s, that will use all three rotation
		/// components, just like how <see cref="Vehicle"/>s and <see cref="Prop"/>s will be placed.
		/// if <see langword="false"/>, pitch will not work and roll will only work on negative numbers for <see cref="Ped"/>s.
		/// If this <see cref="Entity"/> is a <see cref="Vehicle"/>s or <see cref="Prop"/>, setting this parameter
		/// to <see langword="true"/> has no effect as they use all rotations by default.
		/// </param>
		/// <remarks>
		/// Both <see cref="Entity"/>s must have skeletons if they exist in the game. Otherwise, this method will throw
		/// a <see cref="InvalidOperationException"/> or <see cref="ArgumentException"/> so the method can prevent the
		/// game from getting crashed for trying to access the null <c>rage::crSkeleton</c> in a virtual function of
		/// <c>CEntity</c>.
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if called in the game versions earlier than v1.0.791.2.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown if both of the entities exist but this <see cref="Entity"/> does not have a skeleton.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Thrown if both of the entities exist but the <see cref="Entity"/> of <paramref name="boneOfSecondEntity"/>
		/// does not have a skeleton.
		/// </exception>
		public void AttachBoneToYForward(EntityBone boneOfThisEntity, EntityBone boneOfSecondEntity,
			bool activeCollisions = true, bool useBasicAttachIfPed = false)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_791_2_Steam, nameof(Entity),
				nameof(AttachBoneToYForward));

			Entity secondEntity = boneOfSecondEntity.Owner;
			if (!ThisEntityAndSecondEntityExist(secondEntity))
			{
				return;
			}

			ThrowExceptionIfEitherOfEntityDoesNotHaveSkeleton(secondEntity);

			Function.Call(Hash.ATTACH_ENTITY_BONE_TO_ENTITY_BONE_Y_FORWARD, Handle, boneOfThisEntity.Index,
				secondEntity, boneOfSecondEntity.Index, activeCollisions, useBasicAttachIfPed);
		}

		private bool ThisEntityAndSecondEntityExist(Entity secondEntity)
		{
			// Silently return if either of the entities does not exist, just like how ATTACH_ENTITY_BONE_TO_ENTITY_BONE checks
			// We don't want to throw exceptions that much unless otherwise the game will encounter a game crash,
			// as we can't really check all the conditions needed for natives without inspecting the assembly code of them
			return Exists() && (secondEntity?.Exists() ?? false);
		}

		/// <summary>
		/// This method is present since ATTACH_ENTITY_BONE_TO_ENTITY_BONE will crash the game if both entities exist but
		/// either of them does not have a skeleton.
		/// </summary>
		private void ThrowExceptionIfEitherOfEntityDoesNotHaveSkeleton(Entity secondEntity)
		{
			if (!HasSkeleton)
			{
				throw new InvalidOperationException("This entity does not have a skeleton.");
			}

			if (!secondEntity.HasSkeleton)
			{
				throw new ArgumentException("The passed entity does not have a skeleton.", nameof(secondEntity));
			}
		}

		/// <summary>
		/// Attaches this <see cref="Entity"/> to the transformation matrix (physics capsule) of a different
		/// <see cref="Entity"/>.
		/// </summary>
		/// <param name="secondEntity">
		/// The <see cref="Entity"/> to attach this <see cref="Entity"/> to.
		/// </param>
		/// <param name="secondEntityOffset">
		/// The attach point offset of <paramref name="secondEntity"/> in local space.
		/// </param>
		/// <param name="thisEntityOffset">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='thisEntityOffset']"
		/// />
		/// </param>
		/// <param name="rotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotation']"
		/// />
		/// </param>
		/// <param name="physicalStrength">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='physicalStrength']"
		/// />
		/// </param>
		/// <param name="constrainRotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='constrainRotation']"
		/// />
		/// </param>
		/// <param name="doInitialWarp">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='doInitialWarp']"
		/// />
		/// </param>
		/// <param name="addInitialSeparation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='addInitialSeparation']"
		/// />
		/// </param>
		/// <param name="collideWithEntity">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='collideWithEntity']"
		/// />
		/// </param>
		/// <param name="rotationOrder">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotationOrder']"
		/// />
		/// </param>
		public void AttachToMatrixPhysically(Entity secondEntity, Vector3 secondEntityOffset, Vector3 thisEntityOffset,
			Vector3 rotation, float physicalStrength, bool constrainRotation, bool doInitialWarp = true,
			bool collideWithEntity = false, bool addInitialSeparation = true,
			EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY, Handle, secondEntity, -1, -1, secondEntityOffset.X,
				secondEntityOffset.Y, secondEntityOffset.Z, thisEntityOffset.X, thisEntityOffset.Y, thisEntityOffset.Z,
				rotation.X, rotation.Y, rotation.Z, physicalStrength, constrainRotation, doInitialWarp,
				collideWithEntity, addInitialSeparation, (int)rotationOrder);
		}

		/// <summary>
		/// Attaches a bone of this <see cref="Entity"/> to the transformation matrix (physics capsule) of a different
		/// <see cref="Entity"/>.
		/// </summary>
		/// <param name="boneOfThisEntity">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='boneOfThisEntity']"
		/// />
		/// </param>
		/// <param name="secondEntity">
		/// The <see cref="Entity"/> to attach <paramref name="boneOfThisEntity"/> to.
		/// </param>
		/// <param name="secondEntityOffset">
		/// The attach point offset of <paramref name="secondEntity"/> in local space.
		/// </param>
		/// <param name="thisEntityOffset">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='thisEntityOffset']"
		/// />
		/// </param>
		/// <param name="rotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotation']"
		/// />
		/// </param>
		/// <param name="physicalStrength">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='physicalStrength']"
		/// />
		/// </param>
		/// <param name="constrainRotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='constrainRotation']"
		/// />
		/// </param>
		/// <param name="doInitialWarp">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='doInitialWarp']"
		/// />
		/// </param>
		/// <param name="addInitialSeparation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='addInitialSeparation']"
		/// />
		/// </param>
		/// <param name="collideWithEntity">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='collideWithEntity']"
		/// />
		/// </param>
		/// <param name="rotationOrder">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotationOrder']"
		/// />
		/// </param>
		/// <remarks>
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/remarks"
		/// />
		/// </remarks>
		public void AttachBoneToMatrixPhysically(EntityBone boneOfThisEntity, Entity secondEntity,
			Vector3 secondEntityOffset, Vector3 thisEntityOffset, Vector3 rotation, float physicalStrength,
			bool constrainRotation, bool doInitialWarp = true, bool collideWithEntity = false,
			bool addInitialSeparation = true, EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY, Handle, secondEntity, boneOfThisEntity.Index, -1,
				secondEntityOffset.X, secondEntityOffset.Y, secondEntityOffset.Z, thisEntityOffset.X,
				thisEntityOffset.Y, thisEntityOffset.Z, rotation.X, rotation.Y, rotation.Z, physicalStrength,
				constrainRotation, doInitialWarp, collideWithEntity, addInitialSeparation, (int)rotationOrder);
		}

		/// <summary>
		/// Attaches this <see cref="Entity"/> to a bone of a different <see cref="Entity"/>.
		/// </summary>
		/// <param name="boneOfSecondEntity">
		/// The <see cref="EntityBone"/> to attach this <see cref="Entity"/> to that belongs to second/another
		/// <see cref="Entity"/>.
		/// </param>
		/// <param name="secondEntityOffset">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='secondEntityOffset']"
		/// />
		/// </param>
		/// <param name="thisEntityOffset">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='thisEntityOffset']"
		/// />
		/// </param>
		/// <param name="rotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotation']"
		/// />
		/// </param>
		/// <param name="physicalStrength">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='physicalStrength']"
		/// />
		/// </param>
		/// <param name="constrainRotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='constrainRotation']"
		/// />
		/// </param>
		/// <param name="doInitialWarp">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='doInitialWarp']"
		/// />
		/// </param>
		/// <param name="addInitialSeparation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='addInitialSeparation']"
		/// />
		/// </param>
		/// <param name="collideWithEntity">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='collideWithEntity']"
		/// />
		/// </param>
		/// <param name="rotationOrder">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotationOrder']"
		/// />
		/// </param>
		/// <remarks>
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/remarks"
		/// />
		/// </remarks>
		public void AttachToBonePhysically(EntityBone boneOfSecondEntity, Vector3 secondEntityOffset,
			Vector3 thisEntityOffset, Vector3 rotation, float physicalStrength, bool constrainRotation,
			bool doInitialWarp = true, bool collideWithEntity = false, bool addInitialSeparation = true,
			EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY, Handle, boneOfSecondEntity.Owner, -1,
				boneOfSecondEntity.Index, secondEntityOffset.X, secondEntityOffset.Y, secondEntityOffset.Z,
				thisEntityOffset.X, thisEntityOffset.Y, thisEntityOffset.Z, rotation.X, rotation.Y, rotation.Z,
				physicalStrength, constrainRotation, doInitialWarp, collideWithEntity, addInitialSeparation,
				(int)rotationOrder);
		}

		/// <summary>
		/// Attaches a bone of this <see cref="Entity"/> to a bone of a different <see cref="Entity"/>.
		/// </summary>
		/// <param name="boneOfThisEntity">
		/// The <see cref="EntityBone"/> of this <see cref="Entity"/> to attach.
		/// </param>
		/// <param name="boneOfSecondEntity">
		/// The <see cref="EntityBone"/> to attach this <paramref name="boneOfThisEntity"/> to that belongs to
		/// second/another <see cref="Entity"/>.
		/// </param>
		/// <param name="secondEntityOffset">
		/// The attach point offset of <paramref name="boneOfSecondEntity"/> in local space.
		/// </param>
		/// <param name="thisEntityOffset">
		/// The attach point offset of this <see cref="Entity"/> relative to <paramref name="boneOfThisEntity"/>.
		/// </param>
		/// <param name="rotation">
		/// The rotation to apply to this <see cref="Entity"/> relative to the <paramref name="rotation"/>.
		/// </param>
		/// <param name="physicalStrength">
		/// The physical strength. Should be in newton.
		/// Negative values mean that the attachment between the tho <see cref="Entity"/>s
		/// has the infinite physical strength.
		/// A medium strength value is 500.
		/// </param>
		/// <param name="constrainRotation">
		/// Specifies whether you wish to constrain rotation as well as position.
		/// In most cases the answer will be Yes. Unless you want to have a hanging/swinging thing.
		/// </param>
		/// <param name="doInitialWarp">
		/// <para>
		/// Specifies whether to warp this <see cref="Entity"/> to the specified attach point.
		/// If <see langword="true"/> or this <see cref="Entity"/> is a <see cref="Ped"/>,
		/// this method will warp this <see cref="Entity"/> to the specified attach point.
		/// </para>
		/// <para>
		/// Otherwise, If <paramref name="addInitialSeparation"/> is <see langword="true"/>,
		/// the initial separation will be used as an allowed give in the attachment (e.g. a rope length).
		/// If <paramref name="addInitialSeparation"/> is <see langword="false"/>,
		/// this <see cref="Entity"/> will not warp and instead the other one will warp to the attach point.
		/// </para>
		/// <para>
		/// Although setting this parameter to <see langword="true"/> does not make it prevent from warp
		/// this <see cref="Ped"/> to the specified attach point, you need to set this parameter to
		/// <see langword="false"/> if you want this <see cref="Entity"/> and the other <see cref="Entity"/> to allow
		/// separated as long as the initial separation.
		/// </para>
		/// </param>
		/// <param name="addInitialSeparation">
		/// If <see langword="true"/> and <paramref name="doInitialWarp"/> is <see langword="false"/>,
		/// this method will not warp either of the two <see cref="Entity"/>s they can get separated as long as
		/// the initial separation (where the attach point and the two offset determines how high the initial
		/// separation value is).
		/// </param>
		/// <param name="collideWithEntity">
		/// Specifies whether set of the two <see cref="Entity"/>s will collide with each other after attached.
		/// They will collide with other ones by default.
		/// </param>
		/// <param name="rotationOrder">The rotation order.</param>
		/// <remarks>
		/// If a bone index of a <see cref="Entity"/> is invalid, its attach point will fallback to its
		/// transformation matrix.
		/// </remarks>
		public void AttachBoneToBonePhysically(EntityBone boneOfThisEntity, EntityBone boneOfSecondEntity,
			Vector3 secondEntityOffset, Vector3 thisEntityOffset, Vector3 rotation, float physicalStrength,
			bool constrainRotation, bool doInitialWarp = true, bool collideWithEntity = false,
			bool addInitialSeparation = true, EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY, Handle, boneOfSecondEntity.Owner,
				boneOfThisEntity.Index, boneOfSecondEntity.Index, secondEntityOffset.X, secondEntityOffset.Y,
				secondEntityOffset.Z, thisEntityOffset.X, thisEntityOffset.Y, thisEntityOffset.Z, rotation.X,
				rotation.Y, rotation.Z, physicalStrength, constrainRotation, doInitialWarp, collideWithEntity,
				addInitialSeparation, (int)rotationOrder);
		}

		/// <summary>
		/// <para>
		/// Attaches this <see cref="Entity"/> to the transformation matrix (physics capsule) of a different
		/// <see cref="Entity"/> with custom override values of inverse mass scale for the two <see cref="Entity"/>s.
		/// </para>
		/// <para>
		/// Only available in v1.0.2944.0 or later game versions.
		/// </para>
		/// </summary>
		/// <param name="secondEntity">
		/// <inheritdoc
		/// cref="AttachToMatrixPhysically"
		/// path="/param[@name='secondEntity']"
		/// />
		/// </param>
		/// <param name="secondEntityOffset">
		/// <inheritdoc
		/// cref="AttachToMatrixPhysically"
		/// path="/param[@name='secondEntityOffset']"
		/// />
		/// </param>
		/// <param name="thisEntityOffset">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='thisEntityOffset']"
		/// />
		/// </param>
		/// <param name="rotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotation']"
		/// />
		/// </param>
		/// <param name="physicalStrength">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='physicalStrength']"
		/// />
		/// </param>
		/// <param name="constrainRotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='constrainRotation']"
		/// />
		/// </param>
		/// <param name="doInitialWarp">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='doInitialWarp']"
		/// />
		/// </param>
		/// <param name="addInitialSeparation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='addInitialSeparation']"
		/// />
		/// </param>
		/// <param name="collideWithEntity">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='collideWithEntity']"
		/// />
		/// </param>
		/// <param name="rotationOrder">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotationOrder']"
		/// />
		/// </param>
		/// <param name="invMassScaleA">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysicallyOverrideInverseMass"
		/// path="/param[@name='invMassScaleA']"
		/// />
		/// </param>
		/// <param name="invMassScaleB">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysicallyOverrideInverseMass"
		/// path="/param[@name='invMassScaleB']"
		/// />
		/// </param>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if called in the game versions earlier than v1.0.2944.0.
		/// </exception>
		public void AttachToMatrixPhysicallyOverrideInverseMass(Entity secondEntity, Vector3 secondEntityOffset,
			Vector3 thisEntityOffset, Vector3 rotation, float physicalStrength, bool constrainRotation,
			bool doInitialWarp = true, bool collideWithEntity = false, bool addInitialSeparation = true,
			EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ, float invMassScaleA = 1f,
			float invMassScaleB = 1f)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_2944_0, nameof(Entity),
				nameof(AttachToMatrixPhysicallyOverrideInverseMass));

			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY_OVERRIDE_INVERSE_MASS, Handle, secondEntity, -1, -1,
				secondEntityOffset.X, secondEntityOffset.Y, secondEntityOffset.Z, thisEntityOffset.X,
				thisEntityOffset.Y, thisEntityOffset.Z, rotation.X, rotation.Y, rotation.Z, physicalStrength,
				constrainRotation, doInitialWarp, collideWithEntity, addInitialSeparation, (int)rotationOrder,
				invMassScaleA, invMassScaleB);
		}

		/// <summary>
		/// <para>
		/// Attaches a bone of this <see cref="Entity"/> to the transformation matrix (physics capsule) of a different
		/// <see cref="Entity"/>  with custom override values of inverse mass scale for the two <see cref="Entity"/>s.
		/// </para>
		/// <para>
		/// Only available in v1.0.2944.0 or later game versions.
		/// </para>
		/// </summary>
		/// <param name="boneOfThisEntity">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='boneOfThisEntity']"
		/// />
		/// </param>
		/// <param name="secondEntity">
		/// <inheritdoc
		/// cref="AttachBoneToMatrixPhysically"
		/// path="/param[@name='secondEntity']"
		/// />
		/// </param>
		/// <param name="secondEntityOffset">
		/// <inheritdoc
		/// cref="AttachBoneToMatrixPhysically"
		/// path="/param[@name='secondEntityOffset']"
		/// />
		/// </param>
		/// <param name="thisEntityOffset">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='thisEntityOffset']"
		/// />
		/// </param>
		/// <param name="rotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotation']"
		/// />
		/// </param>
		/// <param name="physicalStrength">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='physicalStrength']"
		/// />
		/// </param>
		/// <param name="constrainRotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='constrainRotation']"
		/// />
		/// </param>
		/// <param name="doInitialWarp">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='doInitialWarp']"
		/// />
		/// </param>
		/// <param name="addInitialSeparation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='addInitialSeparation']"
		/// />
		/// </param>
		/// <param name="collideWithEntity">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='collideWithEntity']"
		/// />
		/// </param>
		/// <param name="rotationOrder">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotationOrder']"
		/// />
		/// </param>
		/// <param name="invMassScaleA">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysicallyOverrideInverseMass"
		/// path="/param[@name='invMassScaleA']"
		/// />
		/// </param>
		/// <param name="invMassScaleB">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysicallyOverrideInverseMass"
		/// path="/param[@name='invMassScaleB']"
		/// />
		/// </param>
		/// <remarks>
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/remarks"
		/// />
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if called in the game versions earlier than v1.0.2944.0.
		/// </exception>
		public void AttachBoneToMatrixPhysicallyOverrideInverseMass(EntityBone boneOfThisEntity, Entity secondEntity,
			Vector3 secondEntityOffset, Vector3 thisEntityOffset, Vector3 rotation, float physicalStrength,
			bool constrainRotation, bool doInitialWarp = true, bool collideWithEntity = false,
			bool addInitialSeparation = true, EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ,
			float invMassScaleA = 1f, float invMassScaleB = 1f)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_2944_0, nameof(Entity),
				nameof(AttachBoneToMatrixPhysicallyOverrideInverseMass));

			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY_OVERRIDE_INVERSE_MASS, Handle, secondEntity,
				boneOfThisEntity.Index, -1, secondEntityOffset.X, secondEntityOffset.Y, secondEntityOffset.Z,
				thisEntityOffset.X, thisEntityOffset.Y, thisEntityOffset.Z, rotation.X, rotation.Y, rotation.Z,
				physicalStrength, constrainRotation, doInitialWarp, collideWithEntity, addInitialSeparation,
				(int)rotationOrder, invMassScaleA, invMassScaleB);
		}

		/// <summary>
		/// <para>
		/// Attaches this <see cref="Entity"/> to a bone of a different <see cref="Entity"/>. with custom override
		/// values of inverse mass scale for the two <see cref="Entity"/>s.
		/// </para>
		/// <para>
		/// Only available in v1.0.2944.0 or later game versions.
		/// </para>
		/// </summary>
		/// <param name="boneOfSecondEntity">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='boneOfSecondEntity']"
		/// />
		/// </param>
		/// <param name="secondEntityOffset">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='secondEntityOffset']"
		/// />
		/// </param>
		/// <param name="thisEntityOffset">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='thisEntityOffset']"
		/// />
		/// </param>
		/// <param name="rotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotation']"
		/// />
		/// </param>
		/// <param name="physicalStrength">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='physicalStrength']"
		/// />
		/// </param>
		/// <param name="constrainRotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='constrainRotation']"
		/// />
		/// </param>
		/// <param name="doInitialWarp">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='doInitialWarp']"
		/// />
		/// </param>
		/// <param name="addInitialSeparation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='addInitialSeparation']"
		/// />
		/// </param>
		/// <param name="collideWithEntity">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='collideWithEntity']"
		/// />
		/// </param>
		/// <param name="rotationOrder">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotationOrder']"
		/// />
		/// </param>
		/// <param name="invMassScaleA">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysicallyOverrideInverseMass"
		/// path="/param[@name='invMassScaleA']"
		/// />
		/// </param>
		/// <param name="invMassScaleB">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysicallyOverrideInverseMass"
		/// path="/param[@name='invMassScaleB']"
		/// />
		/// </param>
		/// <remarks>
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/remarks"
		/// />
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if called in the game versions earlier than v1.0.2944.0.
		/// </exception>
		public void AttachToBonePhysicallyOverrideInverseMass(EntityBone boneOfSecondEntity, Vector3 secondEntityOffset,
			Vector3 thisEntityOffset, Vector3 rotation, float physicalStrength, bool constrainRotation,
			bool doInitialWarp = true, bool collideWithEntity = false, bool addInitialSeparation = true,
			EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ, float invMassScaleA = 1f,
			float invMassScaleB = 1f)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_2944_0, nameof(Entity),
				nameof(AttachToBonePhysicallyOverrideInverseMass));

			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY_OVERRIDE_INVERSE_MASS, Handle,
				boneOfSecondEntity.Owner, -1, boneOfSecondEntity.Index, secondEntityOffset.X, secondEntityOffset.Y,
				secondEntityOffset.Z, thisEntityOffset.X, thisEntityOffset.Y, thisEntityOffset.Z, rotation.X,
				rotation.Y, rotation.Z, physicalStrength, constrainRotation, doInitialWarp, collideWithEntity,
				addInitialSeparation, (int)rotationOrder, invMassScaleA, invMassScaleB);
		}

		/// <summary>
		/// <para>
		/// Attaches a bone of this <see cref="Entity"/> to a bone of a different <see cref="Entity"/> with custom
		/// override values of inverse mass scale for the two <see cref="Entity"/>s.
		/// </para>
		/// <para>
		/// Only available in v1.0.2944.0 or later game versions.
		/// </para>
		/// </summary>
		/// <param name="boneOfThisEntity">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='boneOfSecondEntity']"
		/// />
		/// </param>
		/// <param name="boneOfSecondEntity">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='boneOfSecondEntity']"
		/// />
		/// </param>
		/// <param name="secondEntityOffset">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='secondEntityOffset']"
		/// />
		/// </param>
		/// <param name="thisEntityOffset">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='thisEntityOffset']"
		/// />
		/// </param>
		/// <param name="rotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotation']"
		/// />
		/// </param>
		/// <param name="physicalStrength">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='physicalStrength']"
		/// />
		/// </param>
		/// <param name="constrainRotation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='constrainRotation']"
		/// />
		/// </param>
		/// <param name="doInitialWarp">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='doInitialWarp']"
		/// />
		/// </param>
		/// <param name="addInitialSeparation">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='addInitialSeparation']"
		/// />
		/// </param>
		/// <param name="collideWithEntity">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='collideWithEntity']"
		/// />
		/// </param>
		/// <param name="rotationOrder">
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/param[@name='rotationOrder']"
		/// />
		/// </param>
		/// <param name="invMassScaleA">
		/// The inverse mass scale of this <see cref="Entity"/> to multiply.
		/// </param>
		/// <param name="invMassScaleB">
		/// The inverse mass scale of the other <see cref="Entity"/> to multiply.
		/// </param>
		/// <remarks>
		/// <inheritdoc
		/// cref="AttachBoneToBonePhysically"
		/// path="/remarks"
		/// />
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if called in the game versions earlier than v1.0.2944.0.
		/// </exception>
		public void AttachBoneToBonePhysicallyOverrideInverseMass(EntityBone boneOfThisEntity,
			EntityBone boneOfSecondEntity, Vector3 secondEntityOffset, Vector3 thisEntityOffset, Vector3 rotation,
			float physicalStrength, bool constrainRotation, bool doInitialWarp = true, bool collideWithEntity = false,
			bool addInitialSeparation = true, EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ,
			float invMassScaleA = 1f, float invMassScaleB = 1f)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_2944_0, nameof(Entity),
				nameof(AttachBoneToBonePhysicallyOverrideInverseMass));

			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY_OVERRIDE_INVERSE_MASS, Handle,
				boneOfSecondEntity.Owner, boneOfThisEntity.Index, boneOfSecondEntity.Index, secondEntityOffset.X,
				secondEntityOffset.Y, secondEntityOffset.Z, thisEntityOffset.X, thisEntityOffset.Y, thisEntityOffset.Z,
				rotation.X, rotation.Y, rotation.Z, physicalStrength, constrainRotation, doInitialWarp,
				collideWithEntity, addInitialSeparation, (int)rotationOrder, invMassScaleA, invMassScaleB);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is attached to any other <see cref="Entity"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true" /> if this <see cref="Entity"/> is attached to another <see cref="Entity"/>; otherwise, <see langword="false" />.
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
		/// Determines whether this <see cref="Entity"/> is attached to a <see cref="Prop"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true" /> if this <see cref="Entity"/> is attached to a <see cref="Prop"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsAttachedToAnyProp() => Function.Call<bool>(Hash.IS_ENTITY_ATTACHED_TO_ANY_OBJECT, Handle);
		/// <summary>
		/// Determines whether this <see cref="Entity"/> is attached to a <see cref="Ped"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true" /> if this <see cref="Entity"/> is attached to a <see cref="Ped"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsAttachedToAnyPed() => Function.Call<bool>(Hash.IS_ENTITY_ATTACHED_TO_ANY_PED, Handle);
		/// <summary>
		/// Determines whether this <see cref="Entity"/> is attached to a <see cref="Vehicle"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true" /> if this <see cref="Entity"/> is attached to a <see cref="Vehicle"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsAttachedToAnyVehicle() => Function.Call<bool>(Hash.IS_ENTITY_ATTACHED_TO_ANY_VEHICLE, Handle);

		/// <summary>
		/// Gets the <see cref="Entity"/> this <see cref="Entity"/> is attached to.
		/// <remarks>Returns <see langword="null" /> if this <see cref="Entity"/> isn't attached to any entity.</remarks>
		/// </summary>
		public Entity AttachedEntity => FromHandle(Function.Call<int>(Hash.GET_ENTITY_ATTACHED_TO, Handle));

		/// <summary>
		/// Updates an <see cref="Entity"/>'s attachments immediately that is attached to something.
		/// Updates the position of an attached <see cref="Entity"/> (And all of its children) immediately,
		/// so that up to date entity and child entity positions can be grabbed.
		/// </summary>
		/// <remarks>
		/// Also updates the position of the <see cref="Entity"/> within a synchronized scene.
		/// </remarks>
		public void ProcessEntityAttachments() => Function.Call<bool>(Hash.PROCESS_ENTITY_ATTACHMENTS, Handle);

		#endregion

		#region Physics

		/// <summary>
		/// Gets a value that indicates whether this <see cref="Entity"/> has physics.
		/// Before calling physics methods or properties such as <see cref="set_Velocity"/>, you have to check that the entity has physics.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if this <see cref="Entity"/> has physics; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>
		/// Physics are streamed in separately from the drawable object, though <see cref="Entity"/> physics near the player are streamed.
		/// </remarks>
		public bool HasPhysics => Function.Call<bool>(Hash.DOES_ENTITY_HAVE_PHYSICS, Handle);
		/// <summary>
		/// Activates the physics of this <see cref="Entity"/>.
		/// </summary>
		/// <seealso cref="HasPhysics"/>
		public void ActivatePhysics() => Function.Call(Hash.ACTIVATE_PHYSICS, Handle);

		/// <summary>
		/// Sets a <see cref="Entity"/> damping.
		/// </summary>
		public void SetDamping(PhysicsDampingType dampingType, float dampingValue) => Function.Call(Hash.SET_DAMPING, Handle, (int)dampingType, dampingValue);

		/// <summary>
		/// Gets or sets the center of gravity offset.
		/// </summary>
		public Vector3 CenterOfGravityOffset
		{
			get => Function.Call<Vector3>(Hash.GET_CGOFFSET, Handle);
			set => Function.Call(Hash.SET_CGOFFSET, Handle, value.X, value.Y, value.Z);
		}
		/// <summary>
		/// Sets center of gravity at bound center.
		/// </summary>
		public void SetCenterOfGravityAtBoundCenter() => Function.Call(Hash.SET_CG_AT_BOUNDCENTER, Handle);

		/// <summary>
		/// Applies a force to this <see cref="Entity"/>.
		/// </summary>
		/// <param name="direction">The direction to apply the force relative to world coordinates.</param>
		/// <param name="rotation">
		/// <para>The offset from the root component of this <see cref="Entity"/> where the force applies.</para>
		/// <para>Although "rotation" is an incorrectly named parameter, the name is retained for scripts that use the method with named parameters.</para>
		/// </param>
		/// <param name="forceType">Type of the force to apply.</param>
		public void ApplyForce(Vector3 direction, Vector3 rotation = default, ForceType forceType = ForceType.ExternalImpulse)
		{
			Function.Call(Hash.APPLY_FORCE_TO_ENTITY, Handle, (int)forceType, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, false, true, true, false, true);
		}
		/// <summary>
		/// Applies a force to this <see cref="Entity"/>.
		/// </summary>
		/// <param name="direction">The direction to apply the force relative to this <see cref="Entity"/>s rotation</param>
		/// <param name="rotation">The offset from the root component of this <see cref="Entity"/> where the force applies. "rotation" is incorrectly named parameter but is left for scripts that use the method with named parameters.</param>
		/// <param name="forceType">Type of the force to apply.</param>
		public void ApplyForceRelative(Vector3 direction, Vector3 rotation = default, ForceType forceType = ForceType.ExternalImpulse)
		{
			Function.Call(Hash.APPLY_FORCE_TO_ENTITY, Handle, (int)forceType, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, true, true, true, false, true);
		}
		/// <summary>
		/// Applies a world force to this <see cref="Entity"/> using world offset.
		/// </summary>
		/// <inheritdoc cref="ApplyForceInternal(Vector3, Vector3, ForceType, bool, bool, bool, bool, bool)"/>
		public void ApplyWorldForceWorldOffset(Vector3 force, Vector3 offset, ForceType forceType, bool scaleByMass, bool triggerAudio = false, bool scaleByTimeScale = true)
		{
			ApplyForceInternal(force, offset, forceType, false, false, scaleByMass, triggerAudio, scaleByTimeScale);
		}
		/// <summary>
		/// Applies a world force to this <see cref="Entity"/> using relative offset.
		/// </summary>
		/// <inheritdoc cref="ApplyForceInternal(Vector3, Vector3, ForceType, bool, bool, bool, bool, bool)"/>
		public void ApplyWorldForceRelativeOffset(Vector3 force, Vector3 offset, ForceType forceType, bool scaleByMass, bool triggerAudio = false, bool scaleByTimeScale = true)
		{
			ApplyForceInternal(force, offset, forceType, false, true, scaleByMass, triggerAudio, scaleByTimeScale);
		}
		/// <summary>
		/// Applies a relative force to this <see cref="Entity"/> using world offset.
		/// </summary>
		/// <inheritdoc cref="ApplyForceInternal(Vector3, Vector3, ForceType, bool, bool, bool, bool, bool)"/>
		public void ApplyRelativeForceWorldOffset(Vector3 force, Vector3 offset, ForceType forceType, bool scaleByMass, bool triggerAudio = false, bool scaleByTimeScale = true)
		{
			ApplyForceInternal(force, offset, forceType, true, false, scaleByMass, triggerAudio, scaleByTimeScale);
		}
		/// <summary>
		/// Applies a relative force to this <see cref="Entity"/> using relative offset.
		/// </summary>
		/// <inheritdoc cref="ApplyForceInternal(Vector3, Vector3, ForceType, bool, bool, bool, bool, bool)"/>
		public void ApplyRelativeForceRelativeOffset(Vector3 force, Vector3 offset, ForceType forceType, bool scaleByMass, bool triggerAudio = false, bool scaleByTimeScale = true)
		{
			ApplyForceInternal(force, offset, forceType, true, true, scaleByMass, triggerAudio, scaleByTimeScale);
		}
		/// <summary>
		/// Applies a force to this <see cref="Entity"/>.
		/// </summary>
		/// <param name="force">The force to be applied.</param>
		/// <param name="offset">The offset from center of entity at which to apply force.</param>
		/// <param name="forceType">Type of the force to apply.</param>
		/// <param name="relativeForce">
		/// Specifies whether the force vector passed in is in relative or world coordinates.
		/// Local coordinates (<see langword="true"/>) means the force will get automatically transformed into world space before being applied.
		/// </param>
		/// <param name="relativeOffset">Specifies whether the offset passed in is in relative or world coordinates.</param>
		/// <param name="scaleByMass">
		/// <para>Specifies whether to scale the force by mass.</para>
		/// <para>If <see langword="true"/>, force will be multiplied by mass. For example, force passed in is in fact an acceleration rate in <c>m/s*s</c> (force) or velocity change in <c>m/s</c> (impulse).</para>
		/// <para>If <see langword="false"/>, force will be applied directly and it's effect will depend on the mass of the entity. For example, force passed in is a proper force in Newtons (force) or a step change in momentum <c>kg*m/s</c> (impulse).</para>
		/// <para>
		/// In other words, scaling by mass is probably easier in most situations -
		/// if the mass of the object changes it's behaviour shouldn't, and it's easier to picture the effect because an acceleration rate of <c>10.0</c> is approximately the same as gravity (<c>9.81</c> to be more precise).
		/// </para>
		/// </param>
		/// <param name="triggerAudio">
		/// <para>Specifies whether to play audio events related to the force being applied. The sound will play only if the entity type is <see cref="Vehicle"/> and will play a suspension squeal depending on the magnitude of the force.</para>
		/// <para>The sound will play even if regardless of <see cref="ForceType"/> (even with a value other than between 0 to 5).</para>
		/// </param>
		/// <param name="scaleByTimeScale">
		/// <para>Specifies whether scale the force by the current time scale (max: <c>1.0f</c>).</para>
		///	<para>Only affects when <paramref name="forceType"/> is <see cref="ForceType.InternalImpulse"/> or <see cref="ForceType.ExternalImpulse"/>.</para>
		/// </param>
		private void ApplyForceInternal(Vector3 force, Vector3 offset, ForceType forceType, bool relativeForce, bool relativeOffset, bool scaleByMass, bool triggerAudio = false, bool scaleByTimeScale = true)
		{
			// 9th parameter is component index (not bone index), which matters only if the entity is a ped
			Function.Call(Hash.APPLY_FORCE_TO_ENTITY, Handle, (int)forceType, force.X, force.Y, force.Z, offset.X, offset.Y, offset.Z, 0, relativeForce, relativeOffset, scaleByMass, triggerAudio, scaleByTimeScale);
		}

		/// <summary>
		/// Applies a world force to the center of mass of this <see cref="Entity"/>.
		/// <paramref name="forceType"/> must not be <see cref="ForceType.ExternalForce"/> or <see cref="ForceType.ExternalImpulse"/>.
		/// </summary>
		/// <inheritdoc cref="ApplyForceCenterOfMassInternal(Vector3, ForceType, bool, bool, bool)"/>
		public void ApplyWorldForceCenterOfMass(Vector3 force, ForceType forceType, bool scaleByMass, bool applyToChildren = false)
		{
			ApplyForceCenterOfMassInternal(force, forceType, false, scaleByMass, applyToChildren);
		}
		/// <summary>
		/// Applies a relative force to the center of mass of this <see cref="Entity"/>.
		/// <paramref name="forceType"/> must not be <see cref="ForceType.ExternalForce"/> or <see cref="ForceType.ExternalImpulse"/>.
		/// </summary>
		/// <inheritdoc cref="ApplyForceCenterOfMassInternal(Vector3, ForceType, bool, bool, bool)"/>
		public void ApplyRelativeForceCenterOfMass(Vector3 force, ForceType forceType, bool scaleByMass, bool applyToChildren = false)
		{
			ApplyForceCenterOfMassInternal(force, forceType, true, scaleByMass, applyToChildren);
		}
		/// <summary>
		/// Applies a force to the center of mass of this <see cref="Entity"/>.
		/// <paramref name="forceType"/> must not be <see cref="ForceType.ExternalForce"/> or <see cref="ForceType.ExternalImpulse"/>.
		/// </summary>
		/// <param name="force">The force to be applied.</param>
		/// <param name="forceType">Type of the force to apply.</param>
		/// <param name="relativeForce">
		/// Specifies whether the force vector passed in is in relative or world coordinates.
		/// Relative coordinates (<see langword="true"/>) means the force will get automatically transformed into world space before being applied.
		/// </param>
		/// <param name="scaleByMass">
		/// <para>Specifies whether to scale the force by mass.</para>
		/// <para>If <see langword="true"/>, force will be multiplied by mass. For example, force passed in is in fact an acceleration rate in <c>m/s*s</c> (force) or velocity change in <c>m/s</c> (impulse).</para>
		/// <para>If <see langword="false"/>, force will be applied directly and it's effect will depend on the mass of the entity. For example, force passed in is a proper force in Newtons (force) or a step change in momentum <c>kg*m/s</c> (impulse).</para>
		/// <para>
		/// In other words, scaling by mass is probably easier in most situations -
		/// if the mass of the object changes it's behaviour shouldn't, and it's easier to picture the effect because an acceleration rate of <c>10.0</c> is approximately the same as gravity (<c>9.81</c> to be more precise).
		/// </para>
		/// </param>
		/// <param name="applyToChildren">Specifies whether to apply force to children components as well as the specified component.</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="forceType"/> is set to <see cref="ForceType.ExternalForce"/> or <see cref="ForceType.ExternalImpulse"/>, which is not supported by this method.</exception>
		private void ApplyForceCenterOfMassInternal(Vector3 force, ForceType forceType, bool relativeForce, bool scaleByMass, bool applyToChildren = false)
		{
			// The native won't apply the force if apply force type is one of the external types
			if (forceType is ForceType.ExternalForce or ForceType.ExternalImpulse)
			{
				throw new ArgumentException("ForceType.ExternalForce and ForceType.ExternalImpulse are not supported.", nameof(forceType));
			}

			// 6th parameter is component index (not bone index), which matters only if the entity is a ped
			Function.Call(Hash.APPLY_FORCE_TO_ENTITY_CENTER_OF_MASS, Handle, (int)forceType, force.X, force.Y, force.Z, 0, relativeForce, scaleByMass, applyToChildren);
		}

		#endregion

		#region Animation

		/// <summary>
		/// Plays an <see cref="Entity"/> animation.
		/// </summary>
		/// <param name="animName">The animation name.</param>
		/// <param name="clipDictName">The clip/animation dictionary name.</param>
		/// <param name="blendDelta">The blend delta.</param>
		/// <param name="loop">Specifies whether the animation should loop.</param>
		/// <param name="holdLastFrame">Specifies whether the <see cref="Entity"/> should hold on the last frame.</param>
		/// <param name="driveToPose">
		/// Enable drive-to-pose for the animation if <see langword="true"/> (object must be set up with articulation for this to work).
		/// </param>
		/// <param name="startPhase">The start phase between 0f to 1f.</param>
		/// <param name="animFlags">The animation flags.</param>
		/// <remarks>For <see cref="Ped"/>, use <see cref="TaskInvoker.PlayAnimation(ClipDictionary, string)"/>.</remarks>
		/// <returns><see langword="true"/> if the animation has successfully started playing; otherwise, <see langword="false"/></returns>
		public bool PlayAnimation(string animName, ClipDictionary clipDictName, AnimationBlendDelta blendDelta, bool loop, bool holdLastFrame, bool driveToPose = false, float startPhase = 0f, AnimationFlags animFlags = AnimationFlags.None)
		{
			return Function.Call<bool>(Hash.PLAY_ENTITY_ANIM, Handle, animName, clipDictName, blendDelta.Value, loop, holdLastFrame, driveToPose, startPhase, (int)animFlags);
		}

		/// <summary>
		/// Stops an <see cref="Entity"/> animation.
		/// </summary>
		/// <returns><see langword="true"/> if the animation has successfully stopped playing; otherwise, <see langword="false"/></returns>
		public bool StopAnimation(string animName, ClipDictionary clipDictName, AnimationBlendDelta blendDelta)
		{
			return Function.Call<bool>(Hash.STOP_ENTITY_ANIM, Handle, animName, clipDictName, blendDelta.Value);
		}

		/// <summary>
		/// Gets a value that indicates whether this <see cref="Entity"/> is playing the animation.
		/// </summary>
		public bool IsPlayingAnimation(ClipDictionary clipDictName, string animName, EntityAnimationType type = EntityAnimationType.Default)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Handle, clipDictName, animName, (int)type);
		}

		/// <summary>
		/// Gets a value that indicates whether the animation specified is being held on the last frame for this <see cref="Entity"/>.
		/// </summary>
		/// <remarks>
		/// Will only ever return <see langword="true"/> for anims that hold at the end (i.e. anims that loop or end automatically will always return <see langword="false"/>).
		/// </remarks>
		public bool HasFinishedAnimation(ClipDictionary clipDictName, string animName, EntityAnimationType type = EntityAnimationType.Default)
		{
			return Function.Call<bool>(Hash.HAS_ENTITY_ANIM_FINISHED, Handle, clipDictName, animName, (int)type);
		}

		/// <summary>
		/// Gets a value that indicates whether the animation event has been fired from an animation this <see cref="Entity"/> is playing.
		/// Use this to check if a particular event tag is present in an animation playing on the <see cref="Entity"/> this frame.
		///	Some events are instantaneous (so this will only return true once).
		///	Others may have duration, which means that this function may continuously return <see langword="true"/> for a range of values.
		/// </summary>
		/// <param name="eventHash">The event hash.
		/// Use <see cref="Game.GenerateHash(string)"/> or <see cref="Game.GenerateHashAscii(string)"/> to convert the original event name to the hash.
		/// </param>
		/// <remarks>
		/// The event must have been tagged with the <c>VisibleToScript</c> attribute (joaat hash: <c>0xF301E135</c>) in the ycd animation file to make it detectable with this method.
		/// Events can include one or more attributes of different types that can be used to get data from the animation.
		/// </remarks>
		public bool HasAnimationEventFired(int eventHash) => Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, Handle, eventHash);

		/// <summary>
		/// Gets a float value representing animation's current playtime with respect to its total playtime.
		/// This value increasing in a range from [0.0 to 1.0] and wrap back to 0.0 when it reach 1.0.
		/// The phase of the anim is between 0.0 and 1.0 regardless of the anim length.
		/// </summary>
		public float GetAnimationCurrentTime(ClipDictionary clipDictName, string animName)
			=> Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, Handle, clipDictName, animName);

		/// <summary>
		/// Gets the total animation time in milliseconds.
		/// </summary>
		public float GetAnimationTotalTime(ClipDictionary clipDictName, string animName)
			=> Function.Call<float>(Hash.GET_ENTITY_ANIM_TOTAL_TIME, Handle, clipDictName, animName);

		/// <summary>
		/// Searches an animation for the start and end phase of an event.
		/// </summary>
		/// <param name="clipDictName">
		/// The clip/animation dictionary name.
		/// </param>
		/// <param name="animName">
		/// The animation clip name.
		/// </param>
		/// <param name="eventName">
		/// The event name. Will be converted to a joaat hash internally since event name is stored as a hash.
		/// </param>
		/// <param name="startPhase">
		/// If the event tag is found, it's start phase will be filled.
		/// </param>
		/// <param name="endPhase">
		/// If the event tag is found, it's end phase will be filled.
		/// </param>
		/// <returns><see langword="true"/> if this method found an event tag in an animation playing; otherwise, <see langword="false"/>.</returns>
		public bool FindAnimationEventPhase(ClipDictionary clipDictName, string animName, string eventName, out float startPhase, out float endPhase)
		{
			float startPhaseTemp, endPhaseTemp;

			unsafe
			{
				bool foundEventTag = Function.Call<bool>(Hash.HAS_ENTITY_ANIM_FINISHED, Handle, clipDictName, animName, eventName, &startPhaseTemp, &endPhaseTemp);

				startPhase = startPhaseTemp;
				endPhase = endPhaseTemp;
				return foundEventTag;
			}
		}

		#endregion

		/// <summary>
		/// Gets the current <see cref="InteriorProxy"/> associated with this <see cref="Entity"/>.
		/// </summary>
		/// <returns>
		/// The current <see cref="InteriorProxy"/> associated with this <see cref="Entity"/> if they are in an interior;
		/// otherwise, <see langword="null"/>
		/// </returns>
		public InteriorProxy CurrentInteriorProxy
		{
			get
			{
				int handle = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Handle);
				return handle != 0 ? new InteriorProxy(handle) : null;
			}
		}
		/// <summary>
		/// Gets a room key (name hash) from this <see cref="Entity"/> in that room.
		/// </summary>
		/// <returns>
		/// The name hash key of the room the <see cref="Entity"/> is in if it is in a <see cref="InteriorProxy"/>;
		/// otherwise, zero.
		/// </returns>
		/// <remarks>
		/// This method gets the name hash from the <see cref="InteriorInstance"/> of <see cref="InteriorProxy"/>
		/// associated with the <see cref="Entity"/>. The list of rooms are defined in a <c>CMloRoomDef</c> in a corresponding
		/// ytyp file, and this methods hashes the raw name before returning a value.
		/// </remarks>
		public int CurrentInteriorRoomKey => Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Handle);

		/// <summary>
		/// Marks this <see cref="Entity"/> as a mission entity.
		/// </summary>
		/// <param name="grabFromOtherScript">
		/// If <see langword="true"/>, this <see cref="Entity"/> will be grabbed off any script that currently owns it even if the current owner script is not one of SHVDN scripts.
		/// If <see langword="false"/>, this method won't do anything if the script that owns this <see cref="Entity"/> is not one of SHVDN scripts (e.g. a ysc script).
		/// </param>
		public void MarkAsMissionEntity(bool grabFromOtherScript = false)
		{
			// The 2nd parameter is only for multiplayer and we aren't interested in that mode (set to true in most SP scripts)
			Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, Handle, true, grabFromOtherScript);
		}

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
			// Do not set "Handle" property to the value of "handle" again, since the native will have set it to zero, but we still may want to use it otherwise
		}

		/// <summary>
		/// <para>
		/// Destroys this <see cref="Entity"/> and sets <see cref="PoolObject.Handle"/> to 0.
		/// If this <see cref="Entity"/> is <see cref="Vehicle"/>, the occupants will not be deleted but their tasks will be cleared.
		/// </para>
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

		/// <summary>
		/// Converts an <see cref="Entity"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(Entity value)
		{
			return new InputArgument((ulong)(value?.Handle ?? 0));
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
