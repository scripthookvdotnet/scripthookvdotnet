//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public class EntityBone
	{
		internal EntityBone(Entity owner, int boneIndex)
		{
			Owner = owner;
			Index = boneIndex;
		}
		internal EntityBone(Entity owner, string boneName) : this(owner, Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, owner.NativeValue, boneName))
		{
		}

		/// <summary>
		/// Gets the bone index of this <see cref="EntityBone"/>.
		/// </summary>
		public int Index
		{
			get;
		}

		/// <summary>
		/// Gets the owner <see cref="Entity"/> this bone belongs to.
		/// </summary>
		public Entity Owner
		{
			get;
		}

		/// <summary>
		/// Determines if this <see cref="EntityBone"/> is valid.
		/// </summary>
		public bool IsValid => Owner.Exists() && Index != -1;

		/// <summary>
		/// Gets or sets the dynamic <see cref="Matrix"/> of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Matrix PoseMatrix
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBonePoseAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Matrix.Zero;
				}

				return new Matrix(SHVDN.NativeMemory.ReadMatrix(address));
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBonePoseAddress(Owner.Handle, Index);
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
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Matrix.Zero;
				}

				return new Matrix(SHVDN.NativeMemory.ReadMatrix(address));
			}
		}

		/// <summary>
		/// Gets or sets the current pose offset (dynamic position) of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 Pose
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBonePoseAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x30));
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBonePoseAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteVector3(address + 0x30, value.ToArray());
			}
		}

		/// <summary>
		/// Gets the position of this <see cref="EntityBone"/> in world coordinates.
		/// </summary>
		public Vector3 Position => Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, Owner.Handle, Index);

		/// <summary>
		/// Gets the position of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 RelativePosition
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x30));
			}
		}

		/// <summary>
		/// Gets the vector that points above this <see cref="EntityBone"/> relative to the world.
		/// </summary>
		public Vector3 UpVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Owner.UpVector;
				}
				return Owner.Matrix.TransformPoint(new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).TransformPoint(Vector3.RelativeTop)) - Position;
			}
		}

		/// <summary>
		/// Gets the vector that points to the right of this <see cref="EntityBone"/> relative to the world.
		/// </summary>
		public Vector3 RightVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Owner.RightVector;
				}
				return Owner.Matrix.TransformPoint(new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).TransformPoint(Vector3.RelativeRight)) - Position;
			}
		}

		/// <summary>
		/// Gets the vector that points in front of this <see cref="EntityBone"/> relative to the world.
		/// </summary>
		public Vector3 ForwardVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Owner.ForwardVector;
				}
				return Owner.Matrix.TransformPoint(new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).TransformPoint(Vector3.RelativeFront)) - Position;
			}
		}

		/// <summary>
		/// Gets the vector that points above this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 RelativeUpVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Vector3.RelativeTop;
				}
				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x20));
			}
		}

		/// <summary>
		/// Gets the vector that points to the right of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 RelativeRightVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Vector3.RelativeRight;
				}
				return new Vector3(SHVDN.NativeMemory.ReadVector3(address));
			}
		}

		/// <summary>
		/// Gets the vector that points in front of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		public Vector3 RelativeForwardVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Vector3.RelativeFront;
				}
				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x10));
			}
		}

		/// <summary>
		/// Gets the fragment group index of this <see cref="EntityBone"/>. -1 will be returned if the <see cref="Entity"/> does not exist or <see cref="Index"/> is invalid.
		/// </summary>
		public int FragmentGroupIndex
		{
			get
			{
				IntPtr address = Owner.MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return -1;
				}

				return SHVDN.NativeMemory.GetFragmentGroupIndexByEntityBoneIndex(address, Index);
			}
		}

		/// <summary>
		/// Gets the position in world coordinates of an offset relative this <see cref="EntityBone"/>
		/// </summary>
		/// <param name="offset">The offset from this <see cref="EntityBone"/>.</param>
		public Vector3 GetOffsetPosition(Vector3 offset)
		{
			IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(Owner.Handle, Index);
			if (address == IntPtr.Zero)
			{
				return Owner.Matrix.TransformPoint(offset);
			}
			return Owner.Matrix.TransformPoint(new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).TransformPoint(offset));
		}

		/// <summary>
		/// Gets the position relative to the <see cref="Entity"/> of an offset relative this <see cref="EntityBone"/>
		/// </summary>
		/// <param name="offset">The offset from this <see cref="EntityBone"/>.</param>
		public Vector3 GetRelativeOffsetPosition(Vector3 offset)
		{
			IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(Owner.Handle, Index);
			if (address == IntPtr.Zero)
			{
				return offset;
			}
			return new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).TransformPoint(offset);
		}

		/// <summary>
		/// Gets the relative offset of this <see cref="EntityBone"/> from a world coordinates position
		/// </summary>
		/// <param name="worldCoords">The world coordinates.</param>
		public Vector3 GetPositionOffset(Vector3 worldCoords)
		{
			IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(Owner.Handle, Index);
			if (address == IntPtr.Zero)
			{
				return Owner.Matrix.InverseTransformPoint(worldCoords);
			}
			return new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).InverseTransformPoint(Owner.Matrix.InverseTransformPoint(worldCoords));
		}

		/// <summary>
		/// Gets the relative offset of this <see cref="EntityBone"/> from an offset from the <see cref="Entity"/>
		/// </summary>
		/// <param name="entityOffset">The <see cref="Entity"/> offset.</param>
		public Vector3 GetRelativePositionOffset(Vector3 entityOffset)
		{
			IntPtr address = SHVDN.NativeMemory.GetEntityBoneMatrixAddress(Owner.Handle, Index);
			if (address == IntPtr.Zero)
			{
				return entityOffset;
			}
			return new Matrix(SHVDN.NativeMemory.ReadMatrix(address)).InverseTransformPoint(entityOffset);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same bone as this <see cref="EntityBone"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same bone as this <see cref="EntityBone"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is EntityBone bone)
			{
				return Owner == bone.Owner && Index == bone.Index;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="EntityBone"/>s refer to the same bone.
		/// </summary>
		/// <param name="left">The left <see cref="EntityBone"/>.</param>
		/// <param name="right">The right <see cref="EntityBone"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same bone as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(EntityBone left, EntityBone right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		/// <summary>
		/// Determines if two <see cref="EntityBone"/>s don't refer to the same bone.
		/// </summary>
		/// <param name="left">The left <see cref="EntityBone"/>.</param>
		/// <param name="right">The right <see cref="EntityBone"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same bone as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(EntityBone left, EntityBone right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Determines if an <see cref="EntityBone"/> refers to a specific bone.
		/// </summary>
		/// <param name="entityBone">The <see cref="EntityBone"/> to check.</param>
		/// <param name="boneId">The <see cref="Bone"/> ID to check against.</param>
		/// <returns><see langword="true" /> if <paramref name="entityBone"/> refers to the <paramref name="boneId"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(EntityBone entityBone, Bone boneId)
		{
			if (entityBone is null)
			{
				return false;
			}

			return entityBone.Owner is Ped ped && ped.Bones[boneId].Index == entityBone.Index;
		}
		/// <summary>
		/// Determines if an <see cref="EntityBone"/> doesn't refer to a specific bone.
		/// </summary>
		/// <param name="entityBone">The <see cref="EntityBone"/> to check.</param>
		/// <param name="boneId">The <see cref="Bone"/> ID to check against.</param>
		/// <returns><see langword="true" /> if <paramref name="entityBone"/> does not refer to the <paramref name="boneId"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(EntityBone entityBone, Bone boneId)
		{
			return !(entityBone == boneId);
		}

		/// <summary>
		/// Converts an <see cref="EntityBone"/> to a bone index.
		/// </summary>
		public static implicit operator int(EntityBone entityBone)
		{
			return entityBone is null ? -1 : entityBone.Index;
		}
		/// <summary>
		/// Converts an <see cref="EntityBone"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(EntityBone entityBone)
		{
			return new InputArgument((ulong)entityBone.Index);
		}

		public override int GetHashCode()
		{
			return Index.GetHashCode() ^ Owner.GetHashCode();
		}
	}
}
