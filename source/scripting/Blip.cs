using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public enum BlipColor
	{
		White,
		Red,
		Green,
		Blue,
		Yellow = 66
	}
	public enum BlipSprite
	{
		Standard = 1,
		BigBlip,
		PoliceOfficer,
		PoliceArea,
		Square,
		Player,
		North,
		Waypoint,
		BigCircle,
		BigCircleOutline,
		ArrowUpOutlined,
		ArrowDownOutlined,
		ArrowUp,
		ArrowDown,
		PoliceHelicopterAnimated,
		Jet,
		Number1,
		Number2,
		Number3,
		Number4,
		Number5,
		Number6,
		Number7,
		Number8,
		Number9,
		Number10,
		GTAOCrew,
		GTAOFriendly,
		Lift = 36,
		RaceFinish = 38,
		Safehouse = 40,
		PoliceOfficer2,
		PoliceCarDot,
		PoliceHelicopter,
		ChatBubble = 47,
		Garage2 = 50,
		Drugs,
		Store,
		PoliceCar = 56,
		PolicePlayer = 58,
		PoliceStation = 60,
		Hospital,
		Helicopter = 64,
		StrangersAndFreaks,
		ArmoredTruck,
		TowTruck = 68,
		Barber = 71,
		LosSantosCustoms,
		Clothes,
		TattooParlor = 75,
		Simeon,
		Lester,
		Michael,
		Trevor,
		Rampage = 84,
		VinewoodTours,
		Lamar,
		Franklin = 88,
		Chinese,
		Airport,
		Bar = 93,
		BaseJump,
		CarWash = 100,
		ComedyClub = 102,
		Dart,
		FIB = 106,
		DollarSign = 108,
		Golf,
		AmmuNation,
		Exile = 112,
		ShootingRange = 119,
		Solomon,
		StripClub,
		Tennis,
		Triathlon = 126,
		OffRoadRaceFinish,
		Key = 134,
		MovieTheater,
		Music,
		Marijuana = 140,
		Hunting,
		ArmsTraffickingGround = 147,
		Nigel = 149,
		AssaultRifle,
		Bat,
		Grenade,
		Health,
		Knife,
		Molotov,
		Pistol,
		RPG,
		Shotgun,
		SMG,
		Sniper,
		SonicWave,
		PointOfInterest,
		GTAOPassive,
		GTAOUsingMenu,
		Link = 171,
		Minigun = 173,
		GrenadeLauncher,
		Armor,
		Castle,
		Camera = 184,
		Handcuffs = 188,
		Yoga = 197,
		Cab,
		Number11,
		Number12,
		Number13,
		Number14,
		Number15,
		Number16,
		Shrink,
		Epsilon,
		PersonalVehicleCar = 225,
		PersonalVehicleBike,
		Custody = 237,
		ArmsTraffickingAir = 251,
		Fairground = 266,
		PropertyManagement,
		Altruist = 269,
		Enemy,
		Chop = 273,
		Dead,
		Hooker = 279,
		Friend,
		BountyHit = 303,
		GTAOMission,
		GTAOSurvival,
		CrateDrop,
		PlaneDrop,
		Sub,
		Race,
		Deathmatch,
		ArmWrestling,
		AmmuNationShootingRange = 313,
		RaceAir,
		RaceCar,
		RaceSea,
		GarbageTruck = 318,
		SafehouseForSale = 350,
		Package,
		MartinMadrazo,
		EnemyHelicopter,
		Boost,
		Devin,
		Marina,
		Garage,
		GolfFlag,
		Hangar,
		Helipad,
		JerryCan,
		Masks,
		HeistSetup,
		Incapacitated,
		PickupSpawn,
		BoilerSuit,
		Completed,
		Rockets,
		GarageForSale,
		HelipadForSale,
		MarinaForSale,
		HangarForSale,
		Business = 374,
		BusinessForSale,
		RaceBike,
		Parachute,
		TeamDeathmatch,
		RaceFoot,
		VehicleDeathmatch,
		Barry,
		Dom,
		MaryAnn,
		Cletus,
		Josh,
		Minute,
		Omega,
		Tonya,
		Paparazzo,
		Crosshair,
		Creator = 398,
		CreatorDirection,
		Abigail,
		Blimp,
		Repair,
		Testosterone,
		Dinghy,
		Fanatic,
		Information = 407,
		CaptureBriefcase,
		LastTeamStanding,
		Boat,
		CaptureHouse,
		JerryCan2 = 415,
		RP,
		GTAOPlayerSafehouse,
		GTAOPlayerSafehouseDead,
		CaptureAmericanFlag,
		CaptureFlag,
		Tank,
		HelicopterAnimated,
		Plane,
		PlayerNoColor = 425,
		GunCar,
		Speedboat,
		Heist,
		Stopwatch = 430,
		DollarSignCircled,
		Crosshair2,
		DollarSignSquared = 434
	}

	public sealed class Blip : PoolObject, IEquatable<Blip>
	{
		public Blip(int handle) : base(handle)
		{
		}

		public Vector3 Position
		{
			get
			{
				return Function.Call<Vector3>(Hash.GET_BLIP_INFO_ID_COORD, Handle);
			}
			set
			{
				Function.Call(Hash.SET_BLIP_COORDS, Handle, value.X, value.Y, value.Z);
			}
		}
		public int Rotation
		{
			set
			{
				Function.Call(Hash.SET_BLIP_ROTATION, Handle, value);
			}
		}
		public float Scale
		{
			set
			{
				Function.Call(Hash.SET_BLIP_SCALE, Handle, value);
			}
		}

		public int Type
		{
			get
			{
				return Function.Call<int>(Hash.GET_BLIP_INFO_ID_TYPE, Handle);
			}
		}
		public int Alpha
		{
			get
			{
				return Function.Call<int>(Hash.GET_BLIP_ALPHA, Handle);
			}
			set
			{
				Function.Call(Hash.SET_BLIP_ALPHA, Handle, value);
			}
		}
		public int Priority
		{
			set
			{
				Function.Call(Hash.SET_BLIP_PRIORITY, Handle, value);
			}
		}
		public int NumberLabel
		{
			set
			{
				Function.Call(Hash.SHOW_NUMBER_ON_BLIP, Handle, value);
			}
		}
		public BlipColor Color
		{
			get
			{
				return (BlipColor)Function.Call<int>(Hash.GET_BLIP_COLOUR, Handle);
			}
			set
			{
				Function.Call(Hash.SET_BLIP_COLOUR, Handle, value);
			}
		}
		public BlipSprite Sprite
		{
			get
			{
				return (BlipSprite)Function.Call<int>(Hash.GET_BLIP_SPRITE, Handle);
			}
			set
			{
				Function.Call(Hash.SET_BLIP_SPRITE, Handle, value);
			}
		}
		public string Name
		{
			set
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, "STRING");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, value);
				Function.Call(Hash.END_TEXT_COMMAND_SET_BLIP_NAME, Handle);
			}
		}
		public Entity Entity
		{
			get
			{
				return Function.Call<Entity>(Hash.GET_BLIP_INFO_ID_ENTITY_INDEX, Handle);
			}
		}

		public bool ShowRoute
		{
			set
			{
				Function.Call(Hash.SET_BLIP_ROUTE, Handle, value);
			}
		}
		public bool IsFriendly
		{
			set
			{
				Function.Call(Hash.SET_BLIP_AS_FRIENDLY, Handle, value);
			}
		}
		public bool IsFlashing
		{
			get
			{
				return Function.Call<bool>(Hash.IS_BLIP_FLASHING, Handle);
			}
			set
			{
				Function.Call(Hash.SET_BLIP_FLASHES, Handle, value);
			}
		}
		public bool IsOnMinimap
		{
			get
			{
				return Function.Call<bool>(Hash.IS_BLIP_ON_MINIMAP, Handle);
			}
		}
		public bool IsShortRange
		{
			get
			{
				return Function.Call<bool>(Hash.IS_BLIP_SHORT_RANGE, Handle);
			}
			set
			{
				Function.Call(Hash.SET_BLIP_AS_SHORT_RANGE, Handle, value);
			}
		}

		public void RemoveNumberLabel()
		{
			Function.Call(Hash.HIDE_NUMBER_ON_BLIP, Handle);
		}

		public void Remove()
		{
			Function.Call(Hash.REMOVE_BLIP, new OutputArgument(Handle));
		}

		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_BLIP_EXIST, Handle);
		}
		public static bool Exists(Blip blip)
		{
			return !ReferenceEquals(blip, null) && blip.Exists();
		}

		public bool Equals(Blip blip)
		{
			return !ReferenceEquals(blip, null) && Handle == blip.Handle;
		}
		public override bool Equals(object obj)
		{
			return !ReferenceEquals(obj, null) && obj.GetType() == GetType() && Equals((Blip)obj);
		}

		public sealed override int GetHashCode()
		{
			return Handle;
		}

		public static bool operator ==(Blip left, Blip right)
		{
			return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);
		}
		public static bool operator !=(Blip left, Blip right)
		{
			return !(left == right);
		}
	}
}
