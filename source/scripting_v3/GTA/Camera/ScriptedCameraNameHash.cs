//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// An enumeration of hashes of camera names where you can safely use with
	/// <see cref="Camera.Create(ScriptedCameraNameHash, bool)"/> out of all camera metadata in <c>cameras.ymt</c>.
	/// </summary>
	/// <remarks>
	/// You can find name hashes for camera metadata in <c>cameras.ymt</c>.
	/// </remarks>
	public enum ScriptedCameraNameHash : uint
	{
		DefaultScriptedCamera = 0x019286A9,
		/// <summary>
		/// An in-game fly camera designed for use in the mission creator.
		/// </summary>
		DefaultScriptedFlyCamera = 0xFFE1773D,
		/// <summary>
		/// Smoothed and velocity constrained spline, not continuous velocity.
		/// </summary>
		DefaultSplineCamera = 0xAC2E098,
		DefaultAnimatedCamera = 0x397ED48C,
		DefaultTransitionCamera = 0xCFB4228D,
		/// <summary>
		/// Smoothed and velocity constrained spline, not continuous velocity.
		/// </summary>
		TimedSplineCamera = 0x69D5F9D0,
		/// <summary>
		/// Rounded spline with continuous velocity.
		/// </summary>
		RoundedSplineCamera = 0x1B43F791,
		/// <summary>
		/// Smoothed spline with continuous velocity.
		/// </summary>
		SmoothedSplineCamera = 0xF8E013D4,
		/// <summary>
		/// Smoothed and velocity constrained spline, not continuous velocity, custom speeds can be set.
		/// </summary>
		CustomTimedSplineCamera = 0x634C33D4,
	}
}
