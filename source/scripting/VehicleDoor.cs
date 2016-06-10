using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;
using GTA.Native;

namespace GTA
{

	public sealed class VehicleDoor
	{
		#region Fields
		Vehicle _owner;
		#endregion

		internal VehicleDoor(Vehicle owner, VehicleDoorIndex index)
		{
			_owner = owner;
			Index = index;
		}

		public VehicleDoorIndex Index { get; private set; }

		public float AngleRatio
		{
			get
			{
				return Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, _owner.Handle, (int)Index);
			}
		}
		public bool CanBeBrokenOff
		{
			set
			{
				Function.Call(Hash._SET_VEHICLE_DOOR_BREAKABLE, _owner.Handle, (int)Index, value);
			}
		}
		public bool IsOpen
		{
			get
			{
				return AngleRatio > 0;
			}
		}
		public bool IsFullyOpen
		{
			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_DOOR_FULLY_OPEN, _owner.Handle, (int)Index);
			}
		}     
		public bool IsBroken
		{
			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_DOOR_DAMAGED, _owner.Handle, (int)Index);
			}
		}
		public Vehicle Vehicle
		{
			get { return _owner; }
		}

		public void Open(bool instantly)
		{
			Open(instantly);
		}
		public void Open(bool loose, bool instantly)
		{
			Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, _owner.Handle, (int)Index, loose, instantly);
		}
		public void Close(bool instantly)
		{
			Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, _owner.Handle, (int)Index, instantly);
		}
		public void BreakOff()
		{
			Function.Call(Hash.SET_VEHICLE_DOOR_BROKEN, _owner.Handle, (int)Index, true);
		}
		public void Delete()
		{
			Function.Call(Hash.SET_VEHICLE_DOOR_BROKEN, _owner.Handle, (int)Index, false);
		}
	}
}
