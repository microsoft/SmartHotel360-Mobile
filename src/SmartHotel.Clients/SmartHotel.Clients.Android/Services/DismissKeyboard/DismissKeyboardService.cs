using Android.Views.InputMethods;
using Plugin.CurrentActivity;
using SmartHotel.Clients.Core.Services.DismissKeyboard;
using SmartHotel.Clients.Droid.Services.DismissKeyboard;

[assembly: Xamarin.Forms.Dependency(typeof(DismissKeyboardService))]
namespace SmartHotel.Clients.Droid.Services.DismissKeyboard
{
    public class DismissKeyboardService : IDismissKeyboardService
    {
        public void DismissKeyboard()
        {
            InputMethodManager inputMethodManager = InputMethodManager.FromContext(CrossCurrentActivity.Current.Activity.ApplicationContext);

            inputMethodManager.HideSoftInputFromWindow(
                CrossCurrentActivity.Current.Activity.Window.DecorView.WindowToken, HideSoftInputFlags.NotAlways);
        }
    }
}