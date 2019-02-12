using System;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	public class HandlingData : IEquatable<HandlingData>
	{
		internal HandlingData(IntPtr address)
		{
			MemoryAddress = address;
		}

		/// <summary>
		/// Gets the memory address where the <see cref="HandlingData"/> is stored in memory.
		/// </summary>
		public IntPtr MemoryAddress { get; private set; }

		/// <summary>
		/// Returns true if this <see cref="HandlingData"/> is valid.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="HandlingData"/> is valid; otherwise, <c>false</c>.
		/// </value>
		public bool IsValid
		{
			get
			{
				return MemoryAddress != IntPtr.Zero;
			}
		}

		/// <summary>
		/// Gets or sets the bias between front and rear for the anti-roll bar.
		/// </summary>
		/// <value>
		/// The anti roll bar bias front. 0.0f is fully front, 1.0f is fully rear.
		/// </value>
		public float AntiRollBarBiasFront
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return MemoryAccess.ReadFloat(MemoryAddress + 0xE0);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xE0, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xDC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xDC, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x6C);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x6C, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xAC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xAC, value);
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

				return MemoryAccess.ReadVector3(MemoryAddress + 0x20);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteVector3(MemoryAddress + 0x20, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x5C);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x5C, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x58);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x58, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xF0);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xF0, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xF8);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xF8, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x54);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x54, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xFC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xFC, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x7C);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x7C, value);
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

				return MemoryAccess.ReadVector3(MemoryAddress + 0x30);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteVector3(MemoryAddress + 0x30, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x60);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x60, value);
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

				return MemoryAccess.ReadInt(MemoryAddress + 0x50);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteInt(MemoryAddress + 0x50, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xC, value);
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

				return MemoryAccess.ReadInt(MemoryAddress + 0x118);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteInt(MemoryAddress + 0x118, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x104);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x104, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x40);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x40, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x100);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x100, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xE8);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xE8, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xEC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xEC, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x10C);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x10C, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x110);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x110, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x114);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x114, value);
			}
		}

		/// <summary>
		/// Gets or sets a value that multiplies the game's calculation of the angle of the steer wheel will turn while at full turn.
		/// Steering lock is directly related to over/under-steer.
		/// </summary>
		/// <value>
		/// The value that multiplies the game's calculation of the angle of the steer wheel, between 0.01 and above.
		/// </value>
		public float SteeringLock
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return MemoryAccess.ReadFloat(MemoryAddress + 0x80);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x80, value);
			}
		}

		/// <summary>
		/// Gets or sets the damping scale bias between front and rear wheels.
		/// This value determines which suspension is stronger, front or rear.
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xD4);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xD4, value);
			}
		}

		public float SuspensionCompressionDamping
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return MemoryAccess.ReadFloat(MemoryAddress + 0xC0);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xC0, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xBC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xBC, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xCC);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xCC, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xD0);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xD0, value);
			}
		}

		public float SuspensionReboundDamping
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return MemoryAccess.ReadFloat(MemoryAddress + 0xC4);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xC4, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xC8);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xC8, value);
			}
		}

		/// <summary>
		/// Gets or sets the value that determines the distribution of traction from front to rear.
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xB0);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xB0, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x88);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x88, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0x90);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0x90, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xA8);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xA8, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xA0);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xA0, value);
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

				return MemoryAccess.ReadFloat(MemoryAddress + 0xF4);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				MemoryAccess.WriteFloat(MemoryAddress + 0xF4, value);
			}
		}

		static public HandlingData GetByHash(int handlingNameHash)
		{
			return new HandlingData(MemoryAccess.GetHandlingDataByHandlingNameHash(handlingNameHash));
		}
		static public HandlingData GetByVehicleModel(Model VehicleModel)
		{
			return new HandlingData(MemoryAccess.GetHandlingDataByModelHash(VehicleModel.Hash));
		}

		public bool Equals(HandlingData handlingData)
		{
			return MemoryAddress == handlingData.MemoryAddress;
		}
		public override bool Equals(object obj)
		{
			return obj != null && Equals((HandlingData)obj);
		}

		public override int GetHashCode()
		{
			return MemoryAddress.GetHashCode();
		}

		public static bool operator ==(HandlingData left, HandlingData right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(HandlingData left, HandlingData right)
		{
			return !left.Equals(right);
		}
	}
}
