//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;

namespace GTA
{
	public enum ShapeTestStatus
	{
		/// <summary>
		/// Shapetest requests are discarded if they are ignored for a frame or as soon as the results are returned.
		/// </summary>
		NonExistent,
		/// <summary>
		/// Not ready yet; try again next frame.
		/// </summary>
		NotReady,
		/// <summary>
		/// The result is ready and the results have been returned to you.
		/// The shape test request has also just been destroyed.
		/// </summary>
		Ready,
	}
}
