//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;

namespace GTA
{
	public sealed class VehicleWheelCollection
	{
		#region Fields
		readonly Vehicle _owner;
		readonly Dictionary<int, VehicleWheel> _vehicleWheels = new Dictionary<int, VehicleWheel>();
		#endregion

		internal VehicleWheelCollection(Vehicle owner)
		{
			_owner = owner;
		}

		public VehicleWheel this[int index]
		{
			get
			{
				if (!_vehicleWheels.TryGetValue(index, out VehicleWheel vehicleWheel))
				{
					vehicleWheel = new VehicleWheel(_owner, index);
					_vehicleWheels.Add(index, vehicleWheel);
				}

				return vehicleWheel;
			}
		}

		public int Count
		{
			get
			{
				var address = _owner.MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.WheelCountOffset == 0)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt32(address + SHVDN.NativeMemory.WheelCountOffset);
			}
		}
	}
}
