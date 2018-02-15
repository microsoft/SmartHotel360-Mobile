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
        private readonly IAnalyticService _analyticService;
        private readonly IAuthenticationService _authenticationService;

        private ValidatableObject<string> _userName;
        private ValidatableObject<string> _password;

        public LoginViewModel(
            IAnalyticService analyticService,
            IAuthenticationService authenticationService)
        {
            _analyticService = analyticService;
            _authenticationService = authenticationService;

            _userName = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();

            AddValidations();
        }

        public ValidatableObject<string> UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public ValidatableObject<string> Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand SignInCommand => new AsyncCommand(SignInAsync);

        public ICommand MicrosoftSignInCommand => new AsyncCommand(MicrosoftSignInAsync);

        public ICommand SettingsCommand => new AsyncCommand(NavigateToSettingsAsync);

        private async Task SignInAsync()
        {
            IsBusy = true;

            bool isValid = Validate();

            if (isValid)
            {
                bool isAuth = await _authenticationService.LoginAsync(UserName.Value, Password.Value);

                if (isAuth)
                {
                    IsBusy = false;

                    _analyticService.TrackEvent("SignIn");
                    await NavigationService.NavigateToAsync<MainViewModel>();
                }
            }

            IsBusy = false;
        }

        private async Task MicrosoftSignInAsync()
        {
            try
            {
                IsBusy = true;

                bool succeeded = await _authenticationService.LoginWithMicrosoftAsync();

                if (succeeded)
                {
                    _analyticService.TrackEvent("MicrosoftSignIn");
                    await NavigationService.NavigateToAsync<MainViewModel>();
                }
            }
            catch (ServiceAuthenticationException)
            {
                await DialogService.ShowAlertAsync("Please, try again", "Login error", "Ok");
            }
            catch(Exception)
            {
                await DialogService.ShowAlertAsync("An error occurred, try again", "Error", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddValidations()
        {
            _userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Username should not be empty" });
            _userName.Validations.Add(new EmailRule());
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password should not be empty" });
        }

        private bool Validate()
        {
            bool isValidUser = _userName.Validate();
            bool isValidPassword = _password.Validate();

            return isValidUser && isValidPassword;
        }

        private Task NavigateToSettingsAsync(object obj)
        {
            return NavigationService.NavigateToAsync(typeof(SettingsViewModel<RemoteSettings>));
        }
    }
}