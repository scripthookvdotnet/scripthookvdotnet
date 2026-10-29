//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Reflection;

namespace SHVDN
{
    public sealed class ConsoleCommand : Attribute
    {
        public ConsoleCommand()
            : this(string.Empty)
        {
        }

        public ConsoleCommand(string help)
        {
            Help = help;
        }

        public string Help { get; }

        internal string Name => MethodInfo.Name;

        internal string Namespace => MethodInfo.DeclaringType.FullName;

        internal MethodInfo MethodInfo { get; set; }
    }
}
