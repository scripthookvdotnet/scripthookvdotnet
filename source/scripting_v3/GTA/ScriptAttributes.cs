using System;

namespace GTA
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class ScriptAttributes : Attribute
	{
		public string Author;
		public string SupportURL;
		public bool NoDefaultInstance;
	}
}
