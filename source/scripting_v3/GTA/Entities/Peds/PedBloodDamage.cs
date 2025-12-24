namespace GTA
{
    /// <summary>
    /// Specifies the type of blood damage applied to a pedestrian.
    /// </summary>
    /// <remarks>
    /// The game does not actually use integer-based data; these enum values are mapped
    /// by SHVDN to the internal string names.
    /// <para>
    /// Each value corresponds to an entry defined in <c>peddamage.xml</c>.
    /// </para>
    /// </remarks>
    public enum PedBloodDamage
    {
        BulletSmall,
        BulletLarge,
        ShotgunSmall,
        ShotgunSmallMonolithic,
        ShotgunLarge,
        NonFatalHeadshot,
        Stab,
        BasicSlash,
        BackSplash,
        ScriptedBackSplash
    }

    internal static class PedBloodDamageExtensions
    {
        /// <summary>
        /// Gets the internal name for this <see cref="PedBloodDamage"/> value.
        /// </summary>
        /// <param name="bloodDamage">
        /// The blood damage type whose internal name should be retrieved.
        /// </param>
        /// <returns>
        /// The corresponding internal engine name, or <see cref="string.Empty"/> if
        /// <paramref name="bloodDamage"/> is outside the valid range.
        /// </returns>
        public static string ToInternalName(this PedBloodDamage bloodDamage) => PedBloodDamageHelpers.GetInternalName(bloodDamage);
    }

    internal static class PedBloodDamageHelpers
    {
        private static readonly string[] s_names = {
            "BulletSmall",
            "BulletLarge",
            "ShotgunSmall",
            "ShotgunSmallMonolithic",
            "ShotgunLarge",
            "NonFatalHeadshot",
            "stab", //This is not a typo, the internal name is lowercase 's'
            "BasicSlash",
            "BackSplash",
            "Scripted_Ped_Splash_Back"
        };

        /// <summary>
        /// Gets the number of defined blood damage entries.
        /// </summary>
        public static readonly int Count = s_names.Length;

        /// <summary>
        /// Gets the internal name for the specified <see cref="PedBloodDamage"/> value.
        /// </summary>
        /// <param name="bloodDamage">
        /// The blood damage type whose internal name should be retrieved.
        /// </param>
        /// <returns>
        /// The corresponding internal engine name, or <see cref="string.Empty"/> if
        /// <paramref name="bloodDamage"/> is outside the valid range.
        /// </returns>
        public static string GetInternalName(PedBloodDamage bloodDamage)
        {
            int val = (int)bloodDamage;

            if (val < 0 || val >= Count)
            {
                return string.Empty;
            }

            return s_names[val];
        }
    }
}
