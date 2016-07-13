using GTA.Math;
using GTA.Native;

namespace GTA
{
	public struct RaycastResult
	{
		public RaycastResult(int handle) : this()
		{
			var hitPositionArg = new OutputArgument();
			var hitSomethingArg = new OutputArgument();
			var entityHandleArg = new OutputArgument();
			var surfaceNormalArg = new OutputArgument();
			Result = Function.Call<int>(Hash._GET_RAYCAST_RESULT, handle, hitSomethingArg, hitPositionArg, surfaceNormalArg, entityHandleArg);

			DitHit = hitSomethingArg.GetResult<bool>();
			HitPosition = hitPositionArg.GetResult<Vector3>();
			SurfaceNormal = surfaceNormalArg.GetResult<Vector3>();
			HitEntity = Entity.FromHandle(entityHandleArg.GetResult<int>());
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
