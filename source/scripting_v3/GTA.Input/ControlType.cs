//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//
namespace GTA.Input
{
    public enum ControlType : int
    {
        /// <summary>
        /// Player controls are affected by player control disabling, ped arrest etc.
        /// </summary>
        Player = 0,
        /// <summary>
        /// Camera controls behave like <see cref="Player"/> in native calls, but have their own disable flags internally
        /// </summary>
        Camera = 1,
        /// <summary>
        /// Frontend controls are affected by system UI, loading state, etc.
        /// </summary>
        Frontend = 2,
    }
}
