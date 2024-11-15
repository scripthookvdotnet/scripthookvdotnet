//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace SHVDN
{
    public sealed class Script : IDisposable
    {
        internal SemaphoreSlim _waitEvent;
        internal SemaphoreSlim _continueEvent;
        internal readonly ConcurrentQueue<Tuple<bool, KeyEventArgs>> _keyboardEvents = new();

        private Thread _thread; // The thread hosting the execution of the script

        private int _interval;
        private bool _isPaused;
        private bool _isRunning;
        private string _name;
        private string _fileName;
        private object _scriptInstance;

        private bool _nativeCallResetsTimeout;

        // Use a reader-writer lock rather than a monitor lock because all the fields are not too frequently written
        private readonly ReaderWriterLockSlim _rwLock = new ();

        private readonly CheapThreadSafeStopwatch _stopwatch = new();

        public void Dispose()
        {
            _rwLock.Dispose();
        }

        internal SemaphoreSlim WaitEvent
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return _waitEvent;
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
            set
            {
                _rwLock.EnterWriteLock();
                try
                {
                    _waitEvent = value;
                }
                finally
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }

        internal SemaphoreSlim ContinueEvent
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return _continueEvent;
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
            set
            {
                _rwLock.EnterWriteLock();
                try
                {
                    _continueEvent = value;
                }
                finally
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }

        internal CheapThreadSafeStopwatch StopwatchForTimeout => _stopwatch;

        private Thread Thread
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return _thread;
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
            set
            {
                _rwLock.EnterWriteLock();
                try
                {
                    _thread = value;
                }
                finally
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Gets or sets the interval in ms between each <see cref="Tick"/>.
        /// </summary>
        public int Interval
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return _interval;
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
            set
            {
                _rwLock.EnterWriteLock();
                try
                {
                    _interval = value;
                }
                finally
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Gets whether executing of this script is paused or not.
        /// </summary>
        public bool IsPaused
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return _isPaused;
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
            private set
            {
                _rwLock.EnterWriteLock();
                try
                {
                    _isPaused = value;
                }
                finally
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Gets the status of this script.
        /// So <see langword="true" /> if it is running and <see langword="false" /> if it was aborted.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return _isRunning;
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
            private set
            {
                _rwLock.EnterWriteLock();
                try
                {
                    _isRunning = value;
                }
                finally
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }

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
        public string Name
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return _name;
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
            internal set
            {
                _rwLock.EnterWriteLock();
                try
                {
                    _name = value;
                }
                finally
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Gets the path to the file that contains this script.
        /// </summary>
        public string Filename
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return _fileName;
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
            internal set
            {
                _rwLock.EnterWriteLock();
                try
                {
                    _fileName = value;
                }
                finally
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Gets the instance of the script.
        /// </summary>
        public object ScriptInstance
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return _scriptInstance;
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
            internal set
            {
                _rwLock.EnterWriteLock();
                try
                {
                    _scriptInstance = value;
                }
                finally
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }

        internal bool NativeCallResetsTimeout
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return _nativeCallResetsTimeout;
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
            set
            {
                _rwLock.EnterWriteLock();
                try
                {
                    _nativeCallResetsTimeout = value;
                }
                finally
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }

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
                WaitEvent = new SemaphoreSlim(0);
                ContinueEvent = new SemaphoreSlim(0);

                Thread th = new Thread(MainLoop);
                // By setting this property to true, script thread should stop executing when the main thread stops executing
                // Note: The exe may not stop executing if some scripts create Thread instances and use them without setting Thread.IsBackground false
                th.IsBackground = true;

                th.Start();

                Thread = th;
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
        /// <remarks>
        /// This method may suffer race condtions in a script instances if a dedicated thread is used to run a script,
        /// since this method will be called in the script main method when the script itself calls it, but it will be
        /// called in the main method of the `<c>ScriptDomain</c>` (which is different from the script thread) when
        /// the `<c>ScriptDomain</c>` aborted it.
        /// </remarks>
        // We can't completely eliminate race condition issues as long as we use dedicated threads to run scripts...
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
                Thread = null;
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
        /// Pause execution of this script for at least the specified duration.
        /// </summary>
        /// <param name="ms">The duration in milliseconds to pause.</param>
        public void Wait(int ms)
        {
            if (IsUsingThread)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                do
                {
                    _waitEvent.Release();
                    _continueEvent.Wait();
                }
                while (sw.ElapsedMilliseconds < ms);
            }
            else
            {
                System.Threading.Thread.Sleep(ms);
            }
        }
    }
}
