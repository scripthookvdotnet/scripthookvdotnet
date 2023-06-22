//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GTA
{
	public sealed class PedBoneCollection : EntityBoneCollection, IEnumerable<PedBone>
	{
		public new sealed class Enumerator : IEnumerator<PedBone>
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
		public PedBone this[Bone boneId] => new((Ped)_owner, boneId);

		/// <summary>
		/// Gets the <see cref="PedBone"/> at the specified bone index.
		/// </summary>
		/// <param name="boneIndex">The bone index.</param>
		public new PedBone this[int boneIndex] => new((Ped)_owner, boneIndex);

		/// <summary>
		/// <para>
		/// Gets the <see cref="PedBone"/> with the specified bone name. Use this overload only if you know a correct bone tag.
		/// If the corresponding bone is not found, the <see cref="EntityBone.Index"/> of the returned instance will return <c>-1</c>.
		/// </para>
		/// <para>
		/// This method will try to find the corresponding bone by the hash calcutated with <c>(ElfHashUppercased(string) % 0xFE8F + 0x170)</c>,
		/// where <c>ElfHashUppercased(string)</c> will convert ASCII lowercase characters to uppercase ones before hashing characters.
		/// </para>
		/// </summary>
		/// <param name="boneName">Name of the bone.</param>
		/// <remarks>
		/// Registered bone tag values (in the model) may be different from the calculated hashes from corresponding bone names.
		/// For example, <see cref="Ped"/>s have the bones in their skeletons whose name is <c>SKEL_Spine3</c> and whose ID is <c>24818</c>, which doesn't match the hashed value of <c>SKEL_Spine3</c> but matches that of <c>BONETAG_SPINE3</c>.
		/// On the other hand, <see cref="Ped"/>s have the bone in their skeletons whose name is <c>IK_Head</c> and whose ID is <c>12844</c>, which matches the hashed value of <c>IK_Head</c>.
		/// </remarks>
		public new PedBone this[string boneName] => new((Ped)_owner, boneName);

		/// <summary>
		/// Gets the core bone of this <see cref="Ped"/>.
		/// </summary>
		public new PedBone Core => new((Ped)_owner, -1);

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
