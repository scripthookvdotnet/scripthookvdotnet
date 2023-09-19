//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Represents the vehicle weapon handling data class for <c>CVehicleWeaponHandlingData</c>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// You can use up to 6 elements per array for weapons and turrets in v1.0.1180.2 or later game versions (although turret arrays can internally contain up to 12 elements).
	///	In the game versions prior to v1.0.1180.2, you can use up to 4 elements per array for weapons and 3 elements per array for turrets.
	/// </para>
	/// <para>
	/// Resetting vehicle part states will be needed to apply the changes to existing vehicles that use this sub handling data, such as changing vehicle mods or calling <see cref="Vehicle.Repair()"/>.
	/// </para>
	/// </remarks>
	public sealed class VehicleWeaponHandlingData : BaseSubHandlingData
	{
		static readonly int s_elemCountForWeaponPropertyArrays;
		// Vehicle turret arrays can have up to 12 elements since b1180, but it looks like only 6 element can be actually used
		static readonly int s_elemCountActuallyUsedForTurretPropertyArrays;

		static VehicleWeaponHandlingData()
		{
			// Although we could get element count from parStructure->parMemberArray->parMemberDefinition,
			// it won't worth it because only one update changed max element count of array members of CVehicleWeaponHandlingData as of b2845
			if (Game.Version >= GameVersion.v1_0_1180_2_Steam)
			{
				s_elemCountForWeaponPropertyArrays = 6;
				s_elemCountActuallyUsedForTurretPropertyArrays = 6;
			}
			else
			{
				s_elemCountForWeaponPropertyArrays = 4;
				s_elemCountActuallyUsedForTurretPropertyArrays = 3;
			}
		}

		internal VehicleWeaponHandlingData(IntPtr address, HandlingData parent) : base(address, parent, HandlingType.Weapon)
		{
		}

		/// <summary>
		/// Gets or sets usable vehicle weapon hashes.
		/// </summary>
		public VehicleWeaponHash[] WeaponHash
		{
			get
			{
				if (!IsValid)
				{
					return Array.Empty<VehicleWeaponHash>();
				}

				var result = new VehicleWeaponHash[s_elemCountForWeaponPropertyArrays];

				const int memberOffset = 0x8;
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = (VehicleWeaponHash)SHVDN.NativeMemory.ReadInt32(MemoryAddress + memberOffset + i * 4);
				}

				return result;
			}
			set
			{
				if (value.Length > s_elemCountForWeaponPropertyArrays)
				{
					throw new ArgumentException($"The amount of {nameof(WeaponHash)} values values must be between 0 and {s_elemCountForWeaponPropertyArrays.ToString()}.", nameof(value));
				}
				if (!IsValid)
				{
					return;
				}

				int[] arrayToFill = new int[s_elemCountForWeaponPropertyArrays];
				for (int i = 0; i < value.Length; i++)
				{
					// VehicleWeaponHash.Invalid should have been zero since the game always treats zero as a invalid weapon hash but not necessarily for 0xFFFFFFFF
					if (value[i] == VehicleWeaponHash.Invalid)
					{
						continue;
					}

					arrayToFill[i] = (int)value[i];
				}

				const int memberOffset = 0x8;
				for (int i = 0; i < arrayToFill.Length; i++)
				{
					SHVDN.NativeMemory.WriteInt32(MemoryAddress + memberOffset + i * 4, (int)arrayToFill[i]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the seat number indices that can control the weapons.
		/// For example, when <see cref="VehicleSeat.Passenger"/> is set at index 1,
		/// the <see cref="Ped"/> on the passenger seat can use the weapon at the index 1 of <see cref="WeaponHash"/>.
		/// </summary>
		/// <remarks>
		/// Set <see cref="VehicleSeat.Driver"/> if the weapon hash is not set.
		/// </remarks>
		public VehicleSeat[] WeaponSeats
		{
			get
			{
				if (!IsValid)
				{
					return Array.Empty<VehicleSeat>();
				}

				var result = new VehicleSeat[s_elemCountForWeaponPropertyArrays];

				int memberOffset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x20 : 0x18;
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = (VehicleSeat)SHVDN.NativeMemory.ReadInt32(MemoryAddress + memberOffset + i * 4);
				}

				// Make values match what native functions uses for vehicle seats
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = (VehicleSeat)((int)result[i] - 1);
				}

				return result;
			}
			set
			{
				if (value.Length > s_elemCountForWeaponPropertyArrays)
				{
					throw new ArgumentException($"The amount of {nameof(VehicleSeat)} values must be between 0 and {s_elemCountForWeaponPropertyArrays.ToString()}.", nameof(value));
				}
				if (!IsValid)
				{
					return;
				}

				int[] arrayToFill = new int[s_elemCountForWeaponPropertyArrays];
				for (int i = 0; i < value.Length; i++)
				{
					// Make values match what game code uses outside native functions
					arrayToFill[i] = (int)value[i] + 1;
				}

				int memberOffset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x20 : 0x18;
				for (int i = 0; i < arrayToFill.Length; i++)
				{
					SHVDN.NativeMemory.WriteInt32(MemoryAddress + memberOffset + i * 4, arrayToFill[i]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the vehicle mod type values which seats vehicle weapons are related to.
		/// Setting incorrect values may result in wrong vehicle weapon handling.
		/// </summary>
		/// <remarks>
		/// <para>
		/// <see cref="VehicleModType.None"/> indicates no mod type is related, which will be set when no value is specified in the <c>handling.meta</c> file.
		/// </para>
		/// <para>
		/// Only available in v1.0.1103.2 or later game versions.
		/// </para>
		/// </remarks>
		public VehicleModType[] WeaponVehicleModType
		{
			get
			{
				if (Game.Version < GameVersion.v1_0_1103_2_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1103_2_Steam), nameof(VehicleWeaponHandlingData), nameof(WeaponVehicleModType));
				}
				if (!IsValid)
				{
					return Array.Empty<VehicleModType>();
				}

				var result = new VehicleModType[s_elemCountForWeaponPropertyArrays];
				int memberOffset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x38 : 0x28;
				for (int i = 0; i < result.Length; i++)
				{
					int modTypeForNative = SHVDN.NativeMemory.ReadInt32(MemoryAddress + memberOffset + i * 4);
					if (modTypeForNative == (int)VehicleModType.None)
					{
						result[i] = VehicleModType.None;
						continue;
					}

					result[i] = GetModTypeForNativeFunction(modTypeForNative);
				}

				return result;
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1103_2_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1103_2_Steam), nameof(VehicleWeaponHandlingData), nameof(WeaponVehicleModType));
				}
				if (value.Length > s_elemCountForWeaponPropertyArrays)
				{
					throw new ArgumentException($"The amount of {nameof(VehicleModType)} values values must be between 0 and {s_elemCountForWeaponPropertyArrays.ToString()}.", nameof(value));
				}
				if (!IsValid)
				{
					return;
				}

				int[] arrayToFill = new int[s_elemCountForWeaponPropertyArrays];
				for (int i = 0; i < value.Length; i++)
				{
					VehicleModType currentValue = value[i];
					if (currentValue == VehicleModType.None)
					{
						arrayToFill[i] = (int)VehicleModType.None;
						continue;
					}

					arrayToFill[i] = GetModTypeValueForInternalGameCode(currentValue);
				}

				int memberOffset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x38 : 0x28;
				for (int i = 0; i < arrayToFill.Length; i++)
				{
					SHVDN.NativeMemory.WriteInt32(MemoryAddress + memberOffset + i * 4, arrayToFill[i]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the max turret speed in angular speed.
		/// </summary>
		public float[] TurretSpeed
		{
			get
			{
				if (!IsValid)
				{
					return Array.Empty<float>();
				}

				int memberOffset;
				if (Game.Version >= GameVersion.v1_0_1180_2_Steam)
				{
					memberOffset = 0x50;
				}
				else if (Game.Version >= GameVersion.v1_0_1103_2_Steam)
				{
					memberOffset = 0x38;
				}
				else
				{
					memberOffset = 0x28;
				}

				float[] result = new float[s_elemCountActuallyUsedForTurretPropertyArrays];
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = SHVDN.NativeMemory.ReadFloat(MemoryAddress + memberOffset + i * 4);
				}

				return result;
			}
			set
			{
				if (value.Length > s_elemCountActuallyUsedForTurretPropertyArrays)
				{
					throw new ArgumentException($"The amount of turret speed values must be between 0 and {s_elemCountActuallyUsedForTurretPropertyArrays.ToString()}.", nameof(value));
				}
				if (!IsValid)
				{
					return;
				}

				int memberOffset;
				if (Game.Version >= GameVersion.v1_0_1180_2_Steam)
				{
					memberOffset = 0x50;
				}
				else if (Game.Version >= GameVersion.v1_0_1103_2_Steam)
				{
					memberOffset = 0x38;
				}
				else
				{
					memberOffset = 0x28;
				}

				float[] arrayToFill = new float[s_elemCountActuallyUsedForTurretPropertyArrays];
				for (int i = 0; i < value.Length; i++)
				{
					arrayToFill[i] = value[i];
				}

				for (int i = 0; i < arrayToFill.Length; i++)
				{
					SHVDN.NativeMemory.WriteFloat(MemoryAddress + memberOffset + i * 4, arrayToFill[i]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the minimum turret pitch in radians.
		/// </summary>
		public float[] TurretPitchMin
		{
			get
			{
				if (!IsValid)
				{
					return Array.Empty<float>();
				}

				int memberOffset;
				if (Game.Version >= GameVersion.v1_0_1180_2_Steam)
				{
					memberOffset = 0x80;
				}
				else if (Game.Version >= GameVersion.v1_0_1103_2_Steam)
				{
					memberOffset = 0x44;
				}
				else
				{
					memberOffset = 0x34;
				}

				float[] result = new float[s_elemCountActuallyUsedForTurretPropertyArrays];
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = SHVDN.NativeMemory.ReadFloat(MemoryAddress + memberOffset + i * 4);
				}

				return result;
			}
			set
			{
				if (value.Length > s_elemCountActuallyUsedForTurretPropertyArrays)
				{
					throw new ArgumentException($"The amount of minimum turret pitch values must be between 0 and {s_elemCountActuallyUsedForTurretPropertyArrays.ToString()}.", nameof(value));
				}
				if (!IsValid)
				{
					return;
				}

				int memberOffset;
				if (Game.Version >= GameVersion.v1_0_1180_2_Steam)
				{
					memberOffset = 0x80;
				}
				else if (Game.Version >= GameVersion.v1_0_1103_2_Steam)
				{
					memberOffset = 0x44;
				}
				else
				{
					memberOffset = 0x34;
				}

				float[] arrayToFill = new float[s_elemCountActuallyUsedForTurretPropertyArrays];
				for (int i = 0; i < value.Length; i++)
				{
					arrayToFill[i] = value[i];
				}

				for (int i = 0; i < arrayToFill.Length; i++)
				{
					SHVDN.NativeMemory.WriteFloat(MemoryAddress + memberOffset + i * 4, arrayToFill[i]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the maximum turret pitch in radians.
		/// </summary>
		public float[] TurretPitchMax
		{
			get
			{
				if (!IsValid)
				{
					return Array.Empty<float>();
				}

				int memberOffset;
				if (Game.Version >= GameVersion.v1_0_1180_2_Steam)
				{
					memberOffset = 0xB0;
				}
				else if (Game.Version >= GameVersion.v1_0_1103_2_Steam)
				{
					memberOffset = 0x50;
				}
				else
				{
					memberOffset = 0x40;
				}

				float[] result = new float[s_elemCountActuallyUsedForTurretPropertyArrays];
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = SHVDN.NativeMemory.ReadFloat(MemoryAddress + memberOffset + i * 4);
				}

				return result;
			}
			set
			{
				if (value.Length > s_elemCountActuallyUsedForTurretPropertyArrays)
				{
					throw new ArgumentException($"The amount of maximum turret pitch values must be between 0 and {s_elemCountActuallyUsedForTurretPropertyArrays.ToString()}.", nameof(value));
				}
				if (!IsValid)
				{
					return;
				}

				int memberOffset;
				if (Game.Version >= GameVersion.v1_0_1180_2_Steam)
				{
					memberOffset = 0xB0;
				}
				else if (Game.Version >= GameVersion.v1_0_1103_2_Steam)
				{
					memberOffset = 0x50;
				}
				else
				{
					memberOffset = 0x40;
				}

				float[] arrayToFill = new float[s_elemCountActuallyUsedForTurretPropertyArrays];
				for (int i = 0; i < value.Length; i++)
				{
					arrayToFill[i] = value[i];
				}

				for (int i = 0; i < arrayToFill.Length; i++)
				{
					SHVDN.NativeMemory.WriteFloat(MemoryAddress + memberOffset + i * 4, arrayToFill[i]);
				}
			}
		}

		private int GetModTypeValueForInternalGameCode(VehicleModType modTypeForNativeFunction)
		{
			// This kind of correction was introduced in b393 so return the same value if the game version is earlier than b393
			if (Game.Version < GameVersion.v1_0_393_2_Steam)
			{
				return (int)modTypeForNativeFunction;
			}

			int modTypeForNativeFunctionInt = (int)modTypeForNativeFunction;
			if (modTypeForNativeFunctionInt > 10)
			{
				if ((uint)modTypeForNativeFunctionInt > 24u)
				{
					return modTypeForNativeFunctionInt - 14;
				}
				else
				{
					return modTypeForNativeFunctionInt + 25;
				}
			}

			return modTypeForNativeFunctionInt;
		}

		private VehicleModType GetModTypeForNativeFunction(int modTypeForInternalCode)
		{
			if (Game.Version < GameVersion.v1_0_393_2_Steam)
			{
				return (VehicleModType)modTypeForInternalCode;
			}

			if (35 < modTypeForInternalCode && modTypeForInternalCode < 50)
			{
				return (VehicleModType)(modTypeForInternalCode - 25);
			}
			else if (10 < modTypeForInternalCode && modTypeForInternalCode < 36)
			{
				return (VehicleModType)(modTypeForInternalCode + 14);
			}

			return (VehicleModType)modTypeForInternalCode;
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same vehicle weapon handling data as this <see cref="FlyingHandlingData"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true"/> if the <paramref name="obj"/> is the same vehicle weapon handling data as this <see cref="FlyingHandlingData"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is VehicleWeaponHandlingData data)
			{
				return MemoryAddress == data.MemoryAddress;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="VehicleWeaponHandlingData"/>s refer to the same vehicle weapon handling data.
		/// </summary>
		/// <param name="left">The left <see cref="VehicleWeaponHandlingData"/>.</param>
		/// <param name="right">The right <see cref="VehicleWeaponHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is the same vehicle weapon handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(VehicleWeaponHandlingData left, VehicleWeaponHandlingData right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="VehicleWeaponHandlingData"/>s don't refer to the same vehicle weapon handling data.
		/// </summary>
		/// <param name="left">The left <see cref="VehicleWeaponHandlingData"/>.</param>
		/// <param name="right">The right <see cref="VehicleWeaponHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is not the same vehicle weapon handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(VehicleWeaponHandlingData left, VehicleWeaponHandlingData right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return MemoryAddress.GetHashCode();
		}
	}
}
