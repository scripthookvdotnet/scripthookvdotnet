//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// An enumeration of known flags for the <c>CTaskCombat</c>.
	/// </summary>
	[Flags]
	public enum TaskGoToPointAnyMeansFlags
	{
		Default = 0,
		/// <summary>
		/// Ignores the health of the vehicle (default behaviour is to not use any vehicle with less than 600 health).
		/// </summary>
		IgnoreVehicleHealth = 1,
		/// <summary>
		/// Considers all nearby <see cref="Vehicle"/> for suitability (default behavior is to consider only
		/// the <see cref="Vehicle"/> closest to the <see cref="Ped"/>).
		/// </summary>
		ConsiderAllNearbyVehicles = 2,
		/// <summary>
		/// Performs the same tests as is done in <see cref="Vehicle.get_IsDriveable"/>.
		/// </summary>
		ProperIsDriveableCheck = 4,
		/// <summary>
		/// Instructs the <see cref="Ped"/> to remain in the <see cref="Vehicle"/> at the end,
		/// so that multiple tasks can be chained together.
		/// </summary>
		RemainInVehicleAtDestination = 8,
		/// <summary>
		/// <see cref="Ped"/>s will never abandon the <see cref="Vehicle"/> it is in.
		/// </summary>
		NeverAbandonVehicle = 16,
		/// <summary>
		/// <see cref="Ped"/>s will never abandon the <see cref="Vehicle"/> it is in if <see cref="Vehicle"/> is moving.
		/// </summary>
		NeverAbandonVehicleIfMoving = 32,
		/// <summary>
		/// <see cref="Ped"/>s will use the targeting system for threats and register any threats they can attack
		/// (rather than just using the closest targetable <see cref="Ped"/>).
		/// </summary>
		UseAITargetingForThreats = 64,
	}
}
