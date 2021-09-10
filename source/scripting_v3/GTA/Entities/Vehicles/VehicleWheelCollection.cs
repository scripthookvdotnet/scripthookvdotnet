//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Collections;
using System.Collections.Generic;

namespace GTA
{
	public sealed class VehicleWheelCollection : IEnumerable<VehicleWheel>, IEnumerable
	{
		#region Fields
		readonly Vehicle _owner;
		// Vehicles have up to 10 wheels
		const int MAX_WHEEL_COUNT = 10;
		readonly VehicleWheel[] _vehicleWheels = new VehicleWheel[MAX_WHEEL_COUNT];
		VehicleWheel[] _vehicleWheelsForIndex6And7;
		VehicleWheel _nullWheel;
		#endregion

		internal VehicleWheelCollection(Vehicle owner)
		{
			_owner = owner;
		}

		[Obsolete("VehicleWheel indexer overload with int index does not support any of the wheels wheel_lm2, wheel_rm2, wheel_lm3, or wheel_lm3 for legacy scripts compatibility in v3 API. Use VehicleWheel indexer overload with VehicleWheelBoneId enum instead.")]
		public VehicleWheel this[int index]
		{
			get
			{
				// Return null wheel instance to avoid scripts that targets to between 3.0 to 3.1 not working due to a exception
				// The vehicle wheel id array for natives defines only 8 elements, and any other values can result in undefined behavior or even memory access violation
				if (index < 0 || index > 7)
				{
					return _nullWheel ?? (_nullWheel = new VehicleWheel(_owner, -1));
				}

				if (index < 6)
				{
					VehicleWheelBoneId boneId = VehicleWheel.vehicleWheelBoneIndexTableForNatives[index];
					int boneIndexZeroBased = (int)boneId - 11;
					return _vehicleWheels[boneIndexZeroBased] ?? (_vehicleWheels[boneIndexZeroBased] = new VehicleWheel(_owner, index));
				}
				// Use a special array in case some scripts access to index 6 or 7 wheel and read Index property
				else
				{
					if (_vehicleWheelsForIndex6And7 == null)
						_vehicleWheelsForIndex6And7 = new VehicleWheel[2];

					return _vehicleWheelsForIndex6And7[index - 6] ?? (_vehicleWheelsForIndex6And7[index - 6] = new VehicleWheel(_owner, index));
				}
			}
		}

		public VehicleWheel this[VehicleWheelBoneId boneId]
		{
			get
			{
				if (!VehicleWheel.IsBoneIdValid(boneId))
					throw new ArgumentOutOfRangeException(nameof(boneId));

				int boneIndexZeroBased = (int)boneId - 11;
				return _vehicleWheels[boneIndexZeroBased] ?? (_vehicleWheels[boneIndexZeroBased] = new VehicleWheel(_owner, boneId));
			}
		}

		public VehicleWheel GetWheelByIndexOfCollection(int index)
		{
			if (index < 0 || index >= Count)
				throw new ArgumentOutOfRangeException(nameof(index));

			var vehicleAddr = _owner.MemoryAddress;
			if (vehicleAddr == IntPtr.Zero)
				return null;

			var wheelAddr = SHVDN.NativeMemory.GetVehicleWheelAddressByIndexOfWheelArray(vehicleAddr, index);
			if (wheelAddr == IntPtr.Zero)
				return null;

			var boneId = (VehicleWheelBoneId)SHVDN.NativeMemory.ReadInt32(wheelAddr + SHVDN.NativeMemory.VehicleWheelIdOffset);
			int boneIndexZeroBased = (int)boneId - 11;
			return _vehicleWheels[boneIndexZeroBased] ?? (_vehicleWheels[boneIndexZeroBased] = new VehicleWheel(_owner, boneId, wheelAddr));
		}

		public IEnumerator<VehicleWheel> GetEnumerator()
		{
			// No elements will be return if the vehicle is a boat, a train or a submarine
			if (!VehicleWheel.CanVehicleHasWheels(_owner))
				yield break;

			var wheelCount = Count;
			for (int i = 0; i < wheelCount; i++)
			{
				var vehicleAddr = _owner.MemoryAddress;
				if (vehicleAddr == IntPtr.Zero)
					yield break;

				var wheelAddr = SHVDN.NativeMemory.GetVehicleWheelAddressByIndexOfWheelArray(vehicleAddr, i);
				if (wheelAddr == IntPtr.Zero)
					yield break;

				var boneId = (VehicleWheelBoneId)SHVDN.NativeMemory.ReadInt32(wheelAddr + SHVDN.NativeMemory.VehicleWheelIdOffset);
				int boneIndexZeroBased = (int)boneId - 11;
				yield return _vehicleWheels[boneIndexZeroBased] ?? (_vehicleWheels[boneIndexZeroBased] = new VehicleWheel(_owner, boneId, wheelAddr));
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
