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
		array<int> ^entities = MemoryAccess::GetAllPedHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entities[i]))
			{
				list->Add(gcnew Ped(entities[i]));
			}
		}

		return list->ToArray();
	}
	array<Ped ^> ^World::GetAllPeds(Model model)
	{
		List<Ped ^> ^list = gcnew List<Ped ^>();
		array<int> ^entities = MemoryAccess::GetAllPedHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<int>(Native::Hash::GET_ENTITY_MODEL, entities[i]) == model.Hash)
			{
				list->Add(gcnew Ped(entities[i]));
			}
		}

		return list->ToArray();
	}
	array<Ped ^> ^World::GetNearbyPeds(Math::Vector3 position, float radius)
	{
		auto resultHandles = gcnew List<Ped ^>();
		float r2 = radius * radius;
		array<int> ^entities = MemoryAccess::GetAllPedHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entities[i]))
			{
				if (Math::Vector3::Subtract(Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_COORDS, entities[i], 0), position).LengthSquared() < r2)
					resultHandles->Add(gcnew Ped(entities[i]));
			}
		}
		return resultHandles->ToArray();
	}
	array<Ped ^> ^World::GetNearbyPeds(Math::Vector3 position, float radius, Model model)
	{
		auto resultHandles = gcnew List<Ped ^>();
		float r2 = radius * radius;
		array<int> ^entities = MemoryAccess::GetAllPedHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<int>(Native::Hash::GET_ENTITY_MODEL, entities[i]) == model.Hash)
			{
				if (Math::Vector3::Subtract(Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_COORDS, entities[i], 0), position).LengthSquared() < r2)
					resultHandles->Add(gcnew Ped(entities[i]));
			}
		}
		return resultHandles->ToArray();
	}
	array<Vehicle ^> ^World::GetAllVehicles()
	{
		List<Vehicle ^> ^list = gcnew List<Vehicle ^>();
		array<int> ^entities = MemoryAccess::GetAllVehicleHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entities[i]))
			{
				list->Add(gcnew Vehicle(entities[i]));
			}
		}

		return list->ToArray();
	}
	array<Vehicle ^> ^World::GetAllVehicles(Model model)
	{
		List<Vehicle ^> ^list = gcnew List<Vehicle ^>();
		array<int> ^entities = MemoryAccess::GetAllVehicleHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<int>(Native::Hash::GET_ENTITY_MODEL, entities[i]) == model.Hash)
			{
				list->Add(gcnew Vehicle(entities[i]));
			}
		}

		return list->ToArray();
	}
	array<Vehicle ^> ^World::GetNearbyVehicles(Math::Vector3 position, float radius)
	{
		auto resultHandles = gcnew List<Vehicle ^>();
		float r2 = radius * radius;
		array<int> ^entities = MemoryAccess::GetAllVehicleHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entities[i]))
			{
				if (Math::Vector3::Subtract(Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_COORDS, entities[i], 0), position).LengthSquared() < r2)
					resultHandles->Add(gcnew Vehicle(entities[i]));
			}
		}
		return resultHandles->ToArray();
	}
	array<Vehicle ^> ^World::GetNearbyVehicles(Math::Vector3 position, float radius, Model model)
	{
		auto resultHandles = gcnew List<Vehicle ^>();
		float r2 = radius * radius;
		array<int> ^entities = MemoryAccess::GetAllVehicleHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<int>(Native::Hash::GET_ENTITY_MODEL, entities[i]) == model.Hash)
			{
				if (Math::Vector3::Subtract(Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_COORDS, entities[i], 0), position).LengthSquared() < r2)
					resultHandles->Add(gcnew Vehicle(entities[i]));
			}
		}
		return resultHandles->ToArray();
	}
	array<Prop ^> ^World::GetAllProps()
	{
		List<Prop ^> ^list = gcnew List<Prop ^>();
		array<int> ^entities = MemoryAccess::GetAllObjectHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entities[i]))
			{
				list->Add(gcnew Prop(entities[i]));
			}
		}

		return list->ToArray();
	}
	array<Prop ^> ^World::GetAllProps(Model model)
	{
		List<Prop ^> ^list = gcnew List<Prop ^>();
		array<int> ^entities = MemoryAccess::GetAllObjectHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<int>(Native::Hash::GET_ENTITY_MODEL, entities[i]) == model.Hash)
			{
				list->Add(gcnew Prop(entities[i]));
			}
		}

		return list->ToArray();
	}
	array<Prop ^> ^World::GetNearbyProps(Math::Vector3 position, float radius)
	{
		auto resultHandles = gcnew List<Prop ^>();
		float r2 = radius * radius;
		array<int> ^entities = MemoryAccess::GetAllObjectHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entities[i]))
			{
				if (Math::Vector3::Subtract(Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_COORDS, entities[i], 0), position).LengthSquared() < r2)
					resultHandles->Add(gcnew Prop(entities[i]));
			}
		}
		return resultHandles->ToArray();
	}
	array<Prop ^> ^World::GetNearbyProps(Math::Vector3 position, float radius, Model model)
	{
		auto resultHandles = gcnew List<Prop ^>();
		float r2 = radius * radius;
		array<int> ^entities = MemoryAccess::GetAllObjectHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<int>(Native::Hash::GET_ENTITY_MODEL, entities[i]) == model.Hash)
			{
				if (Math::Vector3::Subtract(Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_COORDS, entities[i], 0), position).LengthSquared() < r2)
					resultHandles->Add(gcnew Prop(entities[i]));
			}
		}
		return resultHandles->ToArray();
	}
	array<Entity ^> ^World::GetAllEntities()
	{
		List<Entity ^> ^list = gcnew List<Entity ^>();
		array<int> ^entities = MemoryAccess::GetAllEntityHandles();

		for (int i = 0; i < entities->Length; i++)
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
			}
		}

		return list->ToArray();
	}
	array<Entity ^> ^World::GetNearbyEntities(Math::Vector3 position, float radius)
	{
		List<Entity ^> ^list = gcnew List<Entity ^>();
		array<int> ^entities = MemoryAccess::GetAllEntityHandles();
		float r2 = radius * radius;
		for (int i = 0; i < entities->Length; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entities[i]))
			{
				if (Math::Vector3::Subtract(Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_COORDS, entities[i], 0), position).LengthSquared() < r2)
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
					}
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
	Math::Vector3 World::GetNextPositionOnStreet(Math::Vector3 position)
	{
		Native::OutputArgument ^outPos = gcnew Native::OutputArgument();

		for (int i = 1; i < 40; i++)
		{
			Native::Function::Call(Native::Hash::GET_NTH_CLOSEST_VEHICLE_NODE, position.X, position.Y, position.Z, i, outPos, 1, 0x40400000, 0);
			const Math::Vector3 newPos = outPos->GetResult<Math::Vector3>();

			if (!Native::Function::Call<bool>(Native::Hash::IS_POINT_OBSCURED_BY_A_MISSION_ENTITY, newPos.X, newPos.Y, newPos.Z, 5.0f, 5.0f, 5.0f, 0))
			{
				return newPos;
			}
		}
		return Math::Vector3();
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
		AddExplosion(position, type, radius, cameraShake, true, false);
	}
	void World::AddExplosion(Math::Vector3 position, ExplosionType type, float radius, float cameraShake, bool isAudible, bool isInvisible)
	{
		Native::Function::Call(Native::Hash::ADD_EXPLOSION, position.X, position.Y, position.Z, static_cast<int>(type), radius, isAudible, isInvisible, cameraShake);
	}
	void World::AddOwnedExplosion(Ped ^ped, Math::Vector3 position, ExplosionType type, float radius, float cameraShake)
	{
		AddOwnedExplosion(ped, position, type, radius, cameraShake, true, false);
	}
	void World::AddOwnedExplosion(Ped ^ped, Math::Vector3 position, ExplosionType type, float radius, float cameraShake, bool isAudible, bool isInvisible)
	{
		Native::Function::Call(Native::Hash::ADD_OWNED_EXPLOSION, ped->Handle, position.X, position.Y, position.Z, static_cast<int>(type), radius, isAudible, isInvisible, cameraShake);
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
}