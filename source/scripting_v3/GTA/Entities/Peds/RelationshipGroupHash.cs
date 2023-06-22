//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// An enumeration of hashes ambient peds use and ones for 3 families of protagonists.
	/// </summary>
	public enum RelationshipGroupHash : uint
	{
		/// <summary>
		/// The player (<c>"PLAYER"</c>) relationship group hash.
		/// </summary>
		Player = 0x6F0783F5,
		/// <summary>
		/// The civilian male (<c>"CIVMALE"</c>) relationship group hash.
		/// </summary>
		CivilianMale = 0x02B8FA80,
		/// <summary>
		/// The civilian female (<c>"CIVFEMALE"</c>) relationship group hash.
		/// </summary>
		CivilianFemale = 0x47033600,
		/// <summary>
		/// The cop (<c>"COP"</c>) relationship group hash.
		/// </summary>
		Cop = 0xA49E591C,
		/// <summary>
		/// The security guard (<c>"SECURITY_GUARD"</c>) relationship group hash.
		/// </summary>
		SecurityGuard = 0xF50B51B7,
		/// <summary>
		/// The private security (<c>"PRIVATE_SECURITY"</c>) relationship group hash.
		/// </summary>
		PrivateSecurity = 0xA882EB57,
		/// <summary>
		/// The fireman/firefighter (<c>"FIREMAN"</c>) relationship group hash.
		/// </summary>
		Fireman = 0xFC2CA767,
		/// <summary>
		/// The ambient gang relationship group hash of The Lost MC (<c>"AMBIENT_GANG_LOST"</c>).
		/// </summary>
		AmbientGangLost = 0x90C7DA60,
		/// <summary>
		/// The ambient gang relationship group hash of Vagos (<c>"AMBIENT_GANG_MEXICAN"</c>).
		/// </summary>
		AmbientGangMexican = 0x11A9A7E3,
		/// <summary>
		/// The ambient gang relationship group hash of The Families (<c>"AMBIENT_GANG_FAMILY"</c>).
		/// </summary>
		AmbientGangFamily = 0x45897C40,
		/// <summary>
		/// The ambient gang relationship group hash of Ballas (<c>"AMBIENT_GANG_BALLAS"</c>).
		/// </summary>
		AmbientGangBallas = 0xC26D562A,
		/// <summary>
		/// The ambient gang relationship group hash of Armenian Mob (<c>"AMBIENT_GANG_MARABUNTE"</c>).
		/// </summary>
		AmbientGangMarabunte = 0x7972FFBD,
		/// <summary>
		/// The ambient gang relationship group hash of Altruist Cult (<c>"AMBIENT_GANG_CULT"</c>).
		/// </summary>
		AmbientGangCult = 0x783E3868,
		/// <summary>
		/// The ambient gang relationship group hash of Marabunta Grande (<c>"AMBIENT_GANG_SALVA"</c>).
		/// </summary>
		AmbientGangSalva = 0x936E7EFB,
		/// <summary>
		/// The ambient gang relationship group hash of Los Santos Triads (<c>"AMBIENT_GANG_WEICHENG"</c>).
		/// </summary>
		AmbientGangWeiCheng = 0x6A3B9F86,
		/// <summary>
		/// The ambient gang relationship group hash of Rednecks (<c>"AMBIENT_GANG_HILLBILLY"</c>).
		/// </summary>
		AmbientGangHillbilly = 0xB3598E9C,
		/// <summary>
		/// The <c>"HATES_PLAYER"</c> relationship group hash.
		/// </summary>
		HatesPlayer = 0x84DCFAAD,
		/// <summary>
		/// The <c>"HEN"</c> relationship group hash.
		/// The whale <see cref="Ped"/>s, the dolphin <see cref="Ped"/>, and the stingray <see cref="Ped"/> also belong to this relationship group.
		/// </summary>
		Hen = 0xC01035F9,
		/// <summary>
		/// Gets the wild animal relationship group hash (<c>"WILD_ANIMAL"</c>).
		/// </summary>
		WildAnimal = 0x7BEA6617,
		/// <summary>
		/// Gets the shark (<c>"SHARK"</c>) relationship group hash.
		/// </summary>
		Shark = 0x229503C8,
		/// <summary>
		/// Gets the cougar/mountain lion (<c>"COUGAR"</c>) relationship group hash.
		/// </summary>
		Cougar = 0xCE133D78,
		/// <summary>
		/// The relationship group hash for <c>"NO_RELATIONSHIP"</c>.
		/// <see cref="Ped"/>s created by a script belongs to this hash until another relationship group hash is assigned.
		/// </summary>
		NoRelationship = 0xFADE4843,
		/// <summary>
		/// The army (<c>"ARMY"</c>) relationship group hash.
		/// </summary>
		Army = 0xE3D976F3,
		/// <summary>
		/// The guard dog (<c>"GUARD_DOG"</c>) relationship group hash.
		/// </summary>
		GuardDog = 0x522B964A,
		/// <summary>
		/// The <c>"AGGRESSIVE_INVESTIGATE"</c> relationship group hash.
		/// </summary>
		AggressiveInvestigate = 0xEB47D4E0,
		/// <summary>
		/// The paramedics (<c>"MEDIC"</c>) relationship group hash.
		/// </summary>
		Medic = 0xB0423AA0,
		/// <summary>
		/// The cat (<c>"CAT"</c>) relationship group hash.
		/// </summary>
		Cat = 0x4503A9A9,
		/// <summary>
		/// The relationship group hash for Michael's family (<c>"RELGROUPHASH_FAMILY_M"</c>).
		/// </summary>
		FamilyMichael = 0x8540F5A2,
		/// <summary>
		/// The relationship group hash for Franklin's family (<c>"RELGROUPHASH_FAMILY_F"</c>).
		/// </summary>
		FamilyFranklin = 0x79455D9F,
		/// <summary>
		/// The relationship group hash for Trevor's family (<c>"RELGROUPHASH_FAMILY_T"</c>).
		/// </summary>
		FamilyTrevor = 0x2B10C143,
	}
}
