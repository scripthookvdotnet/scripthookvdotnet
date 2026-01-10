namespace GTA
{
    /// <summary>
    /// Flags that define various visual and functional properties of a <see cref="Blip"/>.
    /// </summary>
    public enum BlipPropertyFlag
    {
        /// <summary>
        /// Determines whether a <see cref="Blip"/>'s color is rendered with increased brightness.
        /// </summary>
        /// <remarks>
        /// Not all <see cref="BlipColor"/> values are affected. 
        /// For example, <see cref="BlipColor.Blue"/> maps internally to <c>HUD_COLOUR_BLUE</c> by default 
        /// and switches to <c>HUD_COLOUR_BLUELIGHT</c> when this flag is enabled.
        /// </remarks>
        Brightness = 1,

        /// <summary>
        /// Determines whether a <see cref="Blip"/> will flash at intervals defined by <see cref="Blip.FlashInterval"/>.
        /// </summary>
        Flashing,

        /// <summary>
        /// Indicates whether the <see cref="Blip"/> is visible only at short range.
        /// </summary>
        Shortrange,

        /// <summary>
        /// Determines whether the <see cref="Blip"/> has a GPS route attached to it.
        /// </summary>
        Route,

        /// <summary>
        /// Shows the height indicator arrows on a <see cref="Blip"/>.
        /// </summary>
        ShowHeight,

        /// <summary>
        /// Determines whether markers are drawn at long distances (ideal for high-speed races).
        /// </summary>
        MarkerLongDist,

        /// <summary>
        /// Minimizes the <see cref="Blip"/> when it reaches the edge of the map.
        /// </summary>
        MinimiseOnEdge,

        /// <summary>
        /// Marks the <see cref="Blip"/> as "dead."
        /// </summary>
        Dead,

        /// <summary>
        /// Uses a larger vertical distance threshold before displaying the up/down arrows on the <see cref="Blip"/>.
        /// </summary>
        UseExtendedHeightThreshold,

        /// <summary>
        /// Marks a <see cref="Blip"/> as created for a ped in a relationship group.
        /// </summary>
        /// <remarks>
        /// Used in <c>CMiniMap::GetBlipAttachedToEntity</c> to identify blips specifically created for relationship-group peds. 
        /// </remarks>
        CreatedForRelationshipGroupPed,

        /// <summary>
        /// Shows a direction cone on the <see cref="Blip"/>.
        /// </summary>
        ShowCone,

        /// <summary>
        /// Indicates that the <see cref="Blip"/> is associated with a mission creator.
        /// </summary>
        /// <remarks>
        /// Visually, this behaves similarly to <see cref="MinimiseOnEdge"/>. 
        /// However, the <see cref="Blip"/> will not be visible when the exterior map is hidden or when inside an interior.
        /// </remarks>
        MissionCreator,

        /// <summary>
        /// Marks the <see cref="Blip"/> as high detail for the pause map legend.
        /// </summary>
        HighDetail,

        /// <summary>
        /// Hides the <see cref="Blip"/> from the pause map legend.
        /// </summary>
        HiddenOnLegend,

        /// <summary>
        /// Shows a tick indicator on the <see cref="Blip"/>.
        /// </summary>
        ShowTick,

        /// <summary>
        /// Shows a gold tick indicator on the <see cref="Blip"/>.
        /// </summary>
        ShowGoldTick,

        /// <summary>
        /// Shows the "for sale" ($) indicator on the <see cref="Blip"/>.
        /// </summary>
        ShowForSale,

        /// <summary>
        /// Displays the heading/direction indicator on the <see cref="Blip"/>.
        /// </summary>
        ShowHeadingIndicator,

        /// <summary>
        /// Displays an outline indicator on the <see cref="Blip"/>.
        /// </summary>
        ShowOutlineIndicator,

        /// <summary>
        /// Displays the friend indicator on the <see cref="Blip"/>.
        /// </summary>
        ShowFriendIndicator,

        /// <summary>
        /// Displays the crew indicator on the <see cref="Blip"/>.
        /// </summary>
        ShowCrewIndicator,

        /// <summary>
        /// Always shows the height indicator even if the <see cref="Blip"/> is off the edge of the minimap.
        /// </summary>
        UseHeightOnEdge,

        /// <summary>
        /// Marks the <see cref="Blip"/> as being hovered on the pause map.
        /// </summary>
        HoveredOnPausemap,

        /// <summary>
        /// Uses a shorter vertical distance threshold before displaying the up/down arrows on the <see cref="Blip"/>.
        /// </summary>
        UseShortHeightThreshold,
    };
}
