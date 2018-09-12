using SmartHotel.Clients.Core.Services.Analytic;
using SmartHotel.Clients.Core.Services.OpenUri;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
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

		bool isInitializing;
        readonly IOpenUriService openUrlService;
        readonly IAnalyticService analyticService;
		readonly IRoomDevicesDataService roomDevicesDataService;

		public MyRoomViewModel(
			IOpenUriService openUrlService,
            IAnalyticService analyticService,
			IRoomDevicesDataService roomDevicesDataService)
        {
            this.openUrlService = openUrlService;
            this.analyticService = analyticService;
			this.roomDevicesDataService = roomDevicesDataService;

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
	            bool changed = SetProperty(ref desiredAmbientLight, value);
	            if ( !isInitializing && changed )
	            {
	                // TODO: Call the RoomDevicesDataService to update once the value has stopped changing
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
	            bool changed = SetProperty(ref desiredTemperature, value);
	            if ( !isInitializing && changed )
	            {
	                // TODO: Call the RoomDevicesDataService to update once the value has stopped changing
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

		public ICommand AmbientCommand => new Command( SetAmbient );

		public ICommand NeedCommand => new Command( SetNeed );

		public ICommand FindCommand => new Command( SetFind );

		public ICommand OpenDoorCommand => new AsyncCommand( OpenDoorAsync );

		public ICommand CheckoutCommand => new AsyncCommand( CheckoutAsync );

		public ICommand OpenBotCommand => new AsyncCommand( OpenBotAsync );

		public ICommand EcoModeCommand => new Command( EcoMode );

		public override async Task InitializeAsync( object navigationData )
		{
			IsBusy = true;

			isInitializing = true;
			IsEcoMode = false;

			Task<RoomTemperature> roomTemperatureTask = roomDevicesDataService.GetRoomTemperatureAsync();
			Task<RoomAmbientLight> roomAmbientLightTask = roomDevicesDataService.GetRoomAmbientLightAsync();
			await Task.WhenAll( roomTemperatureTask, roomAmbientLightTask );

			RoomTemperature roomTemperature = roomTemperatureTask.Result;
			TemperatureMaximum = roomTemperature.Maximum.RawValue;
			TemperatureMinimum = roomTemperature.Minimum.RawValue;
			CurrentTemperature = roomTemperature.Value.RawValue;
			DesiredTemperature = roomTemperature.Desired.RawValue;

			RoomAmbientLight roomAmbientLight = roomAmbientLightTask.Result;
			AmbientLightMaximum = roomAmbientLight.Maximum.RawValue;
			AmbientLightMinimum = roomAmbientLight.Minimum.RawValue;
			CurrentAmbientLight = roomAmbientLight.Value.RawValue;
			DesiredAmbientLight = roomAmbientLight.Desired.RawValue;

			MusicVolume = 45;
			WindowBlinds = 80;

			isInitializing = false;
			IsBusy = false;
		}

		public Task OnViewAppearingAsync( VisualElement view )
		{
			roomDevicesDataService.StartCheckingRoomSensorData();

			return Task.FromResult( true );
		}

		public Task OnViewDisappearingAsync( VisualElement view )
		{
			roomDevicesDataService.StopCheckingRoomSensorData();

			return Task.FromResult( true );
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
						bots );

				switch ( selectedBot )
				{
                    case skype:
                        openUrlService.OpenSkypeBot(AppSettings.SkypeBotId);
                        analyticService.TrackEvent("SkypeBot");
						break;
				}
			}
			catch ( Exception ex )
			{
				Debug.WriteLine( $"OpenBot: {ex}" );

				await DialogService.ShowAlertAsync(
				  Resources.BotError,
				  Resources.ExceptionTitle,
				  Resources.DialogOk );
			}
		}

        void EcoMode()
		{
			if ( IsEcoMode )
				ActivateDefaultMode( true );
			else
				ActivateEcoMode( true );
		}

        void ActivateDefaultMode(bool showToast = false)
		{
			IsEcoMode = false;

			DesiredAmbientLight = 3400;
			DesiredTemperature = 70;
			MusicVolume = 45;
			WindowBlinds = 80;

			if ( showToast )
			{
				DialogService.ShowToast( Resources.DeactivateEcoMode, 1000 );
			}
		}

        void ActivateEcoMode(bool showToast = false)
		{
			IsEcoMode = true;

			DesiredAmbientLight = 2400;
			DesiredTemperature = 60;
			MusicVolume = 40;
			WindowBlinds = 50;

			if ( showToast )
			{
				DialogService.ShowToast( Resources.ActivateEcoMode, 1000 );
			}
		}
	}
}