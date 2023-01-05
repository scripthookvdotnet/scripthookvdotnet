//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;

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

		[Obsolete("Formation.Default is obsolete, use Formation.Loose instead.")]
		Default = 0,
		[Obsolete("Formation.Circle1 is obsolete, use Formation.SurroundFacingInwards instead.")]
		Circle1,
		[Obsolete("Formation.Circle2 is obsolete, use Formation.SurroundFacingAhead instead.")]
		Circle2,
		[Obsolete("Formation.Line is obsolete, use Formation.FollowInLine instead.")]
		Line,
	}
}
