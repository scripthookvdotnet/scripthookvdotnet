//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;

namespace GTA
{
	/// <summary>
	/// Set of flags to define the behaviour of the enter and exit vehicle tasks.
	/// Shares the same flags with <see cref="LeaveVehicleFlags"/>.
	/// </summary>
	[Flags]
	public enum EnterVehicleFlags
	{
		None = 0,
		/// <summary>
		/// Resume the enter vehicle task even if the task is interupted (bumped, shot).
		/// </summary>
		ResumeIfInterupted = 1,
		/// <summary>
		/// Warp the <see cref="Ped"/> to entry point ready to open the door/enter seat.
		/// </summary>
		WarpToDoor = 2,
		/// <summary>
		/// Allow the <see cref="Ped"/> to jack regardless of relationship status if they have to jack someone to complete the enter vehicle task.
		/// Without this flag, the <see cref="Ped"/> won't jack those they respect or like where the relationship is set to <see cref="Relationship.Companion"/> or <see cref="Relationship.Respect"/> towards them,
		/// but may jack those they don't respect or like.
		/// Without this flag, the <see cref="Ped"/> will abort the enter vehicle task if they have to jack one of those who respect or like to complete the enter vehicle task.
		/// </summary>
		JackAnyone = 8,
		/// <summary>
		/// Warp the <see cref="Ped"/> onto the <see cref="Vehicle"/>.
		/// </summary>
		WarpIn = 16,
		/// <summary>
		/// Dont close the vehicle door.
		/// </summary>
		DontCloseDoor = 256,
		/// <summary>
		/// Allow ped to warp to the seat if entry is blocked.
		/// If the shuffle link to that seat is blocked by someone but the entry point for the shuffle link is not directly blocked, the <see cref="Ped"/> won't warp.
		/// Consider using <see cref="WarpIfShuffleLinkIsBlocked"/> if you want the <see cref="Ped"/> to warp when the direct door and the shuffle link to that seat is blocked by someone.
		/// </summary>
		WarpIfDoorIsBlocked = 512,
		/// <summary>
		/// Use entry/exit point on the left hand side.
		/// </summary>
		UseLeftEntry = 131072,
		/// <summary>
		/// Use entry/exit point on the right hand side.
		/// </summary>
		UseRightEntry = 262144,
		/// <summary>
		/// When jacking just open the door and/or pull the ped out, but don't get in.
		/// </summary>
		JustPullPedOut = 524288,
		/// <summary>
		/// Disable shuffling, forcing ped to use direct door only.
		/// </summary>
		BlockSeatShuffling = 1048576,
		/// <summary>
		/// Allow ped to warp if the direct door is blocked and the shuffle link to that seat is blocked by someone.
		/// Unlike <see cref="WarpIfDoorIsBlocked"/>, this flag allows the the <see cref="Ped"/> to warp when the direct door and the shuffle link to that seat is blocked by someone (regardless of whether the door linked to the shuffle link is directly blocked).
		/// </summary>
		WarpIfShuffleLinkIsBlocked = 4194304,
		/// <summary>
		/// Never jack anyone when entering/exiting.
		/// This flag takes precedence over <see cref="AllowJacking"/>.
		/// </summary>
		DontJackAnyone = 8388608,
		[Obsolete("Use EnterVehicleFlags.JackAnyone instead."), EditorBrowsable(EditorBrowsableState.Never)]
		AllowJacking = JackAnyone,
		[Obsolete("Use EnterVehicleFlags.UseRightEntry instead."), EditorBrowsable(EditorBrowsableState.Never)]
		EnterFromOppositeSide = UseRightEntry,
		[Obsolete("Use EnterVehicleFlags.JustPullPedOut instead."), EditorBrowsable(EditorBrowsableState.Never)]
		OnlyOpenDoor = JustPullPedOut,
	}
}
