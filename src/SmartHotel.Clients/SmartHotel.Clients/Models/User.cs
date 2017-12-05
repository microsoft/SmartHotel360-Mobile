namespace SmartHotel.Clients.Core.Models
{
    public class User
    {
        public string Id { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }

        public bool LoggedInWithMicrosoftAccount { get; set; }
    }
}
