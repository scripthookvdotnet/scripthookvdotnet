//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SHVDN
{
	public static class WinAPIWrapper
	{
		// This function should be used instead of DateTime.UtcNow when you wait for some task to complete until the wait timeout expires
		// GTA V can safely run at 187 FPS and less but it causes major issues when running at more than 187 FPS, which makes the resolution of GetTickCount64 basically acceptable
		[SuppressUnmanagedCodeSecurity]
		[DllImport("Kernel32.dll")]
		public static extern ulong GetTickCount64();
	}
}
