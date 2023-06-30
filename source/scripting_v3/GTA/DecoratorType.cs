//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum DecoratorType
	{
		Unknown,
		Float,
		Bool,
		Int,
		/// <remarks>
		/// The relevant native functions do not appear in production builds.
		/// </remarks>
		String,
		Time,
	}
}
