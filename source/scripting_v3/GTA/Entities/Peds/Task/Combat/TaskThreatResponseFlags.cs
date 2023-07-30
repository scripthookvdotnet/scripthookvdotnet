//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// An enumeration of known flags for the <c>CTaskThreatResponse</c>.
	/// </summary>
	[Flags]
	public enum TaskThreatResponseFlags
	{
		None = 0,
		CanFightArmedPedsWhenNotArmed = 16,
	}
}
