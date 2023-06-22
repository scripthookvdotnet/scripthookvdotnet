//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

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
		ExtraWindow1 = MiddleLeftWindow,
		[Obsolete("ExtraWindow2 is obsolete, please use MiddleRightWindow instead.")]
		ExtraWindow2 = MiddleRightWindow,
		[Obsolete("ExtraWindow3 is obsolete, please use Windshield instead.")]
		ExtraWindow3 = Windshield,
		[Obsolete("ExtraWindow4 is obsolete, please use BackWindshield instead.")]
		ExtraWindow4 = BackWindshield,
	}
}
