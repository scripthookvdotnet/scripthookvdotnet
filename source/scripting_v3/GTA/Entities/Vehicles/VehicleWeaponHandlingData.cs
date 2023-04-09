//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;

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
		static readonly int _elemCountForWeaponPropertyArrays;
		static readonly int _elemCountActuallyUsedForTurretPropertyArrays;
		// This field is used only when we get how many elements are needed to be filled
		static readonly int _elemCountForTurretPropertyArrays;

		static VehicleWeaponHandlingData()
		{
			// Although we could get element count from parStructure->parMemberArray->parMemberDefinition,
			// it won't worth it because only one update changed max element count of array members of CVehicleWeaponHandlingData as of b2845
			if (Game.Version >= GameVersion.v1_0_1180_2_Steam)
			{
				_elemCountForWeaponPropertyArrays = 6;
				_elemCountActuallyUsedForTurretPropertyArrays = 6;
				_elemCountForTurretPropertyArrays = 12;
			}
			else
			{
				_elemCountForWeaponPropertyArrays = 4;
				_elemCountActuallyUsedForTurretPropertyArrays = 3;
				_elemCountForTurretPropertyArrays = 3;
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

				var result = new VehicleWeaponHash[_elemCountForWeaponPropertyArrays];

				var memberOffset = 0x8;
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = (VehicleWeaponHash)SHVDN.NativeMemory.ReadInt32(MemoryAddress + memberOffset + i * 4);
				}

				return result;
			}
			set
			{
				if (value.Length > _elemCountForWeaponPropertyArrays)
				{
					throw new ArgumentException("The amount of WeaponHash must be between 0 and " + _elemCountForWeaponPropertyArrays.ToString(), nameof(value));
				}
				if (!IsValid)
				{
					return;
				}

				var arrayToFill = new int[_elemCountForWeaponPropertyArrays];
				for (int i = 0; i < value.Length; i++)
				{
					// VehicleWeaponHash.Invalid should have been zero since the game always treats zero as a invalid weapon hash but not necessarily for 0xFFFFFFFF
					if (value[i] == VehicleWeaponHash.Invalid)
					{
						continue;
					}

					arrayToFill[i] = (int)value[i];
				}

				var memberOffset = 0x8;
				for (int i = 0; i < arrayToFill.Length; i++)
				{
					SHVDN.NativeMemory.WriteInt32(MemoryAddress + memberOffset + i * 4, (int)arrayToFill[i]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the seat number indicies that can control the weapons.
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

				var result = new VehicleSeat[_elemCountForWeaponPropertyArrays];

				var memberOffset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x20 : 0x18;
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
				if (value.Length > _elemCountForWeaponPropertyArrays)
				{
					throw new ArgumentException("The amount of VehicleSeat must be between 0 and " + _elemCountForWeaponPropertyArrays.ToString(), nameof(value));
				}
				if (!IsValid)
				{
					return;
				}

				var arrayToFill = new int[_elemCountForWeaponPropertyArrays];
				for (int i = 0; i < value.Length; i++)
				{
					// Make values match what game code uses outside native functions
					arrayToFill[i] = (int)value[i] + 1;
				}

				var memberOffset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x20 : 0x18;
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
		/// <see cref="VehicleModType.None"/> incidates no mod type is related, which will be set when no value is specified in the <c>handling.meta</c> file.
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
					throw new GameVersionNotSupportedException(GameVersion.v1_0_1103_2_Steam, nameof(VehicleWeaponHandlingData), nameof(WeaponVehicleModType));
				}
				if (!IsValid)
				{
					return Array.Empty<VehicleModType>();
				}

				var result = new VehicleModType[_elemCountForWeaponPropertyArrays];
				var memberOffset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x38 : 0x28;
				for (int i = 0; i < result.Length; i++)
				{
					var modTypeForNative = SHVDN.NativeMemory.ReadInt32(MemoryAddress + memberOffset + i * 4);
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
					throw new GameVersionNotSupportedException(GameVersion.v1_0_1103_2_Steam, nameof(VehicleWeaponHandlingData), nameof(WeaponVehicleModType));
				}
				if (value.Length > _elemCountForWeaponPropertyArrays)
				{
					throw new ArgumentException("The amount of VehicleModType must be between 0 and " + _elemCountForWeaponPropertyArrays.ToString(), nameof(value));
				}
				if (!IsValid)
				{
					return;
				}

				var arrayToFill = new int[_elemCountForWeaponPropertyArrays];
				for (int i = 0; i < value.Length; i++)
				{
					var currentValue = value[i];
					if (currentValue == VehicleModType.None)
					{
						arrayToFill[i] = (int)VehicleModType.None;
						continue;
					}

					arrayToFill[i] = GetModTypeValueForInternalGameCode(currentValue);
				}

				var memberOffset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x38 : 0x28;
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

				var result = new float[_elemCountActuallyUsedForTurretPropertyArrays];
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = SHVDN.NativeMemory.ReadFloat(MemoryAddress + memberOffset + i * 4);
				}

				return result;
			}
			set
			{
				if (value.Length > _elemCountActuallyUsedForTurretPropertyArrays)
				{
					throw new ArgumentException("The amount of turret speed values must be between 0 and " + _elemCountActuallyUsedForTurretPropertyArrays.ToString(), nameof(value));
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

				var arrayToFill = new float[_elemCountForWeaponPropertyArrays];
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

				var result = new float[_elemCountActuallyUsedForTurretPropertyArrays];
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = SHVDN.NativeMemory.ReadFloat(MemoryAddress + memberOffset + i * 4);
				}

				return result;
			}
			set
			{
				if (value.Length > _elemCountActuallyUsedForTurretPropertyArrays)
				{
					throw new ArgumentException("The amount of minimum turret pitch values must be between 0 and " + _elemCountActuallyUsedForTurretPropertyArrays.ToString(), nameof(value));
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

				var arrayToFill = new float[_elemCountForWeaponPropertyArrays];
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

				var result = new float[_elemCountActuallyUsedForTurretPropertyArrays];
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = SHVDN.NativeMemory.ReadFloat(MemoryAddress + memberOffset + i * 4);
				}

				return result;
			}
			set
			{
				if (value.Length > _elemCountActuallyUsedForTurretPropertyArrays)
				{
					throw new ArgumentException("The amount of maximum turret pitch values must be between 0 and " + _elemCountActuallyUsedForTurretPropertyArrays.ToString(), nameof(value));
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

				var arrayToFill = new float[_elemCountForWeaponPropertyArrays];
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
				return MemoryAddress == data.MemoryAddress && Parent == data.Parent;
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
			return left is null ? right is null : left.Equals(right);
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
