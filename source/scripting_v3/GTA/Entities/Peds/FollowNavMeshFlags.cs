//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Set of flags which may be passed in to <see cref="TaskInvoker.FollowNavMeshTo(Math.Vector3, PedMoveBlendRatio?, int, float, FollowNavMeshFlags, float)"/>
	/// and <see cref="TaskInvoker.FollowNavMeshTo(Math.Vector3, PedMoveBlendRatio, int, float, FollowNavMeshFlags, float, float, float, float)"/>.
	/// The latter overload takes 3 additional parameters, which in some cases must contain values required for the extra functionality -
	/// where this is the case it is described in the individual documents of the flags.
	/// </summary>
	[Flags]
	public enum FollowNavMeshFlags
	{
		Default = 0,
		/// <summary>
		/// Will ensure the <see cref="Ped"/> continues to move whilst waiting for the path to be found,
		/// and will not slow down at the end of their route.
		/// </summary>
		NoStopping = 1,
		/// <summary>
		/// Performs a slide-to-coord at the and of the task.
		/// This requires that the parameter <c>slideToCoordHeading</c> is set correctly.
		/// </summary>
		AdvancedSlideToCoordAndAchieveHeadingAtEnd = 2,
		/// <summary>
		/// If the navmesh is not loaded in under the target position, then this will cause the ped to get as close as is possible on whatever navmesh is loaded.
		/// The navmesh must still be loaded at the path start.
		/// </summary>
		GoFarAsPossibleIfTargetNavmeshNotLoaded = 4,
		/// <summary>
		/// Will allow navigation underwater - by default this is not allowed.
		/// </summary>
		AllowSwimmingUnderwater = 8,
		/// <summary>
		/// Will only allow navigation on pavements.
		/// If the path starts or ends off the pavement, the command will fail.
		/// Likewise if no pavement-only route can be found even although the start and end are on pavement.
		/// </summary>
		KeepToPavements = 16,
		/// <summary>
		/// Prevents the path from entering water at all.
		/// </summary>
		NeverEnterWater = 32,
		/// <summary>
		/// Disables object-avoidance for this path.
		/// The ped may still make minor steering adjustments to avoid objects, but will not pathfind around them.
		/// </summary>
		DontAvoidObjects = 64,
		/// <summary>
		/// Specifies that the navmesh route will only be able to traverse up slopes which are under the angle specified in the parameter <c>maxSlopeNavigable</c>.
		/// </summary>
		AdvancedUseMaxSlopeNavigable = 128,
		/// <summary>
		/// The entity will look ahead in its path for a longer distance to make the walk/run start go more in the right direction.
		/// Especially useful when ped start from inside an object boundaries but has to be used carefully,
		/// the ped might be more prone to walk into things during the walk/runstart with this flag set.
		/// </summary>
		AccurateWalkRunStart = 1024,
		/// <summary>
		/// Disables ped-avoidance for this path while we move.
		/// </summary>
		DontAvoidPeds = 2048,
		/// <summary>
		/// If target pos is inside the boundingbox of an object it will otherwise be pushed out.
		/// This flag should be used with extreme caution. Use only if asked specificly to use this.
		/// </summary>
		DontAdjustTargetPosition = 4096,
		/// <summary>
		/// Turns off the default behaviour, which is to stop exactly at the target position.
		/// Occasionally this can cause footsliding/skating problems.
		/// </summary>
		SuppressExactStop = 8192,
		/// <summary>
		/// Prevents the path-search from finding paths outside of this search distance.
		/// This can be used to prevent peds from finding long undesired routes.
		/// The value the parameter <c>clampMaxSearchDistance</c> must be set, and this value must be between 1 and 255 (corresponds to game units).
		/// The seach area is limited to an axis aligned box containing a sphere of the given radius.
		/// </summary>
		AdvancedUseClampMaxSearchDistance = 16384,
		/// <summary>
		/// Pulls out the paths from edges at corners for a longer distance, to prevent peds walking into stuff.
		/// This could in rare cases generate bigger quirks in the paths so use only when it is necessary.
		/// </summary>
		PullFromEdgeExtra = 32768
	}
}
