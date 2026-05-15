//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
    /// <summary> Provides static methods for managing fires within the game world. </summary>
    public static class FireManager
    {
        /// <summary>
        /// Creates a fire at the specified location, with the specified maximum number of children fires and whether it's a gas fire.
        /// </summary>
        /// <param name="position">The position where the fire will be created.</param>
        /// <param name="maxChildrenCount">The maximum number of children fires. Must be less than 25 or else the fire will not be created.</param>
        /// <param name="isGasFire">If true, makes the fire look like it originates from a line of gasoline.</param>
        /// <returns>The created <see cref="ScriptFire"/> instance.</returns>
        public static ScriptFire StartScriptFire(Vector3 position, int maxChildrenCount, bool isGasFire = false)
        {
            int handle = Function.Call<int>(Hash.START_SCRIPT_FIRE, position.X, position.Y, position.Z, maxChildrenCount, isGasFire);
            return new ScriptFire(handle);
        }

        /// <summary>
        /// Gets the number of active fires within a specified radius of a given position.
        /// </summary>
        /// <param name="position">The center point, as a Vector3, from which to search for fires.</param>
        /// <param name="radius">The radius within which to count active fires. </param>
        /// <returns>The number of active fires found within the specified radius of the given position.</returns>
        public static int GetNumberOfFiresInRadius(Vector3 position, float radius)
            => Function.Call<int>(Hash.GET_NUMBER_OF_FIRES_IN_RANGE, position.X, position.Y, position.Z, radius);

        /// <summary>
        /// Stops all active fires within the specified radius of the given position.
        /// </summary>
        /// <param name="position">The center point around which to stop fires.</param>
        /// <param name="radius">The radius, within which all fires will be stopped. </param>
        public static void StopFireInRadius(Vector3 position, float radius)
            => Function.Call(Hash.STOP_FIRE_IN_RANGE, position.X, position.Y, position.Z, radius);

        /// <summary>
        /// Attempts to find the position of the closest fire relative to the specified location.
        /// </summary>
        /// <param name="position">The world position from which to search for the nearest fire.</param>
        /// <param name="firePosition">If method returns true, contains the world position of the closest fire to the specified location; otherwise, <seealso cref="Vector3.Zero"/>
        /// This parameter is passed uninitialized.</param>
        /// <returns>true if a fire position is found near the specified location; otherwise, false.</returns>
        public static bool TryGetClosestFirePosition(Vector3 position, out Vector3 firePosition)
        {
            NativeVector3 outPos;
            unsafe
            {
                bool foundFire = Function.Call<bool>(Hash.GET_CLOSEST_FIRE_POS, &outPos, position.X, position.Y, position.Z);
                firePosition = foundFire ? outPos : Vector3.Zero;
                return foundFire;
            }
        }
    }
}
