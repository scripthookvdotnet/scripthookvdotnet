using System;

namespace GTA.Models.World.Enums
{
    [Flags]
    public enum InvertAxisType
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4
    }
}