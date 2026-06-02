using System;

namespace GTA.Math
{
    /// <summary>
    /// Represents a helper class for random number generation.
    /// </summary>
    internal static class RandomHelper
    {
        /// <summary>
        /// Gets the random number generator instance that is initialized with a unique seed
        /// based on the current time and a new <see cref="Guid"/>.
        /// </summary>
        public static Random Instance { get; } = new(Guid.NewGuid().GetHashCode());
    }
}
