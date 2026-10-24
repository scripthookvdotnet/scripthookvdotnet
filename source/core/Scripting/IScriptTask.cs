//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace SHVDN
{
    /// <summary>
    /// The interface for tasks that must be run on the main thread (e.g. calling native functions) because of thread local storage (TLS).
    /// </summary>
    public interface IScriptTask
    {
        void Run();
    }
}
