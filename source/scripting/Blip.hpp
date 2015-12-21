#pragma once

#include "Vector3.hpp"

namespace GTA
{
	public enum class BlipColor
	{
		White = 0,
		Red = 1,
		Green = 2,
		Blue = 3,
		Yellow = 66,
	};
	public enum class BlipSprite
	{
		Standard = 1,
		BigBlip = 2,
		PoliceOfficer = 3,
		PoliceArea = 4,
		Square = 5,
		Player = 6,
		North = 7,
		Waypoint = 8,
		BigCircle = 9,
		BigCircleOutline = 10,
		ArrowUpOutlined = 11,
		ArrowDownOutlined = 12,
		ArrowUp = 13,
		ArrowDown = 14,
		PoliceHelicopterAnimated = 15,
		Jet = 16,
		Number1 = 17,
		Number2 = 18,
		Number3 = 19,
		Number4 = 20,
		Number5 = 21,
		Number6 = 22,
		Number7 = 23,
		Number8 = 24,
		Number9 = 25,
		Number10 = 26,
		GTAOCrew = 27,
		GTAOFriendly = 28,
		Lift = 36,
		RaceFinish = 38,
		Safehouse = 40,
		PoliceOfficer2 = 41,
		PoliceCarDot = 42,
		PoliceHelicopter = 43,
		ChatBubble = 47,
		Garage2 = 50,
		Drugs = 51,
		Store = 52,
		PoliceCar = 56,
		PolicePlayer = 58,
		PoliceStation = 60,
		Hospital = 61,
		Helicopter = 64,
		StrangersAndFreaks = 65,
		ArmoredTruck = 66,
		TowTruck = 68,
		Barber = 71,
		LosSantosCustoms = 72,
		Clothes = 73,
		TattooParlor = 75,
		Simeon = 76,
		Lester = 77,
		Michael = 78,
		Trevor = 79,
		Rampage = 84,
		VinewoodTours = 85,
		Lamar = 86,
		Franklin = 88,
		Chinese = 89,
		Airport = 90,
		Bar = 93,
		BaseJump = 94,
		CarWash = 100,
		ComedyClub = 102,
		Dart = 103,
		FIB = 106,
		DollarSign = 108,
		Golf = 109,
		AmmuNation = 110,
		Exile = 112,
		ShootingRange = 119,
		Solomon = 120,
		StripClub = 121,
		Tennis = 122,
		Triathlon = 126,
		OffRoadRaceFinish = 127,
		Key = 134,
		MovieTheater = 135,
		Music = 136,
		Marijuana = 140,
		Hunting = 141,
		ArmsTraffickingGround = 147,
		Nigel = 149,
		AssaultRifle = 150,
		Bat = 151,
		Grenade = 152,
		Health = 153,
		Knife = 154,
		Molotov = 155,
		Pistol = 156,
		RPG = 157,
		Shotgun = 158,
		SMG = 159,
		Sniper = 160,
		SonicWave = 161,
		PointOfInterest = 162,
		GTAOPassive = 163,
		GTAOUsingMenu = 164,
		Link = 171,
		Minigun = 173,
		GrenadeLauncher = 174,
		Armor = 175,
		Castle = 176,
		Camera = 184,
		Handcuffs = 188,
		Yoga = 197,
		Cab = 198,
		Number11 = 199,
		Number12 = 200,
		Number13 = 201,
		Number14 = 202,
		Number15 = 203,
		Number16 = 204,
		Shrink = 205,
		Epsilon = 206,
		PersonalVehicleCar = 225,
		PersonalVehicleBike = 226,
		Custody = 237,
		ArmsTraffickingAir = 251,
		Fairground = 266,
		PropertyManagement = 267,
		Altruist = 269,
		Enemy = 270,
		Chop = 273,
		Dead = 274,
		Hooker = 279,
		Friend = 280,
		BountyHit = 303,
		GTAOMission = 304,
		GTAOSurvival = 305,
		CrateDrop = 306,
		PlaneDrop = 307,
		Sub = 308,
		Race = 309,
		Deathmatch = 310,
		ArmWrestling = 311,
		AmmuNationShootingRange = 313,
		RaceAir = 314,
		RaceCar = 315,
		RaceSea = 316,
		GarbageTruck = 318,
		SafehouseForSale = 350,
		Package = 351,
		MartinMadrazo = 352,
		EnemyHelicopter = 353,
		Boost = 354,
		Devin = 355,
		Marina = 356,
		Garage = 357,
		GolfFlag = 358,
		Hangar = 359,
		Helipad = 360,
		JerryCan = 361,
		Masks = 362,
		HeistSetup = 363,
		Incapacitated = 364,
		PickupSpawn = 365,
		BoilerSuit = 366,
		Completed = 367,
		Rockets = 368,
		GarageForSale = 369,
		HelipadForSale = 370,
		MarinaForSale = 371,
		HangarForSale = 372,
		Business = 374,
		BusinessForSale = 375,
		RaceBike = 376,
		Parachute = 377,
		TeamDeathmatch = 378,
		RaceFoot = 379,
		VehicleDeathmatch = 380,
		Barry = 381,
		Dom = 382,
		MaryAnn = 383,
		Cletus = 384,
		Josh = 385,
		Minute = 386,
		Omega = 387,
		Tonya = 388,
		Paparazzo = 389,
		Crosshair = 390,
		Creator = 398,
		CreatorDirection = 399,
		Abigail = 400,
		Blimp = 401,
		Repair = 402,
		Testosterone = 403,
		Dinghy = 404,
		Fanatic = 405,
		Information = 407,
		CaptureBriefcase = 408,
		LastTeamStanding = 409,
		Boat = 410,
		CaptureHouse = 411,
		JerryCan2 = 415,
		RP = 416,
		GTAOPlayerSafehouse = 417,
		GTAOPlayerSafehouseDead = 418,
		CaptureAmericanFlag = 419,
		CaptureFlag = 420,
		Tank = 421,
		HelicopterAnimated = 422,
		Plane = 423,
		PlayerNoColor = 425,
		GunCar = 426,
		Speedboat = 427,
		Heist = 428,
		Stopwatch = 430,
		DollarSignCircled = 431,
		Crosshair2 = 432,
		DollarSignSquared = 434,
	};

	public ref class Blip sealed
	{
	public:
		Blip(int handle);

		property int Handle
		{
			int get();
		}
		property int Alpha
		{
			int get();
			void set(int alpha);
		}
		property BlipColor Color
		{
			BlipColor get();
			void set(BlipColor color);
		}
		property bool IsFlashing
		{
			bool get();
			void set(bool value);
		}
		property bool IsFriendly
		{
			void set(bool value);
		}
		property bool IsOnMinimap
		{
			bool get();
		}
		property bool IsShortRange
		{
			bool get();
			void set(bool value);
		}
		property System::String ^Name
		{
			void set(System::String ^value);
		}
		property Math::Vector3 Position
		{
			Math::Vector3 get();
			void set(Math::Vector3 value);
		}
		property float Scale
		{
			void set(float scale);
		}
		property bool ShowRoute
		{
			void set(bool value);
		}
		property BlipSprite Sprite
		{
			BlipSprite get();
			void set(BlipSprite sprite);
		}
		property int Type
		{
			int get();
		}

		void HideNumber();
		void ShowNumber(int number);

		bool Exists();
		static bool Exists(Blip ^blip);
		void Remove();
		virtual bool Equals(Blip ^blip);

		virtual inline int GetHashCode() override
		{
			return Handle;
		}
		static inline bool operator==(Blip ^left, Blip ^right)
		{
			if (ReferenceEquals(left, nullptr))
			{
				return ReferenceEquals(right, nullptr);
			}

			return left->Equals(right);
		}
		static inline bool operator!=(Blip ^left, Blip ^right)
		{
			return !operator==(left, right);
		}

	private:
		int _handle;
	};
}