//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	public sealed class VehicleToggleMod
	{
		internal VehicleToggleMod(Vehicle owner, VehicleToggleModType modType)
		{
			Vehicle = owner;
			Type = modType;
		}

		public Vehicle Vehicle
		{
			get;
		}

		public VehicleToggleModType Type
		{
			get;
		}

		public void Remove()
		{
			Function.Call(Hash.REMOVE_VEHICLE_MOD, Vehicle.Handle, (int)Type);
		}

		public bool IsInstalled
		{
			get => Function.Call<bool>(Hash.IS_TOGGLE_MOD_ON, Vehicle.Handle, (int)Type);
			set => Function.Call(Hash.TOGGLE_VEHICLE_MOD, Vehicle.Handle, (int)Type, value);
		}

		public string LocalizedTypeName => Function.Call<string>(Hash.GET_MOD_SLOT_NAME, Vehicle.Handle, (int)Type);
	}
}
