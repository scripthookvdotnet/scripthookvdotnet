//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// A interface for streaming resources that can be requested and pinned by scripts
	/// (by increasing reference counts).
	/// </summary>
	public interface IScriptStreamingResource
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
