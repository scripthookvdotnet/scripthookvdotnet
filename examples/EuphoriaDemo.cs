using System;
using GTA;

public class EuphoriaDemo : Script
{
    public EuphoriaDemo()
    {
        KeyDown += EuphoriaDemo_KeyDown;
    }

    private void EuphoriaDemo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        if (e.KeyCode == System.Windows.Forms.Keys.J)
        {
            //Make sure all arguments have their default value
            Game.PlayerPed.Euphoria.ArmsWindmillAdaptive.ResetArguments();

            //Set all the arguments used for the Message
            //Intellisense with give a list of available arguments
            Game.PlayerPed.Euphoria.ArmsWindmillAdaptive.AngSpeed = 10f;
            Game.PlayerPed.Euphoria.ArmsWindmillAdaptive.BodyStiffness = 0f;
            Game.PlayerPed.Euphoria.ArmsWindmillAdaptive.Amplitude = 2f;
            Game.PlayerPed.Euphoria.ArmsWindmillAdaptive.LeftElbowAngle = 6f;
            Game.PlayerPed.Euphoria.ArmsWindmillAdaptive.RightElbowAngle = 6f;
            Game.PlayerPed.Euphoria.ArmsWindmillAdaptive.DisableOnImpact = false;
            Game.PlayerPed.Euphoria.ArmsWindmillAdaptive.BendLeftElbow = true;
            Game.PlayerPed.Euphoria.ArmsWindmillAdaptive.BendRightElbow = true;

            //Start the WindMill message and run for 5 seconds
            Game.PlayerPed.Euphoria.ArmsWindmillAdaptive.Start(5000);


            //start body balance so ped doesnt fall over
            //as we already have a message running, we dont need to put a time in this message
            Game.PlayerPed.Euphoria.BodyBalance.Start();

        }
        else if (e.KeyCode == System.Windows.Forms.Keys.K)
        {
            //Make sure all arguments have their default value
            Game.PlayerPed.Euphoria.ApplyImpulse.ResetArguments();

            //Set the Magnitude and Direction of the Impulse in world coords
            //In this example its just the direction the ped is facing
            Game.PlayerPed.Euphoria.ApplyImpulse.Impulse = Game.PlayerPed.ForwardVector * 1000;

            //Start the ApplyImpulse message and run for 2 seconds
            Game.PlayerPed.Euphoria.ApplyImpulse.Start(2000);

            //No need for the BodyBalance Helper on this, The ped would fall over anyway

        }else if (e.KeyCode == System.Windows.Forms.Keys.L)
        {
            Game.PlayerPed.Euphoria.ShotFallToKnees.ResetArguments();

            Game.PlayerPed.Euphoria.ShotFallToKnees.FallToKnees = true;
        }
    }
}