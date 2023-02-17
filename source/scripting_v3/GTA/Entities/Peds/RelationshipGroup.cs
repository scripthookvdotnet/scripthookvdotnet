//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	public struct RelationshipGroup : INativeValue, IEquatable<RelationshipGroup>
	{
		#region Initialize Static Properties of Frequently Used Relationship Groups
		static RelationshipGroup()
		{
			Player = new(Game.GenerateHash("PLAYER"));
			CivilianMale = new(Game.GenerateHash("CIVMALE"));
			CivilianFemale = new(Game.GenerateHash("CIVFEMALE"));
			Cop = new(Game.GenerateHash("COP"));
			SecurityGuard = new(Game.GenerateHash("SECURITY_GUARD"));
			PrivateSecurity = new(Game.GenerateHash("PRIVATE_SECURITY"));
			Fireman = new(Game.GenerateHash("FIREMAN"));
			AmbientGangLost = new(Game.GenerateHash("AMBIENT_GANG_LOST"));
			AmbientGangMexican = new(Game.GenerateHash("AMBIENT_GANG_MEXICAN"));
			AmbientGangFamily = new(Game.GenerateHash("AMBIENT_GANG_FAMILY"));
			AmbientGangBallas = new(Game.GenerateHash("AMBIENT_GANG_BALLAS"));
			AmbientGangMarabunte = new(Game.GenerateHash("AMBIENT_GANG_MARABUNTE"));
			AmbientGangCult = new(Game.GenerateHash("AMBIENT_GANG_CULT"));
			AmbientGangSalva = new(Game.GenerateHash("AMBIENT_GANG_SALVA"));
			AmbientGangWeiCheng = new(Game.GenerateHash("AMBIENT_GANG_WEICHENG"));
			AmbientGangHillbilly = new(Game.GenerateHash("AMBIENT_GANG_HILLBILLY"));
			HatesPlayer = new(Game.GenerateHash("HATES_PLAYER"));
			Hen = new(Game.GenerateHash("Hen"));
			WildAnimal = new(Game.GenerateHash("WILD_ANIMAL"));
			Shark = new(Game.GenerateHash("SHARK"));
			Cougar = new(Game.GenerateHash("COUGAR"));
			NoRelationship = new(Game.GenerateHash("NO_RELATIONSHIP"));
			Army = new(Game.GenerateHash("ARMY"));
			GuardDog = new(Game.GenerateHash("GUARD_DOG"));
			AggressiveInvestigate = new(Game.GenerateHash("AGGRESSIVE_INVESTIGATE"));
			Medic = new(Game.GenerateHash("MEDIC"));
			Cat = new(Game.GenerateHash("CAT"));

			// these relationship groups will be added by main_persistent.ysc
			FamilyMichael = new(Game.GenerateHash("RELGROUPHASH_FAMILY_M"));
			FamilyFranklin = new(Game.GenerateHash("RELGROUPHASH_FAMILY_F"));
			FamilyTrevor = new(Game.GenerateHash("RELGROUPHASH_FAMILY_T"));
		}
		#endregion

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
			Function.Call(Native.Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, relationship, Hash, targetGroup.NativeValue);

			if (bidirectionally)
			{
				Function.Call(Native.Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, relationship, targetGroup.NativeValue, Hash);
			}
		}

		public void ClearRelationshipBetweenGroups(RelationshipGroup targetGroup, Relationship relationship, bool bidirectionally = false)
		{
			Function.Call(Native.Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, relationship, Hash, targetGroup.NativeValue);

			if (bidirectionally)
			{
				Function.Call(Native.Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, relationship, targetGroup.NativeValue, Hash);
			}
		}

		public void Remove()
		{
			Function.Call(Native.Hash.REMOVE_RELATIONSHIP_GROUP, Hash);
		}

		#region Frequently Used Relationship Groups

		/// <summary>
		/// Gets the player (<c>"PLAYER"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup Player
		{
			get;
		}
		/// <summary>
		/// Gets the civilian male (<c>"CIVMALE"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup CivilianMale
		{
			get;
		}
		/// <summary>
		/// Gets the civilian female (<c>"CIVFEMALE"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup CivilianFemale
		{
			get;
		}
		/// <summary>
		/// Gets the cop (<c>"COP"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup Cop
		{
			get;
		}
		/// <summary>
		/// Gets the security guard (<c>"SECURITY_GUARD"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup SecurityGuard
		{
			get;
		}
		/// <summary>
		/// Gets the private security (<c>"PRIVATE_SECURITY"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup PrivateSecurity
		{
			get;
		}
		/// <summary>
		/// Gets the fireman/firefighter (<c>"FIREMAN"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup Fireman
		{
			get;
		}
		/// <summary>
		/// Gets the ambient gang <see cref="RelationshipGroup"/> of The Lost MC (<c>"AMBIENT_GANG_LOST"</c>).
		/// </summary>
		static public RelationshipGroup AmbientGangLost
		{
			get;
		}
		/// <summary>
		/// Gets the ambient gang <see cref="RelationshipGroup"/> of Vagos (<c>"AMBIENT_GANG_MEXICAN"</c>).
		/// </summary>
		static public RelationshipGroup AmbientGangMexican
		{
			get;
		}
		/// <summary>
		/// Gets the ambient gang <see cref="RelationshipGroup"/> of The Families (<c>"AMBIENT_GANG_FAMILY"</c>).
		/// </summary>
		static public RelationshipGroup AmbientGangFamily
		{
			get;
		}
		/// <summary>
		/// Gets the ambient gang <see cref="RelationshipGroup"/> of Ballas (<c>"AMBIENT_GANG_BALLAS"</c>).
		/// </summary>
		static public RelationshipGroup AmbientGangBallas
		{
			get;
		}
		/// <summary>
		/// Gets the ambient gang <see cref="RelationshipGroup"/> of Armenian Mob (<c>"AMBIENT_GANG_MARABUNTE"</c>).
		/// </summary>
		static public RelationshipGroup AmbientGangMarabunte
		{
			get;
		}
		/// <summary>
		/// Gets the ambient gang <see cref="RelationshipGroup"/> of Altruist Cult (<c>"AMBIENT_GANG_CULT"</c>).
		/// </summary>
		static public RelationshipGroup AmbientGangCult
		{
			get;
		}	
		/// <summary>
		/// Gets the ambient gang <see cref="RelationshipGroup"/> of Marabunta Grande (<c>"AMBIENT_GANG_SALVA"</c>).
		/// </summary>
		static public RelationshipGroup AmbientGangSalva
		{
			get;
		}
		/// <summary>
		/// Gets the ambient gang <see cref="RelationshipGroup"/> of Los Santos Triads (<c>"AMBIENT_GANG_WEICHENG"</c>).
		/// </summary>
		static public RelationshipGroup AmbientGangWeiCheng
		{
			get;
		}
		/// <summary>
		/// Gets the ambient <see cref="RelationshipGroup"/> of Rednecks (<c>"AMBIENT_GANG_HILLBILLY"</c>).
		/// </summary>
		static public RelationshipGroup AmbientGangHillbilly
		{
			get;
		}
		/// <summary>
		/// Gets the <c>"HATES_PLAYER"</c> <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup HatesPlayer
		{
			get;
		}
		/// <summary>
		/// Gets the <c>"HEN"</c> <see cref="RelationshipGroup"/>.
		/// The whale <see cref="Ped"/>s, the dolphin <see cref="Ped"/>, and the stingray <see cref="Ped"/> also belong to this <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup Hen
		{
			get;
		}
		/// <summary>
		/// Gets the wild animal <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup WildAnimal
		{
			get;
		}
		/// <summary>
		/// Gets the shark (<c>"FIREMAN"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup Shark
		{
			get;
		}
		/// <summary>
		/// Gets the cougar/mountain lion (<c>"COUGAR"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup Cougar
		{
			get;
		}
		/// <summary>
		/// Gets the (<c>"NO_RELATIONSHIP"</c>) <see cref="RelationshipGroup"/>.
		/// <see cref="Ped"/>s created by a script belongs to this <see cref="RelationshipGroup"/> until another <see cref="RelationshipGroup"/> is assigned.
		/// </summary>
		static public RelationshipGroup NoRelationship
		{
			get;
		}
		/// <summary>
		/// Gets the army (<c>"ARMY"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup Army
		{
			get;
		}
		/// <summary>
		/// Gets the army (<c>"GUARD_DOG"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup GuardDog
		{
			get;
		}
		/// <summary>
		/// Gets the <c>"AGGRESSIVE_INVESTIGATE"</c> <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup AggressiveInvestigate
		{
			get;
		}
		/// <summary>
		/// Gets the paramedics (<c>"MEDIC"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup Medic
		{
			get;
		}
		/// <summary>
		/// Gets the cat (<c>"CAT"</c>) <see cref="RelationshipGroup"/>.
		/// </summary>
		static public RelationshipGroup Cat
		{
			get;
		}
		/// <summary>
		/// Gets the <see cref="RelationshipGroup"/> for Michael's family (<c>"RELGROUPHASH_FAMILY_M"</c>).
		/// </summary>
		static public RelationshipGroup FamilyMichael
		{
			get;
		}
		/// <summary>
		/// Gets the <see cref="RelationshipGroup"/> for Franklin's family (<c>"RELGROUPHASH_FAMILY_F"</c>).
		/// </summary>
		static public RelationshipGroup FamilyFranklin
		{
			get;
		}
		/// <summary>
		/// Gets the <see cref="RelationshipGroup"/> for Trevor's family (<c>"RELGROUPHASH_FAMILY_T"</c>).
		/// </summary>
		static public RelationshipGroup FamilyTrevor
		{
			get;
		}

		#endregion

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
