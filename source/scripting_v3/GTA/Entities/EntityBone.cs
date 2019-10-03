using System;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	//public class ExtensionMethods
	//{
	//	public static implicit operator InputArgument(Entity e) => new InputArgument(e.NativeValue);
	//}

	public class EntityBone
	{
		#region Fields
		protected readonly Entity _owner;
		protected readonly int _index;
		#endregion

		internal EntityBone(Entity owner, int boneIndex)
		{
			_owner = owner;
			_index = boneIndex;
		}

		internal EntityBone(Entity owner, string boneName)
		{
			_owner = owner;
			_index = Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, owner.NativeValue, boneName);
		}

		/// <summary>
		/// Gets the bone index of this <see cref="EntityBone"/>.
		/// </summary>
		public int Index => _index;

		public Entity Owner => _owner;

		public static implicit operator int(EntityBone bone)
		{
			return ReferenceEquals(bone, null) ? -1 : bone.Index;
		}

		/// <summary>
		/// Gets the position of this <see cref="EntityBone"/> in world coords.
		/// </summary>
		public Vector3 Position => Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, _owner.Handle, _index);

		/// <summary>
		/// Gets or sets the current pose offset (dynamic position) of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 Pose
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBonePoseAddress(_owner.Handle, _index);
				if (address == IntPtr.Zero)
				{
					return new Vector3();
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x30));
			}

			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBonePoseAddress(_owner.Handle, _index);
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteVector3(address + 0x30, value.ToArray());
			}
		}

		/// <summary>
		/// Gets or sets the dynamic <see cref="Matrix"/> of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Matrix PoseMatrix
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBonePoseAddress(_owner.Handle, _index);
				if (address == IntPtr.Zero)
				{
					return new Matrix();
				}
				return new Matrix(SHVDN.NativeMemory.ReadMatrix(address));
			}

			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBonePoseAddress(_owner.Handle, _index);
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteMatrix(address, value.ToArray());
			}
		}

		/// <summary>
		/// Gets the <see cref="Matrix"/> of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Matrix RelativeMatrix
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if (address == IntPtr.Zero)
				{
					return new Matrix();
				}
				return new Matrix(SHVDN.NativeMemory.ReadMatrix(address));
			}
		}

		/// <summary>
		/// Gets the vector that points to the right of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 RelativeRightVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if(address == IntPtr.Zero)
				{
					return Vector3.RelativeRight;
				}
				return new Vector3(SHVDN.NativeMemory.ReadVector3(address));
			}
		}

		/// <summary>
		/// Gets the vector that points infront of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 RelativeForwardVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if(address == IntPtr.Zero)
				{
					return Vector3.RelativeFront;
				}
				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x10));
			}
		}

		/// <summary>
		/// Gets the vector that points above this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 RelativeUpVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if(address == IntPtr.Zero)
				{
					return Vector3.RelativeTop;
				}
				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x20));
			}
		}

		//Probably a nicer way to Get these Vectors but it works

		/// <summary>
		/// Gets the vector that points to the right of this <see cref="EntityBone"/> relative to the world.
		/// </summary>
		public Vector3 RightVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if(address == IntPtr.Zero)
				{
					return _owner.RightVector;
				}
				return _owner.Matrix.TransformPoint(new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).TransformPoint(Vector3.RelativeRight)) - Position;
			}
		}

		/// <summary>
		/// Gets the vector that points infront of this <see cref="EntityBone"/> relative to the world.
		/// </summary>
		public Vector3 ForwardVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if (address == IntPtr.Zero)
				{
					return _owner.ForwardVector;
				}
				return _owner.Matrix.TransformPoint(new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).TransformPoint(Vector3.RelativeFront)) - Position;
			}
		}

		/// <summary>
		/// Gets the vector that points above this <see cref="EntityBone"/> relative to the world.
		/// </summary>
		public Vector3 UpVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if(address == IntPtr.Zero)
				{
					return _owner.UpVector;
				}
				return _owner.Matrix.TransformPoint(new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).TransformPoint(Vector3.RelativeTop)) - Position;
			}
		}

		/// <summary>
		/// Gets the position of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 RelativePosition
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if(address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}
				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x30));
			}
		}

		/// <summary>
		/// Gets the position in world coords of an offset relative this <see cref="EntityBone"/>
		/// </summary>
		/// <param name="offset">The offset from this <see cref="EntityBone"/>.</param>
		public Vector3 GetOffsetPosition(Vector3 offset)
		{
			IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(_owner.Handle, _index);
			if(address == IntPtr.Zero)
			{
				return _owner.Matrix.TransformPoint(offset);
			}
			return _owner.Matrix.TransformPoint(new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).TransformPoint(offset));
		}

		/// <summary>
		/// Gets the position relative to the <see cref="Entity"/> of an offset relative this <see cref="EntityBone"/>
		/// </summary>
		/// <param name="offset">The offset from this <see cref="EntityBone"/>.</param>
		public Vector3 GetRelativeOffsetPosition(Vector3 offset)
		{
			IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(_owner.Handle, _index);
			if(address == IntPtr.Zero)
			{
				return offset;
			}
			return new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).TransformPoint(offset);
		}

		/// <summary>
		/// Gets the relative offset of this <see cref="EntityBone"/> from a world coords position
		/// </summary>
		/// <param name="worldCoords">The world coords.</param>
		public Vector3 GetPositionOffset(Vector3 worldCoords)
		{
			IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(_owner.Handle, _index);
			if(address == IntPtr.Zero)
			{
				return _owner.Matrix.InverseTransformPoint(worldCoords);
			}
			return new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).InverseTransformPoint(_owner.Matrix.InverseTransformPoint(worldCoords));
		}

		/// <summary>
		/// Gets the relative offset of this <see cref="EntityBone"/> from an offset from the <see cref="Entity"/>
		/// </summary>
		/// <param name="entityOffset">The <see cref="Entity"/> offset.</param>
		public Vector3 GetRelativePositionOffset(Vector3 entityOffset)
		{
			IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(_owner.Handle, _index);
			if(address == IntPtr.Zero)
			{
				return entityOffset;
			}
			return new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).InverseTransformPoint(entityOffset);
		}

		/// <summary>
		/// Determines if this <see cref="EntityBone"/> is valid
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="EntityBone"/> is valid; otherwise, <c>false</c>.
		/// </value>
		public bool IsValid => Entity.Exists(_owner) && _index != -1;

		/// <summary>
		/// Checks if two <see cref="EntityBone"/>s refer to the same <see cref="EntityBone"/>
		/// </summary>
		/// <param name="entityBone">The other <see cref="EntityBone"/>.</param>
		/// <returns><c>true</c> if they are the same bone of the same <see cref="Entity"/>; otherwise, false</returns>
		public bool Equals(EntityBone entityBone)
		{
			return !ReferenceEquals(entityBone, null) && _owner == entityBone._owner && Index == entityBone.Index;
		}
		public override bool Equals(object obj)
		{
			return !ReferenceEquals(obj, null) && obj.GetType() == GetType() && Equals((EntityBone)obj);
		}

		public static bool Exists(EntityBone entityBone)
		{
			return !ReferenceEquals(entityBone, null) && entityBone.IsValid;
		}

		public static bool operator ==(EntityBone left, EntityBone right)
		{
			return ReferenceEquals(left, null)
				? ReferenceEquals(right, null)
				: !ReferenceEquals(right, null) && left.Equals(right);
		}

		public static bool operator !=(EntityBone bone, EntityBone other)
		{
			return !(bone == other);
		}

		public static bool operator ==(EntityBone entityBone, Bone boneId)
		{
			return !ReferenceEquals(entityBone, null) && entityBone._owner is Ped && (entityBone._owner as Ped).Bones[boneId].Index == entityBone.Index;
		}

		public static bool operator !=(EntityBone entityBone, Bone boneId)
		{
			return !(entityBone == boneId);
		}

		public override int GetHashCode()
		{
			return Index.GetHashCode() ^ _owner.GetHashCode();
		}

		public static implicit operator InputArgument(EntityBone entityBone)
		{
			return new InputArgument(entityBone.Index);
		}
	}
}
