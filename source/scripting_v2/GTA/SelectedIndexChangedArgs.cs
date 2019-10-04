//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
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
