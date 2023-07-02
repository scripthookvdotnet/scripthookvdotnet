//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// An enumeration of hashes of cam hint helper names.
	/// </summary>
	/// <remarks>
	/// You can find name hashes for camHintHelperMetadata in <c>cameras.ymt</c>
	/// </remarks>
	public enum CameraHintHelperNameHash : uint
	{
		/// <summary>
		/// The gameplay director will use the hint helper set to the current hint metadata
		/// if this value is specified for <see cref="GameplayCamera.SetCoordHint"/> or
		/// <see cref="GameplayCamera.SetEntityHint"/>.
		/// </summary>
		None = 0,
		/// <remarks>
		/// The original name is <c>DEFAULT_HINT_HELPER</c>, which does not appear in ysc scripts.
		/// </remarks>
		DefaultHintHelper = 0x6B83254B,
		/// <remarks>
		/// The original name is <c>SKY_DIVING_HINT_HELPER</c>, which does not appear in ysc scripts.
		/// </remarks>
		SkyDivingHintHelper = 0xD4A14726,
		/// <remarks>
		/// The original name is <c>VEHICLE_HINT_HELPER</c>, which does not appear in ysc scripts.
		/// </remarks>
		VehicleHintHelper = 0xACADA6B,
		NoFovHintHelper = 0x5A17CB40,
		VehicleHighZoomHintHelper = 0x66EADDF5,
		/// <remarks>
		/// "arm3" or "armenian3" means the mission "Complications".
		/// </remarks>
		Arm3VehicleHintHelper = 0x66EADDF5,
		/// <remarks>
		/// "agency heist3b" means the mission "The Bureau Raid".
		/// </remarks>
		AgencyHeist3BSkyDivingHintHelper = 0x484D2486,
		/// <remarks>
		/// "family3" means the mission "Marriage Counseling".
		/// </remarks>
		Family3HouseVehicleHintHelper = 0xBD03944C,
		/// <remarks>
		/// "arm2" or "armenian2" means the mission "Repossession".
		/// "mcs6" eventually specifies the final cutscene for said mission.
		/// </remarks>
		Arm2Mcs6VehicleHintHelper = 0x77D59397,
		KillerCamHintHelper = 0x6DF7FDE1,
		/// <remarks>
		/// "family3" means the mission "Marriage Counseling".
		/// </remarks>
		Family3CoachOnBalconyVehicleHintHelper = 0xFAC492F0,
		ChopHintHelper = 0xB833D00,
	}
}
