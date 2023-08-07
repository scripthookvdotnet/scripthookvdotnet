//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;

namespace GTA
{
	public enum Formation
	{
		/// <summary>
		/// The default value.
		/// </summary>
		Loose,
		SurroundFacingInwards,
		SurroundFacingAhead,
		LineAbreast,
		FollowInLine,

		[Obsolete("Use Formation.Loose instead."), EditorBrowsable(EditorBrowsableState.Never)]
		Default = Loose,
		[Obsolete("Use Formation.SurroundFacingInwards instead."), EditorBrowsable(EditorBrowsableState.Never)]
		Circle1 = SurroundFacingInwards,
		[Obsolete("Use Formation.SurroundFacingAhead instead."), EditorBrowsable(EditorBrowsableState.Never)]
		Circle2 = SurroundFacingAhead,
		[Obsolete("Use Formation.FollowInLine instead."), EditorBrowsable(EditorBrowsableState.Never)]
		Line = FollowInLine,
	}
}
