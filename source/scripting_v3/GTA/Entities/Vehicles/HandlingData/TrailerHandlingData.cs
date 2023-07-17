//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Represents the trailer handling data class for <c>CTrailerHandlingData</c>, which is for trailers.
	/// </summary>
	public sealed class TrailerHandlingData : BaseSubHandlingData
	{
		internal TrailerHandlingData(IntPtr address, HandlingData parent) : base(address, parent, HandlingType.Trailer)
		{
		}

		/// <summary>
		/// Gets or sets the attach limit angle in degrees that indicates how much the trailer is allowed to lean in the yaw axis (local x-axis).
		/// </summary>
		/// <remarks>
		/// The value will be read only when this trailer starts to get towed.
		/// </remarks>
		/// <value>
		/// The attach limit angle in degrees that indicates how much the trailer is allowed to lean in the yaw axis (local x-axis).
		/// </value>
		public float AttachLimitPitch
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
		/// Gets or sets the attach limit angle in degrees that indicates how much the trailer is allowed to lean in the roll axis (local y-axis).
		/// </summary>
		/// <remarks>
		/// The value will be read only when this trailer starts to get towed.
		/// </remarks>
		/// <value>
		/// The attach limit angle that in degrees indicates how much the trailer is allowed to lean in the roll axis (local y-axis).
		/// </value>
		public float AttachLimitRoll
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
		/// Gets or sets the attach limit angle that in degrees indicates how much the trailer is allowed to lean in the yaw axis (local z-axis).
		/// </summary>
		/// <remarks>
		/// The value will be read only when this trailer starts to get towed.
		/// </remarks>
		/// <value>
		/// The attach limit angle that in degrees indicates how much the trailer is allowed to lean in the yaw axis (local z-axis).
		/// </value>
		public float AttachLimitYaw
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
		/// Gets or sets the upright spring constant that have effects when the trailer is rotated too much in the pitch axis.
		/// Lower value will make higher spring force. The value should be negative.
		/// </summary>
		/// <value>
		/// The upright spring constant that have effects when the trailer is rotated too much in the pitch axis.
		/// </value>
		public float UprightSpringConstant
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
		/// Gets or sets the upright damping constant.
		/// </summary>
		/// <value>
		/// The upright damping constant.
		/// </value>
		public float UprightDampingConstant
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
		/// Gets or sets the attachment distance constraint max distance.
		/// Any value less than or equal to zero will default to the old spherical constraint.
		/// </summary>
		/// <value>
		/// The attachment distance constraint max distance.
		/// </value>
		public float AttachedMaxDistance
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
		/// Gets or sets the attachment distance constraint max penetration.
		/// Any value less than or equal to zero will default to the old spherical constraint.
		/// </summary>
		/// <value>
		/// The attachment distance constraint max penetration.
		/// </value>
		public float AttachedMaxPenetration
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x20);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x20, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates the trailer should appear either heavier or lighter
		/// relative to the towing vehicle without actually changing its real mass.
		/// If less than <c>1f</c>, the trailer will appear heavier.
		/// </summary>
		/// <value>
		/// The value that indicates the trailer should appear either heavier or lighter
		/// relative to the towing vehicle without actually changing its real mass.
		/// </value>
		public float PositionConstraintMassRatio
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
		/// Determines if an <see cref="object"/> refers to the same trailer handling data as this <see cref="TrailerHandlingData"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true"/> if the <paramref name="obj"/> is the same trailer handling data as this <see cref="TrailerHandlingData"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is TrailerHandlingData data)
			{
				return MemoryAddress == data.MemoryAddress;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="TrailerHandlingData"/>s refer to the same trailer handling data.
		/// </summary>
		/// <param name="left">The left <see cref="TrailerHandlingData"/>.</param>
		/// <param name="right">The right <see cref="TrailerHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is the same trailer handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(TrailerHandlingData left, TrailerHandlingData right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="TrailerHandlingData"/>s don't refer to the trailer handling data.
		/// </summary>
		/// <param name="left">The left <see cref="TrailerHandlingData"/>.</param>
		/// <param name="right">The right <see cref="TrailerHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is not the same trailer handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(TrailerHandlingData left, TrailerHandlingData right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return MemoryAddress.GetHashCode();
		}
	}
}
