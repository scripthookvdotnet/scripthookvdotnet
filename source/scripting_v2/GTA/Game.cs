//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GTA
{
	public static class Game
	{
		#region Fields

		private static Player s_cachedPlayer;

		internal static readonly string[] s_radioNames = {
			"RADIO_01_CLASS_ROCK",
			"RADIO_02_POP",
			"RADIO_03_HIPHOP_NEW",
			"RADIO_04_PUNK",
			"RADIO_05_TALK_01",
			"RADIO_06_COUNTRY",
			"RADIO_07_DANCE_01",
			"RADIO_08_MEXICAN",
			"RADIO_09_HIPHOP_OLD",
			"RADIO_11_TALK_02",
			"RADIO_12_REGGAE",
			"RADIO_13_JAZZ",
			"RADIO_14_DANCE_02",
			"RADIO_15_MOTOWN",
			"RADIO_16_SILVERLAKE",
			"RADIO_17_FUNK",
			"RADIO_18_90S_ROCK",
			"RADIO_19_USER",
			"RADIO_20_THELAB",
			"RADIO_21_DLC_XM17",
			"RADIO_22_DLC_BATTLE_MIX1_RADIO",
			"RADIO_23_DLC_XM19_RADIO",
			"RADIO_27_DLC_PRHEI4",
			"RADIO_34_DLC_HEI4_KULT",
			"RADIO_35_DLC_HEI4_MLR",
			"RADIO_36_AUDIOPLAYER",
			"RADIO_37_MOTOMAMI",
			"RADIO_OFF"
		};
		#endregion

		/// <summary>
		/// Gets the current frame rate in frames per second.
		/// </summary>
		public static float FPS => 1.0f / LastFrameTime;
		/// <summary>
		/// Gets the time in seconds it took for the last frame to render.
		/// </summary>
		public static float LastFrameTime => Function.Call<float>(Hash.GET_FRAME_TIME);

		/// <summary>
		/// Gets the actual screen resolution the game is being rendered at.
		/// </summary>
		public static Size ScreenResolution
		{
			get
			{
				int w, h;
				unsafe { Function.Call(Hash._GET_SCREEN_ACTIVE_RESOLUTION, &w, &h); }
				return new Size(w, h);
			}
		}

		/// <summary>
		/// Gets the local <see cref="GTA.Player"/> that you are controlling.
		/// </summary>
		public static Player Player
		{
			get
			{
				int handle = SHVDN.NativeMemory.GetLocalPlayerIndex();

				if (s_cachedPlayer == null || handle != s_cachedPlayer.Handle)
				{
					s_cachedPlayer = new Player(handle);
				}

				return s_cachedPlayer;
			}
		}

		/// <summary>
		/// Gets the current game language.
		/// </summary>
		public static Language Language => Function.Call<Language>(Hash._GET_UI_LANGUAGE_ID);

		/// <summary>
		/// Gets the version of the game.
		/// </summary>
		public static GameVersion Version => (GameVersion)(SHVDN.NativeMemory.GetGameVersion() + 1);

		public static GlobalCollection Globals { get; private set; } = new();

		/// <summary>
		/// Pause/resume the game.
		/// </summary>
		/// <param name="value">True/false for pause/resume.</param>
		public static void Pause(bool value)
		{
			Function.Call(Hash.SET_GAME_PAUSED, value);
		}
		/// <summary>
		/// Pauses or resumes the in-game clock.
		/// </summary>
		/// <param name="value">Pauses the game clock if set to <see langword="true" />; otherwise, resumes the game clock.</param>
		public static void PauseClock(bool value)
		{
			Function.Call(Hash.PAUSE_CLOCK, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the pause menu is active.
		/// </summary>
		public static bool IsPaused
		{
			get => Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE);
			set => Function.Call(Hash.SET_PAUSE_MENU_ACTIVE, value);
		}

		/// <summary>
		/// Performs an automatic game save if allowed by the game settings.
		/// </summary>
		public static void DoAutoSave()
		{
			Function.Call(Hash.DO_AUTO_SAVE);
		}
		/// <summary>
		/// Shows the save menu enabling the user to perform a manual game save.
		/// </summary>
		public static void ShowSaveMenu()
		{
			Function.Call(Hash.SET_SAVE_MENU_ACTIVE, true);
		}

		/// <summary>
		/// Gets a value indicating whether there is a loading screen being displayed.
		/// </summary>
		public static bool IsLoading => Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE);
		/// <summary>
		/// Gets a value indicating whether the screen is faded in.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the screen is faded in; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsScreenFadedIn => Function.Call<bool>(Hash.IS_SCREEN_FADED_IN);
		/// <summary>
		/// Gets a value indicating whether the screen is faded out.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the screen is faded out; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsScreenFadedOut => Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT);
		/// <summary>
		/// Gets a value indicating whether the screen is fading in.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the screen is fading in; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsScreenFadingIn => Function.Call<bool>(Hash.IS_SCREEN_FADING_IN);
		/// <summary>
		/// Gets a value indicating whether the screen is fading out.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the screen is fading out; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsScreenFadingOut => Function.Call<bool>(Hash.IS_SCREEN_FADING_OUT);

		/// <summary>
		/// Fades the screen in over a specific time, useful for transitioning.
		/// </summary>
		/// <param name="time">The time for the fade in to take.</param>
		public static void FadeScreenIn(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_IN, time);
		}
		/// <summary>
		/// Fades the screen out over a specific time, useful for transitioning.
		/// </summary>
		/// <param name="time">The time for the fade out to take.</param>
		public static void FadeScreenOut(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_OUT, time);
		}

		/// <summary>
		/// Gets a value indicating whether there is a waypoint set on the map.
		/// </summary>
		public static bool IsWaypointActive => Function.Call<bool>(Hash.IS_WAYPOINT_ACTIVE);

		/// <summary>
		/// Gets whether the last input was made with a GamePad or keyboard and mouse.
		/// </summary>
		public static InputMode CurrentInputMode => Function.Call<bool>(Hash._GET_LAST_INPUT_METHOD, 2) ? InputMode.MouseAndKeyboard : InputMode.GamePad;

		/// <summary>
		/// Gets or sets the current radio station.
		/// </summary>
		public static RadioStation RadioStation
		{
			get
			{
				string radioName = Function.Call<string>(Hash.GET_PLAYER_RADIO_STATION_NAME);

				if (String.IsNullOrEmpty(radioName))
				{
					return RadioStation.RadioOff;
				}

				return (RadioStation)Array.IndexOf(s_radioNames, radioName);
			}
			set
			{
				if (Enum.IsDefined(typeof(RadioStation), value) && value != RadioStation.RadioOff)
				{
					Function.Call(Hash.SET_RADIO_TO_STATION_NAME, s_radioNames[(int)value]);
				}
				else
				{
					Function.Call(Hash.SET_RADIO_TO_STATION_NAME, "OFF");
				}
			}
		}

		/// <summary>
		/// Gets how many milliseconds the game has been open in this session
		/// </summary>
		public static int GameTime => Function.Call<int>(Hash.GET_GAME_TIMER);
		/// <summary>
		/// Gets the total number of frames that have been rendered in this session.
		/// </summary>
		public static int FrameCount => Function.Call<int>(Hash.GET_FRAME_COUNT);

		/// <summary>
		/// Sets the time scale of the game.
		/// </summary>
		/// <value>
		/// The time scale, only accepts values in range 0.0f to 1.0f.
		/// </value>
		public static float TimeScale
		{
			get => SHVDN.NativeMemory.TimeScale;
			set => Function.Call(Hash.SET_TIME_SCALE, value);
		}

		/// <summary>
		/// Sets how far the minimap should be zoomed in.
		/// </summary>
		/// <value>
		/// The radar zoom; accepts values from 0 to 200.
		/// </value>
		public static int RadarZoom
		{
			set => Function.Call(Hash.SET_RADAR_ZOOM, value);
		}
		/// <summary>
		/// Gets or sets the maximum wanted level a <see cref="GTA.Player"/> can receive.
		/// </summary>
		/// <value>
		/// The maximum wanted level, only accepts values 0 to 5 inclusive.
		/// </value>
		public static int MaxWantedLevel
		{
			get => Function.Call<int>(Hash.GET_MAX_WANTED_LEVEL);
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (value > 5)
				{
					value = 5;
				}

				Function.Call(Hash.SET_MAX_WANTED_LEVEL, value);
			}
		}
		public static float WantedMultiplier
		{
			set => Function.Call(Hash.SET_WANTED_LEVEL_MULTIPLIER, value);
		}

		/// <summary>
		/// Gets or sets a value informing the engine if a mission is in progress.
		/// </summary>
		public static bool MissionFlag
		{
			get => Function.Call<bool>(Hash.GET_MISSION_FLAG);
			set => Function.Call(Hash.SET_MISSION_FLAG, value);
		}

		public static bool ShowsPoliceBlipsOnRadar
		{
			set => Function.Call(Hash.SET_POLICE_RADAR_BLIPS, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether to render the world with a night vision filter.
		/// </summary>
		public static bool Nightvision
		{
			get => !Function.Call<bool>(Hash._IS_NIGHTVISION_INACTIVE);
			set => Function.Call(Hash.SET_NIGHTVISION, value);
		}
		/// <summary>
		/// Gets or sets a value indicating whether to render the world with a thermal vision filter.
		/// </summary>
		public static bool ThermalVision
		{
			get => Function.Call<bool>(Hash._IS_SEETHROUGH_ACTIVE);
			set => Function.Call(Hash.SET_SEETHROUGH, value);
		}

		public static string GetUserInput(int maxLength)
		{
			return GetUserInput(WindowTitle.FMMC_KEY_TIP8, "", maxLength);
		}
		public static string GetUserInput(WindowTitle windowTitle, int maxLength)
		{
			return GetUserInput(windowTitle, "", maxLength);
		}
		public static string GetUserInput(string defaultText, int maxLength)
		{
			return GetUserInput(WindowTitle.FMMC_KEY_TIP8, defaultText, maxLength);
		}
		public static string GetUserInput(WindowTitle windowTitle, string defaultText, int maxLength)
		{
			SHVDN.ScriptDomain.CurrentDomain.PauseKeyEvents(true);

			Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, true, windowTitle.ToString(), "", defaultText, "", "", "", maxLength + 1);

			while (Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD) == 0)
			{
				Script.Yield();
			}

			SHVDN.ScriptDomain.CurrentDomain.PauseKeyEvents(false);

			return Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
		}

		/// <summary>
		/// Gets an analog value of a <see cref="Control"/> input.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns>The <see cref="Control"/> value.</returns>
		public static int GetControlValue(int index, Control control)
		{
			return Function.Call<int>(Hash.GET_CONTROL_VALUE, index, (int)control);
		}
		/// <summary>
		/// Gets an analog value of a <see cref="Control"/> input between -1.0f and 1.0f.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns>The <see cref="Control"/> value.</returns>
		public static float GetControlNormal(int index, Control control)
		{
			return Function.Call<float>(Hash.GET_CONTROL_NORMAL, index, (int)control);
		}
		/// <summary>
		/// Gets an analog value of a disabled <see cref="Control"/> input between -1.0f and 1.0f.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns>The normalized <see cref="Control"/> value.</returns>
		public static float GetDisabledControlNormal(int index, Control control)
		{
			return Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, index, (int)control);
		}
		/// <summary>
		/// Override a <see cref="Control"/> by giving it a user-defined value this frame.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <param name="value">the value to set the control to.</param>
		public static void SetControlNormal(int index, Control control, float value)
		{
			Function.Call(Hash._SET_CONTROL_NORMAL, index, (int)control, value);
		}

		public static bool IsKeyPressed(Keys key)
		{
			return SHVDN.ScriptDomain.CurrentDomain.IsKeyPressed(key);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> is currently pressed.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> is pressed; otherwise, <see langword="false" /></returns>
		/// <remarks>
		/// Does not test whether the control is disabled before checking whether a <see cref="Control"/> is currently pressed.
		/// like <c>IS_CONTROL_PRESSED</c> does.
		/// </remarks>
		public static bool IsControlPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, index, (int)control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> was just pressed this frame.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> was just pressed this frame; otherwise, <see langword="false" /></returns>
		/// <remarks>
		/// Does not test whether the control is disabled before checking whether a <see cref="Control"/> was just pressed this frame
		/// like <c>IS_CONTROL_JUST_PRESSED</c> does.
		/// </remarks>
		public static bool IsControlJustPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, index, (int)control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> was just released this frame.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> was just released this frame; otherwise, <see langword="false" /></returns>
		/// <remarks>
		/// Does not test whether the control is disabled before checking whether a <see cref="Control"/> was just released this frame
		/// like <c>IS_CONTROL_JUST_RELEASED</c> does.
		/// </remarks>
		public static bool IsControlJustReleased(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, index, (int)control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> is disabled and currently pressed.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> is pressed; otherwise, <see langword="false" /></returns>
		public static bool IsDisabledControlPressed(int index, Control control)
		{
			return IsControlPressed(index, control) && !IsControlEnabled(index, control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> is disabled and was just pressed this frame.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> was just released this frame; otherwise, <see langword="false" /></returns>
		public static bool IsDisabledControlJustPressed(int index, Control control)
		{
			return IsControlJustPressed(index, control) && !IsControlEnabled(index, control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> is enabled and was just released this frame.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> was just released this frame; otherwise, <see langword="false" /></returns>
		public static bool IsDisabledControlJustReleased(int index, Control control)
		{
			return IsControlJustReleased(index, control) && !IsControlEnabled(index, control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> is enabled and currently pressed.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> is pressed; otherwise, <see langword="false" /></returns>
		public static bool IsEnabledControlPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_PRESSED, index, (int)control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> is enabled and was just pressed this frame.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> was just released this frame; otherwise, <see langword="false" /></returns>
		public static bool IsEnabledControlJustPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, index, (int)control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> is enabled and was just released this frame.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> was just released this frame; otherwise, <see langword="false" /></returns>
		public static bool IsEnabledControlJustReleased(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, index, (int)control);
		}

		/// <summary>
		/// Gets whether a <see cref="Control"/> is enabled or disabled this frame.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> is Enabled; otherwise, <see langword="false" /></returns>
		public static bool IsControlEnabled(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_ENABLED, index, (int)control);
		}
		/// <inheritdoc cref="EnableControlThisFrame(int, Control)"/>
		[Obsolete("The Game.EnableControl is obsolete, use Game.EnableControlThisFrame instead.")]
		public static void EnableControl(int index, Control control)
		{
			EnableControlThisFrame(index, control);
		}
		/// <summary>
		/// Makes the engine respond to the given <see cref="Control"/> this frame.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/> to enable.</param>
		public static void EnableControlThisFrame(int index, Control control)
		{
			Function.Call(Hash.ENABLE_CONTROL_ACTION, index, (int)control, true);
		}
		/// <inheritdoc cref="DisableControlThisFrame(int, Control)"/>
		[Obsolete("The Game.DisableControl is obsolete, use Game.DisableControlThisFrame instead.")]
		public static void DisableControl(int index, Control control)
		{
			DisableControlThisFrame(index, control);
		}
		/// <summary>
		/// Makes the engine ignore input from the given <see cref="Control"/> this frame.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		/// <param name="control">The <see cref="Control"/>.</param>
		public static void DisableControlThisFrame(int index, Control control)
		{
			Function.Call(Hash.DISABLE_CONTROL_ACTION, index, (int)control, true);
		}
		/// <summary>
		/// Enables all <see cref="Control"/>s this frame.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		public static void EnableAllControlsThisFrame(int index)
		{
			Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, index);
		}
		/// <summary>
		/// Disables all <see cref="Control"/>s this frame.
		/// </summary>
		/// <param name="index">
		/// Supposed to be the control type index. 0 means player control, 1 means camera control, and 2 means frontend control.
		/// However, this value has no practical effect as control native functions eventually use the same CControl instance in any cases.
		/// </param>
		public static void DisableAllControlsThisFrame(int index)
		{
			Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, index);
		}

		/// <summary>
		/// Plays music from the game's music files.
		/// </summary>
		/// <param name="musicFile">The music file to play.</param>
		public static void PlayMusic(string musicFile)
		{
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, musicFile);
		}
		public static void PlaySound(string soundFile, string soundSet)
		{
			Audio.ReleaseSound(Audio.PlaySoundFrontend(soundFile, soundSet));
		}
		/// <summary>
		/// Cancels playing a music file.
		/// </summary>
		/// <param name="musicFile">The music file to stop.</param>
		public static void StopMusic(string musicFile)
		{
			Function.Call(Hash.CANCEL_MUSIC_EVENT, musicFile); // Needs a general Game.StopMusic()
		}

		/// <summary>
		/// Calculates a Jenkins One At A Time hash from the given <see cref="string"/> which can then be used by any native function that takes a hash.
		/// Can be called in any thread.
		/// </summary>
		/// <param name="input">The input <see cref="string"/> to hash.</param>
		/// <returns>The Jenkins hash of the input <see cref="string"/>.</returns>
		/// <remarks>
		/// <para>Converts ASCII uppercase characters to lowercase ones and backslash characters to slash ones before converting into a hash.</para>
		/// <para>
		/// Although the <c>GET_HASH_KEY</c> native compute hash from the substring between two double quotes if the first character is a double quote character,
		/// This method does not consider such case since no practical occurrences of such edge case are found.
		/// </para>
		/// </remarks>
		public static int GenerateHash(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return 0;
			}

			return unchecked((int)SHVDN.NativeMemory.GetHashKey(input));
		}

		/// <summary>
		/// Returns a localized <see cref="string"/> from the games language files with a specified GXT key.
		/// </summary>
		/// <param name="entry">The GXT key.</param>
		public static string GetGXTEntry(string entry)
		{
			return Function.Call<string>(Hash._GET_LABEL_TEXT, entry);
		}
	}
}
