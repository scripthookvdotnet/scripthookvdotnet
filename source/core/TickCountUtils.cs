//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SHVDN
{
	public static class TickCountUtils
	{
		// This function should be used instead of DateTime.UtcNow when you wait for some task to complete until the wait timeout expires
		// GTA V can safely run at 187 FPS or less but it causes major issues when running at more than 187 FPS, which makes the resolution of Environment.TickCount basically acceptable
		// This method is not expected to use when elapsed time can be more than 24.9 days
		public static int GetElapsedTickCount(int startTickCount) => Environment.TickCount - startTickCount;
	}
}
