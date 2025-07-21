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

namespace GTA
{
    internal static class ThrowHelper
    {
        /// <summary>
        /// Throws an <see cref="NullReferenceException"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="NullReferenceException"/>
        internal static void ThrowArgumentNullException(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }
        /// <summary>
        /// Throws an <see cref="NullReferenceException"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="NullReferenceException"/>
        internal static void ThrowArgumentNullException(string paramName, string message)
        {
            throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        internal static void ThrowArgumentOutOfRangeException(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <exception cref="ArgumentException"/>
        internal static void ThrowArgumentOutOfRangeException(string paramName, string message)
        {
            throw new ArgumentOutOfRangeException(paramName, message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        internal static void ThrowArgumentException()
        {
            throw new ArgumentException();
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <exception cref="ArgumentException"/>
        internal static void ThrowArgumentException(string message)
        {
            throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        /// <exception cref="ArgumentException"/>
        internal static void ThrowArgumentException(string message, string paramName)
        {
            throw new ArgumentException(message, paramName);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> with the acceptable range info.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="value">The value.</param>
        /// <param name="minInclusive">The min value of the acceptable range inclusive.</param>
        /// <param name="maxInclusive">The max value of the acceptable range inclusive.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        internal static void ThrowArgumentOutOfRangeException(string paramName, long value, long minInclusive, long maxInclusive)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"Value should be in range [{minInclusive.ToString()}, {maxInclusive.ToString()}].");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> with the acceptable range info.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="value">The value.</param>
        /// <param name="minInclusive">The min value of the acceptable range inclusive.</param>
        /// <param name="maxInclusive">The max value of the acceptable range inclusive.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        internal static void ThrowArgumentOutOfRangeException(string paramName, double value, double minInclusive, double maxInclusive)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"Value should be in range [{minInclusive.ToString()}, {maxInclusive.ToString()}].");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> with the acceptable range info if
        /// <paramref name="value"/> is not in the range.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <param name="minInclusive">The min value of the acceptable range inclusive.</param>
        /// <param name="maxInclusive">The max value of the acceptable range inclusive.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is not between <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>.
        /// </exception>
        internal static void CheckArgumentRange(string paramName, long value, long minInclusive, long maxInclusive)
        {
            if (value < minInclusive || value > maxInclusive)
            {
                ThrowArgumentOutOfRangeException(paramName, value, minInclusive, maxInclusive);
            }
        }
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> with the acceptable range info if
        /// <paramref name="value"/> is not in the range.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <param name="minInclusive">The min value of the acceptable range inclusive.</param>
        /// <param name="maxInclusive">The max value of the acceptable range inclusive.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is not between <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>.
        /// </exception>
        internal static void CheckArgumentRange(string paramName, int value, int minInclusive, int maxInclusive)
        {
            if (value < minInclusive || value > maxInclusive)
            {
                ThrowArgumentOutOfRangeException(paramName, value, minInclusive, maxInclusive);
            }
        }

        internal static void ThrowArgumentException_Arg_CannotBeNaN(string paramName)
        {
            throw new ArgumentException("GameClockDuration does not accept floating point Not-a-Number values.", nameof(paramName));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> with a parameter name for an enum value out of legal
        /// range, which means the value is not defined.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        internal static void ArgumentOutOfRangeException_Enum_Value(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, "Enum value was out of legal range.");
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <exception cref="InvalidOperationException"/>
        internal static void ThrowInvalidOperationException(string message)
        {
            throw new InvalidOperationException(message);
        }
    }
}
