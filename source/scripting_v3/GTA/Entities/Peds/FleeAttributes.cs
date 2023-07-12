//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// An enumeration of known flee attributes for the <see cref="Ped"/>,
	/// which is used to set or unset the flee attributes on a <c>CPedIntelligence</c> of <c>CPed</c>.
	/// </summary>
	/// <remarks>
	/// You can set or unset multiple attributes as <c>SET_PED_FLEE_ATTRIBUTES</c> set or unset the bits
	/// of flee attributes by just using simple bitwise operation (adds bits if the bool parameter is set,
	/// removes bits if the bool is not set).
	/// </remarks>
	[Flags]
	public enum FleeAttributes
	{
		UseCover = 1,
		UseVehicle = 2,
		/// <remarks>
		/// Set by default on <see cref="Ped"/> creation if the ped model definition, which is instantiated from
		/// a <c>peds.ymt</c> or <c>peds.meta</c> file (exists as a <c>CPedModelInfo</c> in the game memory), has
		/// a reference to <c>CTaskDataInfo</c> (defined in <c>taskdata.meta</c>) where the <c>Flags</c> field has
		/// <c>CanScreamDuringFlee</c>.
		/// </remarks>
		CanScream = 4,
		/// <remarks>
		/// Set by default on <see cref="Ped"/> creation if the ped model definition, which is instantiated from
		/// a <c>peds.ymt</c> or <c>peds.meta</c> file (exists as a <c>CPedModelInfo</c> in the game memory), has
		/// a reference to <c>CTaskDataInfo</c> (defined in <c>taskdata.meta</c>) where the <c>Flags</c> field has
		/// <c>PreferFleeOnPavements</c>.
		/// </remarks>
		PreferPavements = 8,
		WanderAtEnd = 16,
		LookForCrowds = 32,
		ReturnToOriginalPositionAfterFlee = 64,
		DisableHandsUp = 128,
		UpdateToNearestHatedPed = 256,
		NeverFlee = 512,
		DisableCover = 1024,
		DisableExitVehicle = 2048,
		DisableReverseInVehicle = 4096,
		DisableAccelerateInVehicle = 8192,
		DisableFleeFromIndirectThreats = 16384,
		CowerInsteadOfFlee = 32768,
		ForceExitVehicle = 65536,
		DisableHesitateInVehicle = 131072,
		DisableAmbientClips = 262144,
	}
}
