using System;
using System.ComponentModel;
using GTA.Math;
using GTA.Native;

namespace GTA
{
    public static partial class World
    {
        /// <inheritdoc cref="GTA.Chrono.GameClock.IsPaused"/>
        [Obsolete("World.IsClockPaused is obsolete, use GTA.Chrono.IsPaused instead.")]
        public static bool IsClockPaused
        {
            get => SHVDN.NativeMemory.IsClockPaused;
            set => Function.Call(Hash.PAUSE_CLOCK, value);
        }

        /// <summary>
        /// Pauses or resumes the in-game clock.
        /// </summary>
        /// <param name="value">Pauses the game clock if set to <see langword="true" />; otherwise, resumes the game clock.</param>
        [Obsolete("The World.PauseClock is obsolete, use GTA.Chrono.IsPaused instead.")]
        public static void PauseClock(bool value)
        {
            IsClockPaused = value;
        }

        /// <summary>
        /// Gets or sets the current date and time in the GTA world.
        /// </summary>
        /// <value>
        /// The current date and time.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The internal date is not valid for the Gregorian calendar or the internal time of day is not normalized.
        /// </exception>
        [Obsolete("World.CurrentDate is obsolete because DateTime can represent the years only in the range of 1 to 9999, while the game supports wider range of years." +
            "Use properties or methods of GTA.Chrono.GameClock instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public static DateTime CurrentDate
        {
            get
            {
                int year = Function.Call<int>(Hash.GET_CLOCK_YEAR);
                int month = Function.Call<int>(Hash.GET_CLOCK_MONTH) + 1;
                int day = System.Math.Min(Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH), s_calendar.GetDaysInMonth(year, month));
                int hour = Function.Call<int>(Hash.GET_CLOCK_HOURS);
                int minute = Function.Call<int>(Hash.GET_CLOCK_MINUTES);
                int second = Function.Call<int>(Hash.GET_CLOCK_SECONDS);

                return new DateTime(year, month, day, hour, minute, second);
            }
            set
            {
                Function.Call(Hash.SET_CLOCK_DATE, value.Day, value.Month - 1, value.Year);
                Function.Call(Hash.SET_CLOCK_TIME, value.Hour, value.Minute, value.Second);
            }
        }

        /// <summary>
        /// Gets or sets the current time of day in the GTA world.
        /// </summary>
        /// <value>
        /// The current time of day.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// One of the values minutes or seconds are smaller than <c>0</c> or larger than <c>59</c>, or the hour value
        /// is smaller than <c>0</c> or larger than <c>23</c>.
        /// </exception>
        /// <remarks>
        /// The resolution of the value is 1 second.
        /// </remarks>
        [Obsolete("World.CurrentTimeOfDay is obsolete, use GTA.Chrono.GameClock.Today instead.")]
        public static TimeSpan CurrentTimeOfDay
        {
            get
            {
                int hours = Function.Call<int>(Hash.GET_CLOCK_HOURS);
                int minutes = Function.Call<int>(Hash.GET_CLOCK_MINUTES);
                int seconds = Function.Call<int>(Hash.GET_CLOCK_SECONDS);

                return new TimeSpan(hours, minutes, seconds);
            }
            set => Function.Call(Hash.SET_CLOCK_TIME, value.Hours, value.Minutes, value.Seconds);
        }

        /// <inheritdoc cref="GTA.Chrono.GameClock.MillisecondsPerGameMinute"/>
        [Obsolete("World.MillisecondsPerGameMinute is obsolete, use GTA.Chrono.GameClock.MillisecondsPerGameMinute instead.")]
        public static int MillisecondsPerGameMinute
        {
            get => Function.Call<int>(Hash.GET_MILLISECONDS_PER_GAME_MINUTE);
            set => SHVDN.NativeMemory.MillisecondsPerGameMinute = value;
        }

        /// <summary>
        /// Creates a pickup <see cref="Prop"/> similar to those dropped by dead <see cref="Ped"/>s.
        /// These types of pickups are part of the ambient population and will get removed if the player moves too far away from them.
        /// </summary>
        [Obsolete("The World.CreateAmbientPickup overload with non-optional custom model and amount (named \"value\") parameters are obsolete since they can lead to confusion in custom model parameter (which is actually not mandatory)." +
            "Use World.CreateAmbientPickup(PickupType, Vector3, PickupPlacementFlags, int, Model, bool) instead.")]
        public static Prop CreateAmbientPickup(PickupType type, Vector3 position, Model model, int value)
            => CreateAmbientPickup(type, position, PickupPlacementFlags.None, value, model, false);

        /// <summary>
        /// Spawns a <see cref="Pickup"/> at the specified position.
        /// </summary>
        [Obsolete("The World.CreatePickup overloads with non-optional custom model and amount (named \"value\") parameters are obsolete since they can lead to confusion in custom model parameter (which is actually not mandatory)." +
            "Use a World.CreatePickup overload with optional placement flags and amount parameters instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public static Pickup CreatePickup(PickupType type, Vector3 position, Model model, int value)
            => CreatePickup(type, position, PickupPlacementFlags.None, value, model);
        /// <summary>
        /// Spawns a <see cref="Pickup"/> at the specified position.
        /// </summary>
        [Obsolete("The World.CreatePickup overloads with non-optional custom model and amount (named \"value\") parameters are obsolete since they can lead to confusion in custom model parameter (which is actually not mandatory)." +
            "Use a World.CreatePickup overload with optional placement flags and amount parameters instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public static Pickup CreatePickup(PickupType type, Vector3 position, Vector3 rotation, Model model, int value)
            => CreatePickup(type, position, rotation, PickupPlacementFlags.None, value, EulerRotationOrder.YXZ, model);

        /// <summary>
        /// Destroys all scripted <see cref="Camera"/>s.
        /// </summary>
        [Obsolete("World.DestroyAllCameras is obsolete. Use Camera.DeleteAllCameras instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public static void DestroyAllCameras()
        {
            Function.Call(Hash.DESTROY_ALL_CAMS, 0);
        }

        /// <summary>
        /// Creates a <see cref="Camera"/>, use <see cref="ScriptCameraDirector.StartRendering()"/> to switch to
        /// this camera.
        /// </summary>
        /// <param name="position">The position of the camera.</param>
        /// <param name="rotation">The rotation of the camera.</param>
        /// <param name="fov">The field of view of the camera.</param>
        /// <remarks>
        /// This overload (<see cref="World.CreateCamera(Vector3, Vector3, float)"/>) does not return <see langword="null"/>
        /// even if the method fails to create and <c>CREATE_CAM_WITH_PARAMS</c> returns -1 due to the camera pool being full.
        /// This is done for compatibility for scripts built against v3.6.0 or earlier.
        /// </remarks>
        [Obsolete("World.CreateCamera is obsolete. Use Camera.Create instead."),
         EditorBrowsable(EditorBrowsableState.Never)]
        public static Camera CreateCamera(Vector3 position, Vector3 rotation, float fov)
        {
            return new Camera(Function.Call<int>(Hash.CREATE_CAM_WITH_PARAMS, "DEFAULT_SCRIPTED_CAMERA", position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, fov, 1, 2));
        }

        /// <summary>
        /// Gets or sets the rendering camera.
        /// </summary>
        /// <value>
        /// The rendering <see cref="Camera"/>.
        /// </value>
        /// <remarks>
        /// Setting to <see langword="null" /> sets the rendering <see cref="Camera"/> to <see cref="GameplayCamera"/>.
        /// The getter will return an invalid <see cref="Camera"/> where <see cref="PoolObject.Handle"/> is -1 if the
        /// rendering camera does not match any scripted cameras the scripted camera director is managing.
        /// </remarks>
        [Obsolete("World.RenderingCamera is obsolete. " +
            "Use ScriptCameraDirector.RenderingCam to get the rendering scripted camera. " +
            "Use ScriptCameraDirector.StartRendering or ScriptCameraDirector.StopRendering to tell the game to render " +
            "or stop rendering a scripted camera."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public static Camera RenderingCamera
        {
            get => new(Function.Call<int>(Hash.GET_RENDERING_CAM));
            set
            {
                if (value == null)
                {
                    Function.Call(Hash.RENDER_SCRIPT_CAMS, false, 0, 3000, 1, 0);
                }
                else
                {
                    value.IsActive = true;
                    Function.Call(Hash.RENDER_SCRIPT_CAMS, true, 0, 3000, 1, 0);
                }
            }
        }

        /// <summary>
        /// Fires a single bullet in the world.
        /// </summary>
        /// <param name="sourcePosition">Where the bullet is fired from.</param>
        /// <param name="targetPosition">Where the bullet is fired to.</param>
        /// <param name="owner">The <see cref="Ped"/> who fired the bullet, leave <see langword="null" /> for no one.</param>
        /// <param name="weaponAsset">The weapon that the bullet is fired from.</param>
        /// <param name="damage">The damage the bullet will cause.</param>
        /// <param name="speed">The speed, only affects projectile weapons, leave -1 for default.</param>
        [Obsolete("Use ShootSingleBullet instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public static void ShootBullet(Vector3 sourcePosition, Vector3 targetPosition, Ped owner, WeaponAsset weaponAsset, int damage, float speed = -1f)
        {
            Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS, sourcePosition.X, sourcePosition.Y, sourcePosition.Z, targetPosition.X, targetPosition.Y, targetPosition.Z, damage, 1, weaponAsset.Hash, (owner == null ? 0 : owner.Handle), 1, 0, speed);
        }

        /// <summary>
        /// Creates a 3D raycast between 2 points.
        /// </summary>
        /// <param name="source">The source of the raycast.</param>
        /// <param name="target">The target of the raycast.</param>
        /// <param name="radius">The radius of the raycast.</param>
        /// <param name="options">What type of objects the raycast should intersect with.</param>
        /// <param name="ignoreEntity">Specify an <see cref="Entity"/> that the raycast should ignore, leave null for no entities ignored.</param>
        [Obsolete("World.RaycastCapsule is obsolete because the result may not be made in the same frame you call the method. " +
                  "Use ShapeTest.StartTestCapsule instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public static RaycastResult RaycastCapsule(Vector3 source, Vector3 target, float radius, IntersectFlags options, Entity ignoreEntity = null)
        {
            return new RaycastResult(Function.Call<int>(Hash.START_SHAPE_TEST_CAPSULE,
                source.X,
                source.Y,
                source.Z,
                target.X,
                target.Y,
                target.Z,
                radius,
                (int)options,
                ignoreEntity == null
                    ? 0
                    : ignoreEntity.Handle,
                7));
        }
        /// <summary>
        /// Creates a 3D raycast between 2 points.
        /// </summary>
        /// <param name="source">The source of the raycast.</param>
        /// <param name="direction">The direction of the raycast.</param>
        /// <param name="radius">The radius of the raycast.</param>
        /// <param name="maxDistance">How far the raycast should go out to.</param>
        /// <param name="options">What type of objects the raycast should intersect with.</param>
        /// <param name="ignoreEntity">Specify an <see cref="Entity"/> that the raycast should ignore, leave null for no entities ignored.</param>
        [Obsolete("World.RaycastCapsule is obsolete because the result may not be made in the same frame you call the method. " +
                  "Use ShapeTest.StartTestCapsule instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public static RaycastResult RaycastCapsule(Vector3 source, Vector3 direction, float maxDistance, float radius, IntersectFlags options, Entity ignoreEntity = null)
        {
            Vector3 target = source + direction * maxDistance;

            return new RaycastResult(Function.Call<int>(Hash.START_SHAPE_TEST_CAPSULE,
                source.X,
                source.Y,
                source.Z,
                target.X,
                target.Y,
                target.Z,
                radius,
                (int)options,
                ignoreEntity == null
                    ? 0
                    : ignoreEntity.Handle,
                7));
        }

        /// <summary>
        /// Gets the height of the ground at a given position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The height measured in meters</returns>
        [Obsolete("Use GetGroundHeight(Vector3, out float, GetGroundHeightMode) instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public static float GetGroundHeight(Vector2 position)
        {
            return GetGroundHeight(new Vector3(position.X, position.Y, 1000f));
        }

        /// <summary>
        /// Gets the height of the ground at a given position.
        /// Note : If the Vector3 is already below the ground, this will return 0.
        /// You may want to use the other overloaded function to be safe.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The height measured in meters</returns>
        [Obsolete("Use GetGroundHeight(Vector3, out float, GetGroundHeightMode) instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public static float GetGroundHeight(Vector3 position)
        {
            float resultArg;
            unsafe
            {
                Function.Call(Hash.GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, position.Z, &resultArg, false);
            }
            return resultArg;
        }

        /// <summary>
        /// Gets the nearest safe coordinate to position a <see cref="Ped"/>.
        /// </summary>
        /// <param name="position">The position to check around.</param>
        /// <param name="sidewalk">if set to <see langword="true" /> Only find positions on the sidewalk.</param>
        /// <param name="flags">The flags.</param>
        [Obsolete("World.GetSafeCoordForPed is obsolete since there is no way to check if the method is failed while GET_SAFE_COORD_FOR_PED provides one." +
        "Use GetSafePositionForPed instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public static Vector3 GetSafeCoordForPed(Vector3 position, bool sidewalk = true, int flags = 0)
        {
            NativeVector3 outPos;
            unsafe
            {
                if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, sidewalk, &outPos, flags))
                {
                    return outPos;
                }
            }
            return Vector3.Zero;
        }

        /// <summary>
        /// Gets the straight line distance between 2 positions.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>The distance.</returns>
        [Obsolete("Use Vector3.Distance(Vector3, Vector3) instead.")]
        public static float GetDistance(Vector3 origin, Vector3 destination)
        {
            return Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z, 1);
        }
    }
}
