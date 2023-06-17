//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// An enumeration of possible helmet prop flags for <see cref="Ped"/>s.
	/// This flags will restrict helmet types using AND bitwise, so no helmet props will be used if none of the available helmet props are found that match all the flags specified.
	/// The original enum name should be <c>ePedCompFlags</c>, but this enum uses <c>HelmetPropFlags</c> instead of <c>PedComponentFlags</c>
	/// as this enum is only used for native functions for ped helmets in practice.
	/// </summary>
	/// <remarks>
	/// You can check if the listed enum names are correct by searching the dumped exe for hashed values like <c>PV_FLAG_[enum name (snake case)]</c>
	/// (<c>PV</c> would stand for PedVariation).
	/// </remarks>
	[Flags]
	public enum HelmetPropFlags : uint
	{
		None = 0u,
		Bulky = 1u,
		Job = 2u,
		Sunny = 4u,
		Wet = 8u,
		Cold = 16u,
		NotInCar = 32u,
		BikeOnly = 64u,
		NotIndoors = 128u,
		FireRetardent = 256u,
		/// <summary>
		/// Specifies helmets that disables the critical hit on the <see cref="Ped"/> (regardless of <see cref="Ped.CanSufferCriticalHits"/>).
		/// </summary>
		/// <remarks>The original name is <c>PV_FLAG_ARMOURED</c>, but this enum uses the american name.</remarks>
		Armored = 512u,
		/// <remarks>The original name is <c>PV_FLAG_LIGHTLY_ARMOURED</c>, but this enum uses the american name.</remarks>
		LightlyArmored = 1024u,
		HighDetail = 2048u,
		/// <summary>
		/// Regular motorcycle helmets should be used for most <see cref="Ped"/>s.
		/// </summary>
		DefaultHelmet = 4096u,
		/// <summary>
		/// Different helmets may be used for some <see cref="Ped"/> models (e.g. <see cref="PedHash.Franklin"/>).
		/// </summary>
		RandomHelmet = 8192u,
		/// <remarks>
		/// Fire helmets will be used for <see cref="PedHash.Michael"/> and <see cref="PedHash.Trevor"/>, but this value does not explicitly specify fire helmets
		/// for other <see cref="Ped"/> models (e.g. online freemode <see cref="Ped"/> models).
		/// </remarks>
		ScriptHelmet = 16384u,
		FlightHelmet = 32768u,
		/// <summary>
		/// Specifies helmets that should not be directly seen in the first person view.
		/// </summary>
		HideInFirstPerson = 65536u,
		UsePhysicsHat2 = 131072u,
		PilotHelmet = 262144u,
	}
}
