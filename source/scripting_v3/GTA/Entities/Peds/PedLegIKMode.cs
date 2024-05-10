namespace GTA
{
    /// <summary>
    /// An enumeration of all the possible <see cref="Ped"/> leg IK mode. No other values are available in the game.
    /// </summary>
    public enum PedLegIKMode
    {
        /// <summary>
        /// No leg IK at all.
        /// </summary>
        Off,
        /// <summary>
        /// Fixup legs based on standing capsule impacts.
        /// </summary>
        Partial,
        /// <summary>
        /// Fixup legs using probes for each foot.
        /// </summary>
        Full,
        /// <summary>
        /// Fixup legs using probes for each foot with melee support.
        /// </summary>
        FullMelee,
    }
}
