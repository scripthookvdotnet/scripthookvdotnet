//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// An enumeration of the flags to be passed in to <see cref="World.GetSafePositionForPed(Math.Vector3, out Math.Vector3, GetSafePositionFlags)"/>
	/// to govern which navmesh polygons it considers.
	/// </summary>
	/// <remarks>
	/// The test function that is used for this enum can be found with <c>"75 07 32 C0 E9 88 00 00 00 F6 C1 04 74 0A 8B 42 24"</c> (which is called via a function pointer).
	/// The test function uses the same struct layout as YnvPoly class in CodeWalker.
	/// <see href="https://github.com/dexyfex/CodeWalker/blob/9d76f2c6c42b580e67aabf293e3c57be5edbb190/CodeWalker.Core/GameFiles/FileTypes/YnvFile.cs#L756"/>
	/// </remarks>
	[Flags]
	public enum GetSafePositionFlags
	{
		Default = 0,
		/// <summary>
		/// Only navmesh polygons marked as pavement.
		/// </summary>
		/// <remarks>
		/// The pavement flag is named <c>B02_IsFootpath</c> in CodeWalker30_dev44.
		/// </remarks>
		OnlyPavement = 1,
		/// <summary>
		/// Only navmesh polygons not marked as "isolated".
		/// </summary>
		/// <remarks>
		/// The isolated flag is named <c>B15_InteractionUnk</c> in CodeWalker30_dev44.
		/// </remarks>
		NotIsolated = 2,
		/// <summary>
		/// No navmesh polygons created from interiors.
		/// </summary>
		/// <remarks>
		/// The interior flag is named <c>B14_IsInterior</c> in CodeWalker30_dev44.
		/// </remarks>
		NotInterior = 4,
		/// <summary>
		/// No navmesh polygons marked as water.
		/// </summary>
		/// <remarks>
		/// The water flag is named <c>B07_IsWater</c> in CodeWalker30_dev44.
		/// </remarks>
		NotWater = 8,
		/// <summary>
		/// Only navmesh polygons marked as "network spawn candidate".
		/// </summary>
		/// <remarks>
		/// The flag is named <c>B17_IsFlatGround</c> in CodeWalker30_dev44.
		/// Despite the name in the said CodeWalker build, some slope navmesh polygons has the "network spawn candidate" flag.
		/// </remarks>
		OnlyNetworkSpawn = 16,
		/// <summary>
		/// Specify whether to use a flood-fill from the starting position, as opposed to scanning all polygons within the search volume.
		/// </summary>
		UseFloodFill = 32,
	}
}
