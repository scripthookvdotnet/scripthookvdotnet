//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
    /// <summary>
    /// Set of flags which may be passed in.
    /// </summary>
    [Flags]
    public enum MoveNetworkFlags
    {
        Default = 0,
        UseKinematicPhysics = 4,
        /// <remarks>
        /// With this flag set, the move network task methods in <see cref="TaskInvoker"/> cannot be used as a part of
        /// a <see cref="TaskSequence"/>.
        /// </remarks>
        Secondary = 8,
        UseFirstPersonArmIkLeft = 16,
        UseFirstPersonArmIkRight = 32,
        EnableCollisionOnNetworkCloneWhenFixed = 64
    }
}
