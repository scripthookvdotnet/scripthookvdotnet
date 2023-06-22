//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Set of enumerations of the available park types for <see cref="TaskInvoker.ParkVehicle(Vehicle, Math.Vector3, float, float, bool)"/>.
	/// </summary>
	public enum ParkType
	{
		Parallel,
		PerpendicularNoseIn,
		PerpendicularBackIn,
		PullOver,
		LeaveParallelSpace,
		BackOutPerpendicularSpace,
		PassengerExit,
		PullOverImmediate,
	}
}
