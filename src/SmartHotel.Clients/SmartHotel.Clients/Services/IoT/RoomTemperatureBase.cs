using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHotel.Clients.Core.Services.IoT
{
    public abstract class RoomTemperatureBase
    {
        protected readonly TemperatureValue Default;
        
        protected RoomTemperatureBase(TemperatureValue defaultValue, TemperatureValue minimum, TemperatureValue maximum)
        {
            Default = defaultValue;
            Minimum = minimum;
            Maximum = maximum;

            Value = Default;
        }

        public TemperatureValue Value { get; protected set; }

        public TemperatureValue Minimum { get; protected set; }
        public TemperatureValue Maximum { get; protected set; }
    }
}
