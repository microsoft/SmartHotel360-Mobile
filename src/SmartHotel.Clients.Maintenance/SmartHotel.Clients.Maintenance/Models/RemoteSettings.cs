namespace SmartHotel.Clients.Maintenance.Models
{
    public class RemoteSettings
    {
        public RemoteSettings()
        {
            Urls = new EndpointsData();
        }

        public EndpointsData Urls { get; set; }

        public class EndpointsData
        {
            public string Tasks { get; set; }
        }
    }
}
