using GTA.Math;
using GTA.Native;

namespace GTA
{
	public static class Fire
	{
		/// <summary>
		/// Starts a fire in the world
		/// </summary>
		/// <param name="position">Location of fire</param>
		/// <param name="maxChildren">The max amount of times a fire can spread to other objects.Must be 25 or less, or the function will do nothing.</param>
		/// <param name="isGasFire">Whether or not the fire is powered by gasoline.</param>
		/// <returns>handle ID of the fire. (It can be used - for example - with FIRE::REMOVE_SCRIPT_FIRE in order to extinguish the fire)</returns>
		public static Hash StartScriptFire(Vector3 position, int maxChildren, bool isGasFire)
		{
			return Function.Call<Hash>(Hash.START_SCRIPT_FIRE, position.X, position.Y, position.Z, maxChildren, isGasFire);
		}

		/// <summary>
		/// Checks whether or not an entity is currently on fire
		/// </summary>
		/// <param name="entity">The entity to check</param>
		/// <returns>thre if the entity is on fire and false is the entity is not on fire</returns>
		public static bool IsEntityOnFire(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_ON_FIRE, entity); 
		}

		/// <summary>
		/// Sets an entity on fire. It seems to work only on pedestrians.
		/// </summary>
		/// <param name="entity">Entity (in this case, a ped) that will be set on fire.</param>
		/// <returns>The entity that has been set on fire</returns>
		public static Entity StartEntityFire(Ped entity)
		{
			return Function.Call<Ped>(Hash.START_ENTITY_FIRE, entity);
		}

		/// <summary>
		/// Removes an existing fire
		/// </summary>
		/// <param name="fireHandle">The handle of the fire to remove</param>
		public static void RemoveScriptFire(int fireHandle)
		{
			Function.Call(Hash.REMOVE_SCRIPT_FIRE, fireHandle);
		}

		/// <summary>
		/// Removes an existing fire
		/// </summary>
		/// <param name="fireHandle">The handle of the fire to remove</param>
		public static void RemoveScriptFire(Hash fireHandle)
		{
			Function.Call(Hash.REMOVE_SCRIPT_FIRE, fireHandle);
		}

		/// <summary>
		/// Stops the fire set on a ped through StartEntityFire
		/// </summary>
		/// <param name="entity">The entity with the fire to be stopped</param>
		public static void StopEntityFire(Ped entity)
		{
			Function.Call(Hash.STOP_ENTITY_FIRE, entity);
		}

		/// <summary>
		/// Gets the number of fires located within a radius from a location
		/// </summary>
		/// <param name="location">The center location to find fires</param>
		/// <param name="radius">The radius from the location to find fires</param>
		/// <returns>The number of fires found in the range</returns>
		public static int GetNumberOfFiresInRange(Vector3 location, float radius)
		{
			return Function.Call<int>(Hash.GET_NUMBER_OF_FIRES_IN_RANGE, location.X, location.Y, location.Z, radius);
		}

		/// <summary>
		/// Set a vehicle's on-fire status
		/// </summary>
		/// <param name="vehicle">The vehicle to affect</param>
		/// <param name="isOnFire">true if the vehicle is to be on fire and false if the vehicle should not be on fire</param>
		public static void SetVehiclePetrolFire(Vehicle vehicle, bool isOnFire)
		{
			Function.Call(Hash.SET_DISABLE_VEHICLE_PETROL_TANK_FIRES, vehicle, isOnFire);
		}
	}
}
