//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
    /// <summary>
    /// Represents an animation blend duration in seconds.
    /// </summary>
    /// <remarks>
    /// This struct does not accept negative values as they do not make sense as blend duration values, though
    /// `<c>CMoveNetworkHelper</c>`s of subclasses of `<c>CTaskMotionBase</c>` would adjust negative values to zero
    /// when they are passed as blend duration values.
    /// </remarks>
    public readonly struct AnimationBlendDuration : IEquatable<AnimationBlendDuration>
    {
        public AnimationBlendDuration(float value)
        {
            if (value < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value), "The value should not be negative.");
            }

            Value = value;
        }

        /// <summary>
        /// Returns the raw value.
        /// </summary>
        public float Value { get; }

        // All the static constant properties below are based on the `#define` macros that define blend duration values
        // in `animdefines.h` in the source code. We are not bringing our opinion here.
        /// <summary>
        /// The slowest predefined value for blend out.
        /// Returns the same struct as <c>new AnimationBlendDelta(0f)</c>.
        /// </summary>
        public static AnimationBlendDuration Instant => new(0f);
        /// <summary>
        /// Returns the same struct as <c>new AnimationBlendDelta(0.0625f)</c>.
        /// </summary>
        public static AnimationBlendDuration Fast => new(0.0625f);
        /// <summary>
        /// Returns the same struct as <c>new AnimationBlendDelta(0.125f)</c>.
        /// </summary>
        public static AnimationBlendDuration Normal => new(0.125f);
        /// <summary>
        /// Returns the same struct as <c>new AnimationBlendDelta(0.25f)</c>.
        /// </summary>
        public static AnimationBlendDuration Slow => new(0.25f);
        /// <summary>
        /// Returns the same struct as <c>new AnimationBlendDelta(0.5f)</c>.
        /// </summary>
        public static AnimationBlendDuration ReallySlow => new(0.5f);
        /// <summary>
        /// Returns the same struct as <c>new AnimationBlendDelta(1.0f)</c>.
        /// </summary>
        public static AnimationBlendDuration SuperSlow => new(1.0f);
        /// <summary>
        /// Returns the same struct as <c>new AnimationBlendDelta(1.0f)</c>.
        /// </summary>
        public static AnimationBlendDuration MigrateSlow => new(10.0f);

        public static explicit operator AnimationBlendDuration(float value) => new(value);
        public static explicit operator float(AnimationBlendDuration value) => value.Value;
        public static implicit operator InputArgument(AnimationBlendDuration value)
        {
            return value.Value;
        }

        public static AnimationBlendDuration FromBlendDelta(AnimationBlendDelta blendDelta)
        {
            // Performs the same calculation as how `fwAnimHelpers::CalcBlendDuration(float blendDelta)` does
            float deltaFloat = (blendDelta.Value < 0 ? -blendDelta.Value : blendDelta.Value);
            float duration = (deltaFloat == (float)AnimationBlendDelta.InstantBlendIn
                ? 0.0f
                : (1.0f / deltaFloat)
                );

            return new AnimationBlendDuration(duration);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> has the same value as <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(AnimationBlendDuration left, AnimationBlendDuration right)
            => left.Equals(right);
        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> has a different value than <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(AnimationBlendDuration left, AnimationBlendDuration right) => !left.Equals(right);

        public bool Equals(AnimationBlendDuration duration)
        {
            return Value == duration.Value;
        }
        public override bool Equals(object obj)
        {
            if (obj is AnimationBlendDuration asset)
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
