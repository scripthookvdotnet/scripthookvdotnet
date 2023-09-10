//
// Copyright (C) 2023 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// A set of flags to define how <see cref="Ped"/>s should behave in vehicle chases.
	/// </summary>
	[Flags]
	public enum VehicleChaseBehaviorFlags : uint
	{
		CantBlock = 1,
		CantBlockFromPursue = 2,
		CantPursue = 4,
		CantRam = 8,
		CantSpinOut = 16,
		CantMakeAggressiveMove = 32,
		CantCruiseInFrontDuringBlock = 64,
		UseContinuousRam = 128,
		CantPullAlongside = 256,
		CantPullAlongsideInFront = 512,
	}
}
