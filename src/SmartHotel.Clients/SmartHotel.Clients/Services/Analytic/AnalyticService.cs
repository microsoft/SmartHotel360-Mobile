using Microsoft.AppCenter.Analytics;
using System.Collections.Generic;

namespace SmartHotel.Clients.Core.Services.Analytic
{
   public class AnalyticService : IAnalyticService
    {
        public void TrackEvent(string name, Dictionary<string, string> properties = null)
        {
            Analytics.TrackEvent(name, properties);
        }
    }
}