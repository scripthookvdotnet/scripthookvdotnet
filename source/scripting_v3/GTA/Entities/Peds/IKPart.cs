//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;

namespace GTA
{
    /// <summary>
    /// An enumeration of all the possible IK parts that is only used in
    /// <see cref="Ped.SetIKTarget(IKPart, PedBone, Vector3, IKTargetFlags, int, int)"/>.
    /// </summary>
    /// <remarks>
    /// The members for `<c>IK_PART_INVALID</c>`, `<c>IK_PART_SPINE</c>`, `<c>IK_PART_LEG_LEFT</c>`, and
    /// `<c>IK_PART_LEG_RIGHT</c>`, whose values are 0, 2, 5, and 6 respectively, are not defined in this enum.
    /// This is because this enum is only meant to be used in `<c>SET_IK_TARGET</c>` and it actually does not support
    /// any of the 4 members mentioned earlier.
    /// </remarks>
    public enum IKPart
    {
        Head = 1,
        ArmLeft = 3,
        ArmRight = 4,
    }
}
