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
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Forms;

namespace SHVDN
{
	public class Script
	{
		Thread thread; // The thread hosting the execution of the script
		internal SemaphoreSlim waitEvent = new SemaphoreSlim(0);
		internal SemaphoreSlim continueEvent = new SemaphoreSlim(0);
		internal ConcurrentQueue<Tuple<bool, KeyEventArgs>> keyboardEvents = new ConcurrentQueue<Tuple<bool, KeyEventArgs>>();

		/// <summary>
		/// Gets or sets the interval in ms between each <see cref="Tick"/>.
		/// </summary>
		public int Interval { get; set; }

		/// <summary>
		/// Gets the execution status of this script.
		/// </summary>
		public bool IsRunning { get; private set; }

		/// <summary>
		/// An event that is raised every tick of the script. 
		/// </summary>
		public event EventHandler Tick;
		/// <summary>
		/// An event that is raised when this script gets aborted for any reason.
		/// </summary>
		public event EventHandler Aborted;

		/// <summary>
		/// An event that is raised when a key is lifted.
		/// </summary>
		public event KeyEventHandler KeyUp;
		/// <summary>
		/// An event that is raised when a key is first pressed.
		/// </summary>
		public event KeyEventHandler KeyDown;

		/// <summary>
		/// Gets the type name of this script.
		/// </summary>
		public string Name => ScriptInstance.GetType().FullName;

		/// <summary>
		/// Gets the path to the file that contains this script.
		/// </summary>
		public string Filename { get; internal set; }

		/// <summary>
		/// Gets the instance of the script.
		/// </summary>
		public object ScriptInstance { get; internal set; }

		/// <summary>
		/// The main execution logic of all scripts.
		/// </summary>
		void MainLoop()
		{
			IsRunning = true;

			// Wait for script domain to continue this script
			continueEvent.Wait();

			while (IsRunning)
			{
				// Process keyboard events
				while (keyboardEvents.TryDequeue(out Tuple<bool, KeyEventArgs> ev))
				{
					try
					{
						if (!ev.Item1)
							KeyUp?.Invoke(this, ev.Item2);
						else
							KeyDown?.Invoke(this, ev.Item2);
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
					break;
				}

				// Yield execution to next tick
				Wait(Interval);
			}

			// Abort in case the script encountered an unhandled exception above
			if (IsRunning)
				Abort();
		}

		/// <summary>
		/// Starts execution of this script.
		/// </summary>
		public void Start()
		{
			thread = new Thread(new ThreadStart(MainLoop));
			thread.Start();

			Log.Message(Log.Level.Info, "Started script ", Name, ".");
		}
		/// <summary>
		/// Aborts execution of this script.
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

			IsRunning = false;

			waitEvent.Release();

			if (thread != null)
			{
				thread.Abort(); thread = null;

				Log.Message(Log.Level.Info, "Aborted script ", Name, ".");
			}
		}

		/// <summary>
		/// Pause execution of this script for the specified time.
		/// </summary>
		/// <param name="ms">The time in milliseconds to pause.</param>
		public void Wait(int ms)
		{
			DateTime resumeTime = DateTime.UtcNow + TimeSpan.FromMilliseconds(ms);

			do
			{
				waitEvent.Release();
				continueEvent.Wait();
			}
			while (DateTime.UtcNow < resumeTime);
		}
	}
}
