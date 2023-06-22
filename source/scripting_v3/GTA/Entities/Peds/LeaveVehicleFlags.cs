//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Set of flags to define the behaviour of the enter and exit vehicle tasks.
	/// Shares the same flags with <see cref="EnterVehicleFlags"/>.
	/// </summary>
	[Flags]
	public enum LeaveVehicleFlags
	{
		None = 0,
		/// <summary>
		/// Warp the ped out of the vehicle.
		/// </summary>
		WarpOut = 16,
		/// <summary>
		/// Don't wait for the vehicle to stop before exiting.
		/// </summary>
		DontWaitForVehicleToStop = 64,
		/// <summary>
		/// Dont close the vehicle door.
		/// </summary>
		LeaveDoorOpen = 256,
		/// <summary>
		/// Allow ped to warp to the seat if entry is blocked. The player <see cref="Ped"/> will warp out of the vehicle without any flags if the entry is blocked.
		/// If the shuffle link to that seat is blocked by someone but the entry point for the shuffle link is not directly blocked, the <see cref="Ped"/> won't warp.
		/// Consider using <see cref="WarpIfShuffleLinkIsBlocked"/> if you want the <see cref="Ped"/> to warp when the direct door and the shuffle link to that seat is blocked by someone.
		/// </summary>
		WarpIfDoorIsBlocked = 512,
		/// <summary>
		/// Jump out of the vehicle regardness of its speed.
		/// </summary>
		BailOut = 4096,
		/// <summary>
		/// <see cref="TaskInvoker.LeaveVehicle(LeaveVehicleFlags)"/> (or <c>TASK_LEAVE_ANY_VEHICLE</c>) auto defaults the <see cref="WarpIfDoorIsBlocked"/>, set this flag to not set that.
		/// </summary>
		DontDefaultWarpIfDoorBlocked = 65536,
		/// <summary>
		/// Use entry/exit point on the left hand side.
		/// </summary>
		FromLeftSide = 131072,
		/// <summary>
		/// Use entry/exit point on the right hand side.
		/// </summary>
		FromRightSide = 262144,
		/// <summary>
		/// Disable shuffling, forcing ped to use direct door only.
		/// </summary>
		BlockSeatShuffling = 1048576,
		/// <summary>
		/// Allow ped to warp if the direct door is blocked and the shuffle link to that seat is blocked by someone. The player <see cref="Ped"/> will warp out of the vehicle without any flags if the entry is blocked.
		/// Unlike <see cref="WarpIfDoorIsBlocked"/>, this flag allows the the <see cref="Ped"/> to warp when the direct door and the shuffle link to that seat is blocked by someone (regardless of whether the door linked to the shuffle link is directly blocked).
		/// </summary>
		WarpIfShuffleLinkIsBlocked = 4194304,
		/// <summary>
		/// Never jack anyone when entering/exiting.
		/// </summary>
		DontJackAnyone = 8388608,
		/// <summary>
		/// Wait for our entry point to be clear of peds before exiting.
		/// </summary>
		WaitForEntryPointToBeClear = 16777216,
	}
}
