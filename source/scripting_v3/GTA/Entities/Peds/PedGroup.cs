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


        /// <summary>
        /// Gets the number of members in the group.
        /// </summary>
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

        /// <summary>
        /// Sets the maximum allowed distance from the group leader before a member leaves the group.
        /// </summary>
        public float SeparationRange
        {
            set => Function.Call(Hash.SET_GROUP_SEPARATION_RANGE, Handle, value);
        }

        /// <summary>
        /// Sets the <see cref="Formation"/> this <see cref="PedGroup"/> will use.
        /// </summary>
        public Formation Formation
        {
            set => Function.Call(Hash.SET_GROUP_FORMATION, Handle, (int)value);
        }

        /// <summary>
        /// Adds the specified <see cref="Ped"/> to this <see cref="PedGroup"/> as either a member or the leader.
        /// </summary>
        /// <param name="ped">The <see cref="Ped"/> to add to the group.</param>
        /// <param name="leader">
        /// If <c>true</c>, assigns the <paramref name="ped"/> as the group leader; otherwise, adds them as a regular member.
        /// </param>
        /// <remarks>
        /// The game checks against an internal limit of 8 members per group.
        /// </remarks>
        public void Add(Ped ped, bool leader)
        {
            Function.Call(leader ? Hash.SET_PED_AS_GROUP_LEADER : Hash.SET_PED_AS_GROUP_MEMBER, ped.Handle, Handle);
        }

        /// <summary>
        /// Removes the specified <see cref="Ped"/> from this <see cref="PedGroup"/>.
        /// </summary>
        /// <param name="ped">The <see cref="Ped"/> to remove from the group.</param>
        public void Remove(Ped ped)
        {
            Function.Call(Hash.REMOVE_PED_FROM_GROUP, ped.Handle);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Ped"/> is a member of this <see cref="PedGroup"/>.
        /// </summary>
        /// <param name="ped">The <see cref="Ped"/> to check for membership.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="ped"/> is a member of this group; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Ped ped)
        {
            return Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, ped.Handle, Handle);
        }

        /// <summary>
        /// Gets the leader <see cref="Ped"/> of this <see cref="PedGroup"/>.
        /// </summary>
        /// <returns>
        /// The leader <see cref="Ped"/> of the group, or <c>null</c> if the group has no leader.
        /// </returns>
        public Ped Leader
        {
            get
            {
                int handle = Function.Call<int>(Hash.GET_PED_AS_GROUP_LEADER, Handle);
                return handle != 0 ? new Ped(handle) : null;
            }
        }

        /// <summary>
        /// Gets the member <see cref="Ped"/> at the specified index within this <see cref="PedGroup"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the group member (0–7).
        /// </param>
        /// <returns>
        /// The <see cref="Ped"/> at the specified index, or <c>null</c> if no member exists at that position.
        /// </returns>
        public Ped GetMember(int index)
        {
            int handle = Function.Call<int>(Hash.GET_PED_AS_GROUP_MEMBER, Handle, index);
            return handle != 0 ? new Ped(handle) : null;
        }

        /// <summary>
        /// Returns all <see cref="Ped"/>s in this <see cref="PedGroup"/> as an array.
        /// </summary>
        /// <param name="includingLeader">
        /// If <c>true</c>, includes the group leader in the returned array; otherwise, only members are included.
        /// </param>
        /// <returns>
        /// An array of <see cref="Ped"/> objects representing the group's members (and optionally the leader).
        /// </returns>
        public Ped[] ToArray(bool includingLeader = true)
        {
            return ToList(includingLeader).ToArray();
        }

        /// <summary>
        /// Returns all <see cref="Ped"/>s in this <see cref="PedGroup"/> as a <c>List</c>.
        /// </summary>
        /// <param name="includingLeader">
        /// If <c>true</c>, includes the group leader in the returned list; otherwise, only members are included.
        /// </param>
        /// <returns>
        /// A list of <see cref="Ped"/> objects representing the group's members (and optionally the leader).
        /// </returns>
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
        /// Deletes this <see cref="PedGroup"/>.
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
