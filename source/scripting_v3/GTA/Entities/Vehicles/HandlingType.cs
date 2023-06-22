//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// An Enumeration of all possible sub handling type.
	/// </summary>
	/// <remarks>
	/// You can confirm if the listed names are correct by searching some dumped exe for the hashed values from strings like <c>HANDLING_TYPE_FLYING</c> (without case conversion).
	/// The exe has the hashed value of <c>HANDLING_TYPE_TRAIN</c>, which is <c>0xE0F200C3</c>, but it does not have a concrete class for train handling data.
	/// </remarks>
	public enum HandlingType
	{
		Bike,
		Flying,
		VerticalFlying,
		Boat,
		SeaPlane,
		Submarine,
		Trailer = 7,
		Car,
		Weapon,
		SpecialFlight,
	}
}
