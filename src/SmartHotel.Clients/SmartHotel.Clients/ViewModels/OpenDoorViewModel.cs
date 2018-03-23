using Rg.Plugins.Popup.Services;
using SmartHotel.Clients.Core.Services.NFC;
using SmartHotel.Clients.Core.ViewModels.Base;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.Models;
using Newtonsoft.Json;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class OpenDoorViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INfcService _nfcService;

        public OpenDoorViewModel(
            IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _nfcService = DependencyService.Get<INfcService>();
        }

        public ICommand ClosePopupCommand => new AsyncCommand(ClosePopupAsync);

        public override async Task InitializeAsync(object navigationData)
        {
            if (_nfcService != null)
            {
                if (!_nfcService.IsAvailable)
                {
                    await DialogService.ShowAlertAsync(Resources.NoNfc, Resources.Information, Resources.DialogOk);
                    return;
                }

                var authenticatedUser = _authenticationService.AuthenticatedUser;

                var nfcParameter = new NfcParameter
                {
                    Username = authenticatedUser.Name,
                    Avatar = authenticatedUser.AvatarUrl
                };

                var serializedMessage = JsonConvert.SerializeObject(nfcParameter);

                MessagingCenter.Send(serializedMessage, MessengerKeys.SendNFCToken);
            }
        }

        private Task ClosePopupAsync()
        {
            return PopupNavigation.PopAllAsync(true);
        }
    }
}