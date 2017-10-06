#pragma once

#include "Vector2.hpp"
#include "Vector3.hpp"
#include "Interface.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Blip;
	ref class Camera;
	ref class Entity;
	ref class Ped;
	ref class Prop;
	ref class Rope;
	ref class Vehicle;
	ref class Pickup;
	value class Model;
	value class RaycastResult;
	#pragma endregion

	public enum class ExplosionType
	{
		Grenade = 0,
		GrenadeL = 1,
		StickyBomb = 2,
		Molotov1 = 3,
		Rocket = 4,
		TankShell = 5,
		HiOctane = 6,
		Car = 7,
		Plane = 8,
		PetrolPump = 9,
		Bike = 10,
		Steam = 11,
		Flame = 12,
		WaterHydrant = 13,
		GasCanister = 14,
		Boat = 15,
		ShipDestroy = 16,
		Truck = 17,
		Bullet = 18,
		SmokeGL = 19,
		SmokeG = 20,
		BZGas = 21,
		Flare = 22,
		GasCanister2 = 23,
		Extinguisher = 24,
		ProgramAR = 25,
		Train = 26,
		Barrel = 27,
		Propane = 28,
		Blimp = 29,
		FlameExplode = 30,
		Tanker = 31,
		PlaneRocket = 32,
		VehicleBullet = 33,
		GasTank = 34,
		FireWork = 35,
		SnowBall = 36,
		ProxMine = 37,
		Valkyrie = 38
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
	public enum class PickupType : System::UInt32
	{
		CustomScript = 0x2C014CA6,
		VehicleCustomScript = 0xA5B8CAA9,
		Parachute = 0x6773257D,
		PortablePackage = 0x80AB931C,
		PortableCrateUnfixed = 0x6E717A95,

		Health = 0x8F707C18,
		HealthSnack = 0x1CD2CF66,
		Armour = 0x4BFB42D1,

		MoneyCase = 0xCE6FDD6B,
		MoneySecurityCase = 0xDE78F17E,
		MoneyVariable = 0xFE18F3AF,
		MoneyMedBag = 0x14568F28,
		MoneyPurse = 0x1E9A99F8,
		MoneyDepBag = 0x20893292,
		MoneyWallet = 0x5DE0AD3E,
		MoneyPaperBag = 0x711D02A4,

		WeaponPistol = 0xF9AFB48F,
		WeaponCombatPistol = 0x8967B4F3,
		WeaponAPPistol = 0x3B662889,
		WeaponSNSPistol = 0xC5B72713,
		WeaponHeavyPistol = 0x9CF13918,
		WeaponMicroSMG = 0x1D9588D3,
		WeaponSMG = 0x3A4C2AD2,
		WeaponMG = 0x85CAA9B1,
		WeaponCombatMG = 0xB2930A14,
		WeaponAssaultRifle = 0xF33C83B0,
		WeaponCarbineRifle = 0xDF711959,
		WeaponAdvancedRifle = 0xB2B5325E,
		WeaponSpecialCarbine = 0x968339D,
		WeaponBullpupRifle = 0x815D66E8,
		WeaponPumpShotgun = 0xA9355DCD,
		WeaponSawnoffShotgun = 0x96B412A3,
		WeaponAssaultShotgun = 0x9299C95B,
		WeaponSniperRifle = 0xFE2A352C,
		WeaponHeavySniper = 0x693583AD,
		WeaponGrenadeLauncher = 0x2E764125,
		WeaponRPG = 0x4D36C349,
		WeaponMinigun = 0x2F36B434,
		WeaponGrenade = 0x5E0683A1,
		WeaponStickyBomb = 0x7C119D58,
		WeaponSmokeGrenade = 0x1CD604C7,
		WeaponMolotov = 0x2DD30479,
		WeaponPetrolCan = 0xC69DE3FF,
		WeaponKnife = 0x278D8734,
		WeaponNightstick = 0x5EA16D74,
		WeaponBat = 0x81EE601E,
		WeaponCrowbar = 0x872DC888,
		WeaponGolfclub = 0x88EAACA7,
		WeaponBottle = 0xFA51ABF5,

		VehicleWeaponPistol = 0xA54AE7B7,
		VehicleWeaponCombatPistol = 0xD0AACEF7,
		VehicleWeaponAPPistol = 0xCC8B3905,
		VehicleWeaponMicroSMG = 0xB86AEE5B,
		VehicleWeaponSawnoffShotgun = 0x2E071B5A,
		VehicleWeaponGrenade = 0xA717F898,
		VehicleWeaponSmokeGrenade = 0x65A7D8E9,
		VehicleWeaponStickyBomb = 0x2C804FE3,
		VehicleWeaponMolotov = 0x84D676D4,
		VehicleHealth = 0x98D79EF,

		AmmoPistol = 0x20796A82,
		AmmoSMG = 0x116FC4E6,
		AmmoMG = 0xDE58E0B3,
		AmmoRifle = 0xE4BD2FC6,
		AmmoShotgun = 0x77F3F2DD,
		AmmoSniper = 0xC02CF125,
		AmmoGrenadeLauncher = 0x881AB0A8,
		AmmoRPG = 0x84837FD7,
		AmmoMinigun = 0xF25A01B9,
		AmmoMissileMP = 0xF99E15D0,
		AmmoBulletMP = 0x550447A9,
		AmmoGrenadeLauncherMP = 0xA421A532
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
		Unknown = -1,
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
		Halloween,
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
			GTA::Weather get();
			void set(GTA::Weather value);
		}
		static property GTA::Weather NextWeather
		{
			GTA::Weather get();
			void set(GTA::Weather value);
		}
		/// <summary>Gets or sets the current transition ratio of the weather.</summary>
		/// <value>A Single representing the current time ratio between 0.0f and 1.0f.</value>
		static property float WeatherTransition
		{
			float get();
			void set(float value);
		}

		static array<Blip ^> ^GetActiveBlips();
		static array<Ped ^> ^GetAllPeds();
		static array<Ped ^> ^GetAllPeds(Model model);
		static array<Ped ^> ^GetNearbyPeds(Ped ^ped, float radius);
		static array<Ped ^> ^GetNearbyPeds(Math::Vector3 position, float radius);
		static array<Ped ^> ^GetNearbyPeds(Math::Vector3 position, float radius, Model model);
		static array<Vehicle ^> ^GetAllVehicles();
		static array<Vehicle ^> ^GetAllVehicles(Model model);
		static array<Vehicle ^> ^GetNearbyVehicles(Ped ^ped, float radius);
		static array<Vehicle ^> ^GetNearbyVehicles(Math::Vector3 position, float radius);
		static array<Vehicle ^> ^GetNearbyVehicles(Math::Vector3 position, float radius, Model model);
		static array<Prop ^> ^GetAllProps();
		static array<Prop ^> ^GetAllProps(Model model);
		static array<Prop ^> ^GetNearbyProps(Math::Vector3 position, float radius);
		static array<Prop ^> ^GetNearbyProps(Math::Vector3 position, float radius, Model model);
		static array<Entity ^> ^GetAllEntities();
		static array<Entity ^> ^GetNearbyEntities(Math::Vector3 position, float radius);
		static Ped ^GetClosestPed(Math::Vector3 position, float radius);
		static Vehicle ^GetClosestVehicle(Math::Vector3 position, float radius);
		generic <typename T> where T : ISpatial
		static T GetClosest(Math::Vector3 position, ... array<T> ^spatials);
		static float GetDistance(Math::Vector3 origin, Math::Vector3 destination);
		static float CalculateTravelDistance(Math::Vector3 origin, Math::Vector3 destination);
		static float GetGroundHeight(Math::Vector2 position);
		static float GetGroundHeight(Math::Vector3 position);
		static Math::Vector3 GetWaypointPosition();
		static RaycastResult GetCrosshairCoordinates();
		static Math::Vector3 GetSafeCoordForPed(Math::Vector3 position);
		static Math::Vector3 GetSafeCoordForPed(Math::Vector3 position, bool sidewalk);
		static Math::Vector3 GetSafeCoordForPed(Math::Vector3 position, bool sidewalk, int flags);
		static Math::Vector3 GetNextPositionOnStreet(Math::Vector3 position);
		static Math::Vector3 GetNextPositionOnStreet(Math::Vector2 position, bool unoccupied);
		static Math::Vector3 GetNextPositionOnStreet(Math::Vector3 position, bool unoccupied);
		static Math::Vector3 GetNextPositionOnSidewalk(Math::Vector2 position);
		static Math::Vector3 GetNextPositionOnSidewalk(Math::Vector3 position);
		static System::String ^GetZoneName(Math::Vector2 position);
		static System::String ^GetZoneName(Math::Vector3 position);
		static System::String ^GetZoneNameLabel(Math::Vector2 position);
		static System::String ^GetZoneNameLabel(Math::Vector3 position);
		static System::String ^GetStreetName(Math::Vector2 position);
		static System::String ^GetStreetName(Math::Vector3 position);

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
		static void ShootBullet(Math::Vector3 sourcePosition, Math::Vector3 targetPosition, Ped ^owner, Model model, int damage, float speed);
		static void AddExplosion(Math::Vector3 position, ExplosionType type, float radius, float cameraShake);
		static void AddExplosion(Math::Vector3 position, ExplosionType type, float radius, float cameraShake, bool Aubidble, bool Invis);
		static void AddOwnedExplosion(Ped ^ped, Math::Vector3 position, ExplosionType type, float radius, float cameraShake);
		static void AddOwnedExplosion(Ped ^ped, Math::Vector3 position, ExplosionType type, float radius, float cameraShake, bool Aubidble, bool Invis);
		static Rope ^AddRope(RopeType type, Math::Vector3 position, Math::Vector3 rotation, float length, float minLength, bool breakable);
		static void DestroyAllCameras();
		static void SetBlackout(bool enable);

		static int AddRelationshipGroup(System::String ^groupName);
		static void RemoveRelationshipGroup(int group);
		static Relationship GetRelationshipBetweenGroups(int group1, int group2);
		static void SetRelationshipBetweenGroups(Relationship relationship, int group1, int group2);
		static void ClearRelationshipBetweenGroups(Relationship relationship, int group1, int group2);

		static RaycastResult Raycast(Math::Vector3 source, Math::Vector3 target, IntersectOptions options);
		static RaycastResult Raycast(Math::Vector3 source, Math::Vector3 target, IntersectOptions options, Entity ^ignoreEntity);
		static RaycastResult Raycast(Math::Vector3 source, Math::Vector3 direction, float maxDistance, IntersectOptions options);
		static RaycastResult Raycast(Math::Vector3 source, Math::Vector3 direction, float maxDistance, IntersectOptions options, Entity ^ignoreEntity);
		static RaycastResult RaycastCapsule(Math::Vector3 source, Math::Vector3 target, float radius, IntersectOptions options);
		static RaycastResult RaycastCapsule(Math::Vector3 source, Math::Vector3 target, float radius, IntersectOptions options, Entity ^ignoreEntity);
		static RaycastResult RaycastCapsule(Math::Vector3 source, Math::Vector3 direction, float maxDistance, float radius, IntersectOptions options);
		static RaycastResult RaycastCapsule(Math::Vector3 source, Math::Vector3 direction, float maxDistance, float radius, IntersectOptions options, Entity ^ignoreEntity);

		static void DrawMarker(MarkerType type, Math::Vector3 pos, Math::Vector3 dir, Math::Vector3 rot, Math::Vector3 scale, System::Drawing::Color color);
		static void DrawMarker(MarkerType type, Math::Vector3 pos, Math::Vector3 dir, Math::Vector3 rot, Math::Vector3 scale, System::Drawing::Color color, bool bobUpAndDown, bool faceCamY, int unk2, bool rotateY, System::String ^textueDict, System::String ^textureName, bool drawOnEnt);
		static void DrawLightWithRange(Math::Vector3 position, System::Drawing::Color color, float range, float intensity);
		static void DrawSpotLight(Math::Vector3 pos, Math::Vector3 dir, System::Drawing::Color color, float distance, float brightness, float roundness, float radius, float fadeout);
		static void DrawSpotLightWithShadow(Math::Vector3 pos, Math::Vector3 dir, System::Drawing::Color color, float distance, float brightness, float roundness, float radius, float fadeout);

		static void TransitionToWeather(GTA::Weather weather, float duration);

	internal:
		static initonly array<System::String ^> ^_weatherNames = { "EXTRASUNNY", "CLEAR", "CLOUDS", "SMOG", "FOGGY", "OVERCAST", "RAIN", "THUNDER", "CLEARING", "NEUTRAL", "SNOW", "BLIZZARD", "SNOWLIGHT", "XMAS", "HALLOWEEN" };
		static initonly System::Globalization::GregorianCalendar ^_gregorianCalendar = gcnew System::Globalization::GregorianCalendar();
	};
}