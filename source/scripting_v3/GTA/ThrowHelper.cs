//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

// This file defines an internal static class used to throw exceptions in BCL code.
// The main purpose is to reduce the Jitted code size a lot and the IL code size.
// To understand how .NET Foundation utilize their ThrowHelper class to reduce code size runtime, see also the
// implementation of ThrowHelper at the commit between .NET 7 and 8 codebase from the link below (note: ThrowHelper
// is present in .NET Framework 4 as well):
// https://github.com/dotnet/runtime/blob/8c090757a69370e46da63dfae9f4ed44cc90ec01/src/libraries/System.Private.CoreLib/src/System/ThrowHelper.cs

using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace GTA
{
    internal static class ThrowHelper
    {
        #region ThrowArgumentNullException
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> for the specified parameter.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentNullException(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> for the specified parameter with the provided message.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="message">The error message.</param>
        /// <exception cref="ArgumentNullException"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentNullException(string paramName, string message)
        {
            throw new ArgumentNullException(paramName, message);
        }

        #endregion

        #region ThrowArgumentOutOfRangeException

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> for the specified parameter.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> for the specified parameter with the provided message.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="message">The error message.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException(string paramName, string message)
        {
            throw new ArgumentOutOfRangeException(paramName, message);
        }

        #endregion

        #region ThrowArgumentException

        /// <summary>
        /// Throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException()
        {
            throw new ArgumentException();
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> with the provided message.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <exception cref="ArgumentException"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException(string message)
        {
            throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> for the specified parameter with the provided message.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="ArgumentException"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentException(string message, string paramName)
        {
            throw new ArgumentException(message, paramName);
        }

        #endregion

        #region ThrowArgumentOutOfRangeException & ThrowEnumArgumentOutOfRangeException

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> for the specified parameter with the given range information.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="minInclusive">The inclusive minimum allowed value.</param>
        /// <param name="maxInclusive">The inclusive maximum allowed value.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException(string paramName, long value, long minInclusive, long maxInclusive)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"Value should be in range [{minInclusive.ToString(CultureInfo.InvariantCulture)}, {maxInclusive.ToString(CultureInfo.InvariantCulture)}].");
        }

        ///<inheritdoc cref="ThrowArgumentOutOfRangeException(string, long, long, long)"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentOutOfRangeException(string paramName, double value, double minInclusive, double maxInclusive)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"Value should be in range [{minInclusive.ToString(CultureInfo.InvariantCulture)}, {maxInclusive.ToString(CultureInfo.InvariantCulture)}].");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> with a parameter name for an enum value out of legal
        /// range, which means the value is not defined.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowEnumArgumentOutOfRangeException(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, "Enum value was out of legal range.");
        }

        #endregion

        #region CheckArgumentRange

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is not within the specified range.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="minInclusive">The inclusive minimum allowed value.</param>
        /// <param name="maxInclusive">The inclusive maximum allowed value.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="value"/> is not between <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void CheckArgumentRange(string paramName, int value, int minInclusive, int maxInclusive)
        {
            if (value < minInclusive || value > maxInclusive)
            {
                ThrowArgumentOutOfRangeException(paramName, value, minInclusive, maxInclusive);
            }
        }

        /// <inheritdoc cref="CheckArgumentRange(string, int, int, int)"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void CheckArgumentRange(string paramName, float value, float minInclusive, float maxInclusive)
        {
            if (value < minInclusive || value > maxInclusive)
            {
                ThrowArgumentOutOfRangeException(paramName, value, minInclusive, maxInclusive);
            }
        }

        /// <inheritdoc cref="CheckArgumentRange(string, int, int, int)"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void CheckArgumentRange(string paramName, double value, double minInclusive, double maxInclusive)
        {
            if (value < minInclusive || value > maxInclusive)
            {
                ThrowArgumentOutOfRangeException(paramName, value, minInclusive, maxInclusive);
            }
        }

        /// <inheritdoc cref="CheckArgumentRange(string, int, int, int)"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void CheckArgumentRange(string paramName, long value, long minInclusive, long maxInclusive)
        {
            if (value < minInclusive || value > maxInclusive)
            {
                ThrowArgumentOutOfRangeException(paramName, value, minInclusive, maxInclusive);
            }
        }

        #endregion

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> for the specified parameter when a floating-point value is NaN.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="ArgumentException"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowArgumentExceptionForNaN(string paramName)
        {
            throw new ArgumentException("The floating-point value cannot be NaN.", paramName);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <exception cref="InvalidOperationException"/>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ThrowInvalidOperationException(string message)
        {
            throw new InvalidOperationException(message);
        }
    }
}
