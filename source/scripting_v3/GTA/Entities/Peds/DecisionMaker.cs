//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a decision maker for <see cref="Ped"/>s, which determines what and how <see cref="Ped"/>s should response to events.
	/// Events can cause <see cref="Ped"/>s to start certain tasks. You can see how decision makers are configured in <c>events.meta</c>.
	/// </summary>
	public struct DecisionMaker : INativeValue, IEquatable<DecisionMaker>
	{
		DecisionMaker(string name) : this()
		{
			Hash = (DecisionMakerTypeHash)Game.GenerateHash(name);
		}
		public DecisionMaker(DecisionMakerTypeHash hash) : this()
		{
			Hash = hash;
		}
		public DecisionMaker(int hash) : this((DecisionMakerTypeHash)hash)
		{
			
		}
		public DecisionMaker(uint hash) : this((DecisionMakerTypeHash)hash)
		{
		}

		/// <summary>
		/// Gets the hash for this <see cref="DecisionMaker"/>.
		/// </summary>
		public DecisionMakerTypeHash Hash
		{
			get; private set;
		}

		/// <summary>
		/// Gets the native representation of this <see cref="DecisionMaker"/>.
		/// </summary>
		public ulong NativeValue
		{
			get => (ulong)Hash;
			set => Hash = (DecisionMakerTypeHash)unchecked((int)value);
		}

		/// <summary>
		/// Indicates whether <see cref="Hash"/> of this <see cref="DecisionMaker"/> is set to <c>0</c>, which can be created by <see langword="default"/> operator.
		/// </summary>
		public bool IsNullValue => (Hash == 0);

		public bool Equals(DecisionMaker group)
		{
			return Hash == group.Hash;
		}
		public override bool Equals(object obj)
		{
			if (obj is DecisionMaker group)
			{
				return Equals(group);
			}

			return false;
		}

		public static bool operator ==(DecisionMaker left, DecisionMaker right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(DecisionMaker left, DecisionMaker right)
		{
			return !(left == right);
		}

		public static implicit operator DecisionMaker(int source)
		{
			return new DecisionMaker(source);
		}
		public static implicit operator DecisionMaker(uint source)
		{
			return new DecisionMaker(source);
		}
		public static implicit operator DecisionMaker(DecisionMakerTypeHash source)
		{
			return new DecisionMaker(source);
		}
		public static implicit operator DecisionMaker(string source)
		{
			return new DecisionMaker(source);
		}

		public static implicit operator InputArgument(DecisionMaker value)
		{
			return new InputArgument((ulong)value.Hash);
		}

		public override int GetHashCode()
		{
			return (int)Hash;
		}

		public override string ToString()
		{
			return Hash.ToString();
		}
	}
}
