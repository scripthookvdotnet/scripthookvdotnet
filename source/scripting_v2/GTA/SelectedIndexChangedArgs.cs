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
