using SmartHotel.Clients.Core.Services.DismissKeyboard;
using SmartHotel.Clients.UWP.Services.DismissKeyboard;
using Windows.UI.ViewManagement;

[assembly: Xamarin.Forms.Dependency(typeof(DismissKeyboardService))]
namespace SmartHotel.Clients.UWP.Services.DismissKeyboard
{
    class DismissKeyboardService : IDismissKeyboardService
    {
        public void DismissKeyboard()
        {
            InputPane.GetForCurrentView().TryHide();
        }
    }
}