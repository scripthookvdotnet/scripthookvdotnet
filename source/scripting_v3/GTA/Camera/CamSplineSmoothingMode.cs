//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// An enumeration of camera spline smoothing mode.
	/// </summary>
	public enum CamSplineSmoothingMode
	{
		/// <summary>
		/// No smoothing just moves at a constant rate.
		/// </summary>
		NoSmooth,
		/// <summary>
		/// Decelerates when approaching a node.
		/// </summary>
		SlowInSmooth,
		/// <summary>
		/// Accelerates slowly when leaving a node.
		/// </summary>
		SlowOutSmooth,
		/// <summary>
		/// Decelerates when approaching a node and accelerates slowly when leaving a node.
		/// </summary>
		SlowInOutSmooth,
		VerySlowIn,
		VerySlowOut,
		VerySlowInSlowOut,
		SlowInVerySlowOut,
		VerySlowInVerySlowOut,
		EaseIn,
		EaseOut,
		QuadraticEaseIn,
		QuadraticEaseOut,
		QuadraticEaseInOut,
		CubicEaseIn,
		CubicEaseOut,
		CubicEaseInOut,
		QuarticEaseIn,
		QuarticEaseOut,
		QuarticEaseInOut,
		QuinticEaseIn,
		QuinticEaseOut,
		QuinticEaseInOut,
		CircularEaseIn,
		CircularEaseOut,
		CircularEaseInOut
	}
}
