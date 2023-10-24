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
		internal static void ThrowArgumentOutOfRangeException(string paramName, long value, long minInclusive, long maxInclusive)
		{
			throw new ArgumentOutOfRangeException(paramName, value, $"Value should be in range [{minInclusive.ToString()}, {maxInclusive.ToString()}].");
		}

		internal static void ThrowArgumentOutOfRangeException(string paramName, double value, double minInclusive, double maxInclusive)
		{
			throw new ArgumentOutOfRangeException(paramName, value, $"Value should be in range [{minInclusive.ToString()}, {maxInclusive.ToString()}].");
		}

		internal static void CheckArgumentRange(string paramName, long value, long minInclusive, long maxInclusive)
		{
			if (value < minInclusive || value > maxInclusive)
			{
				ThrowArgumentOutOfRangeException(paramName, value, minInclusive, maxInclusive);
			}
		}
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
	}
}
