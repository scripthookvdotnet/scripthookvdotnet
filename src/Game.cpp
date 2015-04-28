#include "Game.hpp"
#include "Native.hpp"

namespace GTA
{
	Player ^GTA::Game::Player::get()
	{
		return gcnew GTA::Player(Native::Function::Call<int>(Native::Hash::PLAYER_ID));
	}
	int Game::GameTime::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_GAME_TIMER);
	}
	float Game::LastFrameTime::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_FRAME_TIME);
	}
	float Game::FPS::get()
	{
		return (1.0f / LastFrameTime);
	}
	bool Game::IsPaused::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PAUSE_MENU_ACTIVE);
	}
	void Game::IsPaused::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PAUSE_MENU_ACTIVE, value);
	}
	GTA::RadioStation Game::RadioStation::get()
	{
		return static_cast<GTA::RadioStation>(Native::Function::Call<int>(Native::Hash::GET_PLAYER_RADIO_STATION_INDEX));
	}
	void Game::RadioStation::set(GTA::RadioStation value)
	{
		Native::Function::Call(Native::Hash::SET_RADIO_TO_STATION_INDEX, static_cast<int>(value));
	}
	void Game::RadarZoom::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_RADAR_ZOOM, value);
	}
	void Game::TimeScale::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_TIME_SCALE, value);
	}
	void Game::WantedMultiplier::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_WANTED_LEVEL_MULTIPLIER, value);
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
}