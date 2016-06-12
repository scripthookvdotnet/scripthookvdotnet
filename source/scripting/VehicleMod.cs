using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using GTA.Native;

namespace GTA
{

	public sealed class VehicleMod
	{
		#region Fields
		Vehicle _owner;

		#endregion

		internal VehicleMod(Vehicle owner, VehicleModType modType)
		{
			_owner = owner;
			ModType = modType;
		}

		public VehicleModType ModType { get; private set; }

		public int Index
		{
			get
			{
				return Function.Call<int>(Hash.GET_VEHICLE_MOD, _owner.Handle, ModType);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_MOD, _owner.Handle, ModType, value, Variation);
			}
		}

		public bool Variation
		{
			get
			{				  
				return Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, _owner.Handle, ModType);
			}
			set
			{
				Function.Call(Hash.SET_VEHICLE_MOD, _owner.Handle, ModType, Index, value);
			}	 

		}

		public string LocalizedModTypeName
		{
			get
			{
				return Function.Call<string>(Hash.GET_MOD_SLOT_NAME, _owner.Handle, ModType);
			}
		}
		public string LocalizedModName
		{
			get
			{
				return Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, _owner.Handle, ModType, Index);
			}
		}
		public int ModCount
		{
			get
			{
				return Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, _owner.Handle, ModType);
			}
		}
		public Vehicle Vehicle
		{
			get { return _owner; }
		}

		public void Remove()
		{
			Function.Call(Hash.REMOVE_VEHICLE_MOD, _owner.Handle, ModType);
		}
	}
}
