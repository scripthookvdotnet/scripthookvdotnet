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
using System.Threading;
using System.Collections.Concurrent;
using WinForms = System.Windows.Forms;

namespace GTA
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class RequireScript : Attribute
	{
		#region Fields
		internal Type _dependency;
		#endregion

		public RequireScript(Type dependency)
		{
			_dependency = dependency;
		}
	}
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class ScriptAttributes : Attribute
	{
		public string Author;
		public string SupportURL;
	}

	/// <summary>
	/// A base class for all user scripts to inherit.
	/// Only scripts that inherit directly from this class and have a default (parameterless) public constructor will be detected and started.
	/// </summary>
	public abstract class Script
	{
		#region Fields
		internal int _interval = 0;
		internal bool _running = false;
		internal ScriptDomain _scriptdomain;
		internal Thread _thread;
		internal SemaphoreSlim _waitEvent = new SemaphoreSlim(0);
		internal SemaphoreSlim _continueEvent = new SemaphoreSlim(0);
		internal ConcurrentQueue<Tuple<bool, WinForms.KeyEventArgs>> _keyboardEvents = new ConcurrentQueue<Tuple<bool, WinForms.KeyEventArgs>>();
		internal ScriptSettings _settings;
		#endregion

		public Script()
		{
			Filename = ScriptDomain.CurrentDomain.LookupScriptFilename(this);
			_scriptdomain = ScriptDomain.CurrentDomain;
		}

		/// <summary>
		/// An event that is raised every tick of the script. 
		/// Put code that needs to be looped each frame in here.
		/// </summary>
		public event EventHandler Tick;
		/// <summary>
		/// An event that is raised when a key is lifted.
		/// The <see cref="System.Windows.Forms.KeyEventArgs"/> contains the key that was lifted.
		/// </summary>
		public event WinForms.KeyEventHandler KeyUp;
		/// <summary>
		/// An event that is raised when a key is first pressed.
		/// The <see cref="System.Windows.Forms.KeyEventArgs"/> contains the key that was pressed.
		/// </summary>
		public event WinForms.KeyEventHandler KeyDown;
		/// <summary>
		/// An event that is raised when this <see cref="Script"/> gets aborted for any reason.
		/// This should be used for cleaning up anything created during this <see cref="Script"/>.
		/// </summary>
		public event EventHandler Aborted;

		/// <summary>
		/// Gets the name of this <see cref="Script"/>.
		/// </summary>
		public string Name => GetType().FullName;
		/// <summary>
		/// Gets the filename of this <see cref="Script"/>.
		/// </summary>
		public string Filename { get; internal set; }

		/// <summary>
		/// Gets the Directory where this <see cref="Script"/> is stored.
		/// </summary>
		public string BaseDirectory => Path.GetDirectoryName(Filename);

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
			get
			{
				return _interval;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				_interval = value;
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
		/// Starts execution of this <see cref="Script"/>.
		/// </summary>
		internal void Start()
		{
			ThreadStart threadDelegate = new ThreadStart(MainLoop);
			_thread = new Thread(threadDelegate);
			_thread.Start();

			ScriptDomain.OnStartScript(this);
		}
		/// <summary>
		/// Aborts execution of this <see cref="Script"/>.
		/// </summary>
		public void Abort()
		{
			try
			{
				Aborted?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				ScriptDomain.HandleUnhandledException(this, new UnhandledExceptionEventArgs(ex, true));
			}

			_running = false;
			_waitEvent.Release();

			if (_thread != null)
			{
				_thread.Abort();
				_thread = null;

				ScriptDomain.OnAbortScript(this);
			}
		}

		/// <summary>
		/// Pauses execution of the <see cref="Script"/> for a specific amount of time.
		/// Must be called inside the main script loop (the <see cref="Tick"/> event or any sub methods called from it).
		/// </summary>
		/// <param name="ms">The time in milliseconds to pause for.</param>
		public static void Wait(int ms)
		{
			Script script = ScriptDomain.ExecutingScript;

			if (ReferenceEquals(script, null) || !script._running)
			{
				throw new InvalidOperationException("Illegal call to 'Script.Wait()' outside main loop!");
			}

			var resume = DateTime.UtcNow + TimeSpan.FromMilliseconds(ms);

			do
			{
				script._waitEvent.Release();
				script._continueEvent.Wait();
			}
			while (DateTime.UtcNow < resume);
		}
		/// <summary>
		/// Yields the execution of the script for 1 frame.
		/// </summary>
		public static void Yield()
		{
			Wait(0);
		}

		/// <summary>
		/// The main execution logic of all scripts.
		/// </summary>
		internal void MainLoop()
		{
			_running = true;

			// Wait for domain to run scripts
			_continueEvent.Wait();

			// Run main loop
			while (_running)
			{
				Tuple<bool, WinForms.KeyEventArgs> keyevent = null;

				// Process events
				while (_keyboardEvents.TryDequeue(out keyevent))
				{
					try
					{
						if (keyevent.Item1)
						{
							KeyDown?.Invoke(this, keyevent.Item2);
						}
						else
						{
							KeyUp?.Invoke(this, keyevent.Item2);
						}
					}
					catch (Exception ex)
					{
						ScriptDomain.HandleUnhandledException(this, new UnhandledExceptionEventArgs(ex, false));
						break;
					}
				}

				try
				{
					Tick?.Invoke(this, EventArgs.Empty);
				}
				catch (Exception ex)
				{
					ScriptDomain.HandleUnhandledException(this, new UnhandledExceptionEventArgs(ex, true));

					Abort();
					break;
				}

				// Yield execution to next tick
				Wait(_interval);
			}
		}
	}
}
