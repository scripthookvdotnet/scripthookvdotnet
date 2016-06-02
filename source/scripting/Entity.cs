using System;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	public enum ForceType
	{
		MinForce,
		MaxForceRot,
		MinForce2,
		MaxForceRot2,
		ForceNoRot,
		ForceRotPlusForce
	}

	public abstract class Entity : IEquatable<Entity>, IHandleable, ISpatial
	{
		public Entity(int handle)
		{
			Handle = handle;
		}

		public int Handle { get; private set; }
		public IntPtr MemoryAddress
		{
			get
			{
				return MemoryAccess.GetEntityAddress(Handle);
			}
		}

		public int Health
		{
			get
			{
				return Function.Call<int>(Hash.GET_ENTITY_HEALTH, Handle) - 100;
			}
			set
			{
				Function.Call(Hash.SET_ENTITY_HEALTH, Handle, value + 100);
			}
		}
		public virtual int MaxHealth
		{
			get
			{
				return Function.Call<int>(Hash.GET_ENTITY_MAX_HEALTH, Handle) - 100;
			}
			set
			{
				Function.Call(Hash.SET_ENTITY_MAX_HEALTH, Handle, value + 100);
			}
		}
		public bool IsDead
		{
			get
			{
				return Function.Call<bool>(Hash.IS_ENTITY_DEAD, Handle);
			}
		}
		public bool IsAlive
		{
			get
			{
				return !IsDead;
			}
		}

		public Model Model
		{
			get
			{
				return new Model(Function.Call<int>(Hash.GET_ENTITY_MODEL, Handle));
			}
		}

		public virtual Vector3 Position
		{
			get
			{
				return Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, Handle, 0);
			}
			set
			{
				Function.Call(Hash.SET_ENTITY_COORDS, Handle, value.X, value.Y, value.Z, 0, 0, 0, 1);
			}
		}
		public Vector3 PositionNoOffset
		{
			set
			{
				Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, Handle, value.X, value.Y, value.Z, 1, 1, 1);
			}
		}
		public virtual Vector3 Rotation
		{
			get
			{
				return Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION, Handle, 0);
			}
			set
			{
				Function.Call(Hash.SET_ENTITY_ROTATION, Handle, value.X, value.Y, value.Z, 2, 1);
			}
		}
		public Quaternion Quaternion
		{
			get
			{
				var x = new OutputArgument();
				var y = new OutputArgument();
				var z = new OutputArgument();
				var w = new OutputArgument();
				Function.Call(Hash.GET_ENTITY_QUATERNION, Handle, x, y, z, w);

				return new Quaternion(x.GetResult<float>(), y.GetResult<float>(), z.GetResult<float>(), w.GetResult<float>());
			}
			set
			{
				Function.Call(Hash.SET_ENTITY_QUATERNION, Handle, value.X, value.Y, value.Z, value.W);
			}
		}
		public float Heading
		{
			get
			{
				return Function.Call<float>(Hash.GET_ENTITY_HEADING, Handle);
			}
			set
			{
				Function.Call<float>(Hash.SET_ENTITY_HEADING, Handle, value);
			}
		}
		public Vector3 UpVector
		{
			get
			{
				return Vector3.Cross(RightVector, ForwardVector);
			}
		}
		public Vector3 RightVector
		{
			get
			{
				Vector3 rotation = Rotation;
				const double D2R = 0.01745329251994329576923690768489;
				double s = System.Math.Cos(rotation.Y * D2R);
				double x = s * System.Math.Cos(-rotation.Z * D2R);
				double y = s * System.Math.Sin(rotation.Z * D2R);
				double z = System.Math.Sin(-rotation.Y * D2R);
				return new Vector3((float)x, (float)y, (float)z);
			}
		}
		public Vector3 ForwardVector
		{
			get
			{
				return Function.Call<Vector3>(Hash.GET_ENTITY_FORWARD_VECTOR, Handle);
			}
		}

		public bool FreezePosition
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return false;
				}

				return (MemoryAccess.ReadByte(MemoryAddress + 0x2E) & (1 << 1)) != 0;
			}
			set
			{
				Function.Call(Hash.FREEZE_ENTITY_POSITION, Handle, value);
			}
		}

		public Vector3 Velocity
		{
			get
			{
				return Function.Call<Vector3>(Hash.GET_ENTITY_VELOCITY, Handle);
			}
			set
			{
				Function.Call(Hash.SET_ENTITY_VELOCITY, Handle, value.X, value.Y, value.Z);
			}
		}
		public float MaxSpeed
		{
			set
			{
				Function.Call(Hash.SET_ENTITY_MAX_SPEED, Handle, value);
			}
		}

		public bool HasGravity
		{
			set
			{
				Function.Call(Hash.SET_ENTITY_HAS_GRAVITY, Handle, value);
			}
		}
		public float HeightAboveGround
		{
			get
			{
				return Function.Call<float>(Hash.GET_ENTITY_HEIGHT_ABOVE_GROUND, Handle);
			}
		}

		public int LodDistance
		{
			get
			{
				return Function.Call<int>(Hash.GET_ENTITY_LOD_DIST, Handle);
			}
			set
			{
				Function.Call(Hash.SET_ENTITY_LOD_DIST, Handle, value);
			}
		}

		public bool IsVisible
		{
			get
			{
				return Function.Call<bool>(Hash.IS_ENTITY_VISIBLE, Handle);
			}
			set
			{
				Function.Call(Hash.SET_ENTITY_VISIBLE, Handle, value);
			}
		}
		public bool IsOccluded
		{
			get
			{
				return Function.Call<bool>(Hash.IS_ENTITY_OCCLUDED, Handle);
			}
		}
		public bool IsOnScreen
		{
			get
			{
				return Function.Call<bool>(Hash.IS_ENTITY_ON_SCREEN, Handle);
			}
		}
		public bool IsUpright
		{
			get
			{
				return Function.Call<bool>(Hash.IS_ENTITY_UPRIGHT, Handle);
			}
		}
		public bool IsUpsideDown
		{
			get
			{
				return Function.Call<bool>(Hash.IS_ENTITY_UPSIDEDOWN, Handle);
			}
		}

		public bool IsInAir
		{

			get
			{
				return Function.Call<bool>(Hash.IS_ENTITY_IN_AIR, Handle);
			}
		}
		public bool IsInWater
		{

			get
			{
				return Function.Call<bool>(Hash.IS_ENTITY_IN_WATER, Handle);
			}
		}

		public bool IsPersistent
		{
			get
			{
				return Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, Handle);
			}
			set
			{
				if (value)
				{
					Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, Handle, true, false);
				}
				else
				{
					MarkAsNoLongerNeeded();
				}
			}
		}

		public bool IsOnFire
		{
			get
			{
				return Function.Call<bool>(Hash.IS_ENTITY_ON_FIRE, Handle);
			}
		}
		public bool IsFireProof
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return false;
				}

				return (MemoryAccess.ReadInt(MemoryAddress + 392) & (1 << 5)) != 0;
			}
			set
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return;
				}

				IntPtr address = MemoryAddress + 392;
				const int mask = 1 << 5;

				if (value)
				{
					MemoryAccess.WriteInt(address, MemoryAccess.ReadInt(address) | mask);
				}
				else
				{
					MemoryAccess.WriteInt(address, MemoryAccess.ReadInt(address) & ~mask);
				}
			}
		}
		public bool IsMeleeProof
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return false;
				}

				return (MemoryAccess.ReadInt(MemoryAddress + 392) & (1 << 7)) != 0;
			}
			set
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return;
				}

				IntPtr address = MemoryAddress + 392;
				const int mask = 1 << 7;

				if (value)
				{
					MemoryAccess.WriteInt(address, MemoryAccess.ReadInt(address) | mask);
				}
				else
				{
					MemoryAccess.WriteInt(address, MemoryAccess.ReadInt(address) & ~mask);
				}
			}
		}
		public bool IsBulletProof
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return false;
				}

				return (MemoryAccess.ReadInt(MemoryAddress + 392) & (1 << 4)) != 0;
			}
			set
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return;
				}

				IntPtr address = MemoryAddress + 392;
				const int mask = 1 << 4;

				if (value)
				{
					MemoryAccess.WriteInt(address, MemoryAccess.ReadInt(address) | mask);
				}
				else
				{
					MemoryAccess.WriteInt(address, MemoryAccess.ReadInt(address) & ~mask);
				}
			}
		}
		public bool IsExplosionProof
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return false;
				}

				return (MemoryAccess.ReadInt(MemoryAddress + 392) & (1 << 11)) != 0;
			}
			set
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return;
				}

				IntPtr address = MemoryAddress + 392;
				const int mask = 1 << 11;

				if (value)
				{
					MemoryAccess.WriteInt(address, MemoryAccess.ReadInt(address) | mask);
				}
				else
				{
					MemoryAccess.WriteInt(address, MemoryAccess.ReadInt(address) & ~mask);
				}
			}
		}
		public bool IsCollisionProof
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return false;
				}

				return (MemoryAccess.ReadInt(MemoryAddress + 392) & (1 << 6)) != 0;
			}
			set
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return;
				}

				IntPtr address = MemoryAddress + 392;
				const int mask = 1 << 6;

				if (value)
				{
					MemoryAccess.WriteInt(address, MemoryAccess.ReadInt(address) | mask);
				}
				else
				{
					MemoryAccess.WriteInt(address, MemoryAccess.ReadInt(address) & ~mask);
				}
			}
		}
		public bool IsInvincible
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return false;
				}

				return (MemoryAccess.ReadInt(MemoryAddress + 392) & (1 << 8)) != 0;
			}
			set
			{
				Function.Call(Hash.SET_ENTITY_INVINCIBLE, Handle, value);
			}
		}
		public bool IsOnlyDamagedByPlayer
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return false;
				}

				return (MemoryAccess.ReadInt(MemoryAddress + 392) & (1 << 9)) != 0;
			}
			set
			{
				Function.Call(Hash.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER, Handle, value);
			}
		}

		public int Alpha
		{
			get
			{
				return Function.Call<int>(Hash.GET_ENTITY_ALPHA, Handle);
			}
			set
			{
				Function.Call(Hash.SET_ENTITY_ALPHA, Handle, value, false);
			}
		}
		public void ResetAlpha()
		{
			Function.Call(Hash.RESET_ENTITY_ALPHA, Handle);
		}

		public bool HasCollided
		{
			get
			{
				return Function.Call<bool>(Hash.HAS_ENTITY_COLLIDED_WITH_ANYTHING, Handle);
			}
		}
		public bool HasCollision
		{
			get
			{
				if (MemoryAddress == IntPtr.Zero)
				{
					return false;
				}

				return (MemoryAccess.ReadByte(MemoryAddress + 0x29) & (1 << 1)) != 0;
			}
			set
			{
				Function.Call(Hash.SET_ENTITY_COLLISION, Handle, value, false);
			}
		}
		public void SetNoCollision(Entity entity, bool toggle)
		{
			Function.Call(Hash.SET_ENTITY_NO_COLLISION_ENTITY, Handle, entity.Handle, !toggle);
		}

		public bool HasBeenDamagedBy(Entity entity)
		{
			return Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY, Handle, entity.Handle, 1);
		}

		public bool IsInArea(Vector3 minBounds, Vector3 maxBounds)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_IN_AREA, Handle, minBounds.X, minBounds.Y, minBounds.Z, maxBounds.X, maxBounds.Y, maxBounds.Z);
		}
		public bool IsInAngledArea(Vector3 origin, Vector3 edge, float angle)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_IN_ANGLED_AREA, Handle, origin.X, origin.Y, origin.Z, edge.X, edge.Y, edge.Z, angle, false, true, false);
		}
		public bool IsInRangeOf(Vector3 position, float range)
		{
			return Vector3.Subtract(Position, position).LengthSquared() < range * range;
		}
		public bool IsNearEntity(Entity entity, Vector3 bounds)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_AT_ENTITY, Handle, entity.Handle, bounds.X, bounds.Y, bounds.Z, false, true, false);
		}
		public bool IsTouching(Model model)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_TOUCHING_MODEL, Handle, model.Hash);
		}
		public bool IsTouching(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_TOUCHING_ENTITY, Handle, entity.Handle);
		}

		public Vector3 GetOffsetInWorldCoords(Vector3 offset)
		{
			return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, Handle, offset.X, offset.Y, offset.Z);
		}
		public Vector3 GetOffsetFromWorldCoords(Vector3 worldCoords)
		{
			return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, Handle, worldCoords.X, worldCoords.Y, worldCoords.Z);
		}

		public int GetBoneIndex(string boneName)
		{
			return Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, Handle, boneName);
		}
		public Vector3 GetBoneCoord(int boneIndex)
		{
			return Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, Handle, boneIndex);
		}
		public Vector3 GetBoneCoord(string boneName)
		{
			return GetBoneCoord(GetBoneIndex(boneName));
		}
		public bool HasBone(string boneName)
		{
			return GetBoneIndex(boneName) != -1;
		}

		public Blip AddBlip()
		{
			return new Blip(Function.Call<int>(Hash.ADD_BLIP_FOR_ENTITY, Handle));
		}
		public Blip CurrentBlip
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

		public void AttachTo(Entity entity, int boneIndex)
		{
			AttachTo(entity, boneIndex, Vector3.Zero, Vector3.Zero);
		}
		public void AttachTo(Entity entity, int boneIndex, Vector3 position, Vector3 rotation)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Handle, entity.Handle, boneIndex, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 0, 0, 0, 0, 2, 1);
		}
		public void Detach()
		{
			Function.Call(Hash.DETACH_ENTITY, Handle, true, true);
		}
		public bool IsAttached()
		{
			return Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, Handle);
		}
		public bool IsAttachedTo(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_ATTACHED_TO_ENTITY, Handle, entity.Handle);
		}
		public Entity GetEntityAttachedTo()
		{
			int handle = Function.Call<int>(Hash.GET_ENTITY_ATTACHED_TO, Handle);

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

			return null;
		}

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
			ApplyForceRelative(direction, rotation, ForceType.MaxForceRot2);
		}
		public void ApplyForceRelative(Vector3 direction, Vector3 rotation, ForceType forceType)
		{
			Function.Call(Hash.APPLY_FORCE_TO_ENTITY, Handle, (int)forceType, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, true, true, true, false, true);
		}

		public void Delete()
		{
			Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, Handle, false, true);
			Function.Call(Hash.DELETE_ENTITY, new OutputArgument(Handle));
		}
		public void MarkAsNoLongerNeeded()
		{
			Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, Handle, false, true);
			Function.Call(Hash.SET_ENTITY_AS_NO_LONGER_NEEDED, new OutputArgument(Handle));
		}

		public bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_ENTITY_EXIST, Handle);
		}
		public static bool Exists(Entity entity)
		{
			return !ReferenceEquals(entity, null) && entity.Exists();
		}

		public bool Equals(Entity entity)
		{
			return !ReferenceEquals(entity, null) && Handle == entity.Handle;
		}
		public override bool Equals(object obj)
		{
			return !ReferenceEquals(obj, null) && obj.GetType() == GetType() && Equals((Entity)obj);
		}

		public override int GetHashCode()
		{
			return Handle;
		}

		public static bool operator ==(Entity left, Entity right)
		{
			return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);
		}
		public static bool operator !=(Entity left, Entity right)
		{
			return !(left == right);
		}
	}
}
