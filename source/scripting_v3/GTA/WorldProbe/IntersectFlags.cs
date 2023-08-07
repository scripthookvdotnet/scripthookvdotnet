//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;

namespace GTA
{
	[Flags]
	public enum IntersectFlags
	{
		Map = 1,
		Vehicles = 2,
		/// <summary>
		/// Detects <see cref="Ped"/> who are not ragdolled (not running any NM tasks) by detecting the simple capsule shape of <see cref="Ped"/>.
		/// </summary>
		PedCapsules = 4,
		/// <summary>
		/// Detects <see cref="Ped"/>'s ragdoll. Can detect those who are not ragdolled.
		/// </summary>
		Ragdolls = 8,
		Peds = PedCapsules | Ragdolls,
		/// <summary>
		/// Detects <see cref="Prop"/>s.
		/// </summary>
		Objects = 16,
		Pickups = 32,
		Glass = 64,
		Rivers = 128,
		/// <summary>
		/// Detects foliage, which can be affected by the wind or contacts of <see cref="Entity"/>.
		/// </summary>
		Foliage = 256,
		Everything = 511,

		LosToEntity = Map | Objects,
		BoundingBox = Vehicles | PedCapsules | Ragdolls | Objects | Pickups | Glass,

		[Obsolete("IntersectFlags.MissionEntities is obsolete because it is incorrect, use IntersectFlags.Vehicles instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		MissionEntities = Vehicles,
		[Obsolete("IntersectFlags.Vegetation is obsolete because it is incorrect, use IntersectFlags.Foliage instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		Vegetation = Foliage,
	}
}
