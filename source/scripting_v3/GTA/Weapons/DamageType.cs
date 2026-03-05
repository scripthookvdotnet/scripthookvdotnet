namespace GTA
{
    public enum DamageType
    {
        /// <summary>
        /// The source of damage is unknown or cannot be determined.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// No damage can be applied. Used for harmless interactions such as snowballs, flares or petrol cans.
        /// </summary>
        None,

        /// <summary>
        /// Damage caused by hand-to-hand combat or melee weapons such as fists, bats, knives, and similar.
        /// </summary>
        /// <remarks>
        /// <para>The Shocker is counted as <see cref="Electric"/> instead of <see cref="Melee"/>.</para>
        /// </remarks>
        Melee,

        /// <summary>
        /// Damage from standard ballistic firearms, including pistols, rifles, shotguns and SMGs.
        /// </summary>
        /// <remarks>
        /// <para>Mk II explosive rounds will register as <see cref="Explosive"/> instead of <see cref="Bullet"/>.</para>
        /// <para>Other Mk II ammo types such as Hollow Point or FMJ do not affect the damage type.</para>
        /// </remarks>
        Bullet,

        /// <summary>
        /// Damage from non-lethal rubber bullets. Rarely used in vanilla.
        /// </summary>
        /// <remarks>
        /// <para>See <see cref="Bullet"/> for standard ballistic damage.</para>
        /// </remarks>
        BulletRubber,

        /// <summary>
        /// Damage from explosive sources such as grenades, RPGs, bombs or the Railgun.
        /// </summary>
        /// <remarks>
        /// <para>Includes damage from Mk II explosive rounds. See <see cref="Bullet"/> for standard ballistic damage.</para>
        /// </remarks>
        Explosive,

        /// <summary>
        /// Damage caused by fire-based sources, such as Molotovs or incendiary rounds.
        /// </summary>
        Fire,

        /// <summary>
        /// Damage resulting from vehicle collisions with entities or objects.
        /// </summary>
        Collision,

        /// <summary>
        /// Damage caused by falling from heights.
        /// </summary>
        Fall,

        /// <summary>
        /// Damage caused by drowning when an entity is submerged underwater.
        /// </summary>
        Drown,

        /// <summary>
        /// Damage from electric-based weapons, such as the Taser or Shocker.
        /// </summary>
        /// <remarks>
        /// <para>The Shocker is counted as <see cref="Electric"/> despite being a melee weapon. See <see cref="Melee"/>.</para>
        /// </remarks>
        Electric,

        /// <summary>
        /// Damage caused by environmental barbed wire.
        /// </summary>
        BarbedWire,

        /// <summary>
        /// Damage caused by direct contact with a fire extinguisher's spray.
        /// </summary>
        FireExtinguisher,

        /// <summary>
        /// Damage from smoke-based grenades or gas, including tear gas or BZ gas.
        /// </summary>
        Smoke,

        /// <summary>
        /// Damage caused by mounted water cannon weapons, such as those on fire trucks.
        /// </summary>
        WaterCannon,

        /// <summary>
        /// Damage from tranquilizer darts or weapons, causing sedation rather than lethal harm.
        /// </summary>
        Tranquilizer
    }
}
