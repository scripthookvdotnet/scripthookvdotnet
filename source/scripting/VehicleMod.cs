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
                return Function.Call<int>(Hash.GET_VEHICLE_MOD, _owner.Handle, (int)ModType);
            }
            set
            {
                Function.Call(Hash.SET_VEHICLE_MOD, _owner.Handle, (int)ModType, (int)ModType, true);
            }
        }

		public string LocalizedModTypeName
		{
			get
			{
				return Function.Call<string>(Hash.GET_MOD_SLOT_NAME, _owner.Handle, (int)ModType);
            }
        }
        public string LocalizedModName
        {
            get
            {
                return Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, _owner.Handle, (int)ModType, Index);
            }
        }
		public int ModCount
		{
			get
			{
                return Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, _owner.Handle, (int)ModType);
            }
		}
        public Vehicle Vehicle
        {
            get { return _owner; }
        }

        public void Remove()
        {
            Function.Call(Hash.REMOVE_VEHICLE_MOD, _owner.Handle, (int)ModType);
        }
    }
}
