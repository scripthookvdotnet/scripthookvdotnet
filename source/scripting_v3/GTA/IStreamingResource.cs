//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// A streaming resource.
	/// </summary>
	public interface IStreamingResource
	{
		bool IsLoaded
		{
			get;
		}

		void Request();
		bool Request(int timeout);

		void MarkAsNoLongerNeeded();
	}
}
