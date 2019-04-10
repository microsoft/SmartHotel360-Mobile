using Rg.Plugins.Popup.Services;
using SmartHotel.Clients.Core.Services.Analytic;
using SmartHotel.Clients.Core.ViewModels.Base;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {
        readonly IAnalyticService analyticService;

        public CheckoutViewModel(IAnalyticService analyticService) => this.analyticService = analyticService;

        public string UserName => AppSettings.User?.Name;

        public string UserAvatar => AppSettings.User?.AvatarUrl;

        public ICommand ClosePopupCommand => new AsyncCommand(ClosePopupAsync);

        public ICommand CheckoutCommand => new AsyncCommand(CheckoutAsync);

        Task ClosePopupAsync()
        {
            AppSettings.HasBooking = false;

            MessagingCenter.Send(this, MessengerKeys.CheckoutRequested);
            analyticService.TrackEvent("Checkout");

            if (PopupNavigation.Instance.PopupStack.Any())
            {
                return PopupNavigation.Instance.PopAllAsync(true);
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        async Task CheckoutAsync()
        {
            AppSettings.HasBooking = false;

            MessagingCenter.Send(this, MessengerKeys.CheckoutRequested);
            analyticService.TrackEvent("Checkout");            

            if (PopupNavigation.Instance.PopupStack.Any())
            {
                await PopupNavigation.Instance.PopAllAsync(true);
            }

            await NavigationService.NavigateToAsync<BookingViewModel>();
        }
    }
}