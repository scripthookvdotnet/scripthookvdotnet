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
		Ped player = Game.Player.Character;

		if (player.IsInVehicle())
		{
			Vehicle vehicle = player.CurrentVehicle;

			if (Game.IsControlPressed(2, Control.VehicleMoveLeftOnly))
			{
				if (vehicle.Speed < 10.0f)
				{
					vehicle.LeftIndicatorLightOn = _active[0] = true;
					vehicle.RightIndicatorLightOn = _active[1] = false;
					_timeLeft[0] = DateTime.Now + TimeSpan.FromMilliseconds(3000);
				}
			}
			else if (_active[0] && DateTime.Now > _timeLeft[0])
			{
				vehicle.LeftIndicatorLightOn = _active[0] = false;
			}

			if (Game.IsControlPressed(2, Control.VehicleMoveRightOnly))
			{
				if (vehicle.Speed < 10.0f)
				{
					vehicle.LeftIndicatorLightOn = _active[0] = false;
					vehicle.RightIndicatorLightOn = _active[1] = true;
					_timeLeft[1] = DateTime.Now + TimeSpan.FromMilliseconds(3000);
				}
			}
			else if (_active[1] && DateTime.Now > _timeLeft[1])
			{
				vehicle.RightIndicatorLightOn = _active[1] = false;
			}
		}
	}
}