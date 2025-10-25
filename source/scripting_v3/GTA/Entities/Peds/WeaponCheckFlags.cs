using System;

namespace GTA
{
    /// <summary>
    /// An enumeration of flags used for checking what <see cref="Weapon"/>s to check for with <see cref="Ped.IsArmed"/>.
    /// </summary>
    [Flags]
    public enum WeaponCheckFlags
    {
        /// <summary>
        /// Checks only for melee weapons (e.g., bat, knife).  
        /// True only when equipped with a melee weapon.
        /// </summary>
        IncludeMelee = 1,

        /// <summary>
        /// Checks only for explosive/projectile weapons (e.g., RPG, grenade launcher).  
        /// </summary>
        IncludeProjectile = 2,

        /// <summary>
        /// Checks only for firearms (e.g., pistol, rifle).  
        /// </summary>
        IncludeGun = 4,

        /// <summary>
        /// Checks for any weapon except fists.  
        /// </summary>
        All = IncludeMelee | IncludeProjectile | IncludeGun
    }
}
