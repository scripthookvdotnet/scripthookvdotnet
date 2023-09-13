//
// Copyright (C) 2015 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using GTA.Math;

namespace GTA
{
	[Flags]
	public enum PickupPlacementFlags : uint
	{
		None = 0,
		/// <summary>
		/// Only used in MP.
		/// This is used for pickups that are created locally on each machine and only networked when collected.
		/// </summary>
		Map = 1,
		/// <summary>
		/// Sets the pickup as fixed so it cannot move.
		/// </summary>
		Fixed = 2,
		/// <summary>
		/// Sets the pickup as regenerating.
		/// </summary>
		/// <remarks>
		/// Cannot be used in <see cref="World.CreateAmbientPickup(PickupType, Vector3, PickupPlacementFlags, int, Model, bool)"/>.
		/// </remarks>
		Regenerates = 4,
		/// <summary>
		/// Places the pickup on the ground.
		/// </summary>
		SnapToGround = 8,
		/// <summary>
		/// Orientates the pickup correctly on the ground.
		/// </summary>
		OrientToGround = 16,
		/// <summary>
		/// Only used in MP.
		/// Creates the pickup non-networked.
		/// </summary>
		LocalOnly = 32,
		/// <summary>
		/// Intended to give the pickup a simple blip, but does not seem to work.
		/// </summary>
		BlippedSimple = 64,
		/// <summary>
		/// Gives the pickup a complex blip.
		/// </summary>
		BlippedComplex = 128,
		/// <summary>
		/// Some pickups need to be orientated differently to lie on the ground properly.
		/// Use this flag if your pickup is not lying correctly.
		/// </summary>
		Upright = 256,
		/// <summary>
		/// Pickup will rotate.
		/// </summary>
		/// <remarks>
		/// Cannot be used in <see cref="World.CreateAmbientPickup(PickupType, Vector3, PickupPlacementFlags, int, Model, bool)"/>.
		/// </remarks>
		Rotate = 512,
		/// <summary>
		/// Pickup will always face the player.
		/// </summary>
		FaceToPlayer = 1024,
		/// <summary>
		/// Pickup will be hidden when the player is using the phone camera.
		/// </summary>
		HideInPhotos = 2048,
		/// <summary>
		/// The pickup is being dropped as a gift to another player.
		/// </summary>
		PlayerGift = 4096,
		/// <summary>
		/// The pickup is lying on an object and probes for that when snapping or orientating to ground.
		/// </summary>
		OnObject = 8192,
		/// <summary>
		/// Set pickups to glow even if pickup can't be picked up because of team checks.
		/// </summary>
		GlowInTeam = 16384,
		/// <summary>
		/// If set on a weapon pickup, it will auto equip the picked up weapon.
		/// It will ignore autoswap logic.
		/// </summary>
		AutoEquip = 32768,
		/// <summary>
		/// If set, the pickup can be collected by a ped in a vehicle.
		/// </summary>
		CollectableInVehicle = 65536,
		/// <summary>
		/// if set the weapon pickup will render SD model only (HD&lt;-&gt;SD model switch will be disabled).
		/// </summary>
		DisableWeaponHdModel = 131072,
		/// <summary>
		/// If set the pickup will render as deferred model (no transparency/alpha blending in this render mode).
		/// </summary>
		ForceDeferredModel = 262144,
	}
}
