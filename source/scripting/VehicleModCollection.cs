using GTA.Native;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GTA
{
	public enum VehicleModType
	{
		Spoilers,
		FrontBumper,
		RearBumper,
		SideSkirt,
		Exhaust,
		Frame,
		Grille,
		Hood,
		Fender,
		RightFender,
		Roof,
		Engine,
		Brakes,
		Transmission,
		Horns,
		Suspension,
		Armor,
		FrontWheel = 23,
		RearWheel,
		PlateHolder,
		VanityPlates,
		TrimDesign,
		Ornaments,
		Dashboard,
		DialDesign,
		DoorSpeakers,
		Seats,
		SteeringWheels,
		ColumnShifterLevers,
		Plaques,
		Speakers,
		Trunk,
		Hydraulics,
		EngineBlock,
		AirFilter,
		Struts,
		ArchCover,
		Aerials,
		Trim,
		Tank,
		Windows,
		Livery = 48
	}

	public enum VehicleToggleModType
	{
		Turbo = 18,
		TireSmoke = 20,
		XenonHeadlights = 22
	}

	public enum VehicleWheelType
	{
		Sport,
		Muscle,
		Lowrider,
		SUV,
		Offroad,
		Tuner,
		BikeWheels,
		HighEnd,
		BennysOriginals,
		BennysBespoke
	}

	public sealed class VehicleModCollection
	{
		#region Fields

		Vehicle _owner;
		readonly Dictionary<VehicleModType, VehicleMod> _vehicleMods = new Dictionary<VehicleModType, VehicleMod>();
		readonly Dictionary<VehicleToggleModType, VehicleToggleMod> _vehicleToggleMods = new Dictionary<VehicleToggleModType, VehicleToggleMod>();

		private static readonly ReadOnlyDictionary<VehicleWheelType, Tuple<string, string>> _wheelNames = new ReadOnlyDictionary
			<VehicleWheelType, Tuple<string, string>>(
			new Dictionary<VehicleWheelType, Tuple<string, string>>
			{
				{VehicleWheelType.BikeWheels, new Tuple<string, string>("CMOD_WHE1_0", "Bike")},
				{VehicleWheelType.HighEnd, new Tuple<string, string>("CMOD_WHE1_1", "High End")},
				{VehicleWheelType.Lowrider, new Tuple<string, string>("CMOD_WHE1_2", "Lowrider")},
				{VehicleWheelType.Muscle, new Tuple<string, string>("CMOD_WHE1_3", "Muscle")},
				{VehicleWheelType.Offroad, new Tuple<string, string>("CMOD_WHE1_4", "Offroad")},
				{VehicleWheelType.Sport, new Tuple<string, string>("CMOD_WHE1_5", "Sport")},
				{VehicleWheelType.SUV, new Tuple<string, string>("CMOD_WHE1_6", "SUV")},
				{VehicleWheelType.Tuner, new Tuple<string, string>("CMOD_WHE1_7", "Tuner")},
				{VehicleWheelType.BennysOriginals, new Tuple<string, string>("CMOD_WHE1_8", "Benny's Originals")},
				{VehicleWheelType.BennysBespoke, new Tuple<string, string>("CMOD_WHE1_9", "Benny's Bespoke")}
			});
		#endregion

		internal VehicleModCollection(Vehicle owner)
		{
			_owner = owner;
		}


		public VehicleMod this[VehicleModType modType]
		{
			get
			{
				VehicleMod vehicleMod = null;

				if (!_vehicleMods.TryGetValue(modType, out vehicleMod))
				{
					vehicleMod = new VehicleMod(_owner, modType);
					_vehicleMods.Add(modType, vehicleMod);
				}

				return vehicleMod;
			}
		}

		public VehicleToggleMod this[VehicleToggleModType modType]
		{
			get
			{
				VehicleToggleMod vehicleToggleMod = null;

				if (!_vehicleToggleMods.TryGetValue(modType, out vehicleToggleMod))
				{
					vehicleToggleMod = new VehicleToggleMod(_owner, modType);
					_vehicleToggleMods.Add(modType, vehicleToggleMod);
				}

				return vehicleToggleMod;
			}
		}

		public bool HasVehicleMod(VehicleModType type)
		{
			return Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, _owner.Handle, type) > 0;
		}

		public VehicleMod[] GetAllMods()
		{
			return
				Enum.GetValues(typeof(VehicleModType)).Cast<VehicleModType>().Where(HasVehicleMod).Select(modType => this[modType]).ToArray();
		}

		public VehicleWheelType WheelType
		{
			get { return Function.Call<VehicleWheelType>(Hash.GET_VEHICLE_WHEEL_TYPE, _owner.Handle); }
			set { Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, _owner.Handle, value); }
		}

		public VehicleWheelType[] AllowedWheelTypes
		{
			get
			{
				if (_owner.Model.IsBicycle || _owner.Model.IsBike)
				{
					return new VehicleWheelType[] {VehicleWheelType.BikeWheels};
				}
				if (_owner.Model.IsCar)
				{
					var res = new List<VehicleWheelType>()
					{
						VehicleWheelType.Sport,
						VehicleWheelType.Muscle,
						VehicleWheelType.Lowrider,
						VehicleWheelType.SUV,
						VehicleWheelType.Offroad,
						VehicleWheelType.Tuner,
						VehicleWheelType.HighEnd
					};
					switch ((VehicleHash)_owner.Model)
					{
						case VehicleHash.Faction2:
						case VehicleHash.Buccaneer2:
						case VehicleHash.Chino2:
						case VehicleHash.Moonbeam2:
						case VehicleHash.Primo2:
						case VehicleHash.Voodoo2:
						case VehicleHash.SabreGT2:
						case VehicleHash.Tornado5:
						case VehicleHash.Virgo2:
						case VehicleHash.Minivan2:
						case VehicleHash.SlamVan3:
						case VehicleHash.Faction3:
							res.AddRange(new VehicleWheelType[] {VehicleWheelType.BennysOriginals, VehicleWheelType.BennysBespoke});
							break;
						case VehicleHash.SultanRS:
						case VehicleHash.Banshee2:
							res.Add(VehicleWheelType.BennysOriginals);
							break;
					}
					return res.ToArray();
				}
				return new VehicleWheelType[0];
			}
		}

		public string LocalizedWheelTypeName
		{
			get { return GetLocalizedWheelTypeName(WheelType); }
		}

		public string GetLocalizedWheelTypeName(VehicleWheelType wheelType)
		{
			if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
			{
				Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
				Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
			}
			if (_wheelNames.ContainsKey(wheelType))
			{
				if (Game.DoesGXTEntryExist(_wheelNames[wheelType].Item1))
				{
					return Game.GetGXTEntry(_wheelNames[wheelType].Item1);
				}
				return _wheelNames[wheelType].Item2;
			}
			throw new ArgumentException("Wheel Type is undefined", "wheelType");
		}

		public void InstallModKit()
		{
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, _owner.Handle, 0);
		}

		public bool RequestAdditionTextFile(int timeout = 1000)
		{
			if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
			{
				Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
				Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
				int end = Game.GameTime + timeout;
				{
					while (Game.GameTime < end)
					{
						if (Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
							return true;
						Script.Yield();
					}
					return false;
				}
			}
			return true;
		}
	}
}
