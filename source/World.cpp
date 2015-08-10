#include "World.hpp"
#include "Native.hpp"
#include "NativeMemory.hpp"

#include "Blip.hpp"
#include "Camera.hpp"
#include "Ped.hpp"
#include "Prop.hpp"
#include "Raycast.hpp"
#include "Rope.hpp"
#include "Vehicle.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	DateTime World::CurrentDate::get()
	{
		int year = Native::Function::Call<int>(Native::Hash::GET_CLOCK_YEAR);
		int month = Native::Function::Call<int>(Native::Hash::GET_CLOCK_MONTH);
		int day = Native::Function::Call<int>(Native::Hash::GET_CLOCK_DAY_OF_MONTH);
		int hour = Native::Function::Call<int>(Native::Hash::GET_CLOCK_HOURS);
		int minute = Native::Function::Call<int>(Native::Hash::GET_CLOCK_MINUTES);
		int second = Native::Function::Call<int>(Native::Hash::GET_CLOCK_SECONDS);

		return DateTime(year, month, day, hour, minute, second);
	}
	void World::CurrentDate::set(DateTime value)
	{
		Native::Function::Call(Native::Hash::SET_CLOCK_DATE, value.Year, value.Month, value.Day);
		Native::Function::Call(Native::Hash::SET_CLOCK_TIME, value.Hour, value.Minute, value.Second);
	}
	TimeSpan World::CurrentDayTime::get()
	{
		int hour = Native::Function::Call<int>(Native::Hash::GET_CLOCK_HOURS);
		int minute = Native::Function::Call<int>(Native::Hash::GET_CLOCK_MINUTES);
		int second = Native::Function::Call<int>(Native::Hash::GET_CLOCK_SECONDS);

		return TimeSpan(hour, minute, second);
	}
	void World::CurrentDayTime::set(TimeSpan value)
	{
		Native::Function::Call(Native::Hash::SET_CLOCK_TIME, value.Hours, value.Minutes, value.Seconds);
	}
	void World::GravityLevel::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_GRAVITY_LEVEL, value);
	}
	Camera ^World::RenderingCamera::get()
	{
		return Native::Function::Call<Camera ^>(Native::Hash::GET_RENDERING_CAM);
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
	void World::Weather::set(GTA::Weather value)
	{
		Native::Function::Call(Native::Hash::SET_WEATHER_TYPE_NOW, sWeatherNames[static_cast<int>(value)]);
	}

	array<Blip ^> ^World::GetActiveBlips()
	{
		Collections::Generic::List<Blip ^> ^res = gcnew Collections::Generic::List<Blip ^>();

		int it = Native::Function::Call<int>(Native::Hash::_GET_BLIP_INFO_ID_ITERATOR);
		int handle = Native::Function::Call<int>(Native::Hash::GET_FIRST_BLIP_INFO_ID, it);

		while (Native::Function::Call<bool>(Native::Hash::DOES_BLIP_EXIST, handle))
		{
			res->Add(gcnew Blip(handle));

			handle = Native::Function::Call<int>(Native::Hash::GET_NEXT_BLIP_INFO_ID, it);
		}

		return res->ToArray();
	}
	array<Ped ^> ^World::GetAllPeds()
	{
		List<Ped ^> ^list = gcnew List<Ped ^>();
		array<int> ^entities = MemoryAccess::GetEntityHandleList();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entities[i]) && Native::Function::Call<bool>(Native::Hash::IS_ENTITY_A_PED, entities[i]))
			{
				list->Add(gcnew Ped(entities[i]));
			}
		}

		return list->ToArray();
	}
	array<Ped ^> ^World::GetNearbyPeds(Ped ^ped, float radius)
	{
		return GetNearbyPeds(ped, radius, 10000);
	}
	array<Ped ^> ^World::GetNearbyPeds(Ped ^ped, float radius, int maxAmount)
	{
		const Math::Vector3 position = ped->Position;
		Collections::Generic::List<Ped ^> ^result = gcnew Collections::Generic::List<Ped ^>();
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
	array<Ped ^> ^World::GetNearbyPeds(Math::Vector3 position, float radius)
	{
		auto handles = GetAllPeds();
		auto resultHandles = gcnew List<Ped ^>();

		for (int i = 0; i < handles->Length; i++)
		{
			if (handles[i]->Position.DistanceTo(position) <= radius)
				resultHandles->Add(handles[i]);
		}

		return resultHandles->ToArray();
	}
	array<Vehicle ^> ^World::GetAllVehicles()
	{
		List<Vehicle ^> ^list = gcnew List<Vehicle ^>();
		array<int> ^entities = MemoryAccess::GetEntityHandleList();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entities[i]) && Native::Function::Call<bool>(Native::Hash::IS_ENTITY_A_VEHICLE, entities[i]))
			{
				list->Add(gcnew Vehicle(entities[i]));
			}
		}

		return list->ToArray();
	}
	array<Vehicle ^> ^World::GetNearbyVehicles(Ped ^ped, float radius)
	{
		return GetNearbyVehicles(ped, radius, 10000);
	}
	array<Vehicle ^> ^World::GetNearbyVehicles(Ped ^ped, float radius, int maxAmount)
	{
		const Math::Vector3 position = ped->Position;
		Collections::Generic::List<Vehicle ^> ^result = gcnew Collections::Generic::List<Vehicle ^>();
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
	array<Vehicle ^> ^World::GetNearbyVehicles(Math::Vector3 position, float radius)
	{
		auto handles = GetAllVehicles();
		auto resultHandles = gcnew List<Vehicle ^>();

		for (int i = 0; i < handles->Length; i++)
		{
			if (handles[i]->Position.DistanceTo(position) <= radius)
				resultHandles->Add(handles[i]);
		}

		return resultHandles->ToArray();
	}
	array<Prop ^> ^World::GetAllProps()
	{
		List<Prop ^> ^list = gcnew List<Prop ^>();
		array<int> ^entities = MemoryAccess::GetEntityHandleList();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entities[i]) && Native::Function::Call<bool>(Native::Hash::IS_ENTITY_AN_OBJECT, entities[i]))
			{
				list->Add(gcnew Prop(entities[i]));
			}
		}

		return list->ToArray();
	}
	array<Entity ^> ^World::GetAllEntities()
	{
		List<Entity ^> ^list = gcnew List<Entity ^>();
		array<int> ^entities = MemoryAccess::GetEntityHandleList();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entities[i]))
			{
				switch (Native::Function::Call<int>(Native::Hash::GET_ENTITY_TYPE, entities[i]))
				{
					case 1:
						list->Add(gcnew Ped(entities[i]));
						break;
					case 2:
						list->Add(gcnew Vehicle(entities[i]));
						break;
					case 3:
						list->Add(gcnew Prop(entities[i]));
						break;
				}
			}
		}

		return list->ToArray();
	}
	Ped ^World::GetClosestPed(Math::Vector3 position, float radius)
	{
		int handle = 0;

		if (!Native::Function::Call<bool>(Native::Hash::GET_CLOSEST_PED, position.X, position.Y, position.Z, radius, true, true, &handle, false, false, -1))
		{
			return nullptr;
		}

		return gcnew Ped(handle);
	}
	Vehicle ^World::GetClosestVehicle(Math::Vector3 position, float radius)
	{
		return Native::Function::Call<Vehicle ^>(Native::Hash::GET_CLOSEST_VEHICLE, position.X, position.Y, position.Z, radius, 0, 70); // Last parameter still unknown.
	}
	float World::GetDistance(Math::Vector3 origin, Math::Vector3 destination)
	{
		return Native::Function::Call<float>(Native::Hash::GET_DISTANCE_BETWEEN_COORDS, origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z, 1);
	}
	float World::GetGroundHeight(Math::Vector2 position)
	{
		float height = 0.0f;
		Native::Function::Call(Native::Hash::GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, 1000.0f, &height);

		return height;
	}
	float World::GetGroundHeight(Math::Vector3 position)
	{
		return GetGroundHeight(Math::Vector2(position.X, position.Y));
	}

	Blip ^World::CreateBlip(Math::Vector3 position)
	{
		return Native::Function::Call<Blip ^>(Native::Hash::ADD_BLIP_FOR_COORD, position.X, position.Y, position.Z);
	}
	Blip ^World::CreateBlip(Math::Vector3 position, float radius)
	{
		return Native::Function::Call<Blip ^>(Native::Hash::ADD_BLIP_FOR_RADIUS, position.X, position.Y, position.Z, radius);
	}
	Camera ^World::CreateCamera(Math::Vector3 position, Math::Vector3 rotation, float fov)
	{
		return Native::Function::Call<Camera ^>(Native::Hash::CREATE_CAM_WITH_PARAMS, "DEFAULT_SCRIPTED_CAMERA", position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, fov, 1, 2);
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

		return Native::Function::Call<Ped ^>(Native::Hash::CREATE_PED, 26, model.Hash, position.X, position.Y, position.Z, heading, false, false);
	}
	Ped ^World::CreateRandomPed(Math::Vector3 position)
	{
		return Native::Function::Call<Ped ^>(Native::Hash::CREATE_RANDOM_PED, position.X, position.Y, position.Z);
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

		return Native::Function::Call<Vehicle ^>(Native::Hash::CREATE_VEHICLE, model.Hash, position.X, position.Y, position.Z, heading, false, false);
	}
	Prop ^World::CreateProp(Model model, Math::Vector3 position, bool dynamic, bool placeOnGround)
	{
		if (placeOnGround)
		{
			position.Z = World::GetGroundHeight(position);
		}

		if (!model.Request(1000))
		{
			return nullptr;
		}

		return Native::Function::Call<Prop ^>(Native::Hash::CREATE_OBJECT, model.Hash, position.X, position.Y, position.Z, 1, 1, dynamic);
	}
	Prop ^World::CreateProp(Model model, Math::Vector3 position, Math::Vector3 rotation, bool dynamic, bool placeOnGround)
	{
		Prop ^p = World::CreateProp(model, position, dynamic, placeOnGround);

		if (Object::ReferenceEquals(p, nullptr))
		{
			return nullptr;
		}

		p->Rotation = rotation;

		return p;
	}

	void World::ShootBullet(Math::Vector3 sourcePosition, Math::Vector3 targetPosition, Ped ^owner, Model model, int damage)
	{
		Native::Function::Call(Native::Hash::SHOOT_SINGLE_BULLET_BETWEEN_COORDS, sourcePosition.X, sourcePosition.Y, sourcePosition.Z, targetPosition.X, targetPosition.Y, targetPosition.Z, damage, 1, model.Hash, owner->Handle, 1, 0, -1);
	}
	void World::AddExplosion(Math::Vector3 position, ExplosionType type, float radius, float cameraShake)
	{
		Native::Function::Call(Native::Hash::ADD_EXPLOSION, position.X, position.Y, position.Z, static_cast<int>(type), radius, true, false, cameraShake);
	}
	void World::AddOwnedExplosion(Ped ^ped, Math::Vector3 position, ExplosionType type, float radius, float cameraShake)
	{
		Native::Function::Call(Native::Hash::ADD_OWNED_EXPLOSION, ped->Handle, position.X, position.Y, position.Z, static_cast<int>(type), radius, true, false, cameraShake);
	}
	Rope ^World::AddRope(RopeType type, Math::Vector3 position, Math::Vector3 rotation, float length, float minLength, bool breakable)
	{
		Native::Function::Call(Native::Hash::ROPE_LOAD_TEXTURES);

		return Native::Function::Call<Rope ^>(Native::Hash::ADD_ROPE, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, length, static_cast<int>(type), length, minLength, 0.5f, false, false, true, 1.0f, breakable, nullptr);
	}
	void World::DestroyAllCameras()
	{
		Native::Function::Call(Native::Hash::DESTROY_ALL_CAMS, 0);
	}
	void World::SetBlackout(bool enable)
	{
		Native::Function::Call(Native::Hash::_SET_BLACKOUT, enable);
	}

	int World::AddRelationshipGroup(String ^groupName)
	{
		int handle = 0;
		Native::Function::Call(Native::Hash::ADD_RELATIONSHIP_GROUP, groupName, &handle);

		return handle;
	}
	void World::RemoveRelationshipGroup(int group)
	{
		Native::Function::Call(Native::Hash::REMOVE_RELATIONSHIP_GROUP, group);
	}
	Relationship World::GetRelationshipBetweenGroups(int group1, int group2)
	{
		return static_cast<Relationship>(Native::Function::Call<int>(Native::Hash::GET_RELATIONSHIP_BETWEEN_GROUPS, group1, group2));
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

	RaycastResult World::Raycast(Math::Vector3 source, Math::Vector3 target, IntersectOptions options)
	{
		return Raycast(source, target, options, nullptr);
	}
	RaycastResult World::Raycast(Math::Vector3 source, Math::Vector3 target, IntersectOptions options, Entity ^entity)
	{
		return RaycastResult(Native::Function::Call<int>(Native::Hash::_CAST_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, static_cast<int>(options), entity == nullptr ? 0 : entity->Handle, 7));
	}

	void World::DrawMarker(MarkerType type, Math::Vector3 pos, Math::Vector3 dir, Math::Vector3 rot, Math::Vector3 scale, Drawing::Color color)
	{
		DrawMarker(type, pos, dir, rot, scale, color, false, false, 2, false, nullptr, nullptr, false);
	}
	void World::DrawMarker(MarkerType type, Math::Vector3 pos, Math::Vector3 dir, Math::Vector3 rot, Math::Vector3 scale, Drawing::Color color, bool bobUpAndDown, bool faceCamY, int unk2, bool rotateY, String ^textueDict, String ^textureName, bool drawOnEnt)
	{
		Native::InputArgument^ dict = gcnew Native::InputArgument(0), ^ name = gcnew Native::InputArgument(0);

		if (textueDict != nullptr && textureName != nullptr)
		{
			if (textueDict->Length > 0 && textureName->Length > 0)
			{
				dict = gcnew Native::InputArgument(textueDict);
				name = gcnew Native::InputArgument(textureName);
			}
		}

		Native::Function::Call(Native::Hash::DRAW_MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, color.R, color.G, color.B, color.A, bobUpAndDown, faceCamY, unk2, rotateY, dict, name, drawOnEnt);
	}

	void World::DrawLightWithRange(Math::Vector3 position, Drawing::Color color, float range, float intensity)
	{
		Native::Function::Call(Native::Hash::DRAW_LIGHT_WITH_RANGE, position.X, position.Y, position.Z, color.R, color.G, color.B, range, intensity);
	}
	void World::DrawSpotLight(Math::Vector3 pos, Math::Vector3 dir, Drawing::Color color, float distance, float brightness, float roundness, float radius, float fadeout)
	{
		Native::Function::Call(Native::Hash::DRAW_SPOT_LIGHT, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, color.R, color.G, color.B, distance, brightness, roundness, radius, fadeout);
	}
	void World::DrawSpotLightWithShadow(Math::Vector3 pos, Math::Vector3 dir, Drawing::Color color, float distance, float brightness, float roundness, float radius, float fadeout)
	{
		Native::Function::Call(Native::Hash::_0x5BCA583A583194DB, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, color.R, color.G, color.B, distance, brightness, roundness, radius, fadeout);
	}

	float World::CalculateTravelDistance(Math::Vector3 origin, Math::Vector3 destination)
	{
		return Native::Function::Call<float>(Native::Hash::CALCULATE_TRAVEL_DISTANCE_BETWEEN_POINTS, origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z);
	}
	Math::Vector3 World::GetNextPositionOnSidewalk(Math::Vector2 position)
	{
		return World::GetNextPositionOnSidewalk(Math::Vector3(position.X, position.Y, 0));
	}
	Math::Vector3 World::GetNextPositionOnSidewalk(Math::Vector3 position)
	{
		Native::OutputArgument ^outPos = gcnew Native::OutputArgument();

		if (Native::Function::Call<bool>(Native::Hash::GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, true, outPos, 0))
		{
			return outPos->GetResult<Math::Vector3>();
		}
		else if (Native::Function::Call<bool>(Native::Hash::GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, false, outPos, 0))
		{
			return outPos->GetResult<Math::Vector3>();
		}
		else return Math::Vector3::Zero;
	}
	Math::Vector3 World::GetNextPositionOnStreet(Math::Vector3 position)
	{
		return World::GetNextPositionOnStreet(position, false);
	}
	Math::Vector3 World::GetNextPositionOnStreet(Math::Vector2 position, bool unoccupied)
	{
		return World::GetNextPositionOnStreet(Math::Vector3(position.X, position.Y, 0), unoccupied);
	}
	Math::Vector3 World::GetNextPositionOnStreet(Math::Vector3 position, bool unoccupied)
	{
		Native::OutputArgument ^outPos = gcnew Native::OutputArgument();

		if (unoccupied)
		{
			for (int i = 1; i < 40; i++)
			{
				Native::Function::Call(Native::Hash::GET_NTH_CLOSEST_VEHICLE_NODE, position.X, position.Y, position.Z, i, outPos, 1, 0x40400000, 0);
				const Math::Vector3 newPos = outPos->GetResult<Math::Vector3>();

				if (!Native::Function::Call<bool>(Native::Hash::IS_POINT_OBSCURED_BY_A_MISSION_ENTITY, newPos.X, newPos.Y, newPos.Z, 5.0f, 5.0f, 5.0f, 0))
				{
					return newPos;
				}
			}
		}
		else if (Native::Function::Call<bool>(Native::Hash::GET_NTH_CLOSEST_VEHICLE_NODE, position.X, position.Y, position.Z, 1, outPos, 1, 0x40400000, 0))
		{
			return outPos->GetResult<Math::Vector3>();
		}
		return Math::Vector3();
	}
	Math::Vector3 World::GetSafeCoordForPed(Math::Vector3 position)
	{
		return World::GetSafeCoordForPed(Math::Vector3(position.X, position.Y, position.Z), true, 0);
	}
	Math::Vector3 World::GetSafeCoordForPed(Math::Vector3 position, bool sidewalk)
	{
		return World::GetSafeCoordForPed(position, sidewalk, 0);
	}
	Math::Vector3 World::GetSafeCoordForPed(Math::Vector3 position, bool sidewalk, int flags)
	{
		Native::OutputArgument ^outPos = gcnew Native::OutputArgument();

		if (Native::Function::Call<bool>(Native::Hash::GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, sidewalk, outPos, flags))
		{
			return outPos->GetResult<Math::Vector3>();
		}
		return Math::Vector3::Zero;
	}
	Street World::GetStreetAtCoord(Math::Vector2 position)
	{
		return World::GetStreetAtCoord(Math::Vector3(position.X, position.Y, 0));
	}
	Street World::GetStreetAtCoord(Math::Vector3 position)
	{
		Native::OutputArgument ^streetHash = gcnew Native::OutputArgument();
		Native::OutputArgument ^crossingHash = gcnew Native::OutputArgument();

		Native::Function::Call(Native::Hash::GET_STREET_NAME_AT_COORD, position.X, position.Y, position.Z, streetHash, crossingHash);

		System::String ^name = Native::Function::Call<System::String ^>(Native::Hash::GET_STREET_NAME_FROM_HASH_KEY, streetHash->GetResult<int>());
		System::String ^crossing = Native::Function::Call<System::String ^>(Native::Hash::GET_STREET_NAME_FROM_HASH_KEY, crossingHash->GetResult<int>());

		Street street = { name, crossing };

		return street;
	}
	Zone World::GetZone(System::String ^code)
	{
		System::String ^name;
		if (System::Enum::IsDefined(ZoneID::typeid, code))
		{
			switch ((ZoneID)System::Enum::Parse(ZoneID::typeid, code))
			{
			case ZoneID::AIRP:
				name = "Los Santos International Airport";
				break;
			case ZoneID::ALAMO:
				name = "Alamo Sea";
				break;
			case ZoneID::ALTA:
				name = "Alta";
				break;
			case ZoneID::ARMYB:
				name = "Fort Zancudo";
				break;
			case ZoneID::BANHAMC:
				name = "Banham Canyon"; // Banham Canyon Dr
				break;
			case ZoneID::BANNING:
				name = "Banning";
				break;
			case ZoneID::BEACH:
				name = "Vespucci Beach";
				break;
			case ZoneID::BHAMCA:
				name = "Banham Canyon";
				break;
			case ZoneID::BRADP:
				name = "Braddock Pass";
				break;
			case ZoneID::BRADT:
				name = "Braddock Tunnel";
				break;
			case ZoneID::BURTON:
				name = "Burton";
				break;
			case ZoneID::CALAFB:
				name = "Calafia Bridge";
				break;
			case ZoneID::CANNY:
				name = "Raton Canyon";
				break;
			case ZoneID::CCREAK:
				name = "Cassidy Creek";
				break;
			case ZoneID::CHAMH:
				name = "Chamberlain Hills";
				break;
			case ZoneID::CHIL:
				name = "Vinewood Hills";
				break;
			case ZoneID::CHU:
				name = "Chumash";
				break;
			case ZoneID::CMSW:
				name = "Chiliad Mountain State Wilderness";
				break;
			case ZoneID::CYPRE:
				name = "Cypress Flats";
				break;
			case ZoneID::DAVIS:
				name = "Davis";
				break;
			case ZoneID::DELBE:
				name = "Del Perro Beach";
				break;
			case ZoneID::DELPE:
				name = "Del Perro";
				break;
			case ZoneID::DELSOL:
				name = "Puerto Del Sol";
				break;
			case ZoneID::DESRT:
				name = "Grand Senora Desert";
				break;
			case ZoneID::DOWNT:
				name = "Downtown";
				break;
			case ZoneID::DTVINE:
				name = "Downtown Vinewood";
				break;
			case ZoneID::EAST_V:
				name = "East Vinewood";
				break;
			case ZoneID::EBURO:
				name = "El Burro Heights";
				break;
			case ZoneID::ELGORL:
				name = "El Gordo Lighthouse";
				break;
			case ZoneID::ELYSIAN:
				name = "Elysian Island";
				break;
			case ZoneID::GALFISH:
				name = "Galilee";
				break;
			case ZoneID::golf:
				name = "GWC and Golfing Society";
				break;
			case ZoneID::GRAPES:
				name = "Grapeseed";
				break;
			case ZoneID::GREATC:
				name = "Great Chaparral";
				break;
			case ZoneID::HARMO:
				name = "Harmony";
				break;
			case ZoneID::HAWICK:
				name = "Hawick";
				break;
			case ZoneID::HORS:
				name = "Vinewood Racetrack";
				break;
			case ZoneID::HUMLAB:
				name = "Humane Labs and Research";
				break;
			case ZoneID::JAIL:
				name = "Bolingbroke Penitentiary";
				break;
			case ZoneID::KOREAT:
				name = "Little Seoul";
				break;
			case ZoneID::LACT:
				name = "Land Act Reservoir";
				break;
			case ZoneID::LAGO:
				name = "Lago Zancudo";
				break;
			case ZoneID::LDAM:
				name = "Land Act Dam";
				break;
			case ZoneID::LEGSQU:
				name = "Legion Square";
				break;
			case ZoneID::LMESA:
				name = "La Mesa";
				break;
			case ZoneID::LOSPUER:
				name = "La Puerta";
				break;
			case ZoneID::MIRR:
				name = "Mirror Park";
				break;
			case ZoneID::MORN:
				name = "Morningwood";
				break;
			case ZoneID::MOVIE:
				name = "Richards Majestic";
				break;
			case ZoneID::MTCHIL:
				name = "Mount Chiliad";
				break;
			case ZoneID::MTGORDO:
				name = "Mount Gordo";
				break;
			case ZoneID::MTJOSE:
				name = "Mount Josiah";
				break;
			case ZoneID::MURRI:
				name = "Murrieta Heights";
				break;
			case ZoneID::NCHU:
				name = "North Chumash";
				break;
			case ZoneID::NOOSE:
				name = "N.O.O.S.E.";
				break;
			case ZoneID::OCEANA:
				name = "Pacific Ocean";
				break;
			case ZoneID::PALCOV:
				name = "Paleto Cove";
				break;
			case ZoneID::PALETO:
				name = "Paleto Bay";
				break;
			case ZoneID::PALFOR:
				name = "Paleto Forest";
				break;
			case ZoneID::PALHIGH:
				name = "Palomino Highlands";
				break;
			case ZoneID::PALMPOW:
				name = "Palmer-Taylor Power Station";
				break;
			case ZoneID::PBLUFF:
				name = "Pacific Bluffs";
				break;
			case ZoneID::PBOX:
				name = "Pillbox Hill";
				break;
			case ZoneID::PROCOB:
				name = "Procopio Beach";
				break;
			case ZoneID::RANCHO:
				name = "Rancho";
				break;
			case ZoneID::RGLEN:
				name = "Richman Glen";
				break;
			case ZoneID::RICHM:
				name = "Richman";
				break;
			case ZoneID::ROCKF:
				name = "Rockford Hills";
				break;
			case ZoneID::RTRAK:
				name = "Redwood Lights Track";
				break;
			case ZoneID::SanAnd:
				name = "San Andreas";
				break;
			case ZoneID::SANCHIA:
				name = "San Chianski Mountain Range";
				break;
			case ZoneID::SANDY:
				name = "Sandy Shores";
				break;
			case ZoneID::SKID:
				name = "Mission Row";
				break;
			case ZoneID::SLAB:
				name = "Stab City";
				break;
			case ZoneID::STAD:
				name = "Maze Bank Arena";
				break;
			case ZoneID::STRAW:
				name = "Strawberry";
				break;
			case ZoneID::TATAMO:
				name = "Tataviam Mountains";
				break;
			case ZoneID::TERMINA:
				name = "Terminal";
				break;
			case ZoneID::TEXTI:
				name = "Textile City";
				break;
			case ZoneID::TONGVAH:
				name = "Tongva Hills";
				break;
			case ZoneID::TONGVAV:
				name = "Tongva Valley";
				break;
			case ZoneID::VCANA:
				name = "Vespucci Canals";
				break;
			case ZoneID::VESP:
				name = "Vespucci";
				break;
			case ZoneID::VINE:
				name = "Vinewood";
				break;
			case ZoneID::WINDF:
				name = "RON Alternates Wind Farm";
				break;
			case ZoneID::WVINE:
				name = "West Vinewood";
				break;
			case ZoneID::ZANCUDO:
				name = "Zancudo River";
				break;
			case ZoneID::ZP_ORT:
				name = "Port of South Los Santos";
				break;
			case ZoneID::ZQ_UAR:
				name = "Davis Quartz";
				break;
			default:
				name = "Unknown";
			}
		}
		Zone zone = { code, name };
		return zone;
	}
	Zone World::GetZone(Math::Vector2 position)
	{
		return GetZone(Math::Vector3(position.X, position.Y, 0));
	}
	Zone World::GetZone(Math::Vector3 position)
	{
		System::String ^code = GetZoneName(position);
		return World::GetZone(code);
	}
	System::String ^World::GetZoneName(Math::Vector2 position)
	{
		return GetZoneName(Math::Vector3(position.X, position.Y, 0));
	}
	System::String ^World::GetZoneName(Math::Vector3 position)
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_NAME_OF_ZONE, position.X, position.Y, position.Z);
	}
}