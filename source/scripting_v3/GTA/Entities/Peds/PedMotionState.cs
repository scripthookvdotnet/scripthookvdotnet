//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    /// <summary>
    /// An enumeration of all the possible motion states for <see cref="Ped"/>s, which represents
    /// `<c>CPedMotionStates::eMotionState</c>` (mangled name for PSO parsers: <c>CPedMotionStates__eMotionState</c>).
    /// </summary>
    /// <remarks>
    /// All the canonical names have the prefix "<c>MotionState_</c>" at the beginning.
    /// </remarks>
    public enum PedMotionState : uint
    {
        None = 0xEE717723,
        Idle = 0x9072A713,
        Walk = 0xD827C3DB,
        Run = 0xFFF7E7A4,
        Sprint = 0xBD8817DB,
        /// <remarks>
        /// The canonical name is "<c>MotionState_Crouch_Idle</c>".
        /// </remarks>
        CrouchIdle = 0x43FB099E,
        /// <remarks>
        /// The canonical name is "<c>MotionState_Crouch_Walk</c>".
        /// </remarks>
        CrouchWalk = 0x08C31A98,
        /// <remarks>
        /// The canonical name is "<c>MotionState_Crouch_Run</c>".
        /// </remarks>
        CrouchRun = 0x3593CF09,
        DoNothing = 0x0EC17E58,
        AnimatedVelocity = 0x551AAC43,
        InVehicle = 0x94D9D58D,
        Aiming = 0x3F67C6AF,
        /// <remarks>
        /// The canonical name is "<c>MotionState_Driving_Idle</c>".
        /// </remarks>
        DivingIdle = 0x4848CDED,
        /// <remarks>
        /// The canonical name is "<c>MotionState_Driving_Swim</c>".
        /// </remarks>
        DivingSwim = 0x916E828C,
        SwimmingTreadWater = 0xD1BF11C7,
        Dead = 0x0DBB071C,
        /// <remarks>
        /// The canonical name is "<c>MotionState_Stealth_Swim</c>".
        /// </remarks>
        StealthIdle = 0x422D7A25,
        /// <remarks>
        /// The canonical name is "<c>MotionState_Stealth_Walk</c>".
        /// </remarks>
        StealthWalk = 0x042AB6A2,
        /// <remarks>
        /// The canonical name is "<c>MotionState_Stealth_Run</c>".
        /// </remarks>
        StealthRun = 0xFB0B79E1,
        Parachuting = 0xBAC0F10B,
        /// <remarks>
        /// The canonical name is "<c>MotionState_ActionMode_Idle</c>".
        /// </remarks>
        ActionModeIdle = 0xDA40A0DC,
        /// <remarks>
        /// The canonical name is "<c>MotionState_ActionMode_Walk</c>".
        /// </remarks>
        ActionModeWalk = 0xD2905EA7,
        /// <remarks>
        /// The canonical name is "<c>MotionState_ActionMode_Run</c>".
        /// </remarks>
        ActionModeRun = 0x31BADE14,
        Jetpack = 0x535E6A5E
    }
}
