#pragma once

#include "Vector2.hpp"
#include "Vector3.hpp"

namespace GTA
{
	ref class Blip;
	ref class Camera;
	ref class Entity;
	ref class Ped;
	ref class Prop;
	ref class Rope;
	ref class Vehicle;
	ref class Pickup;
	enum class PickupType : System::UInt32;
	value class Model;
	value class RaycastResult;

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
	public enum class IntersectOptions
	{
		Everything = -1,
		Map = 1,
		Mission_Entities = 2,
		Peds1 = 12,//4 and 8 both seem to be peds
		Objects = 16,
		Unk1 = 32,
		Unk2 = 64,
		Unk3 = 128,
		Vegetation = 256,
		Unk4 = 512
	};
	public enum class MarkerType
	{
		UpsideDownCone = 0,
		VerticalCylinder = 1,
		ThickChevronUp = 2,
		ThinChevronUp = 3,
		CheckeredFlagRect = 4,
		CheckeredFlagCircle = 5,
		VerticleCircle = 6,
		PlaneModel = 7,
		LostMCDark = 8,
		LostMCLight = 9,
		Number0 = 10,
		Number1 = 11,
		Number2 = 12,
		Number3 = 13,
		Number4 = 14,
		Number5 = 15,
		Number6 = 16,
		Number7 = 17,
		Number8 = 18,
		Number9 = 19,
		ChevronUpx1 = 20,
		ChevronUpx2 = 21,
		ChevronUpx3 = 22,
		HorizontalCircleFat = 23,
		ReplayIcon = 24,
		HorizontalCircleSkinny = 25,
		HorizontalCircleSkinny_Arrow = 26,
		HorizontalSplitArrowCircle = 27,
		DebugSphere = 28
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
	public enum class RopeType
	{
		Normal = 4,
	};
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

	public value struct Street {
		System::String ^Name;
		System::String ^Crossing;
	};
	public value struct Zone {
		System::String ^Code;
		System::String ^Name;
	};
	public enum class ZoneID {
		AIRP,
		ALAMO,
		ALTA,
		ARMYB,
		BANHAMC,
		BANNING,
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

	public ref class World sealed abstract
	{
	public:
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
		static property Camera ^RenderingCamera
		{
			Camera ^get();
			void set(Camera ^renderingCamera);
		}
		static property GTA::Weather Weather
		{
			void set(GTA::Weather value);
		}

		static array<Blip ^> ^GetActiveBlips();
		static array<Ped ^> ^GetAllPeds();
		static array<Ped ^> ^GetNearbyPeds(Ped ^ped, float radius);
		static array<Ped ^> ^GetNearbyPeds(Ped ^ped, float radius, int maxAmount);
		static array<Ped ^> ^GetNearbyPeds(Math::Vector3 position, float radius);
		static array<Vehicle ^> ^GetAllVehicles();
		static array<Vehicle ^> ^GetNearbyVehicles(Ped ^ped, float radius);
		static array<Vehicle ^> ^GetNearbyVehicles(Ped ^ped, float radius, int maxAmount);
		static array<Vehicle ^> ^GetNearbyVehicles(Math::Vector3 position, float radius);
		static array<Prop ^> ^GetAllProps();
		static array<Entity ^> ^GetAllEntities();
		static Ped ^GetClosestPed(Math::Vector3 position, float radius);
		static Vehicle ^GetClosestVehicle(Math::Vector3 position, float radius);
		static float GetDistance(Math::Vector3 origin, Math::Vector3 destination);
		static float GetGroundHeight(Math::Vector2 position);
		static float GetGroundHeight(Math::Vector3 position);

		static Blip ^CreateBlip(Math::Vector3 position);
		static Blip ^CreateBlip(Math::Vector3 position, float radius);
		static Camera ^CreateCamera(Math::Vector3 position, Math::Vector3 rotation, float fov);
		static Ped ^CreatePed(Model model, Math::Vector3 position);
		static Ped ^CreatePed(Model model, Math::Vector3 position, float heading);
		static Ped ^CreateRandomPed(Math::Vector3 position);
		static Vehicle ^CreateVehicle(Model model, Math::Vector3 position);
		static Vehicle ^CreateVehicle(Model model, Math::Vector3 position, float heading);
		static Prop ^CreateProp(Model model, Math::Vector3 position, bool dynamic, bool placeOnGround);
		static Prop ^CreateProp(Model model, Math::Vector3 position, Math::Vector3 rotation, bool dynamic, bool placeOnGround);
		static Pickup ^CreatePickup(PickupType type, Math::Vector3 position, Model model, int value);
		static Pickup ^CreatePickup(PickupType type, Math::Vector3 position, Math::Vector3 rotation, Model model, int value);
		static Prop ^CreateAmbientPickup(PickupType type, Math::Vector3 position, Model model, int value);

		static void ShootBullet(Math::Vector3 sourcePosition, Math::Vector3 targetPosition, Ped ^owner, Model model, int damage);
		static void AddExplosion(Math::Vector3 position, ExplosionType type, float radius, float cameraShake);
		static void AddOwnedExplosion(Ped ^ped, Math::Vector3 position, ExplosionType type, float radius, float cameraShake);
		static Rope ^AddRope(RopeType type, Math::Vector3 position, Math::Vector3 rotation, float length, float minLength, bool breakable);
		static void DestroyAllCameras();
		static void SetBlackout(bool enable);

		static int AddRelationshipGroup(System::String ^groupName);
		static void RemoveRelationshipGroup(int group);
		static Relationship GetRelationshipBetweenGroups(int group1, int group2);
		static void SetRelationshipBetweenGroups(Relationship relationship, int group1, int group2);
		static void ClearRelationshipBetweenGroups(Relationship relationship, int group1, int group2);

		static RaycastResult Raycast(Math::Vector3 source, Math::Vector3 target, IntersectOptions options);
		static RaycastResult Raycast(Math::Vector3 source, Math::Vector3 target, IntersectOptions options, Entity ^entity);

		static void DrawMarker(MarkerType type, Math::Vector3 pos, Math::Vector3 dir, Math::Vector3 rot, Math::Vector3 scale, System::Drawing::Color color);
		static void DrawMarker(MarkerType type, Math::Vector3 pos, Math::Vector3 dir, Math::Vector3 rot, Math::Vector3 scale, System::Drawing::Color color, bool bobUpAndDown, bool faceCamY, int unk2, bool rotateY, System::String ^textueDict, System::String ^textureName, bool drawOnEnt);

		static void DrawLightWithRange(Math::Vector3 position, System::Drawing::Color color, float range, float intensity);
		static void DrawSpotLight(Math::Vector3 pos, Math::Vector3 dir, System::Drawing::Color color, float distance, float brightness, float roundness, float radius, float fadeout);
		static void DrawSpotLightWithShadow(Math::Vector3 pos, Math::Vector3 dir, System::Drawing::Color color, float distance, float brightness, float roundness, float radius, float fadeout);

		static float CalculateTravelDistance(Math::Vector3 origin, Math::Vector3 destination);
		static Math::Vector3 GetNextPositionOnSidewalk(Math::Vector2 position);
		static Math::Vector3 GetNextPositionOnSidewalk(Math::Vector3 position);
		static Math::Vector3 GetNextPositionOnStreet(Math::Vector3 position);
		static Math::Vector3 GetNextPositionOnStreet(Math::Vector2 position, bool unoccupied);
		static Math::Vector3 GetNextPositionOnStreet(Math::Vector3 position, bool unoccupied);
		static Math::Vector3 GetSafeCoordForPed(Math::Vector3 position);
		static Math::Vector3 GetSafeCoordForPed(Math::Vector3 position, bool sidewalk);
		static Math::Vector3 GetSafeCoordForPed(Math::Vector3 position, bool sidewalk, int flags);
		static Street GetStreetAtCoord(Math::Vector2 position);
		static Street GetStreetAtCoord(Math::Vector3 position);
		static Zone GetZone(System::String ^code);
		static Zone GetZone(Math::Vector2 position);
		static Zone GetZone(Math::Vector3 position);
		static System::String ^GetZoneName(Math::Vector2 position);
		static System::String ^GetZoneName(Math::Vector3 position);

	internal:
		static initonly array<System::String ^> ^sWeatherNames = { "EXTRASUNNY", "CLEAR", "CLOUDS", "SMOG", "FOGGY", "OVERCAST", "RAIN", "THUNDER", "CLEARING", "NEUTRAL", "SNOW", "BLIZZARD", "SNOWLIGHT", "XMAS" };
	};
}