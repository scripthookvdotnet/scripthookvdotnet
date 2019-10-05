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
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0xAA8 : 0xA88;
				offset = Game.Version >= GameVersion.v1_0_505_2_Steam ? 0xA98 : offset;
				offset = Game.Version >= GameVersion.v1_0_791_2_Steam ? 0xAB8 : offset;
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0xAE8 : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0xB18 : offset;

				return SHVDN.NativeMemory.ReadInt32(_owner.MemoryAddress + offset);
			}
		}
	}
}
