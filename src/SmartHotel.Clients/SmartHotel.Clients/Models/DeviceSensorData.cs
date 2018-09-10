using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Models
{
	[DataContract(Name = "deviceSensorData")]
    public class DeviceSensorData
    {
	    [DataMember(Name = "id")]
	    public string Id { get; set; }
	    [DataMember(Name = "sensorId")]
	    public string SensorId { get; set; }
	    [DataMember(Name = "roomId")]
	    public string RoomId { get; set; }
	    [DataMember(Name = "sensorReading")]
	    public string SensorReading { get; set; }
	    [DataMember(Name = "sensorDataType")]
	    public string SensorDataType { get; set; }
	    [DataMember(Name = "DesiredValue")]
	    public string DesiredValue { get; set; }
	    [DataMember(Name = "EventTimestamp")]
	    public DateTime EventTimestamp { get; set; }
    }
}
