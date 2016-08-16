using GTA.Math;
using GTA.Native;

namespace GTA
{
	public struct RaycastResult
	{
		public unsafe RaycastResult(int handle) : this()
		{
		    NativeVector3 hitPositionArg;
		    bool hitSomethingArg;
		    int entityHandleArg;
		    NativeVector3 surfaceNormalArg;
			Result = Function.Call<int>(Hash._GET_RAYCAST_RESULT, handle, &hitSomethingArg, &hitPositionArg, &surfaceNormalArg, &entityHandleArg);

			DitHit = hitSomethingArg;
			HitPosition = hitPositionArg;
			SurfaceNormal = surfaceNormalArg;
			HitEntity = Entity.FromHandle(entityHandleArg);
		}

		public Entity HitEntity { get; private set; }
		public Vector3 HitPosition { get; private set; }
		public Vector3 SurfaceNormal { get; private set; }

		public bool DitHit { get; private set; }
		public bool DitHitEntity
		{
			get
			{
				return !ReferenceEquals(HitEntity, null);
			}
		}

		public int Result { get; private set; }
	}
}
