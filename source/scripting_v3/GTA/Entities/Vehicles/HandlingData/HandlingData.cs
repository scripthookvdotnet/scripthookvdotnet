//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using System;

namespace GTA
{
	/// <summary>
	/// <para>This class has most regular handling data. Currently compatible with 1.0.2060.0 or later.</para>
	/// <para>
	/// Note that this class gets data from or sets data to the <c>CHandlingData</c> instance as is, and thus not all the handling values don't match the equivalent values in the <c>handling.meta</c> file.
	/// The game multiplies or divides some values after reading values from the <c>handling.meta</c> file.
	/// </para>
	/// </summary>
	public sealed class HandlingData
	{
		internal HandlingData(IntPtr address)
		{
			MemoryAddress = address;
		}

		/// <summary>
		/// Gets the memory address where the <see cref="HandlingData"/> is stored in memory.
		/// </summary>
		public IntPtr MemoryAddress
		{
			get;
		}

		/// <summary>
		/// Returns true if this <see cref="HandlingData"/> is valid.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="HandlingData"/> is valid; otherwise, <see langword="false" />.
		/// </value>
		public bool IsValid => MemoryAddress != IntPtr.Zero;

		/// <summary>
		/// Gets or sets the bias between front and rear for the anti-roll bar.
		/// This value will be set to the equivalent value in the <c>handling.meta</c> multiplied by 2 when <see cref="HandlingData"/> instances are initialized.
		/// </summary>
		/// <value>
		/// The anti roll bar bias front. 0.0f is fully front, 2.0f is fully rear.
		/// </value>
		public float AntiRollBarBiasFront
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xE0);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xE0, value);
			}
		}

		/// <summary>
		/// Gets or sets the spring constant that is transmitted to the opposite wheel when under compression.
		/// Larger numbers result in a larger force being applied.
		/// </summary>
		/// <value>
		/// The anti roll bar force.
		/// </value>
		public float AntiRollBarForce
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xDC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xDC, value);
			}
		}

		public float BoostMaxSpeed
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x130);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x130, value);
			}
		}

		public float BrakeBiasFront
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x74);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x74, value);
			}
		}

		public float BrakeForce
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x6C);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x6C, value);
			}
		}

		public float CamberStiffness
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xAC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xAC, value);
			}
		}

		public Vector3 CenterOfMassOffset
		{
			get
			{
				if (!IsValid)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(MemoryAddress + 0x20));
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteVector3(MemoryAddress + 0x20, value.ToInternalFVector3());
			}
		}

		/// <summary>
		/// Gets or sets the clutch speed multiplier on down shifts.
		/// </summary>
		/// <value>
		/// The clutch speed multiplier on down shifts.
		/// </value>
		public float ClutchChangeRateScaleDownShift
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x5C);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x5C, value);
			}
		}

		/// <summary>
		/// Gets or sets the clutch speed multiplier on up shifts.
		/// </summary>
		/// <value>
		/// The clutch speed multiplier on up shifts.
		/// </value>
		public float ClutchChangeRateScaleUpShift
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x58);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x58, value);
			}
		}

		public float CollisionDamageMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xF0);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xF0, value);
			}
		}

		public float DeformationDamageMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xF8);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xF8, value);
			}
		}

		/// <summary>
		/// Gets or sets the amount of downforce applied to the vehicle.
		/// </summary>
		/// <value>
		/// The amount of downforce applied to the vehicle.
		/// </value>
		public float DownForceModifier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x14);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x14, value);
			}
		}

		/// <summary>
		/// <para>Gets or sets how much the vehicle gives rear axles force. The rest of the force will be given to front axles. This value will be set to the equivalent value in the <c>handling.meta</c> multiplied by 2 when <see cref="HandlingData"/> instances are initialized.</para>
		/// <para>0.0 is rear wheel drive, 2.0 is front wheel drive, and any value between 0.01 and 0.199 is four wheel drive (1.0 give both front and rear axles equal force, being perfect 4WD.)</para>
		/// </summary>
		/// <value>
		/// The percent the vehicle gives rear axles force (between 0.0 to 2.0).
		/// </value>
		public float DriveBiasFront
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x48);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x48, value);
			}
		}

		/// <summary>
		/// Gets or sets the drive inertia that determines how fast the engine acceleration is.
		/// </summary>
		/// <value>
		/// The drive inertia.
		/// </value>
		/// <remarks>
		/// If you want a vehicle with high torque but slow acceleration (e.g. a truck), lower the driver inertia and specify a high drive force.
		/// </remarks>
		public float DriveInertia
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x54);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x54, value);
			}
		}

		public float EngineDamageMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xFC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xFC, value);
			}
		}

		public float HandBrakeForce
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x7C);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x7C, value);
			}
		}

		public Vector3 InertiaMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(MemoryAddress + 0x30));
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteVector3(MemoryAddress + 0x30, value.ToInternalFVector3());
			}
		}

		/// <summary>
		/// Gets or sets the drag coefficient.
		/// </summary>
		/// <value>
		/// The drag coefficient.
		/// </value>
		public float InitialDragCoefficient
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
		/// Gets or sets the power engine produces in top gear.
		/// </summary>
		/// <value>
		/// The power engine that produces in top gear.
		/// </value>
		public float InitialDriveForce
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x60);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x60, value);
			}
		}

		/// <summary>
		/// Gets or sets the number of gears (excluding reverse).
		/// </summary>
		/// <value>
		/// The number of gears (excluding reverse).
		/// </value>
		public int InitialDriveGears
		{
			get
			{
				if (!IsValid)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt32(MemoryAddress + 0x50);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteInt32(MemoryAddress + 0x50, value);
			}
		}

		/// <summary>
		/// Determines the speed at redline in high gear; Controls the final drive of the vehicle's gearbox.
		/// Setting this value does not guarantee the vehicle will reach this speed.
		/// </summary>
		/// <value>
		/// the speed at redline in high gear.
		/// </value>
		public float InitialDriveMaxFlatVelocity
		{
			get
			{
				if (!IsValid)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x68);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x68, value);
			}
		}

		/// <summary>
		/// How much traction is reduced at low speed, 0.0 means normal traction. It affects mainly car burnout (spinning wheels when car doesn't move) when pressing gas.
		/// Decreasing value will cause less burnout, less sliding at start. However, the higher value, the more burnout car gets.
		/// </summary>
		/// <value>
		/// How much traction is reduced at low speed.
		/// </value>
		public float LowSpeedTractionLossMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xA8);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xA8, value);
			}
		}

		/// <summary>
		/// Gets or sets the weight.
		/// </summary>
		/// <value>
		/// The weight in Kilograms.
		/// </value>
		public float Mass
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

		public int MonetaryValue
		{
			get
			{
				if (!IsValid)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt32(MemoryAddress + 0x118);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteInt32(MemoryAddress + 0x118, value);
			}
		}

		/// <summary>
		/// Gets or sets the amount of oil.
		/// </summary>
		/// <value>
		/// The amount of oil.
		/// </value>
		public float OilVolume
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x104);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x104, value);
			}
		}

		/// <summary>
		/// Gets or sets the percentage of the "floating height" after it falls into the water, before sinking.
		/// </summary>
		/// <value>
		/// The percentage between 0 and 1.
		/// </value>
		/// <remarks>
		/// The default value for vanilla land vehicles is 0.85. The value will stop sinking the vehicle to float for a moment before sinking.
		/// An invalid number will cause the vehicle to sink without the driver drowning.
		/// </remarks>
		public float PercentSubmerged
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x40);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x40, value);
			}
		}

		/// <summary>
		/// Gets or sets the amount of petrol that will leak after damaging a vehicle's tank.
		/// </summary>
		/// <value>
		/// The amount of petrol.
		/// </value>
		public float PetrolTankVolume
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x100);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x100, value);
			}
		}

		/// <summary>
		/// Gets or Sets the rate at which a vehicle consumes petrol (gasoline).
		/// </summary>
		/// <value>
		/// The petrol consumption rate.
		/// </value>
		/// <remarks>
		/// <para>The default value in vanilla handling.meta files is 0.5f.</para>
		/// <para>There is a good chance that this attribute may be used in missions where there is a script that changes the gas level of the vehicle.</para>
		/// <para>It should probably be used more frequently to adjust how fast the vehicle's petrol leaks.</para>
		/// </remarks>
		public float PetrolConsumptionRate
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x108);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x108, value);
			}
		}


		/// <summary>
		/// Gets or sets the rotation values in degree the parts pop-up headlights needs to be rotated when headlights are on.
		/// </summary>
		/// <value>
		/// The rotation values in degree. Can be negative.
		/// </value>
		public float PopUpLightRotation
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x18);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x18, value);
			}
		}

		public float RocketBoostCapacity
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x120);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x120, value);
			}
		}

		public float RollCenterHeightFront
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xE8);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xE8, value);
			}
		}

		public float RollCenterHeightRear
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xEC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xEC, value);
			}
		}

		public float SeatOffsetDistanceX
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x10C);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x10C, value);
			}
		}

		public float SeatOffsetDistanceY
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x110);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x110, value);
			}
		}

		public float SeatOffsetDistanceZ
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x114);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x114, value);
			}
		}

		/// <summary>
		/// <para>
		/// Gets or sets a value that multiplies the game's calculation of the angle of the steer wheel will turn while at full turn in radians.
		/// Steering lock is directly related to over/under-steer.
		/// </para>
		/// <para>When <see cref="HandlingData"/> instances are initialized, the game converts the value in degrees read from <c>handling.meta</c> to radians before this value is initialized.</para>
		/// </summary>
		/// <value>
		/// The value that multiplies the game's calculation of the angle of the steer wheel in radians, between 0.01 and above.
		/// </value>
		public float SteeringLock
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x80);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x80, value);
			}
		}

		/// <summary>
		/// Gets or sets the damping scale bias between front and rear wheels.
		/// This value determines which suspension is stronger, front or rear.
		/// This value will be set to the equivalent value in the <c>handling.meta</c> multiplied by 2 when <see cref="HandlingData"/> instances are initialized.
		/// </summary>
		/// <value>
		/// The suspension bias front.
		/// </value>
		/// <remarks>
		/// if more wheels at back (e.g. trucks), front suspension should be stronger.
		/// </remarks>
		public float SuspensionBiasFront
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xD4);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xD4, value);
			}
		}

		/// <summary>
		/// Gets or sets the damping during strut compression.
		/// This value will be set to the equivalent value in the <c>handling.meta</c> divided by 10 when <see cref="HandlingData"/> instances are initialized.
		/// </summary>
		/// <value>
		/// The damping during strut compression.
		/// </value>
		public float SuspensionCompressionDamping
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xC0);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xC0, value);
			}
		}

		/// <summary>
		/// Gets or sets the suspension force.
		/// Lower limit for zero force at full extension is calculated using (1.0f / (force * number of wheels)).
		/// </summary>
		/// <value>
		/// The suspension force.
		/// </value>
		public float SuspensionForce
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xBC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xBC, value);
			}
		}

		/// <summary>
		/// Gets or sets how far the wheels can move down from their original position.
		/// </summary>
		/// <value>
		/// The suspension lower limit.
		/// </value>
		public float SuspensionLowerLimit
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xCC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xCC, value);
			}
		}

		/// <summary>
		/// Gets or sets the adjustment from artist positioning.
		/// </summary>
		/// <value>
		/// The suspension raise.
		/// </value>
		public float SuspensionRaise
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xD0);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xD0, value);
			}
		}

		/// <summary>
		/// Gets or sets the damping during strut rebound.
		/// This value will be set to the equivalent value in the <c>handling.meta</c> divided by 10 when <see cref="HandlingData"/> instances are initialized.
		/// </summary>
		/// <value>
		/// The damping during strut rebound.
		/// </value>
		public float SuspensionReboundDamping
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xC4);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xC4, value);
			}
		}

		/// <summary>
		/// Gets or sets how far the wheels can move up from their original position.
		/// </summary>
		/// <value>
		/// The suspension upper limit.
		/// </value>
		public float SuspensionUpperLimit
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xC8);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xC8, value);
			}
		}

		/// <summary>
		/// Gets or sets the value that determines the distribution of traction from front to rear.
		/// This value will be set to the equivalent value in the <c>handling.meta</c> multiplied by 2 when <see cref="HandlingData"/> instances are initialized.
		/// </summary>
		/// <value>
		/// The value that determines distribution of traction from front to rear.
		/// </value>
		public float TractionBiasFront
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xB0);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xB0, value);
			}
		}

		public float TractionCurveMax
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x88);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x88, value);
			}
		}

		public float TractionCurveMin
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x90);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x90, value);
			}
		}

		public float TractionCurveLateral
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x98);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x98, value);
			}
		}

		/// <summary>
		/// Gets or sets how much traction is affected by material grip differences from 1.0f.
		/// </summary>
		/// <value>
		/// The traction loss multiplier.
		/// </value>
		public float TractionLossMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xB8);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xB8, value);
			}
		}

		/// <summary>
		/// Gets or sets the maximum distance for traction spring.
		/// </summary>
		/// <value>
		/// The traction loss multiplier.
		/// </value>
		public float TractionSpringDeltaMax
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xA0);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xA0, value);
			}
		}

		public float WeaponDamageMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0xF4);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0xF4, value);
			}
		}

		/// <summary>
		/// Gets of Sets the multiplier for how much damage a vehicle takes from weapons.
		/// </summary>
		/// <value>
		/// The weapon damage scaling multiplier.
		/// </value>
		/// <remarks>
		/// <para>The default value in vanilla handling.meta files is 0.5f.</para>
		/// <para>This attribute scales the amount of damage a vehicle takes based on its remaining health. A vehicle that is nearly destroyed will take less damage from weapons than a fully healthy vehicle. Changing this attribute can affect the vehicle's durability in combat situations.</para>
		/// </remarks>
		public float WeaponDamageScaledToVehicleHealthMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x168 : 0x160;
				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + offset);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_1180_2_Steam ? 0x168 : 0x160;
				SHVDN.NativeMemory.WriteFloat(MemoryAddress + offset, value);
			}
		}

		/// <summary>Gets the <see cref="GTA.BikeHandlingData"/> of this <see cref="HandlingData"/>.</summary>
		/// <value>A <see cref="GTA.BikeHandlingData"/> of the <see cref="HandlingData"/>.</value>
		/// <remarks>If the <see cref="HandlingData"/> does not have a bike handling data, this property returns <see langword="null"/>.</remarks>
		public BikeHandlingData BikeHandlingData
		{
			get
			{
				IntPtr bikeHandlingDataAddress = GetSubHandlingData(HandlingType.Bike);
				return bikeHandlingDataAddress != IntPtr.Zero ? new BikeHandlingData(bikeHandlingDataAddress, this) : null;
			}
		}
		/// <summary>Gets the non-vertical <see cref="GTA.FlyingHandlingData"/> of this <see cref="HandlingData"/>.</summary>
		/// <value>A non-vertical <see cref="GTA.FlyingHandlingData"/> of the <see cref="HandlingData"/>.</value>
		/// <remarks>If the <see cref="HandlingData"/> does not have a non-vertical flying handling data, this property returns <see langword="null"/>.</remarks>
		public FlyingHandlingData FlyingHandlingData
		{
			get
			{
				IntPtr flyingHandlingDataAddress = GetSubHandlingData(HandlingType.Flying);
				return flyingHandlingDataAddress != IntPtr.Zero ? new FlyingHandlingData(flyingHandlingDataAddress, this, HandlingType.Flying) : null;
			}
		}
		/// <summary>Gets the vertical <see cref="GTA.FlyingHandlingData"/> of this <see cref="HandlingData"/>.</summary>
		/// <value>A <see cref="GTA.FlyingHandlingData"/> of the <see cref="HandlingData"/>.</value>
		/// <remarks>If the <see cref="HandlingData"/> does not have a vertical flying handling data, this property returns <see langword="null"/>.</remarks>
		public FlyingHandlingData VerticalFlyingHandlingData
		{
			get
			{
				IntPtr flyingHandlingDataAddress = GetSubHandlingData(HandlingType.VerticalFlying);
				return flyingHandlingDataAddress != IntPtr.Zero ? new FlyingHandlingData(flyingHandlingDataAddress, this, HandlingType.VerticalFlying) : null;
			}
		}
		/// <summary>Gets the vertical <see cref="GTA.BoatHandlingData"/> of this <see cref="HandlingData"/>.</summary>
		/// <value>A <see cref="GTA.BoatHandlingData"/> of the <see cref="HandlingData"/>.</value>
		/// <remarks>If the <see cref="HandlingData"/> does not have a boat handling data, this property returns <see langword="null"/>.</remarks>
		public BoatHandlingData BoatHandlingData
		{
			get
			{
				IntPtr boatHandlingDataAddress = GetSubHandlingData(HandlingType.Boat);
				return boatHandlingDataAddress != IntPtr.Zero ? new BoatHandlingData(boatHandlingDataAddress, this) : null;
			}
		}
		/// <summary>Gets the vertical <see cref="GTA.SeaPlaneHandlingData"/> of this <see cref="HandlingData"/>.</summary>
		/// <value>A <see cref="GTA.SeaPlaneHandlingData"/> of the <see cref="HandlingData"/>.</value>
		/// <remarks>If the <see cref="HandlingData"/> does not have a sea plane handling data, this property returns <see langword="null"/>.</remarks>
		public SeaPlaneHandlingData SeaPlaneHandlingData
		{
			get
			{
				IntPtr seaPlaneHandlingDataAddress = GetSubHandlingData(HandlingType.SeaPlane);
				return seaPlaneHandlingDataAddress != IntPtr.Zero ? new SeaPlaneHandlingData(seaPlaneHandlingDataAddress, this) : null;
			}
		}
		/// <summary>Gets the vertical <see cref="GTA.SubmarineHandlingData"/> of this <see cref="HandlingData"/>.</summary>
		/// <value>A <see cref="GTA.SubmarineHandlingData"/> of the <see cref="HandlingData"/>.</value>
		/// <remarks>If the <see cref="HandlingData"/> does not have a submarine handling data, this property returns <see langword="null"/>.</remarks>
		public SubmarineHandlingData SubmarineHandlingData
		{
			get
			{
				IntPtr submarineHandlingDataAddress = GetSubHandlingData(HandlingType.Boat);
				return submarineHandlingDataAddress != IntPtr.Zero ? new SubmarineHandlingData(submarineHandlingDataAddress, this) : null;
			}
		}
		/// <summary>Gets the vertical <see cref="GTA.TrailerHandlingData"/> of this <see cref="HandlingData"/>.</summary>
		/// <value>A <see cref="GTA.TrailerHandlingData"/> of the <see cref="HandlingData"/>.</value>
		/// <remarks>If the <see cref="HandlingData"/> does not have a trailer handling data, this property returns <see langword="null"/>.</remarks>
		public TrailerHandlingData TrailerHandlingData
		{
			get
			{
				IntPtr trailerHandlingDataAddress = GetSubHandlingData(HandlingType.Trailer);
				return trailerHandlingDataAddress != IntPtr.Zero ? new TrailerHandlingData(trailerHandlingDataAddress, this) : null;
			}
		}
		/// <summary>Gets the <see cref="GTA.CarHandlingData"/> of this <see cref="HandlingData"/>.</summary>
		/// <value>A vertical <see cref="GTA.CarHandlingData"/> of the <see cref="HandlingData"/>.</value>
		/// <remarks>If the <see cref="HandlingData"/> does not have a car handling data, this property returns <see langword="null"/>.</remarks>
		public CarHandlingData CarHandlingData
		{
			get
			{
				IntPtr carHandlingDataAddress = GetSubHandlingData(HandlingType.Car);
				return carHandlingDataAddress != IntPtr.Zero ? new CarHandlingData(carHandlingDataAddress, this) : null;
			}
		}
		/// <summary>Gets the <see cref="GTA.VehicleWeaponHandlingData"/> of this <see cref="HandlingData"/>.</summary>
		/// <value>A vertical <see cref="GTA.VehicleWeaponHandlingData"/> of the <see cref="HandlingData"/>.</value>
		/// <remarks>If the <see cref="HandlingData"/> does not have a vehicle weapon handling data, this property returns <see langword="null"/>.</remarks>
		public VehicleWeaponHandlingData VehicleWeaponHandlingData
		{
			get
			{
				IntPtr vehicleWeaponHandlingDataAddress = GetSubHandlingData(HandlingType.Weapon);
				return vehicleWeaponHandlingDataAddress != IntPtr.Zero ? new VehicleWeaponHandlingData(vehicleWeaponHandlingDataAddress, this) : null;
			}
		}

		private IntPtr GetSubHandlingData(HandlingType type)
		{
			if (!IsValid)
			{
				return IntPtr.Zero;
			}

			return SHVDN.NativeMemory.Vehicle.GetSubHandlingData(MemoryAddress, (int)type);
		}

		public static HandlingData GetByHash(int handlingNameHash)
		{
			return new HandlingData(SHVDN.NativeMemory.GetHandlingDataByHandlingNameHash(handlingNameHash));
		}
		public static HandlingData GetByVehicleModel(Model VehicleModel)
		{
			return new HandlingData(SHVDN.NativeMemory.GetHandlingDataByModelHash(VehicleModel.Hash));
		}

		public override bool Equals(object obj)
		{
			if (obj is HandlingData data)
			{
				return MemoryAddress == data.MemoryAddress;
			}

			return false;
		}

		public static bool operator ==(HandlingData left, HandlingData right)
		{
			return left?.Equals(right) ?? right is null;
		}
		public static bool operator !=(HandlingData left, HandlingData right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return MemoryAddress.GetHashCode();
		}
	}
}
