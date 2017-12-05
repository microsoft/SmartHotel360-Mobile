using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text;

namespace SmartHotel.Clients.Core.Services.Authentication
{
    public static class AuthenticationResultHelper
    {
        public static Models.User GetUserFromResult(AuthenticationResult ar)
        {
            JObject data = ParseIdToken(ar.IdToken);

            var user = new Models.User
            {
                Id = ar.User.UniqueId,
                Token = ar.Token,
                Email = GetTokenValue(data, "emails"),
                Name = GetTokenValue(data, "given_name"),
                LastName = GetTokenValue(data, "family_name")
            };

            return user;
        }

        private static JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }

        private static string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());

            return decoded;
        }

        private static string GetTokenValue(JObject data, string key)
        {
            string value = string.Empty;

            try
            {
                JToken token = data[key];

                value = token.HasValues
                    ? token.First.ToString()
                    : token.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting token data from B2C: {ex}");
            }

            return value;
        }
    }
}