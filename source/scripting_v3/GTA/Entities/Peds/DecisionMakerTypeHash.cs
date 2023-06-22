//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// An enumeration of possible decision maker type hashes.
	/// The max number of decision makers are hardcoded to <c>25</c>.
	/// </summary>
	public enum DecisionMakerTypeHash : uint
	{
		/// <summary>
		/// The player (<c>"PLAYER"</c>) decision maker type hash, which has no event responses just like <see cref="Empty"/> unless modified.
		/// </summary>
		Player = 0x6F0783F5,
		/// <summary>
		/// The cop (<c>"COP"</c>) decision maker type hash.
		/// </summary>
		Cop = 0xA49E591C,
		/// <summary>
		/// The fireman/firefighter (<c>"FIREMAN"</c>) decision maker type hash.
		/// </summary>
		Fireman = 0xFC2CA767,
		/// <summary>
		/// The on-duty paramedics (<c>"MEDIC"</c>) decision maker type hash.
		/// </summary>
		Medic = 0xB0423AA0,
		/// <summary>
		/// The off duty Emergency Medical Technician (<c>"OFFDUTY_EMT"</c>) decision maker type hash.
		/// </summary>
		OffDutyEmt = 0x2D15038A,
		/// <summary>
		/// The security (<c>"SECURITY"</c>) decision maker type hash.
		/// </summary>
		Security = 0xC014351C,
		/// <summary>
		/// The NOOSE (<c>"SWAT"</c>) decision maker type hash.
		/// </summary>
		Swat = 0x98787966,
		/// <summary>
		/// The decision maker type hash which is empty (<c>"EMPTY"</c>).
		/// </summary>
		Empty = 0xBBD57BED,
		/// <summary>
		/// The decision maker type hash which is the base/parent of certain other decision makers (<c>"BASE"</c>).
		/// This decision maker is the parent of <see cref="Cop"/>, <see cref="Security"/>, <see cref="Default"/>, and <see cref="Gang"/>.
		/// </summary>
		Base = 0x44E21C90,
		/// <summary>
		/// The default (<c>"DEFAULT"</c>) decision maker type hash.
		/// </summary>
		Default = 0xE4DF46D5,
		/// <summary>
		/// The gang (<c>"GANG"</c>) decision maker type hash.
		/// </summary>
		Gang = 0xBC066B98,
		/// <summary>
		/// The decision maker type hash for The Families (<c>"FAMILY"</c>). The parent of this decision maker is <see cref="Gang"/>.
		/// </summary>
		Family = 0x1736D49F,
		/// <summary>
		/// The gull (<c>"GULL"</c>) decision maker type hash.
		/// </summary>
		Gull = 0x34032066,
		/// <summary>
		/// The hen (<c>"HEN"</c>) decision maker type hash.
		/// </summary>
		Hen = 0xC01035F9,
		/// <summary>
		/// Gets the rat (<c>"RAT"</c>) decision maker type hash.
		/// </summary>
		Rat = 0x22050993,
		/// <summary>
		/// Gets the fish (<c>"FISH"</c>) decision maker type hash.
		/// </summary>
		Fish = 0x612920B7,
		/// <summary>
		/// Gets the shark (<c>"SHARK"</c>) decision maker type hash.
		/// </summary>
		Shark = 0x229503C8,
		/// <summary>
		/// Gets the horse (<c>"HORSE"</c>) decision maker type hash.
		/// </summary>
		Horse = 0x95A6F147,

		// the all values below except Dog are written in PascalCase in events.meta

		/// <summary>
		/// Gets the domestic animal (<c>"DomesticAnimal"</c>) decision maker type hash.
		/// </summary>
		DomesticAnimal = 0x35A13E37,
		/// <summary>
		/// Gets the domestic animal (<c>"DOG"</c>) decision maker type hash.
		/// </summary>
		Dog = 0x8B09733B,
		/// <summary>
		/// The wild animal  (<c>"WildAnimal"</c>) decision maker type hash.
		/// </summary>
		WildAnimal = 0x878D0AD0,
		/// <summary>
		/// The cougar/mountain lion (<c>"Cougar"</c>) decision maker type hash.
		/// </summary>
		Cougar = 0xCE133D78,
		/// <summary>
		/// The small animal (<c>"SmallAnimal"</c>) decision maker type hash.
		/// </summary>
		SmallAnimal = 0xAEF97295,
		/// <summary>
		/// The cat (<c>"Cat"</c>) decision maker type hash.
		/// </summary>
		Cat = 0x4503A9A9,
		/// <summary>
		/// The rabbit (<c>"Rabbit"</c>) decision maker type hash.
		/// </summary>
		Rabbit = 0xCD3B55FA,
	}
}
