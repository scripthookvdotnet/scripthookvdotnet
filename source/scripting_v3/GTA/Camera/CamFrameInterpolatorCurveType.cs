//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    /// <summary>
    /// An enumeration of all the curve types of camera frame interpolation, where the enum members specify how to
    /// scale base blend level (from <c>0f</c> to <c>1f</c>) of a camera frame interpolator.
    /// Corresponds to what <c>camFrameInterpolator::eCurveTypes</c> in the game code defines.
    /// </summary>
    public enum CamFrameInterpolatorCurveType
    {
        /// <summary>
        /// Does not scale the blend Level.
        /// </summary>
        Linear,
        /// <summary>
        /// The interpolator works the same with this type as how they work with <see cref="SlowInOut"/>.
        /// </summary>
        SinAccelDecel,
        /// <summary>
        /// The interpolator works the same with this type as how they work with <see cref="SlowIn"/>.
        /// </summary>
        Accel,
        /// <summary>
        /// The interpolator works the same with this type as how they work with <see cref="SlowOut"/>.
        /// </summary>
        Decel,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the sine ease-in curve function once.
        /// </summary>
        /// <remarks>
        /// The sign ease-in curve function can be defined as the below code:
        /// <code>
        /// static float SlowIn(float t)
        /// {
        ///     return 1.0f-dSlowOut(t);
        /// }
        /// </code>
        /// where <c>dSlowOut</c> can be defined as the below code:
        /// <code>
        /// static float dSlowOut (float t)
        /// {
        ///     if (t&gt;0.0f)
        ///     {
        ///         if (t&lt;1.0f)
        ///         {
        ///             return (float)Math.Sin(t * (float)Math.PI * 0.5f);
        ///         }
        ///         return 0.0f;
        ///     }
        ///     return 1.0f;
        /// }
        /// </code>
        /// </remarks>
        SlowIn,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the sine ease-out curve function once.
        /// </summary>
        /// <remarks>
        /// The sign ease-out curve function can be defined as the below code:
        /// <code>
        /// static float SlowOut (float t)
        /// {
        ///     if (t&gt;0.0f)
        ///     {
        ///         if (t&lt;1.0f)
        ///         {
        ///             return (float)Math.Sin(t * (float)MathF.PI * 0.5f);
        ///         }
        ///         return 0.0f;
        ///     }
        ///     return 1.0f;
        /// }
        /// </code>
        /// </remarks>
        SlowOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the sine ease-in-out curve function once.
        /// </summary>
        /// <remarks>
        /// The sign ease-in-out curve function can be defined as the below code:
        /// <code>
        /// static float SlowInOut (float t)
        /// {
        ///     return 0.5f*(1.0f-dSlowInOut(t));
        /// }
        /// </code>
        /// where <c>dSlowInOut</c> can be defined as the below code:
        /// <code>
        /// static float dSlowInOut (float t)
        /// {
        ///     if (t&gt;0.0f)
        ///     {
        ///         if (t&lt;1.0f)
        ///         {
        ///             return (float)Math.Cos(t * (float)MathF.PI);
        ///         }
        ///         return -1.0f;
        ///     }
        ///     return 1.0f;
        /// }
        /// </code>
        /// </remarks>
        SlowInOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the sine ease-in curve function twice.
        /// </summary>
        /// <remarks>
        /// See the <c>remarks</c> of <see cref="SlowIn"/> to view how the sign ease-in function can be defined.
        /// </remarks>
        VerySlowIn,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the sine ease-out curve function twice.
        /// </summary>
        /// <remarks>
        /// See the <c>remarks</c> of <see cref="SlowOut"/> to view how the sign ease-out function can be defined.
        /// </remarks>
        VerySlowOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the sine ease-in-out curve function once
        /// then applying the sine ease-in curve function once.
        /// </summary>
        /// <remarks>
        /// See the <c>remarks</c> tags of <see cref="SlowInOut"/> and <see cref="SlowIn"/> to view how
        /// the sine ease-in-out and sign-in function can be defined respectively.
        /// </remarks>
        VerySlowInSlowOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the sine ease-in-out curve function once
        /// then applying the sine ease-out curve function once.
        /// </summary>
        /// <remarks>
        /// See the <c>remarks</c> tags of <see cref="SlowInOut"/> and <see cref="SlowOut"/> to view how
        /// the sine ease-in-out and sign ease-out function can be defined respectively.
        /// </remarks>
        SlowInVerySlowOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the sine ease-in-out curve function twice.
        /// </summary>
        /// <remarks>
        /// See the <c>remarks</c> tags of <see cref="SlowInOut"/> to view how the sine ease-in-out function can be
        /// defined.
        /// </remarks>
        VerySlowInVerySlowOut,
        EaseIn,
        EaseOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the quadratic ease-in curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The quadratic ease-in function can be defined like `<c>var QuadraticEaseIn = (float t) => t * t;</c>`, in the range
        /// of [0f, 1f].
        /// </remarks>
        QuadraticEaseIn,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the quadratic ease-in-out curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The quadratic ease-out function can be defined like `<c>var QuadraticEaseOut = (float t) => 1.0f - QuadraticEaseIn(1.0f - t);</c>`,
        /// in the range of [0f, 1f].
        /// </remarks>
        QuadraticEaseOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the quadratic ease-in-out curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The quadratic ease-in-out function can be defined as the below code:
        /// <code>
        /// static float QuadraticEaseInOut (float t)
        /// {
        ///     // `[A function that asserts if the passed value is between 0f and 1f]`
        ///     if (t&lt;0.5)
        ///     {
        ///         return QuadraticEaseIn(2.0f * t)/2.0f;
        ///     }
        ///     else
        ///     {
        ///         return 1.0f - QuadraticEaseIn((1.0f - t)*2.0f) /2.0f;
        ///     }
        /// }
        /// </code>
        /// </remarks>
        QuadraticEaseInOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the cubic ease-in curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The cubic ease-in function can be defined like `<c>var CubicEaseIn = (float t) => t * t * t;</c>`,
        /// in the range of [0f, 1f].
        /// </remarks>
        CubicEaseIn,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the cubic ease-in-out curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The cubic ease-out function can be defined like `<c>var CubicEaseOut = (float t) => 1.0f - CubicEaseIn(1.0f - t);</c>`,
        /// in the range of [0f, 1f].
        /// </remarks>
        CubicEaseOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the cubic ease-in-out curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The ease-in-out function can be defined as the below code:
        /// <code>
        /// static float CubicEaseInOut (float t)
        /// {
        ///     // `[A function that asserts if the passed value is between 0f and 1f]`
        ///     if (t&lt;0.5)
        ///     {
        ///         return CubicEaseIn(2.0f * t)/2.0f;
        ///     }
        ///     else
        ///     {
        ///         return 1.0f - CubicEaseIn((1.0f - t)*2.0f) /2.0f;
        ///     }
        /// }
        /// </code>
        /// </remarks>
        CubicEaseInOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the quartic ease-in curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The quartic ease-in function can be defined like `<c>var QuarticEaseIn = (float t) => t * t * t * t;</c>`,
        /// in the range of [0f, 1f].
        /// </remarks>
        QuarticEaseIn,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the quartic ease-in-out curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The quartic ease-out function can be defined like `<c>var QuarticEaseOut = (float t) => 1.0f - QuarticEaseIn(1.0f - t);</c>`,
        /// in the range of [0f, 1f].
        /// </remarks>
        QuarticEaseOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the quartic ease-in-out curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The quartic ease-in-out function can be defined as the below code:
        /// <code>
        /// static float QuarticEaseInOut (float t)
        /// {
        ///     // `[A function that asserts if the passed value is between 0f and 1f]`
        ///     if (t&lt;0.5)
        ///     {
        ///         return QuarticEaseIn(2.0f * t)/2.0f;
        ///     }
        ///     else
        ///     {
        ///         return 1.0f - QuarticEaseIn((1.0f - t)*2.0f) /2.0f;
        ///     }
        /// }
        /// </code>
        /// </remarks>
        QuarticEaseInOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the quintic ease-in curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The quintic ease-in function can be defined like `<c>var QuinticEaseIn = (float t) => t * t * t * t * t;</c>`,
        /// in the range of [0f, 1f].
        /// </remarks>
        QuinticEaseIn,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the quintic ease-in-out curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The quintic ease-out function can be defined like `<c>var QuinticEaseOut = (float t) => 1.0f - QuinticEaseIn(1.0f - t);</c>`,
        /// in the range of [0f, 1f].
        /// </remarks>
        QuinticEaseOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the quintic ease-in-out curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The quintic ease-in-out function can be defined as the below code:
        /// <code>
        /// static float QuinticEaseInOut (float t)
        /// {
        ///     // `[A function that asserts if the passed value is between 0f and 1f]`
        ///     if (t&lt;0.5)
        ///     {
        ///         return QuinticEaseIn(2.0f * t)/2.0f;
        ///     }
        ///     else
        ///     {
        ///         return 1.0f - QuinticEaseIn((1.0f - t)*2.0f) /2.0f;
        ///     }
        /// }
        /// </code>
        /// </remarks>
        QuinticEaseInOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the circular ease-in curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The circular ease-in function can be defined as the below code:
        /// <code>
        /// static float CircularEaseIn (float t)
        /// {
        ///     // `[A function that asserts if the passed value is between 0f and 1f]`
        ///     return 1.0f - (float)Math.Sqrt(1.0f - (t * t));
        /// }
        /// </code>
        /// </remarks>
        CircularEaseIn,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the circular ease-out curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The circular ease-out function can be defined as the below code:
        /// <code>
        /// static float CircularEaseOut (float t)
        /// {
        ///     // `[A function that asserts if the passed value is between 0f and 1f]`
        ///     float oneMinusT = 1.0f - t;
        ///     return (float)Math.Sqrt(1.0f - (oneMinusT*oneMinusT));
        /// }
        /// </code>
        /// </remarks>
        CircularEaseOut,
        /// <summary>
        /// The interpolator scales the blend Level for lerping by applying the circular ease-in-out curve function
        /// once.
        /// </summary>
        /// <remarks>
        /// The circular ease-in-out function can be defined as the below code:
        /// <code>
        /// static float CircularEaseInOut (float t)
        /// {
        ///     // `[A function that asserts if the passed value is between 0f and 1f]`
        ///     if (t&lt;0.5)
        ///     {
        ///         return CircularEaseIn(2.0f * t)/2.0f;
        ///     }
        ///     else
        ///     {
        ///         return 1.0f - CircularEaseIn((1.0f - t)*2.0f) /2.0f;
        ///     }
        /// }
        /// </code>
        /// </remarks>
        CircularEaseInOut,
    }
}
