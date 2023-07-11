//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using System;

namespace GTA
{
	/// <summary>
	/// Represents the boat handling data class for <c>CBoatHandlingData</c>, which is for boats.
	/// </summary>
	public sealed class BoatHandlingData : BaseSubHandlingData
	{
		internal BoatHandlingData(IntPtr address, HandlingData parent) : base(address, parent, HandlingType.Boat)
		{
		}

		/// <summary>
		/// Gets or sets the multiplier value that indicates how much water wave the boat produce.
		/// Faster boat move will create more water wave.
		/// </summary>
		/// <remarks>
		/// This value multiplies a water push value that is typically between <c>0f</c> and <c>600f</c> to calculate how much water wave should be produced.
		/// <see cref="AquaplanePushWaterCap"/> will cap the multiplied value.
		/// </remarks>
		/// <value>
		/// The multiplier value that indicates how much water wave the boat produce.
		/// </value>
		public float AquaplanePushWaterMultiplier
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
		/// Gets or sets the multiplier value that indicates how much water wave the boat can produce.
		/// </summary>
		/// <value>
		/// The multiplier value that indicates how much water wave the boat can produce.
		/// </value>
		public float AquaplanePushWaterCap
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
		/// Gets or sets the multiplier value that indicates how much water the boat pushes by moving through the water.
		/// </summary>
		/// <value>
		/// The multiplier value that indicates how much water the boat pushes by moving through the water.
		/// </value>
		public float AquaplanePushWaterApply
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
		/// Gets or sets the rudder force factor. Moderately high value like <c>10f</c> will make the boat easier to steer.
		/// </summary>
		/// <value>
		/// The rudder force factor
		/// </value>
		public float RudderForce
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
		/// Gets or sets the vertical offset of propeller from bone when determining if submerged.
		/// </summary>
		/// <value>
		/// The vertical offset of propeller from bone when determining if submerged.
		/// </value>
		public float RudderOffsetSubmerge
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
		/// Gets or sets the vertical offset of propeller from bone when applying force.
		/// </summary>
		/// <value>
		/// The vertical offset of propeller from bone when applying force.
		/// </value>
		public float RudderOffsetForce
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
		/// Gets or sets the resistance value to rotation. the lower the value is, the harder to rotate.
		/// </summary>
		/// <remarks>
		/// Set a <see cref="Vector3"/> with some components zero will let the boat rotate without resistance in corresponding axes.
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

				return new Vector3(SHVDN.NativeMemory.ReadVector3(MemoryAddress + 0x60));
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteVector3(MemoryAddress + 0x60, value.ToInternalFVector3());
			}
		}
		/// <summary>
		/// Gets or sets the drag coefficient on the water.
		/// </summary>
		/// <value>
		/// The drag coefficient  on the water.
		/// </value>
		public float DragCoefficient
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
		/// <summary>
		/// Gets or sets the keel sphere size for physics.
		/// </summary>
		/// <value>
		/// The keel sphere size for physics.
		/// </value>
		public float KeelSphereSize
		{
			get
			{
				if (!IsValid)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(MemoryAddress + 0x78);
			}
			set
			{
				if (!IsValid)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(MemoryAddress + 0x78, value);
			}
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same boat handling data as this <see cref="BoatHandlingData"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true"/> if the <paramref name="obj"/> is the same boat handling data as this <see cref="BoatHandlingData"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is BoatHandlingData data)
			{
				return MemoryAddress == data.MemoryAddress;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="BoatHandlingData"/>s refer to the same boat handling data.
		/// </summary>
		/// <param name="left">The left <see cref="BoatHandlingData"/>.</param>
		/// <param name="right">The right <see cref="BoatHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is the same boat handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(BoatHandlingData left, BoatHandlingData right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="BoatHandlingData"/>s don't refer to the boat handling data.
		/// </summary>
		/// <param name="left">The left <see cref="BoatHandlingData"/>.</param>
		/// <param name="right">The right <see cref="BoatHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is not the same boat handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(BoatHandlingData left, BoatHandlingData right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return MemoryAddress.GetHashCode();
		}
	}
}
