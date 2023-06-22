//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
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
