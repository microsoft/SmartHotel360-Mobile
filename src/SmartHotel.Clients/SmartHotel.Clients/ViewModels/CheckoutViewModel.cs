using Rg.Plugins.Popup.Services;
using SmartHotel.Clients.Core.Services.Analytic;
using SmartHotel.Clients.Core.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {
        private readonly IAnalyticService _analyticService;

        public CheckoutViewModel(IAnalyticService analyticService)
        {
            _analyticService = analyticService;
        }

        public string UserName => AppSettings.User?.Name;

        public string UserAvatar => AppSettings.User?.AvatarUrl;

        public ICommand ClosePopupCommand => new AsyncCommand(ClosePopupAsync);

        public ICommand CheckoutCommand => new AsyncCommand(CheckoutAsync);

        private Task ClosePopupAsync()
        {
            AppSettings.HasBooking = false;

            MessagingCenter.Send(this, MessengerKeys.CheckoutRequested);
            _analyticService.TrackEvent("Checkout");

            return PopupNavigation.PopAllAsync(true);
        }

        private async Task CheckoutAsync()
        {
            AppSettings.HasBooking = false;

            MessagingCenter.Send(this, MessengerKeys.CheckoutRequested);
            _analyticService.TrackEvent("Checkout");

            await PopupNavigation.PopAllAsync(false);
    
            await NavigationService.NavigateToAsync<BookingViewModel>();
        }
    }
}