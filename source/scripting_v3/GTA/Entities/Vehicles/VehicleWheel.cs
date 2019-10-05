//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	public sealed class VehicleWheel
	{
		internal VehicleWheel(Vehicle owner, int index)
		{
			Vehicle = owner;
			Index = index;
		}

		public Vehicle Vehicle
		{
			get;
		}

		public int Index
		{
			get;
		}

		public void Fix()
		{
			Function.Call(Hash.SET_VEHICLE_TYRE_FIXED, Vehicle.Handle, Index);
		}

		public void Burst()
		{
			Function.Call(Hash.SET_VEHICLE_TYRE_BURST, Vehicle.Handle, Index, true, 1000f);
		}
	}
}
