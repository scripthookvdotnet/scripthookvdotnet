#pragma once

#include "Vector3.hpp"

namespace GTA
{
	value class Model;
	ref class Vehicle;

	public enum class Weather
	{
		ExtraSunny,
		Clear,
		Clouds,
		Smog,
		Foggy,
		Overcast,
		Raining,
		ThunderStorm,
		Clearing,
		Neutral,
		Snowing,
		Blizzard,
		Snowlight,
		Christmas,
	};

	public ref class World sealed abstract
	{
	public:
		static property GTA::Weather Weather
		{
			void set(GTA::Weather value);
		}
		static property System::DateTime CurrentDate
		{
			System::DateTime get();
			void set(System::DateTime value);
		}
		static property System::TimeSpan CurrentDayTime
		{
			System::TimeSpan get();
			void set(System::TimeSpan value);
		}
		static property int GravityLevel
		{
			void set(int value);
		}

		static Vehicle ^CreateVehicle(Model model, Math::Vector3 position);
		static Vehicle ^CreateVehicle(Model model, Math::Vector3 position, float heading);
	};
}