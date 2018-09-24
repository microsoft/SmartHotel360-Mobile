using Android.App;
using Android.Views.InputMethods;
using SmartHotel.Clients.Core.Services.DismissKeyboard;
using SmartHotel.Clients.Droid.Services.DismissKeyboard;

[assembly: Xamarin.Forms.Dependency(typeof(DismissKeyboardService))]
namespace SmartHotel.Clients.Droid.Services.DismissKeyboard
{
    public class DismissKeyboardService : IDismissKeyboardService
    {
        public void DismissKeyboard()
        {
            var inputMethodManager = InputMethodManager.FromContext(Android.App.Application.Context);

            inputMethodManager.HideSoftInputFromWindow(
                ((Activity)Xamarin.Forms.Forms.Context).Window.DecorView.WindowToken, HideSoftInputFlags.NotAlways);
        }
    }
}