//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	public sealed class VehicleWindow
	{
		internal VehicleWindow(Vehicle owner, VehicleWindowIndex index)
		{
			Vehicle = owner;
			Index = index;
		}

		public Vehicle Vehicle
		{
			get;
		}

		public VehicleWindowIndex Index
		{
			get;
		}

		public bool IsIntact => Function.Call<bool>(Hash.IS_VEHICLE_WINDOW_INTACT, Vehicle.Handle, Index);

		public void Smash()
		{
			Function.Call(Hash.SMASH_VEHICLE_WINDOW, Vehicle.Handle, Index);
		}

		public void Repair()
		{
			Function.Call(Hash.FIX_VEHICLE_WINDOW, Vehicle.Handle, Index);
		}

		public void Remove()
		{
			Function.Call(Hash.REMOVE_VEHICLE_WINDOW, Vehicle.Handle, Index);
		}

		public void RollUp()
		{
			Function.Call(Hash.ROLL_UP_WINDOW, Vehicle.Handle, Index);
		}

		public void RollDown()
		{
			Function.Call(Hash.ROLL_DOWN_WINDOW, Vehicle.Handle, Index);
		}
	}
}
