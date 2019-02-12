using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public enum BlipColor
	{
		///<summary>The default RGB value of this color is the same as HUD_COLOUR_PURE_WHITE, whose default RGB value is #FFFFFF.</summary>
		PureWhite,
		Red,
		Green,
		Blue,
		///<summary>The default RGB value of this color is the same as HUD_COLOUR_WHITE, whose default RGB value is #F0F0F0.</summary>
		White,
		Yellow,
		NetPlayer1,
		NetPlayer2,
		NetPlayer3,
		NetPlayer4,
		NetPlayer5,
		NetPlayer6,
		NetPlayer7,
		NetPlayer8,
		NetPlayer9,
		NetPlayer10,
		NetPlayer11,
		NetPlayer12,
		NetPlayer13,
		NetPlayer14,
		NetPlayer15,
		NetPlayer16,
		NetPlayer17,
		NetPlayer18,
		NetPlayer19,
		NetPlayer20,
		NetPlayer21,
		NetPlayer22,
		NetPlayer23,
		NetPlayer24,
		NetPlayer25,
		NetPlayer26,
		NetPlayer27,
		NetPlayer28,
		NetPlayer29,
		NetPlayer30,
		NetPlayer31,
		NetPlayer32,
		Freemode,
		InactiveMission,
		GreyDark,
		RedLight,
		///<summary>This color is usually #65B4D4 sky blue, which is similar to <see cref="System.Drawing.Color.SkyBlue"></see>.</summary>
		Michael,
		///<summary>This color is usually #ABEDAB light green, which is very similar to <see cref="System.Drawing.Color.LightGreen"></see>.</summary>
		Franklin,
		///<summary>This color is usually #ABEDAB orange, which is very similar to <see cref="System.Drawing.Color.SandyBrown"></see>.</summary>
		Trevor,
		GolfPlayer1,
		GolfPlayer2,
		GolfPlayer3,
		GolfPlayer4,
		///<summary>This color is always the same as <see cref="Red"></see>, the only difference is color index.</summary>
		Red2,
		Purple,
		Orange,
		GreenDark,
		BlueLight,
		BlueDark,
		Grey,
		YellowDark,
		///<summary>This color is always the same as <see cref="Blue"></see>, the only difference is color index.</summary>
		Blue2,
		PurpleDark,
		///<summary>This color is always the same as <see cref="Red"></see>, the only difference is color index.</summary>
		Red3,
		///<summary>This color is always the same as <see cref="Yellow"></see>, the only difference is color index.</summary>
		Yellow2,
		Pink,
		GreyLight,
		Gang,
		Gang2,
		Gang3,
		///<summary>The default color.</summary>
		Yellow3,
		///<summary>This color is always the same as <see cref="Blue"></see>, the only difference is color index.</summary>
		Blue3,
		///<summary>This color is always the same as <see cref="Blue"></see>, the only difference is color index.</summary>
		Blue4,
		///<summary>This color is always the same as <see cref="Green"></see>, the only difference is color index.</summary>
		Green2,
		///<summary>This color is always the same as <see cref="Yellow"></see>, the only difference is color index.</summary>
		Yellow4,
		///<summary>This color is always the same as <see cref="Yellow"></see>, the only difference is color index.</summary>
		Yellow5,
		///<summary>This color is always the same as <see cref="PureWhite"></see>, the only difference is color index.</summary>
		PureWhite2,
		///<summary>This color is always the same as <see cref="Yellow"></see>, the only difference is color index.</summary>
		Yellow6,
		///<summary>This color is always the same as <see cref="Blue"></see>, the only difference is color index.</summary>
		Blue5,
		///<summary>This color is always the same as <see cref="Red"></see>, the only difference is color index.</summary>
		Red4,
		RedDark,
		///<summary>This color is always the same as <see cref="Blue"></see>, the only difference is color index.</summary>
		Blue6,
		///<summary>This color is always the same as <see cref="BlueDark"></see>, the only difference is color index.</summary>
		BlueDark2,
		///<summary>This color is always the same as <see cref="RedDark"></see>, the only difference is color index.</summary>
		RedDark2,
		MenuYellow,
		SimpleBlipDefault,
		Waypoint,
		///<summary>This color is always the same as <see cref="Blue"></see>, the only difference is color index.</summary>
		Blue7,
	}
	public enum BlipSprite
	{
		///<summary>The standard blip.</summary>
		Destination = 1,
		DestinationBig = 2,
		Police = 3,
		ObjectiveFlashing = 4,
		ObjectiveSquare = 5,
		Player = 6,
		North = 7,
		Waypoint = 8,
		BigCircle = 9,
		BigCircleOutline = 10,
		ArrowUpOutlined = 11,
		ArrowDownOutlined = 12,
		ArrowUp = 13,
		ArrowDown = 14,
		PoliceHelicopter = 15,
		PolicePlane = 16,
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
		CableCar = 36,
		RaceFinishLine = 38,
		Safehouse = 40,
		Police2 = 41,
		PoliceChase = 42,
		PoliceHelicopter2 = 43,
		Snitch = 47,
		CriminalCarsteal = 50,
		CriminalDrugs = 51,
		CriminalHoldups = 52,
		PolicePatrol = 56,
		PolicePlayer = 57,
		CriminalWanted = 58,
		HeistStore = 59,
		PoliceStation = 60,
		Hospital = 61,
		Helicopter = 64,
		Joyriders = 65,
		StrangersAndFreaks = 66,
		ArmoredVan = 67,
		TowTruck = 68,
		Barber = 71,
		LosSantosCustoms = 72,
		ClothesStore = 73,
		TattooParlor = 75,
		Simeon = 76,
		Lester = 77,
		Michael = 78,
		Trevor = 79,
		TheJewelStoreJob = 80,
		Rampage = 84,
		VinewoodTours = 85,
		Lamar = 86,
		Franklin = 88,
		Chinese = 89,
		Airport = 90,
		Bar = 93,
		BaseJump = 94,
		BiolabHeist = 96,
		CarWash = 100,
		ComedyClub = 102,
		Darts = 103,
		ThePortOfLSHeist = 104,
		TheBureauRaid = 105,
		FIB = 106,
		TheBigScore = 107,
		DevinDollarSign = 108,
		Golf = 109,
		AmmuNation = 110,
		Exile = 112,
		TheSharmootaJob = 113,
		ThePaletoScore = 118,
		ShootingRange = 119,
		Solomon = 120,
		StripClub = 121,
		Tennis = 122,
		Exile2 = 123,
		Michael2 = 124,
		Triathlon = 126,
		OffRoadRacing = 127,
		GangPolice = 128,
		GangMexicans = 129,
		GangBikers = 130,
		Snitch2 = 133,
		CriminalCuffKeys = 134,
		MovieTheater = 135,
		MusicVenue = 136,
		PoliceStation2 = 137,
		Stash = 140,
		Hunting = 141,
		Objective2 = 143,
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
		GangPolicePartner = 171,
		Minigun = 173,
		GrenadeLauncher = 174,
		Armor = 175,
		PropertyTakeover = 176,
		CriminalSnitchMexican = 177,
		CriminalSnitchLost = 178,
		PropertyBikers = 181,
		PropertyPolice = 182,
		PropertyVagos = 183,
		Camera = 184,
		PlayerPositon = 185,
		BikerHandcuffKeys = 186,
		VagosHandcuffKeys = 187,
		BikerHandcuffsClosed = 188,
		VagosHandcuffsClosed = 189,
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
		DevinDollarSign2 = 207,
		Trevor2 = 208,
		Trevor3 = 209,
		Franklin2 = 210,
		Franklin3 = 211,
		FranklinC = 214,
		GangVehicle = 225,
		GangVehicleBikers = 226,
		GangVehiclePolice = 227,
		GunCar = 228,
		GangPoliceHighlight = 233,
		CustodyBikers = 237,
		CustodyVagos = 238,
		ArmsTraffickingAir = 251,
		PlayerstateArrested = 252,
		PlayerstateCustody = 253,
		PlayerstateKeyholder = 255,
		PlayerstatePartner = 256,
		Fairground = 266,
		PropertyManagement = 267,
		GangHighlight = 268,
		Altruist = 269,
		Enemy = 270,
		OnMission = 271,
		CashPickup = 272,
		Chop = 273,
		Dead = 274,
		CashPickupLost = 276,
		CashPickupVagos = 277,
		CashPickupPolice = 278,
		DropOffHooker = 279,
		Friend = 280,
		CustodyDropoff = 285,
		OnMissionPolice = 286,
		OnMissionLost = 287,
		OnMissionVagos = 288,
		CriminalCarstealPolice = 289,
		CriminalCarstealLost = 290,
		CriminalCarstealVagos = 291,
		SimeonFamily = 293,
		BountyHit = 303,
		GTAOUserCreatedMission = 304,
		Horde = 305,
		CrateDrop = 306,
		PlaneDrop = 307,
		Sub = 308,
		Race = 309,
		Deathmatch = 310,
		ArmWrestling = 311,
		AmmuNationShootingRange = 313,
		RaceAir = 314,
		StreetRace = 315,
		SeaRace = 316,
		TowTruck2 = 317,
		GarbageTruck = 318,
		GetawayCar = 326,
		GangBike = 348,
		PropertyForSale = 350,
		GangAttackPackage = 351,
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
		MasksStore = 362,
		HeistSetup = 363,
		Incapacitated = 364,
		SpawnPointPickup = 365,
		BoilerSuit = 366,
		Completed = 367,
		Missiles = 368,
		GarageForSale = 369,
		HelipadForSale = 370,
		DockForSale = 371,
		HangarForSale = 372,
		Business = 374,
		BusinessForSale = 375,
		BikeRace = 376,
		Parachute = 377,
		TeamDeathmatch = 378,
		FootRace = 379,
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
		Aim = 390,
		Creator = 398,
		CreatorDirection = 399,
		Abigail = 400,
		Blimp = 401,
		Repair = 402,
		Raging = 403,
		Dinghy = 404,
		Fanatic = 405,
		Invisible = 406,
		Information = 407,
		CaptureBriefcase = 408,
		LastTeamStanding = 409,
		Boat = 410,
		CaptureHouse = 411,
		GTAOCrew2 = 412,
		JerryCan2 = 415,
		RP = 416,
		GTAOPlayerSafehouse = 417,
		GTAOPlayerSafehouseBounty = 418,
		CaptureAmericanFlag = 419,
		CaptureFlag = 420,
		Tank = 421,
		HelicopterAnimated = 422,
		Jet = 423,
		PlayerNoOutline = 425,
		Insurgent = 426,
		Motorboat = 427,
		Heist = 428,
		Stopwatch = 430,
		DollarSignCircled = 431,
		Crosshair2 = 432,
		DollarSignSquared = 434,
		StuntRace = 435,
		HotProperty,
		KillListCompetitive,
		KingOfTheCastle,
		PlayerKing,
		DeadDrop,
		PennedIn,
		Beast,
		CrossTheLinePointer,
		CrossTheLine,
		LamarD,
		Bennys,
		LamarDNumber1,
		LamarDNumber2,
		LamarDNumber3,
		LamarDNumber4,
		LamarDNumber5,
		LamarDNumber6,
		LamarDNumber7,
		LamarDNumber8,
		Yacht,
		FindersKeepers,
		Briefcase2,
		ExecutiveSearch,
		Wifi,
		TurretedLimo,
		AssetRecovery,
		YachtLocation,
		Beasted,
		Zoned,
		Random,
		SlowTime,
		Flipped,
		ThermalVision,
		Doped,
		Railgun,
		Seashark,
		Blind,
		Warehouse,
		WarehouseForSale,
		Office,
		OfficeForSale,
		Truck,
		SpecialCargo,
		Trailer,
		VIP,
		Cargobob,
		AreaOutline,
		Jammed,
		Ghost,
		Detonator,
		Bomb,
		BeastShield,
		Stunt,
		LifeHeart,
		StuntPremium,
		Adversary,
		BikerClubhouse,
		CagedIn,
		TurfWar,
		Joust,
		WeedProduction,
		CocaineProduction,
		FakeIDProduction,
		MethProduction,
		MoneyProduction,
		PackageBikerBusiness,
		Capture1,
		Capture2,
		Capture3,
		Capture4,
		Capture5,
		Capture6,
		Capture7,
		Capture8,
		Capture9,
		Capture10,
		QuadBike,
		Bus,
		DrugsPackage,
		Hop,
		Adversary4,
		Adversary8,
		Adversary10,
		Adversary12,
		Adversary16,
		Laptop,
		Deadline,
		SportsCar,
		VehicleWarehouse,
		RegistrationPapers,
		PoliceStationInverted,
		Junkyard,
		PhantomWedge,
		ArmoredBoxville,
		Ruiner2000,
		RampBuggy,
		Wastelander,
		RocketVoltic,
		TechnicalAqua,
		TargetA,
		TargetB,
		TargetC,
		TargetD,
		TargetE,
		TargetF,
		TargetG,
		TargetH,
		Juggernaut,
		Repair2,
		SpecialVehicleRaceSeries,
		ChallengeSeries,
		RocketBoost,
		HomingRocket,
		MachineGun,
		Parachute2,
		FiveSeconds,
		TenSeconds,
		FifteenSeconds,
		TwentySeconds,
		ThirtySeconds,
		WeaponSupplies,
		Bunker,
		APC,
		Oppressor,
		HalfTrack,
		DuneFAV,
		WeaponizedTampa,
		AntiAircraftTrailer,
		MobileOperationsCenter,
		AdversaryBunker,
		BunkerVehicleWorkshop,
		WeaponWorkshop,
		Cargo,
		GTAOHangar,
		TransformCheckpoint,
		TransformRace,
		AlphaZ1,
		Bombushka,
		Havok,
		HowardNX25,
		Hunter,
		Ultralight,
		Mogul,
		V65Molotok,
		P45Nokota,
		Pyro,
		Rogue,
		Starling,
		Seabreeze,
		Tula,
		Equipment,
		Treasure,
		OrbitalCannon,
		Avenger,
		Facility,
		HeistDoomsday,
		SAMTurret,
		Firewall,
		Node,
		Stromberg,
		Deluxo,
		Thruster,
		Khanjali,
		RCV,
		Volatol,
		Barrage,
		Akula,
		Chernobog,
		CCTV,
		StarterPackIdentifier,
		TurretStation,
		RotatingMirror,
		StaticMirror,
		Proxy,
		TargetAssault,
		SanAndreasSuperSportCircuit,
		SeaSparrow,
		Caracara,
		NightclubProperty,
		CargoBusinessBattle,
		NightclubTruck,
		Jewel,
		Gold,
		Keypad,
		HackTarget,
		HealthHeart,
		BlastIncrease,
		BlastDecrease,
		BombIncrease,
		BombDecrease,
		Rival,
		Drone,
		CashRegister,
		CCTV2,
		TargetBusinessBattle,
		FestivalBus,
		Terrorbyte,
		Menacer,
		Scramjet,
		PounderCustom,
		MuleCustom,
		SpeedoCustom,
		Blimp2,
		OppressorMkII,
		B11StrikeForce,
		ArenaSeries,
		ArenaPremium,
		ArenaWorkshop,
		RaceArenaWar,
		ArenaTurret,
		RCVehicle,
		RCWorkshop,
		FirePit,
		Flipper,
		SeaMine,
		TurnTable,
		Pit,
		Mines,
		BarrelBomb,
		RisingWall,
		Bollards,
		SideBollard,
		Bruiser,
		Brutus,
		Cerberus,
		Deathbike,
		Dominator,
		Impaler,
		Imperator,
		Issi,
		Sasquatch,
		Scarab,
		Slamvam,
		ZR380
	}

	public sealed class Blip : PoolObject, IEquatable<Blip>
	{
		public Blip(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Gets or sets the position of this <see cref="Blip"/>.
		/// </summary>
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
		/// <summary>
		/// Sets the rotation of this <see cref="Blip"/> on the map.
		/// </summary>
		public int Rotation
		{
			set
			{
				Function.Call(Hash.SET_BLIP_ROTATION, Handle, value);
			}
		}
		/// <summary>
		/// Sets the scale of this <see cref="Blip"/> on the map.
		/// </summary>
		public float Scale
		{
			set
			{
				Function.Call(Hash.SET_BLIP_SCALE, Handle, value);
			}
		}

		/// <summary>
		/// Gets the type of this <see cref="Blip"/>.
		/// </summary>
		public int Type
		{
			get
			{
				return Function.Call<int>(Hash.GET_BLIP_INFO_ID_TYPE, Handle);
			}
		}
		/// <summary>
		/// Gets or sets the alpha of this <see cref="Blip"/> on the map.
		/// </summary>
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
		/// <summary>
		/// Sets the priority of this <see cref="Blip"/>.
		/// </summary>
		public int Priority
		{
			set
			{
				Function.Call(Hash.SET_BLIP_PRIORITY, Handle, value);
			}
		}
		/// <summary>
		/// Sets this <see cref="Blip"/>s label to the given number.
		/// </summary>
		public int NumberLabel
		{
			set
			{
				Function.Call(Hash.SHOW_NUMBER_ON_BLIP, Handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the color of this <see cref="Blip"/>.
		/// </summary>
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
		/// <summary>
		/// Gets or sets the sprite of this <see cref="Blip"/>.
		/// </summary>
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
		/// <summary>
		/// Sets this <see cref="Blip"/>s label to the given string.
		/// </summary>
		public string Name
		{
			set
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, MemoryAccess.StringPtr);
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, value);
				Function.Call(Hash.END_TEXT_COMMAND_SET_BLIP_NAME, Handle);
			}
		}
		/// <summary>
		/// Gets the <see cref="Entity"/> this <see cref="Blip"/> is attached to.
		/// </summary>
		public Entity Entity
		{
			get
			{
				return GTA.Entity.FromHandle(Function.Call<int>(Hash.GET_BLIP_INFO_ID_ENTITY_INDEX, Handle));
			}
		}

		/// <summary>
		/// Sets a value indicating whether the route to this <see cref="Blip"/> should be shown on the map.
		/// </summary>
		/// <value>
		///   <c>true</c> to show the route; otherwise, <c>false</c>.
		/// </value>
		public bool ShowRoute
		{
			set
			{
				Function.Call(Hash.SET_BLIP_ROUTE, Handle, value);
			}
		}
		/// <summary>
		/// Sets a value indicating whether this <see cref="Blip"/> is friendly.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Blip"/> is friendly; otherwise, <c>false</c>.
		/// </value>
		public bool IsFriendly
		{
			set
			{
				Function.Call(Hash.SET_BLIP_AS_FRIENDLY, Handle, value);
			}
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Blip"/> is flashing.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Blip"/> is flashing; otherwise, <c>false</c>.
		/// </value>
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
		/// <summary>
		/// Gets a value indicating whether this <see cref="Blip"/> is on minimap.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Blip"/> is on minimap; otherwise, <c>false</c>.
		/// </value>
		public bool IsOnMinimap
		{
			get
			{
				return Function.Call<bool>(Hash.IS_BLIP_ON_MINIMAP, Handle);
			}
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Blip"/> is short range.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Blip"/> is short range; otherwise, <c>false</c>.
		/// </value>
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

		/// <summary>
		/// Removes the number label for this <see cref="Blip"/>.
		/// </summary>
		public void RemoveNumberLabel()
		{
			Function.Call(Hash.HIDE_NUMBER_ON_BLIP, Handle);
		}

		/// <summary>
		/// Removes this <see cref="Blip"/>.
		/// </summary>
		public override void Delete()
		{
		    int handle = Handle;
			unsafe
			{
				Function.Call(Hash.REMOVE_BLIP, &handle);
			}
			Handle = handle;
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
			return Handle.GetHashCode();
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
