using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHotel.Clients.Core.Services.IoT
{
    public abstract class RoomSensorBase
    {
        protected RoomSensorBase(SensorValue defaultValue, SensorValue minimum, SensorValue maximum)
        {
            Minimum = minimum;
            Maximum = maximum;

            Value = defaultValue;
        }

        protected RoomSensorBase(SensorValue defaultValue, SensorValue desiredValue, SensorValue minimum, SensorValue maximum) : this(defaultValue, minimum, maximum)
        {
            Desired = desiredValue;
        }

        public SensorValue Value { get; protected set; }

        public SensorValue Minimum { get; protected set; }
        public SensorValue Maximum { get; protected set; }
        public SensorValue Desired { get; protected set; }
    }
}
