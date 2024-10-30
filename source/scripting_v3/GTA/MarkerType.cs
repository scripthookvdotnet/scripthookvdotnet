//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;

namespace GTA
{
    /// <summary>
    /// An enumeration of marker types.
    /// </summary>
    /// <remarks>
    /// You can find hardcoded model names that code for marker uses to draw markers, such as "<c>PROP_MK_CONE</c>",
    /// in the exe.
    /// </remarks>
    public enum MarkerType
    {
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_CONE</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_CONE</c>".
        /// </summary>
        Cone,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_CYLINDER</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_CYLINDER</c>".
        /// </summary>
        Cylinder,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_ARROW</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_ARROW_3D</c>".
        /// </summary>
        Arrow,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_ARROW_FLAT</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_ARROW_FLAT</c>".
        /// </summary>
        ArrowFlat,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_FLAG</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_FLAG_2</c>".
        /// </summary>
        Flag,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_RING_FLAG</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_FLAG</c>".
        /// </summary>
        RingFlag,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_RING</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_RING</c>".
        /// </summary>
        Ring,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_PLANE</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_PLANE</c>".
        /// </summary>
        Plane,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_BIKE_LOGO_1</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_BIKE_LOGO_1</c>". The marker is a Lost MC logo that has transparent area inside black outline.
        /// </summary>
        BikeLogo1,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_BIKE_LOGO_1</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_BIKE_LOGO_1</c>". The marker is a Lost MC logo that has solid white area inside black outline.
        /// </summary>
        BikeLogo2,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_0</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_0</c>".
        /// </summary>
        Num0,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_1</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_1</c>".
        /// </summary>
        Num1,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_2</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_2</c>".
        /// </summary>
        Num2,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_3</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_3</c>".
        /// </summary>
        Num3,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_4</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_4</c>".
        /// </summary>
        Num4,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_5</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_5</c>".
        /// </summary>
        Num5,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_6</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_6</c>".
        /// </summary>
        Num6,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_7</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_7</c>".
        /// </summary>
        Num7,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_8</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_8</c>".
        /// </summary>
        Num8,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_9</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_9</c>".
        /// </summary>
        Num9,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_CHEVRON_1</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_RACE_CHEVRON_01</c>".
        /// </summary>
        Chevron1,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_CHEVRON_2</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_RACE_CHEVRON_02</c>".
        /// </summary>
        Chevron2,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_CHEVRON_3</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_RACE_CHEVRON_03</c>".
        /// </summary>
        Chevron3,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_RING_FLAT</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_RING_FLAT</c>". Unlike <see cref="Ring"/>, the marker faces in the Z direction.
        /// </summary>
        RingFlat,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_LAP</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_LAP</c>".
        /// </summary>
        Lap,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_HALO</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_HALO</c>".
        /// </summary>
        Halo,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_HALO_POINT</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_HALO_POINT</c>".
        /// </summary>
        HaloPoint,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_HALO_CIRCLE</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_HALO_CIRCLE</c>".
        /// </summary>
        HaloRotate,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_SPHERE</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_HALO_SPHERE</c>".
        /// </summary>
        Sphere,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_MONEY</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_HALO_MONEY</c>".
        /// </summary>
        Money,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_LINES</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_HALO_LINES</c>".
        /// </summary>
        Lines,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_BEAST</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_HALO_BEAST</c>".
        /// </summary>
        Beast,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_QUESTION_MARK</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_RANDOM_TRANSFORM</c>".
        /// </summary>
        QuestionMark,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_TRANSFORM_PLANE</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_TRANSFORM_PLANE</c>".
        /// </summary>
        TransformPlane,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_TRANSFORM_HELICOPTER</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_TRANSFORM_HELICOPTER</c>".
        /// </summary>
        TransformHelicopter,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_TRANSFORM_BOAT</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_TRANSFORM_BOAT</c>".
        /// </summary>
        TransformBoat,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_TRANSFORM_CAR</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_TRANSFORM_CAR</c>".
        /// </summary>
        TransformCar,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_TRANSFORM_BIKE</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_TRANSFORM_BIKE</c>".
        /// </summary>
        TransformBike,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_TRANSFORM_PUSH_BIKE</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_TRANSFORM_PUSH_BIKE</c>".
        /// </summary>
        TransformPushBike,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_TRANSFORM_TRUCK</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_TRANSFORM_TRUCK</c>".
        /// </summary>
        TransformTruck,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_TRANSFORM_PARACHUTE</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_TRANSFORM_PARACHUTE</c>".
        /// </summary>
        TransformParachute,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_TRANSFORM_THRUSTER</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_TRANSFORM_THRUSTER</c>".
        /// </summary>
        TransformThruster,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_WARP</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_WARP</c>".
        /// </summary>
        Warp,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_BOXES</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_ARENA_ICON_BOXMK</c>".
        /// </summary>
        Boxes,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_PIT_LANE</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_AC_PIT_LANE_BLIP</c>".
        /// </summary>
        PitLane,

        [Obsolete("Use `MarkerType.Cone` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_CONE`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        UpsideDownCone = Cone,
        [Obsolete("Use `MarkerType.Cylinder` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_CYLINDER`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        VerticalCylinder = Cylinder,
        [Obsolete("Use `MarkerType.Arrow` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_ARROW`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        ThickChevronUp = Arrow,
        [Obsolete("Use `MarkerType.ArrowFlat` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_ARROW_FLAT`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        ThinChevronUp = ArrowFlat,
        [Obsolete("Use `MarkerType.Flag` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_FLAG`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        CheckeredFlagRect = Flag,
        [Obsolete("Use `MarkerType.RingFlag` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_RING_FLAG`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        CheckeredFlagCircle = RingFlag,
        /// <summary>
        /// Vehicle Circle
        /// </summary>
        [Obsolete("Use `MarkerType.Ring` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_RING`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        VerticleCircle = Ring,
        [Obsolete("Use `MarkerType.Plane` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_PLANE`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        PlaneModel = Plane,
        [Obsolete("Use `MarkerType.BikeLogo1` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_LOGO_1`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        LostMCDark = BikeLogo1,
        [Obsolete("Use `MarkerType.BikeLogo2` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_LOGO_2`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        LostMCLight = BikeLogo2,
        [Obsolete("Use `MarkerType.Num0` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_NUM_0`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Number0 = Num0,
        [Obsolete("Use `MarkerType.Num1` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_NUM_1`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Number1 = Num1,
        [Obsolete("Use `MarkerType.Num2` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_NUM_2`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Number2 = Num2,
        [Obsolete("Use `MarkerType.Num3` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_NUM_3`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Number3 = Num3,
        [Obsolete("Use `MarkerType.Num4` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_NUM_4`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Number4 = Num4,
        [Obsolete("Use `MarkerType.Num5` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_NUM_5`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Number5 = Num5,
        [Obsolete("Use `MarkerType.Num6` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_NUM_6`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Number6 = Num6,
        [Obsolete("Use `MarkerType.Num7` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_NUM_7`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Number7 = Num7,
        [Obsolete("Use `MarkerType.Num8` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_NUM_8`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Number8 = Num8,
        [Obsolete("Use `MarkerType.Num9` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_NUM_9`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Number9 = Num9,
        [Obsolete("Use `MarkerType.Chevron1` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_CHEVRON_1`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        ChevronUpx1 = Chevron1,
        [Obsolete("Use `MarkerType.Chevron2` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_CHEVRON_2`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        ChevronUpx2 = Chevron2,
        [Obsolete("Use `MarkerType.Chevron3` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_BIKE_CHEVRON_3`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        ChevronUpx3 = Chevron3,
        [Obsolete("Use `MarkerType.RingFlat` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_RING_FLAT`.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        HorizontalCircleFat = RingFlat,
        [Obsolete("Use `MarkerType.Lap` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_LAP.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        ReplayIcon = Lap,
        [Obsolete("Use `MarkerType.Halo` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_HALO.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        HorizontalCircleSkinny = Halo,
        [Obsolete("Use `MarkerType.HaloPoint` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_HALO_POINT.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        HorizontalCircleSkinnyArrow = HaloPoint,
        [Obsolete("Use `MarkerType.HaloRotate` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_HALO_ROTATE.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        HorizontalSplitArrowCircle = HaloRotate,
        [Obsolete("`MarkerType.DebugSphere` is obsolete because \"Debug\" of the name is misleading. " +
                  "Use `MarkerType.Sphere` instead, which is named after the canonical name " +
                  "`MarkerType_e::MARKERTYPE_SPHERE`")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        DebugSphere = Sphere,
    }
}
