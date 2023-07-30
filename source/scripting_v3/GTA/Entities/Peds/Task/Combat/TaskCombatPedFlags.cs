//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// An enumeration of known flags for the <c>CTaskCombat</c>.
	/// </summary>
	[Flags]
	public enum TaskCombatFlags
	{
		None = 0,
		PreventChangingTarget = 67108864,
		DisableAimIntro = 134217728,
	}
}
