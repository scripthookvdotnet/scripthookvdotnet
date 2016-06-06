using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using GTA.Native;

namespace GTA
{

	public sealed class VehicleWheel
	{
		#region Fields
		Vehicle _owner;
		#endregion

		internal VehicleWheel(Vehicle owner, int index)
		{
			_owner = owner;
			Index = index;

			int wheelsPtrOffset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0xAA0 : 0xA80;
			int wheelOffset = (index * 8);
			if (index == 45 && _owner.HasBone("wheel_lm2")) //wheel_lm2 is assigned to 45 on the native functions, but it's index is 4 in the wheel collection struct.
			{
				index = 4;
			}
			if (index == 47 && _owner.HasBone("wheel_rm2")) //wheel_lm2 is assigned to 47 on the native functions, but it's index is 5 in the wheel collection struct.
			{
				index = 5;
			}

			if (index < 0 || 19 < index)
			{
				new ArgumentOutOfRangeException("index", "Index values must be between 0 and 19 except for 45 of wheel_lm2 or 47 of wheel_lm2, inclusive.");
			}

			MemoryAddress = MemoryAccess.ReadPtr(MemoryAccess.ReadPtr(owner.MemoryAddress + wheelsPtrOffset) + (index * 8));
		}

		public int Index { get; private set; }
		public IntPtr MemoryAddress { get; private set; }

		public bool Exists
		{
			get
			{
				return Vehicle.Exists() && MemoryAddress != IntPtr.Zero;
			}
		}
		public bool IsBurst
		{
			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_TYRE_BURST, _owner.Handle, Index, false);
			}
		}
		public bool IsBurstCompletely
		{
			get
			{
				return Function.Call<bool>(Hash.IS_VEHICLE_TYRE_BURST, _owner.Handle, Index, true);
			}
		}
		public bool IsTireOnFire
		{
			get
			{
				if (MemoryAddress != IntPtr.Zero)
				{
					int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x1EC : 0x1DC;
					return (MemoryAccess.ReadByte(MemoryAddress + offset) & (1 << 3)) != 0;
				}

				return false;
			}
		}
		public bool IsTouching
		{
			get
			{
				if (MemoryAddress != IntPtr.Zero)
				{
					int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x1EC : 0x1DC;
					return (MemoryAccess.ReadByte(MemoryAddress + offset) & (1 << 0)) != 0;
				}

				return false;
			}
		}
		public float WheelHealth
		{
			get
			{
				if (MemoryAddress != IntPtr.Zero)
				{
					int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x1E0 : 0x1D0;
					return MemoryAccess.ReadFloat(MemoryAddress + offset);
				}

				return 0.0f;
			}
		}
		public float TireHealth
		{
			get
			{
				if (MemoryAddress != IntPtr.Zero)
				{
					int offset = Game.Version >= GameVersion.v1_0_372_2_Steam ? 0x1E4 : 0x1D4;
					return MemoryAccess.ReadFloat(MemoryAddress + offset);
				}

				return 0.0f;
			}
		}
		public Vehicle Vehicle
		{
			get { return _owner; }
		}

		public void Burst()
		{
			Function.Call(Hash.SET_VEHICLE_TYRE_BURST, _owner.Handle, Index, true, 1000f);
		}
		public void Fix()
		{
			Function.Call(Hash.SET_VEHICLE_TYRE_FIXED, _owner.Handle, Index);
		}
	}
}
