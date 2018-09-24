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
        const string skype = "Skype";

        double ambientLight;
        double temperature;
        double musicVolume;
        double windowBlinds;
        bool isEcoMode;
        bool ambient;
        bool need;
        bool find;
        bool noDisturb;

        readonly IOpenUriService openUrlService;
        readonly IAnalyticService analyticService;

        public MyRoomViewModel(
            IOpenUriService openUrlService,
            IAnalyticService analyticService)
        {
            this.openUrlService = openUrlService;
            this.analyticService = analyticService;

            SetNeed();
        }

        public double AmbientLight
        {
            get => ambientLight;
            set => SetProperty(ref ambientLight, value);
        }

        public double Temperature
        {
            get => temperature;
            set => SetProperty(ref temperature, value);
        }

        public double MusicVolume
        {
            get => musicVolume;
            set => SetProperty(ref musicVolume, value);
        }

        public double WindowBlinds
        {
            get => windowBlinds;
            set => SetProperty(ref windowBlinds, value);
        }

        public bool IsEcoMode
        {
            get => isEcoMode;
            set => SetProperty(ref isEcoMode, value);
        }

        public bool Ambient
        {
            get => ambient;
            set => SetProperty(ref ambient, value);
        }

        public bool Need
        {
            get => need;
            set => SetProperty(ref need, value);
        }

        public bool Find
        {
            get => find;
            set => SetProperty(ref find, value);
        }

        public bool NoDisturb
        {
            get => noDisturb;
            set => SetProperty(ref noDisturb, value);
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

        void SetAmbient()
        {
            Ambient = true;
            Need = false;
            Find = false;
        }

        void SetNeed()
        {
            Ambient = false;
            Need = true;
            Find = false;
        }

        void SetFind()
        {
            Ambient = false;
            Need = false;
            Find = true;
        }

        Task OpenDoorAsync() => NavigationService.NavigateToPopupAsync<OpenDoorViewModel>(true);

        Task CheckoutAsync() => NavigationService.NavigateToPopupAsync<CheckoutViewModel>(true);

        async Task OpenBotAsync()
        {
            var bots = new[] { skype };

            try
            {
                var selectedBot =
                    await DialogService.SelectActionAsync(
                        Resources.BotSelectionMessage,
                        Resources.BotSelectionTitle,
                        bots);

                switch (selectedBot)
                {
                    case skype:
                        openUrlService.OpenSkypeBot(AppSettings.SkypeBotId);
                        analyticService.TrackEvent("SkypeBot");
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

        void EcoMode()
        {
            if (IsEcoMode)
                ActivateDefaultMode(true);
            else
                ActivateEcoMode(true);
        }

        void ActivateDefaultMode(bool showToast = false)
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

        void ActivateEcoMode(bool showToast = false)
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