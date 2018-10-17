using System.Collections.Generic;

namespace SmartHotel.Clients.Core.Services.Analytic
{
    public interface IAnalyticService
    {
        void TrackEvent(string name, Dictionary<string, string> properties = null);
    }
}