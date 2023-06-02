using System;
using System.Windows.Forms;
using GTA;

namespace ScriptInstance
{
	// Main script is auto-started and creates AI scripts using key presses.
	// T key to spawn AIone
	// Y key to spawn AItwo
	// G key to change AIone animation 
	// H key to SetWait(6) for AItwo
	// J key to pause AIone

	public class Main : Script
	{
		private AI AIone = null;
		private AI AItwo = null;

		public Main()
		{
			KeyDown += OnKeyDown;

			Interval = 1000;
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.T) // Create AI Script and store as AIone
			{
				SpawnAIone();
			}
			else if (e.KeyCode == Keys.Y) // Create AI Script and store as AItwo
			{
				SpawnAItwo();
			}
			else if (e.KeyCode == Keys.G) // Changes AIone animation
			{
				if (AIone != null)
				{
					if (AIone.animation == "Jump")
					{
						AIone.animation = "HandsUp";
					}
					else
					{
						AIone.animation = "Jump";
					}

					GTA.UI.Notification.Show("SpawnAI: Animation(" + AIone.animation + ");");
				}
			}
			else if (e.KeyCode == Keys.H) // Sets Wait() for AItwo
			{
				AItwo.SetWait(6000);
			}
			else if (e.KeyCode == Keys.J) // Toggles Pause() for AIone
			{
				if (AIone.IsPaused)
				{
					AIone.Resume();
				}
				else
				{
					AIone.Pause();
				}
			}
		}

		private void SpawnAIone()
		{
			if (AIone == null)
			{
				AIone = InstantiateScript<AI>();

				if (AIone != null)
				{
					GTA.UI.Notification.Show("SpawnAI: Ped(1);");
				}
			}
			else
			{
				AIone.Abort();

				AIone = null; // Clear instance to create a new script next time

				GTA.UI.Notification.Show("SpawnAI: Ped(1).Abort();");
			}
		}

		private void SpawnAItwo()
		{
			if (AItwo == null || !AItwo.IsRunning)
			{
				AItwo = InstantiateScript<AI>();

				if (AItwo != null)
				{
					GTA.UI.Notification.Show("SpawnAI: Ped(2);");
				}
			}
			else
			{
				AItwo.Abort();

				// Instead of setting AItwo to null, can also checking status with 'IsRunning'

				GTA.UI.Notification.Show("SpawnAI: Ped(2).Abort();");
			}
		}
	}

	[ScriptAttributes(NoDefaultInstance = true)]
	public class AI : Script
	{
		public AI()
		{
			Tick += OnTick;
			Aborted += OnShutdown;

			Interval = 3000;
		}

		private Ped ped = null;
		private int wait = -1;
		public string animation = "HandsUp";

		public void SetWait(int ms)
		{
			if (ms > wait)
			{
				wait = ms;
			}
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (wait > -1)
			{
				Wait(wait);
				wait = -1;
			}

			if (ped == null)
			{
				ped = World.CreatePed(PedHash.Beach01AMY, Game.LocalPlayerPed.Position + (GTA.Math.Vector3.RelativeFront * 3));
			}

			// Repeat animation if alive
			if (ped != null && ped.IsAlive)
			{
				if (animation == "HandsUp")
				{
					ped.Task.HandsUp(1000);
				}
				else if (animation == "Jump")
				{
					ped.Task.Jump();
				}
			}
		}

		private void OnShutdown(object sender, EventArgs e)
		{
			// Clear pedestrian on script abort
			ped?.Delete();
		}
	}
}
