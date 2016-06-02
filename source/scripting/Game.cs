using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GTA.Native;

namespace GTA
{
	public enum GameVersion
	{
		Unknown = -1,
		v1_0_335_2_Steam,
		v1_0_335_2_NoSteam,
		v1_0_350_1_Steam,
		v1_0_350_2_NoSteam,
		v1_0_372_2_Steam,
		v1_0_372_2_NoSteam,
		v1_0_393_2_Steam,
		v1_0_393_2_NoSteam,
		v1_0_393_4_Steam,
		v1_0_393_4_NoSteam,
		v1_0_463_1_Steam,
		v1_0_463_1_NoSteam,
		v1_0_505_2_Steam,
		v1_0_505_2_NoSteam,
		v1_0_573_1_Steam,
		v1_0_573_1_NoSteam,
		v1_0_617_1_Steam,
		v1_0_617_1_NoSteam,
		v1_0_678_1_Steam,
		v1_0_678_1_NoSteam
	}
	public enum Language
	{
		American,
		French,
		German,
		Italian,
		Spanish,
		Portuguese,
		Polish,
		Russian,
		Korean,
		Chinese,
		Japanese,
		Mexican
	}
	public enum InputMode
	{
		MouseAndKeyboard,
		GamePad
	}
	public enum WindowTitle
	{
		CELL_EMAIL_BOD,
		CELL_EMAIL_BODE,
		CELL_EMAIL_BODF,
		CELL_EMAIL_SOD,
		CELL_EMAIL_SODE,
		CELL_EMAIL_SODF,
		CELL_EMASH_BOD,
		CELL_EMASH_BODE,
		CELL_EMASH_BODF,
		CELL_EMASH_SOD,
		CELL_EMASH_SODE,
		CELL_EMASH_SODF,
		FMMC_KEY_TIP10,
		FMMC_KEY_TIP12,
		FMMC_KEY_TIP12F,
		FMMC_KEY_TIP12N,
		FMMC_KEY_TIP8,
		FMMC_KEY_TIP8F,
		FMMC_KEY_TIP8FS,
		FMMC_KEY_TIP8S,
		FMMC_KEY_TIP9,
		FMMC_KEY_TIP9F,
		FMMC_KEY_TIP9N,
		PM_NAME_CHALL
	}

	public static class Game
	{
		#region Fields
		internal static readonly string[] _radioNames = {
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
			"RADIO_OFF"
		};

		static Player _cachedPlayer;
		#endregion

		static Game()
		{
			Version = (GameVersion)MemoryAccess.GetGameVersion();
			Globals = new GlobalCollection();
		}

		public static GameVersion Version { get; private set; }
		public static Language Language
		{
			get
			{
				return (Language)Function.Call<int>(Hash._GET_UI_LANGUAGE_ID);
			}
		}
		public static Size ScreenResolution
		{
			get
			{
				var width = new OutputArgument();
				var height = new OutputArgument();
				Function.Call(Hash._GET_SCREEN_ACTIVE_RESOLUTION, width, height);

				return new Size(width.GetResult<int>(), height.GetResult<int>());
			}
		}

		public static int GameTime
		{
			get
			{
				return Function.Call<int>(Hash.GET_GAME_TIMER);
			}
		}
		public static float TimeScale
		{
			set
			{
				Function.Call(Hash.SET_TIME_SCALE, value);
			}
		}
		public static float FPS
		{
			get
			{
				return 1.0f / LastFrameTime;
			}
		}
		public static float LastFrameTime
		{
			get
			{
				return Function.Call<float>(Hash.GET_FRAME_TIME);
			}
		}

		public static GlobalCollection Globals { get; private set; }

		public static int MaxWantedLevel
		{
			get
			{
				return Function.Call<int>(Hash.GET_MAX_WANTED_LEVEL);
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value > 5)
				{
					value = 5;
				}

				Function.Call(Hash.SET_MAX_WANTED_LEVEL, value);
			}
		}
		public static float WantedMultiplier
		{
			set
			{
				Function.Call(Hash.SET_WANTED_LEVEL_MULTIPLIER, value);
			}
		}

		public static int RadarZoom
		{
			set
			{
				Function.Call(Hash.SET_RADAR_ZOOM, value);
			}
		}
		public static bool ShowsPoliceBlipsOnRadar
		{
			set
			{
				Function.Call(Hash.SET_POLICE_RADAR_BLIPS, value);
			}
		}

		public static RadioStation RadioStation
		{
			get
			{
				string radioName = Function.Call<string>(Hash.GET_PLAYER_RADIO_STATION_NAME);

				if (String.IsNullOrEmpty(radioName))
				{
					return RadioStation.RadioOff;
				}

				return (RadioStation)Array.IndexOf(_radioNames, radioName);
			}
			set
			{
				if (Enum.IsDefined(typeof(RadioStation), value) && value != RadioStation.RadioOff)
				{
					Function.Call(Hash.SET_RADIO_TO_STATION_NAME, _radioNames[(int)value]);
				}
				else
				{
					Function.Call(Hash.SET_RADIO_TO_STATION_NAME, string.Empty);
				}
			}
		}

		public static Player Player
		{
			get
			{
				int handle = Function.Call<int>(Hash.PLAYER_ID);

				if (ReferenceEquals(_cachedPlayer, null) || handle != _cachedPlayer.Handle)
				{
					_cachedPlayer = new Player(handle);
				}

				return _cachedPlayer;
			}
		}
		public static Ped PlayerPed
		{
			get
			{
				return Player.Character;
			}
		}

		public static bool Nightvision
		{
			get
			{
				return !Function.Call<bool>(Hash._IS_NIGHTVISION_INACTIVE);
			}
			set
			{
				Function.Call(Hash.SET_NIGHTVISION, value);
			}
		}
		public static bool ThermalVision
		{
			get
			{
				return Function.Call<bool>(Hash._IS_SEETHROUGH_ACTIVE);
			}
			set
			{
				Function.Call(Hash.SET_SEETHROUGH, value);
			}
		}

		public static bool MissionFlag
		{
			get
			{
				return Function.Call<bool>(Hash.GET_MISSION_FLAG);
			}
			set
			{
				Function.Call(Hash.SET_MISSION_FLAG, value);
			}
		}
		public static bool IsWaypointActive
		{
			get
			{
				return Function.Call<bool>(Hash.IS_WAYPOINT_ACTIVE);
			}
		}

		public static bool IsPaused
		{
			get
			{
				return Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE);
			}
			set
			{
				Function.Call(Hash.SET_PAUSE_MENU_ACTIVE, value);
			}
		}
		public static bool IsLoading
		{
			get
			{
				return Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE);
			}
		}
		public static bool IsScreenFadedIn
		{
			get
			{
				return Function.Call<bool>(Hash.IS_SCREEN_FADED_IN);
			}
		}
		public static bool IsScreenFadedOut
		{
			get
			{
				return Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT);
			}
		}
		public static bool IsScreenFadingIn
		{
			get
			{
				return Function.Call<bool>(Hash.IS_SCREEN_FADING_IN);
			}
		}
		public static bool IsScreenFadingOut
		{
			get
			{
				return Function.Call<bool>(Hash.IS_SCREEN_FADING_OUT);
			}
		}

		public static InputMode CurrentInputMode
		{
			get
			{
				return Function.Call<bool>(Hash._IS_INPUT_DISABLED, 2) ? InputMode.MouseAndKeyboard : InputMode.GamePad;
			}
		}

		public static bool IsKeyPressed(Keys key)
		{
			return ScriptDomain.CurrentDomain.IsKeyPressed(key);
		}
		public static bool IsControlPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, index, control);
		}
		public static bool IsControlJustPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, index, control);
		}
		public static bool IsControlJustReleased(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, index, control);
		}
		public static bool IsEnabledControlPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_PRESSED, index, control);
		}
		public static bool IsEnabledControlJustPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, index, control);
		}
		public static bool IsEnabledControlJustReleased(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, index, control);
		}
		public static bool IsDisabledControlPressed(int index, Control control)
		{
			return IsControlPressed(index, control) && !IsControlEnabled(index, control);
		}
		public static bool IsDisabledControlJustPressed(int index, Control control)
		{
			return IsControlJustPressed(index, control) && !IsControlEnabled(index, control);
		}
		public static bool IsDisabledControlJustReleased(int index, Control control)
		{
			return IsControlJustReleased(index, control) && !IsControlEnabled(index, control);
		}
		public static bool IsControlEnabled(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_ENABLED, index, control);
		}

		public static void EnableControlThisFrame(int index, Control control)
		{
			Function.Call(Hash.ENABLE_CONTROL_ACTION, index, control, true);
		}
		public static void DisableControlThisFrame(int index, Control control)
		{
			Function.Call(Hash.DISABLE_CONTROL_ACTION, index, control, true);
		}
		public static void DisableAllControlsThisFrame(int index)
		{
			Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, index);
		}
		public static void EnableAllControlsThisFrame(int index)
		{
			Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, index);
		}

		public static float GetControlNormal(int index, Control control)
		{
			return Function.Call<float>(Hash.GET_CONTROL_NORMAL, index, control);
		}
		public static float GetDisabledControlNormal(int index, Control control)
		{
			return Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, index, control);
		}
		public static int GetControlValue(int index, Control control)
		{
			return Function.Call<int>(Hash.GET_CONTROL_VALUE, index, control);
		}
		public static void SetControlNormal(int index, Control control, float value)
		{
			Function.Call(Hash._SET_CONTROL_NORMAL, index, control, value);
		}

		public static void Pause(bool value)
		{
			Function.Call(Hash.SET_GAME_PAUSED, value);
		}
		public static void PauseClock(bool value)
		{
			Function.Call(Hash.PAUSE_CLOCK, value);
		}

		public static void DoAutoSave()
		{
			Function.Call(Hash.DO_AUTO_SAVE);
		}
		public static void ShowSaveMenu()
		{
			Function.Call(Hash.SET_SAVE_MENU_ACTIVE, true);
		}

		public static void FadeScreenIn(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_IN, time);
		}
		public static void FadeScreenOut(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_OUT, time);
		}

		public static string GetGXTEntry(string entry)
		{
			return Function.Call<string>(Hash._GET_LABEL_TEXT, entry);
		}

		public static int GenerateHash(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return 0;
			}

			uint hash = 0;
			char[] chars = input.ToCharArray();

			//converts ascii uppercase to lowercase
			for (int i = 0, length = chars.Length; i < length; i++)
			{
				if (chars[i] >= 'A' && chars[i] <= 'Z')
				{
					chars[i] = (char)(chars[i] + 32);
				}
			}

			byte[] bytes = Encoding.UTF8.GetBytes(chars);

			for (int i = 0, length = bytes.Length; i < length; i++)
			{
				hash += bytes[i];
				hash += (hash << 10);
				hash ^= (hash >> 6);
			}

			hash += (hash << 3);
			hash ^= (hash >> 11);
			hash += (hash << 15);

			return (int)hash;
		}

		public static void PlaySound(string soundFile, string soundSet)
		{
			Audio.ReleaseSound(Audio.PlaySoundFrontend(soundFile, soundSet));
		}

		public static void PlayMusic(string musicFile)
		{
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, musicFile);
		}
		public static void StopMusic(string musicFile)
		{
			Function.Call(Hash.CANCEL_MUSIC_EVENT, musicFile);
		}

		public static string GetUserInput(int maxLength)
		{
			return GetUserInput(WindowTitle.FMMC_KEY_TIP8, string.Empty, maxLength);
		}
		public static string GetUserInput(string defaultText, int maxLength)
		{
			return GetUserInput(WindowTitle.FMMC_KEY_TIP8, defaultText, maxLength);
		}
		public static string GetUserInput(WindowTitle windowTitle, int maxLength)
		{
			return GetUserInput(windowTitle, string.Empty, maxLength);
		}
		public static string GetUserInput(WindowTitle windowTitle, string defaultText, int maxLength)
		{
			ScriptDomain.CurrentDomain.PauseKeyboardEvents(true);

			Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, true, windowTitle.ToString(), string.Empty, defaultText, string.Empty, string.Empty, string.Empty, maxLength + 1);

			while (Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD) == 0)
			{
				Script.Yield();
			}

			ScriptDomain.CurrentDomain.PauseKeyboardEvents(false);

			return Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
		}
	}
}
