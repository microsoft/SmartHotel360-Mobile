using System;

namespace SmartHotel.Clients.Core.Helpers
{
    /// <summary>
    /// Invoke action when predicate is meet after certain idle time 
    /// </summary>
    public class DelayedAction
    {
        private readonly Func<bool> _predicate;
        private readonly Action _action;
        private readonly TimeSpan _idleTime;

        private DateTime? _lastestPulse;
        private DateTime? _firstPulse;

        private bool _isRunning;
        private readonly Timer _timer;

        public DelayedAction(Func<bool> predicate, Action action, TimeSpan idleTime)
        {
            _predicate = predicate;
            _action = action;
            _idleTime = idleTime;

            _timer = new Timer(idleTime, TimerTick);
        }

        private void TimerTick()
        {
            _timer.Stop();
            _isRunning = false;
            var now = DateTime.UtcNow;
            if (_lastestPulse != null)
            {
                if (_firstPulse != null)
                {
                    var howLongWaiting = _lastestPulse.Value.ToUniversalTime() -
                                         _firstPulse.Value.ToUniversalTime();
                    if (howLongWaiting > _idleTime &&
                        now - _lastestPulse.Value.ToUniversalTime() > _idleTime)
                    {
                        if (_predicate.Invoke())
                            _action();

                        _firstPulse = null;
                        _lastestPulse = null;
                        _isRunning = false;
                    }
                    else
                    {
                        _timer.Start();
                    }
                }
            }
        }

        public void Pulse()
        {
            _lastestPulse = DateTime.UtcNow;
            if (_firstPulse == null)
            {
                _firstPulse = DateTime.UtcNow;
            }

            if (_isRunning)
                return;

            _isRunning = true;
            _timer.Start();

            //Debug.WriteLine($"DelayedAction: Pulse  {Thread.CurrentThread.ManagedThreadId }");
        }
    }
}