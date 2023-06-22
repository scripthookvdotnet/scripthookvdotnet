//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	public class MenuItemIndexArgs : EventArgs
	{
		public MenuItemIndexArgs(int index)
		{
			Index = index;
		}

		public int Index { get; }
	}
}
