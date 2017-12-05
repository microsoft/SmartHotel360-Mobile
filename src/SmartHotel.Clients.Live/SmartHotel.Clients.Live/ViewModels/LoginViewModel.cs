using SmartHotel.Clients.Live.ViewModels.Base;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Live.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _userName;
        private string _password;

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public ICommand SignInCommand => new Command(SignInAsync);

        public ICommand RegisterCommand => new Command(Register);

        public ICommand ValidateCommand => new Command(() => Validate());

        private async void SignInAsync()
        {
            IsBusy = true;

            bool isValid = Validate();

            if (isValid)
            {
                await NavigationService.NavigateToAsync<MainViewModel>();        
            }

            IsBusy = false;
        }

        private void Register()
        {
            // Navigate to RegisterViewModel
        }

        private bool Validate()
        {
            bool isValidUser = !string.IsNullOrEmpty(UserName);
            bool isValidPassword = !string.IsNullOrEmpty(Password);

            return isValidUser && isValidPassword;
        }
    }
}