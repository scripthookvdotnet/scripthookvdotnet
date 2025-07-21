//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
    public enum AbortScriptMode
    {
        Default,
        Off,
        On
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ScriptAttributes : Attribute
    {
        public string Author;
        public string SupportURL;
        public bool NoScriptThread;
        public bool NoDefaultInstance;

        private AbortScriptMode _nativeCallResetsTimeout;

        /// <summary>
        /// Determines whether native calls resets timeout, which is set to <see cref="AbortScriptMode.Default"/>
        /// by default.
        /// </summary>
        /// <remarks>
        /// If set to <see cref="AbortScriptMode.Default"/>, the script domain will not
        /// reset script timeout (unless the script uses v3.6.0 or earlier <b>only for compatibility reasons</b>).
        /// </remarks>
        public AbortScriptMode NativeCallResetsTimeout
        {
            get { return _nativeCallResetsTimeout; }
            set { _nativeCallResetsTimeout = value; }
        }
    }
}
