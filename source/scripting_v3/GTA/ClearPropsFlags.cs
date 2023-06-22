//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum ClearPropsFlags
	{
		/// <summary>
		/// Force to respawn new ambient <see cref="Prop"/>s even if the player is too close for them to respawn without possible collision with the player.
		/// </summary>
		ForceRespawnAmbientProps = 2,
		/// <summary>
		/// Clears door <see cref="Prop"/>s that are managed by native door system.
		/// </summary>
		/// <remarks>
		/// Strictly, this flag lets <see cref="World.ClearAreaOfProps(Math.Vector3, float, ClearPropsFlags)"/> clear <see cref="Prop"/>s that uses the <c>CDoor</c> class (a subclass of <c>CObject</c>) that are managed by native door system.
		/// Door <see cref="Prop"/>s created as a regular <c>CObject</c> can be deleted without this flag.
		/// </remarks>
		IncludeDoors = 4,
		/// <summary>
		/// Clears props with brain ysc scripts, which can be attached with <c>REGISTER_OBJECT_SCRIPT_BRAIN</c>.
		/// For example, register props can be cleared if this flag is set.
		/// </summary>
		IncludePropsWithScriptBrains = 8,
		/// <summary>
		/// Does not clear ladders.
		/// </summary>
		ExcludeLadder = 16,
	}
}
