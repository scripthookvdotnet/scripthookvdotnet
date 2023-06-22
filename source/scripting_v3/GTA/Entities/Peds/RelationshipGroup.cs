//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	public struct RelationshipGroup : INativeValue, IEquatable<RelationshipGroup>
	{
		RelationshipGroup(string name) : this()
		{
			int hashArg;
			unsafe
			{
				Function.Call(Native.Hash.ADD_RELATIONSHIP_GROUP, name, &hashArg);
			}

			Hash = hashArg;
		}
		public RelationshipGroup(int hash) : this()
		{
			Hash = hash;
		}
		public RelationshipGroup(uint hash) : this((int)hash)
		{
		}
		public RelationshipGroup(RelationshipGroupHash hash) : this((int)hash)
		{
		}

		/// <summary>
		/// Gets the hash for this <see cref="RelationshipGroup"/>.
		/// </summary>
		public int Hash
		{
			get; private set;
		}

		/// <summary>
		/// Gets the native representation of this <see cref="RelationshipGroup"/>.
		/// </summary>
		public ulong NativeValue
		{
			get => (ulong)Hash;
			set => Hash = unchecked((int)value);
		}

		public Relationship GetRelationshipBetweenGroups(RelationshipGroup targetGroup)
		{
			return Function.Call<Relationship>(Native.Hash.GET_RELATIONSHIP_BETWEEN_GROUPS, Hash, targetGroup.NativeValue);
		}

		public void SetRelationshipBetweenGroups(RelationshipGroup targetGroup, Relationship relationship, bool bidirectionally = false)
		{
			Function.Call(Native.Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, Hash, targetGroup);

			if (bidirectionally)
			{
				Function.Call(Native.Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, targetGroup, Hash);
			}
		}

		public void ClearRelationshipBetweenGroups(RelationshipGroup targetGroup, Relationship relationship, bool bidirectionally = false)
		{
			Function.Call(Native.Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, Hash, targetGroup);

			if (bidirectionally)
			{
				Function.Call(Native.Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, targetGroup, Hash);
			}
		}

		public void Remove()
		{
			Function.Call(Native.Hash.REMOVE_RELATIONSHIP_GROUP, Hash);
		}

		public bool Equals(RelationshipGroup group)
		{
			return Hash == group.Hash;
		}
		public override bool Equals(object obj)
		{
			if (obj is RelationshipGroup group)
			{
				return Equals(group);
			}

			return false;
		}

		public static bool operator ==(RelationshipGroup left, RelationshipGroup right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(RelationshipGroup left, RelationshipGroup right)
		{
			return !(left == right);
		}

		public static implicit operator RelationshipGroup(int source)
		{
			return new RelationshipGroup(source);
		}
		public static implicit operator RelationshipGroup(uint source)
		{
			return new RelationshipGroup(source);
		}
		public static implicit operator RelationshipGroup(RelationshipGroupHash source)
		{
			return new RelationshipGroup(source);
		}
		public static implicit operator RelationshipGroup(string source)
		{
			return new RelationshipGroup(source);
		}

		public static implicit operator InputArgument(RelationshipGroup value)
		{
			return new InputArgument((ulong)value.Hash);
		}

		public override int GetHashCode()
		{
			return Hash;
		}

		public override string ToString()
		{
			return "0x" + Hash.ToString("X");
		}
	}
}
