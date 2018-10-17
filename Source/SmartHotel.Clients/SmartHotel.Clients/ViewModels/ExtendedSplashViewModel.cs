using System.Threading.Tasks;
using SmartHotel.Clients.Core.ViewModels.Base;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class ExtendedSplashViewModel : ViewModelBase
    {
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            await NavigationService.InitializeAsync();

            IsBusy = false;
        }
    }
}
