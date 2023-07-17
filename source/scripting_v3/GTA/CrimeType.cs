//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum CrimeType
	{
		None,
		/// <remarks>
		/// <para>Does not increase the crime value without providing a custom crime value.</para>
		/// <para>Does not refocus the search area.</para>
		/// </remarks>
		PosessionGun,
		/// <remarks>
		/// <para>Will increase the crime value by 5 when a custom crime value is not provided.</para>
		/// <para>Does not refocus the search area.</para>
		/// </remarks>
		RunRedlight,
		/// <remarks>
		/// <para>Will increase the crime value by 5 when a custom crime value is not provided.</para>
		/// <para>Does not refocus the search area.</para>
		/// </remarks>
		RecklessDriving,
		/// <remarks>
		/// Increases the crime value by 5 when a custom crime value is not provided.
		/// <para>Does not refocus the search area.</para>
		/// </remarks>
		Speeding,
		/// <remarks>
		/// <para>Does not increase the crime value without providing a custom crime value.</para>
		/// <para>Does not refocus the search area.</para>
		/// </remarks>
		DriveAgainstTraffic,
		/// <remarks>
		/// <para>Does not increase the crime value without providing a custom crime value.</para>
		/// <para>Does not refocus the search area.</para>
		/// </remarks>
		RidingBikeWithoutHelmet,
		/// <summary>The crime for stealing vehicles but not grand theft auto (car theft).</summary>
		/// <remarks>
		/// <para>Will increase the crime value by 15 when a custom crime value is not provided.</para>
		/// <para>Does not refocus the search area.</para>
		/// </remarks>
		StealVehicle,
		/// <summary>The crime for grand theft auto (car theft).</summary>
		/// <remarks>
		/// <para>Will increase the crime value by 15 when a custom crime value is not provided.</para>
		/// <para>Does not refocus the search area.</para>
		/// </remarks>
		StealCar,
		/// <remarks>Will increase the crime value by 3 when a custom crime value is not provided.</remarks>
		BlockPoliceCar,
		/// <remarks>Will increase the crime value by 3 when a custom crime value is not provided.</remarks>
		StandOnPoliceCar,
		/// <summary>The crime for hitting regular <see cref="Ped"/>s with melee attacks.</summary>
		/// <remarks>Will increase the crime value by 5 when a custom crime value is not provided.</remarks>
		HitPed,
		/// <summary>The crime for hitting law enforcement officers with melee attacks.</summary>
		/// <remarks>Will increase the crime value by 20 when a custom crime value is not provided.</remarks>
		HitCop,
		/// <remarks>Will increase the crime value by 35 when a custom crime value is not provided.</remarks>
		ShootPed,
		/// <remarks>Will increase the crime value by 80 when a custom crime value is not provided.</remarks>
		ShootCop,
		/// <remarks>Will increase the crime value by 18 when a custom crime value is not provided.</remarks>
		RunOverPed,
		/// <remarks>Will increase the crime value by 80 when a custom crime value is not provided.</remarks>
		RunOverCop,
		/// <remarks>Will increase the crime value by 400 when a custom crime value is not provided.</remarks>
		DestroyHeli,
		/// <remarks>Will increase the crime value by 20 when a custom crime value is not provided.</remarks>
		PedSetOnFire,
		/// <remarks>Will increase the crime value by 80 when a custom crime value is not provided.</remarks>
		CopSetOnFire,
		/// <remarks>Will increase the crime value by 20 when a custom crime value is not provided.</remarks>
		CarSetOnFire,
		/// <remarks>Will increase the crime value by 400 when a custom crime value is not provided.</remarks>
		DestroyPlane,
		/// <remarks>Will increase the crime value by 25 when a custom crime value is not provided.</remarks>
		CauseExplosion,
		/// <summary>The crime for stabbing regular <see cref="Ped"/>s with sharp melee weapons, which have the <c>MeleeBlade</c> flag in the <c>weapons.meta</c>.</summary>
		/// <remarks>Will increase the crime value by 35 when a custom crime value is not provided.</remarks>
		StabPed,
		/// <summary>The crime for stabbing law enforcement officers with sharp melee weapons, which have the <c>MeleeBlade</c> flag in the <c>weapons.meta</c>.</summary>
		/// <remarks>Will increase the crime value by 100 when a custom crime value is not provided.</remarks>
		StabCop,
		/// <remarks>Will increase the crime value by 70 when a custom crime value is not provided.</remarks>
		DestroyVehicle,
		/// <remarks>Will increase the crime value by 2 when a custom crime value is not provided.</remarks>
		DamageToProperty,
		/// <remarks>Does not increase the crime value without providing a custom crime value.</remarks>
		TargetCop,
		/// <remarks>Will increase the crime value by 10 when a custom crime value is not provided.</remarks>
		FirearmDischarge,
		/// <remarks>Does not increase the crime value without providing a custom crime value.</remarks>
		ResistArrest,
		/// <remarks>Does not increase the crime value without providing a custom crime value.</remarks>
		Molotov,
		/// <remarks>Will increase the crime value by 15 when a custom crime value is not provided.</remarks>
		ShootNonLethalPed,
		/// <remarks>Will increase the crime value by 60 when a custom crime value is not provided.</remarks>
		ShootNonLethalCop,
		/// <remarks>Does not increase the crime value without providing a custom crime value.</remarks>
		KillCop,
		/// <remarks>Does not increase the crime value without providing a custom crime value.</remarks>
		ShootAtCop,
		/// <remarks>Will increase the crime value by 15 when a custom crime value is not provided.</remarks>
		ShootVehicle,
		/// <remarks>Will increase the crime value by 25 when a custom crime value is not provided.</remarks>
		TerroristActivity,
		/// <summary>An unknown crime whose name is based on the intended name <c>CRIME_HASSLE</c>.</summary>
		/// <remarks>
		/// <para>Will increase the crime value by 55 when a custom crime value is not provided.</para>
		/// <para>The game does not use police radios for this crime.</para>
		/// </remarks>
		Hassle,
		/// <remarks>Does not increase the crime value without providing a custom crime value.</remarks>
		ThrowGrenade,
		/// <remarks>Will increase the crime value by 25 when a custom crime value is not provided.</remarks>
		VehicleExplosion,
		/// <remarks>Will increase the crime value by 20 when a custom crime value is not provided.</remarks>
		KillPed,
		/// <remarks>
		/// if a custom crime value is not provided, this value will set the crime value by 551 if the current wanted level is less than 3
		/// (regardless of the current crime value even if it is 550 or more) and will increase the crime value by 20 if the current wanted level is 3 or more.
		/// </remarks>
		StealthKillCop,
		/// <remarks>Does not increase the crime value without providing a custom crime value.</remarks>
		Suicide,
		/// <remarks>Will increase the crime value by 5 when a custom crime value is not provided.</remarks>
		Disturbance,
		/// <remarks>Will increase the crime value by 5 when a custom crime value is not provided.</remarks>
		CivilianNeedsAssistance,
		/// <remarks>Will increase the crime value by 20 when a custom crime value is not provided.</remarks>
		StealthKillPed,
		/// <remarks>Will increase the crime value by 35 when a custom crime value is not provided.</remarks>
		ShootPedSuppressed,
		/// <remarks>Will increase the crime value by 15 when a custom crime value is not provided.</remarks>
		JackDeadPed,
		/// <remarks>Will increase the crime value by 5 when a custom crime value is not provided.</remarks>
		ChainExplosion,
	}
}
