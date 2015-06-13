using System;
using System.Windows.Forms;
using GTA;

public class IndicatorControl : Script
{
	public IndicatorControl()
	{
		Tick += OnTick;
		Interval = 100;
	}

	private bool mLeftActive = false, mRightActive = false;
	private DateTime mTimeLeftSwitchOff, mTimeRightSwitchOff;

	void OnTick(object sender, EventArgs e)
	{
		Ped player = Game.Player.Character;

		if (player.IsInVehicle())
		{
			Vehicle vehicle = player.CurrentVehicle;

			if (Game.IsControlPressed(2, GTA.Control.INPUT_VEH_MOVE_LEFT_ONLY))
			{
				if (vehicle.Speed < 10.0f)
				{
					vehicle.LeftIndicatorLightOn = this.mLeftActive = true;
					vehicle.RightIndicatorLightOn = this.mRightActive = false;
					this.mTimeLeftSwitchOff = DateTime.Now + TimeSpan.FromMilliseconds(3000);
				}
			}
			else if (this.mLeftActive && DateTime.Now > mTimeLeftSwitchOff)
			{
				vehicle.LeftIndicatorLightOn = this.mLeftActive = false;
			}

			if (Game.IsControlPressed(2, GTA.Control.INPUT_VEH_MOVE_RIGHT_ONLY))
			{
				if (vehicle.Speed < 10.0f)
				{
					vehicle.LeftIndicatorLightOn = this.mLeftActive = false;
					vehicle.RightIndicatorLightOn = this.mRightActive = true;
					this.mTimeRightSwitchOff = DateTime.Now + TimeSpan.FromMilliseconds(3000);
				}
			}
			else if (this.mRightActive && DateTime.Now > mTimeRightSwitchOff)
			{
				vehicle.RightIndicatorLightOn = this.mRightActive = false;
			}
		}
	}
}