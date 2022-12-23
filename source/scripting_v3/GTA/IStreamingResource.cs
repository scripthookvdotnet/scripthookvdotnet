//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
	/// <summary>
	/// A streaming resource.
	/// </summary>
	public interface IStreamingResource
	{
		public bool IsLoaded
		{
			get;
		}
		public void Request();
		public bool Request(int timeout);
		public void MarkAsNoLongerNeeded();
	}
}
