using GTA.Math;
using SHVDN;

namespace GTA
{
    /// <summary>
    /// Struct holding weapon stats for displaying in the HUD.
    /// Used with GET_WEAPON_HUD_STATS.
    /// All values are clamped to the range 0–100.
    /// </summary>
    public struct WeaponHudStats
    {
        /// <summary>
        /// Weapon damage as displayed in the HUD.
        /// </summary>
        public int Damage;

        /// <summary>
        /// Weapon firing speed as displayed in the HUD.
        /// </summary>
        public int Speed;

        /// <summary>
        /// Weapon clip/magazine capacity as displayed in the HUD.
        /// </summary>
        public int Capacity;

        /// <summary>
        /// Weapon accuracy as displayed in the HUD.
        /// </summary>
        public int Accuracy;

        /// <summary>
        /// Effective range of the weapon as displayed in the HUD.
        /// </summary>
        public int Range;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaponHudStats"/> struct.
        /// </summary>
        /// <param name="damage">Damage value (0–100)</param>
        /// <param name="speed">Speed value (0–100)</param>
        /// <param name="capacity">Capacity value (0–100)</param>
        /// <param name="accuracy">Accuracy value (0–100)</param>
        /// <param name="range">Range value (0–100)</param>
        internal WeaponHudStats(int damage, int speed, int capacity, int accuracy, int range)
        {
            Damage = damage;
            Speed = speed;
            Capacity = capacity;
            Accuracy = accuracy;
            Range = range;
        }

        internal static WeaponHudStats Empty => new(0, 0, 0, 0, 0);
        /// <summary>
        /// Converts from the internal, memory-layout struct <see cref="ScrWeaponHudStats"/>.
        /// </summary>
        public static implicit operator WeaponHudStats(ScrWeaponHudStats value)
            => new(value.Damage, value.Speed, value.Capacity, value.Accuracy, value.Range);

        /// <summary>
        /// Converts to the internal, memory-layout struct <see cref="ScrWeaponHudStats"/>.
        /// </summary>
        public static implicit operator ScrWeaponHudStats(WeaponHudStats value)
            => new(value.Damage, value.Speed, value.Capacity, value.Accuracy, value.Range);
    }
}
