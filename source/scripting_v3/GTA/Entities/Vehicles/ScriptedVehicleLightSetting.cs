//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// An enumeration of all possible values of scripted vehicle light setting.
	/// </summary>
	public enum ScriptedVehicleLightSetting
	{
		NoVehicleLightOverride,
		ForceVehicleLightsOff,
		ForceVehicleLightsOn,
		/// <summary>
		/// Sets vehicle lights on.
		/// </summary>
		SetVehicleLightsOn,
		/// <summary>
		/// Sets vehicle lights off.
		/// Note that next light switch will turn on the full beam if previous state was beam on (either regular or full).
		/// </summary>
		SetVehicleLightsOff
	}
}
