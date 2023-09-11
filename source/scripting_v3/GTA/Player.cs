//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.ComponentModel;
using System.Drawing;

namespace GTA
{
	public sealed class Player : INativeValue
	{
		#region Fields
		Ped _ped;
		#endregion

		internal Player(int handle)
		{
			Handle = handle;
		}

		public int Handle
		{
			get;
			private set;
		}

		public ulong NativeValue
		{
			get => (ulong)Handle;
			set => Handle = unchecked((int)value);
		}

		/// <summary>
		/// Gets the memory address of <c>CPlayerInfo</c> for this <see cref="Player"/>, where most of the player specific variables are stored in memory.
		/// </summary>
		/// <remarks>
		/// Some the native functions exposed in the namespace <c>PLAYER</c> just access some of members of <c>CPed</c> but none of those of <c>CPlayerInfo</c>.
		/// For example, <see cref="ForcedAim"/> only access one member of <c>CPed</c>, which does not affect non-player <see cref="Ped"/>s.
		/// </remarks>
		public IntPtr MemoryAddress => SHVDN.NativeMemory.GetCPlayerInfoAddress(Handle);

		/// <summary>
		/// Gets the <see cref="Ped"/> this <see cref="Player"/> is controlling.
		/// </summary>
		public Ped Character
		{
			get
			{
				int handle = SHVDN.NativeMemory.GetPlayerPedHandle(Handle);

				if (_ped == null || handle != _ped.Handle)
				{
					_ped = new Ped(handle);
				}

				return _ped;
			}
		}

		/// <summary>
		/// Gets the Social Club name of this <see cref="Player"/>.
		/// </summary>
		public string Name => Function.Call<string>(Hash.GET_PLAYER_NAME, Handle);

		/// <summary>
		/// Gets or sets how much money this <see cref="Player"/> has.
		/// <remarks>Only works if current player is <see cref="PedHash.Michael"/>, <see cref="PedHash.Franklin"/> or <see cref="PedHash.Trevor"/></remarks>
		/// </summary>
		public int Money
		{
			get
			{
				int stat;

				switch ((PedHash)Character.Model.Hash)
				{
					case PedHash.Michael:
						stat = Game.GenerateHash("SP0_TOTAL_CASH");
						break;
					case PedHash.Franklin:
						stat = Game.GenerateHash("SP1_TOTAL_CASH");
						break;
					case PedHash.Trevor:
						stat = Game.GenerateHash("SP2_TOTAL_CASH");
						break;
					default:
						return 0;
				}

				int result;
				unsafe
				{
					Function.Call(Hash.STAT_GET_INT, stat, &result, -1);
				}

				return result;
			}
			set
			{
				int stat;

				switch ((PedHash)Character.Model.Hash)
				{
					case PedHash.Michael:
						stat = Game.GenerateHash("SP0_TOTAL_CASH");
						break;
					case PedHash.Franklin:
						stat = Game.GenerateHash("SP1_TOTAL_CASH");
						break;
					case PedHash.Trevor:
						stat = Game.GenerateHash("SP2_TOTAL_CASH");
						break;
					default:
						return;
				}

				Function.Call(Hash.STAT_SET_INT, stat, value, 1);
			}
		}

		#region Targeting

		/// <summary>
		/// Determines whether this <see cref="Player"/> is targeting the specified <see cref="Entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to check.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Player"/> is targeting the specified <see cref="Entity"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsTargeting(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Handle, entity.Handle);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is targeting anything.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is targeting anything; otherwise, <see langword="false" />.
		/// </value>
		public bool IsTargetingAnything => Function.Call<bool>(Hash.IS_PLAYER_TARGETTING_ANYTHING, Handle);

		/// <summary>
		/// Gets the <see cref="Entity"/> this <see cref="Player"/> is free aiming.
		/// </summary>
		/// <returns>The <see cref="Entity"/> if this <see cref="Player"/> is free aiming any <see cref="Entity"/>; otherwise, <see langword="null" /></returns>
		public Entity TargetedEntity
		{
			get
			{
				int entityHandle;
				unsafe
				{
					if (Function.Call<bool>(Hash.GET_ENTITY_PLAYER_IS_FREE_AIMING_AT, Handle, &entityHandle))
					{
						return Entity.FromHandle(entityHandle);
					}
				}
				return null;
			}
		}
		/// <summary>
		/// Gets the <see cref="Building"/> this <see cref="Player"/> is free aiming.
		/// You can use this property to detect if the raycast collided with something (although you cannot detect if the raycast collided with <see cref="AnimatedBuilding"/>).
		/// </summary>
		/// <remarks>
		/// <para>This functions the same as <see cref="TargetedEntity"/> except this property returns a <see cref="Building"/> instead of an <see cref="Entity"/>.</para>
		/// <para>You should not expect the returned <see cref="Building"/> instance has meaningful position info as its matrix is set to <see cref="Matrix.Identity"/> most of the time.</para>
		/// </remarks>
		/// <returns>The <see cref="Building"/> if this <see cref="Player"/> is free aiming any <see cref="Building"/>; otherwise, <see langword="null"/></returns>
		public Building TargetedBuilding
		{
			get
			{
				int buildingHandle = SHVDN.NativeMemory.GetTargetedBuildingHandleOfPlayer(Handle);
				return buildingHandle != 0 ? new Building(buildingHandle) : null;
			}
		}

		/// <summary>
		/// Gets the position where this <see cref="Player"/> is free aiming at.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Bullets and projectiles this <see cref="Player"/> shoot will go through this point.
		/// For projectiles, this applies only if the gravity is set to the default value.
		/// </para>
		/// <para>
		/// The value will be the position the current asynchronous shape test yields
		/// if this <see cref="Player"/> is pressing <see cref="Control.Aim"/> this frame.
		/// The range how far the shape test will be tested depends on the <c>WeaponRange</c> value in the <c>weapons.meta</c> file.
		/// You should not use what this property returns as the exact crosshair target position,
		/// because the shape test for crosshair target processes asynchronously.
		/// </para>
		/// If <see cref="Player"/> is not pressing <see cref="Control.Aim"/> this frame, The value will be the position of
		/// the last asynchronous shape test result when this <see cref="Player"/> was pressing <see cref="Control.Aim"/>.
		/// </remarks>
		/// <returns>The position where this <see cref="Player"/> is free aiming at.</returns>
		public Vector3 FreeAimingPosition
		{
			get
			{
				IntPtr cPlayerPedTargetingAddr = SHVDN.NativeMemory.GetCPlayerPedTargetingAddress(Handle);
				if (cPlayerPedTargetingAddr == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(cPlayerPedTargetingAddr + 0x60));
			}
		}

		/// <summary>
		/// Gets the <see cref="Entity"/> this <see cref="Player"/> is locking on when they are aiming with a firearm using a controller or they are locking on unarmed or with a melee weapon.
		/// </summary>
		/// <returns>The <see cref="Entity"/> if this <see cref="Player"/> is automatically locking on any <see cref="Entity"/>; otherwise, <see langword="null" /></returns>
		public Entity LockedOnEntity
		{
			get
			{
				int entityHandle;
				unsafe
				{
					if (Function.Call<bool>(Hash.GET_PLAYER_TARGET_ENTITY, Handle, &entityHandle))
					{
						return Entity.FromHandle(entityHandle);
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Gets the position where this <see cref="Player"/> is automatically targeting at.
		/// The position should be where the <see cref="Bone.SkelSpine3"/> bone is if the target is <see cref="Ped"/>.
		/// </summary>
		/// <remarks>
		/// The value will be <see cref="Vector3.Zero"/> when <see cref="LockedOnEntity"/> returns <see langword="null"/>.
		/// </remarks>
		/// <returns>The position where this <see cref="Player"/> is automatically targeting at.</returns>
		public Vector3 AutoTargetPosition
		{
			get
			{
				IntPtr cPlayerPedTargetingAddr = SHVDN.NativeMemory.GetCPlayerPedTargetingAddress(Handle);
				if (cPlayerPedTargetingAddr == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(cPlayerPedTargetingAddr + 0x40));
			}
		}

		#endregion

		#region Wanted Info

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
			get => Function.Call<int>(Hash.GET_PLAYER_WANTED_LEVEL, Handle);
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
			get => Function.Call<Vector3>(Hash.GET_PLAYER_WANTED_CENTRE_POSITION, Handle);
			set => Function.Call(Hash.SET_PLAYER_WANTED_CENTRE_POSITION, Handle, value.X, value.Y, value.Z);
		}

		/// <summary>
		/// Gets or sets the current crime value that determines the real wanted level when the game updates the real wanted level.
		/// </summary>
		/// <remarks>
		/// For instance, if this value is 32 and a vehicle theft crime you started gets reported (increases by 18) without crime directly getting spotted by the police,
		/// this value will be 50 and the wanted level will be one when the game updates the real wanted level using this value.
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

				IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
				if (cWantedAddress == IntPtr.Zero)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt32(cWantedAddress + SHVDN.NativeMemory.CurrentCrimeValueOffset);
			}
			set
			{
				if (SHVDN.NativeMemory.CurrentCrimeValueOffset == 0)
				{
					return;
				}

				IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
				if (cWantedAddress == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteInt32(cWantedAddress + SHVDN.NativeMemory.CurrentCrimeValueOffset, value);
			}
		}
		/// <summary>
		/// Gets or sets the pending crime value that will be applied when the game ticks
		/// if <see cref="TimeToApplyPendingCrimeValue"/> is not zero and less than <see cref="Game.GameTime"/>.
		/// </summary>
		/// <remarks>
		/// The game sets this value only when this <see cref="Player"/> commit a crime that will immediately increase their wanted level such as targeting a police officer,
		/// when <c>SET_PLAYER_WANTED_LEVEL</c> is called and the wanted level is to increase, or when the game applies this value to <see cref="CurrentCrimeValue"/>.
		/// </remarks>
		/// <value>
		/// The pending crime value.
		/// </value>
		public int PendingCrimeValue
		{
			get
			{
				if (SHVDN.NativeMemory.PendingCrimeValueOffset == 0)
				{
					return 0;
				}

				IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
				if (cWantedAddress == IntPtr.Zero)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt32(cWantedAddress + SHVDN.NativeMemory.PendingCrimeValueOffset);
			}
			set
			{
				if (SHVDN.NativeMemory.PendingCrimeValueOffset == 0)
				{
					return;
				}

				IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
				if (cWantedAddress == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteInt32(cWantedAddress + SHVDN.NativeMemory.PendingCrimeValueOffset, value);
			}
		}
		/// <summary>
		/// Gets or sets the game time when <see cref="PendingCrimeValue"/> will be set to <see cref="CurrentCrimeValue"/>.
		/// If zero, the game will not apply <see cref="PendingCrimeValue"/>.
		/// </summary>
		/// <remarks>
		/// The game sets this value only when <c>SET_PLAYER_WANTED_LEVEL</c> is called and the wanted level is to increase
		/// or when the game applies <see cref="PendingCrimeValue"/> to <see cref="CurrentCrimeValue"/> and set this value to zero.
		/// </remarks>
		/// <value>
		/// The game time when <see cref="PendingCrimeValue"/> will be set to <see cref="CurrentCrimeValue"/>.
		/// </value>
		public int TimeToApplyPendingCrimeValue
		{
			get
			{
				if (SHVDN.NativeMemory.TimeToApplyPendingCrimeValueOffset == 0)
				{
					return 0;
				}

				IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
				if (cWantedAddress == IntPtr.Zero)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt32(cWantedAddress + SHVDN.NativeMemory.TimeToApplyPendingCrimeValueOffset);
			}
			set
			{
				if (SHVDN.NativeMemory.TimeToApplyPendingCrimeValueOffset == 0)
				{
					return;
				}

				IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
				if (cWantedAddress == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteInt32(cWantedAddress + SHVDN.NativeMemory.TimeToApplyPendingCrimeValueOffset, value);
			}
		}

		/// <summary>
		/// Gets or sets the last time when the search area got refocused for this <see cref="Player"/>.
		/// When you commit a crime that refocus the search area, this value will update.
		/// </summary>
		/// <remarks>
		/// The game will set this value to zero when the wanted level is zero.
		/// </remarks>
		public int LastTimeSearchAreaRefocused
		{
			get
			{
				if (SHVDN.NativeMemory.CWantedLastTimeSearchAreaRefocusedOffset == 0)
				{
					return 0;
				}

				IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
				if (cWantedAddress == IntPtr.Zero)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt32(cWantedAddress + SHVDN.NativeMemory.CWantedLastTimeSearchAreaRefocusedOffset);
			}
			set
			{
				if (SHVDN.NativeMemory.CWantedLastTimeSearchAreaRefocusedOffset == 0)
				{
					return;
				}

				IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
				if (cWantedAddress == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteInt32(cWantedAddress + SHVDN.NativeMemory.CWantedLastTimeSearchAreaRefocusedOffset, value);
			}
		}

		/// <summary>
		/// Gets or sets the last game time when this <see cref="Player"/> is spotted by the police.
		/// The game will set this value to zero when the wanted level is zero.
		/// </summary>
		/// <remarks>
		/// The game will set to the game time as long as this <see cref="Player"/> is spotted by the police each frame,
		/// but you can make the <see cref="Player"/> getting in the hidden evasion phase up to 1 or 2 seconds if the police does not know where the <see cref="Player"/> is.
		/// </remarks>
		public int LastTimeSpottedByPolice
		{
			get
			{
				if (SHVDN.NativeMemory.CWantedLastTimeSpottedByPoliceOffset == 0)
				{
					return 0;
				}

				IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
				if (cWantedAddress == IntPtr.Zero)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt32(cWantedAddress + SHVDN.NativeMemory.CWantedLastTimeSpottedByPoliceOffset);
			}
			set
			{
				if (SHVDN.NativeMemory.CWantedLastTimeSpottedByPoliceOffset == 0)
				{
					return;
				}

				IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
				if (cWantedAddress == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteInt32(cWantedAddress + SHVDN.NativeMemory.CWantedLastTimeSpottedByPoliceOffset, value);
			}
		}

		/// <summary>
		/// Gets or sets the game time when hidden evasion phase gets started.
		/// </summary>
		/// <remarks>
		/// The game will set to zero when this <see cref="Player"/> is spotted by the police each frame,
		/// but you can set small value (but not zero) to clear the wanted level when the <see cref="Player"/> is in the hidden evasion phase
		/// if not supressed by <c>SUPPRESS_LOSING_WANTED_LEVEL_IF_HIDDEN_THIS_FRAME</c>.
		/// </remarks>
		public int StartTimeOfHiddenEvasion
		{
			get
			{
				if (SHVDN.NativeMemory.CWantedStartTimeOfHiddenEvasionOffset == 0)
				{
					return 0;
				}

				IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
				if (cWantedAddress == IntPtr.Zero)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt32(cWantedAddress + SHVDN.NativeMemory.CWantedStartTimeOfHiddenEvasionOffset);
			}
			set
			{
				if (SHVDN.NativeMemory.CWantedStartTimeOfHiddenEvasionOffset == 0)
				{
					return;
				}

				IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
				if (cWantedAddress == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteInt32(cWantedAddress + SHVDN.NativeMemory.CWantedStartTimeOfHiddenEvasionOffset, value);
			}
		}
		/// <summary>
		/// Returns <see langword="true"/> if this <see cref="Player"/> has a wanted level and the stars are displayed gray.
		/// </summary>
		/// <remarks>
		/// Technically, this property returns <see langword="true"/> when the flag for stars graying out is set
		/// and <see cref="LastTimeSpottedByPolice"/> has more value by more than <c>1000</c> or <c>2000</c> (depending on a unknown state)
		/// than <see cref="Game.GameTime"/>.
		/// </remarks>
		public bool AreWantedStarsGrayedOut => Function.Call<bool>(Hash.ARE_PLAYER_STARS_GREYED_OUT, Handle);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Player"/> is ignored by the police.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is ignored by the police; otherwise, <see langword="false" />.
		/// </value>
		public bool IgnoredByPolice
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

				return SHVDN.NativeMemory.IsBitSet(cWantedAddress + SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset, 16);
			}
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

				return SHVDN.NativeMemory.IsBitSet(cWantedAddress + SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset, 18);
			}
			set => Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether cops will be dispatched for this <see cref="Player"/>.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if cops will be dispatched; otherwise, <see langword="false" />.
		/// </value>
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

				return !SHVDN.NativeMemory.IsBitSet(cWantedAddress + SHVDN.NativeMemory.CWantedIgnorePlayerFlagOffset, 23);
			}
			set => Function.Call(Hash.SET_DISPATCH_COPS_FOR_PLAYER, Handle, value);
		}

		/// <summary>
		/// Sets the pending wanted level for the player.
		/// The wanted level changes takes 10 seconds before it gets applied (emulating the time it takes a citizen to
		/// report the crime) if the passed wanted level is higher than the current.
		/// Otherwise, the change will get applied immediately, including <see cref="CurrentCrimeValue"/> and
		/// <see cref="WantedCenterPosition"/>.
		/// </summary>
		public void SetPendingWantedLevel(int wantedLevel, bool delayLawResponse = false)
			=> Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Handle, wantedLevel, delayLawResponse);

		/// <summary>
		/// Sets the pending wanted level for the player only if its higher than the current.
		/// The wanted level changes takes 10 seconds before it gets applied.
		/// </summary>
		public void SetPendingWantedLevelNoDrop(int wantedLevel, bool delayLawResponse = false)
			=> Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NO_DROP, Handle, wantedLevel, delayLawResponse);

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

			IntPtr cWantedAddress = SHVDN.NativeMemory.GetCWantedAddress(Handle);
			if (cWantedAddress == IntPtr.Zero)
			{
				return;
			}

			int currentWantedLevel = SHVDN.NativeMemory.ReadInt32(cWantedAddress + SHVDN.NativeMemory.CurrentWantedLevelOffset);

			if (wantedLevel <= 0 || wantedLevel >= currentWantedLevel)
			{
				// Just call SET_PLAYER_WANTED_LEVEL and SET_PLAYER_WANTED_LEVEL_NOW, because setting to the current wanted level or above won't refocus the search area (the crime value will be reset)
				Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Handle, wantedLevel, false);
				Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Handle, false);
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
			int threshold = Function.Call<int>(Hash.GET_WANTED_LEVEL_THRESHOLD, Handle, wantedLevelToApply);

			CurrentCrimeValue = threshold + ADDITIONAL_CRIME_VALUE;
			SHVDN.NativeMemory.WriteInt32(cWantedAddress + SHVDN.NativeMemory.CurrentWantedLevelOffset, wantedLevelToApply);

			// Set the pending crime value just like SET_PLAYER_WANTED_LEVEL does (SET_PLAYER_WANTED_LEVEL_NOW does not clear the value)
			PendingCrimeValue = threshold + ADDITIONAL_CRIME_VALUE;
		}

		/// <summary>
		/// Reports a crime for this <see cref="Player"/>.
		/// </summary>
		/// <param name="crimeToReport">The crime time to report.</param>
		/// <param name="crimeValue">
		/// If left at zero, the crime will get evaluated.
		/// It not zero, the crime value will be overridden to specify an amount (can be both positive or negative).
		/// </param>
		/// <remarks>
		/// Clearing the wanted level will disable to increase the crime value for commiting crimes for 2 seconds.
		/// </remarks>
		public void ReportCrime(CrimeType crimeToReport, int crimeValue = 0) => Function.Call(Hash.REPORT_CRIME, Handle, (int)crimeToReport, crimeValue);

		/// <summary>
		/// Forces this <see cref="Player"/> to get spotted by police.
		/// </summary>
		/// <remarks>
		/// Unlike when you commit a crime that refocuses the search area, this method also updates <see cref="LastTimeSpottedByPolice"/>.
		/// </remarks>
		public void ForceGetSpottedByPolice() => Function.Call(Hash.REPORT_POLICE_SPOTTED_PLAYER, Handle);
		/// <summary>
		/// Force hidden evasion to start for this <see cref="Player"/>, making wanted stars flashing and cops using vision cones to search for the player.
		/// You can use this method at any point that police know where this player is.
		/// </summary>
		public void ForceStartHiddenEvasion() => Function.Call(Hash.FORCE_START_HIDDEN_EVASION, Handle);

		#endregion

		#region Special Ability

		/// <summary>
		/// Gets or sets the remaining value of the special ability meter for this <see cref="Player"/>.
		/// </summary>
		/// <remarks>
		/// Returns <c>0f</c> if this <see cref="Player"/> does not have their special ability.
		/// </remarks>
		/// <value>
		/// the remaining value of the special ability meter for this <see cref="Player"/>.
		/// </value>
		public float RemainingSpecialAbilityMeter
		{
			get
			{
				IntPtr specialAbilityStructAddr = SHVDN.NativeMemory.GetPrimarySpecialAbilityStructAddress(Handle);
				if (specialAbilityStructAddr == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(specialAbilityStructAddr + 0x34);
			}
			set
			{
				IntPtr specialAbilityStructAddr = SHVDN.NativeMemory.GetPrimarySpecialAbilityStructAddress(Handle);
				if (specialAbilityStructAddr == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(specialAbilityStructAddr + 0x34, value);
			}
		}

		/// <summary>
		/// Gets or sets the maximum value of the special ability meter for this <see cref="Player"/>.
		/// </summary>
		/// <remarks>
		/// Returns <c>0f</c> if this <see cref="Player"/> does not have their special ability.
		/// </remarks>
		/// <value>
		/// the maximum value of the special ability meter for this <see cref="Player"/>.
		/// </value>
		public float MaxSpecialAbilityMeter
		{
			get
			{
				IntPtr specialAbilityStructAddr = SHVDN.NativeMemory.GetPrimarySpecialAbilityStructAddress(Handle);
				if (specialAbilityStructAddr == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(specialAbilityStructAddr + 0x3C);
			}
			set
			{
				IntPtr specialAbilityStructAddr = SHVDN.NativeMemory.GetPrimarySpecialAbilityStructAddress(Handle);
				if (specialAbilityStructAddr == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(specialAbilityStructAddr + 0x3C, value);
			}
		}

		/// <summary>
		/// Gets a value that indicates whether this <see cref="Player"/> has their special ability.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if this <see cref="Player"/> has their special ability; otherwise,
		/// <see langword="false"/>.
		/// </value>
		public bool HasSpecialAbility
			=> SHVDN.NativeMemory.GetPrimarySpecialAbilityStructAddress(Handle) != IntPtr.Zero;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is using their special ability.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if this <see cref="Player"/> is using their special ability; otherwise,
		/// <see langword="false"/>.
		/// </value>
		public bool IsSpecialAbilityActive => Function.Call<bool>(Hash.IS_SPECIAL_ABILITY_ACTIVE, Handle);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Player"/> can use their special ability.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if this  <see cref="Player"/> can use their special ability; otherwise,
		/// <see langword="false"/>.
		/// </value>
		public bool IsSpecialAbilityEnabled
		{
			get => Function.Call<bool>(Hash.IS_SPECIAL_ABILITY_ENABLED, Handle);
			set => Function.Call(Hash.ENABLE_SPECIAL_ABILITY, Handle, value);
		}

		/// <inheritdoc cref="ChargeSpecialAbility(int, bool)"/>
		public void ChargeSpecialAbility(int absoluteAmount) => ChargeSpecialAbility(absoluteAmount, true);
		/// <summary>
		/// Charges the special ability for this <see cref="Player"/>.
		/// </summary>
		/// <param name="absoluteAmount">The absolute amount.</param>
		/// <param name="ignoreActive">
		/// If <see langword="true"/>, thid method will do nothing if the special ability is active.
		/// </param>
		public void ChargeSpecialAbility(int absoluteAmount, bool ignoreActive)
		{
			Function.Call(Hash.SPECIAL_ABILITY_CHARGE_ABSOLUTE, Handle, absoluteAmount, ignoreActive);
		}

		/// <inheritdoc cref="ChargeSpecialAbility(float, bool)"/>
		public void ChargeSpecialAbility(float normalizedRatio) => ChargeSpecialAbility(normalizedRatio, true);
		/// <summary>
		/// Charges the special ability for this <see cref="Player"/>.
		/// </summary>
		/// <param name="normalizedRatio">The amount between <c>0.0f</c> and <c>1.0f</c></param>
		/// <param name="ignoreActive">
		/// If <see langword="false"/>, thid method will do nothing if the special ability is active.
		/// </param>
		public void ChargeSpecialAbility(float normalizedRatio, bool ignoreActive)
		{
			Function.Call(Hash.SPECIAL_ABILITY_CHARGE_NORMALIZED, Handle, normalizedRatio, ignoreActive);
		}

		/// <inheritdoc cref="RefillSpecialAbility(bool)"/>
		public void RefillSpecialAbility() => RefillSpecialAbility(true);
		/// <summary>
		/// Refills the special ability for this <see cref="Player"/>.
		/// </summary>
		/// <param name="ignoreActive">
		/// If <see langword="false"/>, thid method will do nothing if the special ability is active.
		/// </param>
		public void RefillSpecialAbility(bool ignoreActive)
		{
			Function.Call(Hash.SPECIAL_ABILITY_FILL_METER, Handle, ignoreActive);
		}

		/// <inheritdoc cref="DepleteSpecialAbility(bool)"/>
		public void DepleteSpecialAbility() => DepleteSpecialAbility(true);
		/// <summary>
		/// Depletes the special ability for this <see cref="Player"/>.
		/// </summary>
		/// <param name="ignoreActive">
		/// If <see langword="false"/>, this method will to nothing if the special ability is active.
		/// </param>
		public void DepleteSpecialAbility(bool ignoreActive)
		{
			Function.Call(Hash.SPECIAL_ABILITY_DEPLETE_METER, Handle, ignoreActive);
		}

		/// <summary>
		/// Activates the special ability for this <see cref="Player"/>.
		/// </summary>
		/// <remarks>
		/// Although <c>SPECIAL_ABILITY_ACTIVATE</c> is not present in any versions prior to v1.0.678.1,
		/// you can call this method without having the SHVDN runtime stopped in any versions.
		/// </remarks>
		public void ActivateSpecialAbility()
		{
			if (Game.Version >= GameVersion.v1_0_678_1_Steam)
			{
				Function.Call(Hash.SPECIAL_ABILITY_ACTIVATE, Handle, 0);
				return;
			}

			// SPECIAL_ABILITY_ACTIVATE is not present, have to manually call what SPECIAL_ABILITY_ACTIVATE eventually
			// calls if the player has their special ability
			SHVDN.NativeMemory.ActivateSpecialAbility(Handle);
		}

		/// <summary>
		/// Deactivates the special ability for this <see cref="Player"/>.
		/// </summary>
		/// <remarks>
		/// The current time scale for the special ability, which is different from the one that can be accessed via
		/// <see cref="Game.TimeScale"/>, will gradually go to 1.0 (in the ease-out style where the value starts
		/// quickly, slowing down the animation continues).
		/// </remarks>
		public void DeactivateSpecialAbility() => Function.Call(Hash.SPECIAL_ABILITY_DEACTIVATE, Handle, 0);

		/// <summary>
		/// Deactivates the special ability for this <see cref="Player"/> instantly without applying the fadeout fx.
		/// Also immediately sets the current time scale for the special ability, which is different from the one that
		/// can be accessed via <see cref="Game.TimeScale"/>, to 1.0.
		/// </summary>
		public void DeactivateSpecialAbilityInstantly()
			=> Function.Call(Hash.SPECIAL_ABILITY_DEACTIVATE_FAST, Handle, 0);

		#endregion

		/// <summary>
		/// Gets or sets the maximum amount of armor this <see cref="Player"/> can have.
		/// The value range is between 0 and 65535.
		/// </summary>
		/// <remarks>
		/// This value is used for the setter of <see cref="Entity.MaxHealth"/> and when the game respawns player <see cref="Ped"/>s.
		/// </remarks>
		public int MaxHealth
		{
			get
			{
				if (SHVDN.NativeMemory.CPlayerInfoMaxHealthOffset == 0)
				{
					return 0;
				}

				IntPtr cPlayerInfoAddress = SHVDN.NativeMemory.GetCPlayerInfoAddress(Handle);
				if (cPlayerInfoAddress == IntPtr.Zero)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadUInt16(cPlayerInfoAddress + SHVDN.NativeMemory.CPlayerInfoMaxHealthOffset);
			}
			set
			{
				if (SHVDN.NativeMemory.CPlayerInfoMaxHealthOffset == 0)
				{
					return;
				}

				IntPtr cPlayerInfoAddress = SHVDN.NativeMemory.GetCPlayerInfoAddress(Handle);
				if (cPlayerInfoAddress == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteUInt16(cPlayerInfoAddress + SHVDN.NativeMemory.CPlayerInfoMaxHealthOffset, (ushort)value);
			}
		}

		/// <summary>
		/// Gets or sets the maximum amount of armor this <see cref="Player"/> can carry.
		/// The value range is between 0 and 65535.
		/// </summary>
		public int MaxArmor
		{
			get => Function.Call<int>(Hash.GET_PLAYER_MAX_ARMOUR, Handle);
			set => Function.Call(Hash.SET_PLAYER_MAX_ARMOUR, Handle, value);
		}

		/// <summary>
		/// Gets or sets the primary parachute tint for this <see cref="Player"/>.
		/// </summary>
		public ParachuteTint PrimaryParachuteTint
		{
			get
			{
				int result;
				unsafe
				{
					Function.Call(Hash.GET_PLAYER_PARACHUTE_TINT_INDEX, Handle, &result);
				}
				return (ParachuteTint)result;
			}
			set => Function.Call(Hash.SET_PLAYER_PARACHUTE_TINT_INDEX, Handle, (int)value);
		}
		/// <summary>
		/// Gets or sets the reserve parachute tint for this <see cref="Player"/>.
		/// </summary>
		public ParachuteTint ReserveParachuteTint
		{
			get
			{
				int result;
				unsafe
				{
					Function.Call(Hash.GET_PLAYER_RESERVE_PARACHUTE_TINT_INDEX, Handle, &result);
				}
				return (ParachuteTint)result;
			}
			set => Function.Call(Hash.SET_PLAYER_RESERVE_PARACHUTE_TINT_INDEX, Handle, (int)value);
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Player"/> can leave a parachute smoke trail.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> can leave a parachute smoke trail; otherwise, <see langword="false" />.
		/// </value>
		public bool CanLeaveParachuteSmokeTrail
		{
			set => Function.Call(Hash.SET_PLAYER_CAN_LEAVE_PARACHUTE_SMOKE_TRAIL, Handle, value);
		}

		/// <summary>
		/// Gets or sets the color of the parachute smoke trail for this <see cref="Player"/>.
		/// </summary>
		/// <value>
		/// The color of the parachute smoke trail for this <see cref="Player"/>.
		/// </value>
		public Color ParachuteSmokeTrailColor
		{
			get
			{
				int r, g, b;
				unsafe
				{
					Function.Call(Hash.GET_PLAYER_PARACHUTE_SMOKE_TRAIL_COLOR, Handle, &r, &g, &b);
				}
				return Color.FromArgb(r, g, b);
			}
			set => Function.Call(Hash.SET_PLAYER_PARACHUTE_SMOKE_TRAIL_COLOR, Handle, value.R, value.G, value.B);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is dead.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Player"/> is dead; otherwise, <see langword="false" />.
		/// </value>
		public bool IsDead => Function.Call<bool>(Hash.IS_PLAYER_DEAD, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is alive.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Player"/> is alive; otherwise, <see langword="false" />.
		/// </value>
		public bool IsAlive => !IsDead;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is aiming.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Player"/> is aiming; otherwise, <see langword="false" />.
		/// </value>
		public bool IsAiming => Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is climbing.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is climbing; otherwise, <see langword="false" />.
		/// </value>
		public bool IsClimbing => Function.Call<bool>(Hash.IS_PLAYER_CLIMBING, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is riding a train.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is riding a train; otherwise, <see langword="false" />.
		/// </value>
		public bool IsRidingTrain => Function.Call<bool>(Hash.IS_PLAYER_RIDING_TRAIN, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is pressing a horn.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is pressing a horn; otherwise, <see langword="false" />.
		/// </value>
		public bool IsPressingHorn => Function.Call<bool>(Hash.IS_PLAYER_PRESSING_HORN, Handle);

		/// <summary>
		/// Returns <see langword="false"/> if the screen is fading due to this <see cref="Player"/> being killed or arrested or failing a critical mission.
		/// </summary>
		/// <value>
		///  <see langword="false"/> if the screen is fading due to this <see cref="Player"/> being killed or arrested or failing a critical mission.; otherwise, <see langword="true"/>.
		/// </value>
		public bool IsPlaying => Function.Call<bool>(Hash.IS_PLAYER_PLAYING, Handle);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Player"/> is invincible.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> is invincible; otherwise, <see langword="false" />.
		/// </value>
		public bool IsInvincible
		{
			get => Function.Call<bool>(Hash.GET_PLAYER_INVINCIBLE, Handle);
			set => Function.Call(Hash.SET_PLAYER_INVINCIBLE, Handle, value);
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Player"/> can use cover.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> can use cover; otherwise, <see langword="false" />.
		/// </value>
		public bool CanUseCover
		{
			set => Function.Call(Hash.SET_PLAYER_CAN_USE_COVER, Handle, value);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> can start a mission.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> can start a mission; otherwise, <see langword="false" />.
		/// </value>
		public bool CanStartMission => Function.Call<bool>(Hash.CAN_PLAYER_START_MISSION, Handle);

		/// <summary>
		/// Sets a value indicating whether this <see cref="Player"/> can control ragdoll.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> can control ragdoll; otherwise, <see langword="false" />.
		/// </value>
		public bool CanControlRagdoll
		{
			set => Function.Call(Hash.GIVE_PLAYER_RAGDOLL_CONTROL, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Player"/> can control its <see cref="Ped"/>.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Player"/> can control its <see cref="Ped"/>; otherwise, <see langword="false" />.
		/// </value>
		public bool CanControlCharacter
		{
			get => Function.Call<bool>(Hash.IS_PLAYER_CONTROL_ON, Handle);
			[Obsolete("The setter of Player.CanControlCharacter is obsolete because it fails to switch the controls back on if the flag for ambient script " +
				"is set when the player controls got disabled. Use Player.SetControlState instead."),
			 EditorBrowsable(EditorBrowsableState.Never)]
			set => SetControlState(value);
		}

		/// <summary>
		/// Sets this <see cref="Player"/>'s control state.
		/// </summary>
		/// <param name="setControlOn">Specifies whether the player control should be on.</param>
		/// <param name="flags">
		/// The flags for additional flag modifications or additional function calls such as removing projectiles.
		/// All the flags except for <see cref="SetPlayerControlFlags.AmbientScript"/> work only when <paramref name="setControlOn"/> is set to <see langword="false"/>.
		/// </param>
		public void SetControlState(bool setControlOn, SetPlayerControlFlags flags = SetPlayerControlFlags.None)
		{
			Function.Call(Hash.SET_PLAYER_CONTROL, Handle, setControlOn, (int)flags);
		}

		/// <summary>
		/// Attempts to change the <see cref="Model"/> of this <see cref="Player"/>.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> to change this <see cref="Player"/> to.</param>
		/// <returns><see langword="true" /> if the change was successful; otherwise, <see langword="false" />.</returns>
		public bool ChangeModel(Model model)
		{
			if (!model.IsInCdImage || !model.IsPed || !model.Request(1000))
			{
				return false;
			}

			Function.Call(Hash.SET_PLAYER_MODEL, Handle, model.Hash);
			model.MarkAsNoLongerNeeded();
			return true;
		}

		/// <summary>
		/// Gets how long this <see cref="Player"/> can remain sprinting for.
		/// </summary>
		public float RemainingSprintTime => Function.Call<float>(Hash.GET_PLAYER_SPRINT_TIME_REMAINING, Handle);

		/// <summary>
		/// Gets how much sprint stamina this <see cref="Player"/> currently has.
		/// </summary>
		public float RemainingSprintStamina => Function.Call<float>(Hash.GET_PLAYER_SPRINT_STAMINA_REMAINING, Handle);

		/// <summary>
		/// Gets how long this <see cref="Player"/> can stay underwater before they start losing health.
		/// </summary>
		public float RemainingUnderwaterTime => Function.Call<float>(Hash.GET_PLAYER_UNDERWATER_TIME_REMAINING, Handle);

		/// <summary>
		/// Gets the last <see cref="Vehicle"/> this <see cref="Player"/> used.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if the last vehicle doesn't exist.</remarks>
		public Vehicle LastVehicle => Function.Call<Vehicle>(Hash.GET_PLAYERS_LAST_VEHICLE);

		/// <summary>
		/// Sets a value indicating whether the player is forced to aim.
		/// </summary>
		/// <value>
		///   <see langword="true" /> to make the player always be aiming; otherwise, <see langword="false" />.
		/// </value>
		public bool ForcedAim
		{
			set => Function.Call(Hash.SET_PLAYER_FORCED_AIM, Handle, value);
		}

		/// <summary>
		/// Prevents this <see cref="Player"/> firing this frame.
		/// </summary>
		public void DisableFiringThisFrame()
		{
			Function.Call(Hash.DISABLE_PLAYER_FIRING, Handle, 0);
		}

		/// <summary>
		/// Sets the run speed multiplier for this <see cref="Player"/> this frame.
		/// </summary>
		/// <param name="mult">The factor - min: <c>0.0f</c>, default: <c>1.0f</c>, max: <c>1.499f</c>.</param>
		public void SetRunSpeedMultThisFrame(float mult)
		{
			if (mult > 1.499f)
			{
				mult = 1.499f;
			}

			Function.Call(Hash.SET_RUN_SPRINT_MULTIPLIER_FOR_PLAYER, Handle, mult);
		}

		/// <summary>
		/// Sets the swim speed multiplier for this <see cref="Player"/> this frame.
		/// </summary>
		/// <param name="mult">The factor - min: <c>0.0f</c>, default: <c>1.0f</c>, max: <c>1.499f</c>.</param>
		public void SetSwimSpeedMultThisFrame(float mult)
		{
			if (mult > 1.499f)
			{
				mult = 1.499f;
			}

			Function.Call(Hash.SET_SWIM_MULTIPLIER_FOR_PLAYER, Handle, mult);
		}

		/// <summary>
		/// Makes this <see cref="Player"/> shoot fire bullets this frame.
		/// </summary>
		public void SetFireAmmoThisFrame()
		{
			Function.Call(Hash.SET_FIRE_AMMO_THIS_FRAME, Handle);
		}

		/// <summary>
		/// Makes this <see cref="Player"/> shoot explosive bullets this frame.
		/// </summary>
		public void SetExplosiveAmmoThisFrame()
		{
			Function.Call(Hash.SET_EXPLOSIVE_AMMO_THIS_FRAME, Handle);
		}

		/// <summary>
		/// Makes this <see cref="Player"/> have an explosive melee attack this frame.
		/// </summary>
		public void SetExplosiveMeleeThisFrame()
		{
			Function.Call(Hash.SET_EXPLOSIVE_MELEE_THIS_FRAME, Handle);
		}

		/// <summary>
		/// Lets this <see cref="Player"/> jump really high this frame.
		/// </summary>
		public void SetSuperJumpThisFrame()
		{
			Function.Call(Hash.SET_SUPER_JUMP_THIS_FRAME, Handle);
		}

		/// <summary>
		/// Blocks this <see cref="Player"/> from entering any <see cref="Vehicle"/> this frame.
		/// </summary>
		public void SetMayNotEnterAnyVehicleThisFrame()
		{
			Function.Call(Hash.SET_PLAYER_MAY_NOT_ENTER_ANY_VEHICLE, Handle);
		}

		/// <summary>
		/// Only lets this <see cref="Player"/> enter a specific <see cref="Vehicle"/> this frame.
		/// </summary>
		/// <param name="vehicle">The <see cref="Vehicle"/> this <see cref="Player"/> is allowed to enter.</param>
		public void SetMayOnlyEnterThisVehicleThisFrame(Vehicle vehicle)
		{
			Function.Call(Hash.SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE, Handle, vehicle.Handle);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same player as this <see cref="Player"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same player as this <see cref="Player"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Player player)
			{
				return Handle == player.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="Player"/>s refer to the same player.
		/// </summary>
		/// <param name="left">The left <see cref="Player"/>.</param>
		/// <param name="right">The right <see cref="Player"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same player as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Player left, Player right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="Player"/>s don't refer to the same player.
		/// </summary>
		/// <param name="left">The left <see cref="Player"/>.</param>
		/// <param name="right">The right <see cref="Player"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same player as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(Player left, Player right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts a <see cref="Player"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(Player value)
		{
			return new InputArgument((ulong)value.Handle);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
