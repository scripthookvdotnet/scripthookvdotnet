//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	public sealed class Notification
	{
		private readonly int handle;

		internal Notification(int handle)
		{
			this.handle = handle;
		}

		public void Hide()
		{
			Function.Call(Hash._REMOVE_NOTIFICATION, handle);
		}
	}
}
