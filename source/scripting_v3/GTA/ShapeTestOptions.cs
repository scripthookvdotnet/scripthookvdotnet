//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum ShapeTestOptions
	{
		IgnoreGlass = 1,
		IgnoreSeeThrough = 2,
		IgnoreNoCollision = 4,
		Default = IgnoreGlass | IgnoreSeeThrough | IgnoreNoCollision,
	}
}
