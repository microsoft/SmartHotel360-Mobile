using SmartHotel.Clients.Core.Services.Analytic;
using SmartHotel.Clients.Core.Services.OpenUri;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class MyRoomViewModel : ViewModelBase
    {
        const string Skype = "Skype";
        const string FacebookMessenger = "Facebook Messenger";

        private double _ambientLight;
        private double _temperature;
        private double _musicVolume;
        private double _windowBlinds;
        private bool _isEcoMode;
        private bool _ambient;
        private bool _need;
        private bool _find;
        private bool _noDisturb;

        private readonly IOpenUriService _openUrlService;
        private readonly IAnalyticService _analyticService;

        public MyRoomViewModel(
            IOpenUriService openUrlService,
            IAnalyticService analyticService)
        {
            _openUrlService = openUrlService;
            _analyticService = analyticService;

            SetNeed();
        }

        public double AmbientLight
        {
            get { return _ambientLight; }
            set
            {
                _ambientLight = value;
                OnPropertyChanged();
            }
        }

        public double Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                OnPropertyChanged();
            }
        }

        public double MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                _musicVolume = value;
                OnPropertyChanged();
            }
        }

        public double WindowBlinds
        {
            get { return _windowBlinds; }
            set
            {
                _windowBlinds = value;
                OnPropertyChanged();
            }
        }

        public bool IsEcoMode
        {
            get { return _isEcoMode; }
            set
            {
                _isEcoMode = value;
                OnPropertyChanged();
            }
        }

        public bool Ambient
        {
            get { return _ambient; }
            set
            {
                _ambient = value;
                OnPropertyChanged();
            }
        }

        public bool Need
        {
            get { return _need; }
            set
            {
                _need = value;
                OnPropertyChanged();
            }
        }

        public bool Find
        {
            get { return _find; }
            set
            {
                _find = value;
                OnPropertyChanged();
            }
        }

        public bool NoDisturb
        {
            get { return _noDisturb; }
            set
            {
                _noDisturb = value;
                OnPropertyChanged();
            }
        }

        public ICommand AmbientCommand => new Command(SetAmbient);

        public ICommand NeedCommand => new Command(SetNeed);

        public ICommand FindCommand => new Command(SetFind);

        public ICommand OpenDoorCommand => new AsyncCommand(OpenDoorAsync);

        public ICommand CheckoutCommand => new AsyncCommand(CheckoutAsync);

        public ICommand OpenBotCommand => new AsyncCommand(OpenBotAsync);

        public ICommand EcoModeCommand => new Command(EcoMode);

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            await Task.Delay(500);
            ActivateDefaultMode();

            IsBusy = false;
        }

        private void SetAmbient()
        {
            Ambient = true;
            Need = false;
            Find = false;
        }

        private void SetNeed()
        {
            Ambient = false;
            Need = true;
            Find = false;
        }

        private void SetFind()
        {
            Ambient = false;
            Need = false;
            Find = true;
        }

        private Task OpenDoorAsync()
        {
            return NavigationService.NavigateToPopupAsync<OpenDoorViewModel>(true);
        }

        private Task CheckoutAsync()
        {
            return NavigationService.NavigateToPopupAsync<CheckoutViewModel>(true);
        }

        private async Task OpenBotAsync()
        {
            var bots = new[] { Skype, FacebookMessenger };

            try
            {
                var selectedBot =
                    await DialogService.SelectActionAsync(
                        Resources.BotSelectionMessage,
                        Resources.BotSelectionTitle,
                        bots);

                switch (selectedBot)
                {
                    case Skype:
                        _openUrlService.OpenSkypeBot(AppSettings.SkypeBotId);
                        _analyticService.TrackEvent("SkypeBot");
                        break;
                    case FacebookMessenger:
                        _openUrlService.OpenFacebookBot(AppSettings.FacebookBotId);
                        _analyticService.TrackEvent("FacebookBot");
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"OpenBot: {ex}");

                await DialogService.ShowAlertAsync(
                  Resources.BotError,
                  Resources.ExceptionTitle,
                  Resources.DialogOk);
            }
        }

        private void EcoMode()
        {
            if (IsEcoMode)
                ActivateDefaultMode(true);
            else
                ActivateEcoMode(true);
        }

        private void ActivateDefaultMode(bool showToast = false)
        {
            IsEcoMode = false;

            AmbientLight = 3400;
            Temperature = 70;
            MusicVolume = 45;
            WindowBlinds = 80;

            if (showToast)
            {
                DialogService.ShowToast(Resources.DeactivateEcoMode, 1000);
            }
        }

        private void ActivateEcoMode(bool showToast = false)
        {
            IsEcoMode = true;

            AmbientLight = 2400;
            Temperature = 60;
            MusicVolume = 40;
            WindowBlinds = 50;

            if (showToast)
            {
                DialogService.ShowToast(Resources.ActivateEcoMode, 1000);
            }
        }
    }
}