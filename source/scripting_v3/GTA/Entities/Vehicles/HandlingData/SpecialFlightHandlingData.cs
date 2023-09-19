//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using System;

namespace GTA
{
	/// <summary>
	/// Represents the special flight handling data class for <c>CSpecialFlightHandlingData</c>, which is for vehicles that have special flight feature.
	/// </summary>
	public sealed class SpecialFlightHandlingData : BaseSubHandlingData
	{
		internal SpecialFlightHandlingData(IntPtr address, HandlingData parent) : base(address, parent, HandlingType.SpecialFlight)
		{
		}

		/// <summary>
		/// Gets or sets the constant angular damping vector in each axis.
		/// </summary>
		/// <value>
		/// The angular damping vector in each axis.
		/// </value>
		public Vector3 VectorAngularDamping
		{
			get
			{
				if (!IsValid)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(MemoryAddress + 0x10));
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteVector3(MemoryAddress + 0x10, value.ToInternalFVector3());
			}
		}
		/// <summary>
		/// Gets or sets the angular damping vector in each axis that have effects more when the vehicle is moving faster.
		/// Not available in v1.0.1290.1 or v1.0.1365.1.
		/// </summary>
		/// <value>
		/// The angular damping vector in each axis that have effects more when the vehicle is moving faster.
		/// </value>
		public Vector3 VectorAngularDampingMin
		{
			get
			{
				if (!IsValid || Game.Version < GameVersion.v1_0_1493_0_Steam)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(MemoryAddress + 0x20));
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1493_0_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1493_0_Steam), nameof(SpecialFlightHandlingData), nameof(VectorAngularDampingMin));
				}
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteVector3(MemoryAddress + 0x20, value.ToInternalFVector3());
			}
		}
		/// <summary>
		/// Gets or sets the constant linear damping vector in each axis.
		/// </summary>
		/// <value>
		/// The constant linear damping vector in each axis.
		/// </value>
		public Vector3 VectorLinearDamping
		{
			get
			{
				if (!IsValid)
				{
					return Vector3.Zero;
				}

				int offset = Game.Version >= GameVersion.v1_0_1493_0_Steam ? 0x30 : 0x20;
				return new Vector3(SHVDN.NativeMemory.ReadVector3(MemoryAddress + offset));
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1493_0_Steam ? 0x30 : 0x20;
				SHVDN.NativeMemory.WriteVector3(MemoryAddress + offset, value.ToInternalFVector3());
			}
		}
		/// <summary>
		/// Gets or sets the angular damping vector in each axis that have effects more when the vehicle is moving faster.
		/// Not available in v1.0.1290.1 or v1.0.1365.1.
		/// </summary>
		/// <value>
		/// The angular damping vector in each axis that have effects more when the vehicle is moving faster.
		/// </value>
		public Vector3 VectorLinearDampingMin
		{
			get
			{
				if (!IsValid || Game.Version < GameVersion.v1_0_1493_0_Steam)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(MemoryAddress + 0x40));
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1493_0_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1493_0_Steam), nameof(SpecialFlightHandlingData), nameof(VectorLinearDampingMin));
				}
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteVector3(MemoryAddress + 0x40, value.ToInternalFVector3());
			}
		}
		/// <summary>
		/// Gets or sets the roll (local y-axis) torque scale produced by steering.
		/// </summary>
		/// <value>
		/// The roll (local y-axis) torque scale produced by steering.
		/// </value>
		public float RollTorqueScale
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1493_0_Steam ? 0x70 : 0x50;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1493_0_Steam ? 0x70 : 0x50;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}
		/// <summary>
		/// Gets or sets the yaw (local z-axis) torque scale produced by steering.
		/// </summary>
		/// <value>
		/// The yaw (local z-axis) torque scale produced by steering.
		/// </value>
		public float YawTorqueScale
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1493_0_Steam ? 0x7C : 0x5C;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1493_0_Steam ? 0x7C : 0x5C;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}
		/// <summary>
		/// Gets or sets the pitch (local x-axis) torque scale produced by steering.
		/// </summary>
		/// <value>
		/// The pitch (local x-axis) torque scale produced by steering.
		/// </value>
		public float PitchTorqueScale
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1493_0_Steam ? 0x90 : 0x70;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1493_0_Steam ? 0x90 : 0x70;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}
		/// <summary>
		/// Gets or sets the transition duration in seconds.
		/// </summary>
		/// <remarks>
		/// If the value is less than <c>1f / 60f</c> (or <c>1f / (float)(frameRateValue)</c> if the frame rate is more than 60),
		/// The vehicle does not transform from special flight mode back to normal properly, making wheels not placed in correct positions and wings not retracted.
		/// </remarks>
		/// <value>
		/// The transition duration in seconds.
		/// </value>
		public float TransitionDuration
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1493_0_Steam ? 0x9C : 0x7C;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1493_0_Steam ? 0x9C : 0x7C;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}
		/// <summary>
		/// Gets or sets the velocity scale the vehicle can produce by hovering.
		/// </summary>
		/// <value>
		/// The velocity scale the vehicle can produce by hovering.
		/// </value>
		public float HoverVelocityScale
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1493_0_Steam ? 0xA0 : 0x80;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1493_0_Steam ? 0xA0 : 0x80;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}
		/// <summary>
		/// Gets or sets the minimum speed in m/s before thrust falloff will be applied.
		/// Not available in v1.0.1290.1 or v1.0.1365.1.
		/// </summary>
		/// <value>
		/// The minimum speed in m/s before thrust falloff will be applied.
		/// </value>
		public float MinSpeedForThrustFalloff
		{
			get
			{
				if (!IsValid || Game.Version < GameVersion.v1_0_1493_0_Steam)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xA8);
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1493_0_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1493_0_Steam), nameof(SpecialFlightHandlingData), nameof(MinSpeedForThrustFalloff));
				}
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xA8, value);
			}
		}
		/// <summary>
		/// Gets or sets the thrust scale for braking.
		/// Not available in v1.0.1290.1 or v1.0.1365.1.
		/// </summary>
		/// <value>
		/// The thrust scale for braking.
		/// </value>
		public float BrakingThrustScale
		{
			get
			{
				if (!IsValid || Game.Version < GameVersion.v1_0_1493_0_Steam)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xAC);
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1493_0_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1493_0_Steam), nameof(SpecialFlightHandlingData), nameof(BrakingThrustScale));
				}
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xAC, value);
			}
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same special flight handling data as this <see cref="SpecialFlightHandlingData"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true"/> if the <paramref name="obj"/> is the same special flight handling data as this <see cref="SpecialFlightHandlingData"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is SpecialFlightHandlingData data)
			{
				return MemoryAddress == data.MemoryAddress;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="SpecialFlightHandlingData"/>s refer to the same special flight handling data.
		/// </summary>
		/// <param name="left">The left <see cref="SpecialFlightHandlingData"/>.</param>
		/// <param name="right">The right <see cref="SpecialFlightHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is the same special flight handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(SpecialFlightHandlingData left, SpecialFlightHandlingData right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="SpecialFlightHandlingData"/>s don't refer to the special flight handling data.
		/// </summary>
		/// <param name="left">The left <see cref="SpecialFlightHandlingData"/>.</param>
		/// <param name="right">The right <see cref="SpecialFlightHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is not the same special flight handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(SpecialFlightHandlingData left, SpecialFlightHandlingData right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return MemoryAddress.GetHashCode();
		}
	}
}
