using SmartHotel.Clients.Core.Services.Analytic;
using SmartHotel.Clients.Core.Services.OpenUri;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartHotel.Clients.Core.Helpers;
using SmartHotel.Clients.Core.Services.IoT;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class MyRoomViewModel : ViewModelBase, IHandleViewAppearing, IHandleViewDisappearing
    {
        const string skype = "Skype";

	    double desiredAmbientLight;
	    double currentAmbientLight;
		double ambientLightMinimum = RoomAmbientLight.DefaultMinimum.RawValue;
		double ambientLightMaximum = RoomAmbientLight.DefaultMaximum.RawValue;
	    double desiredTemperature;
	    double currentTemperature;
		double temperatureMinimum = RoomTemperature.DefaultMinimum.RawValue;
		double temperatureMaximum = RoomTemperature.DefaultMaximum.RawValue;
        double musicVolume;
        double windowBlinds;
        bool isEcoMode;
        bool ambient;
        bool need;
        bool find;
        bool noDisturb;
        bool updatingDesiredValuesFromService;

	    bool isInitialized;
	    readonly TimeSpan sliderInertia = TimeSpan.FromSeconds( 1 );
	    readonly Timer delayedTemperatureChangedTimer;
	    readonly Timer delayedLightChangedTimer;
        readonly IOpenUriService openUrlService;
        readonly IAnalyticService analyticService;
		readonly IRoomDevicesDataService roomDevicesDataService;

        public MyRoomViewModel(
            IOpenUriService openUrlService,
            IAnalyticService analyticService,
			IRoomDevicesDataService roomDevicesDataService )
        {
            this.openUrlService = openUrlService;
            this.analyticService = analyticService;
			this.roomDevicesDataService = roomDevicesDataService;


	        delayedTemperatureChangedTimer = new Timer( sliderInertia,
		        async () => { await UpdateRoomTemperature( DesiredTemperature ); } );

	        delayedLightChangedTimer = new Timer( sliderInertia,
		        async () => { await UpdateRoomLight( DesiredAmbientLight ); } );

            SetNeed();
        }

		public bool UseRealRoomDevices => !roomDevicesDataService.UseFakes;

		public double CurrentAmbientLight
		{
	        get => currentAmbientLight;
	        set => SetProperty(ref currentAmbientLight, value);
		}

		public double DesiredAmbientLight
		{
	        get => desiredAmbientLight;
			set
			{
	            var changed = SetProperty(ref desiredAmbientLight, Math.Round(value));
	            if ( changed && !updatingDesiredValuesFromService && IsRoomDevicesLive() )
	            {
		            delayedLightChangedTimer.Stop();
		            delayedLightChangedTimer.Start();
	            }
			}
		}

		public double AmbientLightMinimum
		{
	        get => ambientLightMinimum;
	        set => SetProperty(ref ambientLightMinimum, value);
		}

		public double AmbientLightMaximum
		{
	        get => ambientLightMaximum;
	        set => SetProperty(ref ambientLightMaximum, value);
		}

		public double DesiredTemperature
		{
	        get => desiredTemperature;
			set
			{
	            var changed = SetProperty(ref desiredTemperature, Math.Round(value));

	            if ( changed && !updatingDesiredValuesFromService && IsRoomDevicesLive() )
	            {
		            delayedTemperatureChangedTimer.Stop();
		            delayedTemperatureChangedTimer.Start();
	            }
			}
		}

		public double CurrentTemperature
		{
	        get => currentTemperature;
	        set => SetProperty(ref currentTemperature, value);
		}

		public double TemperatureMinimum
		{
	        get => temperatureMinimum;
	        set => SetProperty(ref temperatureMinimum, value);
		}

		public double TemperatureMaximum
		{
	        get => temperatureMaximum;
	        set => SetProperty(ref temperatureMaximum, value);
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

            IsEcoMode = false;

            await GetRoomSensorData(true);

            MusicVolume = 45;
            WindowBlinds = 80;

            IsBusy = false;
	        isInitialized = true;
        }

	    async Task GetRoomSensorData( bool isInitializing = false )
	    {
		    var roomTemperature = await roomDevicesDataService.GetRoomTemperatureAsync();
	        var roomAmbientLight = await roomDevicesDataService.GetRoomAmbientLightAsync();

	        if (roomTemperature == null || roomAmbientLight == null)
	        {
	            roomDevicesDataService.StopCheckingRoomSensorData();
                await DialogService.ShowAlertAsync("Please ensure that the IoT Demo backend setup is complete and restart.",
	                "Unable to get room sensor information", "OK");
	            return;
	        }

		    CurrentTemperature = roomTemperature.Value.RawValue;
		    CurrentAmbientLight = roomAmbientLight.Value.RawValue;
	        updatingDesiredValuesFromService = true;
	        if (!delayedTemperatureChangedTimer.IsRunning || isInitializing)
	        {
	            DesiredTemperature = roomTemperature.Desired.RawValue;
	        }

	        if (!delayedLightChangedTimer.IsRunning || isInitializing)
	        {
	            DesiredAmbientLight = roomAmbientLight.Desired.RawValue;
	        }
	        updatingDesiredValuesFromService = false;

            if ( isInitializing )
		    {
			    TemperatureMaximum = roomTemperature.Maximum.RawValue;
			    TemperatureMinimum = roomTemperature.Minimum.RawValue;

			    AmbientLightMaximum = roomAmbientLight.Maximum.RawValue;
			    AmbientLightMinimum = roomAmbientLight.Minimum.RawValue;
		    }
	    }

        public Task OnViewAppearingAsync(VisualElement view)
        {
	        roomDevicesDataService.SensorDataChanged += RoomDevicesDataServiceSensorDataChanged;
	        roomDevicesDataService.StartCheckingRoomSensorData();

            return Task.FromResult(true);
        }

        public Task OnViewDisappearingAsync(VisualElement view)
        {
	        roomDevicesDataService.SensorDataChanged -= RoomDevicesDataServiceSensorDataChanged;
            roomDevicesDataService.StopCheckingRoomSensorData();

            return Task.FromResult(true);
        }

	    async void RoomDevicesDataServiceSensorDataChanged( object sender, EventArgs e )
	    {
		    await GetRoomSensorData();
	    }

        bool IsRoomDevicesLive()
        {
            return isInitialized && UseRealRoomDevices;
        }

        async Task UpdateRoomLight(double newDesiredAmbientLight)
        {
	        delayedLightChangedTimer.Stop();

            Debug.WriteLine($"UpdateRoomLight: {newDesiredAmbientLight}");
            await roomDevicesDataService.UpdateDesiredAsync((float)newDesiredAmbientLight / 100f, SensorDataType.Light);
        }

        async Task UpdateRoomTemperature(double newDesiredTemperature)
        {
	        delayedTemperatureChangedTimer.Stop();

			Debug.WriteLine($"UpdateRoomTemperature: {newDesiredTemperature}");
            await roomDevicesDataService.UpdateDesiredAsync((float)newDesiredTemperature, SensorDataType.Temperature);
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

            updatingDesiredValuesFromService = true;
            DesiredAmbientLight = 100;
            DesiredTemperature = 70;
            MusicVolume = 45;
            WindowBlinds = 80;
            updatingDesiredValuesFromService = false;

            if (showToast)
            {
                DialogService.ShowToast(Resources.DeactivateEcoMode, 1000);
            }
        }

        void ActivateEcoMode(bool showToast = false)
        {
            IsEcoMode = true;

            updatingDesiredValuesFromService = true;
            DesiredAmbientLight = 0;
            DesiredTemperature = 60;
            MusicVolume = 40;
            WindowBlinds = 50;
            updatingDesiredValuesFromService = false;

            if (showToast)
            {
                DialogService.ShowToast(Resources.ActivateEcoMode, 1000);
            }
        }
    }
}