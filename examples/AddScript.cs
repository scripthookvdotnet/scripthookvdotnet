using System;
using System.Windows.Forms;
using GTA;

namespace ScriptInstance
{
    // Main Script is AutoStarted and using key press creates AI Scripts.
    // T Key to Spawn AIone
    // Y Key to Spawn AItwo
    // G Key to change AIone Animation 
    // H Key to SetWait(6) for AItwo
    // J Key to Pause AIone

    public class Main : Script
    {
        AI AIone = null;
        AI AItwo = null;

        public Main()
        {
            Tick += OnTick;

            KeyDown += OnKeyDown;

            Interval = 1000;
        }

        private void OnTick(object sender, EventArgs e) { }

        private void SpawnAIone()
        {
            if (AIone == null)
            {
                AIone = (AI)AddScript(typeof(AI));

                if (AIone != null)
                {
                    GTA.UI.Notification.Show("SpawnAI: Ped(1);");
                }
            }
            else
            {
                AIone.Abort();

                AIone = null; // Clear Held instance to create a new script;

                GTA.UI.Notification.Show("SpawnAI: Ped(1).Abort();");
            }
        }

        private void SpawnAItwo()
        {
            if (AItwo == null || !AItwo.IsRunning)
            {
                AItwo = (AI)AddScript(typeof(AI));

                if (AItwo != null)
                {
                    GTA.UI.Notification.Show("SpawnAI: Ped(2);");
                }
            }
            else
            {
                AItwo.Abort();

                //AItwo = null; Instead of clearing this instance i added a check to see if the old instance was runnning.

                GTA.UI.Notification.Show("SpawnAI: Ped(2).Abort();");
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (KeyPress(Keys.T, e)) // Create AI Script and store as AIone
            {
                SpawnAIone();
            }
            else if (KeyPress(Keys.Y, e)) // Create AI Script and store as AItwo
            {
                SpawnAItwo();
            }
            else if (KeyPress(Keys.G, e)) // Changes AIone animation
            {
                if (AIone != null)
                {
                    if (AIone.animation == "Jump")
                    {
                        AIone.animation = "HandsUp";
                    }
                    else AIone.animation = "Jump";

                    GTA.UI.Notification.Show("SpawnAI: Animation(" + AIone.animation + ");");
                }
            }
            else if (KeyPress(Keys.H, e)) // Sets Wait() for AItwo
            {
                AItwo.SetWait(6000);
            }
            else if (KeyPress(Keys.J, e)) // Toggles Pause() for AIone
            {
                if (AIone.IsPaused) AIone.Start();
                else AIone.Pause();
            }
        }

        private bool KeyPress(Keys key, KeyEventArgs e = null)
        {
            if (e != null && e.KeyCode == key)
            {
                e.Handled = true;

                return true;
            }

            return false;
        }
    }

    [AutoStart(false)]
    public class AI : Script
    {
        public AI()
        {
            Aborted += Shutdown;

            Tick += OnTick;

            Interval = 3000;
        }

        private Ped ped = null;
        public string animation = "HandsUp";

        private void OnTick(object sender, EventArgs e)
        {
            if (ped == null || !ped.Exists())
            {
                Model model = new Model(PedHash.Stripper01Cutscene);

                ped = World.CreatePed(model, Game.Player.Character.Position + (GTA.Math.Vector3.RelativeFront * 3));
            }

            if (ped != null && ped.IsAlive) // OnTick Repeats animation if Alive.
            {
                if (animation == "HandsUp")
                {
                    ped.Task.HandsUp(1000);
                }
                else if (animation == "Jump")
                {
                    ped.Task.Jump();

                    //Abort();
                }
            }
        }

        private void Shutdown(object sender, EventArgs e)
        {
            if (ped != null) // On Abort() clears the ingame Ped
            {
                ped.MarkAsNoLongerNeeded();

                ped.Delete();
            }
        }
    }

    public class KeyFilter : Script
    {
        public KeyFilter()
        {
            KeyDown += OnKey;
        }

        private void OnKey(object sender, KeyEventArgs e)
        {
            Yield();

            if (e != null && !e.Handled)
            {
                e.Handled = true;
            }
        }
    }
}