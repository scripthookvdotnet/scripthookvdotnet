//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum ScriptVehicleWheelIndex
	{
		/// <summary>
		/// The invalid value for <see cref="VehicleWheel.ScriptIndex"/>.
		/// Do not directly use this value for native functions.
		/// </summary>
		Invalid = -1,
		CarFrontLeft,
		CarFrontRight,
		CarMidLeft,
		CarMidRight,
		CarRearLeft,
		CarRearRight,
		/// <remarks>
		/// Internally specifies the same wheel index as <see cref="CarFrontLeft"/>.
		/// <see cref="VehicleWheel.ScriptIndex"/> will never return this value when you get a <see cref="VehicleWheel"/>
		/// via <see cref="VehicleWheelCollection.this[VehicleWheelBoneId]"/> and will return <see cref="CarFrontLeft"/> instead.
		/// </remarks>
		BikeFront,
		/// <remarks>
		/// Internally specifies the same wheel index as <see cref="CarRearLeft"/>.
		/// <see cref="VehicleWheel.ScriptIndex"/> will never return this value when you get a <see cref="VehicleWheel"/>
		/// via <see cref="VehicleWheelCollection.this[VehicleWheelBoneId]"/> and will return <see cref="CarRearLeft"/> instead.
		/// </remarks>
		BikeRear
	}
}
