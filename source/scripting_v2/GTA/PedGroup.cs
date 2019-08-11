using System;
using System.Collections;
using System.Collections.Generic;
using GTA.Native;

namespace GTA
{
	public class PedGroup : IEquatable<PedGroup>, IEnumerable<Ped>, IHandleable, IDisposable
	{
		public PedGroup() : this(Function.Call<int>(Hash.CREATE_GROUP, 0))
		{
		}
		public PedGroup(int handle)
		{
			Handle = handle;
		}
		
		public void Dispose()
		{
			Function.Call(Native.Hash.REMOVE_GROUP, Handle);
			GC.SuppressFinalize(this);
		}

		public int Handle { get; }

		public int MemberCount
		{
			get
			{
				int count, val1;
				unsafe { Function.Call(Hash.GET_GROUP_SIZE, Handle, &val1, &count); }
				return count;
			}
		}

		public Ped Leader => Function.Call<Ped>(Hash._0x5CCE68DBD5FE93EC, Handle);

		public float SeparationRange
		{
			set => Function.Call(Hash.SET_GROUP_SEPARATION_RANGE, Handle, value);
		}
		public FormationType FormationType
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

		public Ped GetMember(int index)
		{
			return Function.Call<Ped>(Hash.GET_PED_AS_GROUP_MEMBER, Handle, index);
		}

		public Ped[] ToArray(bool includingLeader)
		{
			return ToList(includingLeader).ToArray();
		}

		public List<Ped> ToList(bool includingLeader)
		{
			var list = new List<Ped>();

			if (includingLeader)
			{
				Ped ped = Leader;
				if (Ped.Exists(ped))
					list.Add(ped);
			}

			for (int i = 0; i < MemberCount; i++)
			{
				Ped ped = GetMember(i);
				if (Ped.Exists(ped))
					list.Add(ped);
			}

			return list;
		}

		public bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_GROUP_EXIST, Handle);
		}
		public static bool Exists(PedGroup pedGroup)
		{
			return !(pedGroup is null) && pedGroup.Exists();
		}

		public bool Equals(PedGroup pedGroup)
		{
			return !(pedGroup is null) && Handle == pedGroup.Handle;
		}
		public override bool Equals(object pedGroup)
		{
			return !(pedGroup is null) && pedGroup.GetType() == GetType() && Equals((PedGroup)pedGroup);
		}

		public override int GetHashCode()
		{
			return Handle;
		}

		public class enumerator : IEnumerator<Ped>
		{
			int index;
			PedGroup group;

			public enumerator(PedGroup group)
			{
				index = -2;
				this.group = group;
			}

			public Ped Current { get; private set; }
			object IEnumerator.Current => Current;

			public void Reset()
			{
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				if (index < (group.MemberCount - 1))
				{
					index++;
					Current = index < 0 ? group.Leader : group.GetMember(index);

					if (!(Current is null) && Current.Exists())
						return true;

					return MoveNext();
				}

				return false;
			}
		}

		public IEnumerator<Ped> GetEnumerator()
		{
			return new enumerator(this);
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new enumerator(this);
		}
	}
}
