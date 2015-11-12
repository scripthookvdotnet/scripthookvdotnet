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
	};
	public enum class RadioStation
	{
		LosSantosRockRadio,
		NonStopPopFM,
		RadioLosSantos,
		ChannelX,
		WestCoastTalkRadio,
		RebelRadio,
		SoulwaxFM,
		EastLosFM,
		WestCoastClassics,
		TheBlueArk,
		WorldWideFM,
		FlyloFM,
		TheLowdown,
		TheLab,
		RadioMirrorPark,
		Space,
		VinewoodBoulevardRadio,
		SelfRadio,
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
		static void EnableControl(int index, Control control);
		static void DisableControl(int index, Control control);

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

	private:
		static GameVersion _gameVersion = GameVersion::Unknown;
		static GlobalCollection ^_globals = nullptr;
		static GTA::Player ^_cachedPlayer = nullptr;
	};
}