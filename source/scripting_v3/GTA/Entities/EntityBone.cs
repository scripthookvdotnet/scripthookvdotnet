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

		#region Attaching

		/// <summary>
		/// Attaches this <see cref="EntityBone"/> to a <see cref="EntityBone"/> of a different <see cref="Entity"/>.
		/// </summary>
		/// <param name="boneOfSecondEntity">
		/// The <see cref="EntityBone"/> to attach this <see cref="EntityBone"/> to that belongs to another
		/// <see cref="Entity"/>.
		/// </param>
		/// <param name="activeCollisions">
		/// If <see langword="true"/> this <see cref="Entity"/> collide with other <see cref="Entity"/>s (does not
		/// collide with map collision or static <see cref="Prop"/>s that do not have physics/colliders).
		/// </param>
		/// <param name="useBasicAttachIfPed">
		/// Specifies whether the method forces a path, even for <see cref="Ped"/>s, that will use all three rotation
		/// components, just like how <see cref="Vehicle"/>s and <see cref="Prop"/>s will be placed.
		/// if <see langword="false"/>, pitch will not work and roll will only work on negative numbers for <see cref="Ped"/>s.
		/// If the owner <see cref="Entity"/> of this <see cref="EntityBone"/> is a <see cref="Vehicle"/>s or
		/// <see cref="Prop"/>, setting this parameter to <see langword="true"/> has no effect as they use all
		/// rotations by default.
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
		public void AttachToBone(EntityBone boneOfSecondEntity, bool activeCollisions = true,
			bool useBasicAttachIfPed = false)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_791_2_Steam, nameof(EntityBone),
				nameof(AttachToBone));

			Entity secondEntity = boneOfSecondEntity.Owner;

			if (!ThisEntityAndSecondEntityExist(secondEntity))
			{
				return;
			}

			ThrowExceptionIfEitherOfEntityDoesNotHaveSkeleton(secondEntity);

			Function.Call(Hash.ATTACH_ENTITY_BONE_TO_ENTITY_BONE, Owner, secondEntity, Index, boneOfSecondEntity.Index,
				activeCollisions, useBasicAttachIfPed);
		}

		/// <summary>
		/// Attaches this <see cref="EntityBone"/> to a <see cref="EntityBone"/> of a different <see cref="Entity"/>
		/// assuming that the bone is facing along the y axis.
		/// </summary>
		/// <param name="boneOfSecondEntity">
		/// The <see cref="EntityBone"/> to attach this <see cref="EntityBone"/> to that belongs to another
		/// <see cref="Entity"/>.
		/// </param>
		/// <param name="activeCollisions">
		/// If <see langword="true"/> this <see cref="Entity"/> collide with other <see cref="Entity"/>s (does not
		/// collide with map collision or static <see cref="Prop"/>s that do not have physics/colliders).
		/// </param>
		/// <param name="useBasicAttachIfPed">
		/// Specifies whether the method forces a path, even for <see cref="Ped"/>s, that will use all three rotation
		/// components, just like how <see cref="Vehicle"/>s and <see cref="Prop"/>s will be placed.
		/// if <see langword="false"/>, pitch will not work and roll will only work on negative numbers for <see cref="Ped"/>s.
		/// If the owner <see cref="Entity"/> of this <see cref="EntityBone"/> is a <see cref="Vehicle"/>s or
		/// <see cref="Prop"/>, setting this parameter to <see langword="true"/> has no effect as they use all
		/// rotations by default.
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
		public void AttachToBoneYForward(EntityBone boneOfSecondEntity, bool activeCollisions = true,
			bool useBasicAttachIfPed = false)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_791_2_Steam, nameof(Entity),
				nameof(AttachToBoneYForward));

			Entity secondEntity = boneOfSecondEntity.Owner;
			if (!ThisEntityAndSecondEntityExist(secondEntity))
			{
				return;
			}

			ThrowExceptionIfEitherOfEntityDoesNotHaveSkeleton(secondEntity);

			Function.Call(Hash.ATTACH_ENTITY_BONE_TO_ENTITY_BONE_Y_FORWARD, Owner, secondEntity, Index,
				boneOfSecondEntity.Index, activeCollisions, useBasicAttachIfPed);
		}

		private bool ThisEntityAndSecondEntityExist(Entity secondEntity)
		{
			// Silently return if either of the entities does not exist, just like how ATTACH_ENTITY_BONE_TO_ENTITY_BONE checks
			// We don't want to throw exceptions that much unless otherwise the game will encounter a game crash,
			// as we can't really check all the conditions needed for natives without inspecting the assembly code of them
			return (Owner?.Exists() ?? false) && (secondEntity?.Exists() ?? false);
		}

		/// <summary>
		/// This method is present since ATTACH_ENTITY_BONE_TO_ENTITY_BONE will crash the game if both entities exist but
		/// either of them does not have a skeleton.
		/// </summary>
		private void ThrowExceptionIfEitherOfEntityDoesNotHaveSkeleton(Entity secondEntity)
		{
			if (!(Owner?.HasSkeleton ?? false))
			{
				throw new InvalidOperationException("The entity of this bone does not have a skeleton.");
			}

			if (!(secondEntity?.HasSkeleton ?? false))
			{
				throw new ArgumentException("The entity of passed bone does not have a skeleton.", nameof(secondEntity));
			}
		}

		/// <summary>
		/// Attaches this <see cref="EntityBone"/> to the transformation matrix (physics capsule) of a different
		/// <see cref="Entity"/>.
		/// </summary>
		/// <param name="secondEntity">
		/// The <see cref="Entity"/> to attach this <see cref="EntityBone"/> to.
		/// </param>
		/// <param name="secondEntityOffset">
		/// The attach point offset of <paramref name="secondEntity"/> in local space.
		/// </param>
		/// <param name="thisEntityOffset">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='thisEntityOffset']"
		/// />
		/// </param>
		/// <param name="rotation">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='rotation']"
		/// />
		/// </param>
		/// <param name="physicalStrength">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='physicalStrength']"
		/// />
		/// </param>
		/// <param name="constrainRotation">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='constrainRotation']"
		/// />
		/// </param>
		/// <param name="doInitialWarp">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='doInitialWarp']"
		/// />
		/// </param>
		/// <param name="addInitialSeparation">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='addInitialSeparation']"
		/// />
		/// </param>
		/// <param name="collideWithEntity">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='collideWithEntity']"
		/// />
		/// </param>
		/// <param name="rotationOrder">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='rotationOrder']"
		/// />
		/// </param>
		/// <remarks>
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/remarks"
		/// />
		/// </remarks>
		public void AttachToEntityPhysically( Entity secondEntity, Vector3 secondEntityOffset,
			Vector3 thisEntityOffset, Vector3 rotation, float physicalStrength, bool constrainRotation,
			bool doInitialWarp = true, bool collideWithEntity = false, bool addInitialSeparation = true,
			EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY, Owner, secondEntity, Index, -1,
				secondEntityOffset.X, secondEntityOffset.Y, secondEntityOffset.Z, thisEntityOffset.X,
				thisEntityOffset.Y, thisEntityOffset.Z, rotation.X, rotation.Y, rotation.Z, physicalStrength,
				constrainRotation, doInitialWarp, collideWithEntity, addInitialSeparation, (int)rotationOrder);
		}

		/// <summary>
		/// Attaches a bone of this <see cref="Entity"/> to a bone of a different <see cref="Entity"/>.
		/// </summary>
		/// <param name="boneOfSecondEntity">
		/// The <see cref="EntityBone"/> to attach this <see cref="EntityBone"/> to that belongs to
		/// second/another <see cref="Entity"/>.
		/// </param>
		/// <param name="secondEntityOffset">
		/// The attach point offset of <paramref name="boneOfSecondEntity"/> in local space.
		/// </param>
		/// <param name="thisEntityOffset">
		/// The attach point offset of this <see cref="Entity"/> relative to this <see cref="EntityBone"/>.
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
		public void AttachToBonePhysically(EntityBone boneOfSecondEntity, Vector3 secondEntityOffset,
			Vector3 thisEntityOffset, Vector3 rotation, float physicalStrength, bool constrainRotation,
			bool doInitialWarp = true, bool collideWithEntity = false, bool addInitialSeparation = true,
			EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY, Owner, boneOfSecondEntity.Owner,
				Index, boneOfSecondEntity.Index, secondEntityOffset.X, secondEntityOffset.Y, secondEntityOffset.Z,
				thisEntityOffset.X, thisEntityOffset.Y, thisEntityOffset.Z, rotation.X, rotation.Y, rotation.Z,
				physicalStrength, constrainRotation, doInitialWarp, collideWithEntity, addInitialSeparation,
				(int)rotationOrder);
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
		/// <param name="secondEntity">
		/// <inheritdoc
		/// cref="AttachToEntityPhysically"
		/// path="/param[@name='secondEntity']"
		/// />
		/// </param>
		/// <param name="secondEntityOffset">
		/// <inheritdoc
		/// cref="AttachToEntityPhysically"
		/// path="/param[@name='secondEntityOffset']"
		/// />
		/// </param>
		/// <param name="thisEntityOffset">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='thisEntityOffset']"
		/// />
		/// </param>
		/// <param name="rotation">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='rotation']"
		/// />
		/// </param>
		/// <param name="physicalStrength">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='physicalStrength']"
		/// />
		/// </param>
		/// <param name="constrainRotation">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='constrainRotation']"
		/// />
		/// </param>
		/// <param name="doInitialWarp">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='doInitialWarp']"
		/// />
		/// </param>
		/// <param name="addInitialSeparation">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='addInitialSeparation']"
		/// />
		/// </param>
		/// <param name="collideWithEntity">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='collideWithEntity']"
		/// />
		/// </param>
		/// <param name="rotationOrder">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='rotationOrder']"
		/// />
		/// </param>
		/// <param name="invMassScaleA">
		/// <inheritdoc
		/// cref="AttachToBonePhysicallyOverrideInverseMass"
		/// path="/param[@name='invMassScaleA']"
		/// />
		/// </param>
		/// <param name="invMassScaleB">
		/// <inheritdoc
		/// cref="AttachToBonePhysicallyOverrideInverseMass"
		/// path="/param[@name='invMassScaleB']"
		/// />
		/// </param>
		/// <remarks>
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/remarks"
		/// />
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if called in the game versions earlier than v1.0.2944.0.
		/// </exception>
		public void AttachToEntityPhysicallyOverrideInverseMass(Entity secondEntity, Vector3 secondEntityOffset,
			Vector3 thisEntityOffset, Vector3 rotation, float physicalStrength, bool constrainRotation,
			bool doInitialWarp = true, bool collideWithEntity = false, bool addInitialSeparation = true,
			EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ, float invMassScaleA = 1f,
			float invMassScaleB = 1f)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_2944_0, nameof(Entity),
				nameof(AttachToEntityPhysicallyOverrideInverseMass));

			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY_OVERRIDE_INVERSE_MASS, Owner, secondEntity,
				Index, -1, secondEntityOffset.X, secondEntityOffset.Y, secondEntityOffset.Z, thisEntityOffset.X,
				thisEntityOffset.Y, thisEntityOffset.Z, rotation.X, rotation.Y, rotation.Z, physicalStrength,
				constrainRotation, doInitialWarp, collideWithEntity, addInitialSeparation, (int)rotationOrder,
				invMassScaleA, invMassScaleB);
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
		/// <param name="boneOfSecondEntity">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='boneOfSecondEntity']"
		/// />
		/// </param>
		/// <param name="secondEntityOffset">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='secondEntityOffset']"
		/// />
		/// </param>
		/// <param name="thisEntityOffset">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='thisEntityOffset']"
		/// />
		/// </param>
		/// <param name="rotation">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='rotation']"
		/// />
		/// </param>
		/// <param name="physicalStrength">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='physicalStrength']"
		/// />
		/// </param>
		/// <param name="constrainRotation">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='constrainRotation']"
		/// />
		/// </param>
		/// <param name="doInitialWarp">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='doInitialWarp']"
		/// />
		/// </param>
		/// <param name="addInitialSeparation">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='addInitialSeparation']"
		/// />
		/// </param>
		/// <param name="collideWithEntity">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
		/// path="/param[@name='collideWithEntity']"
		/// />
		/// </param>
		/// <param name="rotationOrder">
		/// <inheritdoc
		/// cref="AttachToBonePhysically"
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
		/// cref="AttachToBonePhysically"
		/// path="/remarks"
		/// />
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if called in the game versions earlier than v1.0.2944.0.
		/// </exception>
		public void AttachToBonePhysicallyOverrideInverseMass(EntityBone boneOfSecondEntity,
			Vector3 secondEntityOffset, Vector3 thisEntityOffset, Vector3 rotation, float physicalStrength,
			bool constrainRotation, bool doInitialWarp = true, bool collideWithEntity = false,
			bool addInitialSeparation = true, EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ,
			float invMassScaleA = 1f, float invMassScaleB = 1f)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_2944_0, nameof(Entity),
				nameof(AttachToBonePhysicallyOverrideInverseMass));

			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY_OVERRIDE_INVERSE_MASS, Owner,
				boneOfSecondEntity.Owner, Index, boneOfSecondEntity.Index, secondEntityOffset.X,
				secondEntityOffset.Y, secondEntityOffset.Z, thisEntityOffset.X, thisEntityOffset.Y, thisEntityOffset.Z,
				rotation.X, rotation.Y, rotation.Z, physicalStrength, constrainRotation, doInitialWarp,
				collideWithEntity, addInitialSeparation, (int)rotationOrder, invMassScaleA, invMassScaleB);
		}

		#endregion

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
