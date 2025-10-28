//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
    public enum VehicleMissionType
    {
        None = 0,
        Cruise = 1,
        Ram = 2,
        Block = 3,
        GoTo = 4,
        Stop = 5,
        Attack = 6,
        Follow = 7,
        Flee = 8,
        Circle = 9,
        EscortLeft = 10,
        EscortRight = 11,
        EscortRear = 12,
        EscortFront = 13,
        GoToRacing = 14,
        FollowRecording = 15,
        PoliceBehaviour = 16,
        ParkPerpendicular = 17,
        ParkParallel = 18,
        Land = 19,
        LandAndWait = 20,
        Crash = 21,
        PullOver = 22,
        HeliProtect = 23,

        [Obsolete("Use VehicleMissionType.EscortRear instead.")]
        Escort = EscortRear,
    }
}
