//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;

namespace GTA
{
	public sealed class VehicleDoorCollection
	{
		#region Fields
		readonly Vehicle _owner;
		readonly Dictionary<VehicleDoorIndex, VehicleDoor> _vehicleDoors = new();
		#endregion

		internal VehicleDoorCollection(Vehicle owner)
		{
			_owner = owner;
		}

		public VehicleDoor this[VehicleDoorIndex index]
		{
			get
			{
				if (!_vehicleDoors.TryGetValue(index, out VehicleDoor vehicleDoor))
				{
					vehicleDoor = new VehicleDoor(_owner, index);
					_vehicleDoors.Add(index, vehicleDoor);
				}

				return vehicleDoor;
			}
		}

		public bool Contains(VehicleDoorIndex door)
		{
			switch (door)
			{
				case VehicleDoorIndex.FrontLeftDoor:
					return _owner.Bones.Contains("door_dside_f");
				case VehicleDoorIndex.FrontRightDoor:
					return _owner.Bones.Contains("door_pside_f");
				case VehicleDoorIndex.BackLeftDoor:
					return _owner.Bones.Contains("door_dside_r");
				case VehicleDoorIndex.BackRightDoor:
					return _owner.Bones.Contains("door_pside_r");
				case VehicleDoorIndex.Hood:
					return _owner.Bones.Contains("bonnet");
				case VehicleDoorIndex.Trunk:
					return _owner.Bones.Contains("boot");
			}
			return false;
		}

		public VehicleDoor[] ToArray()
		{
			var result = new List<VehicleDoor>();
			foreach (VehicleDoorIndex doorindex in Enum.GetValues(typeof(VehicleDoorIndex)))
			{
				if (Contains(doorindex))
				{
					result.Add(this[doorindex]);
				}
			}
			return result.ToArray();
		}

		public IEnumerator<VehicleDoor> GetEnumerator()
		{
			return (ToArray() as IEnumerable<VehicleDoor>).GetEnumerator();
		}

	}
}
