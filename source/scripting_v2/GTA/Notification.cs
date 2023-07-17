//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	public sealed class Notification
	{
		private readonly int _handle;

		internal Notification(int handle)
		{
			this._handle = handle;
		}

		public void Hide()
		{
			Function.Call(Hash._REMOVE_NOTIFICATION, _handle);
		}
	}
}
