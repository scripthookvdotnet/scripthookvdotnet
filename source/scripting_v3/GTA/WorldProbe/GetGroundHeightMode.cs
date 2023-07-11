//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum GetGroundHeightMode
	{
		/// <summary>
		/// Does not consider water as ground.
		/// </summary>
		Normal,
		/// <summary>
		/// Consider water as ground and takes the waves into account.
		/// Does not make any difference from <see cref="Normal"/> in earlier game versions such as v1.0.372.2.
		/// </summary>
		ConsiderWaterAsGround,
		/// <summary>
		/// Consider water as ground but does not take the waves into account.
		/// Does not make any difference from <see cref="Normal"/> in earlier game versions such as v1.0.372.2.
		/// </summary>
		ConsiderWaterAsGroundNoWaves,
	}
}
