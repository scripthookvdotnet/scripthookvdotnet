//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a interior proxy, which is for <c>CAnimatedBuilding</c>.
	/// </summary>
	public sealed class AnimatedBuilding : IExistable
	{
		internal AnimatedBuilding(int handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// Creates a new instance of an <see cref="AnimatedBuilding"/> from the given handle.
		/// </summary>
		/// <param name="handle">The building handle.</param>
		/// <returns>
		/// Returns a <see cref="AnimatedBuilding"/> if this handle corresponds to a <see cref="AnimatedBuilding"/>.
		/// Returns <see langword="null" /> if no <see cref="AnimatedBuilding"/> exists this the specified <paramref name="handle"/>
		/// </returns>
		public static AnimatedBuilding FromHandle(int handle) => SHVDN.NativeMemory.AnimatedBuildingHandleExists(handle) ? new AnimatedBuilding(handle) : null;

		/// <summary>
		/// The handle of this <see cref="AnimatedBuilding"/>. This property is provided mainly for safer instance handling, but this is also used for equality comparison.
		/// </summary>
		public int Handle
		{
			get; private set;
		}

		/// <summary>
		/// Gets the memory address where the <see cref="AnimatedBuilding"/> is stored in memory.
		/// </summary>
		public IntPtr MemoryAddress => SHVDN.NativeMemory.GetAnimatedBuildingAddress(Handle);

		/// <summary>
		/// Gets the model of this <see cref="AnimatedBuilding"/>.
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
		/// Gets this <see cref="AnimatedBuilding"/>s matrix which stores position and rotation information.
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
		/// Gets the rotation of this <see cref="AnimatedBuilding"/>.
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
		/// Gets the quaternion of this <see cref="AnimatedBuilding"/>.
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
		/// Gets or sets the position of this <see cref="AnimatedBuilding"/>.
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
			return SHVDN.NativeMemory.AnimatedBuildingHandleExists(Handle);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same entity as this <see cref="AnimatedBuilding"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same entity as this <see cref="AnimatedBuilding"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is AnimatedBuilding entity)
			{
				return Handle == entity.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="AnimatedBuilding"/>s refer to the same entity.
		/// </summary>
		/// <param name="left">The left <see cref="AnimatedBuilding"/>.</param>
		/// <param name="right">The right <see cref="AnimatedBuilding"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same entity as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(AnimatedBuilding left, AnimatedBuilding right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="AnimatedBuilding"/>s don't refer to the same entity.
		/// </summary>
		/// <param name="left">The left <see cref="AnimatedBuilding"/>.</param>
		/// <param name="right">The right <see cref="AnimatedBuilding"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same entity as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(AnimatedBuilding left, AnimatedBuilding right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
