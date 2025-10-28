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

        public float SeparationRange
        {
            set => Function.Call(Hash.SET_GROUP_SEPARATION_RANGE, Handle, value);
        }

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
