//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.Drawing;
using System.Text;
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

        // Performs ASCII uppercase to ASCII lowercase and backslash to slash conversion, does not perform any conversions to non-ASCII characters.
        // Use this table because character conversion with this table performs faster than calculating converted characters using branch jump instructions.
        // The former method is used in GTA5.exe and the latter one is used in GTAIV.exe.
        private static readonly byte[] s_lookupTableForGetHashKey =
        {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A,
            0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15,
            0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B,
            0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36,
            0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x40, 0x61,
            0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C,
            0x6D, 0x6E, 0x6F, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77,
            0x78, 0x79, 0x7A, 0x5B, 0x2F, 0x5D, 0x5E, 0x5F, 0x60, 0x61, 0x62,
            0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D,
            0x6E, 0x6F, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78,
            0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83,
            0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E,
            0x8F, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99,
            0x9A, 0x9B, 0x9C, 0x9D, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA3, 0xA4,
            0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAB, 0xAC, 0xAD, 0xAE, 0xAF,
            0xB0, 0xB1, 0xB2, 0xB3, 0xB4, 0xB5, 0xB6, 0xB7, 0xB8, 0xB9, 0xBA,
            0xBB, 0xBC, 0xBD, 0xBE, 0xBF, 0xC0, 0xC1, 0xC2, 0xC3, 0xC4, 0xC5,
            0xC6, 0xC7, 0xC8, 0xC9, 0xCA, 0xCB, 0xCC, 0xCD, 0xCE, 0xCF, 0xD0,
            0xD1, 0xD2, 0xD3, 0xD4, 0xD5, 0xD6, 0xD7, 0xD8, 0xD9, 0xDA, 0xDB,
            0xDC, 0xDD, 0xDE, 0xDF, 0xE0, 0xE1, 0xE2, 0xE3, 0xE4, 0xE5, 0xE6,
            0xE7, 0xE8, 0xE9, 0xEA, 0xEB, 0xEC, 0xED, 0xEE, 0xEF, 0xF0, 0xF1,
            0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC,
            0xFD, 0xFE, 0xFF
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
        /// Gets a value indicating whether there is a loading screen being displayed if Script Hook V v1.0.3337.0 or
        /// older is installed to the game.
        /// </summary>
        /// <remarks>
        /// This property always return <see langword="false"/> since Script Hook V v1.0.3351.0+ This is because
        /// SHV changed the way SHV scripts start in v1.0.3351.0 (SHV version and not game version) and they never be
        /// able to start before the game finished showing the loading screen since SHV v1.0.3351.0+. See
        /// <see href="https://github.com/scripthookvdotnet/scripthookvdotnet/issues/1549">#1549 on the main GitHub
        /// repository</see> for details.
        /// </remarks>
        [Obsolete("`Game.IsLoading` is obsolete because Script Hook V changed the way SHV scripts start in" +
            "v1.0.3351.0 (SHV version and not game version) and they never be able to start before the game " +
            "finished showing the loading screen since SHV v1.0.3351.0+. It is advised not to use `Game.IsLoading`" +
            "at all.")]
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
        /// <remarks>
        /// Setting to <see cref="RadioStation.RadioOff"/> has the effect only when the player is using
        /// the radio of a <see cref="Vehicle"/> or the mobile radio, since
        /// <c>audRadioAudioEntity::RetuneToStation(const char *stationName)</c> does nothing if the player is not
        /// using their radio when the argument is a string that says "<c>OFF</c>".
        /// </remarks>
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
        /// Gets an analog value of a <see cref="Control"/> input in the range of [0, 255].
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <returns>The <see cref="Control"/> value in the range of [0, 255].</returns>
        public static int GetControlValue(int index, Control control)
        {
            return Function.Call<int>(Hash.GET_CONTROL_VALUE, index, (int)control);
        }
        /// <summary>
        /// Gets an analog value of a <see cref="Control"/> input between -1.0f and 1.0f.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <returns>The <see cref="Control"/> value between -1.0f and 1.0f.</returns>
        /// <remarks>
        /// Tests whether the control is disabled before getting an analog value of a given <see cref="Control"/>.
        /// Will return zero if the control is disabled.
        /// </remarks>
        public static float GetControlNormal(int index, Control control)
        {
            return Function.Call<float>(Hash.GET_CONTROL_NORMAL, index, (int)control);
        }
        /// <summary>
        /// Gets an analog value of a <see cref="Control"/> input between -1.0f and 1.0f even if
        /// the <see cref="Control"/> is disabled.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <returns>The normalized <see cref="Control"/> value between -1.0f and 1.0f.</returns>
        public static float GetDisabledControlNormal(int index, Control control)
        {
            return Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, index, (int)control);
        }
        /// <summary>
        /// Sets the value of a <see cref="Control"/> for the next frame (not this frame) if the control is enabled.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <param name="value">the value to set the control to for the next frame.</param>
        /// <remarks>
        /// Does not set a value for the next frame if the control is disabled.
        /// Does not return a bool value despite the fact <c>SET_CONTROL_VALUE_NEXT_FRAME</c> returns
        /// <see langword="true"/> if the control is enabled and returns <see langword="false"/> otherwise.
        /// </remarks>
        public static void SetControlNormal(int index, Control control, float value)
        {
            Function.Call(Hash._SET_CONTROL_NORMAL, index, (int)control, value);
        }

        /// <summary>
        /// Gets whether the specified key is currently held down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        public static bool IsKeyPressed(Keys key)
        {
            return SHVDN.ScriptDomain.CurrentDomain.IsKeyPressed(key);
        }

        /// <summary>
        /// Gets whether a <see cref="Control"/> is currently pressed/down.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <returns>
        /// <para>
        /// <see langword="true"/> if the <see cref="Control"/> is pressed/down; otherwise, <see langword="false"/>.
        /// </para>
        /// <para>
        /// Strictly, returns <see langword="true"/> when <see cref="GetDisabledControlNormal(int, Control)"/>
        /// returns <c>0.5f</c> or more; otherwise, returns <see langword="false"/>.
        /// </para>
        /// </returns>
        /// <remarks>
        /// Does not test whether the control is disabled before checking whether a <see cref="Control"/> is currently
        /// pressed like how <c>IS_CONTROL_PRESSED</c> does.
        /// </remarks>
        public static bool IsControlPressed(int index, Control control)
        {
            return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, index, (int)control);
        }
        /// <summary>
        /// Gets whether a <see cref="Control"/> was just pressed/down this frame and was not pressed/down last frame.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <returns>
        /// <para>
        /// <see langword="true" /> if the <see cref="Control"/> was pressed/down this frame and was not pressed/down
        /// last frame;  otherwise, <see langword="false"/>.
        /// </para>
        /// <para>
        /// Strictly, returns <see langword="true"/> when <see cref="GetDisabledControlNormal(int, Control)"/>
        /// returns <c>0.5f</c> or more this frame and it returns a value less than <c>0.5f</c> last frame; otherwise,
        /// returns <see langword="false"/>.
        /// </para>
        /// </returns>
        /// <remarks>
        /// Does not test whether the control is disabled before checking whether a <see cref="Control"/> was just
        /// pressed this frame like <c>IS_CONTROL_JUST_PRESSED</c> does.
        /// </remarks>
        public static bool IsControlJustPressed(int index, Control control)
        {
            return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, index, (int)control);
        }
        /// <summary>
        /// Gets whether a <see cref="Control"/> was just released/up this frame and was not released/up last frame.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <returns>
        /// <para>
        /// <see langword="true" /> if the <see cref="Control"/> was just released/up this frame and was not
        /// released/up last frame; otherwise, <see langword="false"/>.
        /// </para>
        /// <para>
        /// Strictly, returns <see langword="true"/> when <see cref="GetDisabledControlNormal(int, Control)"/>
        /// returns a value less than <c>0.5f</c> and it returns <c>0.5f</c> last frame; otherwise, returns
        /// <see langword="false"/>.
        /// </para>
        /// </returns>
        /// <remarks>
        /// Does not test whether the control is disabled before checking whether a <see cref="Control"/> was just
        /// released this frame like <c>IS_CONTROL_JUST_RELEASED</c> does.
        /// </remarks>
        public static bool IsControlJustReleased(int index, Control control)
        {
            return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, index, (int)control);
        }
        /// <summary>
        /// Gets whether a <see cref="Control"/> is disabled and currently pressed/down.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <returns>
        /// <para>
        /// <see langword="true"/> if the <see cref="Control"/> is disabled and currently pressed/down; otherwise,
        /// <see langword="false"/>.
        /// </para>
        /// <para>
        /// Strictly, returns <see langword="true"/> when <see cref="IsControlEnabled(int, Control)"/> returns
        /// <see langword="false"/> and <see cref="GetControlNormal(int, Control)"/> returns <c>0.5f</c> or more;
        /// otherwise, returns <see langword="false"/>.
        /// </para>
        /// </returns>
        public static bool IsDisabledControlPressed(int index, Control control)
        {
            return IsControlPressed(index, control) && !IsControlEnabled(index, control);
        }
        /// <summary>
        /// Gets whether a <see cref="Control"/> is disabled and pressed/down this frame and was not pressed/down
        /// last frame.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <returns>
        /// <para>
        /// <see langword="true"/> if the <see cref="Control"/> is disabled and pressed/down this frame, and was
        /// not pressed/down last frame; otherwise, <see langword="false"/>.
        /// </para>
        /// <para>
        /// Strictly, returns <see langword="true"/> when <see cref="IsControlEnabled(int, Control)"/> returns
        /// <see langword="false"/> and <see cref="GetControlNormal(int, Control)"/> returns <c>0.5f</c> or more and
        /// it returns a value less than <c>0.5f</c> last frame; otherwise, returns
        /// <see langword="false"/>.
        /// </para>
        /// </returns>
        public static bool IsDisabledControlJustPressed(int index, Control control)
        {
            return IsControlJustPressed(index, control) && !IsControlEnabled(index, control);
        }
        /// <summary>
        /// Gets whether a <see cref="Control"/> is disabled and was just released/up this frame and was not released/up
        /// last frame.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <returns>
        /// <para>
        /// <see langword="true"/> if the <see cref="Control"/> is disabled and released/up this frame, and was not
        /// released/up last frame; otherwise, <see langword="false"/>.
        /// </para>
        /// <para>
        /// Strictly, returns <see langword="true"/> when <see cref="IsControlEnabled(int, Control)"/> returns
        /// <see langword="false"/> and <see cref="GetDisabledControlNormal(int, Control)"/> returns a value less than
        /// <c>0.5f</c> and it returns <c>0.5f</c> or more last frame; otherwise, returns <see langword="false"/>.
        /// </para>
        /// </returns>
        public static bool IsDisabledControlJustReleased(int index, Control control)
        {
            return IsControlJustReleased(index, control) && !IsControlEnabled(index, control);
        }
        /// <summary>
        /// Gets whether a <see cref="Control"/> is enabled and currently pressed/down.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <returns>
        /// <para>
        /// <see langword="true"/> if the <see cref="Control"/> is enabled and currently pressed/down; otherwise,
        /// <see langword="false"/>.
        /// </para>
        /// <para>
        /// Strictly, returns <see langword="true"/> when <see cref="GetControlNormal(int, Control)"/> returns
        /// <c>0.5f</c> or more; otherwise, returns <see langword="false"/>.
        /// </para>
        /// </returns>
        public static bool IsEnabledControlPressed(int index, Control control)
        {
            return Function.Call<bool>(Hash.IS_CONTROL_PRESSED, index, (int)control);
        }
        /// <summary>
        /// Gets whether a <see cref="Control"/> is enabled and pressed/down this frame and was not pressed/down
        /// last frame.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <returns>
        /// <para>
        /// <see langword="true"/> if the <see cref="Control"/> is enabled and pressed/down this frame, and was
        /// not pressed/down last frame; otherwise, <see langword="false"/>.
        /// </para>
        /// <para>
        /// Strictly, returns <see langword="true"/> when <see cref="GetControlNormal(int, Control)"/> returns
        /// <c>0.5f</c> or more and it returns a value less than <c>0.5f</c> last frame; otherwise, returns
        /// <see langword="false"/>.
        /// </para>
        /// </returns>
        public static bool IsEnabledControlJustPressed(int index, Control control)
        {
            return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, index, (int)control);
        }
        /// <summary>
        /// Gets whether a <see cref="Control"/> is enabled and was just released/up this frame and was not released/up
        /// last frame.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">The <see cref="Control"/> to check.</param>
        /// <returns>
        /// <para>
        /// <see langword="true"/> if the <see cref="Control"/> is enabled and released/up this frame, and was not
        /// released/up last frame; otherwise, <see langword="false"/>.
        /// </para>
        /// <para>
        /// Strictly, returns <see langword="true"/> when <see cref="GetControlNormal(int, Control)"/> returns
        /// a value less than <c>0.5f</c> and it returns <c>0.5f</c> or more last frame; otherwise, returns
        /// <see langword="false"/>.
        /// </para>
        /// </returns>
        public static bool IsEnabledControlJustReleased(int index, Control control)
        {
            return Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, index, (int)control);
        }

        /// <summary>
        /// Gets whether a <see cref="Control"/> is enabled or disabled this frame.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
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
        /// Enables the action input for the given <see cref="Control"/> and related action inputs in the control
        /// system for the main player  so enabled control variants of control (action input) methods and script
        /// commands (native functions) will accept the given <see cref="Control"/>.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">
        /// The <see cref="Control"/> to disable. Related action inputs will also be enabled.
        /// </param>
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
        /// Disables the action input for the given <see cref="Control"/> and related action inputs in the control
        /// system for the main player so enabled control variants of control (action input) methods and script
        /// commands (native functions) will not accept the given <see cref="Control"/>.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        /// <param name="control">
        /// The <see cref="Control"/> to disable. Related action inputs will also be disabled.
        /// </param>
        public static void DisableControlThisFrame(int index, Control control)
        {
            Function.Call(Hash.DISABLE_CONTROL_ACTION, index, (int)control, true);
        }
        /// <summary>
        /// Enables all <see cref="Control"/>s this frame.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
        /// </param>
        public static void EnableAllControlsThisFrame(int index)
        {
            Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, index);
        }
        /// <summary>
        /// Disables all <see cref="Control"/>s this frame.
        /// </summary>
        /// <param name="index">
        /// The control type index. 0 means player control, 1 means camera control (the same as 0 in practice),
        /// and 2 means frontend control.
        /// The empty control is used when the warning screen is being displayed or the player is arrested (not when dead),
        /// if the control type index is 0 or 1.
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

            uint hash = 0;
            foreach (byte c in Encoding.UTF8.GetBytes(input))
            {
                hash += s_lookupTableForGetHashKey[c];
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }

            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);

            return (int)hash;
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
