//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a shape test handle.
	/// You need to call <see cref="GetResult(out ShapeTestResult)"/> or <see cref="GetResultIncludingMaterial(out ShapeTestResult, out MaterialHash)"/>
	/// every frame until one of the methods returns <see cref="ShapeTestStatus.Ready"/>.
	/// </summary>
	public struct ShapeTestHandle : IEquatable<ShapeTestHandle>, INativeValue
	{
		internal ShapeTestHandle(int handle) : this()
		{
			Handle = handle;
		}

		/// <summary>
		/// Gets the shape test handle.
		/// </summary>
		public int Handle
		{
			get; private set;
		}

		/// <summary>
		/// Gets the native representation of this <see cref="Model"/>.
		/// </summary>
		public ulong NativeValue
		{
			get => (ulong)Handle;
			set => Handle = unchecked((int)value);
		}

		/// <summary>
		/// Gets if the request of <see cref="ShapeTestHandle"/> is failed.
		/// There is a limit to the number that can be in the system. Therefore, native functions for shape tests may fail to create the shapetest requests.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if the request is failed; otherwise, <see langword="false" />.
		/// </value>
		public bool IsRequestFailed => Handle == 0;

		/// <summary>
		/// If status returned is <see cref="ShapeTestStatus.Ready"/>, then returns whether something was hit, and if so nearest hit position and normal.
		/// You need to call this method until the result is ready since the shape test result may not be finished in the same frame you start the shape test.
		/// </summary>
		/// <remarks>
		/// The shape test request is destroyed by this call if <see cref="ShapeTestStatus.Ready"/> is returned.
		/// If this is not called every frame then the request will be destroyed.
		/// </remarks>
		public ShapeTestStatus GetResult(out ShapeTestResult result)
		{
			NativeVector3 hitPositionArg;
			bool hitSomethingArg;
			int guidHandleArg;
			NativeVector3 surfaceNormalArg;
			ShapeTestStatus shapeTestStatus;
			unsafe
			{
				shapeTestStatus = Function.Call<ShapeTestStatus>(Hash.GET_SHAPE_TEST_RESULT, Handle, &hitSomethingArg, &hitPositionArg, &surfaceNormalArg, &guidHandleArg);
			}
			result = new ShapeTestResult(hitSomethingArg, hitPositionArg, surfaceNormalArg, guidHandleArg);

			return shapeTestStatus;
		}

		/// <summary>
		/// If status returned is <see cref="ShapeTestStatus.Ready"/>, then returns whether something was hit, and if so nearest hit position and normal.
		/// You need to call this method until the result is ready since the shape test result may not be finished in the same frame you start the shape test.
		/// </summary>
		/// <remarks>
		/// The shape test request is destroyed by this call if <see cref="ShapeTestStatus.Ready"/> is returned.
		/// If this is not called every frame then the request will be destroyed.
		/// </remarks>
		public (ShapeTestStatus status, ShapeTestResult result) GetResult()
		{
			ShapeTestStatus status = GetResult(out ShapeTestResult result);
			return (status, result);
		}

		/// <summary>
		/// If status returned is <see cref="ShapeTestStatus.Ready"/>, then returns whether something was hit, and if so nearest hit position, normal, and a hash of the material name.
		/// You need to call this method until the result is ready since the shape test result may not be finished in the same frame you start the shape test.
		/// </summary>
		/// <remarks>
		/// The shape test request is destroyed by this call if <see cref="ShapeTestStatus.Ready"/> is returned.
		/// If this is not called every frame then the request will be destroyed.
		/// </remarks>
		public ShapeTestStatus GetResultIncludingMaterial(out ShapeTestResult result, out MaterialHash materialHash)
		{
			NativeVector3 hitPositionArg;
			bool hitSomethingArg;
			int materialHashArg;
			int guidHandleArg;
			NativeVector3 surfaceNormalArg;
			ShapeTestStatus shapeTestStatus;

			unsafe
			{
				shapeTestStatus = Function.Call<ShapeTestStatus>(Hash.GET_SHAPE_TEST_RESULT_INCLUDING_MATERIAL, Handle, &hitSomethingArg, &hitPositionArg, &surfaceNormalArg, &materialHashArg, &guidHandleArg);
			}
			result = new ShapeTestResult(hitSomethingArg, hitPositionArg, surfaceNormalArg, guidHandleArg);
			materialHash = (MaterialHash)materialHashArg;

			return shapeTestStatus;
		}

		/// <summary>
		/// If status returned is <see cref="ShapeTestStatus.Ready"/>, then returns whether something was hit, and if so nearest hit position, normal, and a hash of the material name.
		/// You need to call this method until the result is ready since the shape test result may not be finished in the same frame you start the shape test.
		/// </summary>
		/// <remarks>
		/// The shape test request is destroyed by this call if <see cref="ShapeTestStatus.Ready"/> is returned.
		/// If this is not called every frame then the request will be destroyed.
		/// </remarks>
		public (ShapeTestStatus status, ShapeTestResult result, MaterialHash materialHash) GetResultIncludingMaterial()
		{
			ShapeTestStatus status = GetResultIncludingMaterial(out ShapeTestResult result, out MaterialHash materialHash);
			return (status, result, materialHash);
		}

		public bool Equals(ShapeTestHandle model)
		{
			return Handle == model.Handle;
		}
		public override bool Equals(object obj)
		{
			if (obj is ShapeTestHandle model)
			{
				return Equals(model);
			}

			return false;
		}

		public static bool operator ==(ShapeTestHandle left, ShapeTestHandle right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(ShapeTestHandle left, ShapeTestHandle right)
		{
			return !left.Equals(right);
		}

		public static implicit operator InputArgument(ShapeTestHandle value)
		{
			return new InputArgument((ulong)value.Handle);
		}

		public override int GetHashCode()
		{
			return Handle;
		}
	}
}
