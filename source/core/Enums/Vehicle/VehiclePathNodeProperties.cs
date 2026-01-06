using System;

namespace SHVDN
{
    [Flags]
    public enum VehiclePathNodeProperties
    {
        None = 0,
        OffRoad = 1,
        OnPlayersRoad = 2,
        NoBigVehicles = 4,
        SwitchedOff = 8,
        TunnelOrInterior = 16,
        LeadsToDeadEnd = 32,
        /// <summary>
        /// <see cref="Boat"/> takes precedence over this flag.
        /// </summary>
        Highway = 64,
        Junction = 128,
        /// <summary>
        /// Cannot be used with <see cref="GiveWay"/>, because vehicle nodes can have either traffic-light or give-way feature as a special function but cannot have both of them.
        /// </summary>
        TrafficLight = 256,
        /// <summary>
        /// Cannot be used with <see cref="TrafficLight"/>, because vehicle nodes can have either traffic-light or give-way feature as a special function but cannot have both of them.
        /// </summary>
        GiveWay = 512,
        /// <summary>
        /// Cannot be used with <see cref="Highway"/>.
        /// </summary>
        Boat = 1024,

        // GET_VEHICLE_NODE_PROPERTIES will not set any of the values below set as flags
        DontAllowGps = 2048,
    }
}
