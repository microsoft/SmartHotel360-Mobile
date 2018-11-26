using System;
using System.Runtime.Serialization;

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
	    [DataMember(Name = "eventTimestamp")]
	    public DateTime EventTimestamp { get; set; }
	    [DataMember( Name = "iotHubDeviceId" )]
	    public string IoTHubDeviceId { get; set; }
    }

    [DataContract(Name = "deviceSensorData")]
    public class DeviceRequest
    {
        [DataMember(Name = "roomId")]
        public string RoomId { get; set; }
        [DataMember(Name = "sensorId")]
        public string SensorId { get; set; }
        [DataMember(Name = "deviceId")]
        public string DeviceId { get; set; }
        [DataMember(Name = "methodName")]
        public string MethodName { get; set; }
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
