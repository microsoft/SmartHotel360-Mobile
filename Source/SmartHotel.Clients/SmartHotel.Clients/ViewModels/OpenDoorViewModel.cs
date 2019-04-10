using Rg.Plugins.Popup.Services;
using SmartHotel.Clients.Core.Services.NFC;
using SmartHotel.Clients.Core.ViewModels.Base;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.Services.Analytic;
using SmartHotel.Clients.Core.Models;
using Newtonsoft.Json;
using System.Linq;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class OpenDoorViewModel : ViewModelBase
    {
        readonly IAuthenticationService authenticationService;
        readonly INfcService nfcService;
        readonly IAnalyticService analyticService;

        public OpenDoorViewModel(
            IAuthenticationService authenticationService,
            IAnalyticService analyticService)
        {
            this.authenticationService = authenticationService;
            this.analyticService = analyticService;
            nfcService = DependencyService.Get<INfcService>();
        }

        public ICommand ClosePopupCommand => new AsyncCommand(ClosePopupAsync);

        public override async Task InitializeAsync(object navigationData)
        {
            if (nfcService != null)
            {
                if (!nfcService.IsAvailable)
                {
                    await DialogService.ShowAlertAsync(Resources.NoNfc, Resources.Information, Resources.DialogOk);
                    return;
                }

                var authenticatedUser = authenticationService.AuthenticatedUser;

                var nfcParameter = new NfcParameter
                {
                    Username = authenticatedUser.Name,
                    Avatar = authenticatedUser.AvatarUrl
                };

                var serializedMessage = JsonConvert.SerializeObject(nfcParameter);

                MessagingCenter.Send(serializedMessage, MessengerKeys.SendNFCToken);
                analyticService.TrackEvent("OpenDoor");
            }
        }

        Task ClosePopupAsync()
        {
            if (PopupNavigation.Instance.PopupStack.Any())
            {
                return PopupNavigation.Instance.PopAllAsync(true);
            }
            else
            {
                return Task.CompletedTask;
            }
        }
    }
}