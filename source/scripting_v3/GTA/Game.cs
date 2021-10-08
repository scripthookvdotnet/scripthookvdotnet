//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace GTA
{
	public static class Game
	{
		#region Fields
		static Player cachedPlayer = null;

		internal static readonly string[] radioNames = {
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
			"RADIO_OFF"
		};
		internal static readonly string[] windowTitles = {
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
		/// Gets the <see cref="GTA.Player"/> that you are controlling.
		/// </summary>
		public static Player Player
		{
			get
			{
				int handle = Function.Call<int>(Hash.PLAYER_ID);

				if (cachedPlayer == null || handle != cachedPlayer.Handle)
				{
					cachedPlayer = new Player(handle);
				}

				return cachedPlayer;
			}
		}

		/// <summary>
		/// Gets the blip of the <see cref="GTA.Player"/> that you are controlling.
		/// </summary>
		public static Blip PlayerBlip => new Blip(Function.Call<int>(Hash.GET_MAIN_PLAYER_BLIP_ID));

		/// <summary>
		/// Gets the north blip, which is shown on the radar.
		/// </summary>
		public static Blip NorthBlip => Version >= GameVersion.v1_0_463_1_Steam ? new Blip(Function.Call<int>((Hash)0x3F0CF9CB7E589B88)) : new Blip(SHVDN.NativeMemory.GetNorthBlip());

		/// <summary>
		/// Gets the current game language.
		/// </summary>
		public static Language Language => Function.Call<Language>(Hash.GET_CURRENT_LANGUAGE);

		/// <summary>
		/// Gets the version of the game.
		/// </summary>
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
		/// Gets or Sets the time scale of the game.
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
		/// The maximum wanted level, only accepts values 0 to 5.
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
		public static RadioStation RadioStation
		{
			get
			{
				string radioName = Function.Call<string>(Hash.GET_PLAYER_RADIO_STATION_NAME);

				if (String.IsNullOrEmpty(radioName))
				{
					return RadioStation.RadioOff;
				}

				return (RadioStation)Array.IndexOf(radioNames, radioName);
			}
			set
			{
				if (Enum.IsDefined(typeof(RadioStation), value) && value != RadioStation.RadioOff)
				{
					Function.Call(Hash.SET_RADIO_TO_STATION_NAME, radioNames[(int)value]);
				}
				else
				{
					Function.Call(Hash.SET_RADIO_TO_STATION_NAME, SHVDN.NativeMemory.NullString);
				}
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
		/// Performs an automatic game save.
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
			GTA.Native.Function.Call(GTA.Native.Hash.SET_GAME_PAUSED, value);
		}

		/// <summary>
		/// Gets a value indicating whether there is a loading screen being displayed.
		/// </summary>
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

			Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, true, windowTitles[(int)windowTitle], SHVDN.NativeMemory.NullString, defaultText, SHVDN.NativeMemory.NullString, SHVDN.NativeMemory.NullString, SHVDN.NativeMemory.NullString, maxLength + 1);

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
			return Function.Call<bool>(Hash._HAS_CHEAT_STRING_JUST_BEEN_ENTERED, GenerateHash(cheat));
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
				throw new ArgumentException("The amount of buttons must be between 6 and 29", "buttons");
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

			return Function.Call<bool>(Hash._HAS_BUTTON_COMBINATION_JUST_BEEN_ENTERED, hash, buttons.Length);
		}

		/// <summary>
		/// Gets whether the last input was made with a GamePad or keyboard and mouse.
		/// </summary>
		public static InputMethod LastInputMethod => Function.Call<bool>(Hash._IS_USING_KEYBOARD, 2) ? InputMethod.MouseAndKeyboard : InputMethod.GamePad;

		/// <summary>
		/// Gets the current targeting mode of the local player.
		/// </summary>
		public static PlayerTargetingMode PlayerTargetingMode => (PlayerTargetingMode)GetProfileSetting(0);

		/// <summary>
		/// Gets a value indicating whether the controller vibration is enabled.
		/// </summary>
		public static bool IsVibrationEnabled => GetProfileSetting(2) != 0;

		/// <summary>
		/// Gets an analog value of a <see cref="Control"/> input.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns>The <see cref="Control"/> value.</returns>
		public static int GetControlValue(Control control)
		{
			return Function.Call<int>(Hash.GET_CONTROL_VALUE, 0, control);
		}
		/// <summary>
		/// Gets an analog value of a <see cref="Control"/> input between -1.0f and 1.0f.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns>The normalized <see cref="Control"/> value.</returns>
		public static float GetControlValueNormalized(Control control)
		{
			return Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, control);
		}
		/// <summary>
		/// Gets an analog value of a disabled <see cref="Control"/> input between -1.0f and 1.0f.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns>The normalized <see cref="Control"/> value.</returns>
		public static float GetDisabledControlValueNormalized(Control control)
		{
			return Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, 0, control);
		}
		/// <summary>
		/// Override a <see cref="Control"/> by giving it a user-defined value this frame.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <param name="value">the value to set the control to.</param>
		public static void SetControlValueNormalized(Control control, float value)
		{
			Function.Call(Hash._SET_CONTROL_NORMAL, 0, control, value);
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
		/// Gets whether a <see cref="Control"/> is currently pressed.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> is pressed; otherwise, <see langword="false" /></returns>
		public static bool IsControlPressed(Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> was just pressed this frame
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> was just pressed this frame; otherwise, <see langword="false" /></returns>
		public static bool IsControlJustPressed(Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> was just released this frame
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> was just released this frame; otherwise, <see langword="false" /></returns>
		public static bool IsControlJustReleased(Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 0, control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> is enabled and currently pressed.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> is pressed; otherwise, <see langword="false" /></returns>
		public static bool IsEnabledControlPressed(Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 0, control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> is enabled and was just pressed this frame.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> was just pressed this frame; otherwise, <see langword="false" /></returns>
		public static bool IsEnabledControlJustPressed(Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 0, control);
		}
		/// <summary>
		/// Gets whether a <see cref="Control"/> is enabled and was just released this frame.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> was just released this frame; otherwise, <see langword="false" /></returns>
		public static bool IsEnabledControlJustReleased(Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, 0, control);
		}

		/// <summary>
		/// Gets whether a <see cref="Control"/> is enabled or disabled this frame.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to check.</param>
		/// <returns><see langword="true" /> if the <see cref="Control"/> is Enabled; otherwise, <see langword="false" /></returns>
		public static bool IsControlEnabled(Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_ENABLED, 0, control);
		}
		/// <summary>
		/// Makes the engine respond to the given <see cref="Control"/> this frame.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> to enable..</param>
		public static void EnableControlThisFrame(Control control)
		{
			Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, control, true);
		}
		/// <summary>
		/// Makes the engine ignore input from the given <see cref="Control"/> this frame.
		/// </summary>
		/// <param name="control">The <see cref="Control"/>.</param>
		public static void DisableControlThisFrame(Control control)
		{
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, control, true);
		}
		/// <summary>
		/// Enables all <see cref="Control"/>s this frame.
		/// </summary>
		public static void EnableAllControlsThisFrame()
		{
			Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, 0);
		}
		/// <summary>
		/// Disables all <see cref="Control"/>s this frame.
		/// </summary>
		public static void DisableAllControlsThisFrame()
		{
			Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
		}

		/// <summary>
		/// Calculates a Jenkins One At A Time hash from the given <see cref="string"/> which can then be used by any native function that takes a hash.
		/// Can be called in any thread.
		/// </summary>
		/// <param name="input">The input <see cref="string"/> to hash.</param>
		/// <returns>The Jenkins hash of the input <see cref="string"/>.</returns>
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
		/// <returns>The localized <see cref="string"/> if the key exists; otherwise, <see cref="string.Empty"/></returns>
		public static string GetLocalizedString(string entry)
		{
			return Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, entry) ? Function.Call<string>(Hash._GET_LABEL_TEXT, entry) : string.Empty;
		}
		/// <summary>
		/// Returns a localized <see cref="string"/> from the games language files with a specified GXT key hash.
		/// </summary>
		/// <param name="entryLabelHash">The GXT key hash.</param>
		/// <returns>The localized <see cref="string"/> if the key hash exists; otherwise, <see cref="string.Empty"/></returns>
		public static string GetLocalizedString(int entryLabelHash)
		{
			return SHVDN.NativeMemory.GetGXTEntryByHash(entryLabelHash);
		}

		/// <summary>
		/// Gets an value associated with the specified index of the profile setting.
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
		/// <returns>The address of a region matching the pattern, or <see cref="IntPtr.Zero" /> if none was found.</returns>
		/// <remarks>This function takes the Cheat Engine/IDA format ("48 8B 0D ?? ?? ? ? 44 8B C6 8B D5 8B D8" for example, where ?? and ? are wildcards).</remarks>
		public static IntPtr FindPattern(string pattern)
		{
			string newPattern = string.Empty;
			string newMask = string.Empty;

			foreach (string rawHex in pattern.Split(' '))
			{
				if (string.IsNullOrEmpty(rawHex))
				{
					continue;
				}

				if (rawHex == "??" || rawHex == "?")
				{
					newPattern += "\x00";
					newMask += "?";
					continue;
				}

				char character = (char)short.Parse(rawHex, NumberStyles.AllowHexSpecifier);
				newPattern += character;
				newMask += "x";
			}

			return FindPattern(newPattern, newMask);
		}
		/// <summary>
		/// Searches the address space of the current process for a memory pattern.
		/// </summary>
		/// <param name="pattern">The pattern.</param>
		/// <param name="mask">The pattern mask.</param>
		/// <returns>The address of a region matching the pattern, or <see cref="IntPtr.Zero" /> if none was found.</returns>
		/// <remarks>This function takes the classic format ("\x48\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC6\x8B\xD5\x8B\xD8" as the pattern and "xxx????xxxxxxx" as the mask for example, where \x00 in the pattern and ? In the mask is a wildcard).</remarks>
		public static IntPtr FindPattern(string pattern, string mask)
		{
			unsafe
			{
				byte* address = SHVDN.NativeMemory.FindPattern(pattern, mask);
				return address == null ? IntPtr.Zero : new IntPtr(address);
			}
		}
	}
}
