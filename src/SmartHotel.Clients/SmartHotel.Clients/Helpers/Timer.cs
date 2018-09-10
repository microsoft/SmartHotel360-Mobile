using System;
using System.Threading;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Helpers
{
	// https://forums.xamarin.com/discussion/comment/149877/#Comment_149877
    public class Timer
    {
	    private readonly TimeSpan _timespan;
	    private readonly Action _callback;
	    private readonly bool _isRecurring;

	    private CancellationTokenSource _cancellation;

	    public Timer(TimeSpan timespan, Action callback, bool isRecurring = false)
	    {
		    _timespan = timespan;
		    _callback = callback;
		    _isRecurring = isRecurring;
		    _cancellation = new CancellationTokenSource();
	    }

	    public void Start()
	    {
		    CancellationTokenSource cts = _cancellation; // safe copy
		    Device.StartTimer(_timespan,
			    () => {
				    if (cts.IsCancellationRequested) return false;
				    _callback.Invoke();
				    return _isRecurring;
			    });
	    }

	    public void Stop()
	    {
		    Interlocked.Exchange(ref _cancellation, new CancellationTokenSource()).Cancel();
	    }
    }
}
