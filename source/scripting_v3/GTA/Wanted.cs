//
// Copyright (C) 2025 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using GTA.Math;
using GTA.Native;

namespace GTA
{
    /// <summary>
    /// Represents a data of player wanted info.
    /// </summary>
    /// <remarks>
    /// For dispatch data, see <see cref="DispatchData"/>.
    /// </remarks>
    public class Wanted
    {
        internal Wanted(int playerIndex)
        {
            _playerIndex = playerIndex;
        }

        private readonly int _playerIndex;

        /// <summary>
        /// Gets or sets the wanted level for this <see cref="Player"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Will refocus the search area if you set a value less than the current value and is not zero.
        /// </para>
        /// <para>
        /// Hardcoded to clamp to at most 5 since <c>SET_PLAYER_WANTED_LEVEL</c> just sets the pending crime value to
        /// zero when passed wanted level is a value other than from 1 to 5 (inclusive).
        /// Also, the game does not read <c>WantedLevel6</c> items from <c>dispatch.meta</c>.
        /// </para>
        /// </remarks>
        public int WantedLevel
        {
            get => Function.Call<int>(Hash.GET_PLAYER_WANTED_LEVEL, _playerIndex);
        }

        /// <summary>
        /// Sets the wanted level to a specified value with the 10-second delay. If <paramref name="newLevel"/> is
        /// the same as <see cref="WantedLevel"/> or smaller, the change will get applied immediately.
        /// </summary>
        /// <param name="newLevel">The new wanted level to set.</param>
        /// <param name="delayLawResponse">
        /// If <see langword="true"/>, law response will be delayed and the hidden evasion will get started later
        /// because of the law response delay.
        /// The law response delay time varies for population zones and the delay time for the population zone where
        /// the <see cref="Player"/> <see cref="Ped"/> is in will be used.
        /// </param>
        /// <remarks>
        /// <para>
        /// The wanted level change will not take place for 10 seconds (emulating the time it takes a citizen to report
        /// the crime) unless <paramref name="newLevel"/> is the same as <see cref="WantedLevel"/> or smaller.
        /// Call <see cref="ApplyWantedLevelChangeNow"/> after this method if you wish to apply the new wanted level
        /// without having to wait for 10 seconds. If <paramref name="newLevel"/> is the same as
        /// <see cref="WantedLevel"/> or smaller, the change will get applied immediately, as well as the changes of
        /// <see cref="CurrentCrimeValue"/> and <see cref="LastPositionSpottedByPolice"/>.
        /// </para>
        /// <para>
        /// Will refocus the search area if you set a value less than the current value and is not zero.
        /// </para>
        /// <para>
        /// Hardcoded to clamp to at most 5 since <c>SET_PLAYER_WANTED_LEVEL</c> just sets <see cref="NewCrimeValue"/>
        /// to zero when passed wanted level is a value other than from 1 to 5 (inclusive).
        /// Also, the game does not read <c>WantedLevel6</c> items from <c>dispatch.meta</c>.
        /// </para>
        /// </remarks>
        public void SetWantedLevel(int newLevel, bool delayLawResponse)
        {
            Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, _playerIndex, newLevel, delayLawResponse);
        }

        /// <summary>
        /// Sets the wanted level with the 10-second delay if <paramref name="newLevel"/> is greater than
        /// <see cref="WantedLevel"/> and the desired crime value (threshold) is more than
        /// <see cref="CurrentCrimeValue"/> or the same.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The wanted level change will not take place for 10 seconds (emulating the time it takes a citizen to report
        /// the crime). Call <see cref="ApplyWantedLevelChangeNow"/> after this method if you wish to apply the new
        /// wanted level without having to wait for 10 seconds.
        /// </para>
        /// <para>
        /// Hardcoded to clamp to at most 5 since <c>SET_PLAYER_WANTED_LEVEL</c> just sets <see cref="NewCrimeValue"/>
        /// to zero when passed wanted level is a value other than from 1 to 5 (inclusive).
        /// Also, the game does not read <c>WantedLevel6</c> items from <c>dispatch.meta</c>.
        /// </para>
        /// </remarks>
        /// <inheritdoc cref="SetWantedLevel"/>
        public void SetWantedLevelNoDrop(int newLevel, bool delayLawResponse)
        {
            Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NO_DROP, _playerIndex, newLevel, delayLawResponse);
        }

        /// <summary>
        /// Applies <see cref="NewCrimeValue"/> immediately, which can be set with <see cref="SetWantedLevel"/> and
        /// <see cref="SetWantedLevelNoDrop"/>.
        /// </summary>
        /// <param name="delayLawResponse">
        /// If <see langword="true"/>, law response will be delayed and the hidden evasion will get started later
        /// because of the law response delay.
        /// The law response delay time varies for population zones and the delay time for the population zone where
        /// the <see cref="Player"/> <see cref="Ped"/> is in will be used.
        /// </param>
        public void ApplyWantedLevelChangeNow(bool delayLawResponse)
        {
            Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, _playerIndex, delayLawResponse);
        }

        /// <summary>
        /// Gets the last position spotted by police for this <see cref="Player"/>.
        /// </summary>
        /// <value>
        /// The place in world coordinates where the last position spotted by police.
        /// </value>
        public Vector3 LastPositionSpottedByPolice
        {
            get => Function.Call<Vector3>(Hash.GET_PLAYER_WANTED_CENTRE_POSITION, _playerIndex);
        }

        /// <summary>
        /// Sets <see cref="LastPositionSpottedByPolice"/> and the center position of the current search area.
        /// </summary>
        public void SetRadiusCenter(Vector3 pos)
        {
            Function.Call(Hash.SET_PLAYER_WANTED_CENTRE_POSITION, _playerIndex, pos.X, pos.Y, pos.Z);
        }

        /// <summary>
        /// Gets or sets the current crime value that determines the real wanted level when the game updates the real
        /// wanted level.
        /// </summary>
        /// <remarks>
        /// For instance, if this value is 32 and a vehicle theft crime you started gets reported (increases by 18)
        /// without crime directly getting spotted by the police,
        /// this value will be 50 and the wanted level will be one when the game updates the real wanted level using
        /// this value.
        /// </remarks>
        /// <value>
        /// The current crime value.
        /// </value>
        public int CurrentCrimeValue
        {
            get
            {
                if (SHVDN.NativeMemory.CurrentCrimeValueOffset == 0)
                {
                    return 0;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return 0;
                }

                return SHVDN.MemDataMarshal.ReadInt32(cWantedAddress + SHVDN.NativeMemory.CurrentCrimeValueOffset);
            }
            set
            {
                if (SHVDN.NativeMemory.CurrentCrimeValueOffset == 0)
                {
                    return;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return;
                }

                SHVDN.MemDataMarshal.WriteInt32(cWantedAddress + SHVDN.NativeMemory.CurrentCrimeValueOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the pending/new crime value that will be applied when the game ticks
        /// if <see cref="TimeWhenNewCrimeValueTakesEffect"/> is not zero and less than <see cref="Game.GameTime"/>.
        /// </summary>
        /// <remarks>
        /// The game sets this value only when this <see cref="Player"/> commit a crime that will immediately increase
        /// their wanted level such as targeting a police officer, when <c>SET_PLAYER_WANTED_LEVEL</c> (which
        /// <see cref="SetWantedLevel"/> calls) is called and the wanted level is to increase, or when the game applies
        /// this value to <see cref="CurrentCrimeValue"/>.
        /// </remarks>
        /// <value>
        /// The pending/new crime value.
        /// </value>
        public int NewCrimeValue
        {
            get
            {
                if (SHVDN.NativeMemory.NewCrimeValueOffset == 0)
                {
                    return 0;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return 0;
                }

                return SHVDN.MemDataMarshal.ReadInt32(cWantedAddress + SHVDN.NativeMemory.NewCrimeValueOffset);
            }
            set
            {
                if (SHVDN.NativeMemory.NewCrimeValueOffset == 0)
                {
                    return;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return;
                }

                SHVDN.MemDataMarshal.WriteInt32(cWantedAddress + SHVDN.NativeMemory.NewCrimeValueOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the game time when <see cref="NewCrimeValue"/> will be set to <see cref="CurrentCrimeValue"/>.
        /// If zero, the game will not apply <see cref="NewCrimeValue"/>.
        /// </summary>
        /// <remarks>
        /// The game sets this value only when <c>SET_PLAYER_WANTED_LEVEL</c> is called and the wanted level is to
        /// increase or when the game applies <see cref="NewCrimeValue"/> to <see cref="CurrentCrimeValue"/> and set
        /// this value to zero.
        /// </remarks>
        /// <value>
        /// The game time when <see cref="NewCrimeValue"/> will be set to <see cref="CurrentCrimeValue"/>.
        /// </value>
        public int TimeWhenNewCrimeValueTakesEffect
        {
            get
            {
                if (SHVDN.NativeMemory.TimeWhenNewCrimeValueTakesEffectOffset == 0)
                {
                    return 0;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return 0;
                }

                return SHVDN.MemDataMarshal.ReadInt32(cWantedAddress + SHVDN.NativeMemory.TimeWhenNewCrimeValueTakesEffectOffset);
            }
            set
            {
                if (SHVDN.NativeMemory.TimeWhenNewCrimeValueTakesEffectOffset == 0)
                {
                    return;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return;
                }

                SHVDN.MemDataMarshal.WriteInt32(cWantedAddress
                    + SHVDN.NativeMemory.TimeWhenNewCrimeValueTakesEffectOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the last time when the search area got refocused for this <see cref="Player"/>.
        /// When you commit a crime that refocus the search area, this value will update.
        /// </summary>
        /// <remarks>
        /// The game will set this value to zero when the wanted level is zero.
        /// </remarks>
        public int TimeSearchLastRefocused
        {
            get
            {
                if (SHVDN.NativeMemory.CWantedTimeSearchLastRefocusedOffset == 0)
                {
                    return 0;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return 0;
                }

                return SHVDN.MemDataMarshal.ReadInt32(cWantedAddress
                                                      + SHVDN.NativeMemory.CWantedTimeSearchLastRefocusedOffset);
            }
            set
            {
                if (SHVDN.NativeMemory.CWantedTimeSearchLastRefocusedOffset == 0)
                {
                    return;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return;
                }

                SHVDN.MemDataMarshal.WriteInt32(cWantedAddress
                                                + SHVDN.NativeMemory.CWantedTimeSearchLastRefocusedOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the last game time when this <see cref="Player"/> is spotted by the police.
        /// The game will set this value to zero when the wanted level is zero.
        /// </summary>
        /// <remarks>
        /// The game will set to the game time as long as this <see cref="Player"/> is spotted by the police each
        /// frame, but you can make the <see cref="Player"/> getting in the hidden evasion phase up to 1 or 2 seconds
        /// if the police does not know where the <see cref="Player"/> is.
        /// </remarks>
        public int TimeLastSpotted
        {
            get
            {
                if (SHVDN.NativeMemory.CWantedTimeLastSpottedOffset == 0)
                {
                    return 0;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return 0;
                }

                return SHVDN.MemDataMarshal.ReadInt32(cWantedAddress
                                                      + SHVDN.NativeMemory.CWantedTimeLastSpottedOffset);
            }
            set
            {
                if (SHVDN.NativeMemory.CWantedTimeLastSpottedOffset == 0)
                {
                    return;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return;
                }

                SHVDN.MemDataMarshal.WriteInt32(cWantedAddress + SHVDN.NativeMemory.CWantedTimeLastSpottedOffset, value);
            }
        }

        /// <summary>
        /// Gets or sets the game time when hidden evasion phase gets started.
        /// </summary>
        /// <remarks>
        /// The game will set to zero when this <see cref="Player"/> is spotted by the police each frame,
        /// but you can set small value (but not zero) to clear the wanted level when the <see cref="Player"/> is in
        /// the hidden evasion phase if not suppressed by <c>SUPPRESS_LOSING_WANTED_LEVEL_IF_HIDDEN_THIS_FRAME</c>.
        /// </remarks>
        public int TimeHiddenEvasionStarted
        {
            get
            {
                if (SHVDN.NativeMemory.CWantedTimeHiddenEvasionStartedOffset == 0)
                {
                    return 0;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return 0;
                }

                return SHVDN.MemDataMarshal.ReadInt32(cWantedAddress
                                                      + SHVDN.NativeMemory.CWantedTimeHiddenEvasionStartedOffset);
            }
            set
            {
                if (SHVDN.NativeMemory.CWantedTimeHiddenEvasionStartedOffset == 0)
                {
                    return;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return;
                }

                SHVDN.MemDataMarshal.WriteInt32(cWantedAddress
                                                + SHVDN.NativeMemory.CWantedTimeHiddenEvasionStartedOffset, value);
            }
        }
        /// <summary>
        /// Returns <see langword="true"/> if this <see cref="Wanted"/> has a wanted level and the stars are displayed
        /// gray to indicate that cops are searching.
        /// </summary>
        /// <remarks>
        /// Technically, this property returns <see langword="true"/> when the flag for stars graying out is set
        /// and <see cref="TimeLastSpotted"/> has more value by more than <c>1000</c> or <c>2000</c> (depending on
        /// an unknown state).
        /// than <see cref="Game.GameTime"/>.
        /// </remarks>
        public bool HasGrayedOutStars => Function.Call<bool>(Hash.ARE_PLAYER_STARS_GREYED_OUT, _playerIndex);

        /// <summary>
        /// Gets a value that indicates whether this <see cref="Player"/> is ignored by the police.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Player"/> is ignored by the police; otherwise,
        /// <see langword="false"/>.
        /// </value>
        public bool PoliceBackOff
        {
            get
            {
                if (SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset == 0)
                {
                    return false;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return false;
                }

                return SHVDN.MemDataMarshal.IsBitSet(cWantedAddress + SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset,
                    16);
            }
        }

        /// <summary>
        /// Sets <see cref="PoliceBackOff"/> to a specified value. Also stops every law enforcement vehicles that has
        /// a NPC driver by setting the velocity to the zero vector if set to <see langword="true"/>.
        /// </summary>
        /// <param name="value">
        /// <see langword="true"/> if this <see cref="Player"/> should be ignored by the police; otherwise,
        /// <see langword="false"/>.
        /// </param>
        public void SetPoliceIgnorePlayer(bool value)
        {
            Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, _playerIndex, value);
        }

        /// <summary>
        /// Gets or sets a value that indicates whether this <see cref="Player"/> is ignored by everyone.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Player"/> is ignored by everyone; otherwise,
        /// <see langword="false"/>.
        /// </value>
        public bool EverybodyBackOff
        {
            get
            {
                if (SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset == 0)
                {
                    return false;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return false;
                }

                return SHVDN.MemDataMarshal.IsBitSet(cWantedAddress + SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset,
                    18);
            }
        }

        /// <summary>
        /// Sets <see cref="EverybodyBackOff"/> to a specified value. Also stops every law enforcement vehicles that
        /// has a NPC driver by setting the velocity to the zero vector if set to <see langword="true"/>.
        /// </summary>
        /// <param name="value">
        /// <see langword="true"/> if this <see cref="Player"/> should be ignored by everyone; otherwise,
        /// <see langword="false"/>.
        /// </param>
        public void SetEveryoneIgnorePlayer(bool value)
        {
            Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, _playerIndex, value);
        }

        /// <summary>
        /// Gets or sets a value that indicates whether cops will be dispatched for this <see cref="Player"/>.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if cops will be dispatched; otherwise, <see langword="false"/>.
        /// </value>
        public bool DispatchesCopsForPlayer
        {
            get
            {
                if (SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset == 0)
                {
                    return false;
                }

                IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
                if (cWantedAddress == IntPtr.Zero)
                {
                    return false;
                }

                return !SHVDN.MemDataMarshal.IsBitSet(cWantedAddress
                                                      + SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset, 23);
            }
            set => Function.Call(Hash.SET_DISPATCH_COPS_FOR_PLAYER, _playerIndex, value);
        }

        /// <summary>
        /// Sets the wanted level for this <see cref="Player"/> but without refocusing the search area.
        /// </summary>
        /// <remarks>
        /// When the previous wanted level is zero, you cannot avoid refocusing the search area with this method.
        /// </remarks>
        public void SetWantedLevelNoRefocusSearchArea(int wantedLevel)
        {
            if (SHVDN.NativeMemory.CurrentCrimeValueOffset == 0 || SHVDN.NativeMemory.CurrentWantedLevelOffset == 0)
            {
                return;
            }

            IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(_playerIndex);
            if (cWantedAddress == IntPtr.Zero)
            {
                return;
            }

            int currentWantedLevel = SHVDN.MemDataMarshal.ReadInt32(cWantedAddress
                                                                    + SHVDN.NativeMemory.CurrentWantedLevelOffset);

            if (wantedLevel <= 0 || wantedLevel >= currentWantedLevel)
            {
                // Just call SET_PLAYER_WANTED_LEVEL and SET_PLAYER_WANTED_LEVEL_NOW, because setting to the current
                // wanted level or above won't refocus the search area (the crime value will be reset)
                Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, _playerIndex, wantedLevel, false);
                Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, _playerIndex, false);
                return;
            }

            // Clamps and/or sets the wanted level just like SET_PLAYER_WANTED_LEVEL does
            int wantedLevelToApply = wantedLevel;
            if (wantedLevelToApply >= 6)
            {
                wantedLevelToApply = 5;
            }

            // This additional crime value is hardcoded in a function that is called by SET_PLAYER_WANTED_LEVEL
            const int ADDITIONAL_CRIME_VALUE = 20;
            int threshold = Function.Call<int>(Hash.GET_WANTED_LEVEL_THRESHOLD, _playerIndex, wantedLevelToApply);

            CurrentCrimeValue = threshold + ADDITIONAL_CRIME_VALUE;
            SHVDN.MemDataMarshal.WriteInt32(cWantedAddress + SHVDN.NativeMemory.CurrentWantedLevelOffset,
                wantedLevelToApply);

            // Set the pending crime value just like SET_PLAYER_WANTED_LEVEL does (`SET_PLAYER_WANTED_LEVEL_NOW` does
            // not clear the value)
            NewCrimeValue = threshold + ADDITIONAL_CRIME_VALUE;
        }

        /// <summary>
        /// Reports a crime for this <see cref="Player"/>.
        /// </summary>
        /// <param name="crimeToReport">The crime time to report.</param>
        /// <param name="crimeValue">
        /// If left at zero, the crime will get evaluated.
        /// Else, the crime value will be overridden to specify an amount (can be both positive or negative).
        /// </param>
        /// <remarks>
        /// Clearing the wanted level will disable to increase the crime value for commiting crimes for 2 seconds.
        /// </remarks>
        public void ReportCrime(CrimeType crimeToReport, int crimeValue = 0)
            => Function.Call(Hash.REPORT_CRIME, _playerIndex, (int)crimeToReport, crimeValue);

        /// <summary>
        /// Forces this <see cref="Player"/> to get spotted by police.
        /// </summary>
        /// <remarks>
        /// Unlike when you commit a crime that refocuses the search area, this method also updates
        /// <see cref="TimeLastSpotted"/>.
        /// </remarks>
        public void ReportPoliceSpottingPlayer() => Function.Call(Hash.REPORT_POLICE_SPOTTED_PLAYER, _playerIndex);
        /// <summary>
        /// Force hidden evasion to start for this <see cref="Player"/>, making wanted stars flashing and cops using
        /// vision cones to search for the player.
        /// You can use this method at any point that police know where this player is.
        /// </summary>
        public void ForceStartHiddenEvasion() => Function.Call(Hash.FORCE_START_HIDDEN_EVASION, _playerIndex);
    }
}
