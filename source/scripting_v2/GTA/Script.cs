//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.IO;
using System.Windows.Forms;

namespace GTA
{
	public abstract class Script : IDisposable
	{
		Viewport _viewport;
		ScriptSettings _settings;

		public Script()
		{
			Filename = SHVDN.ScriptDomain.CurrentDomain.LookupScriptFilename(GetType());
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
		}

		public static void Wait(int ms)
		{
			SHVDN.Script script = SHVDN.ScriptDomain.ExecutingScript;

			if (ReferenceEquals(script, null) || !script.IsRunning)
				throw new InvalidOperationException("Illegal call to 'Script.Wait()' outside main loop!");

			script.Wait(ms);
		}
		public static void Yield()
		{
			Wait(0);
		}

		public event EventHandler Tick
		{
			add { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.Tick += value; }
			remove { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.Tick -= value; }
		}
		public event EventHandler Aborted
		{
			add { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.Aborted += value; }
			remove { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.Aborted -= value; }
		}

		public event KeyEventHandler KeyUp
		{
			add { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.KeyUp += value; }
			remove { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.KeyUp -= value; }
		}
		public event KeyEventHandler KeyDown
		{
			add { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.KeyDown += value; }
			remove { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.KeyDown -= value; }
		}

		public Keys ActivateKey = Keys.NumPad5;
		public Keys BackKey = Keys.NumPad0;
		public Keys LeftKey = Keys.NumPad4;
		public Keys RightKey = Keys.NumPad6;
		public Keys UpKey = Keys.NumPad8;
		public Keys DownKey = Keys.NumPad2;

		public string Name => GetType().FullName;
		public string Filename { get; private set; }

		public Viewport View
		{
			get
			{
				if (_viewport == null)
				{
					_viewport = new Viewport();

					Tick += (object sender, EventArgs e) => {
						_viewport.Draw();
					};
					KeyUp += (object sender, KeyEventArgs e) => {
						if (e.KeyCode == ActivateKey) _viewport.HandleActivate();
						else if (e.KeyCode == BackKey) _viewport.HandleBack();
						else if (e.KeyCode == LeftKey) _viewport.HandleChangeItem(false);
						else if (e.KeyCode == RightKey) _viewport.HandleChangeItem(true);
						else if (e.KeyCode == UpKey) _viewport.HandleChangeSelection(false);
						else if (e.KeyCode == DownKey) _viewport.HandleChangeSelection(true);
					};
				}

				return _viewport;
			}
		}
		public ScriptSettings Settings
		{
			get
			{
				if (_settings == null)
				{
					string path = Path.ChangeExtension(Filename, ".ini");

					_settings = ScriptSettings.Load(path);
				}

				return _settings;
			}
		}

		public void Abort()
		{
			SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).Abort();
		}

		protected int Interval
		{
			get => SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).Interval;
			set
			{
				if (value < 0)
					value = 0;
				var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (s != null)
					s.Interval = value;
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
