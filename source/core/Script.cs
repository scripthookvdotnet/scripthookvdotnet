/**
 * Copyright (C) 2015 crosire
 *
 * This software is  provided 'as-is', without any express  or implied  warranty. In no event will the
 * authors be held liable for any damages arising from the use of this software.
 * Permission  is granted  to anyone  to use  this software  for  any  purpose,  including  commercial
 * applications, and to alter it and redistribute it freely, subject to the following restrictions:
 *
 *   1. The origin of this software must not be misrepresented; you must not claim that you  wrote the
 *      original  software. If you use this  software  in a product, an  acknowledgment in the product
 *      documentation would be appreciated but is not required.
 *   2. Altered source versions must  be plainly  marked as such, and  must not be  misrepresented  as
 *      being the original software.
 *   3. This notice may not be removed or altered from any source distribution.
 */

using System;
using System.Threading;
using System.Collections.Concurrent;
using WinForms = System.Windows.Forms;

namespace GTA
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class RequireScript : Attribute
	{
		internal Type _dependency;

		public RequireScript(Type dependency)
		{
			this._dependency = dependency;
		}
	}
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class ScriptAttributes : Attribute
	{
		public string Author;
		public string SupportURL;
	}

	public abstract class Script
	{
		#region Fields
		internal int _interval = 0;
		internal bool _running = false;
		internal string _filename;
		internal ScriptDomain _scriptdomain;
		internal Thread _thread;
		internal AutoResetEvent _waitEvent = new AutoResetEvent(false);
		internal AutoResetEvent _continueEvent = new AutoResetEvent(false);
		internal ConcurrentQueue<Tuple<bool, WinForms.KeyEventArgs>> _keyboardEvents = new ConcurrentQueue<Tuple<bool, WinForms.KeyEventArgs>>();
		internal ScriptSettings _settings;
		#endregion

		public Script()
		{
			_filename = ScriptDomain.CurrentDomain.LookupScriptFilename(GetType());
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
		/// An event that is raised when this script gets aborted for any reason.
		/// This should be used for cleaning up anything created during this script
		/// </summary>
		public event EventHandler Aborted;

		/// <summary>
		/// Gets the name of this <see cref="Script"/>.
		/// </summary>
		public string Name
		{
			get
			{
				return GetType().FullName;
			}
		}
		/// <summary>
		/// Gets the filename of this <see cref="Script"/>.
		/// </summary>
		public string Filename
		{
			get
			{
				return _filename;
			}
		}

		/// <summary>
		/// Gets the Directory where this <see cref="Script"/> is stored.
		/// </summary>
		public string BaseDirectory
		{
			get
			{
				return System.IO.Path.GetDirectoryName(_filename);
			}
		}

		public ScriptSettings Settings
		{
			get
			{
				if (ReferenceEquals(_settings, null))
				{
					string path = System.IO.Path.ChangeExtension(_filename, ".ini");

					_settings = ScriptSettings.Load(path);
				}

				return _settings;
			}
		}

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

		public string GetRelativeFilePath(string filePath)
		{
			return System.IO.Path.Combine(BaseDirectory, filePath);
		}

		internal void Start()
		{
			ThreadStart threadDelegate = delegate { MainLoop(); };
			_thread = new Thread(threadDelegate);
			_thread.Start();

			ScriptDomain.OnStartScript(this);
		}
		public void Abort()
		{
			try
			{
				Aborted(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				ScriptDomain.HandleUnhandledException(this, new UnhandledExceptionEventArgs(ex, true));
			}

			_running = false;
			_waitEvent.Set();

			if (ReferenceEquals(_thread, null))
			{
				return;
			}

			_thread.Abort();
			_thread = null;

			ScriptDomain.OnAbortScript(this);
		}

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
				script._waitEvent.Set();
				script._continueEvent.WaitOne();
			}
			while (DateTime.UtcNow < resume);
		}
		public static void Yield()
		{
			Wait(0);
		}

		public void MainLoop()
		{
			_running = true;

			// Wait for domain to run scripts
			_continueEvent.WaitOne();

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
							KeyDown(this, keyevent.Item2);
						}
						else
						{
							KeyUp(this, keyevent.Item2);
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
					Tick(this, EventArgs.Empty);
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

		/// <summary>
		/// Returns a string that represents this <see cref="Script"/>.
		/// </summary>
		public override string ToString()
		{
			return Name;
		}
}
}
