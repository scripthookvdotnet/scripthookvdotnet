using GTA.Native;
using System;
using System.Collections.Generic;

namespace GTA
{
    public sealed class VehicleWheelCollection
    {
        #region Fields
        Vehicle _owner;
        readonly Dictionary<int, VehicleWheel> _vehicleWheels = new Dictionary<int, VehicleWheel>();
        #endregion

        internal VehicleWheelCollection(Vehicle owner)
        {
            _owner = owner;
        }

        public VehicleWheel this[int index]
        {
            get
            {
                VehicleWheel vehicleWheel = null;

                if (!_vehicleWheels.TryGetValue(index, out vehicleWheel))
                {
                    vehicleWheel = new VehicleWheel(_owner, index);
                    _vehicleWheels.Add(index, vehicleWheel);
                }

                return vehicleWheel;
            }
        }
    }
}
