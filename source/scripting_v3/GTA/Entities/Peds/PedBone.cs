//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	public sealed class PedBone : EntityBone
	{
		internal PedBone(Ped owner, int boneIndex) : base(owner, boneIndex)
		{
			Owner = owner;
		}
		internal PedBone(Ped owner, string boneName) : base(owner, boneName)
		{
			Owner = owner;
		}
		// This overload is present to avoid redundant bone id fetching
		internal PedBone(Ped owner, int boneIndex, int boneId) : base(owner, boneIndex, boneId)
		{
			Owner = owner;
			Tag = (Bone)boneId;
		}
		internal PedBone(Ped owner, Bone boneId)
			: base(owner, Function.Call<int>(Hash.GET_PED_BONE_INDEX, owner.Handle, (int)boneId), (int)boneId)
		{
			Owner = owner;
			// Call the base Tag getter to set the tested value (will set the PedBone.Tag to -1 if the base Index property returns -1)
			Tag = (Bone)base.Tag;
		}

		public new Ped Owner
		{
			get;
		}

		/// <summary>
		/// Gets the bone tag (identifier) of this <see cref="PedBone"/>.
		/// Will return the same value as <see cref="EntityBone.Tag"/> but the returned type is <see cref="Bone"/>.
		/// If the bone does not exist, <see cref="Bone.Invalid"/> will be returned.
		/// </summary>
		public new Bone Tag
		{
			get;
		}

		/// <summary>
		/// Gets the sibling bone of this <see cref="PedBone"/>.
		/// To check existence of the next sibling bone, you can use <see cref="EntityBone.Index"/> or <see cref="PedBone.Tag"/>.
		/// </summary>
		public new PedBone NextSibling
		{
			get
			{
				SHVDN.NativeMemory.GetNextSiblingBoneIndexAndIdOfEntityBoneIndex(Owner.Handle, Index, out int boneIndex, out int boneTag);
				return new PedBone(Owner, boneIndex, boneTag);
			}
		}

		/// <summary>
		/// Gets the parent bone of this <see cref="PedBone"/>.
		/// To check existence of the next sibling bone, you can use <see cref="EntityBone.Index"/> or <see cref="PedBone.Tag"/>.
		/// </summary>
		public new PedBone Parent
		{
			get
			{
				SHVDN.NativeMemory.GetParentBoneIndexAndIdOfEntityBoneIndex(Owner.Handle, Index, out int boneIndex, out int boneTag);
				return new PedBone(Owner, boneIndex, boneTag);
			}
		}
	}
}
