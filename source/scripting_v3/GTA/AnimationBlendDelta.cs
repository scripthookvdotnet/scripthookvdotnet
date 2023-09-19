//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	/// <summary>
	/// Represents an animation blend delta that determines the rate at which the animation task will blend in or out.
	/// The blend duration in seconds is equal to <c>1.0f / (the blend delta value)</c>.
	/// </summary>
	/// <remarks>
	/// To prevent animation blending not getting finished forever, this struct throws an <see cref="ArgumentException"/>
	/// if the passed value is not more than zero when the constructor is called.
	/// </remarks>
	public readonly struct AnimationBlendDelta : IEquatable<AnimationBlendDelta>
	{
		public AnimationBlendDelta(float value)
		{
			// Don't accept zero. If the blend delta is zero, CTaskScriptedAnimation can't finish animation blending
			// because of infinity duration (which will be made by division of 1.0 by 0).
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "The value should be positive.");
			}

			Value = value;
		}

		/// <summary>
		/// Returns the raw value.
		/// </summary>
		public float Value { get; }

		/// <summary>
		/// The slowest predefined value. Returns the same struct as <c>new AnimationBlendDelta(1.5f)</c>.
		/// </summary>
		// commands_task.h (leaked code) defines the const value WALK_BLEND_IN (1.5) and WALK_BLEND_OUT (-1.5)
		public static AnimationBlendDelta Walk => new(1.5f);
		/// <summary>
		/// Returns the same struct as <c>new AnimationBlendDelta(2.0f)</c>.
		/// </summary>
		public static AnimationBlendDelta VerySlow => new(2.0f);
		/// <summary>
		/// Returns the same struct as <c>new AnimationBlendDelta(4.0f)</c>.
		/// </summary>
		public static AnimationBlendDelta Slow => new(4.0f);
		/// <summary>
		/// Returns the same struct as <c>new AnimationBlendDelta(8.0f)</c>.
		/// </summary>
		public static AnimationBlendDelta Normal => new(8.0f);
		/// <summary>
		/// Returns the same struct as <c>new AnimationBlendDelta(16.0f)</c>.
		/// </summary>
		public static AnimationBlendDelta Fast => new(16.0f);
		/// <summary>
		/// Returns the same struct as <c>new AnimationBlendDelta(1000.0f)</c>.
		/// </summary>
		public static AnimationBlendDelta Instant => new(1000.0f);

		public static explicit operator AnimationBlendDelta(float value) => new(value);
		public static explicit operator float(AnimationBlendDelta value) => value.Value;
		public static implicit operator InputArgument(AnimationBlendDelta value)
		{
			return value.Value;
		}

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(AnimationBlendDelta left, AnimationBlendDelta right) => left.Equals(right);
		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(AnimationBlendDelta left, AnimationBlendDelta right) => !left.Equals(right);

		public bool Equals(AnimationBlendDelta moveBlendRatio)
		{
			return Value == moveBlendRatio.Value;
		}
		public override bool Equals(object obj)
		{
			if (obj is AnimationBlendDelta asset)
			{
				return Equals(asset);
			}

			return false;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode() => Value.GetHashCode();

		/// <summary>
		/// Converts the value of the object to its equivalent string representation.
		/// </summary>
		/// <returns>The string representation of the value of this instance.</returns>
		public override string ToString() => Value.ToString();
	}
}
