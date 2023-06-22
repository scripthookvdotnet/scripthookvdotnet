//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum VehicleType
	{
		/// <summary>
		/// The default/invalid type.
		/// </summary>
		/// <remarks>
		/// The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_NONE</c>.
		/// The hash value for <c>VEHICLE_TYPE_NONE</c> is <c>0x2F2B9BC</c> (hashed by Jenkins one-at-a-time hash but without lowercase conversion before hashing).
		/// </remarks>
		None = -1,
		/// <summary>
		/// The non-special automobile type, such as general cars, taxis, trucks, and tanks.
		/// </summary>
		/// <remarks>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_CAR</c>.</para>
		/// <para>The internal class in memory is <c>CAutomobile</c>.</para>
		/// </remarks>
		Automobile = 0x0,
		/// <summary>
		/// The airplane type.
		/// </summary>
		/// <remarks>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_PLANE</c>.</para>
		/// <para>The internal class in memory is <c>CPlane</c>, which is a subclass of <c>CAutomobile</c>.</para>
		/// </remarks>
		Plane = 0x1,
		/// <summary>
		/// The trailer type.
		/// </summary>
		/// <remarks>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_TRAILER</c>.</para>
		/// <para>The internal class in memory is <c>CTrailer</c>, which is a subclass of <c>CAutomobile</c>.</para>
		/// </remarks>
		Trailer = 0x2,
		/// <summary>
		/// The non-special quad bike type. Also includes tricycles, such as <see cref="VehicleHash.Chimera"/>, <see cref="VehicleHash.RRocket"/>, and <see cref="VehicleHash.Stryder"/>.
		/// </summary>
		/// <remarks>
		/// <para>Amphibious quad bikes are not classified as this type but classified as <see cref="AmphibiousQuadBike"/>.</para>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_QUADBIKE</c>.</para>
		/// <para>The internal class in memory is <c>CQuadBike</c>, which is a subclass of <c>CAutomobile</c>.</para>
		/// </remarks>
		QuadBike = 0x3,
		/// <summary>
		/// The submarine car type for the submarine cars, which can travel underwater like submarines.
		/// </summary>
		/// <remarks>
		/// <para>Amphibious automobiles are not classified as this type but classified as <see cref="AmphibiousAutomobile"/>.</para>
		/// <para>Submarines are not classified as this type but classified as <see cref="Submarine"/>.</para>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_SUBMARINECAR</c>.</para>
		/// <para>The internal class in memory is <c>CSubmarineCar</c>, which is a subclass of <c>CAutomobile</c>.</para>
		/// </remarks>
		SubmarineCar = 0x5,
		/// <summary>
		/// The amphibious automobile type.
		/// </summary>
		/// <remarks>
		/// <para>Submarine cars are not classified as this type but classified as <see cref="SubmarineCar"/>.</para>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_AMPHIBIOUS_AUTOMOBILE</c>.</para>
		/// <para>The internal class in memory is <c>CAmphibiousAutomobile</c>, which is a subclass of <c>CAutomobile</c>.</para>
		/// </remarks>
		AmphibiousAutomobile = 0x6,
		/// <summary>
		/// The amphibious quad bike type.
		/// </summary>
		/// <remarks>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_AMPHIBIOUS_QUADBIKE</c>.</para>
		/// <para>The internal class in memory is <c>CAmphibiousQuadBike</c>, which is a subclass of <c>CAmphibiousAutomobile</c>.</para>
		/// </remarks>
		AmphibiousQuadBike = 0x7,
		/// <summary>
		/// The helicopter type. <see cref="VehicleHash.Thruster"/> is also classified as this type.
		/// </summary>
		/// <remarks>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_HELI</c>.</para>
		/// <para>The internal class in memory is <c>CHeli</c>, which is a subclass of <c>CAutomobile</c> via <c>CRotaryWingAircraft</c>.</para>
		/// </remarks>
		Helicopter = 0x8,
		/// <summary>
		/// The blimp type.
		/// </summary>
		/// <remarks>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_BLIMP</c>.</para>
		/// <para>The internal class in memory is <c>CBlimp</c>, which is a subclass of <c>CHeli</c>.</para>
		/// </remarks>
		Blimp = 0x9,
		/// <summary>
		/// The autogyro type, which is not used in the stock game.
		/// </summary>
		/// <remarks>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_AUTOGYRO</c>.</para>
		/// <para>The internal class in memory is <c>CAutogyro</c>.</para>
		/// </remarks>
		Autogyro = 0xA,
		/// <summary>
		/// The motorcycle type.
		/// </summary>
		/// <remarks>
		/// <para>Tricycles are not classified as this type but classified as <see cref="QuadBike"/>.</para>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_BIKE</c>.</para>
		/// <para>The internal class in memory is <c>CBike</c>.</para>
		/// </remarks>
		Motorcycle = 0xB,
		/// <summary>
		/// The bicycle type.
		/// </summary>
		/// <remarks>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_BICYCLE</c>.</para>
		/// <para>The internal class in memory is <c>CBmx</c>, which is a subclass of <c>CBike</c>.</para>
		/// </remarks>
		Bicycle = 0xC,
		/// <summary>
		/// The boat type.
		/// </summary>
		/// <remarks>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_BOAT</c>.</para>
		/// <para>The internal class in memory is <c>CBoat</c>.</para>
		/// </remarks>
		Boat = 0xD,
		/// <summary>
		/// The train type.
		/// </summary>
		/// <remarks>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_TRAIN</c>.</para>
		/// <para>The internal class in memory is <c>CTrain</c>.</para>
		/// </remarks>
		Train = 0xE,
		/// <summary>
		/// The submarine type.
		/// </summary>
		/// <remarks>
		/// <para>Submarine cars are not classified as this type but classified as <see cref="SubmarineCar"/>.</para>
		/// <para>The corresponding name in vehicles.meta for this value is <c>VEHICLE_TYPE_TRAIN</c>.</para>
		/// <para>The internal class in memory is <c>CSubmarine</c></para>
		/// </remarks>
		Submarine = 0xF
	}
}
