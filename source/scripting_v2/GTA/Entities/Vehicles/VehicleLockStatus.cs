//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum VehicleLockStatus
	{
		None,
		Unlocked,
		Locked,
		LockedForPlayer,
		/// <summary>
		/// Doesn't allow players to exit the vehicle with the exit vehicle key.
		/// </summary>
		StickPlayerInside,
		/// <summary>
		/// Can be broken into the car. If the glass is broken, the value will be set to 1.
		/// </summary>
		CanBeBrokenInto = 7,
		CanBeBrokenIntoPersist,
		CannotBeTriedToEnter = 10,
	}
}
