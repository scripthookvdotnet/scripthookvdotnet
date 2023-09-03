using GTA.Math;
using GTA.Native;

namespace GTA
{
	/// <summary>
	/// Represents a facade class of general resource streaming stuff,
	/// which indirectly access various global variables for streaming.
	/// </summary>
	public static class Streaming
	{
		/// <summary>
		/// Synchronously loads a location (could be inside an interior, or not).
		/// </summary>
		/// <remarks>
		/// Since this method blocks the game until the location is loaded,
		/// the script may be terminated for timeout at the tick this method is executed.
		/// </remarks>
		public static void LoadScene(Vector3 position) =>
			Function.Call(Hash.LOAD_SCENE, position.X, position.Y, position.Z);

		/// <summary>
		/// Sets the value that indicates whether the game will stream resources.
		/// </summary>
		public static bool IsEnabled
		{
			set => Function.Call(Hash.SET_STREAMING, value);
		}

		/// <summary>
		/// Tell streaming to request collision about point, having game stream extra collision and IPL/YMAP files around
		/// this coordinate. This function needs called every frame the additional data is needed.
		/// </summary>
		/// <param name="point">The coordinate point to stream extra collision and map resources.</param>
		/// <remarks>
		/// Calling this method does not result in loaded collisions for weapon tests for bullets and projectiles
		/// around the specified point.
		/// </remarks>
		public static void RequestCollisionAt(Vector3 point)
			=> Function.Call(Hash.REQUEST_COLLISION_AT_COORD, point.X, point.Y, point.Z);

		/*
		 * There's no RequestAdditionalCollisionAt, because REQUEST_ADDITIONAL_COLLISION_AT_COORD uses the same native
		 * handler as REQUEST_COLLISION_AT_COORD and both behaves exactly the same
		 */

		/// <summary>
		/// Overrides the game focus and sets it to specified position and velocity
		/// The game focus is used for loading map data, collisions, object population etc.
		/// This method overrides it for the specified position and velocity.
		/// </summary>
		/// <param name="position">The position in world space.</param>
		/// <param name="velocity">
		/// The velocity to use for <see cref="Ped"/> &amp; <see cref="Vehicle"/> population in meters.
		/// </param>
		public static void SetFocusPositionAndVelocity(Vector3 position, Vector3 velocity)
			=> Function.Call(Hash.SET_FOCUS_POS_AND_VEL, position.X, position.Y, position.Z, velocity.X, velocity.Y,
				velocity.Z);
		/// <summary>
		/// <para>
		/// Sets the specified entity as the focus for streaming.
		/// </para>
		/// <para>
		/// The game focus is used for loading map data, collisions, object population etc.
		/// This command overrides it for the specified entity.
		/// </para>
		/// </summary>
		/// <remarks>
		/// Does not change anything on streaming focus if an invalid <see cref="Entity"/> is passed.
		/// </remarks>
		public static Entity FocusEntity
		{
			set => Function.Call(Hash.SET_FOCUS_ENTITY, value);
		}
		/// <summary>
		/// Returns <see langowrd="true"/> if the specified <see cref="Entity"/> is the current focus.
		/// </summary>
		public static bool IsEntityFocus(Entity entity) => Function.Call<bool>(Hash.IS_ENTITY_FOCUS, entity);
		/// <summary>
		/// <para>
		/// Clears the overridden game focus and sets it to the player ped again (default).
		/// </para>
		/// <para>
		/// The game focus is used for loading map data, collisions, object population etc.
		/// If this has been overridden by script, it is important to clear it afterwards.
		/// </para>
		/// </summary>
		// CLEAR_FOCUS changes the internal focus type to zero (the default value) without clearing
		// the focus entity address or the focus coordinate and direction velocity (hence "Overridden")
		public static void ClearOverriddenFocus() => Function.Call(Hash.CLEAR_FOCUS);

		/// <summary>
		/// Asynchronously loads a location (could be inside an interior, or not).
		/// Starts a new frustum load scene, which is interior-aware and uses a streaming volume.
		/// </summary>
		/// <param name="position">The position to load around.</param>
		/// <param name="direction">
		/// The direction to stream for <see cref="Ped"/>s and <see cref="Vehicle"/>s
		/// (probably in meters).
		/// </param>
		/// <param name="farClip">The far clip.</param>
		/// <param name="controlFlags">The control flags.</param>
		/// <returns><see langword="true"/> if load scene has started successfully; otherwise, <see langword="false"/>.</returns>
		/// <remarks>You cannot use a new load scene during a player switch.</remarks>
		public static bool StartNewFrustumLoadScene(Vector3 position, Vector3 direction, float farClip,
			NewLoadSceneFlags controlFlags = 0)
			=> Function.Call<bool>(Hash.NEW_LOAD_SCENE_START, position.X, position.Y, position.Z, direction.X,
				direction.Y, direction.Z, farClip, (int)controlFlags);
		/// <summary>
		/// Asynchronously loads a location (could be inside an interior, or not).
		/// Starts a new spherical load scene, which is interior-aware and uses a streaming volume.
		/// </summary>
		/// <param name="position">The position to load around.</param>
		/// <param name="radius">The radius for the load scene in meters.</param>
		/// <param name="controlFlags">The control flags.</param>
		/// <returns><see langword="true"/> if load scene has started successfully; otherwise, <see langword="false"/>.</returns>
		/// <remarks>You cannot use a new load scene during a player switch.</remarks>
		public static bool StartNewSphereLoadScene(Vector3 position, float radius, NewLoadSceneFlags controlFlags = 0)
			=> Function.Call<bool>(Hash.NEW_LOAD_SCENE_START_SPHERE, position.X, position.Y, position.Z, radius, (int)controlFlags);
		/// <summary>
		/// stops the new load scene, if it is active.
		/// </summary>
		public static void StopNewLoadScene() => Function.Call(Hash.NEW_LOAD_SCENE_STOP);
		/// <summary>
		/// Gets the value that indicates whether a new load scene is currently running.
		/// </summary>
		/// <returns><see langword="true"/> if if a new load scene is active; otherwise, <see langword="false"/>.</returns>
		public static bool IsNewLoadSceneActive => Function.Call<bool>(Hash.IS_NEW_LOAD_SCENE_ACTIVE);
		/// <summary>
		/// Gets the value that indicates whether a new load scene is fully loaded.
		/// </summary>
		/// <returns><see langword="true"/> if if the new load scene is active and fully loaded; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// A new load scene is never guaranteed to eventually return <see langword="true"/> if memory is under heavy load.
		/// Therefore, you might want to set up a custom timeout so you can do some alternative actions
		/// if new load scene is taking too long time to load a location.
		/// </remarks>
		public static bool IsNewLoadSceneLoaded => Function.Call<bool>(Hash.IS_NEW_LOAD_SCENE_LOADED);

		/// <summary>
		/// Sets the population budget allocated for spawning ambient <see cref="Ped"/>s.
		/// </summary>
		/// <param name="amount">The budget amount to allocate from 0 to 3, with 0 being none and 3 being normal.</param>
		public static void SetPedPopulationBudget(int amount) => Function.Call(Hash.SET_PED_POPULATION_BUDGET, amount);
		/// <summary>
		/// Sets the population budget allocated for spawning ambient <see cref="Vehicle"/>s.
		/// </summary>
		/// <param name="amount">The budget amount to allocate from 0 to 3, with 0 being none and 3 being normal.</param>
		public static void SetVehiclePopulationBudget(int amount) => Function.Call(Hash.SET_VEHICLE_POPULATION_BUDGET, amount);
	}
}
