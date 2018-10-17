using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SmartHotel.Clients.Core.Models
{
    [DataContract]
    public class DeviceDesiredData
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "sensorId")]
        public string SensorId { get; set; }
        [DataMember(Name = "roomId")]
        public string RoomId { get; set; }
        [DataMember(Name = "desiredValue")]
        public string DesiredValue { get; set; }
        [DataMember(Name = "EventTimestamp")]
        public DateTime EventTimestamp { get; set; }
    }
}
