#include "Game.hpp"
#include "Native.hpp"
#include "ScriptDomain.hpp"

#include "World.hpp"
#include "Raycast.hpp"

#include <Main.h>

#undef Yield

namespace GTA
{
	Global::Global(int index) : mAddress(getGlobalPtr(index))
	{
	}

	void Global::SetInt(int value)
	{
		*reinterpret_cast<int *>(this->mAddress) = value;
	}
	void Global::SetFloat(float value)
	{
		*reinterpret_cast<float *>(this->mAddress) = value;
	}
	void Global::SetString(System::String ^value)
	{
		const int size = System::Text::Encoding::UTF8->GetByteCount(value);

		System::Runtime::InteropServices::Marshal::Copy(System::Text::Encoding::UTF8->GetBytes(value), 0, static_cast<System::IntPtr>(this->mAddress), size);

		*(reinterpret_cast<char *>(this->mAddress) + size) = '\0';
	}
	void Global::SetVector3(Math::Vector3 value)
	{
		*(reinterpret_cast<float *>(this->mAddress) + 0) = value.X;
		*(reinterpret_cast<float *>(this->mAddress) + 1) = value.Y;
		*(reinterpret_cast<float *>(this->mAddress) + 2) = value.Z;
	}
	int Global::GetInt()
	{
		return *reinterpret_cast<int *>(this->mAddress);
	}
	float Global::GetFloat()
	{
		return *reinterpret_cast<float *>(this->mAddress);
	}
	System::String ^Global::GetString()
	{
		return gcnew System::String(reinterpret_cast<char *>(this->mAddress));
	}
	Math::Vector3 Global::GetVector3()
	{
		return Math::Vector3(*(reinterpret_cast<float *>(this->mAddress) + 0), *(reinterpret_cast<float *>(this->mAddress) + 1), *(reinterpret_cast<float *>(this->mAddress) + 2));
	}

	Global GlobalCollection::default::get(int index)
	{
		return Global(index);
	}
	void GlobalCollection::default::set(int index, Global value)
	{
		*getGlobalPtr(index) = *value.mAddress;
	}

	float Game::FPS::get()
	{
		return (1.0f / LastFrameTime);
	}
	int Game::GameTime::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_GAME_TIMER);
	}
	GlobalCollection ^Game::Globals::get()
	{
		if (ReferenceEquals(_globals, nullptr))
		{
			_globals = gcnew GlobalCollection();
		}

		return _globals;
	}
	bool Game::IsPaused::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PAUSE_MENU_ACTIVE);
	}
	void Game::IsPaused::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PAUSE_MENU_ACTIVE, value);
	}
	bool Game::IsWaypointActive::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_WAYPOINT_ACTIVE);
	}
	GTA::Language Game::Language::get()
	{
		return static_cast<GTA::Language>(Native::Function::Call<int>(Native::Hash::_GET_UI_LANGUAGE_ID));
	}
	float Game::LastFrameTime::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_FRAME_TIME);
	}
	int Game::MaxWantedLevel::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_MAX_WANTED_LEVEL);
	}
	void Game::MaxWantedLevel::set(int value)
	{
		if (value < 0) value = 0;
		if (value > 5) value = 5;

		Native::Function::Call(Native::Hash::SET_MAX_WANTED_LEVEL, value);
	}
	bool Game::MissionFlag::get()
	{
		return Native::Function::Call<bool>(Native::Hash::GET_MISSION_FLAG);
	}
	void Game::MissionFlag::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_MISSION_FLAG, value);
	}
	bool Game::Nightvision::get()
	{
		return !Native::Function::Call<bool>(Native::Hash::_IS_NIGHTVISION_INACTIVE);
	}
	void Game::Nightvision::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_NIGHTVISION, value);
	}
	GTA::Player ^GTA::Game::Player::get()
	{
		int playerHandle = Native::Function::Call<int>(Native::Hash::PLAYER_ID);

		if (ReferenceEquals(_cachedPlayer, nullptr) || playerHandle != _cachedPlayer->Handle)
		{
			_cachedPlayer = gcnew GTA::Player(playerHandle);
		}

		return _cachedPlayer;
	}
	void Game::RadarZoom::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_RADAR_ZOOM, value);
	}
	GTA::RadioStation Game::RadioStation::get()
	{
		return static_cast<GTA::RadioStation>(Native::Function::Call<int>(Native::Hash::GET_PLAYER_RADIO_STATION_INDEX));
	}
	void Game::RadioStation::set(GTA::RadioStation value)
	{
		Native::Function::Call(Native::Hash::SET_RADIO_TO_STATION_INDEX, static_cast<int>(value));
	}
	System::Drawing::Size Game::ScreenResolution::get()
	{
		int w, h;
		Native::Function::Call(Native::Hash::_GET_SCREEN_ACTIVE_RESOLUTION, &w, &h);

		return System::Drawing::Size(w, h);
	}
	bool Game::ThermalVision::get()
	{
		return Native::Function::Call<bool>(Native::Hash::_IS_SEETHROUGH_ACTIVE);
	}
	void Game::ThermalVision::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_SEETHROUGH, value);
	}
	void Game::TimeScale::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_TIME_SCALE, value);
	}
	GameVersion Game::Version::get()
	{
		if (_gameVersion == GameVersion::Unknown)
		{
			_gameVersion = static_cast<GameVersion>(getGameVersion() + 1);
		}

		return _gameVersion;
	}
	void Game::WantedMultiplier::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_WANTED_LEVEL_MULTIPLIER, value);
	}

	bool Game::IsKeyPressed(System::Windows::Forms::Keys key)
	{
		return ScriptDomain::CurrentDomain->IsKeyPressed(key);
	}
	bool Game::IsControlPressed(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_DISABLED_CONTROL_PRESSED, index, static_cast<int>(control));
	}
	bool Game::IsControlJustPressed(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_DISABLED_CONTROL_JUST_PRESSED, index, static_cast<int>(control));
	}
	bool Game::IsControlJustReleased(int index, Control control)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_DISABLED_CONTROL_JUST_RELEASED, index, static_cast<int>(control));
	}
	void Game::EnableControl(int index, Control control)
	{
		Native::Function::Call(Native::Hash::ENABLE_CONTROL_ACTION, index, static_cast<int>(control), true);
	}
	void Game::DisableControl(int index, Control control)
	{
		Native::Function::Call(Native::Hash::DISABLE_CONTROL_ACTION, index, static_cast<int>(control), true);
	}

	void Game::Pause(bool value)
	{
		Native::Function::Call(Native::Hash::SET_GAME_PAUSED, value);
	}
	void Game::PauseClock(bool value)
	{
		Native::Function::Call(Native::Hash::PAUSE_CLOCK, value);
	}
	void Game::DoAutoSave()
	{
		Native::Function::Call(Native::Hash::DO_AUTO_SAVE);
	}
	void Game::ShowSaveMenu()
	{
		Native::Function::Call(Native::Hash::SET_SAVE_MENU_ACTIVE, true);
	}
	void Game::FadeScreenIn(int time)
	{
		Native::Function::Call(Native::Hash::DO_SCREEN_FADE_IN, time);
	}
	void Game::FadeScreenOut(int time)
	{
		Native::Function::Call(Native::Hash::DO_SCREEN_FADE_OUT, time);
	}
	System::String ^Game::GetGXTEntry(System::String ^entry)
	{
		return Native::Function::Call<System::String ^>(Native::Hash::_GET_LABEL_TEXT, entry);
	}
	int Game::GenerateHash(System::String ^input)
	{
		return Native::Function::Call<int>(Native::Hash::GET_HASH_KEY, input);
	}


	void Game::PlaySound(System::String ^soundFile, System::String ^soundSet)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FRONTEND, -1, soundFile, soundSet, 0);
	}
	void Game::PlayMusic(System::String ^musicFile)
	{
		Native::Function::Call(Native::Hash::TRIGGER_MUSIC_EVENT, musicFile);
	}
	void Game::StopMusic(System::String ^musicFile)
	{
		Native::Function::Call(Native::Hash::CANCEL_MUSIC_EVENT, musicFile); //needs a general Game.StopMusic()
	}

	System::String ^Game::GetUserInput(int maxLength)
	{
		return GetUserInput(WindowTitle::FMMC_KEY_TIP8, "", maxLength);
	}
	System::String ^Game::GetUserInput(WindowTitle windowTitle, int maxLength)
	{
		return GetUserInput(windowTitle, "", maxLength);
	}
	System::String ^Game::GetUserInput(System::String^ defaultText, int maxLength)
	{
		return GetUserInput(WindowTitle::FMMC_KEY_TIP8, defaultText, maxLength);
	}
	System::String ^Game::GetUserInput(WindowTitle windowTitle, System::String^ defaultText, int maxLength)
	{
		ScriptDomain::CurrentDomain->PauseKeyboardEvents(true);

		Native::Function::Call(Native::Hash::DISPLAY_ONSCREEN_KEYBOARD, true, windowTitle.ToString(), "", defaultText, "", "", "", maxLength + 1);

		while (Native::Function::Call<int>(Native::Hash::UPDATE_ONSCREEN_KEYBOARD) == 0)
		{
			Script::Wait(0);
		}

		ScriptDomain::CurrentDomain->PauseKeyboardEvents(false);

		return Native::Function::Call<System::String ^>(Native::Hash::GET_ONSCREEN_KEYBOARD_RESULT);
	}
	InputMode Game::CurrentInputMode::get()
	{
		return Native::Function::Call<bool>(Native::Hash::_GET_LAST_INPUT_METHOD, 2) ? InputMode::MouseAndKeyboard : InputMode::GamePad;
	}
}