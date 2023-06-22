//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum GetClosestVehicleNodeFlags
	{
		None = 0,
		IncludeSwitchedOffNodes = 1,
		IncludeBoatNodes = 2,
		IgnoreSlipLanes = 4,
		IgnoreSwitchedOffDeadEnds = 8,
	}
}
