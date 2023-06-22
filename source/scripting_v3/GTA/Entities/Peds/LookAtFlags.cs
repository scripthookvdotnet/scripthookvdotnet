//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Set of flags for the look at task for <see cref="TaskInvoker.LookAt(Entity, int, LookAtFlags, LookAtPriority)"/>
	/// and <see cref="TaskInvoker.LookAt(Math.Vector3, int, LookAtFlags, LookAtPriority)"/>.
	/// </summary>
	[Flags]
	public enum LookAtFlags
	{
		Default,
		/// <summary>
		/// Turn the head toward the target slowly.
		/// </summary>
		SlowTurnRate = 1,
		/// <summary>
		/// Turn the head toward the target quickly.
		/// </summary>
		FastTurnRate = 2,
		/// <summary>
		/// Wide yaw head limit.
		/// </summary>
		ExtendYawLimit = 4,
		/// <summary>
		/// Wide pitch head limit.
		/// </summary>
		ExtendPitchLimit = 8,
		/// <summary>
		/// Widest yaw head limit.
		/// </summary>
		WidestYawLimit = 16,
		/// <summary>
		/// Widest pitch head limit.
		/// </summary>
		WidestPitchLimit = 32,
		/// <summary>
		/// Narrow yaw head limit.
		/// </summary>
		NarrowYawLimit = 64,
		/// <summary>
		/// Narrow pitch head limit.
		/// </summary>
		NarrowPitchLimit = 128,
		/// <summary>
		/// Narrowest yaw head limit.
		/// </summary>
		NarrowestYawLimit = 256,
		/// <summary>
		/// Narrowest pitch head limit.
		/// </summary>
		NarrowestPitchLimit = 512,
		/// <summary>
		/// Keep tracking the target even if they are not in the hard coded FOV.
		/// </summary>
		WhileNotInFov = 2048,
		/// <summary>
		/// Use the camera as the target.
		/// </summary>
		UseCameraFocus = 4096,
		/// <summary>
		/// Only track the target with the eyes.
		/// </summary>
		UseEyesOnly = 8192,
		/// <summary>
		/// Use information in look dir DOF.
		/// </summary>
		UseLookDir = 16384,
	}
}
