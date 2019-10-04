//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum IntersectFlags
	{
		Everything = -1,
		Map = 1,
		MissionEntities = 2,
		Peds = 12,
		Objects = 16,
		Vegetation = 256,
	}
}
