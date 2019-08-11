using System;

namespace GTA
{
	[Flags]
	public enum LeaveVehicleFlags
	{
		None = 0,
		WarpOut = 16,
		LeaveDoorOpen = 256,
		BailOut = 4096,
	}
}
