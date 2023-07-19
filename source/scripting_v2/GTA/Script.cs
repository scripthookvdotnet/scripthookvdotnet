//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.IO;
using System.Windows.Forms;

namespace GTA
{
	public abstract class Script : IDisposable
	{
		#region Fields

		private Viewport _viewport;
		private ScriptSettings _settings;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Script"/> class.
		/// This constructor is called from the script domain of ScriptHookVDotNet and is
		/// not intended to be used directly from your code.
		/// </summary>
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

		/// <summary>
		/// An event that is raised every tick of the script.
		/// Put code that needs to be looped each frame in here.
		/// </summary>
		public event EventHandler Tick
		{
			add
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.Tick += value;
				}
			}
			remove
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.Tick -= value;
				}
			}
		}
		/// <summary>
		/// An event that is raised when this <see cref="Script"/> gets aborted for any reason.
		/// This should be used for cleaning up anything created during this <see cref="Script"/>.
		/// </summary>
		public event EventHandler Aborted
		{
			add
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.Aborted += value;
				}
			}
			remove
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.Aborted -= value;
				}
			}
		}

		/// <summary>
		/// An event that is raised when a key is lifted.
		/// The <see cref="System.Windows.Forms.KeyEventArgs"/> contains the key that was lifted.
		/// </summary>
		public event KeyEventHandler KeyUp
		{
			add
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.KeyUp += value;
				}
			}
			remove
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.KeyUp -= value;
				}
			}
		}
		/// <summary>
		/// An event that is raised when a key is first pressed or being pressed for more than about half a second.
		/// The <see cref="System.Windows.Forms.KeyEventArgs"/> contains the key that was pressed.
		/// </summary>
		public event KeyEventHandler KeyDown
		{
			add
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.KeyDown += value;
				}
			}
			remove
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.KeyDown -= value;
				}
			}
		}

		/// <summary>
		/// Gets the full name of this <see cref="Script"/> type.
		/// </summary>
		public string Name
		{
			get => GetType().FullName;
		}
		/// <summary>
		/// Gets the filename of this <see cref="Script"/>.
		/// </summary>
		public string Filename
		{
			get; private set;
		}

		public Keys ActivateKey = Keys.NumPad5;
		public Keys BackKey = Keys.NumPad0;
		public Keys LeftKey = Keys.NumPad4;
		public Keys RightKey = Keys.NumPad6;
		public Keys UpKey = Keys.NumPad8;
		public Keys DownKey = Keys.NumPad2;

		public Viewport View
		{
			get
			{
				if (_viewport != null)
				{
					return _viewport;
				}

				_viewport = new Viewport();

				Tick += (sender, e) =>
				{
					_viewport.Draw();
				};
				KeyUp += (sender, e) =>
				{
					if (e.KeyCode == ActivateKey)
					{
						_viewport.HandleActivate();
					}
					else if (e.KeyCode == BackKey)
					{
						_viewport.HandleBack();
					}
					else if (e.KeyCode == LeftKey)
					{
						_viewport.HandleChangeItem(false);
					}
					else if (e.KeyCode == RightKey)
					{
						_viewport.HandleChangeItem(true);
					}
					else if (e.KeyCode == UpKey)
					{
						_viewport.HandleChangeSelection(false);
					}
					else if (e.KeyCode == DownKey)
					{
						_viewport.HandleChangeSelection(true);
					}
				};

				return _viewport;
			}
		}
		/// <summary>
		/// Gets an INI file associated with this <see cref="Script"/>.
		/// The file will be in the same location as this <see cref="Script"/> but with an extension of ".ini".
		/// Use this to save and load settings for this <see cref="Script"/>.
		/// </summary>
		public ScriptSettings Settings
		{
			get
			{
				if (_settings != null)
				{
					return _settings;
				}

				string path = Path.ChangeExtension(Filename, ".ini");

				_settings = ScriptSettings.Load(path);

				return _settings;
			}
		}

		/// <summary>
		/// Gets or sets the minumum interval in ms between <see cref="Tick"/> for this <see cref="Script"/>.
		/// Default value is 0 meaning the event will execute once each frame.
		/// </summary>
		protected int Interval
		{
			get => SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).Interval;
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.Interval = value;
				}
			}
		}

		/// <summary>
		/// Returns a string that represents this <see cref="Script"/>.
		/// </summary>
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Aborts execution of this <see cref="Script"/>.
		/// </summary>
		public void Abort()
		{
			SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).Abort();
		}

		/// <summary>
		/// Pauses execution of the <see cref="Script"/> for a specific amount of time.
		/// Must be called inside the main script loop (the <see cref="Tick"/> event or any sub methods called from it).
		/// </summary>
		/// <param name="ms">The time in milliseconds to pause for.</param>
		public static void Wait(int ms)
		{
			SHVDN.Script script = SHVDN.ScriptDomain.ExecutingScript;
			if (script == null || !script.IsRunning)
			{
				throw new InvalidOperationException("Illegal call to 'Script.Wait()' outside main loop!");
			}

			script.Wait(ms);
		}
		/// <summary>
		/// Yields the execution of the script for 1 frame.
		/// </summary>
		public static void Yield()
		{
			Wait(0);
		}
	}
}
