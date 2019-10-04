//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum EnterVehicleFlags
	{
		None = 0,
		WarpToDoor = 2,
		AllowJacking = 8,
		WarpIn = 16,
		EnterFromOppositeSide = 262144,
		OnlyOpenDoor = 524288,
	}
}
