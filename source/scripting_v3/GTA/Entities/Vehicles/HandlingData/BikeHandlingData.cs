//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Represents the bike handling data class for <c>CBikeHandlingData</c>, which is for motorcycles, quad bikes (also trikes), and bicycles.
	/// All the values this class exposes are available in all game versions.
	/// </summary>
	public sealed class BikeHandlingData : BaseSubHandlingData
	{
		internal BikeHandlingData(IntPtr address, HandlingData parent) : base(address, parent, HandlingType.Bike)
		{
		}

		/// <summary>
		/// Gets or sets the value indicating how much the center of mass is shifted by leaning forward in meters.
		/// </summary>
		/// <value>
		/// The value indicating how much the center of mass is shifted by leaning forward in meters.
		/// </value>
		public float LeanForwardCenterOfMassMultiplier
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
		/// Gets or sets the value indicating how much force is applied when leaning forward.
		/// </summary>
		/// <value>
		/// The value indicating how much force is applied when leaning forward.
		/// </value>
		public float LeanForwardForceMultiplier
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
		/// Gets or sets the value indicating how much the center of mass is shifted by leaning backward in meters.
		/// </summary>
		/// <value>
		/// The value indicating how much the center of mass is shifted by leaning backward in meters.
		/// </value>
		public float LeanBackCenterOfMassMultiplier
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
		/// Gets or sets the value indicating how much force is applied when leaning backward.
		/// </summary>
		/// <value>
		/// The value indicating how much force is applied when leaning backward.
		/// </value>
		public float LeanBackwardForceMultiplier
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
		/// Gets or sets The max angle of bike lean when cornering in radians.
		/// </summary>
		/// <value>
		/// The max angle of bike lean when cornering in radians.
		/// </value>
		public float MaxBankAngle
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
		/// <summary>
		/// Gets or sets the angle where the driver's body starts to fully lean when cornering.
		/// </summary>
		/// <value>
		/// The angle where the driver's body starts to fully lean when cornering in radians.
		/// </value>
		public float FullAnimAngle
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x1C);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x1C, value);
			}
		}
		/// <summary>
		/// Gets or sets the value indicating how slowly the driver body lean changes by steering.
		/// </summary>
		/// <value>
		/// The value indicating how much slowly the driver body lean changes by steering.
		/// </value>
		public float DesLeanReturnFraction
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x24);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x24, value);
			}
		}

		/// <summary>
		/// Gets or sets the multiplier indicating how quickly the bike leans by steering.
		/// </summary>
		/// <value>
		/// The multiplier indicating how quickly the bike leans by steering.
		/// </value>
		public float StickLeanMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x28);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x28, value);
			}
		}

		/// <summary>
		/// Gets or sets the value for brake bias/balance value. If this value is negative, the bike will have rear brake bias.
		/// This value has effect even when there is no steer input.
		/// </summary>
		/// <value>
		/// The value for brake bias/balance value.
		/// </value>
		public float BrakingStabilityMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x2C);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x2C, value);
			}
		}

		/// <summary>
		/// Gets or sets the strength multiplier of in-air steering (which applies when no tires are touching surface).
		/// This value should be negative to let the bike steer just like when it's on ground and let the bike upside up when the driver is not leaning forward or backward.
		/// Pitch input still works even when this value is zero, but the bike will try to get upside down when this value is positive.
		/// </summary>
		/// <value>
		/// The value for brake bias/balance value.
		/// </value>
		public float InAirSteerMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x30);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x30, value);
			}
		}

		/// <summary>
		/// Gets or sets the value that affects how much x-axis rotation is needed in radians before an additional positive x-Axis angular force applies to the bike, which will keep the bike from easily wheelieing more.
		/// Only affects when the bike is wheelieing. Should be positive to not let the bike easily wheelie more than a certain angle, but easily wheelie to the specified angle.
		/// The more the difference is, the more positive x-Axis angular force the bike will receive.
		/// </summary>
		/// <remarks>
		/// The formula <c>(float)Math.Asin(Vector3.Dot(Vehicle.ForwardVector, VehicleWheel.SurfaceNormalVector)) - fWheelieBalancePoint</c> (where the deg to rad conversion has been already applied to <c>fWheelieBalancePoint</c>) determines whether to apply negative x-Axis angular force.
		/// If the result is more than 0.05, an additional positive x-Axis angular force will be applied.
		/// </remarks>
		/// <value>
		/// The value that affects how much x-axis rotation is needed before an additional positive x-Axis angular force applies to the bike when the bike is wheelieing in radians.
		/// </value>
		public float WheelieBalancePoint
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x34);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x34, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that affects how much x-axis rotation is needed in radians before an additional negative x-Axis angular force applies to the bike, which will keep the bike from easily stoppieing more.
		/// Only affects when the bike is stoppieing. Should be negative to not let the bike stoppie wheelie more than a certain angle, but easily stoppie to the specified angle.
		/// The more the difference is, the more negative x-Axis angular force the bike will receive.
		/// </summary>
		/// <remarks>
		/// The formula <c>(float)Math.Asin(Vehicle.ForwardVector.Z) - fStoppieBalancePoint</c> (where the deg to rad conversion has been already applied to <c>fStoppieBalancePoint</c>) determines whether to apply negative x-Axis angular force.
		/// If the result is less than -0.05, an additional negative x-Axis angular force will be applied. Unlike wheelieing, the normal vector of the wheel does not affect in physics for stopping at all.
		/// </remarks>
		/// <value>
		/// The value that affects how much x-axis rotation is needed before an additional negative x-Axis angular force applies to the bike when the bike is stoppieing in radians.
		/// </value>
		public float StoppieBalancePoint
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x38);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x38, value);
			}
		}

		/// <summary>
		/// Gets or sets the multiplier for steering when wheelieing. Should be negative.
		/// </summary>
		/// <value>
		/// The multiplier for steering when wheelieing.
		/// </value>
		public float WheelieSteerMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x3C);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x3C, value);
			}
		}

		/// <summary>
		/// Gets or sets An unknown multiplier for rear balance that have effects only when wheelieing. Should be positive.
		/// </summary>
		/// <value>
		/// An unknown multiplier for rear balance that have effects only when wheelieing.
		/// </value>
		public float RearBalanceMultiplier
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
		/// Gets or sets An unknown multiplier for front balance that have effects only when stopping. Should be positive.
		/// </summary>
		/// <value>
		/// An unknown multiplier for front balance that have effects only when stopping.
		/// </value>
		public float FrontBalanceMultiplier
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x44);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x44, value);
			}
		}

		/// <summary>
		/// Gets or sets the lean angle in radians indicating how much the bike should be lean on stand or in very low speed. Should be negative to match the standing animation.
		/// </summary>
		/// <value>
		/// The lean angle in radians indicating how much the bike should be lean on stand or in very low speed.
		/// </value>
		public float BikeOnStandLeanAngle
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x50);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x50, value);
			}
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same bike handling data as this <see cref="BikeHandlingData"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true"/> if the <paramref name="obj"/> is the same bike handling data as this <see cref="BikeHandlingData"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is BikeHandlingData data)
			{
				return MemoryAddress == data.MemoryAddress;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="BikeHandlingData"/>s refer to the same bike handling data.
		/// </summary>
		/// <param name="left">The left <see cref="BikeHandlingData"/>.</param>
		/// <param name="right">The right <see cref="BikeHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is the same bike handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(BikeHandlingData left, BikeHandlingData right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="BikeHandlingData"/>s don't refer to the same bike handling data.
		/// </summary>
		/// <param name="left">The left <see cref="BikeHandlingData"/>.</param>
		/// <param name="right">The right <see cref="BikeHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is not the same bike handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(BikeHandlingData left, BikeHandlingData right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return MemoryAddress.GetHashCode();
		}
	}
}
