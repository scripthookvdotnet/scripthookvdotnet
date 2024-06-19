//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

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
            int cEntityHandleArg;
            NativeVector3 surfaceNormalArg;
            unsafe
            {
                Result = Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT_INCLUDING_MATERIAL, handle, &hitSomethingArg, &hitPositionArg, &surfaceNormalArg, &materialHashArg, &cEntityHandleArg);
            }

            DidHit = hitSomethingArg;
            HitPosition = hitPositionArg;
            SurfaceNormal = surfaceNormalArg;
            MaterialHash = (MaterialHash)materialHashArg;

            // The guid is zero if the shape test didn't hit a `CEntity` or `GET_SHAPE_TEST_RESULT` failed to create
            // a GUID
            if (cEntityHandleArg != 0)
            {
                Entity cPhysicalHandle = Entity.FromHandle(cEntityHandleArg);
                if (cPhysicalHandle != null)
                {
                    HitEntity = cPhysicalHandle;
                }
                // The grabbed GUID/handle is associated with a `CEntity` but the instance isn't a `CPhysical`,
                // so we should try to release the GUID so the script GUID pool will be less likely to crash
                // the game for overflowing. We won't handle non-physical `CEntity`s with script GUIDs in the v3 API.
                else
                {
                    ShapeTestHandle.ReleaseGuid(cEntityHandleArg);

                    HitEntity = null;
                }
            }
            else
            {
                HitEntity = null;
            }
        }

        /// <summary>
        /// Gets the result code.
        /// </summary>
        public int Result
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this ray cast collided with anything.
        /// </summary>
        public bool DidHit
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Entity" /> this ray cast collided with.
        /// <remarks>Returns <see langword="null" /> if the ray cast didn't collide with any <see cref="Entity"/>.</remarks>
        /// </summary>
        public Entity HitEntity
        {
            get;
        }
        /// <summary>
        /// Gets the world coordinates where this ray cast collided.
        /// <remarks>Returns <see cref="Vector3.Zero"/> if the ray cast didn't collide with anything.</remarks>
        /// </summary>
        public Vector3 HitPosition
        {
            get;
        }

        /// <summary>
        /// Gets the normal of the surface where this ray cast collided.
        /// <remarks>Returns <see cref="Vector3.Zero"/> if the ray cast didn't collide with anything.</remarks>
        /// </summary>
        public Vector3 SurfaceNormal
        {
            get;
        }

        /// <summary>
        /// Gets a hash indicating the material type of what this ray cast collided with.
        /// <remarks>Returns <see cref="MaterialHash.None"/> if the ray cast didn't collide with anything.</remarks>
        /// </summary>
        public MaterialHash MaterialHash
        {
            get;
        }
    }
}
