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
		// Vehicles have up to 10 wheels
		const int MAX_WHEEL_COUNT = 10;
		readonly VehicleWheel[] _vehicleWheels = new VehicleWheel[MAX_WHEEL_COUNT];
		VehicleWheel[] _vehicleWheelsForIndex6And7;
		VehicleWheel _nullWheel;
		#endregion

		internal VehicleWheelCollection(Vehicle owner)
		{
			Vehicle = owner;
		}

		[Obsolete("VehicleWheel indexer overload with int index does not support any of the wheels wheel_lm2, wheel_rm2, wheel_lm3, or wheel_lm3, but provided for legacy scripts compatibility in v3 API. Use VehicleWheel indexer overload with VehicleWheelBoneId enum instead.")]
		public VehicleWheel this[int index]
		{
			get
			{
				// Return null wheel instance to avoid scripts that targets to between 3.0 to 3.1 not working due to a exception
				// The vehicle wheel id array for natives defines only 8 elements, and any other values can result in undefined behavior or even memory access violation
				if (index < 0 || index > 7)
				{
					return _nullWheel ?? (_nullWheel = new VehicleWheel(Vehicle, -1));
				}

				if (index < 6)
				{
					VehicleWheelBoneId boneId = VehicleWheel.vehicleWheelBoneIndexTableForNatives[index];
					int boneIndexZeroBased = (int)boneId - 11;
					return _vehicleWheels[boneIndexZeroBased] ?? (_vehicleWheels[boneIndexZeroBased] = new VehicleWheel(Vehicle, index));
				}
				// Use a special array in case some scripts access to index 6 or 7 wheel and read Index property
				else
				{
					if (_vehicleWheelsForIndex6And7 == null)
					{
						_vehicleWheelsForIndex6And7 = new VehicleWheel[2];
					}

					return _vehicleWheelsForIndex6And7[index - 6] ?? (_vehicleWheelsForIndex6And7[index - 6] = new VehicleWheel(Vehicle, index));
				}
			}
		}

		public VehicleWheel this[VehicleWheelBoneId boneId]
		{
			get
			{
				if (!VehicleWheel.IsBoneIdValid(boneId))
				{
					throw new ArgumentOutOfRangeException(nameof(boneId));
				}

				int boneIndexZeroBased = (int)boneId - 11;
				return _vehicleWheels[boneIndexZeroBased] ?? (_vehicleWheels[boneIndexZeroBased] = new VehicleWheel(Vehicle, boneId));
			}
		}

		/// <summary>
		/// Gets the <see cref="VehicleWheel"/> by index.
		/// </summary>
		/// <param name="index">The index of the wheel collection. The order is the same as how the wheel array of the owner <see cref="Vehicle"/> is aligned.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public VehicleWheel GetWheelByIndexOfCollection(int index)
		{
			if (index < 0 || index >= Count)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			var vehicleAddr = Vehicle.MemoryAddress;
			if (vehicleAddr == IntPtr.Zero)
			{
				return null;
			}

			var wheelAddr = SHVDN.NativeMemory.GetVehicleWheelAddressByIndexOfWheelArray(vehicleAddr, index);
			if (wheelAddr == IntPtr.Zero)
			{
				return null;
			}

			var boneId = (VehicleWheelBoneId)SHVDN.NativeMemory.ReadInt32(wheelAddr + SHVDN.NativeMemory.VehicleWheelIdOffset);
			int boneIndexZeroBased = (int)boneId - 11;
			return _vehicleWheels[boneIndexZeroBased] ?? (_vehicleWheels[boneIndexZeroBased] = new VehicleWheel(Vehicle, boneId, wheelAddr));
		}

		public IEnumerator<VehicleWheel> GetEnumerator()
		{
			// No elements will be returned if the vehicle is a boat, a train or a submarine
			if (!VehicleWheel.CanVehicleHaveWheels(Vehicle))
			{
				yield break;
			}

			var wheelCount = Count;
			for (int i = 0; i < wheelCount; i++)
			{
				var vehicleAddr = Vehicle.MemoryAddress;
				if (vehicleAddr == IntPtr.Zero)
				{
					yield break;
				}

				var wheelAddress = SHVDN.NativeMemory.GetVehicleWheelAddressByIndexOfWheelArray(vehicleAddr, i);

				var boneId = (VehicleWheelBoneId)SHVDN.NativeMemory.ReadInt32(wheelAddress + SHVDN.NativeMemory.VehicleWheelIdOffset);
				int boneIndexZeroBased = (int)boneId - 11;
				yield return _vehicleWheels[boneIndexZeroBased] ?? (_vehicleWheels[boneIndexZeroBased] = new VehicleWheel(Vehicle, boneId, wheelAddress));
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <summary>
		/// Gets the <see cref="Vehicle"/>this <see cref="VehicleWheelCollection"/> belongs to.
		/// </summary>
		public Vehicle Vehicle
		{
			get;
		}

		/// <summary>
		/// Gets the number of <see cref="VehicleWheel"/> this <see cref="VehicleWheelCollection"/> has. <c>0</c> will be returned if the owner vehicle does not exist.
		/// </summary>
		public int Count
		{
			get
			{
				var address = Vehicle.MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.WheelCountOffset == 0)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt32(address + SHVDN.NativeMemory.WheelCountOffset);
			}
		}

		/// <summary>
		/// Gets an <c>array</c> of all <see cref="VehicleWheel"/>s this <see cref="VehicleWheelCollection"/> has.
		/// </summary>
		public VehicleWheel[] GetAllWheels()
		{
			if (!VehicleWheel.CanVehicleHaveWheels(Vehicle))
			{
				return Array.Empty<VehicleWheel>();
			}

			var vehicleAddress = Vehicle.MemoryAddress;
			if (vehicleAddress == IntPtr.Zero || SHVDN.NativeMemory.WheelCountOffset == 0)
			{
				return Array.Empty<VehicleWheel>();
			}

			var wheelCount = SHVDN.NativeMemory.ReadInt32(vehicleAddress + SHVDN.NativeMemory.WheelCountOffset);
			var returnWheelArray = new VehicleWheel[wheelCount];

			for (int i = 0; i < returnWheelArray.Length; i++)
			{
				var wheelAddress = SHVDN.NativeMemory.GetVehicleWheelAddressByIndexOfWheelArray(vehicleAddress, i);
				var boneId = (VehicleWheelBoneId)SHVDN.NativeMemory.ReadInt32(wheelAddress + SHVDN.NativeMemory.VehicleWheelIdOffset);
				var vehicleWheelInstance = new VehicleWheel(Vehicle, boneId, wheelAddress);
				returnWheelArray[i] = vehicleWheelInstance;

				int boneIndexZeroBased = (int)boneId - 11;
				if (_vehicleWheels[boneIndexZeroBased] == null)
				{
					_vehicleWheels[boneIndexZeroBased] = vehicleWheelInstance;
				}
			}

			return returnWheelArray;
		}
	}
}
