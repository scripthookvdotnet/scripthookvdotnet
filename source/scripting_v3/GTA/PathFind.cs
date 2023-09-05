//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public static class PathFind
	{
		/// <summary>
		/// Gets an array of all the vehicle <see cref="PathNode"/>s that meet <paramref name="predicate"/>.
		/// Without <paramref name="predicate"/> set to filter vehicle nodes, the array contains can be more than 20000 vehicle nodes and manually filtering the array may cost significant time.
		/// Therefore, you should set pass some predicate as <paramref name="predicate"/> to filter vehicle nodes unless you need to retrieve all loaded vehicle parameters without testing.
		/// </summary>
		/// <param name="predicate">The predicate the node must meet to consider.</param>
		public static PathNode[] GetAllVehicleNodes(Func<VehiclePathNodePropertyFlags, bool> predicate = null)
		{
			Func<int, bool> convertedPredicate = predicate != null ? predInt => predicate((VehiclePathNodePropertyFlags)predInt) : null;
			return Array.ConvertAll(SHVDN.NativeMemory.PathFind.GetAllLoadedVehicleNodes(convertedPredicate), handle => new PathNode(handle));
		}

		/// <summary>
		/// Gets an array of nearby vehicle <see cref="PathNode"/>s that meet <paramref name="predicate"/>.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Ped"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="PathNode"/>s.</param>
		/// <param name="predicate">The predicate the node must meet to consider.</param>
		public static PathNode[] GetNearbyVehicleNodes(Vector3 position, float radius, Func<VehiclePathNodePropertyFlags, bool> predicate = null)
		{
			Func<int, bool> convertedPredicate = predicate != null ? predInt => predicate((VehiclePathNodePropertyFlags)predInt) : null;
			return Array.ConvertAll(SHVDN.NativeMemory.PathFind.GetLoadedVehicleNodesInRange(position.X, position.Y, position.Z, radius, convertedPredicate), handle => new PathNode(handle));
		}

		/// <summary>
		/// Gets the vehicle <see cref="PathNode"/>s in the specified area that meet <paramref name="predicate"/>.
		/// </summary>
		/// <param name="min">The minimum bound of the area.</param>
		/// <param name="max">The maximum bound of the area.</param>
		/// <param name="predicate">The predicate the node must meet to consider.</param>
		public static PathNode[] GetVehicleNodesInArea(Vector3 min, Vector3 max, Func<VehiclePathNodePropertyFlags, bool> predicate = null)
		{
			Func<int, bool> convertedPredicate = predicate != null ? predInt => predicate((VehiclePathNodePropertyFlags)predInt) : null;
			return Array.ConvertAll(SHVDN.NativeMemory.PathFind.GetLoadedVehicleNodesInArea(min.X, min.Y, min.Z, max.X, max.Y, max.Z, convertedPredicate), handle => new PathNode(handle));
		}

		/// <summary>
		/// Gets the closest vehicle <see cref="PathNode"/> among the ones and that meet <paramref name="predicate"/>.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Ped"/> against.</param>
		/// <param name="radius">The maximun distance from the <paramref name="position"/> to detect <see cref="PathNode"/>s.</param>
		/// <param name="predicate">The predicate the node must meet to consider.</param>
		public static PathNode GetClosestVehicleNode(Vector3 position, float radius, Func<VehiclePathNodePropertyFlags, bool> predicate = null)
		{
			Func<int, bool> convertedPredicate = predicate != null ? predInt => predicate((VehiclePathNodePropertyFlags)predInt) : null;
			int resultHandle = SHVDN.NativeMemory.PathFind.GetClosestLoadedVehiclePathNode(position.X, position.Y, position.Z, radius, convertedPredicate);
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
				bool foundNode = Function.Call<bool>(Hash.GET_CLOSEST_VEHICLE_NODE,
					position.X,
					position.Y,
					position.Z,
					&outPos,
					(int)flags,
					zMeasureMult,
					zTolerance);

				closestNodePosition = outPos;
				return foundNode;
			}
		}

		/// <summary>
		/// Gets the Nth closest <see cref="PathNode"/>.
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
			int outNodeHandle = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID,
				position.X,
				position.Y,
				position.Z,
				nthClosest,
				(int)flags,
				zMeasureMult,
				zTolerance);
			return outNodeHandle != 0 ? new PathNode(outNodeHandle) : null;
		}

		/// <summary>
		/// Gets the position where the Nth closest vehicle <see cref="PathNode"/> is located.
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
				bool foundNode = Function.Call<bool>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE,
					position.X,
					position.Y,
					position.Z,
					nthClosest,
					&outPos,
					(int)flags,
					zMeasureMult,
					zTolerance);

				closestNodePosition = outPos;
				return foundNode;
			}
		}

		/// <summary>
		/// Gets the Nth closest <see cref="PathNode"/>.
		/// Also retrieves the heading and number of the lanes of the <see cref="PathNode"/>.
		/// </summary>
		/// <param name="position">The position to check the <see cref="PathNode"/>s against.</param>
		/// <param name="nthClosest">
		/// The numeric position the in a series of closest nodes.
		/// If this is set to 1 then the closest node will be returned. If this is set to 2 then the second closest node will be returned and so on.
		/// </param>
		/// <param name="heading">
		/// The heading of the first node link that has forward lanes among the array of node links of the found vehicle path node.
		/// If none of the node links have forward lanes, <c>0f</c> will be returned.
		/// </param>
		/// <param name="numLanes">
		/// The number of forward and backward lanes of the first node link that has forward lanes among the array of node links
		/// of the found vehicle path node.
		/// If none of the node links have forward lanes, <c>1</c> will be returned.
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
				int outNodeHandle = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID_WITH_HEADING,
					position.X,
					position.Y,
					position.Z,
					nthClosest,
					&outHeading,
					&outNumLanes,
					(int)flags,
					zMeasureMult,
					zTolerance);

				heading = outHeading;
				numLanes = outNumLanes;
				return outNodeHandle != 0 ? new PathNode(outNodeHandle) : null;
			}
		}

		/// <summary>
		/// Gets the position, heading, and number of lanes of the Nth closest vehicle <see cref="PathNode"/>.
		/// </summary>
		/// <param name="position">The position to check the <see cref="PathNode"/>s against.</param>
		/// <param name="nthClosest">
		/// The numeric position the in a series of closest nodes.
		/// If this is set to 1 then the closest node will be returned. If this is set to 2 then the second closest node will be returned and so on.
		/// </param>
		/// <param name="closestNodePosition">The position where the closest node is.</param>
		/// <param name="heading">
		/// The heading of the first node link that has forward lanes among the array of node links of the found vehicle path node.
		/// If none of the node links have forward lanes, <c>0f</c> will be returned.
		/// </param>
		/// <param name="numLanes">
		/// The number of forward and backward lanes of the first node link that has forward lanes among the array of node links
		/// of the found vehicle path node.
		/// If none of the node links have forward lanes, <c>1</c> will be returned.
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
				bool foundNode = Function.Call<bool>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING,
					position.X,
					position.Y,
					position.Z,
					nthClosest,
					&outPos,
					&outHeading,
					&outNumLanes,
					(int)flags,
					zMeasureMult,
					zTolerance);

				closestNodePosition = outPos;
				heading = outHeading;
				numLanes = outNumLanes;
				return foundNode;
			}
		}

		/// <summary>
		/// Gets a value indicating whether <see cref="PathNode"/>s are loaded for the region specified.
		/// </summary>
		/// <param name="min">The minimum position of the region.</param>
		/// <param name="max">The maximum position of the region.</param>
		/// <returns>
		///   <see langword="true"/> if <see cref="PathNode"/>s are loaded for the region specified; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool ArePathNodesLoadedForArea(Vector2 min, Vector2 max) => Function.Call<bool>(Hash.ARE_NODES_LOADED_FOR_AREA, min.X, min.Y, max.X, max.Y);
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
		/// <param name="min">The minimum position of the region.</param>
		/// <param name="max">The maximum position of the region.</param>
		/// <returns>
		///   <see langword="true"/> if <see cref="PathNode"/>s are loaded for the region specified; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool RequestPathNodesInAreaThisFrame(Vector2 min, Vector2 max) => Function.Call<bool>(Hash.REQUEST_PATH_NODES_IN_AREA_THIS_FRAME, min.X, min.Y, max.X, max.Y);

		/// <summary>
		/// Toggles ped paths in a cubic area. <see cref="Ped"/>s will walk in areas that are switched on and new
		/// <see cref="Ped"/>s will be generated on them. If a ped path is switched off, no <see cref="Ped"/>s will be
		/// created on it and <see cref="Ped"/>s that already exist will try to avoid walking through it.
		/// To revert to the original state, use <see cref="SetPedPathsBackToOriginal(Vector3, Vector3, bool)"/>.
		/// </summary>
		/// <param name="min">The minimum position of the region.</param>
		/// <param name="max">The maximum position of the region.</param>
		/// <param name="active">
		/// Specifies wheter ped nodes in area should be switched on or off. If <see langword="true"/>, they will be
		/// switched on.
		/// </param>
		/// <param name="forceAbortCurrentPath">
		/// If <see langword="true"/>, this method will avoid possible stalls by forcing any active pathfinding request
		/// to be aborted before switching ped navmeshes.
		/// Use this if there are reports of this method causing brief hangs waiting for navmesh data to be accessible,
		/// but be aware that if timing-critical pathfinding is occuring, that it can be interruped by this.
		/// </param>
		public static void SwitchPedPathsInArea(Vector3 min, Vector3 max, bool active, bool forceAbortCurrentPath) =>
			Function.Call(Hash.SET_PED_PATHS_IN_AREA, min.X, min.Y, min.Z, max.X, max.Y, max.Z, active, forceAbortCurrentPath);
		/// <summary>
		/// Sets all ped paths (navmeshes) in area back to their original state as per the map data and ynv files define.
		/// </summary>
		/// <param name="min">The minimum position of the region.</param>
		/// <param name="max">The maximum position of the region.</param>
		/// <param name="forceAbortCurrentPath">
		/// If <see langword="true"/>, this method will avoid possible stalls by forcing any active pathfinding request
		/// to be aborted before switching ped navmeshes.
		/// Use this if there are reports of this method causing brief hangs waiting for navmesh data to be accessible,
		/// but be aware that if timing-critical pathfinding is occuring, that it can be interruped by this.
		/// </param>
		public static void SetPedPathsBackToOriginal(Vector3 min, Vector3 max, bool forceAbortCurrentPath) =>
			Function.Call(Hash.SET_PED_PATHS_BACK_TO_ORIGINAL, min.X, min.Y, min.Z, max.X, max.Y, max.Z, forceAbortCurrentPath);

		/// <summary>
		/// Toggles vehicle nodes in angled area. <see cref="Vehicle"/>s will drive on to roads that are switched on and new <see cref="Vehicle"/>s will be generated on them.
		/// A vehicle node is switched off, no <see cref="Vehicle"/>s should be created on it and <see cref="Vehicle"/>s that already exist will try to avoid driving on to it.
		/// To undo effects of this method, use <see cref="SetVehicleNodesBackToOriginal(Vector3, Vector3)"/> or <see cref="SetVehicleNodesBackToOriginalInAngledArea(Vector3, Vector3, float)"/>.
		/// </summary>
		/// <param name="min">The minimum position of the region.</param>
		/// <param name="max">The maximum position of the region.</param>
		/// <param name="active">Specifies wheter vehicle nodes in area should be switched on or off. If <see langword="true"/>, they will be switched on.</param>
		public static void SwitchVehicleNodesInArea(Vector3 min, Vector3 max, bool active) => Function.Call(Hash.SET_ROADS_IN_AREA, min.X, min.Y, min.Z, max.X, max.Y, max.Z, active, false);
		/// <summary>
		/// <para>
		/// Toggles vehicle nodes in angled area. <see cref="Vehicle"/>s will drive on to roads that are switched on and new <see cref="Vehicle"/>s will be generated on them.
		/// A vehicle node is switched off, no <see cref="Vehicle"/>s should be created on it and <see cref="Vehicle"/>s that already exist will try to avoid driving on to it.
		/// To undo effects of this method, use <see cref="SetVehicleNodesBackToOriginal(Vector3, Vector3)"/> or <see cref="SetVehicleNodesBackToOriginalInAngledArea(Vector3, Vector3, float)"/>.
		/// </para>
		/// <para>
		/// <paramref name="position1"/> and <paramref name="position2"/> define the midpoints of two parallel sides and <paramref name="areaWidth"/> is the width of these sides.
		/// </para>
		/// </summary>
		/// <param name="position1">One of the midpoints of two parallel sides, which should be different from <paramref name="position2"/>.</param>
		/// <param name="position2">One of the midpoints of two parallel sides, which should be different from <paramref name="position1"/>.</param>
		/// <param name="areaWidth">The width of these sides that defines <paramref name="position1"/> and <paramref name="position2"/>.</param>
		/// <param name="active">Specifies wheter vehicle nodes in area should be switched on or off. If <see langword="true"/>, they will be switched on.</param>
		public static void SwitchVehicleNodesInAngledArea(Vector3 position1, Vector3 position2, float areaWidth, bool active)
			=> Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, position1.X, position1.Y, position1.Z, position2.X, position2.Y, position2.Z, areaWidth, false, active, false);
		/// <summary>
		/// Sets all vehicle nodes in area back to their original state as per area and ynd file defines (which is loaded as <c>CPathRegion</c> in the game process memory).
		/// </summary>
		/// <param name="min">The minimum position of the region.</param>
		/// <param name="max">The maximum position of the region.</param>
		public static void SetVehicleNodesBackToOriginal(Vector3 min, Vector3 max) => Function.Call(Hash.SET_ROADS_BACK_TO_ORIGINAL, min.X, min.Y, min.Z, max.X, max.Y, max.Z, false);
		/// <summary>
		/// <para>
		/// Sets all vehicle nodes in area back to their original state as per area and ynd file defines (which is loaded as <c>CPathRegion</c> in the game process memory).
		/// </para>
		/// <para>
		/// <paramref name="position1"/> and <paramref name="position2"/> define the midpoints of two parallel sides and <paramref name="areaWidth"/> is the width of these sides.
		/// </para>
		/// </summary>
		/// <param name="position1">One of the midpoints of two parallel sides, which should be different from <paramref name="position2"/>.</param>
		/// <param name="position2">One of the midpoints of two parallel sides, which should be different from <paramref name="position1"/>.</param>
		/// <param name="areaWidth">The width of these sides that defines <paramref name="position1"/> and <paramref name="position2"/>.</param>
		public static void SetVehicleNodesBackToOriginalInAngledArea(Vector3 position1, Vector3 position2, float areaWidth)
			=> Function.Call(Hash.SET_ROADS_BACK_TO_ORIGINAL_IN_ANGLED_AREA, position1.X, position1.Y, position1.Z, position2.X, position2.Y, position2.Z, areaWidth, false);
	}
}
