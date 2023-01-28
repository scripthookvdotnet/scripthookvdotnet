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
	public class EntityBoneCollection : IEnumerable<EntityBone>
	{
		public class Enumerator : IEnumerator<EntityBone>
		{
			#region Fields
			readonly EntityBoneCollection collection;
			int currentIndex = -1; // Skip the CORE bone index(-1)
			#endregion

			public Enumerator(EntityBoneCollection collection)
			{
				this.collection = collection;
			}

			public EntityBone Current => collection[currentIndex];

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

		#region Fields
		protected readonly Entity _owner;
		#endregion

		internal EntityBoneCollection(Entity owner)
		{
			_owner = owner;
		}

		/// <summary>
		/// Gets the <see cref="EntityBone"/> at the specified bone index.
		/// </summary>
		/// <param name="boneIndex">The bone index.</param>
		public EntityBone this[int boneIndex]
		{
			get => new EntityBone(_owner, boneIndex);
		}

		/// <summary>
		/// <para>
		/// Gets the <see cref="EntityBone"/> with the specified bone name.
		/// If the corresponding bone is not found, the <see cref="EntityBone.Index"/> of the returned instance will return <c>-1</c>.
		/// To access to the chassis bone of <see cref="Vehicle"/>, use <see cref="this[int]"/> with the index <c>0</c> as the chassis bone index and ID will always be <c>0</c> (hardcoded to the exe).
		/// </para>
		/// <para>
		/// This method will try to find the corresponding bone by the hash calcutated with <c>(ElfHashUppercased(string) % 0xFE8F + 0x170)</c>,
		/// where <c>ElfHashUppercased(string)</c> will convert ASCII lowercase characters to uppercase ones before hashing characters.
		/// </para>
		/// </summary>
		/// <param name="boneName">Name of the bone.</param>
		/// <remarks>
		/// Registered bone tag values (in the model) may be different from the calculated hashes from corresponding bone names.
		/// For example, <see cref="Ped"/>s have the bone in their skeletons whose name is <c>SKEL_Spine3</c> and whose ID is <c>24818</c>, which doesn't match the hashed value of <c>SKEL_Spine3</c> but matches that of <c>BONETAG_SPINE3</c>.
		/// </remarks>
		public EntityBone this[string boneName]
		{
			get => new EntityBone(_owner, boneName);
		}

		/// <summary>
		/// Gets the number of bones that this <see cref="Entity"/> has.
		/// </summary>
		public int Count => SHVDN.NativeMemory.GetEntityBoneCount(_owner.Handle);

		/// <summary>
		/// Determines whether this <see cref="Entity"/> has a bone with the specified bone name
		/// </summary>
		/// <param name="boneName">Name of the bone.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> has a bone with the specified bone name; otherwise, <see langword="false" />.
		/// </returns>
		public bool Contains(string boneName)
		{
			return Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, _owner.Handle, boneName) != -1;
		}

		/// <summary>
		/// Gets the core bone of this <see cref="Entity"/>.
		/// </summary>
		public EntityBone Core => new EntityBone(_owner, -1);

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<EntityBone> GetEnumerator()
		{
			return new Enumerator(this);
		}

		public override int GetHashCode()
		{
			return _owner.GetHashCode() ^ Count.GetHashCode();
		}
	}
}
