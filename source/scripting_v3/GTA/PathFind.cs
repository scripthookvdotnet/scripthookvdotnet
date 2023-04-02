//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public static class PathFind
	{
		/// <summary>
		/// Gets an <c>array</c> of all the <see cref="PathNode"/>s that meet <paramref name="predicate"/>.
		/// Without <paramref name="predicate"/> set to filter vehicle nodes, the array contains can be more than 20000 vehicle nodes and manually filtering the array may cost significant time.
		/// Therefore, you should set pass some predicate as <paramref name="predicate"/> to filter vehicle nodes unless you need to retrieve all loaded vehicle parameters without testing.
		/// </summary>
		/// <param name="predicate">The predicate the node must meet to consider.</param>
		public static PathNode[] GetAllVehiclePathNodes(Func<VehiclePathNodePropertyFlags, bool> predicate = null)
		{
			Func<int, bool> convertedPredicate = predicate != null ? predInt => predicate((VehiclePathNodePropertyFlags)predInt) : null;
			return Array.ConvertAll(SHVDN.NativeMemory.PathFind.GetAllLoadedVehicleNodes(convertedPredicate), handle => new PathNode(handle));
		}

		/// <summary>
		/// Gets the position where the closest vehicle node is located among the ones that meet <paramref name="predicate"/>.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Ped"/> against.</param>
		/// <param name="radius">The maximun distance from the <paramref name="position"/> to detect <see cref="PathNode"/>s.</param>
		/// <param name="predicate">The predicate the node must meet to consider.</param>
		public static PathNode[] GetNearbyVehiclePathNodes(Vector3 position, float radius, Func<VehiclePathNodePropertyFlags, bool> predicate = null)
		{
			Func<int, bool> convertedPredicate = predicate != null ? predInt => predicate((VehiclePathNodePropertyFlags)predInt) : null;
			return Array.ConvertAll(SHVDN.NativeMemory.PathFind.GetLoadedVehicleNodesInRange(position.X, position.Y, position.Z, radius, convertedPredicate), handle => new PathNode(handle));
		}

		/// <summary>
		/// Gets the position where the closest vehicle node is located among the ones that meet <paramref name="predicate"/>.
		/// </summary>
		/// <param name="min">The minimum bound of the area.</param>
		/// <param name="max">The maximum bound of the area.</param>
		/// <param name="predicate">The predicate the node must meet to consider.</param>
		public static PathNode[] GetVehiclePathNodesInArea(Vector3 min, Vector3 max, Func<VehiclePathNodePropertyFlags, bool> predicate = null)
		{
			Func<int, bool> convertedPredicate = predicate != null ? predInt => predicate((VehiclePathNodePropertyFlags)predInt) : null;
			return Array.ConvertAll(SHVDN.NativeMemory.PathFind.GetLoadedVehicleNodesInArea(min.X, min.Y, min.Z, max.X, max.Y, max.Z, convertedPredicate), handle => new PathNode(handle));
		}

		/// <summary>
		/// Gets the closest vehicle node is located among the ones that meet <paramref name="predicate"/>.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Ped"/> against.</param>
		/// <param name="radius">The maximun distance from the <paramref name="position"/> to detect <see cref="PathNode"/>s.</param>
		/// <param name="predicate">The predicate the node must meet to consider.</param>
		public static PathNode GetClosestVehiclePathNode(Vector3 position, float radius, Func<VehiclePathNodePropertyFlags, bool> predicate = null)
		{
			Func<int, bool> convertedPredicate = predicate != null ? predInt => predicate((VehiclePathNodePropertyFlags)predInt) : null;
			var resultHandle = SHVDN.NativeMemory.PathFind.GetClosestLoadedVehiclePathNode(position.X, position.Y, position.Z, radius, convertedPredicate);
			return resultHandle != 0 ? new PathNode(resultHandle) : null;
		}


		/// <summary>
		/// Gets the position where the closest vehicle node is located.
		/// </summary>
		/// <param name="position">The position to check the <see cref="PathNode"/>s against.</param>
		/// <param name="closestNodePosition">The position where the closest node is.</param>
		/// <param name="flags">The flags to consider for the search.</param>
		/// <param name="zMeasureMult">The factor how strongly should the difference in Z direction be weighted if the Z coords is more than <paramref name="zTolerance"/>.</param>
		/// <param name="zTolerance">
		/// The minimum difference how far apart to the Z coords have to be before the Z coords difference is considered.
		/// If the Z coords difference is the same as this value or less, the Z coords difference will be considered as zero.
		/// </param>
		/// <returns>
		///   <see langword="true"/> if the closest <see cref="PathNode"/> is found; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool GetClosestVehicleNodePosition(Vector3 position, out Vector3 closestNodePosition, GetClosestVehicleNodeFlags flags = GetClosestVehicleNodeFlags.None, float zMeasureMult = 3f, float zTolerance = 0f)
		{
			unsafe
			{
				NativeVector3 outPos;
				var foundNode = Function.Call<bool>(Hash.GET_CLOSEST_VEHICLE_NODE, position.X, position.Y, position.Z, &outPos, flags, zMeasureMult, zTolerance);

				closestNodePosition = outPos;
				return foundNode;
			}
		}

		/// <summary>
		/// Gets the <see cref="PathNode"/> where the closest vehicle node is located.
		/// </summary>
		/// <param name="position">The position to check the <see cref="PathNode"/>s against.</param>
		/// <param name="nthClosest">
		/// The numeric position the in a series of closest nodes.
		/// If this is set to 1 then the closest node will be returned. If this is set to 2 then the second closest node will be returned and so on.
		/// </param>
		/// <param name="flags">The flags to consider for the search.</param>
		/// <param name="zMeasureMult">The factor how strongly should the difference in Z direction be weighted if the Z coords is more than <paramref name="zTolerance"/>.</param>
		/// <param name="zTolerance">
		/// The minimum difference how far apart to the Z coords have to be before the Z coords difference is considered.
		/// If the Z coords difference is the same as this value or less, the Z coords difference will be considered as zero.
		/// </param>
		/// <returns>
		///   Returns <see langword="null"/> if nth closest <see cref="PathNode"/> is found; otherwise, <see langword="null"/>.
		/// </returns>
		public static PathNode GetNthClosestVehicleNode(Vector3 position, int nthClosest, GetClosestVehicleNodeFlags flags = GetClosestVehicleNodeFlags.None, float zMeasureMult = 3f, float zTolerance = 0f)
		{
			unsafe
			{
				int outNodeHandle = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID, position.X, position.Y, position.Z, nthClosest, flags, zMeasureMult, zTolerance);
				return outNodeHandle != 0 ? new PathNode(outNodeHandle) : null;
			}
		}

		/// <summary>
		/// Gets the position where the closest vehicle node is located.
		/// </summary>
		/// <param name="position">The position to check the <see cref="PathNode"/>s against.</param>
		/// <param name="nthClosest">
		/// The numeric position the in a series of closest nodes.
		/// If this is set to 1 then the closest node will be returned. If this is set to 2 then the second closest node will be returned and so on.
		/// </param>
		/// <param name="closestNodePosition">The position where the closest node is.</param>
		/// <param name="flags">The flags to consider for the search.</param>
		/// <param name="zMeasureMult">The factor how strongly should the difference in Z direction be weighted if the Z coords is more than <paramref name="zTolerance"/>.</param>
		/// <param name="zTolerance">
		/// The minimum difference how far apart to the Z coords have to be before the Z coords difference is considered.
		/// If the Z coords difference is the same as this value or less, the Z coords difference will be considered as zero.
		/// </param>
		/// <returns>
		///   <see langword="true"/> if the nth closest <see cref="PathNode"/> is found; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool GetNthClosestVehicleNodePosition(Vector3 position, int nthClosest, out Vector3 closestNodePosition, GetClosestVehicleNodeFlags flags = GetClosestVehicleNodeFlags.None, float zMeasureMult = 3f, float zTolerance = 0f)
		{
			unsafe
			{
				NativeVector3 outPos;
				var foundNode = Function.Call<bool>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE, position.X, position.Y, position.Z, nthClosest, &outPos, flags, zMeasureMult, zTolerance);

				closestNodePosition = outPos;
				return foundNode;
			}
		}

		/// <summary>
		/// Gets the position where the closest vehicle node is located.
		/// </summary>
		/// <param name="position">The position to check the <see cref="PathNode"/>s against.</param>
		/// <param name="nthClosest">
		/// The numeric position the in a series of closest nodes.
		/// If this is set to 1 then the closest node will be returned. If this is set to 2 then the second closest node will be returned and so on.
		/// </param>
		/// <param name="heading">
		/// The heading the first node link that has forward lanes is heading to among the array of node links of the found vehicle path node.
		/// If no node links of the path node have forward lanes, <c>0f</c> will be returned.
		/// </param>
		/// <param name="numLanes">
		/// The number of forward and backward lanes the first node link that has forward lanes has.
		/// If no node links of the path node have forward lanes, <c>1</c> will be returned.
		/// </param>
		/// <param name="flags">The flags to consider for the search.</param>
		/// <param name="zMeasureMult">The factor how strongly should the difference in Z direction be weighted if the Z coords is more than <paramref name="zTolerance"/>.</param>
		/// <param name="zTolerance">
		/// The minimum difference how far apart to the Z coords have to be before the Z coords difference is considered.
		/// If the Z coords difference is the same as this value or less, the Z coords difference will be considered as zero.
		/// </param>
		/// <returns>
		///   Returns <see langword="null"/> if nth closest <see cref="PathNode"/> is found; otherwise, <see langword="null"/>.
		/// </returns>
		public static PathNode GetNthClosestVehicleNodeWithHeading(Vector3 position, int nthClosest, out float heading, out int numLanes, GetClosestVehicleNodeFlags flags = GetClosestVehicleNodeFlags.None, float zMeasureMult = 3f, float zTolerance = 0f)
		{
			unsafe
			{
				float outHeading;
				int outNumLanes;
				int outNodeHandle = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID_WITH_HEADING, position.X, position.Y, position.Z, nthClosest, &outHeading, &outNumLanes, flags, zMeasureMult, zTolerance);

				heading = outHeading;
				numLanes = outNumLanes;
				return outNodeHandle != 0 ? new PathNode(outNodeHandle) : null;
			}
		}

		/// <summary>
		/// Gets the <see cref="PathNode"/> where the closest vehicle node is located.
		/// </summary>
		/// <param name="position">The position to check the <see cref="PathNode"/>s against.</param>
		/// <param name="nthClosest">
		/// The numeric position the in a series of closest nodes.
		/// If this is set to 1 then the closest node will be returned. If this is set to 2 then the second closest node will be returned and so on.
		/// </param>
		/// <param name="closestNodePosition">The position where the closest node is.</param>
		/// <param name="heading">
		/// The heading the first node link that has forward lanes is heading to among the array of node links of the found vehicle path node.
		/// If no node links of the path node have forward lanes, <c>0f</c> will be returned.
		/// </param>
		/// <param name="numLanes">
		/// The number of forward and backward lanes the first node link that has forward lanes has.
		/// If no node links of the path node have forward lanes, <c>1</c> will be returned.
		/// </param>
		/// <param name="flags">The flags to consider for the search.</param>
		/// <param name="zMeasureMult">The factor how strongly should the difference in Z direction be weighted if the Z coords is more than <paramref name="zTolerance"/>.</param>
		/// <param name="zTolerance">
		/// The minimum difference how far apart to the Z coords have to be before the Z coords difference is considered.
		/// If the Z coords difference is the same as this value or less, the Z coords difference will be considered as zero.
		/// </param>
		/// <returns>
		///   <see langword="true"/> if the nth closest <see cref="PathNode"/> is found; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool GetNthClosestVehicleNodePositionWithHeading(Vector3 position, int nthClosest, out Vector3 closestNodePosition, out float heading, out int numLanes, GetClosestVehicleNodeFlags flags = GetClosestVehicleNodeFlags.None, float zMeasureMult = 3f, float zTolerance = 0f)
		{
			unsafe
			{
				NativeVector3 outPos;
				float outHeading;
				int outNumLanes;
				var foundNode = Function.Call<bool>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING, position.X, position.Y, position.Z, nthClosest, &outPos, &outHeading, &outNumLanes, flags, zMeasureMult, zTolerance);

				closestNodePosition = outPos;
				heading = outHeading;
				numLanes = outNumLanes;
				return foundNode;
			}
		}

		/// <summary>
		/// Gets a value indicating whether <see cref="PathNode"/>s are loaded for the region specified.
		/// </summary>
		/// <param name="min">The minimum position of the region. The z value will be ignored.</param>
		/// <param name="max">The maximum position of the region. The z value will be ignored.</param>
		/// <returns>
		///   <see langword="true"/> if <see cref="PathNode"/>s are loaded for the region specified; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool AreNodesLoadedForArea(Vector3 min, Vector3 max) => Function.Call<bool>(Hash.ARE_NODES_LOADED_FOR_AREA, min.X, min.Y, max.X, max.Y);

		/// <summary>
		/// <para>
		/// Requests the path nodes in the given region to stream this frame.
		/// </para>
		/// <para>
		/// This does not guarantee that the nodes will be loaded this frame.
		/// Therefore, you should keep calling this method for as long as you wish nodes to be present in the given area.
		/// If you stop calling this method, the nodes may be streamed out again at any time.
		/// </para>
		/// </summary>
		/// <param name="min">The minimum position of the region. The z value will be ignored.</param>
		/// <param name="max">The maximum position of the region. The z value will be ignored.</param>
		/// <returns>
		///   <see langword="true"/> if <see cref="PathNode"/>s are loaded for the region specified; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool RequestPathNodesInAreaThisFrame(Vector3 min, Vector3 max) => Function.Call<bool>(Hash.REQUEST_PATH_NODES_IN_AREA_THIS_FRAME, min.X, min.Y, max.X, max.Y);
	}
}
