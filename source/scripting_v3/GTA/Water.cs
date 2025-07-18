//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
    /// <summary>
    /// Represents a collection of static methods about water.
    /// </summary>
    public static class Water
    {
        /// <summary>
        /// Gets the level/height of the water below the position including the waves.
        /// This method takes the waves into account so the result may be different depending on
        /// the exact frame of calling.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if successfully get the level/height of the water below
        /// the position; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// This method gets the level of the water excluding the waves first, then adds the wave
        /// level delta if the method have successfully retrieved the level/height of the water.
        /// Therefore, this method returns <see langword="false"/> when
        /// <see cref="GetWaterLevelNoWaves"/> returns <see langword="false"/> with the same
        /// arguments.
        /// </remarks>
        public static bool GetWaterLevel(Vector3 position, out float height)
        {
            unsafe
            {
                float returnZ;
                bool foundWater =
                    Function.Call<bool>(Hash.GET_WATER_HEIGHT, position.X, position.Y, position.Z, &returnZ);

                height = returnZ;
                return foundWater;
            }
        }
        /// <summary>
        /// Gets the height of the water below the position excluding the waves.
        /// This method does not take the waves into account so the result will be the same between
        /// different frames.
        /// </summary>
        /// <returns></returns>
        public static bool GetWaterLevelNoWaves(Vector3 position, out float height)
        {
            unsafe
            {
                float returnZ;
                bool foundWater = Function.Call<bool>(Hash.GET_WATER_HEIGHT_NO_WAVES, position.X, position.Y,
                    position.Z, &returnZ);

                height = returnZ;
                return foundWater;
            }
        }

        /// <summary>
        /// Test a directed line probe against the water.
        /// </summary>
        /// <param name="startPos">The start of the probe.</param>
        /// <param name="endPos">The end of the probe.</param>
        /// <param name="intersectionPos">
        /// When this method returns, contains the intersection position on the water, the line probe hits water before
        /// hitting land. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the line probe hits water before hitting land; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool TestLineAgainstWater(Vector3 startPos, Vector3 endPos, out Vector3 intersectionPos)
        {
            unsafe
            {
                NativeVector3 outPos;
                bool hitWaterBeforeLand = Function.Call<bool>(Hash.TEST_PROBE_AGAINST_WATER, startPos.X, startPos.Y,
                    startPos.Z, endPos.X, endPos.Y, endPos.Z, &outPos);

                intersectionPos = outPos;
                return hitWaterBeforeLand;
            }
        }
    }
}
