using System;
using System.Windows.Forms;
using GTA;

public class ExitVehicle : Script
{
	public ExitVehicle()
	{
		KeyDown += OnKeyDown;
	}

	DateTime mLastExit;

	void OnKeyDown(object sender, KeyEventArgs e)
	{
		Ped player = Game.Player.Character;

		if (e.KeyCode == Keys.F && DateTime.Now > this.mLastExit && player.IsInVehicle())
		{
			Wait(250);

			Vehicle vehicle = player.CurrentVehicle;
            bool isDriver = vehicle.GetPedOnSeat(VehicleSeat.Driver) == player;

			if (Game.IsKeyPressed(Keys.F))
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