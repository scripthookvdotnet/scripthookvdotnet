using GTA.Math;
using GTA.Native;

namespace GTA
{

    /// <summary>
    /// Represents a scripted incident.
    /// </summary>
    /// <remarks>
    /// The game enforces a maximum of 50 active incidents at any given time.
    /// </remarks>
    public class Incident
    {
        public int Handle { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Incident"/> is valid.
        /// </summary>
        public bool IsValid => Function.Call<bool>(Hash.IS_INCIDENT_VALID, Handle);

        /// <summary>
        /// Sets the ideal distance from this <see cref="Incident"/> to spawn units at.
        /// </summary>
        public float IdealSpawnDistance
        {
            set => Function.Call(Hash.SET_IDEAL_SPAWN_DISTANCE_FOR_INCIDENT, Handle, value);
        }
        private Incident(int handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// Sets how many units of a specific type are requested for this incident.
        /// </summary>
        /// <remarks>
        /// The internal limit per <see cref="DispatchType"/> is 256 units.
        /// </remarks>
        /// <param name="dispatchType">The type of units to dispatch.</param>
        /// <param name="unitCount">The count of units to dispatch.</param>
        public void SetRequestedResources(DispatchType dispatchType, int unitCount)
        {
            Function.Call(Hash.SET_INCIDENT_REQUESTED_UNITS, Handle, (int)dispatchType, unitCount);
        }

        /// <summary>
        /// Creates a new incident at the specified position.
        /// </summary>
        /// <param name="dispatchType">
        /// The type of units to initially dispatch to the incident.
        /// </param>
        /// <param name="position">
        /// The position where units should be dispatched to.
        /// </param>
        /// <param name="unitCount">
        /// The number of units to spawn.  
        /// For <see cref="DispatchType.PoliceAutomobile"/>, <see cref="DispatchType.PoliceAutomobileWaitCrusing"/>, 
        /// or <see cref="DispatchType.PoliceAutomobileWaitPulledOver"/>, this represents the number of peds.    
        /// The maximum supported value is 256.
        /// </param>
        /// <param name="time">
        /// The duration of the incident in ms.  
        /// If set to a negative value, the incident will persist indefinitely until manually cleared.
        /// </param>
        /// <param name="overrideRelGroup">
        /// The relationship group of the spawned units.  
        /// Only applies to <see cref="DispatchType.BikerBackup"/>; otherwise, it is ignored.
        /// </param>
        /// <param name="assassinsLevel">
        /// The skill/stats level of the dispatched units.
        /// </param>
        /// <returns>
        /// The created <see cref="Incident"/> instance, or <c>null</c> if creation failed.
        /// </returns>
        public static Incident Create(DispatchType dispatchType, Vector3 position, int unitCount, float time = -1f, RelationshipGroupHash overrideRelGroup = RelationshipGroupHash.None, AssassinsLevel assassinsLevel = AssassinsLevel.Low)
        {
            int handle;
            bool wasSuccessful;

            unsafe
            {
                wasSuccessful = Function.Call<bool>(Hash.CREATE_INCIDENT, (int)dispatchType, position.X, position.Y, position.Z, unitCount, time, &handle, (uint)overrideRelGroup, (int)assassinsLevel);
            }

            return wasSuccessful ? new Incident(handle) : null;
        }

        /// <summary>
        /// Creates a new incident at the position of a specified entity.
        /// </summary>
        /// <param name="dispatchType">
        /// The type of units to initially dispatch to the incident.
        /// </param>
        /// <param name="entity">
        /// The entity where units should be dispatched to.
        /// </param>
        /// <param name="unitCount">
        /// The number of units to spawn.  
        /// For <see cref="DispatchType.PoliceAutomobile"/>, <see cref="DispatchType.PoliceAutomobileWaitCrusing"/>, 
        /// or <see cref="DispatchType.PoliceAutomobileWaitPulledOver"/>, this represents the number of peds.  
        /// The maximum supported value is 256.
        /// </param>
        /// <param name="time">
        /// The duration of the incident in ms.  
        /// If set to a negative value, the incident will persist indefinitely until manually cleared.
        /// </param>
        /// <param name="overrideRelGroup">
        /// The relationship group of the spawned units.  
        /// Only applies to <see cref="DispatchType.BikerBackup"/>; otherwise, it is ignored.
        /// </param>
        /// <param name="assassinsLevel">
        /// The skill/stats level of the dispatched units.
        /// </param>
        /// <returns>
        /// The created <see cref="Incident"/> instance, or <c>null</c> if creation failed.
        /// </returns>
        public static Incident CreateWithEntity(DispatchType dispatchType, Entity entity, int unitCount, float time = -1f, RelationshipGroupHash overrideRelGroup = RelationshipGroupHash.None, AssassinsLevel assassinsLevel = AssassinsLevel.Low)
        {
            int handle;
            bool wasSuccessful;

            unsafe
            {
                wasSuccessful = Function.Call<bool>(Hash.CREATE_INCIDENT_WITH_ENTITY, (int)dispatchType, entity, unitCount, time, &handle, (uint)overrideRelGroup, (int)assassinsLevel);
            }

            return wasSuccessful ? new Incident(handle) : null;
        }

        /// <summary>
        /// Creates an <see cref="Incident"/> instance from an existing handle.
        /// </summary>
        /// <param name="handle">The handle of the incident.</param>
        /// <returns>
        /// The <see cref="Incident"/> instance, or <c>null</c> if the handle is invalid.
        /// </returns>
        public static Incident FromHandle(int handle)
        {
            bool isValid = Function.Call<bool>(Hash.IS_INCIDENT_VALID, handle);

            return isValid ? new Incident(handle) : null;
        }

        /// <summary>
        /// Deletes this <see cref="Incident"/>.
        /// </summary>
        public void Delete()
        {
            Function.Call(Hash.DELETE_INCIDENT, Handle);
        }
    }
}
