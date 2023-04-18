//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

namespace GTA
{
	public enum VehicleWindowIndex
	{
		FrontLeftWindow,
		FrontRightWindow,
		BackLeftWindow,
		BackRightWindow,
		MiddleLeftWindow,
		MiddleRightWindow,
		Windshield,
		BackWindshield,
		[Obsolete("ExtraWindow1 is obsolete, please use MiddleLeftWindow instead.")]
		ExtraWindow1 = 4,
		[Obsolete("ExtraWindow2 is obsolete, please use MiddleRightWindow instead.")]
		ExtraWindow2 = 5,
		[Obsolete("ExtraWindow3 is obsolete, please use Windshield instead.")]
		ExtraWindow3 = 6,
		[Obsolete("ExtraWindow4 is obsolete, please use BackWindshield instead.")]
		ExtraWindow4 = 7,
	}
}
