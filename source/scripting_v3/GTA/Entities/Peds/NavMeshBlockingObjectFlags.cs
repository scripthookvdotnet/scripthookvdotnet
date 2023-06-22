//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
    [Flags]
    public enum NavMeshBlockingObjectFlags
    {
        Default = 0,
        /// <summary>
        /// Blocking object will block wander paths.
        /// </summary>
        WanderPath = 1,
        /// <summary>
        /// Blocking object will block (regular) shortest-paths.
        /// </summary>
        ShortestPath = 2,
        /// <summary>
        /// Blocking object will block flee paths.
        /// </summary>
        FleePath = 4,
        AllPaths = WanderPath | ShortestPath | FleePath,
    }
}
