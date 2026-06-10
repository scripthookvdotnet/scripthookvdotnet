//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RequireScript : Attribute
    {
        public Type Dependency { get; }

        public RequireScript(Type dependency)
        {
            Dependency = dependency;
        }
    }
}
