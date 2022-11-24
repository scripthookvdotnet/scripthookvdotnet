//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum DrawBoxFlags
	{
		OutsideOnly = 1,
		InsideOnly = 2,
		BothSides = OutsideOnly | InsideOnly
	}
}
