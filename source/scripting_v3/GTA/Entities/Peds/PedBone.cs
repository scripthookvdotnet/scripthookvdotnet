//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	public class PedBone : EntityBone
	{
		#region Fields
		private new readonly Ped _owner;
		#endregion

		public new Ped Owner => _owner;

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

		public new bool IsValid => Ped.Exists(Owner) && Index != -1;
	}
}
