//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;

namespace GTA
{
	/// <summary>
	/// Represents a shape test result.
	/// </summary>
	public readonly struct ShapeTestResult
	{
		internal ShapeTestResult(bool didHit, Vector3 hitPosition, Vector3 surfaceNormal, int guidHandle) : this()
		{
			DidHit = didHit;
			HitPosition = hitPosition;
			SurfaceNormal = surfaceNormal;
			GuidHandle = guidHandle;
		}

		private int GuidHandle
		{
			get;
		}

		/// <summary>
		/// Gets a value indicating whether this shape test collided with anything.
		/// </summary>
		public bool DidHit
		{
			get;
		}

		/// <summary>
		/// Try to get the <see cref="Entity" /> this shape test hit.
		/// <remarks>Returns <see langword="false" /> if the shape test didn't hit or what was hit wasn't a <see cref="Entity" />.</remarks>
		/// </summary>
		public bool TryGetHitEntity(out Entity hitEntity)
		{
			hitEntity = Entity.FromHandle(GuidHandle);
			return hitEntity != null;
		}

		/// <summary>
		/// Gets the world coordinates where this shape test hit.
		/// <remarks>Returns <see cref="Vector3.Zero"/> if the shape test didn't hit anything.</remarks>
		/// </summary>
		public Vector3 HitPosition
		{
			get;
		}

		/// <summary>
		/// Gets the normal of the surface where this shape test hit.
		/// <remarks>Returns <see cref="Vector3.Zero"/> if the shape test didn't hit anything.</remarks>
		/// </summary>
		public Vector3 SurfaceNormal
		{
			get;
		}
	}
}
