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
