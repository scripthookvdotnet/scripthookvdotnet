#include "World.hpp"
#include "Native.hpp"
#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Prop.hpp"
#include "Camera.hpp"

namespace GTA
{
	void World::Weather::set(GTA::Weather value)
	{
		Native::Function::Call(Native::Hash::SET_WEATHER_TYPE_NOW, sWeatherNames[static_cast<int>(value)]);
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

			const int amount = Native::Function::Call<int>(Native::Hash::GET_PED_NEARBY_PEDS, ped->Handle, handles, -1);

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

			const int amount = Native::Function::Call<int>(Native::Hash::GET_PED_NEARBY_VEHICLES, ped->Handle, handles, -1);

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
	Vehicle ^World::GetClosestVehicle(Math::Vector3 position, float radius)
	{
		return Native::Function::Call<Vehicle ^>(Native::Hash::GET_CLOSEST_VEHICLE, position.X, position.Y, position.Z, radius, 0, 70); // Last parameter still unknown.
	}
	float World::GetDistance(Math::Vector3 origin, Math::Vector3 destination)
	{
		return Native::Function::Call<float>(Native::Hash::GET_DISTANCE_BETWEEN_COORDS, origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z, 1);
	}
	float World::GetGroundZ(Math::Vector3 position)
	{
		float ground_z;
		Native::Function::Call(Native::Hash::GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, position.Z, &ground_z);
		return ground_z;
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

		const int handle = Native::Function::Call<int>(Native::Hash::CREATE_PED, 26, model.Hash, position.X, position.Y, position.Z, heading, false, false);

		if (handle == 0)
		{
			return nullptr;
		}

		return gcnew Ped(handle);
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

		const int handle = Native::Function::Call<int>(Native::Hash::CREATE_VEHICLE, model.Hash, position.X, position.Y, position.Z, heading, false, false);

		if (handle == 0)
		{
			return nullptr;
		}

		return gcnew Vehicle(handle);
	}
	Prop ^World::CreateProp(Model model, Math::Vector3 position, bool dynamic, bool placeOnGround)
	{
		if (placeOnGround)
			position.Z = World::GetGroundZ(position);

		const int handle = Native::Function::Call<int>(Native::Hash::CREATE_OBJECT, model.Hash, position.X, position.Y, position.Z, 1, 1, dynamic);

		if (handle == 0)
			return nullptr;
		
		return gcnew Prop(handle);
	}
	Prop ^World::CreateProp(Model model, Math::Vector3 position, Math::Vector3 rotation, bool dynamic, bool placeOnGround)
	{
		Prop ^p = World::CreateProp(model, position, dynamic, placeOnGround);
		
		if (System::Object::ReferenceEquals(p, nullptr))
		{
			return nullptr;
		}

		p->Rotation = rotation;

		return p;
	}

	void World::AddExplosion(Math::Vector3 position, ExplosionType type, float radius, float cameraShake)
	{
		Native::Function::Call(Native::Hash::ADD_EXPLOSION, position.X, position.Y, position.Z, static_cast<int>(type), radius, true, false, cameraShake);
	}
	void World::AddOwnedExplosion(Ped ^ped, Math::Vector3 position, ExplosionType type, float radius, float cameraShake)
	{
		Native::Function::Call(Native::Hash::ADD_OWNED_EXPLOSION, ped->Handle, position.X, position.Y, position.Z, static_cast<int>(type), radius, true, false, cameraShake);
	}

	Camera ^World::CreateCamera(Math::Vector3 position, Math::Vector3 rotation, float fov)
	{
		const int handle = Native::Function::Call<int>(Native::Hash::CREATE_CAM_WITH_PARAMS, "DEFAULT_SCRIPTED_CAMERA", position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, fov, 1, 2);

		if (handle == 0)
		{
			return nullptr;
		}

		return gcnew Camera(handle);
	}
	Camera ^World::RenderingCamera::get()
	{
		int handle = Native::Function::Call<int>(Native::Hash::GET_RENDERING_CAM);

		if (handle == 0)
		{
			return nullptr;
		}

		return gcnew Camera(handle);
	}
	void World::RenderingCamera::set(Camera ^renderingCamera)
	{
		if (renderingCamera == nullptr)
		{
			Native::Function::Call(Native::Hash::RENDER_SCRIPT_CAMS, false, 0, 3000, 1, 0);
		}
		else
		{
			renderingCamera->IsActive = true;

			Native::Function::Call(Native::Hash::RENDER_SCRIPT_CAMS, true, 0, 3000, 1, 0);
		}
	}
	void World::DestroyAllCameras()
	{
		Native::Function::Call(Native::Hash::DESTROY_ALL_CAMS, 0);
	}

	Blip ^World::CreateBlip(Math::Vector3 position)
	{
		const int handle = Native::Function::Call<int>(Native::Hash::ADD_BLIP_FOR_COORD, position.X, position.Y, position.Z);

		if (handle == 0)
		{
			return nullptr;
		}

		return gcnew Blip(handle);
	}
	Blip ^World::CreateBlip(Math::Vector3 position, float radius)
	{
		const int handle = Native::Function::Call<int>(Native::Hash::ADD_BLIP_FOR_RADIUS, position.X, position.Y, position.Z, radius);

		if (handle == 0)
		{
			return nullptr;
		}

		return gcnew Blip(handle);
	}

	int World::AddRelationShipGroup(System::String ^groupName)
	{
		int handle = 0;
		Native::Function::Call(Native::Hash::ADD_RELATIONSHIP_GROUP, groupName, &handle);

		return handle;
	}
	void World::RemoveRelationShipGroup(int group)
	{
		Native::Function::Call(Native::Hash::REMOVE_RELATIONSHIP_GROUP, group);
	}
	void World::SetRelationshipBetweenGroups(Relationship relationship, int group1, int group2)
	{
		Native::Function::Call(Native::Hash::SET_RELATIONSHIP_BETWEEN_GROUPS, static_cast<int>(relationship), group1, group2);
		Native::Function::Call(Native::Hash::SET_RELATIONSHIP_BETWEEN_GROUPS, static_cast<int>(relationship), group2, group1);
	}
	void World::ClearRelationshipBetweenGroups(Relationship relationship, int group1, int group2)
	{
		Native::Function::Call(Native::Hash::CLEAR_RELATIONSHIP_BETWEEN_GROUPS, static_cast<int>(relationship), group1, group2);
		Native::Function::Call(Native::Hash::CLEAR_RELATIONSHIP_BETWEEN_GROUPS, static_cast<int>(relationship), group2, group1);
	}
	Relationship World::GetRelationshipBetweenGroups(int group1, int group2)
	{
		return static_cast<Relationship>(Native::Function::Call<int>(Native::Hash::GET_RELATIONSHIP_BETWEEN_GROUPS, group1, group2));
	}
}