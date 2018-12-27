#include "World.hpp"
#include "Native.hpp"
#include "NativeMemory.hpp"

#include "Blip.hpp"
#include "Camera.hpp"
#include "Ped.hpp"
#include "Pickup.hpp"
#include "Prop.hpp"
#include "Raycast.hpp"
#include "Rope.hpp"
#include "Vehicle.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	namespace
	{
		private enum class ZoneID
		{
			AIRP,
			ALAMO,
			ALTA,
			ARMYB,
			BANHAMC,
			BANNING,
			BAYTRE,
			BEACH,
			BHAMCA,
			BRADP,
			BRADT,
			BURTON,
			CALAFB,
			CANNY,
			CCREAK,
			CHAMH,
			CHIL,
			CHU,
			CMSW,
			CYPRE,
			DAVIS,
			DELBE,
			DELPE,
			DELSOL,
			DESRT,
			DOWNT,
			DTVINE,
			EAST_V,
			EBURO,
			ELGORL,
			ELYSIAN,
			GALFISH,
			GALLI,
			golf,
			GRAPES,
			GREATC,
			HARMO,
			HAWICK,
			HORS,
			HUMLAB,
			JAIL,
			KOREAT,
			LACT,
			LAGO,
			LDAM,
			LEGSQU,
			LMESA,
			LOSPUER,
			MIRR,
			MORN,
			MOVIE,
			MTCHIL,
			MTGORDO,
			MTJOSE,
			MURRI,
			NCHU,
			NOOSE,
			OCEANA,
			PALCOV,
			PALETO,
			PALFOR,
			PALHIGH,
			PALMPOW,
			PBLUFF,
			PBOX,
			PROCOB,
			RANCHO,
			RGLEN,
			RICHM,
			ROCKF,
			RTRAK,
			SanAnd,
			SANCHIA,
			SANDY,
			SKID,
			SLAB,
			STAD,
			STRAW,
			TATAMO,
			TERMINA,
			TEXTI,
			TONGVAH,
			TONGVAV,
			VCANA,
			VESP,
			VINE,
			WINDF,
			WVINE,
			ZANCUDO,
			ZP_ORT,
			ZQ_UAR
		};
	}

	DateTime World::CurrentDate::get()
	{
		int year = Native::Function::Call<int>(Native::Hash::GET_CLOCK_YEAR);
		int month = Native::Function::Call<int>(Native::Hash::GET_CLOCK_MONTH);
		int day = System::Math::Min(Native::Function::Call<int>(Native::Hash::GET_CLOCK_DAY_OF_MONTH), _gregorianCalendar->GetDaysInMonth(year, month));
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
	GTA::Weather World::Weather::get()
	{
		for (int i = 0; i < _weatherNames->Length; i++)
		{
			int weatherHash = Native::Function::Call<int>(Native::Hash::_GET_CURRENT_WEATHER_TYPE);
			
			if (weatherHash == Game::GenerateHash(_weatherNames[i]))
			{
				return static_cast<GTA::Weather>(i);
			}
		}
		return GTA::Weather::Unknown;
	}
	void World::Weather::set(GTA::Weather value)
	{
		if (Enum::IsDefined(value.GetType(), value) && value != GTA::Weather::Unknown)
		{
			Native::Function::Call(Native::Hash::SET_WEATHER_TYPE_NOW, _weatherNames[static_cast<int>(value)]);
		}
	}
	GTA::Weather World::NextWeather::get()
	{
		for (int i = 0, length = _weatherNames->Length; i < length; i++)
		{
			if (Native::Function::Call<bool>(Native::Hash::IS_NEXT_WEATHER_TYPE, _weatherNames[i]))
			{
				return static_cast<GTA::Weather>(i);
			}
		}
		return GTA::Weather::Unknown;
	}
	void World::NextWeather::set(GTA::Weather value)
	{
		if (Enum::IsDefined(value.GetType(), value) && value != GTA::Weather::Unknown)
		{
			int currentWeatherHash, nextWeatherHash;
			float weatherTransition;

			Native::Function::Call(Native::Hash::_GET_WEATHER_TYPE_TRANSITION, &currentWeatherHash, &nextWeatherHash, &weatherTransition);

			nextWeatherHash = Game::GenerateHash(_weatherNames[static_cast<int>(value)]);
			Native::Function::Call(Native::Hash::_SET_WEATHER_TYPE_TRANSITION, currentWeatherHash, nextWeatherHash, weatherTransition);
		}
	}
	float World::WeatherTransition::get()
	{
		int currentWeatherHash, nextWeatherHash;
		float weatherTransition;

		Native::Function::Call(Native::Hash::_GET_WEATHER_TYPE_TRANSITION, &currentWeatherHash, &nextWeatherHash, &weatherTransition);
		return weatherTransition;
	}
	void World::WeatherTransition::set(float value)
	{
		Native::Function::Call(Native::Hash::_SET_WEATHER_TYPE_TRANSITION, 0, 0, value);
	}

	array<Blip ^> ^World::GetActiveBlips()
	{
		Collections::Generic::List<Blip ^> ^res = gcnew Collections::Generic::List<Blip ^>();

		for each(BlipSprite sprite in Enum::GetValues(BlipSprite::typeid))
		{
			int handle = Native::Function::Call<int>(Native::Hash::GET_FIRST_BLIP_INFO_ID, static_cast<int>(sprite));

			while (Native::Function::Call<bool>(Native::Hash::DOES_BLIP_EXIST, handle))
			{
				res->Add(gcnew Blip(handle));

				handle = Native::Function::Call<int>(Native::Hash::GET_NEXT_BLIP_INFO_ID, static_cast<int>(sprite));
			}
		}

		return res->ToArray();
	}
	array<Ped ^> ^World::GetAllPeds()
	{
		array<int> ^entities = Native::MemoryAccess::GetPedHandles();
		array<Ped ^> ^resultHandles = gcnew array<Ped ^>(entities->Length);

		for (int i = 0; i < entities->Length; i++)
		{
			resultHandles[i] = gcnew Ped(entities[i]);
		}

		return resultHandles;
	}
	array<Ped ^> ^World::GetAllPeds(Model model)
	{
		array<int> ^entities = Native::MemoryAccess::GetPedHandles(model.Hash);
		array<Ped ^> ^resultHandles = gcnew array<Ped ^>(entities->Length);

		for (int i = 0; i < entities->Length; i++)
		{
			resultHandles[i] = gcnew Ped(entities[i]);
		}

		return resultHandles;
	}
	array<Ped ^> ^World::GetNearbyPeds(Ped ^ped, float radius)
	{
		List<Ped ^> ^resultHandles = gcnew List<Ped ^>();
		array<int> ^entities = Native::MemoryAccess::GetPedHandles(ped->Position, radius);

		for (int i = 0; i < entities->Length; i++)
		{
			if (entities[i] == ped->Handle)
			{
				continue;
			}

			resultHandles->Add(gcnew Ped(entities[i]));
		}

		return resultHandles->ToArray();
	}
	array<Ped ^> ^World::GetNearbyPeds(Math::Vector3 position, float radius)
	{
		array<int> ^entities = Native::MemoryAccess::GetPedHandles(position, radius);
		array<Ped ^> ^resultHandles = gcnew array<Ped ^>(entities->Length);

		for (int i = 0; i < entities->Length; i++)
		{
			resultHandles[i] = gcnew Ped(entities[i]);
		}

		return resultHandles;
	}
	array<Ped ^> ^World::GetNearbyPeds(Math::Vector3 position, float radius, Model model)
	{
		array<int> ^entities = Native::MemoryAccess::GetPedHandles(position, radius, model.Hash);
		array<Ped ^> ^resultHandles = gcnew array<Ped ^>(entities->Length);

		for (int i = 0; i < entities->Length; i++)
		{
			resultHandles[i] = gcnew Ped(entities[i]);
		}

		return resultHandles;
	}
	array<Vehicle ^> ^World::GetAllVehicles()
	{
		array<int> ^entities = Native::MemoryAccess::GetVehicleHandles();
		array<Vehicle ^> ^resultHandles = gcnew array<Vehicle ^>(entities->Length);

		for (int i = 0; i < entities->Length; i++)
		{
			resultHandles[i] = gcnew Vehicle(entities[i]);
		}

		return resultHandles;
	}
	array<Vehicle ^> ^World::GetAllVehicles(Model model)
	{
		array<int> ^entities = Native::MemoryAccess::GetVehicleHandles(model.Hash);
		array<Vehicle ^> ^resultHandles = gcnew array<Vehicle ^>(entities->Length);

		for (int i = 0; i < entities->Length; i++)
		{
			resultHandles[i] = gcnew Vehicle(entities[i]);
		}

		return resultHandles;
	}
	array<Vehicle ^> ^World::GetNearbyVehicles(Ped ^ped, float radius)
	{
		List<Vehicle ^> ^resultHandles = gcnew List<Vehicle ^>();
		array<int> ^entities = Native::MemoryAccess::GetVehicleHandles(ped->Position, radius);
		bool inVehicle = false;
		int vehicleHandle = 0;
		if (ped->IsInVehicle())
		{
			inVehicle = true;
			vehicleHandle = ped->CurrentVehicle->Handle;
		}


		for (int i = 0; i < entities->Length; i++)
		{
			if (inVehicle && entities[i] == vehicleHandle)
			{
				continue;
			}

			resultHandles->Add(gcnew Vehicle(entities[i]));
		}

		return resultHandles->ToArray();
	}
	array<Vehicle ^> ^World::GetNearbyVehicles(Math::Vector3 position, float radius)
	{
		array<int> ^entities = Native::MemoryAccess::GetVehicleHandles(position, radius);
		array<Vehicle ^> ^resultHandles = gcnew array<Vehicle ^>(entities->Length);

		for (int i = 0; i < entities->Length; i++)
		{
			resultHandles[i] = gcnew Vehicle(entities[i]);
		}

		return resultHandles;
	}
	array<Vehicle ^> ^World::GetNearbyVehicles(Math::Vector3 position, float radius, Model model)
	{
		array<int> ^entities = Native::MemoryAccess::GetVehicleHandles(position, radius, model.Hash);
		array<Vehicle ^> ^resultHandles = gcnew array<Vehicle ^>(entities->Length);

		for (int i = 0; i < entities->Length; i++)
		{
			resultHandles[i] = gcnew Vehicle(entities[i]);
		}

		return resultHandles;
	}
	array<Prop ^> ^World::GetAllProps()
	{
		array<int> ^entities = Native::MemoryAccess::GetPropHandles();
		array<Prop ^> ^resultHandles = gcnew array<Prop ^>(entities->Length);

		for (int i = 0; i < entities->Length; i++)
		{
			resultHandles[i] = gcnew Prop(entities[i]);
		}

		return resultHandles;
	}
	array<Prop ^> ^World::GetAllProps(Model model)
	{
		array<int> ^entities = Native::MemoryAccess::GetPropHandles(model.Hash);
		array<Prop ^> ^resultHandles = gcnew array<Prop ^>(entities->Length);

		for (int i = 0; i < entities->Length; i++)
		{
			resultHandles[i] = gcnew Prop(entities[i]);
		}

		return resultHandles;
	}
	array<Prop ^> ^World::GetNearbyProps(Math::Vector3 position, float radius)
	{
		array<int> ^entities = Native::MemoryAccess::GetPropHandles(position, radius);
		array<Prop ^> ^resultHandles = gcnew array<Prop ^>(entities->Length);

		for (int i = 0; i < entities->Length; i++)
		{
			resultHandles[i] = gcnew Prop(entities[i]);
		}

		return resultHandles;
	}
	array<Prop ^> ^World::GetNearbyProps(Math::Vector3 position, float radius, Model model)
	{
		array<int> ^entities = Native::MemoryAccess::GetPropHandles(position, radius, model.Hash);
		array<Prop ^> ^resultHandles = gcnew array<Prop ^>(entities->Length);

		for (int i = 0; i < entities->Length; i++)
		{
			resultHandles[i] = gcnew Prop(entities[i]);
		}

		return resultHandles;
	}
	array<Entity ^> ^World::GetAllEntities()
	{
		List<Entity ^> ^resultHandles = gcnew List<Entity ^>();
		array<int> ^entities = Native::MemoryAccess::GetEntityHandles();

		for (int i = 0; i < entities->Length; i++)
		{
			switch (Native::Function::Call<int>(Native::Hash::GET_ENTITY_TYPE, entities[i]))
			{
				case 1:
					resultHandles->Add(gcnew Ped(entities[i]));
					break;
				case 2:
					resultHandles->Add(gcnew Vehicle(entities[i]));
					break;
				case 3:
					resultHandles->Add(gcnew Prop(entities[i]));
					break;
			}
		}

		return resultHandles->ToArray();
	}
	array<Entity ^> ^World::GetNearbyEntities(Math::Vector3 position, float radius)
	{
		List<Entity ^> ^resultHandles = gcnew List<Entity ^>();
		array<int> ^entities = Native::MemoryAccess::GetEntityHandles(position, radius);

		for (int i = 0; i < entities->Length; i++)
		{
			switch (Native::Function::Call<int>(Native::Hash::GET_ENTITY_TYPE, entities[i]))
			{
				case 1:
					resultHandles->Add(gcnew Ped(entities[i]));
					break;
				case 2:
					resultHandles->Add(gcnew Vehicle(entities[i]));
					break;
				case 3:
					resultHandles->Add(gcnew Prop(entities[i]));
					break;
			}
		}

		return resultHandles->ToArray();
	}
	Ped ^World::GetClosestPed(Math::Vector3 position, float radius)
	{
		array<int> ^entities = Native::MemoryAccess::GetPedHandles(position, radius);
		float closestDist2 = radius * radius;
		int closestHandle = 0;
		for (int i = 0; i < entities->Length; i++)
		{
			float dist2 = Math::Vector3::Subtract(Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_COORDS, entities[i], 0), position).LengthSquared();
			if (dist2 <= closestDist2)
			{
				closestHandle = entities[i];
				closestDist2 = dist2;
			}

		}
		if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, closestHandle))
		{
			return gcnew Ped(closestHandle);
		}
		return nullptr;
	}
	Vehicle ^World::GetClosestVehicle(Math::Vector3 position, float radius)
	{
		array<int> ^entities = Native::MemoryAccess::GetVehicleHandles(position, radius);
		float closestDist2 = radius * radius;
		int closestHandle = 0;
		for (int i = 0; i < entities->Length; i++)
		{
			float dist2 = Math::Vector3::Subtract(Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_COORDS, entities[i], 0), position).LengthSquared();
			if (dist2 <= closestDist2)
			{
				closestHandle = entities[i];
				closestDist2 = dist2;
			}

		}
		if (Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, closestHandle))
		{
			return gcnew Vehicle(closestHandle);
		}
		return nullptr;
	}
	generic <typename T> where T : ISpatial
	T World::GetClosest(Math::Vector3 position, ... array<T> ^spatials)
	{
		float closestDist2 = 3e38f;
		ISpatial ^closest = nullptr;
		for (int i = 0; i < spatials->Length; i++)
		{
			float dist2 = Math::Vector3::Subtract(spatials[i]->Position,  position).LengthSquared();
			if (dist2 <= closestDist2)
			{
				closest = spatials[i];
				closestDist2 = dist2;
			}
		}
		return static_cast<T>(closest);
	}
	float World::GetDistance(Math::Vector3 origin, Math::Vector3 destination)
	{
		return Native::Function::Call<float>(Native::Hash::GET_DISTANCE_BETWEEN_COORDS, origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z, 1);
	}
	float World::CalculateTravelDistance(Math::Vector3 origin, Math::Vector3 destination)
	{
		return Native::Function::Call<float>(Native::Hash::CALCULATE_TRAVEL_DISTANCE_BETWEEN_POINTS, origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z);
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
	Math::Vector3 World::GetWaypointPosition()
	{
		if (!Game::IsWaypointActive)
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
	RaycastResult World::GetCrosshairCoordinates()
	{
		return Raycast(GameplayCamera::Position, GameplayCamera::Direction, 1000.0f, IntersectOptions::Everything);
	}
	Math::Vector3 World::GetSafeCoordForPed(Math::Vector3 position)
	{
		return GetSafeCoordForPed(Math::Vector3(position.X, position.Y, position.Z), true, 0);
	}
	Math::Vector3 World::GetSafeCoordForPed(Math::Vector3 position, bool sidewalk)
	{
		return GetSafeCoordForPed(position, sidewalk, 0);
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
	Math::Vector3 World::GetNextPositionOnStreet(Math::Vector3 position)
	{
		return GetNextPositionOnStreet(position, false);
	}
	Math::Vector3 World::GetNextPositionOnStreet(Math::Vector2 position, bool unoccupied)
	{
		return GetNextPositionOnStreet(Math::Vector3(position.X, position.Y, 0), unoccupied);
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

		return Math::Vector3::Zero;
	}
	Math::Vector3 World::GetNextPositionOnSidewalk(Math::Vector2 position)
	{
		return GetNextPositionOnSidewalk(Math::Vector3(position.X, position.Y, 0));
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
		else
		{
			return Math::Vector3::Zero;
		}
	}
	System::String ^World::GetZoneName(Math::Vector2 position)
	{
		return GetZoneName(Math::Vector3(position.X, position.Y, 0));
	}
	System::String ^World::GetZoneName(Math::Vector3 position)
	{
		System::String ^code = GetZoneNameLabel(position);

		ZoneID id;

		if (System::Enum::TryParse(code, id))
		{
			switch (id)
			{
				case ZoneID::AIRP:
					return "Los Santos International Airport";
				case ZoneID::ALAMO:
					return "Alamo Sea";
				case ZoneID::ALTA:
					return "Alta";
				case ZoneID::ARMYB:
					return "Fort Zancudo";
				case ZoneID::BANHAMC:
					return "Banham Canyon";
				case ZoneID::BANNING:
					return "Banning";
				case ZoneID::BAYTRE:
					return "Baytree Canyon";
				case ZoneID::BEACH:
					return "Vespucci Beach";
				case ZoneID::BHAMCA:
					return "Banham Canyon";
				case ZoneID::BRADP:
					return "Braddock Pass";
				case ZoneID::BRADT:
					return "Braddock Tunnel";
				case ZoneID::BURTON:
					return "Burton";
				case ZoneID::CALAFB:
					return "Calafia Bridge";
				case ZoneID::CANNY:
					return "Raton Canyon";
				case ZoneID::CCREAK:
					return "Cassidy Creek";
				case ZoneID::CHAMH:
					return "Chamberlain Hills";
				case ZoneID::CHIL:
					return "Vinewood Hills";
				case ZoneID::CHU:
					return "Chumash";
				case ZoneID::CMSW:
					return "Chiliad Mountain State Wilderness";
				case ZoneID::CYPRE:
					return "Cypress Flats";
				case ZoneID::DAVIS:
					return "Davis";
				case ZoneID::DELBE:
					return "Del Perro Beach";
				case ZoneID::DELPE:
					return "Del Perro";
				case ZoneID::DELSOL:
					return "Puerto Del Sol";
				case ZoneID::DESRT:
					return "Grand Senora Desert";
				case ZoneID::DOWNT:
					return "Downtown";
				case ZoneID::DTVINE:
					return "Downtown Vinewood";
				case ZoneID::EAST_V:
					return "East Vinewood";
				case ZoneID::EBURO:
					return "El Burro Heights";
				case ZoneID::ELGORL:
					return "El Gordo Lighthouse";
				case ZoneID::ELYSIAN:
					return "Elysian Island";
				case ZoneID::GALFISH:
					return "Galilee";
				case ZoneID::GALLI:
					return "Galileo Park";
				case ZoneID::golf:
					return "GWC and Golfing Society";
				case ZoneID::GRAPES:
					return "Grapeseed";
				case ZoneID::GREATC:
					return "Great Chaparral";
				case ZoneID::HARMO:
					return "Harmony";
				case ZoneID::HAWICK:
					return "Hawick";
				case ZoneID::HORS:
					return "Vinewood Racetrack";
				case ZoneID::HUMLAB:
					return "Humane Labs and Research";
				case ZoneID::JAIL:
					return "Bolingbroke Penitentiary";
				case ZoneID::KOREAT:
					return "Little Seoul";
				case ZoneID::LACT:
					return "Land Act Reservoir";
				case ZoneID::LAGO:
					return "Lago Zancudo";
				case ZoneID::LDAM:
					return "Land Act Dam";
				case ZoneID::LEGSQU:
					return "Legion Square";
				case ZoneID::LMESA:
					return "La Mesa";
				case ZoneID::LOSPUER:
					return "La Puerta";
				case ZoneID::MIRR:
					return "Mirror Park";
				case ZoneID::MORN:
					return "Morningwood";
				case ZoneID::MOVIE:
					return "Richards Majestic";
				case ZoneID::MTCHIL:
					return "Mount Chiliad";
				case ZoneID::MTGORDO:
					return "Mount Gordo";
				case ZoneID::MTJOSE:
					return "Mount Josiah";
				case ZoneID::MURRI:
					return "Murrieta Heights";
				case ZoneID::NCHU:
					return "North Chumash";
				case ZoneID::NOOSE:
					return "N.O.O.S.E.";
				case ZoneID::OCEANA:
					return "Pacific Ocean";
				case ZoneID::PALCOV:
					return "Paleto Cove";
				case ZoneID::PALETO:
					return "Paleto Bay";
				case ZoneID::PALFOR:
					return "Paleto Forest";
				case ZoneID::PALHIGH:
					return "Palomino Highlands";
				case ZoneID::PALMPOW:
					return "Palmer-Taylor Power Station";
				case ZoneID::PBLUFF:
					return "Pacific Bluffs";
				case ZoneID::PBOX:
					return "Pillbox Hill";
				case ZoneID::PROCOB:
					return "Procopio Beach";
				case ZoneID::RANCHO:
					return "Rancho";
				case ZoneID::RGLEN:
					return "Richman Glen";
				case ZoneID::RICHM:
					return "Richman";
				case ZoneID::ROCKF:
					return "Rockford Hills";
				case ZoneID::RTRAK:
					return "Redwood Lights Track";
				case ZoneID::SanAnd:
					return "San Andreas";
				case ZoneID::SANCHIA:
					return "San Chianski Mountain Range";
				case ZoneID::SANDY:
					return "Sandy Shores";
				case ZoneID::SKID:
					return "Mission Row";
				case ZoneID::SLAB:
					return "Stab City";
				case ZoneID::STAD:
					return "Maze Bank Arena";
				case ZoneID::STRAW:
					return "Strawberry";
				case ZoneID::TATAMO:
					return "Tataviam Mountains";
				case ZoneID::TERMINA:
					return "Terminal";
				case ZoneID::TEXTI:
					return "Textile City";
				case ZoneID::TONGVAH:
					return "Tongva Hills";
				case ZoneID::TONGVAV:
					return "Tongva Valley";
				case ZoneID::VCANA:
					return "Vespucci Canals";
				case ZoneID::VESP:
					return "Vespucci";
				case ZoneID::VINE:
					return "Vinewood";
				case ZoneID::WINDF:
					return "RON Alternates Wind Farm";
				case ZoneID::WVINE:
					return "West Vinewood";
				case ZoneID::ZANCUDO:
					return "Zancudo River";
				case ZoneID::ZP_ORT:
					return "Port of South Los Santos";
				case ZoneID::ZQ_UAR:
					return "Davis Quartz";
			}
		}

		return System::String::Empty;
	}
	System::String ^World::GetZoneNameLabel(Math::Vector2 position)
	{
		return GetZoneNameLabel(Math::Vector3(position.X, position.Y, 0));
	}
	System::String ^World::GetZoneNameLabel(Math::Vector3 position)
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_NAME_OF_ZONE, position.X, position.Y, position.Z);
	}
	System::String ^World::GetStreetName(Math::Vector2 position)
	{
		return GetStreetName(Math::Vector3(position.X, position.Y, 0));
	}
	System::String ^World::GetStreetName(Math::Vector3 position)
	{
		int streetHash = 0, crossingHash = 0;
		Native::Function::Call(Native::Hash::GET_STREET_NAME_AT_COORD, position.X, position.Y, position.Z, &streetHash, &crossingHash);

		return Native::Function::Call<System::String ^>(Native::Hash::GET_STREET_NAME_FROM_HASH_KEY, streetHash);
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
	Pickup ^World::CreatePickup(PickupType type, Math::Vector3 position, Model model, int value)
	{
		if (!model.Request(1000))
		{
			return nullptr;
		}

		const int handle = Native::Function::Call<int>(Native::Hash::CREATE_PICKUP, static_cast<int>(type), position.X, position.Y, position.Z, 0, value, true, model.Hash);

		if (handle == 0)
		{
			return nullptr;
		}

		return gcnew Pickup(handle);
	}
	Pickup ^World::CreatePickup(PickupType type, Math::Vector3 position, Math::Vector3 rotation, Model model, int value)
	{
		if (!model.Request(1000))
		{
			return nullptr;
		}

		const int handle = Native::Function::Call<int>(Native::Hash::CREATE_PICKUP_ROTATE, static_cast<int>(type), position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 0, value, 2, true, model.Hash);

		if (handle == 0)
		{
			return nullptr;
		}

		return gcnew Pickup(handle);
	}
	Prop ^World::CreateAmbientPickup(PickupType type, Math::Vector3 position, Model model, int value)
	{
		if (!model.Request(1000))
		{
			return nullptr;
		}

		const int handle = Native::Function::Call<int>(Native::Hash::CREATE_AMBIENT_PICKUP, static_cast<int>(type), position.X, position.Y, position.Z, 0, value, model.Hash, false, true);

		if (handle == 0)
		{
			return nullptr;
		}

		return gcnew Prop(handle);
	}

	void World::ShootBullet(Math::Vector3 sourcePosition, Math::Vector3 targetPosition, Ped ^owner, Model model, int damage)
	{
		ShootBullet(sourcePosition, targetPosition, owner, model, damage, -1.0f);
	}
	void World::ShootBullet(Math::Vector3 sourcePosition, Math::Vector3 targetPosition, Ped ^owner, Model model, int damage, float speed)
	{
		Native::Function::Call(Native::Hash::SHOOT_SINGLE_BULLET_BETWEEN_COORDS, sourcePosition.X, sourcePosition.Y, sourcePosition.Z, targetPosition.X, targetPosition.Y, targetPosition.Z, damage, 1, model.Hash, owner->Handle, 1, 0, speed);
	}
	void World::AddExplosion(Math::Vector3 position, ExplosionType type, float radius, float cameraShake)
	{
		Native::Function::Call(Native::Hash::ADD_EXPLOSION, position.X, position.Y, position.Z, static_cast<int>(type), radius, true, false, cameraShake);
	}
	void World::AddExplosion(Math::Vector3 position, ExplosionType type, float radius, float cameraShake, bool Aubidble, bool Invis)
	{
		Native::Function::Call(Native::Hash::ADD_EXPLOSION, position.X, position.Y, position.Z, static_cast<int>(type), radius, Aubidble, Invis, cameraShake);
	}
	void World::AddOwnedExplosion(Ped ^ped, Math::Vector3 position, ExplosionType type, float radius, float cameraShake)
	{
		Native::Function::Call(Native::Hash::ADD_OWNED_EXPLOSION, ped->Handle, position.X, position.Y, position.Z, static_cast<int>(type), radius, true, false, cameraShake);
	}
	void World::AddOwnedExplosion(Ped ^ped, Math::Vector3 position, ExplosionType type, float radius, float cameraShake, bool Aubidble, bool Invis)
	{
		Native::Function::Call(Native::Hash::ADD_OWNED_EXPLOSION, ped->Handle, position.X, position.Y, position.Z, static_cast<int>(type), radius, Aubidble, Invis, cameraShake);
	}
	Rope ^World::AddRope(RopeType type, Math::Vector3 position, Math::Vector3 rotation, float length, float minLength, bool breakable)
	{
		Native::Function::Call(Native::Hash::ROPE_LOAD_TEXTURES);

		return Native::Function::Call<Rope ^>(Native::Hash::ADD_ROPE, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, length, static_cast<int>(type), length, minLength, 0.5f, false, false, true, 1.0f, breakable, 0);
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
	RaycastResult World::Raycast(Math::Vector3 source, Math::Vector3 target, IntersectOptions options, Entity ^ignoreEntity)
	{
		return RaycastResult(Native::Function::Call<int>(Native::Hash::_CAST_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, static_cast<int>(options), ignoreEntity == nullptr ? 0 : ignoreEntity->Handle, 7));
	}
	RaycastResult World::Raycast(Math::Vector3 source, Math::Vector3 direction, float maxDistance, IntersectOptions options)
	{
		return Raycast(source, direction, maxDistance, options, nullptr);
	}
	RaycastResult World::Raycast(Math::Vector3 source, Math::Vector3 direction, float maxDistance, IntersectOptions options, Entity ^ignoreEntity)
	{
		Math::Vector3 target = source + (direction * maxDistance);
		return RaycastResult(Native::Function::Call<int>(Native::Hash::_CAST_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, static_cast<int>(options), ignoreEntity == nullptr ? 0 : ignoreEntity->Handle, 7));
	}
	RaycastResult World::RaycastCapsule(Math::Vector3 source, Math::Vector3 target, float radius, IntersectOptions options)
	{
		return RaycastCapsule(source, target, radius, options, nullptr);
	}
	RaycastResult World::RaycastCapsule(Math::Vector3 source, Math::Vector3 target, float radius, IntersectOptions options, Entity ^ignoreEntity)
	{
		return RaycastResult(Native::Function::Call<int>(Native::Hash::_CAST_3D_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, radius, static_cast<int>(options), ignoreEntity == nullptr ? 0 : ignoreEntity->Handle, 7));
	}
	RaycastResult World::RaycastCapsule(Math::Vector3 source, Math::Vector3 direction, float maxDistance, float radius, IntersectOptions options)
	{
		return RaycastCapsule(source, direction, maxDistance, radius, options, nullptr);
	}
	RaycastResult World::RaycastCapsule(Math::Vector3 source, Math::Vector3 direction, float maxDistance, float radius, IntersectOptions options, Entity ^ignoreEntity)
	{
		Math::Vector3 target = source + (direction * maxDistance);
		return RaycastResult(Native::Function::Call<int>(Native::Hash::_CAST_3D_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, radius, static_cast<int>(options), ignoreEntity == nullptr ? 0 : ignoreEntity->Handle, 7));
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
		Native::Function::Call(Native::Hash::_DRAW_SPOT_LIGHT_WITH_SHADOW, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, color.R, color.G, color.B, distance, brightness, roundness, radius, fadeout);
	}
	void World::TransitionToWeather(GTA::Weather value, float duration)
	{
		if (Enum::IsDefined(value.GetType(), value) && value != GTA::Weather::Unknown)
		{
			Native::Function::Call(Native::Hash::_SET_WEATHER_TYPE_OVER_TIME, _weatherNames[static_cast<int>(value)], duration);
		}
	}
}