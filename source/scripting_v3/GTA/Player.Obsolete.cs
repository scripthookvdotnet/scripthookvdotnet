using System;
using GTA.Math;
using GTA.Native;

namespace GTA
{
    public sealed partial class Player : INativeValue
    {
        /// <summary>
        /// Gets or sets the wanted level for this <see cref="Player"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Will refocus the search area if you set a value less than the current value and is not zero.
        /// </para>
        /// <para>
        /// Hardcoded to clamp to at most 5 since <c>SET_PLAYER_WANTED_LEVEL</c> just sets the pending crime value to zero
        /// when passed wanted level is a value other than from 1 to 5 (inclusive).
        /// Also, the game does not read <c>WantedLevel6</c> items from <c>dispatch.meta</c>.
        /// </para>
        /// </remarks>
        public int WantedLevel
        {
            [Obsolete("Use `GTA.Wanted.WantedLevel` instead via the property `GTA.Player.Wanted`.")]
            get => Function.Call<int>(Hash.GET_PLAYER_WANTED_LEVEL, Handle);
            [Obsolete("Use the combination of `SetWantedLevel` and `ApplyWantedLevelChangeNow` of `GTA.Wanted` " +
                      "instead via the property `GTA.Player.Wanted`.")]
            set
            {
                Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Handle, value, false);
                Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Handle, false);
            }
        }


        /// <summary>
        /// Gets or sets the wanted center position for this <see cref="Player"/>.
        /// </summary>
        /// <value>
        /// The place in world coordinates where the police think this <see cref="Player"/> is.
        /// </value>
        public Vector3 WantedCenterPosition
        {
            [Obsolete("Use `GTA.Wanted.LastPositionSpottedByPolice` instead via the property `GTA.Player.Wanted`.")]
            get => Function.Call<Vector3>(Hash.GET_PLAYER_WANTED_CENTRE_POSITION, Handle);
            [Obsolete("Use `GTA.Wanted.SetRadiusCenter` instead via the property `GTA.Player.Wanted`.")]
            set => Function.Call(Hash.SET_PLAYER_WANTED_CENTRE_POSITION, Handle, value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Player"/> is ignored by the police.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="Player"/> is ignored by the police; otherwise, <see langword="false" />.
        /// </value>
        public bool IgnoredByPolice
        {
            [Obsolete("Use `GTA.Wanted.PoliceBackOff` instead via the property `GTA.Player.Wanted`.")]
            get
            {
                if (SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset == 0)
                {
                    return false;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return false;
                }

                return SHVDN.MemDataMarshal.IsBitSet(cWantedAddress + SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset, 16);
            }
            [Obsolete("Use `GTA.Wanted.SetPoliceIgnorePlayer` instead via the property `GTA.Player.Wanted`.")]
            set => Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Handle, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Player"/> is ignored by everyone.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="Player"/> is ignored by everyone; otherwise, <see langword="false" />.
        /// </value>
        public bool IgnoredByEveryone
        {
            [Obsolete("Use `GTA.Wanted.EveryoneBackOff` instead via the property `GTA.Player.Wanted`.")]
            get
            {
                if (SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset == 0)
                {
                    return false;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return false;
                }

                return SHVDN.MemDataMarshal.IsBitSet(cWantedAddress + SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset, 18);
            }
            [Obsolete("Use `GTA.Wanted.SetEveryoneIgnorePlayer` instead via the property `GTA.Player.Wanted`.")]
            set => Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Handle, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether cops will be dispatched for this <see cref="Player"/>.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if cops will be dispatched; otherwise, <see langword="false" />.
        /// </value>
        [Obsolete("Use `GTA.Wanted.DispatchesCopsForPlayer` instead via the property `GTA.Player.Wanted`.")]
        public bool DispatchsCops
        {
            get
            {
                if (SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset == 0)
                {
                    return false;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return false;
                }

                return !SHVDN.MemDataMarshal.IsBitSet(cWantedAddress + SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset, 23);
            }
            set => Function.Call(Hash.SET_DISPATCH_COPS_FOR_PLAYER, Handle, value);
        }
    }
}
