using GTA.Math;
using GTA.Native;

namespace GTA
{
	public readonly struct RaycastResult
	{
		public RaycastResult(int handle) : this()
		{
			NativeVector3 hitPositionArg;
			bool hitSomethingArg;
			int materialHashArg;
			int entityHandleArg;
			NativeVector3 surfaceNormalArg;
			unsafe
			{
				Result = Function.Call<int>(Hash._GET_SHAPE_TEST_RESULT_EX, handle, &hitSomethingArg, &hitPositionArg, &surfaceNormalArg, &materialHashArg, &entityHandleArg);
			}

			DidHit = hitSomethingArg;
			HitPosition = hitPositionArg;
			SurfaceNormal = surfaceNormalArg;
			MaterialHash = (MaterialHash)materialHashArg;
			HitEntity = Entity.FromHandle(entityHandleArg);
		}

		/// <summary>
		/// Gets the raycast result code.
		/// </summary>
		public int Result { get; }

		/// <summary>
		/// Gets the <see cref="Entity" /> this raycast collided with.
		/// <remarks>Returns <c>null</c> if the raycast didn't collide with any <see cref="Entity"/>.</remarks>
		/// </summary>
		public Entity HitEntity { get; }
		/// <summary>
		/// Gets the world coordinates where this raycast collided.
		/// <remarks>Returns <see cref="Vector3.Zero"/> if the raycast didn't collide with anything.</remarks>
		/// </summary>
		public Vector3 HitPosition { get; }

		/// <summary>
		/// Gets the normal of the surface where this raycast collided.
		/// <remarks>Returns <see cref="Vector3.Zero"/> if the raycast didn't collide with anything.</remarks>
		/// </summary>
		public Vector3 SurfaceNormal { get; }

		/// <summary>
		/// Gets a hash indicating the material type of what this raycast collided with.
		/// <remarks>Returns <see cref="MaterialHash.None"/> if the raycast didn't collide with anything.</remarks>
		/// </summary>
		public MaterialHash MaterialHash { get; }

		/// <summary>
		/// Gets a value indicating whether this raycast collided with anything.
		/// </summary>
		public bool DidHit { get; }
		/// <summary>
		/// Gets a value indicating whether this raycast collided with any <see cref="Entity"/>.
		/// </summary>
		public bool DidHitEntity { get { return !ReferenceEquals(HitEntity, null); } }
	}
}
