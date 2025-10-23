using System;

namespace GTA
{
    [Flags]
    public enum WeaponCheckFlags
    {
        /// <summary>
        /// No weapons included.  
        /// The native will always return false.
        /// </summary>
        None = 0,

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
        /// Checks for melee or firearms.  
        /// </summary>
        MeleeOrGun = IncludeMelee | IncludeGun,

        /// <summary>
        /// Checks for firearms or explosives.  
        /// </summary>
        GunOrExplosive = IncludeGun | IncludeProjectile, 

        /// <summary>
        /// Checks for any weapon except fists.  
        /// </summary>
        All = IncludeMelee | IncludeProjectile | IncludeGun
    }
}
