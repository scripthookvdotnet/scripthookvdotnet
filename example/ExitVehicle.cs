using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;

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
			bool isDriver = Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, vehicle, (int)VehicleSeat.Driver) == player;

			if (IsKeyPressed(Keys.F))
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