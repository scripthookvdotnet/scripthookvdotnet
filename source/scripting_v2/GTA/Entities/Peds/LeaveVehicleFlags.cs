//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum LeaveVehicleFlags
	{
		None = 0,
		WarpOut = 16,
		LeaveDoorOpen = 256,
		BailOut = 4096,
	}
}
