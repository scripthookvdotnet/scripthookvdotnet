using GTA.Native;
using System;
using System.Collections.Generic;

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
		FrontWheels = 23,
		BackWheels,
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
		HighEnd
	}

	public sealed class VehicleModCollection
	{
		#region Fields

		Vehicle _owner;
		readonly Dictionary<int, VehicleMod> _vehicleMods = new Dictionary<int, VehicleMod>();
		readonly Dictionary<int, VehicleToggleMod> _vehicleToggleMods = new Dictionary<int, VehicleToggleMod>();

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

				if (!_vehicleMods.TryGetValue((int) modType, out vehicleMod))
				{
					vehicleMod = new VehicleMod(_owner, modType);
					_vehicleMods.Add((int) modType, vehicleMod);
				}

				return vehicleMod;
			}
		}

		public VehicleToggleMod this[VehicleToggleModType modType]
		{
			get
			{
				VehicleToggleMod vehicleToggleMod = null;

				if (!_vehicleToggleMods.TryGetValue((int) modType, out vehicleToggleMod))
				{
					vehicleToggleMod = new VehicleToggleMod(_owner, modType);
					_vehicleToggleMods.Add((int) modType, vehicleToggleMod);
				}

				return vehicleToggleMod;
			}
		}

		public VehicleWheelType WheelType
		{
			get { return Function.Call<VehicleWheelType>(Hash.GET_VEHICLE_WHEEL_TYPE, _owner.Handle); }
			set { Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, _owner.Handle, value); }
		}

		public void InstallModKit()
		{
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, _owner.Handle, 0);
		}
	}
}
