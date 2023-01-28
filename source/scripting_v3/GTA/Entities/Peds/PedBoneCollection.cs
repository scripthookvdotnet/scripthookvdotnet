//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GTA
{
	public class PedBoneCollection : EntityBoneCollection, IEnumerable<PedBone>
	{
		public new class Enumerator : IEnumerator<PedBone>
		{
			#region Fields
			readonly PedBoneCollection collection;
			int currentIndex = -1; // Skip the CORE bone index(-1)
			#endregion

			public Enumerator(PedBoneCollection collection)
			{
				this.collection = collection;
			}

			public PedBone Current => collection[currentIndex];

			object IEnumerator.Current => collection[currentIndex];

			public void Reset()
			{
				currentIndex = -1;
			}

			public bool MoveNext()
			{
				return ++currentIndex < collection.Count;
			}

			void IDisposable.Dispose()
			{
			}
		}

		internal PedBoneCollection(Ped owner) : base(owner)
		{
		}

		/// <summary>
		/// Gets the <see cref="PedBone"/> with the specified <paramref name="boneId"/>.
		/// </summary>
		/// <param name="boneId">The bone Id.</param>
		public PedBone this[Bone boneId]
		{
			get => new PedBone((Ped)_owner, boneId);
		}

		/// <summary>
		/// Gets the <see cref="PedBone"/> at the specified bone index.
		/// </summary>
		/// <param name="boneIndex">The bone index.</param>
		public new PedBone this[int boneIndex]
		{
			get => new PedBone((Ped)_owner, boneIndex);
		}

		/// <summary>
		/// Gets the <see cref="PedBone"/> with the specified bone name.
		/// </summary>
		/// <param name="boneName">Name of the bone.</param>
		public new PedBone this[string boneName]
		{
			get => new PedBone((Ped)_owner, boneName);
		}

		/// <summary>
		/// Gets the core bone of this <see cref="Ped"/>.
		/// </summary>
		public new PedBone Core => new PedBone((Ped)_owner, -1);

		/// <summary>
		/// Gets the last damaged bone for this <see cref="Ped"/>.
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
						return this[(Bone)outBone];
					}
				}
				return this[Bone.SkelRoot];
			}
		}

		/// <summary>
		/// Clears the last damage a bone on this <see cref="Ped"/> received.
		/// </summary>
		public void ClearLastDamaged()
		{
			Function.Call(Hash.CLEAR_PED_LAST_DAMAGE_BONE, _owner.Handle);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public new IEnumerator<PedBone> GetEnumerator()
		{
			return new Enumerator(this);
		}
	}
}
