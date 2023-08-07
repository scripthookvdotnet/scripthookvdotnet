//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;

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

		[Obsolete("Use ParachuteLandingType.Invalid instead."), EditorBrowsable(EditorBrowsableState.Never)]
		None = -1,
		[Obsolete("Use ParachuteLandingType.Regular instead."), EditorBrowsable(EditorBrowsableState.Never)]
		Stumbling = 1,
		[Obsolete("Use ParachuteLandingType.Fast instead."), EditorBrowsable(EditorBrowsableState.Never)]
		Rolling,
		[Obsolete("Use ParachuteLandingType.Crashing instead."), EditorBrowsable(EditorBrowsableState.Never)]
		Ragdoll,
	}
}
