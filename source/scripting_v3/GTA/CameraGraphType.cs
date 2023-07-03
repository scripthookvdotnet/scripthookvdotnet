//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// An enumeration of camera graphs which apply as a modifier to the default speed OR time of some motion.
	/// </summary>
	public enum CameraGraphType
	{
		Linear,
		SinAccelDecel,
		Accel,
		Decel,
		SlowIn,
		SlowOut,
		SlowInOut,
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
		CircularEaseInOut,
	}
}
