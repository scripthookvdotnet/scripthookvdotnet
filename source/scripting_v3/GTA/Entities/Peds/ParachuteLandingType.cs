//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	public enum ParachuteLandingType
	{
		/// <summary>
		/// Ped is not in a valid parachute landing state.
		/// </summary>
		Invalid,
		Slow,
		/// <summary>
		/// Ped is landing at regular speed (they are stumbling).
		/// </summary>
		Regular,
		/// <summary>
		/// Ped is landing at fast speed (they are rolling).
		/// </summary>
		Fast,
		/// <summary>
		/// Ped is crashing (ragdolling).
		/// </summary>
		Crashing,
		Water,

		[Obsolete("ParachuteLandingType.None is obsolete, use ParachuteLandingType.Invalid instead.")]
		None = -1,
		[Obsolete("ParachuteLandingType.Stumbling is obsolete, use ParachuteLandingType.Regular instead.")]
		Stumbling = 1,
		[Obsolete("ParachuteLandingType.Rolling is obsolete, use ParachuteLandingType.Fast instead.")]
		Rolling,
		[Obsolete("ParachuteLandingType.Ragdoll is obsolete, use ParachuteLandingType.Crashing instead.")]
		Ragdoll,
	}
}
