//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// An enumeration of possible flags used to disable ragdoll behaviors from various sources.
	/// </summary>
	[Flags]
	public enum RagdollBlockingFlags
	{
		BulletImpact = 1,
		VehicleImpact = 2,
		Fire = 4,
		Electrocution = 8,
		/// <summary>
		/// Blocks ragdoll activation from any impact with a <see cref="Ped"/> (player characters running into
		/// the <see cref="Ped"/>, or active ragdolls colliding with them).
		/// </summary>
		PlayerImpact = 16,
		Explosion = 32,
		ImpactObject = 64,
		Melee = 128,
		RubberBullet = 256,
		Falling = 512,
		WaterJet = 1024,
		Drowning = 2048,
		/// <summary>
		/// Allows blocking of ragdoll activation for dead <see cref="Ped"/>s.
		/// By default, dead <see cref="Ped"/>s' ragdolls are allowed to activate regardless of how these flags
		/// have been set.
		/// </summary>
		AllowBlockDeadPed = 4096,
		/// <summary>
		/// Blocks ragdoll activation from an animated player running into the character (but not from collisions
		/// with other ragdolls).
		/// </summary>
		PlayerBump = 8192,
		/// <summary>
		/// Blocks ragdoll activation from a ragdolling player colliding with the character (but not from animated
		/// player bumps or collisions with active non-player ragdolls).
		/// </summary>
		PlayerRagdollBump = 16384,
		/// <summary>
		/// Blocks ragdoll activation from a ragdolling non-player colliding with the character (but not from any
		/// collisions with players, ragdolling or otherwise).
		/// </summary>
		PedRagdollBump = 32768,
		/// <summary>
		/// Blocks ragdoll activation from grabbing a <see cref="Vehicle"/> door whilst it pulls away.
		/// </summary>
		VehicleGrab = 65536,
		SmokeGrenade = 131072,
	}
}
