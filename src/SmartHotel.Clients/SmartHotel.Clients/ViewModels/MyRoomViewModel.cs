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
	public class MyRoomViewModel : ViewModelBase
	{
		const string Skype = "Skype";
		const string FacebookMessenger = "Facebook Messenger";

		private double _desiredAmbientLight;
		private double _currentAmbientLight;
		private double _ambientLightMinimum;
		private double _ambientLightMaximum = 100;
		private double _desiredTemperature;
		private double _currentTemperature;
		private double _temperatureMinimum = 60;
		private double _temperatureMaximum = 90;
		private double _musicVolume;
		private double _windowBlinds;
		private bool _isEcoMode;
		private bool _ambient;
		private bool _need;
		private bool _find;
		private bool _noDisturb;

		private bool _isInitializing;

		private readonly IOpenUriService _openUrlService;
		private readonly IAnalyticService _analyticService;
		private readonly IRoomDevicesDataService _roomDevicesDataService;

		public MyRoomViewModel(
			IOpenUriService openUrlService,
			IAnalyticService analyticService,
			IRoomDevicesDataService roomDevicesDataService )
		{
			_openUrlService = openUrlService;
			_analyticService = analyticService;
			_roomDevicesDataService = roomDevicesDataService;

			SetNeed();
		}

		public bool UseRealRoomDevices => !_roomDevicesDataService.UseFakes;

		public double CurrentAmbientLight
		{
			get => _currentAmbientLight;
			set
			{
				_currentAmbientLight = value;
				OnPropertyChanged();
			}
		}

		public double DesiredAmbientLight
		{
			get => _desiredAmbientLight;
			set
			{
				_desiredAmbientLight = value;
				OnPropertyChanged();
				if ( !_isInitializing )
				{
					// TODO: Call the RoomDevicesDataService to update once the value has stopped changing
				}
			}
		}

		public double AmbientLightMinimum
		{
			get => _ambientLightMinimum;
			set
			{
				_ambientLightMinimum = value;
				OnPropertyChanged();
			}
		}

		public double AmbientLightMaximum
		{
			get => _ambientLightMaximum;
			set
			{
				_ambientLightMaximum = value;
				OnPropertyChanged();
			}
		}

		public double DesiredTemperature
		{
			get => _desiredTemperature;
			set
			{
				_desiredTemperature = value;
				OnPropertyChanged();
				if ( _isInitializing )
				{
					// TODO: Call the RoomDevicesDataService to update once the value has stopped changing
				}
			}
		}

		public double CurrentTemperature
		{
			get => _currentTemperature;
			set
			{
				_currentTemperature = value;
				OnPropertyChanged();
			}
		}

		public double TemperatureMinimum
		{
			get => _temperatureMinimum;
			set
			{
				_temperatureMinimum = value;
				OnPropertyChanged();
			}
		}

		public double TemperatureMaximum
		{
			get => _temperatureMaximum;
			set
			{
				_temperatureMaximum = value;
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

			_isInitializing = true;
			IsEcoMode = false;

			Task<RoomTemperature> roomTemperatureTask = _roomDevicesDataService.GetRoomTemperatureAsync();
			Task<RoomAmbientLight> roomAmbientLightTask = _roomDevicesDataService.GetRoomAmbientLightAsync();
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

			_isInitializing = false;
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
			return NavigationService.NavigateToPopupAsync<OpenDoorViewModel>( true );
		}

		private Task CheckoutAsync()
		{
			return NavigationService.NavigateToPopupAsync<CheckoutViewModel>( true );
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
						bots );

				switch ( selectedBot )
				{
					case Skype:
						_openUrlService.OpenSkypeBot( AppSettings.SkypeBotId );
						_analyticService.TrackEvent( "SkypeBot" );
						break;
					case FacebookMessenger:
						_openUrlService.OpenFacebookBot( AppSettings.FacebookBotId );
						_analyticService.TrackEvent( "FacebookBot" );
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

		private void EcoMode()
		{
			if ( IsEcoMode )
				ActivateDefaultMode( true );
			else
				ActivateEcoMode( true );
		}

		private void ActivateDefaultMode( bool showToast = false )
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

		private void ActivateEcoMode( bool showToast = false )
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