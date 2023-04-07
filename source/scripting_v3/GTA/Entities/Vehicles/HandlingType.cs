//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// An Enumeration of all possible sub handling type.
	/// </summary>
	/// <remarks>
	/// You can confirm if the listed names are correct by searching some dumped exe for the hashed values from strings like <c>HANDLING_TYPE_FLYING</c> (without case conversion).
	/// </remarks>
	public enum HandlingType
	{
		Bike,
		Flying,
		VerticalFlying,
		Boat,
		SeaPlane,
		Submarine,
		Train,
		Trailer,
		Car,
		Weapon,
		SpecialFlight,
		Invalid = -1,
	}
}
