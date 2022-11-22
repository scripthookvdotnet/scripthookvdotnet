//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	public sealed class VehicleDoor
	{
		internal VehicleDoor(Vehicle owner, VehicleDoorIndex index)
		{
			Vehicle = owner;
			Index = index;
		}

		public Vehicle Vehicle
		{
			get;
		}

		public VehicleDoorIndex Index
		{
			get;
		}

		public bool IsOpen => AngleRatio > 0;

		public bool IsFullyOpen => Function.Call<bool>(Hash.IS_VEHICLE_DOOR_FULLY_OPEN, Vehicle.Handle, Index);

		public void Open(bool loose = false, bool instantly = false)
		{
			Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, Vehicle.Handle, Index, loose, instantly);
		}

		public void Close(bool instantly = false)
		{
			Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, Vehicle.Handle, Index, instantly);
		}

		public bool IsBroken => Function.Call<bool>(Hash.IS_VEHICLE_DOOR_DAMAGED, Vehicle.Handle, Index);

		public bool CanBeBroken
		{
			set => Function.Call(Hash.SET_DOOR_ALLOWED_TO_BE_BROKEN_OFF, Vehicle.Handle, Index, value);
		}

		public void Break(bool stayInTheWorld = true)
		{
			Function.Call(Hash.SET_VEHICLE_DOOR_BROKEN, Vehicle.Handle, Index, !stayInTheWorld);
		}

		public float AngleRatio
		{
			get => Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, Vehicle.Handle, Index);
			set => Function.Call(Hash.SET_VEHICLE_DOOR_CONTROL, Vehicle.Handle, Index, 1, value);
		}
	}
}
