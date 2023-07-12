//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum CombatMovement
	{
		/// <summary>
		/// Stands totally still during combat.
		/// </summary>
		Stationary,
		/// <summary>
		/// Seeks a defensive position.
		/// </summary>
		Defensive,
		/// <summary>
		/// Will advance forward in combat.
		/// </summary>
		WillAdvance,
		/// <summary>
		/// Will retreat if the enemy gets too close.
		/// </summary>
		WillRetreat
	}
}
