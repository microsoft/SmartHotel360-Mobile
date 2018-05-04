using SmartHotel.Clients.Core.Services.Settings;
using SmartHotel.Clients.Core.Validations;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class SettingsViewModel<TRemoteSettingsModel> : ViewModelBase
        where TRemoteSettingsModel : class
    {
        private readonly ISettingsService<TRemoteSettingsModel> _settingsService;

        private ValidatableObject<string> _settingsFileUrl;
        private TRemoteSettingsModel _remoteSettings;

        public SettingsViewModel(
            ISettingsService<TRemoteSettingsModel> settingsService)
        {
            _settingsService = settingsService;

            _settingsFileUrl = new ValidatableObject<string>();

            AddValidations();
        }

        public ValidatableObject<string> SettingsFileUrl
        {
            get
            {
                return _settingsFileUrl;
            }
            set
            {
                _settingsFileUrl = value;
                OnPropertyChanged();
            }
        }

        public TRemoteSettingsModel RemoteSettings
        {
            get
            {
                return _remoteSettings;
            }
            set
            {
                _remoteSettings = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateCommand => new AsyncCommand(UpdateSettingsAsync);

        public override async Task InitializeAsync(object navigationData)
        {
            SettingsFileUrl.Value = _settingsService.RemoteFileUrl;
            RemoteSettings = await _settingsService.LoadSettingsAsync();
        }

        private void AddValidations()
        {
            _settingsFileUrl.Validations.Add(new IsNotNullOrEmptyRule<string>());
            _settingsFileUrl.Validations.Add(new ValidUrlRule());
        }

        private bool Validate()
        {
            return _settingsFileUrl.Validate();
        }

        private async Task UpdateSettingsAsync(object obj)
        {
            try
            {
                IsBusy = true;

                if (Validate())
                {
                    RemoteSettings = await _settingsService.LoadRemoteSettingsAsync(_settingsFileUrl.Value);
                    await _settingsService.PersistRemoteSettingsAsync(RemoteSettings);
                    _settingsService.RemoteFileUrl = SettingsFileUrl.Value;

                    await DialogService.ShowAlertAsync("Remote settings were successfully loaded", "JSON settings loaded!", "Accept");
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(ex.Message, "Error loading JSON settings", "Accept");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}