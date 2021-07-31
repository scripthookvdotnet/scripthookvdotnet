//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
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
		#endregion

		public static float FPS => 1.0f / LastFrameTime;
		public static float LastFrameTime => Function.Call<float>(Hash.GET_FRAME_TIME);

		public static Size ScreenResolution
		{
			get
			{
				int w, h;
				unsafe { Function.Call(Hash._GET_SCREEN_ACTIVE_RESOLUTION, &w, &h); }
				return new Size(w, h);
			}
		}

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

		public static Language Language => Function.Call<Language>(Hash._GET_UI_LANGUAGE_ID);

		public static GameVersion Version => (GameVersion)(SHVDN.NativeMemory.GetGameVersion() + 1);

		public static GlobalCollection Globals { get; private set; } = new GlobalCollection();

		public static void Pause(bool value)
		{
			Function.Call(Hash.SET_GAME_PAUSED, value);
		}
		public static void PauseClock(bool value)
		{
			Function.Call(Hash.PAUSE_CLOCK, value);
		}

		public static bool IsPaused
		{
			get => Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE);
			set => Function.Call(Hash.SET_PAUSE_MENU_ACTIVE, value);
		}

		public static void DoAutoSave()
		{
			Function.Call(Hash.DO_AUTO_SAVE);
		}

		public static void ShowSaveMenu()
		{
			Function.Call(Hash.SET_SAVE_MENU_ACTIVE, true);
		}

		public static bool IsLoading => Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE);
		public static bool IsScreenFadedIn => Function.Call<bool>(Hash.IS_SCREEN_FADED_IN);
		public static bool IsScreenFadedOut => Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT);
		public static bool IsScreenFadingIn => Function.Call<bool>(Hash.IS_SCREEN_FADING_IN);
		public static bool IsScreenFadingOut => Function.Call<bool>(Hash.IS_SCREEN_FADING_OUT);

		public static void FadeScreenIn(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_IN, time);
		}
		public static void FadeScreenOut(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_OUT, time);
		}

		public static bool IsWaypointActive => Function.Call<bool>(Hash.IS_WAYPOINT_ACTIVE);

		public static InputMode CurrentInputMode => Function.Call<bool>(Hash._GET_LAST_INPUT_METHOD, 2) ? InputMode.MouseAndKeyboard : InputMode.GamePad;

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
					Function.Call(Hash.SET_RADIO_TO_STATION_NAME, "OFF");
				}
			}
		}

		public static int GameTime => Function.Call<int>(Hash.GET_GAME_TIMER);
		public static int FrameCount => Function.Call<int>(Hash.GET_FRAME_COUNT);

		public static float TimeScale
		{
			get => SHVDN.NativeMemory.TimeScale;
			set => Function.Call(Hash.SET_TIME_SCALE, value);
		}

		public static int RadarZoom
		{
			set => Function.Call(Hash.SET_RADAR_ZOOM, value);
		}
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

		public static bool MissionFlag
		{
			get => Function.Call<bool>(Hash.GET_MISSION_FLAG);
			set => Function.Call(Hash.SET_MISSION_FLAG, value);
		}

		public static bool ShowsPoliceBlipsOnRadar
		{
			set => Function.Call(Hash.SET_POLICE_RADAR_BLIPS, value);
		}

		public static bool Nightvision
		{
			get => !Function.Call<bool>(Hash._IS_NIGHTVISION_INACTIVE);
			set => Function.Call(Hash.SET_NIGHTVISION, value);
		}
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

		public static int GetControlValue(int index, Control control)
		{
			return Function.Call<int>(Hash.GET_CONTROL_VALUE, index, (int)control);
		}
		public static float GetControlNormal(int index, Control control)
		{
			return Function.Call<float>(Hash.GET_CONTROL_NORMAL, index, (int)control);
		}
		public static float GetDisabledControlNormal(int index, Control control)
		{
			return Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, index, (int)control);
		}

		public static void SetControlNormal(int index, Control control, float value)
		{
			Function.Call(Hash._SET_CONTROL_NORMAL, index, (int)control, value);
		}

		public static bool IsKeyPressed(Keys key)
		{
			return SHVDN.ScriptDomain.CurrentDomain.IsKeyPressed(key);
		}
		public static bool IsControlPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, index, (int)control);
		}
		public static bool IsControlJustPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, index, (int)control);
		}
		public static bool IsControlJustReleased(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, index, (int)control);
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
		public static bool IsEnabledControlPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_PRESSED, index, (int)control);
		}
		public static bool IsEnabledControlJustPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, index, (int)control);
		}
		public static bool IsEnabledControlJustReleased(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, index, (int)control);
		}

		public static bool IsControlEnabled(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_ENABLED, index, (int)control);
		}
		[Obsolete("The Game.EnableControl is obsolete, use Game.EnableControlThisFrame instead.")]
		public static void EnableControl(int index, Control control)
		{
			EnableControlThisFrame(index, control);
		}
		public static void EnableControlThisFrame(int index, Control control)
		{
			Function.Call(Hash.ENABLE_CONTROL_ACTION, index, (int)control, true);
		}
		[Obsolete("The Game.DisableControl is obsolete, use Game.DisableControlThisFrame instead.")]
		public static void DisableControl(int index, Control control)
		{
			DisableControlThisFrame(index, control);
		}
		public static void DisableControlThisFrame(int index, Control control)
		{
			Function.Call(Hash.DISABLE_CONTROL_ACTION, index, (int)control, true);
		}
		public static void EnableAllControlsThisFrame(int index)
		{
			Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, index);
		}
		public static void DisableAllControlsThisFrame(int index)
		{
			Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, index);
		}

		public static void PlayMusic(string musicFile)
		{
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, musicFile);
		}
		public static void PlaySound(string soundFile, string soundSet)
		{
			Audio.ReleaseSound(Audio.PlaySoundFrontend(soundFile, soundSet));
		}
		public static void StopMusic(string musicFile)
		{
			Function.Call(Hash.CANCEL_MUSIC_EVENT, musicFile); // Needs a general Game.StopMusic()
		}

		public static int GenerateHash(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return 0;
			}

			return unchecked((int)SHVDN.NativeMemory.GetHashKey(input));
		}

		public static string GetGXTEntry(string entry)
		{
			return Function.Call<string>(Hash._GET_LABEL_TEXT, entry);
		}
	}
}
