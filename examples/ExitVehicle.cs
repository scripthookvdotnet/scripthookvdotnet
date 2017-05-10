using System;
using GTA;
using GTA.Native;

public class ExitVehicle : Script
{
	public ExitVehicle()
	{
		Tick += OnTick;
	}

	DateTime _lastExit;

	void OnTick(object sender, EventArgs e)
	{
		Ped playerPed = Game.Player.Character;

		if (Game.IsControlPressed(Control.VehicleExit) && DateTime.Now > _lastExit && playerPed.IsInVehicle())
		{
			Wait(250);

			Vehicle vehicle = playerPed.CurrentVehicle;
			bool isPlayerTheDriver = vehicle.GetPedOnSeat(VehicleSeat.Driver) == playerPed;

			if (Game.IsControlPressed(Control.VehicleExit))
			{
				playerPed.Task.LeaveVehicle(vehicle, true);
			}
			else
			{
				playerPed.Task.LeaveVehicle(vehicle, false);

				Wait(0);

				if (isPlayerTheDriver)
				{
					vehicle.IsEngineRunning = true;
				}
			}

			_lastExit = DateTime.Now + TimeSpan.FromMilliseconds(2000);
		}
	}
}