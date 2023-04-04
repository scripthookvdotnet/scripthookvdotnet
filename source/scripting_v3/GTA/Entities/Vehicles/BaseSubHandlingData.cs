//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using System;

namespace GTA
{
	/// <summary>
	/// Represents the base sub handling data class for <c>CBaseSubHandlingData</c>.
	/// </summary>
	public abstract class BaseSubHandlingData
	{
		internal BaseSubHandlingData(IntPtr address, HandlingData parent)
		{
			MemoryAddress = address;
			Parent = parent;
		}

		/// <summary>
		/// Gets the memory address where the <see cref="BaseSubHandlingData"/> is stored in memory.
		/// </summary>
		public IntPtr MemoryAddress
		{
			get;
		}

		/// <summary>
		/// Gets the parent <see cref="BaseSubHandlingData"/>.
		/// </summary>
		public HandlingData Parent
		{
			get;
		}

		/// <summary>
		/// Returns true if this <see cref="BaseSubHandlingData"/> is valid.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="BaseSubHandlingData"/> is valid; otherwise, <see langword="false" />.
		/// </value>
		public bool IsValid => MemoryAddress != IntPtr.Zero && Parent != null;

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same sub handling data as this <see cref="BaseSubHandlingData"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true"/> if the <paramref name="obj"/> is the same entity as this <see cref="BaseSubHandlingData"/>; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is BaseSubHandlingData data)
			{
				return MemoryAddress == data.MemoryAddress && Parent == data.Parent;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="BaseSubHandlingData"/>s refer to the ub handling data.
		/// </summary>
		/// <param name="left">The left <see cref="BaseSubHandlingData"/>.</param>
		/// <param name="right">The right <see cref="BaseSubHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is the same ub handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(BaseSubHandlingData left, BaseSubHandlingData right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		/// <summary>
		/// Determines if two <see cref="BaseSubHandlingData"/>s don't refer to the same car handling data.
		/// </summary>
		/// <param name="left">The left <see cref="BaseSubHandlingData"/>.</param>
		/// <param name="right">The right <see cref="BaseSubHandlingData"/>.</param>
		/// <returns><see langword="true"/> if <paramref name="left"/> is not the same ub handling data as <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(BaseSubHandlingData left, BaseSubHandlingData right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return MemoryAddress.GetHashCode();
		}
	}
}
