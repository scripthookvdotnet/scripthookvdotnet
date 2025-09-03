//
// Copyright (C) 2025 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    /// <summary>
    /// An enumeration of all the possible vehicle stuck types, which represents the same enum as
    /// `<c>eVehStuckTypes</c>` in the game codebase.
    /// </summary>
    public enum VehicleStuckType
    {
        /// <summary>
        /// The associated stuck timer increases when the <see cref="Vehicle"/> is rotated roughly more than
        /// 107.458 degrees around some axis on xy-plane from the identity rotation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Strictly, the associated stuck timer increases when the value at index `<c>[2, 2]</c>` of
        /// <see cref="Entity.Matrix"/> of the <see cref="Vehicle"/>, which is the Z coordinate (3rd column) of
        /// the local Z axis (3rd row), is less than `<c>-0.3</c>`, and resets to zero otherwise.
        /// </para>
        /// <para>
        /// Since the game assumes matrix scales of all <see cref="Entity"/>s are the same as the unit size when it
        /// does various stuff on them, and the timer may not work properly if the <see cref="Vehicle"/>'s matrix scale
        /// is too different.
        /// </para>
        /// </remarks>
        OnRoof,
        /// <summary>
        /// The associated stuck timer increases when the <see cref="Vehicle"/> is rotated roughly more than
        /// 30 degrees around some axis on xy-plane from the identity rotation, there are some of the wheels
        /// of the <see cref="Vehicle"/> that do not contact anything.
        /// </summary>
        /// <remarks>
        /// Strictly, the associated stuck timer increases when the value at index `<c>[2, 2]</c>` of
        /// <see cref="Entity.Matrix"/> of the <see cref="Vehicle"/>, which is the Z coordinate (3rd column) of
        /// the local Z axis (3rd row), is less than `<c>0.5</c>`, and there are some of the wheels of
        /// the <see cref="Vehicle"/> that do not contact anything. If <see cref="VehicleType"/> of
        /// the <see cref="Vehicle"/> is a bike type (including <see cref="VehicleType.Bicycle"/>), it must not
        /// be marked as "abandoned" as well before the timer can increase. Resets to zero when some of the conditions
        /// mentioned are not met.
        /// <para>
        /// Since the game assumes matrix scales of all <see cref="Entity"/>s are the same as the unit size when it
        /// does various stuff on them, and the timer may not work properly if the <see cref="Vehicle"/>'s matrix scale
        /// is too different.
        /// </para>
        /// </remarks>
        OnSide,
        /// <summary>
        /// The associated stuck timer increases when none of the drive wheels of the <see cref="Vehicle"/> is touching
        /// if it is a land one or a plane that does not have the vertical flight mode. For boats and submarine
        /// vehicles that have colliders (which need to physically move), the timer increase when the body is not in
        /// water at all or the propeller not in water, and the magnitude of collision impulse is not zero.
        /// Resets to zero when some of the conditions appropriate for the <see cref="Vehicle"/> are not met.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The hung-up stuck timer is not get updated when some fixed flag is set in the <see cref="Vehicle"/> such as
        /// the one that is changed by <see cref="Entity.IsCollisionEnabled"/> and the one that is set to
        /// <see langword="true"/> when the <see cref="Vehicle"/> is waiting for collision around it.
        /// If <see cref="VehicleType"/> of the <see cref="Vehicle"/> is a bike type (including
        /// <see cref="VehicleType.Bicycle"/>), it is marked as "abandoned", and it is not marked as inaccessible (by
        /// a enter vehicle task that is trying to have a <see cref="Ped"/> enter a open seat), the stuck timer will
        /// not get updated regardless of whether any fixed flag is set.
        /// The hung-up stuck timer will be reset to zero if the position delta is too large regardless of whether
        /// the conditions mentioned above will be met.
        /// </para>
        /// <para>
        /// There are some special cases to determine whether the stuck timer will increase. The cases mentioned below
        /// will not apply when the conditions not to get updated are met or the position delta is too large.
        /// <list type="bullet">
        /// <item><description>
        /// If the <see cref="Vehicle"/> is considered a "watercraft" in the internal stuck check update function but
        /// does not have a collider, the conditions for land <see cref="Vehicle"/>s will be used to determine whether
        /// the stuck timer will be increased.
        /// </description></item>
        /// <item><description>
        /// If the <see cref="Vehicle"/> is a boat, the hung-up stuck timer increases even when the propeller is in
        /// water but not fully submerged.
        /// </description></item>
        /// <item><description>
        /// The timer will be reset to zero if it is a helicopter or a plane that has a vertical flight mode, if
        /// the <see cref="Vehicle"/> is an amphibious quad bike that is not touching water at all and its wheels are
        /// fully retracted, or if <see cref="Vehicle.SpecialFlightModeCurrentRatio"/> on the <see cref="Vehicle"/> is
        /// exactly set to `<c>1f</c>`.
        /// </description></item>
        /// <item><description>
        /// If the <see cref="Vehicle"/> is marked as inaccessible by a enter vehicle task that is trying to have
        /// a <see cref="Ped"/> enter a open seat, the timer will increase. Open seats are ones of
        /// <see cref="Vehicle"/>s where the <see cref="VehicleType"/>s of them are
        /// <see cref="VehicleType.Motorcycle"/>, <see cref="VehicleType.Bicycle"/>,
        /// <see cref="VehicleType.AmphibiousQuadBike"/>, or <see cref="VehicleType.Boat"/> with a model flag
        /// <see cref="Model.IsJetSki"/>.
        /// </description></item>
        /// </list>
        /// </para>
        /// <para>
        /// There are some special cases that makes the <see cref="Vehicle"/> not hung-up after testing the 2 regular
        /// cases and the special cases mentioned in the above bullet lists. If the <see cref="Vehicle"/> is a boat or
        /// an amphibious quad bike and is using a low LOD anchor, if it is a submarine car and is using the submarine
        /// mode, if it is considered a "watercraft" and collision is not loaded around the position, the timer will be
        /// reset to zero.
        /// These conditions will not apply when the conditions not to get updated are met or the position delta is too
        /// large.
        /// </para>
        /// <para>
        /// Collision being not loaded around the <see cref="Vehicle"/> does not necessarily avoid the hung-up stuck
        /// timer being increased unless it is considered a "watercraft" Therefore, hung-up stuck timers on land
        /// vehicles with the collision not being loaded around them are likely to increase as long as none of
        /// the drive wheels of them is touching anything.
        /// </para>
        /// </remarks>
        HungUp,
        /// <summary>
        /// The associated stuck timer increases when the throttle is strong enough is touching and none of the brake
        /// and handbrake is used at all if it is a land one. The throttle threshold is higher for
        /// <see cref="Vehicle"/>s that <see cref="Player"/> <see cref="Ped"/>s are driving.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The jammed stuck timer is not get updated when some fixed flag is set in the <see cref="Vehicle"/> such as
        /// the one that is changed by <see cref="Entity.IsCollisionEnabled"/> and the one that is set to
        /// <see langword="true"/> when the <see cref="Vehicle"/> is waiting for collision around it, or when
        /// <see cref="Vehicle.IsEngineStarting"/> on the <see cref="Vehicle"/> is <see langword="true"/>.
        /// </para>
        /// <para>
        /// There are some special cases to determine whether the jammed timer will increase. The cases mentioned below
        /// apply in the order listed so later conditions will take precedence but will not apply when either of
        /// the conditions not to get updated is met or the position delta is too large.
        /// <list type="bullet">
        /// <item><description>
        /// If not all of the wheels of the <see cref="Vehicle"/> is touching anything except for physical
        /// <see cref="Entity"/>s that aren't trailer <see cref="Vehicle"/>s, and the z-axis value of the collision
        /// normal is more than `<c>0.5f</c>`, the stuck timer will be increased. This condition was created to detect
        /// if the <see cref="Vehicle"/> is "jammed" driving onto a car transporter.
        /// </description></item>
        /// <item><description>
        /// If the <see cref="Vehicle"/> is an amphibious quad bike and the wheels are fully retracted, the stuck timer
        /// will be reset to zero.
        /// </description></item>
        /// <item><description>
        /// On the condition that the <see cref="Vehicle"/> is a "watercraft", the stuck timer will increase if
        /// the <see cref="Vehicle"/> is a boat, is touching water at all, and is using a low LOD anchor. However,
        /// if it is a "watercraft" and no collision is loaded around it, the stuck timer will be reset to zero.
        /// </description></item>
        /// <item><description>
        /// If the <see cref="Vehicle"/> is a plane and there's a landing gear that is already broken off, the stuck
        /// timer will increase.
        /// </description></item>
        /// <item><description>
        /// If the <see cref="Vehicle"/> is a trailer, the stuck timer will be reset to zero.
        /// </description></item>
        /// </list>
        /// </para>
        /// </remarks>
        Jammed,
        /// <summary>
        /// Resets all of the 4 stuck timers.
        /// </summary>
        /// <remarks>
        /// Cannot be used for <see cref="Vehicle.IsStuckTimerUp"/>.
        /// </remarks>
        ResetAll,
    }
}
