//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

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
        UpsideDownCone,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_CYLINDER</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_CYLINDER</c>".
        /// </summary>
        VerticalCylinder,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_ARROW</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_ARROW_3D</c>".
        /// </summary>
        ThickChevronUp,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_ARROW_FLAT</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_ARROW_FLAT</c>".
        /// </summary>
        ThinChevronUp,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_FLAG</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_FLAG_2</c>".
        /// </summary>
        CheckeredFlagRect,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_RING_FLAG</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_FLAG</c>".
        /// </summary>
        CheckeredFlagCircle,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_RING</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_RING</c>".
        /// </summary>
        VerticleCircle,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_PLANE</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_PLANE</c>".
        /// </summary>
        PlaneModel,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_BIKE_LOGO_1</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_BIKE_LOGO_1</c>". The marker has transparent area inside black outline (despite this member
        /// name).
        /// </summary>
        LostMCDark,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_BIKE_LOGO_1</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_BIKE_LOGO_1</c>". The marker has solid white area inside black outline.
        /// </summary>
        LostMCLight,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_0</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_0</c>".
        /// </summary>
        Number0,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_1</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_1</c>".
        /// </summary>
        Number1,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_2</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_2</c>".
        /// </summary>
        Number2,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_3</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_3</c>".
        /// </summary>
        Number3,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_4</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_4</c>".
        /// </summary>
        Number4,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_5</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_5</c>".
        /// </summary>
        Number5,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_6</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_6</c>".
        /// </summary>
        Number6,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_7</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_7</c>".
        /// </summary>
        Number7,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_8</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_8</c>".
        /// </summary>
        Number8,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_NUM_9</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_NUM_9</c>".
        /// </summary>
        Number9,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_CHEVRON_1</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_RACE_CHEVRON_01</c>".
        /// </summary>
        ChevronUpx1,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_CHEVRON_2</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_RACE_CHEVRON_02</c>".
        /// </summary>
        ChevronUpx2,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_CHEVRON_3</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_RACE_CHEVRON_03</c>".
        /// </summary>
        ChevronUpx3,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_RING_FLAT</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_RING_FLAT</c>".
        /// </summary>
        HorizontalCircleFat,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_LAP</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_LAP</c>".
        /// </summary>
        ReplayIcon,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_HALO</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_HALO</c>".
        /// </summary>
        HorizontalCircleSkinny,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_HALO_POINT</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_HALO_POINT</c>".
        /// </summary>
        HorizontalCircleSkinny_Arrow,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_HALO_CIRCLE</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_HALO_CIRCLE</c>".
        /// </summary>
        HorizontalSplitArrowCircle,
        /// <summary>
        /// The marker type for <c>MarkerType_e::MARKERTYPE_SPHERE</c>, which uses the prop <see cref="Model"/>
        /// "<c>PROP_MK_HALO_SPHERE</c>".
        /// </summary>
        /// <remarks>
        /// Despite the member name in this enum definition, this member is not meant to use only for debugging purposes
        /// (the member name named wrongly).
        /// </remarks>
        DebugSphere,
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
    }
}
