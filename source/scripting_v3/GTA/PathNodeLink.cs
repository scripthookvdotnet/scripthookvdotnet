//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	public sealed class PathNodeLink
	{
		internal PathNodeLink(int areaId, int indexOfNodeLinkArray)
		{
			AreaId = areaId;
			Index = indexOfNodeLinkArray;
		}

		/// <summary>
		/// Gets the area id this <see cref="PathNodeLink"/> belongs to.
		/// </summary>
		public int AreaId
		{
			get;
		}

		/// <summary>
		/// Gets the index for the node link array of the region this <see cref="PathNodeLink"/> belongs to.
		/// </summary>
		public int Index
		{
			get;
		}

		/// <summary>
		/// Gets target area id for the target <see cref="PathNode"/>.
		/// </summary>
		public int TargetAreaId
		{
			get
			{
				GetAreaIdAndNodeIdToTargetNode(out int targetAreaId, out _);
				return targetAreaId;
			}
		}

		/// <summary>
		/// Gets target node id for the target <see cref="PathNode"/>.
		/// </summary>
		public int TargetNodeId
		{
			get
			{
				GetAreaIdAndNodeIdToTargetNode(out _, out int targetNodeId);
				return targetNodeId;
			}
		}

		private bool GetAreaIdAndNodeIdToTargetNode(out int targetAreaId, out int targetNodeId) => SHVDN.NativeMemory.PathFind.GetTargetAreaAndNodeIdToTargetNode(AreaId, Index, out targetAreaId, out targetNodeId);

		/// <summary>
		/// Gets to the <see cref="PathNode"/> this <see cref="PathNodeLink"/> is targeted at.
		/// </summary>
		/// <returns>The <see cref="PathNode"/> this <see cref="PathNodeLink"/> is targeted at if both this <see cref="PathNodeLink"/> and the target <see cref="PathNode"/> are loaded; otherwise, <see langword="null"/>.</returns>

		public PathNode TargetPathNode
		{
			get
			{
				int targetNodeHandle = SHVDN.NativeMemory.PathFind.GetTargetNodeHandleFromNodeLink(AreaId, Index);
				if (targetNodeHandle == 0)
				{
					return null;
				}

				return new PathNode(targetNodeHandle);
			}
		}

		/// <summary>
		/// Gets the number of forward and backward lanes this <see cref="PathNodeLink"/> has.
		/// </summary>
		/// <param name="forwardLaneCount">The number of forward lanes if the <see cref="PathNodeLink"/> is loaded. If not loaded, zero will be returned.</param>
		/// <param name="backwardLaneCount">The number of backward lanes if the <see cref="PathNodeLink"/> is loaded. If not loaded, zero will be returned.</param>
		/// <returns><see langword="true"/> if this <see cref="PathNodeLink"/> is loaded; otherwise, <see langword="false"/>.</returns>
		public bool GetForwardAndBackwardLaneCounts(out int forwardLaneCount, out int backwardLaneCount) => SHVDN.NativeMemory.PathFind.GetPathNodeLinkLanes(AreaId, Index, out forwardLaneCount, out backwardLaneCount);

		/// <summary>
		/// Gets the number of forward and backward lanes this <see cref="PathNodeLink"/> has.
		/// </summary>
		/// <returns>The numbers of forward and backward lanes this <see cref="PathNodeLink"/> has if loaded; otherwise, the tuples with both values filled fith zero.</returns>
		public (int forwardLaneCount, int backwardLaneCount) GetForwardAndBackwardLaneCounts()
		{
			GetForwardAndBackwardLaneCounts(out int forwardLaneCount, out int backwardLaneCount);
			return (forwardLaneCount, backwardLaneCount);
		}

		/// <summary>
		/// Gets the memory address of this <see cref="PathNodeLink"/>.
		/// </summary>
		public IntPtr MemoryAddress => SHVDN.NativeMemory.PathFind.GetPathNodeLinkAddress(AreaId, Index);

		/// <summary>
		/// Determines if this <see cref="PathNodeLink"/> is loaded.
		/// </summary>
		/// <returns><see langword="true"/> if this <see cref="PathNodeLink"/> is loaded; otherwise, <see langword="false"/>.</returns>
		public bool IsLoaded => MemoryAddress != IntPtr.Zero;

		/// <summary>
		/// Gets the number of forward lanes this <see cref="PathNodeLink"/> has.
		/// </summary>
		/// <returns>The number of forward lanes if this <see cref="PathNodeLink"/> is loaded; otherwise, zero.</returns>
		public int ForwardLaneCount
		{
			get
			{
				GetForwardAndBackwardLaneCounts(out int forwardLaneCount, out _);
				return forwardLaneCount;
			}
		}
		/// <summary>
		/// Gets the number of backward lanes this <see cref="PathNodeLink"/> has.
		/// </summary>
		/// <returns>The number of backward lanes if this <see cref="PathNodeLink"/> is loaded; otherwise, zero.</returns>
		public int BackwardLaneCount
		{
			get
			{
				GetForwardAndBackwardLaneCounts(out _, out int backwardLaneCount);
				return backwardLaneCount;
			}
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same path node link as this <see cref="PathNodeLink"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same path node link as this <see cref="PathNodeLink"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is PathNodeLink pathNodeLink)
			{
				return Index == pathNodeLink.Index && AreaId == pathNodeLink.AreaId;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="PathNodeLink"/>s refer to the same path node link.
		/// </summary>
		/// <param name="left">The left <see cref="PathNodeLink"/>.</param>
		/// <param name="right">The right <see cref="PathNodeLink"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same path node link as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(PathNodeLink left, PathNodeLink right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		/// <summary>
		/// Determines if two <see cref="PathNodeLink"/>s don't refer to the same path node link.
		/// </summary>
		/// <param name="left">The left <see cref="PathNodeLink"/>.</param>
		/// <param name="right">The right <see cref="PathNodeLink"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same path node link as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(PathNodeLink left, PathNodeLink right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 5039 + AreaId.GetHashCode();
			hash = hash * 883 + Index.GetHashCode();
			return hash;
		}
	}
}
