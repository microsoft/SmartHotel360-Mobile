using System.Threading.Tasks;
using SmartHotel.Clients.Core.Models;

namespace SmartHotel.Clients.Core.Services.Authentication
{
    public class FakeAuthenticationService : IAuthenticationService
    {
        private static bool AuthSucceded;

        public bool IsAuthenticated
        {
            get
            {
                return AuthSucceded;
            }
        }

        public User AuthenticatedUser => new User
        {
            Email = "john@contoso.com",
            Name = "John",
            LastName = "Doe"
        };

        public async Task<bool> LoginAsync(string userName, string password)
        {
            await Task.Delay(500);

            bool succeeded = true;

            if (userName.StartsWith("1"))
            {
                succeeded = false;
            }

            AuthSucceded = succeeded;

            return succeeded;
        }

        public Task<bool> LoginWithMicrosoftAsync()
        {
            return Task.FromResult(false);
        }

        public Task<bool> UserIsAuthenticatedAndValidAsync()
        {
            return Task.FromResult(IsAuthenticated);
        }

        public Task LogoutAsync()
        {
            AuthSucceded = false;

            return Task.FromResult(false);
        }
    }
}
