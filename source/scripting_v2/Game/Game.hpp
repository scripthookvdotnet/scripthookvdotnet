#pragma once

#include "Player.hpp"
#include "Controls.hpp"
#include "Vector3.hpp"

namespace GTA
{
	public enum class GameVersion
	{
		Unknown = 0,

		VER_1_0_335_2_STEAM,
		VER_1_0_335_2_NOSTEAM,
		VER_1_0_350_1_STEAM,
		VER_1_0_350_2_NOSTEAM,
		VER_1_0_372_2_STEAM,
		VER_1_0_372_2_NOSTEAM,
		VER_1_0_393_2_STEAM,
		VER_1_0_393_2_NOSTEAM,
		VER_1_0_393_4_STEAM,
		VER_1_0_393_4_NOSTEAM,
		VER_1_0_463_1_STEAM,
		VER_1_0_463_1_NOSTEAM,
		VER_1_0_505_2_STEAM,
		VER_1_0_505_2_NOSTEAM,
		VER_1_0_573_1_STEAM,
		VER_1_0_573_1_NOSTEAM,
		VER_1_0_617_1_STEAM,
		VER_1_0_617_1_NOSTEAM,
		VER_1_0_678_1_STEAM,
		VER_1_0_678_1_NOSTEAM,
		VER_1_0_757_2_STEAM,
		VER_1_0_757_2_NOSTEAM,
		VER_1_0_757_3_STEAM,
		VER_1_0_757_4_NOSTEAM,
		VER_1_0_791_2_STEAM,
		VER_1_0_791_2_NOSTEAM,
		VER_1_0_877_1_STEAM,
		VER_1_0_877_1_NOSTEAM,
		VER_1_0_944_2_STEAM,
		VER_1_0_944_2_NOSTEAM,
		VER_1_0_1011_1_STEAM,
		VER_1_0_1011_1_NOSTEAM,
		VER_1_0_1032_1_STEAM,
		VER_1_0_1032_1_NOSTEAM,
		VER_1_0_1103_2_STEAM,
		VER_1_0_1103_2_NOSTEAM,
		VER_1_0_1180_2_STEAM,
		VER_1_0_1180_2_NOSTEAM,
		VER_1_0_1290_1_STEAM,
		VER_1_0_1290_1_NOSTEAM,
		VER_1_0_1365_1_STEAM,
		VER_1_0_1365_1_NOSTEAM,
		VER_1_0_1493_0_STEAM,
		VER_1_0_1493_0_NOSTEAM,
		VER_1_0_1493_1_STEAM,
		VER_1_0_1493_1_NOSTEAM,
		VER_1_0_1604_0_STEAM,
		VER_1_0_1604_0_NOSTEAM,
		VER_1_0_1604_1_STEAM,
		VER_1_0_1604_1_NOSTEAM,
		///<summary>This value also represents the exe version 1.0.1734.0 for Steam version. 1.0.1737.0 basically works in the same way as 1.0.1734.0 but with bug fixes.</summary>
		VER_1_0_1737_0_STEAM,
		///<summary>This value also represents the exe version 1.0.1734.0 for non-Steam version. 1.0.1737.0 basically works in the same way as 1.0.1734.0 but with bug fixes.</summary>
		VER_1_0_1737_0_NOSTEAM
	};
	public enum class Language
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
		Mexican,
		ChineseSimplified
	};
	public enum class RadioStation
	{
		Unknown = -1,
		LosSantosRockRadio = 0,
		NonStopPopFM = 1,
		RadioLosSantos = 2,
		ChannelX = 3,
		WestCoastTalkRadio = 4,
		RebelRadio = 5,
		SoulwaxFM = 6,
		EastLosFM = 7,
		WestCoastClassics = 8,
		BlaineCountyRadio = 9,
		TheBlueArk = 10,
		WorldWideFM = 11,
		FlyloFM = 12,
		TheLowdown = 13,
		RadioMirrorPark = 14,
		Space = 15,
		VinewoodBoulevardRadio = 16,
		SelfRadio = 17,
		TheLab = 18,
		BlondedLosSantos = 19,
		LosSantosUndergroundRadio = 20,
		RadioOff = 255,
	};
	public enum class WindowTitle
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
		PM_NAME_CHALL,
	};
	public enum class InputMode
	{
		MouseAndKeyboard,
		GamePad
	};

	public value class Global
	{
	public:
		property unsigned long long *MemoryAddress
		{
			unsigned long long *get();
		}

		void SetInt(int value);
		void SetFloat(float value);
		void SetString(System::String ^value);
		void SetVector3(Math::Vector3 Value);
		int GetInt();
		float GetFloat();
		System::String ^GetString();
		Math::Vector3 GetVector3();

	internal:
		Global(int index);

		System::UInt64 *mAddress;
	};
	public ref class GlobalCollection
	{
	public:
		property Global default[int]
		{
			Global get(int index);
			void set(int index, Global value);
		}

	internal:
		GlobalCollection() {}
	};

	public ref class Game sealed abstract
	{
	public:
		static property InputMode CurrentInputMode
		{
			InputMode get();
		}
		static property float FPS
		{
			float get();
		}
		static property int GameTime
		{
			int get();
		}
		static property GlobalCollection ^Globals
		{
			GlobalCollection ^get();
		}
		static property bool IsPaused
		{
			bool get();
			void set(bool value);
		}
		static property bool IsLoading
		{
			bool get();
		}
		static property bool IsScreenFadedIn
		{
			bool get();
		}
		static property bool IsScreenFadedOut
		{
			bool get();
		}
		static property bool IsScreenFadingIn
		{
			bool get();
		}
		static property bool IsScreenFadingOut
		{
			bool get();
		}
		static property bool IsWaypointActive
		{
			bool get();
		}
		static property GTA::Language Language
		{
			GTA::Language get();
		}
		static property float LastFrameTime
		{
			float get();
		}
		static property int MaxWantedLevel
		{
			int get();
			void set(int value);
		}
		static property bool MissionFlag
		{
			bool get();
			void set(bool value);
		}
		static property bool Nightvision
		{
			bool get();
			void set(bool value);
		}
		static property GTA::Player ^Player
		{
			GTA::Player ^get();
		}
		static property int RadarZoom
		{
			void set(int value);
		}
		static property GTA::RadioStation RadioStation
		{
			GTA::RadioStation get();
			void set(GTA::RadioStation value);
		}
		static property System::Drawing::Size ScreenResolution
		{
			System::Drawing::Size get();
		}
		static property bool ShowsPoliceBlipsOnRadar
		{
			void set(bool value);
		}
		static property bool ThermalVision
		{
			bool get();
			void set(bool value);
		}
		static property float TimeScale
		{
			void set(float value);
		}
		static property GameVersion Version
		{
			GameVersion get();
		}
		static property float WantedMultiplier
		{
			void set(float value);
		}

		static bool IsKeyPressed(System::Windows::Forms::Keys key);
		static bool IsControlPressed(int index, Control control);
		static bool IsControlJustPressed(int index, Control control);
		static bool IsControlJustReleased(int index, Control control);
		static bool IsEnabledControlPressed(int index, Control control);
		static bool IsEnabledControlJustPressed(int index, Control control);
		static bool IsEnabledControlJustReleased(int index, Control control);
		static bool IsDisabledControlPressed(int index, Control control);
		static bool IsDisabledControlJustPressed(int index, Control control);
		static bool IsDisabledControlJustReleased(int index, Control control);
		static bool IsControlEnabled(int index, Control control);
		[System::ObsoleteAttribute("The Game.EnableControl is obsolete, use Game.EnableControlThisFrame instead.")]
		static void EnableControl(int index, Control control);
		[System::ObsoleteAttribute("The Game.DisableControl is obsolete, use Game.DisableControlThisFrame instead.")]
		static void DisableControl(int index, Control control);
		static void EnableControlThisFrame(int index, Control control);
		static void DisableControlThisFrame(int index, Control control);
		static void DisableAllControlsThisFrame(int index);
		static void EnableAllControlsThisFrame(int index);
		static float GetControlNormal(int index, Control control);
		static float GetDisabledControlNormal(int index, Control control);
		static int GetControlValue(int index, Control control);
		static void SetControlNormal(int index, Control control, float value);

		static void Pause(bool value);
		static void PauseClock(bool value);
		static void DoAutoSave();
		static void ShowSaveMenu();
		static void FadeScreenIn(int time);
		static void FadeScreenOut(int time);
		static System::String ^GetGXTEntry(System::String ^entry);
		static int GenerateHash(System::String ^input);

		static void PlaySound(System::String ^soundFile, System::String ^soundSet);
		static void PlayMusic(System::String ^musicFile);
		static void StopMusic(System::String ^musicFile);

		static System::String ^GetUserInput(int maxLength);
		static System::String ^GetUserInput(WindowTitle windowTitle, int maxLength);
		static System::String ^GetUserInput(System::String^ defaultText, int maxLength);
		static System::String ^GetUserInput(WindowTitle windowTitle, System::String^ defaultText, int maxLength);

	internal:
		static initonly array<System::String ^> ^_radioNames = { "RADIO_01_CLASS_ROCK", "RADIO_02_POP", "RADIO_03_HIPHOP_NEW", "RADIO_04_PUNK", "RADIO_05_TALK_01", "RADIO_06_COUNTRY", "RADIO_07_DANCE_01", "RADIO_08_MEXICAN", "RADIO_09_HIPHOP_OLD", "RADIO_11_TALK_02", "RADIO_12_REGGAE", "RADIO_13_JAZZ", "RADIO_14_DANCE_02", "RADIO_15_MOTOWN", "RADIO_16_SILVERLAKE", "RADIO_17_FUNK", "RADIO_18_90S_ROCK", "RADIO_19_USER", "RADIO_20_THELAB", "RADIO_21_DLC_XM17", "RADIO_22_DLC_BATTLE_MIX1_RADIO", "RADIO_OFF" };

	private:
		static GameVersion _gameVersion = GameVersion::Unknown;
		static GlobalCollection ^_globals = nullptr;
		static GTA::Player ^_cachedPlayer = nullptr;
	};
}
