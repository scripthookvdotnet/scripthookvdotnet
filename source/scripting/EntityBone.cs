using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA.Math;
using GTA.Native;

namespace GTA
{
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
			_index = Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, owner, boneName);
		}

		/// <summary>
		/// Gets the bone index of this <see cref="EntityBone"/>.
		/// </summary>
		public int Index { get { return _index;} }

		public Entity Owner { get { return _owner; } }

		public static implicit operator int(EntityBone bone)
		{
			return ReferenceEquals(bone, null) ? -1 : bone.Index;
		}

		/// <summary>
		/// Gets the position of this <see cref="EntityBone"/> in world coords.
		/// </summary>
		public Vector3 Position
		{
			get
			{
				return Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, _owner.Handle, _index);
			}
		}

		/// <summary>
		/// Gets the <see cref="Matrix"/> of this <see cref="EntityBone"/> realtive to the <see cref="Entity"/> its part of.
		/// </summary>
		public Matrix RelativeMatrix
		{
			get
			{
				IntPtr address = MemoryAccess.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if (address == IntPtr.Zero)
				{
					return new Matrix();
				}
				return MemoryAccess.ReadMatrix(address);
			}
		}
		/// <summary>
		/// Gets the vector that points to the right of this <see cref="EntityBone"/> realtive to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 RelativeRightVector
		{
			get
			{
				IntPtr address = MemoryAccess.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if(address == IntPtr.Zero)
				{
					return Vector3.RelativeRight;
				}
				return MemoryAccess.ReadVector3(address);
			}
		}
		/// <summary>
		/// Gets the vector that points infront of this <see cref="EntityBone"/> realtive to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 RelativeForwardVector
		{
			get
			{
				IntPtr address = MemoryAccess.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if(address == IntPtr.Zero)
				{
					return Vector3.RelativeFront;
				}
				return MemoryAccess.ReadVector3(address + 0x10);
			}
		}
		/// <summary>
		/// Gets the vector that points above this <see cref="EntityBone"/> realtive to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 RelativeUpVector
		{
			get
			{
				IntPtr address = MemoryAccess.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if(address == IntPtr.Zero)
				{
					return Vector3.RelativeTop;
				}
				return MemoryAccess.ReadVector3(address + 0x20);
			}
		}

		/// <summary>
		/// Gets the position of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 RelativePosition
		{
			get
			{
				IntPtr address = MemoryAccess.GetEntityBoneMatrixAddress(_owner.Handle, _index);
				if(address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}
				return MemoryAccess.ReadVector3(address + 0x30);
			}
		}

		/// <summary>
		/// Gets the position in world coords of an offset relative this <see cref="EntityBone"/>
		/// </summary>
		/// <param name="offset">The offset from this <see cref="EntityBone"/>.</param>
		public Vector3 GetOffsetPosition(Vector3 offset)
		{
			IntPtr address = MemoryAccess.GetEntityBoneMatrixAddress(_owner.Handle, _index);
			if(address == IntPtr.Zero)
			{
				return _owner.Matrix.TransformPoint(offset);
			}
			return _owner.Matrix.TransformPoint(MemoryAccess.ReadMatrix(address).TransformPoint(offset));
		}

		/// <summary>
		/// Gets the position relative to the <see cref="Entity"/> of an offset relative this <see cref="EntityBone"/>
		/// </summary>
		/// <param name="offset">The offset from this <see cref="EntityBone"/>.</param>
		public Vector3 GetRelativeOffsetPosition(Vector3 offset)
		{
			IntPtr address = MemoryAccess.GetEntityBoneMatrixAddress(_owner.Handle, _index);
			if(address == IntPtr.Zero)
			{
				return offset;
			}
			return MemoryAccess.ReadMatrix(address).TransformPoint(offset);
		}

		/// <summary>
		/// Gets the relative offset of this <see cref="EntityBone"/> from a world coords position
		/// </summary>
		/// <param name="worldCoords">The world coords.</param>
		public Vector3 GetPositionOffset(Vector3 worldCoords)
		{
			IntPtr address = MemoryAccess.GetEntityBoneMatrixAddress(_owner.Handle, _index);
			if(address == IntPtr.Zero)
			{
				return _owner.Matrix.InverseTransformPoint(worldCoords);
			}
			return MemoryAccess.ReadMatrix(address).InverseTransformPoint(_owner.Matrix.InverseTransformPoint(worldCoords));
		}

		/// <summary>
		/// Gets the relative offset of this <see cref="EntityBone"/> from an offset from the <see cref="Entity"/>
		/// </summary>
		/// <param name="entityOffset">The <see cref="Entity"/> offset.</param>
		public Vector3 GetRelativePositionOffset(Vector3 entityOffset)
		{
			IntPtr address = MemoryAccess.GetEntityBoneMatrixAddress(_owner.Handle, _index);
			if(address == IntPtr.Zero)
			{
				return entityOffset;
			}
			return MemoryAccess.ReadMatrix(address).InverseTransformPoint(entityOffset);
		}

		/// <summary>
		/// Determines if this <see cref="EntityBone"/> is valid
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="EntityBone"/> is valid; otherwise, <c>false</c>.
		/// </value>
		public bool IsValid
		{
			get { return Entity.Exists(_owner) && _index != -1; }
		}

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

	public class PedBone : EntityBone
	{
		#region Fields
		private new readonly Ped _owner;
		#endregion

		public new Ped Owner
		{
			get { return _owner;}
		}

		internal PedBone(Ped owner, int boneIndex) : base(owner, boneIndex)
		{
			_owner = owner;
		}

		internal PedBone(Ped owner, string boneName) : base(owner, boneName)
		{
			_owner = owner;
		}

		internal PedBone(Ped owner, Bone boneId)
			: base(owner, Function.Call<int>(Hash.GET_PED_BONE_INDEX, owner.Handle, boneId))
		{
			_owner = owner;
		}

	    public new bool IsValid
	    {
	        get
	        {
	            return Ped.Exists(Owner) && Index != -1;
	        }
	    }

	}

	public class EntityBoneCollection
	{
		#region Fields
		protected readonly Entity _owner;
		#endregion
		internal EntityBoneCollection(Entity owner)
		{
			_owner = owner;
		}

		/// <summary>
		/// Gets the <see cref="EntityBone"/> with the specified bone name.
		/// </summary>
		/// <param name="boneName">Name of the bone.</param>
		public EntityBone this[string boneName]
		{
			get { return new EntityBone(_owner, boneName); }
		}

		/// <summary>
		/// Gets the <see cref="EntityBone"/> at the specified bone index.
		/// </summary>
		/// <param name="boneIndex">The bone index.</param>
		public EntityBone this[int boneIndex]
		{
			get { return new EntityBone(_owner, boneIndex); }
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> has a bone with the specified bone name
		/// </summary>
		/// <param name="boneName">Name of the bone.</param>
		/// <returns>
		///   <c>true</c> if this <see cref="Entity"/> has a bone with the specified bone name; otherwise, <c>false</c>.
		/// </returns>
		public bool HasBone(string boneName)
		{
			return Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, _owner.Handle, boneName) !=  -1;
		}

		/// <summary>
		/// Gets the number of bones that this <see cref="Entity"/> has.
		/// </summary>
		public int Count
		{
			get { return MemoryAccess.GetEntityBoneCount(_owner.Handle); }
		}

		public EntityBone Core
		{
			get { return new EntityBone(_owner, -1); }
		}

		public override int GetHashCode()
		{
			return _owner.GetHashCode() ^ Count.GetHashCode();
		}
	}

	public class PedBoneCollection : EntityBoneCollection
	{
		#region Fields
		private new readonly Ped _owner;
		#endregion
		internal PedBoneCollection(Ped owner) : base(owner)
		{
			_owner = owner;
		}

		/// <summary>
		/// Gets the <see cref="PedBone"/> with the specified bone name.
		/// </summary>
		/// <param name="boneName">Name of the bone.</param>
		public new PedBone this[string boneName]
		{
			get { return new PedBone(_owner, boneName); }
		}

		/// <summary>
		/// Gets the <see cref="PedBone"/> at the specified bone index.
		/// </summary>
		/// <param name="boneIndex">The bone index.</param>
		public new PedBone this[int boneIndex]
		{
			get { return new PedBone(_owner, boneIndex); }
		}

		/// <summary>
		/// Gets the <see cref="EntityBone"/> with the specified boneId.
		/// </summary>
		/// <param name="boneId">The boneId.</param>
		public PedBone this[Bone boneId]
		{
			get { return new PedBone(_owner, boneId); }
		}

	    public new PedBone Core
	    {
	        get { return new PedBone(_owner, -1); }
	    }

		/// <summary>
		/// Gets the last damaged Bone for this <see cref="Ped"/>.
		/// </summary>
		public PedBone LastDamaged
		{
			get
			{
			    int outBone;
				unsafe
				{
					if (Function.Call<bool>(Hash.GET_PED_LAST_DAMAGE_BONE, _owner.Handle, &outBone))
					{
						return this[(Bone) outBone];
					}
				}
				return this[Bone.SKEL_ROOT];
			}
		}

		/// <summary>
		/// Clears the last damage a bone on this <see cref="Ped"/> received.
		/// </summary>
		public void ClearLastDamaged()
		{
			Function.Call(Hash.CLEAR_PED_LAST_DAMAGE_BONE, _owner.Handle);
		}
	}
}
