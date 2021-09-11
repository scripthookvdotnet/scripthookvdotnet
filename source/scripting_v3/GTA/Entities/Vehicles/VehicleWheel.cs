//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using GTA.Native;

namespace GTA
{
	public sealed class VehicleWheel
	{
		#region Fields
		IntPtr _cachedAddress;

		internal static readonly VehicleWheelBoneId[] vehicleWheelBoneIndexTableForNatives = {
			VehicleWheelBoneId.WheelLeftFront,
			VehicleWheelBoneId.WheelRightFront,
			VehicleWheelBoneId.WheelLeftMiddle1,
			VehicleWheelBoneId.WheelRightMiddle1,
			VehicleWheelBoneId.WheelLeftRear,
			VehicleWheelBoneId.WheelRightRear,
			VehicleWheelBoneId.WheelLeftFront,
			VehicleWheelBoneId.WheelLeftRear,
		};
		#endregion

		internal VehicleWheel(Vehicle owner, int index)
		{
			Vehicle = owner;

			#region Index Assignment
#pragma warning disable CS0618
			switch (index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					BoneId = vehicleWheelBoneIndexTableForNatives[index];
					Index = index;
					break;
				default:
					BoneId = VehicleWheelBoneId.Invalid;
					Index = -1;
					break;
			}
#pragma warning restore CS0618
			#endregion
		}

		internal VehicleWheel(Vehicle owner, VehicleWheelBoneId boneIndex)
		{
			Vehicle = owner;
			BoneId = boneIndex;

			#region Index Assignment
#pragma warning disable CS0618
			switch (boneIndex)
			{
				case VehicleWheelBoneId.WheelLeftFront:
				case VehicleWheelBoneId.WheelRightFront:
					Index = (int)boneIndex - 11;
					break;
				case VehicleWheelBoneId.WheelLeftRear:
				case VehicleWheelBoneId.WheelRightRear:
					Index = (int)boneIndex - 9;
					break;
				case VehicleWheelBoneId.WheelLeftMiddle1:
				case VehicleWheelBoneId.WheelRightMiddle1:
					Index = (int)boneIndex - 13;
					break;
				default:
					// Natives for vehicle wheels don't support the middle 2 wheels or middle 3 wheels
					// Can fetch some correct value even if any value outside 0 to 7 is passed as the wheel id to the natives, but it's kind of a undefined behavior because the array for wheel id has only 8 elements
					Index = -1;
					break;
			}
#pragma warning restore CS0618
			#endregion
		}

		internal VehicleWheel(Vehicle owner, VehicleWheelBoneId boneIndex, IntPtr wheelAddress) : this(owner, boneIndex)
		{
			_cachedAddress = wheelAddress;
		}

		public Vehicle Vehicle
		{
			get;
		}

		[Obsolete("VehicleWheel.Index does not support any of the wheels wheel_lm2, wheel_rm2, wheel_lm3, or wheel_lm3 for legacy scripts compatibility in v3 API. Use VehicleWheel.BoneId instead.")]
		public int Index
		{
			get;
		}

		public VehicleWheelBoneId BoneId
		{
			get;
		}

		public IntPtr MemoryAddress
		{
			get
			{
				if (!IsBoneIdValid(BoneId))
					return IntPtr.Zero;

				// Check if the vehicle is not boat, train, or submarine. This also checks if the vehicle exists (0xFFFFFFFF will be returned if doesn't exist)
				if (!CanVehicleHaveWheels(Vehicle))
						return IntPtr.Zero;

				if (_cachedAddress != IntPtr.Zero)
					return _cachedAddress;

				return GetMemoryAddressInit();
			}
		}

		public bool IsTouchingSurface
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero)
					return false;

				return SHVDN.NativeMemory.IsWheelTouchingSurface(address, Vehicle.MemoryAddress);
			}
		}

		public bool IsBurst
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.VehicleTireHealthOffset == 0)
					return false;

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.VehicleWheelHealthOffset) < 1000f;
			}
		}
		public bool IsBurstCompletely
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.VehicleTireHealthOffset == 0)
					return false;

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.VehicleTireHealthOffset) == 0f;
			}
		}
		public float Health
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.VehicleWheelHealthOffset == 0)
					return 0f;

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.VehicleWheelHealthOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.VehicleWheelHealthOffset == 0)
					return;

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.VehicleWheelHealthOffset, value);
			}
		}
		public float TireHealth
		{
			get
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.VehicleTireHealthOffset == 0)
					return 0f;

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.VehicleTireHealthOffset);
			}
			set
			{
				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.VehicleTireHealthOffset == 0)
					return;

				// Change value to 0 if the value is negative. IS_VEHICLE_TYRE_BURST returns true only if value is exactly 0 when the 3rd parameter (the bool completely) is a non-zero value.
				if (value < 0f)
					value = 0f;

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.VehicleTireHealthOffset, value);
			}
		}
		public float WearMultiplier
		{
			get
			{
				if (Game.Version < GameVersion.v1_0_1868_0_Steam)
					return 0f;

				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.VehicleTireWearMultiplierOffset == 0)
					return 0f;

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.VehicleTireWearMultiplierOffset);
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1868_0_Steam)
					throw new GameVersionNotSupportedException(GameVersion.v1_0_1868_0_Steam, nameof(VehicleWheel), nameof(WearMultiplier));

				var address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.VehicleTireWearMultiplierOffset == 0)
					return;

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.VehicleTireWearMultiplierOffset, value);
			}
		}

		public void Fix()
		{
			// Do what SET_VEHICLE_TYRE_FIXED exactly does
			Fix(false);
		}
		public void Fix(bool leaveOtherBurstedTiresNotShowing)
		{
			var address = MemoryAddress;
			if (address == IntPtr.Zero)
				return;

			SHVDN.NativeMemory.FixVehicleWheel(address);

			if (!leaveOtherBurstedTiresNotShowing)
			{
				var customShaderEffectVehicleAddr = SHVDN.NativeMemory.ReadAddress(SHVDN.NativeMemory.ReadAddress(Vehicle.MemoryAddress + 0x48) + 0x20);
				SHVDN.NativeMemory.ClearBit(customShaderEffectVehicleAddr + SHVDN.NativeMemory.ShouldShowOnlyVehicleTiresWithPositiveHealthOffset, 1);
			}
		}

		public void Puncture(float damage = 1000f)
		{
			var address = MemoryAddress;
			if (address == IntPtr.Zero)
				return;

			// Do what SET_VEHICLE_TYRE_BURST exactly does with false (zero) as 3rd parameter
			SHVDN.NativeMemory.PunctureTire(address, damage, Vehicle.MemoryAddress);
		}

		public void Burst()
		{
			var address = MemoryAddress;
			if (address == IntPtr.Zero)
				return;

			// Do what SET_VEHICLE_TYRE_BURST exactly does with true (non-zero) as 3rd parameter and 1000f as 4th parameter
			SHVDN.NativeMemory.BurstTireOnRim(address, Vehicle.MemoryAddress);
		}

		// boats, trains, and submarines cannot have wheels
		internal static bool CanVehicleHaveWheels(Vehicle vehicle) => (uint)vehicle.Type <= 0xC;
		private IntPtr GetMemoryAddressInit()
		{
			var vehicleAddr = Vehicle.MemoryAddress;

			var wheelIndexOfArrayPtr = SHVDN.NativeMemory.ReadByte(vehicleAddr + SHVDN.NativeMemory.WheelBoneIdToPtrArrayIndexOffset + ((int)BoneId - 11));
			if (wheelIndexOfArrayPtr == 0xFF)
				return IntPtr.Zero;

			var vehicleWheelArrayAddr = SHVDN.NativeMemory.ReadAddress(vehicleAddr + SHVDN.NativeMemory.WheelPtrArrayOffset);
			_cachedAddress = SHVDN.NativeMemory.ReadAddress(vehicleWheelArrayAddr + 0x8 * wheelIndexOfArrayPtr);

			return _cachedAddress;
		}
		internal static bool IsBoneIdValid(VehicleWheelBoneId boneId)
		{
			switch (boneId)
			{
				case VehicleWheelBoneId.WheelLeftFront:
				case VehicleWheelBoneId.WheelRightFront:
				case VehicleWheelBoneId.WheelLeftRear:
				case VehicleWheelBoneId.WheelRightRear:
				case VehicleWheelBoneId.WheelLeftMiddle1:
				case VehicleWheelBoneId.WheelRightMiddle1:
				case VehicleWheelBoneId.WheelLeftMiddle2:
				case VehicleWheelBoneId.WheelRightMiddle2:
				case VehicleWheelBoneId.WheelLeftMiddle3:
				case VehicleWheelBoneId.WheelRightMiddle3:
					return true;
				default:
					return false;
			}
		}
	}
}
