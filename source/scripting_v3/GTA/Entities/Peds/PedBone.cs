//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	public class PedBone : EntityBone
	{
		internal PedBone(Ped owner, int boneIndex) : base(owner, boneIndex)
		{
			Owner = owner;
		}
		internal PedBone(Ped owner, string boneName) : base(owner, boneName)
		{
			Owner = owner;
		}
		internal PedBone(Ped owner, Bone boneId)
			: base(owner, Function.Call<int>(Hash.GET_PED_BONE_INDEX, owner.Handle, boneId))
		{
			Owner = owner;
		}

		public new Ped Owner
		{
			get;
		}
	}
}
