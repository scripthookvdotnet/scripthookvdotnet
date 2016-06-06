using GTA.Native;
using System;
using System.Collections.Generic;

namespace GTA
{
    public enum VehicleDoorIndex
    {
        FrontRightDoor = 1,
        FrontLeftDoor = 0,
        BackRightDoor = 3,
        BackLeftDoor = 2,
        Hood = 4,
        Trunk
    }

    public sealed class VehicleDoorCollection
    {
        #region Fields
        Vehicle _owner;
        readonly Dictionary<int, VehicleDoor> _vehicleDoors = new Dictionary<int, VehicleDoor>();
        #endregion

        internal VehicleDoorCollection(Vehicle owner)
        {
            _owner = owner;
        }

        public VehicleDoor this[VehicleDoorIndex index]
        {
            get
            {
                VehicleDoor vehicleDoor = null;

                if (!_vehicleDoors.TryGetValue((int)index, out vehicleDoor))
                {
                    vehicleDoor = new VehicleDoor(_owner, index);
                    _vehicleDoors.Add((int)index, vehicleDoor);
                }

                return vehicleDoor;
            }
        }
    }
}
