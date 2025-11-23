using System.Collections.Generic;

namespace GTA
{
    public sealed class VehicleExtraCollection
    {
        /// <summary>
        /// Represents the max numbers of <see cref="VehicleExtra"/>s a <see cref="Vehicle"/> can have.
        /// /// </summary>
        private const int MaxExtras = 12;

        private Dictionary<VehicleExtraIndex, VehicleExtra> _vehicleExtras = new();

        /// <summary>
        /// Gets the owner <see cref="Vehicle"/> of this <see cref="VehicleExtraCollection"/>.
        /// </summary>
        public Vehicle Owner { get; private set; }

        internal VehicleExtraCollection(Vehicle vehicle)
        {
            Owner = vehicle;
        }

        public VehicleExtra this[VehicleExtraIndex extra]
        {
            get
            {
                if (_vehicleExtras.TryGetValue(extra, out VehicleExtra vehicleExtra))
                {
                    return vehicleExtra;
                }

                vehicleExtra = new VehicleExtra(Owner, extra);

                if (!vehicleExtra.Exists())
                {
                    return null;
                }

                _vehicleExtras.Add(extra, vehicleExtra);
                return vehicleExtra;
            }
        }

        /// <summary>
        /// Checks whether a specific <see cref="VehicleExtraIndex"/> exists in this
        /// <see cref="VehicleExtraCollection"/>.
        /// </summary>
        /// <param name="extra">The extra index to test for availability.</param>
        /// <returns>
        /// <c>true</c> if the extra exists in this collection; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(VehicleExtraIndex extra) => _vehicleExtras.ContainsKey(extra);

    }
}
