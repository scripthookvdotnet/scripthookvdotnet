//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum NewLoadSceneFlags
	{
		RequireCollision = 1,
		LongSwitchCutscene = 2,
		InteriorAndExterior = 4,
	}
}
