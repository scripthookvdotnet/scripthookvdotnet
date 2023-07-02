//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public sealed class PathNode : INativeValue
	{
		internal PathNode(int nativeHandle)
		{
			Handle = nativeHandle;
		}

		/// <summary>
		/// Gets the area id.
		/// </summary>
		public int AreaId
		{
			get
			{
				uint areaIdAndNodeIdCorrected = (uint)Handle - 1;
				return (int)(areaIdAndNodeIdCorrected & 0xFFFF);
			}
		}

		/// <summary>
		/// Gets the node id.
		/// </summary>
		public int NodeId
		{
			get
			{
				uint areaIdAndNodeIdCorrected = (uint)Handle - 1;
				return ((ushort)areaIdAndNodeIdCorrected >> 0x10);
			}
		}

		/// <summary>
		/// Gets the handle for native funtions for vehicle path nodes.
		/// </summary>
		public int Handle
		{
			get; private set;
		}

		/// <summary>
		/// Gets the native representation of this <see cref="PathNode"/>.
		/// </summary>
		public ulong NativeValue
		{
			get => (ulong)Handle;
			set => Handle = unchecked((int)value);
		}

		/// <summary>
		/// Gets the memory address of this <see cref="PathNode"/>.
		/// </summary>
		public IntPtr MemoryAddress => SHVDN.NativeMemory.PathFind.GetPathNodeAddress(Handle);

		/// <summary>
		/// Determines if this <see cref="PathNode"/> is loaded.
		/// </summary>
		/// <returns><see langword="true"/> if this <see cref="PathNode"/> is loaded; otherwise, <see langword="false"/>.</returns>
		public bool IsLoaded => MemoryAddress != IntPtr.Zero;

		// All the property and methods in this class use our custom implementation because natives for node handles will crash the game if lower half bits are not zero and the CPathRegion of the specified area ID is not loaded
		/// <summary>
		/// Determines if this <see cref="PathNode"/> is loaded.
		/// </summary>
		/// <returns><see langword="true"/> if this <see cref="PathNode"/> is loaded; otherwise, <see langword="false"/>.</returns>
		public Vector3 Position => new Vector3(SHVDN.NativeMemory.PathFind.GetPathNodePosition(Handle));
		/// <summary>
		/// Determines if this <see cref="PathNode"/> is switched off for ambient population.
		/// </summary>
		/// <returns><see langword="true"/> if this <see cref="PathNode"/> is switched off for ambient population; otherwise, <see langword="false"/>.</returns>
		public bool IsSwitchedOff
		{
			get => SHVDN.NativeMemory.PathFind.GetPathNodeSwitchedOffFlag(Handle);
			set => SHVDN.NativeMemory.PathFind.SetPathNodeSwitchedOffFlag(Handle, value);
		}
		/// <summary>
		/// Determines if this <see cref="PathNode"/> has GPS allowed.
		/// </summary>
		/// <returns><see langword="true"/> if this <see cref="PathNode"/> has GPS allowed; otherwise, <see langword="false"/>.</returns>
		public bool IsGpsAllowed => (GetVehicleNodePropertyFlags() & VehiclePathNodePropertyFlags.DontAllowGps) != VehiclePathNodePropertyFlags.DontAllowGps;

		/// <summary>
		/// Gets the value indicating how busy the <see cref="PathNode"/> is.
		/// </summary>
		/// <returns>The value indicating how busy the <see cref="PathNode"/> is. if the node is loaded and is for vehicles; otherwise, zero.</returns>
		public int VehicleDensity => SHVDN.NativeMemory.PathFind.GetVehiclePathNodeDensity(Handle);

		/// <summary>
		/// Gets the property flags if this <see cref="PathNode"/> has.
		/// </summary>
		/// <returns>The property flags if this <see cref="PathNode"/> has if the node is loaded and is for vehicles; otherwise, <see langword="VehiclePathNodePropertyFlags.None"/>.</returns>
		public VehiclePathNodePropertyFlags GetVehicleNodePropertyFlags()
		{
			// After more properties and methods are implemented for ped path node to this class, this should return None if the path node is for peds
			return (VehiclePathNodePropertyFlags)SHVDN.NativeMemory.PathFind.GetVehiclePathNodePropertyFlags(Handle);
		}

		/// <summary>
		/// Gets all the <see cref="PathNodeLink"/> this <see cref="PathNode"/> links to.
		/// </summary>
		/// <returns>The <see cref="PathNodeLink"/> this <see cref="PathNode"/> links to if this <see cref="PathNode"/> is loaded; otherwise, the empty array.</returns>
		public PathNodeLink[] GetAllPathNodeLinks()
		{
			int[] nodeLinkIndices = SHVDN.NativeMemory.PathFind.GetPathNodeLinkIndicesOfPathNode(Handle);
			int nodeLinkCount = nodeLinkIndices.Length;
			if (nodeLinkCount == 0)
			{
				return Array.Empty<PathNodeLink>();
			}

			var result = new PathNodeLink[nodeLinkCount];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = new PathNodeLink(AreaId, nodeLinkIndices[i]);
			}

			return result;
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same path node as this <see cref="PathNode"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same path node as this <see cref="PathNode"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is PathNode pathNode)
			{
				return Handle == pathNode.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="PathNode"/>s refer to the same path node.
		/// </summary>
		/// <param name="left">The left <see cref="PathNode"/>.</param>
		/// <param name="right">The right <see cref="PathNode"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same path node as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(PathNode left, PathNode right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		/// <summary>
		/// Determines if two <see cref="PathNode"/>s don't refer to the same path node.
		/// </summary>
		/// <param name="left">The left <see cref="PathNode"/>.</param>
		/// <param name="right">The right <see cref="PathNode"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same path node as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(PathNode left, PathNode right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts a <see cref="PathNode"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(PathNode value)
		{
			return new InputArgument((ulong)(value?.Handle ?? 0));
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
