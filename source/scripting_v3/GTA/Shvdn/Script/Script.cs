//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.IO;
using WinForms = System.Windows.Forms;

namespace GTA
{
	/// <summary>
	/// A base class for all user scripts to inherit.
	/// Only scripts that inherit directly from this class and have a default (parameterless) public constructor will be detected and started.
	/// </summary>
	public abstract class Script
	{
		#region Fields
		ScriptSettings _settings;
		#endregion

		class InstantiateScriptTask : SHVDN.IScriptTask
		{
			internal Type _type;
			internal SHVDN.Script _script;

			public void Run()
			{
				_script = SHVDN.ScriptDomain.CurrentDomain.InstantiateScript(_type);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Script"/> class.
		/// This constructor is called from the script domain of ScriptHookVDotNet and is
		/// not intended to be used directly from your code.
		/// To instantiate scripts from a running script instance, use <see cref="InstantiateScript{T}"/>.
		/// </summary>
		public Script()
		{
			Name = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this)?.Name ?? string.Empty;
			Filename = SHVDN.ScriptDomain.CurrentDomain.LookupScriptFilename(GetType());
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
		public event WinForms.KeyEventHandler KeyUp
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
		public event WinForms.KeyEventHandler KeyDown
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
		/// Gets the name of this <see cref="Script"/>.
		/// </summary>
		public string Name
		{
			get;
		}
		/// <summary>
		/// Gets the filename of this <see cref="Script"/>.
		/// </summary>
		public string Filename
		{
			get;
		}

		/// <summary>
		/// Gets the Directory where this <see cref="Script"/> is stored.
		/// </summary>
		public string BaseDirectory => Path.GetDirectoryName(Filename);

		/// <summary>
		/// Checks if this <see cref="Script"/> is paused.
		/// </summary>
		public bool IsPaused => SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).IsPaused;

		/// <summary>
		/// Checks if this <see cref="Script"/> is running.
		/// </summary>
		public bool IsRunning => SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).IsRunning;

		/// <summary>
		/// Checks if this <see cref="Script"/> is executing.
		/// </summary>
		public bool IsExecuting => SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).IsExecuting;

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
		/// Gets the full file path for a file relative to this <see cref="Script"/>.
		/// e.g: <c>GetRelativeFilePath("ScriptFiles\texture1.png")</c> may return <c>"C:\Program Files\Rockstar Games\Grand Theft Auto V\scripts\ScriptFiles\texture1.png"</c>.
		/// </summary>
		/// <param name="filePath">The file path relative to the location of this <see cref="Script"/>.</param>
		public string GetRelativeFilePath(string filePath)
		{
			return Path.Combine(BaseDirectory, filePath);
		}

		/// <summary>
		/// Aborts execution of this <see cref="Script"/>.
		/// </summary>
		public void Abort()
		{
			SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).Abort();
		}

		/// <summary>
		/// Pause execution of this <see cref="Script"/>.
		/// </summary>
		public void Pause()
		{
			SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).Pause();
		}

		/// <summary>
		/// Starts execution of this <see cref="Script"/> after it has been Paused.
		/// </summary>
		public void Resume()
		{
			SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).Resume();
		}

		/// <summary>
		/// Pauses execution of the <see cref="Script"/> for a specific amount of time.
		/// Must be called inside the main script loop (the <see cref="Tick"/> event or any sub methods called from it).
		/// </summary>
		/// <param name="ms">The time in milliseconds to pause for.</param>
		public static void Wait(int ms)
		{
			SHVDN.Script script = SHVDN.ScriptDomain.ExecutingScript;
			if (script == null || !script.IsRunning || !script.IsUsingThread)
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

		/// <summary>
		/// Spawns a new <see cref="Script"/> instance of the specified type.
		/// </summary>
		/// <remarks>
		/// You need to call this method on the main script thread so instantiation can suceeed.
		/// Do not call this method in field initializers or your script constructor, and doing so
		/// will result in failure to instantiate a new script instance.
		/// </remarks>
		public static T InstantiateScript<T>() where T : Script
		{
			var task = new InstantiateScriptTask { _type = typeof(T) };
			SHVDN.ScriptDomain.CurrentDomain.ExecuteTaskInScriptDomainThread(task);

			if (task._script == null)
			{
				return null;
			}

			task._script.Start();

			return (T)task._script.ScriptInstance;
		}
	}
}
