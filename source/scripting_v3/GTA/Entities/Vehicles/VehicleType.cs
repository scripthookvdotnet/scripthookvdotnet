//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

namespace GTA
{
	public enum VehicleType
	{
		Invalid = -1,
		/// <summary>
		/// The general automobile type, such as general cars, taxis, trucks, and tanks.
		/// </summary>
		/// <remarks>The internal class in memory is <c>CAutomobile</c>.</remarks>
		Automobile = 0x0,
		/// <summary>
		/// The plane type.
		/// </summary>
		/// <remarks>The internal class in memory is <c>CPlane</c>, which is a subclass of <c>CAutomobile</c>.</remarks>
		Plane = 0x1,
		/// <summary>
		/// The trailer type.
		/// </summary>
		/// <remarks>The internal class in memory is <c>CTrailer</c>, which is a subclass of <c>CAutomobile</c>.</remarks>
		Trailer = 0x2,
		/// <summary>
		/// The general quad bike type.
		/// </summary>
		/// <remarks>
		/// <para>Amphibious quad bikes are not classified as this type but classified as <see cref="AmphibiousQuadBike" />.</para>
		/// <para>The internal class in memory is <c>CQuadBike</c>, which is a subclass of <c>CAutomobile</c>.</para>
		/// </remarks>
		QuadBike = 0x3,
		/// <summary>
		/// The submarine car type for the submarine cars, which can travel underwater like submarines.
		/// </summary>
		/// <remarks>
		/// <para>Amphibious automobiles are not classified as this type but classified as <see cref="AmphibiousAutomobile" />.</para>
		/// <para>Submarines are not classified as this type but classified as <see cref="Submarine" />.</para>
		/// <para>The internal class in memory is <c>CSubmarineCar</c>, which is a subclass of <c>CAutomobile</c>.</para>
		/// </remarks>
		SubmarineCar = 0x5,
		/// <summary>
		/// The amphibious automobile type.
		/// </summary>
		/// <remarks>
		/// <para>Submarine cars are not classified as this type but classified as <see cref="SubmarineCar" />.</para>
		/// <para>The internal class in memory is <c>CAmphibiousAutomobile</c>, which is a subclass of <c>CAutomobile</c>.</para>
		/// </remarks>
		AmphibiousAutomobile = 0x6,
		/// <summary>
		/// The amphibious quad bike type.
		/// </summary>
		/// <remarks>The internal class in memory is <c>CAmphibiousQuadBike</c>, which is a subclass of <c>CAmphibiousAutomobile</c>.</remarks>
		AmphibiousQuadBike = 0x7,
		/// <summary>
		/// The helicopter type. <see cref="VehicleHash.Thruster" /> is also classified as this type.
		/// </summary>
		/// <remarks>The internal class in memory is <c>CHeli</c>, which is a subclass of <c>CAutomobile</c> via <c>CRotaryWingAircraft</c>.</remarks>
		Helicopter = 0x8,
		/// <summary>
		/// The blimp type.
		/// </summary>
		/// <remarks>The internal class in memory is <c>CBlimp</c>, which is a subclass of <c>CHeli</c>.</remarks>
		Blimp = 0x9,
		/// <summary>
		/// The autogyro type, which is not used in the stock game.
		/// </summary>
		/// <remarks>The internal class in memory is <c>CAutogyro</c>.</remarks>
		Autogyro = 0xA,
		/// <summary>
		/// The motorcycle type.
		/// </summary>
		/// <remarks>The internal class in memory is <c>CBike</c>.</remarks>
		Motorcycle = 0xB,
		/// <summary>
		/// The bicycle type.
		/// </summary>
		/// <remarks>The internal class in memory is <c>CBmx</c>, which is a subclass of <c>CBike</c>.</remarks>
		Bicycle = 0xC,
		/// <remarks>The internal class in memory is <c>CBoat</c>, which is a subclass of <c>CBoat</c>.</remarks>
		/// <summary>
		/// The boat type.
		/// </summary>
		Boat = 0xD,
		/// <summary>
		/// The train type.
		/// </summary>
		/// <remarks>The internal class in memory is <c>CTrain</c>, which is a subclass of <c>CTrain</c>.</remarks>
		Train = 0xE,
		/// <summary>
		/// The submarine type.
		/// </summary>
		/// <remarks>
		/// <para>Submarine cars are not classified as this type but classified as <see cref="SubmarineCar" />.</para>
		/// <para>The internal class in memory is <c>CSubmarine</c></para>
		/// </remarks>
		Submarine = 0xF
	}
}
