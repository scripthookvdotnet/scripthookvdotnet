using System.Collections.Generic;

namespace GTA
{
    public sealed class VehicleExtraCollection
    {
        private const int MaxExtras = 12;
        private Dictionary<VehicleExtraIndex, VehicleExtra> _vehicleExtras = new();

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
    }
}
