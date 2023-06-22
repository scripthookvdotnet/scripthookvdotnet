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
	public sealed class PedGroup : PoolObject, IEnumerable<Ped>, IDisposable
	{
		public sealed class Enumerator : IEnumerator<Ped>
		{
			#region Fields
			readonly PedGroup collection;
			Ped current;
			int currentIndex = -2;
			#endregion

			public Enumerator(PedGroup group)
			{
				collection = group;
			}

			public Ped Current => current;

			object IEnumerator.Current => current;

			public void Reset()
			{
			}

			public bool MoveNext()
			{
				if (currentIndex++ < (collection.MemberCount - 1))
				{
					current = currentIndex < 0 ? collection.Leader : collection.GetMember(currentIndex);

					if (current != null)
					{
						return true;
					}

					return MoveNext();
				}

				return false;
			}

			void IDisposable.Dispose()
			{
			}
		}

		public PedGroup() : base(Function.Call<int>(Hash.CREATE_GROUP, 0))
		{
		}
		public PedGroup(int handle) : base(handle)
		{
		}

		public void Dispose()
		{
			Function.Call(Hash.REMOVE_GROUP, Handle);
			GC.SuppressFinalize(this);
		}

		public int MemberCount
		{
			get
			{
				long unknBool;
				int count;
				unsafe
				{
					Function.Call(Hash.GET_GROUP_SIZE, Handle, &unknBool, &count);
				}
				return count;
			}
		}

		public float SeparationRange
		{
			set => Function.Call(Hash.SET_GROUP_SEPARATION_RANGE, Handle, value);
		}

		public Formation Formation
		{
			set => Function.Call(Hash.SET_GROUP_FORMATION, Handle, (int)value);
		}

		public void Add(Ped ped, bool leader)
		{
			Function.Call(leader ? Hash.SET_PED_AS_GROUP_LEADER : Hash.SET_PED_AS_GROUP_MEMBER, ped.Handle, Handle);
		}
		public void Remove(Ped ped)
		{
			Function.Call(Hash.REMOVE_PED_FROM_GROUP, ped.Handle);
		}

		public bool Contains(Ped ped)
		{
			return Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, ped.Handle, Handle);
		}

		public Ped Leader
		{
			get
			{
				int handle = Function.Call<int>(Hash.GET_PED_AS_GROUP_LEADER, Handle);
				return handle != 0 ? new Ped(handle) : null;
			}
		}

		public Ped GetMember(int index)
		{
			int handle = Function.Call<int>(Hash.GET_PED_AS_GROUP_MEMBER, Handle, index);
			return handle != 0 ? new Ped(handle) : null;
		}

		public Ped[] ToArray(bool includingLeader = true)
		{
			return ToList(includingLeader).ToArray();
		}

		public List<Ped> ToList(bool includingLeader = true)
		{
			int memberCount = MemberCount;
			int expectedListSize = includingLeader ? 1 + memberCount : memberCount;
			var result = new List<Ped>(expectedListSize);

			if (includingLeader)
			{
				Ped leader = Leader;

				if (leader != null)
				{
					result.Add(leader);
				}
			}

			for (int i = 0; i < memberCount; i++)
			{
				Ped member = GetMember(i);

				if (member != null)
				{
					result.Add(member);
				}
			}

			return result;
		}

		/// <summary>
		/// Removes this <see cref="PedGroup"/>.
		/// </summary>
		public override void Delete()
		{
			Function.Call(Hash.REMOVE_GROUP, Handle);
		}

		/// <summary>
		/// Determines if this <see cref="PedGroup"/> exists.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="PedGroup"/> exists; otherwise, <see langword="false" />.</returns>
		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_GROUP_EXIST, Handle);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same group as this <see cref="PedGroup"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same group as this <see cref="PedGroup"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is PedGroup group)
			{
				return Handle == group.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="PedGroup"/>s refer to the same group.
		/// </summary>
		/// <param name="left">The left <see cref="Checkpoint"/>.</param>
		/// <param name="right">The right <see cref="Checkpoint"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same group as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(PedGroup left, PedGroup right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="PedGroup"/>s don't refer to the same group.
		/// </summary>
		/// <param name="left">The left <see cref="PedGroup"/>.</param>
		/// <param name="right">The right <see cref="PedGroup"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same group as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(PedGroup left, PedGroup right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts a <see cref="PedGroup"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(PedGroup value)
		{
			return new InputArgument((ulong)value.Handle);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}
		public IEnumerator<Ped> GetEnumerator()
		{
			return new Enumerator(this);
		}
	}
}
