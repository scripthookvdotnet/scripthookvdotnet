//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// <para>
	/// Represents the car handling data class for <c>CCarHandlingData</c>.
	/// </para>
	/// <para>
	/// Only <c>fBackEndPopUpCarImpulseMult</c>, <c>fBackEndPopUpBuildingImpulseMult</c>, and <c>fBackEndPopUpMaxDeltaSpeed</c> are available in all the versions prior to v1.0.1365.1.
	/// The other values are available only in v1.0.1365.1 or later game versions.
	/// </para>
	/// </summary>
	public sealed class CarHandlingData : BaseSubHandlingData
	{
		internal CarHandlingData(IntPtr address, HandlingData parent) : base(address, parent, HandlingType.Car)
		{
		}

		/// <summary>
		/// Gets or sets the toe of the vehicle's front wheels in radians. Positive value makes front wheels toe-in and Negative value makes front wheels toe-out.
		/// Only available in v1.0.1365.1 or later game versions.
		/// </summary>
		/// <value>
		/// The toe of the vehicle's front wheels in radians.
		/// </value>
		public float ToeFront
		{
			get
			{
				if (!IsValid || Game.Version < GameVersion.v1_0_1365_1_Steam)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x14);
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1365_1_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_1365_1_Steam, nameof(CarHandlingData), nameof(ToeFront));
				}

				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x14, value);
			}
		}
		/// <summary>
		/// Gets or sets the toe of the vehicle's rear wheels in radians. Positive value makes rear wheels toe-in and Negative value makes rear wheels toe-out.
		/// Only available in v1.0.1365.1 or later game versions.
		/// </summary>
		/// <value>
		/// The toe of the vehicle's rear wheels in radians.
		/// </value>
		public float ToeRear
		{
			get
			{
				if (!IsValid || Game.Version < GameVersion.v1_0_1365_1_Steam)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x18);
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1365_1_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1365_1_Steam), nameof(CarHandlingData), nameof(ToeRear));
				}

				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x18, value);
			}
		}
		/// <summary>
		/// Gets or sets the camber of the vehicle's front wheels.
		/// Only available in v1.0.1365.1 or later game versions.
		/// </summary>
		/// <value>
		/// The camber of the vehicle's front wheels in radians.
		/// </value>
		public float CamberFront
		{
			get
			{
				if (!IsValid || Game.Version < GameVersion.v1_0_1365_1_Steam)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x1C);
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1365_1_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1365_1_Steam), nameof(CarHandlingData), nameof(CamberFront));
				}

				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x1C, value);
			}
		}
		/// <summary>
		/// Gets or sets the camber of the vehicle's rear wheels.
		/// Only available in v1.0.1365.1 or later game versions.
		/// </summary>
		/// <value>
		/// The camber of the vehicle's rear wheels in radians.
		/// </value>
		public float CamberRear
		{
			get
			{
				if (!IsValid || Game.Version < GameVersion.v1_0_1365_1_Steam)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x20);
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1365_1_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1365_1_Steam), nameof(CarHandlingData), nameof(CamberRear));
				}

				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x20, value);
			}
		}
		/// <summary>
		/// Gets or sets the castor angle of the vehicle's wheels in radians.
		/// Only available in v1.0.1365.1 or later game versions.
		/// </summary>
		/// <value>
		/// The castor angle of the vehicle's wheels in radians.
		/// </value>
		public float Castor
		{
			get
			{
				if (!IsValid || Game.Version < GameVersion.v1_0_1365_1_Steam)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x24);
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1365_1_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1365_1_Steam), nameof(CarHandlingData), nameof(Castor));
				}

				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x24, value);
			}
		}
		/// <summary>
		/// Gets or sets the engine resistance. The higher the value is, the slower the vehicles accelerate.
		/// Only available in v1.0.1365.1 or later game versions.
		/// </summary>
		/// <value>
		/// The engine resistance.
		/// </value>
		public float EngineResistance
		{
			get
			{
				if (!IsValid || Game.Version < GameVersion.v1_0_1365_1_Steam)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x28);
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1365_1_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1365_1_Steam), nameof(CarHandlingData), nameof(EngineResistance));
				}

				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x28, value);
			}
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same car handling data as this <see cref="CarHandlingData"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true"/> if the <paramref name="obj"/> is the same car handling data as this <see cref="CarHandlingData"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is CarHandlingData data)
			{
				return MemoryAddress == data.MemoryAddress;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="CarHandlingData"/>s refer to the same car handling data.
		/// </summary>
		/// <param name="left">The left <see cref="CarHandlingData"/>.</param>
		/// <param name="right">The right <see cref="CarHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is the same car handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(CarHandlingData left, CarHandlingData right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="CarHandlingData"/>s don't refer to the same car handling data.
		/// </summary>
		/// <param name="left">The left <see cref="CarHandlingData"/>.</param>
		/// <param name="right">The right <see cref="CarHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is not the same car handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(CarHandlingData left, CarHandlingData right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return MemoryAddress.GetHashCode();
		}
	}
}
