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

        /// <summary>
        /// Creates a new <see cref="PedGroup"/>.
        /// </summary>
        /// <remarks>
        /// Make sure to add a Leader <see cref="Ped"/> to the group in the same frame you created it.
        /// Otherwise, the group may be removed by the game engine.
        /// </remarks>
        public PedGroup() : base(Function.Call<int>(Hash.CREATE_GROUP, 0))
        {
        }

        /// <summary>
        /// Creates a new instance for an existing <see cref="PedGroup"/>.
        /// </summary>
        /// <param name="handle">The handle of the existing <see cref="PedGroup"/>.</param>
        public PedGroup(int handle) : base(handle)
        {
        }


        public void Dispose()
        {
            Function.Call(Hash.REMOVE_GROUP, Handle);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Gets whether this <see cref="PedGroup"/> has a leader.
        /// </summary>
        public bool HasLeader
        {
            get
            {
                bool hasLeader;
                int count;
                unsafe
                {
                    Function.Call(Hash.GET_GROUP_SIZE, Handle, &hasLeader, &count);
                }

                return hasLeader;
            }
        }

        /// <summary>
        /// Gets the number of members in the group.
        /// </summary>
        public int MemberCount
        {
            get
            {
                bool hasLeader;
                int count;
                unsafe
                {
                    Function.Call(Hash.GET_GROUP_SIZE, Handle, &hasLeader, &count);
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
        /// Sets the formation spacing for this <see cref="PedGroup"/>.
        /// </summary>
        /// <param name="spacing">The distance between <see cref="Ped"/>s in the group.</param>
        /// <param name="adjustSpeedMinDistance">
        /// The distance from <paramref name="spacing"/> at which <see cref="Ped"/>s will start to slow down. 
        /// Defaults to -1 (no change) unless auto-calculated (see remarks).
        /// </param>
        /// <param name="adjustSpeedMaxDistance">
        /// The distance from <paramref name="spacing"/> at which <see cref="Ped"/>s will start to speed up. 
        /// Defaults to -1 (no change) unless auto-calculated (see remarks).
        /// </param>
        /// <remarks>
        /// If both <paramref name="adjustSpeedMinDistance"/> and <paramref name="adjustSpeedMaxDistance"/> are less than 0 
        /// and <see cref="Formation"/> is set to <see cref="Formation.Loose"/>, their values are calculated automatically.
        ///
        /// If only one of <paramref name="adjustSpeedMinDistance"/> or <paramref name="adjustSpeedMaxDistance"/> is less than 0, 
        /// its internal value remains unchanged.
        /// </remarks>
        public void SetFormationSpacing(float spacing, float adjustSpeedMinDistance = -1, float adjustSpeedMaxDistance = -1)
        {
            Function.Call(Hash.SET_GROUP_FORMATION_SPACING, Handle, spacing, adjustSpeedMinDistance, adjustSpeedMaxDistance);
        }

        /// <summary>
        /// Resets the formation spacing of this <see cref="PedGroup"/> to the default values.
        /// </summary>
        public void ResetFormationSpacing()
        {
            Function.Call(Hash.RESET_GROUP_FORMATION_DEFAULT_SPACING, Handle);
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
        /// The <see cref="Player"/> has its own group and can not be added to other groups.
        /// <see cref="Ped"/>s cannot be added to this <see cref="PedGroup"/> as Members if the group has no leader.
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
        /// The zero-based index of the group member.
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
        /// <param name="left">The left <see cref="PedGroup"/>.</param>
        /// <param name="right">The right <see cref="PedGroup"/>.</param>
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

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="Ped"/>s in this group.
        /// </summary>
        /// <returns>An enumerator for the <see cref="Ped"/>s in this group.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns a type-safe enumerator that iterates through the <see cref="Ped"/>s in this group.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{Ped}"/> for the <see cref="Ped"/>s in this group.</returns>
        public IEnumerator<Ped> GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}
