//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    /// <summary>
    /// Set of enumerations of the available look priorities for <see cref="TaskInvoker.LookAt(Entity, int, LookAtFlags, LookAtPriority)"/>
    /// and <see cref="TaskInvoker.LookAt(Math.Vector3, int, LookAtFlags, LookAtPriority)"/>.
    /// </summary>
    public enum LookAtPriority
    {
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh,
    }
}
