using System;
using GTA;

public class ExitVehicle : Script
{
	public ExitVehicle()
	{
		Tick += OnTick;
	}

	DateTime mLastExit;

	void OnTick(object sender, EventArgs e)
	{
		Ped player = Game.Player.Character;

		if (Game.IsControlPressed(2, Control.VehicleExit) && DateTime.Now > this.mLastExit && player.IsInVehicle())
		{
			Wait(250);

			Vehicle vehicle = player.CurrentVehicle;
			bool isDriver = vehicle.GetPedOnSeat(VehicleSeat.Driver) == player;

			if (Game.IsControlPressed(2, Control.VehicleExit))
			{
				player.Task.LeaveVehicle(vehicle, true);
			}
			else
			{
				player.Task.LeaveVehicle(vehicle, false);

				Wait(0);

				if (isDriver)
				{
					vehicle.EngineRunning = true;
				}
			}

			this.mLastExit = DateTime.Now + TimeSpan.FromMilliseconds(2000);
		}
	}
}