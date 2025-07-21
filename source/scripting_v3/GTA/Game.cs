//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using GTA.Input;
using GTA.Native;
using Button = GTA.Input.Button;

namespace GTA
{
    public static class Game
    {
        #region Fields
        static Player s_cachedPlayer = null;
        static Ped s_cachedLocalPlayerPed = null;

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
        internal static readonly string[] s_windowTitles = {
            "CELL_EMAIL_BOD",
            "CELL_EMAIL_BODE",
            "CELL_EMAIL_BODF",
            "CELL_EMAIL_SOD",
            "CELL_EMAIL_SODE",
            "FMMC_KEY_TIP10",
            "FMMC_KEY_TIP12",
            "FMMC_KEY_TIP12F",
            "FMMC_KEY_TIP12N",
            "FMMC_KEY_TIP8",
            "FMMC_KEY_TIP8F",
            "FMMC_KEY_TIP8FS",
            "FMMC_KEY_TIP8S",
            "FMMC_KEY_TIP9",
            "FMMC_KEY_TIP9F",
            "FMMC_KEY_TIP9N",
            "PM_NAME_CHALL"
        };
        #endregion

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
        /// Gets the local player <see cref="Ped"/> that you are controlling.
        /// Use this property instead of <see cref="GTA.Player.Character"/> when you are only interested in the player <see cref="Ped"/>
        /// and not the <see cref="GTA.Player"/> instance where a lot of player specific states are stored.
        /// </summary>
        public static Ped LocalPlayerPed
        {
            get
            {
                int handle = SHVDN.NativeMemory.GetLocalPlayerPedHandle();

                if (s_cachedLocalPlayerPed == null || handle != s_cachedLocalPlayerPed.Handle)
                {
                    s_cachedLocalPlayerPed = new Ped(handle);
                }

                return s_cachedLocalPlayerPed;
            }
        }

        /// <summary>
        /// Gets the blip of the <see cref="GTA.Player"/> that you are controlling.
        /// </summary>
        public static Blip PlayerBlip => new(Function.Call<int>(Hash.GET_MAIN_PLAYER_BLIP_ID));

        /// <summary>
        /// Gets the north blip, which is shown on the radar.
        /// </summary>
        public static Blip NorthBlip
            => FileVersion >= VersionConstsForGameVersion.v1_0_463_1
            ? new Blip(Function.Call<int>(Hash.GET_NORTH_BLID_INDEX))
            : new Blip(SHVDN.NativeMemory.GetNorthBlip());

        /// <summary>
        /// Gets the current game language.
        /// </summary>
        public static Language Language => Function.Call<Language>(Hash.GET_CURRENT_LANGUAGE);

        /// <summary>
        /// Gets the "FileVersion" resource value of GTA5.exe, which is the same as what SHV's function
        /// <c>getGameVersionInfo</c> retrieves, as a <see cref="System.Version"/> instance.
        /// </summary>
        public static Version FileVersion => SHVDN.NativeMemory.GameFileVersion;

        /// <summary>
        /// Gets the version of the game.
        /// </summary>
        [Obsolete("`Game.Version` is deprecated because Script Hook V is deprecating `getGameVersion`, which " +
            "the property is based on. Use `Game.FileVersion` instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static GameVersion Version => (GameVersion)SHVDN.NativeMemory.GetGameVersion();

        /// <summary>
        /// Gets the measurement system the game uses to display.
        /// </summary>
        public static MeasurementSystem MeasurementSystem => Function.Call<bool>(Hash.SHOULD_USE_METRIC_MEASUREMENTS) ? MeasurementSystem.Metric : MeasurementSystem.Imperial;

        /// <summary>
        /// Gets how many milliseconds the game has been open in this session
        /// </summary>
        public static int GameTime => Function.Call<int>(Hash.GET_GAME_TIMER);

        /// <summary>
        /// Gets or sets the timescale of the game.
        /// </summary>
        /// <value>
        /// The timescale, only accepts values in range 0.0f to 1.0f.
        /// </value>
        public static float TimeScale
        {
            get => SHVDN.NativeMemory.TimeScale;
            set => Function.Call(Hash.SET_TIME_SCALE, value);
        }

        /// <summary>
        /// Gets the total number of frames that have been rendered in this session.
        /// </summary>
        public static int FrameCount => Function.Call<int>(Hash.GET_FRAME_COUNT);

        /// <summary>
        /// Gets the current frame rate in frames per second.
        /// </summary>
        public static float FPS => 1.0f / LastFrameTime;
        /// <summary>
        /// Gets the time in seconds it took for the last frame to render.
        /// </summary>
        public static float LastFrameTime => Function.Call<float>(Hash.GET_FRAME_TIME);

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

                if (string.IsNullOrEmpty(radioName))
                {
                    return RadioStation.RadioOff;
                }

                return (RadioStation)Array.IndexOf(s_radioNames, radioName);
            }
            set
            {
                if (value == RadioStation.RadioOff)
                {
                    Function.Call(Hash.SET_RADIO_TO_STATION_NAME, "OFF");
                }
                else if (Enum.IsDefined(typeof(RadioStation), value))
                {
                    Function.Call(Hash.SET_RADIO_TO_STATION_NAME, s_radioNames[(int)value]);
                }
                else
                {
                    // Probably do nothing, but just in case static funcs of `audRadioStation` accept
                    // an `audRadioStation` instance that has the null name hash (which is zero)
                    Function.Call(Hash.SET_RADIO_TO_STATION_NAME, SHVDN.NativeMemory.NullString);
                }
            }
        }
        /// <summary>
        /// Makes the specified radio station appear on the radio wheel.
        /// </summary>
        /// <param name="station">Radio station</param>
        public static void UnlockRadioStation(RadioStation station)
        {
            if (Enum.IsDefined(typeof(RadioStation), station) && station != RadioStation.RadioOff)
            {
                Function.Call(Hash.LOCK_RADIO_STATION, s_radioNames[(int)station], false);
            }
        }
        /// <summary>
        /// Prevents the specified radio station from appearing on the radio wheel.
        /// </summary>
        /// <param name="station">Radio station</param>
        public static void LockRadioStation(RadioStation station)
        {
            if (Enum.IsDefined(typeof(RadioStation), station) && station != RadioStation.RadioOff)
            {
                Function.Call(Hash.LOCK_RADIO_STATION, s_radioNames[(int)station], true);
            }
        }
        /// <summary>
        /// Makes all the radio stations appear on the radio wheel.
        /// </summary>
        public static void UnlockAllRadioStations()
        {
            foreach (string station in s_radioNames)
            {
                Function.Call(Hash.LOCK_RADIO_STATION, station, false);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to render the world with a night vision filter.
        /// </summary>
        public static bool IsNightVisionActive
        {
            get => Function.Call<bool>(Hash.GET_USINGNIGHTVISION);
            set => Function.Call(Hash.SET_NIGHTVISION, value);
        }
        /// <summary>
        /// Gets or sets a value indicating whether to render the world with a thermal vision filter.
        /// </summary>
        public static bool IsThermalVisionActive
        {
            get => Function.Call<bool>(Hash.GET_USINGSEETHROUGH);
            set => Function.Call(Hash.SET_SEETHROUGH, value);
        }

        /// <summary>
        /// Gets or sets a value informing the engine if a mission is in progress.
        /// </summary>
        public static bool IsMissionActive
        {
            get => Function.Call<bool>(Hash.GET_MISSION_FLAG);
            set => Function.Call(Hash.SET_MISSION_FLAG, value);
        }
        /// <summary>
        /// Gets or sets a value informing the engine if a random event is in progress.
        /// </summary>
        public static bool IsRandomEventActive
        {
            get => Function.Call<bool>(Hash.GET_RANDOM_EVENT_FLAG);
            set => Function.Call(Hash.SET_RANDOM_EVENT_FLAG, value);
        }
        /// <summary>
        /// Gets a value indicating whether the cutscene is active.
        /// </summary>
        public static bool IsCutsceneActive => Function.Call<bool>(Hash.IS_CUTSCENE_ACTIVE);
        /// <summary>
        /// Gets a value indicating whether there is a waypoint set on the map.
        /// </summary>
        public static bool IsWaypointActive => Function.Call<bool>(Hash.IS_WAYPOINT_ACTIVE);

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
        /// Gets or sets a value indicating whether the pause menu is active.
        /// </summary>
        public static bool IsPaused
        {
            get => Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE);
            set => Function.Call(Hash.SET_PAUSE_MENU_ACTIVE, value);
        }

        /// <summary>
        /// Pause/resume the game.
        /// </summary>
        /// <param name="value">True/false for pause/resume.</param>
        public static void Pause(bool value)
        {
            Function.Call(Hash.SET_GAME_PAUSED, value);
        }

        /// <summary>
        /// Gets a value indicating whether there is a loading screen being displayed if Script Hook V v1.0.3337.0 or
        /// older is installed to the game.
        /// </summary>
        /// <remarks>
        /// This property always return <see langword="false"/> since Script Hook V v1.0.3351.0+ This is because
        /// SHV changed the way SHV scripts start in v1.0.3351.0 (SHV version and not game version) and they will never be
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
        /// Creates an input box for the user to input text using the keyboard.
        /// </summary>
        /// <param name="defaultText">The default text.</param>
        /// <returns>The <see cref="string"/> of what the user entered or <see cref="string.Empty"/> if the user canceled.</returns>
        public static string GetUserInput(string defaultText = "")
        {
            return GetUserInput(WindowTitle.EnterMessage60, defaultText, 60);
        }
        /// <summary>
        /// Creates an input box for the user to input text using the keyboard.
        /// </summary>
        /// <param name="windowTitle">The title of the input box window.</param>
        /// <param name="maxLength">The maximum length of text input allowed.</param>
        /// <param name="defaultText">The default text.</param>
        /// <returns>The <see cref="string"/> of what the user entered or <see cref="string.Empty"/> if the user canceled.</returns>
        public static string GetUserInput(WindowTitle windowTitle, string defaultText, int maxLength)
        {
            SHVDN.ScriptDomain.CurrentDomain.PauseKeyEvents(true);

            Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, true, s_windowTitles[(int)windowTitle], SHVDN.NativeMemory.NullString, defaultText, SHVDN.NativeMemory.NullString, SHVDN.NativeMemory.NullString, SHVDN.NativeMemory.NullString, maxLength + 1);

            while (Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD) == 0)
            {
                Script.Yield();
            }

            SHVDN.ScriptDomain.CurrentDomain.PauseKeyEvents(false);

            return Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
        }

        /// <summary>
        /// Gets whether a cheat code was entered into the cheat text box.
        /// </summary>
        /// <param name="cheat">The name of the cheat to check.</param>
        /// <returns><see langword="true" /> if the cheat was just entered; otherwise, <see langword="false" /></returns>
        public static bool WasCheatStringJustEntered(string cheat)
        {
            return Function.Call<bool>(Hash.HAS_PC_CHEAT_WITH_HASH_BEEN_ACTIVATED, StringHash.AtStringHashUtf8(cheat));
        }
        /// <summary>
        /// Gets whether a specific sequence of <see cref="Button"/>s has been pressed.
        /// </summary>
        /// <param name="buttons">The sequence of <see cref="Button"/>s in the order the user should enter them in-game.</param>
        /// <returns><see langword="true" /> if the combination was just entered; otherwise, <see langword="false" /></returns>
        /// <remarks>
        /// There must be between 6 and 29 inclusive <see cref="Button"/>s otherwise an <see cref="ArgumentException"/> is thrown.
        /// It only works for GamePad inputs. The in-game cheat combinations use the same system.
        /// </remarks>
        public static bool WasButtonCombinationJustEntered(params Button[] buttons)
        {
            if (buttons.Length < 6 || buttons.Length > 29)
            {
                ThrowHelper.ThrowArgumentException("The amount of buttons must be between 6 and 29", nameof(buttons));
            }

            uint hash = 0;

            foreach (Button button in buttons)
            {
                hash += (uint)button;
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }

            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);

            return Function.Call<bool>(Hash.HAS_CHEAT_WITH_HASH_BEEN_ACTIVATED, hash, buttons.Length);
        }

        /// <summary>
        /// Gets whether the last input was made with a GamePad or keyboard and mouse.
        /// </summary>
        public static InputMethod LastInputMethod => Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2) ? InputMethod.MouseAndKeyboard : InputMethod.GamePad;

        /// <summary>
        /// Gets the current targeting mode of the local player.
        /// </summary>
        public static PlayerTargetingMode PlayerTargetingMode => (PlayerTargetingMode)GetProfileSetting(0);

        /// <summary>
        /// Gets a value indicating whether the controller vibration is enabled.
        /// </summary>
        public static bool IsVibrationEnabled => GetProfileSetting(2) != 0;

        /// <summary>
        /// Calculates a Jenkins One At A Time hash from the given <see cref="string"/> which can then be used by any native function that takes a hash.
        /// Can be called in any thread.
        /// </summary>
        /// <param name="input">The input <see cref="string"/> to hash.</param>
        /// <returns>The Jenkins hash of the input <see cref="string"/>.</returns>
        /// <remarks>
        /// Converts ASCII uppercase characters to lowercase ones and backslash characters to slash ones before
        /// converting into a hash. Computes the hash from the substring between two double quotes if the first
        /// character is a double quote character.
        /// </remarks>
        [Obsolete("Use StringHash.AtStringHash(string, uint), StringHash.AtStringHashUtf8(string, uint), " +
            "AtHashValue.FromString(string, uint), or StringHash.AtStringHashUtf8(string, uint) instead.")]
        // Use AtStringHashUtf8 for compatibility reasons
        public static int GenerateHash(string input) => (int)StringHash.AtStringHashUtf8(input);

        /// <summary>
        /// Returns a localized <see cref="string"/> from the games language files with a specified GXT key.
        /// </summary>
        /// <param name="entry">The GXT key.</param>
        /// <returns>The localized <see cref="string"/> if the key exists; otherwise, <see cref="string.Empty"/></returns>
        public static string GetLocalizedString(string entry)
        {
            return Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, entry) ? Function.Call<string>(Hash.GET_FILENAME_FOR_AUDIO_CONVERSATION, entry) : string.Empty;
        }
        /// <summary>
        /// Returns a localized <see cref="string"/> from the games language files with a specified GXT key hash.
        /// </summary>
        /// <param name="entryLabelHash">The GXT key hash.</param>
        /// <returns>The localized <see cref="string"/> if the key hash exists; otherwise, <see cref="string.Empty"/></returns>
        public static string GetLocalizedString(int entryLabelHash)
        {
            return SHVDN.NativeMemory.GetGxtEntryByHash(entryLabelHash);
        }

        /// <summary>
        /// Gets a value associated with the specified index of the profile setting.
        /// </summary>
        /// <param name="index">The index of the profile setting values.</param>
        /// <returns>The integer value associated with the specified index of the profile setting.</returns>
        public static int GetProfileSetting(int index)
        {
            return Function.Call<int>(Hash.GET_PROFILE_SETTING, index);
        }


        /// <summary>
        /// Searches the address space of the current process for a memory pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="startAddress">The address to start searching at. If <see cref="IntPtr.Zero" /> (<see langword="default" />), search is started at the base address.</param>
        /// <returns>The address of a region matching the pattern, or <see cref="IntPtr.Zero" /> if none was found.</returns>
        /// <remarks>This function takes the Cheat Engine/IDA format ("48 8B 0D ?? ?? ? ? 44 8B C6 8B D5 8B D8" for example, where ?? and ? are wildcards).</remarks>
        public static IntPtr FindPattern(string pattern, IntPtr startAddress = default)
        {
            string[] rawHexStringsSplitted = pattern.Split(' ');
            var newPatternBuilder = new StringBuilder(rawHexStringsSplitted.Length);
            var newMaskBuilder = new StringBuilder(rawHexStringsSplitted.Length);

            foreach (string rawHex in rawHexStringsSplitted)
            {
                if (string.IsNullOrEmpty(rawHex))
                {
                    continue;
                }

                if (rawHex == "??" || rawHex == "?")
                {
                    newPatternBuilder.Append("\x00");
                    newMaskBuilder.Append("?");
                    continue;
                }

                char character = (char)short.Parse(rawHex, NumberStyles.AllowHexSpecifier);
                newPatternBuilder.Append(character);
                newMaskBuilder.Append("x");
            }

            return FindPattern(newPatternBuilder.ToString(), newMaskBuilder.ToString(), startAddress);
        }
        /// <summary>
        /// Searches the address space of the current process for a memory pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="mask">The pattern mask.</param>
        /// <param name="startAddress">The address to start searching at. If <see cref="IntPtr.Zero" /> (<see langword="default" />), search is started at the base address.</param>
        /// <returns>The address of a region matching the pattern, or <see cref="IntPtr.Zero" /> if none was found.</returns>
        /// <remarks>This function takes the classic format ("\x48\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC6\x8B\xD5\x8B\xD8" as the pattern and "xxx????xxxxxxx" as the mask for example, where \x00 in the pattern and ? In the mask is a wildcard).</remarks>
        public static IntPtr FindPattern(string pattern, string mask, IntPtr startAddress = default)
        {
            unsafe
            {
                byte* address = (startAddress == IntPtr.Zero ? SHVDN.MemScanner.FindPatternNaive(pattern, mask) : SHVDN.MemScanner.FindPatternNaive(pattern, mask, startAddress));
                return address == null ? IntPtr.Zero : new IntPtr(address);
            }
        }
    }
}
