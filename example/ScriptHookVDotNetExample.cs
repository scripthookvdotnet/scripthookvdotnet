using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;

namespace ScriptHookVDotNetExample
{
    public class MainScript : Script
    {
        public MainScript()
        {
            Tick += OnTick; // Main loop event, called every few milliseconds specified via the Interval property.
            KeyUp += OnKeyUp; // Called when a key or mouse button is released.
            KeyDown += OnKeyDown; // Called when a key or mouse button is pressed.

            Interval = 10; // Tick interval in milliseconds. Set to zero to run as fast as possible.
        }

        bool mIndicatorLeft = false;
        bool mIndicatorRight = false;

        void OnTick(object sender, EventArgs e)
        {
            // Calling native functions:
            // - No return type: GTA.Native.Function.Call(GTA.Native.Hash.SET_MAX_WANTED_LEVEL, 0);
            // - With return type: int id = GTA.Native.Function.Call<int>(GTA.Native.Hash.PLAYER_PED_ID);
        }
        void OnKeyUp(object sender, KeyEventArgs e)
        {

        }
        void OnKeyDown(object sender, KeyEventArgs e)
        {
            Ped player = Game.Player.Character;

            if (player.IsInVehicle())
            {
                Vehicle vehicle = player.CurrentVehicle;

                switch (e.KeyCode)
                {
                    case Keys.Q:
                        vehicle.LeftIndicatorLightOn = mIndicatorLeft = !mIndicatorLeft;
                        break;
                    case Keys.E:
                        vehicle.RightIndicatorLightOn = mIndicatorRight = !mIndicatorRight;
                        break;
                }
            }
        }
    }
}
