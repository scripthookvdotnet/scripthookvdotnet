//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using System;

namespace GTA
{
	/// <summary>
	/// <para>
	/// Represents the flying handling data class for <c>CFlyingHandlingData</c>, which is for aircraft.
	/// </para>
	/// </summary>
	public sealed class FlyingHandlingData : BaseSubHandlingData
	{
		// CFlyingHandlingData actually returns the handling type value from handling type member in its virtual function for getting handling type, but we don't use that function for now
		internal FlyingHandlingData(IntPtr address, HandlingData parent, HandlingType handlingType) : base(address, parent, handlingType)
		{
		}

		/// <summary>
		/// Gets or sets the thrust power value.
		/// </summary>
		/// <value>
		/// The thrust power value.
		/// </value>
		public float Thrust
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x8);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x8, value);
			}
		}
		/// <summary>
		/// Gets or sets the thrust powerloss value with speed.
		/// </summary>
		/// <value>
		/// The thrust powerloss value with speed.
		/// </value>
		public float ThrustFallOff
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xC, value);
			}
		}
		/// <summary>
		/// Gets or sets the thrust direction adjustment value based on steering. Higher value makes the airplane more sensitive.
		/// </summary>
		/// <value>
		/// The thrust direction adjustment value based on steering.
		/// </value>
		public float ThrustVectoring
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x10);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x10, value);
			}
		}
		/// <summary>
		/// Gets or sets the yaw input strength, which scales up with speed.
		/// </summary>
		/// <value>
		/// The yaw input strength, which scales up with speed.
		/// </value>
		public float YawMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x1C : 0x14;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x1C : 0x14;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}
		/// <summary>
		/// Gets or sets the yaw resistance value, which scales up with speed.
		/// </summary>
		/// <value>
		/// The yaw resistance value, which scales up with speed.
		/// </value>
		public float YawStabilize
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x20 : 0x18;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x20 : 0x18;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}
		/// <summary>
		/// Gets or sets the sideways resistance, which scales up with speed.
		/// </summary>
		/// <value>
		/// The sideways resistance, which scales up with speed.
		/// </value>
		public float SideSlipMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x24 : 0x1C;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x24 : 0x1C;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}
		/// <summary>
		/// Gets or sets the roll input strength, which scales up with speed.
		/// </summary>
		/// <value>
		/// The roll input strength, which scales up with speed.
		/// </value>
		public float RollMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x2C : 0x20;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x2C : 0x20;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}
		/// <summary>
		/// Gets or sets the roll resistance value, which scales up with speed.
		/// </summary>
		/// <value>
		/// The roll resistance value, which scales up with speed.
		/// </value>
		public float RollStabilize
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x30 : 0x24;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x30 : 0x24;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}
		/// <summary>
		/// Gets or sets the pitch input strength, which scales up with speed.
		/// </summary>
		/// <value>
		/// The pitch input strength, which scales up with speed.
		/// </value>
		public float PitchMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x38 : 0x28;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x38 : 0x28;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}
		/// <summary>
		/// Gets or sets the pitch resistance value, which scales up with speed.
		/// </summary>
		/// <value>
		/// The pitch resistance value, which scales up with speed.
		/// </value>
		public float PitchStabilize
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x3C : 0x2C;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x3C : 0x2C;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}

		/// <summary>
		/// Gets or sets the base lift factor that's independent from wing's attack angle.
		/// Affects the tendency to point relative upwards with speed.
		/// </summary>
		/// <value>
		/// The base lift factor that's independent from wing's attack angle.
		/// </value>
		public float FormLiftMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x44 : 0x30;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x44 : 0x30;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}

		/// <summary>
		/// Gets or sets the lift factor of the wing's attack angle while rising, which scales up with speed.
		/// </summary>
		/// <value>
		/// The lift factor of the wing's attack angle while rising, which scales up with speed.
		/// </value>
		public float AttackLiftMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x48 : 0x34;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x48 : 0x34;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}

		/// <summary>
		/// Gets or sets the lift factor of the wing's attack angle while diving, which scales up with speed.
		/// </summary>
		/// <value>
		/// The lift factor of the wing's attack angle while diving, which scales up with speed.
		/// </value>
		public float AttackDiveMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x4C : 0x38;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x4C : 0x38;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}

		/// <summary>
		/// Gets or sets the extra drag when the gear (and in some planes, the flaps too) is down, which scales up with speed.
		/// </summary>
		/// <value>
		/// The extra drag when the gear (and in some planes, the flaps too) is down, which scales up with speed.
		/// </value>
		public float GearDownDragV
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x50 : 0x3C;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x50 : 0x3C;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}

		/// <summary>
		/// Gets or sets the Lift multiplier when the gear (and in some planes, the flaps too) is down.
		/// </summary>
		/// <value>
		/// The Lift multiplier when the gear (and in some planes, the flaps too) is down.
		/// </value>
		public float GearDownLiftMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x54 : 0x40;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x54 : 0x40;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}

		/// <summary>
		/// Gets or sets the wind influence factor.
		/// </summary>
		/// <value>
		/// The wind influence factor.
		/// </value>
		public float WindMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x58 : 0x44;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x58 : 0x44;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}

		/// <summary>
		/// Gets or sets a form of air drag/resistance factor, which is separate from <see cref="HandlingData.InitialDragCoefficient"/> and works in a less natural way.
		/// It is advised to change <see cref="HandlingData.InitialDragCoefficient"/> to adjust the air drag behavior instead.
		/// </summary>
		/// <value>
		/// A form of air drag/resistance factor, which is separate from <see cref="HandlingData.InitialDragCoefficient"/>.
		/// </value>
		public float MoveResistance
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x5C : 0x48;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x5C : 0x48;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}

		/// <summary>
		/// Gets or sets the resistance value to rotation. the lower the value is, the less natural wobble.
		/// </summary>
		/// <remarks>
		/// When you set a <see cref="Vector3"/> with some components zero, the airplane will behave the same in corresponding axes before the new value is set.
		/// </remarks>
		/// <value>
		/// The resistance value to rotation.
		/// </value>
		public Vector3 VectorTurnResistance
		{
			get
			{
				if (!IsValid)
				{
					return Vector3.Zero;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x60 : 0x50;
				return new Vector3(SHVDN.NativeMemory.ReadVector3(MemoryAddress + offset));
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x60 : 0x50;
				SHVDN.NativeMemory.WriteVector3(MemoryAddress + offset, value.ToInternalFVector3());
			}
		}

		/// <summary>
		/// Gets or sets an unknown resistance value to rotation or speed.
		/// </summary>
		/// <remarks>
		/// When you set a <see cref="Vector3"/> with some components zero, the airplane will behave the same in corresponding axes before the new value is set.
		/// </remarks>
		/// <value>
		/// An unknown resistance value to rotation or speed.
		/// </value>
		public Vector3 VectorSpeedResistance
		{
			get
			{
				if (!IsValid)
				{
					return Vector3.Zero;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x70 : 0x60;
				return new Vector3(SHVDN.NativeMemory.ReadVector3(MemoryAddress + offset));
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x70 : 0x60;
				SHVDN.NativeMemory.WriteVector3(MemoryAddress + offset, value.ToInternalFVector3());
			}
		}

		/// <summary>
		/// Gets or sets the lift factor that will be made by rolling. Higher value will make more force in local x-axis.
		/// Only available in v1.0.1180.2 or later game versions.
		/// </summary>
		/// <value>
		/// The lift factor that will be made by rolling.
		/// </value>
		public float ExtraLiftWithRoll
		{
			get
			{
				if (!IsValid || Game.Version < GameVersion.v1_0_1180_2_Steam)
				{
					return 0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xC4);
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_1180_2_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1180_2_Steam), nameof(FlyingHandlingData), nameof(ExtraLiftWithRoll));
				}

				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xC4, value);
			}
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same flying handling data as this <see cref="FlyingHandlingData"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true"/> if the <paramref name="obj"/> is the same flying handling data as this <see cref="FlyingHandlingData"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is FlyingHandlingData data)
			{
				return MemoryAddress == data.MemoryAddress;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="FlyingHandlingData"/>s refer to the same flying handling data.
		/// </summary>
		/// <param name="left">The left <see cref="FlyingHandlingData"/>.</param>
		/// <param name="right">The right <see cref="FlyingHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is the same flying handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(FlyingHandlingData left, FlyingHandlingData right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="FlyingHandlingData"/>s don't refer to the same flying handling data.
		/// </summary>
		/// <param name="left">The left <see cref="FlyingHandlingData"/>.</param>
		/// <param name="right">The right <see cref="FlyingHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is not the same flying handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(FlyingHandlingData left, FlyingHandlingData right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return MemoryAddress.GetHashCode();
		}
	}
}
