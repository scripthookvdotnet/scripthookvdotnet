//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	public class SelectedIndexChangedArgs : EventArgs
	{
		public SelectedIndexChangedArgs(int selectedIndex)
		{
			SelectedIndex = selectedIndex;
		}

		public int SelectedIndex
		{
			get;
		}
	}
}
