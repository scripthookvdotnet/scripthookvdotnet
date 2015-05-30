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
		property Math::Vector3 Position
		{
			Math::Vector3 get(); // Vector3 GET_BLIP_INFO_ID_COORD(Any p0) // 0xB7374A66
			void set(Math::Vector3 value); // void SET_BLIP_COORDS(Any p0, Any p1, Any p2, Any p3) // 0x680A34D4
		}
		property float Scale
		{
			void set(float scale); // void SET_BLIP_SCALE(int BlipID, float Scale) // 0x1E6EC434
		}
		property bool IsFlashing
		{
			bool get(); // void SET_BLIP_FLASHES(Any p0, Any p1) // 0xC0047F15
			void set(bool value); // BOOL IS_BLIP_FLASHING(Any p0) // 0x52E111D7
		}
		property BlipColor Color
		{
			BlipColor get(); // Any GET_BLIP_COLOUR(Any p0) // 0xDD6A1E54
			void set(BlipColor color); // void SET_BLIP_COLOUR(int BlipID, int Color) // 0xBB3C5A41
		}
		property int Alpha
		{
			int get(); // Any GET_BLIP_ALPHA(Any p0) // 0x297AF6C8
			void set(int alpha); // void SET_BLIP_ALPHA(Any p0, Any p1) // 0xA791FCCD
		}
		property bool ShowRoute
		{
			void set(bool value); // void SET_BLIP_ROUTE(Object blip, BOOL enabled) // 0x3E160C90
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
		property bool IsOnMinimap
		{
			bool get();
		}
		property bool IsShortRange
		{
			bool get();
			void set(bool value);
		}

		bool Exists(); // BOOL DOES_BLIP_EXIST(Any p0) // 0xAE92DD96
		void SetAsFriendly(); // void SET_BLIP_AS_FRIENDLY(int BlipID, BOOL toggle) // 0xF290CFD8
		void SetAsHostile(); // void SET_BLIP_AS_FRIENDLY(int BlipID, BOOL toggle) // 0xF290CFD8
		void Remove(); // void REMOVE_BLIP(int BlipID) // 0xD8C3C1CD
		void ShowNumber(int number);
		void HideNumber();

		// BOOL IS_BLIP_ON_MINIMAP(Any p0) // 0x258CBA3A
		// void SET_BLIP_AS_SHORT_RANGE(Any p0, Any p1) // 0x5C67725E
		// Vector3 GET_BLIP_COORDS(Any p0) // 0xEF6FF47B
		// void SHOW_NUMBER_ON_BLIP(Any p0, Any p1) // 0x7BFC66C6
		// void HIDE_NUMBER_ON_BLIP(Any p0) // 0x0B6D610D
		// void SET_NEW_WAYPOINT(Any p0, Any p1) // 0x8444E1F0
		// BOOL IS_WAYPOINT_ACTIVE() // 0x5E4DF47B
		// void SET_WAYPOINT_OFF() // 0xB3496E1B
		// void SET_POLICE_RADAR_BLIPS(BOOL Toggle) // 0x8E114B10
		// void SET_THIS_SCRIPT_CAN_REMOVE_BLIPS_CREATED_BY_ANY_SCRIPT(Any p0) // 0xD06F1720
		// void BLIP_SIREN(Any p0) // 0xC0EB6924
		// Any GET_NUMBER_OF_ACTIVE_BLIPS() // 0x144020FA
		// Any GET_NEXT_BLIP_INFO_ID(Any p0) // 0x9356E92F
		// Any GET_FIRST_BLIP_INFO_ID(Any p0) // 0x64C0273D
		// Object GET_BLIP_FROM_ENTITY(Entity entity) // 0x005A2A47
		//  Any ADD_BLIP_FOR_ENTITY(Entity entity) // 0x30822554
		// Any ADD_BLIP_FOR_COORD(float x, float y, float z) // 0xC6F43D0E
		// void SET_BLIP_SPRITE(Object blip, int spriteId) // 0x8DBBB0B9
		// Any GET_BLIP_SPRITE(Any p0) // 0x72FF2E73
		// BOOL DOES_PED_HAVE_AI_BLIP(Any p0) // 0x3BE1257F
	private:
		int mHandle;
	};
}