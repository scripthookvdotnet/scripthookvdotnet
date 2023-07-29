//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
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
		VehicleWheel _nullWheel;
		#endregion

		internal VehicleWheelCollection(Vehicle owner)
		{
			Vehicle = owner;
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
		public VehicleWheel this[ScriptVehicleWheelIndex index]
		{
			get
			{
				int indexInt = (int)index;
				if (indexInt < 0 || indexInt > 7)
				{
					throw new ArgumentOutOfRangeException(nameof(index));
				}

				VehicleWheelBoneId boneId = VehicleWheel.vehicleWheelBoneIndexTableForNatives[indexInt];
				int boneIndexZeroBased = (int)boneId - 11;
				return _vehicleWheels[boneIndexZeroBased] ?? (_vehicleWheels[boneIndexZeroBased] = new VehicleWheel(Vehicle, index));
			}
		}

		[Obsolete("Use VehicleWheelCollection.this[ScriptVehicleWheelIndex] instead.")]
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

				VehicleWheelBoneId boneId = VehicleWheel.vehicleWheelBoneIndexTableForNatives[index];
				int boneIndexZeroBased = (int)boneId - 11;
				return _vehicleWheels[boneIndexZeroBased] ?? (_vehicleWheels[boneIndexZeroBased] = new VehicleWheel(Vehicle, index));
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

			IntPtr vehicleAddr = Vehicle.MemoryAddress;
			if (vehicleAddr == IntPtr.Zero)
			{
				return null;
			}

			IntPtr wheelAddr = SHVDN.NativeMemory.Vehicle.GetVehicleWheelAddressByIndexOfWheelArray(vehicleAddr, index);
			if (wheelAddr == IntPtr.Zero)
			{
				return null;
			}

			var boneId = (VehicleWheelBoneId)SHVDN.NativeMemory.ReadInt32(wheelAddr + SHVDN.NativeMemory.Vehicle.WheelIdOffset);
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

			int wheelCount = Count;
			for (int i = 0; i < wheelCount; i++)
			{
				IntPtr vehicleAddr = Vehicle.MemoryAddress;
				if (vehicleAddr == IntPtr.Zero)
				{
					yield break;
				}

				IntPtr wheelAddress = SHVDN.NativeMemory.Vehicle.GetVehicleWheelAddressByIndexOfWheelArray(vehicleAddr, i);

				var boneId = (VehicleWheelBoneId)SHVDN.NativeMemory.ReadInt32(wheelAddress + SHVDN.NativeMemory.Vehicle.WheelIdOffset);
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
				IntPtr address = Vehicle.MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelCountOffset == 0)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt32(address + SHVDN.NativeMemory.Vehicle.WheelCountOffset);
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

			IntPtr vehicleAddress = Vehicle.MemoryAddress;
			if (vehicleAddress == IntPtr.Zero || SHVDN.NativeMemory.Vehicle.WheelCountOffset == 0)
			{
				return Array.Empty<VehicleWheel>();
			}

			int wheelCount = SHVDN.NativeMemory.ReadInt32(vehicleAddress + SHVDN.NativeMemory.Vehicle.WheelCountOffset);
			var returnWheelArray = new VehicleWheel[wheelCount];

			for (int i = 0; i < returnWheelArray.Length; i++)
			{
				IntPtr wheelAddress = SHVDN.NativeMemory.Vehicle.GetVehicleWheelAddressByIndexOfWheelArray(vehicleAddress, i);
				var boneId = (VehicleWheelBoneId)SHVDN.NativeMemory.ReadInt32(wheelAddress + SHVDN.NativeMemory.Vehicle.WheelIdOffset);
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
