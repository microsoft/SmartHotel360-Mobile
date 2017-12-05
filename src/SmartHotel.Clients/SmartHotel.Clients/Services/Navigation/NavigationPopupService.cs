using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Navigation
{
    public partial class NavigationService : INavigationService
    {

        public Task NavigateToPopupAsync<TViewModel>(bool animate) where TViewModel : ViewModelBase
        {
            return NavigateToPopupAsync<TViewModel>(null, animate);
        }

        public async Task NavigateToPopupAsync<TViewModel>(object parameter, bool animate) where TViewModel : ViewModelBase
        {
            var page = CreateAndBindPage(typeof(TViewModel), parameter);
            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);

            if (page is PopupPage)
            {
                await PopupNavigation.PushAsync(page as PopupPage, animate);
            }
            else
            {
                throw new ArgumentException($"The type ${typeof(TViewModel)} its not a PopupPage type");
            }
        }
    }
}