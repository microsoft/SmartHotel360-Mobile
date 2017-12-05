using System;
using System.Text;

namespace SmartHotel.Clients.Core.Services.Authentication
{
    public class GravatarUrlProvider : IAvatarUrlProvider
    {
        public string GetAvatarUrl(string email)
        {
            var hash = GetMd5(email);

            var uriBuilder = new UriBuilder("https://www.gravatar.com")
            {
                Path = $"avatar/{hash}",
                Query = "size=300"
            };

            return uriBuilder.ToString();
        }

        private static string GetMd5(string email)
        {
            using (var algorithm = System.Security.Cryptography.MD5.Create())
            {
                var result = algorithm.ComputeHash(Encoding.ASCII.GetBytes(email));
                var hash = BitConverter.ToString(result).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
