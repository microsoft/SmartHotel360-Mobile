namespace SmartHotel.Clients.Core.Models
{
    public enum SuggestionType
    {
        Event,
        Restaurant
    }

    public class Suggestion
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Rating { get; set; }

        public int Votes { get; set; }

        public SuggestionType SuggestionType { get; set; }
    }
}