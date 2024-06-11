//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;

namespace GTA
{
    /// <summary>
    /// An enumeration of all the animation flags used in <see cref="Camera.PlayAnim"/>.
    /// </summary>
    [Flags]
    public enum CamAnimationFlags
    {
        None = 0,
        /// <summary>
        /// Repeat the animation.
        /// </summary>
        Looping = 1
    }
}
