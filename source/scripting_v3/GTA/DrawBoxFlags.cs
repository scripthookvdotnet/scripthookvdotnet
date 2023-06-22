//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
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
