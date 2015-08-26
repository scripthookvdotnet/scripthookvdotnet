#include "Game.hpp"
#include "Native.hpp"
#include "ScriptDomain.hpp"
#include "GameplayCamera.hpp"
#include "World.hpp"
#include "Raycast.hpp"
#include <Main.h>

#undef Yield

namespace GTA
{
	void Global::SetInteger(int value)
	{
		*(int*)Address = value;
	}
	void Global::SetFloat(float value)
	{
		*(float*)Address = value;
	}
	void Global::SetString(System::String ^value)
	{
		const int size = System::Text::Encoding::UTF8->GetByteCount(value);

		System::Runtime::InteropServices::Marshal::Copy(System::Text::Encoding::UTF8->GetBytes(value), 0, System::IntPtr(Address), size);
		*(char *)((char*)Address + size) = '\0';
	}
	void Global::SetVector3(Math::Vector3 value)
	{
		*(float*)Address = value.X;
		*(float*)(Address + 1) = value.Y;
		*(float*)(Address + 2) = value.Z;
	}
	int Global::GetInteger()
	{
		return *(int*)Address;
	}
	float Global::GetFloat()
	{
		return *(float*)Address;
	}
	System::String ^Global::GetString()
	{
		return gcnew System::String((char*)Address);
	}
	Math::Vector3 Global::GetVector3()
	{
		return Math::Vector3(*(float*)Address, *(float*)(Address + 1), *(float*)(Address + 2));
	}
	Global::Global(int index)
	{
		Address = getGlobalPtr(index);
	}
	Global GlobalCollection::default::get(int index)
	{
		return Global(index);
	}
	void GlobalCollection::default::set(int index, Global value)
	{
		*getGlobalPtr(index) = *value.Address;
	}
	float Game::FPS::get()
	{
		return (1.0f / LastFrameTime);
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
		return !Native::Function::Call<bool>(Native::Hash::_GET_IS_NIGHTVISION_INACTIVE);
	}
	void Game::Nightvision::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_NIGHTVISION, value);
	}
	GTA::Player ^GTA::Game::Player::get()
	{
		return Native::Function::Call<GTA::Player ^>(Native::Hash::PLAYER_ID);
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
	void Game::TimeScale::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_TIME_SCALE, value);
	}
	GameVersion Game::Version::get()
	{
		if (sGameVersion == GameVersion::Unknown)
		{
			sGameVersion = static_cast<GameVersion>(getGameVersion() + 1);
		}

		return sGameVersion;
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

	void Game::Pause(bool value)
	{
		IsPaused = value;
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
		ScriptDomain::PauseKeyEvents();
		Native::Function::Call(Native::Hash::DISPLAY_ONSCREEN_KEYBOARD, true, windowTitle.ToString(), "", defaultText, "", "", "", maxLength + 1);

		while (Native::Function::Call<int>(Native::Hash::UPDATE_ONSCREEN_KEYBOARD) == 0)
		{
			Script::Yield();
		}
		ScriptDomain::ResumeKeyEvents();
		return Native::Function::Call<System::String ^>(Native::Hash::GET_ONSCREEN_KEYBOARD_RESULT);
	}

	Math::Vector3 Game::GetWaypointPosition()
	{
		if (!IsWaypointActive)
		{
			return Math::Vector3::Zero;
		}

		Math::Vector3 position;
		bool blipFound = false;
		int blipIterator = Native::Function::Call<int>(Native::Hash::_GET_BLIP_INFO_ID_ITERATOR);

		for (int i = Native::Function::Call<int>(Native::Hash::GET_FIRST_BLIP_INFO_ID, blipIterator); Native::Function::Call<bool>(Native::Hash::DOES_BLIP_EXIST, i) != 0; i = Native::Function::Call<int>(Native::Hash::GET_NEXT_BLIP_INFO_ID, blipIterator))
		{
			if (Native::Function::Call<int>(Native::Hash::GET_BLIP_INFO_ID_TYPE, i) == 4)
			{
				position = Native::Function::Call<Math::Vector3>(Native::Hash::GET_BLIP_INFO_ID_COORD, i);
				blipFound = true;
				break;
			}

			if (blipFound)
			{
				bool groundFound = false;
				float height = 0.0f;

				for (int i = 800; i >= 0; i -= 50)
				{
					if (Native::Function::Call<bool>(Native::Hash::GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, static_cast<float>(i), &height))
					{
						groundFound = true;
						position.Z = height;
						break;
					}

					Script::Wait(100);
				}

				if (!groundFound)
				{
					position.Z = 1000.0f;
				}
			}
		}

		return position;
	}
	RaycastResult Game::GetCrosshairCoordinates()
	{
		return World::Raycast(GameplayCamera::Position, GameplayCamera::Direction, 1000.0f, IntersectOptions::Everything);
	}
	GlobalCollection ^Game::Globals::get()
	{
		if (_globals == nullptr)
		{
			_globals = gcnew GlobalCollection();
		}
		return _globals;
	}
}