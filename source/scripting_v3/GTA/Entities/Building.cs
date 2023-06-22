//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a static building, which is for <c>CBuilding</c>.
	/// </summary>
	public sealed class Building : IExistable
	{
		internal Building(int handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// Creates a new instance of an <see cref="Building"/> from the given handle.
		/// </summary>
		/// <param name="handle">The building handle.</param>
		/// <returns>
		/// Returns a <see cref="Building"/> if this handle corresponds to a <see cref="Building"/>.
		/// Returns <see langword="null" /> if no <see cref="Building"/> exists this the specified <paramref name="handle"/>
		/// </returns>
		public static Building FromHandle(int handle) => SHVDN.NativeMemory.BuildingHandleExists(handle) ? new Building(handle) : null;

		/// <summary>
		/// The handle of this <see cref="Building"/>. This property is provided mainly for safer instance handling, but this is also used for equality comparison.
		/// </summary>
		public int Handle
		{
			get; private set;
		}

		/// <summary>
		/// Gets the memory address where the <see cref="Building"/> is stored in memory.
		/// </summary>
		public IntPtr MemoryAddress => SHVDN.NativeMemory.GetBuildingAddress(Handle);

		/// <summary>
		/// Gets the model of this <see cref="Building"/>.
		/// </summary>
		public Model Model
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				return new Model(SHVDN.NativeMemory.GetModelHashFromEntity(address));
			}
		}

		/// <summary>
		/// Gets this <see cref="Building"/>s matrix which stores position and rotation information.
		/// </summary>
		public Matrix Matrix
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Matrix.Zero;
				}

				return new Matrix(SHVDN.NativeMemory.ReadMatrix(address + 0x60));
			}
		}

		/// <summary>
		/// Gets or sets the rotation of this <see cref="Building"/>.
		/// </summary>
		/// <value>
		/// The yaw, pitch, roll rotation values in degree.
		/// </value>
		public Vector3 Rotation
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				unsafe
				{
					float* tempRotationArray = stackalloc float[3];
					SHVDN.NativeMemory.GetRotationFromMatrix(tempRotationArray, address + 0x60);

					return new Vector3(tempRotationArray[0], tempRotationArray[1], tempRotationArray[2]);
				}
			}
		}

		/// <summary>
		/// Gets the quaternion of this <see cref="Building"/>.
		/// </summary>
		public Quaternion Quaternion
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Quaternion.Zero;
				}

				unsafe
				{
					float* tempRotationArray = stackalloc float[4];
					SHVDN.NativeMemory.GetQuaternionFromMatrix(tempRotationArray, address + 0x60);

					return new Quaternion(tempRotationArray[0], tempRotationArray[1], tempRotationArray[2], tempRotationArray[3]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the position of this <see cref="Building"/>.
		/// </summary>
		/// <value>
		/// The position in world space.
		/// </value>
		public Vector3 Position
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.Zero;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x90));
			}
		}

		/// <summary>
		/// Determines if this <see cref="Building"/> exists.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="Building"/> exists; otherwise, <see langword="false" />.</returns>
		public bool Exists()
		{
			return SHVDN.NativeMemory.BuildingHandleExists(Handle);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same entity as this <see cref="Building"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same entity as this <see cref="Building"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Building entity)
			{
				return Handle == entity.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="Building"/>s refer to the same entity.
		/// </summary>
		/// <param name="left">The left <see cref="Building"/>.</param>
		/// <param name="right">The right <see cref="Building"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same entity as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Building left, Building right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="Building"/>s don't refer to the same entity.
		/// </summary>
		/// <param name="left">The left <see cref="Building"/>.</param>
		/// <param name="right">The right <see cref="Building"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same entity as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(Building left, Building right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
