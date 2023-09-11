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
		/// <summary>
		/// The <see cref="Ped"/> will not stop combating the target even if the target uses firearms but the
		/// <see cref="Ped"/> who's executing the task does not have firearms with ammunition.
		/// </summary>
		CanFightArmedPedsWhenNotArmed = 16,
	}
}
