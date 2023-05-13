//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Set of enumerations of the available look priorities for <see cref="TaskInvoker.LookAt(Entity, int, LookAtFlags, LookAtPriority)"/>
	/// and <see cref="TaskInvoker.LookAt(Math.Vector3, int, LookAtFlags, LookAtPriority)"/>.
	/// </summary>
	public enum LookAtPriority
	{
		VeryLow,
		Low,
		Medium,
		High,
		VeryHigh,
	}
}
