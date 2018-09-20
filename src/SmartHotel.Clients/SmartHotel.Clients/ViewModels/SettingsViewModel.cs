using SmartHotel.Clients.Core.Exceptions;
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
        readonly ISettingsService<TRemoteSettingsModel> settingsService;

        ValidatableObject<string> settingsFileUrl;
        TRemoteSettingsModel remoteSettings;

        public bool IsValid { get; set; }

        public SettingsViewModel(
            ISettingsService<TRemoteSettingsModel> settingsService)
        {
            this.settingsService = settingsService;

            settingsFileUrl = new ValidatableObject<string>();

            AddValidations();
        }

        public ValidatableObject<string> SettingsFileUrl
        {
            get => settingsFileUrl;
            set => SetProperty(ref settingsFileUrl, value);
        }

        public TRemoteSettingsModel RemoteSettings
        {
            get => remoteSettings;
            set => SetProperty(ref remoteSettings, value);
        }

        public ICommand UpdateCommand => new AsyncCommand(UpdateSettingsAsync);

        public override async Task InitializeAsync(object navigationData)
        {
            SettingsFileUrl.Value = settingsService.RemoteFileUrl;
            RemoteSettings = await settingsService.LoadSettingsAsync();
        }

        void AddValidations()
        {
            settingsFileUrl.Validations.Add(new IsNotNullOrEmptyRule<string>());
            settingsFileUrl.Validations.Add(new ValidUrlRule());
        }

        bool Validate() => settingsFileUrl.Validate();

        async Task UpdateSettingsAsync(object obj)
        {
            try
            {
                IsBusy = true;

                IsValid = Validate();

                if (IsValid)
                {
                    RemoteSettings = await settingsService.LoadRemoteSettingsAsync(settingsFileUrl.Value);
                    await settingsService.PersistRemoteSettingsAsync(RemoteSettings);
                    settingsService.RemoteFileUrl = SettingsFileUrl.Value;

                    await DialogService.ShowAlertAsync("Remote settings were successfully loaded", "JSON settings loaded!", "Accept");
                }
            }
            catch (ConnectivityException)
            {
                await DialogService.ShowAlertAsync("There is no Internet conection, try again later.", "Error", "Ok");
            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(ex.Message, "Error loading JSON settings", "Accept");
            }
            finally
            {
                MessagingCenter.Send(this, MessengerKeys.LoadSettingsRequested);
                IsBusy = false;
            }
        }
    }
}