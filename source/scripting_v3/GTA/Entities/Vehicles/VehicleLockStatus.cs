//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	public enum VehicleLockStatus
	{
		None,
		Unlocked,
		/// <summary>
		/// The <see cref="Vehicle"/> cannot be entered regardless of whether the door is open or closed, or missing entirely.
		/// Warping into the <see cref="Vehicle"/> is the only way to make <see cref="Ped"/>s get in on a seat.
		/// </summary>
		CannotEnter,
		/// <summary>
		/// Players cannot enter the <see cref="Vehicle"/> regardless of whether the door is open or closed, or missing entirely.
		///Warping into the <see cref="Vehicle"/> is the only way to make <see cref="Ped"/>s get in on a seat.
		/// </summary>
		PlayerCannotEnter,
		/// <summary>
		/// <para>Doesn't allow players to exit the <see cref="Vehicle"/> with the exit vehicle key or button.</para>
		/// <para>The <see cref="Vehicle"/> is locked and must be broken into even if already broken into (the same as <see cref="CanBeBrokenIntoPersist"/>).</para>
		/// </summary>
		PlayerCannotLeaveCanBeBrokenIntoPersist,
		/// <summary>
		/// For players, the <see cref="Vehicle"/> cannot open any door if it has a driver.
		/// For AI, entering vehicle tasks will not start if the target <see cref="Vehicle"/>'s lock status is set to this value and the <see cref="Vehicle"/> has a driver.
		/// </summary>
		/// <remarks>
		/// <para>When a <see cref="Ped"/> is about to open the <see cref="Vehicle"/>'s door or the <see cref="Vehicle"/>'s driver instance is changed, the lock status will be set to <see cref="Unlocked"/>.</para>
		/// <para>AI <see cref="Ped"/>s can still get the driver out of the <see cref="Vehicle"/> to kill the driver, while players cannot do that even if given a task.</para>
		/// </remarks>
		CannotEnterIfDriverExists,
		/// <summary>
		/// <para>Can be broken into the car. When a <see cref="Ped"/> breaks the window of the door the <see cref="Ped"/> is entering through, the value will be set to <see cref="Unlocked"/>.</para>
		/// <para>If the glass is broken when a <see cref="Ped"/> is about to open the <see cref="Vehicle"/>'s door, the value immediately will be set to <see cref="Unlocked"/>.</para>
		/// </summary>
		CanBeBrokenInto = 7,
		/// <summary>
		/// <para>The <see cref="Vehicle"/> is locked and must be broken into.</para>
		/// <para>Even if the door the <see cref="Ped"/> is entering through has its window broken, <see cref="Ped"/>s will always have to try to break it and enter the <see cref="Vehicle"/> consecutively.</para>
		/// </summary>
		CanBeBrokenIntoPersist,
		/// <summary>
		/// <para><see cref="Ped"/>s can only get in on the driver's seat normally only when the <see cref="Vehicle"/> does not have a driver.
		/// Warping into the <see cref="Vehicle"/> is the only way to make <see cref="Ped"/>s get in on any other seat.</para>
		/// <para><see cref="Ped"/>s cannot get any other <see cref="Ped"/>s out of the <see cref="Vehicle"/> to kill them.</para>
		/// </summary>
		/// <remarks>
		/// Changing the <see cref="Vehicle"/>'s lock status to this value does not immediately block <see cref="Ped"/>s' entering vehicle tasks.
		/// </remarks>
		DriversSeatOnlyNoJacking,
		/// <summary>Players cannot attempt to enter the <see cref="Vehicle"/> with the enter vehicle key or button.</summary>
		IgnoredByPlayer,

		[Obsolete("Use VehicleLockStatus.CannotEnter instead.")]
		Locked = 2,
		[Obsolete("Use VehicleLockStatus.PlayerCannotEnter instead.")]
		LockedForPlayer,
		/// <summary>
		/// <para>The <see cref="Vehicle"/> is locked and must be broken into even if already broken into (the same as <see cref="CanBeBrokenIntoPersist"/>).</para>
		/// <para>Doesn't allow players to exit the <see cref="Vehicle"/> with the exit vehicle key or button.</para>
		/// </summary>
		[Obsolete("Use VehicleLockStatus.PlayerCannotLeaveCanBeBrokenIntoPersist instead.")]
		StickPlayerInside,
		[Obsolete("Use VehicleLockStatus.IgnoredByPlayer instead.")]
		CannotBeTriedToEnter = 10,
	}
}
