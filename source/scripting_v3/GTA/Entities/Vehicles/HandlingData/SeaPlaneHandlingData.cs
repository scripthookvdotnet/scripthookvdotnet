//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Represents the sea plane handling data class for <c>CSeaPlaneHandlingData</c>, which is for sea planes.
	/// </summary>
	public sealed class SeaPlaneHandlingData : BaseSubHandlingData
	{
		internal SeaPlaneHandlingData(IntPtr address, HandlingData parent) : base(address, parent, HandlingType.SeaPlane)
		{
		}

		/// <summary>
		/// Gets or sets the coefficient that adds drag to the pontoon of the sea plane when travelling on water in a similar way to the vehicle's initial drag.
		/// </summary>
		/// <value>
		/// The coefficient that adds drag to the pontoon of the sea plane when travelling on water in a similar way to the vehicle's initial drag.
		/// </value>
		public float PontoonDragCoefficient
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
		/// Gets or sets the damping coefficient against positive z-direction.
		/// Higher value will float more stronger when travelling on water.
		/// </summary>
		/// <value>
		/// The damping coefficient against positive z-direction.
		/// </value>
		public float PontoonVerticalDampingCoefficientUp
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
		/// Gets or sets the damping coefficient against negative z-direction.
		/// Higher value will sink more stronger when travelling on water.
		/// </summary>
		/// <value>
		/// The damping coefficient against negative z-direction.
		/// </value>
		public float PontoonVerticalDampingCoefficientDown
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
		/// Determines if an <see cref="object"/> refers to the same sea plane handling data as this <see cref="SeaPlaneHandlingData"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true"/> if the <paramref name="obj"/> is the same sea plane handling data as this <see cref="SeaPlaneHandlingData"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is SeaPlaneHandlingData data)
			{
				return MemoryAddress == data.MemoryAddress;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="SeaPlaneHandlingData"/>s refer to the same sea plane handling data.
		/// </summary>
		/// <param name="left">The left <see cref="SeaPlaneHandlingData"/>.</param>
		/// <param name="right">The right <see cref="SeaPlaneHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is the same sea plane handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(SeaPlaneHandlingData left, SeaPlaneHandlingData right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="SeaPlaneHandlingData"/>s don't refer to the sea plane handling data.
		/// </summary>
		/// <param name="left">The left <see cref="SeaPlaneHandlingData"/>.</param>
		/// <param name="right">The right <see cref="SeaPlaneHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is not the same sea plane handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(SeaPlaneHandlingData left, SeaPlaneHandlingData right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return MemoryAddress.GetHashCode();
		}
	}
}
