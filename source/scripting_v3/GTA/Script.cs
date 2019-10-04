//
// Copyright (C) 2015 crosire
//
// This software is  provided 'as-is', without any express  or implied  warranty. In no event will the
// authors be held liable for any damages arising from the use of this software.
// Permission  is granted  to anyone  to use  this software  for  any  purpose,  including  commercial
// applications, and to alter it and redistribute it freely, subject to the following restrictions:
//
//   1. The origin of this software must not be misrepresented; you must not claim that you  wrote the
//      original  software. If you use this  software  in a product, an  acknowledgment in the product
//      documentation would be appreciated but is not required.
//   2. Altered source versions must  be plainly  marked as such, and  must not be  misrepresented  as
//      being the original software.
//   3. This notice may not be removed or altered from any source distribution.
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
		internal ScriptSettings _settings;
		#endregion

		public Script()
		{
			Name = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).Name;
			Filename = SHVDN.ScriptDomain.CurrentDomain.LookupScriptFilename(GetType());
		}

		/// <summary>
		/// An event that is raised every tick of the script. 
		/// Put code that needs to be looped each frame in here.
		/// </summary>
		public event EventHandler Tick
		{
			add { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.Tick += value; }
			remove { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.Tick -= value; }
		}
		/// <summary>
		/// An event that is raised when this <see cref="Script"/> gets aborted for any reason.
		/// This should be used for cleaning up anything created during this <see cref="Script"/>.
		/// </summary>
		public event EventHandler Aborted
		{
			add { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.Aborted += value; }
			remove { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.Aborted -= value; }
		}

		/// <summary>
		/// An event that is raised when a key is lifted.
		/// The <see cref="System.Windows.Forms.KeyEventArgs"/> contains the key that was lifted.
		/// </summary>
		public event WinForms.KeyEventHandler KeyUp
		{
			add { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.KeyUp += value; }
			remove { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.KeyUp -= value; }
		}
		/// <summary>
		/// An event that is raised when a key is first pressed.
		/// The <see cref="System.Windows.Forms.KeyEventArgs"/> contains the key that was pressed.
		/// </summary>
		public event WinForms.KeyEventHandler KeyDown
		{
			add { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.KeyDown += value; }
			remove { var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this); if (s != null) s.KeyDown -= value; }
		}

		/// <summary>
		/// Gets the name of this <see cref="Script"/>.
		/// </summary>
		public string Name { get; internal set; }
		/// <summary>
		/// Gets the filename of this <see cref="Script"/>.
		/// </summary>
		public string Filename { get; internal set; }

		/// <summary>
		/// Gets the Directory where this <see cref="Script"/> is stored.
		/// </summary>
		public string BaseDirectory => Path.GetDirectoryName(Filename);

		/// <summary>
		/// Checks if this <see cref="Script"/> is paused.
		/// </summary>
		public bool IsPaused
		{
			get { return SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).IsPaused; }
		}

		/// <summary>
		/// Checks if this <see cref="Script"/> is running.
		/// </summary>
		public bool IsRunning
		{
			get { return SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).IsRunning; }
		}

		/// <summary>
		/// Checks if this <see cref="Script"/> is executing.
		/// </summary>
		public bool IsExecuting
		{
			get { return SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).IsExecuting; }
		}

		/// <summary>
		/// Gets an INI file associated with this <see cref="Script"/>.
		/// The File will be in the same location as this <see cref="Script"/> but with an extension of ".ini".
		/// Use this to save and load settings for this <see cref="Script"/>.
		/// </summary>
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

		/// <summary>
		/// Gets or sets the interval in ms between <see cref="Tick"/> for this <see cref="Script"/>.
		/// Default value is 0 meaning the event will execute once each frame.
		/// </summary>
		protected int Interval
		{
			get { return SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).Interval; }
			set {
				if (value < 0)
					value = 0;
				var s = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (s != null)
					s.Interval = value;
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

			if (ReferenceEquals(script, null) || !script.IsRunning)
				throw new InvalidOperationException("Illegal call to 'Script.Wait()' outside main loop!");

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
		/// Adds a new <see cref="Script"/> to the CurrentDomain threads.
		/// </summary>
		public static Script AddScript(Type scriptType)
		{
			SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.InstantiateScript(scriptType);
			if (script == null)
				return null;

			script.Start();

			return (Script)script.ScriptInstance;
		}
	}
}
