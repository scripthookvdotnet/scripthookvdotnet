//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum IntersectFlags
	{
		Map = 1,
		Vehicles = 2,
		/// <summary>
		/// Detect <see cref="Ped"/> who are not ragdolled (not running any NM tasks) by detecting the simple capsule shape of <see cref="Ped"/>.
		/// </summary>
		PedCapsules = 4,
		/// <summary>
		/// Detect <see cref="Ped"/>'s ragdoll. Can detect those who are not ragdolled.
		/// </summary>
		Ragdolls = 8,
		Peds = 12,
		Objects = 16,
		Pickups = 32,
		Glass = 64,
		Rivers = 128,
		/// <summary>
		/// Detect foliage, which can be affected by the wind or contacts of <see cref="Entity"/>.
		/// </summary>
		Foliage = 256,
		Everything = 511,
		BoundingBox = Vehicles | PedCapsules | Ragdolls | Objects | Pickups | Glass,

		[Obsolete("IntersectFlags.MissionEntities is obsolete because it is incorrect, use IntersectFlags.Vehicles instead.")]
		MissionEntities = Vehicles,
		[Obsolete("IntersectFlags.Vegetation is obsolete because it is incorrect, use IntersectFlags.Foliage instead.")]
		Vegetation = Foliage,
	}
}
