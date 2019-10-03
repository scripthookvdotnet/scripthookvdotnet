using System;

namespace GTA
{
	public class ConsoleCommand : Attribute
	{
		public ConsoleCommand() : this(string.Empty)
		{
		}
		public ConsoleCommand(string help)
		{
		}
	}
}
