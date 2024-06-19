//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
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
            int cEntityHandle;
            var hitCoords = new OutputArgument();
            var surfaceNormal = new OutputArgument();
            unsafe
            {
                Result = Function.Call<int>(Hash._GET_RAYCAST_RESULT, handle, &hitSomething, hitCoords, surfaceNormal, &cEntityHandle);
            }

            DitHitAnything = hitSomething != 0;
            HitCoords = hitCoords.GetResult<Vector3>();
            SurfaceNormal = surfaceNormal.GetResult<Vector3>();

            if (cEntityHandle != 0)
            {
                Entity cPhysicalHandle = Entity.FromHandle(cEntityHandle);
                if (cPhysicalHandle != null)
                {
                    HitEntity = cPhysicalHandle;
                }
                // The grabbed GUID/handle is associated with a `CEntity` but the instance isn't a `CPhysical`,
                // so we should try to release the GUID so the script GUID pool will be less likely to crash
                // the game for overflowing. We don't handle non-physical `CEntity`s with script GUIDs in the v2 API.
                else
                {
                    // The native won't delete the GUID from the script GUID pool if it is registered
                    // as a mission entity by some script (strictly when the found `CEntity` has
                    // a `CScriptEntityExtension`), though `SET_ENTITY_AS_MISSION_ENTITY` does nothing if the GUID
                    // isn't associated with a `CPhysical`.
                    Function.Call<int>((Hash)0x2B3334BCA57CD799 /* RELEASE_SCRIPT_GUID_FROM_ENTITY */, cEntityHandle);

                    HitEntity = null;
                }
            }
            // The shape test didn't hit a `CEntity` or `GET_SHAPE_TEST_RESULT` (named `_GET_RAYCAST_RESULT` in this
            // project) failed to create a GUID, no need to test if the entity handle is associated with a `CPhysical`
            // even.
            else
            {
                HitEntity = null;
            }
        }

        public int Result
        {
            get;
        }

        public bool DitHitEntity => HitEntity != null;

        public bool DitHitAnything
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
