using System;
using GTA;

public class IndicatorControl : Script
{
	public IndicatorControl()
	{
		Tick += OnTick;
		Interval = 100;
	}

	readonly bool[] _active = new bool[2];
	readonly DateTime[] _timeLeft = new DateTime[2];

	void OnTick(object sender, EventArgs e)
	{
		Ped playerPed = Game.Player.Character;

		if (playerPed.IsInVehicle())
		{
			Vehicle vehicle = playerPed.CurrentVehicle;

			if (Game.IsControlPressed(Control.VehicleMoveLeftOnly))
			{
				if (vehicle.Speed < 10.0f)
				{
					vehicle.IsLeftIndicatorLightOn = _active[0] = true;
					vehicle.IsRightIndicatorLightOn = _active[1] = false;
					_timeLeft[0] = DateTime.Now + TimeSpan.FromMilliseconds(3000);
				}
			}
			else if (_active[0] && DateTime.Now > _timeLeft[0])
			{
				vehicle.IsLeftIndicatorLightOn = _active[0] = false;
			}

			if (Game.IsControlPressed(Control.VehicleMoveRightOnly))
			{
				if (vehicle.Speed < 10.0f)
				{
					vehicle.IsLeftIndicatorLightOn = _active[0] = false;
					vehicle.IsRightIndicatorLightOn = _active[1] = true;
					_timeLeft[1] = DateTime.Now + TimeSpan.FromMilliseconds(3000);
				}
			}
			else if (_active[1] && DateTime.Now > _timeLeft[1])
			{
				vehicle.IsRightIndicatorLightOn = _active[1] = false;
			}
		}
	}
}