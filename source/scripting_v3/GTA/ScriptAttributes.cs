//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ScriptAttributes : Attribute
	{
		public string Author;
		public string SupportURL;
		public bool NoScriptThread;
		public bool NoDefaultInstance;
	}
}
