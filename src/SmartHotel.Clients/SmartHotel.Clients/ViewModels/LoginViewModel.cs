using SmartHotel.Clients.Core.Exceptions;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Analytic;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.Validations;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        readonly IAnalyticService analyticService;
        readonly IAuthenticationService authenticationService;

        ValidatableObject<string> userName;
        ValidatableObject<string> password;

        public bool IsValid { get; set; }

        public LoginViewModel(
            IAnalyticService analyticService,
            IAuthenticationService authenticationService)
        {
            this.analyticService = analyticService;
            this.authenticationService = authenticationService;

            userName = new ValidatableObject<string>();
            password = new ValidatableObject<string>();

            AddValidations();
        }

        public ValidatableObject<string> UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public ValidatableObject<string> Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public ICommand SignInCommand => new AsyncCommand(SignInAsync);

        public ICommand MicrosoftSignInCommand => new AsyncCommand(MicrosoftSignInAsync);

        public ICommand SettingsCommand => new AsyncCommand(NavigateToSettingsAsync);

        async Task SignInAsync()
        {
            IsBusy = true;

            IsValid = Validate();           

            if (IsValid)
            {
                var isAuth = await authenticationService.LoginAsync(UserName.Value, Password.Value);

                if (isAuth)
                {
                    IsBusy = false;

                    analyticService.TrackEvent("SignIn");
                    await NavigationService.NavigateToAsync<MainViewModel>();
                }
            }

            MessagingCenter.Send(this, MessengerKeys.SignInRequested);

            IsBusy = false;
        }

        async Task MicrosoftSignInAsync()
        {
            try
            {
                IsBusy = true;

                var succeeded = await authenticationService.LoginWithMicrosoftAsync();

                if (succeeded)
                {
                    analyticService.TrackEvent("MicrosoftSignIn");
                    await NavigationService.NavigateToAsync<MainViewModel>();
                }
            }
            catch (ServiceAuthenticationException)
            {
                await DialogService.ShowAlertAsync("Please, try again.", "Login error", "Ok");
            }
            catch(ConnectivityException)
            {
                await DialogService.ShowAlertAsync("There is no Internet conection, try again later.", "Error", "Ok");
            }
            catch(Exception)
            {
                await DialogService.ShowAlertAsync("An error occurred, try again.", "Error", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        void AddValidations()
        {
            userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Username should not be empty" });
            userName.Validations.Add(new EmailRule());
            password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password should not be empty" });
        }

        bool Validate()
        {
            var isValidUser = userName.Validate();
            var isValidPassword = password.Validate();

            return isValidUser && isValidPassword;
        }

        Task NavigateToSettingsAsync(object obj) => NavigationService.NavigateToAsync(typeof(SettingsViewModel<RemoteSettings>));
    }
}