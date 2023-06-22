//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
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

			if (boneIndex != -1)
			{
				Tag = SHVDN.NativeMemory.GetBoneIdForEntityBoneIndex(owner.Handle, boneIndex);
			}
			else
			{
				Tag = -1;
			}
		}
		// This overload is present to avoid redundant bone id fetching
		internal EntityBone(Entity owner, int boneIndex, int boneTag)
		{
			Owner = owner;
			Index = boneIndex;
			Tag = (boneIndex != -1) ? boneTag : -1;
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
		/// Gets the bone tag (identifier) of this <see cref="EntityBone"/>.
		/// If the bone does not exist, <c>-1</c> will be returned.
		/// </summary>
		public int Tag
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
		/// Gets the next sibling bone of this <see cref="EntityBone"/>.
		/// To check existence of the next sibling bone, you can use <see cref="EntityBone.Index"/> or <see cref="EntityBone.Tag"/>.
		/// </summary>
		public EntityBone NextSibling
		{
			get
			{
				SHVDN.NativeMemory.GetNextSiblingBoneIndexAndIdOfEntityBoneIndex(Owner.Handle, Index, out int boneIndex, out int boneTag);
				return new EntityBone(Owner, boneIndex, boneTag);
			}
		}

		/// <summary>
		/// Gets the parent bone of this <see cref="EntityBone"/>.
		/// To check existence of the next sibling bone, you can use <see cref="EntityBone.Index"/> or <see cref="EntityBone.Tag"/>.
		/// </summary>
		public EntityBone Parent
		{
			get
			{
				SHVDN.NativeMemory.GetParentBoneIndexAndIdOfEntityBoneIndex(Owner.Handle, Index, out int boneIndex, out int boneTag);
				return new EntityBone(Owner, boneIndex, boneTag);
			}
		}

		/// <summary>
		/// Gets the bone name of this <see cref="EntityBone"/>.
		/// If the bone does not exist, <see langword="null"/> will be returned.
		/// </summary>
		/// <remarks>
		/// This property return the bone name as registered in the yft file of corresponding model, but the hashed value of this string value may not match <see cref="Tag"/>.
		/// For example, <see cref="Ped"/>s have the bone in their skeletons whose name is <c>SKEL_Spine3</c> and whose ID is <c>24818</c>, which doesn't match the hashed value of <c>SKEL_Spine3</c> but matches that of <c>BONETAG_SPINE3</c>.
		/// </remarks>
		public string Name => SHVDN.NativeMemory.GetEntityBoneName(Owner.Handle, Index);

		/// <summary>
		/// Determines if this <see cref="EntityBone"/> is valid.
		/// </summary>
		public bool IsValid => Owner.Exists() && Index != -1;

		/// <summary>
		/// Gets or sets the dynamic <see cref="Matrix"/> of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		/// <remarks>
		/// While you can change appearance for <see cref="Vehicle"/>s by modifying this property,
		/// you cannot change appearance for <see cref="Ped"/>s or <see cref="Prop"/>s by modifying this property.
		/// </remarks>
		public Matrix PoseMatrix
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneObjectMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Matrix.Zero;
				}

				return new Matrix(SHVDN.NativeMemory.ReadMatrix(address));
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneObjectMatrixAddress(Owner.Handle, Index);
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
		/// <remarks>
		/// While you can change appearance for <see cref="Ped"/>s or <see cref="Prop"/>s by modifying this property,
		/// you cannot change appearance for <see cref="Vehicle"/>s by modifying this property.
		/// </remarks>
		public Matrix RelativeMatrix
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Matrix.Zero;
				}

				return new Matrix(SHVDN.NativeMemory.ReadMatrix(address));
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteMatrix(address, value.ToArray());
			}
		}

		/// <summary>
		/// Gets or sets the current pose offset (dynamic position) of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		/// <remarks>
		/// While you can change appearance for <see cref="Vehicle"/>s by modifying this property,
		/// you cannot change appearance for <see cref="Ped"/>s or <see cref="Prop"/>s by modifying this property.
		/// </remarks>
		public Vector3 Pose
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneObjectMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x30));
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneObjectMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteVector3(address + 0x30, value.ToInternalFVector3());
			}
		}

		/// <summary>
		/// Gets or sets the current pose quaternion of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		/// <remarks>
		/// <para>
		/// While you can change appearance for <see cref="Vehicle"/>s by modifying this property,
		/// you cannot change appearance for <see cref="Ped"/>s or <see cref="Prop"/>s by modifying this property.
		/// </para>
		/// <para>
		/// Remember to normalize the value you are going to set before setting it to this property as the setter does not normalize the value, just like <c>SET_ENTITY_QUATERNION</c> does not.
		/// Setting an unnormalized quaternion may result in unintended scale change if the <see cref="Entity"/> is not playing animations that affect this bone or not simulating physics for this bone.
		/// </para>
		/// </remarks>
		public Quaternion PoseQuaternion
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneObjectMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Quaternion.Zero;
				}

				unsafe
				{
					float* tempRotationArray = stackalloc float[4];
					SHVDN.NativeMemory.GetQuaternionFromMatrix(tempRotationArray, address);

					return new Quaternion(tempRotationArray[0], tempRotationArray[1], tempRotationArray[2], tempRotationArray[3]);
				}
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneObjectMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return;
				}

				var matrixToWrite = new Matrix(SHVDN.NativeMemory.ReadMatrix(address));
				Vector3 scale = matrixToWrite.GetScaleVector();
				var translation = new Vector3(matrixToWrite.M41, matrixToWrite.M42, matrixToWrite.M43);

				matrixToWrite = Matrix.Scaling(scale) * Matrix.RotationQuaternion(value) * Matrix.Translation(translation);
				SHVDN.NativeMemory.WriteMatrix(address, matrixToWrite.ToArray());
			}
		}

		/// <summary>
		/// Gets or sets the current pose rotation of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		/// <remarks>
		/// While you can change appearance for <see cref="Vehicle"/>s by modifying this property,
		/// you cannot change appearance for <see cref="Ped"/>s or <see cref="Prop"/>s by modifying this property.
		/// </remarks>
		public Vector3 PoseRotation
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneObjectMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				unsafe
				{
					float* tempRotationArray = stackalloc float[3];
					SHVDN.NativeMemory.GetRotationFromMatrix(tempRotationArray, address);

					return new Vector3(tempRotationArray[0], tempRotationArray[1], tempRotationArray[2]);
				}
			}
			set => PoseQuaternion = Quaternion.Euler(value, EulerRotationOrder.YXZ);
		}

		/// <summary>
		/// Gets the position of this <see cref="EntityBone"/> in world coordinates.
		/// </summary>
		public Vector3 Position => Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, Owner.Handle, Index);

		/// <summary>
		/// Gets the world quaternion of this <see cref="EntityBone"/>.
		/// </summary>
		public Quaternion Quaternion
		{
			// The result is basically the same as GET_ENTITY_BONE_OBJECT_ROTATION but this getter calculates via quaternions (not matrices)
			get
			{
				IntPtr relativeMatrixAddress = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
				if (relativeMatrixAddress == IntPtr.Zero)
				{
					return Quaternion.Zero;
				}

				var relativeBoneQuaternion = Quaternion.RotationMatrix(new Matrix(SHVDN.NativeMemory.ReadMatrix(relativeMatrixAddress)));

				IntPtr transformMatrixAddress = SHVDN.NativeMemory.GetEntityBoneTransformMatrixAddress(Owner.Handle);
				if (transformMatrixAddress == IntPtr.Zero)
				{
					// GET_ENTITY_BONE_OBJECT_ROTATION considers this edge case
					return relativeBoneQuaternion;
				}

				var globalTransformQuaternion = Quaternion.RotationMatrix(new Matrix(SHVDN.NativeMemory.ReadMatrix(transformMatrixAddress)));

				return globalTransformQuaternion * relativeBoneQuaternion;
			}
		}

		/// <summary>
		/// Gets the world rotation of this <see cref="EntityBone"/>.
		/// </summary>
		public Vector3 Rotation
		{
			get
			{
				Quaternion quaternion = Quaternion;
				if (quaternion == Quaternion.Zero)
				{
					// Failed to get the relative matrix
					return Vector3.Zero;
				}

				return quaternion.ToEuler();
			}
		}

		/// <summary>
		/// Gets the position of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of.
		/// </summary>
		/// <remarks>
		/// While you can change appearance for <see cref="Ped"/>s or <see cref="Prop"/>s by modifying this property,
		/// you cannot change appearance for <see cref="Vehicle"/>s by modifying this property.
		/// </remarks>
		public Vector3 RelativePosition
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x30));
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteVector3((address + 0x30), value.ToInternalFVector3());
			}
		}

		/// <summary>
		/// Gets or sets the quaternion of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of from <see cref="RelativeMatrix"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// While you can change appearance for <see cref="Ped"/>s or <see cref="Prop"/>s by modifying this property,
		/// you cannot change appearance for <see cref="Vehicle"/>s by modifying this property.
		/// </para>
		/// <para>
		/// Remember to normalize the value you are going to set before setting it to this property as the setter does not normalize the value, just like <c>SET_ENTITY_QUATERNION</c> does not.
		/// Setting an unnormalized quaternion may result in unintended scale change if the <see cref="Entity"/> is not playing animations that affect this bone or not simulating physics for this bone.
		/// </para>
		/// </remarks>
		public Quaternion RelativeQuaternion
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Quaternion.Zero;
				}

				unsafe
				{
					float* tempRotationArray = stackalloc float[4];
					SHVDN.NativeMemory.GetQuaternionFromMatrix(tempRotationArray, address);

					return new Quaternion(tempRotationArray[0], tempRotationArray[1], tempRotationArray[2], tempRotationArray[3]);
				}
			}
			set
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return;
				}

				var matrixToWrite = new Matrix(SHVDN.NativeMemory.ReadMatrix(address));
				Vector3 scale = matrixToWrite.GetScaleVector();
				var translation = new Vector3(matrixToWrite.M41, matrixToWrite.M42, matrixToWrite.M43);

				matrixToWrite = Matrix.Scaling(scale) * Matrix.RotationQuaternion(value) * Matrix.Translation(translation);
				SHVDN.NativeMemory.WriteMatrix(address, matrixToWrite.ToArray());
			}
		}

		/// <summary>
		/// Gets the rotation of this <see cref="EntityBone"/> relative to the <see cref="Entity"/> its part of from <see cref="RelativeMatrix"/>.
		/// </summary>
		/// <remarks>
		/// While you can change appearance for <see cref="Ped"/>s or <see cref="Prop"/>s by modifying this property,
		/// you cannot change appearance for <see cref="Vehicle"/>s by modifying this property.
		/// </remarks>
		public Vector3 RelativeRotation
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				unsafe
				{
					float* tempRotationArray = stackalloc float[3];
					SHVDN.NativeMemory.GetRotationFromMatrix(tempRotationArray, address);

					return new Vector3(tempRotationArray[0], tempRotationArray[1], tempRotationArray[2]);
				}
			}
			set => RelativeQuaternion = Quaternion.Euler(value, EulerRotationOrder.YXZ);
		}

		/// <summary>
		/// Gets the vector that points above this <see cref="EntityBone"/> relative to the world.
		/// </summary>
		public Vector3 UpVector
		{
			get
			{
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
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
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
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
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
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
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
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
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
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
				IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
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
			IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
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
			IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
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
			IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
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
			IntPtr address = SHVDN.NativeMemory.GetEntityBoneGlobalMatrixAddress(Owner.Handle, Index);
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
			return left?.Equals(right) ?? right is null;
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
			return entityBone?.Index ?? -1;
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
