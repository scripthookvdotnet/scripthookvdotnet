//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
	public struct RaycastResult
	{
		internal RaycastResult(int handle)
		{
			int hitSomething;
			int entityHandle;
			var hitCoords = new OutputArgument();
			var surfaceNormal = new OutputArgument();
			unsafe
			{
				Result = Function.Call<int>(Hash._GET_RAYCAST_RESULT, handle, &hitSomething, hitCoords, surfaceNormal, &entityHandle);
			}

			DidHitAnything = hitSomething != 0;
			HitEntity = Entity.FromHandle(entityHandle);
			HitCoords = hitCoords.GetResult<Vector3>();
			SurfaceNormal = surfaceNormal.GetResult<Vector3>();
		}

		public int Result
		{
			get;
		}

		public bool DidHitEntity
		{
			get => !(HitEntity is null);
		}

		public bool DidHitAnything
		{
			get;
		}

		public Entity HitEntity
		{
			get;
		}

		public Vector3 HitCoords
		{
			get;
		}

		public Vector3 SurfaceNormal
		{
			get;
		}
	}
}
