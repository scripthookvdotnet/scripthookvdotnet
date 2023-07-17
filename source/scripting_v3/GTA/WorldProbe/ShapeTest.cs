//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
	public static class ShapeTest
	{
		/// <summary>
		/// Start a line-of-sight world probe shape test between 2 points.
		/// </summary>
		/// <param name="startPosition">The position where the shape test starts.</param>
		/// <param name="endPosition">The position where the shape test ends.</param>
		/// <param name="intersectFlags">What type of objects the shape test should intersect with.</param>
		/// <param name="excludeEntity">Specify an <see cref="Entity"/> that the shape test should exclude, leave null for no entities ignored.</param>
		/// <param name="options">Specify options for the shape test.</param>
		/// <value>
		/// The shape test handle.
		/// If this method fails to create the shape test request because there are too many ongoing requests, <see cref="ShapeTestHandle.IsRequestFailed"/> will return <see langword="true" /> on the handle struct.
		/// </value>
		public static ShapeTestHandle StartTestLOSProbe(Vector3 startPosition, Vector3 endPosition, IntersectFlags intersectFlags = IntersectFlags.Map, Entity excludeEntity = null, ShapeTestOptions options = ShapeTestOptions.Default)
		{
			return new ShapeTestHandle(Function.Call<int>(Hash.START_SHAPE_TEST_LOS_PROBE,
				startPosition.X,
				startPosition.Y,
				startPosition.Z,
				endPosition.X,
				endPosition.Y,
				endPosition.Z,
				(int)intersectFlags,
				excludeEntity == null
					? 0
					: excludeEntity.Handle,
				(int)options));
		}

		/// <summary>
		/// Start a expensive synchronous line-of-sight world probe shape test between 2 points and blocks the game until the shape test completes.
		/// </summary>
		/// <param name="startPosition">The position where the shape test starts.</param>
		/// <param name="endPosition">The position where the shape test ends.</param>
		/// <param name="intersectFlags">What type of objects the shape test should intersect with.</param>
		/// <param name="excludeEntity">Specify an <see cref="Entity"/> that the shape test should exclude, leave null for no entities ignored.</param>
		/// <param name="options">Specify options for the shape test.</param>
		/// <remarks>This method is much more expensive than the asynchronous version (<see cref="StartTestLOSProbe(Vector3, Vector3, IntersectFlags, Entity, ShapeTestOptions)"/>).</remarks>
		/// <value>
		/// The shape test handle.
		/// If this method fails to create the shape test request because there are too many ongoing requests, <see cref="ShapeTestHandle.IsRequestFailed"/> will return <see langword="true" /> on the handle struct.
		/// </value>
		public static ShapeTestHandle StartExpensiveSyncTestLOSProbe(Vector3 startPosition, Vector3 endPosition, IntersectFlags intersectFlags = IntersectFlags.Map, Entity excludeEntity = null, ShapeTestOptions options = ShapeTestOptions.Default)
		{
			return new ShapeTestHandle(Function.Call<int>(Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE,
				startPosition.X,
				startPosition.Y,
				startPosition.Z,
				endPosition.X,
				endPosition.Y,
				endPosition.Z,
				(int)intersectFlags,
				excludeEntity == null
					? 0
					: excludeEntity.Handle,
				(int)options));
		}

		/// <summary>
		/// Start a shape test against the <see cref="Entity"/>'s bounding box.
		/// </summary>
		/// <param name="entity">The entity to inspect.</param>
		/// <param name="intersectFlags">What type of objects the shape test should intersect with.</param>
		/// <param name="options">Specify options for the shape test.</param>
		/// <value>
		/// The shape test handle.
		/// If this method fails to create the shape test request because there are too many ongoing requests, <see cref="ShapeTestHandle.IsRequestFailed"/> will return <see langword="true" /> on the handle struct.
		/// </value>
		public static ShapeTestHandle StartTestBoundingBox(Entity entity, IntersectFlags intersectFlags = IntersectFlags.BoundingBox, ShapeTestOptions options = ShapeTestOptions.IgnoreNoCollision)
		{
			return new ShapeTestHandle(Function.Call<int>(Hash.START_SHAPE_TEST_BOUNDING_BOX, entity.Handle, (int)intersectFlags, (int)options));
		}

		/// <summary>
		/// Start a shape test against the <see cref="Entity"/>'s bound where the entity can collide.
		/// </summary>
		/// <param name="sourcePosition">The source position.</param>
		/// <param name="dimension">The dimensions how much the shape test will search from the source position.</param>
		/// <param name="rotationAngles">The rotations in degree how much the dimension will be rotated before the shape test starts.</param>
		/// <param name="rotationOrder">The rotation order in local space the dimensions will be rotated in.</param>
		/// <param name="intersectFlags">What type of objects the shape test should intersect with.</param>
		/// <param name="excludeEntity">Specify an <see cref="Entity"/> that the shape test should exclude, leave null for no entities ignored.</param>
		/// <param name="options">Specify options for the shape test.</param>
		/// <value>
		/// The shape test handle.
		/// If this method fails to create the shape test request because there are too many ongoing requests, <see cref="ShapeTestHandle.IsRequestFailed"/> will return <see langword="true" /> on the handle struct.
		/// </value>
		public static ShapeTestHandle StartTestBox(Vector3 sourcePosition, Vector3 dimension, Vector3 rotationAngles, EulerRotationOrder rotationOrder = EulerRotationOrder.YXZ, IntersectFlags intersectFlags = IntersectFlags.Map, Entity excludeEntity = null, ShapeTestOptions options = ShapeTestOptions.IgnoreNoCollision)
		{
			return new ShapeTestHandle(Function.Call<int>(Hash.START_SHAPE_TEST_BOX,
				sourcePosition.X,
				sourcePosition.Y,
				sourcePosition.Z,
				dimension.X,
				dimension.Y,
				dimension.Z,
				rotationAngles.X,
				rotationAngles.Y,
				rotationAngles.Z,
				(int)rotationOrder,
				(int)intersectFlags,
				excludeEntity == null
					? 0
					: excludeEntity.Handle,
				(int)options));
		}

		/// <summary>
		/// Start a shape test against the <see cref="Entity"/>'s bound where the entity can collide.
		/// </summary>
		/// <param name="entity">The entity to inspect.</param>
		/// <param name="intersectFlags">What type of objects the shape test should intersect with.</param>
		/// <param name="options">Specify options for the shape test.</param>
		/// <value>
		/// The shape test handle.
		/// If this method fails to create the shape test request because there are too many ongoing requests, <see cref="ShapeTestHandle.IsRequestFailed"/> will return <see langword="true" /> on the handle struct.
		/// </value>
		public static ShapeTestHandle StartTestBound(Entity entity, IntersectFlags intersectFlags = IntersectFlags.Map, ShapeTestOptions options = ShapeTestOptions.IgnoreNoCollision)
		{
			return new ShapeTestHandle(Function.Call<int>(Hash.START_SHAPE_TEST_BOUND, entity.Handle, (int)intersectFlags, (int)options));
		}

		/// <summary>
		/// Start a shape test against the area where shape test capsule covers.
		/// </summary>
		/// <param name="startPosition">The position where the shape test starts.</param>
		/// <param name="endPosition">The position where the shape test ends.</param>
		/// <param name="radius">The radius of the shape test capsule.</param>
		/// <param name="intersectFlags">What type of objects the shape test should intersect with.</param>
		/// <param name="excludeEntity">Specify an <see cref="Entity"/> that the shape test should exclude, leave null for no entities ignored.</param>
		/// <param name="options">Specify options for the shape test.</param>
		/// <value>
		/// The shape test handle.
		/// If this method fails to create the shape test request because there are too many ongoing requests, <see cref="ShapeTestHandle.IsRequestFailed"/> will return <see langword="true" /> on the handle struct.
		/// </value>
		public static ShapeTestHandle StartTestCapsule(Vector3 startPosition, Vector3 endPosition, float radius, IntersectFlags intersectFlags = IntersectFlags.Map, Entity excludeEntity = null, ShapeTestOptions options = ShapeTestOptions.IgnoreNoCollision)
		{
			return new ShapeTestHandle(Function.Call<int>(Hash.START_SHAPE_TEST_CAPSULE,
				startPosition.X,
				startPosition.Y,
				startPosition.Z,
				endPosition.X,
				endPosition.Y,
				endPosition.Z,
				radius,
				(int)intersectFlags,
				excludeEntity == null
					? 0
					: excludeEntity.Handle,
				(int)options));
		}

		/// <summary>
		/// Start a shape test against the area where swept sphere (ellipsoid) for shape test covers.
		/// </summary>
		/// <param name="startPosition">The position where the shape test starts.</param>
		/// <param name="endPosition">The position where the shape test ends.</param>
		/// <param name="radius">The radius of the swept sphere.</param>
		/// <param name="intersectFlags">What type of objects the shape test should intersect with.</param>
		/// <param name="excludeEntity">Specify an <see cref="Entity"/> that the shape test should exclude, leave null for no entities ignored.</param>
		/// <param name="options">Specify options for the shape test.</param>
		/// <value>
		/// The shape test handle.
		/// If this method fails to create the shape test request because there are too many ongoing requests, <see cref="ShapeTestHandle.IsRequestFailed"/> will return <see langword="true" /> on the handle struct.
		/// </value>
		public static ShapeTestHandle StartTestSweptSphere(Vector3 startPosition, Vector3 endPosition, float radius, IntersectFlags intersectFlags = IntersectFlags.Map, Entity excludeEntity = null, ShapeTestOptions options = ShapeTestOptions.IgnoreNoCollision)
		{
			return new ShapeTestHandle(Function.Call<int>(Hash.START_SHAPE_TEST_SWEPT_SPHERE,
				startPosition.X,
				startPosition.Y,
				startPosition.Z,
				endPosition.X,
				endPosition.Y,
				endPosition.Z,
				radius,
				(int)intersectFlags,
				excludeEntity == null
					? 0
					: excludeEntity.Handle,
				(int)options));
		}

		/// <summary>
		/// Start a line-of-sight world probe shape test between 2 points calculated based on the mouse cursor position.
		/// Works just like <see cref="StartTestLOSProbe(Vector3, Vector3, IntersectFlags, Entity, ShapeTestOptions)"/> only the start and end points of the probe are calculated based on the mouse cursor position projected into the world.
		/// </summary>
		/// <param name="probeStartPosition">The returned start position of the probe in world space.</param>
		/// <param name="probeEndPosition">The returned end position of the probe in world space.</param>
		/// <param name="intersectFlags">What type of objects the shape test should intersect with.</param>
		/// <param name="excludeEntity">Specify an <see cref="Entity"/> that the shape test should exclude, leave null for no entities ignored.</param>
		/// <param name="options">Specify options for the shape test.</param>
		/// <value>
		/// The shape test handle.
		/// If this method fails to create the shape test request because there are too many ongoing requests, <see cref="ShapeTestHandle.IsRequestFailed"/> will return <see langword="true" /> on the handle struct.
		/// </value>
		public static ShapeTestHandle StartTestMouseCursorLOSProbe(out Vector3 probeStartPosition, out Vector3 probeEndPosition, IntersectFlags intersectFlags = IntersectFlags.Map, Entity excludeEntity = null, ShapeTestOptions options = ShapeTestOptions.Default)
		{
			NativeVector3 outProbeStartPositionNative;
			NativeVector3 outProbeEndPositionNative;
			ShapeTestHandle handle;

			unsafe
			{
				handle = new ShapeTestHandle(Function.Call<int>(Hash.START_SHAPE_TEST_MOUSE_CURSOR_LOS_PROBE,
					&outProbeStartPositionNative,
					&outProbeEndPositionNative,
					(int)intersectFlags,
					excludeEntity == null
						? 0
						: excludeEntity.Handle,
					(int)options));
			}

			probeStartPosition = outProbeStartPositionNative;
			probeEndPosition = outProbeEndPositionNative;
			return handle;
		}

		/// <summary>
		/// Start a shape test between 2 points calculated based on the mouse cursor position.
		/// Works just like <see cref="StartTestLOSProbe(Vector3, Vector3, IntersectFlags, Entity, ShapeTestOptions)"/> only the start and end points of the probe are calculated based on the mouse cursor position projected into the world.
		/// </summary>
		/// <param name="intersectFlags">What type of objects the shape test should intersect with.</param>
		/// <param name="excludeEntity">Specify an <see cref="Entity"/> that the shape test should exclude, leave null for no entities ignored.</param>
		/// <param name="options">Specify options for the shape test.</param>
		/// <value>
		/// The shape test handle and the start and end points of the probe are calculated based on the mouse cursor position projected into the world.
		/// If this method fails to create the shape test request because there are too many ongoing requests, <see cref="ShapeTestHandle.IsRequestFailed"/> will return <see langword="true" /> on the handle struct.
		/// </value>
		public static (ShapeTestHandle handle, Vector3 probeStartPosition, Vector3 probeEndPosition) StartTestMouseCursorLOSProbe(IntersectFlags intersectFlags, Entity excludeEntity = null, ShapeTestOptions options = ShapeTestOptions.Default)
		{
			ShapeTestHandle handle = StartTestMouseCursorLOSProbe(out Vector3 probeStartPosition, out Vector3 probeEndPosition, intersectFlags, excludeEntity, options);
			return (handle, probeStartPosition, probeEndPosition);
		}
	}
}
