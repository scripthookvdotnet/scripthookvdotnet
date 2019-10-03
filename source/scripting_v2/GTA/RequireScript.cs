using System;

namespace GTA
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class RequireScript : Attribute
	{
		public RequireScript(Type dependency)
		{
		}
	}
}
