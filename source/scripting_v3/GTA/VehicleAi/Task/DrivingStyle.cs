//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// An enumeration of frequently used driving styles.
	/// Consider using <see cref="VehicleDrivingFlags"/> since it represents how flags affects how <see cref="Ped"/>s should drive vehicles
	/// more accurately than this enumeration.
	/// </summary>
	[Obsolete("Use VehicleDrivingFlags instead.")]
	public enum DrivingStyle
	{
		Normal = 786603,
		IgnoreLights = 2883621,
		SometimesOvertakeTraffic = 5,
		Rushed = 1074528293,
		AvoidTraffic = 786468,
		AvoidTrafficExtremely = 6,
	}
}
