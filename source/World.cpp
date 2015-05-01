#include "World.hpp"
#include "Native.hpp"
#include "Ped.hpp"
#include "Vehicle.hpp"

namespace GTA
{
	void World::Weather::set(GTA::Weather value)
	{
		array<System::String ^> ^names = { "EXTRASUNNY", "CLEAR", "CLOUDS", "SMOG", "FOGGY", "OVERCAST", "RAIN", "THUNDER", "CLEARING", "NEUTRAL", "SNOW", "BLIZZARD", "SNOWLIGHT", "XMAS" };

		Native::Function::Call(Native::Hash::SET_WEATHER_TYPE_NOW, names[static_cast<int>(value)]);
	}
	System::DateTime World::CurrentDate::get()
	{
		int year = Native::Function::Call<int>(Native::Hash::GET_CLOCK_YEAR);
		int month = Native::Function::Call<int>(Native::Hash::GET_CLOCK_MONTH);
		int day = Native::Function::Call<int>(Native::Hash::GET_CLOCK_DAY_OF_MONTH);
		int hour = Native::Function::Call<int>(Native::Hash::GET_CLOCK_HOURS);
		int minute = Native::Function::Call<int>(Native::Hash::GET_CLOCK_MINUTES);
		int second = Native::Function::Call<int>(Native::Hash::GET_CLOCK_SECONDS);

		return System::DateTime(year, month, day, hour, minute, second);
	}
	void World::CurrentDate::set(System::DateTime value)
	{
		Native::Function::Call(Native::Hash::SET_CLOCK_DATE, value.Year, value.Month, value.Day);
		Native::Function::Call(Native::Hash::SET_CLOCK_TIME, value.Hour, value.Minute, value.Second);
	}
	System::TimeSpan World::CurrentDayTime::get()
	{
		int hour = Native::Function::Call<int>(Native::Hash::GET_CLOCK_HOURS);
		int minute = Native::Function::Call<int>(Native::Hash::GET_CLOCK_MINUTES);
		int second = Native::Function::Call<int>(Native::Hash::GET_CLOCK_SECONDS);

		return System::TimeSpan(hour, minute, second);
	}
	void World::CurrentDayTime::set(System::TimeSpan value)
	{
		Native::Function::Call(Native::Hash::SET_CLOCK_TIME, value.Hours, value.Minutes, value.Seconds);
	}
	void World::GravityLevel::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_GRAVITY_LEVEL, value);
	}

	array<Ped ^> ^World::GetNearbyPeds(Ped ^ped, float radius)
	{
		return GetNearbyPeds(ped, radius, 10000);
	}
	array<Ped ^> ^World::GetNearbyPeds(Ped ^ped, float radius, int maxAmount)
	{
		const Math::Vector3 position = ped->Position;
		System::Collections::Generic::List<Ped ^> ^result = gcnew System::Collections::Generic::List<Ped ^>();
		int *handles = new int[maxAmount * 2 + 2];

		try
		{
			handles[0] = maxAmount;

			const int amount = Native::Function::Call<int>(Native::Hash::GET_PED_NEARBY_PEDS, ped->ID, handles, -1);

			for (int i = 0; i < amount; ++i)
			{
				const int index = i * 2 + 2;

				if (handles[index] != 0 && Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, handles[index]))
				{
					Ped ^currped = gcnew Ped(handles[index]);

					if (Math::Vector3::Subtract(position, currped->Position).LengthSquared() < radius * radius)
					{
						result->Add(currped);
					}
				}
			}
		}
		finally
		{
			delete[] handles;
		}

		return result->ToArray();
	}
	array<Vehicle ^> ^World::GetNearbyVehicles(Ped ^ped, float radius)
	{
		return GetNearbyVehicles(ped, radius, 10000);
	}
	array<Vehicle ^> ^World::GetNearbyVehicles(Ped ^ped, float radius, int maxAmount)
	{
		const Math::Vector3 position = ped->Position;
		System::Collections::Generic::List<Vehicle ^> ^result = gcnew System::Collections::Generic::List<Vehicle ^>();
		int *handles = new int[maxAmount * 2 + 2];

		try
		{
			handles[0] = maxAmount;

			const int amount = Native::Function::Call<int>(Native::Hash::GET_PED_NEARBY_VEHICLES, ped->ID, handles, -1);

			for (int i = 0; i < amount; ++i)
			{
				const int index = i * 2 + 2;

				if (handles[index] != 0 && Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, handles[index]))
				{
					Vehicle ^vehicle = gcnew Vehicle(handles[index]);

					if (Math::Vector3::Subtract(position, vehicle->Position).LengthSquared() < radius * radius)
					{
						result->Add(vehicle);
					}
				}
			}
		}
		finally
		{
			delete[] handles;
		}

		return result->ToArray();
	}

	Ped ^World::CreatePed(Model model, Math::Vector3 position)
	{
		return CreatePed(model, position, 0.0f);
	}
	Ped ^World::CreatePed(Model model, Math::Vector3 position, float heading)
	{
		if (!model.IsPed || !model.Request(1000))
		{
			return nullptr;
		}

		const int id = Native::Function::Call<int>(Native::Hash::CREATE_PED, 26, model.Hash, position.X, position.Y, position.Z, heading, false, false);

		if (id == 0)
		{
			return nullptr;
		}

		return gcnew Ped(id);
	}
	Vehicle ^World::CreateVehicle(Model model, Math::Vector3 position)
	{
		return CreateVehicle(model, position, 0.0f);
	}
	Vehicle ^World::CreateVehicle(Model model, Math::Vector3 position, float heading)
	{
		if (!model.IsVehicle || !model.Request(1000))
		{
			return nullptr;
		}

		const int id = Native::Function::Call<int>(Native::Hash::CREATE_VEHICLE, model.Hash, position.X, position.Y, position.Z, heading, false, false);

		if (id == 0)
		{
			return nullptr;
		}

		return gcnew Vehicle(id);
	}
}