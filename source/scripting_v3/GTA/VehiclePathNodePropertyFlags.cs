//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Set of flags which may be passed in to methods in <see cref="PathFind"/> such as <see cref="PathFind.GetAllVehicleNodes(Func{VehiclePathNodePropertyFlags, bool})"/>.
	/// All the enums except <see cref="DontAllowGps"/> can be used when calling <c>GET_VEHICLE_NODE_PROPERTIES</c> in the same way as <see cref="PathFind.GetAllVehicleNodes(Func{VehiclePathNodePropertyFlags, bool})"/>.
	/// </summary>
	[Flags]
	public enum VehiclePathNodePropertyFlags
	{
		None = 0,
		/// <summary>
		/// The node has been flagged as 'off road', suitable only for 4x4 vehicles, etc.
		/// </summary>
		OffRoad = 1,
		/// <summary>
		/// The node has been dynamically marked as somewhere ahead, possibly on (or near to) the player's current road.
		/// </summary>
		OnPlayersRoad = 2,
		/// <summary>
		/// The node has been marked as not suitable for big vehicles.
		/// </summary>
		NoBigVehicles = 4,
		/// <summary>
		/// The node is switched off for ambient population.
		/// </summary>
		SwitchedOff = 8,
		TunnelOrInterior = 16,
		/// <summary>
		/// The node is, or leads to, a dead end.
		/// </summary>
		LeadsToDeadEnd = 32,
		/// <remarks>
		/// Cannot be used with <see cref="Highway"/> and <see cref="Boat"/> takes precedence over this flag.
		/// </remarks>
		Highway = 64,
		Junction = 128,
		/// <remarks>
		/// Cannot be used with <see cref="GiveWay"/>, because vehicle nodes can have either traffic-light or give-way feature as a special function but cannot have both of them.
		/// </remarks>
		TrafficLight = 256,
		/// <remarks>
		/// Cannot be used with <see cref="TrafficLight"/>, because vehicle nodes can have either traffic-light or give-way feature as a special function but cannot have both of them.
		/// </remarks>
		GiveWay = 512,
		/// <remarks>
		/// Cannot be used with <see cref="Highway"/>.
		/// </remarks>
		Boat = 1024,

		// GET_VEHICLE_NODE_PROPERTIES will not set any of the values below set as flags
		DontAllowGps = 2048,
	}
}
