//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;

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

		[Obsolete("Use MiddleLeftWindow instead."), EditorBrowsable(EditorBrowsableState.Never)]
		ExtraWindow1 = MiddleLeftWindow,
		[Obsolete("Use MiddleRightWindow instead."), EditorBrowsable(EditorBrowsableState.Never)]
		ExtraWindow2 = MiddleRightWindow,
		[Obsolete("Use Windshield instead."), EditorBrowsable(EditorBrowsableState.Never)]
		ExtraWindow3 = Windshield,
		[Obsolete("Use BackWindshield instead."), EditorBrowsable(EditorBrowsableState.Never)]
		ExtraWindow4 = BackWindshield,
	}
}
