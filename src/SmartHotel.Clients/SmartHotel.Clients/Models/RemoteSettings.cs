using Newtonsoft.Json;

namespace SmartHotel.Clients.Core.Models
{
    public class RemoteSettings
    {
        public RemoteSettings()
        {
            Urls = new EndpointsData();
            Tokens = new TokensData();
            B2c = new B2cData();
            Analytics = new AnalyticsData();
            Bot = new BotData();
            Others = new OthersData();
        }

        public EndpointsData Urls { get; set; }

        public TokensData Tokens { get; set; }

        public B2cData B2c { get; set; }

        public AnalyticsData Analytics { get; set; }

        public BotData Bot { get; set; }

        public OthersData Others { get; set; }

        public class EndpointsData
        {
            public string Hotels { get; set; }

            public string Bookings { get; set; }

            public string Suggestions { get; set; }

            public string Notifications { get; set; }

            [JsonProperty("images_base")]
            public string ImagesBaseUri { get; set; }

            public string RoomDevicesEndpoint { get; set; }
        }

        public class TokensData
        {
            public string Bingmaps { get; set; }
        }

        public class B2cData
        {
            public string Client { get; set; }

            public string Tenant { get; set; }

            public string Policy { get; set; }
        }

        public class AnalyticsData
        {
            public string Android { get; set; }

            public string Ios { get; set; }

            public string Uwp { get; set; }
        }

        public class OthersData
        {
            public string FallbackMapsLocation { get; set; }
        }

        public class BotData
        {
            [JsonProperty(PropertyName = "FacebookBotId")]
            public string FacebookId { get; set; }
            [JsonProperty(PropertyName = "id")]
            public string SkypeId { get; set; }
        }
    }
}
