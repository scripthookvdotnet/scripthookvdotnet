//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum CamInVehicleState
	{
		/// <summary>
		/// Gameplay camera in interpolating to the follow vehicle camera.
		/// </summary>
		EnteringVehicle,
		/// <summary>
		/// Gameplay camera is running the follow vehicle camera.
		/// </summary>
		InsideVehicle,
		/// <summary>
		/// Gameplay camera is interpolating from the follow vehicle camera to the follow ped camera.
		/// </summary>
		ExitingVehicle,
		/// <summary>
		/// Gameplay camera is fully running the follow ped camera,
		/// do not need to specify a <see cref="Vehicle"/> when specifying this value.
		/// </summary>
		OutsideVehicle
	}
}
