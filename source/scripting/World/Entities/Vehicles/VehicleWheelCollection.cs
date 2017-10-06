using GTA.Native;
using System;
using System.Collections.Generic;

namespace GTA
{
	public sealed class VehicleWheelCollection
	{
		#region Fields
		Vehicle _owner;
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
				VehicleWheel vehicleWheel = null;

				if (!_vehicleWheels.TryGetValue(index, out vehicleWheel))
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
				if (_owner.MemoryAddress == IntPtr.Zero)
				{
					return 0;
				}

				int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0xAA8 : 0xA88;
				offset = Game.Version >= GameVersion.v1_0_505_2_Steam ? 0xA98 : offset;
				offset = Game.Version >= GameVersion.v1_0_791_2_Steam ? 0xAB8 : offset;
				offset = Game.Version >= GameVersion.v1_0_877_1_Steam ? 0xAE8 : offset;
				offset = Game.Version >= GameVersion.v1_0_944_2_Steam ? 0xB18 : offset;

				return MemoryAccess.ReadInt(_owner.MemoryAddress + offset);
			}
		}
	}
}
