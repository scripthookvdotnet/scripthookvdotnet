#pragma once

#include "Vector3.hpp"

namespace GTA
{
	value class Model;
	ref class Ped;
	ref class Vehicle;
	ref class Camera;
	ref class Blip;
	ref class Prop;
	ref class Rope;

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
	public enum class ExplosionType
	{
		SmallExplosion1 = 1,
		SmallExplosion2 = 2,
		Molotov1 = 3,
		SmallExplosionWithFire1 = 4,
		SmallExplosion3 = 5,
		SmallExplosionWithFire2 = 6,
		SmallExplosion4 = 7,
		ExplosionWithFire1 = 8,
		ExplosionWithFire2 = 9,
		SmallExplosionWithFire3 = 10,
		ValveAir1 = 11,
		ValveFire1 = 12,
		ValveWater1 = 13,
		ValveFire2 = 14,
		ExplosionWithFire3 = 15,
		ExplosionWithFire4 = 16,
		Explosion1 = 17,
		SmallExplosion5 = 18,
		Smoke1 = 19,
		Gas1 = 20,
		Gas2 = 21,
		SignalFire = 22,
		ExplosionWithFire5 = 23,
		ValveAir2 = 24,
		SmallExplosion6 = 25,
		Explosion2 = 26,
		ExplosionWithFire6 = 27,
		Explosion3 = 28,
		BigExplosion1 = 29,
		ValveFire3 = 30,
		Explosion4 = 31,
		Explosion5 = 32,
		SmallExplosion7 = 33,
		Explosion6 = 34
	};
	public enum class Relationship
	{
		Hate = 5,
		Dislike = 4,
		Neutral = 3,
		Like = 2,
		Respect = 1,
		Companion = 0,
		Pedestrians = 255 // or neutral
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
                
                static array<Ped ^> ^GetNearbyPeds(Math::Vector3 position, float radius);
		static array<Ped ^> ^GetNearbyPeds(Ped ^ped, float radius);
		static array<Ped ^> ^GetNearbyPeds(Ped ^ped, float radius, int maxAmount);
		static array<Vehicle ^> ^GetNearbyVehicles(Math::Vector3 position, float radius);
		static array<Vehicle ^> ^GetNearbyVehicles(Ped ^ped, float radius);
		static array<Vehicle ^> ^GetNearbyVehicles(Ped ^ped, float radius, int maxAmount);
		static Ped ^GetClosestPed(Math::Vector3 position, float radius);
		static Vehicle ^GetClosestVehicle(Math::Vector3 position, float radius);
		static float GetDistance(Math::Vector3 origin, Math::Vector3 destination);
		static float GetGroundZ(Math::Vector3 position);

		static Ped ^CreatePed(Model model, Math::Vector3 position);
		static Ped ^CreatePed(Model model, Math::Vector3 position, float heading);
		static Ped ^CreateRandomPed(Math::Vector3 position);
		static Vehicle ^CreateVehicle(Model model, Math::Vector3 position);
		static Vehicle ^CreateVehicle(Model model, Math::Vector3 position, float heading);
		static Prop ^CreateProp(Model model, Math::Vector3 position, bool dynamic, bool placeOnGround);
		static Prop ^CreateProp(Model model, Math::Vector3 position, Math::Vector3 rotation, bool dynamic, bool placeOnGround);

		static void AddExplosion(Math::Vector3 position, ExplosionType type, float radius, float cameraShake);
		static void AddOwnedExplosion(Ped ^ped, Math::Vector3 position, ExplosionType type, float radius, float cameraShake);
		static Rope ^AddRope(Math::Vector3 position, Math::Vector3 rotation, double lenght, int type, double maxLenght, double minLenght, double p10, bool p11, bool p12, bool p13, double p14, bool breakable);

		static Camera ^CreateCamera(Math::Vector3 position, Math::Vector3 rotation, float fov);
		static property Camera ^RenderingCamera
		{
			Camera ^get();
			void set(Camera ^renderingCamera);
		}
		static void DestroyAllCameras();

		static Blip ^CreateBlip(Math::Vector3 position);
		static Blip ^CreateBlip(Math::Vector3 position, float radius);
		static array<Blip ^> ^GetActiveBlips();

		static int AddRelationShipGroup(System::String ^groupName);
		static void RemoveRelationShipGroup(int group);
		static void SetRelationshipBetweenGroups(Relationship relationship, int group1, int group2);
		static void ClearRelationshipBetweenGroups(Relationship relationship, int group1, int group2);
		static Relationship GetRelationshipBetweenGroups(int group1, int group2);

	internal:
		static initonly array<System::String ^> ^sWeatherNames = { "EXTRASUNNY", "CLEAR", "CLOUDS", "SMOG", "FOGGY", "OVERCAST", "RAIN", "THUNDER", "CLEARING", "NEUTRAL", "SNOW", "BLIZZARD", "SNOWLIGHT", "XMAS" };
	};
}
