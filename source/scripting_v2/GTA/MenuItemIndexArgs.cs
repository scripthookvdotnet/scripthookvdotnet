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