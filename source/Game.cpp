#include "Game.hpp"
#include "Native.hpp"
#include "ScriptDomain.hpp"
#include "MemoryAccess.hpp"

namespace GTA
{
	float Game::FPS::get()
	{
		return (1.0f / LastFrameTime);
	}
	GameVersion Game::Version::get()
	{
		if (gVersion != GameVersion::UnknownVersion)
			return gVersion;

		gVersion = MemoryAccess::GetGameVersion();

		return gVersion;
	}
	int Game::GameTime::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_GAME_TIMER);
	}
	bool Game::IsPaused::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PAUSE_MENU_ACTIVE);
	}
	void Game::IsPaused::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PAUSE_MENU_ACTIVE, value);
	}
	float Game::LastFrameTime::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_FRAME_TIME);
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
		return Native::Function::Call<bool>(Native::Hash::_GET_IS_NIGHTVISION_ACTIVE);
	}
	void Game::Nightvision::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_NIGHTVISION, value);
	}
	Player ^GTA::Game::Player::get()
	{
		return gcnew GTA::Player(Native::Function::Call<int>(Native::Hash::PLAYER_ID));
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
	void Game::TimeScale::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_TIME_SCALE, value);
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

	void Game::Pause()
	{
		IsPaused = true;
	}
	void Game::Unpause()
	{
		IsPaused = false;
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
		return Native::Function::Call<System::String ^>(Native::Hash::_0x7B5280EBA9840C72, entry);
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
		return GetUserInput("", maxLength);
	}
	System::String ^Game::GetUserInput(System::String ^startText, int maxLength)
	{
		Native::Function::Call(Native::Hash::DISPLAY_ONSCREEN_KEYBOARD, true, startText, "", "", "", "", "", maxLength);

		while (Native::Function::Call<int>(Native::Hash::UPDATE_ONSCREEN_KEYBOARD) == 0)
		{
			Script::Yield();
		}

		return Native::Function::Call<System::String ^>(Native::Hash::GET_ONSCREEN_KEYBOARD_RESULT);
	}
}