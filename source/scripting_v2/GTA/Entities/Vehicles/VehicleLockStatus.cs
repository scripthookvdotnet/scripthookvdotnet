namespace GTA
{
	public enum VehicleLockStatus
	{
		None = 0,
		Unlocked = 1,
		Locked = 2,
		LockedForPlayer = 3,
		///<summary>Doesn't allow players to exit the vehicle with the exit vehicle key.</summary>
		StickPlayerInside = 4,
		///<summary>Can be broken into the car. if the glass is broken, the value will be set to 1.</summary>
		CanBeBrokenInto = 7,
		CanBeBrokenIntoPersist = 8,
		CannotBeTriedToEnter = 10
	}
}
