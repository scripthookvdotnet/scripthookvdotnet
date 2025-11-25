namespace GTA
{
    /// <summary>
    /// Represents valid <see cref="VehicleExtra"/>s for <see cref="Vehicle"/>s.
    /// </summary>
    public enum VehicleExtraIndex
    {
        Extra1 = 1,
        Extra2,
        Extra3,
        Extra4,
        Extra5,
        Extra6,
        Extra7,
        Extra8,
        Extra9,
        Extra10,
        Extra11,
        Extra12,
        Extra13,
        Extra14,
        Extra15,
        Extra16,
    }

    internal static class VehicleExtraExtensions
    {
        /// <summary>
        /// Returns the internal bone name for this <see cref="VehicleExtraIndex"/>.
        /// </summary>
        /// <param name="extra">The extra index whose bone name should be resolved.</param>
        /// <returns>
        /// The bone name for the specified <see cref="VehicleExtraIndex"/>.
        /// <para>
        /// Special case: <see cref="VehicleExtraIndex.Extra10"/> returns <c>extra_ten</c>.  
        /// All other values return <c>extra_{index}</c>.
        /// </para>
        /// </returns>
        internal static string GetBoneName(this VehicleExtraIndex extra) => VehicleExtraHelpers.GetBoneName(extra);
    }

    internal static class VehicleExtraHelpers
    {
        /// <summary>
        /// Returns the internal bone name for the specified <see cref="VehicleExtraIndex"/>.
        /// </summary>
        /// <param name="extra">The extra index whose bone name should be resolved.</param>
        /// <returns>
        /// The bone name for the specified <see cref="VehicleExtraIndex"/>.
        /// <para>
        /// Special case: <see cref="VehicleExtraIndex.Extra10"/> returns <c>extra_ten</c>.  
        /// All other values return <c>extra_{index}</c>.
        /// </para>
        /// </returns>
        internal static string GetBoneName(VehicleExtraIndex extra)
        {
            if (extra == VehicleExtraIndex.Extra10)
            {
                return "extra_ten";
            }

            return $"extra_{(int)extra}";
        }
    }
}
