//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Represents the submarine handling data class for <c>CSubmarineHandlingData</c>, which is for submarines.
	/// </summary>
	public sealed class SubmarineHandlingData : BaseSubHandlingData
	{
		internal SubmarineHandlingData(IntPtr address, HandlingData parent) : base(address, parent, HandlingType.Submarine)
		{
		}

		/// <summary>
		/// Gets or sets the multiplier value that indicates how fast the submarine rotates in the pitch axis (local x-axis).
		/// </summary>
		/// <value>
		/// The multiplier value that indicates how fast the submarine rotates in the pitch axis (local x-axis).
		/// </value>
		public float PitchMultiplier
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
		/// Gets or sets the maximum angle in degrees that indicates how much the submarine can rotate in the pitch axis (local x-axis).
		/// </summary>
		/// <value>
		/// The multiplier value that indicates how much the submarine can rotate in the pitch axis (local x-axis).
		/// </value>
		public float PitchAngle
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
		/// Gets or sets the multiplier value that indicates how much the submarine can rotate in the yaw axis (local z-axis).
		/// </summary>
		/// <value>
		/// The multiplier value that indicates how much the submarine can rotate in the yaw axis (local z-axis).
		/// </value>
		public float YawMultiplier
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
		/// Gets or sets a value that at least have an effect on buoyancy when the submarine has no driver.
		/// </summary>
		/// <value>
		/// A value that at least have an effect on buoyancy when the submarine has no driver.
		/// </value>
		public float DiveSpeed
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
		/// Determines if an <see cref="object"/> refers to the same submarine handling data as this <see cref="SubmarineHandlingData"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true"/> if the <paramref name="obj"/> is the same submarine handling data as this <see cref="SubmarineHandlingData"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is SubmarineHandlingData data)
			{
				return MemoryAddress == data.MemoryAddress;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="SubmarineHandlingData"/>s refer to the same submarine handling data.
		/// </summary>
		/// <param name="left">The left <see cref="SubmarineHandlingData"/>.</param>
		/// <param name="right">The right <see cref="SubmarineHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is the same submarine handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(SubmarineHandlingData left, SubmarineHandlingData right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="SubmarineHandlingData"/>s don't refer to the submarine handling data.
		/// </summary>
		/// <param name="left">The left <see cref="SubmarineHandlingData"/>.</param>
		/// <param name="right">The right <see cref="SubmarineHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is not the same submarine handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(SubmarineHandlingData left, SubmarineHandlingData right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return MemoryAddress.GetHashCode();
		}
	}
}
