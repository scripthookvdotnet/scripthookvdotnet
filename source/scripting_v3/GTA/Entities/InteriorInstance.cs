//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a interior instance, which is for <c>CInteriorInst</c> (a subclass of <c>CBuilding</c>) and is not used for native functions for interiors.
	/// </summary>
	public sealed class InteriorInstance : IExistable
	{
		internal InteriorInstance(int handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// Creates a new instance of an <see cref="InteriorInstance"/> from the given handle.
		/// </summary>
		/// <param name="handle">The interior instance handle.</param>
		/// <returns>
		/// Returns a <see cref="InteriorInstance"/> if this handle corresponds to a <see cref="InteriorInstance"/>.
		/// Returns <see langword="null" /> if no <see cref="InteriorInstance"/> exists this the specified <paramref name="handle"/>
		/// </returns>
		public static InteriorInstance FromHandle(int handle) => SHVDN.NativeMemory.InteriorInstHandleExists(handle) ? new InteriorInstance(handle) : null;

		/// <summary>
		/// The handle of this <see cref="Building"/>. This property is provided mainly for safer instance handling, but this is also used for equality comparison.
		/// </summary>
		public int Handle
		{
			get; private set;
		}

		/// <summary>
		/// Gets the memory address where the <see cref="InteriorInstance"/> is stored in memory.
		/// </summary>
		public IntPtr MemoryAddress => SHVDN.NativeMemory.GetInteriorInstAddress(Handle);

		/// <summary>
		/// Gets the model of this <see cref="InteriorInstance"/>.
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
		/// Gets this <see cref="InteriorInstance"/>s matrix which stores position and rotation information.
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
		/// Gets the rotation of this <see cref="InteriorInstance"/>.
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
		/// Gets the quaternion of this <see cref="InteriorInstance"/>.
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
		/// Gets or sets the position of this <see cref="InteriorInstance"/>.
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
		/// Gets the <see cref="InteriorProxy"/> this <see cref="InteriorInstance"/> is loaded from.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if this <see cref="InteriorInstance"/> does not exist or SHVDN could not find the <see cref="InteriorProxy"/> pool for some reason.</remarks>
		public InteriorProxy InteriorProxy
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return null;
				}

				int interiorProxyHandle = SHVDN.NativeMemory.GetInteriorProxyHandleFromInteriorInst(Handle);
				return interiorProxyHandle != 0 ? new InteriorProxy(interiorProxyHandle) : null;
			}
		}

		/// <summary>
		/// Determines if this <see cref="InteriorInstance"/> exists.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="InteriorInstance"/> exists; otherwise, <see langword="false" />.</returns>
		public bool Exists()
		{
			return SHVDN.NativeMemory.InteriorInstHandleExists(Handle);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same entity as this <see cref="InteriorInstance"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same entity as this <see cref="InteriorInstance"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is InteriorInstance entity)
			{
				return Handle == entity.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="InteriorInstance"/>s refer to the same entity.
		/// </summary>
		/// <param name="left">The left <see cref="InteriorInstance"/>.</param>
		/// <param name="right">The right <see cref="InteriorInstance"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same entity as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(InteriorInstance left, InteriorInstance right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="InteriorInstance"/>s don't refer to the same entity.
		/// </summary>
		/// <param name="left">The left <see cref="InteriorInstance"/>.</param>
		/// <param name="right">The right <see cref="InteriorInstance"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same entity as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(InteriorInstance left, InteriorInstance right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
