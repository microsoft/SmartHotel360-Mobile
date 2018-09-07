using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHotel.Clients.Core.Services.IoT
{
    public abstract class RoomTemperatureBase
    {
        protected RoomTemperatureBase(TemperatureValue defaultValue, TemperatureValue minimum, TemperatureValue maximum)
        {
            Minimum = minimum;
            Maximum = maximum;

            Value = defaultValue;
        }

        protected RoomTemperatureBase(TemperatureValue defaultValue, TemperatureValue desiredValue, TemperatureValue minimum, TemperatureValue maximum) : this(defaultValue, minimum, maximum)
        {
            Desired = desiredValue;
        }

        public TemperatureValue Value { get; protected set; }

        public TemperatureValue Minimum { get; protected set; }
        public TemperatureValue Maximum { get; protected set; }
        public TemperatureValue Desired { get; protected set; }
    }
}
