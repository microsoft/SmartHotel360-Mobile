using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Internal;
using System;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        readonly IBrowserCookiesService browserCookiesService;
        readonly IAvatarUrlProvider avatarProvider;

        public AuthenticationService(
            IBrowserCookiesService browserCookiesService,
            IAvatarUrlProvider avatarProvider)
        {
            this.browserCookiesService = browserCookiesService;
            this.avatarProvider = avatarProvider;
        }

        public bool IsAuthenticated => AppSettings.User != null;

        public Models.User AuthenticatedUser => AppSettings.User;

        public Task<bool> LoginAsync(string email, string password)
        {
            var user = new Models.User
            {
                Email = email,
                Name = email,
                LastName = string.Empty,
                AvatarUrl = avatarProvider.GetAvatarUrl(email),
                Token = email,
                LoggedInWithMicrosoftAccount = false
            };

            AppSettings.User = user;

            return Task.FromResult(true);
        }

        public async Task<bool> LoginWithMicrosoftAsync()
        {
            var succeeded = false;

            try
            {
                var result = await App.AuthenticationClient.AcquireTokenAsync(
                  new string[] { AppSettings.B2cClientId },
                  string.Empty,
                  UiOptions.SelectAccount,
                  string.Empty,
                  null,
                  $"{AppSettings.B2cAuthority}{AppSettings.B2cTenant}",
                  AppSettings.B2cPolicy);

                var user = AuthenticationResultHelper.GetUserFromResult(result);
                user.AvatarUrl = avatarProvider.GetAvatarUrl(user.Email);
                user.LoggedInWithMicrosoftAccount = true;
                AppSettings.User = user;

                succeeded = true;
            }
            catch (MsalException ex)
            {
                if (ex.ErrorCode != MsalError.AuthenticationCanceled)
                {
                    System.Diagnostics.Debug.WriteLine($"Error with MSAL authentication: {ex}");
                    throw new ServiceAuthenticationException();
                }
            }

            return succeeded;
        }

        public async Task<bool> UserIsAuthenticatedAndValidAsync()
        {
            if (!IsAuthenticated)
            {
                return false;
            }
            else if (!AuthenticatedUser.LoggedInWithMicrosoftAccount)
            {
                return true;
            }
            else
            {
                var refreshSucceded = false;

                try
                {
                    var tokenCache = App.AuthenticationClient.UserTokenCache;
                    var ar = await App.AuthenticationClient.AcquireTokenSilentAsync(
                        new string[] { AppSettings.B2cClientId },
                        AuthenticatedUser.Id,
                        $"{AppSettings.B2cAuthority}{AppSettings.B2cTenant}",
                        AppSettings.B2cPolicy,
                        true);
                    SaveAuthenticationResult(ar);

                    refreshSucceded = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error with MSAL refresh attempt: {ex}");
                }

                return refreshSucceded;
            }
        }

        public async Task LogoutAsync()
        {
            AppSettings.RemoveUserData();
            await browserCookiesService.ClearCookiesAsync();
        }

        void SaveAuthenticationResult(AuthenticationResult result)
        {
            var user = AuthenticationResultHelper.GetUserFromResult(result);
            user.AvatarUrl = avatarProvider.GetAvatarUrl(user.Email);
            AppSettings.User = user;
        }
    }
}
