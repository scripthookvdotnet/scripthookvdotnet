//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Forms;

namespace SHVDN
{
	public sealed class Script
	{
		private Thread _thread; // The thread hosting the execution of the script
		internal SemaphoreSlim _waitEvent;
		internal SemaphoreSlim _continueEvent;
		internal readonly ConcurrentQueue<Tuple<bool, KeyEventArgs>> _keyboardEvents = new();

		/// <summary>
		/// Gets or sets the interval in ms between each <see cref="Tick"/>.
		/// </summary>
		public int Interval { get; set; }

		/// <summary>
		/// Gets whether executing of this script is paused or not.
		/// </summary>
		public bool IsPaused { get; private set; }

		/// <summary>
		/// Gets the status of this script.
		/// So <see langword="true" /> if it is running and <see langword="false" /> if it was aborted.
		/// </summary>
		public bool IsRunning { get; private set; }

		/// <summary>
		/// Gets whether this is the currently executing script.
		/// </summary>
		public bool IsExecuting => ScriptDomain.ExecutingScript == this;

		/// <summary>
		/// Gets whether a dedicated thread is hosting the execution of this script.
		/// </summary>
		public bool IsUsingThread => _thread != null;

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
		/// Gets the instance name of this script.
		/// </summary>
		public string Name { get; internal set; }

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
		private void MainLoop()
		{
			IsRunning = true;

			try
			{
				// Wait for script domain to continue this script
				_continueEvent.Wait();

				while (IsRunning)
				{
					DoTick();

					// Yield execution to next tick
					Wait(Interval);
				}
			}
			catch (ThreadAbortException ex)
			{
				Log.Message(Log.Level.Warning, "Aborted script ", Name, ".", Environment.NewLine, ex.StackTrace);
			}
		}
		internal void DoTick()
		{
			// Process keyboard events
			while (_keyboardEvents.TryDequeue(out Tuple<bool, KeyEventArgs> ev))
			{
				try
				{
					if (!ev.Item1)
					{
						KeyUp?.Invoke(this, ev.Item2);
					}
					else
					{
						KeyDown?.Invoke(this, ev.Item2);
					}
				}
				catch (ThreadAbortException)
				{
					// Stop main loop immediately on a thread abort exception
					throw;
				}
				catch (Exception ex)
				{
					ScriptDomain.HandleUnhandledException(this, new UnhandledExceptionEventArgs(ex, false));
					break; // Break out of key event loop, but continue to run script
				}
			}

			try
			{
				Tick?.Invoke(this, EventArgs.Empty);
			}
			catch (ThreadAbortException)
			{
				// Stop main loop immediately on a thread abort exception
				throw;
			}
			catch (Exception ex)
			{
				ScriptDomain.HandleUnhandledException(this, new UnhandledExceptionEventArgs(ex, true));

				// An exception during tick is fatal, so abort the script and stop main loop
				Abort();
			}
		}

		/// <summary>
		/// Starts execution of this script.
		/// </summary>
		public void Start(bool useThread = true)
		{
			if (useThread)
			{
				_waitEvent = new SemaphoreSlim(0);
				_continueEvent = new SemaphoreSlim(0);

				_thread = new Thread(MainLoop);
				// By setting this property to true, script thread should stop executing when the main thread stops executing
				// Note: The exe may not stop executing if some scripts create Thread instances and use them without setting Thread.IsBackground false
				_thread.IsBackground = true;

				_thread.Start();
			}
			else
			{
				IsRunning = true;
			}

			Log.Message(Log.Level.Info, "Started script ", Name, ".");
		}
		/// <summary>
		/// Aborts execution of this script.
		/// </summary>
		public void Abort()
		{
			IsRunning = false;

			try
			{
				Aborted?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				ScriptDomain.HandleUnhandledException(this, new UnhandledExceptionEventArgs(ex, true));
			}

			if (IsUsingThread)
			{
				_waitEvent.Release();

				_thread.Abort();
				_thread = null;
			}
			else
			{
				Log.Message(Log.Level.Warning, "Aborted script ", Name, ".");
			}
		}

		/// <summary>
		/// Pauses execution of this script.
		/// </summary>
		public void Pause()
		{
			if (IsPaused)
			{
				return; // Pause status has not changed, so nothing to do
			}

			IsPaused = true;

			Log.Message(Log.Level.Info, "Paused script ", Name, ".");
		}
		/// <summary>
		/// Resumes execution of this script.
		/// </summary>
		public void Resume()
		{
			if (!IsPaused)
			{
				return;
			}

			IsPaused = false;

			Log.Message(Log.Level.Info, "Resumed script ", Name, ".");
		}

		/// <summary>
		/// Pause execution of this script for the specified time.
		/// </summary>
		/// <param name="ms">The time in milliseconds to pause.</param>
		public void Wait(int ms)
		{
			if (IsUsingThread)
			{
				int startTickCount = Environment.TickCount;

				do
				{
					_waitEvent.Release();
					_continueEvent.Wait();
				}
				while (Environment.TickCount - startTickCount < ms);
			}
			else
			{
				Thread.Sleep(ms);
			}
		}
	}
}
