using System;
using System.Windows.Forms;
using GTA;

public class ExitVehicle : Script
{
	public ExitVehicle()
	{
		Tick += OnTick;
	}

	const float _HOLD_THRESHOLD = 0.25f;
	float holdTime = 0;

	void OnTick(object sender, EventArgs e)
	{
		Ped player = Game.Player.Character;

		if (player.IsAlive && player.IsInVehicle())
		{
			if (Game.IsControlPressed(2, GTA.Control.INPUT_VEH_EXIT))
			{
				holdTime += Game.LastFrameTime;
			}
			else if (holdTime > 0)
			{
				Vehicle vehicle = player.CurrentVehicle;
				if (holdTime >= _HOLD_THRESHOLD)
				{
					// Exit vehicle, turn off engine
					player.Task.LeaveVehicle(vehicle, true);
				}
				else
				{
					// Exit vehicle, leave engine running (if player is driver)
					player.Task.LeaveVehicle(vehicle, false);
					bool isDriver = vehicle.GetPedOnSeat(VehicleSeat.Driver) == player;
					if (isDriver)
					{
						vehicle.EngineRunning = true;
					}
				}
			}
		}
		else
		{
			holdTime = 0;
		}
	}
}
