//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// Set of enumerations of the state of any active <see cref="TaskInvoker.FollowNavMeshTo(Math.Vector3, PedMoveBlendRatio, int, float, FollowNavMeshFlags, float, float, float, float)"/> task running on the <see cref="Ped"/>.
	/// </summary>
	public enum NavMeshRouteResult
	{
		/// <summary>
		/// No navmesh task was found on the <see cref="Ped"/>.
		/// </summary>
		TaskNotFound,
		/// <summary>
		/// The task has not yet looked for a route.
		/// </summary>
		RouteNotYetTried,
		/// <summary>
		/// The task has tried &amp; failed to find a route (will keep trying).
		/// </summary>
		RouteNotFound,
		/// <summary>
		/// The task has successfully found a route.
		/// </summary>
		RouteFound,
	}
}
