//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using GTA.Math;

namespace GTA
{
    /// <summary>
    /// An enumeration of all the possible flags that is only used in
    /// <see cref="Ped.SetIKTarget(IKPart, PedBone, Vector3, IKTargetFlags, int, int)"/>.
    /// </summary>
    [Flags]
    public enum IKTargetFlags
    {
        Default = 0,
        /// <summary>
        /// Arm target relative to the hand bone.
        /// </summary>
        /// <remarks>
        /// Has effect only in conjunction with <see cref="IKPart.ArmLeft"/> or <see cref="IKPart.ArmRight"/>.
        /// </remarks>
        ArmTargetWrtHandBone = 1,
        /// <summary>
        /// Arm target relative to the point helper.
        /// </summary>
        /// <remarks>
        /// Has effect only in conjunction with <see cref="IKPart.ArmLeft"/> or <see cref="IKPart.ArmRight"/>.
        /// </remarks>
        ArmTargetWrtPointHelper = 2,
        /// <summary>
        /// Arm target relative to the IK helper.
        /// </summary>
        /// <remarks>
        /// Has effect only in conjunction with <see cref="IKPart.ArmLeft"/> or <see cref="IKPart.ArmRight"/>.
        /// </remarks>
        ArmTargetWrtIKHelper = 4,
        /// <summary>
        /// Use animation tags directly.
        /// </summary>
        /// <remarks>
        /// Has effect only in conjunction with <see cref="IKPart.Head"/>.
        /// </remarks>
        IKTagModeNormal = 8,
        /// <summary>
        /// Use animation tags in ALLOW mode.
        /// </summary>
        /// <remarks>
        /// Has effect only in conjunction with <see cref="IKPart.ArmLeft"/> or <see cref="IKPart.ArmRight"/>.
        /// </remarks>
        IKTagModeAllow = 16,
        /// <summary>
        /// Use animation tags in BLOCK mode.
        /// </summary>
        /// <remarks>
        /// Has effect only in conjunction with <see cref="IKPart.ArmLeft"/> or <see cref="IKPart.ArmRight"/>.
        /// </remarks>
        IKTagModeBlock = 32,
        /// <summary>
        /// Solve for orientation in addition to position.
        /// </summary>
        /// <remarks>
        /// Has effect only in conjunction with <see cref="IKPart.ArmLeft"/> or <see cref="IKPart.ArmRight"/>.
        /// </remarks>
        IKArmUseOrientation = 64
    }
}
