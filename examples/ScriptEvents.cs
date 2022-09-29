using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTA.examples
{
	/// <summary>
	/// Script Attribute set NoDefaultInstance = true to not start automatically.
	/// Use Script.InstantiateScript<ScriptEvents>(); to start it.
	/// </summary>
	[ScriptAttributes(Author = "Konijima", NoDefaultInstance = true)]
	public class ScriptEvents : Script
	{
		public ScriptEvents()
		{
			Init += ScriptEvents_Init;
			Started += ScriptEvents_Started;
			Tick += ScriptEvents_Tick;
			KeyDown += ScriptEvents_KeyDown;
			KeyUp += ScriptEvents_KeyUp;
			Aborted += ScriptEvents_Aborted;
		}

		private void ScriptEvents_Init(object sender, EventArgs e)
		{
			Log("We are in the loading screen and the first tick is about to be called.");
		}

		private void ScriptEvents_Started(object sender, EventArgs e)
		{
			Log("We just left the loading screen and the game is ready.");
			UI.Notification.Show("Game is ready!");
		}

		private void ScriptEvents_Tick(object sender, EventArgs e)
		{
			if (IsStarted)
			{
				Log("We are ticking in the game world!");
			}
			else
			{
				Log("We are ticking in the loading screen!");
			}

		}

		private void ScriptEvents_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == System.Windows.Forms.Keys.E)
			{
				Log(LogLevel.Info, "We pressed E key!");
			}
		}

		private void ScriptEvents_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == System.Windows.Forms.Keys.E)
			{
				Log(LogLevel.Info, "We released E key!");
			}
		}

		private void ScriptEvents_Aborted(object sender, EventArgs e)
		{
			Log(LogLevel.Warning, "Script is aborting, time to clean up our mess, if any!");
		}
	}
}
