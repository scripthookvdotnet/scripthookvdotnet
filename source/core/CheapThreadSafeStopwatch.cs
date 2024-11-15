//
// Copyright (C) 2024 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Threading;

namespace SHVDN
{
    /// <summary>
    /// Provides a set of methods and properties that you can use to measure elapsed time within a resolution
    /// poorer than <see cref="System.Diagnostics.Stopwatch"/> but is thread-safe (and cheaper than a stopwatch
    /// that uses the Win32 API <c>PerformanceQueryCounter</c> and where all the methods are thread-safe).
    /// </summary>
    internal sealed class CheapThreadSafeStopwatch
    {
        private uint _elapsed;
        private uint _startTimestamp;
        private bool _isRunning;
        private SpinLock _spinLock;

        public CheapThreadSafeStopwatch()
        {
            Reset();
            _spinLock = new SpinLock();
        }

        public TimeSpan Elapsed => new TimeSpan(ElapsedMilliseconds);

        public uint ElapsedMilliseconds
        {
            get
            {
                bool lockTaken = false;
                _spinLock.Enter(ref lockTaken);
                try
                {
                    uint res = _elapsed;
                    if (_isRunning)
                    {
                        var currTimestamp = (uint)Environment.TickCount;
                        uint elapsedUntilNow = currTimestamp - _startTimestamp;
                        res += elapsedUntilNow;
                    }
                    return res;
                }
                finally
                {
                    if (lockTaken) _spinLock.Exit();
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                bool lockTaken = false;
                _spinLock.Enter(ref lockTaken);
                try
                {
                    return _isRunning;
                }
                finally
                {
                    if (lockTaken) _spinLock.Exit();
                }
            }
        }

        public void Reset()
        {
            DoActionWithLock(() =>
            {
                _elapsed = 0u;
                _isRunning = false;
                _startTimestamp = 0u;
            });
        }

        public void Restart()
        {
            DoActionWithLock(() =>
            {
                _elapsed = 0u;
                _startTimestamp = (uint)Environment.TickCount;
                _isRunning = true;
            });
        }

        public void Start()
        {
            DoActionWithLock(() =>
            {
                if (!_isRunning)
                {
                    _startTimestamp = (uint)Environment.TickCount;
                    _isRunning = true;
                }
            });
        }

        public void Stop()
        {
            DoActionWithLock(() =>
            {
                if (_isRunning)
                {
                    uint endTimestamp = (uint)Environment.TickCount;
                    uint elapsedThisPeriod = endTimestamp - _startTimestamp;
                    _elapsed += elapsedThisPeriod;
                    _isRunning = false;
                }
            });
        }

        private void DoActionWithLock(Action action)
        {
            bool lockTaken = false;
            _spinLock.Enter(ref lockTaken);
            try
            {
                action();
            }
            finally
            {
                if (lockTaken) _spinLock.Exit();
            }
        }

        public override string ToString()
        {
            return Elapsed.ToString();
        }
    }
}
