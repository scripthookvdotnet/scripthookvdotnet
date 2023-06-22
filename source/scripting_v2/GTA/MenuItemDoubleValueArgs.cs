//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	public class MenuItemDoubleValueArgs : EventArgs
	{
		public MenuItemDoubleValueArgs(double value)
		{
			Index = value;
		}

		public double Index { get; }
	}
}
